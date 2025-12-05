using System.Collections;
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
    private static bool needsBasket = true;
    [HarmonyPatch(typeof(scrApple), "Start")]
    public static class ApplesanityStart
    {
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
            if (!ArchipelagoData.Options.Applesanity) return;
            applesanityOn = true;
            var currentscene = SceneManager.GetActiveScene().name;
            if (currentscene == "Home") return; // Do not interact with apples in home, will need to be removed/checked for conditions.
            __instance.enabled = true;
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
                if (!ArchipelagoData.Options.GarysGarden)
                {
                    gardenAdjustment = 13;
                    idkAdjustment = 2;
                }
            if (ArchipelagoData.Options.GoalCompletion == ArchipelagoOptions.GoalCompletionMode.Garden)
                gardenAdjustment += 1;
            if (ArchipelagoData.slotData.ContainsKey("snailshop"))
                if (!ArchipelagoData.Options.Snailshop)
                    snailShopAdjustment = 16;
            if (ArchipelagoData.slotData.ContainsKey("cassette_logic"))
                if (ArchipelagoData.Options.Cassette == ArchipelagoOptions.CassetteMode.Progressive)
                    cassetteAdjustment = 14;
            if (ArchipelagoData.slotData.ContainsKey("seedsanity"))
                if (ArchipelagoData.Options.Seedsanity == ArchipelagoOptions.InsanityLevel.Vanilla)
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
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "Trash Kingdom":
                {
                    __instance.transform.position += new Vector3(0, -0.2f, 0);
                    var list = Locations.ScoutTTApplesList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 257 - adjustment + idkAdjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSFCApplesList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 290 - adjustment + idkAdjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
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
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathApplesList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 510 - adjustment + idkAdjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutHQApplesList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 582 - adjustment + idkAdjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
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
            if (!ArchipelagoData.Options.AppleBasket)
            {
                needsBasket = false;
                return true;
            }
            
            if (ArchipelagoClient.AppleBasketAcquired) return true;
            __instance.StartCoroutine(ShowNotice());
            return false;
        }
        
        private static IEnumerator ShowNotice()
        {
            if (NoticeUp || !SavedData.Instance.Notices) yield break;
            var t = Object.Instantiate(Assets.NoticeAppleBasket, Plugin.NotifcationCanvas.transform);
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
            if (!ArchipelagoClient.AppleBasketAcquired && needsBasket) return;
            if (!applesanityOn) return;
            if (!ApplesanityStart.appleIDs.ContainsKey(__instance))
            {
                return;
            }
            Plugin.BepinLogger.LogInfo($"Applesanity: {applesanityOn} | AppleBasket: {ArchipelagoClient.AppleBasketAcquired} | NeedsBasket: {needsBasket}");
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