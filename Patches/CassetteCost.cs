using Archipelago.MultiClient.Net.Enums;
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
    static int _mitchPrice = 0;
    private static int _maiPriceSafe = 0;
    static int _mimaPrice = 0;
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
            if (int.Parse(ArchipelagoData.slotData["cassette_logic"].ToString()) == 0)
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
                        _mitchIndex = 0;
                        _maiIndex = _mitchIndex + 1;
                        break;
                    case "Trash Kingdom":
                        __instance.price = 5;
                        _maiPrice = 10;
                        _mitchIndex = 2;
                        _maiIndex = _mitchIndex + 1;
                        break;
                    case "Salmon Creek Forest":
                        __instance.price = 5; 
                        _maiPrice = 10;
                        _mitchIndex = 4;
                        _maiIndex = _mitchIndex + 1;
                        break;
                    case "Public Pool":
                        __instance.price = 5; 
                        _maiPrice = 10;
                        _mitchIndex = 7;
                        _maiIndex = _mitchIndex - 1;
                        break;
                    case "The Bathhouse":
                        __instance.price = 5; 
                        _maiPrice = 10;
                        _mitchIndex = 8;
                        _maiIndex = _mitchIndex + 1;
                        break;
                    case "Tadpole inc":
                        __instance.price = 5; 
                        _maiPrice = 10;
                        _mitchIndex = 11;
                        _maiIndex = _mitchIndex - 1;
                        break;
                    case "GarysGarden":
                        __instance.price = 5; 
                        _maiPrice = 10;
                        _mitchIndex = 13;
                        _maiIndex = _mitchIndex - 1;
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
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchIndex) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[_mitchIndex]);
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchIndex);
                        }
                        if (_currentBox == 0)
                        {
                            _answerFix = false;
                        }
                        if (_currentBox == 1 && !_answerFix)
                        {
                            _textbox.conversationLocalized[1] =
                                $"It will cost " + __instance.price +
                                $" cassettes to get '{ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}." +
                                $"\nIt seems {ItemClassification(_mitchIndex)}... ##addinput:No!;skip0; ##addinput:Yes please!;skip1;";
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
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchIndex) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[_mitchIndex]);
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchIndex);
                        }
                        _textbox.canWaklaway = true;
                        if (_currentBox == 0 && !_answerFix)
                        {
                            _textbox.conversationLocalized[0] =
                                $"Come back when you have " + __instance.price +
                                $" cassettes to get '{ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}." +
                                $"\nIt seems {ItemClassification(_mitchIndex)}...";
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
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_maiIndex) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[_maiIndex]);
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_maiIndex);
                        }

                        if (_currentBox == 0)
                        {
                            _answerFix = false;
                        }
                        if (_currentBox == 1 && !_answerFix)
                        {
                            _textbox.conversationLocalized[1] = 
                                "It will cost " + _maiPrice + 
                                $" cassettes to get '{ArchipelagoClient.ScoutedLocations[_maiIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_maiIndex].Player}." +
                                $"\nIt seems {ItemClassification(_maiIndex)}... ##addinput:No!;skip0; ##addinput:Yes please!;skip1;";
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
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_maiIndex) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[_maiIndex]);
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_maiIndex);
                        }
                        _textbox.canWaklaway = true;
                        if (_currentBox == 0 && !_answerFix)
                        {
                            _textbox.conversationLocalized[0] = 
                                "Come back when you have " + _maiPrice + 
                                $" cassettes to get '{ArchipelagoClient.ScoutedLocations[_maiIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_maiIndex].Player}." +
                                $"\nIt seems {ItemClassification(_maiIndex)}...";
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
                        if (!scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains($"MiMa{count-1}"))
                        {
                            scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Add($"MiMa{count-1}");
                        }
                    }
                }
                __instance.price = 5 * count;
                _mimaPrice = __instance.price;
                var gardenAdjustment = 0;
                var snailShopAdjustment = 0;
                var gardenOffset = 0;
                var seedAdjustment = 0;
                var applessAdjustment = 0;
                if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
                {
                    if (int.Parse(ArchipelagoData.slotData["shuffle_garden"].ToString()) == 0)
                    {
                        gardenAdjustment = 13;
                        gardenOffset = 2;
                    }
                }
                if (ArchipelagoData.slotData.ContainsKey("snailshop"))
                {
                    if (int.Parse(ArchipelagoData.slotData["snailshop"].ToString()) == 0)
                    {
                        snailShopAdjustment = 16;
                    }
                }
                if (ArchipelagoData.slotData.ContainsKey("seedsanity"))
                    if (int.Parse(ArchipelagoData.slotData["seedsanity"].ToString()) == 0)
                        seedAdjustment = 30;
                if (!ArchipelagoData.slotData.ContainsKey("applessanity"))
                    if (int.Parse(ArchipelagoData.slotData["applessanity"].ToString()) == 0)
                        applessAdjustment = 296;
                var adjustment = gardenAdjustment + snailShopAdjustment + seedAdjustment + applessAdjustment;
                var offset = 14 - gardenOffset;
                var scoutId = 521 + (count-1) - adjustment;
                _mitchIndex = -1 + count;
                if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                {
                    MitchGameObject.parentNotBought.SetActive(true);
                    MitchGameObject.parentCantBuy.SetActive(false);
                    MitchGameObject.parentBought.SetActive(false);
                    if (_textbox.isOn && _textbox.nameMesh.text is "Mitch" or "ミッチ" &&
                        !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchIndex) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[scoutId+adjustment]);
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchIndex);
                        }
                        if (_currentBox == 0)
                        {
                            _answerFix = false;
                        }
                        if (_currentBox == 1 && !_answerFix)
                        {
                            _textbox.conversationLocalized[1] =
                                $"It will cost " + __instance.price +
                                $" cassettes to get '{ArchipelagoClient.ScoutedLocations[scoutId-offset].ItemName}' for {ArchipelagoClient.ScoutedLocations[scoutId-offset].Player}." +
                                $"\nIt seems {ItemClassification(scoutId-offset)}... ##addinput:No!;skip0; ##addinput:Yes please!;skip1;";
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
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchIndex) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[scoutId+adjustment]);
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchIndex);
                        }
                        _textbox.canWaklaway = true;
                        if (_currentBox == 0 && !_answerFix)
                        {
                            _textbox.conversationLocalized[0] =
                                $"Come back when you have " + __instance.price +
                                $" cassettes to get '{ArchipelagoClient.ScoutedLocations[scoutId - offset].ItemName}' for {ArchipelagoClient.ScoutedLocations[scoutId - offset].Player}." +
                                $"\nIt seems {ItemClassification(scoutId - offset)}...";
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
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchIndex) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[scoutId+adjustment]);
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchIndex);
                        }
                        if (_currentBox == 0)
                        {
                            _answerFix = false;
                        }
                        if (_currentBox == 1 && !_answerFix)
                        {
                            _textbox.conversationLocalized[1] = 
                                "It will cost " + __instance.price + 
                                $" cassettes to get '{ArchipelagoClient.ScoutedLocations[scoutId-offset].ItemName} for {ArchipelagoClient.ScoutedLocations[scoutId-offset].Player}'." +
                                $"\nIt seems {ItemClassification(scoutId-offset)}... ##addinput:No!;skip0; ##addinput:Yes please!;skip1;";
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
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchIndex) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[scoutId+adjustment]);
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchIndex);
                        }
                        _textbox.canWaklaway = true;
                        if (_currentBox == 0 && !_answerFix)
                        {
                            _textbox.conversationLocalized[0] =
                                "Come back when you have " + __instance.price + 
                                $" cassettes to get '{ArchipelagoClient.ScoutedLocations[scoutId-offset].ItemName}' for {ArchipelagoClient.ScoutedLocations[scoutId-offset].Player}." +
                                $"\nIt seems {ItemClassification(scoutId-offset)}...";
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
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchIndex) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[_mitchIndex]);
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchIndex);
                        }
                        if (_currentBox == 0)
                        {
                            _answerFix = false;
                        }
                        if (_currentBox == 1 && !_answerFix)
                        {
                            _textbox.conversationLocalized[1] =
                                $"It will cost " + __instance.price * 5 +
                                $" cassettes to get '{ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}." +
                                $"\nIt seems {ItemClassification(_mitchIndex)}... ##addinput:No!;skip0; ##addinput:Yes please!;skip1;";
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
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchIndex) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[_mitchIndex]);
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchIndex);
                        }
                        _textbox.canWaklaway = true;
                        if (_currentBox == 0 && !_answerFix)
                        {
                            _textbox.conversationLocalized[0] =
                                $"Come back when you have " + __instance.price * 5 +
                                $" cassettes to get '{ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}." +
                                $"\nIt seems {ItemClassification(_mitchIndex)}...";
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
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_maiIndex) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[_maiIndex]);
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_maiIndex);
                        }

                        if (_currentBox == 0)
                        {
                            _answerFix = false;
                        }
                        if (_currentBox == 1 && !_answerFix)
                        {
                            _textbox.conversationLocalized[1] = 
                                "It will cost " + _maiPrice * 5 + 
                                $" cassettes to get '{ArchipelagoClient.ScoutedLocations[_maiIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_maiIndex].Player}." +
                                $"\nIt seems {ItemClassification(_maiIndex)}... ##addinput:No!;skip0; ##addinput:Yes please!;skip1;";
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
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_maiIndex) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[_maiIndex]);
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_maiIndex);
                        }
                        _textbox.canWaklaway = true;
                        if (_currentBox == 0 && !_answerFix)
                        {
                            _textbox.conversationLocalized[0] = 
                                "Come back when you have " + _maiPrice * 5 + 
                                $" cassettes to get '{ArchipelagoClient.ScoutedLocations[_maiIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_maiIndex].Player}." +
                                $"\nIt seems {ItemClassification(_maiIndex)}...";
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
            return classification;
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