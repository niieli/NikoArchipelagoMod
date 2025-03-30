using HarmonyLib;
using KinematicCharacterController.Core;

namespace NikoArchipelago.Patches;

public class LetterSkipPatch
{
    [HarmonyPatch(typeof(scrEnvelope), "Update")]
    public static class LetterDontOpen
    {
        private static void Postfix(scrEnvelope __instance)
        {
            if (__instance.trigger.foundPlayer())
            {
                scrLetterDisplayer.instance.showLetters = false;
                MyCharacterController.instance.blockMovementInput = false;
            }
        }
    }
}