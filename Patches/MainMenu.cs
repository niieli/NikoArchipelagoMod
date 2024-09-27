using HarmonyLib;
using UnityEngine;

namespace NikoArchipelago.Patches;

[HarmonyPatch(typeof(MainMenu), "OnOpen")]
public static class MainMenuPatch
{
    static void Postfix(MainMenu __instance)
    {
        
    }
}