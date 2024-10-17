using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NikoArchipelago.Patches;

public class APMainMenu
{
    public static GameObject TitleScreen;

    public static void TitleScreenAPLogo()
    {
        if (TitleScreen != null)
        {
            var ApLogo = new GameObject("APLogo");
            ApLogo.AddComponent<Image>().sprite = Plugin.APIconSprite;
            ApLogo.layer = LayerMask.NameToLayer("UI");
            ApLogo.transform.SetParent(TitleScreen.transform);
            ApLogo.transform.position = new Vector3((float)1354.03, (float)137.3608, (float)2.12);
            ApLogo.transform.localPosition = new Vector3((float)1433.787, (float)-2.9115, (float)2.12);
            ApLogo.transform.localScale = new Vector3((float)3.2573, (float)3.0645, (float)7.8854);
        }
    }
}