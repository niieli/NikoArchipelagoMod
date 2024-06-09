using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Archipelago.MultiClient.Net.Enums;
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
         * --> Have the checks working
         * --> Add the checks to the UI
         * --> Create APWorld file
         *
         *  0.2.0 UI - Not sure how to make it look nice, preferably use the in-game ui
         *  to create a server button to join an archipelago session + create a new save file for it (likely upon pressing start)
         *  
         *  0.3.0 - Add DeathLink
         * 
         *  1.0.0 - Release
         *  --> Refactor & clean up
         *  --> PR submission
         *
         *  COMPLETE, without APLogo - #Find a way to use the in-game Notification system to show item received and send
         *                              with the AP logo preferably and no noise/voice
         *
         * #Flags are documented on the spreadsheet + Handsome Frog checks are addable
         * 
         * worldsData:
         * -> WorldsData.coinFlags geht von 0-6 coinFlags
         * -> WorldsData.cassetteFlags geht von 0-6 cassetteFlags, etc. Enthält alle welten/level| 7-15 existieren nicht bzw. cutscenes
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
        private bool _loggedError, _loggedSuccess, _newFile, _saveReady, _waited;
        private static bool _dc;
        
        public static CustomButton AptestButton;
        private List<string> _saveDataCoinFlag, _saveDataCassetteFlag, _saveDataFishFlag, _saveDataMiscFlag, _saveDataLetterFlag, _saveDataGeneralFlag;
        private List<bool> _unlockedLevels;
        private List<string> _uniqueFlags = new List<string>();
        private int _coinFlg, _cassetteFlg, _fishFlg, _miscFlg, _letterFlg, _generalFlg, _coinTotal, _coinOld, _levelIndex;
        private int _goToLevel;
        private float _env, _mas, _mus, _sfx;
        public scrNotificationDisplayer noteDisplayer;
        Notification noteItem = ScriptableObject.CreateInstance<Notification>();
        private scrHopOnBump hopOnBump;
        
        private void Awake()
        {
            BepinLogger = Logger;
            ArchipelagoClient = new ArchipelagoClient();
            ArchipelagoConsole.Awake();
            Logger.LogInfo($"Hey, Niko here! Plugin {PLUGIN_NAME} Loaded! :)");
            ArchipelagoConsole.LogMessage($"{ModDisplayInfo} loaded!");
            _saveName = ArchipelagoClient.ServerData.SlotName;
        }

        public void Load()
        {
            levelData_Prefix();
        }

        public void Start()
        {
            levelData_Prefix();
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
        }
        
        public void Update()
        {
            if (!_saveReady) return;
            try
            {
                levelData_Prefix();
                noteDisplayer = scrNotificationDisplayer.instance;
                //Savefile is the same as SlotName & ServerPort
                _saveName = "APSave" + ArchipelagoClient.ServerData.SlotName + ArchipelagoClient.ServerData.Uri.Replace(":", "."); 
                if (scrGameSaveManager.saveName != _saveName && ArchipelagoClient.Authenticated)
                {
                    scrGameSaveManager.saveName = _saveName;
                    var savePath = Path.Combine(Application.persistentDataPath, _saveName + ".json");
                    if (File.Exists(savePath))
                    {
                        scrGameSaveManager.dataPath = savePath;
                        Logger.LogInfo("Found a SaveFile with the current SlotName & Port!");
                        ArchipelagoConsole.LogMessage("Found a SaveFile with the current SlotName & Port!");
                        gameSaveManager.LoadGame();
                    }
                    else
                    {
                        _newFile = true;
                        scrGameSaveManager.dataPath = savePath;
                        Logger.LogWarning("No SaveFile found. Creating a new one!");
                        ArchipelagoConsole.LogMessage("No SaveFile found. Creating a new one!");
                        gameSaveManager.SaveGame();
                        gameSaveManager.LoadGame();
                        gameSaveManager.ClearSaveData();
                    }
                    //Prevents the game from breaking the savefile when currentlevel = 0; Only if it's a new file
                    scrTrainManager.instance.UseTrain(!_newFile ? gameSaveManager.gameData.generalGameData.currentLevel : 1, false);
                    LogFlags();
                    SendNote($"Connected to {ArchipelagoClient.ServerData.Uri} successfully", 5F);
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

        private void LogFlags()
        {
            _saveDataCoinFlag.ForEach(Logger.LogMessage);
            _saveDataCassetteFlag.ForEach(Logger.LogMessage);
            _saveDataFishFlag.ForEach(Logger.LogMessage);
            _saveDataMiscFlag.ForEach(Logger.LogMessage);
            _saveDataLetterFlag.ForEach(Logger.LogMessage);
            _saveDataGeneralFlag.ForEach(Logger.LogMessage);
        }

        public void SendNote(string note, float time)
        {
            var errorNote = noteItem;
            errorNote.key = "!!What's my line?!!";
            var tmp = noteItem;
            //TODO: Find a more suitable fix for errorNote.key or fix the Note.key being set to null
            if (noteDisplayer.notificationQueue.Count > 0)
            {
                noteDisplayer.notificationQueue.Clear();
            }
            tmp.timed = true;
            tmp.avatar = scrSnail.instance.sprFoodFull;
            tmp.duration = time;
            tmp.key = note;
            noteDisplayer.AddNotification(tmp);
            noteDisplayer.textMesh.text = tmp.key;
            Logger.LogInfo("Note: " + noteDisplayer.textMesh.text);
        }

        public void KillPlayer()
        {
            _waited = false;
            scrTrainManager.instance.UseTrain(gameSaveManager.gameData.generalGameData.currentLevel, false);
            ArchipelagoConsole.LogMessage("get deathlinked LMAO");
            StartCoroutine(KillPlayerDelay());
        }
        
        private IEnumerator KillPlayerDelay()
        {
            yield return new WaitForSeconds(6.5f);
            _waited = true;
            SendNote("Deathlink LMAO", 5.0F);
        }
        
        public void Flags()
        {
            _saveDataCoinFlag = scrWorldSaveDataContainer.instance.coinFlags;
            _saveDataCassetteFlag = scrWorldSaveDataContainer.instance.cassetteFlags;
            _saveDataFishFlag = scrWorldSaveDataContainer.instance.fishFlags;
            _saveDataLetterFlag = scrWorldSaveDataContainer.instance.letterFlags;
            _saveDataMiscFlag = scrWorldSaveDataContainer.instance.miscFlags;
            _saveDataGeneralFlag = scrGameSaveManager.instance.gameData.generalGameData.generalFlags;
            _unlockedLevels = scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels;
            _coinTotal = scrGameSaveManager.instance.gameData.generalGameData.coinAmountTotal;

            CheckNewFlag(ref _coinFlg, _saveDataCoinFlag, "Coin");
            CheckNewFlag(ref _cassetteFlg, _saveDataCassetteFlag, "Cassette");
            CheckNewFlag(ref _fishFlg, _saveDataFishFlag, "Fish");
            CheckNewFlag(ref _miscFlg, _saveDataMiscFlag, "Misc");
            CheckNewFlag(ref _letterFlg, _saveDataLetterFlag, "Letter");
            CheckNewFlag(ref _generalFlg, _saveDataGeneralFlag, "General");

            if (_unlockedLevels.Count > _levelIndex)
            {
                Logger.LogWarning("New Level unlocked! " + _unlockedLevels[_levelIndex]);
                _levelIndex++;
            }

            if (_coinTotal <= _coinOld) return;
            Logger.LogWarning("Total Coin Count = " + _coinTotal);
            _coinOld++;
        }

        private void CheckNewFlag(ref int flagIndex, List<string> flagList, string flagType)
        {
            if (flagList.Count > flagIndex)
            {
                Logger.LogFatal($"New {flagType} Flag! '{flagList[flagIndex]}'");
                flagIndex++;
            }
            else if (flagIndex > flagList.Count)
            {
                flagIndex = 0;
            }
        }
        
        private void OnGUI()
        {
            GUI.Label(new Rect(16, 16, 300, 20), ModDisplayInfo);
            ArchipelagoConsole.OnGUI();
            
            string statusMessage;
            if (ArchipelagoClient.Authenticated)
            {
                BackgroundForText(new Rect(10, 10, 280, 60));
                statusMessage = " Status: Connected";
                GUI.Label(new Rect(16, 50, 300, 20), APDisplayInfo + statusMessage);
            }
            else
            {
                BackgroundForText(new Rect(10, 14, 320, 136));
                statusMessage = " Status: Disconnected";
                GUI.Label(new Rect(16, 50, 300, 20), APDisplayInfo + statusMessage);
                GUI.Label(new Rect(16, 70, 150, 20), "Host: ");
                GUI.Label(new Rect(16, 90, 150, 20), "Player Name: ");
                GUI.Label(new Rect(16, 110, 150, 20), "Password: ");

                ArchipelagoClient.ServerData.Uri = GUI.TextField(new Rect(150, 70, 150, 20), ArchipelagoClient.ServerData.Uri);
                ArchipelagoClient.ServerData.SlotName = GUI.TextField(new Rect(150, 90, 150, 20), ArchipelagoClient.ServerData.SlotName);
                ArchipelagoClient.ServerData.Password = GUI.TextField(new Rect(150, 110, 150, 20), ArchipelagoClient.ServerData.Password);

                if (GUI.Button(new Rect(16, 130, 100, 20), "Connect") && !ArchipelagoClient.ServerData.SlotName.IsNullOrWhiteSpace())
                {
                    ArchipelagoClient.Connect();
                }
            }

            if (GUI.Button(new Rect(16, 150, 100, 20), "All Flags"))
            {
                var pls = gameSaveManager.gameData.worldsData;
                for (int i = 0; i < pls.Count; i++)
                {
                    Logger.LogFatal($"This is Level '{i}'!");
                    pls[i].coinFlags.ForEach(Logger.LogInfo);
                }
            }

            _goToLevel = Convert.ToInt32(GUI.TextField(new Rect(150, 170, 80, 20), _goToLevel.ToString()));
            if (GUI.Button(new Rect(16, 190, 100, 20), "TP to Level"))
            {
                scrTrainManager.instance.UseTrain(_goToLevel, false);
            }
            
            if (GUI.Button(new Rect(16, 220, 100, 20), "Note"))
            {
                SendNote("VERY LOOOONG text. Here goes nothing hihihihihihi", 2F);
            }
            if (GUI.Button(new Rect(16, 250, 100, 20), "Kill"))
            {
                KillPlayer();
            }
            if (GUI.Button(new Rect(16, 280, 100, 20), "Cost-1"))
            {
                levelData.levelPrices[scrTrainManager.instance.currentLevel + 1]--;
            }
            if (GUI.Button(new Rect(140, 220, 100, 20), "Disconnect"))
            {
                ArchipelagoClient.Disconnect();
            }
        }

        private void BackgroundForText(Rect rect)
        {
            var startingColor = GUI.color;
            GUI.color = new Color(0f, 0f, 0f, 0.5F);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = startingColor;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(MainMenu))]
        public static void MainMenu_Postfix()
        {
            var quitButton = MainMenu.Instance.ExitButton;
            if (!_dc)
            {
                quitButton.onClick.RemoveListener(MainMenu.Instance.OnExitButtonPressed);
            }
            else
            {
                quitButton.onClick.AddListener(MainMenu.Instance.OnExitButtonPressed);
            }
            if (quitButton && !_dc)
            {
                quitButton.onClick.AddListener(OnQuitButtonPressed);
            }
        }
        
        private static void OnQuitButtonPressed()
        {
            if (_dc) return;
            ArchipelagoClient.Disconnect();
            _dc = true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(levelData))]
        public static void levelData_Prefix()
        {
            levelData.levelPrices[2] = 5;
            levelData.levelPrices[3] = 3;
        }
    }
}
