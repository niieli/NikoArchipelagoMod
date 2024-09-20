using HarmonyLib;

namespace NikoArchipelago.Patches;

public class KioskCost
{
    private static scrKioskManager _kioskManager;
    public static void Init()
    {
        scrKioskManager_Prefix();
        levelData_Prefix();
    }
    [HarmonyPrefix, HarmonyPatch(typeof(levelData))]
    public static void levelData_Prefix()
    {
        levelData.levelPrices[3] = 6;
        levelData.levelPrices[4] = 11;
        levelData.levelPrices[5] = 21;
        levelData.levelPrices[6] = 26;
        levelData.levelPrices[7] = 31;
    }

    [HarmonyPrefix, HarmonyPatch(typeof(scrKioskManager), "RemoveIfObtained", MethodType.Enumerator)]
    public static void scrKioskManager_Prefix()
    {
        _kioskManager.buyableLevel += 8;
    }
}