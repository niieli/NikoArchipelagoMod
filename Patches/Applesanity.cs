using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class Applesanity
{
    [HarmonyPatch(typeof(scrApple), "Start")]
    public static class ApplesanityStart
    {
        private static readonly Dictionary<string, GameObject> CreatedItemsCache = new();
        private static readonly Dictionary<scrApple, Dictionary<string, GameObject>> InstanceItemsCache = new();

        private static GameObject CreateItemPrefab(string prefabName, scrApple instance)
        {
            // Ensure per-instance cache exists
            if (!InstanceItemsCache.ContainsKey(instance))
                InstanceItemsCache[instance] = new Dictionary<string, GameObject>();

            // Check if the instance-specific item already exists
            var instanceCache = InstanceItemsCache[instance];
            if (instanceCache.TryGetValue(prefabName, out var cachedItem))
            {
                Plugin.BepinLogger.LogInfo(
                    $"Reusing existing item for prefab: {prefabName} on instance {instance.name}");
                return cachedItem;
            }

            if (!CreatedItemsCache.TryGetValue(prefabName, out var blueprintPrefab))
            {
                var prefab = Plugin.AssetBundle.LoadAsset<GameObject>(prefabName);
                if (prefab == null)
                {
                    Plugin.BepinLogger.LogError($"Prefab '{prefabName}' not found in AssetBundle.");
                    return null;
                }

                CreatedItemsCache[prefabName] = prefab;
                blueprintPrefab = prefab;
                Plugin.BepinLogger.LogInfo($"Cached prefab blueprint: {prefabName}");
            }

            var itemOverworld = Object.Instantiate(blueprintPrefab, instance.transform, true);
            Plugin.BepinLogger.LogInfo($"Instantiated new item from blueprint: {prefabName}");

            var ogQuads = instance.transform.Find("Quad").gameObject;
            var itemQuads = itemOverworld.transform.Find("Quad").gameObject;

            itemOverworld.transform.localPosition = Vector3.zero;
            itemQuads.transform.position = ogQuads.transform.position;

            if (itemQuads.GetComponent<ScuffedSpin>() == null) itemQuads.AddComponent<ScuffedSpin>();

            if (itemQuads.GetComponent<scrUIwobble>() == null)
            {
                var wobble = itemQuads.AddComponent<scrUIwobble>();
                wobble.wobbleSpeed = 6f;
                wobble.wobbleAngle = 5f;
            }

            // Cache the created item for the specific instance
            instanceCache[prefabName] = itemOverworld;

            return itemOverworld;
        }

        private static GameObject CreateItemOverworld(string itemName, scrApple instance)
        {
            var prefabMap = new Dictionary<string, string>
            {
                { "apProg", "APProgressive" },
                { "apUseful", "APUseful" },
                { "apFiller", "APFiller" },
                { "apTrap", "APTrap" },
                { "apTrap1", "APTrap1" },
                { "apTrap2", "APTrap2" },
                { "coin", "Coin" },
                { "cassette", "Cassette" },
                { "key", "Key" },
                { "contactList", "ContactList" },
                { "apples", "Apples" },
                { "snailMoney", "SnailMoney" },
                { "letter", "Letter" },
                { "bugs", "Bugs" },
                { "hcfish", "HairballFish" },
                { "ttfish", "TurbineFish" },
                { "scffish", "SalmonFish" },
                { "ppfish", "PoolFish" },
                { "bathfish", "BathFish" },
                { "hqfish", "TadpoleFish" },
                { "hckey", "HairballKey" },
                { "ttkey", "TurbineKey" },
                { "scfkey", "SalmonKey" },
                { "ppkey", "PoolKey" },
                { "bathkey", "BathKey" },
                { "hqkey", "TadpoleKey" },
                { "superJump", "SuperJump" },
                { "hairballCity", "HairballCity" },
                { "turbineTown", "TurbineTown" },
                { "salmonCreekForest", "SalmonCreekForest" },
                { "publicPool", "PublicPool" },
                { "bathhouse", "Bathhouse" },
                { "tadpoleHQ", "TadpoleHQ" },
                { "garysGarden", "GarysGarden" },
                { "timepiece", "TimePieceHiT" },
                { "yarn", "YarnHiT" },
                { "yarn2", "Yarn2HiT" },
                { "yarn3", "Yarn3HiT" },
                { "yarn4", "Yarn4HiT" },
                { "yarn5", "Yarn5HiT" }
            };

            if (!prefabMap.TryGetValue(itemName, out var prefabName))
            {
                Plugin.BepinLogger.LogError($"Item name '{itemName}' not recognized.");
                return null;
            }

            return CreateItemPrefab(prefabName, instance);
        }

        private static void Postfix(scrApple __instance)
        {
            if (!ArchipelagoMenu.cacmi) return;
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.slotData.ContainsKey("applesanity")) return;
            if (int.Parse(ArchipelagoData.slotData["applesanity"].ToString()) == 0) return;
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
                if (int.Parse(ArchipelagoData.slotData["shuffle_garden"].ToString()) == 0)
                    gardenAdjustment = 13;

            if (ArchipelagoData.slotData.ContainsKey("snailshop"))
                if (int.Parse(ArchipelagoData.slotData["snailshop"].ToString()) == 0)
                    snailShopAdjustment = 16;

            var adjustment = gardenAdjustment + snailShopAdjustment;
            var ogQuads = __instance.transform.Find("Quad").gameObject;
            Object.Destroy(ogQuads.gameObject);
            ogQuads.SetActive(false);
            var flag = __instance.name;

            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    var list = Locations.ScoutHCApplesList.ToList();
                    var index = list.FindIndex(pair => pair.Value == flag);
                    var offset = 225 - adjustment;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece"
                                    when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.quad = CreateItemOverworld("timepiece", __instance);
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
                                    __instance.quad = CreateItemOverworld("yarn", __instance);
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                        .HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.quad = CreateItemOverworld("apProg", __instance);
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.quad = CreateItemOverworld("apUseful", __instance);
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
                                        __instance.quad = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.None))
                                    {
                                        __instance.quad = CreateItemOverworld("apFiller", __instance);
                                    }

                                    break;
                                }
                            }
                        else
                            __instance.quad = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "Letter" => CreateItemOverworld("letter", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
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
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                    }

                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName
                                                        + "\nLocation: " +
                                                        ArchipelagoClient.ScoutedLocations[index + offset].LocationName
                                                        + "\nLocationID: " +
                                                        ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
                case "Trash Kingdom":
                {
                    var list = Locations.ScoutTTApplesList.ToList();
                    var index = list.FindIndex(pair => pair.Value == flag);
                    var offset = 257 - adjustment;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece"
                                    when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.quad = CreateItemOverworld("timepiece", __instance);
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
                                    __instance.quad = CreateItemOverworld("yarn", __instance);
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                        .HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.quad = CreateItemOverworld("apProg", __instance);
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.quad = CreateItemOverworld("apUseful", __instance);
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
                                        __instance.quad = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.None))
                                    {
                                        __instance.quad = CreateItemOverworld("apFiller", __instance);
                                    }

                                    break;
                                }
                            }
                        else
                            __instance.quad = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "Letter" => CreateItemOverworld("letter", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
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
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                    }

                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName
                                                        + "\nLocation: " +
                                                        ArchipelagoClient.ScoutedLocations[index + offset].LocationName
                                                        + "\nLocationID: " +
                                                        ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSFCApplesList.ToList();
                    var index = list.FindIndex(pair => pair.Value == flag);
                    var offset = 289 - adjustment;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece"
                                    when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.quad = CreateItemOverworld("timepiece", __instance);
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
                                    __instance.quad = CreateItemOverworld("yarn", __instance);
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                        .HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.quad = CreateItemOverworld("apProg", __instance);
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.quad = CreateItemOverworld("apUseful", __instance);
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
                                        __instance.quad = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.None))
                                    {
                                        __instance.quad = CreateItemOverworld("apFiller", __instance);
                                    }

                                    break;
                                }
                            }
                        else
                            __instance.quad = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "Letter" => CreateItemOverworld("letter", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
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
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                    }

                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName
                                                        + "\nLocation: " +
                                                        ArchipelagoClient.ScoutedLocations[index + offset].LocationName
                                                        + "\nLocationID: " +
                                                        ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
                case "Public Pool":
                {
                    var list = Locations.ScoutPPApplesList.ToList();
                    var index = list.FindIndex(pair => pair.Value == flag);
                    var offset = 375 - adjustment;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece"
                                    when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.quad = CreateItemOverworld("timepiece", __instance);
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
                                    __instance.quad = CreateItemOverworld("yarn", __instance);
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                        .HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.quad = CreateItemOverworld("apProg", __instance);
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.quad = CreateItemOverworld("apUseful", __instance);
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
                                        __instance.quad = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.None))
                                    {
                                        __instance.quad = CreateItemOverworld("apFiller", __instance);
                                    }

                                    break;
                                }
                            }
                        else
                            __instance.quad = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "Letter" => CreateItemOverworld("letter", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
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
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                    }

                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName
                                                        + "\nLocation: " +
                                                        ArchipelagoClient.ScoutedLocations[index + offset].LocationName
                                                        + "\nLocationID: " +
                                                        ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathApplesList.ToList();
                    var index = list.FindIndex(pair => pair.Value == flag);
                    var offset = 443 - adjustment;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece"
                                    when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.quad = CreateItemOverworld("timepiece", __instance);
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
                                    __instance.quad = CreateItemOverworld("yarn", __instance);
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                        .HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.quad = CreateItemOverworld("apProg", __instance);
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.quad = CreateItemOverworld("apUseful", __instance);
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
                                        __instance.quad = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.None))
                                    {
                                        __instance.quad = CreateItemOverworld("apFiller", __instance);
                                    }

                                    break;
                                }
                            }
                        else
                            __instance.quad = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "Letter" => CreateItemOverworld("letter", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
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
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                    }

                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName
                                                        + "\nLocation: " +
                                                        ArchipelagoClient.ScoutedLocations[index + offset].LocationName
                                                        + "\nLocationID: " +
                                                        ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutHQApplesList.ToList();
                    var index = list.FindIndex(pair => pair.Value == flag);
                    var offset = 510 - adjustment;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                            switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                            {
                                case "Time Piece"
                                    when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                                    __instance.quad = CreateItemOverworld("timepiece", __instance);
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
                                    __instance.quad = CreateItemOverworld("yarn", __instance);
                                    break;
                                }
                                default:
                                {
                                    if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                        .HasFlag(ItemFlags.Advancement))
                                    {
                                        __instance.quad = CreateItemOverworld("apProg", __instance);
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.NeverExclude))
                                    {
                                        __instance.quad = CreateItemOverworld("apUseful", __instance);
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
                                        __instance.quad = trapTextures[randomIndex];
                                    }
                                    else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                             .HasFlag(ItemFlags.None))
                                    {
                                        __instance.quad = CreateItemOverworld("apFiller", __instance);
                                    }

                                    break;
                                }
                            }
                        else
                            __instance.quad = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "Letter" => CreateItemOverworld("letter", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
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
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                    }

                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName
                                                        + "\nLocation: " +
                                                        ArchipelagoClient.ScoutedLocations[index + offset].LocationName
                                                        + "\nLocationID: " +
                                                        ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
                    break;
                }
            }
        }
    }

    [HarmonyPatch(typeof(scrApple), "GetCollected")]
    public static class ApplesanityTrigger
    {
        private static void Postfix(scrApple __instance)
        {
            if (!scrWorldSaveDataContainer.instance.miscFlags.Contains(__instance.name))
            {
                scrWorldSaveDataContainer.instance.miscFlags.Add(__instance.name);
                scrGameSaveManager.instance.gameData.generalGameData.appleAmount--;
                scrGameSaveManager.instance.SaveGame();
                scrWorldSaveDataContainer.instance.SaveWorld();
            }
        }
    }
}