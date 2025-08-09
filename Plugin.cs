using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Color = UnityEngine.Color;
using UnityEngine.InputSystem;

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
        public const string PluginVersion = "0.7.8";
        
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
        private static scrNotificationDisplayer _noteDisplayer;
        public static bool worldReady;
        private static bool _canLogin;
        public static readonly string ArchipelagoFolderPath = Path.Combine(Application.persistentDataPath, "Archipelago");
        private static readonly string AssetsFolderPath = Path.Combine(Paths.PluginPath, "APAssets");
        public static bool loggedIn, Compatibility, SaveEstablished, PlayerFound;
        public static string Seed;
        public static AssetBundle AssetBundle, AssetBundleXmas;
        public static GameObject TrapFreeze, TrapIronBoots, TrapMyTurn, TrapWhoops, TrapGravity,
            ItemNotification, HintNotification,
            TrapWide, TrapHome, TrapJumpingJacks, TrapPhoneCall, TrapTiny, TrapFast,
            NoticeBonkHelmet, NoticeSodaCan, NoticeParasol, NoticeAC, NoticeSwimCourse, DeathLinkNotice, NoticeBugNet,
            NoticePartyTicket, NoticeAppleBasket;
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
            SnailMoneySprite, BugsSprite, GgSprite, GoalBadSprite,
            ApProgressionSprite, ApUsefulSprite, ApFillerSprite, ApTrapSprite, ApTrap2Sprite, ApTrap3Sprite,
            TimePieceSprite, YarnSprite,
            HairballFlowerSprite, TurbineFlowerSprite, SalmonFlowerSprite, PoolFlowerSprite, BathFlowerSprite, TadpoleFlowerSprite,
            HairballSeedSprite, SalmonSeedSprite, BathSeedSprite,
            HairballCassetteSprite, TurbineCassetteSprite, SalmonCassetteSprite, PoolCassetteSprite, BathCassetteSprite, TadpoleCassetteSprite, GardenCassetteSprite,
            FischerNoteSprite, GabiNoteSprite, MoomyNoteSprite, BlessleyNoteSprite,
            FreezeTrapSprite, IronBootsTrapSprite, MyTurnTrapSprite, WhoopsTrapSprite, SpeedBoostSprite, GravityTrapSprite,
            PhoneCallTrapSprite, JumpingJacksTrapSprite, WideTrapSprite, HomeTrapSprite, TinyTrapSprite,
            PartyTicketSprite, BonkHelmetSprite, BugNetSprite, SodaRepairSprite, ParasolRepairSprite, SwimCourseSprite, TextboxItemSprite, ACRepairSprite,
            AppleBasketSprite, DeathLinkSprite, HairballBoneSprite, TurbineBoneSprite, SalmonBoneSprite, PoolBoneSprite, BathBoneSprite, TadpoleBoneSprite;

        public static Material ProgNotificationTexture, UsefulNotificationTexture, FillerNotificationTexture, TrapNotificationTexture;
        public static GameObject SparksParticleSystem;
        public static GameObject NotifcationCanvas;
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
        public static GameObject BasicBlock;
        
        private void Awake()
        {
            Chainloader.ManagerObject.hideFlags = HideFlags.HideAndDontSave;
            BepinLogger = Logger;
            ArchipelagoClient = new ArchipelagoClient();
            ArchipelagoConsole.Awake();
            Logger.LogInfo($"Hey, Niko here! Plugin {PluginName} Loaded! :)");
            ArchipelagoConsole.LogMessage($"{ModDisplayInfo} loaded!");
            saveName = ArchipelagoClient.ServerData.SlotName;
            if (!Directory.Exists(ArchipelagoFolderPath))
            {
                Directory.CreateDirectory(ArchipelagoFolderPath);
                Logger.LogInfo("Archipelago folder created.");
            }
            SavedData.Instance.LoadSettings();
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
                BugsSprite = AssetBundle.LoadAsset<Sprite>("BugBundle");
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
                TrapFreeze = AssetBundle.LoadAsset<GameObject>("TrapFreeze");
                TrapIronBoots = AssetBundle.LoadAsset<GameObject>("TrapIronBoots");
                TrapMyTurn = AssetBundle.LoadAsset<GameObject>("TrapMyTurn");
                TrapWhoops = AssetBundle.LoadAsset<GameObject>("TrapWhoops");
                FreezeTrapSprite = AssetBundle.LoadAsset<Sprite>("Schneeflocken1");
                IronBootsTrapSprite = AssetBundle.LoadAsset<Sprite>("TrapIronBoots");
                MyTurnTrapSprite = AssetBundle.LoadAsset<Sprite>("imgPepper");
                WhoopsTrapSprite = AssetBundle.LoadAsset<Sprite>("TrapWhoops");
                SpeedBoostSprite = AssetBundle.LoadAsset<Sprite>("SpeedBoost");
                TrapGravity = AssetBundle.LoadAsset<GameObject>("TrapGravity");
                GravityTrapSprite = AssetBundle.LoadAsset<Sprite>("BuzzNote");
                ProgNotificationTexture = AssetBundle.LoadAsset<Material>("APProgressionNotificationMaterial");
                UsefulNotificationTexture = AssetBundle.LoadAsset<Material>("APUsefulNotificationMaterial");
                FillerNotificationTexture = AssetBundle.LoadAsset<Material>("APFillerNotificationMaterial");
                TrapNotificationTexture = AssetBundle.LoadAsset<Material>("APTrapNotificationMaterial");
                ItemNotification = AssetBundle.LoadAsset<GameObject>("ItemNotification");
                HintNotification = AssetBundle.LoadAsset<GameObject>("HintNotification");
                TrapHome = AssetBundle.LoadAsset<GameObject>("TrapHome");
                TrapWide = AssetBundle.LoadAsset<GameObject>("TrapWideHappy");
                TrapJumpingJacks = AssetBundle.LoadAsset<GameObject>("TrapJumpingJacks");
                TrapPhoneCall = AssetBundle.LoadAsset<GameObject>("TrapPhoneCall");
                HomeTrapSprite = AssetBundle.LoadAsset<Sprite>("TrainHome");
                WideTrapSprite = AssetBundle.LoadAsset<Sprite>("WideTrap");
                JumpingJacksTrapSprite = AssetBundle.LoadAsset<Sprite>("JumpingJacksTrap");
                PhoneCallTrapSprite = AssetBundle.LoadAsset<Sprite>("NikoPhone");
                TinyTrapSprite = AssetBundle.LoadAsset<Sprite>("TinyTrap");
                SparksParticleSystem = AssetBundle.LoadAsset<GameObject>("Sparks");
                PartyTicketSprite = AssetBundle.LoadAsset<Sprite>("PartyInvitation");
                BonkHelmetSprite = AssetBundle.LoadAsset<Sprite>("BonkHelmet");
                BugNetSprite = AssetBundle.LoadAsset<Sprite>("BugNet");
                SodaRepairSprite = AssetBundle.LoadAsset<Sprite>("SodaCanRepair");
                ParasolRepairSprite = AssetBundle.LoadAsset<Sprite>("ParasolRepair");
                SwimCourseSprite = AssetBundle.LoadAsset<Sprite>("SwimCourse");
                TextboxItemSprite = AssetBundle.LoadAsset<Sprite>("TextboxItem");
                ACRepairSprite = AssetBundle.LoadAsset<Sprite>("ACRepair");
                TrapTiny = AssetBundle.LoadAsset<GameObject>("TrapTiny");
                NoticeBonkHelmet = AssetBundle.LoadAsset<GameObject>("NoticeBonkHelmet");
                NoticeSodaCan = AssetBundle.LoadAsset<GameObject>("NoticeSodaCan");
                NoticeParasol = AssetBundle.LoadAsset<GameObject>("NoticeParasol");
                NoticeSwimCourse = AssetBundle.LoadAsset<GameObject>("NoticeSwimCourse");
                NoticeAC = AssetBundle.LoadAsset<GameObject>("NoticeAC");
                DeathLinkSprite = AssetBundle.LoadAsset<Sprite>("DeathlinkSprite");
                DeathLinkNotice = AssetBundle.LoadAsset<GameObject>("NoticeDeathLink");
                NoticeBugNet = AssetBundle.LoadAsset<GameObject>("NoticeBugNet");
                NoticePartyTicket = AssetBundle.LoadAsset<GameObject>("NoticePartyTicket");
                AppleBasketSprite = AssetBundle.LoadAsset<Sprite>("AppleBasket");
                HairballBoneSprite = AssetBundle.LoadAsset<Sprite>("HairballBone");
                TurbineBoneSprite = AssetBundle.LoadAsset<Sprite>("TurbineBone");
                SalmonBoneSprite = AssetBundle.LoadAsset<Sprite>("SalmonBone");
                PoolBoneSprite = AssetBundle.LoadAsset<Sprite>("PoolBone");
                BathBoneSprite = AssetBundle.LoadAsset<Sprite>("BathhouseBone");
                TadpoleBoneSprite = AssetBundle.LoadAsset<Sprite>("TadpoleBone");
                NoticeAppleBasket = AssetBundle.LoadAsset<GameObject>("NoticeAppleBasket");
                TrapFast = AssetBundle.LoadAsset<GameObject>("TrapFast");
                _canLogin = true;
            }
            var gameObjectChecker = new GameObject("GameObjectChecker");
            gameObjectChecker.AddComponent<GameObjectChecker>();
            NotifcationCanvas = new GameObject("NotificationCanvas");
            var notecanva = NotifcationCanvas.AddComponent<Canvas>();
            var scaler = NotifcationCanvas.AddComponent<CanvasScaler>();
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.matchWidthOrHeight = 1;
            notecanva.renderMode = RenderMode.ScreenSpaceOverlay;
            notecanva.sortingOrder = 9;
            AddNotificationManager(NotifcationCanvas.transform);
            AddTrapManager(NotifcationCanvas.transform);
            // ArrowTrackerGameObject = new GameObject("APArrowTracker");
            // ArrowTrackerGameObject.AddComponent<ArrowTrackerManager>();
            // DontDestroyOnLoad(ArrowTrackerGameObject);
            DontDestroyOnLoad(NotifcationCanvas);
            DontDestroyOnLoad(gameObjectChecker);
            harmony = new Harmony(PluginGuid);
            harmony.PatchAll();
            SceneManager.sceneLoaded += OnSceneLoaded;
            Logger.LogInfo("Plugin loaded and Harmony patches applied initially!");
            StartCoroutine(CheckForUpdate());
            var toggleSpeed = new InputAction("SpeedToggle", binding: "<Keyboard>/f5");
            // toggleSpeed.AddBinding("<Gamepad>/start");
            // toggleSpeed.AddBinding("<Gamepad>/select");
            // toggleSpeed.AddBinding("<Gamepad>/leftStick");
            toggleSpeed.Enable();

            toggleSpeed.performed += ctx =>
            {
                ToggleSpeed();
            };
        }

        public static void ToggleSpeed()
        {
            MovementSpeed.IsSpeedOn = !MovementSpeed.IsSpeedOn;
            if (!MovementSpeed.IsSpeedOn)
            {
                MyCharacterController.instance.DiveSpeed = 16f;
                MyCharacterController.instance.MaxAirMoveSpeed = 8f;
                MyCharacterController.instance.JumpSpeed = 13f;
                MyCharacterController.instance.DiveCancelHopSpeed = 11f;
                MyCharacterController.instance.MaxStableMoveSpeed = 8f;
                MyCharacterController.instance.MaxWaterMoveSpeed = 11f;
                APSendNote("Disabled Speed Boost", 2f, SpeedBoostSprite);
            }
            else
            {
                APSendNote("Enabled Speed Boost", 2f, SpeedBoostSprite);
                MovementSpeed.MovementSpeedMultiplier();
            }
            BepinLogger.LogInfo("Toggled Speedbost!");
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
        
        private void AddNotificationManager(Transform parent)
        {
            var noteObject = AssetBundle.LoadAsset<GameObject>("APItemNotification");
            var noteUI = Instantiate(noteObject, parent, false);
            noteUI.layer = LayerMask.NameToLayer("UI");
            var apNotificationManager = noteUI.transform.Find("APNotificationManager").gameObject;
            var notificationManager = apNotificationManager.AddComponent<NotificationManager>();
            notificationManager.enabled = true;
        }
        
        private void AddTrapManager(Transform parent)
        {
            var trapObject = AssetBundle.LoadAsset<GameObject>("APTimerTrap");
            var trapUI = Instantiate(trapObject, parent, false);
            trapUI.layer = LayerMask.NameToLayer("UI");
            var apTrapManager = trapUI.transform.Find("APTrapManager").gameObject;
            var trapManager = apTrapManager.AddComponent<TrapManager>();
            trapManager.enabled = true;
            trapUI.transform.position = new Vector3(102f, 1001f, 0);
            trapUI.transform.localScale = new Vector3(0.85f, 0.85f, 1);
        }
        
        private IEnumerator CheckGameSaveManager()
        {
            Logger.LogError("GameSaveManager is null.");
            yield return new WaitUntil(() =>scrGameSaveManager.instance);
            Logger.LogInfo("GameSaveManager is not null.");
            gameSaveManager = scrGameSaveManager.instance;
            saveReady = true;
        }

        private IEnumerator CheckWorldSaveManager()
        {
            Logger.LogError("WorldSaveDataContainer is null.");
            yield return new WaitUntil(() =>scrWorldSaveDataContainer.instance);
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
                    //LogFlags();
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
                    if (SaveEstablished && !scrTrainManager.instance.isLoadingNewScene) // This should fix Home Trap sending unchecked locations in Home
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
                    Logger.LogInfo("\n"+GameObjectChecker.LogFlags);
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
                var duration = 5f;
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
                while (duration > 0)
                {
                    yield return null;
                    duration -= Time.deltaTime;
                }
                annoy = false;
            }
        }
        
        private static IEnumerator SyncState()
        {
            var duration = 4f;
            while (duration > 0)
            {
                yield return null;
                duration -= Time.deltaTime;
            }
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
                if (ArchipelagoData.Options.Keylevels)
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
                if (ArchipelagoData.Options.Fishsanity == ArchipelagoOptions.InsanityLevel.Insanity)
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
                if (ArchipelagoData.Options.Seedsanity == ArchipelagoOptions.InsanityLevel.Insanity)
                {
                    SyncValue(ref ItemHandler.HairballSeedAmount, ArchipelagoClient.HcSeedAmount);
                    SyncValue(ref ItemHandler.SalmonSeedAmount, ArchipelagoClient.SfcSeedAmount);
                    SyncValue(ref ItemHandler.BathSeedAmount, ArchipelagoClient.BathSeedAmount);
                }
            }
            if (ArchipelagoData.slotData.ContainsKey("flowersanity"))
            {
                if (ArchipelagoData.Options.Flowersanity == ArchipelagoOptions.InsanityLevel.Insanity)
                {
                    SyncValue(ref ItemHandler.HairballFlowerAmount, ArchipelagoClient.HcFlowerAmount);
                    SyncValue(ref ItemHandler.TurbineFlowerAmount, ArchipelagoClient.TtFlowerAmount);
                    SyncValue(ref ItemHandler.SalmonFlowerAmount, ArchipelagoClient.SfcFlowerAmount);
                    SyncValue(ref ItemHandler.PoolFlowerAmount, ArchipelagoClient.PpFlowerAmount);
                    SyncValue(ref ItemHandler.BathFlowerAmount, ArchipelagoClient.BathFlowerAmount);
                    SyncValue(ref ItemHandler.TadpoleFlowerAmount, ArchipelagoClient.HqFlowerAmount);
                }
            }
            if (ArchipelagoData.slotData.ContainsKey("bonesanity"))
            {
                if (ArchipelagoData.Options.Bonesanity == ArchipelagoOptions.InsanityLevel.Insanity)
                {
                    SyncValue(ref ItemHandler.HairballBoneAmount, ArchipelagoClient.HcBoneAmount);
                    SyncValue(ref ItemHandler.TurbineBoneAmount, ArchipelagoClient.TtBoneAmount);
                    SyncValue(ref ItemHandler.SalmonBoneAmount, ArchipelagoClient.SfcBoneAmount);
                    SyncValue(ref ItemHandler.PoolBoneAmount, ArchipelagoClient.PpBoneAmount);
                    SyncValue(ref ItemHandler.BathBoneAmount, ArchipelagoClient.BathBoneAmount);
                    SyncValue(ref ItemHandler.TadpoleBoneAmount, ArchipelagoClient.HqBoneAmount);
                }
            }
            if (ArchipelagoData.Options.Cassette == ArchipelagoOptions.CassetteMode.LevelBased)
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
            if (ArchipelagoClient.InvalidSceneItemQueue.Count <= 0 || !ArchipelagoClient.IsValidScene())
            {
                foreach (var t in ArchipelagoClient.InvalidSceneItemQueue)
                {
                    ArchipelagoClient.GiveItem(t, false);
                }
                ArchipelagoClient.InvalidSceneItemQueue.Clear();
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

        public void LogFlags()
        {
            GameObjectChecker.LogFlags.Clear();
            saveDataCoinFlag.ForEach(flag => GameObjectChecker.LogFlags.AppendLine($"CoinFlag: {flag}"));
            saveDataCassetteFlag.ForEach(flag => GameObjectChecker.LogFlags.AppendLine($"CassetteFlag: {flag}"));
            saveDataFishFlag.ForEach(flag => GameObjectChecker.LogFlags.AppendLine($"FishFlag: {flag}"));
            saveDataMiscFlag.ForEach(flag => GameObjectChecker.LogFlags.AppendLine($"MiscFlag: {flag}"));
            saveDataLetterFlag.ForEach(flag => GameObjectChecker.LogFlags.AppendLine($"LetterFlag: {flag}"));
            saveDataGeneralFlag.ForEach(flag => GameObjectChecker.LogFlags.AppendLine($"GeneralFlag: {flag}"));
            Logger.LogInfo(GameObjectChecker.LogFlags);
            GameObjectChecker.LogFlags.Clear();
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
        }

        private void CheckNewFlag(ref int flagIndex, List<string> flagList, string flagType)
        {
            if (flagList.Count > flagIndex)
            {
                GameObjectChecker.LogFlags.AppendLine($"New {flagType} Flag! '{flagList[flagIndex]}'");
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
                //KillPlayer("GETTT DEATHLINKED HEEHEH");
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
                TrapManager.instance.ActivateTrap("Home", 30f);
            }
            if (GUI.Button(new Rect(16, 400, 100, 20), "Iron Boots Trap"))
            {
                TrapManager.instance.ActivateTrap("W I D E", 30f);
            }
            if (GUI.Button(new Rect(16, 420, 100, 20), "My Turn! Trap"))
            {
                TrapManager.instance.ActivateTrap("Jumping Jacks", 30f);
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
                }
                foreach (var keyValuePair in Bugsanity.bugIDs)
                {
                    if (keyValuePair.Key != null)
                    {
                        Logger.LogFatal($"Bugs: {keyValuePair.Key} - {keyValuePair.Value} | Flag: Bug{keyValuePair.Value} " +
                                        $"| ID: {keyValuePair.Value} | Active: {keyValuePair.Key.gameObject.activeInHierarchy}");
                    }
                }
            }
            if (GUI.Button(new Rect(16, 500, 100, 20), "TestNote1"))
            {
                var notification = new APNotification(false, "TestItem", "YourMom", "XXXGamerHDXXX", "THE MOOOOON", 
                    ItemFlags.None, 5f, null, null, FishSprite);
                NotificationManager.AddNewNotification.Enqueue(notification);
            }
            if (GUI.Button(new Rect(16, 520, 100, 20), "TestNote2"))
            {
                var notification = new APNotification(false, "Crazy Shiny Item", "nieli", "XXXGamerHDXXX", "Hairball City - Handsome Frog (Chatsanity)", 
                    ItemFlags.Advancement, 5f, null, null, CoinSprite);
                NotificationManager.AddNewNotification.Enqueue(notification);
            }
            if (GUI.Button(new Rect(16, 540, 100, 20), "TestNote3"))
            {
                var notification = new APNotification(true, "Crazy Shiny Item", "nieli", "XXXGamerHDXXX", "Hairball City - Handsome Frog (Chatsanity)", 
                    ItemFlags.Trap, 5f, null, null, WhoopsTrapSprite, "Not Found");
                NotificationManager.AddNewNotification.Enqueue(notification);
            }
            TrapManager.DebugPhone = Convert.ToInt32(GUI.TextField(new Rect(180, 560, 80, 20), TrapManager.DebugPhone.ToString()));
            TrapManager.DebugPhoneBool = GUI.Toggle(new Rect(120, 560, 80, 20), TrapManager.DebugPhoneBool, "Debug") ;
            if (GUI.Button(new Rect(16, 560, 100, 20), "MsgLength"))
            {
                TrapManager.PhoneOn = true;
            }
            if (GUI.Button(new Rect(16, 580, 100, 20), "TrapLink"))
            {
                ArchipelagoClient.ToggleTrapLink();
            }
            if (GUI.Button(new Rect(16, 600, 100, 20), "TimeTrap"))
            {
                TrapManager.FastTimeOn = true;
            }
            if (GUI.Button(new Rect(16, 620, 100, 20), "Unstuck"))
            {
                MyCharacterController.instance.BackToDaisy();
            }
            if (GUI.Button(new Rect(16, 640, 100, 20), "ALL CONV"))
            {
                var triggers = FindObjectsByType<scrTextboxTrigger>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
                foreach (var textbox in triggers)
                {
                    string parent;
                    if (textbox.transform.parent) parent = textbox.transform.parent.name;
                    else parent = "--";
                    Logger.LogWarning($"Conversation: {textbox.conversation} | Parent: {parent} | Root: {textbox.transform.root.name}");
                }
            }
            if (GUI.Button(new Rect(16, 660, 100, 20), "Bugs"))
            {
                var bugIDs = Bugsanity.bugIDs;
                foreach (var bug in bugIDs)
                {
                    Logger.LogWarning($"BugID: {bug.Value} | Vector3: {bug.Key.transform.position}");
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
