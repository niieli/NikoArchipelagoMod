using HarmonyLib;
using KinematicCharacterController.Core;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class TrackerDisplayerPatch
{
    public static CanvasGroup Ticket;
    public static CanvasGroup Kiosk;
    [HarmonyPatch(typeof(scrDisplayerSwitch), "SetDisplayerVisiability")]
    public static class PatchDisplayerSwitch
    {
        [HarmonyPostfix]
        static void Postfix(bool visable)
        {
            Ticket.alpha = visable ? 1 : 0;
            Kiosk.alpha = visable ? 1 : 0;
        }
    }
}