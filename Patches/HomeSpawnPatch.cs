using System.Collections;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class HomeSpawnPatch
{
    private static bool _teleported;
    [HarmonyPatch(typeof(scrCheckForLevelUnlock), "Update")]
    public static class HomeLevelUnlockPatch
    {
        private static bool Prefix(scrCheckForLevelUnlock __instance)
        {
            if (scrTrainManager.instance != null && scrTrainManager.instance.isLoadingNewScene)
                _teleported = false;
            if (SceneManager.GetActiveScene().name != "Home") return true;
            __instance.transform.Find("Masa Trigger").gameObject.SetActive(false);
            if (!Plugin.newFile)
            {
                __instance.level = 0;
            }
            if (GameObjectChecker.ChatsanityOn && ArchipelagoClient.TicketParty)
            {
                PartyTicket(__instance);
            }
            if (!Plugin.newFile && !_teleported)
            {
                __instance.transform.Find("Move Controls").gameObject.SetActive(false);
                __instance.transform.Find("Move Text").gameObject.SetActive(false);
                __instance.StartCoroutine(WaitForPlayer(__instance));
                _teleported = true;
            }
            return false;
        }

        private static IEnumerator WaitForPlayer(scrCheckForLevelUnlock __instance)
        {
            yield return new WaitUntil(() => MyCharacterController.instance != null && !scrLevelIntroduction.isOn);
            MyCharacterController.instance.requestNewPosition(new Vector3(-6.7028f, 0.0997f, -5.3014f));
            if (MyCharacterController.position == new Vector3(-6.7028f, 0.0997f, -5.3014f))
            {
                __instance.enabled = false;
            }
        }

        private static void PartyTicket(scrCheckForLevelUnlock __instance)
        {
            // First Time GameObject
            __instance.transform.Find("Pepper Meeting").gameObject.SetActive(false);
            __instance.transform.Find("Dispatcher Cam").gameObject.SetActive(false);
            __instance.transform.Find("CameraPassiveArea").gameObject.SetActive(false);
            __instance.transform.Find("Move Controls").gameObject.SetActive(false);
            __instance.transform.Find("Move Text").gameObject.SetActive(false);
            __instance.transform.Find("Masa Trigger").gameObject.SetActive(false);
            __instance.transform.Find("Mata Meeting").gameObject.SetActive(false);
            __instance.transform.Find("PepperCam").gameObject.SetActive(false);
            if (scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("pepperInterview"))
            {
                __instance.transform.gameObject.SetActive(false);
            }
        }
    }
}