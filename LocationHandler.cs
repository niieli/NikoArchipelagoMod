using System;
using System.Linq;
using BepInEx.Logging;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago;

public class LocationHandler : MonoBehaviour
{
    private static long baseID = 598_145_444_000;
    private static int coinFlag, casIndex, miscIndex, letterIndex, genIndex, garyIndex, garyIndex2, fishIndex;
    private static bool _errored, _errored2, _sent;

    public static async void Update2()
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
            if (genFlag.Count > genIndex)
            {
                foreach (var locationEntry in Locations.AchievementsLocations.Where(locationEntry => 
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
        if (scrGameSaveManager.instance.gameData.generalGameData.currentLevel != 1 ||
            !scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("pepperInterview") || _sent) return;
        ArchipelagoClient.SendCompletion();
        Plugin.APSendNote("YOU'RE HIRED! (Completed Goal)", 10f);
        ArchipelagoConsole.LogMessage("YOU'RE HIRED! (Completed Goal)");
        _sent = true;
    }
}