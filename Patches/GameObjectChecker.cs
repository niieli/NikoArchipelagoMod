using System;
using KinematicCharacterController.Core;
using UnityEngine;
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
    
    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FirstMeeting = false;
        MitchAndMaiObject();
        PepperFirstMeetingTrigger();
        TitleScreenObject();
        MainMenuObject();
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
                Instantiate(Plugin.ApUIGameObject);
                ArchipelagoMenu menuScript = Plugin.ApUIGameObject.GetComponent<ArchipelagoMenu>();
                if (menuScript == null)
                {
                    var manager = GameObject.Find("APMenuManager");
                    manager.AddComponent<ArchipelagoMenu>();
                    Plugin.BepinLogger.LogInfo("Added Archipelago Menu!");
                }
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
    private static void MainMenuObject()
    {
        try
        {
            //var menuScreen = FindInActiveObjectByName("Main menu - Buttons");
            var menuScreen = GameObject.Find("Menu system");
            //var menuScreen = GameObject.Find("Main menu - Buttons");
            if (menuScreen != null)
            {
                APMainMenu.MainMenuObject = menuScreen;
                Plugin.BepinLogger.LogInfo("Main menu - Buttons GameObject found!");
            }
            else
            {
                Plugin.BepinLogger.LogInfo("Main menu - Buttons GameObject does not exist!");
            }
        }
        catch (NullReferenceException e)
        {
            Plugin.BepinLogger.LogError($"Error finding 'Main menu - Buttons': {e.Message}");
        }
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