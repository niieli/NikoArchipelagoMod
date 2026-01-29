using System.Linq;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class GardenSeedsanity
{
    [HarmonyPatch(typeof(scrGardenSeed), "Start")]
    public static class GardenSeedStartPatch
    {
        private static void Postfix(scrGardenSeed __instance)
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
            if (ArchipelagoData.Options.GoalCompletion != ArchipelagoOptions.GoalCompletionMode.Garden) return;
            var list = Locations.ScoutGardenSeedsList;
            
            var pair = list.FirstOrDefault(p => p.Value == flag);

            if (pair.Key == 0)
                return;

            var locationId = pair.Key;

            if (!ArchipelagoClient.ScoutedLocations.TryGetValue(locationId, out var scoutedItemInfo))
                return;

            PlaceModelHelper.PlaceModel(scoutedItemInfo, __instance);
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            Object.Destroy(ogQuads.gameObject);
            __instance.quads.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Flag: {flag}")
                .AppendLine($"Item: {scoutedItemInfo.ItemName}")
                .AppendLine($"Location: {scoutedItemInfo.LocationName}")
                .AppendLine($"LocationID: {scoutedItemInfo.LocationId}")
                .AppendLine($"Model: {__instance.quads.name}");
        }
    }

    [HarmonyPatch(typeof(scrGardenSeed), "Update")]
    public static class GardenSeedUpdatePatch
    {
        private static bool Prefix(scrGardenSeed __instance)
        {
            if (ArchipelagoData.slotData == null) return true;
            if (ArchipelagoData.Options.GoalCompletion != ArchipelagoOptions.GoalCompletionMode.Garden) return true;
            if (!__instance.trigger.foundPlayer())
                return false;
            if (!scrWorldSaveDataContainer.instance.miscFlags.Contains(__instance.name))
            {
                scrWorldSaveDataContainer.instance.miscFlags.Add(__instance.name);
                scrWorldSaveDataContainer.instance.SaveWorld();
            }
            MyCharacterController.instance.currentDaisy = __instance.targetDaisy;
            MyCharacterController.instance.BackToDaisy();
            Object.Destroy((Object) __instance.gameObject);
            return false;
        }
    }

}