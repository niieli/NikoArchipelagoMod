using System.Collections;
using System.Reflection;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class FetchQuestPatch
{
    [HarmonyPatch(typeof(scrFetchQuest), "Start")]
    public static class MoveQuestPatch
    {
        private static void Postfix(scrFetchQuest __instance)
        {
            __instance.StartCoroutine(MoveWhenParty(__instance));
        }

        private static IEnumerator MoveWhenParty(scrFetchQuest __instance)
        {
            yield return new WaitUntil(() => ArchipelagoClient.TicketParty);
            var pos = new Vector3(-134.78f, 25f, 69.15f);
            Plugin.BepinLogger.LogInfo("Moved Fetch NPC");
            if (__instance.NPCsQuest.transform.Find("NPC Quest") != null)
                __instance.NPCsQuest.transform.Find("NPC Quest").gameObject.transform.localPosition =
                    new Vector3(-134.78f, 25f, 69.15f);
            if (__instance.NPCsQuest.transform.Find("NPC Quest").gameObject.GetComponent<scrHopOnBump>() != null)
                AccessTools.Field(typeof(scrHopOnBump), "homePos")
                    .SetValue(__instance.NPCsQuest.transform.Find("NPC Quest").gameObject.GetComponent<scrHopOnBump>(), pos);
            if (__instance.NPCsReward.transform.Find("NPCReward") != null)
                __instance.NPCsReward.transform.Find("NPCReward").gameObject.transform.localPosition =
                    new Vector3(-134.78f, 25f, 69.15f);
            if (__instance.NPCsReward.transform.Find("NPCReward").gameObject.GetComponent<scrHopOnBump>() != null)
                AccessTools.Field(typeof(scrHopOnBump), "homePos")
                    .SetValue(__instance.NPCsReward.transform.Find("NPCReward").gameObject.GetComponent<scrHopOnBump>(), pos);
            if (__instance.NPCsReward.transform.Find("Item When back") != null)
                __instance.NPCsReward.transform.Find("Item When back").gameObject.transform.localPosition =
                    new Vector3(-134.78f, 25f, 68.5863f);
            if (__instance.Trigger != null)
                __instance.Trigger.gameObject.transform.localPosition = new Vector3(-134.78f, 25f, 69.15f);
        }
    }
}