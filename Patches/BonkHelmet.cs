﻿using System.Collections;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class BonkHelmet
{
    private static bool _playedNoBonk;
    public static bool NoticeUp;

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
            if (NoticeUp || !SavedData.Instance.Notices) yield break;
            var t = Object.Instantiate(Plugin.NoticeBonkHelmet, Plugin.NotifcationCanvas.transform);
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

    [HarmonyPatch(typeof(scrTreeQuest), "Update")]
    public static class TreemanPatch
    {
        private static void Postfix(scrTreeQuest __instance)
        {
            if (!ArchipelagoData.slotData.ContainsKey("bonk_permit")) return;
            if (int.Parse(ArchipelagoData.slotData["bonk_permit"].ToString()) != 1) return;

            if (ArchipelagoClient.BonkPermitAcquired)
            {
                __instance.trigger.gameObject.SetActive(true);
                return;
            }
            __instance.trigger.gameObject.SetActive(false);
        }
    }
}