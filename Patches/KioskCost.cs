using System.Reflection;
using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public static class KioskCost
{
    private static scrKioskManager _kioskManager;
    private static Plugin plugin;
    private static bool _changed, _changed2, bought, avail, _answerFix;

    [HarmonyPrefix, HarmonyPatch(typeof(levelData))]
    public static void PreFix()
    {
        var slotData = ArchipelagoData.slotData;
        if (ArchipelagoData.slotData == null) return;
        levelData.levelPrices[2] = int.Parse(slotData["kioskhome"].ToString());
        levelData.levelPrices[3] = int.Parse(slotData["kioskhc"].ToString());
        levelData.levelPrices[4] = int.Parse(slotData["kiosktt"].ToString());
        levelData.levelPrices[5] = int.Parse(slotData["kiosksfc"].ToString());
        levelData.levelPrices[6] = int.Parse(slotData["kioskpp"].ToString());
        levelData.levelPrices[7] = int.Parse(slotData["kioskbath"].ToString());
        levelData.levelPrices[8] = int.Parse(slotData["kioskhq"].ToString());
        if (_changed) return;
        Plugin.BepinLogger.LogInfo("Changed Kiosk Price");
        _changed = true;
    }
    
    [HarmonyPatch(typeof(scrKioskManager), "Update")]
    public static class KioskLevelFixPatch
    {
        [HarmonyPostfix]
        public static void PostFix(scrKioskManager __instance)
        {
            var gardenAdjustment = 0;
            var cassetteAdjustment = 0;
            if (ArchipelagoData.slotData == null) return;
            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
            {
                if (int.Parse(ArchipelagoData.slotData["shuffle_garden"].ToString()) == 0)
                {
                    gardenAdjustment = 2;
                }
            }
            if (ArchipelagoData.slotData.ContainsKey("cassette_logic"))
                if (int.Parse(ArchipelagoData.slotData["cassette_logic"].ToString()) == 1)
                {
                    cassetteAdjustment = 14;
                    gardenAdjustment = 0;
                }
            var adjustment = gardenAdjustment + cassetteAdjustment;
            _kioskManager = __instance;
            var currentScene = SceneManager.GetActiveScene().name;
            var levelPriceField = AccessTools.Field(typeof(scrKioskManager), "levelPrice");
            int levelPrice = (int)levelPriceField.GetValue(_kioskManager);
            var hasBoughtField = AccessTools.Field(typeof(scrKioskManager), "hasBought");
            var _hasBought = (bool)hasBoughtField.GetValue(__instance);
            var sentNoteEnoughField = AccessTools.Field(typeof(scrKioskManager), "sentNoteEnough");
            var _sentNoteEnough = (bool)sentNoteEnoughField.GetValue(__instance);
            var buyableLevelField = AccessTools.Field(typeof(scrKioskManager), "buyableLevel");
            var currentBoxField = AccessTools.Field(typeof(scrTextbox), "currentBox");
            int _currentBox = (int)currentBoxField.GetValue(scrTextbox.instance);
    
            if (buyableLevelField != null)
            {
                int currentBuyableLevel = (int)buyableLevelField.GetValue(_kioskManager);
                if (!scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains($"Kiosk{currentScene}"))
                {
                    _hasBought = false;
                    __instance.NPCbought.SetActive(false);
                    __instance.textMesh.gameObject.SetActive(true);
                    if (scrGameSaveManager.instance.gameData.generalGameData.coinAmount >= levelPrice)
                    {
                        if (!avail)
                        {
                            Plugin.BepinLogger.LogInfo($"Kiosk in {currentScene} is available for purchase.");
                            avail = true;
                        }
                        __instance.NPCbuy.SetActive(true);
                        __instance.NPCnomoney.SetActive(false);
                        __instance.NPCbought.SetActive(false);
                        __instance.textMesh.text = scrGameSaveManager.instance.gameData.generalGameData.coinAmount.ToString() + "/" + levelPrice.ToString();
                        __instance.textMesh.gameObject.SetActive(true);
                        if (scrTextbox.instance.isOn && scrTextbox.instance.conversation == "kioskBuy")
                        {
                            if (!__instance.saveManager.gameData.generalGameData.generalFlags.Contains("Hint"+(currentBuyableLevel+12)) && ArchipelagoMenu.Hints)
                            {
                                ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[currentBuyableLevel+12]);
                                __instance.saveManager.gameData.generalGameData.generalFlags.Add("Hint"+(currentBuyableLevel+12));
                            }
                            var kioskBuy0 = 
                                $"Do you want to purchase '{ArchipelagoClient.ScoutedLocations[currentBuyableLevel+12-adjustment].ItemName}' for {ArchipelagoClient.ScoutedLocations[currentBuyableLevel+12].Player}?"
                                + $"\nIt seems {ItemClassification(currentBuyableLevel+12-adjustment)}...";
                            scrTextbox.instance.conversationLocalized[0] = kioskBuy0;
                            if (_currentBox == 0)
                            {
                                _answerFix = false;
                            }
                            if (_currentBox == 1 && !_answerFix)
                            {
                                scrTextbox.instance.conversationLocalized[1] = $"It will cost {levelPrice} coins to purchase. ##addinput:I'll pay!;skip1; ##addinput:No way!;skip0;";
                                _answerFix = true;
                            }
                            if (_currentBox == 3)
                            {
                                if (GameInput.GetButtonDown("Action"))
                                {
                                    if (!bought)
                                    {
                                        Plugin.BepinLogger.LogInfo($"Kiosk in {currentScene} has been bought.");
                                        bought = true;
                                    }

                                    if (!scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains($"Kiosk{currentScene}"))
                                    {
                                        scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Add($"Kiosk{currentScene}");
                                    }
                                    _hasBought = true;
                                    __instance.NPCbought.SetActive(true);
                                    __instance.NPCbuy.SetActive(false);
                                    __instance.textMesh.text = "Bought!";
                                    __instance.textMesh.gameObject.SetActive(true);
                                    scrTextbox.instance.EndConversation();
                                }
                            }
                        }
                    }
                    else
                    {
                        _hasBought = false;
                        __instance.NPCnomoney.SetActive(true);
                        __instance.NPCbuy.SetActive(false);
                        __instance.NPCbought.SetActive(false);
                        __instance.textMesh.text = scrGameSaveManager.instance.gameData.generalGameData.coinAmount.ToString() + "/" + levelPrice.ToString();
                        __instance.textMesh.gameObject.SetActive(true);
                        if (scrTextbox.instance.isOn && scrTextbox.instance.conversation == "kioskNomoney")
                        {
                            if (!__instance.saveManager.gameData.generalGameData.generalFlags.Contains("Hint"+(currentBuyableLevel+12)) && ArchipelagoMenu.Hints)
                            {
                                ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[currentBuyableLevel+12]);
                                __instance.saveManager.gameData.generalGameData.generalFlags.Add("Hint"+(currentBuyableLevel+12));
                            }
                            string noMoneyKiosk0 = 
                                $"Do you want to purchase '{ArchipelagoClient.ScoutedLocations[currentBuyableLevel+12-adjustment].ItemName}' for {ArchipelagoClient.ScoutedLocations[currentBuyableLevel+12-adjustment].Player}?" +
                                $"\nIt seems {ItemClassification(currentBuyableLevel+12-adjustment)}...";
                            string noMoneyKiosk2 = 
                                $"It will cost {levelPrice} coins to purchase.";
                            scrTextbox.instance.conversationLocalized[0] = noMoneyKiosk0;
                            if (_currentBox == 0) _answerFix = false;
                            if (_currentBox == 1 && !_answerFix)
                            {
                                string[] multiworldRef = new[]
                                {
                                    "I got it from a mysterious traveler, so it will cost a bit of money",
                                    "Multiworld stuff... yeah, I got it. Don't ask how... just... it's not cheap.",
                                    "You want something from another timeline? Then you'll need something from this one... like money.",
                                    "Multiworld... huh. Can't even get a coffee without overpaying across dimensions.",
                                    "One of a kind... probably. Honestly, I've lost track.",
                                    "This tech ain't from around here... or anywhere you've ever been. Costs extra to cross dimensions, you know?",
                                    "Multiworld goods don't come cheap-import taxes from infinite realities and all that.",
                                    "One-of-a-kind, straight from a reality where this deal's a bargain. Sadly... you're stuck in this one.",
                                    "Don't ask how I got it. Just know someone in another universe wants it back."
                                };
                                string randomMessage = multiworldRef[new System.Random().Next(multiworldRef.Length)];
                                scrTextbox.instance.conversationLocalized[1] = randomMessage;
                                _answerFix = true;
                            }
                            scrTextbox.instance.conversationLocalized[2] = noMoneyKiosk2;
                        }
                    }
                }
                else if (scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[currentBuyableLevel] && !scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains($"Kiosk{currentScene}"))
                {
                    _hasBought = false;
                    __instance.NPCnomoney.SetActive(true);
                    __instance.NPCbuy.SetActive(false);
                    __instance.NPCbought.SetActive(false);
                    __instance.textMesh.text = scrGameSaveManager.instance.gameData.generalGameData.coinAmount.ToString() + "/" + levelPrice.ToString();
                    __instance.textMesh.gameObject.SetActive(true);
                    if (scrTextbox.instance.isOn && scrTextbox.instance.conversation == "kioskNomoney")
                    {
                        if (!__instance.saveManager.gameData.generalGameData.generalFlags.Contains("Hint"+(currentBuyableLevel+12)) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[currentBuyableLevel+12]);
                            __instance.saveManager.gameData.generalGameData.generalFlags.Add("Hint"+(currentBuyableLevel+12));
                        }
                        string noMoneyKiosk0 = 
                            $"Do you want to purchase '{ArchipelagoClient.ScoutedLocations[currentBuyableLevel+12-adjustment].ItemName}' for {ArchipelagoClient.ScoutedLocations[currentBuyableLevel+12-adjustment].Player}?" +
                            $"\nIt seems {ItemClassification(currentBuyableLevel+12-adjustment)}...";
                        string noMoneyKiosk2 = 
                            $"It will cost {levelPrice} coins to purchase.";
                        scrTextbox.instance.conversationLocalized[0] = noMoneyKiosk0;
                        if (_currentBox == 0) _answerFix = false;
                        if (_currentBox == 1 && !_answerFix)
                        {
                            string[] multiworldRef = new[]
                            {
                                "I got it from a mysterious traveler, so it will cost a bit of money",
                                "Multiworld stuff... yeah, I got it. Don't ask how... just... it's not cheap.",
                                "You want something from another timeline? Then you'll need something from this one... like money.",
                                "Multiworld... huh. Can't even get a coffee without overpaying across dimensions.",
                                "One of a kind... probably. Honestly, I've lost track.",
                                "This tech ain't from around here... or anywhere you've ever been. Costs extra to cross dimensions, you know?",
                                "Multiworld goods don't come cheap-import taxes from infinite realities and all that.",
                                "One-of-a-kind, straight from a reality where this deal's a bargain. Sadly... you're stuck in this one.",
                                "Don't ask how I got it. Just know someone in another universe wants it back."
                            };
                            string randomMessage = multiworldRef[new System.Random().Next(multiworldRef.Length)];
                            scrTextbox.instance.conversationLocalized[1] = randomMessage;
                            _answerFix = true;
                        }
                        scrTextbox.instance.conversationLocalized[2] = noMoneyKiosk2;
                    }
                }
                else
                {
                    _hasBought = true;
                    __instance.NPCnomoney.SetActive(false);
                    __instance.NPCbuy.SetActive(false);
                    __instance.NPCbought.SetActive(true);
                    __instance.textMesh.text = "";
                    __instance.textMesh.gameObject.SetActive(false);
                    _sentNoteEnough = true;
                    scrNotificationDisplayer.instance.RemoveNotification(__instance.noteEnough);
                    if (scrTextbox.instance.isOn && scrTextbox.instance.conversation == "kioskBought")
                    {
                        if (!__instance.saveManager.gameData.generalGameData.generalFlags.Contains("Hint"+(currentBuyableLevel+12)) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[currentBuyableLevel+12]);
                            __instance.saveManager.gameData.generalGameData.generalFlags.Add("Hint"+(currentBuyableLevel+12));
                        }
                        var kioskBought0 = 
                            $"You already bought '{ArchipelagoClient.ScoutedLocations[currentBuyableLevel+12-adjustment].ItemName}' " +
                            $"for {ArchipelagoClient.ScoutedLocations[currentBuyableLevel+12-adjustment].Player}. ##end;";
                        scrTextbox.instance.conversationLocalized[0] = kioskBought0;
                        if (GameInput.GetButtonDown("Action"))
                        {
                            scrTextbox.instance.EndConversation();
                        }
                    }
                }
                _sentNoteEnough = true;
                scrNotificationDisplayer.instance.RemoveNotification(__instance.noteEnough);
            }
    
            if (_changed2) return;
            Plugin.BepinLogger.LogInfo("Changed Kiosk");
            _changed2 = true;
        }

        private static string ItemClassification(int scoutID)
        {
            string classification;
            if (ArchipelagoClient.ScoutedLocations[scoutID].Flags.HasFlag(ItemFlags.Advancement))
            {
                classification = "Important";
            }
            else if (ArchipelagoClient.ScoutedLocations[scoutID].Flags.HasFlag(ItemFlags.NeverExclude))
            {
                classification = "Useful";
            } else if (ArchipelagoClient.ScoutedLocations[scoutID].Flags.HasFlag(ItemFlags.Trap))
            {
                var trapStrings = new[]
                {
                    "iMpOrTaNt",
                    "SUPER IMPORTANT",
                    "like a good deal",
                    "very important trust me",
                    "like the best item",
                    "This is a 1-Time Offer!",
                    "You Need This!",
                    "RARE LOOT!",
                    "Legendary!",
                    "This will help… I promise",
                    "Absolutely NOT a trap",
                    "A MUST PICK UP!",
                    "Collector's really like this one.. hehe",
                    "a very funny item"
                };
                var randomIndex = Random.Range(0, trapStrings.Length);
                classification = trapStrings[randomIndex];
            } else if (ArchipelagoClient.ScoutedLocations[scoutID].Flags.HasFlag(ItemFlags.None))
            {
                classification = "Useless";
            }
            else
            {
                classification = "Unknown";
            }
            //Plugin.BepinLogger.LogInfo("Location: "+ArchipelagoClient.ScoutedLocations[scoutID].LocationName);
            return classification;
        }
    }

    [HarmonyPatch(typeof(scrKioskManager), "RemoveIfObtained")]
    public static class KioskRemoveIfObtainedPatch
    {
        [HarmonyPostfix]
        public static void PostFix(scrKioskManager __instance)
        {
            var hasBoughtField = AccessTools.Field(typeof(scrKioskManager), "hasBought");
            var _hasBought = (bool)hasBoughtField.GetValue(__instance);
            var buyableLevelField = AccessTools.Field(typeof(scrKioskManager), "buyableLevel");
            int currentBuyableLevel = (int)buyableLevelField.GetValue(__instance);
            var currentScene = SceneManager.GetActiveScene().name;
            if (scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains($"Kiosk{currentScene}"))
            {
                _hasBought = true;
            }
            else
            {
                _hasBought = false;
            }
                
        }
    }
}