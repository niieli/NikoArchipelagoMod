using System;
using System.Collections.Generic;
using System.Reflection;
using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago.Stuff;

public static class PlaceModelHelper
{
    // THIS ACTUALLY WORKS??? begone 1000+ codes
    
    private static readonly Dictionary<Type, FieldInfo> QuadFieldCache = new();
    private static readonly Dictionary<string, GameObject> CreatedItemsCache = new();
    private static readonly Dictionary<object, Dictionary<string, GameObject>> InstanceItemsCache = new();

    private static FieldInfo GetQuadField(Type t)
    {
        if (!QuadFieldCache.TryGetValue(t, out var field))
        {
            field = AccessTools.Field(t, "quads");
            if (t.Name == "scrApple")
                field = AccessTools.Field(t, "quad");
            if (t.Name == "scrBugButterfly")
                field = AccessTools.Field(t, "wingRight");
            if (t.Name == "scrBugCatchable")
                field = AccessTools.Field(t, "functionality");
            if (field == null)
                field = AccessTools.Field(t, "quad");
            if (field == null)
                Plugin.BepinLogger.LogError($"Type {t.Name} does not contain a field 'quads' or 'quad'");
            QuadFieldCache[t] = field;
        }
        return field;
    }

    public static void PlaceModel<T>(int index, int offset, T instance)
    {
        if (index + offset >= ArchipelagoClient.ScoutedLocations.Count)
            return;

        var scoutedItemInfo = ArchipelagoClient.ScoutedLocations[index + offset];
        var quadField = GetQuadField(typeof(T));
        if (quadField == null)
            return;

        void SetQuad(GameObject go) => quadField.SetValue(instance, go);

        //TODO: Add more custom stuff for other games
        if (scoutedItemInfo.ItemGame != "Here Comes Niko!")
        {
            if (scoutedItemInfo.ItemName != null)
            {
                SetQuad(scoutedItemInfo.ItemName switch
                {
                    "Time Piece" when scoutedItemInfo.ItemGame == "A Hat in Time" =>
                        CreateItemOverworld("timepiece", instance),

                    "Yarn" when scoutedItemInfo.ItemGame == "A Hat in Time" =>
                        CreateItemOverworld("yarn", instance),

                    _ when scoutedItemInfo.Flags.HasFlag(ItemFlags.Advancement) =>
                        CreateItemOverworld("apProg", instance),

                    _ when scoutedItemInfo.Flags.HasFlag(ItemFlags.NeverExclude) =>
                        CreateItemOverworld("apUseful", instance),

                    _ when scoutedItemInfo.Flags.HasFlag(ItemFlags.Trap) =>
                        CreateItemOverworld(RandomTrapTexture(), instance, -20f),

                    _ => CreateItemOverworld("apFiller", instance)
                });
            }
            else
            {
                SetQuad(scoutedItemInfo.ItemId switch
                {
                    _ when scoutedItemInfo.Flags.HasFlag(ItemFlags.Advancement) =>
                        CreateItemOverworld("apProg", instance),

                    _ when scoutedItemInfo.Flags.HasFlag(ItemFlags.NeverExclude) =>
                        CreateItemOverworld("apUseful", instance),

                    _ when scoutedItemInfo.Flags.HasFlag(ItemFlags.Trap) =>
                        CreateItemOverworld(RandomTrapTexture(), instance, -20f),

                    _ => CreateItemOverworld("apFiller", instance)
                });
            }
                
        }
        else
        {
            SetQuad(scoutedItemInfo.ItemId switch
            {
                ItemID.Coin => CreateItemOverworld("coin", instance),
                ItemID.Cassette => CreateItemOverworld("cassette", instance),
                ItemID.Key => CreateItemOverworld("key", instance),
                ItemID.Apples => CreateItemOverworld("apples", instance),
                ItemID.Bugs => CreateItemOverworld("bugs", instance),
                ItemID.Letter => CreateItemOverworld("letter", instance),
                ItemID.SnailMoney => CreateItemOverworld("snailMoney", instance),
                ItemID.ContactList1 or ItemID.ContactList2 or ItemID.ProgressiveContactList =>
                    CreateItemOverworld("contactList", instance),
                ItemID.HairballCityTicket => CreateItemOverworld("hairballCity", instance),
                ItemID.TurbineTownTicket => CreateItemOverworld("turbineTown", instance),
                ItemID.SalmonCreekForestTicket => CreateItemOverworld("salmonCreekForest", instance),
                ItemID.PublicPoolTicket => CreateItemOverworld("publicPool", instance),
                ItemID.BathhouseTicket => CreateItemOverworld("bathhouse", instance),
                ItemID.TadpoleHqTicket => CreateItemOverworld("tadpoleHQ", instance),
                ItemID.GarysGardenTicket => CreateItemOverworld("garysGarden", instance),
                ItemID.SuperJump => CreateItemOverworld("superJump", instance),
                ItemID.HairballCityFish => CreateItemOverworld("hcfish", instance),
                ItemID.TurbineTownFish => CreateItemOverworld("ttfish", instance),
                ItemID.SalmonCreekForestFish => CreateItemOverworld("scffish", instance),
                ItemID.PublicPoolFish => CreateItemOverworld("ppfish", instance),
                ItemID.BathhouseFish => CreateItemOverworld("bathfish", instance),
                ItemID.TadpoleHqFish => CreateItemOverworld("hqfish", instance),
                ItemID.HairballCityKey => CreateItemOverworld("hckey", instance),
                ItemID.TurbineTownKey => CreateItemOverworld("ttkey", instance),
                ItemID.SalmonCreekForestKey => CreateItemOverworld("scfkey", instance),
                ItemID.PublicPoolKey => CreateItemOverworld("ppkey", instance),
                ItemID.BathhouseKey => CreateItemOverworld("bathkey", instance),
                ItemID.TadpoleHqKey => CreateItemOverworld("hqkey", instance),
                ItemID.HairballCityFlower => CreateItemOverworld("hcflower", instance),
                ItemID.TurbineTownFlower => CreateItemOverworld("ttflower", instance),
                ItemID.SalmonCreekForestFlower => CreateItemOverworld("scfflower", instance),
                ItemID.PublicPoolFlower => CreateItemOverworld("ppflower", instance),
                ItemID.BathhouseFlower => CreateItemOverworld("bathflower", instance),
                ItemID.TadpoleHqFlower => CreateItemOverworld("hqflower", instance),
                ItemID.HairballCityCassette => CreateItemOverworld("hccassette", instance),
                ItemID.TurbineTownCassette => CreateItemOverworld("ttcassette", instance),
                ItemID.SalmonCreekForestCassette => CreateItemOverworld("scfcassette", instance),
                ItemID.PublicPoolCassette => CreateItemOverworld("ppcassette", instance),
                ItemID.BathhouseCassette => CreateItemOverworld("bathcassette", instance),
                ItemID.TadpoleHqCassette => CreateItemOverworld("hqcassette", instance),
                ItemID.GarysGardenCassette => CreateItemOverworld("gardencassette", instance),
                ItemID.HairballCitySeed => CreateItemOverworld("hcseed", instance),
                ItemID.SalmonCreekForestSeed => CreateItemOverworld("scfseed", instance),
                ItemID.BathhouseSeed => CreateItemOverworld("bathseed", instance),
                ItemID.SpeedBoost => CreateItemOverworld("speedboost", instance),
                ItemID.PartyInvitation => CreateItemOverworld("partyTicket", instance),
                ItemID.SafetyHelmet => CreateItemOverworld("bonkHelmet", instance),
                ItemID.BugNet => CreateItemOverworld("bugNet", instance),
                ItemID.SodaRepair => CreateItemOverworld("sodaRepair", instance),
                ItemID.ParasolRepair => CreateItemOverworld("parasolRepair", instance),
                ItemID.SwimCourse => CreateItemOverworld("swimCourse", instance),
                ItemID.Textbox => CreateItemOverworld("textbox", instance),
                ItemID.AcRepair => CreateItemOverworld("acRepair", instance),
                ItemID.AppleBasket => CreateItemOverworld("applebasket", instance),
                ItemID.HairballCityBone => CreateItemOverworld("hcbone", instance),
                ItemID.TurbineTownBone => CreateItemOverworld("ttbone", instance),
                ItemID.SalmonCreekForestBone => CreateItemOverworld("scfbone", instance),
                ItemID.PublicPoolBone => CreateItemOverworld("ppbone", instance),
                ItemID.BathhouseBone => CreateItemOverworld("bathbone", instance),
                ItemID.TadpoleHqBone => CreateItemOverworld("hqbone", instance),
                _ when ItemID.TrapIDs.Contains(scoutedItemInfo.ItemId) => 
                    CreateItemOverworld(Assets.RandomProgTrap(), instance, -20f),
                _ => CreateItemOverworld("apProg", instance),
            });
        }
    }

