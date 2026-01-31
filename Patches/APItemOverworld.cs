using System.Collections;
using System.Linq;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace NikoArchipelago.Patches;

public class APItemOverworld
{
    [HarmonyPatch(typeof(scrCassette), "Start")]
    public static class CassetteTexturePatch
    {
        private static void Postfix(scrCassette __instance)
        {
            if (!ArchipelagoMenu.cacmi) return;
            var flagField = AccessTools.Field(typeof(scrCassette), "flag");
            var flag = (string)flagField.GetValue(__instance);
            if (scrWorldSaveDataContainer.instance.cassetteFlags.Contains(flag) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                return;
            }
            
            
            var currentscene = SceneManager.GetActiveScene().name;
            var list = currentscene switch
            {
                "Hairball City" => Locations.ScoutHCCassetteList,
                "Trash Kingdom" => Locations.ScoutTTCassetteList,
                "Salmon Creek Forest" => Locations.ScoutSCFCassetteList,
                "Public Pool" => Locations.ScoutPPCassetteList,
                "The Bathhouse" => Locations.ScoutBathCassetteList,
                "Tadpole inc" => Locations.ScoutHQCassetteList,
                "GarysGarden" => Locations.ScoutGardenCassetteList,
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
            
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            Object.Destroy(ogQuads.gameObject);
            
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Flag: {flag}")
                .AppendLine($"Item: {scoutedItemInfo.ItemName}")
                .AppendLine($"Location: {scoutedItemInfo.LocationName}")
                .AppendLine($"LocationID: {scoutedItemInfo.LocationId}")
                .AppendLine($"Model: {__instance.quads.name}");
        }
    }

    [HarmonyPatch(typeof(scrCoin), "Start")]
    public static class CoinTexturePatch
    {
        private static void Postfix(scrCoin __instance)
        {
            if (!ArchipelagoMenu.cacmi) return;
            if (scrWorldSaveDataContainer.instance.coinFlags.Contains(__instance.myFlag) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                return;
            }
            
            var currentscene = SceneManager.GetActiveScene().name;
            
            var list = currentscene switch
            {
                "Hairball City" => Locations.ScoutHCCoinList,
                "Trash Kingdom" => Locations.ScoutTTCoinList,
                "Salmon Creek Forest" => Locations.ScoutSCFCoinList,
                "Public Pool" => Locations.ScoutPPCoinList,
                "The Bathhouse" => Locations.ScoutBathCoinList,
                "Tadpole inc" => Locations.ScoutHQCoinList,
                _ => null
            };

            if (list == null)
            {
                Plugin.BepinLogger.LogError($"Couldn't find locations for {__instance.myFlag} | Scene: {currentscene} ");
                return;
            }
            
            var pair = list.FirstOrDefault(p => p.Value == __instance.myFlag);

            if (pair.Key == 0)
                return;

            var locationId = pair.Key;

            if (!ArchipelagoClient.ScoutedLocations.TryGetValue(locationId, out var scoutedItemInfo))
                return;

            PlaceModelHelper.PlaceModel(scoutedItemInfo, __instance);
            
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            ogQuads.SetActive(false);
            
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Flag: {__instance.myFlag}")
                .AppendLine($"Item: {scoutedItemInfo.ItemName}")
                .AppendLine($"Location: {scoutedItemInfo.LocationName}")
                .AppendLine($"LocationID: {scoutedItemInfo.LocationId}")
                .AppendLine($"Model: {__instance.quads.name}");
        }
    }

    [HarmonyPatch(typeof(scrKey), "Start")]
    public static class KeyTexturePatch
    {
        private static void Postfix(scrKey __instance)
        {
            if (!ArchipelagoMenu.kalmi) return;
            if (scrWorldSaveDataContainer.instance.miscFlags.Contains(__instance.flag) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                return;
            }


            var currentscene = SceneManager.GetActiveScene().name;
            var list = Locations.ScoutKeyList;
            
            var pair = list.FirstOrDefault(p => p.Value == __instance.flag);

            if (pair.Key == 0)
                return;

            var locationId = pair.Key;

            if (!ArchipelagoClient.ScoutedLocations.TryGetValue(locationId, out var scoutedItemInfo))
                return;

            PlaceModelHelper.PlaceModel(scoutedItemInfo, __instance);
            
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            ogQuads.SetActive(false);
            
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Flag: {__instance.flag}")
                .AppendLine($"Item: {scoutedItemInfo.ItemName}")
                .AppendLine($"Location: {scoutedItemInfo.LocationName}")
                .AppendLine($"LocationID: {scoutedItemInfo.LocationId}")
                .AppendLine($"Model: {__instance.quads.name}");
        }
    }
    
