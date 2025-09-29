using System.Collections;
using System.Collections.Generic;
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
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            __instance.StartCoroutine(PlaceModelsAfterLoading(__instance));
        }
        
        private static IEnumerator PlaceModelsAfterLoading(scrFishInPond __instance)
        {
            yield return new WaitUntil(() => GameObjectChecker.PreviousScene != SceneManager.GetActiveScene().name);
            var flag = __instance.name;
            int scoutID;
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    ID = __instance.name switch
                    {
                        "Sea1" => 1,
                        "Sea2" => 2,
                        "Sea3" => 3,
                        "Sea4" => 4,
                        "Sea5" => 5,
                        _ => ID
                    };
                    scoutID = 200 + ID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
                case "Trash Kingdom":
                {
                    ID = __instance.name switch
                    {
                        "Sea1" => 1,
                        "Sea2" => 2,
                        "Sea3" => 3,
                        "Sea4" => 4,
                        "Sea5" => 5,
                        _ => ID
                    };
                    scoutID = 207 + ID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    ID = __instance.name switch
                    {
                        "Sea1" => 1,
                        "Sea2" => 2,
                        "Sea3" => 3,
                        "Sea4" => 4,
                        "Sea5" => 5,
                        _ => ID
                    };
                    scoutID = 212 + ID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
                case "Public Pool":
                {
                    ID = __instance.name switch
                    {
                        "Sea1" => 1,
                        "Sea2" => 2,
                        "Sea3" => 3,
                        "Sea4" => 4,
                        "Sea5" => 5,
                        _ => ID
                    };
                    scoutID = 217 + ID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
                case "The Bathhouse":
                {
                    ID = __instance.name switch
                    {
                        "Sea1" => 1,
                        "Sea2" => 2,
                        "Sea3" => 3,
                        "Sea4" => 4,
                        "Sea5" => 5,
                        _ => ID
                    };
                    scoutID = 222 + ID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
                case "Tadpole inc":
                {
                    ID = __instance.name switch
                    {
                        "Sea1" => 1,
                        "Sea2" => 2,
                        "Sea3" => 3,
                        "Sea4" => 4,
                        "Sea5" => 5,
                        _ => ID
                    };
                    scoutID = 227 + ID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
            }
            var ogQuads = __instance.transform.Find("Quad").gameObject;
            Object.Destroy(ogQuads.gameObject);
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"ID: {ID}, Flag: {flag}")
                .AppendLine($"Model: {__instance.quad.name}");
            if (__instance.name.Contains("Sea"))
                __instance.quad.transform.Find("Quads").localScale = new Vector3(0.1f, 0.1f, 0.1f);
            else
                __instance.quad.transform.Find("Quads").localScale = new Vector3(0.25f, 0.25f, 0.25f);
            __instance.quad.gameObject.SetActive(false);
        }
    }
}