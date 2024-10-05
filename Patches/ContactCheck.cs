using System;
using HarmonyLib;

namespace NikoArchipelago.Patches;

public class ContactCheck
{
    [HarmonyPatch(typeof(scrWaveCheck), "Start")]
    public static class PatchWaveStart
    {
        [HarmonyPostfix]
        static void Postfix(scrWaveCheck __instance)
        {
            __instance.SetObjectsActive(!__instance.turnsOn);
            var saveManager = scrGameSaveManager.instance;
            if (saveManager == null || !saveManager.loaded) return;

            if (__instance.wave == scrWaveCheck.Waves.wave1 && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
            {
                __instance.SetObjectsActive(__instance.turnsOn);
            }
            else if (__instance.wave == scrWaveCheck.Waves.wave2 && saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
            {
                __instance.SetObjectsActive(__instance.turnsOn);
            }
        }
    }
    
    [HarmonyPatch(typeof(scrWaveCheck), "Update")]
    public static class PatchListUpdate
    {
        [HarmonyPostfix]
        static void Postfix(scrWaveCheck __instance)
        {
            var didCheckField = AccessTools.Field(typeof(scrWaveCheck), "didCheck");
            var _didCheck = (bool)didCheckField.GetValue(__instance);
            var saveManager = scrGameSaveManager.instance;
            if (!scrWaveCheck.doCheck || _didCheck) return;
            if (__instance.wave == scrWaveCheck.Waves.wave1 && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
            {
                __instance.SetObjectsActive(__instance.turnsOn);
            }
            else if (__instance.wave == scrWaveCheck.Waves.wave2 && saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
            {
                __instance.SetObjectsActive(__instance.turnsOn);
            }
            _didCheck = true;
        }
    }
}