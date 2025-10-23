using System.Collections;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class GardenSeedsanity
{
    public static int ID;

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
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            __instance.StartCoroutine(PlaceModelsAfterLoading(__instance));
        }
        
        private static IEnumerator PlaceModelsAfterLoading(scrGardenSeed __instance)
        {
            yield return new WaitUntil(() => GameObjectChecker.PreviousScene != SceneManager.GetActiveScene().name);
            var flag = __instance.name;
            ID = __instance.name switch
            {
                "GardenSeed 1" => 1,
                "GardenSeed 2" => 2,
                "GardenSeed 3" => 3,
                "GardenSeed 4" => 4,
                "GardenSeed 5" => 5,
                "GardenSeed 6" => 6,
                "GardenSeed 7" => 7,
                "GardenSeed 8" => 8,
                "GardenSeed 9" => 9,
                "GardenSeed 10" => 10,
                _ => ID
            };
            var scoutID = 2300 + ID;
            PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            Object.Destroy(ogQuads.gameObject);
            __instance.quads.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"ID: {ID}, Flag: {flag}")
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