using HarmonyLib;

namespace NikoArchipelago.Patches;

public class SnailShopPatch
{
    [HarmonyPatch(typeof(scrSnail), "Shop")]
    public static class ShopPatch
    {
        static void Postfix(scrSnail __instance)
        {
            //__instance.shopImage.sprite = Plugin.APSprite;
        }
    }
}