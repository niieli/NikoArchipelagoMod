using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class TrackerDisplayerPatch
{
    public static scrUIhider TicketUI;
    public static scrUIhider KioskUI;
    public static scrUIhider KeyUI;
    [HarmonyPatch(typeof(scrDisplayerSwitch), "SetDisplayerVisiability")]
    public static class PatchDisplayerSwitch
    {
        [HarmonyPostfix]
        static void Postfix(bool visable)
        {
            // This should prevent the game from "permanently" disabling them 
            ShowDisplayers.AppleDisplayerUIhider.visible = visable;
            ShowDisplayers.BugDisplayerUIhider.visible = visable;
            //ShowDisplayers.CassetteDisplayerGameObject.visible = visable;
            //ShowDisplayers.CoinDisplayerGameObject.visible = visable;
            ShowDisplayers.KeyDisplayerUIhider.visible = visable;
            if (TicketUI == null || KioskUI == null) return;
            if (ArchipelagoMenu.Ticket)
            {
                TicketUI.visible = visable;
            }
            if (ArchipelagoMenu.Kiosk)
            {
                KioskUI.visible = visable;
            }
            NotificationManager.IsUiOnScreen = visable;
            TrapManager.IsUiOnScreen = visable;
        }
    }
}