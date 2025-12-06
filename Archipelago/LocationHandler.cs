using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using KinematicCharacterController.Core;
using UnityEngine;

namespace NikoArchipelago.Archipelago;

public class LocationHandler : MonoBehaviour
{
    private static long baseID = 598_145_444_000;
    private static int coinFlag, casIndex, miscIndex, letterIndex, achIndex, 
        garyIndex, garyIndex2, fishIndex, genIndex, 
        frogIndex, kioskIndex, shopIndex, mimaIndex, garyMiscIndex, globalChatIndex;
    private static bool _errored, _errored2, _sent;
    private static List<bool> shopFlagsList = [..new bool[16]];
    public static ReadOnlyCollection<long> CheckedLocations;

    public static void Update2()
    {
        try
        {
            //Plugin.BepinLogger.LogInfo($"Checked Locations: {CheckedLocations.Length}");
            var currentLevel = scrGameSaveManager.instance.gameData.generalGameData.currentLevel - 1;
            var coinsFlag = scrWorldSaveDataContainer.instance.coinFlags;
            var casFlag = scrWorldSaveDataContainer.instance.cassetteFlags;
            var miscFlag = scrWorldSaveDataContainer.instance.miscFlags;
            var letterFlag = scrWorldSaveDataContainer.instance.letterFlags;
            var genFlag = scrGameSaveManager.instance.gameData.generalGameData.generalFlags;
            var fishFlag = scrWorldSaveDataContainer.instance.fishFlags;
            var worldsData = scrGameSaveManager.instance.gameData.worldsData;
            var globalChatFlag = scrGameSaveManager.instance.gameData.worldsData[0].miscFlags;
            if (coinsFlag.Count > coinFlag)
            {
                foreach (var locationEntry in Locations.CoinLocations.Where(locationEntry => 
                             locationEntry.Value.Level == currentLevel && coinsFlag[LocationHandler.coinFlag] == locationEntry.Value.Flag))
                {
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
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
                var relevantLocations = Locations.KeyLocations.Concat(Locations.SunflowerSeedsLocations)
                    .Concat(Locations.FlowerbedsLocations)
                    .Concat(Locations.ApplesanityLocations)
                    .Concat(Locations.ChatsanityLevelLocations)
                    .Concat(Locations.ChatsanityNikoThoughtsLocations)
                    .Concat(Locations.BugsanityLocations)
                    .Concat(Locations.BonesanityLocations);
                foreach (var locationEntry in relevantLocations.Where(locationEntry => 
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
                foreach (var locationEntry in Locations.AchievementsLocations
                             .Concat(Locations.GeneralLocations)
                             .Concat(Locations.KioskLocations)
                             .Concat(Locations.ProgressiveMitchMaiLocations)
                             .Concat(Locations.HandsomeLocations)
                             .Where(locationEntry => 
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
            if (globalChatFlag.Count > globalChatIndex)
            {
                foreach (var locationEntry in Locations.ChatsanityGlobalLocations.Where(locationEntry => 
                             globalChatFlag[globalChatIndex] == locationEntry.Value.Flag))
                {
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
                }
                globalChatIndex++;
            }
            else if (globalChatIndex > globalChatFlag.Count)
            {
                globalChatIndex = 0;
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
            if (miscFlag.Count > garyMiscIndex)
            {
                foreach (var locationEntry in Locations.ChatsanityLevelGarysGardenLocations
                             .Concat(Locations.GaryGardenSeedLocations)
                             .Where(locationEntry => 
                             worldsData[7].miscFlags[garyMiscIndex] == locationEntry.Value.Flag)) 
                {
                    ArchipelagoClient.OnLocationChecked(locationEntry.Value.ID);
                }
                garyMiscIndex++;
            }
            else if (garyMiscIndex > miscFlag.Count)
            {
                garyMiscIndex = 0;
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
        if (ArchipelagoData.Options.GoalCompletion == ArchipelagoOptions.GoalCompletionMode.Hired)
        {
            if (scrGameSaveManager.instance.gameData.generalGameData.currentLevel != 1 ||
                !scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("pepperInterview") || _sent) return;
            ArchipelagoClient.SendCompletion();
            var achievement = ScriptableObject.CreateInstance<AchievementObject>();
            achievement.nameKey = "YOU'RE HIRED! (Completed Goal)";
            achievement.icon = Assets.GoalBadSprite;
            AchievementPopup.instance.PopupAchievement(achievement);
            AchievementPopup.instance.nameMesh.text = achievement.nameKey;
            AchievementPopup.instance.UIhider.Show(10f);
            ArchipelagoConsole.LogMessage("YOU'RE HIRED! (Completed Goal)");
            _sent = true;
        }
        else if (ArchipelagoData.Options.GoalCompletion == ArchipelagoOptions.GoalCompletionMode.Employee)
        {
            if (Plugin.ArchipelagoClient.CoinAmount < 76 || _sent) return;
            {
                ArchipelagoClient.SendCompletion();
                var achievement = ScriptableObject.CreateInstance<AchievementObject>();
                achievement.nameKey = "BEST EMPLOYEE (Completed Goal)";
                achievement.icon = Assets.EmployeeSprite;
                AchievementPopup.instance.PopupAchievement(achievement);
                AchievementPopup.instance.nameMesh.text = achievement.nameKey;
                AchievementPopup.instance.UIhider.Show(10f);
                ArchipelagoConsole.LogMessage("BEST EMPLOYEE (Completed Goal)");
                _sent = true;
            }
        } 
        else if (ArchipelagoData.Options.GoalCompletion == ArchipelagoOptions.GoalCompletionMode.Custom)
        {
            if (Plugin.ArchipelagoClient.CoinAmount < ArchipelagoData.Options.CustomGoalCoinAmount || _sent) return;
            {
                ArchipelagoClient.SendCompletion();
                var achievement = ScriptableObject.CreateInstance<AchievementObject>();
                achievement.nameKey = "COIN COLLECTOR! (Completed Goal)";
                achievement.icon = Assets.CoinSprite;
                AchievementPopup.instance.PopupAchievement(achievement);
                AchievementPopup.instance.nameMesh.text = achievement.nameKey;
                AchievementPopup.instance.UIhider.Show(10f);
                ArchipelagoConsole.LogMessage("COIN COLLECTOR! (Completed Goal)");
                _sent = true;
            }
        }
        else if (ArchipelagoData.Options.GoalCompletion == ArchipelagoOptions.GoalCompletionMode.Garden)
        {
            //Implement
            var seedCount = scrGameSaveManager.instance.gameData.worldsData[7].miscFlags
                .Count(x => x.StartsWith("GardenSeed"));
            var talkedToGary = scrGameSaveManager.instance.gameData.worldsData[7].coinFlags.Contains("Gary");
            var goal = seedCount >= 10 && talkedToGary;
            if (!talkedToGary || _sent) return;
            {
                ArchipelagoClient.SendCompletion();
                var achievement = ScriptableObject.CreateInstance<AchievementObject>();
                achievement.nameKey = "Restored Gary's Garden! (Completed Goal)";
                achievement.icon = Assets.GoalBadSprite;
                AchievementPopup.instance.PopupAchievement(achievement);
                AchievementPopup.instance.nameMesh.text = achievement.nameKey;
                AchievementPopup.instance.UIhider.Show(10f);
                ArchipelagoConsole.LogMessage("Restored Gary's Garden! (Completed Goal)");
                _sent = true;
            }
        }
        else if (ArchipelagoData.Options.GoalCompletion == ArchipelagoOptions.GoalCompletionMode.Help)
        {
            var worldData = scrGameSaveManager.instance.gameData.worldsData;
            var helpedEveryone = worldData[0].coinFlags.Contains("Fetch") &&
                                 worldData[1].coinFlags.Contains("volley") &&
                                 worldData[1].coinFlags.Contains("Dustan") && worldData[1].coinFlags.Contains("main") &&
                                 worldData[1].coinFlags.Contains("fishing") && worldData[1].coinFlags.Contains("bug") &&
                                 worldData[1].coinFlags.Contains("graffiti") &&
                                 worldData[1].coinFlags.Contains("hamsterball") &&
                                 worldData[1].coinFlags.Contains("cassetteCoin") &&
                                 worldData[1].coinFlags.Contains("cassetteCoin2") &&
                                 worldData[1].coinFlags.Contains("gamerQuest") &&
                                 worldData[1].coinFlags.Contains("arcadeBone") &&
                                 worldData[1].coinFlags.Contains("arcade") &&
                                 worldData[1].coinFlags.Contains("carrynojump") &&
                                 worldData[2].coinFlags.Contains("fishing") &&
                                 worldData[2].coinFlags.Contains("volley") &&
                                 worldData[2].coinFlags.Contains("flowerPuzzle") &&
                                 worldData[2].coinFlags.Contains("main") && worldData[2].coinFlags.Contains("bug") &&
                                 worldData[2].coinFlags.Contains("Dustan") &&
                                 worldData[2].coinFlags.Contains("cassetteCoin") &&
                                 worldData[2].coinFlags.Contains("cassetteCoin2") &&
                                 worldData[2].coinFlags.Contains("arcadeBone") &&
                                 worldData[2].coinFlags.Contains("arcade") &&
                                 worldData[2].coinFlags.Contains("carrynojump") &&
                                 worldData[3].coinFlags.Contains("main") &&
                                 worldData[3].coinFlags.Contains("cassetteCoin") &&
                                 worldData[3].coinFlags.Contains("Dustan") &&
                                 worldData[3].coinFlags.Contains("hamsterball") &&
                                 worldData[3].coinFlags.Contains("arcadeBone") &&
                                 worldData[3].coinFlags.Contains("tree") && worldData[3].coinFlags.Contains("bug") &&
                                 worldData[3].coinFlags.Contains("volley") &&
                                 worldData[3].coinFlags.Contains("flowerPuzzle") &&
                                 worldData[3].coinFlags.Contains("graffiti") &&
                                 worldData[3].coinFlags.Contains("cassetteCoin2") &&
                                 worldData[3].coinFlags.Contains("fishing") &&
                                 worldData[3].coinFlags.Contains("gamerQuest") &&
                                 worldData[3].coinFlags.Contains("arcade") &&
                                 worldData[3].coinFlags.Contains("carrynojump") &&
                                 worldData[4].coinFlags.Contains("cassetteCoin2") &&
                                 worldData[4].coinFlags.Contains("arcadeBone") &&
                                 worldData[4].coinFlags.Contains("arcade") &&
                                 worldData[4].coinFlags.Contains("fishing") &&
                                 worldData[4].coinFlags.Contains("main") && worldData[4].coinFlags.Contains("volley") &&
                                 worldData[4].coinFlags.Contains("bug") &&
                                 worldData[4].coinFlags.Contains("cassetteCoin") &&
                                 worldData[4].coinFlags.Contains("flowerPuzzle") &&
                                 worldData[5].coinFlags.Contains("carrynojump") &&
                                 worldData[5].coinFlags.Contains("hamsterball") &&
                                 worldData[5].coinFlags.Contains("main") &&
                                 worldData[5].coinFlags.Contains("graffiti") &&
                                 worldData[5].coinFlags.Contains("cassetteCoin") &&
                                 worldData[5].coinFlags.Contains("cassetteCoin2") &&
                                 worldData[5].coinFlags.Contains("Dustan") &&
                                 worldData[5].coinFlags.Contains("volley") &&
                                 worldData[5].coinFlags.Contains("gamerQuest") &&
                                 worldData[5].coinFlags.Contains("fishing") && worldData[5].coinFlags.Contains("bug") &&
                                 worldData[5].coinFlags.Contains("flowerPuzzle") &&
                                 worldData[5].coinFlags.Contains("arcadeBone") &&
                                 worldData[5].coinFlags.Contains("arcade") &&
                                 worldData[6].coinFlags.Contains("cassetteCoin2") &&
                                 worldData[6].coinFlags.Contains("cassetteCoin") &&
                                 worldData[6].coinFlags.Contains("main") && worldData[6].coinFlags.Contains("volley") &&
                                 worldData[6].coinFlags.Contains("fishing") &&
                                 worldData[6].coinFlags.Contains("flowerPuzzle") &&
                                 worldData[6].coinFlags.Contains("arcade") && worldData[6].coinFlags.Contains("bug") &&
                                 worldData[6].coinFlags.Contains("carrynojump") &&
                                 worldData[6].coinFlags.Contains("arcadeBone") &&
                                 worldData[7].coinFlags.Contains("Gary") &&
                                 worldData[7].coinFlags.Contains("cassetteCoin2") &&
                                 worldData[7].coinFlags.Contains("cassetteCoin");
            if (!helpedEveryone || _sent) return;
            {
                ArchipelagoClient.SendCompletion();
                var achievement = ScriptableObject.CreateInstance<AchievementObject>();
                achievement.nameKey = "Everyone's Friend! (Completed Goal)";
                achievement.icon = Assets.GoalBadSprite;
                AchievementPopup.instance.PopupAchievement(achievement);
                AchievementPopup.instance.nameMesh.text = achievement.nameKey;
                AchievementPopup.instance.UIhider.Show(10f);
                ArchipelagoConsole.LogMessage("Everyone's Friend! (Completed Goal)");
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