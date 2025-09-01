using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace NikoArchipelago.Patches;

public class CassetteCost
{
    static bool _gaveCoin = false;
    static bool _gaveCoin2 = false;
    static bool _changed = false;
    static int _maiPrice = 0;
    static int _mitchPrice = 0;
    private static int _maiPriceSafe = 0;
    static int _mimaPrice = 0;
    private static int _maiHint, _mitchHint;
    private static bool _answerFix;
    public static scrCassetteBuyer MitchGameObject;
    public static scrCassetteBuyer MaiGameObject;
    private static scrTextbox _textbox;
    private static scrGameSaveManager _gameSaveManager;
    [HarmonyPatch(typeof(scrCassetteBuyer), "Update")]
    public static class PatchCassetteBuyer
    {
        static bool _logged = false;
        private static int _mitchIndex, _maiIndex;

        private static void Postfix(scrCassetteBuyer __instance)
        {
            if (scrTextbox.instance != null)
            {
                _textbox = scrTextbox.instance;
            }
            var currentBoxField = AccessTools.Field(typeof(scrTextbox), "currentBox");
            int _currentBox = (int)currentBoxField.GetValue(scrTextbox.instance);
            if (scrGameSaveManager.instance != null)
            {
                _gameSaveManager = scrGameSaveManager.instance;
            }
            if (ArchipelagoData.slotData == null) return;
            if (ArchipelagoData.Options.Cassette == ArchipelagoOptions.CassetteMode.LevelBased)
            {
                if (!_logged)
                {
                    Plugin.BepinLogger.LogInfo("Found 'Level Based' Cassette Logic!");
                    _logged = true;
                }
                string currentScene = SceneManager.GetActiveScene().name;
            
                switch (currentScene)
                {
                    case "Hairball City":
                        __instance.price = 5;
                        _maiPrice = 10;
                        _mitchIndex = 11;
                        _maiIndex = 12;
                        _mitchHint = 0;
                        _maiHint = _mitchHint + 1;
                        break;
                    case "Trash Kingdom":
                        __instance.price = 5;
                        _maiPrice = 10;
                        _mitchIndex = 23;
                        _maiIndex = 24;
                        _mitchHint = 2;
                        _maiHint = _mitchHint + 1;
                        break;
                    case "Salmon Creek Forest":
                        __instance.price = 5; 
                        _maiPrice = 10;
                        _mitchIndex = 29;
                        _maiIndex = 39;
                        _mitchHint = 4;
                        _maiHint = _mitchHint + 1;
                        break;
                    case "Public Pool":
                        __instance.price = 5; 
                        _maiPrice = 10;
                        _mitchIndex = 52;
                        _maiIndex = 45;
                        _mitchHint = 7;
                        _maiHint = _mitchHint - 1;
                        break;
                    case "The Bathhouse":
                        __instance.price = 5; 
                        _maiPrice = 10;
                        _mitchIndex = 58;
                        _maiIndex = 59;
                        _mitchHint = 8;
                        _maiHint = _mitchHint + 1;
                        break;
                    case "Tadpole inc":
                        __instance.price = 5; 
                        _maiPrice = 10;
                        _mitchIndex = 69;
                        _maiIndex = 68;
                        _mitchHint = 11;
                        _maiHint = _mitchHint - 1;
                        break;
                    case "GarysGarden":
                        __instance.price = 5; 
                        _maiPrice = 10;
                        _mitchIndex = 200;
                        _maiIndex = 199;
                        _mitchHint = 13;
                        _maiHint = _mitchHint - 1;
                        break;
                }
                _mitchPrice = __instance.price;
                _maiPriceSafe = _maiPrice;
                if (!_textbox.isOn) _answerFix = false;
                if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                {
                    MitchGameObject.parentNotBought.SetActive(true);
                    MitchGameObject.parentCantBuy.SetActive(false);
                    MitchGameObject.parentBought.SetActive(false);
                    if (_textbox.isOn && _textbox.nameMesh.text is "Mitch" or "ミッチ" &&
                        !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchHint))
                        {
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchHint);
                        }
                        if (_currentBox == 0)
                        {
                            _answerFix = false;
                        }
                        if (_currentBox == 1 && !_answerFix)
                        {
                            _textbox.conversationLocalized[1] = CassetteConversation(__instance.price, _mitchIndex);
                            _answerFix = true;
                        }
                        if (_currentBox == 3)
                        {
                            MitchGameObject.buyCassette();
                        }
                    }
                    else if (scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                    {
                        MitchGameObject.parentNotBought.SetActive(false);
                        MitchGameObject.parentCantBuy.SetActive(false);
                        MitchGameObject.parentBought.SetActive(true);
                    }
                }
                else if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount < __instance.price
                         && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                {
                    MitchGameObject.parentNotBought.SetActive(false);
                    MitchGameObject.parentCantBuy.SetActive(true);
                    MitchGameObject.parentBought.SetActive(false);
                    if (!_textbox.isOn) _answerFix = false;
                    if (_textbox.isOn && _textbox.nameMesh.text is "Mitch" or "ミッチ")
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchHint))
                        {
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchHint);
                        }
                        _textbox.canWaklaway = true;
                        if (_currentBox == 0 && !_answerFix)
                        {
                            _textbox.conversationLocalized[0] = CassetteConversation(__instance.price, _mitchIndex, false);
                            _answerFix = true;
                        }
                    }
                }
                if (MitchGameObject.isBought || scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                {
                    MitchGameObject.parentNotBought.SetActive(false);
                    MitchGameObject.parentCantBuy.SetActive(false);
                    MitchGameObject.parentBought.SetActive(true);
                }

                if (!_textbox.isOn) _answerFix = false;
                if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= _maiPrice && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                {
                    MaiGameObject.parentNotBought.SetActive(true);
                    MaiGameObject.parentCantBuy.SetActive(false);
                    MaiGameObject.parentBought.SetActive(false);
                    if (_textbox.isOn && _textbox.nameMesh.text is "Mai" or "マイ" && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_maiHint))
                        {
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_maiHint);
                        }

                        if (_currentBox == 0)
                        {
                            _answerFix = false;
                        }
                        if (_currentBox == 1 && !_answerFix)
                        {
                            _textbox.conversationLocalized[1] = CassetteConversation(_maiPrice, _maiIndex);
                            _answerFix = true;
                        }
                        if (_currentBox == 3)
                        {
                            MaiGameObject.buyCassette();
                        }
                    } else if (scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                    {
                        MaiGameObject.parentNotBought.SetActive(false);
                        MaiGameObject.parentCantBuy.SetActive(false);
                        MaiGameObject.parentBought.SetActive(true);
                    }
                } 
                else if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount < _maiPrice && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                {
                    MaiGameObject.parentNotBought.SetActive(false);
                    MaiGameObject.parentCantBuy.SetActive(true);
                    MaiGameObject.parentBought.SetActive(false);
                    if (!_textbox.isOn)
                    {
                        _answerFix = false;
                    }
                    if (_textbox.isOn && _textbox.nameMesh.text is "Mai" or "マイ")
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_maiHint))
                        {
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_maiHint);
                        }
                        _textbox.canWaklaway = true;
                        if (_currentBox == 0 && !_answerFix)
                        {
                            _textbox.conversationLocalized[0] = CassetteConversation(_maiPrice, _maiIndex, false);
                            _answerFix = true;
                        }
                    }
                }
                if (MaiGameObject.isBought || scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                {
                    MaiGameObject.parentNotBought.SetActive(false);
                    MaiGameObject.parentCantBuy.SetActive(false);
                    MaiGameObject.parentBought.SetActive(true);
                }
            }
            else if (ArchipelagoData.Options.Cassette == ArchipelagoOptions.CassetteMode.Progressive)
            {
                if (!_logged)
                {
                    Plugin.BepinLogger.LogInfo("Found 'Progressive' Cassette Logic!");
                    _logged = true;
                }
                var list = scrGameSaveManager.instance.gameData.worldsData;
                var mitch = list.Count(world => world.coinFlags.Contains("cassetteCoin"));
                var mai = list.Count(world => world.coinFlags.Contains("cassetteCoin2"));
                var count = 1 + mitch + mai;
                for (int i = 0; i < count; i++)
                {
                    if (!scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains($"MiMa{i}"))
                    {
                        scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Add($"MiMa{i}");
                    }
                }
                
                __instance.price = 5 * count;
                _mimaPrice = __instance.price;
                var scoutID = 1000 + count;
                
                _mitchIndex = -1 + count;
                if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                {
                    MitchGameObject.parentNotBought.SetActive(true);
                    MitchGameObject.parentCantBuy.SetActive(false);
                    MitchGameObject.parentBought.SetActive(false);
                    if (_textbox.isOn && _textbox.nameMesh.text is "Mitch" or "ミッチ" &&
                        !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchIndex))
                        {
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchIndex);
                        }
                        if (_currentBox == 0)
                        {
                            _answerFix = false;
                        }
                        if (_currentBox == 1 && !_answerFix)
                        {
                            _textbox.conversationLocalized[1] = CassetteConversation(__instance.price, scoutID);
                            _answerFix = true;
                        }
                        if (_currentBox == 3)
                        {
                            MitchGameObject.buyCassette();
                        }
                    }
                    else if (scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                    {
                        MitchGameObject.parentNotBought.SetActive(false);
                        MitchGameObject.parentCantBuy.SetActive(false);
                        MitchGameObject.parentBought.SetActive(true);
                    }
                }
                else if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount < __instance.price && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                {
                    MitchGameObject.parentNotBought.SetActive(false);
                    MitchGameObject.parentCantBuy.SetActive(true);
                    MitchGameObject.parentBought.SetActive(false);
                    if (!_textbox.isOn)
                    {
                        _answerFix = false;
                    }
                    if (_textbox.isOn && _textbox.nameMesh.text is "Mitch" or "ミッチ")
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchIndex))
                        {
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchIndex);
                        }
                        _textbox.canWaklaway = true;
                        if (_currentBox == 0 && !_answerFix)
                        {
                            _textbox.conversationLocalized[0] = CassetteConversation(__instance.price, scoutID, false);
                            _answerFix = true;
                        }
                    }
                }
                if (MitchGameObject.isBought || scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                {
                    MitchGameObject.parentNotBought.SetActive(false);
                    MitchGameObject.parentCantBuy.SetActive(false);
                    MitchGameObject.parentBought.SetActive(true);
                }
                if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                {
                    MaiGameObject.parentNotBought.SetActive(true);
                    MaiGameObject.parentCantBuy.SetActive(false);
                    MaiGameObject.parentBought.SetActive(false);
                    if (_textbox.isOn && _textbox.nameMesh.text is "Mai" or "マイ" && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchIndex))
                        {
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchIndex);
                        }
                        if (_currentBox == 0)
                        {
                            _answerFix = false;
                        }
                        if (_currentBox == 1 && !_answerFix)
                        {
                            _textbox.conversationLocalized[1] = CassetteConversation(__instance.price, scoutID);
                            _answerFix = true;
                        }
                        if (_currentBox == 3)
                        {
                            MaiGameObject.buyCassette();
                        }
                    } else if (scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                    {
                        MaiGameObject.parentNotBought.SetActive(false);
                        MaiGameObject.parentCantBuy.SetActive(false);
                        MaiGameObject.parentBought.SetActive(true);
                    }
                } 
                else if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount < __instance.price && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                {
                    MaiGameObject.parentNotBought.SetActive(false);
                    MaiGameObject.parentCantBuy.SetActive(true);
                    MaiGameObject.parentBought.SetActive(false);
                    if (!_textbox.isOn)
                    {
                        _answerFix = false;
                    }
                    if (_textbox.isOn && _textbox.nameMesh.text is "Mai" or "マイ")
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchIndex))
                        {
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchIndex);
                        }
                        _textbox.canWaklaway = true;
                        if (_currentBox == 0 && !_answerFix)
                        {
                            _textbox.conversationLocalized[0] = CassetteConversation(__instance.price, scoutID, false);
                            _answerFix = true;
                        }
                    }
                }
                if (MaiGameObject.isBought || scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                {
                    MaiGameObject.parentNotBought.SetActive(false);
                    MaiGameObject.parentCantBuy.SetActive(false);
                    MaiGameObject.parentBought.SetActive(true);
                }

                if (_changed) return;
                Plugin.BepinLogger.LogInfo($"CassetteBuyer price updated to: {__instance.price} (based on {count-1} cassette coins found).");
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
                        _mitchIndex = 11;
                        _maiIndex = 12;
                        _mitchHint = 0;
                        _maiHint = _mitchHint + 1;
                        break;
                    case "Trash Kingdom":
                        __instance.price = int.Parse(slotData["ctt1"].ToString());
                        _maiPrice = int.Parse(slotData["ctt2"].ToString());
                        _mitchIndex = 23;
                        _maiIndex = 24;
                        _mitchHint = 2;
                        _maiHint = _mitchHint + 1;
                        break;
                    case "Salmon Creek Forest":
                        __instance.price = int.Parse(slotData["csfc1"].ToString());
                        _maiPrice = int.Parse(slotData["csfc2"].ToString());
                        _mitchIndex = 29;
                        _maiIndex = 39;
                        _mitchHint = 4;
                        _maiHint = _mitchHint + 1;
                        break;
                    case "Public Pool":
                        __instance.price = int.Parse(slotData["cpp1"].ToString());
                        _maiPrice = int.Parse(slotData["cpp2"].ToString());
                        _mitchIndex = 52;
                        _maiIndex = 45;
                        _mitchHint = 7;
                        _maiHint = _mitchHint - 1;
                        break;
                    case "The Bathhouse":
                        __instance.price = int.Parse(slotData["cbath1"].ToString());
                        _maiPrice = int.Parse(slotData["cbath2"].ToString());
                        _mitchIndex = 58;
                        _maiIndex = 59;
                        _mitchHint = 8;
                        _maiHint = _mitchHint + 1;
                        break;
                    case "Tadpole inc":
                        __instance.price = int.Parse(slotData["chq1"].ToString());
                        _maiPrice = int.Parse(slotData["chq2"].ToString());
                        _mitchIndex = 69;
                        _maiIndex = 68;
                        _mitchHint = 11;
                        _maiHint = _mitchHint - 1;
                        break;
                    case "GarysGarden":
                        __instance.price = int.Parse(slotData["cgg1"].ToString());
                        _maiPrice = int.Parse(slotData["cgg2"].ToString());
                        _mitchIndex = 200;
                        _maiIndex = 199;
                        _mitchHint = 13;
                        _maiHint = _mitchHint - 1;
                        break;
                }
                _mitchPrice = __instance.price;
                _maiPriceSafe = _maiPrice;
                if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price * 5 && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                {
                    MitchGameObject.parentNotBought.SetActive(true);
                    MitchGameObject.parentCantBuy.SetActive(false);
                    MitchGameObject.parentBought.SetActive(false);
                    if (_textbox.isOn && _textbox.nameMesh.text is "Mitch" or "ミッチ" &&
                        !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchHint))
                        {
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchHint);
                        }
                        if (_currentBox == 0)
                        {
                            _answerFix = false;
                        }
                        if (_currentBox == 1 && !_answerFix)
                        {
                            _textbox.conversationLocalized[1] = CassetteConversation(__instance.price * 5, _mitchIndex);
                            _answerFix = true;
                        }
                        if (_currentBox == 3)
                        {
                            MitchGameObject.buyCassette();
                        }
                    }
                    else if (scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                    {
                        MitchGameObject.parentNotBought.SetActive(false);
                        MitchGameObject.parentCantBuy.SetActive(false);
                        MitchGameObject.parentBought.SetActive(true);
                    }
                }
                else if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount < __instance.price * 5  && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                {
                    MitchGameObject.parentNotBought.SetActive(false);
                    MitchGameObject.parentCantBuy.SetActive(true);
                    MitchGameObject.parentBought.SetActive(false);
                    if (!_textbox.isOn)
                    {
                        _answerFix = false;
                    }
                    if (_textbox.isOn && _textbox.nameMesh.text is "Mitch" or "ミッチ")
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchHint))
                        {
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchHint);
                        }
                        _textbox.canWaklaway = true;
                        if (_currentBox == 0 && !_answerFix)
                        {
                            _textbox.conversationLocalized[0] = CassetteConversation(__instance.price * 5, _mitchIndex, false);
                            _answerFix = true;
                        }
                    }
                }
                if (MitchGameObject.isBought || scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                {
                    MitchGameObject.parentNotBought.SetActive(false);
                    MitchGameObject.parentCantBuy.SetActive(false);
                    MitchGameObject.parentBought.SetActive(true);
                }
                if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= _maiPrice * 5 && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                {
                    MaiGameObject.parentNotBought.SetActive(true);
                    MaiGameObject.parentCantBuy.SetActive(false);
                    MaiGameObject.parentBought.SetActive(false);
                    if (_textbox.isOn && _textbox.nameMesh.text is "Mai" or "マイ" && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_maiHint))
                        {
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_maiHint);
                        }

                        if (_currentBox == 0)
                        {
                            _answerFix = false;
                        }
                        if (_currentBox == 1 && !_answerFix)
                        {
                            _textbox.conversationLocalized[1] = CassetteConversation(_maiPrice*5, _maiIndex);
                            _answerFix = true;
                        }
                        if (_currentBox == 3)
                        {
                            MaiGameObject.buyCassette();
                        }
                    } else if (scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                    {
                        MaiGameObject.parentNotBought.SetActive(false);
                        MaiGameObject.parentCantBuy.SetActive(false);
                        MaiGameObject.parentBought.SetActive(true);
                    }
                } 
                else if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount < _maiPrice * 5 && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                {
                    MaiGameObject.parentNotBought.SetActive(false);
                    MaiGameObject.parentCantBuy.SetActive(true);
                    MaiGameObject.parentBought.SetActive(false);
                    if (!_textbox.isOn)
                    {
                        _answerFix = false;
                    }
                    if (_textbox.isOn && _textbox.nameMesh.text is "Mai" or "マイ")
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_maiHint))
                        {
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_maiHint);
                        }
                        _textbox.canWaklaway = true;
                        if (_currentBox == 0 && !_answerFix)
                        {
                            _textbox.conversationLocalized[0] = CassetteConversation(_maiPrice*5, _maiIndex, false);
                            _answerFix = true;
                        }
                    }
                }
                if (MaiGameObject.isBought || scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                {
                    MaiGameObject.parentNotBought.SetActive(false);
                    MaiGameObject.parentCantBuy.SetActive(false);
                    MaiGameObject.parentBought.SetActive(true);
                }
            }
        }
        private static string CassetteConversation(int price, int scoutID, bool canBuy = true)
        {
            string classification;
            var scoutedItem = ArchipelagoClient.ScoutLocation(scoutID);
            var itemName = scoutedItem.ItemName; 
            if (itemName == null)
                itemName = "Item: "+scoutedItem.ItemId;
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
            var text = "";
            if (canBuy)
            {
                text = $"It will cost " + price +
                       $" cassettes to get '{itemName}' for {scoutedItem.Player}." +
                       $"\nIt seems {classification}... ##addinput:No!;skip0; ##addinput:Yes please!;skip1;";
            }
            else
            {
                text = $"Come back when you have " + price +
                       $" cassettes to get '{itemName}' for {scoutedItem.Player}." +
                       $"\nIt seems {classification}...";
            }
            return text;
        }
    }
    public static int MitchPrice(int world = 0, int sanity = 2)
    {
        if (ArchipelagoData.slotData == null) return 0;
        if (sanity == 2)
            _mitchPrice = world switch
            {
                1 => int.Parse(ArchipelagoData.slotData["chc1"].ToString()),
                2 => int.Parse(ArchipelagoData.slotData["ctt1"].ToString()),
                3 => int.Parse(ArchipelagoData.slotData["csfc1"].ToString()),
                4 => int.Parse(ArchipelagoData.slotData["cpp1"].ToString()),
                5 => int.Parse(ArchipelagoData.slotData["cbath1"].ToString()),
                6 => int.Parse(ArchipelagoData.slotData["chq1"].ToString()),
                7 => int.Parse(ArchipelagoData.slotData["cgg1"].ToString()),
                _ => _mitchPrice
            };
        else if (sanity == 1)
        {
            _mitchPrice = _mimaPrice;
            _mitchPrice /= 5;
        }
        return _mitchPrice;
    }
        
    public static int MaiPrice(int world = 0, int sanity = 2)
    {
        if (ArchipelagoData.slotData == null) return 0;
        if (sanity == 2)
            _maiPriceSafe = world switch
            {
                1 => int.Parse(ArchipelagoData.slotData["chc2"].ToString()),
                2 => int.Parse(ArchipelagoData.slotData["ctt2"].ToString()),
                3 => int.Parse(ArchipelagoData.slotData["csfc2"].ToString()),
                4 => int.Parse(ArchipelagoData.slotData["cpp2"].ToString()),
                5 => int.Parse(ArchipelagoData.slotData["cbath2"].ToString()),
                6 => int.Parse(ArchipelagoData.slotData["chq2"].ToString()),
                7 => int.Parse(ArchipelagoData.slotData["cgg2"].ToString()),
                _ => _maiPriceSafe
            };
        else if (sanity == 1)
        {
            _maiPriceSafe = _mimaPrice;
            _maiPriceSafe /= 5;
        }
        return _maiPriceSafe;
    }
}