using System;
using System.Collections;
using Archipelago.MultiClient.Net.Enums;
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
        MitchAndMaiObject();
        PepperFirstMeetingTrigger();
        TitleScreenObject();
        InstantiateAPMenu();
        APItemSent();
        TrackerKiosk();
        TrackerTicket();
        TrackerKey();
        HqWhiteboard();
        HqGarden();
        SpawnGaryHome();
        AssignDisplayers();
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
        var apTrackerUI = Plugin.AssetBundle.LoadAsset<GameObject>("APTrackerKey");
        var ticketPrefab = Instantiate(apTrackerUI, GameObject.Find("UI").transform, false);
        if (ticketPrefab == null)
        {
            Plugin.BepinLogger.LogError("Failed to instantiate apTrackerUI prefab.");
            return;
        }
        ticketPrefab.layer = LayerMask.NameToLayer("UI");
        ticketPrefab.transform.SetSiblingIndex(23);
        var manager = ticketPrefab.transform.Find("APKeyManager")?.gameObject;
        if (manager == null)
        {
            Plugin.BepinLogger.LogError("APKeyManager not found in the prefab.");
            return;
        }
        var tracker = manager.AddComponent<TrackerKeys>();
        if (tracker == null)
        {
            Plugin.BepinLogger.LogError("Failed to add TrackerKeys component to APKeyManager.");
            return;
        }
        tracker.enabled = true;
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
        if (!ArchipelagoData.slotData.ContainsKey("key_level")) return;
        if (int.Parse(ArchipelagoData.slotData["key_level"].ToString()) != 0) return;
        ShowDisplayers.KeyDisplayerUIhider = GameObject.Find("Key Displayer").gameObject.GetComponent<scrUIhider>();
    }

    public void Update()
    {
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