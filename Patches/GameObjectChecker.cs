using System;
using System.Collections;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NikoArchipelago.Patches;

public class GameObjectChecker : MonoBehaviour
{
    public static bool FirstMeeting;
    private void Start()
    {
        Plugin.BepinLogger.LogDebug("GameObjectChecker started!");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FirstMeeting = false;
        MitchAndMaiObject();
        PepperFirstMeetingTrigger();
        TitleScreenObject();
        InstatiateAPMenu();
        TrackerKiosk();
        TrackerTicket();
        StartCoroutine(CheckTrackers());
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
            var cassetteBuyer = GameObject.Find("CassetteBuyer");
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
            var cassetteBuyer2 = GameObject.Find("CassetteBuyer2");
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
        if (SceneManager.GetActiveScene().name == "Home")
        {
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
    }

    private static void TitleScreenObject()
    {
        try
        {
            var titleScreen = GameObject.Find("Title Screen");
            if (titleScreen != null)
            {
                titleScreen.GetComponent<Image>().sprite = Plugin.APLogoSprite;
                var actionScreen = GameObject.Find("ActionButton Title Screen");
                APMainMenu.TitleScreen = actionScreen;
                APMainMenu.TitleScreenAPLogo();
                Plugin.BepinLogger.LogInfo("Added Archipelago Menu!");
                Plugin.BepinLogger.LogInfo("Title Screen GameObject found!");
            }
            else
            {
                Plugin.BepinLogger.LogInfo("Title Screen GameObject does not exist!");
            }
        }
        catch (NullReferenceException e)
        {
            Plugin.BepinLogger.LogError($"Error finding 'Title Screen': {e.Message}");
        }
    }

    private static void InstatiateAPMenu()
    {
        if (!ArchipelagoClient.IsValidScene()) return;
        var apUIGameObject = Plugin.AssetBundle.LoadAsset<GameObject>("APMenuObjectTest1");
        var menuPrefab = Instantiate(apUIGameObject, GameObject.Find("UI").transform, false);
        if (menuPrefab == null)
        {
            Plugin.BepinLogger.LogError("Failed to instantiate ApUIGameObject prefab.");
            return;
        }
        menuPrefab.layer = LayerMask.NameToLayer("UI");
        var manager = menuPrefab.transform.Find("APMenuManager")?.gameObject;
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

    private static void TrackerDisplayer()
    {
        if (!ArchipelagoClient.IsValidScene()) return;
        var ticket = GameObject.Find("TrackerTicket");
        var kiosk = GameObject.Find("TrackerKiosk");
        if (ticket.GetComponent<CanvasGroup>() != null || kiosk.GetComponent<CanvasGroup>() != null)
        {
            //TrackerDisplayerPatch.Ticket = ticket.GetComponent<scrUIhider>();
            //TrackerDisplayerPatch.Kiosk = kiosk.GetComponent<scrUIhider>();
            TrackerDisplayerPatch.Ticket = ticket.GetComponent<CanvasGroup>();
            TrackerDisplayerPatch.Kiosk = kiosk.GetComponent<CanvasGroup>();
        }
    }

    private static IEnumerator CheckTrackers()
    {
        Plugin.BepinLogger.LogError("Looking for TrackerTicket & TrackerKiosk");
        yield return new WaitUntil(() => GameObject.Find("TrackerTicket") || GameObject.Find("TrackerKiosk"));
        Plugin.BepinLogger.LogInfo("Found TrackerTicket and TrackerKiosk");
        TrackerDisplayer();
    }
    
    private static GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}