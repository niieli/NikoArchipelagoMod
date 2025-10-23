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
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            var idkAdjustment = 0;
            var seedAdjustment = 0;
            var appleAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.slotData.ContainsKey("bugsanity")) return;
            if (!ArchipelagoData.Options.Bugsanity) return;
            var currentscene = SceneManager.GetActiveScene().name;
            __instance.enabled = true;
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
                if (!ArchipelagoData.Options.GarysGarden)
                {
                    gardenAdjustment = 13;
                    idkAdjustment = 2;
                }
            if (ArchipelagoData.slotData.ContainsKey("snailshop"))
                if (!ArchipelagoData.Options.Snailshop)
                    snailShopAdjustment = 16;
            if (ArchipelagoData.slotData.ContainsKey("cassette_logic"))
                if (ArchipelagoData.Options.Cassette == ArchipelagoOptions.CassetteMode.Progressive || ArchipelagoData.Options.Cassette != ArchipelagoOptions.CassetteMode.Progressive)
                {
                    cassetteAdjustment = 14;
                }

            if (ArchipelagoData.slotData.ContainsKey("seedsanity"))
                if (ArchipelagoData.Options.Seedsanity == ArchipelagoOptions.InsanityLevel.Vanilla)
                    seedAdjustment = 30;
            if (ArchipelagoData.slotData.ContainsKey("applessanity"))
                if (!ArchipelagoData.Options.Applesanity)
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
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "Trash Kingdom":
                {
                    var list = Locations.ScoutTTBugsList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 668 - adjustment + idkAdjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSCFBugsList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 726 - adjustment + idkAdjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathBugsList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 858 - adjustment + idkAdjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutHQBugsList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 909 - adjustment + idkAdjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
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
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            var idkAdjustment = 0;
            var seedAdjustment = 0;
            var appleAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.slotData.ContainsKey("bugsanity")) return;
            if (!ArchipelagoData.Options.Bugsanity) return;
            var currentscene = SceneManager.GetActiveScene().name;
            __instance.enabled = true;
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
                if (!ArchipelagoData.Options.GarysGarden)
                {
                    gardenAdjustment = 13;
                    idkAdjustment = 2;
                }
            if (ArchipelagoData.slotData.ContainsKey("snailshop"))
                if (!ArchipelagoData.Options.Snailshop)
                    snailShopAdjustment = 16;
            if (ArchipelagoData.slotData.ContainsKey("cassette_logic"))
                if (ArchipelagoData.Options.Cassette == ArchipelagoOptions.CassetteMode.Progressive || ArchipelagoData.Options.Cassette != ArchipelagoOptions.CassetteMode.Progressive)
                    cassetteAdjustment = 14;
            if (ArchipelagoData.slotData.ContainsKey("seedsanity"))
                if (ArchipelagoData.Options.Seedsanity == ArchipelagoOptions.InsanityLevel.Vanilla)
                    seedAdjustment = 30;
            if (ArchipelagoData.slotData.ContainsKey("applessanity"))
                if (!ArchipelagoData.Options.Applesanity)
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
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutHQBugsList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 909 - adjustment + idkAdjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
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