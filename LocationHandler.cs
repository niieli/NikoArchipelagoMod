using System;
using System.Collections.Generic;
using System.Linq;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago;

public class LocationHandler : MonoBehaviour
{
    private static long baseID = 598_145_444_000;
    private static int coinFlag, casIndex, miscIndex, letterIndex, achIndex, garyIndex, garyIndex2, fishIndex, genIndex, frogIndex, kioskIndex, shopIndex;
    private static bool _errored, _errored2, _sent;
    private static List<bool> shopFlagsList = [..new bool[16]];

    public static void Update2()
    {
        try
        {
            var currentLevel = scrGameSaveManager.instance.gameData.generalGameData.currentLevel - 1;
            var coinsFlag = scrWorldSaveDataContainer.instance.coinFlags;
            var casFlag = scrWorldSaveDataContainer.instance.cassetteFlags;
            var miscFlag = scrWorldSaveDataContainer.instance.miscFlags;
            var letterFlag = scrWorldSaveDataContainer.instance.letterFlags;
            var genFlag = scrGameSaveManager.instance.gameData.generalGameData.generalFlags;
            var fishFlag = scrWorldSaveDataContainer.instance.fishFlags;
            var worldsData = scrGameSaveManager.instance.gameData.worldsData;
            if (coinsFlag.Count > coinFlag)
            {
                foreach (var locationEntry in Locations.CoinLocations.Where(locationEntry => 
                             locationEntry.Value.Level == currentLevel && coinsFlag[LocationHandler.coinFlag] == locationEntry.Value.Flag))
                {
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
                    //await ArchipelagoClient.SyncItemsFromDataStorage();
                }
                coinFlag++;
            }
            else if (coinFlag > coinsFlag.Count)
            {
                coinFlag = 0;
            }
            if (casFlag.Count > casIndex)
            {
                foreach (var locationEntry in Locations.CassetteLocations.Where(locationEntry => 
                             locationEntry.Value.Level == currentLevel && casFlag[casIndex] == locationEntry.Value.Flag))
                {
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID); 
                }
                casIndex++;
            }
            else if (casIndex > casFlag.Count)
            {
                casIndex = 0;
            }
            if (miscFlag.Count > miscIndex)
            {
                foreach (var locationEntry in Locations.KeyLocations.Where(locationEntry => 
                             locationEntry.Value.Level == currentLevel && miscFlag[miscIndex] == locationEntry.Value.Flag))
                {
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
                }
                miscIndex++;
            }
            else if (miscIndex > miscFlag.Count)
            {
                miscIndex = 0;
            }
            if (letterFlag.Count > letterIndex)
            {
                foreach (var locationEntry in Locations.LetterLocations.Where(locationEntry => 
                             locationEntry.Value.Level == currentLevel && letterFlag[letterIndex] == locationEntry.Value.Flag))
                {
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
                }
                letterIndex++;
            }
            else if (letterIndex > letterFlag.Count)
            {
                letterIndex = 0;
            }
            if (genFlag.Count > achIndex)
            {
                foreach (var locationEntry in Locations.AchievementsLocations.Where(locationEntry => 
                             genFlag[achIndex] == locationEntry.Value.Flag))
                {
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
                }
                achIndex++;
            }
            else if (achIndex > genFlag.Count)
            {
                achIndex = 0;
            }
            if (genFlag.Count > genIndex)
            {
                foreach (var locationEntry in Locations.GeneralLocations.Where(locationEntry => 
                             genFlag[genIndex] == locationEntry.Value.Flag))
                {
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
                }
                genIndex++;
            }
            else if (genIndex > genFlag.Count)
            {
                genIndex = 0;
            }
            if (genFlag.Count > frogIndex)
            {
                foreach (var locationEntry in Locations.HandsomeLocations.Where(locationEntry => 
                             genFlag[frogIndex] == locationEntry.Value.Flag))
                {
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
                }
                frogIndex++;
            }
            else if (frogIndex > genFlag.Count)
            {
                frogIndex = 0;
            }
            if (genFlag.Count > kioskIndex)
            {
                foreach (var locationEntry in Locations.KioskLocations.Where(locationEntry => 
                             genFlag[kioskIndex] == locationEntry.Value.Flag))
                {
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
                }
                kioskIndex++;
            }
            else if (kioskIndex > genFlag.Count)
            {
                kioskIndex = 0;
            }
            if (fishFlag.Count > fishIndex)
            {
                foreach (var locationEntry in Locations.FishsanityLocations.Where(locationEntry => 
                             fishFlag[fishIndex] == locationEntry.Value.Flag && locationEntry.Value.Level == currentLevel))
                {
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
                }
                fishIndex++;
            }
            else if (fishIndex > fishFlag.Count)
            {
                fishIndex = 0;
            }
            if (coinsFlag.Count > garyIndex)
            {
                foreach (var locationEntry in Locations.GaryGardenCoinLocations.Where(locationEntry => 
                             worldsData[7].coinFlags[garyIndex] == locationEntry.Value.Flag)) 
                {
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
                }
                garyIndex++;
            }
            else if (garyIndex > coinsFlag.Count)
            {
                garyIndex = 0;
            }
            if (casFlag.Count > garyIndex2)
            {
                foreach (var locationEntry in Locations.GaryGardenCassetteLocations.Where(locationEntry => 
                             worldsData[7].cassetteFlags[garyIndex2] == locationEntry.Value.Flag))
                {
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
                }
                garyIndex2++;
            }
            else if (casFlag.Count > garyIndex2)
            {
                garyIndex2 = 0;
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            if (!_errored)
            {
                Plugin.BepinLogger.LogWarning($"Index out of range: {ex.Message}");
                _errored = true;
            }
        }
        catch (Exception ex)
        {
            if (!_errored2)
            {
                Plugin.BepinLogger.LogWarning($"An unexpected error occurred: {ex.Message}");
                _errored2 = true;
            }
        }
    }
    
