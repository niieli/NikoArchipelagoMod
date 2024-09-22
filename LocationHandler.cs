using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net.Enums;
using BepInEx.Logging;
using NikoArchipelago.Archipelago;
using UnityEngine;
using Logger = BepInEx.Logging.Logger;

namespace NikoArchipelago;

public class LocationHandler : MonoBehaviour
{
    private static long baseID = 598_145_444_000;
    public static ManualLogSource BepinLogger;
    private static int index = 0;
    private static List<string> _saveDataCoinFlag;
    private static bool _errored, _errored2;
    
    public static void CheckLocation(int level, long id = 0, string flag = "")
    {
        foreach (var locationEntry in Locations.CoinLocations)
        {
            if (!scrWorldSaveDataContainer.instance.coinFlags[level].Contains(flag)) return;
            if (locationEntry.Value.Flag == flag && locationEntry.Value.Level == level)
            {
                ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
                Plugin.BepinLogger.LogWarning($"Obtained {flag} at {locationEntry.Value.ID} in level {level} successfully!");
            }
        }
    }

    public static async void Update2()
    {
        try
        {
            _saveDataCoinFlag = scrWorldSaveDataContainer.instance.coinFlags;
            var currentLevel = scrGameSaveManager.instance.gameData.generalGameData.currentLevel-1;
            var clel = Plugin.cLevel;
            var cflg = Plugin.cFlags;
            var worldsData = scrGameSaveManager.instance.gameData.worldsData;
            if (cflg.Count > index)
            {
                foreach (var locationEntry in Locations.CoinLocations.Where(locationEntry => 
                             locationEntry.Value.Level == clel && 
                             cflg[index] == locationEntry.Value.Flag))
                {
                    // Check the location if the flag matches
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
                    index++;
                    BepinLogger.LogMessage($"Index is at: {index}, List is '{_saveDataCoinFlag.Count}' long, current flag: {_saveDataCoinFlag[index]}, current level: {currentLevel}");
                    //await ArchipelagoClient.SyncItemsFromDataStorage();
                }
            }
            else if (index > cflg.Count)
            {
                index = 0;
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            if (!_errored)
            {
                // Log the error and reset the index to prevent further out-of-range issues
                Plugin.BepinLogger.LogWarning($"Index out of range: {ex.Message}");
                _errored = true;
                index = 0;
            }
            index = 0;
        }
        catch (Exception ex)
        {
            if (!_errored2)
            {
                // Catch any other exceptions and log them
                Plugin.BepinLogger.LogWarning($"An unexpected error occurred: {ex.Message}");
                _errored2 = true;
            }
        }
    }
    
    public static void WinCompletion()
    {
        //TODO: Still not working
        if (scrGameSaveManager.instance.gameData.generalGameData.currentLevel != 1 ||
            !scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("pepperInterview")) return;
        BepinLogger.LogWarning("HEEEEEEEELEP PEPPER INTERVIEW!");
        ArchipelagoClient.SendCompletion();
    }
}