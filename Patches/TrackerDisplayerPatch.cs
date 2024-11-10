using HarmonyLib;
using KinematicCharacterController.Core;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class TrackerDisplayerPatch
{
    public static CanvasGroup Ticket;
    public static CanvasGroup Kiosk;
    public static scrUIhider TicketUI;
    public static scrUIhider KioskUI;
    [HarmonyPatch(typeof(scrDisplayerSwitch), "SetDisplayerVisiability")]
    public static class PatchDisplayerSwitch
    {
        [HarmonyPostfix]
        static void Postfix(bool visable)
        {
            // if (Ticket == null || Kiosk == null) return;
            // if (ArchipelagoMenu.Ticket)
            // {
            //     Ticket.alpha = visable ? 1 : 0;
            // }
            // if (ArchipelagoMenu.Kiosk)
            // {
            //     Kiosk.alpha = visable ? 1 : 0;
            // }
            if (TicketUI == null || KioskUI == null) return;
            if (ArchipelagoMenu.Ticket)
            {
                TicketUI.visible = visable;
            }
            if (ArchipelagoMenu.Kiosk)
            {
                KioskUI.visible = visable;
            }
        }
    }
}