using HarmonyLib;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class AchievementPatch
{
    private static bool _isClose = false;
    [HarmonyPatch(typeof(scrAchievementManager), "Update")]
    public static class PatchAchievementManager
    {
        [HarmonyPostfix]
        public static void Postfix(scrAchievementManager __instance)
        {
            CheckFrogFanAchievement(__instance);
            CheckHopelessRomanticAchievement(__instance);
            CheckBottledUpAchievement(__instance);
            CheckVolleyDreamsAchievement(__instance);
            CheckSnailFashionShowAchievement(__instance);
            CheckEmployeeOfTheMonthAchievement(__instance);
            CheckLostAtSeaAchievement(__instance);
        }

        private static void CheckFrogFanAchievement(scrAchievementManager instance)
        {
            string flag = "FROG_FAN";
            if (scrGameSaveManager.instance.gameData.generalGameData.frogBumps < 10) return;
            if (CheckForSavedAchievent(flag)) return;
            SaveAchievement(flag);
            var achievement = ScriptableObject.CreateInstance<AchievementObject>();
            achievement.nameKey = "Frog Fan";
            achievement.icon = Assets.FrogFanSprite;
            AchievementPopup.instance.PopupAchievement(achievement);
            AchievementPopup.instance.nameMesh.text = achievement.nameKey;
            //Plugin.APSendNote("Achievement Frog Fan obtained!", 3.5f, Plugin.FrogFanSprite);
        }
        private static void CheckHopelessRomanticAchievement(scrAchievementManager instance)
        {
            string flag = "HOPELESS_ROMANTIC";
            var flags = scrGameSaveManager.instance.gameData.generalGameData.generalFlags;
            if (!flags.Contains("Froggy Hairball City")
                || !flags.Contains("Froggy Trash Kingdom")
                || !flags.Contains("Froggy Salmon Creek Forest")
                || !flags.Contains("Froggy Public Pool")
                || !flags.Contains("Froggy The Bathhouse")) return;
            if (CheckForSavedAchievent(flag)) return;
            SaveAchievement(flag);
            var achievement = ScriptableObject.CreateInstance<AchievementObject>();
            achievement.nameKey = "Hopeless romantic";
            achievement.icon = Assets.HandsomeSprite;
            AchievementPopup.instance.PopupAchievement(achievement);
            AchievementPopup.instance.nameMesh.text = achievement.nameKey;
            //Plugin.APSendNote("Achievement Hopeless Romantic obtained!", 3.5f, Plugin.HandsomeSprite);

        }
        private static void CheckBottledUpAchievement(scrAchievementManager instance)
        {
            string flag = "BOTTLED_UP";
            if (scrGameSaveManager.instance.gameData.generalGameData.bottles < 12) return;
            if (CheckForSavedAchievent(flag)) return;
            SaveAchievement(flag);
            var achievement = ScriptableObject.CreateInstance<AchievementObject>();
            achievement.nameKey = "Bottled up";
            achievement.icon = Assets.BottledSprite;
            AchievementPopup.instance.PopupAchievement(achievement);
            AchievementPopup.instance.nameMesh.text = achievement.nameKey;
            //Plugin.APSendNote("Achievement Bottled Up obtained!", 3.5f, Plugin.BottledSprite);
        }
        private static void CheckEmployeeOfTheMonthAchievement(scrAchievementManager instance)
        {
            string flag = "EMLOYEE_OF_THE_MONTH";
            if (Plugin.ArchipelagoClient.CoinAmount < 76) return;
            if (CheckForSavedAchievent(flag)) return;
            SaveAchievement(flag);
            var achievement = ScriptableObject.CreateInstance<AchievementObject>();
            achievement.nameKey = "Employee of the month!";
            achievement.icon = Assets.EmployeeSprite;
            AchievementPopup.instance.PopupAchievement(achievement);
            AchievementPopup.instance.nameMesh.text = achievement.nameKey;
            //Plugin.APSendNote("Achievement Employee Of The Month obtained!", 3.5f, Plugin.EmployeeSprite);
        }
        private static void CheckLostAtSeaAchievement(scrAchievementManager instance)
        {
            string flag = "LOST_AT_SEA";
            if (_isClose == false) return;
            if (CheckForSavedAchievent(flag)) return;
            SaveAchievement(flag);
            var achievement = ScriptableObject.CreateInstance<AchievementObject>();
            achievement.nameKey = "Lost at sea";
            achievement.icon = Assets.LostSprite;
            AchievementPopup.instance.PopupAchievement(achievement);
            AchievementPopup.instance.nameMesh.text = achievement.nameKey;
            //Plugin.APSendNote("Achievement Lost At Sea obtained!", 3.5f, Plugin.LostSprite);
        }
        private static void CheckVolleyDreamsAchievement(scrAchievementManager instance)
        {
            string flag = "VOLLEY_DREAMS";
            int num = 0;
            for (int index = 0; index < scrGameSaveManager.instance.gameData.worldsData.Count; ++index)
            {
                if (scrGameSaveManager.instance.gameData.worldsData[index].volleyHighscore >= 5)
                    num++;
            }
            if (num < 6) return;
            if (CheckForSavedAchievent(flag)) return;
            SaveAchievement(flag);
            var achievement = ScriptableObject.CreateInstance<AchievementObject>();
            achievement.nameKey = "Volley dreams";
            achievement.icon = Assets.VolleyDreamsSprite;
            AchievementPopup.instance.PopupAchievement(achievement);
            AchievementPopup.instance.nameMesh.text = achievement.nameKey;
            //Plugin.APSendNote("Achievement Volley Dreams obtained!", 3.5f, Plugin.VolleyDreamsSprite);
        }
        private static void CheckSnailFashionShowAchievement(scrAchievementManager instance)
        {
            string flag = "SNAIL_FASHION_SHOW";
            int num = 0;
            for (int index = 0; index < scrGameSaveManager.instance.gameData.generalGameData.snailBoughtClothes.Count; ++index)
            {
                if (scrGameSaveManager.instance.gameData.generalGameData.snailBoughtClothes[index])
                    num++;
            }
            if (num < 16) return;
            if (CheckForSavedAchievent(flag)) return;
            SaveAchievement(flag);
            var achievement = ScriptableObject.CreateInstance<AchievementObject>();
            achievement.nameKey = "Snail fashion show";
            achievement.icon = Assets.SnailFashionSprite;
            AchievementPopup.instance.PopupAchievement(achievement);
            AchievementPopup.instance.nameMesh.text = achievement.nameKey;
            //Plugin.APSendNote("Achievement Snail Fashion Show obtained!", 3.5f, Plugin.SnailFashionSprite);
        }
        
        private static void SaveAchievement(string newAchievement)
        {
            scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Add(newAchievement);
            scrGameSaveManager.instance.SaveGame();
        }
        private static bool CheckForSavedAchievent(string newAchievement)
        {
            for (int i = 0; i < scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Count; i++)
            {
                if (scrGameSaveManager.instance.gameData.generalGameData.generalFlags[i] == newAchievement)
                {
                    return true;
                }
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(scrCoastGaurd), "Update")]
    public static class PatchCoastGaurd
    {
        [HarmonyPostfix]
        private static void Postfix(scrCoastGaurd __instance)
        {
            var isCloseField = AccessTools.Field(typeof(scrCoastGaurd), "isClose");
            bool isClose = (bool)isCloseField.GetValue(__instance);
            if (isClose)
            {
                _isClose = true;
            }
        }
    }
}