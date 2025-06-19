using System.Collections;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class GustPatch
{
    public static bool NoticeUp;
    [HarmonyPatch(typeof(scrGust), "Update")]
    public static class GustPatchUpdate
    {
        private static TriggerChecker _triggerChecker;
        private static bool Prefix(scrGust __instance)
        {
            if (__instance.transform.parent.transform.parent.transform.parent != null)
            {
                if (__instance.transform.parent.transform.parent.transform.parent.name.Contains("Train")) return true;
                if (__instance.transform.parent.transform.parent.transform.parent.name.Contains("Elevator")) return true;
            }
            if (__instance.transform.parent.name.Contains("Water")) return true;
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
                if (__instance.transform.parent.Find("Sparks(Clone)") != null)
                {
                    Object.Destroy(__instance.transform.parent.Find("Sparks(Clone)").gameObject);
                }
                if (__instance.GetComponent<AudioSource>() != null)
                    __instance.GetComponent<AudioSource>().mute = false;
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

            if (GameObjectChecker.IsHamsterball)
            {
                _triggerChecker.gameObject.SetActive(false);
                if (__instance.GetComponentInParent<AudioSource>())
                    __instance.GetComponentInParent<AudioSource>().mute = true;
            }
            else
            {
                _triggerChecker.gameObject.SetActive(true);
                if (__instance.GetComponentInParent<AudioSource>())
                    __instance.GetComponentInParent<AudioSource>().mute = false;
            }
            
            if (_triggerChecker.foundPlayer())
                __instance.StartCoroutine(Notice());
            return false;
        }
        
        private static IEnumerator Notice()
        {
            if (NoticeUp || !SavedData.Instance.Notices) yield break;
            var t = Object.Instantiate(Plugin.NoticeAC, Plugin.NotifcationCanvas.transform);
            NoticeUp = true;
            var time = 0f;
            while (time < 60f)
            {
                time += Time.deltaTime;
                yield return null;
            }
            Object.Destroy(t);
            NoticeUp = false;
        }
    }
}