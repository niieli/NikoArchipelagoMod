using System;
using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace NikoArchipelago.Patches;

public class APItemOverworld
{
    [HarmonyPatch(typeof(scrCassette), "Start")]
    public static class CassetteTexturePatch
    {
        private static readonly Dictionary<scrCassette, Dictionary<string, GameObject>> InstanceItemsCache = new();

        private static GameObject CreateItemPrefab(string prefabName, scrCassette instance, float speed = 6f)
        {
            // Ensure a per-instance cache exists
            if (!InstanceItemsCache.ContainsKey(instance))
            {
                InstanceItemsCache[instance] = new Dictionary<string, GameObject>();
            }

            // Check if the instance-specific item already exists
            var instanceCache = InstanceItemsCache[instance];
            if (instanceCache.TryGetValue(prefabName, out var cachedItem))
            {
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogInfo($"Reusing existing item for prefab: {prefabName} on instance {instance.name}");
                return cachedItem;
            }

            if (!GameObjectChecker.CreatedItemsCache.TryGetValue(prefabName, out var blueprintPrefab))
            {
                GameObject prefab = Plugin.AssetBundle.LoadAsset<GameObject>(prefabName);
                if (prefab == null)
                {
                    if (Plugin.DebugMode)
                        Plugin.BepinLogger.LogError($"Prefab '{prefabName}' not found in AssetBundle.");
                    return null;
                }

                GameObjectChecker.CreatedItemsCache[prefabName] = prefab;
                blueprintPrefab = prefab;
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogInfo($"Cached prefab blueprint: {prefabName}");
            }
            
            GameObject itemOverworld = Object.Instantiate(blueprintPrefab, instance.transform, true);
            if (Plugin.DebugMode)
                Plugin.BepinLogger.LogInfo($"Instantiated new item from blueprint: {prefabName}");

            var ogQuads = instance.transform.Find("Quads").gameObject;
            var itemQuads = itemOverworld.transform.Find("Quads").gameObject;

            itemOverworld.transform.localPosition = Vector3.zero;
            itemQuads.transform.position = ogQuads.transform.position;
            
            if (itemQuads.GetComponent<ScuffedSpin>() == null)
            {
                itemQuads.AddComponent<ScuffedSpin>();
            }
            if (itemQuads.GetComponent<scrUIwobble>() == null)
            {
                var wobble = itemQuads.AddComponent<scrUIwobble>();
                wobble.wobbleSpeed = speed;
                wobble.wobbleAngle = 5f;
            }

            // Cache the created item for the specific instance
            instanceCache[prefabName] = itemOverworld;

            return itemOverworld;
        }

        private static GameObject CreateItemOverworld(string itemName, scrCassette instance, float speed = 6f)
        {
            if (!Assets.PrefabMapping.TryGetValue(itemName, out string prefabName))
            {
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogError($"Item name '{itemName}' not recognized.");
                return null;
            }
            
            return CreateItemPrefab(prefabName, instance, speed);
        }
        
