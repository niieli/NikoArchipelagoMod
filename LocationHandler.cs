using Archipelago.MultiClient.Net.Enums;
using BepInEx.Logging;
using NikoArchipelago.Archipelago;

namespace NikoArchipelago;

public class LocationHandler
{
    private long baseID = 598_145_444_000;
    public static ManualLogSource BepinLogger;
    private static ArchipelagoClient ArchipelagoClient;
    
    public void CheckLocation(int level,long id, string flag)
    {
        //BepinLogger.LogMessage($"Checking location: {baseID}");
        if (!scrWorldSaveDataContainer.instance.coinFlags[level].Contains(flag)) return;
        ArchipelagoClient.OnLocationChecked(baseID + id);
        BepinLogger.LogWarning($"Obtained {flag} at {baseID+id} in level {level} successfully!");
    }

    public void WinCompletion()
    {
        //TODO: Still not working
        if (scrGameSaveManager.instance.gameData.generalGameData.currentLevel != 1 ||
            !scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("pepperInterview")) return;
        BepinLogger.LogWarning("HEEEEEEEELEP PEPPER INTERVIEW!");
        ArchipelagoClient.SendCompletion();
    }
}