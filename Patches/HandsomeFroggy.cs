using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class HandsomeFroggy
{
    [HarmonyPatch(typeof(scrHandsomefrogSaver), "Update")]
    public static class PatchHandsomeFroggy
    {
        static void Postfix(scrHandsomefrogSaver __instance)
        {
            var currentScene = SceneManager.GetActiveScene().name;
            var triggeredField = AccessTools.Field(typeof(scrHandsomefrogSaver), "triggered");
            bool triggered = (bool)triggeredField.GetValue(__instance);
            triggered = false;
            var data = scrGameSaveManager.instance.gameData.generalGameData;
            if (!__instance.textboxTrigger.isNewConversation && !triggered && currentScene != "OutsideTrainBetween")
            {
                if (!data.generalFlags.Contains($"Froggy {currentScene}"))
                {
                    triggered = true;
                    data.generalFlags.Add($"Froggy {currentScene}");
                }
            }
        }
    }
}