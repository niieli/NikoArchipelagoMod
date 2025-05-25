using System.Collections;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class BreakableBlocksPatch
{
    private static bool _playedNoBonk, _noticeUp;
    private static readonly int Timer = Animator.StringToHash("Timer");

    [HarmonyPatch(typeof(scrBreakBlock), "Update")]
    public static class BonkPatchUpdate
    {
        private static bool Prefix(scrBreakBlock __instance)
        {
            if (ArchipelagoData.slotData == null) return true;
            if (!ArchipelagoData.slotData.ContainsKey("bonk_permit")) return true;
            if (int.Parse(ArchipelagoData.slotData["bonk_permit"].ToString()) != 1) return true;
            if (__instance.isLock) return true;

            if (ArchipelagoClient.BonkPermitAcquired) return ArchipelagoClient.BonkPermitAcquired;
            if (__instance.trigger.foundPlayer() &&
                MyCharacterController.instance.state == MyCharacterController.States.Diving)
            {
                if (!_playedNoBonk)
                {
                    // if (__instance.sndNoKeys.Count > 0) //TODO: Add custom sound
                    //     Object.Instantiate<GameObject>(__instance.audioOneshot).GetComponent<scrAudioOneShot>().setup(__instance.sndNoKeys, 0.3f, 1f);
                    _playedNoBonk = true;
                    __instance.StartCoroutine(BonkNotice());
                    Plugin.BepinLogger.LogInfo("You need the Bonk Helmet to break this block!");
                }
                return false;
            }
            if (MyCharacterController.instance.state != MyCharacterController.States.Diving)
                _playedNoBonk = false;
            return false;
        }

        private static IEnumerator BonkNotice()
        {
            if (_noticeUp) yield break;
            var t = Object.Instantiate(Plugin.NoticeBonkHelmet, Plugin.NotifcationCanvas.transform);
            _noticeUp = true;
            yield return new WaitForSeconds(5f);
            Object.Destroy(t);
            _noticeUp = false;
        }
    }
}