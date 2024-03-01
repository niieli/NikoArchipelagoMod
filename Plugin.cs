using System;
using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using HarmonyLib.Tools;
using KinematicCharacterController.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace NikoArchipelago
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInProcess("Here Comes Niko!.exe")]
    public class Plugin : BaseUnityPlugin
    {
        /*
         * Goal for 0.1.0 - Have checks working (Sending & Receiving)
         * 
         * # UI - Not sure how to make it look nice, preferably use the in-game ui
         *  to create a server button to join an archipelago session + create a new save file for it (likely upon pressing start)
         *
         * #Find a way to use the in-game Notification system to show item received and send
         *  with the AP logo preferably and no noise/voice
         *
         * #Flags are documented on the spreadsheet + Handsome Frog checks are addable 
         */
        
        private const string PLUGIN_GUID = "NikoArchipelago";
        private const string PLUGIN_NAME = "NikoArchipelago";
        private const string PLUGIN_VERSION = "0.0.0";
        
        public Notification test;
        public static CustomButton AptestButton;
        private List<string> _saveDataCoinFlag, _saveDataCassetteFlag, _saveDataFishFlag, _saveDataMiscFlag, _saveDataLetterFlag, _saveDataGeneralFlag;
        private List<bool> _unlockedLevels;
        private int _coinFlg, _cassetteFlg, _fishFlg, _miscFlg, _letterFlg, _generalFlg, _coinTotal, _coinOld, _levelIndex;
        private Notification _newFlag = new ();
        private Notification _note = ScriptableObject.CreateInstance<Notification>();
        
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Hey, Niko here! btw Plugin {PLUGIN_NAME} Loaded! :)");
        }


        public void Load()
        {
            LoadPatches();
        }

        public void Start()
        {
            //
        }
        
        public void Update()
        {
            //scrGameSaveManager.instance.SaveGame();
            if (!string.Equals(scrGameSaveManager.saveName, "NieDebug2024Save", StringComparison.Ordinal))
            {
                // scrGameSaveManager.saveName = "APLogicSave";
                scrGameSaveManager.saveName = "NieDebug2024Save";
            }
            Flags();
        }
        
        [HarmonyPatch(typeof(MainMenu))]
        [HarmonyPatch("Start")]
        internal class MainMenu
        {
            [HarmonyPostfix]
            public static void CreateUI()
            {
                var guiGameobject = new GameObject();
                GameObject.DontDestroyOnLoad(guiGameobject);
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

            if (_unlockedLevels.Count > _levelIndex)
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

        public void StuffX()
        {
            scrTrainManager.instance.UseTrain(1,false);
        }

        public TextMesh TextMesh;
        public void NewItem()
        {
            _newFlag.key = "pls work man i have kids and wife";
            _newFlag.name = "Help";
            _newFlag.duration = 5f;
            //scrNotificationDisplayer.instance.AddNotification(_newFlag);
            _note.key = "This is a new note by nieli";
            _note.name = "Testing AP";
            _note.avatar = Sprite.Create(Texture2D.blackTexture, new Rect(), Vector2.down);
            test = _note;
            Logger.LogError(test);
            Logger.LogError(test.name);
            Logger.LogError(test.key);
            scrNotificationDisplayer.instance.AddNotification(test);
        }

        public void SoundFix()
        {
            GameOptions.AudioOptions.ForEach(Logger.LogMessage);
        }
        
        // Apply Harmony patches (?)
        private static void LoadPatches()
        {
            HarmonyFileLog.Enabled = true;
            var harmony = new Harmony("apniko");
            harmony.PatchAll();
        }
        
        public Rect windowRect = new Rect(600, 500, 400, 200);

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
            if (GUI.Button(new Rect(120, 170, 100, 20), "Notification"))
            {
                NewItem();
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
        
        public void OnGUI()
        {
            windowRect = GUI.Window(0, windowRect, WindowFunc, "DEBUG");
            // GUI.Label(new Rect(10, 10, 200, 20), "Hello, IMGUI!");
            // if (GUI.Button(new Rect(10, 40, 100, 20), "+1 Coin"))
            // {
            //     scrGameSaveManager.instance.gameData.generalGameData.coinAmount++;
            // }
            
            // GUI.Box(new Rect(10, 10, 100, 110), "Plugin Menu");
            // if (GUI.Button(new Rect(20, 40, 80, 20), "Add 1 Coin"))
            // {
            //     scrGameSaveManager.instance.gameData.generalGameData.coinAmount++;
            // }
        }

    }

    public class ImGUI : MonoBehaviour
    {
        public string text = string.Empty;
    }

    [HarmonyPatch]
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Instance))]
    class ApMenu
    { 
        public CustomButton apTestButton;
        [HarmonyPostfix]
        public void Menu(MainMenu instance)
        {
            instance.OptionsButton.DefaultColor.g=4F;
            var apButton = apTestButton;
            if (apButton != null)
            {
                apButton.OnClickFinished.AddListener(new UnityAction(MainMenu.Instance.OnExitButtonPressed));
            }
        }
        
    }
}
