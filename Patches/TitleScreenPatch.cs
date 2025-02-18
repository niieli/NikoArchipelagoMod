using HarmonyLib;
using KinematicCharacterController.Core;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class TitleScreenPatch
{
    [HarmonyPatch(typeof(scrLevelIntroduction), "Update")]
    public static class PatchUpdateTitleScreen
    {
        static bool Prefix(scrLevelIntroduction __instance)
        {
            if (!Plugin.loggedIn)
            {
                __instance.startGameHider.visible = true;
                __instance.GameTitle.color = new Color(1f, 1f, 1f, Mathf.Clamp(__instance.GameTitle.color.a + Time.deltaTime, 0.0f, 1f));
                if (GameInput.GetButtonDown("Action"))
                {
                    Plugin.BepinLogger.LogFatal("Player is not logged in. Title screen cannot be ended.");
                    return false;
                }
                return false;
            }
            
            return true;
        }
    }
}