using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class FishInPondPatch
{
    public static int ID;
    
    [HarmonyPatch(typeof(scrFishInPond), "Start")]
    public static class FishBegone
    {
        private static void Postfix(scrFishInPond __instance)
        {
            if (ArchipelagoData.slotData == null) return;
            if (ArchipelagoData.Options.Fishsanity == ArchipelagoOptions.InsanityLevel.Vanilla) return;
            if (GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID())) return;
            var currentscene = SceneManager.GetActiveScene().name;
            var list = currentscene switch
            {
                "Hairball City" => Locations.ScoutHCFishList,
                "Trash Kingdom" => Locations.ScoutTTFishList,
                "Salmon Creek Forest" => Locations.ScoutSCFFishList,
                "Public Pool" => Locations.ScoutPPFishList,
                "The Bathhouse" => Locations.ScoutBathFishList,
                "Tadpole inc" => Locations.ScoutHQFishList,
                _ => null
            };

            if (list == null)
            {
                Plugin.BepinLogger.LogError($"Couldn't find locations for {__instance.name} | Scene: {currentscene} ");
                return;
            }
            
            var pair = list.FirstOrDefault(p => p.Value.Replace("fish", "Sea") == __instance.name);

            if (pair.Key == 0)
                return;

            var locationId = pair.Key;

            if (!ArchipelagoClient.ScoutedLocations.TryGetValue(locationId, out var scoutedItemInfo))
                return;

            PlaceModelHelper.PlaceModel(scoutedItemInfo, __instance);
            
            var ogQuads = __instance.transform.Find("Quad").gameObject;
            Object.Destroy(ogQuads.gameObject);
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Flag: {__instance.name}")
                .AppendLine($"Item: {scoutedItemInfo.ItemName}")
                .AppendLine($"Location: {scoutedItemInfo.LocationName}")
                .AppendLine($"LocationID: {scoutedItemInfo.LocationId}")
                .AppendLine($"Model: {__instance.quad.name}");
            __instance.quad.transform.Find("Quads").localScale = __instance.name.Contains("Sea") ? new Vector3(0.1f, 0.1f, 0.1f) : new Vector3(0.25f, 0.25f, 0.25f);
            __instance.quad.gameObject.SetActive(false);
        }
    }
}