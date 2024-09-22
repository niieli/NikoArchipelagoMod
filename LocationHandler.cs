using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net.Enums;
using BepInEx.Logging;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using Logger = BepInEx.Logging.Logger;

namespace NikoArchipelago;

public class LocationHandler : MonoBehaviour
{
    private static long baseID = 598_145_444_000;
    public static ManualLogSource BepinLogger;
    private static int index, casIndex, miscIndex, letterIndex, genIndex, garyIndex;
    private static bool _errored, _errored2, _sent;
    
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
            var clel = Plugin.cLevel;
            var cflg = Plugin.cFlags;
            var casFlag = scrWorldSaveDataContainer.instance.cassetteFlags;
            var miscFlag = scrWorldSaveDataContainer.instance.miscFlags;
            var letterFlag = scrWorldSaveDataContainer.instance.letterFlags;
            var genFlag = scrGameSaveManager.instance.gameData.generalGameData.generalFlags;
            if (cflg.Count > index)
            {
                foreach (var locationEntry in Locations.CoinLocations.Where(locationEntry => 
                             locationEntry.Value.Level == clel && cflg[index] == locationEntry.Value.Flag))
                {
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
                    Plugin.BepinLogger.LogMessage($"Index is at: {index}, List is '{cflg.Count}' long, current flag: {cflg[index]}, current level: {clel}");
                    //await ArchipelagoClient.SyncItemsFromDataStorage();
                }
                index++;
            }
            else if (index > cflg.Count)
            {
                index = 0;
            }
            if (casFlag.Count > casIndex)
            {
                foreach (var locationEntry in Locations.CassetteLocations.Where(locationEntry => 
                             locationEntry.Value.Level == clel && casFlag[casIndex] == locationEntry.Value.Flag))
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
                             locationEntry.Value.Level == clel && miscFlag[miscIndex] == locationEntry.Value.Flag))
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
                             locationEntry.Value.Level == clel && letterFlag[letterIndex] == locationEntry.Value.Flag))
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
            // if (cflg.Count > garyIndex || casFlag.Count > garyIndex)
            // {
            //     foreach (var locationEntry in Locations.GaryGardenLocations.Where(locationEntry => 
            //                  cflg[garyIndex] == locationEntry.Value.Flag && locationEntry.Value.Level == clel || 
            //                  casFlag[garyIndex] == locationEntry.Value.Flag && locationEntry.Value.Level == clel))
            //     {
            //         ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
            //     }
            //     garyIndex++;
            // }
            // else if (garyIndex > cflg.Count || casFlag.Count > garyIndex)
            // {
            //     garyIndex = 0;
            // }
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
            !scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("pepperInterview") || _sent) return;
        ArchipelagoClient.SendCompletion();
        Plugin.APSendNote("YOU'RE HIRED! (Completed Goal)", 10f);
        ArchipelagoConsole.LogMessage("YOU'RE HIRED! (Completed Goal)");
        _sent = true;
    }
}