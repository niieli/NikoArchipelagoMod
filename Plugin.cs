using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Archipelago.MultiClient.Net.Models;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Patches;
using UnityEngine;
using UnityEngine.SceneManagement;
using Color = UnityEngine.Color;

namespace NikoArchipelago
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInProcess("Here Comes Niko!.exe")]
    public class Plugin : BaseUnityPlugin
    {
        /*
         * Goal for 0.1.0 - Have checks working (Sending & Receiving)
         * 
         *  1.0.0 - Release
         *  --> Refactor & clean up
         *  --> PR submission
         * 
         */
        private const string PluginGuid = "nieli.NikoArchipelago";
        private const string PluginName = nameof(NikoArchipelago);
        private const string PluginVersion = "0.2.1";
        
        private const string ModDisplayInfo = $"{PluginName} v{PluginVersion}";
        private const string APDisplayInfo = $"Archipelago v{ArchipelagoClient.APVersion}";
        public static ManualLogSource BepinLogger;
        public static ArchipelagoClient ArchipelagoClient;
        private string saveName;
        public scrGameSaveManager gameSaveManager;
        private bool loggedError, loggedSuccess, newFile, saveReady;
        private Harmony harmony;

        private List<string> saveDataCoinFlag, saveDataCassetteFlag, saveDataFishFlag, saveDataMiscFlag, saveDataLetterFlag, saveDataGeneralFlag;
        private int coinFlg, cassetteFlg, fishFlg, miscFlg, letterFlg, generalFlg, coinTotal, coinOld;
        private int goToLevel;
        private float env, mas, mus, sfx;
        private static scrNotificationDisplayer _noteDisplayer;
        public bool worldReady;
        private static bool _debugMode,_canLogin;
        private static readonly string ArchipelagoFolderPath = Path.Combine(Application.persistentDataPath, "Archipelago");
        private static readonly string AssetsFolderPath = Path.Combine(Paths.PluginPath, "APAssets");
        public static string Seed;
        private static scrGameSaveManager _gameSaveManagerStatic;
        public static AssetBundle AssetBundle;
        public static Sprite APSprite;
        public static Dictionary<string, object> SlotData;
        private CancellationTokenSource _cancellationTokenSource = new();
        
        private void Awake()
        {
            Chainloader.ManagerObject.hideFlags = HideFlags.HideAndDontSave;
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
            if (!Directory.Exists(ArchipelagoFolderPath))
            {
                Directory.CreateDirectory(ArchipelagoFolderPath);
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
            AssetBundle = AssetBundleLoader.LoadEmbeddedAssetBundle("apassets");
            if (AssetBundle != null)
            {
                APSprite = AssetBundle.LoadAsset<Sprite>("apLogo");
                _canLogin = true;
            }
            harmony = new Harmony(PluginGuid);
            harmony.PatchAll();
            SceneManager.sceneLoaded += OnSceneLoaded;
            Logger.LogInfo("Plugin loaded and Harmony patches applied initially!");
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Logger.LogInfo($"Scene '{scene.name}' loaded. Applying Harmony patches again.");
            harmony.PatchAll();
        }
        
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            harmony.UnpatchSelf();
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
            _gameSaveManagerStatic = scrGameSaveManager.instance;
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
                _noteDisplayer = scrNotificationDisplayer.instance;
                //Savefile is the same as SlotName & ServerPort, ':' is not allowed to be in a filename
                saveName = "APSave" + "_" + ArchipelagoClient.ServerData.SlotName + "_" + ArchipelagoClient.ServerData.Uri.Replace(":", "."); 
                if (scrGameSaveManager.saveName != saveName && ArchipelagoClient.Authenticated)
                {
                    scrGameSaveManager.saveName = saveName;
                    var savePath = Path.Combine(ArchipelagoFolderPath, saveName + "_" + Seed + ".json");
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
                    APSendNote($"Connected to {ArchipelagoClient.ServerData.Uri} successfully", 10F);
                }
                KioskCost.PreFix();
                //MyCharacterController.instance._diveConsumed = true;
                Flags();
                if (worldReady && ArchipelagoClient.Authenticated)
                {
                    //_ = ArchipelagoClient.SyncItemsFromDataStorage();
                    LocationHandler.Update2();
                    LocationHandler.WinCompletion();
                    StartCoroutine(SyncState());
                }
                _debugMode = File.Exists(Path.Combine(Paths.PluginPath, "debug.txt"));
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
            _cancellationTokenSource.Cancel();
            ArchipelagoClient.Disconnect();
            StopAllCoroutines();
            Application.Quit();
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
            var currentScene = SceneManager.GetActiveScene().name;
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
            if (ArchipelagoClient.queuedItems.Count <= 0 || currentScene == "OutsideTrainBetween") yield break;
            foreach (var t in ArchipelagoClient.queuedItems)
            {
                ArchipelagoClient.GiveItem(t);
            }
            ArchipelagoClient.queuedItems.Clear();
        }

        private void LogFlags()
        {
            saveDataCoinFlag.ForEach(Logger.LogInfo);
            saveDataCassetteFlag.ForEach(Logger.LogInfo);
            saveDataFishFlag.ForEach(Logger.LogInfo);
            saveDataMiscFlag.ForEach(Logger.LogInfo);
            saveDataLetterFlag.ForEach(Logger.LogInfo);
            saveDataGeneralFlag.ForEach(Logger.LogInfo);
        }
        
        public static void APSendNote(string note, float time)
        {
            var apNote = ScriptableObject.CreateInstance<Notification>();
            apNote.timed = true;
            apNote.avatar = APSprite;
            apNote.duration = time;
            apNote.key = note;
            _noteDisplayer.AddNotification(apNote);
            BepinLogger.LogMessage("Note: " + _noteDisplayer.textMesh.text);
        }
        public void KillPlayer(string cause)
        {
            ArchipelagoConsole.LogMessage(cause);
            scrTrainManager.instance.UseTrain(gameSaveManager.gameData.generalGameData.currentLevel, false);
            StartCoroutine(SendNoteDelay(cause, 5.0f, 5.0f, true));
        }
        
        private IEnumerator SendNoteDelay(string text, float delay, float noteTime, bool isDeath = false)
        {
            yield return new WaitForSeconds(delay);
            APSendNote(text, noteTime);
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
            coinTotal = scrGameSaveManager.instance.gameData.generalGameData.coinAmountTotal;

            CheckNewFlag(ref coinFlg, saveDataCoinFlag, "Coin");
            CheckNewFlag(ref cassetteFlg, saveDataCassetteFlag, nameof(Cassette));
            CheckNewFlag(ref fishFlg, saveDataFishFlag, nameof(Fish));
            CheckNewFlag(ref miscFlg, saveDataMiscFlag, "Misc");
            CheckNewFlag(ref letterFlg, saveDataLetterFlag, nameof(Letter));
            CheckNewFlag(ref generalFlg, saveDataGeneralFlag, "General");

            if (coinTotal <= coinOld) return;
            Logger.LogMessage("Total Coin Count = " + coinTotal);
            coinOld++;
        }

        private void CheckNewFlag(ref int flagIndex, List<string> flagList, string flagType)
        {
            if (flagList.Count > flagIndex)
            {
                Logger.LogInfo($"New {flagType} Flag! '{flagList[flagIndex]}'");
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
                BackgroundForText(new Rect(10, 10, 280, 140));
                statusMessage = " Status: Connected";
                GUI.Label(new Rect(16, 16, 300, 22), ModDisplayInfo);
                GUI.Label(new Rect(16, 50, 300, 22), APDisplayInfo + statusMessage);
                if (GUI.Button(new Rect(160, 16, 100, 20), "Disconnect") && ArchipelagoClient.Authenticated)
                {
                    ArchipelagoClient.Disconnect();
                }
                Tracker();
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
                if (loggedSuccess && _canLogin)
                {
                    if (GUI.Button(new Rect(16, 130, 100, 20), "Connect") && !ArchipelagoClient.ServerData.SlotName.IsNullOrWhiteSpace())
                    {
                        ArchipelagoClient.Connect();
                    }
                }
                else
                {
                    GUI.Label(new Rect(16, 126, 200, 24), "Initializing Data...");
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
                    pls[i].cassetteFlags.ForEach(Logger.LogInfo);
                    pls[i].miscFlags.ForEach(Logger.LogInfo);
                }
            }
            goToLevel = Convert.ToInt32(GUI.TextField(new Rect(150, 170, 80, 20), goToLevel.ToString()));
            if (GUI.Button(new Rect(16, 190, 100, 20), "TP to Level"))
            {
                scrTrainManager.instance.UseTrain(goToLevel, false);
            }
            if (GUI.Button(new Rect(16, 220, 100, 20), "Test Note"))
            {
                APSendNote("VERY LOOOONG text. Here goes nothing hihihihihihi", 2F);
            }
            if (GUI.Button(new Rect(16, 240, 100, 20), "Kill"))
            {
                KillPlayer("GETTT DEATHLINKED HEEHEH");
            }
            if (GUI.Button(new Rect(16, 260, 100, 20), "AchievementPopup"))
            {
                AchievementPopup.instance.PopupAchievement(scrAchievementManager.instance.obj_lostAtSea);
            }
            if (GUI.Button(new Rect(16, 280, 100, 20), "Cost-1"))
            {
                levelData.levelPrices[scrTrainManager.instance.currentLevel + 1]--;
            }
            if (GUI.Button(new Rect(16, 300, 100, 20), "SlotData"))
            {
                Logger.LogWarning("Deathlink: "+ArchipelagoData.slotData["death_link"]);
                Logger.LogWarning("MinKiosk: "+ArchipelagoData.slotData["min_kiosk_cost"]);
                Logger.LogWarning("MaxKiosk: "+ArchipelagoData.slotData["max_kiosk_cost"]);
                Logger.LogWarning("MinElevator: "+ArchipelagoData.slotData["min_elevator_cost"]);
                Logger.LogWarning("MaxElevator: "+ArchipelagoData.slotData["max_elevator_cost"]);
                Logger.LogWarning("Goal: "+ArchipelagoData.slotData["goal_completion"]);
                Logger.LogWarning("Kiosk Home: "+ArchipelagoData.slotData["kioskhome"]);
                Logger.LogFatal("Kiosk Home Int: "+ArchipelagoData.slotData["kioskhome"].ToString());
                Logger.LogWarning("Kiosk Hairball City: "+ArchipelagoData.slotData["kioskhc"]);
                Logger.LogWarning("Kiosk Turbine Town: "+ArchipelagoData.slotData["kiosktt"]);
                Logger.LogWarning("Kiosk Salmon Creek Forest: "+ArchipelagoData.slotData["kiosksfc"]);
                Logger.LogWarning("Kiosk Public Pool: "+ArchipelagoData.slotData["kioskpp"]);
                Logger.LogWarning("Kiosk Bathhouse: "+ArchipelagoData.slotData["kioskbath"]);
                Logger.LogWarning("Elevator Repair: "+ArchipelagoData.slotData["kioskhq"]);
            }
            if (GUI.Button(new Rect(16, 320, 100, 20), "Current Level"))
            {
                Logger.LogInfo(scrGameSaveManager.instance.gameData.generalGameData.currentLevel-1);
            }
            if (GUI.Button(new Rect(16, 340, 100, 20), "FixKiosk"))
            {
                var currentScene = SceneManager.GetActiveScene().name;
                scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Add($"Kiosk{currentScene}");
            }
        }

        private void BackgroundForText(Rect rect)
        {
            var startingColor = GUI.color;
            GUI.color = new Color(0f, 0f, 0f, 0.5F);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = startingColor;
        }

        private void Tracker()
        {
            // Define the flag-label mappings and positions
            var slotData = ArchipelagoData.slotData;
            var flagData = new (string flagKey, string successMsg, string failureMsg, int xPos, int yPos)[]
            {
                ("APWave1", "Got Contact List 1!", "No Contact List 1!", 16, 70),
                ("APWave2", "Got Contact List 2!", "No Contact List 2!", 170, 70),
                ("KioskHome", "Kiosk Home", $"Kiosk Home({slotData["kioskhome"]})", 16, 90),
                ("KioskHairball City", "Kiosk HC", $"Kiosk HC({slotData["kioskhc"]})", 115, 90),
                ("KioskTrash Kingdom", "Kiosk TT", $"Kiosk TT({slotData["kiosktt"]})", 200, 90),
                ("KioskSalmon Creek Forest", "Kiosk SCF", $"Kiosk SCF({slotData["kiosksfc"]})", 16, 110),
                ("KioskPublic Pool", "Kiosk PP", $"Kiosk PP({slotData["kioskpp"]})", 115, 110),
                ("KioskThe Bathhouse", "Kiosk Bath", $"Kiosk Bath({slotData["kioskbath"]})", 200, 110),
            };
            if (int.Parse(slotData["goal_completion"].ToString()) == 0)
            {
                GUI.Label(new Rect(28, 130, 300, 20), $"Goal: Get Hired | Elevator Repair Cost({slotData["kioskhq"]})");
            }
            else
            {
                GUI.Label(new Rect(24, 130, 300, 20), "Goal: Employee Of The Month! (76 Coins)");
            }

            foreach (var (flagKey, successMsg, failureMsg, xPos, yPos) in flagData)
            {
                DrawFlagStatus(flagKey, successMsg, failureMsg, xPos, yPos);
            }
        }

        // Method to display flag status based on game data
        private void DrawFlagStatus(string flagKey, string successMsg, string failureMsg, int xPos, int yPos)
        {
            var startingColor = GUI.color;
            var generalFlags = scrGameSaveManager.instance.gameData.generalGameData.generalFlags;

            if (generalFlags.Contains(flagKey))
            {
                GUI.color = Color.green;
                GUI.Label(new Rect(xPos, yPos, 300, 20), successMsg);
            }
            else
            {
                GUI.color = Color.red;
                GUI.Label(new Rect(xPos, yPos, 300, 20), failureMsg);
            }

            GUI.color = startingColor;
        }
    }
}
