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
            if (!Plugin.newFile && SceneManager.GetActiveScene().name == "Home")
            {
                __instance.level = 0;
            }

            if (!GameObjectChecker.ChatsanityOn || ArchipelagoClient.TicketParty) return true;
            if (!Plugin.newFile && SceneManager.GetActiveScene().name == "Home" && !_teleported)
            {
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
    }
}