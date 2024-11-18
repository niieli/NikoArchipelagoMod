using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NikoArchipelago.Patches;

public class SmallAPLogo
{
    public static GameObject TitleScreen;

    public static void TitleScreenAPLogo()
    {
        if (TitleScreen == null) return;
        var apLogo = new GameObject("APLogo");
        apLogo.AddComponent<Image>().sprite = Plugin.APIconSprite;
        apLogo.layer = LayerMask.NameToLayer("UI");
        apLogo.transform.SetParent(TitleScreen.transform);
        apLogo.transform.position = new Vector3((float)1296.83, (float)145.2443, 0);
        apLogo.transform.localPosition = new Vector3((float)1362.288, (float)6.9429, 0);
        apLogo.transform.localScale = new Vector3((float)2.1355, (float)2.1736, 0);
    }
}