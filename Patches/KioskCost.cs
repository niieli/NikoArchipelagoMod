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
    private static bool _changed, _changed2, bought, avail, _answerFix, _answerFix2;

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
            if (ArchipelagoData.slotData == null) return;
            _kioskManager = __instance;
            var currentScene = SceneManager.GetActiveScene().name;
            var levelPriceField = AccessTools.Field(typeof(scrKioskManager), "levelPrice");
            int levelPrice = (int)levelPriceField.GetValue(_kioskManager);
            var buyableLevelField = AccessTools.Field(typeof(scrKioskManager), "buyableLevel");
            var currentBoxField = AccessTools.Field(typeof(scrTextbox), "currentBox");
            int currentBox = (int)currentBoxField.GetValue(scrTextbox.instance);
            int scoutID = currentScene switch
            {
                "Home" => 170,
                "Hairball City" => 171,
                "Trash Kingdom" => 172,
                "Salmon Creek Forest" => 173,
                "Public Pool" => 174,
                "The Bathhouse" => 175,
                "Tadpole inc" => 176,
                _ => 0
            };
            if (buyableLevelField != null)
            {
                if (!scrTextbox.instance.isOn)
                    _answerFix2 = false;
                int currentBuyableLevel = (int)buyableLevelField.GetValue(_kioskManager);
                if (!scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains($"Kiosk{currentScene}"))
                {
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
                            if (!__instance.saveManager.gameData.generalGameData.generalFlags.Contains("Hint"+(currentBuyableLevel+12)))
                            {
                                __instance.saveManager.gameData.generalGameData.generalFlags.Add("Hint"+(currentBuyableLevel+12));
                            }
                            if (currentBox == 0 && !_answerFix2)
                            {
                                scrTextbox.instance.conversationLocalized[0] = KioskConversation(scoutID);
                                _answerFix2 = true;
                                _answerFix = false;
                            }
                            if (currentBox == 1 && !_answerFix)
                            {
                                scrTextbox.instance.conversationLocalized[1] = $"It will cost {levelPrice} coins to purchase. ##addinput:I'll pay!;skip1; ##addinput:No way!;skip0;";
                                _answerFix = true;
                            }
                            if (currentBox == 3)
                            {
                                scrTextbox.instance.conversationLocalized[3] = "That is fantastic! ##fx1; ##end;";
                                if (!bought)
                                {
                                    Plugin.BepinLogger.LogInfo($"Kiosk in {currentScene} has been bought.");
                                    bought = true;
                                }

                                if (!scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains($"Kiosk{currentScene}"))
                                {
                                    scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Add($"Kiosk{currentScene}");
                                }
                                if (!scrTextbox.instance.isOn)
                                {
                                    __instance.NPCbought.SetActive(true);
                                    __instance.NPCbuy.SetActive(false);
                                    __instance.textMesh.text = "Bought!";
                                    __instance.textMesh.gameObject.SetActive(true);
                                    scrTextbox.instance.EndConversation();
                                }
                            }
                        }
                        
                        // Teach "Fix Frog" some math
                        if (scrTextbox.instance.isOn && scrTextbox.instance.conversation == "elevatorBuy")
                        {
                            if (currentBox == 4 && !_answerFix2)
                            {
                                scrTextbox.instance.conversationLocalized[4] = 
                                    $"It'll cost {levelPrice} coins to fix. ##addinput:Bye;skip1; ##addinput:I'll pay!;skip0;";
                                _answerFix2 = true;
                            }
                        }
                    }
                    else
                    {
                        __instance.NPCnomoney.SetActive(true);
                        __instance.NPCbuy.SetActive(false);
                        __instance.NPCbought.SetActive(false);
                        __instance.textMesh.text = scrGameSaveManager.instance.gameData.generalGameData.coinAmount.ToString() + "/" + levelPrice.ToString();
                        __instance.textMesh.gameObject.SetActive(true);
                        if (!scrTextbox.instance.isOn)
                            _answerFix2 = false;
                        if (scrTextbox.instance.isOn && scrTextbox.instance.conversation == "kioskNomoney")
                        {
                            if (!__instance.saveManager.gameData.generalGameData.generalFlags.Contains("Hint"+(currentBuyableLevel+12)))
                            {
                                __instance.saveManager.gameData.generalGameData.generalFlags.Add("Hint"+(currentBuyableLevel+12));
                            }
                            string noMoneyKiosk2 = 
                                $"It will cost {levelPrice} coins to purchase.";
                            if (currentBox == 0 && !_answerFix2)
                            {
                                scrTextbox.instance.conversationLocalized[0] = KioskConversation(scoutID);
                                _answerFix2 = true;
                                _answerFix = false;
                            }
                            if (currentBox == 1 && !_answerFix)
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
                        
                        // Teach "Fix Frog" some math
                        if (scrTextbox.instance.isOn && scrTextbox.instance.conversation == "elevatorNoMoney")
                        {
                            if (currentBox == 4 && !_answerFix2)
                            {
                                scrTextbox.instance.conversationLocalized[4] = 
                                    $"It'll cost {levelPrice} coins to fix.";
                                _answerFix2 = true;
                            }
                        }
                    }
                }
                else if (scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[currentBuyableLevel] && !scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains($"Kiosk{currentScene}"))
                {
                    __instance.NPCnomoney.SetActive(true);
                    __instance.NPCbuy.SetActive(false);
                    __instance.NPCbought.SetActive(false);
                    __instance.textMesh.text = scrGameSaveManager.instance.gameData.generalGameData.coinAmount.ToString() + "/" + levelPrice.ToString();
                    __instance.textMesh.gameObject.SetActive(true);
                    if (!scrTextbox.instance.isOn)
                        _answerFix2 = false;
                    if (scrTextbox.instance.isOn && scrTextbox.instance.conversation == "kioskNomoney")
                    {
                        if (!__instance.saveManager.gameData.generalGameData.generalFlags.Contains("Hint"+(currentBuyableLevel+12)))
                        {
                            __instance.saveManager.gameData.generalGameData.generalFlags.Add("Hint"+(currentBuyableLevel+12));
                        }
                        string noMoneyKiosk2 = 
                            $"It will cost {levelPrice} coins to purchase.";
                        if (currentBox == 0 && !_answerFix2)
                        {
                            scrTextbox.instance.conversationLocalized[0] = KioskConversation(scoutID);
                            _answerFix2 = true;
                            _answerFix = false;
                        }
                        if (currentBox == 1 && !_answerFix)
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
                    __instance.NPCnomoney.SetActive(false);
                    __instance.NPCbuy.SetActive(false);
                    __instance.NPCbought.SetActive(true);
                    __instance.textMesh.text = "";
                    __instance.textMesh.gameObject.SetActive(false);
                    scrNotificationDisplayer.instance.RemoveNotification(__instance.noteEnough);
                    if (scrTextbox.instance.isOn && scrTextbox.instance.conversation == "kioskBought")
                    {
                        if (!__instance.saveManager.gameData.generalGameData.generalFlags.Contains("Hint"+(currentBuyableLevel+12)))
                        {
                            __instance.saveManager.gameData.generalGameData.generalFlags.Add("Hint"+(currentBuyableLevel+12));
                        }

                        if (currentBox == 0 && !_answerFix2)
                        {
                            scrTextbox.instance.conversationLocalized[0] = KioskConversation(scoutID);
                            _answerFix2 = true;
                        }
                    }
                }
                scrNotificationDisplayer.instance.RemoveNotification(__instance.noteEnough);
            }
    
            if (_changed2) return;
            Plugin.BepinLogger.LogInfo("Changed Kiosk");
            _changed2 = true;
        }

        private static string KioskConversation(int scoutID, int price = 0, bool canBuy = true)
        {
            string classification;
            var scoutedItem = ArchipelagoClient.ScoutLocation(scoutID);
            var itemName = scoutedItem.ItemName; 
            if (scoutedItem.Flags.HasFlag(ItemFlags.Advancement))
            {
                classification = "Important";
            }
            else if (scoutedItem.Flags.HasFlag(ItemFlags.NeverExclude))
            {
                classification = "Useful";
            } else if (scoutedItem.Flags.HasFlag(ItemFlags.Trap))
            {
                var trapStrings = new[]
                {
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

                if (scoutedItem.IsReceiverRelatedToActivePlayer)
                {
                    var fakeNames = new[]
                    {
                        "Coin ?",
                        "Coin :)",
                        "Shiny Object",
                        "Pon",
                        "Cassette ?",
                        "Coin",
                        "Rupee",
                        "Coin >:(",
                        "A fabulous flower",
                        "COIN!",
                        "CASSETTE!",
                        "REDACTED",
                        "Cool Item(insert cool smiley)",
                        "A",
                        "Noic",
                        "Mixtape",
                        "Home Cassette",
                        "Tickets for a concert",
                    };
                    var randomFakeName = Random.Range(0, fakeNames.Length);
                    itemName = fakeNames[randomFakeName];
                }
                else
                {
                    var fakeNames = new[]
                    {
                        "Hookshot",
                    };
                    var randomFakeName = Random.Range(0, fakeNames.Length);
                    //itemName = fakeNames[randomFakeName]; Not sure if I should do that, maybe later
                }
            } else if (scoutedItem.Flags.HasFlag(ItemFlags.None))
            {
                classification = "Useless";
            }
            else
            {
                classification = "Unknown";
            }

            // if (canBuy) // Maybe in future change it a bit to include price & stuff
            // {
            //     //ggs
            // }
            // else
            // {
            //     //ggs
            // }

            var alreadyBought =
                scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains(
                    $"Kiosk{SceneManager.GetActiveScene().name}");

            string text;
            text = $"Do you want to purchase '{itemName}' for {scoutedItem.Player}?" +
                   $"\nIt seems {classification}...";
            
            if (alreadyBought)
            {
                text = $"You already bought '{scoutedItem.ItemName}' " +
                       $"for {scoutedItem.Player}. ##end;";
            }
            return text;
        }
    }
}