using System;
using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
            {
                if (!ArchipelagoData.Options.GarysGarden)
                {
                    gardenAdjustment = 3;
                }
            }
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
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
            {
                if (!ArchipelagoData.Options.GarysGarden)
                {
                    gardenAdjustment = 2;
                }
            }
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
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
            {
                if (!ArchipelagoData.Options.GarysGarden)
                {
                    gardenAdjustment = 13;
                }
            }
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
    
    [HarmonyPatch(typeof(scrEnvelope), "Start")]
    public static class LetterTexturePatch
    {
        private static void Postfix(scrEnvelope __instance)
        {
            if (!ArchipelagoMenu.kalmi) return;
            if (scrWorldSaveDataContainer.instance.miscFlags.Contains(__instance.myLetter.key) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                return;
            }
            var gardenAdjustment = 0;
            var snailShopAdjustment = 0;
            var cassetteAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
            {
                if (!ArchipelagoData.Options.GarysGarden)
                {
                    gardenAdjustment = 13;
                }
            }
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
            try
            {
                var list = Locations.ScoutLetterList.ToList();
                var index = list.FindIndex(pair => pair.Value == __instance.myLetter.key);
                int offset = 181 - adjustment;
                PlaceModelHelper.PlaceModel(index, offset, __instance);
                GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
                GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                    .AppendLine($"Index: {index}, Offset: {offset}, Flag: {__instance.myLetter.key}")
                    .AppendLine($"Item: {ArchipelagoClient.ScoutedLocations[index + offset].ItemName}")
                    .AppendLine($"Location: {ArchipelagoClient.ScoutedLocations[index + offset].LocationName}")
                    .AppendLine($"LocationID: {ArchipelagoClient.ScoutedLocations[index + offset].LocationId}")
                    .AppendLine($"Model: {__instance.quads.name}");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Plugin.BepinLogger.LogWarning($"Letter not found! Probably using an apworld pre-0.5.0, where this letter does not exist." +
                                              $"\nIf you are on 0.5.0+ and this keeps popping up please report it in the discord");
            }
            
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
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
            {
                if (!ArchipelagoData.Options.GarysGarden)
                {
                    gardenAdjustment = 13;
                }
            }
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