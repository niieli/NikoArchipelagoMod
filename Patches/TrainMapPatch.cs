using System.Collections.Generic;
using HarmonyLib;
using KinematicCharacterController.Core;
using TMPro;

namespace NikoArchipelago.Patches;

public class TrainMapPatch
{

    [HarmonyPatch(typeof(scrTrainMap), "SetupStats")]
    public static class PatchSetupStats
    {
        private static readonly List<int> coinsPerLevel = new()
        { 1, 6, 6, 10, 6, 9, 10, 0 };

        private static readonly List<int> coinsPerLevelWave1 = new()
        { 0, 4, 3, 3, 0, 0, 0, 0 };

        private static readonly List<int> coinsPerLevelWave2 = new()
        { 0, 4, 2, 3, 4, 5, 0, 0 };
        static bool Prefix(scrTrainMap __instance)
        {
            var saveManager = scrGameSaveManager.instance;
            if (saveManager == null) return false;
            var localizationManagerField = AccessTools.Field(typeof(scrTrainMap), "localizationManager");
            var localizationManager = (LocalizationManager)localizationManagerField.GetValue(__instance);

            int count;
            if (saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
            {
                if (saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                {
                    TextMeshProUGUI coinsTextmesh = __instance.coinsTextmesh;
                    string str3 = saveManager.gameData.worldsData[__instance.levelSelected].coinFlags.Count.ToString();
                    count = coinsPerLevel[__instance.levelSelected]+coinsPerLevelWave1[__instance.levelSelected]+coinsPerLevelWave2[__instance.levelSelected];
                    string str4 = count.ToString();
                    coinsTextmesh.text = str3 + " / " + str4;
                }
                else
                {
                    TextMeshProUGUI coinsTextmesh = __instance.coinsTextmesh;
                    string str3 = saveManager.gameData.worldsData[__instance.levelSelected].coinFlags.Count.ToString();
                    count = coinsPerLevel[__instance.levelSelected]+coinsPerLevelWave2[__instance.levelSelected];
                    string str4 = count.ToString();
                    coinsTextmesh.text = str3 + " / " + str4;
                }
            } 
            else if (saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
            {
                TextMeshProUGUI coinsTextmesh = __instance.coinsTextmesh;
                string str1 = saveManager.gameData.worldsData[__instance.levelSelected].coinFlags.Count.ToString();
                count = coinsPerLevel[__instance.levelSelected]+coinsPerLevelWave1[__instance.levelSelected];
                string str2 = count.ToString();
                coinsTextmesh.text = str1 + " / " + str2;
            } 
            else
            {
                TextMeshProUGUI coinsTextmesh = __instance.coinsTextmesh;
                string str5 = saveManager.gameData.worldsData[__instance.levelSelected].coinFlags.Count.ToString();
                count = levelData.coinsPerLevel[__instance.levelSelected];
                string str6 = count.ToString();
                coinsTextmesh.text = str5 + " / " + str6;
            }
            TextMeshProUGUI cassetesTextmesh = __instance.cassetesTextmesh;
            count = saveManager.gameData.worldsData[__instance.levelSelected].cassetteFlags.Count;
            cassetesTextmesh.text = count + " / " + levelData.cassettesPerLevel[__instance.levelSelected];

            TextMeshProUGUI lettersTextmesh = __instance.lettersTextmesh;
            count = saveManager.gameData.worldsData[__instance.levelSelected].letterFlags.Count;
            lettersTextmesh.text = count + " / " + levelData.lettersPerLevel[__instance.levelSelected];

            TextMeshProUGUI keyTextmesh = __instance.keyTextmesh;
            keyTextmesh.text = saveManager.gameData.worldsData[__instance.levelSelected].keyAmount + " / " + levelData.keysPerLevel[__instance.levelSelected];

            __instance.bugsTextmesh.text = saveManager.gameData.worldsData[__instance.levelSelected].bugAmount.ToString();
            __instance.levelTitleTextmesh.text = localizationManager.GetLocalizedValue(__instance.levelTitleKeys[__instance.levelSelected]);

            for (int index = 0; index < __instance.levelNewIcons.Count; ++index)
            {
                if (saveManager.gameData.generalGameData.newIconLevels.Count >= __instance.levelNewIcons.Count)
                {
                    __instance.levelNewIcons[index].SetActive(saveManager.gameData.generalGameData.newIconLevels[index]);
                }
            }

            return false; // Skip original method
        } 
    } 
}
