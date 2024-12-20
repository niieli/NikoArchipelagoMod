using HarmonyLib;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class HomeSpawnPatch
{
    [HarmonyPatch(typeof(scrCheckForLevelUnlock), "Update")]
    public static class HomeLevelUnlockPatch
    {
        private static void Prefix(scrCheckForLevelUnlock __instance)
        {
            if (!Plugin.newFile && SceneManager.GetActiveScene().name == "Home")
            {
                __instance.level = 0;
            }
        }
    }
}