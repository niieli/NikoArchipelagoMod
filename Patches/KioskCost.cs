using HarmonyLib;
using UnityEngine;
using UnityEngine.Bindings;

namespace NikoArchipelago.Patches;

public static class KioskCost
{
    private static scrKioskManager _kioskManager;
    private static Plugin plugin;
    private static bool _changed, _changed2;
    
    public static void Init()
    {
        
    }
    [HarmonyPrefix, HarmonyPatch(typeof(levelData))]
    public static void levelData_Prefix()
    {
        levelData.levelPrices[3] = 6;
        levelData.levelPrices[4] = 11;
        levelData.levelPrices[5] = 21;
        levelData.levelPrices[6] = 26;
        levelData.levelPrices[7] = 31;
        levelData.levelPrices[8] = 46;
        if (_changed) return;
        Plugin.BepinLogger.LogInfo("Changed LevelPrices");
        _changed = true;
    }

    [HarmonyPatch(typeof(scrKioskManager), "Update")]  // Patch the "Start" method in scrKioskManager
    public static class KioskLevelFixPatch
    {
        private static scrKioskManager _kioskManager;
        private static bool _changed2 = false;

        [HarmonyPostfix]
        public static void KioskLevelFix(scrKioskManager __instance)
        {
            if (_changed2) return;

            // Access the instance of scrKioskManager
            _kioskManager = __instance;

            // Use reflection to access private field "buyableLevel" and modify it
            var buyableLevelField = AccessTools.Field(typeof(scrKioskManager), "buyableLevel");
            if (buyableLevelField != null)
            {
                int currentBuyableLevel = (int)buyableLevelField.GetValue(_kioskManager);  // Get current value
                buyableLevelField.SetValue(_kioskManager, currentBuyableLevel + 8);  // Add 8 to the buyable level
            }

            // Log and mark as changed
            Plugin.BepinLogger.LogInfo("Changed Kiosk");
            _changed2 = true;
        }
    }
}