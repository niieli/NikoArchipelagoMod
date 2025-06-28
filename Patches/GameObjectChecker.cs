using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using NikoArchipelago.Trackers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace NikoArchipelago.Patches;

public class GameObjectChecker : MonoBehaviour
{
    private static GameObject _garyGhost, _garyGhostHome;
    private static bool _foundGhost;
    private static bool _foundCamera;
    public static bool FirstMeeting, NoticeStillUp, ChatsanityOn;
    private static bool _checkedGhost;
    private static bool _spawned;
    private static bool _foundNpcs;
    private static bool _missingFrog;
    public static GameObject APMenu;
    private static GameObject WarningNotice;
    public static scrCursor cursor;
    private static bool _sentNote, _sentNote2, _sentNote3, _sentNote4, _sentNote5, 
        _sentNote6, _sentNote7, _sentNote8, _sentNote9, _sentNote10, _sentNote11, _sentNote12, _sentNote13, _sentNote14,
        _sentNote15, _sentNote16, _sentNote17, _sentNote18, _sentNote19, _hatKidFix, oncePerScene, dissmised, onLogin;
    public static readonly HashSet<int> LoggedInstances = [];
    public static readonly Dictionary<string, GameObject> CreatedItemsCache = new();
    public static readonly StringBuilder LogBatch = new();
    public static readonly StringBuilder LogPastItemsBatch = new();
    public static readonly StringBuilder LogFlags = new();
    public static string PreviousScene = "";
    public static bool IsVisible = true;
    private bool startedTimer = false;
    private Coroutine visibilityTimer;
    public static bool IsHamsterball;
    private static bool _turnedOff, _turnedOn;
    private static Gamepad gamepad;
    private static bool toggleSpeedBoost;

