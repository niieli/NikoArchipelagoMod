using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class TrainMapPatch
{
    private static TextMeshProUGUI _applesTextMesh;
    private static TextMeshProUGUI _fishTextMesh;
    private static TextMeshProUGUI _flowersTextMesh;
    private static TextMeshProUGUI _seedsTextMesh;
    private static TextMeshProUGUI _locationsTextMesh;

    private static bool _fishingSanity = true;
    private static bool _flowersSanity = true;
    private static bool _seedsSanity = true;
    private static bool _keySanity = true;
    private static int _cassetteSanity = 2;
    
    private static List<int> applesPerLevel = new()
        { 0, 32, 32, 86, 68, 67, 11, 0 };

    private static List<int> fishPerLevel = new()
        { 0, 5, 5, 5, 5, 5, 5, 0 };

    private static List<int> flowersPerLevel = new()
        { 0, 3, 3, 6, 3, 3, 4, 0 };

    private static List<int> seedsPerLevel = new()
        { 0, 10, 0, 10, 0, 10, 0, 0 };
    
    private static List<int> HandsomePerLevel = new()
        { 0, 1, 1, 1, 1, 1, 0, 0 };
    
    private static List<int> KioskPerLevel = new()
        { 1, 1, 1, 1, 1, 1, 0, 0 };
    
    private static List<int> Achievements = new()
        { 2, 3, 3, 3, 3, 3, 2, 0 };
    
    private static List<int> General = new()
        { 0, 0, 0, 1, 0, 0, 2, 0 };
    
    private static List<int> SnailShop = new()
        { 5 };
    
    private static readonly List<string> levelNames = new()
        { "Home", "Hairball City", "Trash Kingdom", "Salmon Creek Forest", "Public Pool", "The Bathhouse", "Tadpole inc" };

    [HarmonyPatch(typeof(scrTrainMap), "SetupStats")]
    public static class PatchSetupStats
    {
        private static readonly List<int> coinsPerLevel = new()
            { 1, 6, 6, 10, 6, 9, 10, 0 };

        private static readonly List<int> coinsPerLevelWave1 = new()
            { 0, 4, 3, 3, 0, 0, 0, 0 };

        private static readonly List<int> coinsPerLevelWave2 = new()
            { 0, 4, 2, 3, 4, 5, 0, 0 };

        private static int waveX = 0;
        
        static bool Prefix(scrTrainMap __instance)
        {
            var saveManager = scrGameSaveManager.instance;
            if (saveManager == null) return false;
            var localizationManagerField = AccessTools.Field(typeof(scrTrainMap), "localizationManager");
            var localizationManager = (LocalizationManager)localizationManagerField.GetValue(__instance);

            int count;
            
            var mitch = 0;
            var mai = 0;
            var fischer = 0;
            var gabi = 0;
            var moomy = 0;
            var blippyBone = 0;
            var dustan = 0;
            var main = 0;

            var adjustCoin = 0;
            var adjustCassette = 0;
            var adjustLetter = 0;
            switch (__instance.levelSelected)
            {
                case 0:
                    break;
                case 1:
                    if (CassetteCost.MitchGameObject != null)
                    {
                        if (!saveManager.gameData.worldsData[1].coinFlags.Contains("cassetteCoin"))
                            if (((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MitchPrice(1, _cassetteSanity) * 5) 
                                 || (_cassetteSanity == 0 && ItemHandler.HairballCassetteAmount < CassetteCost.MitchPrice(1)))
                                && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                                mitch = -1;
                    }
                    if (CassetteCost.MaiGameObject != null)
                    {
                        if (!saveManager.gameData.worldsData[1].coinFlags.Contains("cassetteCoin2"))
                            if (((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MaiPrice(1, _cassetteSanity) * 5) 
                                 || (_cassetteSanity == 0 && ItemHandler.HairballCassetteAmount < CassetteCost.MaiPrice(1)))
                                && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                                mai = -1;
                    }
                    if (!saveManager.gameData.worldsData[1].coinFlags.Contains("fishing"))
                        if (_fishingSanity && ItemHandler.HairballFishAmount < 5)
                            fischer = -1;
                    if (!saveManager.gameData.worldsData[1].coinFlags.Contains("flowerPuzzle"))
                        if (_flowersSanity && ItemHandler.HairballFlowerAmount < 3)
                            gabi = -1;
                    if (!saveManager.gameData.worldsData[1].coinFlags.Contains("hamsterball"))
                        if (_seedsSanity && ItemHandler.HairballSeedAmount < 10
                                         && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                            moomy = -1;
                    if (!saveManager.gameData.worldsData[1].coinFlags.Contains("hamsterball"))
                        if (_seedsSanity && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                            seedsPerLevel[1] = 0;
                        else seedsPerLevel[1] = 10;
                    if (!saveManager.gameData.worldsData[1].miscFlags.Contains("1"))
                        if ((_keySanity && ItemHandler.HairballKeyAmount < 1) ||
                            (!_keySanity && saveManager.gameData.generalGameData.keyAmount < 1))
                            adjustCassette = -1;
                    break;
                case 2:
                    if (CassetteCost.MitchGameObject != null)
                    {
                        if (!saveManager.gameData.worldsData[2].coinFlags.Contains("cassetteCoin"))
                            if (((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MitchPrice(2, _cassetteSanity) * 5)
                                 || (_cassetteSanity == 0 && ItemHandler.TurbineCassetteAmount < CassetteCost.MitchPrice(2)))
                                && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                                mitch = -1;
                    }
                    if (CassetteCost.MaiGameObject != null)
                    {
                        if (!saveManager.gameData.worldsData[2].coinFlags.Contains("cassetteCoin2"))
                            if (((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MaiPrice(2, _cassetteSanity) * 5) 
                                 || (_cassetteSanity == 0 && ItemHandler.TurbineCassetteAmount < CassetteCost.MaiPrice(2)))
                                && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                                mai = -1;
                    }
                    if (!saveManager.gameData.worldsData[2].coinFlags.Contains("fishing"))
                        if (_fishingSanity && ItemHandler.TurbineFishAmount < 5)
                            fischer = -1;
                    if (!saveManager.gameData.worldsData[2].coinFlags.Contains("flowerPuzzle"))
                        if (_flowersSanity && ItemHandler.TurbineFlowerAmount < 3)
                            gabi = -1;
                    if (!saveManager.gameData.worldsData[2].coinFlags.Contains("Dustan"))
                        if ((_keySanity && ItemHandler.TurbineKeyAmount < 1) || (!_keySanity && saveManager.gameData.generalGameData.keyAmount < 1))
                            dustan = -1;
                    break;
                case 3:
                    if (CassetteCost.MitchGameObject != null)
                    {
                        if (!saveManager.gameData.worldsData[3].coinFlags.Contains("cassetteCoin"))
                            if ((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MitchPrice(3, _cassetteSanity) * 5) 
                                || (_cassetteSanity == 0 && ItemHandler.SalmonCassetteAmount < CassetteCost.MitchPrice(3)))
                                mitch = -1;
                    }
                    if (CassetteCost.MaiGameObject != null)
                    {
                        if (!saveManager.gameData.worldsData[3].coinFlags.Contains("cassetteCoin2"))
                            if (((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MaiPrice(3, _cassetteSanity) * 5)
                                 || (_cassetteSanity == 0 && ItemHandler.SalmonCassetteAmount < CassetteCost.MaiPrice(3)))
                                && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1")
                                && ((_keySanity && ItemHandler.SalmonKeyAmount < 1) || (!_keySanity && saveManager.gameData.generalGameData.keyAmount < 1))
                                && !saveManager.gameData.worldsData[2].coinFlags.Contains("lock2"))
                                mai = -1;
                    }
                    if (!saveManager.gameData.worldsData[3].coinFlags.Contains("fishing"))
                        if (_fishingSanity && ItemHandler.SalmonFishAmount < 5 
                            && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                            fischer = -1;
                    if (!saveManager.gameData.worldsData[3].coinFlags.Contains("fishing"))
                        if (_fishingSanity && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                            fishPerLevel[3] = 0;
                        else fishPerLevel[3] = 5;
                    if (!saveManager.gameData.worldsData[3].coinFlags.Contains("flowerPuzzle"))
                        if (_flowersSanity && ItemHandler.SalmonFlowerAmount < 6)
                            gabi = -1;
                    if (!saveManager.gameData.worldsData[3].coinFlags.Contains("hamsterball"))
                        if (_seedsSanity && ItemHandler.SalmonSeedAmount < 10)
                            moomy = -1;
                    if (!saveManager.gameData.worldsData[2].coinFlags.Contains("lock2"))
                        if ((_keySanity && ItemHandler.SalmonKeyAmount < 1) ||
                            (!_keySanity && saveManager.gameData.generalGameData.keyAmount < 1))
                            adjustLetter = -1;
                    break;
                case 4:
                    if (CassetteCost.MitchGameObject != null)
                    {
                        if (!saveManager.gameData.worldsData[4].coinFlags.Contains("cassetteCoin"))
                            if (((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MitchPrice(4, _cassetteSanity) * 5)
                                 || (_cassetteSanity == 0 && ItemHandler.PoolCassetteAmount < CassetteCost.MitchPrice(4)))
                                && saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                                mitch = -1;
                    }
                    if (CassetteCost.MaiGameObject != null)
                    {
                        if (!saveManager.gameData.worldsData[4].coinFlags.Contains("cassetteCoin2"))
                            if ((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MaiPrice(4, _cassetteSanity) * 5) 
                                || (_cassetteSanity == 0 && ItemHandler.PoolCassetteAmount < CassetteCost.MaiPrice(4)))
                                mai = -1;
                    }
                    if (!saveManager.gameData.worldsData[4].coinFlags.Contains("fishing"))
                        if (_fishingSanity && ItemHandler.PoolFishAmount < 5)
                            fischer = -1;
                    if (!saveManager.gameData.worldsData[4].coinFlags.Contains("flowerPuzzle"))
                        if (_flowersSanity && ItemHandler.PoolFlowerAmount < 3
                            && saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                            gabi = -1;
                    if (!saveManager.gameData.worldsData[4].coinFlags.Contains("flowerPuzzle"))
                        if (_flowersSanity && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                            flowersPerLevel[4] = 0;
                        else flowersPerLevel[4] = 3;
                    if (!saveManager.gameData.worldsData[4].coinFlags.Contains("arcade"))
                        if ((_keySanity && ItemHandler.PoolKeyAmount < 1) || (!_keySanity && saveManager.gameData.generalGameData.keyAmount < 1))
                            blippyBone = -1;
                    break;
                case 5:
                    if (CassetteCost.MitchGameObject != null)
                    {
                        if (!saveManager.gameData.worldsData[5].coinFlags.Contains("cassetteCoin"))
                            if ((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MitchPrice(5, _cassetteSanity) * 5)
                                || (_cassetteSanity == 0 && ItemHandler.BathCassetteAmount < CassetteCost.MitchPrice(5)))
                                mitch = -1;
                    }
                    if (CassetteCost.MaiGameObject != null)
                    {
                        if (!saveManager.gameData.worldsData[5].coinFlags.Contains("cassetteCoin2"))
                            if ((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MaiPrice(5, _cassetteSanity) * 5) 
                                || (_cassetteSanity == 0 && ItemHandler.BathCassetteAmount < CassetteCost.MaiPrice(5)))
                                mai = -1;
                    }
                    if (!saveManager.gameData.worldsData[5].coinFlags.Contains("fishing"))
                        if (_fishingSanity && ItemHandler.BathFishAmount < 5
                            && saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                            fischer = -1;
                    if (!saveManager.gameData.worldsData[5].coinFlags.Contains("fishing"))
                        if (_fishingSanity && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                            fishPerLevel[5] = 0;
                        else fishPerLevel[5] = 5;
                    if (!saveManager.gameData.worldsData[5].coinFlags.Contains("flowerPuzzle"))
                        if (_flowersSanity && ItemHandler.BathFlowerAmount < 3 
                            && saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                            gabi = -1;
                    if (!saveManager.gameData.worldsData[5].coinFlags.Contains("flowerPuzzle"))
                        if (_flowersSanity && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                            flowersPerLevel[5] = 0;
                        else flowersPerLevel[5] = 3;
                    if (!saveManager.gameData.worldsData[5].coinFlags.Contains("hamsterball"))
                        if (_seedsSanity && ItemHandler.BathSeedAmount != 10)
                            moomy = -1;
                    if (!saveManager.gameData.worldsData[5].miscFlags.Contains("arcade"))
                        if (((_keySanity && ItemHandler.BathKeyAmount < 2) || (!_keySanity && saveManager.gameData.generalGameData.keyAmount < 2))
                            && saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                            blippyBone = -1;
                    if (!saveManager.gameData.worldsData[5].miscFlags.Contains("Officelock"))
                        if ((_keySanity && ItemHandler.BathKeyAmount < 2) ||
                            (!_keySanity && saveManager.gameData.generalGameData.keyAmount < 2))
                            main = -1;
                    if (!saveManager.gameData.worldsData[5].miscFlags.Contains("mahjonglock"))
                        if ((_keySanity && ItemHandler.BathKeyAmount < 2) ||
                            (!_keySanity && saveManager.gameData.generalGameData.keyAmount < 2))
                            adjustCassette = -1;
                    break;
                case 6:
                    if (CassetteCost.MitchGameObject != null)
                    {
                        if (!saveManager.gameData.worldsData[6].coinFlags.Contains("cassetteCoin"))
                            if ((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MitchPrice(6, _cassetteSanity) * 5) 
                                || (_cassetteSanity == 0 && ItemHandler.TadpoleCassetteAmount < CassetteCost.MitchPrice(6)))
                                mitch = -1;
                    }
                    if (CassetteCost.MaiGameObject != null)
                    {
                        if (!saveManager.gameData.worldsData[6].coinFlags.Contains("cassetteCoin2"))
                            if ((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MaiPrice(6, _cassetteSanity) * 5) 
                                || (_cassetteSanity == 0 && ItemHandler.TadpoleCassetteAmount < CassetteCost.MaiPrice(6)))
                                mai = -1;
                    }
                    if (!saveManager.gameData.worldsData[6].coinFlags.Contains("fishing"))
                        if (_fishingSanity && ItemHandler.TadpoleFishAmount < 5)
                            fischer = -1;
                    if (!saveManager.gameData.worldsData[6].coinFlags.Contains("flowerPuzzle"))
                        if (_flowersSanity && ItemHandler.TadpoleFlowerAmount < 4)
                            gabi = -1;
                    if (!saveManager.gameData.worldsData[6].coinFlags.Contains("arcade"))
                        if ((_keySanity && ItemHandler.TadpoleKeyAmount < 1) || (!_keySanity && saveManager.gameData.generalGameData.keyAmount < 1))
                            blippyBone = -1;
                    if (!ArchipelagoClient.ElevatorRepaired)
                        adjustLetter = -1;
                    break;
            }
            adjustCoin = mitch+mai+fischer+gabi+moomy+blippyBone+dustan+main;
            
            if (saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
            {
                if (saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                {
                    TextMeshProUGUI coinsTextmesh = __instance.coinsTextmesh;
                    string str3 = saveManager.gameData.worldsData[__instance.levelSelected].coinFlags.Count.ToString();
                    count = coinsPerLevel[__instance.levelSelected] + coinsPerLevelWave1[__instance.levelSelected] +
                            coinsPerLevelWave2[__instance.levelSelected] + adjustCoin;
                    string str4 = count.ToString();
                    coinsTextmesh.text = str3 + " / " + str4;
                    waveX = 4;
                }
                else
                {
                    TextMeshProUGUI coinsTextmesh = __instance.coinsTextmesh;
                    string str3 = saveManager.gameData.worldsData[__instance.levelSelected].coinFlags.Count.ToString();
                    count = coinsPerLevel[__instance.levelSelected] + coinsPerLevelWave2[__instance.levelSelected] + adjustCoin;
                    string str4 = count.ToString();
                    coinsTextmesh.text = str3 + " / " + str4;
                    waveX = 2;
                }
            }
            else if (saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
            {
                TextMeshProUGUI coinsTextmesh = __instance.coinsTextmesh;
                string str1 = saveManager.gameData.worldsData[__instance.levelSelected].coinFlags.Count.ToString();
                count = coinsPerLevel[__instance.levelSelected] + coinsPerLevelWave1[__instance.levelSelected] + adjustCoin;
                string str2 = count.ToString();
                coinsTextmesh.text = str1 + " / " + str2;
                waveX = 1;
            }
            else
            {
                TextMeshProUGUI coinsTextmesh = __instance.coinsTextmesh;
                string str5 = saveManager.gameData.worldsData[__instance.levelSelected].coinFlags.Count.ToString();
                count = levelData.coinsPerLevel[__instance.levelSelected] + adjustCoin;
                string str6 = count.ToString();
                coinsTextmesh.text = str5 + " / " + str6;
                waveX = 0;
            }

            TextMeshProUGUI cassetesTextmesh = __instance.cassetesTextmesh;
            count = saveManager.gameData.worldsData[__instance.levelSelected].cassetteFlags.Count;
            cassetesTextmesh.text = count + " / " + (levelData.cassettesPerLevel[__instance.levelSelected] + adjustCassette);

            TextMeshProUGUI lettersTextmesh = __instance.lettersTextmesh;
            count = saveManager.gameData.worldsData[__instance.levelSelected].letterFlags.Count;
            lettersTextmesh.text = count + " / " + (levelData.lettersPerLevel[__instance.levelSelected] + adjustLetter);

            TextMeshProUGUI keyTextmesh = __instance.keyTextmesh;
            keyTextmesh.text = saveManager.gameData.worldsData[__instance.levelSelected].keyAmount + " / " +
                               levelData.keysPerLevel[__instance.levelSelected];

            __instance.bugsTextmesh.text =
                saveManager.gameData.worldsData[__instance.levelSelected].bugAmount.ToString();
            __instance.levelTitleTextmesh.text =
                localizationManager.GetLocalizedValue(__instance.levelTitleKeys[__instance.levelSelected]);

            for (int index = 0; index < __instance.levelNewIcons.Count; ++index)
            {
                if (saveManager.gameData.generalGameData.newIconLevels.Count >= __instance.levelNewIcons.Count)
                {
                    __instance.levelNewIcons[index]
                        .SetActive(saveManager.gameData.generalGameData.newIconLevels[index]);
                }
            }

            if (_applesTextMesh != null)
            {
                var apples = saveManager.gameData.worldsData[__instance.levelSelected].miscFlags.Count(t => t.StartsWith("Apple"));
                _applesTextMesh.text = apples + " / " + applesPerLevel[__instance.levelSelected];
            }
            else
            {
                Plugin.BepinLogger.LogError("_applesTextMesh is null!");
                _applesTextMesh = __instance.transform.Find("Visuals/Statistics/Statsapples(Clone)/text").GetComponent<TextMeshProUGUI>();
            }
            
            if (_fishTextMesh != null)
            {
                var fish = saveManager.gameData.worldsData[__instance.levelSelected].fishFlags.Count;
                _fishTextMesh.text = fish + " / " + fishPerLevel[__instance.levelSelected];
            }
            else
            {
                Plugin.BepinLogger.LogError("_fishTextMesh is null!");
                _fishTextMesh = __instance.transform.Find("Visuals/Statistics/Statsfish(Clone)/text").GetComponent<TextMeshProUGUI>();
            }
            
            if (_flowersTextMesh != null)
            {
                var flowers = saveManager.gameData.worldsData[__instance.levelSelected].miscFlags.Count(t => t.StartsWith("FPuzzle"));
                _flowersTextMesh.text = flowers + " / " + flowersPerLevel[__instance.levelSelected];
            }
            else
            {
                Plugin.BepinLogger.LogError("_flowersTextMesh is null!");
                _flowersTextMesh = __instance.transform.Find("Visuals/Statistics/Statsflowers(Clone)/text").GetComponent<TextMeshProUGUI>();
            }
            
            if (_seedsTextMesh != null)
            {
                var seeds = saveManager.gameData.worldsData[__instance.levelSelected].miscFlags.Count(t => t.StartsWith("Seed"));
                _seedsTextMesh.text = seeds + " / " + seedsPerLevel[__instance.levelSelected];
            }
            else
            {
                Plugin.BepinLogger.LogError("_seedsTextMesh is null!");
                _seedsTextMesh = __instance.transform.Find("Visuals/Statistics/Statsseeds(Clone)/text").GetComponent<TextMeshProUGUI>();
            }
            
            if (_locationsTextMesh != null)
            {
                if (ArchipelagoClient.TicketCount() == 2 && SnailShop[0] != 0)
                {
                    SnailShop[0] = 10;
                } else if (ArchipelagoClient.TicketCount() == 3 && SnailShop[0] != 0)
                {
                    SnailShop[0] = 14;
                } else if (ArchipelagoClient.TicketCount() >= 4 && SnailShop[0] != 0)
                {
                    SnailShop[0] = 16;
                }
                
                if (ArchipelagoClient.TicketCount() == 5)
                {
                    Achievements[1] = 4;
                    Achievements[2] = 4;
                    Achievements[3] = 4;
                    Achievements[4] = 4;
                    Achievements[5] = 4;
                    Achievements[6] = 4;
                } else if (ArchipelagoClient.TicketCount() >= 6)
                {
                    
                    if (((ItemHandler.SalmonKeyAmount > 0 && ArchipelagoClient.Keysanity) || 
                        (saveManager.gameData.generalGameData.keyAmount > 0 && !ArchipelagoClient.Keysanity)) 
                        && ArchipelagoClient.ElevatorRepaired)
                    {
                        Achievements[1] = 7;
                        Achievements[2] = 7;
                        Achievements[3] = 7;
                        Achievements[4] = 7;
                        Achievements[5] = 7;
                        Achievements[6] = 7; 
                    }
                    else
                    {
                        Achievements[1] = 6;
                        Achievements[2] = 6;
                        Achievements[3] = 6;
                        Achievements[4] = 6;
                        Achievements[5] = 6;
                        Achievements[6] = 6; 
                    }
                    
                }
                if (Plugin.ArchipelagoClient.CoinAmount >= 76)
                {
                    switch (Achievements[1])
                    {
                        case 7:
                            Achievements[1] = 8;
                            Achievements[2] = 8;
                            Achievements[3] = 8;
                            Achievements[4] = 8;
                            Achievements[5] = 8;
                            Achievements[6] = 8;
                            break;
                        case 6:
                            Achievements[1] = 7;
                            Achievements[2] = 7;
                            Achievements[3] = 7;
                            Achievements[4] = 7;
                            Achievements[5] = 7;
                            Achievements[6] = 7;
                            break;
                        case 4:
                            Achievements[1] = 5;
                            Achievements[2] = 5;
                            Achievements[3] = 5;
                            Achievements[4] = 5;
                            Achievements[5] = 5;
                            Achievements[6] = 5;
                            break;
                    }
                }
                
                var totalLocations = coinsPerLevel[__instance.levelSelected]
                                     +levelData.lettersPerLevel[__instance.levelSelected]
                                     +levelData.keysPerLevel[__instance.levelSelected]
                                     +levelData.cassettesPerLevel[__instance.levelSelected]
                                     +applesPerLevel[__instance.levelSelected]
                                     +fishPerLevel[__instance.levelSelected]
                                     +flowersPerLevel[__instance.levelSelected]
                                     +seedsPerLevel[__instance.levelSelected]
                                     +HandsomePerLevel[__instance.levelSelected]
                                     +General[__instance.levelSelected]
                                     +Achievements[__instance.levelSelected]
                                     +KioskPerLevel[__instance.levelSelected]
                                     +SnailShop[0];
                switch (waveX)
                {
                    case 1:
                        totalLocations += coinsPerLevelWave1[__instance.levelSelected];
                        break;
                    case 2:
                        totalLocations += coinsPerLevelWave2[__instance.levelSelected];
                        break;
                    case 4:
                        totalLocations += coinsPerLevelWave1[__instance.levelSelected] 
                                         +coinsPerLevelWave2[__instance.levelSelected];
                        break;
                }
                var locations = saveManager.gameData.worldsData[__instance.levelSelected]
                    .miscFlags
                    .Where(t => !(
                        t.Contains("Fischer") || 
                        t.StartsWith("lock") || 
                        t.Contains("Officelock") || 
                        t.Contains("TurbineLock") || 
                        t.Contains("mahjonglock") ||
                        t == "1"
                    ))
                    .Concat(saveManager.gameData.worldsData[__instance.levelSelected].coinFlags)
                    .Concat(saveManager.gameData.worldsData[__instance.levelSelected].cassetteFlags)
                    .Concat(saveManager.gameData.worldsData[__instance.levelSelected].letterFlags)
                    .Concat(saveManager.gameData.worldsData[__instance.levelSelected].fishFlags)
                    .Count();
                if (__instance.levelSelected == 0)
                {
                    if (saveManager.gameData.generalGameData.generalFlags.Contains($"Kiosk{levelNames[__instance.levelSelected]}"))
                        locations++;
                    if (saveManager.gameData.generalGameData.generalFlags.Contains("FROG_FAN"))
                        locations++;
                    if (saveManager.gameData.generalGameData.generalFlags.Contains("LOST_AT_SEA"))
                        locations++;
                    locations += saveManager.gameData.generalGameData.generalFlags.Count(t => t.StartsWith("Shop"));   
                }
                else
                {
                    if (saveManager.gameData.generalGameData.generalFlags.Contains($"Froggy {levelNames[__instance.levelSelected]}"))
                        locations++;
                    if (saveManager.gameData.generalGameData.generalFlags.Contains("Dustan"))
                        locations++;
                    if (saveManager.gameData.generalGameData.generalFlags.Contains($"Kiosk{levelNames[__instance.levelSelected]}"))
                        locations++;
                    if (saveManager.gameData.generalGameData.generalFlags.Contains("FROG_FAN"))
                        locations++;
                    if (saveManager.gameData.generalGameData.generalFlags.Contains("EMLOYEE_OF_THE_MONTH"))
                        locations++;
                    if (saveManager.gameData.generalGameData.generalFlags.Contains("LOST_AT_SEA"))
                        locations++;
                    if (saveManager.gameData.generalGameData.generalFlags.Contains("HOPELESS_ROMANTIC"))
                        locations++;
                    if (saveManager.gameData.generalGameData.generalFlags.Contains("VOLLEY_DREAMS"))
                        locations++;
                    if (saveManager.gameData.generalGameData.generalFlags.Contains("BOTTLED_UP"))
                        locations++;
                    if (saveManager.gameData.generalGameData.generalFlags.Contains("SNAIL_FASHION_SHOW"))
                        locations++;
                    locations += saveManager.gameData.generalGameData.generalFlags.Count(t => t.StartsWith("Shop"));
                }
                if (__instance.levelSelected == 3 && saveManager.gameData.generalGameData.generalFlags.Contains("CL1 Obtained"))
                {
                    locations++;
                }

                if (__instance.levelSelected == 6)
                {
                    if (saveManager.gameData.generalGameData.generalFlags.Contains("CL2 Obtained"))
                    {
                        locations++;
                    }

                    if (saveManager.gameData.generalGameData.generalFlags.Contains("SecretMove Obtained"))
                    {
                        locations++;
                    }
                }
                _locationsTextMesh.text = locations + " / " + totalLocations;
            }
            else
            {
                Plugin.BepinLogger.LogError("_locationsTextMesh is null!");
                _locationsTextMesh = __instance.transform.Find("Visuals/Statistics/Statslocations(Clone)/text").GetComponent<TextMeshProUGUI>();
            }
            
            return false; // Skip original method
        }
    }

    [HarmonyPatch(typeof(scrTrainMap), "Start")]
    public static class PatchTrainMapStart
    {
        static void Postfix(scrTrainMap __instance)
        {
            if (ArchipelagoData.slotData == null) return;
            if (ArchipelagoData.slotData.ContainsKey("applessanity"))
            {
                if (int.Parse(ArchipelagoData.slotData["applessanity"].ToString()) == 0)
                {
                    applesPerLevel.Clear();
                    applesPerLevel.AddRange(Enumerable.Repeat(0, 8));
                }
            }
            else
            {
                applesPerLevel.Clear();
                applesPerLevel.AddRange(Enumerable.Repeat(0, 8));
            }

            if (ArchipelagoData.slotData.ContainsKey("seedsanity"))
            {
                if (int.Parse(ArchipelagoData.slotData["seedsanity"].ToString()) == 0)
                {
                    seedsPerLevel.Clear();
                    seedsPerLevel.AddRange(Enumerable.Repeat(0, 8));
                    _seedsSanity = false;
                } else if (int.Parse(ArchipelagoData.slotData["seedsanity"].ToString()) == 1)
                {
                    _seedsSanity = false;
                }
            }
            else
            {
                seedsPerLevel.Clear();
                seedsPerLevel.AddRange(Enumerable.Repeat(0, 8));
                _seedsSanity = false;
            }
            
            if (ArchipelagoData.slotData.ContainsKey("flowersanity"))
            {
                if (int.Parse(ArchipelagoData.slotData["flowersanity"].ToString()) == 0)
                {
                    flowersPerLevel.Clear();
                    flowersPerLevel.AddRange(Enumerable.Repeat(0, 8));
                    _flowersSanity = false;
                }else if (int.Parse(ArchipelagoData.slotData["flowersanity"].ToString()) == 1)
                {
                    _flowersSanity = false;
                }
            }
            else
            {
                flowersPerLevel.Clear();
                flowersPerLevel.AddRange(Enumerable.Repeat(0, 8));
                _flowersSanity = false;
            }
            
            if (ArchipelagoData.slotData.ContainsKey("fishsanity"))
            {
                if (int.Parse(ArchipelagoData.slotData["fishsanity"].ToString()) == 0)
                {
                    fishPerLevel.Clear();
                    fishPerLevel.AddRange(Enumerable.Repeat(0, 8));
                    _fishingSanity = false;
                }else if (int.Parse(ArchipelagoData.slotData["fishsanity"].ToString()) == 1)
                {
                    _seedsSanity = false;
                }
            }
            else
            {
                fishPerLevel.Clear();
                fishPerLevel.AddRange(Enumerable.Repeat(0, 8));
                _fishingSanity = false;
            }

            if (ArchipelagoData.slotData.ContainsKey("snailshop"))
            {
                if (int.Parse(ArchipelagoData.slotData["snailshop"].ToString()) == 0)
                { 
                    SnailShop.Clear();
                    SnailShop.AddRange(Enumerable.Repeat(0, 8));
                }
            }
            else
            {
                SnailShop.Clear();
                SnailShop.AddRange(Enumerable.Repeat(0, 8));
            }
            
            if (ArchipelagoData.slotData.ContainsKey("key_level"))
            {
                if (int.Parse(ArchipelagoData.slotData["key_level"].ToString()) == 0)
                {
                    _keySanity = false;
                }
            }
            else
            {
                _keySanity = false;
            }
            
            if (ArchipelagoData.slotData.ContainsKey("cassette_logic"))
            {
                if (int.Parse(ArchipelagoData.slotData["cassette_logic"].ToString()) == 0)
                {
                    _cassetteSanity = 0;
                }else if (int.Parse(ArchipelagoData.slotData["cassette_logic"].ToString()) == 1)
                {
                    _cassetteSanity = 1;
                }
                else
                {
                    _cassetteSanity = 2;
                }
            }
            else
            {
                _cassetteSanity = -1;
            }
        }
    }
}
