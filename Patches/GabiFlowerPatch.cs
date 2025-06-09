using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class GabiFlowerPatch
{
    private static string level;
    private static bool blockedLog;
    private static int solvedCount;
    private static float checkTimer;
    private static bool _answerFix, _answerFix2;
    private static int _scoutID;
    [HarmonyPatch(typeof(scrFlowerPuzzleMaster), "Start")]
    public static class MoomyStartPatch
    {
        private static void Postfix(scrHamsterballMaster __instance)
        {
            level = SceneManager.GetActiveScene().name;
            solvedCount = 0;
        }
    }

    [HarmonyPatch(typeof(scrFlowerPuzzleMaster), "Update")]
    public static class MoomyUpdatePatch
    {
        private static int _currentFlowerCount, _neededFlowerCount;
        private static string _currentLevelName;

        [HarmonyPrefix]
        private static bool Prefix(scrFlowerPuzzleMaster __instance)
        {
            if (ArchipelagoData.slotData == null) return true;
            if (!ArchipelagoData.slotData.ContainsKey("flowersanity")) return true;
            if (int.Parse(ArchipelagoData.slotData["flowersanity"].ToString()) != 2) return true;
            var currentBoxField = AccessTools.Field(typeof(scrTextbox), "currentBox");
            int currentBox = (int)currentBoxField.GetValue(scrTextbox.instance);
            if ((double) checkTimer < 1.0)
            {
                checkTimer += Time.deltaTime * 2f;
            }
            else
            {
                checkTimer = 0.0f;
                bool flag = true;
                for (int index = 0; index < __instance.PuzzleParent.childCount; ++index)
                {
                    if (!__instance.PuzzleParent.GetChild(index).GetComponent<scrFlowerPuzzle>().isComplete)
                        flag = false;
                }
                if (!flag) return true;
            }
            if (CheckAllFlowers(level) < __instance.PuzzleParent.childCount)
            {
                //Plugin.BepinLogger.LogInfo($"GabiFlower Condition: {level}, {__instance.PuzzleParent.childCount}, {CheckAllFlowers(level)}");
                __instance.NPCReward.SetActive(false);
                __instance.NPCQuest.SetActive(true);
                if (__instance.NPCQuest.activeSelf)
                    __instance.NPCTrigger.ChangeIcon(scrTextboxTrigger.IconType.quest, true);

                if (scrTextbox.instance.isOn)
                {
                    if (scrTextbox.instance.conversation != "FlowerQuest") return false;
                    if (currentBox == 0 && !_answerFix)
                    {
                        scrTextbox.instance.conversationLocalized[0] =
                            $"Help, I lost my precious {_currentLevelName} flowers to some sort of... multiworld?!" +
                            $"\nI currently have {CheckAllFlowers(level)}/{_neededFlowerCount}!";
                        _answerFix = true;
                    }

                    if (currentBox == 1 && !_answerFix2)
                    {
                        var scout = ArchipelagoClient.ScoutLocation(_scoutID);
                        var playerName = scout.Player.Name;
                        scrTextbox.instance.conversationLocalized[1] =
                            $"I will give you {playerName}'s '{Assets.GetItemName(scout)}' as a reward.\nI heard it's {Assets.GetClassification(scout)} ##end;";
                        _answerFix2 = true;
                    }
                }
                else
                {
                    _answerFix = false;
                    _answerFix2 = false;
                }
                return false;
            }
            return true;
        }
        private static int CheckAllFlowers(string currentLevel)
        {
            int amountOfFlowers = 0;
            switch (currentLevel)
            {
                case "Hairball City":
                    amountOfFlowers = ItemHandler.HairballFlowerAmount;
                    _currentLevelName = "Hairball City";
                    _neededFlowerCount = 3;
                    _scoutID = 5;
                    break;
                case "Trash Kingdom":
                    amountOfFlowers = ItemHandler.TurbineFlowerAmount;
                    _currentLevelName = "Turbine Town";
                    _neededFlowerCount = 3;
                    _scoutID = 19;
                    break;
                case "Salmon Creek Forest":
                    amountOfFlowers = ItemHandler.SalmonFlowerAmount;
                    _currentLevelName = "Salmon Creek Forest";
                    _neededFlowerCount = 6;
                    _scoutID = 37;
                    break;
                case "Public Pool":
                    amountOfFlowers = ItemHandler.PoolFlowerAmount;
                    _currentLevelName = "Public Pool";
                    _neededFlowerCount = 3;
                    _scoutID = 53;
                    break;
                case "The Bathhouse":
                    amountOfFlowers = ItemHandler.BathFlowerAmount;
                    _currentLevelName = "Bathhouse";
                    _neededFlowerCount = 3;
                    _scoutID = 65;
                    break;
                case "Tadpole inc":
                    amountOfFlowers = ItemHandler.TadpoleFlowerAmount;
                    _currentLevelName = "Tadpole HQ";
                    _neededFlowerCount = 4;
                    _scoutID = 73;
                    break;
                default:
                    return -1;
            }
            _currentFlowerCount = amountOfFlowers;
            return amountOfFlowers;
        }
    }
}