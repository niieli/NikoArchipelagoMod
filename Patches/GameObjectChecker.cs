using System;
using System.Collections;
using System.Reflection;
using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace NikoArchipelago.Patches;

public class GameObjectChecker : MonoBehaviour
{
    private static GameObject _garyGhost, _garyGhostHome;
    private static bool _foundGhost;
    private static bool _foundCamera;
    public static bool FirstMeeting;
    private static bool _checkedGhost;
    private static bool _spawned;
    private static bool _foundNpcs;
    private static bool _missingFrog;
    public static GameObject APMenu;
    private void Start()
    {
        Plugin.BepinLogger.LogDebug("GameObjectChecker started!");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FirstMeeting = false;
        _checkedGhost = false;
        _spawned = false;
        _foundCamera = false;
        _foundNpcs = false;
        _missingFrog = false;
        //LocationCheck();
        MitchAndMaiObject();
        PepperFirstMeetingTrigger();
        TitleScreenObject();
        InstantiateAPMenu();
        APItemSent();
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
        //APArrowTracker();
        //ArchipelagoClient._session.DataStorage[Scope.Slot, "Apples"] = scrGameSaveManager.instance.gameData.generalGameData.appleAmount;
        if (Plugin.newFile && SceneManager.GetActiveScene().name != "Home")
        {
            Plugin.newFile = false;
        }
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static void CheckGameObject(GameObject gameObject)
    {
        //???
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
    
    private static void APItemSent()
    {
        if (!ArchipelagoClient.IsValidScene()) return;
        var apItemSentUI = Plugin.AssetBundle.LoadAsset<GameObject>("APItemSent");
        var itemSentPrefab = Instantiate(apItemSentUI, GameObject.Find("UI").transform, false);
        if (itemSentPrefab == null)
        {
            Plugin.BepinLogger.LogError("Failed to instantiate apItemSentUI prefab.");
            return;
        }
        itemSentPrefab.layer = LayerMask.NameToLayer("UI");
        var manager = itemSentPrefab.transform.Find("APItemSentManager")?.gameObject;
        if (manager == null)
        {
            Plugin.BepinLogger.LogError("APItemSentManager not found in the prefab.");
            return;
        }
        var sentNotification = manager.AddComponent<APItemSentNotification>();
        if (sentNotification == null)
        {
            Plugin.BepinLogger.LogError("Failed to add APItemSentNotification component to APItemSentManager.");
            return;
        }
        sentNotification.enabled = true;
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
        Instantiate(mapStatsApples, GameObject.Find("Statistics").transform, false);
        Instantiate(mapStatsFish, GameObject.Find("Statistics").transform, false);
        Instantiate(mapStatsFlowers, GameObject.Find("Statistics").transform, false);
        Instantiate(mapStatsSeeds, GameObject.Find("Statistics").transform, false);
        Instantiate(mapStatsLocations, GameObject.Find("Statistics").transform, false);
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
        var apple = Instantiate(mapStatsApples, GameObject.Find("Pepper/Whiteboard/Canvas/Statistics").transform, false);
        var fish = Instantiate(mapStatsFish, GameObject.Find("Pepper/Whiteboard/Canvas/Statistics").transform, false);
        var flower = Instantiate(mapStatsFlowers, GameObject.Find("Pepper/Whiteboard/Canvas/Statistics").transform, false);
        var seed = Instantiate(mapStatsSeeds, GameObject.Find("Pepper/Whiteboard/Canvas/Statistics").transform, false);
        var location = Instantiate(mapStatsLocations, GameObject.Find("Pepper/Whiteboard/Canvas/Statistics").transform, false);

        apple.transform.localPosition = new Vector3(95f, -121f, 0f);
        apple.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        fish.transform.localPosition = new Vector3(95f, -192f, 0f);
        flower.transform.localPosition = new Vector3(95f, -252f, 0f);
        flower.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        seed.transform.localPosition = new Vector3(95f, -320f, 0f);
        seed.transform.Find("Image").transform.localRotation = Quaternion.Euler(0f, 0f, 331f);
        location.transform.localPosition = new Vector3(-144f, -447f, -0f);
        location.transform.localScale = new Vector3(0.8166f, 0.8166f, 1.0166f);
        if (GameObject.Find("Pepper/Whiteboard/Canvas/Statistics/keys") != null)
        {
            var keys = GameObject.Find("Pepper/Whiteboard/Canvas/Statistics/keys");
            keys.transform.localPosition = new Vector3(-165f, -288f, 0f);
            keys.transform.localScale = new Vector3(0.7158f, 0.7158f, 0.85f);
        }
        var cassettes = GameObject.Find("Pepper/Whiteboard/Canvas/Statistics/cassettes");
        cassettes.transform.localPosition = new Vector3(-392f, -423f, 0f);
        Plugin.BepinLogger.LogInfo("Added new stats to Whiteboard.");
    }
    
    private static void NpcController()
    {
        if (ArchipelagoData.slotData == null) return;
        if (!ArchipelagoData.slotData.ContainsKey("npcsanity")) return;
        if (int.Parse(ArchipelagoData.slotData["npcsanity"].ToString()) == 0) return;
        if (!ArchipelagoClient.IsValidScene()) return;
        var apTrackerUI = new GameObject("NPCController");
        var tracker = apTrackerUI.AddComponent<NpcController>();
        if (tracker == null)
        {
            Plugin.BepinLogger.LogError("Failed to add NPCController component to NPCController.");
            return;
        }
        tracker.enabled = true;
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
        Plugin.ArrowTrackerGameObject.GetComponent<ArrowTrackerManager>().arrowIndicator.arrowObject = arrowPrefab.transform;
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
    
    private static void AssignDisplayers()
    {
        if (!ArchipelagoClient.IsValidScene()) return;
        ShowDisplayers.InitializeCoroutineHost(scrGameSaveManager.instance);
        ShowDisplayers.CoinDisplayerGameObject = GameObject.Find("CoinDisplayer").gameObject.GetComponent<scrCoinDisplayer>();
        ShowDisplayers.CassetteDisplayerGameObject = GameObject.Find("CassetteDisplayer").gameObject.GetComponent<scrCassetteDisplayer>();
        ShowDisplayers.AppleDisplayerUIhider = GameObject.Find("Apple Displayer").gameObject.GetComponent<scrUIhider>();
        ShowDisplayers.BugDisplayerUIhider = GameObject.Find("Bug Displayer").gameObject.GetComponent<scrUIhider>();
        ShowDisplayers.KeyDisplayerUIhider = GameObject.Find("Key Displayer").gameObject.GetComponent<scrUIhider>();
    }

    public void Update()
    {
        if (GameInput.GetButtonDown("Pause") && MenuHelpers.Menus.Count > 0)
        {
            Cursor.visible = false;
        }
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