using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
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
            if (TicketUI == null || KioskUI == null) return;
            if (ArchipelagoMenu.Ticket)
            {
                TicketUI.visible = visable;
            }
            if (ArchipelagoMenu.Kiosk)
            {
                KioskUI.visible = visable;
            }

            if (!ArchipelagoData.slotData.ContainsKey("key_level")) return;
            if (int.Parse(ArchipelagoData.slotData["key_level"].ToString()) != 1) return;
            KeyUI.visible = visable;
        }
    }
}