    private static List<string> _hatPlayerNames = [];
    private void Start()
    {
        Plugin.BepinLogger.LogDebug("GameObjectChecker started!");
        SceneManager.sceneLoaded += OnSceneLoaded;
        onLogin = false;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        IsHamsterball = false;
        MenuHelpers.Menus.Clear();
        StartCoroutine(ModelLogging());
        if (visibilityTimer != null) StopCoroutine(visibilityTimer);
        visibilityTimer = StartCoroutine(VisibilityTimer());
        FirstMeeting = false;
        _checkedGhost = false;
        _spawned = false;
        _foundCamera = false;
        _foundNpcs = false;
        _missingFrog = false;
        _sentNote = _sentNote2 = _sentNote3 = _sentNote4 = _sentNote5 =  
            _sentNote6 = _sentNote7 = _sentNote8 = _sentNote9 = _sentNote10 = _sentNote11 = _sentNote12 = _sentNote13 = _sentNote14 = 
                _sentNote15 = _sentNote16 = _sentNote17 = _sentNote18 = _sentNote19 = oncePerScene = false;
        Applesanity.ApplesanityStart.appleIDs.Clear();
        Applesanity.ApplesanityStart.nextAppleID = 1;
        Bugsanity.bugIDs.Clear();
        Bugsanity.nextBugID = 1;
        Bonesanity.ID = 0;
        ParasolPatch.NoticeUp = SwimCourse.NoticeUp = SodaCanPatch.NoticeUp = GustPatch.NoticeUp = 
                Bugsanity.NoticeUp = BonkHelmet.NoticeUp = Applesanity.NoticeUp = false;
        ClearNotices();
        //LocationCheck();
        MitchAndMaiObject();
        PepperFirstMeetingTrigger();
        TitleScreenObject();
        InstantiateAPMenu();
        TrackerKiosk();
        TrackerTicket();
        TrackerKey();
        TrackerCassette();
        HqWhiteboard();
        HqGarden();
        SpawnGaryHome();
        AssignDisplayers();
        NpcController();
        Statistics();
        StatisticsWhiteboard();
        MovementSpeed.MovementSpeedMultiplier();
        FixApplePlacement();
        HatPlayers();
        AddSwimCourse();
        AddTextboxPermit();
        StartCoroutine(AddTheRealSkippy());
        //Instantiate(Plugin.BasicBlock, GameObject.Find("Quests").transform);
        if (SceneManager.GetActiveScene().name == "GarysGarden")
            scrScissor.destroyAll = false;
        if (ArchipelagoClient.IsValidScene())
        {
            cursor = GameObject.Find("UI/Menu system/Cursor").GetComponent<scrCursor>();
        }
        //APArrowTracker();
        if (Plugin.newFile && SceneManager.GetActiveScene().name != "Home")
        {
            Plugin.newFile = false;
        }
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private static void LocationCheck()
    {
        int count = 0;
        GameObject[] allObjects = FindObjectsOfType<GameObject>() ;
        foreach (var go in allObjects)
        {
            if (go.tag != "Apple"
                && (go.transform.parent == null // Verhindert NullReferenceException
                    || (go.transform.parent.gameObject.name != "Quests"
                        && go.transform.parent.gameObject.name != "Cassettes"
                        && go.transform.parent.gameObject.name != "Collectables"
                        && go.transform.parent.gameObject.name != "NPCs"))) continue;
            Plugin.BepinLogger.LogFatal($"{count} | Object Name: {go.name} | " 
                                        + $"Object Position: {go.transform.position} | "
                                        //+ $"Object Type: {go.GetType()} | " 
                                        + $"Object InstanceID: {go.GetInstanceID()} |");
            count++;
        }
    }

    private static void AddSwimCourse()
    {
        if (ArchipelagoData.slotData == null) return;
        if (!ArchipelagoData.slotData.ContainsKey("swimming")) return;
        if (int.Parse(ArchipelagoData.slotData["swimming"].ToString()) == 0) return;
        if (!ArchipelagoClient.IsValidScene()) return;
        MyCharacterController.instance.gameObject.AddComponent<SwimCourse>();
    }
    
    private static void AddTextboxPermit()
    {
        if (ArchipelagoData.slotData == null) return;
        if (!ArchipelagoData.slotData.ContainsKey("textbox")) return;
        if (int.Parse(ArchipelagoData.slotData["textbox"].ToString()) == 0) return;
        if (!ArchipelagoClient.IsValidScene()) return;
        var obj = new GameObject("TextboxPermit");
        obj.AddComponent<TextboxPermit>();
    }
    
    private static IEnumerator AddTheRealSkippy()
    {
        if (SceneManager.GetActiveScene().name != "Salmon Creek Forest") yield break;
        yield return new WaitUntil(() => scrGameSaveManager.instance.gameData.worldsData[5].coinFlags.Contains("main"));
        var t = GameObject.Find("NPCs/Poppy and fam/Turn on").transform;
        t.GetChild(5).gameObject.SetActive(true);
        t.GetChild(7).gameObject.SetActive(false);
    }

    private static IEnumerator VisibilityTimer()
    {
        if (ArchipelagoData.slotData == null) yield break;
        if (!ArchipelagoData.slotData.ContainsKey("soda_cans")) yield break;
        
        if (int.Parse(ArchipelagoData.slotData["soda_cans"].ToString()) == 0 
            && int.Parse(ArchipelagoData.slotData["parasols"].ToString()) == 0) yield break;
        
        if (!ArchipelagoClient.IsValidScene()) yield break;
        
        var timer = 0f;
        float switchTimer = 1.25f;
        while (!ArchipelagoClient.ParasolRepairAcquired || !ArchipelagoClient.SodaRepairAcquired)
        {
            timer += Time.deltaTime;
            if (timer >= switchTimer)
            {
                timer = 0f;
                IsVisible = !IsVisible;
            }
            yield return null;
        }
    }

    private static void ClearNotices()
    {
        foreach (Transform child in Plugin.NotifcationCanvas.transform)
        {
            if (child.name.StartsWith("Notice"))
            {
                Destroy(child.gameObject);
            }
        }
    }
    
    private static void MitchAndMaiObject()
    {
        try
        {
            GameObject cassetteBuyer;
            cassetteBuyer = GameObject.Find(SceneManager.GetActiveScene().name == "GarysGarden" ? "CassetteBuyerMitch" : "CassetteBuyer");
            if (cassetteBuyer != null && cassetteBuyer.GetComponent<scrCassetteBuyer>() != null)
            {
                CassetteCost.MitchGameObject = cassetteBuyer.GetComponent<scrCassetteBuyer>();
                Plugin.BepinLogger.LogInfo("Mitch GameObject found!");
            }
            else
            {
                Plugin.BepinLogger.LogInfo("Mitch GameObject does not exist!");
            }
        }
        catch (NullReferenceException e)
        {
            Plugin.BepinLogger.LogError($"Error finding 'CassetteBuyer': {e.Message}");
        }
            
        try
        {
            GameObject cassetteBuyer2;
            cassetteBuyer2 = GameObject.Find(SceneManager.GetActiveScene().name == "GarysGarden" ? "CassetteBuyerMai" : "CassetteBuyer2");
            if (cassetteBuyer2 != null && cassetteBuyer2.GetComponent<scrCassetteBuyer>() != null)
            {
                CassetteCost.MaiGameObject = cassetteBuyer2.GetComponent<scrCassetteBuyer>();
                Plugin.BepinLogger.LogInfo("Mai GameObject found!");
            }
            else
            {
                Plugin.BepinLogger.LogInfo("Mai GameObject does not exist!");
            }
        }
        catch (NullReferenceException e)
        {
            Plugin.BepinLogger.LogError($"Error finding 'CassetteBuyer2': {e.Message}");
        }
    }

    private static void PepperFirstMeetingTrigger()
    {
        if (SceneManager.GetActiveScene().name != "Home") return;
        try
        {
            if (GameObject.Find("Pepper Meeting Trigger") != null)
            {
                Plugin.BepinLogger.LogInfo("Pepper Meeting Trigger GameObject found!");
                FirstMeeting = true;
            }
            else
            {
                Plugin.BepinLogger.LogInfo("Pepper Meeting Trigger GameObject does not exist!");
            }
        }
        catch (NullReferenceException e)
        {
            Plugin.BepinLogger.LogError($"Error finding 'Pepper Meeting Trigger': {e.Message}");
        }
    }

    private static void TitleScreenObject()
    {
        try
        {
            var titleScreen = GameObject.Find("Title Screen");
            if (titleScreen == null) return;
            titleScreen.GetComponent<Image>().sprite = Plugin.APLogoSprite;
            var actionScreen = GameObject.Find("ActionButton Title Screen");
            var textRef = actionScreen.transform.Find("text").GetComponent<TextMeshProUGUI>();
            FirstTimeNotice.TitleScreen = actionScreen;
            FirstTimeNotice.TextReference = textRef;
            // var text = actionScreen.transform.Find("Settings Title Screen/text").GetComponent<TextMeshProUGUI>();
            // text.text = "Settings &";
            FirstTimeNotice.TitleScreenAPLogo();
            Plugin.BepinLogger.LogInfo("Title Screen GameObject found!");
        }
        catch (NullReferenceException e)
        {
            Plugin.BepinLogger.LogError($"Error finding 'Title Screen': {e.Message}");
        }
    }

    private static void InstantiateAPMenu()
    {
        if (!ArchipelagoClient.IsValidScene()) return;
        var apUIGameObject = Plugin.ChristmasEvent ? Plugin.AssetBundleXmas.LoadAsset<GameObject>("APMenuXmasTheme") : Plugin.AssetBundle.LoadAsset<GameObject>("APMenuObjectTest1");
        APMenu = Instantiate(apUIGameObject, GameObject.Find("UI").transform, false);
        if (APMenu == null)
        {
            Plugin.BepinLogger.LogError("Failed to instantiate ApUIGameObject prefab.");
            return;
        }
        APMenu.layer = LayerMask.NameToLayer("UI");
        var manager = APMenu.transform.Find("APMenuManager")?.gameObject;
        if (manager == null)
        {
            Plugin.BepinLogger.LogError("APMenuManager not found in the prefab.");
            return;
        }
        var menu = manager.AddComponent<ArchipelagoMenu>();
        if (menu == null)
        {
            Plugin.BepinLogger.LogError("Failed to add ArchipelagoMenu component to APMenuManager.");
            return;
        }
        APMenu.SetActive(false);
        menu.enabled = true;
    }
    
    private static void TrackerTicket()
    {
        if (!ArchipelagoClient.IsValidScene()) return;
        var apTrackerUI = Plugin.AssetBundle.LoadAsset<GameObject>("APTrackerTicket");
        var ticketPrefab = Instantiate(apTrackerUI, GameObject.Find("UI").transform, false);
        if (ticketPrefab == null)
        {
            Plugin.BepinLogger.LogError("Failed to instantiate apTrackerUI prefab.");
            return;
        }
        ticketPrefab.layer = LayerMask.NameToLayer("UI");
        ticketPrefab.transform.SetSiblingIndex(23);
        var manager = ticketPrefab.transform.Find("APTicketManager")?.gameObject;
        if (manager == null)
        {
            Plugin.BepinLogger.LogError("APTicketManager not found in the prefab.");
            return;
        }
        var tracker = manager.AddComponent<TrackerTicket>();
        if (tracker == null)
        {
            Plugin.BepinLogger.LogError("Failed to add TrackerTicket component to APTicketManager.");
            return;
        }
        tracker.enabled = true;
    }
    
    private static void TrackerKiosk()
    {
        if (!ArchipelagoClient.IsValidScene()) return;
        var apTrackerUI = Plugin.AssetBundle.LoadAsset<GameObject>("APTrackerKiosk");
        var kioskPrefab = Instantiate(apTrackerUI, GameObject.Find("UI").transform, false);
        if (kioskPrefab == null)
        {
            Plugin.BepinLogger.LogError("Failed to instantiate apTrackerUI prefab.");
            return;
        }
        kioskPrefab.layer = LayerMask.NameToLayer("UI");
        kioskPrefab.transform.SetSiblingIndex(23);
        var manager = kioskPrefab.transform.Find("APKioskManager")?.gameObject;
        if (manager == null)
        {
            Plugin.BepinLogger.LogError("APKioskManager not found in the prefab.");
            return;
        }
        var tracker = manager.AddComponent<TrackerKiosk>();
        if (tracker == null)
        {
            Plugin.BepinLogger.LogError("Failed to add TrackerKiosk component to APKioskManager.");
            return;
        }
        tracker.enabled = true;
    }

    private static void TrackerKey()
    {
        if (!ArchipelagoClient.IsValidScene()) return;
        var apTrackerUI = new GameObject("APTrackerKey");
        var tracker = apTrackerUI.AddComponent<TrackerKeys>();
        if (tracker == null)
        {
            Plugin.BepinLogger.LogError("Failed to add TrackerKeys component to APKeyManager.");
            return;
        }
        tracker.enabled = true;
    }
    
    private static void TrackerCassette()
    {
        if (!ArchipelagoClient.IsValidScene()) return;
        var apTrackerUI = new GameObject("APTrackerCassette");
        var tracker = apTrackerUI.AddComponent<TrackerCassettes>();
        if (tracker == null)
        {
            Plugin.BepinLogger.LogError("Failed to add TrackerCassettes component to APTrackerCassette.");
            return;
        }
        tracker.enabled = true;
    }
    
    private static void Statistics()
    {
        if (!ArchipelagoClient.IsValidScene()) return;
        var mapStatsApples = Plugin.AssetBundle.LoadAsset<GameObject>("Statsapples");
        var mapStatsFish = Plugin.AssetBundle.LoadAsset<GameObject>("Statsfish");
        var mapStatsFlowers = Plugin.AssetBundle.LoadAsset<GameObject>("Statsflowers");
        var mapStatsSeeds = Plugin.AssetBundle.LoadAsset<GameObject>("Statsseeds");
        var mapStatsLocations = Plugin.AssetBundle.LoadAsset<GameObject>("Statslocations");
        var mapStatsSnailShop = Plugin.AssetBundle.LoadAsset<GameObject>("Statssnailshop");
        var mapStatschatsanity = Plugin.AssetBundle.LoadAsset<GameObject>("Statschatsanity");
        var mapStatsbugsanity = Plugin.AssetBundle.LoadAsset<GameObject>("Statsbugsanity");
        var mapStatsbonesanity = Plugin.AssetBundle.LoadAsset<GameObject>("Statsbonesanity");
        var mapStatsthoughtsanity = Plugin.AssetBundle.LoadAsset<GameObject>("Statsthoughtsanity");
        var apples = Instantiate(mapStatsApples, GameObject.Find("Statistics").transform, false);
        Instantiate(mapStatsFish, GameObject.Find("Statistics").transform, false);
        Instantiate(mapStatsFlowers, GameObject.Find("Statistics").transform, false);
        Instantiate(mapStatsSeeds, GameObject.Find("Statistics").transform, false);
        var chatsanity = Instantiate(mapStatschatsanity, GameObject.Find("Statistics").transform, false);
        var snailshop = Instantiate(mapStatsSnailShop, GameObject.Find("Statistics").transform, false);
        var achievements = Instantiate(mapStatsLocations, GameObject.Find("Statistics").transform, false);
        var thoughtsanity = Instantiate(mapStatsthoughtsanity, GameObject.Find("Statistics").transform, false);
        var bugsanity = Instantiate(mapStatsbugsanity, GameObject.Find("Statistics").transform, false);
        var bonesanity = Instantiate(mapStatsbonesanity, GameObject.Find("Statistics").transform, false);
        //apples.transform.localPosition = new Vector3(-100f, -176f, 0f);
        snailshop.transform.localPosition = new Vector3(-432f, -387f, 0f);
        snailshop.transform.localScale = new Vector3(1f, 1f, 1f);
        achievements.transform.localPosition = new Vector3(-152.8f, -387f, 0f);
        chatsanity.transform.localPosition = new Vector3(452f, -316f, 0f);
        bugsanity.transform.localPosition = new Vector3(211f, -176f, 0f);
        thoughtsanity.transform.localPosition = new Vector3(418f, -176.7f, 0f);
        Plugin.BepinLogger.LogInfo("Added new stats to Statistics.");
    }
    
    private static void StatisticsWhiteboard()
    {
        if (!ArchipelagoClient.IsValidScene()) return;
        if (GameObject.Find("Pepper/Whiteboard/Canvas/Statistics") == null) return;
        var mapStatsApples = Plugin.AssetBundle.LoadAsset<GameObject>("StatsapplesBoard");
        var mapStatsFish = Plugin.AssetBundle.LoadAsset<GameObject>("StatsfishBoard");
        var mapStatsFlowers = Plugin.AssetBundle.LoadAsset<GameObject>("StatsflowersBoard");
        var mapStatsSeeds = Plugin.AssetBundle.LoadAsset<GameObject>("StatsseedsBoard");
        var mapStatsLocations = Plugin.AssetBundle.LoadAsset<GameObject>("StatslocationsBoard");
        var mapStatsSnailShop = Plugin.AssetBundle.LoadAsset<GameObject>("StatssnailshopBoard");
        var mapStatschatsanity = Plugin.AssetBundle.LoadAsset<GameObject>("StatschatsanityBoard");
        var mapStatsbugsanity = Plugin.AssetBundle.LoadAsset<GameObject>("StatsbugsanityBoard");
        var mapStatsbonesanity = Plugin.AssetBundle.LoadAsset<GameObject>("StatsbonesanityBoard");
        var mapStatsthoughtsanity = Plugin.AssetBundle.LoadAsset<GameObject>("StatsthoughtsanityBoard");
        var apple = Instantiate(mapStatsApples, GameObject.Find("Pepper/Whiteboard/Canvas/Statistics").transform, false);
        var fish = Instantiate(mapStatsFish, GameObject.Find("Pepper/Whiteboard/Canvas/Statistics").transform, false);
        var flower = Instantiate(mapStatsFlowers, GameObject.Find("Pepper/Whiteboard/Canvas/Statistics").transform, false);
        var seed = Instantiate(mapStatsSeeds, GameObject.Find("Pepper/Whiteboard/Canvas/Statistics").transform, false);
        var location = Instantiate(mapStatsLocations, GameObject.Find("Pepper/Whiteboard/Canvas/Statistics").transform, false);
        var snailShop = Instantiate(mapStatsSnailShop, GameObject.Find("Pepper/Whiteboard/Canvas/Statistics").transform, false);
        var chatsanity = Instantiate(mapStatschatsanity, GameObject.Find("Pepper/Whiteboard/Canvas/Statistics").transform, false);
        var thoughtsanity = Instantiate(mapStatsthoughtsanity, GameObject.Find("Pepper/Whiteboard/Canvas/Statistics").transform, false);
        var bugsanity = Instantiate(mapStatsbugsanity, GameObject.Find("Pepper/Whiteboard/Canvas/Statistics").transform, false);
        var bonesanity = Instantiate(mapStatsbonesanity, GameObject.Find("Pepper/Whiteboard/Canvas/Statistics").transform, false);

        apple.transform.localPosition = new Vector3(95f, -100f, 0f);
        apple.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        fish.transform.localPosition = new Vector3(95f, -150f, 0f);
        flower.transform.localPosition = new Vector3(95f, -204f, 0f);
        flower.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        seed.transform.localPosition = new Vector3(95f, -265f, 0f);
        seed.transform.Find("Image").transform.localRotation = Quaternion.Euler(0f, 0f, 331f);
        location.transform.localPosition = new Vector3(-230f, -456f, 0f);
        location.transform.localScale = new Vector3(0.8166f, 0.8166f, 1.0166f);
        snailShop.transform.localPosition = new Vector3(-54f, -456f, 0f);
        snailShop.transform.localScale = new Vector3(0.8166f, 0.8166f, 1.0166f);
        chatsanity.transform.localPosition = new Vector3(-118f, -386f, 0f);
        chatsanity.transform.localScale = new Vector3(0.6648f, 0.6648f, 0.6648f);
        thoughtsanity.transform.localPosition = new Vector3(95f, -325f, 0f);
        thoughtsanity.transform.localScale = new Vector3(0.6f, 0.6f, 0.1f);
        bugsanity.transform.localPosition = new Vector3(-149f, -325f, 0f);
        bugsanity.transform.Find("text").transform.localScale = new Vector3(1f, 1f, 1f);
        bonesanity.transform.localPosition = new Vector3(95f, -386.8f, 0f);
        bonesanity.transform.Find("text").transform.localScale = new Vector3(1f, 1f, 1f);
        if (GameObject.Find("Pepper/Whiteboard/Canvas/Statistics/keys") != null)
        {
            var keys = GameObject.Find("Pepper/Whiteboard/Canvas/Statistics/keys");
            keys.transform.localPosition = new Vector3(-179f, -245f, 0f);
            keys.transform.localScale = new Vector3(0.6158f, 0.6158f, 0.1f);
        }
        var cassettes = GameObject.Find("Pepper/Whiteboard/Canvas/Statistics/cassettes");
        cassettes.transform.localPosition = new Vector3(-431f, -423f, 0f);
        var coins = GameObject.Find("Pepper/Whiteboard/Canvas/Statistics/coins");
        //coins.transform.localPosition = new Vector3(-445f, -111f, 0);
        Plugin.BepinLogger.LogInfo("Added new stats to Whiteboard.");
        if (SceneManager.GetActiveScene().name != "Public Pool") return;
        var canvas = GameObject.Find("Pepper/Whiteboard/Canvas");
        canvas.transform.localPosition = new Vector3(0f, 1.208f, 0.2f);
        Plugin.BepinLogger.LogInfo("Fixed Public Pool Whiteboard.");
    }
    
    private static void NpcController()
    {
        if (ArchipelagoData.slotData == null) return;
        if (!ArchipelagoData.slotData.ContainsKey("chatsanity")) return;
        if (int.Parse(ArchipelagoData.slotData["chatsanity"].ToString()) == 2) Patches.NpcController.IsGlobal = true;
        if (int.Parse(ArchipelagoData.slotData["thoughtsanity"].ToString()) == 1) Patches.NpcController.Thoughtsanity = true;
        if (!ArchipelagoClient.IsValidScene()) return;
        var apTrackerUI = new GameObject("NPCController");
        var tracker = apTrackerUI.AddComponent<NpcController>();
        if (tracker == null)
        {
            Plugin.BepinLogger.LogError("Failed to add NPCController component to NPCController.");
            return;
        }
        tracker.enabled = true;
        ChatsanityOn = true;
    }

    private static void APArrowTracker()
    {
        if (!ArchipelagoClient.IsValidScene()) return;
        var apArrow = Plugin.AssetBundle.LoadAsset<GameObject>("Arrow");
        var arrowUI = Plugin.AssetBundle.LoadAsset<GameObject>("ArrowUI");
        var player = GameObject.Find("PlayerCharacter");
        var arrowPrefab = Instantiate(apArrow, player.transform.Find("CharacterController"), false);
        var arrowUIPrefab = Instantiate(arrowUI, GameObject.Find("UI").transform, false);
        if (arrowPrefab == null)
        {
            Plugin.BepinLogger.LogError("Failed to instantiate apTrackerUI prefab.");
            return;
        }
        arrowPrefab.transform.position = new Vector3(arrowPrefab.transform.position.x, arrowPrefab.transform.position.y+1.5f, arrowPrefab.transform.position.z);
        arrowUIPrefab.layer = LayerMask.NameToLayer("UI");
        // var arrow = player.transform.Find("CharacterController").gameObject.AddComponent<ArrowIndicator>();
        // arrow.player = player.transform.Find("CharacterController");
        // arrow.arrowObject = arrowPrefab.transform;
        // arrow.distanceText = arrowUIPrefab.transform.Find("Distance").GetComponent<TextMeshProUGUI>();
        // arrow.distanceTextShadow = arrowUIPrefab.transform.Find("DistanceShadow").GetComponent<TextMeshProUGUI>();
        // arrow.target = GameObject.Find("Pre Party/FetchQuest/NPCs Reward/NPCReward").transform;
        // arrow.enabled = true;
        // Plugin.ArrowTrackerGameObject.GetComponent<ArrowTrackerManager>().arrowIndicator = arrow;
        Plugin.ArrowTrackerGameObject.GetComponent<ArrowTrackerManager>().arrowIndicator.player = player.transform.Find("CharacterController");
        Plugin.ArrowTrackerGameObject.GetComponent<ArrowTrackerManager>().arrowIndicator.arrow3D = arrowPrefab.transform;
        Plugin.ArrowTrackerGameObject.GetComponent<ArrowTrackerManager>().arrowIndicator.distanceText = arrowUIPrefab.transform.Find("Distance").GetComponent<TextMeshProUGUI>();
        Plugin.ArrowTrackerGameObject.GetComponent<ArrowTrackerManager>().arrowIndicator.distanceTextShadow = arrowUIPrefab.transform.Find("DistanceShadow").GetComponent<TextMeshProUGUI>();
        Plugin.ArrowTrackerGameObject.GetComponent<ArrowTrackerManager>().iconImage = arrowUIPrefab.transform.Find("Icon").GetComponent<Image>();
        
    }

    private static void HqWhiteboard()
    {
        if (SceneManager.GetActiveScene().name != "Tadpole inc") return;
        var whiteboard = GameObject.Find("Pepper/Pepper/Whiteboard");
        var pepper2 = GameObject.Find("Pepper/Pepper/Pepper");
        pepper2.SetActive(false);
        whiteboard.SetActive(true);
        whiteboard.transform.position = new Vector3((float)-61.0821, (float)3.623, (float)-80.2564);
        whiteboard.transform.localPosition = new Vector3((float)-58.2721, (float)-162.1998, (float)-157.5964);
        whiteboard.transform.rotation = Quaternion.Euler(0, (float)31.9692, 0);
        Plugin.BepinLogger.LogInfo("Added Whiteboard to Tadpole HQ");
    }
    
    private static void HqGarden()
    {
        if (ArchipelagoData.slotData == null) return;
        if (Plugin.Compatibility) return;
        if (SceneManager.GetActiveScene().name != "Tadpole inc") return;
        _garyGhost = GameObject.Find("Garys spot").gameObject;
        _garyGhost.AddComponent<GhostActivatorPatch>();
    }

    private static void SpawnGaryHome()
    {
        if (ArchipelagoData.slotData == null) return;
        if (Plugin.Compatibility) return;
        if (SceneManager.GetActiveScene().name != "Home" || _spawned) return;
        var gary = Plugin.AssetBundle.LoadAsset<GameObject>("Gary");
        _garyGhostHome = Instantiate(gary, GameObject.Find("Pre Party").transform, false);
        _garyGhostHome.transform.position = new Vector3((float)-7.9545, (float)2.0436, (float)-2.8);
        _garyGhostHome.transform.localScale = new Vector3((float)0.3091, (float)0.34, (float)-1.6218);
        _garyGhostHome.AddComponent<scrAnimateTextureStrip>();
        var meshRenderer = _garyGhostHome.AddComponent<MeshRenderer>();
        meshRenderer.material = GameObject.Find("BlastFrogTalk").GetComponent<MeshRenderer>().material;
        var texArray = _garyGhostHome.AddComponent<scrAnimateTextureArray>();
        texArray.Bank = GameObject.Find("BlastFrogTalk").GetComponent<scrAnimateTextureArray>().Bank;
        texArray.AnimationStripName = "txrGhost";
        texArray.FrameBegin = 330;
        texArray.FrameDuration = 1;
        texArray.FrameEnd = 332;
        texArray.FrameOffset = 1;
        texArray.FrameTimer = (float)0.787;
        texArray.InitialFrameDuration = 1;
        texArray.Mode = scrAnimateTextureArray.TextureArrayMode.Animate;
        texArray.enabled = true;
        var textboxGameObject = _garyGhostHome.transform.Find("TextboxTrigger")?.gameObject;
        if (textboxGameObject != null)
        {
            var reference = GameObject.Find("BlastFrogTalk/TextboxTrigger").GetComponent<scrTextboxTrigger>();
            var trigger = textboxGameObject.AddComponent<scrTextboxTrigger>();
            trigger.conversation = "GarySender";
            trigger.arrayAnimator = reference.arrayAnimator;
            trigger.animator = reference.animator;
            trigger.effects = reference.effects;
            trigger.voices = reference.voices;
            //trigger.camPoints = reference.camPoints;
            trigger.objIcon = reference.objIcon;
            trigger.eventEffectZero = reference.eventEffectZero;
            trigger.myCameraFollowPoint = reference.myCameraFollowPoint;
            trigger.strIcons = reference.strIcons;
            var collider = textboxGameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            var iconParent  = textboxGameObject.transform.Find("IconParent").gameObject;
            iconParent.AddComponent<scrLookAtCameraFull>();
            var iconTalk = iconParent.transform.Find("sprIconTalk").gameObject;
            iconTalk.AddComponent<scrNPCIcon>();
        }
        _spawned = true;
        _garyGhostHome.SetActive(false);
        Plugin.BepinLogger.LogInfo("Spawned Gary");
    }

    private static void GardenLevelFix()
    {
        if (SceneManager.GetActiveScene().name != "GarysGarden") return;
        var t = GameObject.Find("TrainMap").GetComponent<scrTrainMap>();
        if (!t.mapIsActive && !scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[7])
        {
            t.levelSelected = 0;
        }
    }

    private static void HatKidEasterEgg()
    {
        if (SceneManager.GetActiveScene().name != "Public Pool") return;
        var currentBoxField = AccessTools.Field(typeof(scrTextbox), "currentBox");
        int _currentBox = (int)currentBoxField.GetValue(scrTextbox.instance);
        if (scrTextbox.instance.isOn && scrTextbox.instance.conversation == "hatkid" && !_hatKidFix)
        {
            if (_hatPlayerNames.Count != 0)
            {
                scrTextbox.instance.characterName = _hatPlayerNames[Random.Range(0, _hatPlayerNames.Count)];
                _hatKidFix = true;
            }
        }
        else if (!scrTextbox.instance.isOn)
        {
            _hatKidFix = false;
        }
    }

    private static void HatPlayers()
    {
        if (SceneManager.GetActiveScene().name != "Public Pool") return;
        for(int i = 0; i < ArchipelagoClient._session.Players.AllPlayers.Count(); i++)
            if (ArchipelagoClient._session.Players.GetPlayerInfo(i).Game == "A Hat in Time")
            {
                _hatPlayerNames.Add(ArchipelagoClient._session.Players.GetPlayerInfo(i).Name);
            }
        _hatPlayerNames.ForEach(Plugin.BepinLogger.LogInfo);
    }
    
    private static void AssignDisplayers()
    {
        if (!ArchipelagoClient.IsValidScene()) return;
        ShowDisplayers.InitializeCoroutineHost(scrGameSaveManager.instance);
        ShowDisplayers.CoinDisplayerGameObject = GameObject.Find("CoinDisplayer").gameObject.GetComponent<scrCoinDisplayer>();
        ShowDisplayers.CassetteDisplayerGameObject = GameObject.Find("CassetteDisplayer").gameObject.GetComponent<scrCassetteDisplayer>();
        ShowDisplayers.AppleDisplayerUIhider = GameObject.Find("Apple Displayer").gameObject.GetComponent<scrUIhider>();
        ShowDisplayers.BugDisplayerUIhider = GameObject.Find("Bug Displayer").gameObject.GetComponent<scrUIhider>();
        ShowDisplayers.KeyDisplayerUIhider = GameObject.Find("Key Displayer").gameObject.GetComponent<scrUIhider>();
        //ShowDisplayers.FishDisplayerGameObject = ;
    }

    private static void SendReminderNote()
    {
        if (!ArchipelagoClient.IsValidScene()) return;
        if (ItemHandler.HairballFishAmount >= 5 && !_sentNote10 && ArchipelagoClient.Ticket1
            && !scrGameSaveManager.instance.gameData.worldsData[1].coinFlags.Contains("fishing")
            && SceneManager.GetActiveScene().name != "Hairball City")
        {
            Plugin.APSendNote("You gathered all fish from Hairball City! Come get your reward!", 6.5f,
                Plugin.FischerNoteSprite);
            _sentNote10 = true;
        }

        if (ItemHandler.TurbineFishAmount >= 5 && !_sentNote11 && ArchipelagoClient.Ticket2
            && !scrGameSaveManager.instance.gameData.worldsData[2].coinFlags.Contains("fishing")
            && SceneManager.GetActiveScene().name != "Trash Kingdom")
        {
            Plugin.APSendNote("You gathered all fish from Turbine Town! Come get your reward!", 6.5f,
                Plugin.FischerNoteSprite);
            _sentNote11 = true;
        }

        if (ItemHandler.SalmonFishAmount >= 5 && !_sentNote12 && ArchipelagoClient.Ticket3
            && !scrGameSaveManager.instance.gameData.worldsData[3].coinFlags.Contains("fishing") 
            && scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("APWave1")
            && SceneManager.GetActiveScene().name != "Salmon Creek Forest")
        {
            Plugin.APSendNote("You gathered all fish from Salmon Creek Forest! Come get your reward!", 6.5f,
                Plugin.FischerNoteSprite);
            _sentNote12 = true;
        }

        if (ItemHandler.PoolFishAmount >= 5 && !_sentNote13 && ArchipelagoClient.Ticket4
            && !scrGameSaveManager.instance.gameData.worldsData[4].coinFlags.Contains("fishing")
            && SceneManager.GetActiveScene().name != "Public Pool")
        {
            Plugin.APSendNote("You gathered all fish from Public Pool! Come get your reward!", 6.5f,
                Plugin.FischerNoteSprite);
            _sentNote13 = true;
        }

        if (ItemHandler.BathFishAmount >= 5 && !_sentNote14 && ArchipelagoClient.Ticket5
            && !scrGameSaveManager.instance.gameData.worldsData[5].coinFlags.Contains("fishing") 
            && scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("APWave2")
            && SceneManager.GetActiveScene().name != "The Bathhouse")
        {
            Plugin.APSendNote("You gathered all fish from Bathhouse! Come get your reward!", 6.5f,
                Plugin.FischerNoteSprite);
            _sentNote14 = true;
        }

        if (ItemHandler.TadpoleFishAmount >= 5 && !_sentNote15 && ArchipelagoClient.Ticket6
            && !scrGameSaveManager.instance.gameData.worldsData[6].coinFlags.Contains("fishing")
            && SceneManager.GetActiveScene().name != "Tadpole inc")
        {
            Plugin.APSendNote("You gathered all fish from Tadpole HQ! Come get your reward!", 6.5f,
                Plugin.FischerNoteSprite);
            _sentNote15 = true;
        }

        if (ItemHandler.HairballFlowerAmount >= 3 && !_sentNote4 && ArchipelagoClient.Ticket1
            && !scrGameSaveManager.instance.gameData.worldsData[1].coinFlags.Contains("flowerPuzzle")
            && SceneManager.GetActiveScene().name != "Hairball City")
        {
            Plugin.APSendNote("You gathered all flowers from Hairball City! Come get your reward!", 6.5f,
                Plugin.GabiNoteSprite);
            _sentNote4 = true;
        }

        if (ItemHandler.TurbineFlowerAmount >= 3 && !_sentNote5 && ArchipelagoClient.Ticket2
            && !scrGameSaveManager.instance.gameData.worldsData[2].coinFlags.Contains("flowerPuzzle")
            && SceneManager.GetActiveScene().name != "Trash Kingdom")
        {
            Plugin.APSendNote("You gathered all flowers from Turbine Town! Come get your reward!", 6.5f,
                Plugin.GabiNoteSprite);
            _sentNote5 = true;
        }

        if (ItemHandler.SalmonFlowerAmount >= 6 && !_sentNote6 && ArchipelagoClient.Ticket3
            && !scrGameSaveManager.instance.gameData.worldsData[3].coinFlags.Contains("flowerPuzzle")
            && SceneManager.GetActiveScene().name != "Salmon Creek Forest")
        {
            Plugin.APSendNote("You gathered all flowers from Salmon Creek Forest! Come get your reward!", 6.5f,
                Plugin.GabiNoteSprite);
            _sentNote6 = true;
        }

        if (ItemHandler.PoolFlowerAmount >= 3 && !_sentNote7 && ArchipelagoClient.Ticket4
            && !scrGameSaveManager.instance.gameData.worldsData[4].coinFlags.Contains("flowerPuzzle") 
            && scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("APWave2")
            && SceneManager.GetActiveScene().name != "Public Pool")
        {
            Plugin.APSendNote("You gathered all flowers from Public Pool! Come get your reward!", 6.5f,
                Plugin.GabiNoteSprite);
            _sentNote7 = true;
        }

        if (ItemHandler.BathFlowerAmount >= 3 && !_sentNote8 && ArchipelagoClient.Ticket5
            && !scrGameSaveManager.instance.gameData.worldsData[5].coinFlags.Contains("flowerPuzzle") 
            && scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("APWave2")
            && SceneManager.GetActiveScene().name != "The Bathhouse")
        {
            Plugin.APSendNote("You gathered all flowers from Bathhouse! Come get your reward!", 6.5f,
                Plugin.GabiNoteSprite);
            _sentNote8 = true;
        }

        if (ItemHandler.TadpoleFlowerAmount >= 4 && !_sentNote9 && ArchipelagoClient.Ticket6
            && !scrGameSaveManager.instance.gameData.worldsData[6].coinFlags.Contains("flowerPuzzle")
            && SceneManager.GetActiveScene().name != "Tadpole inc")
        {
            Plugin.APSendNote("You gathered all flowers from Tadpole HQ! Come get your reward!", 6.5f,
                Plugin.GabiNoteSprite);
            _sentNote9 = true;
        }

        if (ItemHandler.HairballSeedAmount >= 10 && !_sentNote && ArchipelagoClient.Ticket1
            && !scrGameSaveManager.instance.gameData.worldsData[1].coinFlags.Contains("hamsterball") 
            && scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("APWave1")
            && SceneManager.GetActiveScene().name != "Hairball City")
        {
            Plugin.APSendNote("You gathered all 10 Seeds from Hairball City! Come get your reward!", 6.5f,
                Plugin.MoomyNoteSprite);
            _sentNote = true;
        }

        if (ItemHandler.SalmonSeedAmount >= 10 && !_sentNote2 && ArchipelagoClient.Ticket3
            && !scrGameSaveManager.instance.gameData.worldsData[3].coinFlags.Contains("hamsterball")
            && SceneManager.GetActiveScene().name != "Salmon Creek Forest")
        {
            Plugin.APSendNote("You gathered all 10 Seeds from Salmon Creek Forest! Come get your reward!", 6.5f,
                Plugin.MoomyNoteSprite);
            _sentNote2 = true;
        }

        if (ItemHandler.BathSeedAmount >= 10 && !_sentNote3 && ArchipelagoClient.Ticket5
            && !scrGameSaveManager.instance.gameData.worldsData[5].coinFlags.Contains("hamsterball")
            && SceneManager.GetActiveScene().name != "The Bathhouse")
        {
            Plugin.APSendNote("You gathered all 10 Seeds from Bathhouse! Come get your reward!", 6.5f,
                Plugin.MoomyNoteSprite);
            _sentNote3 = true;
        }
    }

    private static void FixApplePlacement()
    {
        if (SceneManager.GetActiveScene().name == "Salmon Creek Forest")
        {
            GameObject.Find("Apple Float (36)").transform.position = new Vector3(5f, 128f, 221f);
            GameObject.Find("Apple Float (37)").transform.position += new Vector3(0, -3.5f, -5f);
            GameObject.Find("Apple Float (38)").transform.position += new Vector3(0, -3.5f, -5f);
            GameObject.Find("Apple Float (39)").transform.position += new Vector3(0, -3.5f, -5f);
            GameObject.Find("Apple Float (40)").transform.position += new Vector3(0, -3.5f, -5f);
            GameObject.Find("Apple Float (41)").transform.position += new Vector3(0, -3.5f, -5f);
            GameObject.Find("Apple Float (42)").transform.position += new Vector3(0, -3.5f, -5f);
            GameObject.Find("Apple Float (43)").transform.position += new Vector3(0, -3.5f, -5f);
            GameObject.Find("Apple Float (44)").transform.position += new Vector3(0, -3.5f, -5f);
            GameObject.Find("Apple Float (45)").transform.position += new Vector3(0, -3.5f, -5f);
            
            GameObject.Find("Apple Float (63)").transform.position = new Vector3(59f, 137f, 81f);
            GameObject.Find("Apple Float (64)").transform.position = new Vector3(54f, 136.5f, 82f);
            GameObject.Find("Apple Float (65)").transform.position = new Vector3(51f, 136.5f, 82f);
            GameObject.Find("Apple Float (66)").transform.position = new Vector3(48f, 136.5f, 83f);
            GameObject.Find("Apple Float (67)").transform.position = new Vector3(43f, 136.5f, 84f);
            GameObject.Find("Apple Float (68)").transform.position = new Vector3(40f, 136.5f, 85f);
            GameObject.Find("Apple Float (69)").transform.position = new Vector3(36f, 136.5f, 86f);
            GameObject.Find("Apple Float (70)").transform.position = new Vector3(32f, 136.5f, 86f);
            GameObject.Find("Apple Float (71)").transform.position = new Vector3(-44f, 159f, 75f);
            GameObject.Find("Apple Float (72)").transform.position = new Vector3(-44f, 159f, 82f);
            GameObject.Find("Apple Float (73)").transform.position = new Vector3(-44f, 159f, 85f);
            GameObject.Find("Apple Float (74)").transform.position = new Vector3(-79f, 135f, 0f);
            GameObject.Find("Apple Float (75)").transform.position = new Vector3(-82f, 135f, -3f);
            GameObject.Find("Apple Float (76)").transform.position = new Vector3(-85f, 135f, -7f);
            GameObject.Find("Apple Float (77)").transform.position = new Vector3(-88f, 135f, -8f);
            GameObject.Find("Apple Float (78)").transform.position = new Vector3(79f, 125f, 119f);
            GameObject.Find("Apple Float (79)").transform.position = new Vector3(83f, 125f, 121f);
            GameObject.Find("Apple Float (80)").transform.position = new Vector3(81f, 125f, 117f);
            GameObject.Find("Apple Float (81)").transform.position = new Vector3(27f, 130f, 139f);
            GameObject.Find("Apple Float (82)").transform.position = new Vector3(38f, 132.5f, 135f);
        } else if (SceneManager.GetActiveScene().name == "Tadpole inc")
        {
            GameObject.Find("Apple Float (2)").transform.position = new Vector3(-80f, 2.5f, 15f);
            GameObject.Find("Apple Float (3)").transform.position = new Vector3(-78f, 2.5f, 13f);
            GameObject.Find("Apple Float (4)").transform.position = new Vector3(-83f, 2.5f, 17f);
            GameObject.Find("Apple Float (5)").transform.position = new Vector3(-105f, 2.5f, -40f);
            GameObject.Find("Apple Float (6)").transform.position = new Vector3(-114f, 2.5f, -52f);
            GameObject.Find("Apple Float (7)").transform.position = new Vector3(-100f, 2.5f, -54f);
            GameObject.Find("Apple Float (8)").transform.position = new Vector3(-68f, 2.5f, -69f);
            GameObject.Find("Apple Float (9)").transform.position = new Vector3(-50f, 2.5f, -48f);
            GameObject.Find("Apple Float (10)").transform.position = new Vector3(121f, 5.5f, -78f);
        }
    }

    private static void ApplesanityModWarning()
    {
        if (int.Parse(ArchipelagoData.slotData["applessanity"].ToString()) != 1) return;
        if (ArchipelagoData.slotData.ContainsKey("bugsanity")) return;
        cursor.transform.SetParent(GameObject.Find("UI").transform);
        SpawnWarningNotice();
        NoticeStillUp = true;
        Cursor.lockState = CursorLockMode.None;
        cursor.Visible = true;
        cursor.transform.SetSiblingIndex(99);
    }
    
    private static void SpawnWarningNotice()
    {
        var apUpdateNoticePrefab = Plugin.AssetBundle.LoadAsset<GameObject>("APWarningNotice");
        WarningNotice = Instantiate(apUpdateNoticePrefab, GameObject.Find("UI").transform, false);
        if (WarningNotice == null)
        {
            Plugin.BepinLogger.LogError("Failed to instantiate APWarningNotice prefab.");
            return;
        }
        WarningNotice.layer = LayerMask.NameToLayer("UI");
        WarningNotice.transform.SetSiblingIndex(30);
        var dismiss = WarningNotice.transform.Find("Panel/Dismiss").gameObject.GetComponent<Button>();
        dismiss.onClick.AddListener(DestroyNotice);
        var download = WarningNotice.transform.Find("Panel/Download").gameObject.GetComponent<Button>();
        download.onClick.AddListener(Download);
        WarningNotice.SetActive(true);
    }

    private static void DestroyNotice()
    {
        if (MenuHelpers.Menus.Count == 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            cursor.Visible = false;
        }
        cursor.transform.SetParent(GameObject.Find("UI/Menu system").transform);
        NoticeStillUp = false;
        dissmised = true;
        Destroy(WarningNotice);
    }

    private static void Download()
    {
        Application.OpenURL("https://github.com/niieli/NikoArchipelagoMod/releases/tag/0.6.3");
    }

    private static IEnumerator ModelLogging()
    {
        LoggedInstances.Clear();
        LogBatch.Clear();
        CreatedItemsCache.Clear();
        var waitFor = 1f;
        yield return new WaitUntil(() => PreviousScene != SceneManager.GetActiveScene().name);
        if (!onLogin && Plugin.loggedIn)
        {
            if (Plugin.DebugMode)
                Plugin.BepinLogger.LogInfo("Item Logs:\n"+LogPastItemsBatch);
            onLogin = true;
        }
        while (waitFor > 0)
        {
            yield return null;
            waitFor -= Time.deltaTime;
        }
        if(LogBatch.Length > 0 && Plugin.DebugMode)
            Plugin.BepinLogger.LogInfo("Model logs:\n"+LogBatch);
        PreviousScene = "";
    }

    private void GamepadToggleSpeed()
    {
        gamepad = Gamepad.current;
        if (gamepad == null) return;

        bool combo = gamepad.leftTrigger.isPressed &&
                     gamepad.selectButton.isPressed &&
                     gamepad.leftStickButton.isPressed;

        if (combo && !toggleSpeedBoost)
        {
            toggleSpeedBoost = true;
            Plugin.BepinLogger.LogInfo("Gamepad Combo triggered!");
            Plugin.ToggleSpeed();
        }
        else if (!combo)
        {
            toggleSpeedBoost = false;
        }
    }

    public void Update()
    {
        GamepadToggleSpeed();
        if (PreviousScene == "")
            PreviousScene = SceneManager.GetActiveScene().name;
        if (!ArchipelagoClient.IsValidScene()) return;
        HatKidEasterEgg();
        if (Plugin.ChristmasEvent)
        {
            var t = GameObject.Find("PlayerCamera");
            if (t != null && !_foundCamera)
            {
                var asset = Plugin.AssetBundleXmas.LoadAsset<GameObject>("Snowflakes");
                var w = Instantiate(asset, t.transform, false);
                w.AddComponent<StayOnScreen>();
                _foundCamera = true;
                Plugin.PlayerFound = true;
            }
        }
        if (ArchipelagoData.slotData == null) return;
        if (IsHamsterball)
        {
            if (!_turnedOff)
            {
                MyCharacterController.instance.DiveSpeed = 16f;
                MyCharacterController.instance.MaxAirMoveSpeed = 8f;
                MyCharacterController.instance.JumpSpeed = 13f;
                MyCharacterController.instance.DiveCancelHopSpeed = 11f;
                MyCharacterController.instance.MaxStableMoveSpeed = 8f;
                MyCharacterController.instance.MaxWaterMoveSpeed = 11f;
                _turnedOff = true;
                _turnedOn = false;
            }
        }
        else
        {
            if (!_turnedOn)
            {
                MovementSpeed.MovementSpeedMultiplier();
                _turnedOff = false;
                _turnedOn = true;
            }
        }
        if (scrNotificationDisplayer.instance.notificationQueue.Count > 0)
        {
            scrNotificationDisplayer.instance.avatar.enabled = false;
            scrNotificationDisplayer.instance.avatar.enabled = true;
        }

        if (!dissmised && cursor.transform.parent.name == "UI")
        {
            cursor.Visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (!oncePerScene && !dissmised)
        {
            ApplesanityModWarning();
            oncePerScene = true;
        }
        SendReminderNote();
        if (!scrTextbox.instance.isOn)
        {
            scrTextbox.instance.nameMesh.text = "";
        }
        if (ArchipelagoData.slotData.TryGetValue("cassette_logic", out var logic))
        {
            if (int.Parse(logic.ToString()) == 1)
            {
                var count = 1;
                var list = scrGameSaveManager.instance.gameData.worldsData;
                for (int i = 0; i < list.Count ; i++)
                {
                    if (list[i].coinFlags.Contains("cassetteCoin") || list[i].coinFlags.Contains("cassetteCoin2"))
                    {
                        if (!scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains($"MiMa{count}"))
                        {
                            scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Add($"MiMa{count}");
                        }
                        count++;
                    }
                }
            }
        }
        
        if (!ArchipelagoData.slotData.ContainsKey("garden_access")) return;
        GardenLevelFix();
        if (SceneManager.GetActiveScene().name != "Home") return;
        if (ItemHandler.Garden && !_checkedGhost && int.Parse(ArchipelagoData.slotData["garden_access"].ToString()) == 2)
        {
            _garyGhostHome.SetActive(ItemHandler.Garden);
            _checkedGhost = true;
            Plugin.BepinLogger.LogInfo("Enabled GaryGhost Home");
        }
        if (scrTextbox.instance == null) return;
        if (scrTextbox.instance.nameMesh.text == "???")
        {
            scrTrainManager.instance.UseTrain(24, false);
        }
    }
}