        private static void PlaceModel(int index, int offset, scrCassette __instance)
        {
            if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
            {
                if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                    switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                    {
                        case "Time Piece"
                            when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                            __instance.quads = CreateItemOverworld("timepiece", __instance);
                            break;
                        case "Yarn"
                            when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                        {
                            __instance.quads = CreateItemOverworld("yarn", __instance);
                            break;
                        }
                        default:
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                .HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    "apTrap",
                                    "apTrap1",
                                    "apTrap2"
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = CreateItemOverworld(trapTextures[randomIndex], __instance, -20f);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }

                            break;
                        }
                    }
                else
                    __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                    {
                        "Coin" => CreateItemOverworld("coin", __instance),
                        "Cassette" => CreateItemOverworld("cassette", __instance),
                        "Key" => CreateItemOverworld("key", __instance),
                        "Apples" or "25 Apples" => CreateItemOverworld("apples", __instance),
                        "10 Bugs" or "Bugs" => CreateItemOverworld("bugs", __instance),
                        "Letter" => CreateItemOverworld("letter", __instance),
                        "Snail Money" or "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                        "Contact List 1" or "Contact List 2" or "Progressive Contact List" =>
                            CreateItemOverworld("contactList", __instance),
                        "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                        "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                        "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                        "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                        "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                        "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                        "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                        "Super Jump" => CreateItemOverworld("superJump", __instance),
                        "Hairball City Fish" => CreateItemOverworld("hcfish", __instance),
                        "Turbine Town Fish" => CreateItemOverworld("ttfish", __instance),
                        "Salmon Creek Forest Fish" => CreateItemOverworld("scffish", __instance),
                        "Public Pool Fish" => CreateItemOverworld("ppfish", __instance),
                        "Bathhouse Fish" => CreateItemOverworld("bathfish", __instance),
                        "Tadpole HQ Fish" => CreateItemOverworld("hqfish", __instance),
                        "Hairball City Key" => CreateItemOverworld("hckey", __instance),
                        "Turbine Town Key" => CreateItemOverworld("ttkey", __instance),
                        "Salmon Creek Forest Key" => CreateItemOverworld("scfkey", __instance),
                        "Public Pool Key" => CreateItemOverworld("ppkey", __instance),
                        "Bathhouse Key" => CreateItemOverworld("bathkey", __instance),
                        "Tadpole HQ Key" => CreateItemOverworld("hqkey", __instance),
                        "Hairball City Flower" => CreateItemOverworld("hcflower", __instance),
                        "Turbine Town Flower" => CreateItemOverworld("ttflower", __instance),
                        "Salmon Creek Forest Flower" => CreateItemOverworld("scfflower", __instance),
                        "Public Pool Flower" => CreateItemOverworld("ppflower", __instance),
                        "Bathhouse Flower" => CreateItemOverworld("bathflower", __instance),
                        "Tadpole HQ Flower" => CreateItemOverworld("hqflower", __instance),
                        "Hairball City Cassette" => CreateItemOverworld("hccassette", __instance),
                        "Turbine Town Cassette" => CreateItemOverworld("ttcassette", __instance),
                        "Salmon Creek Forest Cassette" => CreateItemOverworld("scfcassette", __instance),
                        "Public Pool Cassette" => CreateItemOverworld("ppcassette", __instance),
                        "Bathhouse Cassette" => CreateItemOverworld("bathcassette", __instance),
                        "Tadpole HQ Cassette" => CreateItemOverworld("hqcassette", __instance),
                        "Gary's Garden Cassette" => CreateItemOverworld("gardencassette", __instance),
                        "Hairball City Seed" => CreateItemOverworld("hcseed", __instance),
                        "Salmon Creek Forest Seed" => CreateItemOverworld("scfseed", __instance),
                        "Bathhouse Seed" => CreateItemOverworld("bathseed", __instance),
                        "Speed Boost" => CreateItemOverworld("speedboost", __instance),
                        "Party Invitation" => CreateItemOverworld("partyTicket", __instance),
                        "Safety Helmet" => CreateItemOverworld("bonkHelmet", __instance),
                        "Bug Net" => CreateItemOverworld("bugNet", __instance),
                        "Soda Repair" => CreateItemOverworld("sodaRepair", __instance),
                        "Parasol Repair" => CreateItemOverworld("parasolRepair", __instance),
                        "Swim Course" => CreateItemOverworld("swimCourse", __instance),
                        "Textbox" => CreateItemOverworld("textbox", __instance),
                        "AC Repair" => CreateItemOverworld("acRepair", __instance),
                        "Apple Basket" => CreateItemOverworld("applebasket", __instance),
                        "Hairball City Bone" => CreateItemOverworld("hcbone", __instance),
                        "Turbine Town Bone" => CreateItemOverworld("ttbone", __instance),
                        "Salmon Creek Forest Bone" => CreateItemOverworld("scfbone", __instance),
                        "Public Pool Bone" => CreateItemOverworld("ppbone", __instance),
                        "Bathhouse Bone" => CreateItemOverworld("bathbone", __instance),
                        "Tadpole HQ Bone" => CreateItemOverworld("hqbone", __instance),
                        _ when ArchipelagoClient.ScoutedLocations[index + offset].ItemName.EndsWith("Trap") => 
                            CreateItemOverworld(Assets.RandomProgTrap(), __instance, -20f),
                        _ => CreateItemOverworld("apProg", __instance),
                    };
            }
        }
        
        private static void Postfix(scrCassette __instance)
        {
            if (!ArchipelagoMenu.cacmi) return;
            var flagField = AccessTools.Field(typeof(scrCassette), "flag");
            var flag = (string)flagField.GetValue(__instance);
            if (scrWorldSaveDataContainer.instance.cassetteFlags.Contains(flag) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                return;
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
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            Object.Destroy(ogQuads.gameObject);

            

            var index = 0;
            var offset = 0;
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    var list = Locations.ScoutHCCassetteList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 101 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Trash Kingdom":
                {
                    var list = Locations.ScoutTTCassetteList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 111 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSFCCassetteList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 121 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Public Pool":
                {
                    var list = Locations.ScoutPPCassetteList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 132 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathCassetteList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 142 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutHQCassetteList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 152 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "GarysGarden":
                {
                    var list = Locations.ScoutGardenCassetteList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 162;
                    PlaceModel(index, offset, __instance);
                    break;
                }
            }
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Index: {index}, Offset: {offset}, Flag: {flag}")
                .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
                .AppendLine($"Model: {__instance.quads.name}");
        }
    }

    [HarmonyPatch(typeof(scrCoin), "Start")]
    public static class CoinTexturePatch
    {
        private static readonly Dictionary<scrCoin, Dictionary<string, GameObject>> InstanceItemsCache = new();

        private static GameObject CreateItemPrefab(string prefabName, scrCoin instance, float speed = 6f)
        {
            // Ensure per-instance cache exists
            if (!InstanceItemsCache.ContainsKey(instance))
            {
                InstanceItemsCache[instance] = new Dictionary<string, GameObject>();
            }

            // Check if the instance-specific item already exists
            var instanceCache = InstanceItemsCache[instance];
            if (instanceCache.TryGetValue(prefabName, out var cachedItem))
            {
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogInfo($"Reusing existing item for prefab: {prefabName} on instance {instance.name}");
                return cachedItem;
            }

            if (!GameObjectChecker.CreatedItemsCache.TryGetValue(prefabName, out var blueprintPrefab))
            {
                GameObject prefab = Plugin.AssetBundle.LoadAsset<GameObject>(prefabName);
                if (prefab == null)
                {
                    if (Plugin.DebugMode)
                        Plugin.BepinLogger.LogError($"Prefab '{prefabName}' not found in AssetBundle.");
                    return null;
                }

                GameObjectChecker.CreatedItemsCache[prefabName] = prefab;
                blueprintPrefab = prefab;
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogInfo($"Cached prefab blueprint: {prefabName}");
            }
            
            GameObject itemOverworld = Object.Instantiate(blueprintPrefab, instance.transform, true);
            if (Plugin.DebugMode)
                Plugin.BepinLogger.LogInfo($"Instantiated new item from blueprint: {prefabName}");

            var ogQuads = instance.transform.Find("Quads").gameObject;
            var itemQuads = itemOverworld.transform.Find("Quads").gameObject;

            itemOverworld.transform.localPosition = Vector3.zero;
            itemQuads.transform.position = ogQuads.transform.position;
            
            if (itemQuads.GetComponent<ScuffedSpin>() == null)
            {
                itemQuads.AddComponent<ScuffedSpin>();
            }
            if (itemQuads.GetComponent<scrUIwobble>() == null)
            {
                var wobble = itemQuads.AddComponent<scrUIwobble>();
                wobble.wobbleSpeed = speed;
                wobble.wobbleAngle = 5f;
            }

            // Cache the created item for the specific instance
            instanceCache[prefabName] = itemOverworld;

            return itemOverworld;
        }

        private static GameObject CreateItemOverworld(string itemName, scrCoin instance, float speed = 6f)
        {
            if (!Assets.PrefabMapping.TryGetValue(itemName, out string prefabName))
            {
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogError($"Item name '{itemName}' not recognized.");
                return null;
            }
            
            return CreateItemPrefab(prefabName, instance, speed);
        }
        
        private static void PlaceModel(int index, int offset, scrCoin __instance)
        {
            if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
            {
                if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                    switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                    {
                        case "Time Piece"
                            when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                            __instance.quads = CreateItemOverworld("timepiece", __instance);
                            break;
                        case "Yarn"
                            when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                        {
                            __instance.quads = CreateItemOverworld("yarn", __instance);
                            break;
                        }
                        default:
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                .HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    "apTrap",
                                    "apTrap1",
                                    "apTrap2"
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = CreateItemOverworld(trapTextures[randomIndex], __instance, -20f);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }

                            break;
                        }
                    }
                else
                    __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                    {
                        "Coin" => CreateItemOverworld("coin", __instance),
                        "Cassette" => CreateItemOverworld("cassette", __instance),
                        "Key" => CreateItemOverworld("key", __instance),
                        "Apples" or "25 Apples" => CreateItemOverworld("apples", __instance),
                        "10 Bugs" or "Bugs" => CreateItemOverworld("bugs", __instance),
                        "Letter" => CreateItemOverworld("letter", __instance),
                        "Snail Money" or "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                        "Contact List 1" or "Contact List 2" or "Progressive Contact List" =>
                            CreateItemOverworld("contactList", __instance),
                        "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                        "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                        "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                        "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                        "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                        "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                        "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                        "Super Jump" => CreateItemOverworld("superJump", __instance),
                        "Hairball City Fish" => CreateItemOverworld("hcfish", __instance),
                        "Turbine Town Fish" => CreateItemOverworld("ttfish", __instance),
                        "Salmon Creek Forest Fish" => CreateItemOverworld("scffish", __instance),
                        "Public Pool Fish" => CreateItemOverworld("ppfish", __instance),
                        "Bathhouse Fish" => CreateItemOverworld("bathfish", __instance),
                        "Tadpole HQ Fish" => CreateItemOverworld("hqfish", __instance),
                        "Hairball City Key" => CreateItemOverworld("hckey", __instance),
                        "Turbine Town Key" => CreateItemOverworld("ttkey", __instance),
                        "Salmon Creek Forest Key" => CreateItemOverworld("scfkey", __instance),
                        "Public Pool Key" => CreateItemOverworld("ppkey", __instance),
                        "Bathhouse Key" => CreateItemOverworld("bathkey", __instance),
                        "Tadpole HQ Key" => CreateItemOverworld("hqkey", __instance),
                        "Hairball City Flower" => CreateItemOverworld("hcflower", __instance),
                        "Turbine Town Flower" => CreateItemOverworld("ttflower", __instance),
                        "Salmon Creek Forest Flower" => CreateItemOverworld("scfflower", __instance),
                        "Public Pool Flower" => CreateItemOverworld("ppflower", __instance),
                        "Bathhouse Flower" => CreateItemOverworld("bathflower", __instance),
                        "Tadpole HQ Flower" => CreateItemOverworld("hqflower", __instance),
                        "Hairball City Cassette" => CreateItemOverworld("hccassette", __instance),
                        "Turbine Town Cassette" => CreateItemOverworld("ttcassette", __instance),
                        "Salmon Creek Forest Cassette" => CreateItemOverworld("scfcassette", __instance),
                        "Public Pool Cassette" => CreateItemOverworld("ppcassette", __instance),
                        "Bathhouse Cassette" => CreateItemOverworld("bathcassette", __instance),
                        "Tadpole HQ Cassette" => CreateItemOverworld("hqcassette", __instance),
                        "Gary's Garden Cassette" => CreateItemOverworld("gardencassette", __instance),
                        "Hairball City Seed" => CreateItemOverworld("hcseed", __instance),
                        "Salmon Creek Forest Seed" => CreateItemOverworld("scfseed", __instance),
                        "Bathhouse Seed" => CreateItemOverworld("bathseed", __instance),
                        "Speed Boost" => CreateItemOverworld("speedboost", __instance),
                        "Party Invitation" => CreateItemOverworld("partyTicket", __instance),
                        "Safety Helmet" => CreateItemOverworld("bonkHelmet", __instance),
                        "Bug Net" => CreateItemOverworld("bugNet", __instance),
                        "Soda Repair" => CreateItemOverworld("sodaRepair", __instance),
                        "Parasol Repair" => CreateItemOverworld("parasolRepair", __instance),
                        "Swim Course" => CreateItemOverworld("swimCourse", __instance),
                        "Textbox" => CreateItemOverworld("textbox", __instance),
                        "AC Repair" => CreateItemOverworld("acRepair", __instance),
                        "Apple Basket" => CreateItemOverworld("applebasket", __instance),
                        "Hairball City Bone" => CreateItemOverworld("hcbone", __instance),
                        "Turbine Town Bone" => CreateItemOverworld("ttbone", __instance),
                        "Salmon Creek Forest Bone" => CreateItemOverworld("scfbone", __instance),
                        "Public Pool Bone" => CreateItemOverworld("ppbone", __instance),
                        "Bathhouse Bone" => CreateItemOverworld("bathbone", __instance),
                        "Tadpole HQ Bone" => CreateItemOverworld("hqbone", __instance),
                        _ when ArchipelagoClient.ScoutedLocations[index + offset].ItemName.EndsWith("Trap") => 
                            CreateItemOverworld(Assets.RandomProgTrap(), __instance, -20f),
                        _ => CreateItemOverworld("apProg", __instance),
                    };
            }
        }
        
        private static void Postfix(scrCoin __instance)
        {
            if (!ArchipelagoMenu.cacmi) return;
            if (scrWorldSaveDataContainer.instance.coinFlags.Contains(__instance.myFlag) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                return;
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
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            ogQuads.SetActive(false);

            var index = 0;
            var offset = 0;
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    var list = Locations.ScoutHCCoinList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    offset = 36 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Trash Kingdom":
                {
                    var list = Locations.ScoutTTCoinList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    offset = 49 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSFCCoinList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    offset = 58 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Public Pool":
                {
                    var list = Locations.ScoutPPCoinList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    offset = 72 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathCoinList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    offset = 80 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutHQCoinList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    offset = 92 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
            }
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Index: {index}, Offset: {offset}, Flag: {__instance.myFlag}")
                .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
                .AppendLine($"Model: {__instance.quads.name}");
        }
    }

    [HarmonyPatch(typeof(scrKey), "Start")]
    public static class KeyTexturePatch
    {
        private static readonly Dictionary<scrKey, Dictionary<string, GameObject>> InstanceItemsCache = new();

        private static GameObject CreateItemPrefab(string prefabName, scrKey instance, float speed = 6f)
        {
            // Ensure per-instance cache exists
            if (!InstanceItemsCache.ContainsKey(instance))
            {
                InstanceItemsCache[instance] = new Dictionary<string, GameObject>();
            }

            // Check if the instance-specific item already exists
            var instanceCache = InstanceItemsCache[instance];
            if (instanceCache.TryGetValue(prefabName, out var cachedItem))
            {
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogInfo($"Reusing existing item for prefab: {prefabName} on instance {instance.name}");
                return cachedItem;
            }

            if (!GameObjectChecker.CreatedItemsCache.TryGetValue(prefabName, out var blueprintPrefab))
            {
                GameObject prefab = Plugin.AssetBundle.LoadAsset<GameObject>(prefabName);
                if (prefab == null)
                {
                    if (Plugin.DebugMode)
                        Plugin.BepinLogger.LogError($"Prefab '{prefabName}' not found in AssetBundle.");
                    return null;
                }

                GameObjectChecker.CreatedItemsCache[prefabName] = prefab;
                blueprintPrefab = prefab;
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogInfo($"Cached prefab blueprint: {prefabName}");
            }

            GameObject itemOverworld = Object.Instantiate(blueprintPrefab, instance.transform, true);
            if (Plugin.DebugMode)
                Plugin.BepinLogger.LogInfo($"Instantiated new item from blueprint: {prefabName}");

            var ogQuads = instance.transform.Find("Quads").gameObject;
            var itemQuads = itemOverworld.transform.Find("Quads").gameObject;

            itemOverworld.transform.localPosition = Vector3.zero;
            itemQuads.transform.position = ogQuads.transform.position;

            if (itemQuads.GetComponent<ScuffedSpin>() == null)
            {
                itemQuads.AddComponent<ScuffedSpin>();
            }

            if (itemQuads.GetComponent<scrUIwobble>() == null)
            {
                var wobble = itemQuads.AddComponent<scrUIwobble>();
                wobble.wobbleSpeed = speed;
                wobble.wobbleAngle = 5f;
            }

            // Cache the created item for the specific instance
            instanceCache[prefabName] = itemOverworld;

            return itemOverworld;
        }

        private static GameObject CreateItemOverworld(string itemName, scrKey instance, float speed = 6f)
        {
            if (!Assets.PrefabMapping.TryGetValue(itemName, out string prefabName))
            {
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogError($"Item name '{itemName}' not recognized.");
                return null;
            }

            return CreateItemPrefab(prefabName, instance, speed);
        }
        
        private static void PlaceModel(int index, int offset, scrKey __instance)
        {
            if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
            {
                if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                    switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                    {
                        case "Time Piece"
                            when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                            __instance.quads = CreateItemOverworld("timepiece", __instance);
                            break;
                        case "Yarn"
                            when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                        {
                            __instance.quads = CreateItemOverworld("yarn", __instance);
                            break;
                        }
                        default:
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                .HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    "apTrap",
                                    "apTrap1",
                                    "apTrap2"
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = CreateItemOverworld(trapTextures[randomIndex], __instance, -20f);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }

                            break;
                        }
                    }
                else
                    __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                    {
                        "Coin" => CreateItemOverworld("coin", __instance),
                        "Cassette" => CreateItemOverworld("cassette", __instance),
                        "Key" => CreateItemOverworld("key", __instance),
                        "Apples" or "25 Apples" => CreateItemOverworld("apples", __instance),
                        "10 Bugs" or "Bugs" => CreateItemOverworld("bugs", __instance),
                        "Letter" => CreateItemOverworld("letter", __instance),
                        "Snail Money" or "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                        "Contact List 1" or "Contact List 2" or "Progressive Contact List" =>
                            CreateItemOverworld("contactList", __instance),
                        "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                        "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                        "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                        "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                        "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                        "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                        "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                        "Super Jump" => CreateItemOverworld("superJump", __instance),
                        "Hairball City Fish" => CreateItemOverworld("hcfish", __instance),
                        "Turbine Town Fish" => CreateItemOverworld("ttfish", __instance),
                        "Salmon Creek Forest Fish" => CreateItemOverworld("scffish", __instance),
                        "Public Pool Fish" => CreateItemOverworld("ppfish", __instance),
                        "Bathhouse Fish" => CreateItemOverworld("bathfish", __instance),
                        "Tadpole HQ Fish" => CreateItemOverworld("hqfish", __instance),
                        "Hairball City Key" => CreateItemOverworld("hckey", __instance),
                        "Turbine Town Key" => CreateItemOverworld("ttkey", __instance),
                        "Salmon Creek Forest Key" => CreateItemOverworld("scfkey", __instance),
                        "Public Pool Key" => CreateItemOverworld("ppkey", __instance),
                        "Bathhouse Key" => CreateItemOverworld("bathkey", __instance),
                        "Tadpole HQ Key" => CreateItemOverworld("hqkey", __instance),
                        "Hairball City Flower" => CreateItemOverworld("hcflower", __instance),
                        "Turbine Town Flower" => CreateItemOverworld("ttflower", __instance),
                        "Salmon Creek Forest Flower" => CreateItemOverworld("scfflower", __instance),
                        "Public Pool Flower" => CreateItemOverworld("ppflower", __instance),
                        "Bathhouse Flower" => CreateItemOverworld("bathflower", __instance),
                        "Tadpole HQ Flower" => CreateItemOverworld("hqflower", __instance),
                        "Hairball City Cassette" => CreateItemOverworld("hccassette", __instance),
                        "Turbine Town Cassette" => CreateItemOverworld("ttcassette", __instance),
                        "Salmon Creek Forest Cassette" => CreateItemOverworld("scfcassette", __instance),
                        "Public Pool Cassette" => CreateItemOverworld("ppcassette", __instance),
                        "Bathhouse Cassette" => CreateItemOverworld("bathcassette", __instance),
                        "Tadpole HQ Cassette" => CreateItemOverworld("hqcassette", __instance),
                        "Gary's Garden Cassette" => CreateItemOverworld("gardencassette", __instance),
                        "Hairball City Seed" => CreateItemOverworld("hcseed", __instance),
                        "Salmon Creek Forest Seed" => CreateItemOverworld("scfseed", __instance),
                        "Bathhouse Seed" => CreateItemOverworld("bathseed", __instance),
                        "Speed Boost" => CreateItemOverworld("speedboost", __instance),
                        "Party Invitation" => CreateItemOverworld("partyTicket", __instance),
                        "Safety Helmet" => CreateItemOverworld("bonkHelmet", __instance),
                        "Bug Net" => CreateItemOverworld("bugNet", __instance),
                        "Soda Repair" => CreateItemOverworld("sodaRepair", __instance),
                        "Parasol Repair" => CreateItemOverworld("parasolRepair", __instance),
                        "Swim Course" => CreateItemOverworld("swimCourse", __instance),
                        "Textbox" => CreateItemOverworld("textbox", __instance),
                        "AC Repair" => CreateItemOverworld("acRepair", __instance),
                        "Apple Basket" => CreateItemOverworld("applebasket", __instance),
                        "Hairball City Bone" => CreateItemOverworld("hcbone", __instance),
                        "Turbine Town Bone" => CreateItemOverworld("ttbone", __instance),
                        "Salmon Creek Forest Bone" => CreateItemOverworld("scfbone", __instance),
                        "Public Pool Bone" => CreateItemOverworld("ppbone", __instance),
                        "Bathhouse Bone" => CreateItemOverworld("bathbone", __instance),
                        "Tadpole HQ Bone" => CreateItemOverworld("hqbone", __instance),
                        _ when ArchipelagoClient.ScoutedLocations[index + offset].ItemName.EndsWith("Trap") => 
                            CreateItemOverworld(Assets.RandomProgTrap(), __instance, -20f),
                        _ => CreateItemOverworld("apProg", __instance),
                    };
            }
        }

        private static void Postfix(scrKey __instance)
        {
            if (!ArchipelagoMenu.kalmi) return;
            if (scrWorldSaveDataContainer.instance.miscFlags.Contains(__instance.flag) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                return;
            }
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
            {
                if (int.Parse(ArchipelagoData.slotData["shuffle_garden"].ToString()) == 0)
                {
                    gardenAdjustment = 13;
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
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            ogQuads.SetActive(false);

            var list = Locations.ScoutKeyList.ToList();
            var index = list.FindIndex(pair => pair.Value == __instance.flag);
            int offset = 172 - adjustment;
            PlaceModel(index, offset, __instance);
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Index: {index}, Offset: {offset}, Flag: {__instance.flag}")
                .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
                .AppendLine($"Model: {__instance.quads.name}");
        }
    }
    
    [HarmonyPatch(typeof(scrEnvelope), "Start")]
    public static class LetterTexturePatch
    {
        private static readonly Dictionary<scrEnvelope, Dictionary<string, GameObject>> InstanceItemsCache = new();

        private static GameObject CreateItemPrefab(string prefabName, scrEnvelope instance, float speed = 6f)
        {
            // Ensure per-instance cache exists
            if (!InstanceItemsCache.ContainsKey(instance))
            {
                InstanceItemsCache[instance] = new Dictionary<string, GameObject>();
            }

            // Check if the instance-specific item already exists
            var instanceCache = InstanceItemsCache[instance];
            if (instanceCache.TryGetValue(prefabName, out var cachedItem))
            {
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogInfo($"Reusing existing item for prefab: {prefabName} on instance {instance.name}");
                return cachedItem;
            }

            if (!GameObjectChecker.CreatedItemsCache.TryGetValue(prefabName, out var blueprintPrefab))
            {
                GameObject prefab = Plugin.AssetBundle.LoadAsset<GameObject>(prefabName);
                if (prefab == null)
                {
                    if (Plugin.DebugMode)
                        Plugin.BepinLogger.LogError($"Prefab '{prefabName}' not found in AssetBundle.");
                    return null;
                }

                GameObjectChecker.CreatedItemsCache[prefabName] = prefab;
                blueprintPrefab = prefab;
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogInfo($"Cached prefab blueprint: {prefabName}");
            }

            GameObject itemOverworld = Object.Instantiate(blueprintPrefab, instance.transform, true);
            if (Plugin.DebugMode)
                Plugin.BepinLogger.LogInfo($"Instantiated new item from blueprint: {prefabName}");

            var ogQuads = instance.transform.Find("Quads").gameObject;
            var itemQuads = itemOverworld.transform.Find("Quads").gameObject;

            itemOverworld.transform.localPosition = Vector3.zero;
            itemQuads.transform.position = ogQuads.transform.position;

            if (itemQuads.GetComponent<ScuffedSpin>() == null)
            {
                itemQuads.AddComponent<ScuffedSpin>();
            }

            if (itemQuads.GetComponent<scrUIwobble>() == null)
            {
                var wobble = itemQuads.AddComponent<scrUIwobble>();
                wobble.wobbleSpeed = speed;
                wobble.wobbleAngle = 5f;
            }

            // Cache the created item for the specific instance
            instanceCache[prefabName] = itemOverworld;

            return itemOverworld;
        }

        private static GameObject CreateItemOverworld(string itemName, scrEnvelope instance, float speed = 6f)
        {
            if (!Assets.PrefabMapping.TryGetValue(itemName, out string prefabName))
            {
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogError($"Item name '{itemName}' not recognized.");
                return null;
            }

            return CreateItemPrefab(prefabName, instance, speed);
        }
        
        private static void PlaceModel(int index, int offset, scrEnvelope __instance)
        {
            if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
            {
                if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                    switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                    {
                        case "Time Piece"
                            when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                            __instance.quads = CreateItemOverworld("timepiece", __instance);
                            break;
                        case "Yarn"
                            when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                        {
                            __instance.quads = CreateItemOverworld("yarn", __instance);
                            break;
                        }
                        default:
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                .HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    "apTrap",
                                    "apTrap1",
                                    "apTrap2"
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = CreateItemOverworld(trapTextures[randomIndex], __instance, -20f);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }

                            break;
                        }
                    }
                else
                    __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                    {
                        "Coin" => CreateItemOverworld("coin", __instance),
                        "Cassette" => CreateItemOverworld("cassette", __instance),
                        "Key" => CreateItemOverworld("key", __instance),
                        "Apples" or "25 Apples" => CreateItemOverworld("apples", __instance),
                        "10 Bugs" or "Bugs" => CreateItemOverworld("bugs", __instance),
                        "Letter" => CreateItemOverworld("letter", __instance),
                        "Snail Money" or "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                        "Contact List 1" or "Contact List 2" or "Progressive Contact List" =>
                            CreateItemOverworld("contactList", __instance),
                        "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                        "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                        "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                        "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                        "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                        "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                        "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                        "Super Jump" => CreateItemOverworld("superJump", __instance),
                        "Hairball City Fish" => CreateItemOverworld("hcfish", __instance),
                        "Turbine Town Fish" => CreateItemOverworld("ttfish", __instance),
                        "Salmon Creek Forest Fish" => CreateItemOverworld("scffish", __instance),
                        "Public Pool Fish" => CreateItemOverworld("ppfish", __instance),
                        "Bathhouse Fish" => CreateItemOverworld("bathfish", __instance),
                        "Tadpole HQ Fish" => CreateItemOverworld("hqfish", __instance),
                        "Hairball City Key" => CreateItemOverworld("hckey", __instance),
                        "Turbine Town Key" => CreateItemOverworld("ttkey", __instance),
                        "Salmon Creek Forest Key" => CreateItemOverworld("scfkey", __instance),
                        "Public Pool Key" => CreateItemOverworld("ppkey", __instance),
                        "Bathhouse Key" => CreateItemOverworld("bathkey", __instance),
                        "Tadpole HQ Key" => CreateItemOverworld("hqkey", __instance),
                        "Hairball City Flower" => CreateItemOverworld("hcflower", __instance),
                        "Turbine Town Flower" => CreateItemOverworld("ttflower", __instance),
                        "Salmon Creek Forest Flower" => CreateItemOverworld("scfflower", __instance),
                        "Public Pool Flower" => CreateItemOverworld("ppflower", __instance),
                        "Bathhouse Flower" => CreateItemOverworld("bathflower", __instance),
                        "Tadpole HQ Flower" => CreateItemOverworld("hqflower", __instance),
                        "Hairball City Cassette" => CreateItemOverworld("hccassette", __instance),
                        "Turbine Town Cassette" => CreateItemOverworld("ttcassette", __instance),
                        "Salmon Creek Forest Cassette" => CreateItemOverworld("scfcassette", __instance),
                        "Public Pool Cassette" => CreateItemOverworld("ppcassette", __instance),
                        "Bathhouse Cassette" => CreateItemOverworld("bathcassette", __instance),
                        "Tadpole HQ Cassette" => CreateItemOverworld("hqcassette", __instance),
                        "Gary's Garden Cassette" => CreateItemOverworld("gardencassette", __instance),
                        "Hairball City Seed" => CreateItemOverworld("hcseed", __instance),
                        "Salmon Creek Forest Seed" => CreateItemOverworld("scfseed", __instance),
                        "Bathhouse Seed" => CreateItemOverworld("bathseed", __instance),
                        "Speed Boost" => CreateItemOverworld("speedboost", __instance),
                        "Party Invitation" => CreateItemOverworld("partyTicket", __instance),
                        "Safety Helmet" => CreateItemOverworld("bonkHelmet", __instance),
                        "Bug Net" => CreateItemOverworld("bugNet", __instance),
                        "Soda Repair" => CreateItemOverworld("sodaRepair", __instance),
                        "Parasol Repair" => CreateItemOverworld("parasolRepair", __instance),
                        "Swim Course" => CreateItemOverworld("swimCourse", __instance),
                        "Textbox" => CreateItemOverworld("textbox", __instance),
                        "AC Repair" => CreateItemOverworld("acRepair", __instance),
                        "Apple Basket" => CreateItemOverworld("applebasket", __instance),
                        "Hairball City Bone" => CreateItemOverworld("hcbone", __instance),
                        "Turbine Town Bone" => CreateItemOverworld("ttbone", __instance),
                        "Salmon Creek Forest Bone" => CreateItemOverworld("scfbone", __instance),
                        "Public Pool Bone" => CreateItemOverworld("ppbone", __instance),
                        "Bathhouse Bone" => CreateItemOverworld("bathbone", __instance),
                        "Tadpole HQ Bone" => CreateItemOverworld("hqbone", __instance),
                        _ when ArchipelagoClient.ScoutedLocations[index + offset].ItemName.EndsWith("Trap") => 
                            CreateItemOverworld(Assets.RandomProgTrap(), __instance, -20f),
                        _ => CreateItemOverworld("apProg", __instance),
                    };
            }
        }

        private static void Postfix(scrEnvelope __instance)
        {
            if (!ArchipelagoMenu.kalmi) return;
            if (scrWorldSaveDataContainer.instance.miscFlags.Contains(__instance.myLetter.key) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                return;
            }
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
            {
                if (int.Parse(ArchipelagoData.slotData["shuffle_garden"].ToString()) == 0)
                {
                    gardenAdjustment = 13;
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
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            ogQuads.SetActive(false);
            try
            {
                var list = Locations.ScoutLetterList.ToList();
                var index = list.FindIndex(pair => pair.Value == __instance.myLetter.key);
                int offset = 181 - adjustment;
                PlaceModel(index, offset, __instance);
                GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
                GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                    .AppendLine($"Index: {index}, Offset: {offset}, Flag: {__instance.myLetter.key}")
                    .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                    .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                    .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
                    .AppendLine($"Model: {__instance.quads.name}");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Plugin.BepinLogger.LogWarning($"Letter not found! Probably using an apworld pre-0.5.0, where this letter does not exist." +
                                              $"\nIf you are on 0.5.0+ and this keeps popping up please report it in the discord");
            }
            
        }
    }
    
    [HarmonyPatch(typeof(scrList), "Start")]
    public static class ContactListTexturePatch
    {
        private static readonly Dictionary<scrList, Dictionary<string, GameObject>> InstanceItemsCache = new();

        private static GameObject CreateItemPrefab(string prefabName, scrList instance, float speed = 6f)
        {
            // Ensure per-instance cache exists
            if (!InstanceItemsCache.ContainsKey(instance))
            {
                InstanceItemsCache[instance] = new Dictionary<string, GameObject>();
            }

            // Check if the instance-specific item already exists
            var instanceCache = InstanceItemsCache[instance];
            if (instanceCache.TryGetValue(prefabName, out var cachedItem))
            {
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogInfo($"Reusing existing item for prefab: {prefabName} on instance {instance.name}");
                return cachedItem;
            }

            if (!GameObjectChecker.CreatedItemsCache.TryGetValue(prefabName, out var blueprintPrefab))
            {
                GameObject prefab = Plugin.AssetBundle.LoadAsset<GameObject>(prefabName);
                if (prefab == null)
                {
                    if (Plugin.DebugMode)
                        Plugin.BepinLogger.LogError($"Prefab '{prefabName}' not found in AssetBundle.");
                    return null;
                }

                GameObjectChecker.CreatedItemsCache[prefabName] = prefab;
                blueprintPrefab = prefab;
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogInfo($"Cached prefab blueprint: {prefabName}");
            }

            GameObject itemOverworld = Object.Instantiate(blueprintPrefab, instance.transform, true);
            if (Plugin.DebugMode)
                Plugin.BepinLogger.LogInfo($"Instantiated new item from blueprint: {prefabName}");

            var ogQuads = instance.transform.Find("Quads").gameObject;
            var itemQuads = itemOverworld.transform.Find("Quads").gameObject;

            itemOverworld.transform.localPosition = Vector3.zero;
            itemQuads.transform.position = ogQuads.transform.position;

            if (itemQuads.GetComponent<ScuffedSpin>() == null)
            {
                itemQuads.AddComponent<ScuffedSpin>();
            }

            if (itemQuads.GetComponent<scrUIwobble>() == null)
            {
                var wobble = itemQuads.AddComponent<scrUIwobble>();
                wobble.wobbleSpeed = speed;
                wobble.wobbleAngle = 5f;
            }

            // Cache the created item for the specific instance
            instanceCache[prefabName] = itemOverworld;

            return itemOverworld;
        }

        private static GameObject CreateItemOverworld(string itemName, scrList instance, float speed = 6f)
        {
            if (!Assets.PrefabMapping.TryGetValue(itemName, out string prefabName))
            {
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogError($"Item name '{itemName}' not recognized.");
                return null;
            }

            return CreateItemPrefab(prefabName, instance, speed);
        }

        private static void Postfix(scrList __instance)
        {
            if (!ArchipelagoMenu.kalmi) return;
            if (GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                return;
            }
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
            {
                if (int.Parse(ArchipelagoData.slotData["shuffle_garden"].ToString()) == 0)
                {
                    gardenAdjustment = 13;
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
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            ogQuads.SetActive(false);
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutContactList.ToList();
                    var index = list.FindIndex(pair => pair.Value == "CL1 Obtained");
                    var offset = 193 - adjustment;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece"
                                    when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.quads = CreateItemOverworld("timepiece", __instance);
                                    break;
                                case "Yarn"
                                    when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                {
                                    __instance.quads = CreateItemOverworld("yarn", __instance);
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                        .HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.quads = CreateItemOverworld("apProg", __instance);
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.quads = CreateItemOverworld("apUseful", __instance);
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.Trap))
                                    {
                                        var trapTextures = new[]
                                        {
                                            CreateItemOverworld("apTrap", __instance),
                                            CreateItemOverworld("apTrap1", __instance),
                                            CreateItemOverworld("apTrap2", __instance)
                                        };
                                        var randomIndex = Random.Range(0, trapTextures.Length);
                                        __instance.quads = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.None))
                                    {
                                        __instance.quads = CreateItemOverworld("apFiller", __instance);
                                    }

                                    break;
                                }
                            }
                        else
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "Apples" or "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" or "Bugs" => CreateItemOverworld("bugs", __instance),
                                "Letter" => CreateItemOverworld("letter", __instance),
                                "Snail Money" or "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" =>
                                    CreateItemOverworld("contactList", __instance),
                                "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                                "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                                "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                                "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                                "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                                "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                                "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                                "Super Jump" => CreateItemOverworld("superJump", __instance),
                                "Hairball City Fish" => CreateItemOverworld("hcfish", __instance),
                                "Turbine Town Fish" => CreateItemOverworld("ttfish", __instance),
                                "Salmon Creek Forest Fish" => CreateItemOverworld("scffish", __instance),
                                "Public Pool Fish" => CreateItemOverworld("ppfish", __instance),
                                "Bathhouse Fish" => CreateItemOverworld("bathfish", __instance),
                                "Tadpole HQ Fish" => CreateItemOverworld("hqfish", __instance),
                                "Hairball City Key" => CreateItemOverworld("hckey", __instance),
                                "Turbine Town Key" => CreateItemOverworld("ttkey", __instance),
                                "Salmon Creek Forest Key" => CreateItemOverworld("scfkey", __instance),
                                "Public Pool Key" => CreateItemOverworld("ppkey", __instance),
                                "Bathhouse Key" => CreateItemOverworld("bathkey", __instance),
                                "Hairball City Flower" => CreateItemOverworld("hcflower", __instance),
                                "Turbine Town Flower" => CreateItemOverworld("ttflower", __instance),
                                "Salmon Creek Forest Flower" => CreateItemOverworld("scfflower", __instance),
                                "Public Pool Flower" => CreateItemOverworld("ppflower", __instance),
                                "Bathhouse Flower" => CreateItemOverworld("bathflower", __instance),
                                "Tadpole HQ Flower" => CreateItemOverworld("hqflower", __instance),
                                "Hairball City Cassette" => CreateItemOverworld("hccassette", __instance),
                                "Turbine Town Cassette" => CreateItemOverworld("ttcassette", __instance),
                                "Salmon Creek Forest Cassette" => CreateItemOverworld("scfcassette", __instance),
                                "Public Pool Cassette" => CreateItemOverworld("ppcassette", __instance),
                                "Bathhouse Cassette" => CreateItemOverworld("bathcassette", __instance),
                                "Tadpole HQ Cassette" => CreateItemOverworld("hqcassette", __instance),
                                "Gary's Garden Cassette" => CreateItemOverworld("gardencassette", __instance),
                                "Hairball City Seed" => CreateItemOverworld("hcseed", __instance),
                                "Salmon Creek Forest Seed" => CreateItemOverworld("scfseed", __instance),
                                "Bathhouse Seed" => CreateItemOverworld("bathseed", __instance),
                                "Speed Boost" => CreateItemOverworld("speedboost", __instance),
                                "Party Invitation" => CreateItemOverworld("partyTicket", __instance),
                                "Safety Helmet" => CreateItemOverworld("bonkHelmet", __instance),
                                "Bug Net" => CreateItemOverworld("bugNet", __instance),
                                "Soda Repair" => CreateItemOverworld("sodaRepair", __instance),
                                "Parasol Repair" => CreateItemOverworld("parasolRepair", __instance),
                                "Swim Course" => CreateItemOverworld("swimCourse", __instance),
                                "Textbox" => CreateItemOverworld("textbox", __instance),
                                "AC Repair" => CreateItemOverworld("acRepair", __instance),
                                _ when ArchipelagoClient.ScoutedLocations[index + offset].ItemName.EndsWith("Trap") => 
                                    CreateItemOverworld(Assets.RandomProgTrap(), __instance, -20f),
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                    }
                    GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
                    GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                        .AppendLine($"Index: {index}, Offset: {offset}, Flag: CL1 Obtained")
                        .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                        .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                        .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
                        .AppendLine($"Model: {__instance.quads.name}");
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutContactList.ToList();
                    var index = list.FindIndex(pair => pair.Value == "CL2 Obtained");
                    var offset = 193 - adjustment;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece"
                                    when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.quads = CreateItemOverworld("timepiece", __instance);
                                    break;
                                case "Yarn"
                                    when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                {
                                    __instance.quads = CreateItemOverworld("yarn", __instance);
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                        .HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.quads = CreateItemOverworld("apProg", __instance);
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.quads = CreateItemOverworld("apUseful", __instance);
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.Trap))
                                    {
                                        var trapTextures = new[]
                                        {
                                            CreateItemOverworld("apTrap", __instance),
                                            CreateItemOverworld("apTrap1", __instance),
                                            CreateItemOverworld("apTrap2", __instance)
                                        };
                                        var randomIndex = Random.Range(0, trapTextures.Length);
                                        __instance.quads = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.None))
                                    {
                                        __instance.quads = CreateItemOverworld("apFiller", __instance);
                                    }

                                    break;
                                }
                            }
                        else
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "Apples" or "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" or "Bugs" => CreateItemOverworld("bugs", __instance),
                                "Letter" => CreateItemOverworld("letter", __instance),
                                "Snail Money" or "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" =>
                                    CreateItemOverworld("contactList", __instance),
                                "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                                "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                                "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                                "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                                "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                                "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                                "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                                "Super Jump" => CreateItemOverworld("superJump", __instance),
                                "Hairball City Fish" => CreateItemOverworld("hcfish", __instance),
                                "Turbine Town Fish" => CreateItemOverworld("ttfish", __instance),
                                "Salmon Creek Forest Fish" => CreateItemOverworld("scffish", __instance),
                                "Public Pool Fish" => CreateItemOverworld("ppfish", __instance),
                                "Bathhouse Fish" => CreateItemOverworld("bathfish", __instance),
                                "Tadpole HQ Fish" => CreateItemOverworld("hqfish", __instance),
                                "Hairball City Key" => CreateItemOverworld("hckey", __instance),
                                "Turbine Town Key" => CreateItemOverworld("ttkey", __instance),
                                "Salmon Creek Forest Key" => CreateItemOverworld("scfkey", __instance),
                                "Public Pool Key" => CreateItemOverworld("ppkey", __instance),
                                "Bathhouse Key" => CreateItemOverworld("bathkey", __instance),
                                "Hairball City Flower" => CreateItemOverworld("hcflower", __instance),
                                "Turbine Town Flower" => CreateItemOverworld("ttflower", __instance),
                                "Salmon Creek Forest Flower" => CreateItemOverworld("scfflower", __instance),
                                "Public Pool Flower" => CreateItemOverworld("ppflower", __instance),
                                "Bathhouse Flower" => CreateItemOverworld("bathflower", __instance),
                                "Tadpole HQ Flower" => CreateItemOverworld("hqflower", __instance),
                                "Hairball City Cassette" => CreateItemOverworld("hccassette", __instance),
                                "Turbine Town Cassette" => CreateItemOverworld("ttcassette", __instance),
                                "Salmon Creek Forest Cassette" => CreateItemOverworld("scfcassette", __instance),
                                "Public Pool Cassette" => CreateItemOverworld("ppcassette", __instance),
                                "Bathhouse Cassette" => CreateItemOverworld("bathcassette", __instance),
                                "Tadpole HQ Cassette" => CreateItemOverworld("hqcassette", __instance),
                                "Gary's Garden Cassette" => CreateItemOverworld("gardencassette", __instance),
                                "Hairball City Seed" => CreateItemOverworld("hcseed", __instance),
                                "Salmon Creek Forest Seed" => CreateItemOverworld("scfseed", __instance),
                                "Bathhouse Seed" => CreateItemOverworld("bathseed", __instance),
                                "Speed Boost" => CreateItemOverworld("speedboost", __instance),
                                "Party Invitation" => CreateItemOverworld("partyTicket", __instance),
                                "Safety Helmet" => CreateItemOverworld("bonkHelmet", __instance),
                                "Bug Net" => CreateItemOverworld("bugNet", __instance),
                                "Soda Repair" => CreateItemOverworld("sodaRepair", __instance),
                                "Parasol Repair" => CreateItemOverworld("parasolRepair", __instance),
                                "Swim Course" => CreateItemOverworld("swimCourse", __instance),
                                "Textbox" => CreateItemOverworld("textbox", __instance),
                                "AC Repair" => CreateItemOverworld("acRepair", __instance),
                                _ when ArchipelagoClient.ScoutedLocations[index + offset].ItemName.EndsWith("Trap") => 
                                    CreateItemOverworld(Assets.RandomProgTrap(), __instance, -20f),
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                    }
                    GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
                    GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                        .AppendLine($"Index: {index}, Offset: {offset}, Flag: CL2 Obtained")
                        .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                        .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                        .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
                        .AppendLine($"Model: {__instance.quads.name}");
                    break;
                }
            }
        }
    }
    
    [HarmonyPatch(typeof(scrSunflowerSeed), "Start")]
    public static class SunflowerSeedTexturePatch
    {
        private static readonly Dictionary<scrSunflowerSeed, Dictionary<string, GameObject>> InstanceItemsCache = new();

        private static GameObject CreateItemPrefab(string prefabName, scrSunflowerSeed instance, float speed = 6f)
        {
            // Ensure per-instance cache exists
            if (!InstanceItemsCache.ContainsKey(instance))
            {
                InstanceItemsCache[instance] = new Dictionary<string, GameObject>();
            }

            // Check if the instance-specific item already exists
            var instanceCache = InstanceItemsCache[instance];
            if (instanceCache.TryGetValue(prefabName, out var cachedItem))
            {
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogInfo($"Reusing existing item for prefab: {prefabName} on instance {instance.name}");
                return cachedItem;
            }

            if (!GameObjectChecker.CreatedItemsCache.TryGetValue(prefabName, out var blueprintPrefab))
            {
                GameObject prefab = Plugin.AssetBundle.LoadAsset<GameObject>(prefabName);
                if (prefab == null)
                {
                    if (Plugin.DebugMode)
                        Plugin.BepinLogger.LogError($"Prefab '{prefabName}' not found in AssetBundle.");
                    return null;
                }

                GameObjectChecker.CreatedItemsCache[prefabName] = prefab;
                blueprintPrefab = prefab;
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogInfo($"Cached prefab blueprint: {prefabName}");
            }
            
            GameObject itemOverworld = Object.Instantiate(blueprintPrefab, instance.transform, true);
            if (Plugin.DebugMode)
                Plugin.BepinLogger.LogInfo($"Instantiated new item from blueprint: {prefabName}");

            var ogQuads = instance.transform.Find("Quads").gameObject;
            var itemQuads = itemOverworld.transform.Find("Quads").gameObject;

            itemOverworld.transform.localPosition = Vector3.zero;
            itemQuads.transform.position = ogQuads.transform.position;
            
            if (itemQuads.GetComponent<ScuffedSpin>() == null)
            {
                itemQuads.AddComponent<ScuffedSpin>().spinSpeed = 50f;
            }
            if (itemQuads.GetComponent<scrUIwobble>() == null)
            {
                var wobble = itemQuads.AddComponent<scrUIwobble>();
                wobble.wobbleSpeed = 4.75f;
                wobble.wobbleAngle = 5f;
            }

            // Cache the created item for the specific instance
            instanceCache[prefabName] = itemOverworld;

            return itemOverworld;
        }

        private static GameObject CreateItemOverworld(string itemName, scrSunflowerSeed instance, float speed = 6f)
        {
            if (!Assets.PrefabMapping.TryGetValue(itemName, out string prefabName))
            {
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogError($"Item name '{itemName}' not recognized.");
                return null;
            }
            
            return CreateItemPrefab(prefabName, instance, speed);
        }
        
        private static void PlaceModel(int index, int offset, scrSunflowerSeed __instance)
        {
            if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
            {
                if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                    switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                    {
                        case "Time Piece"
                            when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                            __instance.quads = CreateItemOverworld("timepiece", __instance);
                            break;
                        case "Yarn"
                            when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                        {
                            // var yarnTextures = new[]
                            // {
                            //     CreateItemOverworld("yarn", __instance),
                            //     CreateItemOverworld("yarn2", __instance),
                            //     CreateItemOverworld("yarn3", __instance),
                            //     CreateItemOverworld("yarn4", __instance),
                            //     CreateItemOverworld("yarn5", __instance),
                            // };
                            // var randomIndex = Random.Range(0, yarnTextures.Length);
                            // __instance.quad = yarnTextures[randomIndex];
                            __instance.quads = CreateItemOverworld("yarn", __instance);
                            break;
                        }
                        default:
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                .HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    "apTrap",
                                    "apTrap1",
                                    "apTrap2"
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = CreateItemOverworld(trapTextures[randomIndex], __instance, -20f);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }

                            break;
                        }
                    }
                else
                    __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                    {
                        "Coin" => CreateItemOverworld("coin", __instance),
                        "Cassette" => CreateItemOverworld("cassette", __instance),
                        "Key" => CreateItemOverworld("key", __instance),
                        "Apples" or "25 Apples" => CreateItemOverworld("apples", __instance),
                        "10 Bugs" or "Bugs" => CreateItemOverworld("bugs", __instance),
                        "Letter" => CreateItemOverworld("letter", __instance),
                        "Snail Money" or "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                        "Contact List 1" or "Contact List 2" or "Progressive Contact List" =>
                            CreateItemOverworld("contactList", __instance),
                        "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                        "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                        "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                        "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                        "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                        "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                        "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                        "Super Jump" => CreateItemOverworld("superJump", __instance),
                        "Hairball City Fish" => CreateItemOverworld("hcfish", __instance),
                        "Turbine Town Fish" => CreateItemOverworld("ttfish", __instance),
                        "Salmon Creek Forest Fish" => CreateItemOverworld("scffish", __instance),
                        "Public Pool Fish" => CreateItemOverworld("ppfish", __instance),
                        "Bathhouse Fish" => CreateItemOverworld("bathfish", __instance),
                        "Tadpole HQ Fish" => CreateItemOverworld("hqfish", __instance),
                        "Hairball City Key" => CreateItemOverworld("hckey", __instance),
                        "Turbine Town Key" => CreateItemOverworld("ttkey", __instance),
                        "Salmon Creek Forest Key" => CreateItemOverworld("scfkey", __instance),
                        "Public Pool Key" => CreateItemOverworld("ppkey", __instance),
                        "Bathhouse Key" => CreateItemOverworld("bathkey", __instance),
                        "Tadpole HQ Key" => CreateItemOverworld("hqkey", __instance),
                        "Hairball City Flower" => CreateItemOverworld("hcflower", __instance),
                        "Turbine Town Flower" => CreateItemOverworld("ttflower", __instance),
                        "Salmon Creek Forest Flower" => CreateItemOverworld("scfflower", __instance),
                        "Public Pool Flower" => CreateItemOverworld("ppflower", __instance),
                        "Bathhouse Flower" => CreateItemOverworld("bathflower", __instance),
                        "Tadpole HQ Flower" => CreateItemOverworld("hqflower", __instance),
                        "Hairball City Cassette" => CreateItemOverworld("hccassette", __instance),
                        "Turbine Town Cassette" => CreateItemOverworld("ttcassette", __instance),
                        "Salmon Creek Forest Cassette" => CreateItemOverworld("scfcassette", __instance),
                        "Public Pool Cassette" => CreateItemOverworld("ppcassette", __instance),
                        "Bathhouse Cassette" => CreateItemOverworld("bathcassette", __instance),
                        "Tadpole HQ Cassette" => CreateItemOverworld("hqcassette", __instance),
                        "Gary's Garden Cassette" => CreateItemOverworld("gardencassette", __instance),
                        "Hairball City Seed" => CreateItemOverworld("hcseed", __instance),
                        "Salmon Creek Forest Seed" => CreateItemOverworld("scfseed", __instance),
                        "Bathhouse Seed" => CreateItemOverworld("bathseed", __instance),
                        "Speed Boost" => CreateItemOverworld("speedboost", __instance),
                        "Party Invitation" => CreateItemOverworld("partyTicket", __instance),
                        "Safety Helmet" => CreateItemOverworld("bonkHelmet", __instance),
                        "Bug Net" => CreateItemOverworld("bugNet", __instance),
                        "Soda Repair" => CreateItemOverworld("sodaRepair", __instance),
                        "Parasol Repair" => CreateItemOverworld("parasolRepair", __instance),
                        "Swim Course" => CreateItemOverworld("swimCourse", __instance),
                        "Textbox" => CreateItemOverworld("textbox", __instance),
                        "AC Repair" => CreateItemOverworld("acRepair", __instance),
                        "Apple Basket" => CreateItemOverworld("applebasket", __instance),
                        "Hairball City Bone" => CreateItemOverworld("hcbone", __instance),
                        "Turbine Town Bone" => CreateItemOverworld("ttbone", __instance),
                        "Salmon Creek Forest Bone" => CreateItemOverworld("scfbone", __instance),
                        "Public Pool Bone" => CreateItemOverworld("ppbone", __instance),
                        "Bathhouse Bone" => CreateItemOverworld("bathbone", __instance),
                        "Tadpole HQ Bone" => CreateItemOverworld("hqbone", __instance),
                        _ when ArchipelagoClient.ScoutedLocations[index + offset].ItemName.EndsWith("Trap") => 
                            CreateItemOverworld(Assets.RandomProgTrap(), __instance, -20f),
                        _ => CreateItemOverworld("apProg", __instance),
                    };
            }
        }
        
        private static void Postfix(scrSunflowerSeed __instance)
        {
            if (!ArchipelagoMenu.cacmi) return;
            if (scrWorldSaveDataContainer.instance.miscFlags.Contains(__instance.name) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                return;
            }
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.slotData.ContainsKey("seedsanity")) return;
            if (int.Parse(ArchipelagoData.slotData["seedsanity"].ToString()) == 0) return;
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
            {
                if (int.Parse(ArchipelagoData.slotData["shuffle_garden"].ToString()) == 0)
                {
                    gardenAdjustment = 13;
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
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            ogQuads.SetActive(false);

            var index = 0;
            var offset = 0;
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    var list = Locations.ScoutHCSeedsList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.name);
                    offset = 195 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSFCSeedsList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.name);
                    offset = 205 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathSeedsList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.name);
                    offset = 215 - adjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
            }
            __instance.quads.transform.localScale = new Vector3(1.15f, 1f, 1);
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Index: {index}, Offset: {offset}, Flag: {__instance.name}")
                .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
                .AppendLine($"Model: {__instance.quads.name}");
        }
    }
}