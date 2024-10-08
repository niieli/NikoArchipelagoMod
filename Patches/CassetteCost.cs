using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class CassetteCost
{
    [HarmonyPatch(typeof(scrCassetteBuyer), "Update")]
    public static class PatchCassetteBuyer
    {
        static bool _changed = false;
        static bool _logged = false;
        static bool _gaveCoin = false;
        static bool _gaveCoin2 = false;

        private static void Postfix(scrCassetteBuyer __instance)
        {
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
                        break;
                    case "Trash Kingdom":
                        __instance.price = 15 + (5*count); 
                        break;
                    case "Salmon Creek Forest":
                        __instance.price = 25 + (5*count); 
                        break;
                    case "Public Pool":
                        __instance.price = 35 + (5*count); 
                        break;
                    case "The Bathhouse":
                        __instance.price = 45 + (5*count); 
                        break;
                    case "Tadpole inc":
                        __instance.price = 55 + (5*count); 
                        break;
                    case "GarysGarden":
                        __instance.price = 65 + (5*count); 
                        break;
                }
                if (scrTextbox.instance.isOn && scrTextbox.instance.nameMesh.text == "Mitch" && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                {
                    if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price)
                    {
                        scrTextbox.instance.canWaklaway = false;
                        scrTextbox.instance.textMesh.text = "It will Cost " + __instance.price + " Cassettes to get (ItemName) for (PlayerName).)";
                        scrTextbox.instance.answerCount = 2;
                        scrTextbox.instance.answerTextObjects[0].text = "Gimme!";
                        scrTextbox.instance.answerTextObjects[1].text = "Nah...";
                        if (scrTextbox.instance.isOn && scrTextbox.instance.answerSelected == 0 && GameInput.GetButtonDown("Action"))
                        {
                            if (!scrWorldSaveDataContainer.instance.coinFlags.Contains(__instance.myFlag))
                            {
                                __instance.buyCassette();
                                //Object.Instantiate<GameObject>(__instance.coinObtainer).GetComponent<scrObtainCoin>().myFlag = __instance.myFlag; 
                                _gaveCoin = true;
                                //scrWorldSaveDataContainer.instance.coinFlags.Add(__instance.myFlag);
                            }
                            //scrTextbox.instance.EndConversation();
                        }
                        if (scrTextbox.instance.answerSelected == 1 && GameInput.GetButtonDown("Action"))
                        {
                            scrTextbox.instance.EndConversation();
                        }
                        //Plugin.BepinLogger.LogMessage("Answerselected: " + scrTextbox.instance.answerSelected);
                    }
                    else
                    {
                        scrTextbox.instance.canWaklaway = true;
                        scrTextbox.instance.textMesh.text = "Come back when you have " + __instance.price + " Cassettes to get (ItemName) for (PlayerName).";
                    }
                }

                if (scrTextbox.instance.isOn && scrTextbox.instance.nameMesh.text == "Mai")
                {
                    if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                    {
                        scrTextbox.instance.canWaklaway = false;
                        scrTextbox.instance.textMesh.text = "It will Cost " + __instance.price + " Cassettes to get (ItemName) for (PlayerName).";
                        scrTextbox.instance.answerCount = 2;
                        scrTextbox.instance.answerTextObjects[0].text = "Gimme!";
                        scrTextbox.instance.answerTextObjects[1].text = "Nah...";
                        if (scrTextbox.instance.answerSelected == 0 && GameInput.GetButtonDown("Action"))
                        {
                            if (!scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                            {
                                Object.Instantiate<GameObject>(__instance.coinObtainer).GetComponent<scrObtainCoin>().myFlag = __instance.myFlag; 
                                _gaveCoin2 = true;
                                //scrWorldSaveDataContainer.instance.coinFlags.Add("cassetteCoin2");
                            }
                            scrTextbox.instance.EndConversation();
                        }
                        if (scrTextbox.instance.answerSelected == 1 && GameInput.GetButtonDown("Action"))
                        {
                            scrTextbox.instance.EndConversation();
                        }
                    }
                    else
                    {
                        scrTextbox.instance.canWaklaway = true;
                        scrTextbox.instance.textMesh.text = "Come back when you have " + __instance.price + " Cassettes to get (ItemName) for (PlayerName).";
                    }
                }
                if (!_changed)
                {
                    Plugin.BepinLogger.LogInfo($"CassetteBuyer price updated to: {__instance.price} (Scene: {currentScene}, based on {count} cassette coins found).");
                    _changed = true;
                }
            }
            else
            {
                if (!_logged)
                {
                    Plugin.BepinLogger.LogInfo("Found 'Progressive' Cassette Logic!");
                    _logged = true;
                }
                int count = 1;
                var list = scrGameSaveManager.instance.gameData.worldsData;
                for (int i = 0; i < list.Count ; i++)
                {
                    if (list[i].coinFlags.Contains("cassetteCoin") || list[i].coinFlags.Contains("cassetteCoin2"))
                    {
                        count++;
                    }
                }
                __instance.price = 5 * count;
                
                if (!_changed)
                {
                    Plugin.BepinLogger.LogInfo($"CassetteBuyer price updated to: {__instance.price} (based on {count} cassette coins found).");
                    _changed = true;
                }
            }
        }
    }
}