using HarmonyLib;
using KinematicCharacterController.Core;
using UnityEngine;

namespace NikoArchipelago.Patches
{
    [HarmonyPatch(typeof(MyCharacterController), "UpdateVelocity")]
    public class MyCharacterControllerUpdateVelocityPatch
    {
        static void Postfix(ref Vector3 currentVelocity, float deltaTime, MyCharacterController __instance)
        {
            // Prüfen, ob "Kickable" in der Nähe ist
            if (IsTouchingKickable(__instance))
            {
                //Plugin.BepinLogger.LogInfo("Kickable GameObject berührt!");
                __instance.Motor.SetPositionAndRotation(
                    new Vector3(
                        __instance.Motor.InterpolatedPosition.x, 
                        Mathf.Max(__instance.Motor.InterpolatedPosition.y, __instance.levelBottom.position.y),
                        __instance.Motor.InterpolatedPosition.z),
                    __instance.Motor.GetState().Rotation, false);
            }
        }

        // Methode, um zu prüfen, ob sich "Kickable"-Objekte in Berührung befinden
        private static bool IsTouchingKickable(MyCharacterController characterController)
        {
            Collider[] hitColliders = Physics.OverlapSphere(characterController.transform.position, 1.0f); // Radius = 1.0f
            foreach (var hit in hitColliders)
            {
                if (hit.CompareTag("Kickable"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}