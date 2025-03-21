using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using NikoArchipelago.Archipelago;
using TMPro;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class PepperAdvicePatch
{
    private static TextMeshProUGUI _applesTextMesh;
    private static TextMeshProUGUI _fishTextMesh;
    private static TextMeshProUGUI _flowersTextMesh;
    private static TextMeshProUGUI _seedsTextMesh;
    private static TextMeshProUGUI _locationsTextMesh;

    private static bool _fishingSanity = true;
    private static bool _flowersSanity = true;
    private static bool _seedsSanity = true;
    
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
        private static int waveX = 0;
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
            
            var mitch = 0;
            var mai = 0;
            var fischer = 0;
            var gabi = 0;
            var moomy = 0;
            var blippyBone = 0;

            var adjustCoin = 0;
            switch (worldData.worldIndex)
            {
                case 0:
                    break;
                case 1:
                    if (CassetteCost.MitchGameObject != null)
                    {
                        if (saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MitchGameObject.price 
                            && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                            mitch = -1;
                    }
                    if (CassetteCost.MaiGameObject != null)
                    {
                        if (saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MaiGameObject.price 
                            && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                            mai = -1;
                    }
                    if (_fishingSanity && ItemHandler.HairballFishAmount < 5)
                        fischer = -1;
                    if (_flowersSanity && ItemHandler.HairballFlowerAmount < 3)
                        gabi = -1;
                    if ((_seedsSanity && ItemHandler.HairballSeedAmount < 10) && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                        moomy = -1;
                    break;
                case 2:
                    if (CassetteCost.MitchGameObject != null)
                    {
                        if (saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MitchGameObject.price 
                            && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                            mitch = -1;
                    }
                    if (CassetteCost.MaiGameObject != null)
                    {
                        if (saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MaiGameObject.price 
                            && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                            mai = -1;
                    }
                    if (_fishingSanity && ItemHandler.TurbineFishAmount < 5)
                        fischer = -1;
                    if (_flowersSanity && ItemHandler.TurbineFlowerAmount < 3)
                        gabi = -1;
                    break;
                case 3:
                    if (CassetteCost.MitchGameObject != null)
                    {
                        if (saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MitchGameObject.price)
                            mitch = -1;
                    }
                    if (CassetteCost.MaiGameObject != null)
                    {
                        if (saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MaiGameObject.price 
                            && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1")
                            && saveManager.gameData.generalGameData.keyAmount < 1)
                            mai = -1;
                    }
                    if (_fishingSanity && ItemHandler.SalmonFishAmount < 5 
                        && saveManager.gameData.generalGameData.generalFlags.Contains("APWave1"))
                        fischer = -1;
                    if (_flowersSanity && ItemHandler.SalmonFlowerAmount < 6)
                        gabi = -1;
                    if (_seedsSanity && ItemHandler.SalmonSeedAmount < 10)
                        moomy = -1;
                    break;
                case 4:
                    if (CassetteCost.MitchGameObject != null)
                    {
                        if (saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MitchGameObject.price 
                            && saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                            mitch = -1;
                    }
                    if (CassetteCost.MaiGameObject != null)
                    {
                        if (saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MaiGameObject.price)
                            mai = -1;
                    }
                    if (_fishingSanity && ItemHandler.PoolFishAmount < 5)
                        fischer = -1;
                    if (_flowersSanity && ItemHandler.PoolFlowerAmount < 3
                        && saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                        gabi = -1;
                    if (saveManager.gameData.generalGameData.keyAmount < 1)
                        blippyBone = -1;
                    break;
                case 5:
                    if (CassetteCost.MitchGameObject != null)
                    {
                        if (saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MitchGameObject.price)
                            mitch = -1;
                    }
                    if (CassetteCost.MaiGameObject != null)
                    {
                        if (saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MaiGameObject.price)
                            mai = -1;
                    }
                    if (_fishingSanity && ItemHandler.BathFishAmount < 5
                        && saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                        fischer = -1;
                    if (_flowersSanity && ItemHandler.BathFlowerAmount < 3 
                        && saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                        gabi = -1;
                    if (_seedsSanity && ItemHandler.BathSeedAmount < 10)
                        moomy = -1;
                    if (saveManager.gameData.generalGameData.keyAmount < 1 && saveManager.gameData.generalGameData.generalFlags.Contains("APWave2"))
                        blippyBone = -1;
                    break;
                case 6:
                    if (CassetteCost.MitchGameObject != null)
                    {
                        if (saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MitchGameObject.price)
                            mitch = -1;
                    }
                    if (CassetteCost.MaiGameObject != null)
                    {
                        if (saveManager.gameData.generalGameData.cassetteAmount < CassetteCost.MaiGameObject.price)
                            mai = -1;
                    }
                    if (_fishingSanity && ItemHandler.TadpoleFishAmount < 5)
                        fischer = -1;
                    if (_flowersSanity && ItemHandler.TadpoleFlowerAmount < 4)
                        gabi = -1;
                    if (saveManager.gameData.generalGameData.keyAmount < 1)
                        blippyBone = -1;
                    break;
            }
            adjustCoin = mitch+mai+fischer+gabi+moomy+blippyBone;
            
            if (isWave2)
            {
                if (isWave1)
                {
                    _coinsStringBuilder.Clear();
                    _coinsStringBuilder.AppendFormat("{0} / {1}", worldData.coinFlags.Count, coinsPerLevel[worldData.worldIndex]+coinsPerLevelWave1[worldData.worldIndex]+coinsPerLevelWave2[worldData.worldIndex] + adjustCoin);
                    __instance.coinsTextmesh.SetText(_coinsStringBuilder);
                    waveX = 4;
                }
                else
                {
                    _coinsStringBuilder.Clear();
                    _coinsStringBuilder.AppendFormat("{0} / {1}", worldData.coinFlags.Count, coinsPerLevel[worldData.worldIndex]+coinsPerLevelWave2[worldData.worldIndex] + adjustCoin);
                    __instance.coinsTextmesh.SetText(_coinsStringBuilder);
                    waveX = 2;
                }
            }
            else if (isWave1)
            {
                _coinsStringBuilder.Clear();
                _coinsStringBuilder.AppendFormat("{0} / {1}", worldData.coinFlags.Count, coinsPerLevel[worldData.worldIndex]+coinsPerLevelWave1[worldData.worldIndex] + adjustCoin);
                __instance.coinsTextmesh.SetText(_coinsStringBuilder);
                waveX = 1;
            } 
            else
            {
                _coinsStringBuilder.Clear();
                _coinsStringBuilder.AppendFormat("{0} / {1}", worldData.coinFlags.Count, levelData.coinsPerLevel[worldData.worldIndex] + adjustCoin);
                __instance.coinsTextmesh.SetText(_coinsStringBuilder);
                waveX = 0;
            }
            
            _cassettesStringBuilder.Clear();
            _cassettesStringBuilder.AppendFormat("{0} / {1}", worldData.cassetteFlags.Count, levelData.cassettesPerLevel[worldData.worldIndex]);
            __instance.cassetesTextmesh.SetText(_cassettesStringBuilder);
            
            _lettersStringBuilder.Clear();
            _lettersStringBuilder.AppendFormat("{0} / {1}", worldData.letterFlags.Count, levelData.lettersPerLevel[worldData.worldIndex]);
            __instance.lettersTextmesh.SetText(_lettersStringBuilder);
            
            _keyStringBuilder.Clear();
            _keyStringBuilder.AppendFormat("{0} / {1}", worldData.keyAmount, levelData.keysPerLevel[worldData.worldIndex]);
            __instance.keyTextmesh.SetText(_keyStringBuilder);
            
            if (_applesTextMesh != null)
            {
                var apples = worldData.miscFlags.Count(t => t.StartsWith("Apple"));
                _applesTextMesh.text = apples + " / " + applesPerLevel[worldData.worldIndex];
            }
            else
            {
                Plugin.BepinLogger.LogError("_applesTextMesh is null!");
                _applesTextMesh = __instance.transform.Find("Whiteboard/Canvas/Statistics/StatsapplesBoard(Clone)/text").GetComponent<TextMeshProUGUI>();
            }
            
            if (_fishTextMesh != null)
            {
                var fish = worldData.fishFlags.Count;
                _fishTextMesh.text = fish + " / " + fishPerLevel[worldData.worldIndex];
            }
            else
            {
                Plugin.BepinLogger.LogError("_fishTextMesh is null!");
                _fishTextMesh = __instance.transform.Find("Whiteboard/Canvas/Statistics/StatsfishBoard(Clone)/text").GetComponent<TextMeshProUGUI>();
            }
            
            if (_flowersTextMesh != null)
            {
                var flowers = saveManager.gameData.worldsData[worldData.worldIndex].miscFlags.Count(t => t.StartsWith("FPuzzle"));
                _flowersTextMesh.text = flowers + " / " + flowersPerLevel[worldData.worldIndex];
            }
            else
            {
                Plugin.BepinLogger.LogError("_flowersTextMesh is null!");
                _flowersTextMesh = __instance.transform.Find("Whiteboard/Canvas/Statistics/StatsflowersBoard(Clone)/text").GetComponent<TextMeshProUGUI>();
            }
            
            if (_seedsTextMesh != null)
            {
                var seeds = saveManager.gameData.worldsData[worldData.worldIndex].miscFlags.Count(t => t.StartsWith("Seed"));
                _seedsTextMesh.text = seeds + " / " + seedsPerLevel[worldData.worldIndex];
            }
            else
            {
                Plugin.BepinLogger.LogError("_seedsTextMesh is null!");
                _seedsTextMesh = __instance.transform.Find("Whiteboard/Canvas/Statistics/StatsseedsBoard(Clone)/text").GetComponent<TextMeshProUGUI>();
            }
            
            if (_locationsTextMesh != null)
            {
                if (ArchipelagoClient.TicketCount() == 2)
                {
                    SnailShop[0] = 10;
                } else if (ArchipelagoClient.TicketCount() == 3)
                {
                    SnailShop[0] = 14;
                } else if (ArchipelagoClient.TicketCount() >= 4)
                {
                    SnailShop[0] = 16;
                }
                if (ArchipelagoClient.TicketCount() > 5)
                {
                    Achievements[1] = 8;
                    Achievements[2] = 8;
                    Achievements[3] = 8;
                    Achievements[4] = 8;
                    Achievements[5] = 8;
                    Achievements[6] = 8;
                }

                var totalLocations = coinsPerLevel[worldData.worldIndex]
                                     + levelData.lettersPerLevel[worldData.worldIndex]
                                     + levelData.keysPerLevel[worldData.worldIndex]
                                     + levelData.cassettesPerLevel[worldData.worldIndex]
                                     + applesPerLevel[worldData.worldIndex]
                                     + fishPerLevel[worldData.worldIndex]
                                     + flowersPerLevel[worldData.worldIndex]
                                     + seedsPerLevel[worldData.worldIndex]
                                     + HandsomePerLevel[worldData.worldIndex]
                                     + General[worldData.worldIndex]
                                     + Achievements[worldData.worldIndex]
                                     + KioskPerLevel[worldData.worldIndex]
                                     +SnailShop[0];
                switch (waveX)
                {
                    case 1:
                        totalLocations += coinsPerLevelWave1[worldData.worldIndex];
                        break;
                    case 2:
                        totalLocations += coinsPerLevelWave2[worldData.worldIndex];
                        break;
                    case 4:
                        totalLocations += coinsPerLevelWave1[worldData.worldIndex];
                        totalLocations += coinsPerLevelWave2[worldData.worldIndex];
                        break;
                }
                var locations = saveManager.gameData.worldsData[worldData.worldIndex]
                    .miscFlags
                        .Where(t => !t.StartsWith("Fischer"))
                    .Concat(saveManager.gameData.worldsData[worldData.worldIndex].coinFlags)
                    .Concat(saveManager.gameData.worldsData[worldData.worldIndex].cassetteFlags)
                    .Concat(saveManager.gameData.worldsData[worldData.worldIndex].letterFlags)
                    .Concat(saveManager.gameData.worldsData[worldData.worldIndex].fishFlags)
                    .Count();
                if (worldData.worldIndex == 0)
                {
                    if (saveManager.gameData.generalGameData.generalFlags.Contains($"Kiosk{levelNames[worldData.worldIndex]}"))
                        locations++;
                    if (saveManager.gameData.generalGameData.generalFlags.Contains("FROG_FAN"))
                        locations++;
                    if (saveManager.gameData.generalGameData.generalFlags.Contains("LOST_AT_SEA"))
                        locations++;
                    locations += saveManager.gameData.generalGameData.generalFlags.Count(t => t.StartsWith("Shop"));   
                }
                else
                {
                    if (saveManager.gameData.generalGameData.generalFlags.Contains($"Froggy {levelNames[worldData.worldIndex]}"))
                        locations++;
                    if (saveManager.gameData.generalGameData.generalFlags.Contains("Dustan"))
                        locations++;
                    if (saveManager.gameData.generalGameData.generalFlags.Contains($"Kiosk{levelNames[worldData.worldIndex]}"))
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
                if (worldData.worldIndex == 3 && saveManager.gameData.generalGameData.generalFlags.Contains("CL1 Obtained"))
                {
                    locations++;
                }

                if (worldData.worldIndex == 6)
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
                _locationsTextMesh = __instance.transform.Find("Whiteboard/Canvas/Statistics/StatslocationsBoard(Clone)/text").GetComponent<TextMeshProUGUI>();
            }

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
    
    [HarmonyPatch(typeof(scrPepperAdvice), "SetCurrentFocus")]
    public static class PatchPepperAdviceSetCurrentFocus
    {
        // static void Postfix(scrPepperAdvice __instance)
        // {
        //     var currentFocusField = AccessTools.Field(typeof(scrPepperAdvice), "currentFocus");
        //     var currentFocus = (int)currentFocusField.GetValue(__instance);
        //
        //     if (currentFocus > __instance.cameraPoints.Count)
        //     {
        //         currentFocus = __instance.cameraPoints.Count - 1;
        //     }
        // }
    }
}