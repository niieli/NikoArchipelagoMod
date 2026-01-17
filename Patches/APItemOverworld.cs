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
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.Options.GarysGarden)
            {
                gardenAdjustment = 3;
            }
            if (ArchipelagoData.Options.GoalCompletion == ArchipelagoOptions.GoalCompletionMode.Garden)
                gardenAdjustment += 1;
            if (!ArchipelagoData.Options.Snailshop)
            {
                snailShopAdjustment = 16;
            }
            if (ArchipelagoData.Options.Cassette == ArchipelagoOptions.CassetteMode.Progressive)
                cassetteAdjustment = 14; 
            var adjustment = gardenAdjustment + snailShopAdjustment + cassetteAdjustment;
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            Object.Destroy(ogQuads.gameObject);

            var index = 0;
            var offset = 0;
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    var list = Locations.ScoutHCCassetteList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 101 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "Trash Kingdom":
                {
                    var list = Locations.ScoutTTCassetteList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 111 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSFCCassetteList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 121 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "Public Pool":
                {
                    var list = Locations.ScoutPPCassetteList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 132 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathCassetteList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 142 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutHQCassetteList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 152 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "GarysGarden":
                {
                    var list = Locations.ScoutGardenCassetteList.ToList();
                    index = list.FindIndex(pair => pair.Value == flag);
                    offset = 162;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
            }
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Index: {index}, Offset: {offset}, Flag: {flag}")
                .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
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
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.Options.GarysGarden)
            {
                gardenAdjustment = 2;
            }
            if (!ArchipelagoData.Options.Snailshop)
            {
                snailShopAdjustment = 16;
            }
            if (ArchipelagoData.Options.Cassette == ArchipelagoOptions.CassetteMode.Progressive)
                cassetteAdjustment = 14;    
            var adjustment = gardenAdjustment + snailShopAdjustment + cassetteAdjustment;
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            ogQuads.SetActive(false);

            var index = 0;
            var offset = 0;
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    var list = Locations.ScoutHCCoinList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    offset = 36 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "Trash Kingdom":
                {
                    var list = Locations.ScoutTTCoinList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    offset = 49 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSFCCoinList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    offset = 58 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "Public Pool":
                {
                    var list = Locations.ScoutPPCoinList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    offset = 72 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathCoinList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    offset = 80 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutHQCoinList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    offset = 92 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
            }
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Index: {index}, Offset: {offset}, Flag: {__instance.myFlag}")
                .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
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
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.Options.GarysGarden)
                gardenAdjustment = 13;
            if (ArchipelagoData.Options.GoalCompletion == ArchipelagoOptions.GoalCompletionMode.Garden)
                gardenAdjustment += 1;
            if (!ArchipelagoData.Options.Snailshop)
                snailShopAdjustment = 16;
            if (ArchipelagoData.Options.Cassette == ArchipelagoOptions.CassetteMode.Progressive)
                cassetteAdjustment = 14;    
            var adjustment = gardenAdjustment + snailShopAdjustment + cassetteAdjustment;
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            ogQuads.SetActive(false);

            var list = Locations.ScoutKeyList.ToList();
            var index = list.FindIndex(pair => pair.Value == __instance.flag);
            int offset = 172 - adjustment;
            PlaceModelHelper.PlaceModel(index, offset, __instance);
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Index: {index}, Offset: {offset}, Flag: {__instance.flag}")
                .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
                .AppendLine($"Model: {__instance.quads.name}");
        }
    }
    
    private static int _letterID;
    [HarmonyPatch(typeof(scrEnvelope), "Start")]
    public static class LetterTexturePatch
    {
        
        private static void Postfix(scrEnvelope __instance)
        {
            if (!ArchipelagoMenu.kalmi) return;
            if (ArchipelagoData.slotData == null) return;
            var flag = __instance.myLetter.key;
            if (scrWorldSaveDataContainer.instance.miscFlags.Contains(flag) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                if (scrWorldSaveDataContainer.instance.miscFlags.Contains(flag))
                    Object.Destroy(__instance.gameObject);
                return;
            }
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            __instance.StartCoroutine(PlaceModelsAfterLoading(__instance));
        }
        
        private static IEnumerator PlaceModelsAfterLoading(scrEnvelope __instance)
        {
            yield return new WaitUntil(() => GameObjectChecker.PreviousScene != SceneManager.GetActiveScene().name);
            var flag = __instance.myLetter.key;
            int scoutID;
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Home":
                    scoutID = 80;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                case "Hairball City":
                {
                    _letterID = flag switch
                    {
                        "letter7" => 0,
                        "letter1" => 1,
                        _ => _letterID
                    };
                    scoutID = 81 + _letterID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
                case "Trash Kingdom":
                {
                    _letterID = flag switch
                    {
                        "letter2" => 0,
                        "letter8" => 1,
                        _ => _letterID
                    };
                    scoutID = 83 + _letterID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    _letterID = flag switch
                    {
                        "letter9" => 0,
                        "letter3" => 1,
                        _ => _letterID
                    };
                    scoutID = 85 + _letterID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
                case "Public Pool":
                {
                    _letterID = flag switch
                    {
                        "letter10" => 0,
                        "letter4" => 1,
                        _ => _letterID
                    };
                    scoutID = 87 + _letterID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
                case "The Bathhouse":
                {
                    _letterID = flag switch
                    {
                        "letter11" => 0,
                        "letter5" => 1,
                        _ => _letterID
                    };
                    scoutID = 89 + _letterID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
                case "Tadpole inc":
                {
                    scoutID = 250;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
            }
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            Object.Destroy(ogQuads.gameObject);
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"ID: {_letterID}, Flag: {flag}")
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
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.Options.GarysGarden)
                gardenAdjustment = 13;
            if (ArchipelagoData.Options.GoalCompletion == ArchipelagoOptions.GoalCompletionMode.Garden)
                gardenAdjustment += 1;
            if (!ArchipelagoData.Options.Snailshop)
                snailShopAdjustment = 16;
            if (ArchipelagoData.Options.Cassette == ArchipelagoOptions.CassetteMode.Progressive)
                cassetteAdjustment = 14;
            var adjustment = gardenAdjustment + snailShopAdjustment + cassetteAdjustment;
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            ogQuads.SetActive(false);
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutContactList.ToList();
                    var index = list.FindIndex(pair => pair.Value == "CL1 Obtained");
                    var offset = 193 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
                    GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                        .AppendLine($"Index: {index}, Offset: {offset}, Flag: CL1 Obtained")
                        .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                        .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                        .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
                        .AppendLine($"Model: {__instance.quads.name}");
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutContactList.ToList();
                    var index = list.FindIndex(pair => pair.Value == "CL2 Obtained");
                    var offset = 193 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
                    GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                        .AppendLine($"Index: {index}, Offset: {offset}, Flag: CL2 Obtained")
                        .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                        .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                        .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
                        .AppendLine($"Model: {__instance.quads.name}");
                    break;
                }
            }
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
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (!ArchipelagoData.slotData.ContainsKey("seedsanity")) return;
            if (ArchipelagoData.Options.Seedsanity == ArchipelagoOptions.InsanityLevel.Vanilla) return;
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
            {
                if (!ArchipelagoData.Options.GarysGarden)
                {
                    gardenAdjustment = 13;
                }
            }
            if (ArchipelagoData.Options.GoalCompletion == ArchipelagoOptions.GoalCompletionMode.Garden)
                gardenAdjustment += 1;
            if (ArchipelagoData.slotData.ContainsKey("snailshop"))
            {
                if (!ArchipelagoData.Options.Snailshop)
                {
                    snailShopAdjustment = 16;
                }
            }
            if (ArchipelagoData.slotData.ContainsKey("cassette_logic"))
                if (ArchipelagoData.Options.Cassette == ArchipelagoOptions.CassetteMode.Progressive)
                    cassetteAdjustment = 14;
            var adjustment = gardenAdjustment + snailShopAdjustment + cassetteAdjustment;
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            ogQuads.SetActive(false);

            var index = 0;
            var offset = 0;
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    var list = Locations.ScoutHCSeedsList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.name);
                    offset = 195 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSFCSeedsList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.name);
                    offset = 205 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathSeedsList.ToList();
                    index = list.FindIndex(pair => pair.Value == __instance.name);
                    offset = 215 - adjustment;
                    PlaceModelHelper.PlaceModel(index, offset, __instance);
                    break;
                }
            }
            __instance.quads.transform.localScale = new Vector3(1.15f, 1f, 1);
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"Index: {index}, Offset: {offset}, Flag: {__instance.name}")
                .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
                .AppendLine($"Model: {__instance.quads.name}");
        }
    }
}