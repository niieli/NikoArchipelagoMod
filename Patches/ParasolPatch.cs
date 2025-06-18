using System.Collections;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class ParasolPatch
{
    public static bool NoticeUp;
    [HarmonyPatch(typeof(scrBouncer), "Update")]
    public static class ParasolPatchUpdate
    {
        private static float force;
        private static bool Prefix(scrBouncer __instance)
        {
            if (__instance.transform.parent != null)
                if (!__instance.transform.parent.name.Contains("Parasol")) return true;
            if (__instance.transform.parent.transform.parent != null)
                if (__instance.transform.parent.transform.parent.name.Contains("Kiosk")) return true;
            if (ArchipelagoData.slotData == null) return true;
            if (!ArchipelagoData.slotData.ContainsKey("parasols")) return true;
            if (int.Parse(ArchipelagoData.slotData["parasols"].ToString()) != 1) return true;

            if (ArchipelagoClient.ParasolRepairAcquired)
            {
                __instance.objectToAnimate.gameObject.SetActive(true);
                __instance.objectToAnimate.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
                
                if (__instance.force != 0) return true;
                if (__instance.transform.parent.name.Contains("Huge"))
                {
                    __instance.force = 52f;
                }
                else if (__instance.transform.parent.transform.parent.transform.parent)
                {
                    if (__instance.transform.parent.transform.parent.transform.parent.name.Contains("Hamsterball"))
                        __instance.force = 16f;
                }
                else
                    __instance.force = 24f;
                return true;
            }
            if (__instance.objectToAnimate.gameObject != null)
                __instance.objectToAnimate.GetComponent<MeshRenderer>().material.color = new Color(0.09f, 0.09f, 0.09f, 0.85f);

            if (SceneManager.GetActiveScene().name is "Hairball City" or "The Bathhouse" or "Salmon Creek Forest")
            {
                if (__instance.transform.parent != null)
                    if (__instance.transform.parent.name.Contains("Huge"))
                        __instance.force = GameObjectChecker.IsHamsterball ? 0f : 52f;
                if (__instance.transform.parent.transform.parent.transform.parent != null)
                    if (__instance.transform.parent.transform.parent.transform.parent.name.Contains("Hamsterball"))
                        __instance.force = GameObjectChecker.IsHamsterball ? 0f : 16f;
            }
            __instance.force = 0;
            if (__instance.myTrigger.foundPlayer())
            {
                __instance.StartCoroutine(Notice());
            }
            if (__instance.objectToAnimate.gameObject != null)
                __instance.objectToAnimate.gameObject.SetActive(GameObjectChecker.IsVisible);
            return false;
        }
        
        private static IEnumerator Notice()
        {
            if (NoticeUp || !SavedData.Instance.Notices) yield break;
            var t = Object.Instantiate(Plugin.NoticeParasol, Plugin.NotifcationCanvas.transform);
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