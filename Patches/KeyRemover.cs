using System.Collections;
using System.Reflection;
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
        static bool Prefix(scrKey __instance)
        {
            if (_obtaining || !__instance.trigger.foundPlayer()) return false;
            __instance.StartCoroutine(ObtainKeyModified(__instance));
            Object.Instantiate(__instance.audioOneshot).GetComponent<scrAudioOneShot>().setup(__instance.sndGet, 0.6f, 1f);
            return false;
        }
        private static IEnumerator ObtainKeyModified(scrKey instance)
        {
            _obtaining = true;
            instance.trigger.enabled = false;
            var waitFor = 2f;
            Object.Destroy(instance.quads);
            if (!scrWorldSaveDataContainer.instance.miscFlags.Contains(instance.flag) || !scrWorldSaveDataContainer.instance.miscFlags.Contains(instance.flag))
            {
                //scrGameSaveManager.instance.gameData.generalGameData.keyAmount--;
                scrWorldSaveDataContainer.instance.keyAmount++;
                scrWorldSaveDataContainer.instance.miscFlags.Add(instance.flag);
                scrWorldSaveDataContainer.instance.SaveWorld();
            }
            while (waitFor > 0)
            {
                yield return null;
                waitFor -= Time.deltaTime;
            }
            _obtaining = false;
            Object.Destroy(instance.gameObject);
        }
    }
}