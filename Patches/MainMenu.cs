using HarmonyLib;

namespace NikoArchipelago.Patches;

public class MainMenu
{
    // [HarmonyPostfix, HarmonyPatch(typeof(global::MainMenu))]
    // public static void MainMenu_Postfix()
    // {
    //     var quitButton = global::MainMenu.Instance.ExitButton;
    //     if (!_dc)
    //     {
    //         quitButton.onClick.RemoveListener(global::MainMenu.Instance.OnExitButtonPressed);
    //     }
    //     else
    //     {
    //         quitButton.onClick.AddListener(global::MainMenu.Instance.OnExitButtonPressed);
    //     }
    //     if (quitButton && !_dc)
    //     {
    //         quitButton.onClick.AddListener(OnQuitButtonPressed);
    //     }
    // }
}