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
        var infoGameObject = Plugin.ChristmasEvent ? Plugin.AssetBundleXmas.LoadAsset<GameObject>("FirstTimeInfoXmasTheme") : Plugin.AssetBundle.LoadAsset<GameObject>("FirstTimeInfo");
        var notice = Object.Instantiate(infoGameObject, GameObject.Find("UI").transform, false);
        var wobble = notice.AddComponent<scrUIwobble>();
        if (!Plugin.loggedIn)
        {
            notice.transform.Find("Boot").gameObject.SetActive(true);
            notice.transform.Find("LoggedIn").gameObject.SetActive(false);
        }
        else
        {
            notice.transform.Find("Boot").gameObject.SetActive(false);
            notice.transform.Find("LoggedIn").gameObject.SetActive(true);
        }
        wobble.wobbleSpeed = 1.25f;
        wobble.wobbleAngle = 2f;
        notice.layer = LayerMask.NameToLayer("UI");
        notice.transform.SetParent(TitleScreen.transform);
    }
}