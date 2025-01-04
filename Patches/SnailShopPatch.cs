using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using NikoArchipelago.Archipelago;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NikoArchipelago.Patches;

public class SnailShopPatch
{
    [HarmonyPatch(typeof(scrSnail), "Update")]
    public static class ShopPatch
    {
        private static List<Sprite> _oldClothesSprites = scrSnail.instance.clothes;
        private static List<int> _oldUnboughtClothes = scrSnail.instance.unboughtClothes;
        private static List<int> _validClothingNumbers1, _validClothingNumbers2, _validClothingNumbers3, _validClothingNumbers4;
        static void Postfix(scrSnail __instance)
        {
            if (ArchipelagoData.slotData == null) return;
            if (int.Parse(ArchipelagoData.slotData["snailshop"].ToString()) == 0) return;
            var shopIsOpenField = AccessTools.Field(typeof(scrSnail), "shopIsOpen");
            bool shopIsOpen = (bool)shopIsOpenField.GetValue(__instance);
            var clothingForSaleField = AccessTools.Field(typeof(scrSnail), "clothingForSale");
            int clothingForSale = (int)clothingForSaleField.GetValue(__instance);
            if (__instance.mood == scrSnail.Moods.happy || __instance.state == scrSnail.States.shop)
            {
                List<int> validClothingNumbers = null;

                if (ArchipelagoClient.TicketCount() <= 1 && (_validClothingNumbers1 == null || _validClothingNumbers1.Count > 0))
                {
                    _validClothingNumbers1 = [3, 12, 13, 14, 15];
                    validClothingNumbers = _validClothingNumbers1;
                }
                else if ((ArchipelagoClient.TicketCount() == 2 && (_validClothingNumbers2 == null || _validClothingNumbers2.Count > 0)) 
                         || (_validClothingNumbers1 == null || _validClothingNumbers1.Count == 0))
                {
                    _validClothingNumbers2 = [1, 3, 4, 7, 9, 10, 12, 13, 14, 15];
                    validClothingNumbers = _validClothingNumbers2;
                }
                else if ((ArchipelagoClient.TicketCount() == 3 && (_validClothingNumbers3 == null || _validClothingNumbers3.Count > 0)) 
                         || ((_validClothingNumbers2 == null || _validClothingNumbers2.Count == 0) && (_validClothingNumbers1 == null || _validClothingNumbers1.Count == 0)))
                {
                    _validClothingNumbers3 = [1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
                    validClothingNumbers = _validClothingNumbers3;
                } else if ((ArchipelagoClient.TicketCount() >= 4 && (_validClothingNumbers4 == null || _validClothingNumbers4.Count > 0)) 
                           || ((_validClothingNumbers3 == null || _validClothingNumbers3.Count == 0) 
                               && (_validClothingNumbers2 == null || _validClothingNumbers2.Count == 0) 
                               && (_validClothingNumbers1 == null || _validClothingNumbers1.Count == 0)))      
                {
                    _validClothingNumbers4 = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
                    validClothingNumbers = _validClothingNumbers4;
                }

                if (validClothingNumbers != null && !validClothingNumbers.Contains(clothingForSale))
                {
                    validClothingNumbers.RemoveAll(clothing => __instance.boughtClothes.Contains(clothing));

                    if (validClothingNumbers.Count > 0)
                    {
                        try
                        {
                            var newItemForSale = validClothingNumbers[Random.Range(0, validClothingNumbers.Count+1)];
                            clothingForSaleField.SetValue(__instance, newItemForSale);
                            __instance.shopImage.sprite = __instance.clothes[newItemForSale];
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            Plugin.BepinLogger.LogInfo("Caught Exception");
                        }
                    }
                }
            }
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
                if (shopIsOpen && ArchipelagoMenu.ShopHints)
                {
                    for (var j = 0; j < __instance.clothes.Count; j++)
                    {
                        if (scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains(
                                "Hint" + (20 + j))) continue;
                        ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[20 + j]);
                        scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Add("Hint" + (20 + j));
                    }
                    scrGameSaveManager.instance.SaveGame();
                }
                else
                {
                    __instance.shopImage.sprite = __instance.aniInvisible[0];
                }
            }
            else if (__instance.state != scrSnail.States.shop)
            {
                __instance.clothes = _oldClothesSprites;
            }
        }
    }
}