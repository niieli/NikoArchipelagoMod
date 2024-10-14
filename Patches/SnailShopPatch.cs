using System.Collections.Generic;
using HarmonyLib;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class SnailShopPatch
{
    [HarmonyPatch(typeof(scrSnail), "Update")]
    public static class ShopPatch
    {
        private static List<Sprite> _oldClothesSprites = scrSnail.instance.clothes;
        static void Postfix(scrSnail __instance)
        {
            if (ArchipelagoData.slotData == null) return;
            if (int.Parse(ArchipelagoData.slotData["snailshop"].ToString()) == 0) return;
            var shopIsOpenField = AccessTools.Field(typeof(scrSnail), "shopIsOpen");
            bool shopIsOpen = (bool)shopIsOpenField.GetValue(__instance);
            if (__instance.state == scrSnail.States.shop)
            {
                __instance.clothes[0] = Plugin.BowtieSprite;
                __instance.clothes[1] = Plugin.MotorSprite;
                __instance.clothes[2] = Plugin.SunglassesSprite;
                __instance.clothes[3] = Plugin.MahjongSprite;
                __instance.clothes[4] = Plugin.CapSprite;
                __instance.clothes[5] = Plugin.KingSprite;
                __instance.clothes[6] = Plugin.MouseSprite;
                __instance.clothes[7] = Plugin.ClownSprite;
                __instance.clothes[8] = Plugin.CatSprite;
                __instance.clothes[9] = Plugin.BandanaSprite;
                __instance.clothes[10] = Plugin.StarsSprite;
                __instance.clothes[11] = Plugin.SwordSprite;
                __instance.clothes[12] = Plugin.TopHatSprite;
                __instance.clothes[13] = Plugin.GlassesSprite;
                __instance.clothes[14] = Plugin.FlowerSprite;
                __instance.clothes[15] = Plugin.SmallHatSprite;
            }
            else if (__instance.state != scrSnail.States.shop)
            {
                __instance.clothes = _oldClothesSprites;
            }
        }
    }
}