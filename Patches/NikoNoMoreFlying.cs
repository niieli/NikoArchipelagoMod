using System;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using KinematicCharacterController;
using KinematicCharacterController.Core;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class NikoNoMoreFlying
{
    [HarmonyPatch(typeof(MyCharacterController), "UpdateVelocity")]
    public class MyCharacterControllerUpdateVelocityPatch
    {
        [HarmonyPrefix]
        static bool Postfix(MyCharacterController __instance)
        {
            __instance.Motor.SetPositionAndRotation(new Vector3(__instance.Motor.InterpolatedPosition.x, Mathf.Max(__instance.Motor.InterpolatedPosition.y, __instance.levelBottom.position.y), __instance.Motor.InterpolatedPosition.z), __instance.Motor.GetState().Rotation, false);
            if ((double) __instance.Motor.InterpolatedPosition.y <= (double) __instance.levelBottom.position.y)
                return false;
            return true;
        }
    }
}