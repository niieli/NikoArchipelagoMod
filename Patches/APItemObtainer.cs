using System.Linq;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
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

        private static void PlaceModel(ScoutedItemInfo scoutedItemInfo, scrObtainCoin __instance)
        {
            if (scoutedItemInfo.ItemGame != "Here Comes Niko!")
            {
                switch (scoutedItemInfo.ItemName)
                {
                    case "Time Piece" 
                        when scoutedItemInfo.ItemGame == "A Hat in Time":
                        __instance.txrCoin = Assets.TimePieceSprite.texture;
                        break;
                    case "Yarn" 
                        when scoutedItemInfo.ItemGame == "A Hat in Time":
                    {
                        __instance.txrCoin = Assets.YarnSprite.texture;
                        break;
                    }
                    default:
                    {
                        if (scoutedItemInfo.Flags.HasFlag(ItemFlags.Advancement))
                        {
                            __instance.txrCoin = Assets.ApProgressionSprite.texture;
                        }
                        else if (scoutedItemInfo.Flags.HasFlag(ItemFlags.NeverExclude))
                        {
                            __instance.txrCoin = Assets.ApUsefulSprite.texture;
                        }
                        else if (scoutedItemInfo.Flags.HasFlag(ItemFlags.Trap))
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
                        else if (scoutedItemInfo.Flags.HasFlag(ItemFlags.None))
                        {
                            __instance.txrCoin = Assets.ApFillerSprite.texture;
                        }

                        break;
                    }
                }
            }
            else
                __instance.txrCoin = scoutedItemInfo.ItemId switch
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
            Plugin.BepinLogger.LogInfo("-------------------------------------------------"
                                       + "\nItem: " + scoutedItemInfo.ItemName
                                       + "\nLocation: " + scoutedItemInfo.LocationName
                                       + "\nLocationID: " + scoutedItemInfo.LocationId);
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
            
            var currentscene = SceneManager.GetActiveScene().name;
            var list = currentscene switch
            {
                "Home" => Locations.ScoutHomeCoinList,
                "Hairball City" => Locations.ScoutHCCoinList,
                "Trash Kingdom" => Locations.ScoutTTCoinList,
                "Salmon Creek Forest" => Locations.ScoutSCFCoinList,
                "Public Pool" => Locations.ScoutPPCoinList,
                "The Bathhouse" => Locations.ScoutBathCoinList,
                "Tadpole inc" => Locations.ScoutHQCoinList,
                "GarysGarden" => Locations.ScoutGardenCoinList,
                _ => null
            };

            if (list == null)
            {
                Plugin.BepinLogger.LogError($"Couldn't find locations for {__instance.myFlag} | Scene: {currentscene} ");
                return;
            }
            
            var pair = list.FirstOrDefault(p => p.Value == __instance.myFlag);

            if (pair.Key == 0)
                return;

            var locationId = pair.Key;

            if (!ArchipelagoClient.ScoutedLocations.TryGetValue(locationId, out var scoutedItemInfo))
                return;

            PlaceModel(scoutedItemInfo, __instance);
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
        
        private static void PlaceModel(ScoutedItemInfo scoutedItemInfo, scrObtainCassette __instance)
        {
            if (scoutedItemInfo.ItemGame != "Here Comes Niko!")
            {
                switch (scoutedItemInfo.ItemName)
                {
                    case "Time Piece" 
                        when scoutedItemInfo.ItemGame == "A Hat in Time":
                        __instance.txrCassette = Assets.TimePieceSprite.texture;
                        break;
                    case "Yarn" 
                        when scoutedItemInfo.ItemGame == "A Hat in Time":
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
                        if (scoutedItemInfo.Flags.HasFlag(ItemFlags.Advancement))
                        {
                            __instance.txrCassette = Assets.ApProgressionSprite.texture;
                        }
                        else if (scoutedItemInfo.Flags.HasFlag(ItemFlags.NeverExclude))
                        {
                            __instance.txrCassette = Assets.ApUsefulSprite.texture;
                        }
                        else if (scoutedItemInfo.Flags.HasFlag(ItemFlags.Trap))
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
                        else if (scoutedItemInfo.Flags.HasFlag(ItemFlags.None))
                        {
                            __instance.txrCassette = Assets.ApFillerSprite.texture;
                        }

                        break;
                    }
                }
            }
            else
                __instance.txrCassette = scoutedItemInfo.ItemId switch
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
            Plugin.BepinLogger.LogInfo("-------------------------------------------------"
                                       + "\nItem: " + scoutedItemInfo.ItemName
                                       + "\nLocation: " + scoutedItemInfo.LocationName
                                       + "\nLocationID: " + scoutedItemInfo.LocationId);
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
            
            var currentscene = SceneManager.GetActiveScene().name;
            var list = currentscene switch
            {
                "Hairball City" => Locations.ScoutHCCassetteList,
                "Trash Kingdom" => Locations.ScoutTTCassetteList,
                "Salmon Creek Forest" => Locations.ScoutSCFCassetteList,
                "Public Pool" => Locations.ScoutPPCassetteList,
                "The Bathhouse" => Locations.ScoutBathCassetteList,
                "Tadpole inc" => Locations.ScoutHQCassetteList,
                "GarysGarden" => Locations.ScoutGardenCassetteList,
                _ => null
            };

            if (list == null)
            {
                Plugin.BepinLogger.LogError($"Couldn't find locations for {__instance.flag} | Scene: {currentscene} ");
                return;
            }
            
            var pair = list.FirstOrDefault(p => p.Value == __instance.flag);

            if (pair.Key == 0)
                return;

            var locationId = pair.Key;

            if (!ArchipelagoClient.ScoutedLocations.TryGetValue(locationId, out var scoutedItemInfo))
                return;

            PlaceModel(scoutedItemInfo, __instance);
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