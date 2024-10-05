using System.Text;
using HarmonyLib;

namespace NikoArchipelago.Patches;

public class PepperAdvicePatch
{
    [HarmonyPatch(typeof(scrPepperAdvice), "SetupStats")]
    public static class PatchPepperAdviceSetupStats
    {
        static bool Prefix(scrPepperAdvice __instance)
        {
            var saveManagerField = AccessTools.Field(typeof(scrPepperAdvice), "saveManager");
            var saveManager = (scrGameSaveManager)saveManagerField.GetValue(__instance);

            var worldDataField = AccessTools.Field(typeof(scrPepperAdvice), "worldData");
            var worldData = (scrWorldSaveDataContainer)worldDataField.GetValue(__instance);

            var coinsStringBuilderField = AccessTools.Field(typeof(scrPepperAdvice), "coinsStringBuilder");
            var coinsStringBuilder = (StringBuilder)coinsStringBuilderField.GetValue(__instance);
            
            bool isWave1 = saveManager.gameData.generalGameData.generalFlags.Contains("APWave1");
            bool isWave2 = saveManager.gameData.generalGameData.generalFlags.Contains("APWave2");
            
            if (isWave2)
            {
                coinsStringBuilder.Clear();
                coinsStringBuilder.AppendFormat("{0} / {1}", worldData.coinFlags.Count, levelData.coinsPerLevelWave2[worldData.worldIndex]);
                __instance.coinsTextmesh.SetText(coinsStringBuilder);
            }
            else if (isWave1)
            {
                coinsStringBuilder.Clear();
                coinsStringBuilder.AppendFormat("{0} / {1}", worldData.coinFlags.Count, levelData.coinsPerLevelWave1[worldData.worldIndex]);
                __instance.coinsTextmesh.SetText(coinsStringBuilder);
            }
            else
            {
                coinsStringBuilder.Clear();
                coinsStringBuilder.AppendFormat("{0} / {1}", worldData.coinFlags.Count, levelData.coinsPerLevel[worldData.worldIndex]);
                __instance.coinsTextmesh.SetText(coinsStringBuilder);
            }
            
            var cassettesStringBuilderField = AccessTools.Field(typeof(scrPepperAdvice), "cassettesStringBuilder");
            var cassettesStringBuilder = (StringBuilder)cassettesStringBuilderField.GetValue(__instance);

            var lettersStringBuilderField = AccessTools.Field(typeof(scrPepperAdvice), "lettersStringBuilder");
            var lettersStringBuilder = (StringBuilder)lettersStringBuilderField.GetValue(__instance);

            var keyStringBuilderField = AccessTools.Field(typeof(scrPepperAdvice), "keyStringBuilder");
            var keyStringBuilder = (StringBuilder)keyStringBuilderField.GetValue(__instance);
            
            cassettesStringBuilder.Clear();
            cassettesStringBuilder.AppendFormat("{0} / {1}", worldData.cassetteFlags.Count, levelData.cassettesPerLevel[worldData.worldIndex]);
            __instance.cassetesTextmesh.SetText(cassettesStringBuilder);
            
            lettersStringBuilder.Clear();
            lettersStringBuilder.AppendFormat("{0} / {1}", worldData.letterFlags.Count, levelData.lettersPerLevel[worldData.worldIndex]);
            __instance.lettersTextmesh.SetText(lettersStringBuilder);
            
            keyStringBuilder.Clear();
            keyStringBuilder.AppendFormat("{0} / {1}", worldData.keyAmount, levelData.keysPerLevel[worldData.worldIndex]);
            __instance.keyTextmesh.SetText(keyStringBuilder);

            return false; // Skip original method
        }
    }
}