using System;
using KinematicCharacterController.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

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
}