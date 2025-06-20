using System.Collections;
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

public class Bonesanity
{
    public static int ID;
    private static bool _bonesanityOn;
    private static string _currentLevelName;
    private static int _currentScoutID;
    private static int _currentBoneCount;
    private static bool _answerFix;
    private static bool _isInArea;
    private static bool _playedSound;
    [HarmonyPatch(typeof(scrBone), "Start")]
    public static class BonesanityPatch
    {
        private static readonly Dictionary<scrBone, Dictionary<string, GameObject>> InstanceItemsCache = new();
        private static GameObject CreateItemPrefab(string prefabName, scrBone instance, float speed = 6f)
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

        private static GameObject CreateItemOverworld(string itemName, scrBone instance, float speed = 6f)
        {
            if (!Assets.PrefabMapping.TryGetValue(itemName, out string prefabName))
            {
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogError($"Item name '{itemName}' not recognized.");
                return null;
            }
            
            return CreateItemPrefab(prefabName, instance, speed);
        }
        
        private static void PlaceModel(int scoutID, scrBone __instance)
        {
            var scout = ArchipelagoClient.ScoutLocation(scoutID, false);
            if (scout == null) return;
            var itemGame = scout.ItemGame;
            var itemName = scout.ItemName;
            var flags = scout.Flags;
            if (itemGame != "Here Comes Niko!")
                switch (itemName)
                {
                    case "Time Piece"
                        when itemGame == "A Hat in Time":
                        __instance.quads = CreateItemOverworld("timepiece", __instance);
                        break;
                    case "Yarn"
                        when itemGame == "A Hat in Time":
                    {
                        __instance.quads = CreateItemOverworld("yarn", __instance);
                        break;
                    }
                    default:
                    {
                        if (flags.HasFlag(ItemFlags.Advancement))
                        {
                            __instance.quads = CreateItemOverworld("apProg", __instance);
                        }
                        else if (flags.HasFlag(ItemFlags.NeverExclude))
                        {
                            __instance.quads = CreateItemOverworld("apUseful", __instance);
                        }
                        else if (flags.HasFlag(ItemFlags.Trap))
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
                        else if (flags.HasFlag(ItemFlags.None))
                        {
                            __instance.quads = CreateItemOverworld("apFiller", __instance);
                        }

                        break;
                    }
                }
            else
                __instance.quads = itemName switch
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
                    _ when itemName.EndsWith("Trap") =>
                        CreateItemOverworld(Assets.RandomProgTrap(), __instance, -20f),
                    _ => CreateItemOverworld("apProg", __instance)
                };
        }
        
