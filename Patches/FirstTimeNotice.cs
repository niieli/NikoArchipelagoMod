using TMPro;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class FirstTimeNotice
{
    public static GameObject TitleScreen;
    public static TextMeshProUGUI TextReference;
    public static void TitleScreenAPLogo()
    {
        if (TitleScreen == null) return;
        var notice = new GameObject("Notice");
        var text = notice.AddComponent<TextMeshProUGUI>();
        var wobble = notice.AddComponent<scrUIwobble>();
        wobble.wobbleSpeed = 6f;
        wobble.wobbleAngle = 3.5f;
        text.text = !Plugin.loggedIn ? "Open settings for Archipelago Setup!" : "Open settings and see your stats!";
        text.color = Color.red;
        text.fontSize = 20f;
        text.fontStyle = FontStyles.Bold;
        text.font = TextReference.font;
        notice.layer = LayerMask.NameToLayer("UI");
        notice.transform.SetParent(TitleScreen.transform);
        notice.transform.position = new Vector3((float)1411.318, (float)126.5312, (float)2.12);
        notice.transform.localPosition = new Vector3((float)1505.397, (float)-16.4485, (float)2.12);
        notice.transform.localScale = new Vector3((float)3.2573, (float)3.0645, (float)7.8854);
    }
}