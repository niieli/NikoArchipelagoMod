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

        private static void PlaceModel(int index, int offset, scrObtainCoin __instance)
        {
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
                            __instance.txrCoin = Plugin.YarnSprite.texture;
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
                    __instance.txrCoin = ArchipelagoClient.ScoutedLocations[index + offset].ItemId switch
                    {
                        ItemID.Coin => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                        ItemID.Cassette => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                        ItemID.Key => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                        ItemID.Apples => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                        ItemID.Bugs => TextureAndDisplayer(Plugin.BugsSprite.texture, __instance, "Bugs"),
                        ItemID.SnailMoney => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
                        ItemID.ContactList1 or ItemID.ContactList2 or ItemID.ProgressiveContactList => TextureAndDisplayer(Plugin.ContactListSprite.texture, __instance, "Ticket"),
                        ItemID.HairballCityTicket => TextureAndDisplayer(Plugin.HcSprite.texture, __instance, "Ticket"),
                        ItemID.TurbineTownTicket => TextureAndDisplayer(Plugin.TtSprite.texture, __instance, "Ticket"),
                        ItemID.SalmonCreekForestTicket => TextureAndDisplayer(Plugin.SfcSprite.texture, __instance, "Ticket"),
                        ItemID.PublicPoolTicket => TextureAndDisplayer(Plugin.PpSprite.texture, __instance, "Ticket"),
                        ItemID.BathhouseTicket => TextureAndDisplayer(Plugin.BathSprite.texture, __instance, "Ticket"),
                        ItemID.TadpoleHqTicket => TextureAndDisplayer(Plugin.HqSprite.texture, __instance, "Ticket"),
                        ItemID.GarysGardenTicket => TextureAndDisplayer(Plugin.GgSprite.texture, __instance, "Ticket"),
                        ItemID.SuperJump => TextureAndDisplayer(Plugin.SuperJumpSprite.texture, __instance),
                        ItemID.HairballCityFish => TextureAndDisplayer(Plugin.HairballFishSprite.texture, __instance),
                        ItemID.TurbineTownFish => TextureAndDisplayer(Plugin.TurbineFishSprite.texture, __instance),
                        ItemID.SalmonCreekForestFish => TextureAndDisplayer(Plugin.SalmonFishSprite.texture, __instance),
                        ItemID.PublicPoolFish => TextureAndDisplayer(Plugin.PoolFishSprite.texture, __instance),
                        ItemID.BathhouseFish => TextureAndDisplayer(Plugin.BathFishSprite.texture, __instance),
                        ItemID.TadpoleHqFish => TextureAndDisplayer(Plugin.TadpoleFishSprite.texture, __instance),
                        ItemID.HairballCityKey => TextureAndDisplayer(Plugin.HairballKeySprite.texture, __instance),
                        ItemID.TurbineTownKey => TextureAndDisplayer(Plugin.TurbineKeySprite.texture, __instance),
                        ItemID.SalmonCreekForestKey => TextureAndDisplayer(Plugin.SalmonKeySprite.texture, __instance),
                        ItemID.PublicPoolKey => TextureAndDisplayer(Plugin.PoolKeySprite.texture, __instance),
                        ItemID.BathhouseKey => TextureAndDisplayer(Plugin.BathKeySprite.texture, __instance),
                        ItemID.TadpoleHqKey => TextureAndDisplayer(Plugin.TadpoleKeySprite.texture, __instance),
                        ItemID.HairballCityFlower => TextureAndDisplayer(Plugin.HairballFlowerSprite.texture, __instance),
                        ItemID.TurbineTownFlower => TextureAndDisplayer(Plugin.TurbineFlowerSprite.texture, __instance),
                        ItemID.SalmonCreekForestFlower => TextureAndDisplayer(Plugin.SalmonFlowerSprite.texture, __instance),
                        ItemID.PublicPoolFlower => TextureAndDisplayer(Plugin.PoolFlowerSprite.texture, __instance),
                        ItemID.BathhouseFlower => TextureAndDisplayer(Plugin.BathFlowerSprite.texture, __instance),
                        ItemID.TadpoleHqFlower => TextureAndDisplayer(Plugin.TadpoleFlowerSprite.texture, __instance),
                        ItemID.HairballCityCassette => TextureAndDisplayer(Plugin.HairballCassetteSprite.texture, __instance),
                        ItemID.TurbineTownCassette => TextureAndDisplayer(Plugin.TurbineCassetteSprite.texture, __instance),
                        ItemID.SalmonCreekForestCassette => TextureAndDisplayer(Plugin.SalmonCassetteSprite.texture, __instance),
                        ItemID.PublicPoolCassette => TextureAndDisplayer(Plugin.PoolCassetteSprite.texture, __instance),
                        ItemID.BathhouseCassette => TextureAndDisplayer(Plugin.BathCassetteSprite.texture, __instance),
                        ItemID.TadpoleHqCassette => TextureAndDisplayer(Plugin.TadpoleCassetteSprite.texture, __instance),
                        ItemID.HairballCitySeed => TextureAndDisplayer(Plugin.HairballSeedSprite.texture, __instance),
                        ItemID.SalmonCreekForestSeed => TextureAndDisplayer(Plugin.SalmonSeedSprite.texture, __instance),
                        ItemID.BathhouseSeed => TextureAndDisplayer(Plugin.BathSeedSprite.texture, __instance),
                        ItemID.SpeedBoost => TextureAndDisplayer(Plugin.SpeedBoostSprite.texture, __instance),
                        ItemID.WhoopsTrap => TextureAndDisplayer(Plugin.WhoopsTrapSprite.texture, __instance),
                        ItemID.IronBootsTrap => TextureAndDisplayer(Plugin.IronBootsTrapSprite.texture, __instance),
                        ItemID.MyTurnTrap => TextureAndDisplayer(Plugin.MyTurnTrapSprite.texture, __instance),
                        ItemID.FreezeTrap => TextureAndDisplayer(Plugin.FreezeTrapSprite.texture, __instance),
                        ItemID.HomeTrap => TextureAndDisplayer(Plugin.HomeTrapSprite.texture, __instance),
                        ItemID.WideTrap => TextureAndDisplayer(Plugin.WideTrapSprite.texture, __instance),
                        ItemID.PhoneTrap => TextureAndDisplayer(Plugin.PhoneCallTrapSprite.texture, __instance),
                        ItemID.TinyTrap => TextureAndDisplayer(Plugin.TinyTrapSprite.texture, __instance),
                        ItemID.GravityTrap => TextureAndDisplayer(Plugin.GravityTrapSprite.texture, __instance),
                        ItemID.PartyInvitation => TextureAndDisplayer(Plugin.PartyTicketSprite.texture, __instance),
                        ItemID.SafetyHelmet => TextureAndDisplayer(Plugin.BonkHelmetSprite.texture, __instance),
                        ItemID.BugNet => TextureAndDisplayer(Plugin.BugNetSprite.texture, __instance),
                        ItemID.SodaRepair => TextureAndDisplayer(Plugin.SodaRepairSprite.texture, __instance),
                        ItemID.ParasolRepair => TextureAndDisplayer(Plugin.ParasolRepairSprite.texture, __instance),
                        ItemID.SwimCourse => TextureAndDisplayer(Plugin.SwimCourseSprite.texture, __instance),
                        ItemID.Textbox => TextureAndDisplayer(Plugin.TextboxItemSprite.texture, __instance),
                        ItemID.AcRepair => TextureAndDisplayer(Plugin.ACRepairSprite.texture, __instance),
                        ItemID.AppleBasket => TextureAndDisplayer(Plugin.AppleBasketSprite.texture, __instance),
                        ItemID.HairballCityBone => TextureAndDisplayer(Plugin.HairballBoneSprite.texture, __instance),
                        ItemID.TurbineTownBone => TextureAndDisplayer(Plugin.TurbineBoneSprite.texture, __instance),
                        ItemID.SalmonCreekForestBone => TextureAndDisplayer(Plugin.SalmonBoneSprite.texture, __instance),
                        ItemID.PublicPoolBone => TextureAndDisplayer(Plugin.PoolBoneSprite.texture, __instance),
                        ItemID.BathhouseBone => TextureAndDisplayer(Plugin.BathBoneSprite.texture, __instance),
                        ItemID.TadpoleHqBone => TextureAndDisplayer(Plugin.TadpoleBoneSprite.texture, __instance),
                        _ => Plugin.APIconSprite.texture
                    };
            }
            Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
            Plugin.BepinLogger.LogInfo("-------------------------------------------------" 
                                       + "\nItem: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName
                                       + "\nLocation: " +
                                       ArchipelagoClient.ScoutedLocations[index + offset].LocationName
                                       + "\nLocationID: " +
                                       ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
        }
        
        private static void Postfix(scrObtainCoin __instance)
        {
            if (ArchipelagoMenu.SkipPickup)
            {
                Skip(__instance);
                __instance.Invoke("Die", 0f);
                MyCharacterController.instance._diveConsumed = false;
                MyCharacterController.instance.state = MyCharacterController.States.Normal;
            }
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
            {
                if (!ArchipelagoData.Options.GarysGarden)
                {
                    gardenAdjustment = 2;
                }
            }
            if (ArchipelagoData.slotData.ContainsKey("snailshop"))
            {
                if (!ArchipelagoData.Options.Snailshop)
                {
                    snailShopAdjustment = 16;
                }
            }
            if (ArchipelagoData.slotData.ContainsKey("cassette_logic"))
                if (ArchipelagoData.Options.Cassette == ArchipelagoOptions.CassetteMode.Progressive)
                    cassetteAdjustment = 14;
            var adjustment = gardenAdjustment + snailShopAdjustment + cassetteAdjustment;
            
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
                    PlaceModel(index, offset, __instance);
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
                    PlaceModel(index, offset, __instance);
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
                    PlaceModel(index, offset, __instance);
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
                    PlaceModel(index, offset, __instance);
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
                    PlaceModel(index, offset, __instance);
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
                    PlaceModel(index, offset, __instance);
                    break;
                }
            }
        }
        private static void Skip(scrObtainCoin __instance)
        {
            var audio = __instance.GetComponent<AudioSource>();
            __instance.transform.position = MyCharacterController.instance.transform.position;
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
        
        private static void PlaceModel(int index, int offset, scrObtainCassette __instance)
        {
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
                            // var yarnTextures = new[]
                            // {
                            //     Plugin.YarnSprite.texture,
                            //     Plugin.Yarn2Sprite.texture,
                            //     Plugin.Yarn3Sprite.texture,
                            //     Plugin.Yarn4Sprite.texture,
                            //     Plugin.Yarn5Sprite.texture
                            // };
                            // var randomIndex = Random.Range(0, yarnTextures.Length);
                            // __instance.txrCassette = yarnTextures[randomIndex];
                            __instance.txrCassette = Plugin.YarnSprite.texture;
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
                    __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemId switch
                    {
                        ItemID.Coin => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                        ItemID.Cassette => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                        ItemID.Key => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                        ItemID.Apples => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                        ItemID.Bugs => TextureAndDisplayer(Plugin.BugsSprite.texture, __instance, "Bugs"),
                        ItemID.SnailMoney => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
                        ItemID.ContactList1 or ItemID.ContactList2 or ItemID.ProgressiveContactList => TextureAndDisplayer(Plugin.ContactListSprite.texture, __instance, "Ticket"),
                        ItemID.HairballCityTicket => TextureAndDisplayer(Plugin.HcSprite.texture, __instance, "Ticket"),
                        ItemID.TurbineTownTicket => TextureAndDisplayer(Plugin.TtSprite.texture, __instance, "Ticket"),
                        ItemID.SalmonCreekForestTicket => TextureAndDisplayer(Plugin.SfcSprite.texture, __instance, "Ticket"),
                        ItemID.PublicPoolTicket => TextureAndDisplayer(Plugin.PpSprite.texture, __instance, "Ticket"),
                        ItemID.BathhouseTicket => TextureAndDisplayer(Plugin.BathSprite.texture, __instance, "Ticket"),
                        ItemID.TadpoleHqTicket => TextureAndDisplayer(Plugin.HqSprite.texture, __instance, "Ticket"),
                        ItemID.GarysGardenTicket => TextureAndDisplayer(Plugin.GgSprite.texture, __instance, "Ticket"),
                        ItemID.SuperJump => TextureAndDisplayer(Plugin.SuperJumpSprite.texture, __instance),
                        ItemID.HairballCityFish => TextureAndDisplayer(Plugin.HairballFishSprite.texture, __instance),
                        ItemID.TurbineTownFish => TextureAndDisplayer(Plugin.TurbineFishSprite.texture, __instance),
                        ItemID.SalmonCreekForestFish => TextureAndDisplayer(Plugin.SalmonFishSprite.texture, __instance),
                        ItemID.PublicPoolFish => TextureAndDisplayer(Plugin.PoolFishSprite.texture, __instance),
                        ItemID.BathhouseFish => TextureAndDisplayer(Plugin.BathFishSprite.texture, __instance),
                        ItemID.TadpoleHqFish => TextureAndDisplayer(Plugin.TadpoleFishSprite.texture, __instance),
                        ItemID.HairballCityKey => TextureAndDisplayer(Plugin.HairballKeySprite.texture, __instance),
                        ItemID.TurbineTownKey => TextureAndDisplayer(Plugin.TurbineKeySprite.texture, __instance),
                        ItemID.SalmonCreekForestKey => TextureAndDisplayer(Plugin.SalmonKeySprite.texture, __instance),
                        ItemID.PublicPoolKey => TextureAndDisplayer(Plugin.PoolKeySprite.texture, __instance),
                        ItemID.BathhouseKey => TextureAndDisplayer(Plugin.BathKeySprite.texture, __instance),
                        ItemID.TadpoleHqKey => TextureAndDisplayer(Plugin.TadpoleKeySprite.texture, __instance),
                        ItemID.HairballCityFlower => TextureAndDisplayer(Plugin.HairballFlowerSprite.texture, __instance),
                        ItemID.TurbineTownFlower => TextureAndDisplayer(Plugin.TurbineFlowerSprite.texture, __instance),
                        ItemID.SalmonCreekForestFlower => TextureAndDisplayer(Plugin.SalmonFlowerSprite.texture, __instance),
                        ItemID.PublicPoolFlower => TextureAndDisplayer(Plugin.PoolFlowerSprite.texture, __instance),
                        ItemID.BathhouseFlower => TextureAndDisplayer(Plugin.BathFlowerSprite.texture, __instance),
                        ItemID.TadpoleHqFlower => TextureAndDisplayer(Plugin.TadpoleFlowerSprite.texture, __instance),
                        ItemID.HairballCityCassette => TextureAndDisplayer(Plugin.HairballCassetteSprite.texture, __instance),
                        ItemID.TurbineTownCassette => TextureAndDisplayer(Plugin.TurbineCassetteSprite.texture, __instance),
                        ItemID.SalmonCreekForestCassette => TextureAndDisplayer(Plugin.SalmonCassetteSprite.texture, __instance),
                        ItemID.PublicPoolCassette => TextureAndDisplayer(Plugin.PoolCassetteSprite.texture, __instance),
                        ItemID.BathhouseCassette => TextureAndDisplayer(Plugin.BathCassetteSprite.texture, __instance),
                        ItemID.TadpoleHqCassette => TextureAndDisplayer(Plugin.TadpoleCassetteSprite.texture, __instance),
                        ItemID.HairballCitySeed => TextureAndDisplayer(Plugin.HairballSeedSprite.texture, __instance),
                        ItemID.SalmonCreekForestSeed => TextureAndDisplayer(Plugin.SalmonSeedSprite.texture, __instance),
                        ItemID.BathhouseSeed => TextureAndDisplayer(Plugin.BathSeedSprite.texture, __instance),
                        ItemID.SpeedBoost => TextureAndDisplayer(Plugin.SpeedBoostSprite.texture, __instance),
                        ItemID.WhoopsTrap => TextureAndDisplayer(Plugin.WhoopsTrapSprite.texture, __instance),
                        ItemID.IronBootsTrap => TextureAndDisplayer(Plugin.IronBootsTrapSprite.texture, __instance),
                        ItemID.MyTurnTrap => TextureAndDisplayer(Plugin.MyTurnTrapSprite.texture, __instance),
                        ItemID.FreezeTrap => TextureAndDisplayer(Plugin.FreezeTrapSprite.texture, __instance),
                        ItemID.HomeTrap => TextureAndDisplayer(Plugin.HomeTrapSprite.texture, __instance),
                        ItemID.WideTrap => TextureAndDisplayer(Plugin.WideTrapSprite.texture, __instance),
                        ItemID.PhoneTrap => TextureAndDisplayer(Plugin.PhoneCallTrapSprite.texture, __instance),
                        ItemID.TinyTrap => TextureAndDisplayer(Plugin.TinyTrapSprite.texture, __instance),
                        ItemID.GravityTrap => TextureAndDisplayer(Plugin.GravityTrapSprite.texture, __instance),
                        ItemID.PartyInvitation => TextureAndDisplayer(Plugin.PartyTicketSprite.texture, __instance),
                        ItemID.SafetyHelmet => TextureAndDisplayer(Plugin.BonkHelmetSprite.texture, __instance),
                        ItemID.BugNet => TextureAndDisplayer(Plugin.BugNetSprite.texture, __instance),
                        ItemID.SodaRepair => TextureAndDisplayer(Plugin.SodaRepairSprite.texture, __instance),
                        ItemID.ParasolRepair => TextureAndDisplayer(Plugin.ParasolRepairSprite.texture, __instance),
                        ItemID.SwimCourse => TextureAndDisplayer(Plugin.SwimCourseSprite.texture, __instance),
                        ItemID.Textbox => TextureAndDisplayer(Plugin.TextboxItemSprite.texture, __instance),
                        ItemID.AcRepair => TextureAndDisplayer(Plugin.ACRepairSprite.texture, __instance),
                        ItemID.AppleBasket => TextureAndDisplayer(Plugin.AppleBasketSprite.texture, __instance),
                        ItemID.HairballCityBone => TextureAndDisplayer(Plugin.HairballBoneSprite.texture, __instance),
                        ItemID.TurbineTownBone => TextureAndDisplayer(Plugin.TurbineBoneSprite.texture, __instance),
                        ItemID.SalmonCreekForestBone => TextureAndDisplayer(Plugin.SalmonBoneSprite.texture, __instance),
                        ItemID.PublicPoolBone => TextureAndDisplayer(Plugin.PoolBoneSprite.texture, __instance),
                        ItemID.BathhouseBone => TextureAndDisplayer(Plugin.BathBoneSprite.texture, __instance),
                        ItemID.TadpoleHqBone => TextureAndDisplayer(Plugin.TadpoleBoneSprite.texture, __instance),
                        _ => Plugin.APIconSprite.texture
                    };
            }
            Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
            Plugin.BepinLogger.LogInfo("-------------------------------------------------" 
                                       + "\nItem: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName
                                       + "\nLocation: " +
                                       ArchipelagoClient.ScoutedLocations[index + offset].LocationName
                                       + "\nLocationID: " +
                                       ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
        }
        
        private static void Postfix(scrObtainCassette __instance)
        {
            if (ArchipelagoMenu.SkipPickup)
            {
                Skip(__instance);
                __instance.Invoke("Die", 0f);
                MyCharacterController.instance.state = MyCharacterController.States.Normal;
                //MyCharacterController.instance._diveConsumed = false;
            }
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
            {
                if (!ArchipelagoData.Options.GarysGarden)
                {
                    gardenAdjustment = 3;
                }
            }
            if (ArchipelagoData.slotData.ContainsKey("snailshop"))
            {
                if (!ArchipelagoData.Options.Snailshop)
                {
                    snailShopAdjustment = 16;
                }
            }
            if (ArchipelagoData.slotData.ContainsKey("cassette_logic"))
                if (ArchipelagoData.Options.Cassette == ArchipelagoOptions.CassetteMode.Progressive)
                    cassetteAdjustment = 14;
            var adjustment = gardenAdjustment + snailShopAdjustment + cassetteAdjustment;
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    var list = Locations.ScoutHCCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    int offset = 101 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Trash Kingdom":
                {
                    var list = Locations.ScoutTTCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    int offset = 111 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSFCCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    int offset = 121 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Public Pool":
                {
                    var list = Locations.ScoutPPCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    int offset = 132 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    int offset = 142 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutHQCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    int offset = 152 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "GarysGarden":
                {
                    var list = Locations.ScoutGardenCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.flag);
                    const int offset = 162;
                    PlaceModel(index, offset, __instance);
                    break;
                }
            }
        }

        private static void Skip(scrObtainCassette __instance)
        {
            var audio = __instance.GetComponent<AudioSource>();
            __instance.transform.position = MyCharacterController.instance.transform.position;
            if (scrWorldSaveDataContainer.instance.cassetteFlags.Contains(__instance.flag)) return;
            audio.Play();
            scrWorldSaveDataContainer.instance.cassetteFlags.Add(__instance.flag);
            scrWorldSaveDataContainer.instance.gameSaveManager.gameData.generalGameData.cassetteAmount++;
            scrWorldSaveDataContainer.instance.SaveWorld();
            scrWorldSaveDataContainer.instance.gameSaveManager.SaveGame();
        }
    }
}