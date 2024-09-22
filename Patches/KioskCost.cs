using HarmonyLib;
using UnityEngine;
using UnityEngine.Bindings;

namespace NikoArchipelago.Patches;

public class KioskCost
{
    private static scrKioskManager _kioskManager;
    private static Plugin plugin;
    private static bool _changed;
    public KioskCost()
    {
        _kioskManager = GameObject.Find("Kiosk").GetComponent<scrKioskManager>();
    }
    
    public static void Init()
    {
        Harmony.CreateAndPatchAll(typeof(levelData));
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

    [HarmonyPatch(typeof(scrKioskManager), "Start", MethodType.Constructor)]
    public static void KioskLevelFix()
    {
        _kioskManager.buyableLevel += 8;
    }
}