using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class Bugsanity
{
    public static Dictionary<MonoBehaviour, int> bugIDs = new();
    public static int nextBugID = 1;
        
    private static int GetMaxBugIDForScene(string sceneName)
    {
        return sceneName switch
        {
            "Hairball City" or "Trash Kingdom" => 58,
            "Salmon Creek Forest" => 89,
            "The Bathhouse" => 51,
            _ => 999
        };
    }

    private static bool AssignBugID(MonoBehaviour bug)
    {
        var scene = SceneManager.GetActiveScene().name;
        int maxBugID = GetMaxBugIDForScene(scene);
        
        if (maxBugID < nextBugID) return true;
        if (!bugIDs.ContainsKey(bug))
        {
            bugIDs[bug] = nextBugID++;
            //Plugin.BepinLogger.LogInfo($"Assigned Bug CustomID: {bugIDs[bug]} to {bug.gameObject.name}");
        }
        return true;
    }
    
    private static readonly Dictionary<string, GameObject> CreatedItemsCache = new();
    private static readonly Dictionary<MonoBehaviour, Dictionary<string, GameObject>> InstanceItemsCache = new();
    
    [HarmonyPatch(typeof(scrBugButterfly), "Start")]
    public static class BugsanityStart
    {
        private static GameObject CreateItemPrefab(string prefabName, scrBugButterfly instance, float speed = 6f)
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

            GameObject ogQuads = null;
            if (instance.wingRight.transform.Find("Quad") != null)
                ogQuads = instance.wingRight.transform.Find("Quad").gameObject;
            else if (instance.wingLeft.transform.Find("Quad") != null)
                ogQuads = instance.wingLeft.transform.Find("Quad").gameObject;
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

        private static GameObject CreateItemOverworld(string itemName, scrBugButterfly instance, float speed = 6f)
        {
            if (Assets.PrefabMapping.TryGetValue(itemName, out var prefabName))
                return CreateItemPrefab(prefabName, instance, speed);
            Plugin.BepinLogger.LogError($"Item name '{itemName}' not recognized.");
            return null;
        }

        private static void PlaceModel(int index, int offset, scrBugButterfly __instance)
        {
            if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
            {
                if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                    switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                    {
                        case "Time Piece"
                            when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                            __instance.wingRight = CreateItemOverworld("timepiece", __instance);
                            break;
                        case "Yarn"
                            when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                        {
                            __instance.wingRight = CreateItemOverworld("yarn", __instance);
                            break;
                        }
                        default:
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                .HasFlag(ItemFlags.Advancement))
                            {
                                __instance.wingRight = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.wingRight = CreateItemOverworld("apUseful", __instance);
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
                                __instance.wingRight = CreateItemOverworld(trapTextures[randomIndex], __instance, -20f);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.None))
                            {
                                __instance.wingRight = CreateItemOverworld("apFiller", __instance);
                            }

                            break;
                        }
                    }
                else
                    __instance.wingRight = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
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
                        _ when ArchipelagoClient.ScoutedLocations[index + offset].ItemName.EndsWith("Trap") => 
                            CreateItemOverworld(Assets.RandomProgTrap(), __instance, -20f),
                        _ => CreateItemOverworld("apProg", __instance)
                    };
            }
        }
        
        private static bool Prefix(scrBugButterfly __instance) => AssignBugID(__instance);

        private static void Postfix(scrBugButterfly __instance)
        {
            if (!ArchipelagoMenu.cacmi) return;
            if (!bugIDs.ContainsKey(__instance))
            {
                return;
            }
            var flag = "Bug" + bugIDs[__instance];
            if (scrWorldSaveDataContainer.instance.miscFlags.Contains(flag) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                return;
            }
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            var idkAdjustment = 0;
            var seedAdjustment = 0;
            var appleAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.slotData.ContainsKey("bugsanity")) return;
            if (int.Parse(ArchipelagoData.slotData["bugsanity"].ToString()) == 0) return;
            var currentscene = SceneManager.GetActiveScene().name;
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
                if (int.Parse(ArchipelagoData.slotData["cassette_logic"].ToString()) == 1 || int.Parse(ArchipelagoData.slotData["cassette_logic"].ToString()) != 1)
                {
                    cassetteAdjustment = 14;
                }

            if (ArchipelagoData.slotData.ContainsKey("seedsanity"))
                if (int.Parse(ArchipelagoData.slotData["seedsanity"].ToString()) == 0)
                    seedAdjustment = 30;
            if (ArchipelagoData.slotData.ContainsKey("applessanity"))
                if (int.Parse(ArchipelagoData.slotData["applessanity"].ToString()) == 0)
                    appleAdjustment = 371;

            var adjustment = gardenAdjustment + snailShopAdjustment + cassetteAdjustment + seedAdjustment + appleAdjustment;
            GameObject ogQuads = null;
            if (__instance.wingRight.transform.Find("Quad") != null)
                ogQuads = __instance.wingRight;
            else if (__instance.wingLeft.transform.Find("Quad") != null)
                ogQuads = __instance.wingLeft;
            if (ogQuads == null) return;
            ogQuads.SetActive(false);
            if (__instance.functionality.transform.Find("Butterfly (mesh)") != null)
                __instance.functionality.transform.Find("Butterfly (mesh)").gameObject.SetActive(false);
            if (__instance.functionality.transform.Find("Dragonfly.Dragonfly Loop") != null)
                __instance.functionality.transform.Find("Dragonfly.Dragonfly Loop").gameObject.SetActive(false);
            if (__instance.functionality.transform.Find("Firefly_Raw.Firefly_Loop") != null)
                __instance.functionality.transform.Find("Firefly_Raw.Firefly_Loop").gameObject.SetActive(false);
            
            var index = 0;
            var offset = 0;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    var list = Locations.ScoutHCBugsList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 610 - adjustment + idkAdjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Trash Kingdom":
                {
                    var list = Locations.ScoutTTBugsList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 668 - adjustment + idkAdjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSCFBugsList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 726 - adjustment + idkAdjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathBugsList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 858 - adjustment + idkAdjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutHQBugsList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 909 - adjustment + idkAdjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
            }
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Index: {index}, Offset: {offset}, FlagID: Bug{bugIDs[__instance]}")
                .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
                .AppendLine($"Model: {__instance.wingRight.name}");
        }
    }
    
    [HarmonyPatch(typeof(scrBugButterfly), "ManagedUpdate")]
    public static class BugsanityUpdatePatch
    {
        private static bool _noticeUp;
        // private static bool Prefix(scrBugButterfly __instance)
        // {
        //     if (ArchipelagoData.slotData == null) return true;
        //     if (!ArchipelagoData.slotData.ContainsKey("bugsanity")) return true;
        //     if (int.Parse(ArchipelagoData.slotData["bugsanity"].ToString()) == 0) return true;
        //     
        //     var isAliveField = AccessTools.Field(typeof(scrBugButterfly), "isAlive");
        //     bool isAlive = (bool)isAliveField.GetValue(__instance);
        //     var positionField = AccessTools.Field(typeof(scrBugButterfly), "position");
        //     Vector3 position = (Vector3)positionField.GetValue(__instance);
        //     var localScaleField = AccessTools.Field(typeof(scrBugButterfly), "localScale");
        //     Vector3 localScale = (Vector3)localScaleField.GetValue(__instance);
        //     var _playerCameraField = AccessTools.Field(typeof(scrBugButterfly), "_playerCamera");
        //     PlayerCamera _playerCamera = (PlayerCamera)_playerCameraField.GetValue(__instance);
        //     var homescaleField = AccessTools.Field(typeof(scrBugButterfly), "homescale");
        //     float homescale = (float)homescaleField.GetValue(__instance);
        //     Transform butterflyTransform = (Transform)__instance.GetType().GetField("butterflyTransform", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
        //     
        //     float num1 = Vector3.SqrMagnitude(position - (_playerCamera.transform.position + Vector3.up));
        //     float num2 = 1.5f;
        //     if (isAlive && (double) num1 < 6400.0)
        //     {
        //         __instance.Invoke("HandleFlightV2",0f);
        //         if ((double) Vector3.SqrMagnitude(position - (MyCharacterController.position + Vector3.up)) < (double) num2 * (double) num2)
        //         {
        //             Plugin.BepinLogger.LogInfo($"Player obtained {bugIDs[__instance]}!");
        //             isAliveField.SetValue(__instance, false);
        //             __instance.functionality.SetActive(false);
        //             __instance.trigger.resetTrigger();
        //             GameObject gameObject = Object.Instantiate<GameObject>(__instance.audioOneShot);
        //             gameObject.transform.position = position;
        //             Object.Instantiate<GameObject>(__instance.particleEffect).transform.position = position;
        //             gameObject.GetComponent<scrAudioOneShot>().setup(__instance.audioClipsObtain, 0.7f, 1f);
        //         }
        //         if ((double) localScale.x < (double) homescale)
        //         {
        //             localScale += new Vector3(1f, 1f, 1f) * Time.deltaTime;
        //             localScaleField.SetValue(__instance, localScale);
        //             butterflyTransform.localScale = localScale;
        //         }
        //         else
        //         {
        //             if ((double) localScale.x == (double) homescale)
        //                 return false;
        //             localScale = new Vector3(homescale, homescale, homescale);
        //             butterflyTransform.localScale = localScale;
        //         }
        //     }
        //     else
        //     {
        //         if (isAlive) return false;
        //             
        //         if ((double) localScale.x > 0.0)
        //             localScale -= new Vector3(1f, 1f, 1f) * Time.deltaTime;
        //         butterflyTransform.localScale = localScale;
        //     }
        //
        //     return false;
        // }

        private static bool Prefix(scrBugButterfly __instance)
        {
            if (ArchipelagoData.slotData == null) return true;
            if (!ArchipelagoData.slotData.ContainsKey("bug_net")) return true;
            if (int.Parse(ArchipelagoData.slotData["bug_net"].ToString()) == 0) return true;
            
            if (ArchipelagoClient.BugnetAcquired)
            {
                __instance.flightSpeed = 0.5f;
                __instance.trigger.gameObject.SetActive(true);
                return true;
            }
            if (ArchipelagoClient.BugnetAcquired) return true;
            __instance.trigger.gameObject.SetActive(true);
            if (__instance.trigger.foundPlayer())
                __instance.StartCoroutine(ShowNotice());
            __instance.flightSpeed = 0.1f;
            __instance.Invoke("HandleFlightV2", 0f);
            return false;
        }
        
        private static IEnumerator ShowNotice()
        {
            if (_noticeUp) yield break;
            var t = Object.Instantiate(Plugin.NoticeBugNet, Plugin.NotifcationCanvas.transform);
            _noticeUp = true;
            yield return new WaitForSeconds(12.5f);
            Object.Destroy(t);
            _noticeUp = false;
        }

        private static void Postfix(scrBugButterfly __instance)
        {
            var isAliveField = AccessTools.Field(typeof(scrBugButterfly), "isAlive");
            bool isAlive = (bool)isAliveField.GetValue(__instance);
            if (isAlive)
                return;
            if (!bugIDs.ContainsKey(__instance)) return;
            var flag = "Bug" + bugIDs[__instance];
            if (!scrWorldSaveDataContainer.instance.miscFlags.Contains(flag))
            {
                if (__instance.functionality.transform.Find("Butterfly (mesh)") != null)
                    __instance.functionality.transform.Find("Butterfly (mesh)").gameObject.SetActive(false);
                if (__instance.functionality.transform.Find("Dragonfly.Dragonfly Loop") != null)
                    __instance.functionality.transform.Find("Dragonfly.Dragonfly Loop").gameObject.SetActive(false);
                if (__instance.functionality.transform.Find("Firefly_Raw.Firefly_Loop") != null)
                    __instance.functionality.transform.Find("Firefly_Raw.Firefly_Loop").gameObject.SetActive(false);
                Object.Destroy(__instance.wingRight.gameObject);
                __instance.wingRight = __instance.functionality.transform.Find("WingRight").gameObject;
                scrWorldSaveDataContainer.instance.miscFlags.Add(flag);
                //scrWorldSaveDataContainer.instance.bugAmount--;
                scrGameSaveManager.instance.SaveGame();
                scrWorldSaveDataContainer.instance.SaveWorld();
            }
        }
    }
    
    [HarmonyPatch(typeof(scrBugCatchable), "Start")]
    public static class BugsanityStartPart2
    {
        private static GameObject CreateItemPrefab(string prefabName, scrBugCatchable instance, float speed = 6f)
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

            GameObject ogQuads = null;
            if (instance.functionality.transform.Find("Sit") != null)
                ogQuads = instance.functionality.transform.Find("Sit").gameObject;
            
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

        private static GameObject CreateItemOverworld(string itemName, scrBugCatchable instance, float speed = 6f)
        {
            if (!Assets.PrefabMapping.TryGetValue(itemName, out var prefabName))
            {
                Plugin.BepinLogger.LogError($"Item name '{itemName}' not recognized.");
                return null;
            }

            return CreateItemPrefab(prefabName, instance, speed);
        }

        private static void PlaceModel(int index, int offset, scrBugCatchable __instance)
        {
            if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
            {
                if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                    switch (ArchipelagoClient.ScoutedLocations[index + offset].ItemName)
                    {
                        case "Time Piece"
                            when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                            __instance.functionality = CreateItemOverworld("timepiece", __instance);
                            break;
                        case "Yarn"
                            when ArchipelagoClient.ScoutedLocations[index + offset].ItemGame == "A Hat in Time":
                        {
                            __instance.functionality = CreateItemOverworld("yarn", __instance);
                            break;
                        }
                        default:
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                .HasFlag(ItemFlags.Advancement))
                            {
                                __instance.functionality = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.functionality = CreateItemOverworld("apUseful", __instance);
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
                                __instance.functionality = CreateItemOverworld(trapTextures[randomIndex], __instance, -20f);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags
                                     .HasFlag(ItemFlags.None))
                            {
                                __instance.functionality = CreateItemOverworld("apFiller", __instance);
                            }

                            break;
                        }
                    }
                else
                    __instance.functionality = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
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
                        _ when ArchipelagoClient.ScoutedLocations[index + offset].ItemName.EndsWith("Trap") => 
                            CreateItemOverworld(Assets.RandomProgTrap(), __instance, -20f),
                        _ => CreateItemOverworld("apProg", __instance)
                    };
            }
        }
        
        private static bool Prefix(scrBugCatchable __instance) => AssignBugID(__instance);
        
        private static void Postfix(scrBugCatchable __instance)
        {
            if (!ArchipelagoMenu.cacmi) return;
            if (!bugIDs.ContainsKey(__instance))
            {
                return;
            }
            var flag = "Bug" + bugIDs[__instance];
            if (scrWorldSaveDataContainer.instance.miscFlags.Contains(flag) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                return;
            }
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            var idkAdjustment = 0;
            var seedAdjustment = 0;
            var appleAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.slotData.ContainsKey("bugsanity")) return;
            if (int.Parse(ArchipelagoData.slotData["bugsanity"].ToString()) == 0) return;
            var currentscene = SceneManager.GetActiveScene().name;
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
                if (int.Parse(ArchipelagoData.slotData["cassette_logic"].ToString()) == 1 || int.Parse(ArchipelagoData.slotData["cassette_logic"].ToString()) != 1)
                    cassetteAdjustment = 14;
            if (ArchipelagoData.slotData.ContainsKey("seedsanity"))
                if (int.Parse(ArchipelagoData.slotData["seedsanity"].ToString()) == 0)
                    seedAdjustment = 30;
            if (ArchipelagoData.slotData.ContainsKey("applessanity"))
                if (int.Parse(ArchipelagoData.slotData["applessanity"].ToString()) == 0)
                    appleAdjustment = 371;

            var adjustment = gardenAdjustment + snailShopAdjustment + cassetteAdjustment + seedAdjustment + appleAdjustment;
            
            if (__instance.functionality.transform.Find("Sit") != null)
                __instance.functionality.transform.Find("Sit").gameObject.SetActive(false);
            
            var index = 0;
            var offset = 0;
            switch (currentscene)
            {
                case "Public Pool":
                {
                    var list = Locations.ScoutPPBugsList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 815 - adjustment + idkAdjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutHQBugsList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 909 - adjustment + idkAdjustment;
                    PlaceModel(index, offset, __instance);
                    break;
                }
            }
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Index: {index}, Offset: {offset}, FlagID: Bug{bugIDs[__instance]}")
                .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
                .AppendLine($"Model: {__instance.functionality.name}");
        }
    }

    [HarmonyPatch(typeof(scrBugCatchable), "Update")]
    public static class BugsanityPart2UpdatePatch
    {
        private static bool _noticeUp;
        private static bool Prefix(scrBugCatchable __instance)
        {
            if (ArchipelagoData.slotData == null) return true;
            if (!ArchipelagoData.slotData.ContainsKey("bug_net")) return true; 
            if (int.Parse(ArchipelagoData.slotData["bug_net"].ToString()) == 0) return true;
            
            if (ArchipelagoClient.BugnetAcquired) return true;
            if (__instance.trigger.foundPlayer())
                __instance.StartCoroutine(ShowNotice());
            return false;
        }
        private static void Postfix(scrBugCatchable __instance)
        {
            var isAliveField = AccessTools.Field(typeof(scrBugCatchable), "isAlive");
            bool isAlive = (bool)isAliveField.GetValue(__instance);
            if (isAlive)
                return;
            if (!bugIDs.ContainsKey(__instance)) return;
            var flag = "Bug" + bugIDs[__instance];
            if (!scrWorldSaveDataContainer.instance.miscFlags.Contains(flag))
            {
                scrWorldSaveDataContainer.instance.miscFlags.Add(flag);
                //scrWorldSaveDataContainer.instance.bugAmount--;
                scrGameSaveManager.instance.SaveGame();
                scrWorldSaveDataContainer.instance.SaveWorld();
            }
        }

        private static IEnumerator ShowNotice()
        {
            if (_noticeUp) yield break;
            var t = Object.Instantiate(Plugin.NoticeBugNet, Plugin.NotifcationCanvas.transform);
            _noticeUp = true;
            yield return new WaitForSeconds(12.5f);
            Object.Destroy(t);
            _noticeUp = false;
        }
    }
    
    [HarmonyPatch(typeof(scrBugCicada), "Update")]
    public static class BugsanityPart2CicadaUpdatePatch
    {
        private static void Postfix(scrBugCicada __instance)
        {
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.slotData.ContainsKey("bugsanity")) return; 
            if (int.Parse(ArchipelagoData.slotData["bugsanity"].ToString()) == 0) return;
            var key = __instance.GetComponent<scrBugCatchable>();
            if (!bugIDs.ContainsKey(key))
            {
                Plugin.BepinLogger.LogFatal("Not found in keys: "+__instance);
                return;
            }
            var flag = "Bug" + bugIDs[key];
            if (scrWorldSaveDataContainer.instance.miscFlags.Contains(flag)) return;
            __instance.visualFly.SetActive(false);
            __instance.visualSit.SetActive(false);
        }
    }
}