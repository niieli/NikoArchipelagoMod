using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace NikoArchipelago.Patches;

[HarmonyPatch(typeof(scrKey))]
[HarmonyPatch("Update")]
public class KeyRemover
{
    // private static bool Prefix(scrKey __instance)
    // {
    //     var obtainingField = AccessTools.Field(typeof(scrKey), "obtaining");
    //     var _obtaining = (bool)obtainingField.GetValue(__instance);
    //     if (_obtaining || !__instance.trigger.foundPlayer())
    //         return false;
    //     
    //     __instance.StartCoroutine(ObtainKeyModified(__instance));
    //     return false;
    // }
    //
    // private static IEnumerator ObtainKeyModified(scrKey instance)
    // {
    //     var animationDurationField = AccessTools.Field(typeof(scrKey), "animationDuration");
    //     var _animationDuration = (float)animationDurationField.GetValue(instance);
    //     var obtainingField = AccessTools.Field(typeof(scrKey), "obtaining");
    //     var _obtaining = (bool)obtainingField.GetValue(instance);
    //     var obtainTimer = _animationDuration;
    //     instance.trigger.enabled = false;
    //     Object.Destroy(instance.quads);
    //     _obtaining = true;
    //
    //     while (obtainTimer > 0.0f)
    //     {
    //         obtainTimer -= Time.deltaTime;
    //         yield return null;
    //     }
    //     scrWorldSaveDataContainer.instance.keyAmount++;
    //     scrWorldSaveDataContainer.instance.miscFlags.Add(instance.flag);
    //     scrWorldSaveDataContainer.instance.SaveWorld();
    //     Object.Destroy(instance.gameObject);
    // }
    
}