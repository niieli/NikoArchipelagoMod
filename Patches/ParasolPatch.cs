using System.Collections;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class ParasolPatch
{
    [HarmonyPatch(typeof(scrBouncer), "Update")]
    public static class ParasolPatchUpdate
    {
        private static float _timer;
        private static float _nextToggleTime = 8.5f;
        private static bool _currentState = true;
        private static bool _noticeUp;
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
                return true;
            }
            __instance.objectToAnimate.GetComponent<MeshRenderer>().material.color = new Color(0.09f, 0.09f, 0.09f, 0.85f);

            if (__instance.myTrigger.foundPlayer())
            {
                __instance.StartCoroutine(Notice());
            }
            
            // _timer += Time.deltaTime;
            //
            // if (_timer >= _nextToggleTime)
            // {
            //     _timer = 0f;
            //     _currentState = !_currentState;
            // }
            //
            // __instance.objectToAnimate.gameObject.SetActive(_currentState);
            __instance.objectToAnimate.gameObject.SetActive(GameObjectChecker.IsVisible);
            return false;
        }
        
        private static IEnumerator Notice()
        {
            if (_noticeUp) yield break;
            var t = Object.Instantiate(Plugin.NoticeParasol, Plugin.NotifcationCanvas.transform);
            _noticeUp = true;
            yield return new WaitForSeconds(5f);
            Object.Destroy(t);
            _noticeUp = false;
        }
    }
}