    [HarmonyPatch(typeof(scrEnvelope), "Start")]
    public static class LetterTexturePatch
    {
        
        private static void Postfix(scrEnvelope __instance)
        {
            if (!ArchipelagoMenu.kalmi) return;
            var flag = __instance.myLetter.key;
            if (scrWorldSaveDataContainer.instance.miscFlags.Contains(flag) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                if (scrWorldSaveDataContainer.instance.miscFlags.Contains(flag))
                    Object.Destroy(__instance.gameObject);
                return;
            }
            
            var list = Locations.ScoutLetterList;
            
            var pair = list.FirstOrDefault(p => p.Value == flag);

            if (pair.Key == 0)
                return;

            var locationId = pair.Key;

            if (!ArchipelagoClient.ScoutedLocations.TryGetValue(locationId, out var scoutedItemInfo))
                return;

            PlaceModelHelper.PlaceModel(scoutedItemInfo, __instance);
            
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            Object.Destroy(ogQuads.gameObject);
            
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Flag: {flag}")
                .AppendLine($"Item: {scoutedItemInfo.ItemName}")
                .AppendLine($"Location: {scoutedItemInfo.LocationName}")
                .AppendLine($"LocationID: {scoutedItemInfo.LocationId}")
                .AppendLine($"Model: {__instance.quads.name}");
        }
    }
    
    [HarmonyPatch(typeof(scrList), "Start")]
    public static class ContactListTexturePatch
    {
        private static void Postfix(scrList __instance)
        {
            if (!ArchipelagoMenu.kalmi) return;
            if (GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                return;
            }
            
            var flag = "";
            var currentscene = SceneManager.GetActiveScene().name;
            if (currentscene == "Salmon Creek Forest")
            {
                flag = "CL1 Obtained";
            } else if (currentscene == "Tadpole inc")
            {
                flag = "CL2 Obtained";
            }
            var list = Locations.ScoutContactList;
            
            var pair = list.FirstOrDefault(p => p.Value == flag);

            if (pair.Key == 0)
                return;

            var locationId = pair.Key;

            if (!ArchipelagoClient.ScoutedLocations.TryGetValue(locationId, out var scoutedItemInfo))
                return;

            PlaceModelHelper.PlaceModel(scoutedItemInfo, __instance);
            
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            ogQuads.SetActive(false);
            
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Flag: {flag}")
                .AppendLine($"Item: {scoutedItemInfo.ItemName}")
                .AppendLine($"Location: {scoutedItemInfo.LocationName}")
                .AppendLine($"LocationID: {scoutedItemInfo.LocationId}")
                .AppendLine($"Model: {__instance.quads.name}");
        }
    }
    
    [HarmonyPatch(typeof(scrSunflowerSeed), "Start")]
    public static class SunflowerSeedTexturePatch
    {
        private static void Postfix(scrSunflowerSeed __instance)
        {
            if (!ArchipelagoMenu.cacmi) return;
            if (scrWorldSaveDataContainer.instance.miscFlags.Contains(__instance.name) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                return;
            }
            
            var currentscene = SceneManager.GetActiveScene().name;
            var list = currentscene switch
            {
                "Hairball City" => Locations.ScoutHCSeedsList,
                "Salmon Creek Forest" => Locations.ScoutSCFSeedsList,
                "The Bathhouse" => Locations.ScoutBathSeedsList,
                _ => null
            };

            if (list == null)
            {
                Plugin.BepinLogger.LogError($"Couldn't find locations for {__instance.name} | Scene: {currentscene} ");
                return;
            }
            
            var pair = list.FirstOrDefault(p => p.Value == __instance.name);

            if (pair.Key == 0)
                return;

            var locationId = pair.Key;

            if (!ArchipelagoClient.ScoutedLocations.TryGetValue(locationId, out var scoutedItemInfo))
                return;

            PlaceModelHelper.PlaceModel(scoutedItemInfo, __instance);
            __instance.quads.transform.localScale = new Vector3(1.15f, 1f, 1);
            
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            ogQuads.SetActive(false);
            
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Flag: {__instance.name}")
                .AppendLine($"Item: {scoutedItemInfo.ItemName}")
                .AppendLine($"Location: {scoutedItemInfo.LocationName}")
                .AppendLine($"LocationID: {scoutedItemInfo.LocationId}")
                .AppendLine($"Model: {__instance.quads.name}");
        }
    }
}