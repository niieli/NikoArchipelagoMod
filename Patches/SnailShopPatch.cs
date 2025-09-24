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
    private static List<int> _validClothingNumbers1 = [3, 12, 13, 14, 15], 
        _validClothingNumbers2 = [1, 3, 4, 7, 9, 10, 12, 13, 14, 15], 
        _validClothingNumbers3 = [1, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15], 
        _validClothingNumbers4 = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
    
    [HarmonyPatch(typeof(scrSnail), "Update")]
    public static class ShopPatch
    {
        private static List<Sprite> _oldClothesSprites = scrSnail.instance.clothes;
        private static List<int> _oldUnboughtClothes = scrSnail.instance.unboughtClothes;
        private static bool _change;
        private static int oldItemForSale = 50;
        
        static void Postfix(scrSnail __instance)
        {
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.slotData.ContainsKey("snailshop")) return;
            if (!ArchipelagoData.Options.Snailshop) return;
            var shopIsOpenField = AccessTools.Field(typeof(scrSnail), "shopIsOpen");
            bool shopIsOpen = (bool)shopIsOpenField.GetValue(__instance);
            var clothingForSaleField = AccessTools.Field(typeof(scrSnail), "clothingForSale");
            int clothingForSale = (int)clothingForSaleField.GetValue(__instance);
            //TODO: Toggle
            scrGameSaveManager.instance.gameData.generalGameData.snailFood = true;
            __instance.imgFood.sprite = __instance.sprFoodFull;
            scrGameSaveManager.instance.gameData.generalGameData.snailPoopCount = 0;
            scrGameSaveManager.instance.gameData.generalGameData.snailHappyPoints = 99;
            __instance.sleepTimer = 0;
            if (__instance.mood == scrSnail.Moods.happy)
            {
                List<int> validClothingNumbers = null;
                if (ArchipelagoClient.TicketCount() <= 1 && _validClothingNumbers1.Count > 0)
                {
                    validClothingNumbers = _validClothingNumbers1;
                    //Plugin.BepinLogger.LogFatal("1. Condition");
                }
                else if ((ArchipelagoClient.TicketCount() == 2 && _validClothingNumbers2.Count > 0)
                         || _validClothingNumbers1.Count == 0 && _validClothingNumbers2.Any())
                {
                    validClothingNumbers = _validClothingNumbers2;
                    //Plugin.BepinLogger.LogFatal("2. Condition");
                }
                else if ((ArchipelagoClient.TicketCount() == 3 && _validClothingNumbers3.Count > 0) 
                         || _validClothingNumbers2.Count == 0 && _validClothingNumbers1.Count == 0 && _validClothingNumbers3.Any())
                {
                    validClothingNumbers = _validClothingNumbers3;
                    //Plugin.BepinLogger.LogFatal("3. Condition");
                } 
                else if ((ArchipelagoClient.TicketCount() >= 4 && _validClothingNumbers4.Count > 0) 
                         || _validClothingNumbers3.Count == 0 && _validClothingNumbers2.Count == 0 && _validClothingNumbers1.Count == 0)     
                {
                    validClothingNumbers = _validClothingNumbers4;
                    //Plugin.BepinLogger.LogFatal("4. Condition");
                }
                
                _validClothingNumbers1.RemoveAll(clothing => __instance.boughtClothes.Contains(clothing));
                _validClothingNumbers2.RemoveAll(clothing => __instance.boughtClothes.Contains(clothing));
                _validClothingNumbers3.RemoveAll(clothing => __instance.boughtClothes.Contains(clothing));
                _validClothingNumbers4.RemoveAll(clothing => __instance.boughtClothes.Contains(clothing));
                if (validClothingNumbers != null)
                {
                    if (oldItemForSale == 50)
                    {
                        oldItemForSale = clothingForSale;
                    }
                    validClothingNumbers.RemoveAll(clothing => __instance.boughtClothes.Contains(clothing));
                    if (shopIsOpen && __instance.state is scrSnail.States.moveLeft or scrSnail.States.moveRight)
                        _change = true;
                    if (validClothingNumbers.Count > 0 && !validClothingNumbers.Contains(clothingForSale))
                    {
                        var newItemForSale = validClothingNumbers[Random.Range(0, validClothingNumbers.Count)];
                        clothingForSaleField.SetValue(__instance, newItemForSale);
                        __instance.shopImage.sprite = __instance.clothes[newItemForSale];
                        Plugin.BepinLogger.LogInfo($"Available clothes count is {validClothingNumbers.Count}");
                        foreach (var clothing in validClothingNumbers)
                            Plugin.BepinLogger.LogInfo($"Available clothes for sale are: {clothing}");
                    } else if (validClothingNumbers.Count > 1 && validClothingNumbers.Contains(clothingForSale) && _change 
                               && oldItemForSale == clothingForSale)
                    {
                        var newItemForSale = 0;
                        do
                        {
                            newItemForSale = validClothingNumbers[Random.Range(0, validClothingNumbers.Count)];
                            Plugin.BepinLogger.LogInfo($"Item for sale: {newItemForSale} | old sale: {clothingForSale}");
                        } while (oldItemForSale == newItemForSale);

                        clothingForSaleField.SetValue(__instance, newItemForSale);
                        __instance.shopImage.sprite = __instance.clothes[newItemForSale];
                        //Plugin.BepinLogger.LogInfo($"Boolean _change: {_change}");
                        foreach (var clothing in validClothingNumbers)
                            Plugin.BepinLogger.LogInfo($"Available clothes for sale are: {clothing}");
                    }
                    else if(__instance.textMesh.text.Contains("GAME") || __instance.textMesh.text.Contains("WARDROBE"))
                    {
                        oldItemForSale = 50;
                        _change = false;
                    }
                }
            }
            if (__instance.state == scrSnail.States.shop)
            {
                __instance.clothes[0] = Assets.BowtieSprite;
                __instance.clothes[1] = Assets.MotorSprite;
                __instance.clothes[2] = Assets.SunglassesSprite;
                __instance.clothes[3] = Assets.MahjongSprite;
                __instance.clothes[4] = Assets.CapSprite;
                __instance.clothes[5] = Assets.KingSprite;
                __instance.clothes[6] = Assets.MouseSprite;
                __instance.clothes[7] = Assets.ClownSprite;
                __instance.clothes[8] = Assets.CatSprite;
                __instance.clothes[9] = Assets.BandanaSprite;
                __instance.clothes[10] = Assets.StarsSprite;
                __instance.clothes[11] = Assets.SwordSprite;
                __instance.clothes[12] = Assets.TopHatSprite;
                __instance.clothes[13] = Assets.GlassesSprite;
                __instance.clothes[14] = Assets.FlowerSprite;
                __instance.clothes[15] = Assets.SmallHatSprite;
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