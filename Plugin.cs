using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.Serialization;

namespace NikoArchipelago
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInProcess("Here Comes Niko!.exe")]
    public class Plugin : BaseUnityPlugin
    {
        /*
         * Goal for 0.1.0 - Have checks working (Sending & Receiving)
         * 
         * 1.0.0 UI - Not sure how to make it look nice, preferably use the in-game ui
         *  to create a server button to join an archipelago session + create a new save file for it (likely upon pressing start)
         *
         * #Find a way to use the in-game Notification system to show item received and send
         *  with the AP logo preferably and no noise/voice
         *
         * #Flags are documented on the spreadsheet + Handsome Frog checks are addable 
         */
        private const string PLUGIN_GUID = "nieli.NikoArchipelago";
        private const string PLUGIN_NAME = "NikoArchipelago";
        private const string PLUGIN_VERSION = "0.0.1";
        
        private const string ModDisplayInfo = $"{PLUGIN_NAME} v{PLUGIN_VERSION}";
        private const string APDisplayInfo = $"Archipelago v{ArchipelagoClient.APVersion}";
        public static ManualLogSource BepinLogger;
        public static ArchipelagoClient ArchipelagoClient;
        private string _saveName;
        public scrGameSaveManager gameSaveManager;
        private bool _saveReady;
        private bool _loggedError, _loggedSuccess;
        
        public static CustomButton AptestButton;
        private List<string> _saveDataCoinFlag, _saveDataCassetteFlag, _saveDataFishFlag, _saveDataMiscFlag, _saveDataLetterFlag, _saveDataGeneralFlag;
        private List<bool> _unlockedLevels;
        private List<string> _uniqueFlags;
        private int _coinFlg, _cassetteFlg, _fishFlg, _miscFlg, _letterFlg, _generalFlg, _coinTotal, _coinOld, _levelIndex;
        private readonly Notification _note = ScriptableObject.CreateInstance<Notification>();
        private int _goToLevel;
        private float _env, _mas, _mus, _sfx;
        public scrNotificationDisplayer noteDisplayer;
        
        private void Awake()
        {
            BepinLogger = Logger;
            ArchipelagoClient = new ArchipelagoClient();
            ArchipelagoConsole.Awake();
            // Plugin startup logic
            Logger.LogInfo($"Hey, Niko here! btw Plugin {PLUGIN_NAME} Loaded! :)");
            ArchipelagoConsole.LogMessage($"{ModDisplayInfo} loaded!");
            _saveName = ArchipelagoClient.ServerData.SlotName;
        }


        public void Load()
        {
            //LoadPatches();
            MainMenu_Postfix();
            levelData_Prefix();
        }

        public void Start()
        {
            levelData_Prefix();
            //MyCharacterController.instance._diveConsumed = true;
            GameOptions.MasterVolume = _mas;
            GameOptions.EnvVolume = _env;
            GameOptions.MusicVolume = _mus;
            GameOptions.SFXVolume = _sfx;
            StartCoroutine(CheckGameSaveManager());
        }
        
        private IEnumerator CheckGameSaveManager()
        {
            while (!scrGameSaveManager.instance)
            {
                Logger.LogError("GameSaveManager is null.");
                yield return new WaitForSeconds(3.0f);
            }
            Logger.LogInfo("GameSaveManager is not null.");
            gameSaveManager = scrGameSaveManager.instance;
            _saveReady = true;
            // Perform additional actions here if needed
        }
        
        public void Update()
        {
            //MainMenu_Postfix();
            if (!_saveReady) return;
            try
            {
                levelData_Prefix();
                noteDisplayer = scrNotificationDisplayer.instance;
                _saveName = "APSave" + ArchipelagoClient.ServerData.SlotName + ArchipelagoClient.ServerData.Uri; //Savefile is the same as SlotName & ServerPort
                if (scrGameSaveManager.saveName != _saveName && ArchipelagoClient.Authenticated)
                {
                    scrGameSaveManager.saveName = _saveName;
                    var savePath = Path.Combine(Application.persistentDataPath, _saveName + ".json");
                    if (File.Exists(savePath)) //Check if there already is a Save with the SlotName & ServerPort
                    {
                        scrGameSaveManager.dataPath = Path.Combine(Application.persistentDataPath, _saveName + ".json");
                        Logger.LogInfo("Found a SaveFile with the current SlotName & Port!");
                        ArchipelagoConsole.LogMessage("Found a SaveFile with the current SlotName & Port!");
                        gameSaveManager.LoadGame();
                    }
                    else
                    {
                        scrGameSaveManager.dataPath = Path.Combine(Application.persistentDataPath, _saveName + ".json");
                        Logger.LogWarning("No SaveFile found. Creating a new one!");
                        ArchipelagoConsole.LogMessage("No SaveFile found. Creating a new one!");
                        gameSaveManager.SaveGame();
                        gameSaveManager.LoadGame();
                        gameSaveManager.ClearSaveData();
                    }
                    scrTrainManager.instance.UseTrain(gameSaveManager.gameData.generalGameData.currentLevel, false);
                    SendNote($"Connected to {ArchipelagoClient.ServerData.Uri} successfully");
                    _saveDataCoinFlag.ForEach(Logger.LogMessage);
                    _saveDataCassetteFlag.ForEach(Logger.LogMessage);
                    _saveDataFishFlag.ForEach(Logger.LogMessage);
                    _saveDataMiscFlag.ForEach(Logger.LogMessage);
                    _saveDataLetterFlag.ForEach(Logger.LogMessage);
                    _saveDataGeneralFlag.ForEach(Logger.LogMessage); //Quick flag checking
                }
                //MyCharacterController.instance._diveConsumed = true;
                Flags();
                if (_loggedSuccess) return;
                Logger.LogMessage("Game finished initialising");
                _loggedSuccess = true;
            }
            catch (Exception e)
            {
                if (!_loggedError)
                {
                    Logger.LogMessage("Waiting for game to finish initialising");
                    _loggedError = true;
                }
            }
        }

        public void SendNote(string note)
        {
            _note.key = note;
            _note.duration = 2.5F;
            _note.timed = true;
            //_note.avatar = _spriteRenderer.sprite;
            _note.avatar = scrSnail.instance.sprFoodFull;
            noteDisplayer.AddNotification(_note);
            noteDisplayer.textMesh.text = _note.key;
            //NoteDisplayer.AnimatePopup();
            Logger.LogInfo("Note: "+noteDisplayer.textMesh.text);
        }
        
        public void ConvertFlags()
        {
            var worldsData = scrGameSaveManager.instance.gameData.worldsData; // Enthält alle welten/level
            for (int i = 0; i < worldsData.Count; i++)
            {
                Logger.LogFatal($"This is Level '{i}'!");
                worldsData[i].coinFlags.ForEach(Logger.LogInfo); //Gibt von welt 0-6 coinFlags | 7-15 existieren nicht bzw. cutscenes
                Logger.LogFatal("Cassette Flags:");
                worldsData[i].cassetteFlags.ForEach(Logger.LogInfo); //Give all cassettes unique flags Cassette (2) = Cassette22/turCassette2
                for (int j = 0; j < 70; j++)
                {
                    _uniqueFlags.Add("Cassette" + j);
                }
                Logger.LogFatal("Letter Flags:");
                worldsData[i].letterFlags.ForEach(Logger.LogInfo);
                Logger.LogFatal("Misc Flags:");
                worldsData[i].miscFlags.ForEach(Logger.LogInfo);
            }
        }
        
        public void Flags()
        {
            // Assign lists
            _saveDataCoinFlag = scrWorldSaveDataContainer.instance.coinFlags;
            _saveDataCassetteFlag = scrWorldSaveDataContainer.instance.cassetteFlags;
            _saveDataFishFlag = scrWorldSaveDataContainer.instance.fishFlags;
            _saveDataLetterFlag = scrWorldSaveDataContainer.instance.letterFlags;
            _saveDataMiscFlag = scrWorldSaveDataContainer.instance.miscFlags;
            _saveDataGeneralFlag = scrGameSaveManager.instance.gameData.generalGameData.generalFlags;
            _unlockedLevels = scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels;
            _coinTotal = scrGameSaveManager.instance.gameData.generalGameData.coinAmountTotal;
            if (_saveDataCoinFlag.Count > _coinFlg) // Check if a new flag has been added
            {
                Logger.LogFatal("New Coin Flag! '" + _saveDataCoinFlag[_coinFlg] + "'");
                _coinFlg++;
            }
            else if (_coinFlg > _saveDataCoinFlag.Count) // Probably not needed, but just in case of a level transition
            {
                _coinFlg = 0;
            }

            if (_saveDataCassetteFlag.Count > _cassetteFlg) // Check if a new flag has been added
            {
                Logger.LogFatal("New Cassette Flag! '" + _saveDataCassetteFlag[_cassetteFlg] + "'");
                _cassetteFlg++;
            }
            else if (_cassetteFlg > _saveDataCassetteFlag.Count) // Probably not needed, but just in case of a level transition
            {
                _cassetteFlg = 0;
            }

            if (_saveDataFishFlag.Count > _fishFlg) // Check if a new flag has been added
            {
                Logger.LogFatal("New Fish Flag! '" + _saveDataFishFlag[_fishFlg] + "'");
                _fishFlg++;
            }
            else if (_fishFlg > _saveDataFishFlag.Count) // Probably not needed, but just in case of a level transition
            {
                _fishFlg = 0;
            }

            if (_saveDataMiscFlag.Count > _miscFlg) // Check if a new flag has been added
            {
                Logger.LogFatal("New Misc Flag! '" + _saveDataMiscFlag[_miscFlg] + "'");
                _miscFlg++;
            }
            else if (_miscFlg > _saveDataMiscFlag.Count) // Probably not needed, but just in case of a level transition
            {
                _miscFlg = 0;
            }

            if (_saveDataLetterFlag.Count > _letterFlg) // Check if a new flag has been added
            {
                Logger.LogFatal("New Letter Flag! '" + _saveDataLetterFlag[_letterFlg] + "'");
                _letterFlg++;
            }
            else if (_letterFlg > _saveDataLetterFlag.Count) // Probably not needed, but just in case of a level transition
            {
                _letterFlg = 0;
            }
            
            if (_saveDataGeneralFlag.Count > _generalFlg) // Check if a new flag has been added
            {
                Logger.LogFatal("New General Flag! '" + _saveDataGeneralFlag[_generalFlg] + "'");
                _generalFlg++;
            }
            else if (_generalFlg > _saveDataGeneralFlag.Count) // Probably not needed, but just in case of a level transition
            {
                _generalFlg = 0;
            }

            if (_unlockedLevels.Count > _levelIndex) //TODO: Besseres System zum checken, ob ein level freigeschaltet ist oder nicht (True or False)
            {
                Logger.LogWarning("New Level unlocked! " + _unlockedLevels[_levelIndex]);
                _levelIndex++;
            }

            if (_coinTotal > _coinOld)
            {
                Logger.LogWarning("Total Coin Count = " + _coinTotal);
                _coinOld++;
            }
        }

        void WindowFunc(int windowID)
        {
            GUI.color = Color.yellow;
            if (GUI.Button(new Rect(10, 20, 100, 20), "Current Flags"))
            {
                // Notification note = ScriptableObject.CreateInstance<Notification>();
                // note.key = "test";
                // scrNotificationDisplayer.instance.AddNotification(note);
                //scrWorldSaveDataContainer.instance.coinFlags.Add("volley");
                //Logger.LogInfo(scrGameSaveManager.instance.gameData.worldsData.ToList());
                //System.Collections.Generic.List<scrWorldSaveDataContainer>;
                _saveDataCoinFlag.ForEach(Logger.LogFatal);
                _saveDataCassetteFlag.ForEach(Logger.LogInfo);
                _saveDataFishFlag.ForEach(Logger.LogError);
                _saveDataMiscFlag.ForEach(Logger.LogWarning);
                _saveDataLetterFlag.ForEach(Logger.LogMessage);
                _saveDataGeneralFlag.ForEach(Logger.LogInfo);
                Logger.LogFatal("Current Level: " + scrTrainManager.instance.currentLevel);
            }
            GUI.color = Color.red;
            if (GUI.Button(new Rect(290, 170, 100, 20), "RESET"))
            {
                //scrGameSaveManager.instance.loaded=false;
                //scrGameSaveManager.instance.LoadGame();
                //scrWorldSaveDataContainer.instance.LoadWorld();
                //print(scrWorldSaveDataContainer.instance.coinFlags);
                //Logger.LogMessage(scrWorldSaveDataContainer.instance.coinFlags);
                //Logger.LogMessage(scrGameSaveManager.instance.copiedGameData.generalGameData.generalFlags);
                scrWorldSaveDataContainer.instance.gameSaveManager.ClearSaveData();
                scrGameSaveManager.instance.LoadGame();
            }
            GUI.color = Color.white;
            if (GUI.Button(new Rect(120, 50, 100, 20), "Add Flag"))
            {
                var pls = scrGameSaveManager.instance.gameData.worldsData; // Enthält alle welten/level
                for (int i = 0; i < pls.Count; i++)
                {
                    if (i == 0)
                    {
                        pls[i].letterFlags.Add("letter12");
                    }
                    Logger.LogFatal($"This is Level '{i}'!");
                    pls[i].coinFlags.ForEach(Logger.LogInfo); //Gibt von welt 0-6 coinFlags | 7-15 existieren nicht bzw. cutscenes
                }
                _unlockedLevels[7] = true; // Schaltet ein Level frei hier (7) = Tadpole HQ
                // scrGameSaveManager.instance.gameData.generalGameData.currentLevel = 1;
                // scrTrainManager.instance.UseTrain(temp+1, false);
            }

            if (GUI.Button(new Rect(120, 20, 100, 20), "Alle Flags"))
            {
                var pls = scrGameSaveManager.instance.gameData.worldsData; // Enthält alle welten/level
                for (int i = 0; i < pls.Count; i++)
                {
                    Logger.LogFatal($"This is Level '{i}'!");
                    pls[i].coinFlags.ForEach(Logger.LogInfo); //Gibt von welt 0-6 coinFlags | 7-15 existieren nicht bzw. cutscenes
                    Logger.LogFatal("Cassette Flags:");
                    pls[i].cassetteFlags.ForEach(Logger.LogInfo);
                    Logger.LogFatal("Letter Flags:");
                    pls[i].letterFlags.ForEach(Logger.LogInfo);
                    Logger.LogFatal("Misc Flags:");
                    pls[i].miscFlags.ForEach(Logger.LogInfo);
                }
                Logger.LogFatal("Current Level: " + scrTrainManager.instance.currentLevel);
            }
            if (GUI.Button(new Rect(230, 20, 100, 20), "TP to 1"))
            {
                scrTrainManager.instance.UseTrain(1, false);
            }
            if (GUI.Button(new Rect(230, 50, 100, 20), "TP to 2"))
            {
                scrTrainManager.instance.UseTrain(2, false);
            }
            if (GUI.Button(new Rect(230, 80, 100, 20), "TP to 3"))
            {
                scrTrainManager.instance.UseTrain(3, false);
            }
            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }
        
        private void OnGUI()
        {
            // show the mod is currently loaded in the corner
            GUI.Label(new Rect(16, 16, 300, 20), ModDisplayInfo);
            ArchipelagoConsole.OnGUI();

            string statusMessage;
            // show the Archipelago Version and whether we're connected or not
            if (ArchipelagoClient.Authenticated)
            {
                // if your game doesn't usually show the cursor this line may be necessary
                //Cursor.visible = false;

                statusMessage = " Status: Connected";
                GUI.Label(new Rect(16, 50, 300, 20), APDisplayInfo + statusMessage);
            }
            else
            {
                // if your game doesn't usually show the cursor this line may be necessary
                //Cursor.visible = true;

                statusMessage = " Status: Disconnected";
                GUI.Label(new Rect(16, 50, 300, 20), APDisplayInfo + statusMessage);
                GUI.Label(new Rect(16, 70, 150, 20), "Host: ");
                GUI.Label(new Rect(16, 90, 150, 20), "Player Name: ");
                GUI.Label(new Rect(16, 110, 150, 20), "Password: ");

                ArchipelagoClient.ServerData.Uri = GUI.TextField(new Rect(150, 70, 150, 20),
                    ArchipelagoClient.ServerData.Uri);
                ArchipelagoClient.ServerData.SlotName = GUI.TextField(new Rect(150, 90, 150, 20),
                    ArchipelagoClient.ServerData.SlotName);
                ArchipelagoClient.ServerData.Password = GUI.TextField(new Rect(150, 110, 150, 20),
                    ArchipelagoClient.ServerData.Password);

                // requires that the player at least puts *something* in the slot name
                if (GUI.Button(new Rect(16, 130, 100, 20), "Connect") &&
                    !ArchipelagoClient.ServerData.SlotName.IsNullOrWhiteSpace())
                {
                    ArchipelagoClient.Connect();
                }
            }
            // this is a good place to create and add a bunch of debug buttons
            if (GUI.Button(new Rect(16, 150, 100, 20), "All Flags"))
            {
                //levelData.levelPrices[1]++;
                //scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[3]=true;
                //var pls = scrGameSaveManager.instance.gameData.worldsData; // Enthält alle welten/level
                var pls = gameSaveManager.gameData.worldsData;
                for (int i = 0; i < pls.Count; i++)
                {
                    // if (i == 0)
                    // {
                    //     pls[i].letterFlags.Add("letter12");
                    // }
                    Logger.LogFatal($"This is Level '{i}'!");
                    pls[i].coinFlags.ForEach(Logger.LogInfo); //Gibt von welt 0-6 coinFlags | 7-15 existieren nicht bzw. cutscenes
                }
            }

            _goToLevel = Convert.ToInt32(GUI.TextField(new Rect(150, 170, 80, 20), _goToLevel.ToString()));
            if (GUI.Button(new Rect(16, 190, 100, 20),"TP to Level"))
            {
                scrTrainManager.instance.UseTrain(_goToLevel,false);
            }
            
            if (GUI.Button(new Rect(16, 220, 100, 20),"Note"))
            {
                // _note.key = "No, this is Patrick!";
                // _note.duration = 2.5F;
                // _note.timed = true;
                // //_note.avatar = apLogo;
                // _note.avatar = scrSnail.instance.sprFoodFull;
                // NoteDisplayer.AddNotification(_note);
                // NoteDisplayer.textMesh.text = _note.key;
                // //NoteDisplayer.AnimatePopup();
                // Logger.LogInfo("Note: "+NoteDisplayer.textMesh.text);
                SendNote("Hallo?");
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(MainMenu))]
        public static void MainMenu_Postfix()
        {
            var startButton = MainMenu.Instance.ExitButton;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(levelData))]
        public static void levelData_Prefix()
        {
            levelData.levelPrices[2] = 5;
            levelData.levelPrices[3] = 3;
        }
    }
}
