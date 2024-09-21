using System.Collections;
using System.Collections.Generic;
using Archipelago.MultiClient.Net.Enums;
using BepInEx.Logging;
using NikoArchipelago.Archipelago;

namespace NikoArchipelago;

public class LocationHandler
{
    private long baseID = 598_145_444_000;
    public static ManualLogSource BepinLogger;
    public static Plugin Plugin;
    
    private static scrWorldSaveDataContainer _worldSaveDataContainer;
    private static ArchipelagoClient _archipelagoClient;
    
    // Initialization method to set up the required dependencies
    public static void Initialize(scrWorldSaveDataContainer dataContainer, ArchipelagoClient client)
    {
        _worldSaveDataContainer = dataContainer;
        _archipelagoClient = client;
        Plugin.BepinLogger.LogInfo("LocationHandler Initialized");
    }
    public void CheckLocation(int level,long id, string flag)
    {
        Location locations = Locations.GetLocation(id);
        if (!scrWorldSaveDataContainer.instance.coinFlags[level].Contains(flag)) return;
        _archipelagoClient.OnLocationChecked(baseID + id);
        Plugin.BepinLogger.LogWarning($"Obtained {flag} at {baseID+id} in level {level} successfully!");
    }

    private static HashSet<long> checkedLocations = new HashSet<long>(); // To track checked location IDs

    public static void CheckLocationsForFlags(int level)
    {
        if (scrWorldSaveDataContainer.instance == null)
        {
            Plugin.BepinLogger.LogError("worldSaveDataContainer is null in CheckLocationsForFlags.");
            return;
        }

        if (_archipelagoClient == null)
        {
            Plugin.BepinLogger.LogError("archipelagoClient is null in CheckLocationsForFlags.");
            return;
        }

        // Get the flag string for the current level
        var levelFlag = scrWorldSaveDataContainer.instance.coinFlags[level];
        if (string.IsNullOrEmpty(levelFlag))
        {
            Plugin.BepinLogger.LogWarning($"No flag found for level {level}.");
            return;
        }

        // Iterate through each Location object in the dictionary
        foreach (var locationEntry in Locations.LocationNames)
        {
            var location = locationEntry.Value;
            if (string.IsNullOrEmpty(location.Flag))
            {
                Plugin.BepinLogger.LogWarning($"Location {location.ID} has an empty flag.");
                continue;
            }
            Plugin.BepinLogger.LogInfo($"Checking location: {location.ID} with flag {location.Flag}");

            // Skip if this location has already been checked
            if (checkedLocations.Contains(location.ID))
            {
                Plugin.BepinLogger.LogInfo($"Location {location.ID} has already been checked.");
                continue;
            }

            // Check if the location's flag matches the flag for the current level
            if (location.Level == level && location.Flag == levelFlag)
            {
                Plugin.BepinLogger.LogInfo($"Match found for flag {location.Flag} at location {location.ID}");

                _archipelagoClient.OnLocationChecked(location.ID);
                Plugin.BepinLogger.LogWarning($"Obtained {location.Flag} at {location.ID} in level {level} successfully!");

                // Add this location to the checked list to avoid checking it again
                checkedLocations.Add(location.ID);
            }
        }
    }
    public void WinCompletion()
    {
        //TODO: Still not working
        if (scrGameSaveManager.instance.gameData.generalGameData.currentLevel != 1 ||
            !scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("pepperInterview")) return;
        BepinLogger.LogWarning("HEEEEEEEELEP PEPPER INTERVIEW!");
        _archipelagoClient.SendCompletion();
    }
}