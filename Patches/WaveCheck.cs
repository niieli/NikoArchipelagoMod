using HarmonyLib;

namespace NikoArchipelago.Patches;

public class WaveCheck
{
    [HarmonyPatch(typeof(scrTriggerWave), "Start")]
    public static class PatchTriggerWaveStart
    {
        [HarmonyPostfix]
        static void Postfix(scrTriggerWave __instance)
        {
            var saveManager = scrGameSaveManager.instance;
            if (saveManager == null || !saveManager.loaded) return;

            if (__instance.wave == scrTriggerWave.Waves.wave1 && !saveManager.gameData.generalGameData.generalFlags.Contains("CL1 Obtained"))
            {
                saveManager.gameData.generalGameData.wave1 = false;
            }
            if (__instance.wave == scrTriggerWave.Waves.wave2 && !saveManager.gameData.generalGameData.generalFlags.Contains("CL2 Obtained"))
            {
                saveManager.gameData.generalGameData.wave2 = false;
            }
        }
    }
}