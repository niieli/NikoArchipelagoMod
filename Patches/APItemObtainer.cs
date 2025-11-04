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
                case "GardenSeed":
                    ShowDisplayers.GardenSeedDisplayer(timer);
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
                            __instance.txrCoin = Assets.TimePieceSprite.texture;
                            break;
                        case "Yarn" 
                            when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                        {
                            __instance.txrCoin = Assets.YarnSprite.texture;
                            break;
                        }
                        default:
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                            {
                                __instance.txrCoin = Assets.ApProgressionSprite.texture;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.txrCoin = Assets.ApUsefulSprite.texture;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    Assets.ApTrapSprite.texture,
                                    Assets.ApTrap2Sprite.texture,
                                    Assets.ApTrap3Sprite.texture
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.txrCoin = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.txrCoin = Assets.ApFillerSprite.texture;
                            }

                            break;
                        }
                    }
                }
                else
                    __instance.txrCoin = ArchipelagoClient.ScoutedLocations[index + offset].ItemId switch
                    {
                        ItemID.Coin => TextureAndDisplayer(Assets.CoinSprite.texture, __instance, "Coin"),
                        ItemID.Cassette => TextureAndDisplayer(Assets.CassetteSprite.texture, __instance, "Cassette"), 
                        ItemID.Key => TextureAndDisplayer(Assets.KeySprite.texture, __instance, "Key"),
                        ItemID.Apples => TextureAndDisplayer(Assets.ApplesSprite.texture, __instance, "Apples"),
                        ItemID.Bugs => TextureAndDisplayer(Assets.BugsSprite.texture, __instance, "Bugs"),
                        ItemID.SnailMoney => TextureAndDisplayer(Assets.SnailMoneySprite.texture, __instance),
                        ItemID.ContactList1 or ItemID.ContactList2 or ItemID.ProgressiveContactList => TextureAndDisplayer(Assets.ContactListSprite.texture, __instance, "Ticket"),
                        ItemID.HairballCityTicket => TextureAndDisplayer(Assets.HcSprite.texture, __instance, "Ticket"),
                        ItemID.TurbineTownTicket => TextureAndDisplayer(Assets.TtSprite.texture, __instance, "Ticket"),
                        ItemID.SalmonCreekForestTicket => TextureAndDisplayer(Assets.SfcSprite.texture, __instance, "Ticket"),
                        ItemID.PublicPoolTicket => TextureAndDisplayer(Assets.PpSprite.texture, __instance, "Ticket"),
                        ItemID.BathhouseTicket => TextureAndDisplayer(Assets.BathSprite.texture, __instance, "Ticket"),
                        ItemID.TadpoleHqTicket => TextureAndDisplayer(Assets.HqSprite.texture, __instance, "Ticket"),
                        ItemID.GarysGardenTicket => TextureAndDisplayer(Assets.GgSprite.texture, __instance, "Ticket"),
                        ItemID.SuperJump => TextureAndDisplayer(Assets.SuperJumpSprite.texture, __instance),
                        ItemID.HairballCityFish => TextureAndDisplayer(Assets.HairballFishSprite.texture, __instance),
                        ItemID.TurbineTownFish => TextureAndDisplayer(Assets.TurbineFishSprite.texture, __instance),
                        ItemID.SalmonCreekForestFish => TextureAndDisplayer(Assets.SalmonFishSprite.texture, __instance),
                        ItemID.PublicPoolFish => TextureAndDisplayer(Assets.PoolFishSprite.texture, __instance),
                        ItemID.BathhouseFish => TextureAndDisplayer(Assets.BathFishSprite.texture, __instance),
                        ItemID.TadpoleHqFish => TextureAndDisplayer(Assets.TadpoleFishSprite.texture, __instance),
                        ItemID.HairballCityKey => TextureAndDisplayer(Assets.HairballKeySprite.texture, __instance),
                        ItemID.TurbineTownKey => TextureAndDisplayer(Assets.TurbineKeySprite.texture, __instance),
                        ItemID.SalmonCreekForestKey => TextureAndDisplayer(Assets.SalmonKeySprite.texture, __instance),
                        ItemID.PublicPoolKey => TextureAndDisplayer(Assets.PoolKeySprite.texture, __instance),
                        ItemID.BathhouseKey => TextureAndDisplayer(Assets.BathKeySprite.texture, __instance),
                        ItemID.TadpoleHqKey => TextureAndDisplayer(Assets.TadpoleKeySprite.texture, __instance),
                        ItemID.HairballCityFlower => TextureAndDisplayer(Assets.HairballFlowerSprite.texture, __instance),
                        ItemID.TurbineTownFlower => TextureAndDisplayer(Assets.TurbineFlowerSprite.texture, __instance),
                        ItemID.SalmonCreekForestFlower => TextureAndDisplayer(Assets.SalmonFlowerSprite.texture, __instance),
                        ItemID.PublicPoolFlower => TextureAndDisplayer(Assets.PoolFlowerSprite.texture, __instance),
                        ItemID.BathhouseFlower => TextureAndDisplayer(Assets.BathFlowerSprite.texture, __instance),
                        ItemID.TadpoleHqFlower => TextureAndDisplayer(Assets.TadpoleFlowerSprite.texture, __instance),
                        ItemID.HairballCityCassette => TextureAndDisplayer(Assets.HairballCassetteSprite.texture, __instance),
                        ItemID.TurbineTownCassette => TextureAndDisplayer(Assets.TurbineCassetteSprite.texture, __instance),
                        ItemID.SalmonCreekForestCassette => TextureAndDisplayer(Assets.SalmonCassetteSprite.texture, __instance),
                        ItemID.PublicPoolCassette => TextureAndDisplayer(Assets.PoolCassetteSprite.texture, __instance),
                        ItemID.BathhouseCassette => TextureAndDisplayer(Assets.BathCassetteSprite.texture, __instance),
                        ItemID.TadpoleHqCassette => TextureAndDisplayer(Assets.TadpoleCassetteSprite.texture, __instance),
                        ItemID.HairballCitySeed => TextureAndDisplayer(Assets.HairballSeedSprite.texture, __instance),
                        ItemID.SalmonCreekForestSeed => TextureAndDisplayer(Assets.SalmonSeedSprite.texture, __instance),
                        ItemID.BathhouseSeed => TextureAndDisplayer(Assets.BathSeedSprite.texture, __instance),
                        ItemID.SpeedBoost => TextureAndDisplayer(Assets.SpeedBoostSprite.texture, __instance),
                        ItemID.WhoopsTrap => TextureAndDisplayer(Assets.WhoopsTrapSprite.texture, __instance),
                        ItemID.IronBootsTrap => TextureAndDisplayer(Assets.IronBootsTrapSprite.texture, __instance),
                        ItemID.MyTurnTrap => TextureAndDisplayer(Assets.MyTurnTrapSprite.texture, __instance),
                        ItemID.FreezeTrap => TextureAndDisplayer(Assets.FreezeTrapSprite.texture, __instance),
                        ItemID.HomeTrap => TextureAndDisplayer(Assets.HomeTrapSprite.texture, __instance),
                        ItemID.WideTrap => TextureAndDisplayer(Assets.WideTrapSprite.texture, __instance),
                        ItemID.PhoneTrap => TextureAndDisplayer(Assets.PhoneCallTrapSprite.texture, __instance),
                        ItemID.TinyTrap => TextureAndDisplayer(Assets.TinyTrapSprite.texture, __instance),
                        ItemID.GravityTrap => TextureAndDisplayer(Assets.GravityTrapSprite.texture, __instance),
                        ItemID.PartyInvitation => TextureAndDisplayer(Assets.PartyTicketSprite.texture, __instance),
                        ItemID.SafetyHelmet => TextureAndDisplayer(Assets.BonkHelmetSprite.texture, __instance),
                        ItemID.BugNet => TextureAndDisplayer(Assets.BugNetSprite.texture, __instance),
                        ItemID.SodaRepair => TextureAndDisplayer(Assets.SodaRepairSprite.texture, __instance),
                        ItemID.ParasolRepair => TextureAndDisplayer(Assets.ParasolRepairSprite.texture, __instance),
                        ItemID.SwimCourse => TextureAndDisplayer(Assets.SwimCourseSprite.texture, __instance),
                        ItemID.Textbox => TextureAndDisplayer(Assets.TextboxItemSprite.texture, __instance),
                        ItemID.AcRepair => TextureAndDisplayer(Assets.ACRepairSprite.texture, __instance),
                        ItemID.AppleBasket => TextureAndDisplayer(Assets.AppleBasketSprite.texture, __instance),
                        ItemID.HairballCityBone => TextureAndDisplayer(Assets.HairballBoneSprite.texture, __instance),
                        ItemID.TurbineTownBone => TextureAndDisplayer(Assets.TurbineBoneSprite.texture, __instance),
                        ItemID.SalmonCreekForestBone => TextureAndDisplayer(Assets.SalmonBoneSprite.texture, __instance),
                        ItemID.PublicPoolBone => TextureAndDisplayer(Assets.PoolBoneSprite.texture, __instance),
                        ItemID.BathhouseBone => TextureAndDisplayer(Assets.BathBoneSprite.texture, __instance),
                        ItemID.TadpoleHqBone => TextureAndDisplayer(Assets.TadpoleBoneSprite.texture, __instance),
                        ItemID.HairballCityTextbox => TextureAndDisplayer(Assets.HairballTextboxSprite.texture, __instance),
                        ItemID.TurbineTownTextbox => TextureAndDisplayer(Assets.TurbineTextboxSprite.texture, __instance),
                        ItemID.SalmonCreekForestTextbox => TextureAndDisplayer(Assets.SalmonTextboxSprite.texture, __instance),
                        ItemID.PublicPoolTextbox => TextureAndDisplayer(Assets.PoolTextboxSprite.texture, __instance),
                        ItemID.BathhouseTextbox => TextureAndDisplayer(Assets.BathTextboxSprite.texture, __instance),
                        ItemID.TadpoleHqTextbox => TextureAndDisplayer(Assets.TadpoleTextboxSprite.texture, __instance),
                        ItemID.GarysGardenSeed => TextureAndDisplayer(Assets.GardenSeedSprite.texture, __instance, "GardenSeed"),
                        _ => Assets.APIconSprite.texture
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
                case "GardenSeed":
                    ShowDisplayers.GardenSeedDisplayer(timer);
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
                            __instance.txrCassette = Assets.TimePieceSprite.texture;
                            break;
                        case "Yarn" 
                            when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                        {
                            // var yarnTextures = new[]
                            // {
                            //     Assets.YarnSprite.texture,
                            //     Assets.Yarn2Sprite.texture,
                            //     Assets.Yarn3Sprite.texture,
                            //     Assets.Yarn4Sprite.texture,
                            //     Assets.Yarn5Sprite.texture
                            // };
                            // var randomIndex = Random.Range(0, yarnTextures.Length);
                            // __instance.txrCassette = yarnTextures[randomIndex];
                            __instance.txrCassette = Assets.YarnSprite.texture;
                            break;
                        }
                        default:
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                            {
                                __instance.txrCassette = Assets.ApProgressionSprite.texture;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.txrCassette = Assets.ApUsefulSprite.texture;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    Assets.ApTrapSprite.texture,
                                    Assets.ApTrap2Sprite.texture,
                                    Assets.ApTrap3Sprite.texture
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.txrCassette = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.txrCassette = Assets.ApFillerSprite.texture;
                            }

                            break;
                        }
                    }
                }
                else
                    __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemId switch
                    {
                        ItemID.Coin => TextureAndDisplayer(Assets.CoinSprite.texture, __instance, "Coin"),
                        ItemID.Cassette => TextureAndDisplayer(Assets.CassetteSprite.texture, __instance, "Cassette"), 
                        ItemID.Key => TextureAndDisplayer(Assets.KeySprite.texture, __instance, "Key"),
                        ItemID.Apples => TextureAndDisplayer(Assets.ApplesSprite.texture, __instance, "Apples"),
                        ItemID.Bugs => TextureAndDisplayer(Assets.BugsSprite.texture, __instance, "Bugs"),
                        ItemID.SnailMoney => TextureAndDisplayer(Assets.SnailMoneySprite.texture, __instance),
                        ItemID.ContactList1 or ItemID.ContactList2 or ItemID.ProgressiveContactList => TextureAndDisplayer(Assets.ContactListSprite.texture, __instance, "Ticket"),
                        ItemID.HairballCityTicket => TextureAndDisplayer(Assets.HcSprite.texture, __instance, "Ticket"),
                        ItemID.TurbineTownTicket => TextureAndDisplayer(Assets.TtSprite.texture, __instance, "Ticket"),
                        ItemID.SalmonCreekForestTicket => TextureAndDisplayer(Assets.SfcSprite.texture, __instance, "Ticket"),
                        ItemID.PublicPoolTicket => TextureAndDisplayer(Assets.PpSprite.texture, __instance, "Ticket"),
                        ItemID.BathhouseTicket => TextureAndDisplayer(Assets.BathSprite.texture, __instance, "Ticket"),
                        ItemID.TadpoleHqTicket => TextureAndDisplayer(Assets.HqSprite.texture, __instance, "Ticket"),
                        ItemID.GarysGardenTicket => TextureAndDisplayer(Assets.GgSprite.texture, __instance, "Ticket"),
                        ItemID.SuperJump => TextureAndDisplayer(Assets.SuperJumpSprite.texture, __instance),
                        ItemID.HairballCityFish => TextureAndDisplayer(Assets.HairballFishSprite.texture, __instance),
                        ItemID.TurbineTownFish => TextureAndDisplayer(Assets.TurbineFishSprite.texture, __instance),
                        ItemID.SalmonCreekForestFish => TextureAndDisplayer(Assets.SalmonFishSprite.texture, __instance),
                        ItemID.PublicPoolFish => TextureAndDisplayer(Assets.PoolFishSprite.texture, __instance),
                        ItemID.BathhouseFish => TextureAndDisplayer(Assets.BathFishSprite.texture, __instance),
                        ItemID.TadpoleHqFish => TextureAndDisplayer(Assets.TadpoleFishSprite.texture, __instance),
                        ItemID.HairballCityKey => TextureAndDisplayer(Assets.HairballKeySprite.texture, __instance),
                        ItemID.TurbineTownKey => TextureAndDisplayer(Assets.TurbineKeySprite.texture, __instance),
                        ItemID.SalmonCreekForestKey => TextureAndDisplayer(Assets.SalmonKeySprite.texture, __instance),
                        ItemID.PublicPoolKey => TextureAndDisplayer(Assets.PoolKeySprite.texture, __instance),
                        ItemID.BathhouseKey => TextureAndDisplayer(Assets.BathKeySprite.texture, __instance),
                        ItemID.TadpoleHqKey => TextureAndDisplayer(Assets.TadpoleKeySprite.texture, __instance),
                        ItemID.HairballCityFlower => TextureAndDisplayer(Assets.HairballFlowerSprite.texture, __instance),
                        ItemID.TurbineTownFlower => TextureAndDisplayer(Assets.TurbineFlowerSprite.texture, __instance),
                        ItemID.SalmonCreekForestFlower => TextureAndDisplayer(Assets.SalmonFlowerSprite.texture, __instance),
                        ItemID.PublicPoolFlower => TextureAndDisplayer(Assets.PoolFlowerSprite.texture, __instance),
                        ItemID.BathhouseFlower => TextureAndDisplayer(Assets.BathFlowerSprite.texture, __instance),
                        ItemID.TadpoleHqFlower => TextureAndDisplayer(Assets.TadpoleFlowerSprite.texture, __instance),
                        ItemID.HairballCityCassette => TextureAndDisplayer(Assets.HairballCassetteSprite.texture, __instance),
                        ItemID.TurbineTownCassette => TextureAndDisplayer(Assets.TurbineCassetteSprite.texture, __instance),
                        ItemID.SalmonCreekForestCassette => TextureAndDisplayer(Assets.SalmonCassetteSprite.texture, __instance),
                        ItemID.PublicPoolCassette => TextureAndDisplayer(Assets.PoolCassetteSprite.texture, __instance),
                        ItemID.BathhouseCassette => TextureAndDisplayer(Assets.BathCassetteSprite.texture, __instance),
                        ItemID.TadpoleHqCassette => TextureAndDisplayer(Assets.TadpoleCassetteSprite.texture, __instance),
                        ItemID.HairballCitySeed => TextureAndDisplayer(Assets.HairballSeedSprite.texture, __instance),
                        ItemID.SalmonCreekForestSeed => TextureAndDisplayer(Assets.SalmonSeedSprite.texture, __instance),
                        ItemID.BathhouseSeed => TextureAndDisplayer(Assets.BathSeedSprite.texture, __instance),
                        ItemID.SpeedBoost => TextureAndDisplayer(Assets.SpeedBoostSprite.texture, __instance),
                        ItemID.WhoopsTrap => TextureAndDisplayer(Assets.WhoopsTrapSprite.texture, __instance),
                        ItemID.IronBootsTrap => TextureAndDisplayer(Assets.IronBootsTrapSprite.texture, __instance),
                        ItemID.MyTurnTrap => TextureAndDisplayer(Assets.MyTurnTrapSprite.texture, __instance),
                        ItemID.FreezeTrap => TextureAndDisplayer(Assets.FreezeTrapSprite.texture, __instance),
                        ItemID.HomeTrap => TextureAndDisplayer(Assets.HomeTrapSprite.texture, __instance),
                        ItemID.WideTrap => TextureAndDisplayer(Assets.WideTrapSprite.texture, __instance),
                        ItemID.PhoneTrap => TextureAndDisplayer(Assets.PhoneCallTrapSprite.texture, __instance),
                        ItemID.TinyTrap => TextureAndDisplayer(Assets.TinyTrapSprite.texture, __instance),
                        ItemID.GravityTrap => TextureAndDisplayer(Assets.GravityTrapSprite.texture, __instance),
                        ItemID.PartyInvitation => TextureAndDisplayer(Assets.PartyTicketSprite.texture, __instance),
                        ItemID.SafetyHelmet => TextureAndDisplayer(Assets.BonkHelmetSprite.texture, __instance),
                        ItemID.BugNet => TextureAndDisplayer(Assets.BugNetSprite.texture, __instance),
                        ItemID.SodaRepair => TextureAndDisplayer(Assets.SodaRepairSprite.texture, __instance),
                        ItemID.ParasolRepair => TextureAndDisplayer(Assets.ParasolRepairSprite.texture, __instance),
                        ItemID.SwimCourse => TextureAndDisplayer(Assets.SwimCourseSprite.texture, __instance),
                        ItemID.Textbox => TextureAndDisplayer(Assets.TextboxItemSprite.texture, __instance),
                        ItemID.AcRepair => TextureAndDisplayer(Assets.ACRepairSprite.texture, __instance),
                        ItemID.AppleBasket => TextureAndDisplayer(Assets.AppleBasketSprite.texture, __instance),
                        ItemID.HairballCityBone => TextureAndDisplayer(Assets.HairballBoneSprite.texture, __instance),
                        ItemID.TurbineTownBone => TextureAndDisplayer(Assets.TurbineBoneSprite.texture, __instance),
                        ItemID.SalmonCreekForestBone => TextureAndDisplayer(Assets.SalmonBoneSprite.texture, __instance),
                        ItemID.PublicPoolBone => TextureAndDisplayer(Assets.PoolBoneSprite.texture, __instance),
                        ItemID.BathhouseBone => TextureAndDisplayer(Assets.BathBoneSprite.texture, __instance),
                        ItemID.TadpoleHqBone => TextureAndDisplayer(Assets.TadpoleBoneSprite.texture, __instance),
                        ItemID.HairballCityTextbox => TextureAndDisplayer(Assets.HairballTextboxSprite.texture, __instance),
                        ItemID.TurbineTownTextbox => TextureAndDisplayer(Assets.TurbineTextboxSprite.texture, __instance),
                        ItemID.SalmonCreekForestTextbox => TextureAndDisplayer(Assets.SalmonTextboxSprite.texture, __instance),
                        ItemID.PublicPoolTextbox => TextureAndDisplayer(Assets.PoolTextboxSprite.texture, __instance),
                        ItemID.BathhouseTextbox => TextureAndDisplayer(Assets.BathTextboxSprite.texture, __instance),
                        ItemID.TadpoleHqTextbox => TextureAndDisplayer(Assets.TadpoleTextboxSprite.texture, __instance),
                        ItemID.GarysGardenSeed => TextureAndDisplayer(Assets.GardenSeedSprite.texture, __instance, "GardenSeed"),
                        _ => Assets.APIconSprite.texture
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
            if (ArchipelagoData.Options.GoalCompletion == ArchipelagoOptions.GoalCompletionMode.Garden)
                gardenAdjustment += 1;
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