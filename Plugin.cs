using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Patches;
using UnityEngine;

namespace NikoArchipelago
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
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
         * TODO: Für release: Optionen von SlotData bekommen,
         * TODO: Cassette kosten ändern,
         * TODO: Notification Queue fixen,
         * TODO: Kiosk level check zu 8+ ändern (sonst temporär den Preis bei shuffled Tickets auf 99 setzen),
         * TODO: Fehlende Locations implementieren (Gary's Garden),
         */
        private const string PluginGuid = "nieli.NikoArchipelago";
        private const string PluginName = nameof(NikoArchipelago);
        private const string PluginVersion = "0.0.1";
        
        private const string ModDisplayInfo = $"{PluginName} v{PluginVersion}";
        private const string APDisplayInfo = $"Archipelago v{ArchipelagoClient.APVersion}";
        public static ManualLogSource BepinLogger;
        public static ArchipelagoClient ArchipelagoClient;
        private string saveName;
        public scrGameSaveManager gameSaveManager;
        private bool loggedError, loggedSuccess, newFile, saveReady, waited;

        public static CustomButton AptestButton;
        private List<string> saveDataCoinFlag, saveDataCassetteFlag, saveDataFishFlag, saveDataMiscFlag, saveDataLetterFlag, saveDataGeneralFlag;
        private List<bool> unlockedLevels;
        private int coinFlg, cassetteFlg, fishFlg, miscFlg, letterFlg, generalFlg, coinTotal, coinOld, levelIndex;
        private int goToLevel;
        private float env, mas, mus, sfx;
        public static scrNotificationDisplayer NoteDisplayer;
        Notification noteItem = ScriptableObject.CreateInstance<Notification>();
        private scrHopOnBump hopOnBump;
        private static scrKioskManager _kioskManager;
        public bool worldReady;
        public static int cLevel;
        public static List<string> cFlags;
        private bool _debugMode;
        private static readonly string archipelagoFolderPath = Path.Combine(Application.persistentDataPath, "Archipelago");
        public static string seed;
        
        private void Awake()
        {
            BepinLogger = Logger;
            ArchipelagoClient = new ArchipelagoClient();
            ArchipelagoConsole.Awake();
            Logger.LogInfo($"Hey, Niko here! Plugin {PluginName} Loaded! :)");
            ArchipelagoConsole.LogMessage($"{ModDisplayInfo} loaded!");
            saveName = ArchipelagoClient.ServerData.SlotName;
            mas = 0.5f;
            env = 0.4f;
            sfx = 0.4f;
            mus = 0.4f;
            if (!Directory.Exists(archipelagoFolderPath))
            {
                Directory.CreateDirectory(archipelagoFolderPath);
                Logger.LogInfo("Archipelago folder created.");
            }
        }

        public void Load()
        {
        }

        public void Start()
        {
            GameOptions.MasterVolume = mas;
            GameOptions.EnvVolume = env;
            GameOptions.MusicVolume = mus;
            GameOptions.SFXVolume = sfx;
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
            saveReady = true;
        }

        private IEnumerator CheckWorldSaveManager()
        {
            while (!scrWorldSaveDataContainer.instance)
            {
                Logger.LogError("WorldSaveDataContainer is null.");
                yield return new WaitForSeconds(3.0f);
            }
            Logger.LogInfo("WorldSaveDataContainer is not null.");
            worldReady = true;
        }
        
        public void Update()
        {
            if (!saveReady) return;
            try
            {
                NoteDisplayer = scrNotificationDisplayer.instance;
                //Savefile is the same as SlotName & ServerPort, ':' is not allowed to be in a filename
                saveName = "APSave" + "_" + ArchipelagoClient.ServerData.SlotName + "_" + ArchipelagoClient.ServerData.Uri.Replace(":", "."); 
                if (scrGameSaveManager.saveName != saveName && ArchipelagoClient.Authenticated)
                {
                    scrGameSaveManager.saveName = saveName;
                    var savePath = Path.Combine(archipelagoFolderPath, saveName + "_" + seed + ".json");
                    if (File.Exists(savePath))
                    {
                        scrGameSaveManager.dataPath = savePath;
                        Logger.LogInfo("Found a SaveFile with the current SlotName & Port!");
                        ArchipelagoConsole.LogMessage("Found a SaveFile with the current SlotName & Port!");
                        gameSaveManager.LoadGame();
                    }
                    else
                    {
                        newFile = true;
                        scrGameSaveManager.dataPath = savePath;
                        Logger.LogWarning("No SaveFile found. Creating a new one!");
                        ArchipelagoConsole.LogMessage("No SaveFile found. Creating a new one!");
                        gameSaveManager.SaveGame();
                        gameSaveManager.LoadGame();
                        gameSaveManager.ClearSaveData();
                    }
                    //Prevents the game from breaking the savefile when currentlevel = 0; Only if it's a new file load the currentlevel
                    scrTrainManager.instance.UseTrain(!newFile ? gameSaveManager.gameData.generalGameData.currentLevel : 1, false);
                    if (newFile)
                    {
                        ArchipelagoClient.Disconnect();
                        StartCoroutine(FirstLoginFix());
                    }
                    LogFlags();
                    StartCoroutine(CheckWorldSaveManager());
                    SendNote($"Connected to {ArchipelagoClient.ServerData.Uri} successfully", 10F);
                }
                KioskCost.levelData_Prefix();
                //MyCharacterController.instance._diveConsumed = true;
                Flags();
                if (worldReady && ArchipelagoClient.Authenticated)
                {
                    //_ = ArchipelagoClient.SyncItemsFromDataStorage();
                    StartCoroutine(DelayInformation());
                    //CassetteCost.Update();
                    LocationHandler.Update2();
                    LocationHandler.WinCompletion();
                    StartCoroutine(SyncState());
                }

                if (File.Exists(Path.Combine(Paths.PluginPath, "debug.txt")))
                {
                    _debugMode = true;
                }
                if (loggedSuccess) return;
                Logger.LogMessage("Game finished initialising");
                loggedSuccess = true;
            }
            catch (Exception e)
            {
                if (!loggedError)
                {
                    Logger.LogMessage("Waiting for game to finish initialising. Error:"+e);
                    loggedError = true;
                }
            }
        }

        private void OnApplicationQuit()
        {
            ArchipelagoClient.Disconnect();
            Application.Quit();
        }
        
        private IEnumerator DelayInformation()
        {
            yield return new WaitForSeconds(1.0f);
            cLevel = gameSaveManager.gameData.generalGameData.currentLevel - 1;
            cFlags = scrWorldSaveDataContainer.instance.coinFlags;
        }
        
        private IEnumerator FirstLoginFix()
        {
            yield return new WaitForSeconds(3.0f);
            ArchipelagoClient.Connect();
        }
        
        private static IEnumerator SyncState()
        {
            yield return new WaitForSeconds(4f);
            var generalGameData = scrGameSaveManager.instance.gameData.generalGameData;
            void SyncValue<T>(ref T gameDataValue, T clientValue)
            {
                if (!EqualityComparer<T>.Default.Equals(gameDataValue, clientValue))
                {
                    gameDataValue = clientValue;
                }
            }
            // Sync Coins, Cassettes, Keys
            SyncValue(ref generalGameData.coinAmount, ArchipelagoClient.CoinAmount);
            SyncValue(ref generalGameData.coinAmountTotal, ArchipelagoClient.CoinAmount);
            SyncValue(ref generalGameData.cassetteAmount, ArchipelagoClient.CassetteAmount);
            SyncValue(ref generalGameData.keyAmount, ArchipelagoClient.KeyAmount);
            // Sync Special Unlocks (Super Jump, Contact Lists)
            SyncValue(ref generalGameData.secretMove, ArchipelagoClient.SuperJump);
            SyncValue(ref generalGameData.wave1, ArchipelagoClient.ContactList1);
            SyncValue(ref generalGameData.wave2, ArchipelagoClient.ContactList2);
            // Sync Level Unlocks (Tickets) - No ref here
            void SyncLevel(int levelIndex, bool clientValue)
            {
                if (generalGameData.unlockedLevels[levelIndex] != clientValue)
                {
                    generalGameData.unlockedLevels[levelIndex] = clientValue;
                }
            }
            // Sync levels
            SyncLevel(2, ArchipelagoClient.Ticket1);
            SyncLevel(3, ArchipelagoClient.Ticket2);
            SyncLevel(4, ArchipelagoClient.Ticket3);
            SyncLevel(5, ArchipelagoClient.Ticket4);
            SyncLevel(6, ArchipelagoClient.Ticket5);
            SyncLevel(7, ArchipelagoClient.Ticket6);
        }

        private void LogFlags()
        {
            saveDataCoinFlag.ForEach(Logger.LogMessage);
            saveDataCassetteFlag.ForEach(Logger.LogMessage);
            saveDataFishFlag.ForEach(Logger.LogMessage);
            saveDataMiscFlag.ForEach(Logger.LogMessage);
            saveDataLetterFlag.ForEach(Logger.LogMessage);
            saveDataGeneralFlag.ForEach(Logger.LogMessage);
        }

        public void SendNote(string note, float time)
        {
            var errorNote = noteItem;
            errorNote.key = "!!What's my line?!!";
            var tmp = noteItem;
            //TODO: Find a more suitable fix for errorNote.key or fix the Note.key being set to null
            if (NoteDisplayer.notificationQueue.Count > 0)
            {
                NoteDisplayer.notificationQueue.Clear();
            }
            tmp.timed = true;
            tmp.avatar = scrSnail.instance.sprFoodFull;
            tmp.duration = time;
            tmp.key = note;
            NoteDisplayer.AddNotification(tmp);
            NoteDisplayer.textMesh.text = tmp.key;
            Logger.LogMessage("Note: " + NoteDisplayer.textMesh.text);
        }
        
        public static void APSendNote(string note, float time)
        {
            var apNote = ScriptableObject.CreateInstance<Notification>();
            //TODO: create a new apNote instead of using the same one and add it to the queue
            if (NoteDisplayer.notificationQueue.Count > 0)
            {
                NoteDisplayer.notificationQueue.Clear();
            }
            apNote.timed = true;
            apNote.avatar = scrSnail.instance.sprFoodFull;
            apNote.duration = time;
            apNote.key = note;
            NoteDisplayer.AddNotification(apNote);
            NoteDisplayer.textMesh.text = note;
            BepinLogger.LogMessage("Note: " + NoteDisplayer.textMesh.text);
            //var i = noteDisplayer.notificationQueue.Count;
            // for (int j = 0; j < i; j++)
            // {
            //     noteDisplayer.notificationQueue[j].duration = 0.1f;
            // }
        }
        public void KillPlayer(string cause)
        {
            ArchipelagoConsole.LogMessage(cause);
            scrTrainManager.instance.UseTrain(gameSaveManager.gameData.generalGameData.currentLevel, false);
            StartCoroutine(SendNoteDelay(cause, 8.2f, 5.0f, true));
        }
        
        private IEnumerator SendNoteDelay(string text, float delay, float noteTime, bool isDeath = false)
        {
            waited = false;
            yield return new WaitForSeconds(delay);
            waited = true;
            SendNote(text, noteTime);
            if (isDeath)
            {
                //Add grace period
            }
        }
        
        public void Flags()
        {
            saveDataCoinFlag = scrWorldSaveDataContainer.instance.coinFlags;
            saveDataCassetteFlag = scrWorldSaveDataContainer.instance.cassetteFlags;
            saveDataFishFlag = scrWorldSaveDataContainer.instance.fishFlags;
            saveDataLetterFlag = scrWorldSaveDataContainer.instance.letterFlags;
            saveDataMiscFlag = scrWorldSaveDataContainer.instance.miscFlags;
            saveDataGeneralFlag = scrGameSaveManager.instance.gameData.generalGameData.generalFlags;
            unlockedLevels = scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels;
            coinTotal = scrGameSaveManager.instance.gameData.generalGameData.coinAmountTotal;

            CheckNewFlag(ref coinFlg, saveDataCoinFlag, "Coin");
            CheckNewFlag(ref cassetteFlg, saveDataCassetteFlag, nameof(Cassette));
            CheckNewFlag(ref fishFlg, saveDataFishFlag, nameof(Fish));
            CheckNewFlag(ref miscFlg, saveDataMiscFlag, "Misc");
            CheckNewFlag(ref letterFlg, saveDataLetterFlag, nameof(Letter));
            CheckNewFlag(ref generalFlg, saveDataGeneralFlag, "General");

            if (unlockedLevels.Count > levelIndex)
            {
                Logger.LogWarning("New Level unlocked! " + unlockedLevels[levelIndex]);
                levelIndex++;
            }

            if (coinTotal <= coinOld) return;
            Logger.LogMessage("Total Coin Count = " + coinTotal);
            coinOld++;
        }

        private void CheckNewFlag(ref int flagIndex, List<string> flagList, string flagType)
        {
            if (flagList.Count > flagIndex)
            {
                Logger.LogMessage($"New {flagType} Flag! '{flagList[flagIndex]}'");
                flagIndex++;
            }
            else if (flagIndex > flagList.Count)
            {
                flagIndex = 0;
            }
        }
        
        private void OnGUI()
        {
            ArchipelagoConsole.OnGUI();
            string statusMessage;
            if (ArchipelagoClient.Authenticated)
            {
                BackgroundForText(new Rect(10, 10, 280, 60));
                statusMessage = " Status: Connected";
                GUI.Label(new Rect(16, 16, 300, 20), ModDisplayInfo);
                GUI.Label(new Rect(16, 50, 300, 20), APDisplayInfo + statusMessage);
                if (GUI.Button(new Rect(160, 16, 100, 20), "Disconnect") && ArchipelagoClient.Authenticated)
                {
                    ArchipelagoClient.Disconnect();
                }
            }
            else
            {
                BackgroundForText(new Rect(10, 14, 320, 136));
                statusMessage = " Status: Disconnected";
                GUI.Label(new Rect(16, 16, 300, 20), ModDisplayInfo);
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

            if (!_debugMode) return;
            if (GUI.Button(new Rect(16, 150, 100, 20), "All Flags"))
            {
                var pls = gameSaveManager.gameData.worldsData;
                for (int i = 0; i < pls.Count; i++)
                {
                    Logger.LogFatal($"This is Level '{i}'!");
                    pls[i].coinFlags.ForEach(Logger.LogInfo);
                }
            }
            goToLevel = Convert.ToInt32(GUI.TextField(new Rect(150, 170, 80, 20), goToLevel.ToString()));
            if (GUI.Button(new Rect(16, 190, 100, 20), "TP to Level"))
            {
                scrTrainManager.instance.UseTrain(goToLevel, false);
            }
            if (GUI.Button(new Rect(16, 220, 100, 20), "Note"))
            {
                SendNote("VERY LOOOONG text. Here goes nothing hihihihihihi", 2F);
            }
            if (GUI.Button(new Rect(16, 250, 100, 20), "Kill"))
            {
                KillPlayer("GETTT DEATHLINKED HEEHEH");
            }

            if (GUI.Button(new Rect(16, 280, 100, 20), "Cost-1"))
            {
                levelData.levelPrices[scrTrainManager.instance.currentLevel + 1]--;
            }
        }

        private void BackgroundForText(Rect rect)
        {
            var startingColor = GUI.color;
            GUI.color = new Color(0f, 0f, 0f, 0.5F);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = startingColor;
        }
    }
    
}
