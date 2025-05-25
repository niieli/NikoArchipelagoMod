using System.Collections;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class GustPatch
{
    [HarmonyPatch(typeof(scrGust), "Update")]
    public static class GustPatchUpdate
    {
        private static bool _noticeUp;
        private static TriggerChecker _triggerChecker;
        private static bool Prefix(scrGust __instance)
        {
            if (__instance.transform.parent.transform.parent.transform.parent != null)
                if (__instance.transform.parent.transform.parent.transform.parent.name.Contains("Train")) return true;
            if (__instance.transform.parent.transform.parent != null)
                if (__instance.transform.parent.transform.parent.name.Contains("Train")) return true;
            
            if (ArchipelagoData.slotData == null) return true;
            if (!ArchipelagoData.slotData.ContainsKey("ac_repair")) return true;
            if (int.Parse(ArchipelagoData.slotData["ac_repair"].ToString()) != 1) return true;
            
            if (ArchipelagoClient.acRepairAcquired)
            {
                if (__instance.spinner != null)
                    __instance.spinner.speed = 640f;
                if (__instance.particleEffect != null)
                    __instance.particleEffect.SetActive(true);
                if (__instance.transform.parent.Find("Sparks(Clone)"))
                {
                    Object.Destroy(__instance.transform.parent.Find("Sparks(Clone)").gameObject);
                }
                return true;
            }
            _triggerChecker = __instance.GetComponent<TriggerChecker>();
            if (__instance.spinner != null)
                __instance.spinner.speed = 0;
            if (__instance.particleEffect != null)
                __instance.particleEffect.SetActive(false);
            
            if (!__instance.transform.parent.Find("Sparks(Clone)"))
            {
                var sparks = Object.Instantiate(Plugin.SparksParticleSystem, __instance.transform.parent, false);
                sparks.transform.position = __instance.transform.position;
                var lights = sparks.GetComponent<ParticleSystem>().lights;
                lights.light = new Light();
            }

            if (_triggerChecker.foundPlayer())
                __instance.StartCoroutine(Notice());
            return false;
        }
        
        private static IEnumerator Notice()
        {
            if (_noticeUp) yield break;
            var t = Object.Instantiate(Plugin.NoticeAC, Plugin.NotifcationCanvas.transform);
            _noticeUp = true;
            yield return new WaitForSeconds(5f);
            Object.Destroy(t);
            _noticeUp = false;
        }
    }
}