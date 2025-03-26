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
                            // var yarnTextures = new[]
                            // {
                            //     Plugin.YarnSprite.texture,
                            //     Plugin.Yarn2Sprite.texture,
                            //     Plugin.Yarn3Sprite.texture,
                            //     Plugin.Yarn4Sprite.texture,
                            //     Plugin.Yarn5Sprite.texture
                            // };
                            // var randomIndex = Random.Range(0, yarnTextures.Length);
                            // __instance.txrCoin = yarnTextures[randomIndex];
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
                    __instance.txrCoin = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                    {
                        "Coin" => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                        "Cassette" => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                        "Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                        "Apples" or "25 Apples" => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                        "Bugs" or "10 Bugs" => TextureAndDisplayer(Plugin.BugSprite.texture, __instance, "Bugs"),
                        "Snail Money" or "1000 Snail Dollar" => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
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
                        "Hairball City Flower" => TextureAndDisplayer(Plugin.HairballFlowerSprite.texture, __instance),
                        "Turbine Town Flower" => TextureAndDisplayer(Plugin.TurbineFlowerSprite.texture, __instance),
                        "Salmon Creek Forest Flower" => TextureAndDisplayer(Plugin.SalmonFlowerSprite.texture, __instance),
                        "Public Pool Flower" => TextureAndDisplayer(Plugin.PoolFlowerSprite.texture, __instance),
                        "Bathhouse Flower" => TextureAndDisplayer(Plugin.BathFlowerSprite.texture, __instance),
                        "Tadpole HQ Flower" => TextureAndDisplayer(Plugin.TadpoleFlowerSprite.texture, __instance),
                        "Hairball City Cassette" => TextureAndDisplayer(Plugin.HairballCassetteSprite.texture, __instance),
                        "Turbine Town Cassette" => TextureAndDisplayer(Plugin.TurbineCassetteSprite.texture, __instance),
                        "Salmon Creek Forest Cassette" => TextureAndDisplayer(Plugin.SalmonCassetteSprite.texture, __instance),
                        "Public Pool Cassette" => TextureAndDisplayer(Plugin.PoolCassetteSprite.texture, __instance),
                        "Bathhouse Cassette" => TextureAndDisplayer(Plugin.BathCassetteSprite.texture, __instance),
                        "Tadpole HQ Cassette" => TextureAndDisplayer(Plugin.TadpoleCassetteSprite.texture, __instance),
                        "Hairball City Seed" => TextureAndDisplayer(Plugin.HairballSeedSprite.texture, __instance),
                        "Salmon Creek Forest Seed" => TextureAndDisplayer(Plugin.SalmonSeedSprite.texture, __instance),
                        "Bathhouse Seed" => TextureAndDisplayer(Plugin.BathSeedSprite.texture, __instance),
                        "Speed Boost" => TextureAndDisplayer(Plugin.SpeedBoostSprite.texture, __instance),
                        "Whoops! Trap" => TextureAndDisplayer(Plugin.WhoopsTrapSprite.texture, __instance),
                        "Iron Boots Trap" => TextureAndDisplayer(Plugin.IronBootsTrapSprite.texture, __instance),
                        "My Turn! Trap" => TextureAndDisplayer(Plugin.MyTurnTrapSprite.texture, __instance),
                        "Freeze Trap" => TextureAndDisplayer(Plugin.FreezeTrapSprite.texture, __instance),
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
                MyCharacterController.instance.state = MyCharacterController.States.Normal;
                MyCharacterController.instance._diveConsumed = false;
            }
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
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
            if (ArchipelagoData.slotData.ContainsKey("cassette_logic"))
                if (int.Parse(ArchipelagoData.slotData["cassette_logic"].ToString()) == 1)
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
                    __instance.txrCassette = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                    {
                        "Coin" => TextureAndDisplayer(Plugin.CoinSprite.texture, __instance, "Coin"),
                        "Cassette" => TextureAndDisplayer(Plugin.CassetteSprite.texture, __instance, "Cassette"), 
                        "Key" => TextureAndDisplayer(Plugin.KeySprite.texture, __instance, "Key"),
                        "Apples" or "25 Apples" => TextureAndDisplayer(Plugin.ApplesSprite.texture, __instance, "Apples"),
                        "Bugs" or "10 Bugs" => TextureAndDisplayer(Plugin.BugSprite.texture, __instance, "Bugs"),
                        "Snail Money" or "1000 Snail Dollar" => TextureAndDisplayer(Plugin.SnailMoneySprite.texture, __instance),
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
                        "Hairball City Flower" => TextureAndDisplayer(Plugin.HairballFlowerSprite.texture, __instance),
                        "Turbine Town Flower" => TextureAndDisplayer(Plugin.TurbineFlowerSprite.texture, __instance),
                        "Salmon Creek Forest Flower" => TextureAndDisplayer(Plugin.SalmonFlowerSprite.texture, __instance),
                        "Public Pool Flower" => TextureAndDisplayer(Plugin.PoolFlowerSprite.texture, __instance),
                        "Bathhouse Flower" => TextureAndDisplayer(Plugin.BathFlowerSprite.texture, __instance),
                        "Tadpole HQ Flower" => TextureAndDisplayer(Plugin.TadpoleFlowerSprite.texture, __instance),
                        "Hairball City Cassette" => TextureAndDisplayer(Plugin.HairballCassetteSprite.texture, __instance),
                        "Turbine Town Cassette" => TextureAndDisplayer(Plugin.TurbineCassetteSprite.texture, __instance),
                        "Salmon Creek Forest Cassette" => TextureAndDisplayer(Plugin.SalmonCassetteSprite.texture, __instance),
                        "Public Pool Cassette" => TextureAndDisplayer(Plugin.PoolCassetteSprite.texture, __instance),
                        "Bathhouse Cassette" => TextureAndDisplayer(Plugin.BathCassetteSprite.texture, __instance),
                        "Tadpole HQ Cassette" => TextureAndDisplayer(Plugin.TadpoleCassetteSprite.texture, __instance),
                        "Hairball City Seed" => TextureAndDisplayer(Plugin.HairballSeedSprite.texture, __instance),
                        "Salmon Creek Forest Seed" => TextureAndDisplayer(Plugin.SalmonSeedSprite.texture, __instance),
                        "Bathhouse Seed" => TextureAndDisplayer(Plugin.BathSeedSprite.texture, __instance),
                        "Speed Boost" => TextureAndDisplayer(Plugin.SpeedBoostSprite.texture, __instance),
                        "Whoops! Trap" => TextureAndDisplayer(Plugin.WhoopsTrapSprite.texture, __instance),
                        "Iron Boots Trap" => TextureAndDisplayer(Plugin.IronBootsTrapSprite.texture, __instance),
                        "My Turn! Trap" => TextureAndDisplayer(Plugin.MyTurnTrapSprite.texture, __instance),
                        "Freeze Trap" => TextureAndDisplayer(Plugin.FreezeTrapSprite.texture, __instance),
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
            if (ArchipelagoData.slotData.ContainsKey("cassette_logic"))
                if (int.Parse(ArchipelagoData.slotData["cassette_logic"].ToString()) == 1)
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
            if (scrWorldSaveDataContainer.instance.cassetteFlags.Contains(__instance.flag)) return;
            audio.Play();
            scrWorldSaveDataContainer.instance.cassetteFlags.Add(__instance.flag);
            scrWorldSaveDataContainer.instance.gameSaveManager.gameData.generalGameData.cassetteAmount++;
            scrWorldSaveDataContainer.instance.SaveWorld();
            scrWorldSaveDataContainer.instance.gameSaveManager.SaveGame();
        }
    }
}