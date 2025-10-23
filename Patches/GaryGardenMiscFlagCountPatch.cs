using System.Collections;
using System.Linq;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NikoArchipelago.Patches;

public class GaryGardenMiscFlagCountPatch
{
    [HarmonyPatch(typeof(scrMiscFlagCountSwitch), "Update")]
    public static class MiscFlagCountSwitchUpdatePatch
    {
        [HarmonyPrefix]
        private static bool Prefix(scrMiscFlagCountSwitch __instance)
        {
            int count = scrWorldSaveDataContainer.instance.miscFlags.Count(x => x.StartsWith("GardenSeed"));
            if (ArchipelagoData.slotData != null)
                if (ArchipelagoData.Options.GoalCompletion == ArchipelagoOptions.GoalCompletionMode.Garden)
                {
                    count = ItemHandler.GarysGardenSeedAmount;
                }
            foreach (GameObject gameObject in __instance.objectsToSwitch)
            {
                if (__instance.objectsToSwitch[count] != gameObject)
                    gameObject.SetActive(false);
            }
            if (__instance.objectsToSwitch[count] != null)
                __instance.objectsToSwitch[count].SetActive(true);
            if (count < __instance.objectsToSwitch.Count - 1)
                return false;
            scrScissor.destroyAll = true;
            MyCharacterController.instance.currentDaisy = __instance.endDaisy;
            scrWaterIsDeath.isOn = false;
            scrDaisy.alwaysOpen = true;
            return false;
        }
    }
    
    [HarmonyPatch(typeof(scrMiscFlagCountSwitch), "Rise")]
    public static class MiscFlagCountSwitchRisePatch
    {
        [HarmonyPrefix]
        private static bool Prefix(scrMiscFlagCountSwitch __instance)
        {
            __instance.StartCoroutine(AnimateRise(__instance));
            return false;
        }
    }
    
    [HarmonyPatch(typeof(scrMiscFlagCountSwitch), "RemoveIfObtained")]
    public static class MiscFlagCountSwitchRemovePatch
    {
        [HarmonyPrefix]
        private static bool Prefix(scrMiscFlagCountSwitch __instance)
        {
            __instance.StartCoroutine(RemoveIfObtained(__instance));
            return false;
        }
    }

    private static IEnumerator AnimateRise(scrMiscFlagCountSwitch instance)
    {
        int flagCount = scrWorldSaveDataContainer.instance.miscFlags.Count(x => x.StartsWith("GardenSeed")) + 1;
        if (ArchipelagoData.slotData != null)
            if (ArchipelagoData.Options.GoalCompletion == ArchipelagoOptions.GoalCompletionMode.Garden)
            {
                flagCount = ItemHandler.GarysGardenSeedAmount + 1;
            }
        float myTimer = 0.0f;
        if (flagCount < 0 || flagCount >= instance.objectsToRise.Count)
            yield break;
        if (instance.objectsToRise[flagCount] != null)
        {
            GameObject gameObject = instance.objectsToRise[flagCount];
            while (myTimer <= 1.0)
            {
                myTimer += Time.deltaTime / 2.5f;
                if (instance.objectsToRise[flagCount] != null)
                    instance.objectsToRise[flagCount].transform.position = Vector3.Lerp(new Vector3(0.0f, -70f, 0.0f),
                        new Vector3(0.0f, 0.0f, 0.0f), myTimer);
                yield return null;
            }
        }
    }
    private static IEnumerator RemoveIfObtained(scrMiscFlagCountSwitch instance)
    {
        while (!scrWorldSaveDataContainer.instance.worldLoaded)
            yield return null;
        int count = scrWorldSaveDataContainer.instance.miscFlags.Count(x => x.StartsWith("GardenSeed"));
        if (ArchipelagoData.slotData != null)
            if (ArchipelagoData.Options.GoalCompletion == ArchipelagoOptions.GoalCompletionMode.Garden)
            {
                count = ItemHandler.GarysGardenSeedAmount;
            }
        for (int index = 0; index <= count + 1; ++index)
        {
            if (index >= instance.objectsToRise.Count)
                yield break;
            if (instance.objectsToRise[index] != null)
                instance.objectsToRise[index].transform.position = new Vector3(instance.objectsToRise[index].transform.position.x, 0.0f, instance.objectsToRise[index].transform.position.z);
        }
    }
}