    public static void WinCompletion()
    {
        if (int.Parse(ArchipelagoData.slotData["goal_completion"].ToString()) == 0)
        {
            if (scrGameSaveManager.instance.gameData.generalGameData.currentLevel != 1 ||
                !scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("pepperInterview") || _sent) return;
            ArchipelagoClient.SendCompletion();
            var achievement = ScriptableObject.CreateInstance<AchievementObject>();
            achievement.nameKey = "YOU'RE HIRED! (Completed Goal)";
            achievement.icon = Plugin.ApProgressionSprite;
            AchievementPopup.instance.PopupAchievement(achievement);
            AchievementPopup.instance.nameMesh.text = achievement.nameKey;
            AchievementPopup.instance.UIhider.Show(10f);
            ArchipelagoConsole.LogMessage("YOU'RE HIRED! (Completed Goal)");
            _sent = true;
        }
        else
        {
            if (scrGameSaveManager.instance.gameData.generalGameData.coinAmountTotal != 76 || _sent) return;
            {
                ArchipelagoClient.SendCompletion();
                var achievement = ScriptableObject.CreateInstance<AchievementObject>();
                achievement.nameKey = "BEST EMPLOYEE (Completed Goal)";
                achievement.icon = Plugin.EmployeeSprite;
                AchievementPopup.instance.PopupAchievement(achievement);
                AchievementPopup.instance.nameMesh.text = achievement.nameKey;
                AchievementPopup.instance.UIhider.Show(10f);
                ArchipelagoConsole.LogMessage("BEST EMPLOYEE (Completed Goal)");
                _sent = true;
            }
        }
    }

    public static void SnailShop()
    {
        var shopFlag = scrGameSaveManager.instance.gameData.generalGameData.snailBoughtClothes;
        for (shopIndex = 0; shopIndex < shopFlagsList.Count; shopIndex++)
        {
            if (shopFlag[shopIndex] && !shopFlagsList[shopIndex])
            {
                foreach (var locationEntry in Locations.SnailShopLocations.Where(locationEntry => 
                             shopFlag[shopIndex] && shopIndex == locationEntry.Value.Level && !scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("Shop"+shopIndex)))
                {
                    scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Add("Shop"+shopIndex);
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
                    Plugin.BepinLogger.LogInfo("Snail Shop Location: " + locationEntry.Value.ID + " | " + locationEntry.Value.Level);
                }
                shopFlagsList[shopIndex] = true;
                shopIndex++;
            }
        }
    }
}