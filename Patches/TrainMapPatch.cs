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
    private static TextMeshProUGUI _snailShopTextMesh;
    private static TextMeshProUGUI _chatsanityLevelTextMesh;

    private static bool _fishingSanity = true, _fishingSanityLocation = true;
    private static bool _flowersSanity = true, _flowersSanityLocation = true;
    private static bool _seedsSanity = true, _seedsSanityLocation = true;
    private static bool _keySanity = true;
    private static int _cassetteSanity = 2;
    private static bool _chatsanityLevel = true;
    
    private static List<int> applesPerLevel = new()
        { 0, 32, 33, 126, 94, 72, 14, 0 };

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
        { 2 };
    
    private static List<int> General = new()
        { 0, 0, 0, 1, 0, 0, 2, 0 };
    
    private static List<int> SnailShop = new()
        { 5 };
    
    private static List<int> ChatsanityLevel = new()
        { 37, 41, 35, 46, 37, 44, 38, 16 };
    
    private static readonly List<string> levelNames = new()
        { "Home", "Hairball City", "Trash Kingdom", "Salmon Creek Forest", "Public Pool", "The Bathhouse", "Tadpole inc" };

    [HarmonyPatch(typeof(scrTrainMap), "SetupStats")]
    public static class PatchSetupStats
    {
        private static readonly List<int> coinsPerLevel = new()
            { 1, 6, 6, 10, 6, 9, 10, 0 };

        private static readonly List<int> coinsPerLevelWave1 = new()
            { 0, 5, 3, 3, 0, 0, 0, 0 };

        private static readonly List<int> coinsPerLevelWave2 = new()
            { 0, 3, 2, 3, 4, 5, 0, 0 };

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
                        {
                            if (((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MitchPrice(1, _cassetteSanity) * 5) 
                                 || (_cassetteSanity == 0 && ItemHandler.HairballCassetteAmount < CassetteCost.MitchPrice(1)))
                                && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                                mitch = -1;
                        } else if (saveManager.gameData.worldsData[1].coinFlags.Contains("cassetteCoin")
                                   && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                        {
                            mitch = 1;
                        }
                            
                    }
                    if (CassetteCost.MaiGameObject != null)
                    {
                        if (!saveManager.gameData.worldsData[1].coinFlags.Contains("cassetteCoin2"))
                        {
                            if (((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MaiPrice(1, _cassetteSanity) * 5) 
                                 || (_cassetteSanity == 0 && ItemHandler.HairballCassetteAmount < CassetteCost.MaiPrice(1)))
                                && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                                mai = -1;
                        } else if (saveManager.gameData.worldsData[1].coinFlags.Contains("cassetteCoin2") 
                                   && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                        {
                            mai = 1;
                        }
                            
                    }
                    if (!saveManager.gameData.worldsData[1].coinFlags.Contains("fishing"))
                        if (_fishingSanity && ItemHandler.HairballFishAmount < 5)
                            fischer = -1;
                    if (!saveManager.gameData.worldsData[1].coinFlags.Contains("flowerPuzzle"))
                        if (_flowersSanity && ItemHandler.HairballFlowerAmount < 3)
                            gabi = -1;

                    // Seedsanity
                    if (!saveManager.gameData.worldsData[1].coinFlags.Contains("hamsterball"))
                    {
                        if (_seedsSanity && ItemHandler.HairballSeedAmount < 10
                                         && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                            moomy = -1;
                    } else if (saveManager.gameData.worldsData[1].coinFlags.Contains("hamsterball") 
                               && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                    {
                        moomy = 1;
                    }
                    
                    if (_seedsSanityLocation)
                    {
                        if (saveManager.gameData.worldsData[1].miscFlags.Count(t => t.StartsWith("Seed")) > 0
                            && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                        {
                            seedsPerLevel[1] = saveManager.gameData.worldsData[1].miscFlags.Count(t => t.StartsWith("Seed"));
                        } else if (!saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                        {
                            seedsPerLevel[1] = 0;
                        } else seedsPerLevel[1] = 10;
                    }
                    
                    if (!saveManager.gameData.worldsData[1].miscFlags.Contains("1"))
                        if ((_keySanity && ItemHandler.HairballKeyAmount < 1) ||
                            (!_keySanity && saveManager.gameData.generalGameData.keyAmount < 1))
                            adjustCassette = -1;
                    break;
                case 2:
                    if (CassetteCost.MitchGameObject != null)
                    {
                        if (!saveManager.gameData.worldsData[2].coinFlags.Contains("cassetteCoin"))
                        {
                            if (((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MitchPrice(2, _cassetteSanity) * 5)
                                 || (_cassetteSanity == 0 && ItemHandler.TurbineCassetteAmount < CassetteCost.MitchPrice(2)))
                                && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                                mitch = -1;
                        } else if (saveManager.gameData.worldsData[2].coinFlags.Contains("cassetteCoin")
                                   && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                        {
                            mitch = 1;
                        }
                            
                    }
                    if (CassetteCost.MaiGameObject != null)
                    {
                        if (!saveManager.gameData.worldsData[2].coinFlags.Contains("cassetteCoin2"))
                        {
                            if (((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MaiPrice(2, _cassetteSanity) * 5) 
                                 || (_cassetteSanity == 0 && ItemHandler.TurbineCassetteAmount < CassetteCost.MaiPrice(2)))
                                && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                                mai = -1;
                        } else if (saveManager.gameData.worldsData[2].coinFlags.Contains("cassetteCoin2")
                                   && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                        {
                            mai = 1;
                        }
                            
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
                        {
                            if (((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MaiPrice(3, _cassetteSanity) * 5)
                                 || (_cassetteSanity == 0 && ItemHandler.SalmonCassetteAmount < CassetteCost.MaiPrice(3)))
                                && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1")
                                && ((_keySanity && ItemHandler.SalmonKeyAmount < 1) || (!_keySanity && saveManager.gameData.generalGameData.keyAmount < 1))
                                && !saveManager.gameData.worldsData[2].coinFlags.Contains("lock2"))
                                mai = -1;
                        } else if (saveManager.gameData.worldsData[3].coinFlags.Contains("cassetteCoin2") 
                                   && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                        {
                            mai = 1;
                        }
                    }
                    
                    // Fishsanity
                    if (!saveManager.gameData.worldsData[3].coinFlags.Contains("fishing"))
                    {
                        if (_fishingSanity && ItemHandler.SalmonFishAmount < 5
                                           && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                            fischer = -1;
                    }
                    else if (saveManager.gameData.worldsData[3].coinFlags.Contains("fishing") 
                             && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                        fischer = 1;
                    
                    if (_fishingSanityLocation)
                    {
                        if (saveManager.gameData.worldsData[3].fishFlags.Count > 0 
                            && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                        {
                            fishPerLevel[3] = saveManager.gameData.worldsData[3].fishFlags.Count;
                        } else if (!saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                        {
                            fishPerLevel[3] = 0;
                        } else fishPerLevel[3] = 5;
                    }
                    
                    if (!saveManager.gameData.worldsData[3].coinFlags.Contains("flowerPuzzle"))
                        if (_flowersSanity && ItemHandler.SalmonFlowerAmount < 6)
                            gabi = -1;
                    if (!saveManager.gameData.worldsData[3].coinFlags.Contains("hamsterball"))
                        if (_seedsSanity && ItemHandler.SalmonSeedAmount < 10)
                            moomy = -1;
                    if (!saveManager.gameData.worldsData[3].miscFlags.Contains("lock2") || !saveManager.gameData.worldsData[3].letterFlags.Contains("letter9"))
                        if ((_keySanity && ItemHandler.SalmonKeyAmount < 1) ||
                            (!_keySanity && saveManager.gameData.generalGameData.keyAmount < 1))
                            adjustLetter = -1;
                    break;
                case 4:
                    if (CassetteCost.MitchGameObject != null)
                    {
                        if (!saveManager.gameData.worldsData[4].coinFlags.Contains("cassetteCoin"))
                        {
                            if (((_cassetteSanity is 2 or 1 && saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MitchPrice(4, _cassetteSanity) * 5)
                                 || (_cassetteSanity == 0 && ItemHandler.PoolCassetteAmount < CassetteCost.MitchPrice(4)))
                                && saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                                mitch = -1;
                        } else if (saveManager.gameData.worldsData[4].coinFlags.Contains("cassetteCoin") 
                                   && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                        {
                            mitch = 1;
                        }
                            
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
                    
                    // Flowersanity
                    if (!saveManager.gameData.worldsData[4].coinFlags.Contains("flowerPuzzle"))
                    {
                        if (_flowersSanity && ItemHandler.PoolFlowerAmount < 3 && saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                            gabi = -1;
                    } else if (saveManager.gameData.worldsData[4].coinFlags.Contains("flowerPuzzle")
                               && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                    {
                        gabi = 1;
                    }

                    if (_flowersSanityLocation)
                    {
                        if (saveManager.gameData.worldsData[4].miscFlags.Count(t => t.StartsWith("FPuzzle")) > 0
                            && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                        {
                            flowersPerLevel[4] = saveManager.gameData.worldsData[4].miscFlags.Count(t => t.StartsWith("FPuzzle"));
                        } else if (!saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                        {
                            flowersPerLevel[4] = 0;
                        } else flowersPerLevel[4] = 3;
                    }
                    
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

                    // Fishsanity
                    if (!saveManager.gameData.worldsData[5].coinFlags.Contains("fishing"))
                    {
                        if (_fishingSanity && ItemHandler.BathFishAmount < 5
                            && saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                            fischer = -1;
                    }
                    else if (saveManager.gameData.worldsData[5].coinFlags.Contains("fishing") 
                             && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                        fischer = 1;
                    
                    if (_fishingSanityLocation)
                    {
                        if (saveManager.gameData.worldsData[5].fishFlags.Count > 0 
                            && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                        {
                            fishPerLevel[5] = saveManager.gameData.worldsData[5].fishFlags.Count;
                        } else if (!saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                        {
                            fishPerLevel[5] = 0;
                        } else fishPerLevel[5] = 5;
                    }

                    // Flowersanity
                    if (!saveManager.gameData.worldsData[5].coinFlags.Contains("flowerPuzzle"))
                    {
                        if (_flowersSanity && ItemHandler.BathFlowerAmount < 3 && saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                            gabi = -1;
                    } else if (saveManager.gameData.worldsData[5].coinFlags.Contains("flowerPuzzle")
                               && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                    {
                        gabi = 1;
                    }

                    if (_flowersSanityLocation)
                    {
                        if (saveManager.gameData.worldsData[5].miscFlags.Count(t => t.StartsWith("FPuzzle")) > 0
                            && !saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                        {
                            flowersPerLevel[5] = saveManager.gameData.worldsData[5].miscFlags.Count(t => t.StartsWith("FPuzzle"));
                        } else if (!saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                        {
                            flowersPerLevel[5] = 0;
                        } else flowersPerLevel[5] = 3;
                    }
                        
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
                if (Achievements[0] != 0)
                {
                    if (ArchipelagoClient.TicketCount() < 5)
                    {
                        Achievements[0] = 2;
                    } else if (ArchipelagoClient.TicketCount() == 5)
                    {
                        Achievements[0] = 4;
                    } else if (ArchipelagoClient.TicketCount() >= 6)
                    {
                    
                        if (((ItemHandler.SalmonKeyAmount > 0 && ArchipelagoClient.Keysanity) || 
                             (saveManager.gameData.generalGameData.keyAmount > 0 && !ArchipelagoClient.Keysanity)) 
                            && ArchipelagoClient.ElevatorRepaired)
                        {
                            Achievements[0] = 7;
                        }
                        else
                        {
                            Achievements[0] = 6;
                        }
                    
                    }
                    if (Plugin.ArchipelagoClient.CoinAmount >= 76)
                    {
                        Achievements[0] = Achievements[0] switch
                        {
                            7 => 8,
                            6 => 7,
                            4 => 5,
                            _ => Achievements[0]
                        };
                    }
                    var totalLocations = Achievements[0];
                    var locations = 0;
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
                    if (locations > Achievements[0])
                        _locationsTextMesh.text = locations + " / " + locations;
                    else 
                        _locationsTextMesh.text = locations + " / " + totalLocations;
                }
                else
                {
                    _locationsTextMesh.text = "X";
                }
                
            }
            else
            {
                Plugin.BepinLogger.LogError("_locationsTextMesh is null!");
                _locationsTextMesh = __instance.transform.Find("Visuals/Statistics/Statslocations(Clone)/text").GetComponent<TextMeshProUGUI>();
            }
            
            if (_snailShopTextMesh != null)
            {
                if (SnailShop[0] != 0)
                {
                    if (ArchipelagoClient.TicketCount() == 1)
                    {
                        SnailShop[0] = 5;
                    }
                    else if (ArchipelagoClient.TicketCount() == 2)
                    {
                        SnailShop[0] = 10;
                    } else if (ArchipelagoClient.TicketCount() == 3)
                    {
                        SnailShop[0] = 14;
                    } else if (ArchipelagoClient.TicketCount() >= 4)
                    {
                        SnailShop[0] = 16;
                    }
                    var shop = saveManager.gameData.generalGameData.generalFlags.Count(t => t.StartsWith("Shop"));
                    if (shop > SnailShop[0])
                        _snailShopTextMesh.text = shop + " / " + shop;
                    else 
                        _snailShopTextMesh.text = shop + " / " + SnailShop[0];
                }
                else
                {
                    _snailShopTextMesh.text = "X";
                }
            }
            else
            {
                Plugin.BepinLogger.LogError("_snailShopTextMesh is null!");
                _snailShopTextMesh = __instance.transform.Find("Visuals/Statistics/Statssnailshop(Clone)/text").GetComponent<TextMeshProUGUI>();
            }
            
            if (_chatsanityLevelTextMesh != null)
            {
                var chats = saveManager.gameData.worldsData[__instance.levelSelected].miscFlags.Count(t => t.StartsWith("CHAT"));
                _chatsanityLevelTextMesh.text = chats + " / " + ChatsanityLevel[__instance.levelSelected];
            }
            else
            {
                Plugin.BepinLogger.LogError("_chatsanityLevelTextMesh is null!");
                _chatsanityLevelTextMesh = __instance.transform.Find("Visuals/Statistics/Statschatsanity(Clone)/text").GetComponent<TextMeshProUGUI>();
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
                    _seedsSanityLocation = false;
                } else if (int.Parse(ArchipelagoData.slotData["seedsanity"].ToString()) == 1)
                {
                    _seedsSanity = false;
                    _seedsSanityLocation = true;
                }
            }
            else
            {
                seedsPerLevel.Clear();
                seedsPerLevel.AddRange(Enumerable.Repeat(0, 8));
                _seedsSanity = false;
                _seedsSanityLocation = false;
            }
            
            if (ArchipelagoData.slotData.ContainsKey("flowersanity"))
            {
                if (int.Parse(ArchipelagoData.slotData["flowersanity"].ToString()) == 0)
                {
                    flowersPerLevel.Clear();
                    flowersPerLevel.AddRange(Enumerable.Repeat(0, 8));
                    _flowersSanity = false;
                    _flowersSanityLocation = false;
                }else if (int.Parse(ArchipelagoData.slotData["flowersanity"].ToString()) == 1)
                {
                    _flowersSanity = false;
                    _flowersSanityLocation = true;
                }
            }
            else
            {
                flowersPerLevel.Clear();
                flowersPerLevel.AddRange(Enumerable.Repeat(0, 8));
                _flowersSanity = false;
                _flowersSanityLocation = false;
            }
            
            if (ArchipelagoData.slotData.ContainsKey("fishsanity"))
            {
                if (int.Parse(ArchipelagoData.slotData["fishsanity"].ToString()) == 0)
                {
                    fishPerLevel.Clear();
                    fishPerLevel.AddRange(Enumerable.Repeat(0, 8));
                    _fishingSanity = false;
                    _fishingSanityLocation = false;
                } else if (int.Parse(ArchipelagoData.slotData["fishsanity"].ToString()) == 1)
                {
                    _fishingSanity = false;
                    _fishingSanityLocation = true;
                }
            }
            else
            {
                fishPerLevel.Clear();
                fishPerLevel.AddRange(Enumerable.Repeat(0, 8));
                _fishingSanity = false;
                _fishingSanityLocation = false;
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
                } else if (int.Parse(ArchipelagoData.slotData["cassette_logic"].ToString()) == 1)
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
            
            if (ArchipelagoData.slotData.ContainsKey("achievements"))
            {
                if (int.Parse(ArchipelagoData.slotData["achievements"].ToString()) == 2)
                { 
                    Achievements.Clear();
                    Achievements.AddRange(Enumerable.Repeat(0, 8));
                }
            }
            else
            {
                Achievements.Clear();
                Achievements.AddRange(Enumerable.Repeat(0, 8));
            }
            
            if (ArchipelagoData.slotData.ContainsKey("chatsanity"))
            {
                if (int.Parse(ArchipelagoData.slotData["chatsanity"].ToString()) == 0)
                { 
                    ChatsanityLevel.Clear();
                    ChatsanityLevel.AddRange(Enumerable.Repeat(0, 8));
                }
            }
            else
            {
                ChatsanityLevel.Clear();
                ChatsanityLevel.AddRange(Enumerable.Repeat(0, 8));
            }
        }
    }
}
