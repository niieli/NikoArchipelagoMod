using System.Linq;
using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class APItemObtainer
{
    [HarmonyPatch(typeof(scrObtainCoin), "Start")]
    public static class ObtainCoinPatch
    {
        private static Texture TextureAndDisplayer(Texture texture, scrObtainCoin instance, string displayer = "")
        {
            var timer = 5f;
            instance.coinDisplayer.visible = false;
            switch (displayer)
            {
                case "Coin":
                    instance.coinDisplayer.visible = true;
                    break;
                case "Cassette":
                    ShowDisplayers.CassetteDisplayer(timer);
                    break;
                case "Key":
                    ShowDisplayers.KeyDisplayer(timer);
                    break;
                case "Ticket":
                    ShowDisplayers.TicketDisplayer(timer);
                    break;
                case "Apples":
                    ShowDisplayers.AppleDisplayer(timer);
                    break;
                case "Bugs":
                    ShowDisplayers.BugDisplayer(timer);
                    break;
                case "":
                    break;
            }
            return texture;
        }
        private static void Postfix(scrObtainCoin __instance)
        {
            if (ArchipelagoMenu.SkipPickup)
            {
                Skip(__instance);
                __instance.Invoke("Die", 0f);
            }
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
            {
                if (int.Parse(ArchipelagoData.slotData["shuffle_garden"].ToString()) == 0)
                {
                    gardenAdjustment = 2;
                }
            }
            if (ArchipelagoData.slotData.ContainsKey("snailshop"))
            {
                if (int.Parse(ArchipelagoData.slotData["snailshop"].ToString()) == 0)
                {
                    snailShopAdjustment = 16;
                }
            }
            var adjustment = gardenAdjustment + snailShopAdjustment;
            
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Home" or "Hairball City":
                {
                    var list = Locations.ScoutHCCoinList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    var offset = 36 - adjustment;
                    if (Locations.ScoutMiMaList.ContainsValue(__instance.myFlag))
                    { 
                        list = Locations.ScoutMiMaList.ToList();
                        index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                        offset = 0;
                    }
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.txrCoin = Plugin.TimePieceSprite.texture;
                                    break;
                                case "Yarn" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                {
                                    var yarnTextures = new[]
                                    {
                                        Plugin.YarnSprite.texture,
                                        Plugin.Yarn2Sprite.texture,
                                        Plugin.Yarn3Sprite.texture,
                                        Plugin.Yarn4Sprite.texture,
                                        Plugin.Yarn5Sprite.texture
                                    };
                                    var randomIndex = Random.Range(0, yarnTextures.Length);
                                    __instance.txrCoin = yarnTextures[randomIndex];
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.txrCoin = Plugin.ApProgressionSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.txrCoin = Plugin.ApUsefulSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                                    {
                                        var trapTextures = new[]
                                        {
                                            Plugin.ApTrapSprite.texture,
                                            Plugin.ApTrap2Sprite.texture,
                                            Plugin.ApTrap3Sprite.texture
                                        };
                                        var randomIndex = Random.Range(0, trapTextures.Length);
                                        __instance.txrCoin = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                                    {
                                        __instance.txrCoin = Plugin.ApFillerSprite.texture;
                                    }

                                    break;
                                }
                            }
                        }
                        else
                            __instance.txrCoin = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                                "Cassette" => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                                "Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                                "25 Apples" => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                                "10 Bugs" => TextureAndDisplayer(Plugin.BugSprite.texture, __instance, "Bugs"),
                                "1000 Snail Dollar" => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => TextureAndDisplayer(Plugin.ContactListSprite.texture, __instance, "Ticket"),
                                "Hairball City Ticket" => TextureAndDisplayer(Plugin.HcSprite.texture, __instance, "Ticket"),
                                "Turbine Town Ticket" => TextureAndDisplayer(Plugin.TtSprite.texture, __instance, "Ticket"),
                                "Salmon Creek Forest Ticket" => TextureAndDisplayer(Plugin.SfcSprite.texture, __instance, "Ticket"),
                                "Public Pool Ticket" => TextureAndDisplayer(Plugin.PpSprite.texture, __instance, "Ticket"),
                                "Bathhouse Ticket" => TextureAndDisplayer(Plugin.BathSprite.texture, __instance, "Ticket"),
                                "Tadpole HQ Ticket" => TextureAndDisplayer(Plugin.HqSprite.texture, __instance, "Ticket"),
                                "Gary's Garden Ticket" => TextureAndDisplayer(Plugin.GgSprite.texture, __instance, "Ticket"),
                                "Super Jump" => TextureAndDisplayer(Plugin.SuperJumpSprite.texture, __instance),
                                "Hairball City Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Turbine Town Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Salmon Creek Forest Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Public Pool Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Bathhouse Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Tadpole HQ Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Hairball City Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Turbine Town Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Salmon Creek Forest Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Public Pool Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Bathhouse Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Tadpole HQ Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                       + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                       + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
                case "Trash Kingdom":
                {
                    var list = Locations.ScoutTTCoinList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    var offset = 49 - adjustment;
                    if (Locations.ScoutMiMaList.ContainsValue(__instance.myFlag))
                    {
                        list = Locations.ScoutMiMaList.ToList();
                        index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                        offset = 2;
                    }
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.txrCoin = Plugin.TimePieceSprite.texture;
                                    break;
                                case "Yarn" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                {
                                    var yarnTextures = new[]
                                    {
                                        Plugin.YarnSprite.texture,
                                        Plugin.Yarn2Sprite.texture,
                                        Plugin.Yarn3Sprite.texture,
                                        Plugin.Yarn4Sprite.texture,
                                        Plugin.Yarn5Sprite.texture
                                    };
                                    var randomIndex = Random.Range(0, yarnTextures.Length);
                                    __instance.txrCoin = yarnTextures[randomIndex];
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.txrCoin = Plugin.ApProgressionSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.txrCoin = Plugin.ApUsefulSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                                    {
                                        var trapTextures = new[]
                                        {
                                            Plugin.ApTrapSprite.texture,
                                            Plugin.ApTrap2Sprite.texture,
                                            Plugin.ApTrap3Sprite.texture
                                        };
                                        var randomIndex = Random.Range(0, trapTextures.Length);
                                        __instance.txrCoin = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                                    {
                                        __instance.txrCoin = Plugin.ApFillerSprite.texture;
                                    }

                                    break;
                                }
                            }
                        }
                        else
                            __instance.txrCoin = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                                "Cassette" => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                                "Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                                "25 Apples" => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                                "10 Bugs" => TextureAndDisplayer(Plugin.BugSprite.texture, __instance, "Bugs"),
                                "1000 Snail Dollar" => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => TextureAndDisplayer(Plugin.ContactListSprite.texture, __instance, "Ticket"),
                                "Hairball City Ticket" => TextureAndDisplayer(Plugin.HcSprite.texture, __instance, "Ticket"),
                                "Turbine Town Ticket" => TextureAndDisplayer(Plugin.TtSprite.texture, __instance, "Ticket"),
                                "Salmon Creek Forest Ticket" => TextureAndDisplayer(Plugin.SfcSprite.texture, __instance, "Ticket"),
                                "Public Pool Ticket" => TextureAndDisplayer(Plugin.PpSprite.texture, __instance, "Ticket"),
                                "Bathhouse Ticket" => TextureAndDisplayer(Plugin.BathSprite.texture, __instance, "Ticket"),
                                "Tadpole HQ Ticket" => TextureAndDisplayer(Plugin.HqSprite.texture, __instance, "Ticket"),
                                "Gary's Garden Ticket" => TextureAndDisplayer(Plugin.GgSprite.texture, __instance, "Ticket"),
                                "Super Jump" => TextureAndDisplayer(Plugin.SuperJumpSprite.texture, __instance),
                                "Hairball City Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Turbine Town Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Salmon Creek Forest Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Public Pool Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Bathhouse Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Tadpole HQ Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Hairball City Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Turbine Town Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Salmon Creek Forest Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Public Pool Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Bathhouse Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Tadpole HQ Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                       + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                       + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSFCCoinList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    var offset = 58 - adjustment;
                    if (Locations.ScoutMiMaList.ContainsValue(__instance.myFlag))
                    {
                        list = Locations.ScoutMiMaList.ToList();
                        index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                        offset = 4;
                    }
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.txrCoin = Plugin.TimePieceSprite.texture;
                                    break;
                                case "Yarn" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                {
                                    var yarnTextures = new[]
                                    {
                                        Plugin.YarnSprite.texture,
                                        Plugin.Yarn2Sprite.texture,
                                        Plugin.Yarn3Sprite.texture,
                                        Plugin.Yarn4Sprite.texture,
                                        Plugin.Yarn5Sprite.texture
                                    };
                                    var randomIndex = Random.Range(0, yarnTextures.Length);
                                    __instance.txrCoin = yarnTextures[randomIndex];
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.txrCoin = Plugin.ApProgressionSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.txrCoin = Plugin.ApUsefulSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                                    {
                                        var trapTextures = new[]
                                        {
                                            Plugin.ApTrapSprite.texture,
                                            Plugin.ApTrap2Sprite.texture,
                                            Plugin.ApTrap3Sprite.texture
                                        };
                                        var randomIndex = Random.Range(0, trapTextures.Length);
                                        __instance.txrCoin = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                                    {
                                        __instance.txrCoin = Plugin.ApFillerSprite.texture;
                                    }

                                    break;
                                }
                            }
                        }
                        else
                            __instance.txrCoin = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                                "Cassette" => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                                "Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                                "25 Apples" => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                                "10 Bugs" => TextureAndDisplayer(Plugin.BugSprite.texture, __instance, "Bugs"),
                                "1000 Snail Dollar" => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => TextureAndDisplayer(Plugin.ContactListSprite.texture, __instance, "Ticket"),
                                "Hairball City Ticket" => TextureAndDisplayer(Plugin.HcSprite.texture, __instance, "Ticket"),
                                "Turbine Town Ticket" => TextureAndDisplayer(Plugin.TtSprite.texture, __instance, "Ticket"),
                                "Salmon Creek Forest Ticket" => TextureAndDisplayer(Plugin.SfcSprite.texture, __instance, "Ticket"),
                                "Public Pool Ticket" => TextureAndDisplayer(Plugin.PpSprite.texture, __instance, "Ticket"),
                                "Bathhouse Ticket" => TextureAndDisplayer(Plugin.BathSprite.texture, __instance, "Ticket"),
                                "Tadpole HQ Ticket" => TextureAndDisplayer(Plugin.HqSprite.texture, __instance, "Ticket"),
                                "Gary's Garden Ticket" => TextureAndDisplayer(Plugin.GgSprite.texture, __instance, "Ticket"),
                                "Super Jump" => TextureAndDisplayer(Plugin.SuperJumpSprite.texture, __instance),
                                "Hairball City Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Turbine Town Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Salmon Creek Forest Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Public Pool Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Bathhouse Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Tadpole HQ Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Hairball City Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Turbine Town Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Salmon Creek Forest Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Public Pool Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Bathhouse Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Tadpole HQ Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                       + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                       + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
                case "Public Pool":
                {
                    var list = Locations.ScoutPPCoinList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    var offset = 72 - adjustment;
                    if (Locations.ScoutMiMaList.ContainsValue(__instance.myFlag))
                    {
                        list = Locations.ScoutMiMaList.ToList();
                        index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                        offset = __instance.myFlag == "cassetteCoin" ? 7 : 5;
                        
                    }
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.txrCoin = Plugin.TimePieceSprite.texture;
                                    break;
                                case "Yarn" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                {
                                    var yarnTextures = new[]
                                    {
                                        Plugin.YarnSprite.texture,
                                        Plugin.Yarn2Sprite.texture,
                                        Plugin.Yarn3Sprite.texture,
                                        Plugin.Yarn4Sprite.texture,
                                        Plugin.Yarn5Sprite.texture
                                    };
                                    var randomIndex = Random.Range(0, yarnTextures.Length);
                                    __instance.txrCoin = yarnTextures[randomIndex];
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.txrCoin = Plugin.ApProgressionSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.txrCoin = Plugin.ApUsefulSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                                    {
                                        var trapTextures = new[]
                                        {
                                            Plugin.ApTrapSprite.texture,
                                            Plugin.ApTrap2Sprite.texture,
                                            Plugin.ApTrap3Sprite.texture
                                        };
                                        var randomIndex = Random.Range(0, trapTextures.Length);
                                        __instance.txrCoin = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                                    {
                                        __instance.txrCoin = Plugin.ApFillerSprite.texture;
                                    }

                                    break;
                                }
                            }
                        }
                        else
                            __instance.txrCoin = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                                "Cassette" => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                                "Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                                "25 Apples" => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                                "10 Bugs" => TextureAndDisplayer(Plugin.BugSprite.texture, __instance, "Bugs"),
                                "1000 Snail Dollar" => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => TextureAndDisplayer(Plugin.ContactListSprite.texture, __instance, "Ticket"),
                                "Hairball City Ticket" => TextureAndDisplayer(Plugin.HcSprite.texture, __instance, "Ticket"),
                                "Turbine Town Ticket" => TextureAndDisplayer(Plugin.TtSprite.texture, __instance, "Ticket"),
                                "Salmon Creek Forest Ticket" => TextureAndDisplayer(Plugin.SfcSprite.texture, __instance, "Ticket"),
                                "Public Pool Ticket" => TextureAndDisplayer(Plugin.PpSprite.texture, __instance, "Ticket"),
                                "Bathhouse Ticket" => TextureAndDisplayer(Plugin.BathSprite.texture, __instance, "Ticket"),
                                "Tadpole HQ Ticket" => TextureAndDisplayer(Plugin.HqSprite.texture, __instance, "Ticket"),
                                "Gary's Garden Ticket" => TextureAndDisplayer(Plugin.GgSprite.texture, __instance, "Ticket"),
                                "Super Jump" => TextureAndDisplayer(Plugin.SuperJumpSprite.texture, __instance),
                                "Hairball City Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Turbine Town Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Salmon Creek Forest Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Public Pool Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Bathhouse Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Tadpole HQ Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Hairball City Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Turbine Town Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Salmon Creek Forest Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Public Pool Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Bathhouse Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Tadpole HQ Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                       + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                       + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathCoinList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    var offset = 80 - adjustment;
                    if (Locations.ScoutMiMaList.ContainsValue(__instance.myFlag))
                    {
                        list = Locations.ScoutMiMaList.ToList();
                        index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                        offset = 8;
                    }
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.txrCoin = Plugin.TimePieceSprite.texture;
                                    break;
                                case "Yarn" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                {
                                    var yarnTextures = new[]
                                    {
                                        Plugin.YarnSprite.texture,
                                        Plugin.Yarn2Sprite.texture,
                                        Plugin.Yarn3Sprite.texture,
                                        Plugin.Yarn4Sprite.texture,
                                        Plugin.Yarn5Sprite.texture
                                    };
                                    var randomIndex = Random.Range(0, yarnTextures.Length);
                                    __instance.txrCoin = yarnTextures[randomIndex];
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.txrCoin = Plugin.ApProgressionSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.txrCoin = Plugin.ApUsefulSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                                    {
                                        var trapTextures = new[]
                                        {
                                            Plugin.ApTrapSprite.texture,
                                            Plugin.ApTrap2Sprite.texture,
                                            Plugin.ApTrap3Sprite.texture
                                        };
                                        var randomIndex = Random.Range(0, trapTextures.Length);
                                        __instance.txrCoin = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                                    {
                                        __instance.txrCoin = Plugin.ApFillerSprite.texture;
                                    }

                                    break;
                                }
                            }
                        }
                        else
                            __instance.txrCoin = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                                "Cassette" => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                                "Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                                "25 Apples" => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                                "10 Bugs" => TextureAndDisplayer(Plugin.BugSprite.texture, __instance, "Bugs"),
                                "1000 Snail Dollar" => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => TextureAndDisplayer(Plugin.ContactListSprite.texture, __instance, "Ticket"),
                                "Hairball City Ticket" => TextureAndDisplayer(Plugin.HcSprite.texture, __instance, "Ticket"),
                                "Turbine Town Ticket" => TextureAndDisplayer(Plugin.TtSprite.texture, __instance, "Ticket"),
                                "Salmon Creek Forest Ticket" => TextureAndDisplayer(Plugin.SfcSprite.texture, __instance, "Ticket"),
                                "Public Pool Ticket" => TextureAndDisplayer(Plugin.PpSprite.texture, __instance, "Ticket"),
                                "Bathhouse Ticket" => TextureAndDisplayer(Plugin.BathSprite.texture, __instance, "Ticket"),
                                "Tadpole HQ Ticket" => TextureAndDisplayer(Plugin.HqSprite.texture, __instance, "Ticket"),
                                "Gary's Garden Ticket" => TextureAndDisplayer(Plugin.GgSprite.texture, __instance, "Ticket"),
                                "Super Jump" => TextureAndDisplayer(Plugin.SuperJumpSprite.texture, __instance),
                                "Hairball City Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Turbine Town Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Salmon Creek Forest Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Public Pool Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Bathhouse Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Tadpole HQ Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Hairball City Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Turbine Town Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Salmon Creek Forest Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Public Pool Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Bathhouse Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Tadpole HQ Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                       + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                       + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
                case "Tadpole inc" or "GarysGarden":
                {
                    var index = 0;
                    var offset = 0;
                    if (Locations.ScoutMiMaList.ContainsValue(__instance.myFlag))
                    {
                        var list = Locations.ScoutMiMaList.ToList();
                        index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                        if (currentscene == "GarysGarden")
                        {
                            offset = __instance.myFlag == "cassetteCoin" ? 13 : 11;
                        }
                        else
                        {
                            offset = __instance.myFlag == "cassetteCoin" ? 11 : 9;
                        }
                    }
                    else
                    {
                        var list = Locations.ScoutHQCoinList.ToList();
                        index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                        offset = 92 - adjustment;
                    }
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.txrCoin = Plugin.TimePieceSprite.texture;
                                    break;
                                case "Yarn" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                {
                                    var yarnTextures = new[]
                                    {
                                        Plugin.YarnSprite.texture,
                                        Plugin.Yarn2Sprite.texture,
                                        Plugin.Yarn3Sprite.texture,
                                        Plugin.Yarn4Sprite.texture,
                                        Plugin.Yarn5Sprite.texture
                                    };
                                    var randomIndex = Random.Range(0, yarnTextures.Length);
                                    __instance.txrCoin = yarnTextures[randomIndex];
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.txrCoin = Plugin.ApProgressionSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.txrCoin = Plugin.ApUsefulSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                                    {
                                        var trapTextures = new[]
                                        {
                                            Plugin.ApTrapSprite.texture,
                                            Plugin.ApTrap2Sprite.texture,
                                            Plugin.ApTrap3Sprite.texture
                                        };
                                        var randomIndex = Random.Range(0, trapTextures.Length);
                                        __instance.txrCoin = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                                    {
                                        __instance.txrCoin = Plugin.ApFillerSprite.texture;
                                    }

                                    break;
                                }
                            }
                        }
                        else
                            __instance.txrCoin = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                                "Cassette" => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                                "Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                                "25 Apples" => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                                "10 Bugs" => TextureAndDisplayer(Plugin.BugSprite.texture, __instance, "Bugs"),
                                "1000 Snail Dollar" => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => TextureAndDisplayer(Plugin.ContactListSprite.texture, __instance, "Ticket"),
                                "Hairball City Ticket" => TextureAndDisplayer(Plugin.HcSprite.texture, __instance, "Ticket"),
                                "Turbine Town Ticket" => TextureAndDisplayer(Plugin.TtSprite.texture, __instance, "Ticket"),
                                "Salmon Creek Forest Ticket" => TextureAndDisplayer(Plugin.SfcSprite.texture, __instance, "Ticket"),
                                "Public Pool Ticket" => TextureAndDisplayer(Plugin.PpSprite.texture, __instance, "Ticket"),
                                "Bathhouse Ticket" => TextureAndDisplayer(Plugin.BathSprite.texture, __instance, "Ticket"),
                                "Tadpole HQ Ticket" => TextureAndDisplayer(Plugin.HqSprite.texture, __instance, "Ticket"),
                                "Gary's Garden Ticket" => TextureAndDisplayer(Plugin.GgSprite.texture, __instance, "Ticket"),
                                "Super Jump" => TextureAndDisplayer(Plugin.SuperJumpSprite.texture, __instance),
                                "Hairball City Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Turbine Town Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Salmon Creek Forest Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Public Pool Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Bathhouse Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Tadpole HQ Fish" => TextureAndDisplayer(Plugin.FishSprite.texture, __instance),
                                "Hairball City Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Turbine Town Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Salmon Creek Forest Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Public Pool Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Bathhouse Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                "Tadpole HQ Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance),
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                       + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                       + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
            }
        }
        private static void Skip(scrObtainCoin __instance)
        {
            var audio = __instance.GetComponent<AudioSource>();
            if (scrWorldSaveDataContainer.instance.coinFlags.Contains(__instance.myFlag)) return;
            audio.Play();
            scrWorldSaveDataContainer.instance.coinFlags.Add(__instance.myFlag);
            scrWorldSaveDataContainer.instance.gameSaveManager.gameData.generalGameData.coinAmount++;
            scrWorldSaveDataContainer.instance.SaveWorld();
            scrWorldSaveDataContainer.instance.gameSaveManager.SaveGame();
        }
    }

    [HarmonyPatch(typeof(scrObtainCassette), "Start")]
    public static class ObtainCassettePatch
    {
        private static Texture TextureAndDisplayer(Texture texture, scrObtainCassette instance, string displayer = "")
        {
            var timer = 5f;
            var cassetteDisplayer = Traverse.Create(instance).Field("cassetteDisplayer").GetValue<scrCassetteDisplayer>();
            cassetteDisplayer.visible = false;
            switch (displayer)
            {
                case "Coin":
                    ShowDisplayers.CoinDisplayer(timer);
                    break;
                case "Cassette":
                    cassetteDisplayer.visible = true;
                    break;
                case "Key":
                    ShowDisplayers.KeyDisplayer(timer);
                    break;
                case "Ticket":
                    ShowDisplayers.TicketDisplayer(timer);
                    break;
                case "Apples":
                    ShowDisplayers.AppleDisplayer(timer);
                    break;
                case "Bugs":
                    ShowDisplayers.BugDisplayer(timer);
                    break;
                case "":
                    break;
            }
            return texture;
        }
        private static void Postfix(scrObtainCassette __instance)
        {
            if (ArchipelagoMenu.SkipPickup)
            {
                Skip(__instance);
                __instance.Invoke("Die", 0f);
            }
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
            {
                if (int.Parse(ArchipelagoData.slotData["shuffle_garden"].ToString()) == 0)
                {
                    gardenAdjustment = 3;
                }
            }
            if (ArchipelagoData.slotData.ContainsKey("snailshop"))
            {
                if (int.Parse(ArchipelagoData.slotData["snailshop"].ToString()) == 0)
                {
                    snailShopAdjustment = 16;
                }
            }
            var adjustment = gardenAdjustment + snailShopAdjustment;
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    var list = Locations.ScoutHCCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    int offset = 101 - adjustment;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.txrCassette = Plugin.TimePieceSprite.texture;
                                    break;
                                case "Yarn" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                {
                                    var yarnTextures = new[]
                                    {
                                        Plugin.YarnSprite.texture,
                                        Plugin.Yarn2Sprite.texture,
                                        Plugin.Yarn3Sprite.texture,
                                        Plugin.Yarn4Sprite.texture,
                                        Plugin.Yarn5Sprite.texture
                                    };
                                    var randomIndex = Random.Range(0, yarnTextures.Length);
                                    __instance.txrCassette = yarnTextures[randomIndex];
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.txrCassette = Plugin.ApProgressionSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.txrCassette = Plugin.ApUsefulSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                                    {
                                        var trapTextures = new[]
                                        {
                                            Plugin.ApTrapSprite.texture,
                                            Plugin.ApTrap2Sprite.texture,
                                            Plugin.ApTrap3Sprite.texture
                                        };
                                        var randomIndex = Random.Range(0, trapTextures.Length);
                                        __instance.txrCassette = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                                    {
                                        __instance.txrCassette = Plugin.ApFillerSprite.texture;
                                    }

                                    break;
                                }
                            }
                        }
                        else
                            __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                                "Cassette" => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                                "Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                                "25 Apples" => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                                "10 Bugs" => TextureAndDisplayer(Plugin.BugSprite.texture, __instance, "Bugs"),
                                "1000 Snail Dollar" => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => TextureAndDisplayer(Plugin.ContactListSprite.texture, __instance, "Ticket"),
                                "Hairball City Ticket" => TextureAndDisplayer(Plugin.HcSprite.texture, __instance, "Ticket"),
                                "Turbine Town Ticket" => TextureAndDisplayer(Plugin.TtSprite.texture, __instance, "Ticket"),
                                "Salmon Creek Forest Ticket" => TextureAndDisplayer(Plugin.SfcSprite.texture, __instance, "Ticket"),
                                "Public Pool Ticket" => TextureAndDisplayer(Plugin.PpSprite.texture, __instance, "Ticket"),
                                "Bathhouse Ticket" => TextureAndDisplayer(Plugin.BathSprite.texture, __instance, "Ticket"),
                                "Tadpole HQ Ticket" => TextureAndDisplayer(Plugin.HqSprite.texture, __instance, "Ticket"),
                                "Gary's Garden Ticket" => TextureAndDisplayer(Plugin.GgSprite.texture, __instance, "Ticket"),
                                "Super Jump" => TextureAndDisplayer(Plugin.SuperJumpSprite.texture, __instance),
                                "Hairball City Fish" => TextureAndDisplayer(Plugin.HairballFishSprite.texture, __instance),
                                "Turbine Town Fish" => TextureAndDisplayer(Plugin.TurbineFishSprite.texture, __instance),
                                "Salmon Creek Forest Fish" => TextureAndDisplayer(Plugin.SalmonFishSprite.texture, __instance),
                                "Public Pool Fish" => TextureAndDisplayer(Plugin.PoolFishSprite.texture, __instance),
                                "Bathhouse Fish" => TextureAndDisplayer(Plugin.BathFishSprite.texture, __instance),
                                "Tadpole HQ Fish" => TextureAndDisplayer(Plugin.TadpoleFishSprite.texture, __instance),
                                "Hairball City Key" => TextureAndDisplayer(Plugin.HairballKeySprite.texture, __instance),
                                "Turbine Town Key" => TextureAndDisplayer(Plugin.TurbineKeySprite.texture, __instance),
                                "Salmon Creek Forest Key" => TextureAndDisplayer(Plugin.SalmonKeySprite.texture, __instance),
                                "Public Pool Key" => TextureAndDisplayer(Plugin.PoolKeySprite.texture, __instance),
                                "Bathhouse Key" => TextureAndDisplayer(Plugin.BathKeySprite.texture, __instance),
                                "Tadpole HQ Key" => TextureAndDisplayer(Plugin.TadpoleKeySprite.texture, __instance),
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName
                                                       + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                       + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
                case "Trash Kingdom":
                {
                    var list = Locations.ScoutTTCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    int offset = 111 - adjustment;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.txrCassette = Plugin.TimePieceSprite.texture;
                                    break;
                                case "Yarn" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                {
                                    var yarnTextures = new[]
                                    {
                                        Plugin.YarnSprite.texture,
                                        Plugin.Yarn2Sprite.texture,
                                        Plugin.Yarn3Sprite.texture,
                                        Plugin.Yarn4Sprite.texture,
                                        Plugin.Yarn5Sprite.texture
                                    };
                                    var randomIndex = Random.Range(0, yarnTextures.Length);
                                    __instance.txrCassette = yarnTextures[randomIndex];
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.txrCassette = Plugin.ApProgressionSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.txrCassette = Plugin.ApUsefulSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                                    {
                                        var trapTextures = new[]
                                        {
                                            Plugin.ApTrapSprite.texture,
                                            Plugin.ApTrap2Sprite.texture,
                                            Plugin.ApTrap3Sprite.texture
                                        };
                                        var randomIndex = Random.Range(0, trapTextures.Length);
                                        __instance.txrCassette = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                                    {
                                        __instance.txrCassette = Plugin.ApFillerSprite.texture;
                                    }

                                    break;
                                }
                            }
                        }
                        else
                            __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                                "Cassette" => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                                "Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                                "25 Apples" => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                                "10 Bugs" => TextureAndDisplayer(Plugin.BugSprite.texture, __instance, "Bugs"),
                                "1000 Snail Dollar" => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => TextureAndDisplayer(Plugin.ContactListSprite.texture, __instance, "Ticket"),
                                "Hairball City Ticket" => TextureAndDisplayer(Plugin.HcSprite.texture, __instance, "Ticket"),
                                "Turbine Town Ticket" => TextureAndDisplayer(Plugin.TtSprite.texture, __instance, "Ticket"),
                                "Salmon Creek Forest Ticket" => TextureAndDisplayer(Plugin.SfcSprite.texture, __instance, "Ticket"),
                                "Public Pool Ticket" => TextureAndDisplayer(Plugin.PpSprite.texture, __instance, "Ticket"),
                                "Bathhouse Ticket" => TextureAndDisplayer(Plugin.BathSprite.texture, __instance, "Ticket"),
                                "Tadpole HQ Ticket" => TextureAndDisplayer(Plugin.HqSprite.texture, __instance, "Ticket"),
                                "Gary's Garden Ticket" => TextureAndDisplayer(Plugin.GgSprite.texture, __instance, "Ticket"),
                                "Super Jump" => TextureAndDisplayer(Plugin.SuperJumpSprite.texture, __instance),
                                "Hairball City Fish" => TextureAndDisplayer(Plugin.HairballFishSprite.texture, __instance),
                                "Turbine Town Fish" => TextureAndDisplayer(Plugin.TurbineFishSprite.texture, __instance),
                                "Salmon Creek Forest Fish" => TextureAndDisplayer(Plugin.SalmonFishSprite.texture, __instance),
                                "Public Pool Fish" => TextureAndDisplayer(Plugin.PoolFishSprite.texture, __instance),
                                "Bathhouse Fish" => TextureAndDisplayer(Plugin.BathFishSprite.texture, __instance),
                                "Tadpole HQ Fish" => TextureAndDisplayer(Plugin.TadpoleFishSprite.texture, __instance),
                                "Hairball City Key" => TextureAndDisplayer(Plugin.HairballKeySprite.texture, __instance),
                                "Turbine Town Key" => TextureAndDisplayer(Plugin.TurbineKeySprite.texture, __instance),
                                "Salmon Creek Forest Key" => TextureAndDisplayer(Plugin.SalmonKeySprite.texture, __instance),
                                "Public Pool Key" => TextureAndDisplayer(Plugin.PoolKeySprite.texture, __instance),
                                "Bathhouse Key" => TextureAndDisplayer(Plugin.BathKeySprite.texture, __instance),
                                "Tadpole HQ Key" => TextureAndDisplayer(Plugin.TadpoleKeySprite.texture, __instance),
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName
                                                       + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                       + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSFCCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    int offset = 121 - adjustment;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.txrCassette = Plugin.TimePieceSprite.texture;
                                    break;
                                case "Yarn" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                {
                                    var yarnTextures = new[]
                                    {
                                        Plugin.YarnSprite.texture,
                                        Plugin.Yarn2Sprite.texture,
                                        Plugin.Yarn3Sprite.texture,
                                        Plugin.Yarn4Sprite.texture,
                                        Plugin.Yarn5Sprite.texture
                                    };
                                    var randomIndex = Random.Range(0, yarnTextures.Length);
                                    __instance.txrCassette = yarnTextures[randomIndex];
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.txrCassette = Plugin.ApProgressionSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.txrCassette = Plugin.ApUsefulSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                                    {
                                        var trapTextures = new[]
                                        {
                                            Plugin.ApTrapSprite.texture,
                                            Plugin.ApTrap2Sprite.texture,
                                            Plugin.ApTrap3Sprite.texture
                                        };
                                        var randomIndex = Random.Range(0, trapTextures.Length);
                                        __instance.txrCassette = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                                    {
                                        __instance.txrCassette = Plugin.ApFillerSprite.texture;
                                    }

                                    break;
                                }
                            }
                        }
                        else
                            __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                                "Cassette" => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                                "Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                                "25 Apples" => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                                "10 Bugs" => TextureAndDisplayer(Plugin.BugSprite.texture, __instance, "Bugs"),
                                "1000 Snail Dollar" => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => TextureAndDisplayer(Plugin.ContactListSprite.texture, __instance, "Ticket"),
                                "Hairball City Ticket" => TextureAndDisplayer(Plugin.HcSprite.texture, __instance, "Ticket"),
                                "Turbine Town Ticket" => TextureAndDisplayer(Plugin.TtSprite.texture, __instance, "Ticket"),
                                "Salmon Creek Forest Ticket" => TextureAndDisplayer(Plugin.SfcSprite.texture, __instance, "Ticket"),
                                "Public Pool Ticket" => TextureAndDisplayer(Plugin.PpSprite.texture, __instance, "Ticket"),
                                "Bathhouse Ticket" => TextureAndDisplayer(Plugin.BathSprite.texture, __instance, "Ticket"),
                                "Tadpole HQ Ticket" => TextureAndDisplayer(Plugin.HqSprite.texture, __instance, "Ticket"),
                                "Gary's Garden Ticket" => TextureAndDisplayer(Plugin.GgSprite.texture, __instance, "Ticket"),
                                "Super Jump" => TextureAndDisplayer(Plugin.SuperJumpSprite.texture, __instance),
                                "Hairball City Fish" => TextureAndDisplayer(Plugin.HairballFishSprite.texture, __instance),
                                "Turbine Town Fish" => TextureAndDisplayer(Plugin.TurbineFishSprite.texture, __instance),
                                "Salmon Creek Forest Fish" => TextureAndDisplayer(Plugin.SalmonFishSprite.texture, __instance),
                                "Public Pool Fish" => TextureAndDisplayer(Plugin.PoolFishSprite.texture, __instance),
                                "Bathhouse Fish" => TextureAndDisplayer(Plugin.BathFishSprite.texture, __instance),
                                "Tadpole HQ Fish" => TextureAndDisplayer(Plugin.TadpoleFishSprite.texture, __instance),
                                "Hairball City Key" => TextureAndDisplayer(Plugin.HairballKeySprite.texture, __instance),
                                "Turbine Town Key" => TextureAndDisplayer(Plugin.TurbineKeySprite.texture, __instance),
                                "Salmon Creek Forest Key" => TextureAndDisplayer(Plugin.SalmonKeySprite.texture, __instance),
                                "Public Pool Key" => TextureAndDisplayer(Plugin.PoolKeySprite.texture, __instance),
                                "Bathhouse Key" => TextureAndDisplayer(Plugin.BathKeySprite.texture, __instance),
                                "Tadpole HQ Key" => TextureAndDisplayer(Plugin.TadpoleKeySprite.texture, __instance),
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName
                                                       + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                       + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
                case "Public Pool":
                {
                    var list = Locations.ScoutPPCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    int offset = 132 - adjustment;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.txrCassette = Plugin.TimePieceSprite.texture;
                                    break;
                                case "Yarn" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                {
                                    var yarnTextures = new[]
                                    {
                                        Plugin.YarnSprite.texture,
                                        Plugin.Yarn2Sprite.texture,
                                        Plugin.Yarn3Sprite.texture,
                                        Plugin.Yarn4Sprite.texture,
                                        Plugin.Yarn5Sprite.texture
                                    };
                                    var randomIndex = Random.Range(0, yarnTextures.Length);
                                    __instance.txrCassette = yarnTextures[randomIndex];
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.txrCassette = Plugin.ApProgressionSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.txrCassette = Plugin.ApUsefulSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                                    {
                                        var trapTextures = new[]
                                        {
                                            Plugin.ApTrapSprite.texture,
                                            Plugin.ApTrap2Sprite.texture,
                                            Plugin.ApTrap3Sprite.texture
                                        };
                                        var randomIndex = Random.Range(0, trapTextures.Length);
                                        __instance.txrCassette = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                                    {
                                        __instance.txrCassette = Plugin.ApFillerSprite.texture;
                                    }

                                    break;
                                }
                            }
                        }
                        else
                            __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                                "Cassette" => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                                "Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                                "25 Apples" => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                                "10 Bugs" => TextureAndDisplayer(Plugin.BugSprite.texture, __instance, "Bugs"),
                                "1000 Snail Dollar" => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => TextureAndDisplayer(Plugin.ContactListSprite.texture, __instance, "Ticket"),
                                "Hairball City Ticket" => TextureAndDisplayer(Plugin.HcSprite.texture, __instance, "Ticket"),
                                "Turbine Town Ticket" => TextureAndDisplayer(Plugin.TtSprite.texture, __instance, "Ticket"),
                                "Salmon Creek Forest Ticket" => TextureAndDisplayer(Plugin.SfcSprite.texture, __instance, "Ticket"),
                                "Public Pool Ticket" => TextureAndDisplayer(Plugin.PpSprite.texture, __instance, "Ticket"),
                                "Bathhouse Ticket" => TextureAndDisplayer(Plugin.BathSprite.texture, __instance, "Ticket"),
                                "Tadpole HQ Ticket" => TextureAndDisplayer(Plugin.HqSprite.texture, __instance, "Ticket"),
                                "Gary's Garden Ticket" => TextureAndDisplayer(Plugin.GgSprite.texture, __instance, "Ticket"),
                                "Super Jump" => TextureAndDisplayer(Plugin.SuperJumpSprite.texture, __instance),
                                "Hairball City Fish" => TextureAndDisplayer(Plugin.HairballFishSprite.texture, __instance),
                                "Turbine Town Fish" => TextureAndDisplayer(Plugin.TurbineFishSprite.texture, __instance),
                                "Salmon Creek Forest Fish" => TextureAndDisplayer(Plugin.SalmonFishSprite.texture, __instance),
                                "Public Pool Fish" => TextureAndDisplayer(Plugin.PoolFishSprite.texture, __instance),
                                "Bathhouse Fish" => TextureAndDisplayer(Plugin.BathFishSprite.texture, __instance),
                                "Tadpole HQ Fish" => TextureAndDisplayer(Plugin.TadpoleFishSprite.texture, __instance),
                                "Hairball City Key" => TextureAndDisplayer(Plugin.HairballKeySprite.texture, __instance),
                                "Turbine Town Key" => TextureAndDisplayer(Plugin.TurbineKeySprite.texture, __instance),
                                "Salmon Creek Forest Key" => TextureAndDisplayer(Plugin.SalmonKeySprite.texture, __instance),
                                "Public Pool Key" => TextureAndDisplayer(Plugin.PoolKeySprite.texture, __instance),
                                "Bathhouse Key" => TextureAndDisplayer(Plugin.BathKeySprite.texture, __instance),
                                "Tadpole HQ Key" => TextureAndDisplayer(Plugin.TadpoleKeySprite.texture, __instance),
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName
                                                       + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                       + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    int offset = 142 - adjustment;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.txrCassette = Plugin.TimePieceSprite.texture;
                                    break;
                                case "Yarn" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                {
                                    var yarnTextures = new[]
                                    {
                                        Plugin.YarnSprite.texture,
                                        Plugin.Yarn2Sprite.texture,
                                        Plugin.Yarn3Sprite.texture,
                                        Plugin.Yarn4Sprite.texture,
                                        Plugin.Yarn5Sprite.texture
                                    };
                                    var randomIndex = Random.Range(0, yarnTextures.Length);
                                    __instance.txrCassette = yarnTextures[randomIndex];
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.txrCassette = Plugin.ApProgressionSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.txrCassette = Plugin.ApUsefulSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                                    {
                                        var trapTextures = new[]
                                        {
                                            Plugin.ApTrapSprite.texture,
                                            Plugin.ApTrap2Sprite.texture,
                                            Plugin.ApTrap3Sprite.texture
                                        };
                                        var randomIndex = Random.Range(0, trapTextures.Length);
                                        __instance.txrCassette = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                                    {
                                        __instance.txrCassette = Plugin.ApFillerSprite.texture;
                                    }

                                    break;
                                }
                            }
                        }
                        else
                            __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                                "Cassette" => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                                "Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                                "25 Apples" => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                                "10 Bugs" => TextureAndDisplayer(Plugin.BugSprite.texture, __instance, "Bugs"),
                                "1000 Snail Dollar" => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => TextureAndDisplayer(Plugin.ContactListSprite.texture, __instance, "Ticket"),
                                "Hairball City Ticket" => TextureAndDisplayer(Plugin.HcSprite.texture, __instance, "Ticket"),
                                "Turbine Town Ticket" => TextureAndDisplayer(Plugin.TtSprite.texture, __instance, "Ticket"),
                                "Salmon Creek Forest Ticket" => TextureAndDisplayer(Plugin.SfcSprite.texture, __instance, "Ticket"),
                                "Public Pool Ticket" => TextureAndDisplayer(Plugin.PpSprite.texture, __instance, "Ticket"),
                                "Bathhouse Ticket" => TextureAndDisplayer(Plugin.BathSprite.texture, __instance, "Ticket"),
                                "Tadpole HQ Ticket" => TextureAndDisplayer(Plugin.HqSprite.texture, __instance, "Ticket"),
                                "Gary's Garden Ticket" => TextureAndDisplayer(Plugin.GgSprite.texture, __instance, "Ticket"),
                                "Super Jump" => TextureAndDisplayer(Plugin.SuperJumpSprite.texture, __instance),
                                "Hairball City Fish" => TextureAndDisplayer(Plugin.HairballFishSprite.texture, __instance),
                                "Turbine Town Fish" => TextureAndDisplayer(Plugin.TurbineFishSprite.texture, __instance),
                                "Salmon Creek Forest Fish" => TextureAndDisplayer(Plugin.SalmonFishSprite.texture, __instance),
                                "Public Pool Fish" => TextureAndDisplayer(Plugin.PoolFishSprite.texture, __instance),
                                "Bathhouse Fish" => TextureAndDisplayer(Plugin.BathFishSprite.texture, __instance),
                                "Tadpole HQ Fish" => TextureAndDisplayer(Plugin.TadpoleFishSprite.texture, __instance),
                                "Hairball City Key" => TextureAndDisplayer(Plugin.HairballKeySprite.texture, __instance),
                                "Turbine Town Key" => TextureAndDisplayer(Plugin.TurbineKeySprite.texture, __instance),
                                "Salmon Creek Forest Key" => TextureAndDisplayer(Plugin.SalmonKeySprite.texture, __instance),
                                "Public Pool Key" => TextureAndDisplayer(Plugin.PoolKeySprite.texture, __instance),
                                "Bathhouse Key" => TextureAndDisplayer(Plugin.BathKeySprite.texture, __instance),
                                "Tadpole HQ Key" => TextureAndDisplayer(Plugin.TadpoleKeySprite.texture, __instance),
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName
                                                       + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                       + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutHQCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    int offset = 152 - adjustment;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.txrCassette = Plugin.TimePieceSprite.texture;
                                    break;
                                case "Yarn" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                {
                                    var yarnTextures = new[]
                                    {
                                        Plugin.YarnSprite.texture,
                                        Plugin.Yarn2Sprite.texture,
                                        Plugin.Yarn3Sprite.texture,
                                        Plugin.Yarn4Sprite.texture,
                                        Plugin.Yarn5Sprite.texture
                                    };
                                    var randomIndex = Random.Range(0, yarnTextures.Length);
                                    __instance.txrCassette = yarnTextures[randomIndex];
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.txrCassette = Plugin.ApProgressionSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.txrCassette = Plugin.ApUsefulSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                                    {
                                        var trapTextures = new[]
                                        {
                                            Plugin.ApTrapSprite.texture,
                                            Plugin.ApTrap2Sprite.texture,
                                            Plugin.ApTrap3Sprite.texture
                                        };
                                        var randomIndex = Random.Range(0, trapTextures.Length);
                                        __instance.txrCassette = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                                    {
                                        __instance.txrCassette = Plugin.ApFillerSprite.texture;
                                    }

                                    break;
                                }
                            }
                        }
                        else
                            __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                                "Cassette" => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                                "Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                                "25 Apples" => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                                "10 Bugs" => TextureAndDisplayer(Plugin.BugSprite.texture, __instance, "Bugs"),
                                "1000 Snail Dollar" => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => TextureAndDisplayer(Plugin.ContactListSprite.texture, __instance, "Ticket"),
                                "Hairball City Ticket" => TextureAndDisplayer(Plugin.HcSprite.texture, __instance, "Ticket"),
                                "Turbine Town Ticket" => TextureAndDisplayer(Plugin.TtSprite.texture, __instance, "Ticket"),
                                "Salmon Creek Forest Ticket" => TextureAndDisplayer(Plugin.SfcSprite.texture, __instance, "Ticket"),
                                "Public Pool Ticket" => TextureAndDisplayer(Plugin.PpSprite.texture, __instance, "Ticket"),
                                "Bathhouse Ticket" => TextureAndDisplayer(Plugin.BathSprite.texture, __instance, "Ticket"),
                                "Tadpole HQ Ticket" => TextureAndDisplayer(Plugin.HqSprite.texture, __instance, "Ticket"),
                                "Gary's Garden Ticket" => TextureAndDisplayer(Plugin.GgSprite.texture, __instance, "Ticket"),
                                "Super Jump" => TextureAndDisplayer(Plugin.SuperJumpSprite.texture, __instance),
                                "Hairball City Fish" => TextureAndDisplayer(Plugin.HairballFishSprite.texture, __instance),
                                "Turbine Town Fish" => TextureAndDisplayer(Plugin.TurbineFishSprite.texture, __instance),
                                "Salmon Creek Forest Fish" => TextureAndDisplayer(Plugin.SalmonFishSprite.texture, __instance),
                                "Public Pool Fish" => TextureAndDisplayer(Plugin.PoolFishSprite.texture, __instance),
                                "Bathhouse Fish" => TextureAndDisplayer(Plugin.BathFishSprite.texture, __instance),
                                "Tadpole HQ Fish" => TextureAndDisplayer(Plugin.TadpoleFishSprite.texture, __instance),
                                "Hairball City Key" => TextureAndDisplayer(Plugin.HairballKeySprite.texture, __instance),
                                "Turbine Town Key" => TextureAndDisplayer(Plugin.TurbineKeySprite.texture, __instance),
                                "Salmon Creek Forest Key" => TextureAndDisplayer(Plugin.SalmonKeySprite.texture, __instance),
                                "Public Pool Key" => TextureAndDisplayer(Plugin.PoolKeySprite.texture, __instance),
                                "Bathhouse Key" => TextureAndDisplayer(Plugin.BathKeySprite.texture, __instance),
                                "Tadpole HQ Key" => TextureAndDisplayer(Plugin.TadpoleKeySprite.texture, __instance),
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName
                                                       + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                       + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
                case "GarysGarden":
                {
                    var list = Locations.ScoutGardenCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    const int offset = 162;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.txrCassette = Plugin.TimePieceSprite.texture;
                                    break;
                                case "Yarn" 
                                when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                {
                                    var yarnTextures = new[]
                                    {
                                        Plugin.YarnSprite.texture,
                                        Plugin.Yarn2Sprite.texture,
                                        Plugin.Yarn3Sprite.texture,
                                        Plugin.Yarn4Sprite.texture,
                                        Plugin.Yarn5Sprite.texture
                                    };
                                    var randomIndex = Random.Range(0, yarnTextures.Length);
                                    __instance.txrCassette = yarnTextures[randomIndex];
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.txrCassette = Plugin.ApProgressionSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.txrCassette = Plugin.ApUsefulSprite.texture;
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                                    {
                                        var trapTextures = new[]
                                        {
                                            Plugin.ApTrapSprite.texture,
                                            Plugin.ApTrap2Sprite.texture,
                                            Plugin.ApTrap3Sprite.texture
                                        };
                                        var randomIndex = Random.Range(0, trapTextures.Length);
                                        __instance.txrCassette = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                                    {
                                        __instance.txrCassette = Plugin.ApFillerSprite.texture;
                                    }

                                    break;
                                }
                            }
                        }
                        else
                            __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                                "Cassette" => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                                "Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                                "25 Apples" => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                                "10 Bugs" => TextureAndDisplayer(Plugin.BugSprite.texture, __instance, "Bugs"),
                                "1000 Snail Dollar" => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => TextureAndDisplayer(Plugin.ContactListSprite.texture, __instance, "Ticket"),
                                "Hairball City Ticket" => TextureAndDisplayer(Plugin.HcSprite.texture, __instance, "Ticket"),
                                "Turbine Town Ticket" => TextureAndDisplayer(Plugin.TtSprite.texture, __instance, "Ticket"),
                                "Salmon Creek Forest Ticket" => TextureAndDisplayer(Plugin.SfcSprite.texture, __instance, "Ticket"),
                                "Public Pool Ticket" => TextureAndDisplayer(Plugin.PpSprite.texture, __instance, "Ticket"),
                                "Bathhouse Ticket" => TextureAndDisplayer(Plugin.BathSprite.texture, __instance, "Ticket"),
                                "Tadpole HQ Ticket" => TextureAndDisplayer(Plugin.HqSprite.texture, __instance, "Ticket"),
                                "Gary's Garden Ticket" => TextureAndDisplayer(Plugin.GgSprite.texture, __instance, "Ticket"),
                                "Super Jump" => TextureAndDisplayer(Plugin.SuperJumpSprite.texture, __instance),
                                "Hairball City Fish" => TextureAndDisplayer(Plugin.HairballFishSprite.texture, __instance),
                                "Turbine Town Fish" => TextureAndDisplayer(Plugin.TurbineFishSprite.texture, __instance),
                                "Salmon Creek Forest Fish" => TextureAndDisplayer(Plugin.SalmonFishSprite.texture, __instance),
                                "Public Pool Fish" => TextureAndDisplayer(Plugin.PoolFishSprite.texture, __instance),
                                "Bathhouse Fish" => TextureAndDisplayer(Plugin.BathFishSprite.texture, __instance),
                                "Tadpole HQ Fish" => TextureAndDisplayer(Plugin.TadpoleFishSprite.texture, __instance),
                                "Hairball City Key" => TextureAndDisplayer(Plugin.HairballKeySprite.texture, __instance),
                                "Turbine Town Key" => TextureAndDisplayer(Plugin.TurbineKeySprite.texture, __instance),
                                "Salmon Creek Forest Key" => TextureAndDisplayer(Plugin.SalmonKeySprite.texture, __instance),
                                "Public Pool Key" => TextureAndDisplayer(Plugin.PoolKeySprite.texture, __instance),
                                "Bathhouse Key" => TextureAndDisplayer(Plugin.BathKeySprite.texture, __instance),
                                "Tadpole HQ Key" => TextureAndDisplayer(Plugin.TadpoleKeySprite.texture, __instance),
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName
                                                       + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                       + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
            }
        }

        private static void Skip(scrObtainCassette __instance)
        {
            var audio = __instance.GetComponent<AudioSource>();
            if (scrWorldSaveDataContainer.instance.cassetteFlags.Contains(__instance.flag)) return;
            audio.Play();
            scrWorldSaveDataContainer.instance.cassetteFlags.Add(__instance.flag);
            scrWorldSaveDataContainer.instance.gameSaveManager.gameData.generalGameData.cassetteAmount++;
            scrWorldSaveDataContainer.instance.SaveWorld();
            scrWorldSaveDataContainer.instance.gameSaveManager.SaveGame();
        }
    }
}