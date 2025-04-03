using System.Collections;
using HarmonyLib;
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
            if (SceneManager.GetActiveScene().name == "Home" && scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains(__instance.flag))
            {
                // Make Fetch Quest & Kiosk Check Unmissable
                __instance.interview.SetActive(true); 
                var questFrog = __instance.interview.transform.Find("FetchQuest/NPCs Quest/NPC Quest");
                questFrog.transform.position = new Vector3((float)-138.1868, (float)25.2446, (float)45.4684);
                questFrog.transform.localPosition = new Vector3((float)-131.9129, (float)25.032, (float)69.1832);
                var rewardFrog = __instance.interview.transform.Find("FetchQuest/NPCs Reward/NPCReward")?.gameObject;
                if (rewardFrog != null)
                {
                    rewardFrog.transform.position = new Vector3((float)-138.1868, (float)25.2446, (float)45.4684);
                    rewardFrog.transform.localPosition = new Vector3((float)-131.9129, (float)25.032, (float)69.1832);
                }
                var trigger = __instance.interview.transform.Find("FetchQuest/Trigger")?.gameObject;
                if (trigger != null)
                {
                    trigger.transform.position = new Vector3((float)-137.3358, (float)25.3026, (float)45.2441);
                    trigger.transform.localPosition = new Vector3((float)-131.1818, (float)25.09, (float)68.9589);
                }
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
    }
}