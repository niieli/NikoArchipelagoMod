﻿using System.Collections;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class SodaCanPatch
{
    [HarmonyPatch(typeof(scrCannon), "Update")]
    public static class SodaCanPatchUpdate
    {
        private static bool _noticeUp;
        private static bool Prefix(scrCannon __instance)
        {
            if (ArchipelagoData.slotData == null) return true;
            if (!ArchipelagoData.slotData.ContainsKey("soda_cans")) return true;
            if (int.Parse(ArchipelagoData.slotData["soda_cans"].ToString()) != 1) return true;
            
            if (__instance.transform.parent.name == "Working" || __instance.transform.root.name == "CoastGuard") return true;
            if (ArchipelagoClient.SodaRepairAcquired)
            {
                __instance.mesh.gameObject.SetActive(true);
                __instance.mesh.transform.Find("Mesh").gameObject.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
                if (__instance.transform.Find("Sparks(Clone)"))
                    Object.Destroy(__instance.transform.Find("Sparks(Clone)").gameObject);
                return true;
            }
            __instance.mesh.transform.Find("Mesh").gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.25f, 0.25f, 0.25f, 0.85f);
            if (!__instance.transform.Find("Sparks(Clone)"))
            {
                var sparks2 = Object.Instantiate(Plugin.SparksParticleSystem, __instance.transform, false);
                var sparks = Object.Instantiate(Plugin.SparksParticleSystem, __instance.transform, false);
                sparks.transform.position = __instance.mesh.transform.Find("Mesh").position;
                sparks2.transform.position = __instance.mesh.transform.Find("Mesh").position;
                var lights = sparks.GetComponent<ParticleSystem>().lights;
                lights.light = new Light();
                var lights2 = sparks2.GetComponent<ParticleSystem>().lights;
                lights2.light = new Light();
                sparks2.transform.rotation = Quaternion.Euler(sparks2.transform.rotation.x-90f,90f,90f);
            }

            if (__instance.myTrigger.foundPlayer())
            {
                __instance.StartCoroutine(Notice());
            }
            __instance.mesh.gameObject.SetActive(GameObjectChecker.IsVisible);
            return false;
        }
        
        private static IEnumerator Notice()
        {
            if (_noticeUp) yield break;
            var t = Object.Instantiate(Plugin.NoticeSodaCan, Plugin.NotifcationCanvas.transform);
            _noticeUp = true;
            yield return new WaitForSeconds(5f); 
            Object.Destroy(t);
            _noticeUp = false;
        }
    }
}