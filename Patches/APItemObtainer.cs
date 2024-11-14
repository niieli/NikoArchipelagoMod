using System.Linq;
using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class APItemObtainer
{
    [HarmonyPatch(typeof(scrObtainCoin), "Start")]
    public static class ObtainCoinPatch
    {
        private static void Postfix(scrObtainCoin __instance)
        {
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Home" or "Hairball City":
                {
                    var index = 0;
                    var list = Locations.ScoutHCCoinList.ToList();
                    var offset = 36;
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
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                        }
                        else
                            __instance.txrCoin = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => __instance.txrCoin,
                                "Cassette" => Plugin.CassetteSprite.texture,
                                "Key" => Plugin.KeySprite.texture,
                                "25 Apples" => Plugin.ApplesSprite.texture,
                                "10 Bugs" => Plugin.BugSprite.texture,
                                "1000 Snail Dollar" => Plugin.SnailMoneySprite.texture,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => Plugin.ContactListSprite.texture,
                                "Hairball City Ticket" => Plugin.HcSprite.texture,
                                "Turbine Town Ticket" => Plugin.TtSprite.texture,
                                "Salmon Creek Forest Ticket" => Plugin.SfcSprite.texture,
                                "Public Pool Ticket" => Plugin.PpSprite.texture,
                                "Bathhouse Ticket" => Plugin.BathSprite.texture,
                                "Tadpole HQ Ticket" => Plugin.HqSprite.texture,
                                "Gary's Garden Ticket" => Plugin.GgSprite.texture,
                                "Super Jump" => Plugin.SuperJumpSprite.texture,
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    break;
                }
                case "Trash Kingdom":
                {
                    var list = Locations.ScoutTTCoinList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    var offset = 49;
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
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                        }
                        else
                            __instance.txrCoin = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => __instance.txrCoin,
                                "Cassette" => Plugin.CassetteSprite.texture,
                                "Key" => Plugin.KeySprite.texture,
                                "25 Apples" => Plugin.ApplesSprite.texture,
                                "10 Bugs" => Plugin.BugSprite.texture,
                                "1000 Snail Dollar" => Plugin.SnailMoneySprite.texture,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => Plugin.ContactListSprite.texture,
                                "Hairball City Ticket" => Plugin.HcSprite.texture,
                                "Turbine Town Ticket" => Plugin.TtSprite.texture,
                                "Salmon Creek Forest Ticket" => Plugin.SfcSprite.texture,
                                "Public Pool Ticket" => Plugin.PpSprite.texture,
                                "Bathhouse Ticket" => Plugin.BathSprite.texture,
                                "Tadpole HQ Ticket" => Plugin.HqSprite.texture,
                                "Gary's Garden Ticket" => Plugin.GgSprite.texture,
                                "Super Jump" => Plugin.SuperJumpSprite.texture,
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSFCCoinList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    var offset = 58;
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
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                        }
                        else
                            __instance.txrCoin = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => __instance.txrCoin,
                                "Cassette" => Plugin.CassetteSprite.texture,
                                "Key" => Plugin.KeySprite.texture,
                                "25 Apples" => Plugin.ApplesSprite.texture,
                                "10 Bugs" => Plugin.BugSprite.texture,
                                "1000 Snail Dollar" => Plugin.SnailMoneySprite.texture,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => Plugin.ContactListSprite.texture,
                                "Hairball City Ticket" => Plugin.HcSprite.texture,
                                "Turbine Town Ticket" => Plugin.TtSprite.texture,
                                "Salmon Creek Forest Ticket" => Plugin.SfcSprite.texture,
                                "Public Pool Ticket" => Plugin.PpSprite.texture,
                                "Bathhouse Ticket" => Plugin.BathSprite.texture,
                                "Tadpole HQ Ticket" => Plugin.HqSprite.texture,
                                "Gary's Garden Ticket" => Plugin.GgSprite.texture,
                                "Super Jump" => Plugin.SuperJumpSprite.texture,
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    break;
                }
                case "Public Pool":
                {
                    var list = Locations.ScoutPPCoinList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    var offset = 72;
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
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                        }
                        else
                            __instance.txrCoin = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => __instance.txrCoin,
                                "Cassette" => Plugin.CassetteSprite.texture,
                                "Key" => Plugin.KeySprite.texture,
                                "25 Apples" => Plugin.ApplesSprite.texture,
                                "10 Bugs" => Plugin.BugSprite.texture,
                                "1000 Snail Dollar" => Plugin.SnailMoneySprite.texture,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => Plugin.ContactListSprite.texture,
                                "Hairball City Ticket" => Plugin.HcSprite.texture,
                                "Turbine Town Ticket" => Plugin.TtSprite.texture,
                                "Salmon Creek Forest Ticket" => Plugin.SfcSprite.texture,
                                "Public Pool Ticket" => Plugin.PpSprite.texture,
                                "Bathhouse Ticket" => Plugin.BathSprite.texture,
                                "Tadpole HQ Ticket" => Plugin.HqSprite.texture,
                                "Gary's Garden Ticket" => Plugin.GgSprite.texture,
                                "Super Jump" => Plugin.SuperJumpSprite.texture,
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathCoinList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    var offset = 80;
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
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                        }
                        else
                            __instance.txrCoin = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => __instance.txrCoin,
                                "Cassette" => Plugin.CassetteSprite.texture,
                                "Key" => Plugin.KeySprite.texture,
                                "25 Apples" => Plugin.ApplesSprite.texture,
                                "10 Bugs" => Plugin.BugSprite.texture,
                                "1000 Snail Dollar" => Plugin.SnailMoneySprite.texture,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => Plugin.ContactListSprite.texture,
                                "Hairball City Ticket" => Plugin.HcSprite.texture,
                                "Turbine Town Ticket" => Plugin.TtSprite.texture,
                                "Salmon Creek Forest Ticket" => Plugin.SfcSprite.texture,
                                "Public Pool Ticket" => Plugin.PpSprite.texture,
                                "Bathhouse Ticket" => Plugin.BathSprite.texture,
                                "Tadpole HQ Ticket" => Plugin.HqSprite.texture,
                                "Gary's Garden Ticket" => Plugin.GgSprite.texture,
                                "Super Jump" => Plugin.SuperJumpSprite.texture,
                                _ => Plugin.APIconSprite.texture
                            };
                    }
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
                        offset = 92;
                    }
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
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
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                        }
                        else
                            __instance.txrCoin = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => __instance.txrCoin,
                                "Cassette" => Plugin.CassetteSprite.texture,
                                "Key" => Plugin.KeySprite.texture,
                                "25 Apples" => Plugin.ApplesSprite.texture,
                                "10 Bugs" => Plugin.BugSprite.texture,
                                "1000 Snail Dollar" => Plugin.SnailMoneySprite.texture,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => Plugin.ContactListSprite.texture,
                                "Hairball City Ticket" => Plugin.HcSprite.texture,
                                "Turbine Town Ticket" => Plugin.TtSprite.texture,
                                "Salmon Creek Forest Ticket" => Plugin.SfcSprite.texture,
                                "Public Pool Ticket" => Plugin.PpSprite.texture,
                                "Bathhouse Ticket" => Plugin.BathSprite.texture,
                                "Tadpole HQ Ticket" => Plugin.HqSprite.texture,
                                "Gary's Garden Ticket" => Plugin.GgSprite.texture,
                                "Super Jump" => Plugin.SuperJumpSprite.texture,
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    break;
                }
            }
        }
    }

    [HarmonyPatch(typeof(scrObtainCassette), "Start")]
    public static class ObtainCassettePatch
    {
        private static void Postfix(scrObtainCassette __instance)
        {
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    var list = Locations.ScoutHCCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    const int offset = 101;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
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
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                        }
                        else
                            __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => Plugin.CoinSprite.texture,
                                "Cassette" => Plugin.CassetteSprite.texture,
                                "Key" => Plugin.KeySprite.texture,
                                "25 Apples" => Plugin.ApplesSprite.texture,
                                "10 Bugs" => Plugin.BugSprite.texture,
                                "1000 Snail Dollar" => Plugin.SnailMoneySprite.texture,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => Plugin.ContactListSprite.texture,
                                "Hairball City Ticket" => Plugin.HcSprite.texture,
                                "Turbine Town Ticket" => Plugin.TtSprite.texture,
                                "Salmon Creek Forest Ticket" => Plugin.SfcSprite.texture,
                                "Public Pool Ticket" => Plugin.PpSprite.texture,
                                "Bathhouse Ticket" => Plugin.BathSprite.texture,
                                "Tadpole HQ Ticket" => Plugin.HqSprite.texture,
                                "Gary's Garden Ticket" => Plugin.GgSprite.texture,
                                "Super Jump" => Plugin.SuperJumpSprite.texture,
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
                case "Trash Kingdom":
                {
                    var list = Locations.ScoutTTCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    const int offset = 111;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                            {
                                __instance.txrCassette = Plugin.ApProgressionSprite.texture;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.txrCassette = Plugin.ApUsefulSprite.texture;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.txrCassette = Plugin.ApFillerSprite.texture;
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
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                        }
                        else
                            __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => Plugin.CoinSprite.texture,
                                "Cassette" => Plugin.CassetteSprite.texture,
                                "Key" => Plugin.KeySprite.texture,
                                "25 Apples" => Plugin.ApplesSprite.texture,
                                "10 Bugs" => Plugin.BugSprite.texture,
                                "1000 Snail Dollar" => Plugin.SnailMoneySprite.texture,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => Plugin.ContactListSprite.texture,
                                "Hairball City Ticket" => Plugin.HcSprite.texture,
                                "Turbine Town Ticket" => Plugin.TtSprite.texture,
                                "Salmon Creek Forest Ticket" => Plugin.SfcSprite.texture,
                                "Public Pool Ticket" => Plugin.PpSprite.texture,
                                "Bathhouse Ticket" => Plugin.BathSprite.texture,
                                "Tadpole HQ Ticket" => Plugin.HqSprite.texture,
                                "Gary's Garden Ticket" => Plugin.GgSprite.texture,
                                "Super Jump" => Plugin.SuperJumpSprite.texture,
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSFCCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    const int offset = 121;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
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
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                        }
                        else
                            __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => Plugin.CoinSprite.texture,
                                "Cassette" => Plugin.CassetteSprite.texture,
                                "Key" => Plugin.KeySprite.texture,
                                "25 Apples" => Plugin.ApplesSprite.texture,
                                "10 Bugs" => Plugin.BugSprite.texture,
                                "1000 Snail Dollar" => Plugin.SnailMoneySprite.texture,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => Plugin.ContactListSprite.texture,
                                "Hairball City Ticket" => Plugin.HcSprite.texture,
                                "Turbine Town Ticket" => Plugin.TtSprite.texture,
                                "Salmon Creek Forest Ticket" => Plugin.SfcSprite.texture,
                                "Public Pool Ticket" => Plugin.PpSprite.texture,
                                "Bathhouse Ticket" => Plugin.BathSprite.texture,
                                "Tadpole HQ Ticket" => Plugin.HqSprite.texture,
                                "Gary's Garden Ticket" => Plugin.GgSprite.texture,
                                "Super Jump" => Plugin.SuperJumpSprite.texture,
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
                case "Public Pool":
                {
                    var list = Locations.ScoutPPCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    const int offset = 132;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
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
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                        }
                        else
                            __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => Plugin.CoinSprite.texture,
                                "Cassette" => Plugin.CassetteSprite.texture,
                                "Key" => Plugin.KeySprite.texture,
                                "25 Apples" => Plugin.ApplesSprite.texture,
                                "10 Bugs" => Plugin.BugSprite.texture,
                                "1000 Snail Dollar" => Plugin.SnailMoneySprite.texture,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => Plugin.ContactListSprite.texture,
                                "Hairball City Ticket" => Plugin.HcSprite.texture,
                                "Turbine Town Ticket" => Plugin.TtSprite.texture,
                                "Salmon Creek Forest Ticket" => Plugin.SfcSprite.texture,
                                "Public Pool Ticket" => Plugin.PpSprite.texture,
                                "Bathhouse Ticket" => Plugin.BathSprite.texture,
                                "Tadpole HQ Ticket" => Plugin.HqSprite.texture,
                                "Gary's Garden Ticket" => Plugin.GgSprite.texture,
                                "Super Jump" => Plugin.SuperJumpSprite.texture,
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    const int offset = 142;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
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
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                        }
                        else
                            __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => Plugin.CoinSprite.texture,
                                "Cassette" => Plugin.CassetteSprite.texture,
                                "Key" => Plugin.KeySprite.texture,
                                "25 Apples" => Plugin.ApplesSprite.texture,
                                "10 Bugs" => Plugin.BugSprite.texture,
                                "1000 Snail Dollar" => Plugin.SnailMoneySprite.texture,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => Plugin.ContactListSprite.texture,
                                "Hairball City Ticket" => Plugin.HcSprite.texture,
                                "Turbine Town Ticket" => Plugin.TtSprite.texture,
                                "Salmon Creek Forest Ticket" => Plugin.SfcSprite.texture,
                                "Public Pool Ticket" => Plugin.PpSprite.texture,
                                "Bathhouse Ticket" => Plugin.BathSprite.texture,
                                "Tadpole HQ Ticket" => Plugin.HqSprite.texture,
                                "Gary's Garden Ticket" => Plugin.GgSprite.texture,
                                "Super Jump" => Plugin.SuperJumpSprite.texture,
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutHQCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    const int offset = 152;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
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
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                        }
                        else
                            __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => Plugin.CoinSprite.texture,
                                "Cassette" => Plugin.CassetteSprite.texture,
                                "Key" => Plugin.KeySprite.texture,
                                "25 Apples" => Plugin.ApplesSprite.texture,
                                "10 Bugs" => Plugin.BugSprite.texture,
                                "1000 Snail Dollar" => Plugin.SnailMoneySprite.texture,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => Plugin.ContactListSprite.texture,
                                "Hairball City Ticket" => Plugin.HcSprite.texture,
                                "Turbine Town Ticket" => Plugin.TtSprite.texture,
                                "Salmon Creek Forest Ticket" => Plugin.SfcSprite.texture,
                                "Public Pool Ticket" => Plugin.PpSprite.texture,
                                "Bathhouse Ticket" => Plugin.BathSprite.texture,
                                "Tadpole HQ Ticket" => Plugin.HqSprite.texture,
                                "Gary's Garden Ticket" => Plugin.GgSprite.texture,
                                "Super Jump" => Plugin.SuperJumpSprite.texture,
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
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
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                        }
                        else
                            __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => Plugin.CoinSprite.texture,
                                "Cassette" => Plugin.CassetteSprite.texture,
                                "Key" => Plugin.KeySprite.texture,
                                "25 Apples" => Plugin.ApplesSprite.texture,
                                "10 Bugs" => Plugin.BugSprite.texture,
                                "1000 Snail Dollar" => Plugin.SnailMoneySprite.texture,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => Plugin.ContactListSprite.texture,
                                "Hairball City Ticket" => Plugin.HcSprite.texture,
                                "Turbine Town Ticket" => Plugin.TtSprite.texture,
                                "Salmon Creek Forest Ticket" => Plugin.SfcSprite.texture,
                                "Public Pool Ticket" => Plugin.PpSprite.texture,
                                "Bathhouse Ticket" => Plugin.BathSprite.texture,
                                "Tadpole HQ Ticket" => Plugin.HqSprite.texture,
                                "Gary's Garden Ticket" => Plugin.GgSprite.texture,
                                "Super Jump" => Plugin.SuperJumpSprite.texture,
                                _ => Plugin.APIconSprite.texture
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
            }
        }
    }
}