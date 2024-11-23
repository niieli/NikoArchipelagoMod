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
                    break;
                case "Trash Kingdom":
                    amountOfFish = ItemHandler.TurbineFishAmount;
                    break;
                case "Salmon Creek Forest":
                    amountOfFish = ItemHandler.SalmonFishAmount;
                    break;
                case "Public Pool":
                    amountOfFish = ItemHandler.PoolFishAmount;
                    break;
                case "The Bathhouse":
                    amountOfFish = ItemHandler.BathFishAmount;
                    break;
                case "Tadpole inc":
                    amountOfFish = ItemHandler.TadpoleFishAmount;
                    break;
                default:
                    return false;
            }
            return amountOfFish >= 5;
        }
    }
}