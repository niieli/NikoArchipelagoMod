using System.Numerics;
using HarmonyLib;
using KinematicCharacterController.Core;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class WaypointSystemPatch
{
    [HarmonyPatch(typeof(scrWaypointSystem), "Update")]
    public static class WaypointSystemUpdatePatch
    {
        private static void Postfix(scrWaypointSystem __instance)
        {
            if (MyCharacterController.instance == null) return;
            var distance = Traverse.Create(__instance).Field("distance").GetValue<float>();
            var realDistance = Mathf.Sqrt(distance);
            __instance.distanceText.text = $"{realDistance:F1}";
            __instance.distanceTextShadow.text = $"{realDistance:F1}";
        }
    }
}