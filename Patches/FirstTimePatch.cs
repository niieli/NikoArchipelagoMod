using HarmonyLib;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class FirstTimePatch
{
    [HarmonyPatch(typeof(scrCheckForLevelUnlock), "Update")]
    public static class LevelUnlockPatch
    {
        private static void Postfix(scrCheckForLevelUnlock __instance)
        {
            if (GameObjectChecker.FirstMeeting && GameObject.Find("Pepper Meeting Trigger") != null)
            {
                GameObject.Find("Pepper Meeting Trigger").SetActive(false);
            }
        }
    }
}