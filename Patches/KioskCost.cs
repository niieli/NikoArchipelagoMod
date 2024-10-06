using HarmonyLib;
using KinematicCharacterController.Core;
using Newtonsoft.Json.Linq;
using NikoArchipelago.Archipelago;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public static class KioskCost
{
    private static scrKioskManager _kioskManager;
    private static Plugin plugin;
    private static bool _changed, _changed2, bought, avail;

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
            _kioskManager = __instance;
            var currentScene = SceneManager.GetActiveScene().name;
            var levelPriceField = AccessTools.Field(typeof(scrKioskManager), "levelPrice");
            int levelPrice = (int)levelPriceField.GetValue(_kioskManager);
            var hasBoughtField = AccessTools.Field(typeof(scrKioskManager), "hasBought");
            var _hasBought = (bool)hasBoughtField.GetValue(__instance);
            var sentNoteEnoughField = AccessTools.Field(typeof(scrKioskManager), "sentNoteEnough");
            var _sentNoteEnough = (bool)sentNoteEnoughField.GetValue(__instance);
            var buyableLevelField = AccessTools.Field(typeof(scrKioskManager), "buyableLevel");
    
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
                        //TODO: Change to be universal and not dependent on en being selected
                        // if (scrTextbox.instance.isOn && scrTextbox.instance.nameMesh.text.Contains("Dispatcher"))
                        // {
                        //     scrTextbox.instance.textMesh.text = $"It will cost {levelPrice} Coins to purchase.";
                        //     if (GameInput.GetButtonDown("Action"))
                        //     {
                        //         scrTextbox.instance.EndConversation();
                        //     }
                        // }
                        if (scrTextbox.instance.answerSelected == 0 && scrTextbox.instance.isOn && scrTextbox.instance.nameMesh.text.Contains("Dispatcher") && scrTextbox.instance.textMesh.text.Contains("That is fantastic."))
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
                            scrTrainManager.instance.UseTrain(currentBuyableLevel-1, false);
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
                }
            }
    
            if (_changed2) return;
            Plugin.BepinLogger.LogInfo("Changed Kiosk");
            _changed2 = true;
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