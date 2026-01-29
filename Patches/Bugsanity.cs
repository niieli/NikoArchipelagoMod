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
    public static bool NoticeUp;
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
            if (scene == "Salmon Creek Forest")
            {
                if (bugIDs[bug] == 47)
                    bug.transform.position += new Vector3(0, 12f, 0f);
                if (bugIDs[bug] == 40)
                    bug.transform.position = new Vector3(-31f, 160f, -135f);
                if (bugIDs[bug] == 71)
                    bug.transform.position = new Vector3(-105f, 131f, 100f);
                if (bugIDs[bug] == 49)
                    bug.transform.position = new Vector3(-110f, 129f, 75.8f);
                if (bugIDs[bug] == 1)
                    bug.transform.position = new Vector3(-105.7f, 130f, 87f);
                if (bugIDs[bug] == 54)
                    bug.transform.position = new Vector3(2.6f, 194.5f, -78.7f);
                if (bugIDs[bug] == 84)
                    bug.transform.position = new Vector3(-55.82f, 178f, -50.7f);
                if (bugIDs[bug] == 76)
                    bug.transform.position = new Vector3(-53.82f, 184f, -48.7f);
                if (bugIDs[bug] == 42)
                    bug.transform.position = new Vector3(-28.6f, 181.5f, -80.4f);
                if (bugIDs[bug] == 81)
                    bug.transform.position = new Vector3(-60f, 178f, -63.9f);
            }
        }
        return true;
    }
    
    [HarmonyPatch(typeof(scrBugButterfly), "Start")]
    public static class BugsanityStart
    {
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
            
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.Options.Bugsanity) return;
            var currentscene = SceneManager.GetActiveScene().name;
            __instance.enabled = true;
            
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
            
            var list = currentscene switch
            {
                "Hairball City" => Locations.ScoutHCBugsList,
                "Trash Kingdom" => Locations.ScoutTTBugsList,
                "Salmon Creek Forest" => Locations.ScoutSCFBugsList,
                "Public Pool" => Locations.ScoutPPBugsList,
                "The Bathhouse" => Locations.ScoutBathBugsList,
                "Tadpole inc" => Locations.ScoutHQBugsList,
                _ => null
            };

            if (list == null)
            {
                Plugin.BepinLogger.LogError($"Couldn't find locations for {flag} | Scene: {currentscene} ");
                return;
            }
            
            var pair = list.FirstOrDefault(p => p.Value == flag);

            if (pair.Key == 0)
                return;

            var locationId = pair.Key;

            if (!ArchipelagoClient.ScoutedLocations.TryGetValue(locationId, out var scoutedItemInfo))
                return;

            PlaceModelHelper.PlaceModel(scoutedItemInfo, __instance);
            
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Flag: {flag}")
                .AppendLine($"Item: {scoutedItemInfo.ItemName}")
                .AppendLine($"Location: {scoutedItemInfo.LocationName}")
                .AppendLine($"LocationID: {scoutedItemInfo.LocationId}")
                .AppendLine($"Model: {__instance.wingRight.name}");
        }
    }
    
    [HarmonyPatch(typeof(scrBugButterfly), "ManagedUpdate")]
    public static class BugsanityUpdatePatch
    {
        private static bool Prefix(scrBugButterfly __instance)
        {
            if (ArchipelagoData.slotData == null) return true;
            if (!ArchipelagoData.slotData.ContainsKey("bug_net")) return true;
            if (!ArchipelagoData.Options.BugNet) return true;
            
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
            if (NoticeUp || !SavedData.Instance.Notices) yield break;
            var t = Object.Instantiate(Assets.NoticeBugNet, Plugin.NotifcationCanvas.transform);
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
            
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.Options.Bugsanity) return;
            var currentscene = SceneManager.GetActiveScene().name;
            __instance.enabled = true;
            
            if (__instance.functionality.transform.Find("Sit") != null)
                __instance.functionality.transform.Find("Sit").gameObject.SetActive(false);
            
            var list = currentscene switch
            {
                "Public Pool" => Locations.ScoutPPBugsList,
                "Tadpole inc" => Locations.ScoutHQBugsList,
                _ => null
            };

            if (list == null)
            {
                Plugin.BepinLogger.LogError($"Couldn't find locations for {flag} | Scene: {currentscene} ");
                return;
            }
            
            var pair = list.FirstOrDefault(p => p.Value == flag);

            if (pair.Key == 0)
                return;

            var locationId = pair.Key;

            if (!ArchipelagoClient.ScoutedLocations.TryGetValue(locationId, out var scoutedItemInfo))
                return;

            PlaceModelHelper.PlaceModel(scoutedItemInfo, __instance);
            
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"FlagID: Bug{bugIDs[__instance]}")
                .AppendLine($"Item: {scoutedItemInfo.ItemName}")
                .AppendLine($"Location: {scoutedItemInfo.LocationName}")
                .AppendLine($"LocationID: {scoutedItemInfo.LocationId}")
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
            if (!ArchipelagoData.Options.BugNet) return true;
            
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
                scrGameSaveManager.instance.SaveGame();
                scrWorldSaveDataContainer.instance.SaveWorld();
            }
        }

        private static IEnumerator ShowNotice()
        {
            if (_noticeUp || !SavedData.Instance.Notices) yield break;
            var t = Object.Instantiate(Assets.NoticeBugNet, Plugin.NotifcationCanvas.transform);
            _noticeUp = true;
            var time = 0f;
            while (time < 60f)
            {
                time += Time.deltaTime;
                yield return null;
            }
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
            if (!ArchipelagoData.Options.Bugsanity) return;
            var key = __instance.GetComponent<scrBugCatchable>();
            if (!bugIDs.ContainsKey(key))
            {
                Plugin.BepinLogger.LogFatal("Not found in keys: "+__instance);
                return;
            }
            if (__instance.transform.childCount < 2) return;
            __instance.visualFly.SetActive(false);
            __instance.visualSit.SetActive(false);
        }
    }
}