    private static string RandomTrapTexture()
    {
        var trapTextures = new[] { "apTrap", "apTrap1", "apTrap2" };
        return trapTextures[UnityEngine.Random.Range(0, trapTextures.Length)];
    }

    private static GameObject CreateItemOverworld<T>(string itemName, T instance, float speed = 6f)
    {
        if (!Assets.PrefabMapping.TryGetValue(itemName, out var prefabName))
        {
            Plugin.BepinLogger.LogError($"Item name '{itemName}' not recognized.");
            return null;
        }
        return CreateItemPrefab(prefabName, instance, speed);
    }

    private static GameObject CreateItemPrefab<T>(string prefabName, T instance, float speed = 6f)
    {
        if (!InstanceItemsCache.ContainsKey(instance))
            InstanceItemsCache[instance] = new Dictionary<string, GameObject>();

        var instanceCache = InstanceItemsCache[instance];
        if (instanceCache.TryGetValue(prefabName, out var cachedItem))
            return cachedItem;

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
        }

        var itemOverworld = UnityEngine.Object.Instantiate(blueprintPrefab, (instance as Component)?.transform, true);

        var ogQuads = FindRealQuads(instance);

        var itemQuads = itemOverworld.transform.Find("Quads")?.gameObject;
        if (ogQuads == null || itemQuads == null) return null;

