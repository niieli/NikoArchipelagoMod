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
            if (ArchipelagoMenu.Ticket)
            {
                Ticket.alpha = visable ? 1 : 0;
            }
            if (ArchipelagoMenu.Kiosk)
            {
                Kiosk.alpha = visable ? 1 : 0;
            }
        }
    }
}