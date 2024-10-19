using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class CassetteCost
{
    static bool _gaveCoin = false;
    static bool _gaveCoin2 = false;
    static bool _changed = false;
    static int _maiPrice = 0;
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
                if (_textbox.isOn && _textbox.nameMesh.text == "Mitch" && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                {
                    if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchIndex) && ArchipelagoMenu.Hints)
                    {
                        ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[_mitchIndex]);
                        _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchIndex);
                    }
                    if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price)
                    {
                        if (_textbox.textMesh.text.Contains("Wanna trade"))
                        {
                            _textbox.textMesh.text = 
                                $"It will Cost " + __instance.price + $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}.";
                        }
                    }
                    else
                    {
                        _textbox.canWaklaway = true;
                        _textbox.textMesh.text = 
                            $"Come back when you have " + __instance.price + 
                            $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}.";
                    }
                }

                if (_textbox.isOn && _textbox.nameMesh.text == "Mai" && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                {
                    if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_maiIndex) && ArchipelagoMenu.Hints)
                    {
                        ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[_maiIndex]);
                        _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_maiIndex);
                    }
                    if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price)
                    {
                        if (_textbox.textMesh.text.Contains("Wanna trade"))
                        {
                            _textbox.textMesh.text = 
                                "It will Cost " + __instance.price + 
                                $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_maiIndex].ItemName} for {ArchipelagoClient.ScoutedLocations[_maiIndex].Player}'.";
                        }
                    }
                    else
                    {
                        _textbox.canWaklaway = true;
                        _textbox.textMesh.text = 
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
                
                if (_textbox.isOn && _textbox.nameMesh.text == "Mitch" && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                {
                    if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchIndex) && ArchipelagoMenu.Hints)
                    {
                        ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[_mitchIndex]);
                        _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchIndex);
                    }
                    if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price)
                    {
                        if (_textbox.textMesh.text.Contains("Wanna trade"))
                        {
                            _textbox.textMesh.text = 
                                $"It will Cost " + __instance.price + 
                                $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}.";
                        }
                    }
                    else
                    {
                        _textbox.canWaklaway = true;
                        _textbox.textMesh.text = 
                            $"Come back when you have " + __instance.price + 
                            $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}.";
                    }
                }

                if (_textbox.isOn && _textbox.nameMesh.text == "Mai" && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                {
                    if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint" + _maiIndex) && ArchipelagoMenu.Hints)
                    {
                        ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[_maiIndex]);
                        _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint" + _maiIndex);
                    }
                    
                    if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price)
                    {
                        if (_textbox.textMesh.text.Contains("Wanna trade"))
                        {
                            _textbox.textMesh.text = 
                                "It will Cost " + __instance.price + 
                                $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_maiIndex].ItemName} for {ArchipelagoClient.ScoutedLocations[_maiIndex].Player}'.";
                        }
                    }
                    else
                    {
                        _textbox.canWaklaway = true;
                        _textbox.textMesh.text = 
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
                if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= __instance.price * 5)
                {
                    MitchGameObject.parentNotBought.SetActive(true);
                    MitchGameObject.parentCantBuy.SetActive(false);
                    MitchGameObject.parentBought.SetActive(false);
                    if (_textbox.isOn && _textbox.nameMesh.text == "Mitch" &&
                        !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchIndex) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[_mitchIndex]);
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchIndex);
                        }
                        if (_textbox.textMesh.text.Contains("Wanna trade"))
                            _textbox.textMesh.text =
                                $"It will Cost " + __instance.price * 5 +
                                $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}.";
                        if (_textbox.textMesh.text.Contains("Alright, you got it")) MitchGameObject.buyCassette();
                    }
                    else if (scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin"))
                    {
                        MitchGameObject.parentNotBought.SetActive(false);
                        MitchGameObject.parentCantBuy.SetActive(false);
                        MitchGameObject.parentBought.SetActive(true);
                    }
                }
                else
                {
                    MitchGameObject.parentNotBought.SetActive(false);
                    MitchGameObject.parentCantBuy.SetActive(true);
                    MitchGameObject.parentBought.SetActive(false);
                    if (_textbox.isOn && _textbox.nameMesh.text == "Mitch")
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_mitchIndex) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[_mitchIndex]);
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_mitchIndex);
                        }
                        _textbox.canWaklaway = true;
                        _textbox.textMesh.text =
                            $"Come back when you have " + __instance.price * 5 +
                            $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_mitchIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_mitchIndex].Player}.";
                    }
                }
                if (MitchGameObject.isBought)
                {
                    MitchGameObject.parentNotBought.SetActive(false);
                    MitchGameObject.parentCantBuy.SetActive(false);
                    MitchGameObject.parentBought.SetActive(true);
                }
                if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount >= _maiPrice * 5)
                {
                    MaiGameObject.parentNotBought.SetActive(true);
                    MaiGameObject.parentCantBuy.SetActive(false);
                    MaiGameObject.parentBought.SetActive(false);
                    if (_textbox.isOn && _textbox.nameMesh.text == "Mai" && !scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_maiIndex) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[_maiIndex]);
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_maiIndex);
                        }
                        if (_textbox.textMesh.text.Contains("Wanna trade"))
                        {
                            _textbox.textMesh.text = 
                                "It will Cost " + _maiPrice * 5 + 
                                $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_maiIndex].ItemName} for {ArchipelagoClient.ScoutedLocations[_maiIndex].Player}'.";
                            if (_textbox.textMesh.text.Contains("Alright, you got it"))
                            {
                                MaiGameObject.buyCassette();
                            }
                        }
                    } else if (scrWorldSaveDataContainer.instance.coinFlags.Contains("cassetteCoin2"))
                    {
                        MaiGameObject.parentNotBought.SetActive(false);
                        MaiGameObject.parentCantBuy.SetActive(false);
                        MaiGameObject.parentBought.SetActive(true);
                    }
                } 
                else
                {
                    MaiGameObject.parentNotBought.SetActive(false);
                    MaiGameObject.parentCantBuy.SetActive(true);
                    MaiGameObject.parentBought.SetActive(false);
                    if (_textbox.isOn && _textbox.nameMesh.text == "Mai")
                    {
                        if (!_gameSaveManager.gameData.generalGameData.generalFlags.Contains("Hint"+_maiIndex) && ArchipelagoMenu.Hints)
                        {
                            ArchipelagoClient._session.Locations.ScoutLocationsAsync(true, Locations.ScoutIDs[_maiIndex]);
                            _gameSaveManager.gameData.generalGameData.generalFlags.Add("Hint"+_maiIndex);
                        }
                        _textbox.canWaklaway = true;
                        _textbox.textMesh.text = 
                            "Come back when you have " + _maiPrice * 5 + 
                            $" Cassettes to get '{ArchipelagoClient.ScoutedLocations[_maiIndex].ItemName}' for {ArchipelagoClient.ScoutedLocations[_maiIndex].Player}.";
                    }
                }
                if (MaiGameObject.isBought)
                {
                    MaiGameObject.parentNotBought.SetActive(false);
                    MaiGameObject.parentCantBuy.SetActive(false);
                    MaiGameObject.parentBought.SetActive(true);
                }
            }
        }
    }
}