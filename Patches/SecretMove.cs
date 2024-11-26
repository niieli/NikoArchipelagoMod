using System.Reflection;
using HarmonyLib;
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
        private static MethodInfo _checkApplesMethod;
        private static bool Prefix(scrAppleSwitch __instance)
        {
            __instance.minAppleAmount = 249;
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
            }
            return false;
        }
    }
}