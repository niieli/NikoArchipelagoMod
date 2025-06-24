using System.Collections;
using HarmonyLib;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class PepperInterviewPatch
{
    [HarmonyPatch(typeof(scrPepperInterview), "Start")]
    public static class PatchPepperInterviewStart
    {
        static void Postfix(scrPepperInterview __instance)
        {
            if (GameObjectChecker.ChatsanityOn && ArchipelagoClient.TicketParty)
            {
                __instance.StartCoroutine(PartyTicket(__instance));
            }
            if (SceneManager.GetActiveScene().name == "Home" && scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains(__instance.flag))
            {
                __instance.interview.SetActive(true); 
                __instance.postInterview.transform.Find("Geusts/Kiosk")?.gameObject.SetActive(false);
            }
            __instance.StartCoroutine(CheckElevator(__instance));
        }

        private static IEnumerator CheckElevator(scrPepperInterview instance)
        {
            if (SceneManager.GetActiveScene().name != "Tadpole inc") yield break;
            instance.postInterview.SetActive(true);
            instance.postInterview.transform.Find("Pepper/TextboxTrigger")?.gameObject.SetActive(false);
            instance.interview.SetActive(false);
            yield return new WaitUntil(() => GameObject.Find("Working") || Plugin.NoAntiCheese);
            instance.interview.SetActive(true);
            Plugin.BepinLogger.LogInfo("Elevator has been repaired! Goal is now accessible");
        }

        private static IEnumerator PartyTicket(scrPepperInterview instance)
        {
            // Party GameObject
            instance.postInterview.SetActive(true);
            instance.postInterview.transform.Find("Pepper Greeting Trigger").gameObject.SetActive(false);
            instance.postInterview.transform.Find("Everyone").gameObject.SetActive(false);
            instance.postInterview.transform.Find("Everyone Trigger").gameObject.SetActive(false);
            instance.postInterview.transform.Find("Everyone cam").gameObject.SetActive(false);
            instance.postInterview.transform.Find("Credits").gameObject.SetActive(false);
            instance.postInterview.transform.Find("Radio").gameObject.SetActive(false);
            instance.postInterview.transform.Find("Ending Screen").gameObject.SetActive(false);
            instance.postInterview.transform.Find("Geusts/Kiosk").gameObject.SetActive(false);
            
            yield return new WaitUntil(() => scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains(instance.flag));
            instance.postInterview.transform.Find("Pepper Greeting Trigger").gameObject.SetActive(true);
            instance.postInterview.transform.Find("Everyone").gameObject.SetActive(true);
            instance.postInterview.transform.Find("Everyone Trigger").gameObject.SetActive(true);
            instance.postInterview.transform.Find("Everyone cam").gameObject.SetActive(true);
            instance.postInterview.transform.Find("Credits").gameObject.SetActive(true);
            instance.postInterview.transform.Find("Radio").gameObject.SetActive(true);
            instance.postInterview.transform.Find("Ending Screen").gameObject.SetActive(true);
            if (instance.transform.Find("First Time") != null)
            {
                Object.Destroy(instance.transform.Find("First Time").gameObject);
            }
        }
    }
}