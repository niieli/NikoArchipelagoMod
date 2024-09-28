using System.Collections.Generic;
using HarmonyLib;
using KinematicCharacterController.Core;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public static class KioskCost
{
    private static scrKioskManager _kioskManager;
    private static Plugin plugin;
    private static bool _changed, _changed2, bought, avail;
    
    [HarmonyPrefix, HarmonyPatch(typeof(levelData))]
    public static void levelData_Prefix()
    {
        levelData.levelPrices[3] = 6;
        levelData.levelPrices[4] = 11;
        levelData.levelPrices[5] = 21;
        levelData.levelPrices[6] = 26;
        levelData.levelPrices[7] = 31;
        levelData.levelPrices[8] = 46;
        if (_changed) return;
        Plugin.BepinLogger.LogInfo("Changed LevelPrices");
        _changed = true;
    }

    [HarmonyPatch(typeof(scrKioskManager), "Update")]
    public static class KioskLevelFixPatch
    {
        [HarmonyPostfix]
        public static void PostFix(scrKioskManager __instance)
        {
            //TODO: Less scuffed
            _kioskManager = __instance;
            var currentScene = SceneManager.GetActiveScene().name;
            var levelPriceField = AccessTools.Field(typeof(scrKioskManager), "levelPrice");
            int levelPrice = (int)levelPriceField.GetValue(_kioskManager);
            var hasBoughtField = AccessTools.Field(typeof(scrKioskManager), "hasBought");
            var _hasBought = (bool)hasBoughtField.GetValue(__instance);
            var buyableLevelField = AccessTools.Field(typeof(scrKioskManager), "buyableLevel");
    
            if (buyableLevelField != null)
            {
                int currentBuyableLevel = (int)buyableLevelField.GetValue(_kioskManager);
                if (!scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains($"Kiosk{currentScene}"))
                {
                    if (scrGameSaveManager.instance.gameData.generalGameData.coinAmount >= levelPrice)
                    {
                        if (!avail)
                        {
                            Plugin.BepinLogger.LogInfo($"Kiosk in {currentScene} is available for purchase.");
                            avail = true;
                        }
                        __instance.NPCbuy.SetActive(true);
                        __instance.NPCnomoney.SetActive(false);
                        __instance.textMesh.text = scrGameSaveManager.instance.gameData.generalGameData.coinAmount.ToString() + "/" + levelPrice.ToString();
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
                            __instance.NPCbought.SetActive(true);
                            __instance.NPCbuy.SetActive(false);
                            __instance.textMesh.text = "Bought!";
                        }
                    }
                    else
                    {
                        __instance.NPCnomoney.SetActive(true);
                        __instance.NPCbuy.SetActive(false);
                        __instance.textMesh.text = scrGameSaveManager.instance.gameData.generalGameData.coinAmount.ToString() + "/" + levelPrice.ToString();
                    }
                }
                else if (scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[currentBuyableLevel])
                {
                    __instance.NPCnomoney.SetActive(true);
                    __instance.NPCbuy.SetActive(false);
                    __instance.textMesh.text = scrGameSaveManager.instance.gameData.generalGameData.coinAmount.ToString() + "/" + levelPrice.ToString();
                }
                else
                {
                    __instance.NPCbought.SetActive(true);
                    __instance.NPCbuy.SetActive(false);
                    __instance.textMesh.text = "";
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
    //
    // [HarmonyPatch(typeof(scrKioskManager), "Update")]
    // public static class KioskLevelFixPatch
    // {
    //     private static scrKioskManager _kioskManager;
    //     private static bool _changed2;
    //
    //     // Dictionary to track whether the kiosk in each scene has been bought
    //     private static readonly Dictionary<string, bool> kioskPurchasedStatus = new()
    //     {
    //         { "Home", false },
    //         { "Trash Kingdom", false },
    //         { "Salmon Creek Forest", false },
    //         { "Public Pool", false },
    //         { "The Bathhouse", false }
    //     };
    //
    //     [HarmonyPostfix]
    //     public static void KioskLevelFix(scrKioskManager __instance)
    //     {
    //         _kioskManager = __instance;
    //         var currentScene = SceneManager.GetActiveScene().name;
    //         var buyableLevelField = AccessTools.Field(typeof(scrKioskManager), "buyableLevel");
    //
    //         if (buyableLevelField != null && currentScene != "Tadpole inc")
    //         {
    //             // Check if the kiosk in the current scene has already been bought
    //             if (kioskPurchasedStatus.ContainsKey(currentScene) && kioskPurchasedStatus[currentScene])
    //             {
    //                 Plugin.BepinLogger.LogInfo($"Kiosk in {currentScene} has already been bought.");
    //                 return; // If already bought, do nothing
    //             }
    //
    //             switch (currentScene)
    //             {
    //                 case "Home":
    //                     buyableLevelField.SetValue(_kioskManager, 8);
    //                     break;
    //                 case "Trash Kingdom":
    //                     buyableLevelField.SetValue(_kioskManager, 9);
    //                     break;
    //                 case "Salmon Creek Forest":
    //                     buyableLevelField.SetValue(_kioskManager, 10);
    //                     break;
    //                 case "Public Pool":
    //                     buyableLevelField.SetValue(_kioskManager, 11);
    //                     break;
    //                 case "The Bathhouse":
    //                     buyableLevelField.SetValue(_kioskManager, 12);
    //                     break;
    //             }
    //
    //             if (_kioskManager.SomePurchaseConditionMet()) // Replace with actual check
    //             {
    //                 Plugin.BepinLogger.LogInfo($"Kiosk in {currentScene} has been bought.");
    //                 kioskPurchasedStatus[currentScene] = true; // Mark the kiosk as bought for this scene
    //             }
    //         }
    //
    //         if (_changed2) return;
    //         Plugin.BepinLogger.LogInfo("Changed Kiosk");
    //         _changed2 = true;
    //     }
    // }
}