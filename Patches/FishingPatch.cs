using System.Reflection;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace NikoArchipelago.Patches;

public class FishingPatch
{
    private static string level;
    private static bool blockedLog;
    [HarmonyPatch(typeof(scrFishingMaster), "Start")]
    public static class FishingStartPatch
    {
        private static void Postfix(scrFishingMaster __instance)
        {
          FischerReady();
          level = SceneManager.GetActiveScene().name;
        }
    }
    private static void FischerReady()
    {
        if (!scrWorldSaveDataContainer.instance.miscFlags.Contains("Fischer talked"))
            scrWorldSaveDataContainer.instance.miscFlags.Add("Fischer talked");
    }

    [HarmonyPatch(typeof(scrFishingMaster), "Update")]
    public static class FishingUpdatePatch
    {
        private static MethodInfo _checkIfLastFishMethod;
        private static int _currentFishCount;
        private static string _currentLevelName;

        [HarmonyPrefix]
        private static bool Prefix(scrFishingMaster __instance)
        {
            if (ArchipelagoData.slotData == null) return true;
            if (!ArchipelagoData.slotData.ContainsKey("fishsanity")) return true;
            if (int.Parse(ArchipelagoData.slotData["fishsanity"].ToString()) != 2) return true;
            var textboxTrigger = Traverse.Create(__instance).Field("textboxTrigger").GetValue<scrTextboxTrigger>();
            var textbox = Traverse.Create(__instance).Field("textbox").GetValue<scrTextbox>();
            var gotCoin = Traverse.Create(__instance).Field("gotCoin").GetValue<bool>();

            if (scrWorldSaveDataContainer.instance.fishFlags.Count >= __instance.fishLocations.Count && !__instance.fisherNewFish.activeSelf && !textbox.isOn)
            {
                bool allFishCollected = CheckAllFish(level);
                if (!allFishCollected)
                {
                    if (blockedLog) return false;
                    Plugin.BepinLogger.LogInfo($"Reward blocked: Need all 5 fish of {level}");
                    blockedLog = true;
                    return false;
                }
            } else if (textbox.isOn && textbox.characterName == "Fischer" && !CheckAllFish(level))
            {
                textbox.camIndex = 2;
                textbox.textMesh.text = $"You need all 5 Fish of {_currentLevelName} to obtain my reward!\nYou have {_currentFishCount}/5 for this level!";
                textbox.textMesh.maxVisibleCharacters = 99;
                if (GameInput.GetButtonDown("Action"))
                    textbox.EndConversation();
            }
            return true;
        }

    private static bool CheckAllFish(string currentLevel)
        {
            int amountOfFish = 0;
            switch (currentLevel)
            {
                case "Hairball City":
                    amountOfFish = ItemHandler.HairballFishAmount;
                    _currentLevelName = "Hairball City";
                    break;
                case "Trash Kingdom":
                    amountOfFish = ItemHandler.TurbineFishAmount;
                    _currentLevelName = "Turbine Town";
                    break;
                case "Salmon Creek Forest":
                    amountOfFish = ItemHandler.SalmonFishAmount;
                    _currentLevelName = "Salmon Creek Forest";
                    break;
                case "Public Pool":
                    amountOfFish = ItemHandler.PoolFishAmount;
                    _currentLevelName = "Public Pool";
                    break;
                case "The Bathhouse":
                    amountOfFish = ItemHandler.BathFishAmount;
                    _currentLevelName = "Bathhouse";
                    break;
                case "Tadpole inc":
                    amountOfFish = ItemHandler.TadpoleFishAmount;
                    _currentLevelName = "Tadpole HQ";
                    break;
                default:
                    return false;
            }
            _currentFishCount = amountOfFish;
            return amountOfFish >= 5;
        }
    }
}