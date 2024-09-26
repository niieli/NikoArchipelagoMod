using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class AchievementPatch
{
    [HarmonyPatch(typeof(scrAchievementManager), "Update")]
    public static class PatchAchievementManager
    {
        private static bool _lostAtSea,
            _frogFan,
            _hopelessRomantic,
            _employeeOfTheMonth,
            _bottledUp,
            _volleyDreams,
            _snailFashionShow;
        private static MethodInfo _saveAchievementMethod;
        // [HarmonyPrefix]
        // public static bool Prefix(scrAchievementManager __instance)
        // {
        //     var hasInitedField = AccessTools.Field(typeof(scrAchievementManager), "hasInited");
        //     bool hasInited = (bool)hasInitedField.GetValue(__instance);
        //     var lostAtSeaField = AccessTools.Field(typeof(scrAchievementManager), "lostAtSea");
        //     _lostAtSea = (bool)lostAtSeaField.GetValue(__instance);
        //     var frogFanField = AccessTools.Field(typeof(scrAchievementManager), "frogFan");
        //     _frogFan = (bool)frogFanField.GetValue(__instance);
        //     var hopelessRomanticField = AccessTools.Field(typeof(scrAchievementManager), "hopelessRomantic");
        //     _hopelessRomantic = (bool)hopelessRomanticField.GetValue(__instance);
        //     var employeeOfTheMonthField = AccessTools.Field(typeof(scrAchievementManager), "employeeOfTheMonth");
        //     _employeeOfTheMonth = (bool)employeeOfTheMonthField.GetValue(__instance);
        //     var bottledUpField = AccessTools.Field(typeof(scrAchievementManager), "bottledUp");
        //     _bottledUp = (bool)bottledUpField.GetValue(__instance);
        //     var volleyDreamsField = AccessTools.Field(typeof(scrAchievementManager), "volleyDreams");
        //     _volleyDreams = (bool)volleyDreamsField.GetValue(__instance);
        //     var snailFashionShowField = AccessTools.Field(typeof(scrAchievementManager), "snailFashionShow");
        //     _snailFashionShow = (bool)snailFashionShowField.GetValue(__instance);
        //     if (_saveAchievementMethod == null)
        //     {
        //         _saveAchievementMethod = AccessTools.Method(typeof(scrAchievementManager), "SaveAchievement");
        //     }
        //     if (hasInited)
        //     {
        //         SaveAchievementIfNotExists(__instance, _frogFan, __instance.obj_frogFan);
        //         SaveAchievementIfNotExists(__instance, _lostAtSea, __instance.obj_lostAtSea);
        //         SaveAchievementIfNotExists(__instance, _hopelessRomantic, __instance.obj_hopelessRomantic);
        //         SaveAchievementIfNotExists(__instance, _employeeOfTheMonth, __instance.obj_employeeOfTheMonth);
        //         SaveAchievementIfNotExists(__instance, _bottledUp, __instance.obj_bottledUp);
        //         SaveAchievementIfNotExists(__instance, _volleyDreams, __instance.obj_volleyDreams);
        //         SaveAchievementIfNotExists(__instance, _snailFashionShow, __instance.obj_snailFashionShow);
        //         hasInited = true;
        //     }
        //     CheckFrogFanAchievement(__instance);
        //
        //     return false;
        // }
        
        [HarmonyPostfix]
        public static void Postfix(scrAchievementManager __instance)
        {
            if (_saveAchievementMethod == null)
            {
                _saveAchievementMethod = AccessTools.Method(typeof(scrAchievementManager), "SaveAchievement");
            }
            CheckFrogFanAchievement(__instance);
            CheckHopelessRomanticAchievement(__instance);
            CheckBottledUpAchievement(__instance);
            CheckVolleyDreamsAchievement(__instance);
            CheckSnailFashionShowAchievement(__instance);
            __instance.LostAtSea();
            __instance.EmployeeOfTheMonth();
            
        }

        private static void CheckFrogFanAchievement(scrAchievementManager instance)
        {
            string flag = "FROG_FAN";
            if (scrGameSaveManager.instance.gameData.generalGameData.frogBumps < 5) return;
            if (instance.CheckForSavedAchievent(flag)) return;
            if (_saveAchievementMethod != null)
            {
                _saveAchievementMethod.Invoke(instance, [flag]);
            }
            Plugin.APSendNote("Achievement Frog Fan obtained!", 3.5f);
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
            if (instance.CheckForSavedAchievent(flag)) return;
            if (_saveAchievementMethod != null)
            {
                _saveAchievementMethod.Invoke(instance, [flag]);
            }
            Plugin.APSendNote("Achievement Hopeless Romantic obtained!", 3.5f);

        }
        private static void CheckBottledUpAchievement(scrAchievementManager instance)
        {
            string flag = "BOTTLED_UP";
            if (scrGameSaveManager.instance.gameData.generalGameData.bottles < 12) return;
            if (instance.CheckForSavedAchievent(flag)) return;
            if (_saveAchievementMethod != null)
            {
                _saveAchievementMethod.Invoke(instance, [flag]);
            }
            Plugin.APSendNote("Achievement Bottled Up obtained!", 3.5f);
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
            if (instance.CheckForSavedAchievent(flag)) return;
            if (_saveAchievementMethod != null)
            {
                _saveAchievementMethod.Invoke(instance, [flag]);
            }
            Plugin.APSendNote("Achievement Volley Dreams obtained!", 3.5f);
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
            if (instance.CheckForSavedAchievent(flag)) return;
            if (_saveAchievementMethod != null)
            {
                _saveAchievementMethod.Invoke(instance, [flag]);
            }
            Plugin.APSendNote("Achievement Snail Fashion Show obtained!", 3.5f);
        }
    }
}