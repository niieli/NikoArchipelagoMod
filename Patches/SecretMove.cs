using System.Reflection;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class SecretMove
{
    [HarmonyPatch(typeof(scrObtainSecretMove), "Start")]
    public static class PatchSecretMove
    {
        [HarmonyPrefix]
        static bool Prefix(scrObtainSecretMove __instance)
        {
            var appleCost = 250;
            var saveManager = scrGameSaveManager.instance;
            if (saveManager.gameData.generalGameData.appleAmount >= appleCost &&
                !saveManager.gameData.generalGameData.generalFlags.Contains("SecretMove Obtained"))
            {
                if (!saveManager.gameData.generalGameData.generalFlags.Contains("SecretMove Obtained"))
                {
                    saveManager.gameData.generalGameData.appleAmount -= appleCost;
                    saveManager.gameData.generalGameData.generalFlags.Add("SecretMove Obtained");
                }
            }
            saveManager.SaveGame();
            return false;
        }
    }
    [HarmonyPatch(typeof(scrAppleSwitch), "Update")]
    public static class PatchSecretMove2
    {
        private static bool _answerFix = false;
        private static MethodInfo _checkApplesMethod;
        private const int ScoutID = 166;
        private const int AppleAmount = 249;

        private static bool Prefix(scrAppleSwitch __instance)
        {
            __instance.minAppleAmount = AppleAmount;
            if (_checkApplesMethod == null)
            {
                _checkApplesMethod = AccessTools.Method(typeof(scrAppleSwitch), "CheckApples");
            }
            var secretMoveSetField = AccessTools.Field(typeof(scrAppleSwitch), "secretMoveSet");
            var _secretMoveSet = (bool)secretMoveSetField.GetValue(__instance);
            var timerField = AccessTools.Field(typeof(scrAppleSwitch), "timer");
            var _timer = (float)timerField.GetValue(__instance);
            if (scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("SecretMove Obtained"))
            {
                if (!_secretMoveSet)
                {
                    _secretMoveSet = true;
                    __instance.secretMovePost.SetActive(true);
                    for (int i = 0; i < __instance.objectsToTurnOn.Count; i++)
                    {
                        __instance.objectsToTurnOn[i].SetActive(false);
                    }

                    for (int i = 0; i < __instance.objectsToTurnOff.Count; i++)
                    {
                        __instance.objectsToTurnOff[i].SetActive(false);
                    }
                }
            }
            else
            {
                __instance.secretMovePost.SetActive(false);
                if (__instance.constantCheck)
                {
                    _timer += Time.deltaTime;
                    if ((double) _timer <= (double)__instance.checkInterval)
                    {
                        _checkApplesMethod.Invoke(__instance, []);
                        _timer = 0.0f;
                    }
                }
                var currentBoxField = AccessTools.Field(typeof(scrTextbox), "currentBox");
                int currentBox = (int)currentBoxField.GetValue(scrTextbox.instance);
                if (scrTextbox.instance.isOn && scrTextbox.instance.conversation == "masterNotEnough")
                {
                    if (currentBox == 1 && !_answerFix)
                    {
                        var item = ArchipelagoClient.ScoutLocation(ScoutID);
                        var itemName = Assets.GetItemName(item);
                        var playerName = item.Player.Name;
                        scrTextbox.instance.conversationLocalized[1] =
                            $"You want my secret '{itemName}' for {playerName}, right?\nI mean it's {Assets.GetClassification(item)}";
                        _answerFix = true;
                    }

                    if (currentBox == 2)
                        _answerFix = false;

                    if (currentBox == 3 && !_answerFix)
                    {
                        scrTextbox.instance.conversationLocalized[3] =
                            $"Come back with {AppleAmount+1} apples and It's yours my friend.";
                        _answerFix = true;
                    }
                }
                else
                    _answerFix = false;
            }
            return false;
        }
    }
}