        itemOverworld.transform.localPosition = Vector3.zero;
        itemQuads.transform.position = ogQuads.transform.position;

        if (itemQuads.GetComponent<ScuffedSpin>() == null) itemQuads.AddComponent<ScuffedSpin>();
        if (itemQuads.GetComponent<scrUIwobble>() == null)
        {
            var wobble = itemQuads.AddComponent<scrUIwobble>();
            wobble.wobbleSpeed = speed;
            wobble.wobbleAngle = 5f;
        }

        instanceCache[prefabName] = itemOverworld;
        return itemOverworld;
    }

    // We need to find the quad that the original model uses, so we can replace at the exact position
    private static GameObject FindRealQuads(object instance)
    {
        if (instance is scrBugButterfly bugButterfly)
        {
            if (bugButterfly.wingRight.transform.Find("Quad") != null) return bugButterfly.wingRight.transform.Find("Quad").gameObject;
            if (bugButterfly.wingLeft.transform.Find("Quad") != null) return bugButterfly.wingLeft.transform.Find("Quad").gameObject;
            
            return null;
        }
        
        if (instance is scrBugCatchable bugCatchable)
        {
            if (bugCatchable.functionality.transform.Find("Sit") != null) return bugCatchable.functionality.transform.Find("Sit").gameObject;
            
            return null;
        }
        
        var t = (instance as Component)?.transform;
        if (t?.Find("Quad") != null) return t.Find("Quad").gameObject;
        if (t?.Find("Quads") != null) return t.Find("Quads").gameObject;
        if (t?.Find("Square") != null) return t.Find("Square").gameObject;
        
        return null;
    }
}