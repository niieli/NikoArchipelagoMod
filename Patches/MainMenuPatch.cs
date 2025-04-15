using HarmonyLib;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class MainMenuPatch
{
    [HarmonyPatch(typeof(MainMenu), "OnOpen")]
    public static class MainMenuOpenPatch
    {
        private static void Postfix(MainMenu __instance)
        {
            if (GameObjectChecker.APMenu == null) return;
            GameObjectChecker.APMenu.transform.SetParent(__instance.transform, false);
            GameObjectChecker.APMenu.transform.position = new Vector3((float)970.1976, (float)574.5077, 0);
            GameObjectChecker.APMenu.transform.localPosition = new Vector3((float)10.1976, (float)-61.4923, 0);
            GameObjectChecker.APMenu.SetActive(true);
            if (Plugin.APUpdateNotice == null) return;
            Plugin.APUpdateNotice.transform.SetParent(__instance.transform, false);
            Plugin.APUpdateNotice.transform.localPosition = new Vector3(0, -60, 0);
            Plugin.APUpdateNotice.SetActive(true);


        }
    }
    [HarmonyPatch(typeof(MainMenu), "OnClose")]
    public static class MainMenuClosePatch
    {
        private static void Postfix(MainMenu __instance)
        {
            if (GameObjectChecker.APMenu == null) return;
            GameObjectChecker.APMenu.transform.SetParent(__instance.transform, false);
            GameObjectChecker.APMenu.transform.position = new Vector3((float)970.1976, (float)574.5077, 0);
            GameObjectChecker.APMenu.transform.localPosition = new Vector3((float)10.1976, (float)-61.4923, 0);
            GameObjectChecker.APMenu.SetActive(false);
            if (Plugin.APUpdateNotice == null) return;
            Plugin.APUpdateNotice.transform.SetParent(__instance.transform, false);
            Plugin.APUpdateNotice.transform.localPosition = new Vector3(0, -60, 0);
            Plugin.APUpdateNotice.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            GameObjectChecker.cursor.Visible = false;
        }
    }
}