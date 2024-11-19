using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class PepperAdvicePatch
{
    [HarmonyPatch(typeof(scrPepperAdvice), "SetupStats")]
    public static class PatchPepperAdviceSetupStats
    {
        private static readonly List<int> coinsPerLevel = new()
            { 1, 6, 6, 10, 6, 9, 10, 0 };

        private static readonly List<int> coinsPerLevelWave1 = new()
            { 0, 4, 3, 3, 0, 0, 0, 0 };

        private static readonly List<int> coinsPerLevelWave2 = new()
            { 0, 4, 2, 3, 4, 5, 0, 0 };
        private static StringBuilder _coinsStringBuilder = new(16);
        private static StringBuilder _cassettesStringBuilder = new(16);
        private static StringBuilder _lettersStringBuilder = new(16);
        private static StringBuilder _keyStringBuilder = new(16);
        static bool Prefix(scrPepperAdvice __instance)
        {
            var saveManagerField = AccessTools.Field(typeof(scrPepperAdvice), "saveManager");
            var saveManager = (scrGameSaveManager)saveManagerField.GetValue(__instance);

            var worldDataField = AccessTools.Field(typeof(scrPepperAdvice), "worldData");
            var worldData = (scrWorldSaveDataContainer)worldDataField.GetValue(__instance);

            // var coinsStringBuilderField = AccessTools.Field(typeof(scrPepperAdvice), "coinsStringBuilder");
            // var coinsStringBuilder = (StringBuilder)coinsStringBuilderField.GetValue(__instance);
            
            bool isWave1 = saveManager.gameData.generalGameData.generalFlags.Contains("APWave1");
            bool isWave2 = saveManager.gameData.generalGameData.generalFlags.Contains("APWave2");
            
            if (isWave2)
            {
                if (isWave1)
                {
                    _coinsStringBuilder.Clear();
                    _coinsStringBuilder.AppendFormat("{0} / {1}", worldData.coinFlags.Count, coinsPerLevel[worldData.worldIndex]+coinsPerLevelWave1[worldData.worldIndex]+coinsPerLevelWave2[worldData.worldIndex]);
                    __instance.coinsTextmesh.SetText(_coinsStringBuilder);
                }
                else
                {
                    _coinsStringBuilder.Clear();
                    _coinsStringBuilder.AppendFormat("{0} / {1}", worldData.coinFlags.Count, coinsPerLevel[worldData.worldIndex]+coinsPerLevelWave2[worldData.worldIndex]);
                    __instance.coinsTextmesh.SetText(_coinsStringBuilder);
                }
            }
            else if (isWave1)
            {
                _coinsStringBuilder.Clear();
                _coinsStringBuilder.AppendFormat("{0} / {1}", worldData.coinFlags.Count, coinsPerLevel[worldData.worldIndex]+coinsPerLevelWave1[worldData.worldIndex]);
                __instance.coinsTextmesh.SetText(_coinsStringBuilder);
            } 
            else
            {
                _coinsStringBuilder.Clear();
                _coinsStringBuilder.AppendFormat("{0} / {1}", worldData.coinFlags.Count, levelData.coinsPerLevel[worldData.worldIndex]);
                __instance.coinsTextmesh.SetText(_coinsStringBuilder);
            }
            
            // var cassettesStringBuilderField = AccessTools.Field(typeof(scrPepperAdvice), "cassettesStringBuilder");
            // var cassettesStringBuilder = (StringBuilder)cassettesStringBuilderField.GetValue(__instance);

            // var lettersStringBuilderField = AccessTools.Field(typeof(scrPepperAdvice), "lettersStringBuilder");
            // var lettersStringBuilder = (StringBuilder)lettersStringBuilderField.GetValue(__instance);

            // var keyStringBuilderField = AccessTools.Field(typeof(scrPepperAdvice), "keyStringBuilder");
            // var keyStringBuilder = (StringBuilder)keyStringBuilderField.GetValue(__instance);
            
            _cassettesStringBuilder.Clear();
            _cassettesStringBuilder.AppendFormat("{0} / {1}", worldData.cassetteFlags.Count, levelData.cassettesPerLevel[worldData.worldIndex]);
            __instance.cassetesTextmesh.SetText(_cassettesStringBuilder);
            
            _lettersStringBuilder.Clear();
            _lettersStringBuilder.AppendFormat("{0} / {1}", worldData.letterFlags.Count, levelData.lettersPerLevel[worldData.worldIndex]);
            __instance.lettersTextmesh.SetText(_lettersStringBuilder);
            
            _keyStringBuilder.Clear();
            _keyStringBuilder.AppendFormat("{0} / {1}", worldData.keyAmount, levelData.keysPerLevel[worldData.worldIndex]);
            __instance.keyTextmesh.SetText(_keyStringBuilder);

            return false; // Skip original method
        }
    }

    [HarmonyPatch(typeof(scrPepperAdvice), "Start")]
    public static class PatchPepperAdviceStart
    {
        static void Postfix(scrPepperAdvice __instance)
        {
            __instance.trigger.gameObject.SetActive(false);
        }
    }
}