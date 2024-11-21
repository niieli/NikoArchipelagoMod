using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class KeyRemover
{
    [HarmonyPatch(typeof(scrKey), "Update")]
    public static class KeyUpdatePatch
    {
        private static bool _obtaining;
        [HarmonyPrefix]
        static void Prefix(scrKey __instance)
        {
            if (_obtaining || !__instance.trigger.foundPlayer()) return;
            __instance.StartCoroutine(ObtainKeyModified(__instance));
        }
        private static IEnumerator ObtainKeyModified(scrKey instance)
        {
            _obtaining = true;
            if (!scrWorldSaveDataContainer.instance.miscFlags.Contains(instance.flag) || !scrWorldSaveDataContainer.instance.miscFlags.Contains(instance.flag))
            {
                scrGameSaveManager.instance.gameData.generalGameData.keyAmount--;
                scrWorldSaveDataContainer.instance.SaveWorld();
            }
            yield return null;
        }
    }
}