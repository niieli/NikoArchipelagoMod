using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using Archipelago.MultiClient.Net.Enums;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Patches;
using NikoArchipelago.Stuff;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Color = UnityEngine.Color;
using Object = UnityEngine.Object;

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
        public const string PluginVersion = "0.6.3";
        
        private const string ModDisplayInfo = $"{PluginName} v{PluginVersion}";
        private const string APDisplayInfo = $"Archipelago v{ArchipelagoClient.APVersion}";
        public static ManualLogSource BepinLogger;
        public static ArchipelagoClient ArchipelagoClient;
        public static string saveName;
        public scrGameSaveManager gameSaveManager;
        private bool loggedError, loggedSuccess;
        public static bool newFile;
        public static bool saveReady, DebugMode;
        private Harmony harmony;

        private List<string> saveDataCoinFlag, saveDataCassetteFlag, saveDataFishFlag, saveDataMiscFlag, saveDataLetterFlag, saveDataGeneralFlag;
        private int coinFlg, cassetteFlg, fishFlg, miscFlg, letterFlg, generalFlg, coinTotal, coinOld;
        private int goToLevel, snowAmount;
        private float env, mas, mus, sfx;
        private static scrNotificationDisplayer _noteDisplayer;
        public static bool worldReady;
        private static bool _canLogin;
        public static readonly string ArchipelagoFolderPath = Path.Combine(Application.persistentDataPath, "Archipelago");
        private static readonly string AssetsFolderPath = Path.Combine(Paths.PluginPath, "APAssets");
        public static bool loggedIn, Compatibility, SaveEstablished, PlayerFound;
        public static string Seed;
        public static AssetBundle AssetBundle, AssetBundleXmas;
        public static GameObject FreezeTrap, IronBootsTrap, MyTurnTrap, WhoopsTrap, GravityTrap;
        public static Sprite APSprite, BandanaSprite, BowtieSprite, CapSprite, 
            CatSprite, ClownSprite, FlowerSprite, 
            GlassesSprite, KingSprite, MahjongSprite, MotorSprite, MouseSprite, 
            SmallHatSprite, StarsSprite, SwordSprite, TopHatSprite, SunglassesSprite, 
            APLogoSprite, APIconSprite, CoinSprite, CassetteSprite, 
            FishSprite, HairballFishSprite, TurbineFishSprite, SalmonFishSprite, PoolFishSprite, BathFishSprite, TadpoleFishSprite,
            KeySprite, HairballKeySprite, TurbineKeySprite, SalmonKeySprite, PoolKeySprite, BathKeySprite, TadpoleKeySprite,
            ContactListSprite, BottledSprite, EmployeeSprite, FrogFanSprite, 
            HandsomeSprite, LostSprite, SuperJumpSprite,
            SnailFashionSprite, VolleyDreamsSprite, ApplesSprite, LetterSprite,
            HcSprite, TtSprite, SfcSprite, PpSprite, BathSprite, HqSprite,
            SnailMoneySprite, BugSprite, GgSprite, GoalBadSprite,
            ApProgressionSprite, ApUsefulSprite, ApFillerSprite, ApTrapSprite, ApTrap2Sprite, ApTrap3Sprite,
            TimePieceSprite, YarnSprite,
            HairballFlowerSprite, TurbineFlowerSprite, SalmonFlowerSprite, PoolFlowerSprite, BathFlowerSprite, TadpoleFlowerSprite,
            HairballSeedSprite, SalmonSeedSprite, BathSeedSprite,
            HairballCassetteSprite, TurbineCassetteSprite, SalmonCassetteSprite, PoolCassetteSprite, BathCassetteSprite, TadpoleCassetteSprite, GardenCassetteSprite,
            FischerNoteSprite, GabiNoteSprite, MoomyNoteSprite, BlessleyNoteSprite,
            FreezeTrapSprite, IronBootsTrapSprite, MyTurnTrapSprite, WhoopsTrapSprite, SpeedBoostSprite, GravityTrapSprite;

        public static GameObject ApUIGameObject, ArrowTrackerGameObject;
        public static Texture2D CassetteTexture;
        public static Image APLogoImage; 
        public static Dictionary<string, object> SlotData;
        private CancellationTokenSource _cancellationTokenSource = new();
        private DateTime _christmasTime = new(DateTime.Now.Year, 12, 25);
        public static bool ChristmasEvent, NoXmasEvent, NoAntiCheese;
        private static ArchipelagoData _archipelagoData;
        private static bool _appleAmount, annoy, _onlyOnce;
        private static int _realAppleAmount;
        private const string GithubAPIURL = "https://api.github.com/repos/niieli/NikoArchipelagoMod/releases/latest";
        public static GameObject APUpdateNotice;
        private string latestVersion = "";
        private string latestReleaseUrl = "";
        
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
            var now = DateTime.Now;
            var currentYear = now.Month == 1 ? now.Year - 1 : now.Year;
            var christmasTime = new DateTime(currentYear, 12, 25);
            var startChrismas = christmasTime.AddDays(-24);
            var endChrismas = christmasTime.AddDays(18);
            if (DateTime.Now.Ticks > startChrismas.Ticks && DateTime.Now.Ticks < endChrismas.Ticks)
            {
                ChristmasEvent = true;
                Logger.LogInfo($"Christmas Event Active.");
                AssetBundleXmas = AssetBundleLoader.LoadEmbeddedAssetBundle("apxmas");
            }
            else
            {
                NoXmasEvent = true;
            }
            NoAntiCheese = false;
            GameOptions.MasterVolume = mas;
            GameOptions.EnvVolume = env;
            GameOptions.MusicVolume = mus;
            GameOptions.SFXVolume = sfx;
            StartCoroutine(CheckGameSaveManager());
            if (Application.unityVersion.StartsWith("2020.2.4f1"))
            {
                Logger.LogInfo($"Found Pre-DLC version {Application.unityVersion} ! - Using compatibility...");
                Compatibility = true;
            }
            else
            {
                Logger.LogInfo($"Found DLC version {Application.unityVersion} !");
                AssetBundle = AssetBundleLoader.LoadEmbeddedAssetBundle("apassets");
            }
            if (AssetBundle != null)
            {
                APSprite = AssetBundle.LoadAsset<Sprite>("apLogo");
                BandanaSprite = AssetBundle.LoadAsset<Sprite>("BandanaAP");
                BowtieSprite = AssetBundle.LoadAsset<Sprite>("BowtieAP");
                CapSprite = AssetBundle.LoadAsset<Sprite>("CapAP");
                CatSprite = AssetBundle.LoadAsset<Sprite>("CatAP");
                ClownSprite = AssetBundle.LoadAsset<Sprite>("ClownFaceAP");
                FlowerSprite = AssetBundle.LoadAsset<Sprite>("FlowerAP");
                GlassesSprite = AssetBundle.LoadAsset<Sprite>("GlassesAP");
                KingSprite = AssetBundle.LoadAsset<Sprite>("KingStaffAP");
                MahjongSprite = AssetBundle.LoadAsset<Sprite>("MahjongAP");
                MotorSprite = AssetBundle.LoadAsset<Sprite>("MotorcycleAP");
                MouseSprite = AssetBundle.LoadAsset<Sprite>("MouseAP");
                SmallHatSprite = AssetBundle.LoadAsset<Sprite>("SmallHatAP");
                StarsSprite = AssetBundle.LoadAsset<Sprite>("StarsAP");
                SwordSprite = AssetBundle.LoadAsset<Sprite>("SwordAP");
                TopHatSprite = AssetBundle.LoadAsset<Sprite>("TophatAP");
                SunglassesSprite = AssetBundle.LoadAsset<Sprite>("SunglassesAP");
                APLogoSprite = AssetBundle.LoadAsset<Sprite>("HereComesNikoAPLogo");
                APLogoImage = AssetBundle.LoadAsset<Image>("HereComesNikoAPLogo");
                APIconSprite = AssetBundle.LoadAsset<Sprite>("nikoApLogo");
                CoinSprite = AssetBundle.LoadAsset<Sprite>("sprCoinLit");
                CassetteSprite = AssetBundle.LoadAsset<Sprite>("txrCassette");
                ContactListSprite = AssetBundle.LoadAsset<Sprite>("txrList");
                BottledSprite = AssetBundle.LoadAsset<Sprite>("BOTTLED_UP");
                EmployeeSprite = AssetBundle.LoadAsset<Sprite>("EMLOYEE_OF_THE_MONTH");
                FrogFanSprite = AssetBundle.LoadAsset<Sprite>("FROG_FAN");
                HandsomeSprite = AssetBundle.LoadAsset<Sprite>("HOPELESS_ROMANTIC");
                LostSprite = AssetBundle.LoadAsset<Sprite>("LOST_AT_SEA");
                SnailFashionSprite = AssetBundle.LoadAsset<Sprite>("SNAIL_FASHION_SHOW");
                VolleyDreamsSprite = AssetBundle.LoadAsset<Sprite>("VOLLEY_DREAMS");
                LetterSprite = AssetBundle.LoadAsset<Sprite>("txrLetter");
                ApplesSprite = AssetBundle.LoadAsset<Sprite>("AppleBundle");
                KeySprite = AssetBundle.LoadAsset<Sprite>("txrKey");
                HcSprite = AssetBundle.LoadAsset<Sprite>("TrainHairball");
                TtSprite = AssetBundle.LoadAsset<Sprite>("TrainTurbine");
                SfcSprite = AssetBundle.LoadAsset<Sprite>("TrainSalmon");
                PpSprite = AssetBundle.LoadAsset<Sprite>("TrainPool");
                BathSprite = AssetBundle.LoadAsset<Sprite>("TrainBath");
                HqSprite = AssetBundle.LoadAsset<Sprite>("TrainTadpole");
                GgSprite = AssetBundle.LoadAsset<Sprite>("GarysGarden");
                SnailMoneySprite = AssetBundle.LoadAsset<Sprite>("SnailMoney");
                BugSprite = AssetBundle.LoadAsset<Sprite>("Butterfly");
                ApProgressionSprite = AssetBundle.LoadAsset<Sprite>("ApProgression");
                ApUsefulSprite = AssetBundle.LoadAsset<Sprite>("ApUseful");
                ApFillerSprite = AssetBundle.LoadAsset<Sprite>("ApFiller");
                ApTrapSprite = AssetBundle.LoadAsset<Sprite>("ApTrap");
                ApTrap2Sprite = AssetBundle.LoadAsset<Sprite>("ApTrap2");
                ApTrap3Sprite = AssetBundle.LoadAsset<Sprite>("ApTrap3");
                CassetteTexture = AssetBundle.LoadAsset<Texture2D>("ApProgression");
                GoalBadSprite = AssetBundle.LoadAsset<Sprite>("goalBad");
                SuperJumpSprite = AssetBundle.LoadAsset<Sprite>("SuperJump");
                FishSprite = AssetBundle.LoadAsset<Sprite>("imgMapfish");
                HairballFishSprite = AssetBundle.LoadAsset<Sprite>("HairballFish");
                TurbineFishSprite = AssetBundle.LoadAsset<Sprite>("TurbineFish");
                SalmonFishSprite = AssetBundle.LoadAsset<Sprite>("SalmonFish");
                PoolFishSprite = AssetBundle.LoadAsset<Sprite>("PoolFish");
                BathFishSprite = AssetBundle.LoadAsset<Sprite>("BathFish");
                TadpoleFishSprite = AssetBundle.LoadAsset<Sprite>("TadpoleFish");
                HairballKeySprite = AssetBundle.LoadAsset<Sprite>("txrHairballKey");
                TurbineKeySprite = AssetBundle.LoadAsset<Sprite>("txrTurbineKey");
                SalmonKeySprite = AssetBundle.LoadAsset<Sprite>("txrSalmonKey");
                PoolKeySprite = AssetBundle.LoadAsset<Sprite>("txrPoolKey");
                BathKeySprite = AssetBundle.LoadAsset<Sprite>("txrBathKey");
                TadpoleKeySprite = AssetBundle.LoadAsset<Sprite>("txrTadpoleKey");
                TimePieceSprite = AssetBundle.LoadAsset<Sprite>("timepiece2D");
                YarnSprite = AssetBundle.LoadAsset<Sprite>("Yarn");
                HairballFlowerSprite = AssetBundle.LoadAsset<Sprite>("HairballFlower");
                TurbineFlowerSprite = AssetBundle.LoadAsset<Sprite>("TurbineFlower");
                SalmonFlowerSprite = AssetBundle.LoadAsset<Sprite>("SalmonFlower");
                PoolFlowerSprite = AssetBundle.LoadAsset<Sprite>("PoolFlower");
                BathFlowerSprite = AssetBundle.LoadAsset<Sprite>("BathFlower");
                TadpoleFlowerSprite = AssetBundle.LoadAsset<Sprite>("TadpoleFlower");
                HairballSeedSprite = AssetBundle.LoadAsset<Sprite>("HairballSeed");
                SalmonSeedSprite = AssetBundle.LoadAsset<Sprite>("SalmonSeed");
                BathSeedSprite = AssetBundle.LoadAsset<Sprite>("BathSeed");
                HairballCassetteSprite = AssetBundle.LoadAsset<Sprite>("HairballCassette");
                TurbineCassetteSprite = AssetBundle.LoadAsset<Sprite>("TurbineCassette");
                SalmonCassetteSprite = AssetBundle.LoadAsset<Sprite>("SalmonCassette");
                PoolCassetteSprite = AssetBundle.LoadAsset<Sprite>("PoolCassette");
                BathCassetteSprite = AssetBundle.LoadAsset<Sprite>("BathCassette");
                TadpoleCassetteSprite = AssetBundle.LoadAsset<Sprite>("TadpoleCassette");
                GardenCassetteSprite = AssetBundle.LoadAsset<Sprite>("GardenCassette");
                FischerNoteSprite = AssetBundle.LoadAsset<Sprite>("sprFischerHappyNote");
                GabiNoteSprite = AssetBundle.LoadAsset<Sprite>("sprFlowerNote");
                MoomyNoteSprite = AssetBundle.LoadAsset<Sprite>("sprHamsterNote");
                BlessleyNoteSprite = AssetBundle.LoadAsset<Sprite>("sprBugNote");
                FreezeTrap = AssetBundle.LoadAsset<GameObject>("FreezeTrap");
                IronBootsTrap = AssetBundle.LoadAsset<GameObject>("IronBootsTrap");
                MyTurnTrap = AssetBundle.LoadAsset<GameObject>("MyTurnTrap");
                WhoopsTrap = AssetBundle.LoadAsset<GameObject>("WhoopsTrap");
                FreezeTrapSprite = AssetBundle.LoadAsset<Sprite>("Schneeflocken1");
                IronBootsTrapSprite = AssetBundle.LoadAsset<Sprite>("TrapIronBoots");
                MyTurnTrapSprite = AssetBundle.LoadAsset<Sprite>("imgPepper");
                WhoopsTrapSprite = AssetBundle.LoadAsset<Sprite>("TrapWhoops");
                SpeedBoostSprite = AssetBundle.LoadAsset<Sprite>("SpeedBoost");
                GravityTrap = AssetBundle.LoadAsset<GameObject>("GravityTrap");
                GravityTrapSprite = AssetBundle.LoadAsset<Sprite>("BuzzNote");
                _canLogin = true;
            }
            var gameObjectChecker = new GameObject("GameObjectChecker");
            gameObjectChecker.AddComponent<GameObjectChecker>();
            // ArrowTrackerGameObject = new GameObject("APArrowTracker");
            // ArrowTrackerGameObject.AddComponent<ArrowTrackerManager>();
            // DontDestroyOnLoad(ArrowTrackerGameObject);
            DontDestroyOnLoad(gameObjectChecker);
            harmony = new Harmony(PluginGuid);
            harmony.PatchAll();
            SceneManager.sceneLoaded += OnSceneLoaded;
            Logger.LogInfo("Plugin loaded and Harmony patches applied initially!");
            StartCoroutine(CheckForUpdate());
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
        
        private IEnumerator CheckForUpdate()
        {
            yield return new WaitUntil(ArchipelagoClient.IsValidScene);
            using (UnityWebRequest request = UnityWebRequest.Get(GithubAPIURL))
            {
                request.SetRequestHeader("User-Agent", "Unity-BepInEx-UpdateChecker");
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string json = request.downloadHandler.text;
                    latestVersion = ExtractJsonValue(json, "tag_name");
                    latestReleaseUrl = ExtractJsonValue(json, "html_url");

                    if (!string.IsNullOrEmpty(latestVersion) &&
                        Version.TryParse(latestVersion, out var latest) &&
                        Version.TryParse(PluginVersion, out var current) &&
                        latest > current)
                    {
                        Logger.LogMessage("Found a new update! " + latestVersion);
                        SpawnUpdateNotice();
                    }
                }
                else
                {
                    Logger.LogError("Failed to check for updates.");
                }
            }
        }

        private string ExtractJsonValue(string json, string key)
        {
            int startIndex = json.IndexOf($"\"{key}\":\"", StringComparison.Ordinal) + key.Length + 4;
            if (startIndex > key.Length + 3)
            {
                int endIndex = json.IndexOf("\"", startIndex, StringComparison.Ordinal);
                return json.Substring(startIndex, endIndex - startIndex).Trim();
            }
            return string.Empty;
        }

        private void SpawnUpdateNotice()
        {
            var apUpdateNoticePrefab = AssetBundle.LoadAsset<GameObject>("APUpdateNotice");
            APUpdateNotice = Instantiate(apUpdateNoticePrefab, GameObject.Find("UI").transform, false);
            if (APUpdateNotice == null)
            {
                BepinLogger.LogError("Failed to instantiate APUpdateNotice prefab.");
                return;
            }
            APUpdateNotice.layer = LayerMask.NameToLayer("UI");
            APUpdateNotice.transform.SetSiblingIndex(30);
            APUpdateNotice.transform.Find("Panel/VersionBack").gameObject.GetComponent<TextMeshProUGUI>().text 
                = "Version: "+latestVersion;
            APUpdateNotice.transform.Find("Panel/VersionFront").gameObject.GetComponent<TextMeshProUGUI>().text 
                = "Version: "+latestVersion;
            var dismiss = APUpdateNotice.transform.Find("Panel/Dismiss").gameObject.GetComponent<Button>();
            dismiss.onClick.AddListener(DestroyNotice);
            var download = APUpdateNotice.transform.Find("Panel/Download").gameObject.GetComponent<Button>();
            download.onClick.AddListener(DownloadUpdate);
            APUpdateNotice.SetActive(false);
        }

        private void DestroyNotice()
        {
            Destroy(APUpdateNotice);
        }

        private void DownloadUpdate()
        {
            Application.OpenURL(latestReleaseUrl);
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
            //Logger.LogFatal("LoggedIn: " + loggedIn); Something broke somehow | Need to investigate on why it skips setting this true
            //Logger.LogFatal("ItemIndex: " + ArchipelagoClient.GetItemIndex());
            if (!saveReady) return;
            try
            {
                _noteDisplayer = scrNotificationDisplayer.instance;
                //Savefile is the same as SlotName & ServerPort, ':' is not allowed to be in a filename
                //saveName = "APSave" + "_" + ArchipelagoClient.ServerData.SlotName + "_" + ArchipelagoClient.ServerData.Uri.Replace(":", ".");
                saveName = "APSave" + "_" + ArchipelagoClient.ServerData.SlotName;
                if (scrGameSaveManager.saveName != saveName && ArchipelagoClient.Authenticated)
                {
                    scrGameSaveManager.saveName = saveName;
                    var savePath = Path.Combine(ArchipelagoFolderPath, saveName + "_" + Seed + "_" + ArchipelagoClient._session.Players.ActivePlayer.Slot + ".json");

                    // Check if the save file exists
                    if (File.Exists(savePath))
                    {
                        scrGameSaveManager.dataPath = savePath;
                        Logger.LogInfo("Found a SaveFile with the current SlotName & Port!");
                        ArchipelagoConsole.LogMessage("Found a SaveFile with the current SlotName & Port!");
                        gameSaveManager.LoadGame();
                    }
                    else
                    {
                        // If no save file is found, create a new one
                        newFile = true;
                        scrGameSaveManager.dataPath = savePath;
                        Logger.LogWarning("No SaveFile found. Creating a new one!");
                        ArchipelagoConsole.LogMessage("No SaveFile found. Creating a new one!");
                        
                        gameSaveManager.SaveGame();
                        gameSaveManager.LoadGame();
                        gameSaveManager.ClearSaveData();
                    }

                    if (gameSaveManager.gameData.generalGameData.currentLevel == 0)
                    {
                        scrTrainManager.instance.UseTrain(1, false);
                    }
                    else
                    {
                        scrTrainManager.instance.UseTrain(!newFile ? gameSaveManager.gameData.generalGameData.currentLevel : 1, false);
                    }
                    if (newFile)
                        StartCoroutine(FirstLoginFix());
                    LogFlags();
                    StartCoroutine(CheckWorldSaveManager());
                    loggedIn = true;
                    SaveEstablished = false;
                    _realAppleAmount = gameSaveManager.gameData.generalGameData.appleAmount;
                    ArchipelagoClient.CheckReceivedItems();
                    //scrGameSaveManager.instance.gameData.generalGameData.snailSteps = ArchipelagoClient._session.DataStorage["SnailMoney"];
                    // APSendNote($"Connected to {ArchipelagoClient.ServerData.Uri} successfully", 10F);
                }
                if (scrGameSaveManager.saveName == saveName && !ArchipelagoClient.Authenticated)
                {
                    loggedIn = false;
                    if (!_onlyOnce)
                    {
                        StartCoroutine(DisconnectNotification());
                        _onlyOnce = true;
                    }
                }

                if (scrGameSaveManager.instance.gameData.generalGameData.currentLevel == 0)
                {
                    scrTrainManager.instance.UseTrain(1, false);
                }

                if (!loggedIn && ArchipelagoClient.Authenticated && SaveEstablished)
                {
                    loggedIn = true;
                    _onlyOnce = false;
                }

                KioskCost.PreFix();
                //MyCharacterController.instance._diveConsumed = true;
                Flags();
                if (worldReady & ArchipelagoClient.Authenticated)
                {
                    StartCoroutine(SyncState());
                    if (SaveEstablished)
                    {
                        LocationHandler.Update2();
                        LocationHandler.SnailShop();
                        LocationHandler.WinCompletion();
                        ArchipelagoClient.CheckLocationState();
                    }
                    
                }
                DebugMode = File.Exists(Path.Combine(Paths.PluginPath, "debug.txt")) || ArchipelagoMenu.forceDebug;
                if (!MyCharacterController.instance.blockMovementInput && !SaveEstablished)
                {
                    SaveEstablished = true;
                    Logger.LogMessage("Save file safety check finished!");
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
            if (ArchipelagoClient.Authenticated)
            {
                ArchipelagoClient.Disconnect();
                Environment.FailFast("OnApplicationQuit"); //TODO: This but better
            }
            _cancellationTokenSource.Cancel();
            StopAllCoroutines();
            //Application.Quit();
        }
        
        private static IEnumerator FirstLoginFix()
        {
            yield return new WaitUntil(ArchipelagoClient.IsValidScene);
            if (!ArchipelagoClient.Authenticated)
            {
                ArchipelagoClient.Connect();
                APSendNote($"Connected to {ArchipelagoClient.ServerData.Uri} successfully", 6F);
                loggedIn = true; 
            }
        }
        
        private static IEnumerator DisconnectNotification()
        {
            while (!loggedIn)
            {
                if (!annoy && ArchipelagoClient.IsValidScene())
                {
                    BepinLogger.LogWarning("You have disconnected from the server.");
                    var achievement = ScriptableObject.CreateInstance<AchievementObject>();
                    achievement.nameKey = "Disconnected from Server";
                    achievement.icon = ApFillerSprite;
                    AchievementPopup.instance.PopupAchievement(achievement);
                    AchievementPopup.instance.nameMesh.text = achievement.nameKey;
                    AchievementPopup.instance.audioSource.volume = 0.35f;
                    annoy = true;
                }
                yield return new WaitForSeconds(5f);
                annoy = false;
            }
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
            if (ArchipelagoClient.CoinAmount >= int.Parse(ArchipelagoData.slotData["kioskhq"].ToString()))
            {
                ArchipelagoClient.ElevatorRepaired = true;
            }
            if (ArchipelagoData.slotData.ContainsKey("key_level"))
            {
                if (int.Parse(ArchipelagoData.slotData["key_level"].ToString()) == 1)
                {
                    ArchipelagoClient.Keysanity = true;
                    SyncValue(ref ItemHandler.HairballKeyAmount, ArchipelagoClient.HcKeyAmount - ItemHandler.UsedKeysHairball());
                    SyncValue(ref ItemHandler.TurbineKeyAmount, ArchipelagoClient.TtKeyAmount - ItemHandler.UsedKeysTurbine());
                    SyncValue(ref ItemHandler.SalmonKeyAmount, ArchipelagoClient.SfcKeyAmount - ItemHandler.UsedKeysSalmon());
                    SyncValue(ref ItemHandler.PoolKeyAmount, ArchipelagoClient.PpKeyAmount - ItemHandler.UsedKeysPool());
                    SyncValue(ref ItemHandler.BathKeyAmount, ArchipelagoClient.BathKeyAmount - ItemHandler.UsedKeysBath());
                    SyncValue(ref ItemHandler.TadpoleKeyAmount, ArchipelagoClient.HqKeyAmount - ItemHandler.UsedKeysTadpole());
                    FakeLevelSpecificKeyAmount();
                }
                else
                {
                    SyncValue(ref generalGameData.keyAmount, ArchipelagoClient.KeyAmount - ItemHandler.UsedKeys());
                    if (generalGameData.keyAmount < 0) generalGameData.keyAmount = 0;
                }
            } else
            {
                SyncValue(ref generalGameData.keyAmount, ArchipelagoClient.KeyAmount - ItemHandler.UsedKeys());
                if (generalGameData.keyAmount < 0) generalGameData.keyAmount = 0;
            }

            if (ArchipelagoData.slotData.ContainsKey("fishsanity"))
            {
                if (int.Parse(ArchipelagoData.slotData["fishsanity"].ToString()) == 2)
                {
                    SyncValue(ref ItemHandler.HairballFishAmount, ArchipelagoClient.HcFishAmount);
                    SyncValue(ref ItemHandler.TurbineFishAmount, ArchipelagoClient.TtFishAmount);
                    SyncValue(ref ItemHandler.SalmonFishAmount, ArchipelagoClient.SfcFishAmount);
                    SyncValue(ref ItemHandler.PoolFishAmount, ArchipelagoClient.PpFishAmount);
                    SyncValue(ref ItemHandler.BathFishAmount, ArchipelagoClient.BathFishAmount);
                    SyncValue(ref ItemHandler.TadpoleFishAmount, ArchipelagoClient.HqFishAmount);
                }
            }
            if (ArchipelagoData.slotData.ContainsKey("seedsanity"))
            {
                if (int.Parse(ArchipelagoData.slotData["seedsanity"].ToString()) == 2)
                {
                    SyncValue(ref ItemHandler.HairballSeedAmount, ArchipelagoClient.HcSeedAmount);
                    SyncValue(ref ItemHandler.SalmonSeedAmount, ArchipelagoClient.SfcSeedAmount);
                    SyncValue(ref ItemHandler.BathSeedAmount, ArchipelagoClient.BathSeedAmount);
                }
            }
            if (ArchipelagoData.slotData.ContainsKey("flowersanity"))
            {
                if (int.Parse(ArchipelagoData.slotData["flowersanity"].ToString()) == 2)
                {
                    SyncValue(ref ItemHandler.HairballFlowerAmount, ArchipelagoClient.HcFlowerAmount);
                    SyncValue(ref ItemHandler.TurbineFlowerAmount, ArchipelagoClient.TtFlowerAmount);
                    SyncValue(ref ItemHandler.SalmonFlowerAmount, ArchipelagoClient.SfcFlowerAmount);
                    SyncValue(ref ItemHandler.PoolFlowerAmount, ArchipelagoClient.PpFlowerAmount);
                    SyncValue(ref ItemHandler.BathFlowerAmount, ArchipelagoClient.BathFlowerAmount);
                    SyncValue(ref ItemHandler.TadpoleFlowerAmount, ArchipelagoClient.HqFlowerAmount);
                }
            }
            if (int.Parse(ArchipelagoData.slotData["cassette_logic"].ToString()) == 0)
            {
                SyncValue(ref ItemHandler.HairballCassetteAmount, ArchipelagoClient.HcCassetteAmount);
                SyncValue(ref ItemHandler.TurbineCassetteAmount, ArchipelagoClient.TtCassetteAmount);
                SyncValue(ref ItemHandler.SalmonCassetteAmount, ArchipelagoClient.SfcCassetteAmount);
                SyncValue(ref ItemHandler.PoolCassetteAmount, ArchipelagoClient.PpCassetteAmount);
                SyncValue(ref ItemHandler.BathCassetteAmount, ArchipelagoClient.BathCassetteAmount);
                SyncValue(ref ItemHandler.TadpoleCassetteAmount, ArchipelagoClient.HqCassetteAmount);
                SyncValue(ref ItemHandler.GardenCassetteAmount, ArchipelagoClient.GgCassetteAmount);
                FakeLevelSpecificCassetteAmount();
            }
            else
            {
                SyncValue(ref generalGameData.cassetteAmount, ArchipelagoClient.CassetteAmount);
            }
            SyncValue(ref generalGameData.secretMove, ArchipelagoClient.SuperJump);
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
            SyncValue(ref ItemHandler.Garden, ArchipelagoClient.TicketGary);
            //ArchipelagoClient._session.DataStorage["Apples"] = scrGameSaveManager.instance.gameData.generalGameData.appleAmount;
            //ArchipelagoClient._session.DataStorage["SnailMoney"] = scrGameSaveManager.instance.gameData.generalGameData.snailSteps;
            if (ArchipelagoClient.queuedItems2.Count <= 0 || !ArchipelagoClient.IsValidScene())
            {
                foreach (var t in ArchipelagoClient.queuedItems2)
                {
                    ArchipelagoClient.GiveItem(t, false);
                }
                ArchipelagoClient.queuedItems2.Clear();
            }
            if (ArchipelagoClient.queuedItems.Count <= 0 || !ArchipelagoClient.IsValidScene() || !SaveEstablished) yield break;
            foreach (var t in ArchipelagoClient.queuedItems)
            {
                ArchipelagoClient.GiveItem(t);
            }
            ArchipelagoClient.queuedItems.Clear();
        }

        private static void FakeLevelSpecificKeyAmount()
        {
            scrGameSaveManager.instance.gameData.generalGameData.keyAmount = SceneManager.GetActiveScene().name switch
            {
                "Hairball City" => ItemHandler.HairballKeyAmount,
                "Trash Kingdom" => ItemHandler.TurbineKeyAmount,
                "Salmon Creek Forest" => ItemHandler.SalmonKeyAmount,
                "Public Pool" => ItemHandler.PoolKeyAmount,
                "The Bathhouse" => ItemHandler.BathKeyAmount,
                "Tadpole inc" => ItemHandler.TadpoleKeyAmount,
                _ => 0
            };
            if (scrGameSaveManager.instance.gameData.generalGameData.keyAmount < 0) scrGameSaveManager.instance.gameData.generalGameData.keyAmount = 0;
        }
        
        private static void FakeLevelSpecificCassetteAmount()
        {
            scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount = SceneManager.GetActiveScene().name switch
            {
                "Hairball City" => ItemHandler.HairballCassetteAmount,
                "Trash Kingdom" => ItemHandler.TurbineCassetteAmount,
                "Salmon Creek Forest" => ItemHandler.SalmonCassetteAmount,
                "Public Pool" => ItemHandler.PoolCassetteAmount,
                "The Bathhouse" => ItemHandler.BathCassetteAmount,
                "Tadpole inc" => ItemHandler.TadpoleCassetteAmount,
                "GarysGarden" => ItemHandler.GardenCassetteAmount,
                _ => 0
            };
        }

        public static void MovementSpeedMultiplier()
        {
            if (!ArchipelagoClient.IsValidScene()) return;
            float amount = ArchipelagoClient.SpeedBoostAmount;
            if (ArchipelagoClient.SpeedBoostAmount < 0)
            {
                return;
            }
            amount++;
            amount *= 1.15f;
            MyCharacterController.instance.DiveSpeed += amount;
            MyCharacterController.instance.MaxAirMoveSpeed += amount;
            MyCharacterController.instance.JumpSpeed += (amount*0.25f);
            MyCharacterController.instance.MaxStableMoveSpeed += amount;
            MyCharacterController.instance.MaxWaterMoveSpeed += amount;

            BepinLogger.LogInfo($"Speed Boost Applied: {amount}x");
        }

        public void LogFlags()
        {
            saveDataCoinFlag.ForEach(Logger.LogInfo);
            saveDataCassetteFlag.ForEach(Logger.LogInfo);
            saveDataFishFlag.ForEach(Logger.LogInfo);
            saveDataMiscFlag.ForEach(Logger.LogInfo);
            saveDataLetterFlag.ForEach(Logger.LogInfo);
            saveDataGeneralFlag.ForEach(Logger.LogInfo);
        }
        
        public static void APSendNote(string note, float time, Sprite sprite = null)
        {
            var apNote = ScriptableObject.CreateInstance<Notification>();
            apNote.timed = true;
            apNote.avatar = sprite ? sprite : APSprite;
            apNote.duration = time;
            apNote.key = note;
            _noteDisplayer.AddNotification(apNote);
            BepinLogger.LogMessage("Note: " + _noteDisplayer.textMesh.text);
        }
        public void KillPlayer(string cause)
        {
            ArchipelagoConsole.LogMessage(cause);
            if (Compatibility)
            {
                scrTrainManager.instance.UseTrain(gameSaveManager.gameData.generalGameData.currentLevel);
            }
            else
            {
                scrTrainManager.instance.UseTrain(gameSaveManager.gameData.generalGameData.currentLevel, false);
            }
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
            Logger.LogInfo("Total Coin Count: " + coinTotal);
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
            if (ArchipelagoClient.Authenticated && ArchipelagoMenu.status)
            {
                BackgroundForText(new Rect(10, 10, 260, 90));
                statusMessage = " Status: Connected";
                GUI.Label(new Rect(16, 16, 300, 22), ModDisplayInfo);
                GUI.Label(new Rect(16, 50, 300, 22), APDisplayInfo + statusMessage);
                if (GUI.Button(new Rect(160, 16, 100, 20), "Disconnect") && ArchipelagoClient.Authenticated)
                {
                    ArchipelagoClient.Disconnect();
                }
                Tracker();
            }

            if (!DebugMode) return;
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
                if (Compatibility)
                {
                    scrTrainManager.instance.UseTrain(goToLevel);
                }
                else
                {
                    scrTrainManager.instance.UseTrain(goToLevel, false);
                }
            }
            if (GUI.Button(new Rect(16, 220, 140, 20), "Money +1000"))
            {
                scrGameSaveManager.instance.gameData.generalGameData.snailSteps += 1000;
            }
            if (GUI.Button(new Rect(16, 240, 100, 20), "Kill"))
            {
                KillPlayer("GETTT DEATHLINKED HEEHEH");
            }
            if (GUI.Button(new Rect(16, 260, 140, 20), "Item Index"))
            {
                ArchipelagoClient.GetItemIndex();
            }
            if (GUI.Button(new Rect(16, 280, 140, 20), "Collect State"))
            {
                ArchipelagoClient.CheckLocationState();
            }
            if (GUI.Button(new Rect(16, 300, 100, 20), "SlotData"))
            {
                Logger.LogWarning("Deathlink: "+ArchipelagoData.slotData["death_link"]);
                Logger.LogWarning("Goal: "+ArchipelagoData.slotData["goal_completion"]);
                Logger.LogWarning("Kiosk Home: "+ArchipelagoData.slotData["kioskhome"]);
                Logger.LogWarning("Kiosk Hairball City: "+ArchipelagoData.slotData["kioskhc"]);
                Logger.LogWarning("Kiosk Turbine Town: "+ArchipelagoData.slotData["kiosktt"]);
                Logger.LogWarning("Kiosk Salmon Creek Forest: "+ArchipelagoData.slotData["kiosksfc"]);
                Logger.LogWarning("Kiosk Public Pool: "+ArchipelagoData.slotData["kioskpp"]);
                Logger.LogWarning("Kiosk Bathhouse: "+ArchipelagoData.slotData["kioskbath"]);
                Logger.LogWarning("Elevator Repair: "+ArchipelagoData.slotData["kioskhq"]);
                Logger.LogWarning("Garden Access: "+ArchipelagoData.slotData["garden_access"]);
                Logger.LogWarning("Fishsanity: "+ArchipelagoData.slotData["fishsanity"]);
                Logger.LogWarning("Seedsanity: "+ArchipelagoData.slotData["seedsanity"]);
                Logger.LogWarning("Flowersanity: "+ArchipelagoData.slotData["flowersanity"]);
                Logger.LogWarning("Applesanity: "+ArchipelagoData.slotData["applessanity"]);
                Logger.LogWarning("Hairball City - Mitch: "+ArchipelagoData.slotData["chc1"]);
                Logger.LogWarning("Hairball City - Mai: "+ArchipelagoData.slotData["chc2"]);
                Logger.LogWarning("Turbine Town - Mitch: "+ArchipelagoData.slotData["ctt1"]);
                Logger.LogWarning("Turbine Town - Mai: "+ArchipelagoData.slotData["ctt2"]);
                Logger.LogWarning("Salmon Creek Forest - Mitch: "+ArchipelagoData.slotData["csfc1"]);
                Logger.LogWarning("Salmon Creek Forest - Mai: "+ArchipelagoData.slotData["csfc2"]);
                Logger.LogWarning("Public Pool - Mitch: "+ArchipelagoData.slotData["cpp1"]);
                Logger.LogWarning("Public Pool - Mai: "+ArchipelagoData.slotData["cpp2"]);
                Logger.LogWarning("Bathhouse - Mitch: "+ArchipelagoData.slotData["cbath1"]);
                Logger.LogWarning("Bathhouse - Mai: "+ArchipelagoData.slotData["cbath2"]);
                Logger.LogWarning("Tadpole HQ - Mitch: "+ArchipelagoData.slotData["chq1"]);
                Logger.LogWarning("Tadpole HQ - Mai: "+ArchipelagoData.slotData["chq2"]);
                Logger.LogWarning("Gary's Garden - Mitch: "+ArchipelagoData.slotData["cgg1"]);
                Logger.LogWarning("Gary's Garden - Mai: "+ArchipelagoData.slotData["cgg2"]);
            }
            if (GUI.Button(new Rect(16, 320, 100, 20), "Current Level"))
            {
                Logger.LogInfo(scrGameSaveManager.instance.gameData.generalGameData.currentLevel-1);
            }

            if (GUI.Button(new Rect(16, 340, 100, 20), "Resync"))
            {
                ArchipelagoClient.CheckReceivedItems();
                foreach (var t in ArchipelagoClient._session.Items.AllItemsReceived)
                {
                    Logger.LogWarning("Counted Item: " + t.ItemName + " | ItemID: " + t.ItemId);
                }
                ArchipelagoClient.CheckLocationState();
            }
            snowAmount = Convert.ToInt32(GUI.TextField(new Rect(150, 360, 80, 20), snowAmount.ToString()));
            if (GUI.Button(new Rect(16, 360, 100, 20), "Snowflake Amount"))
            {
                StayOnScreen.snowflakeAmount = snowAmount;
                Logger.LogInfo($"Snowflake Amount: {StayOnScreen.snowflakeAmount}");
            }
            
            if (GUI.Button(new Rect(16, 380, 100, 20), "Freeze Trap"))
            {
                TrapManager.instance.ActivateTrap("Zero Gravity", 30f);
            }
            if (GUI.Button(new Rect(16, 400, 100, 20), "Iron Boots Trap"))
            {
                TrapManager.instance.ActivateTrap("Iron Boots", 10f);
            }
            if (GUI.Button(new Rect(16, 420, 100, 20), "My Turn! Trap"))
            {
                TrapManager.instance.ActivateTrap("My Turn!", 30f);
            }
            if (GUI.Button(new Rect(16, 450, 110, 20), "No Anti-Cheese"))
            {
                NoAntiCheese = true;
                APSendNote("Disabled Anti-Cheese...", 7f, ApFillerSprite);
            }
            if (GUI.Button(new Rect(16, 480, 100, 20), "Apples"))
            {
                // scrApple[] apples = GameObject.FindObjectsByType<scrApple>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
                // foreach (var apple in apples)
                // {
                //     int uniqueID = apple.GetInstanceID();
                //     Logger.LogFatal($"GameObject Name: {apple.name} | Apple Position: {apple.transform.position} |" +
                //                     $"New Apple Flag: {apple.transform.position.magnitude} | GameObject Active: {apple.gameObject.activeInHierarchy}");
                // }
                
                // GameObject[] apples = GameObject.FindGameObjectsWithTag("Apple");
                // foreach (GameObject apple in apples)
                // {
                //     int uniqueID = apple.GetInstanceID();
                //     Logger.LogFatal($"GameObject Name: {apple.name} | Apple Instance ID: {uniqueID} | GameObject Active: {apple.gameObject.activeInHierarchy}");
                // }

                foreach (var keyValuePair in Applesanity.ApplesanityStart.appleIDs)
                {
                    if (keyValuePair.Key != null)
                    {
                        Logger.LogFatal($"Apples: {keyValuePair.Key} - {keyValuePair.Value} | Flag: Apple{keyValuePair.Value} " +
                                        $"| ID: {keyValuePair.Value} | Active: {keyValuePair.Key.gameObject.activeInHierarchy}");
                    }
                    else
                    {
                        Applesanity.ApplesanityStart.appleIDs.Remove(keyValuePair.Key);
                    }
                }
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
            var slotData = ArchipelagoData.slotData;
             var flagData = new (string flagKey, string successMsg, string failureMsg, int xPos, int yPos)[]
             {
            //   ("APWave1", "Got Contact List 1!", "No Contact List 1!", 16, 70),
            //   ("APWave2", "Got Contact List 2!", "No Contact List 2!", 170, 70),
            //     ("KioskHome", "Kiosk Home", $"Kiosk Home({slotData["kioskhome"]})", 16, 90),
            //     ("KioskHairball City", "Kiosk HC", $"Kiosk HC({slotData["kioskhc"]})", 115, 90),
            //     ("KioskTrash Kingdom", "Kiosk TT", $"Kiosk TT({slotData["kiosktt"]})", 200, 90),
            //     ("KioskSalmon Creek Forest", "Kiosk SCF", $"Kiosk SCF({slotData["kiosksfc"]})", 16, 110),
            //     ("KioskPublic Pool", "Kiosk PP", $"Kiosk PP({slotData["kioskpp"]})", 115, 110),
            //     ("KioskThe Bathhouse", "Kiosk Bath", $"Kiosk Bath({slotData["kioskbath"]})", 200, 110),
             };
            if (int.Parse(slotData["goal_completion"].ToString()) == 0)
            {
                GUI.Label(new Rect(34, 70, 300, 20), $"Goal: Get Hired | Repair the elevator!");
            }
            else
            {
                GUI.Label(new Rect(19, 70, 300, 20), "Goal: Employee Of The Month! (76 Coins)");
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
