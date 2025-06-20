﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class Applesanity
{
    private static bool applesanityOn = false;
    public static bool NoticeUp;
    [HarmonyPatch(typeof(scrApple), "Start")]
    public static class ApplesanityStart
    {
        private static readonly Dictionary<string, GameObject> CreatedItemsCache = new();
        private static readonly Dictionary<scrApple, Dictionary<string, GameObject>> InstanceItemsCache = new();
        
        private static GameObject CreateItemPrefab(string prefabName, scrApple instance, float speed = 6f)
        {
            // Ensure per-instance cache exists
            if (!InstanceItemsCache.ContainsKey(instance))
                InstanceItemsCache[instance] = new Dictionary<string, GameObject>();

            // Check if the instance-specific item already exists
            var instanceCache = InstanceItemsCache[instance];
            if (instanceCache.TryGetValue(prefabName, out var cachedItem))
            {
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogInfo(
                        $"Reusing existing item for prefab: {prefabName} on instance {instance.name}");
                return cachedItem;
            }

            if (!CreatedItemsCache.TryGetValue(prefabName, out var blueprintPrefab))
            {
                var prefab = Plugin.AssetBundle.LoadAsset<GameObject>(prefabName);
                if (prefab == null)
                {
                    if (Plugin.DebugMode)
                        Plugin.BepinLogger.LogError($"Prefab '{prefabName}' not found in AssetBundle.");
                    return null;
                }

                CreatedItemsCache[prefabName] = prefab;
                blueprintPrefab = prefab;
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogInfo($"Cached prefab blueprint: {prefabName}");
            }

            var itemOverworld = Object.Instantiate(blueprintPrefab, instance.transform, true);
            if (Plugin.DebugMode)
                Plugin.BepinLogger.LogInfo($"Instantiated new item from blueprint: {prefabName}");

            GameObject ogQuads = null;
            if (instance.transform.Find("Quad") != null)
                ogQuads = instance.transform.Find("Quad").gameObject;
            else if (instance.transform.Find("Square") != null)
                ogQuads = instance.transform.Find("Square").gameObject;
            var itemQuads = itemOverworld.transform.Find("Quads").gameObject;

            if (ogQuads == null) return null;
            itemOverworld.transform.localPosition = Vector3.zero;
            itemQuads.transform.position = ogQuads.transform.position;

            if (itemQuads.GetComponent<ScuffedSpin>() == null) itemQuads.AddComponent<ScuffedSpin>();

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

        private static GameObject CreateItemOverworld(string itemName, scrApple instance, float speed = 6f)
        {
            if (!Assets.PrefabMapping.TryGetValue(itemName, out var prefabName))
            {
                Plugin.BepinLogger.LogError($"Item name '{itemName}' not recognized.");
                return null;
            }

            return CreateItemPrefab(prefabName, instance, speed);
        }

        private static void PlaceModel(int index, int offset, scrApple __instance)
        {
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
                                    "apTrap",
                                    "apTrap1",
                                    "apTrap2"
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quad = CreateItemOverworld(trapTextures[randomIndex], __instance, -20f);
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

        public static Dictionary<scrApple, int> appleIDs = new();
        public static int nextAppleID = 1;

        private static bool Prefix(scrApple __instance)
        {
            // scrApple[] allApples = Resources.FindObjectsOfTypeAll<scrApple>();
            //
            // foreach (var apple in allApples)
            // {
            //     if (!appleIDs.ContainsKey(apple))
            //     {
            //         appleIDs[apple] = nextAppleID++;
            //         Plugin.BepinLogger.LogInfo($"Assigned ID {appleIDs[apple]} to: {apple.gameObject.name} (Active: {apple.gameObject.activeInHierarchy})");
            //     }
            // }
            var maxAppleID = 999;
            if (SceneManager.GetActiveScene().name == "Hairball City")
            {
                maxAppleID = ApplesanityTrigger.appleCountHC;
            } else if (SceneManager.GetActiveScene().name == "Trash Kingdom")
            {
                maxAppleID = ApplesanityTrigger.appleCountTT;
            }else if (SceneManager.GetActiveScene().name == "Salmon Creek Forest")
            {
                maxAppleID = ApplesanityTrigger.appleCountSCF;
            }else if (SceneManager.GetActiveScene().name == "Public Pool")
            {
                maxAppleID = ApplesanityTrigger.appleCountPP;
            }else if (SceneManager.GetActiveScene().name == "The Bathhouse")
            {
                maxAppleID = ApplesanityTrigger.appleCountBath;
            }else if (SceneManager.GetActiveScene().name == "Tadpole inc")
            {
                maxAppleID = ApplesanityTrigger.appleCountHQ;
            }

            if (maxAppleID < nextAppleID) return true;
            if (!appleIDs.ContainsKey(__instance))
            {
                appleIDs[__instance] = nextAppleID++;
                //Plugin.BepinLogger.LogInfo($"Assigned Apple CustomID: {appleIDs[__instance]} to {__instance.gameObject.name}");
            }
            return true;
        }

        private static void Postfix(scrApple __instance)
        {
            if (!ArchipelagoMenu.cacmi) return;
            if (!appleIDs.ContainsKey(__instance))
            {
                return;
            }
            var flag = "Apple" + appleIDs[__instance];
            if (scrWorldSaveDataContainer.instance.miscFlags.Contains(flag) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                return;
            }
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            var idkAdjustment = 0;
            var seedAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.slotData.ContainsKey("applessanity")) return;
            if (int.Parse(ArchipelagoData.slotData["applessanity"].ToString()) == 0) return;
            applesanityOn = true;
            var currentscene = SceneManager.GetActiveScene().name;
            if (currentscene == "Home") return; // Do not interact with apples in home, will need to be removed/checked for conditions.
            __instance.enabled = true;
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
                if (int.Parse(ArchipelagoData.slotData["shuffle_garden"].ToString()) == 0)
                {
                    gardenAdjustment = 13;
                    idkAdjustment = 2;
                }
            if (ArchipelagoData.slotData.ContainsKey("snailshop"))
                if (int.Parse(ArchipelagoData.slotData["snailshop"].ToString()) == 0)
                    snailShopAdjustment = 16;
            if (ArchipelagoData.slotData.ContainsKey("cassette_logic"))
                if (int.Parse(ArchipelagoData.slotData["cassette_logic"].ToString()) == 1)
                    cassetteAdjustment = 14;
            if (ArchipelagoData.slotData.ContainsKey("seedsanity"))
                if (int.Parse(ArchipelagoData.slotData["seedsanity"].ToString()) == 0)
                    seedAdjustment = 30;

            var adjustment = gardenAdjustment + snailShopAdjustment + cassetteAdjustment + seedAdjustment;
            GameObject ogQuads = null;
            if (__instance.transform.Find("Quad") != null)
                ogQuads = __instance.transform.Find("Quad").gameObject;
            else if (__instance.transform.Find("Square") != null)
                ogQuads = __instance.transform.Find("Square").gameObject;
            if (ogQuads == null) return;
            Object.Destroy(ogQuads.gameObject);
            ogQuads.SetActive(false);
            __instance.transform.position += new Vector3(0, 0.25f, 0);
            if (__instance.transform.Find("Particle System Sparkle") != null)
                if (__instance.transform.Find("Particle System Sparkle").gameObject.activeInHierarchy)
                    __instance.transform.position += new Vector3(0, 0.75f, 0);
            var index = 0;
            var offset = 0;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    var list = Locations.ScoutHCApplesList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 225 - adjustment + idkAdjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Trash Kingdom":
                {
                    __instance.transform.position += new Vector3(0, -0.2f, 0);
                    var list = Locations.ScoutTTApplesList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 257 - adjustment + idkAdjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSFCApplesList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 290 - adjustment + idkAdjustment;
                    PlaceModel(index, offset, __instance);
                    if (appleIDs[__instance] == 74 || appleIDs[__instance] == 106 || appleIDs[__instance] == 120)
                    {
                        __instance.transform.position += new Vector3(0, 0.5f, 0);
                        Plugin.BepinLogger.LogInfo($"Moved Apple: {appleIDs[__instance]} to {__instance.transform.position}");
                    }
                    if (appleIDs[__instance] == 75)
                    {
                        __instance.transform.position = new Vector3(27.5f, 130.3f, 139.6f);
                        Plugin.BepinLogger.LogInfo($"Moved Apple: {appleIDs[__instance]} to {__instance.transform.position}");
                    }
                    if (appleIDs[__instance] == 92)
                    {
                        __instance.transform.position = new Vector3(38.8f, 133.5f, 135.4f);
                        Plugin.BepinLogger.LogInfo($"Moved Apple: {appleIDs[__instance]} to {__instance.transform.position}");
                    }
                    if (appleIDs[__instance] == 52 || appleIDs[__instance] == 42 || appleIDs[__instance] == 35 || appleIDs[__instance] == 55 || appleIDs[__instance] == 59 || appleIDs[__instance] == 115)
                    {
                        __instance.transform.position += new Vector3(0, 0.3f, 0);
                        Plugin.BepinLogger.LogInfo($"Moved Apple: {appleIDs[__instance]} to {__instance.transform.position}");
                    }
                    if (appleIDs[__instance] == 38)
                    {
                        __instance.transform.position -= new Vector3(0, 0.3f, 0);
                        Plugin.BepinLogger.LogInfo($"Moved Apple: {appleIDs[__instance]} to {__instance.transform.position}");
                    }
                    if (appleIDs[__instance] == 111 || appleIDs[__instance] == 83 || appleIDs[__instance] == 32)
                    {
                        __instance.transform.position += new Vector3(0, 0.8f, 0);
                        Plugin.BepinLogger.LogInfo($"Moved Apple: {appleIDs[__instance]} to {__instance.transform.position}");
                    }
                    break;
                }
                case "Public Pool":
                {
                    __instance.transform.position += new Vector3(0, -0.125f, 0);
                    var list = Locations.ScoutPPApplesList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 416 - adjustment + idkAdjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathApplesList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 510 - adjustment + idkAdjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutHQApplesList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 582 - adjustment + idkAdjustment;
                    PlaceModel(index, offset, __instance);
                    if (appleIDs[__instance] == 11)
                    {
                        __instance.transform.position = new Vector3(111f, 3.5f, 5f);
                        Plugin.BepinLogger.LogInfo($"Moved Apple: {appleIDs[__instance]} to {__instance.transform.position}");
                    }
                    break;
                }
            }
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Index: {index}, Offset: {offset}, FlagID: Apple{appleIDs[__instance]}")
                .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
                .AppendLine($"Model: {__instance.quad.name}");
        }
    }

    [HarmonyPatch(typeof(scrApple), "OnTriggerEnter")]
    public static class AppleBasketPatch
    {
        
    }

    [HarmonyPatch(typeof(scrApple), "GetCollected")]
    public static class ApplesanityTrigger
    {
        public static int appleCountHC = 32;
        public static int appleCountTT = 33;
        public static int appleCountSCF = 126;
        public static int appleCountPP = 94;
        public static int appleCountBath = 72;
        public static int appleCountHQ = 14;
        
        private static bool Prefix(scrApple __instance)
        {
            if (ArchipelagoData.slotData == null) return true;
            if (!ArchipelagoData.slotData.ContainsKey("applebasket")) return true;
            if (int.Parse(ArchipelagoData.slotData["applebasket"].ToString()) == 0) return true;
            
            if (ArchipelagoClient.AppleBasketAcquired) return true;
            __instance.StartCoroutine(ShowNotice());
            return false;
        }
        
        private static IEnumerator ShowNotice()
        {
            if (NoticeUp || !SavedData.Instance.Notices) yield break;
            var t = Object.Instantiate(Plugin.NoticeAppleBasket, Plugin.NotifcationCanvas.transform);
            NoticeUp = true;
            var time = 0f;
            while (time < 60f)
            {
                time += Time.deltaTime;
                yield return null;
            }
            Object.Destroy(t);
            NoticeUp = false;
        }
        private static void Postfix(scrApple __instance)
        {
            if (!ArchipelagoClient.AppleBasketAcquired) return;
            if (!applesanityOn) return;
            if (!ApplesanityStart.appleIDs.ContainsKey(__instance))
            {
                return;
            }
            var flag = "Apple" + ApplesanityStart.appleIDs[__instance];
            if (!scrWorldSaveDataContainer.instance.miscFlags.Contains(flag))
            {
                scrAppleDisplayer.show = false;
                scrWorldSaveDataContainer.instance.miscFlags.Add(flag);
                scrGameSaveManager.instance.gameData.generalGameData.appleAmount--;
                scrGameSaveManager.instance.SaveGame();
                scrWorldSaveDataContainer.instance.SaveWorld();
            }
        }
    }
}