        private static void Postfix(scrBone __instance)
        {
            if (!ArchipelagoMenu.cacmi) return;
            var flag = __instance.name;
            if (scrWorldSaveDataContainer.instance.miscFlags.Contains(flag) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                if (scrWorldSaveDataContainer.instance.miscFlags.Contains(flag))
                    Object.Destroy(__instance.gameObject);
                return;
            }
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.slotData.ContainsKey("bonesanity")) return;
            if (int.Parse(ArchipelagoData.slotData["bonesanity"].ToString()) == 0) return;
            _bonesanityOn = true;
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            __instance.StartCoroutine(PlaceModelsAfterLoading(__instance));
        }

        private static IEnumerator PlaceModelsAfterLoading(scrBone __instance)
        {
            yield return new WaitUntil(() => GameObjectChecker.PreviousScene != SceneManager.GetActiveScene().name && !IsTransitioning());
            var flag = __instance.name;
            int scoutID;
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    ID = __instance.name switch
                    {
                        "Bone" => 1,
                        "Bone (1)" => 2,
                        "Bone (2)" => 3,
                        "Bone (3)" => 4,
                        "Bone (4)" => 5,
                        _ => ID
                    };
                    scoutID = 2200 + ID;
                    PlaceModel(scoutID, __instance);
                    break;
                }
                case "Trash Kingdom":
                {
                    ID = __instance.name switch
                    {
                        "Bone" => 1,
                        "Bone (1)" => 2,
                        "Bone (2)" => 3,
                        "Bone (3)" => 4,
                        "Bone (4)" => 5,
                        _ => ID
                    };
                    scoutID = 2205 + ID;
                    PlaceModel(scoutID, __instance);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    ID = __instance.name switch
                    {
                        "Bone" => 1,
                        "Bone (1)" => 2,
                        "Bone (2)" => 3,
                        "Bone (3)" => 4,
                        "Bone (4)" => 5,
                        _ => ID
                    };
                    scoutID = 2210 + ID;
                    PlaceModel(scoutID, __instance);
                    break;
                }
                case "Public Pool":
                {
                    ID = __instance.name switch
                    {
                        "Bone" => 1,
                        "Bone (1)" => 2,
                        "Bone (2)" => 3,
                        "Bone (3)" => 4,
                        "Bone (4)" => 5,
                        _ => ID
                    };
                    scoutID = 2215 + ID;
                    PlaceModel(scoutID, __instance);
                    break;
                }
                case "The Bathhouse":
                {
                    ID = __instance.name switch
                    {
                        "Bone" => 1,
                        "Bone (1)" => 2,
                        "Bone (2)" => 3,
                        "Bone (3)" => 4,
                        "Bone (4)" => 5,
                        _ => ID
                    };
                    scoutID = 2220 + ID;
                    PlaceModel(scoutID, __instance);
                    break;
                }
                case "Tadpole inc":
                {
                    ID = __instance.name switch
                    {
                        "Bone" => 1,
                        "Bone (1)" => 2,
                        "Bone (2)" => 3,
                        "Bone (3)" => 4,
                        "Bone (4)" => 5,
                        _ => ID
                    };
                    scoutID = 2225 + ID;
                    PlaceModel(scoutID, __instance);
                    break;
                }
            }
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            Object.Destroy(ogQuads.gameObject);
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"ID: {ID}, Flag: {flag}")
                .AppendLine($"Model: {__instance.quads.name}");
        }
        
        private static bool IsTransitioning(bool levelIntroduction = true)
        {
            return scrTrainManager.instance.isLoadingNewScene
                   || scrTransitionManager.instance.state != scrTransitionManager.States.idle
                   || !ArchipelagoClient.IsValidScene()
                   || (scrLevelIntroduction.isOn && levelIntroduction);
        }
    }

    [HarmonyPatch(typeof(scrBone), "Update")]
    public static class BonesanityCollectPatch
    {
        private static bool Prefix(scrBone __instance)
        {
            if (!_bonesanityOn) return true;
            if (__instance.trigger.foundPlayer() &&
                !scrWorldSaveDataContainer.instance.miscFlags.Contains(__instance.name))
            {
                scrWorldSaveDataContainer.instance.miscFlags.Add(__instance.name);
                Object.Instantiate<GameObject>(__instance.effect).transform.position = __instance.transform.position;
                if (__instance.gameObject != null && __instance.gameObject)
                    Object.Destroy((Object) __instance.gameObject);
            }
            if (__instance.quads != null && __instance.quads)
                __instance.quads.transform.localEulerAngles = new Vector3(0.0f, 0.0f, Mathf.Sin(Time.time * 5f) * 10f);
            __instance.transform.eulerAngles += new Vector3(0.0f, Time.deltaTime * 90f, 0.0f);
            return false;
        }
    }
    
    [HarmonyPatch(typeof(scrBoneQuest), "Update")]
    public static class BonesanityRewardPatch
    {
        private static bool Prefix(scrBoneQuest __instance)
        {
            if (ArchipelagoData.slotData == null) return true;
            if (!ArchipelagoData.slotData.ContainsKey("bonesanity")) return true;
            if (int.Parse(ArchipelagoData.slotData["bonesanity"].ToString()) != 2) return true;
            if (scrWorldSaveDataContainer.instance.miscFlags.Count(x => x.StartsWith("Bone")) >= 5 && scrWorldSaveDataContainer.instance.coinFlags.Contains("arcadeBone"))
            {
                Object.Destroy((Object)__instance.gameObject);
            }
            var currentBoxField = AccessTools.Field(typeof(scrTextbox), "currentBox");
            int currentBox = (int)currentBoxField.GetValue(scrTextbox.instance);
            if (__instance.areaTrigger.foundPlayer())
            {
                _isInArea = true;
                var totalBones = 5;
                __instance.UIhider.visible = true;
                __instance.UItext.text = $"{scrWorldSaveDataContainer.instance.miscFlags.Count(x => x.StartsWith("Bone")).ToString()} / {totalBones.ToString()}";
            }
            else
            {
                _isInArea = false;
                __instance.UIhider.visible = false;
            }
            
            if (!HasEnough() && !scrWorldSaveDataContainer.instance.coinFlags.Contains("arcadeBone"))
            {
                if (!scrTextbox.instance.isOn || scrTextbox.instance.conversation != "dogeBoneQuest")
                {
                    _answerFix = false;
                    return false;
                }
                if (currentBox != 0 || _answerFix) return false;
                var item = ArchipelagoClient.ScoutLocation(_currentScoutID);
                var itemName = item.ItemName;
                var playerName = item.Player.Name;
                var flags = item.Flags;
                string classification = "";
                if (flags.HasFlag(ItemFlags.Advancement))
                    classification = "Important";
                else if (flags.HasFlag(ItemFlags.NeverExclude))
                    classification = "Useful";
                else if (flags.HasFlag(ItemFlags.Trap))
                    classification = "#@+*$3%&!";
                else if (flags.HasFlag(ItemFlags.None))
                    classification = "Useless";
                scrTextbox.instance.conversationLocalized[0] =
                    $"Gimme {5-_currentBoneCount} more {_currentLevelName} B O N E S and I will show you {playerName}'s '{itemName}'! ({classification})";
                _answerFix = true;
                return false;
            }

            if (!_playedSound && _isInArea)
            {
                _playedSound = true;
                GameObject gameObject = Object.Instantiate<GameObject>(__instance.audioOneShot);
                gameObject.transform.position = __instance.transform.position;
                gameObject.GetComponent<scrAudioOneShot>().setup(__instance.sndBoneGotAll, 0.7f, 1f);
                Object.Instantiate<GameObject>(__instance.smoke).transform.position = __instance.coin.transform.position;
            }
            __instance.NPCQuest.SetActive(false);
            __instance.NPCPost.SetActive(true);
            if (__instance.coin != null && __instance.coin)
                __instance.coin.SetActive(true);
            if (scrWorldSaveDataContainer.instance.miscFlags.Count(x => x.StartsWith("Bone")) < 5)
            {
                if (!_isInArea)
                    __instance.UIhider.visible = false;
            }
            else
            {
                if (!_isInArea) return false;
                GameObject gameObject = Object.Instantiate<GameObject>(__instance.audioOneShot);
                gameObject.transform.position = __instance.transform.position;
                gameObject.GetComponent<scrAudioOneShot>().setup(__instance.sndBoneGotAll, 0.7f, 1f);
                __instance.UIhider.visible = false;
                Object.Destroy((Object)__instance.gameObject);
            }
            return false;
        }

        private static bool HasEnough()
        {
            var level = SceneManager.GetActiveScene().name;
            int amountOfBones = 0;
            switch (level)
            {
                case "Hairball City":
                    amountOfBones = ItemHandler.HairballBoneAmount;
                    _currentLevelName = "Hairball City";
                    _currentScoutID = 14;
                    break;
                case "Trash Kingdom":
                    amountOfBones = ItemHandler.TurbineBoneAmount;
                    _currentLevelName = "Turbine Town";
                    _currentScoutID = 25;
                    break;
                case "Salmon Creek Forest":
                    amountOfBones = ItemHandler.SalmonBoneAmount;
                    _currentLevelName = "Salmon Creek Forest";
                    _currentScoutID = 32;
                    break;
                case "Public Pool":
                    amountOfBones = ItemHandler.PoolBoneAmount;
                    _currentLevelName = "Public Pool";
                    _currentScoutID = 46;
                    break;
                case "The Bathhouse":
                    amountOfBones = ItemHandler.BathBoneAmount;
                    _currentLevelName = "Bathhouse";
                    _currentScoutID = 66;
                    break;
                case "Tadpole inc":
                    amountOfBones = ItemHandler.TadpoleBoneAmount;
                    _currentLevelName = "Tadpole HQ";
                    _currentScoutID = 77;
                    break;
                default:
                    return false;
            }
            _currentBoneCount = amountOfBones;
            return amountOfBones >= 5;
        }
    }

    [HarmonyPatch(typeof(scrArcadeManager), "Update")]
    public static class BlippyStayWithUs
    {
        private static bool Prefix(scrArcadeManager __instance)
        {
            var trans = GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<scrTransitionManager>();
            var completed = (bool)AccessTools.Field(typeof(scrArcadeManager), "completed").GetValue(__instance);
            var hasRemoved = (bool)AccessTools.Field(typeof(scrArcadeManager), "hasRemoved").GetValue(__instance);
            if (__instance.insideTrigger.foundPlayer())
            {
                __instance.SecretLevel.SetActive(true);
            }
            else
            {
                int state = (int) trans.state;
            }

            if (!completed)
            {
                foreach (var t in __instance.objectsToTurnOn)
                    t.SetActive(false);
            }

            if (completed)
            {
                for (int index = 0; index < __instance.objectsToTurnOff.Count; ++index)
                {
                    if (index is 2 or 1)
                    {
                        __instance.objectsToTurnOff[index].SetActive(true);
                        continue;
                    }
                        
                    __instance.objectsToTurnOff[index].SetActive(false);
                }
            }
            if (!completed && (Object) __instance.coin == (Object) null && MyCharacterController.instance.state != MyCharacterController.States.Occupied)
            {
                if (__instance.insideTrigger.foundPlayer())
                {
                    __instance.teleporter.Teleport(true);
                    scrAudioMixerMaster.instance.ChangeState(scrAudioMixerMaster.States.normal);
                }
                AccessTools.Field(typeof(scrArcadeManager), "completed").SetValue(__instance, true);
            }
            if (!completed || hasRemoved || __instance.teleporter.isTeleporting)
                return false;
            __instance.Invoke("Complete", 0f);
            return false;
        }
    }
}