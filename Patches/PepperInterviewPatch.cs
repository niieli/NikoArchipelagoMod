using System.Collections;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class PepperInterviewPatch
{
    [HarmonyPatch(typeof(scrPepperInterview), "Start")]
    public static class PatchPepperAdviceUpdate
    {
        static void Postfix(scrPepperInterview __instance)
        {
            __instance.StartCoroutine(CheckElevator(__instance));
        }

        private static IEnumerator CheckElevator(scrPepperInterview instance)
        {
            if (SceneManager.GetActiveScene().name != "Tadpole inc") yield break;
            instance.postInterview.SetActive(true);
            instance.postInterview.transform.Find("Pepper/TextboxTrigger")?.gameObject.SetActive(false);
            instance.interview.SetActive(false);
            yield return new WaitUntil(() => GameObject.Find("Working"));
            instance.interview.SetActive(true);
            Plugin.BepinLogger.LogInfo("Elevator has been repaired! Goal is now accessible");
        }
    }
}