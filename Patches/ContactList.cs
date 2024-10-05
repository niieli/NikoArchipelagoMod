using HarmonyLib;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class ContactList
{
    [HarmonyPatch(typeof(scrList), "Start")]
    public static class PatchListStart
    {
        static void Postfix(scrList __instance)
        {
            var saveManager = scrGameSaveManager.instance;
            if (saveManager == null || !saveManager.loaded) return;

            if (__instance.wave == scrList.Waves.wave1 && saveManager.gameData.generalGameData.generalFlags.Contains("CL1 Obtained"))
            {
                Object.Destroy(__instance.gameObject);
            }
            else if (__instance.wave == scrList.Waves.wave2 && saveManager.gameData.generalGameData.generalFlags.Contains("CL2 Obtained"))
            {
                Object.Destroy(__instance.gameObject);
            }
        }
    }
    [HarmonyPatch(typeof(scrList), "Update")]
    public static class PatchListUpdate
    {
        static void Postfix(scrList __instance)
        {
            var saveManager = scrGameSaveManager.instance;
            if (saveManager == null || !saveManager.loaded) return;

            if (!__instance.trigger.foundPlayer()) return;
            __instance.textbox.Initiate(); //Either have Itemname inside Textbox or NO Textbox..

            Object.Instantiate(__instance.particleEffect).transform.position = __instance.transform.position;

            if (__instance.wave == scrList.Waves.wave1)
            {
                if (!saveManager.gameData.generalGameData.generalFlags.Contains("CL1 Obtained"))
                {
                    saveManager.gameData.generalGameData.generalFlags.Add("CL1 Obtained");
                }
            }
            else if (__instance.wave == scrList.Waves.wave2)
            {
                if (!saveManager.gameData.generalGameData.generalFlags.Contains("CL2 Obtained"))
                {
                    saveManager.gameData.generalGameData.generalFlags.Add("CL2 Obtained");
                }
            }

            saveManager.SaveGame();

            Object.Destroy(__instance.gameObject);
        }
    }
}