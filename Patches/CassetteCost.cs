using System.Collections;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Packets;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class CassetteCost
{
    static bool _gaveCoin = false;
    static bool _gaveCoin2 = false;
    static bool _changed = false;
    static int _maiPrice = 0;
    [HarmonyPatch(typeof(scrCassetteBuyer), "Update")]
    public static class PatchCassetteBuyer
    {
        static bool _logged = false;
        private static int _mitchIndex, _maiIndex;

        private static void Postfix(scrCassetteBuyer __instance)
        {
            //Plugin.BepinLogger.LogMessage("Answerselected: " + scrTextbox.instance.answerSelected);
            if (ArchipelagoData.slotData == null) return;
            if (int.Parse(ArchipelagoData.slotData["cassette_logic"].ToString()) == 0)
            {
                if (!_logged)
                {
                    Plugin.BepinLogger.LogInfo("Found 'Level Based' Cassette Logic!");
                    _logged = true;
                }
                
                int count = 0;
                for (int i = 0; i < scrWorldSaveDataContainer.instance.coinFlags.Count; i++)
                {
                    if (scrWorldSaveDataContainer.instance.coinFlags[i].Contains("cassetteCoin") || scrWorldSaveDataContainer.instance.coinFlags[i].Contains("cassetteCoin2"))
                    {
                        count++;
                    }
                }
                string currentScene = SceneManager.GetActiveScene().name;
            
                switch (currentScene)
                {
                    case "Hairball City":
                        __instance.price = 5 + (5*count);
                        _mitchIndex = 0;
                        _maiIndex = _mitchIndex + 1;
                        break;
                    case "Trash Kingdom":
                        __instance.price = 15 + (5*count);
                        _mitchIndex = 2;
                        _maiIndex = _mitchIndex + 1;
                        break;
                    case "Salmon Creek Forest":
                        __instance.price = 25 + (5*count); 
                        _mitchIndex = 4;
                        _maiIndex = _mitchIndex + 1;
                        break;
                    case "Public Pool":
                        __instance.price = 35 + (5*count); 
                        _mitchIndex = 7;
                        _maiIndex = _mitchIndex - 1;
                        break;
                    case "The Bathhouse":
                        __instance.price = 45 + (5*count); 
                        _mitchIndex = 8;
                        _maiIndex = _mitchIndex + 1;
                        break;
                    case "Tadpole inc":
                        __instance.price = 55 + (5*count); 
                        _mitchIndex = 11;
                        _maiIndex = _mitchIndex - 1;
                        break;
                    case "GarysGarden":
                        __instance.price = 65 + (5*count); 
                        _mitchIndex = 13;
                        _maiIndex = _mitchIndex - 1;
                        break;
                }
                if (scrTextbox.instance.isOn && scrTextbox.instance.nameMesh.text == "Mitch" && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                {
                    if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price)
                    {
                        //scrTextbox.instance.canWaklaway = true;
                        //scrTextbox.instance.TurnOn("song0NotBought");
                        if (scrTextbox.instance.textMesh.text.Contains("Wanna trade"))
                        {
                            //ArchipelagoClient.ScoutByScene(HintCreationPolicy.CreateAndAnnounceOnce);
                            scrTextbox.instance.textMesh.text = 
                                $"It will Cost " + __instance.price + $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}.";
                            //TODO: Turn into Prefix to make custom Icon obtainer and fix the status: 
                            //TODO: this.parentBought.SetActive(true);
                            //TODO: this.parentNotBought.SetActive(false);
                            //TODO: this.parentCantBuy.SetActive(false);
                            //TODO: this.isBought = true; 
                            //scrTextbox.instance.answerCount = 2;
                            // scrTextbox.instance.answerTextObjects[1].text = "Gimme!";
                            // scrTextbox.instance.answerTextObjects[0].text = "Nah...";
                            // if (GameInput.GetButtonDown("Action"))
                            // {
                            //     if (scrTextbox.instance.answerSelected == 1)
                            //     {
                            //         if (!scrWorldSaveDataContainer.instance.coinFlags.Contains(__instance.myFlag) && scrTextbox.instance.textMesh.text.Contains("you got"))
                            //             __instance.buyCassette();
                            //         //Object.Instantiate<GameObject>(__instance.coinObtainer).GetComponent<scrObtainCoin>().myFlag = __instance.myFlag; 
                            //         //scrWorldSaveDataContainer.instance.coinFlags.Add(__instance.myFlag);
                            //     }
                            //     else if (scrTextbox.instance.answerSelected == 0)
                            //     {
                            //         scrTextbox.instance.EndConversation();
                            //     }
                            // }
                        }
                    }
                    else
                    {
                        scrTextbox.instance.canWaklaway = true;
                        scrTextbox.instance.textMesh.text = 
                            $"Come back when you have " + __instance.price + 
                            $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}.";
                    }
                }

                if (scrTextbox.instance.isOn && scrTextbox.instance.nameMesh.text == "Mai" && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                {
                    if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price)
                    {
                        //scrTextbox.instance.canWaklaway = true;
                        //scrTextbox.instance.textMesh.text = "It will Cost " + __instance.price + $" Cassettes to get {ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName} for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}.";
                        // scrTextbox.instance.answerCount = 2;
                        // scrTextbox.instance.answerTextObjects[0].text = "Gimme!";
                        // scrTextbox.instance.answerTextObjects[1].text = "Nah...";
                        if (scrTextbox.instance.textMesh.text.Contains("Wanna trade"))
                        {
                            scrTextbox.instance.textMesh.text = 
                                "It will Cost " + __instance.price + 
                                $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_maiIndex].ItemName} for {ArchipelagoClient.ScoutedLocations[_maiIndex].Player}'.";
                            // if (scrTextbox.instance.answerSelected == 0 && scrTextbox.instance.isOn)
                            // {
                            //     if (!scrWorldSaveDataContainer.instance.coinFlags.Contains(__instance.myFlag))
                            //         __instance.buyCassette();
                            //     //Object.Instantiate<GameObject>(__instance.coinObtainer).GetComponent<scrObtainCoin>().myFlag = __instance.myFlag; 
                            //     //scrWorldSaveDataContainer.instance.coinFlags.Add(__instance.myFlag);
                            // }
                            // else if (scrTextbox.instance.answerSelected == 1)
                            // {
                            //     scrTextbox.instance.EndConversation();
                            // }
                        }
                    }
                    else
                    {
                        scrTextbox.instance.canWaklaway = true;
                        scrTextbox.instance.textMesh.text = 
                            "Come back when you have " + __instance.price + 
                            $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_maiIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_maiIndex].Player}.";
                    }
                }
                if (_changed) return;
                Plugin.BepinLogger.LogInfo($"CassetteBuyer price updated to: {__instance.price} (Scene: {currentScene}, based on {count} cassette coins found).");
                _changed = true;
            }
            else if (int.Parse(ArchipelagoData.slotData["cassette_logic"].ToString()) == 1)
            {
                if (!_logged)
                {
                    Plugin.BepinLogger.LogInfo("Found 'Progressive' Cassette Logic!");
                    _logged = true;
                }
                var count = 1;
                var list = scrGameSaveManager.instance.gameData.worldsData;
                for (int i = 0; i < list.Count ; i++)
                {
                    if (list[i].coinFlags.Contains("cassetteCoin") || list[i].coinFlags.Contains("cassetteCoin2"))
                    {
                        count++;
                    }
                }
                __instance.price = 5 * count;
                
                var currentScene = SceneManager.GetActiveScene().name;
                switch (currentScene)
                {
                    case "Hairball City":
                        _mitchIndex = 0;
                        _maiIndex = _mitchIndex + 1;
                        break;
                    case "Trash Kingdom":
                        _mitchIndex = 2;
                        _maiIndex = _mitchIndex + 1;
                        break;
                    case "Salmon Creek Forest":
                        _mitchIndex = 4;
                        _maiIndex = _mitchIndex + 1;
                        break;
                    case "Public Pool":
                        _mitchIndex = 7;
                        _maiIndex = _mitchIndex - 1;
                        break;
                    case "The Bathhouse":
                        _mitchIndex = 8;
                        _maiIndex = _mitchIndex + 1;
                        break;
                    case "Tadpole inc":
                        _mitchIndex = 11;
                        _maiIndex = _mitchIndex - 1;
                        break;
                    case "GarysGarden":
                        _mitchIndex = 13;
                        _maiIndex = _mitchIndex - 1;
                        break;
                }
                
                if (scrTextbox.instance.isOn && scrTextbox.instance.nameMesh.text == "Mitch" && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                {
                    if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price)
                    {
                        if (scrTextbox.instance.textMesh.text.Contains("Wanna trade"))
                        {
                            scrTextbox.instance.textMesh.text = 
                                $"It will Cost " + __instance.price + 
                                $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}.";
                        }
                    }
                    else
                    {
                        scrTextbox.instance.canWaklaway = true;
                        scrTextbox.instance.textMesh.text = 
                            $"Come back when you have " + __instance.price + 
                            $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}.";
                    }
                }

                if (scrTextbox.instance.isOn && scrTextbox.instance.nameMesh.text == "Mai" && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                {
                    if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price)
                    {
                        if (scrTextbox.instance.textMesh.text.Contains("Wanna trade"))
                        {
                            scrTextbox.instance.textMesh.text = 
                                "It will Cost " + __instance.price + 
                                $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_maiIndex].ItemName} for {ArchipelagoClient.ScoutedLocations[_maiIndex].Player}'.";
                        }
                    }
                    else
                    {
                        scrTextbox.instance.canWaklaway = true;
                        scrTextbox.instance.textMesh.text = 
                            "Come back when you have " + __instance.price + 
                            $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_maiIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_maiIndex].Player}.";
                    }
                }

                if (_changed) return;
                Plugin.BepinLogger.LogInfo($"CassetteBuyer price updated to: {__instance.price} (based on {count} cassette coins found).");
                _changed = true;
            }
            else
            {
                if (!_logged)
                {
                    Plugin.BepinLogger.LogInfo("Found 'Scattered' Cassette Logic!");
                    _logged = true;
                }
                var currentScene = SceneManager.GetActiveScene().name;
                var slotData = ArchipelagoData.slotData;
                switch (currentScene)
                {
                    case "Hairball City":
                        __instance.price = int.Parse(slotData["chc1"].ToString());
                        _maiPrice = int.Parse(slotData["chc2"].ToString());
                        _mitchIndex = 0;
                        _maiIndex = _mitchIndex + 1;
                        break;
                    case "Trash Kingdom":
                        __instance.price = int.Parse(slotData["ctt1"].ToString());
                        _maiPrice = int.Parse(slotData["ctt2"].ToString());
                        _mitchIndex = 2;
                        _maiIndex = _mitchIndex + 1;
                        break;
                    case "Salmon Creek Forest":
                        __instance.price = int.Parse(slotData["csfc1"].ToString());
                        _maiPrice = int.Parse(slotData["csfc2"].ToString());
                        _mitchIndex = 4;
                        _maiIndex = _mitchIndex + 1;
                        break;
                    case "Public Pool":
                        __instance.price = int.Parse(slotData["cpp1"].ToString());
                        _maiPrice = int.Parse(slotData["cpp2"].ToString());
                        _mitchIndex = 7;
                        _maiIndex = _mitchIndex - 1;
                        break;
                    case "The Bathhouse":
                        __instance.price = int.Parse(slotData["cbath1"].ToString());
                        _maiPrice = int.Parse(slotData["cbath2"].ToString());
                        _mitchIndex = 8;
                        _maiIndex = _mitchIndex + 1;
                        break;
                    case "Tadpole inc":
                        __instance.price = int.Parse(slotData["chq1"].ToString());
                        _maiPrice = int.Parse(slotData["chq2"].ToString());
                        _mitchIndex = 11;
                        _maiIndex = _mitchIndex - 1;
                        break;
                    case "GarysGarden":
                        __instance.price = int.Parse(slotData["cgg1"].ToString());
                        _maiPrice = int.Parse(slotData["cgg2"].ToString());
                        _mitchIndex = 13;
                        _maiIndex = _mitchIndex - 1;
                        break;
                }
                if (scrTextbox.instance.isOn && scrTextbox.instance.nameMesh.text == "Mitch" && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                {
                    if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price * 5)
                    {
                        if (scrTextbox.instance.textMesh.text.Contains("Wanna trade"))
                        {
                            scrTextbox.instance.textMesh.text = 
                                $"It will Cost " + __instance.price * 5 + 
                                $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}.";
                        }
                    }
                    else
                    {
                        scrTextbox.instance.canWaklaway = true;
                        scrTextbox.instance.textMesh.text = 
                            $"Come back when you have " + __instance.price * 5 + 
                            $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}.";
                    }
                }

                if (scrTextbox.instance.isOn && scrTextbox.instance.nameMesh.text == "Mai" && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                {
                    if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= _maiPrice * 5)
                    {
                        if (scrTextbox.instance.textMesh.text.Contains("Wanna trade"))
                        {
                            scrTextbox.instance.textMesh.text = 
                                "It will Cost " + _maiPrice * 5 + 
                                $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_maiIndex].ItemName} for {ArchipelagoClient.ScoutedLocations[_maiIndex].Player}'.";
                        }
                    }
                    else
                    {
                        scrTextbox.instance.canWaklaway = true;
                        scrTextbox.instance.textMesh.text = 
                            "Come back when you have " + _maiPrice * 5 + 
                            $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_maiIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_maiIndex].Player}.";
                    }
                }
            }
        }
    }
}