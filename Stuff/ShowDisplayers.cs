using System.Collections;
using NikoArchipelago.Patches;
using UnityEngine;

namespace NikoArchipelago.Stuff;

public class ShowDisplayers
{
    public static scrCoinDisplayer CoinDisplayerGameObject; 
    public static scrCassetteDisplayer CassetteDisplayerGameObject;
    public static scrUIhider KeyDisplayerUIhider, AppleDisplayerUIhider, BugDisplayerUIhider;
    public static scrFishDisplayer FishDisplayerGameObject;
    
    private static MonoBehaviour _coroutineHost;

    public static void InitializeCoroutineHost(MonoBehaviour host)
    {
        _coroutineHost = host;
    }
    
    public static void CoinDisplayer(float timer = 3.5f)
    {
        _coroutineHost.StartCoroutine(ShowCoinDisplayer(timer));
    } 
    public static void CassetteDisplayer(float timer = 3.5f)
    {
        _coroutineHost.StartCoroutine(ShowCassetteDisplayer(timer));
    }
    public static void KeyDisplayer(float timer = 3.5f)
    {
        _coroutineHost.StartCoroutine(ShowKeyDisplayer(timer));
    }
    public static void AppleDisplayer(float timer = 3.5f)
    {
        _coroutineHost.StartCoroutine(ShowAppleDisplayer(timer));
    }
    public static void BugDisplayer(float timer = 3.5f)
    {
        _coroutineHost.StartCoroutine(ShowBugDisplayer(timer));
    }
    public static void TicketDisplayer(float timer = 3.5f)
    {
        _coroutineHost.StartCoroutine(ShowTicketDisplayer(timer));
    }

    
    private static IEnumerator ShowCoinDisplayer(float duration)
    {
        CoinDisplayerGameObject.visible = true;
        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
        }
        CoinDisplayerGameObject.visible = false;
    }
    private static IEnumerator ShowCassetteDisplayer(float duration)
    {
        CassetteDisplayerGameObject.visible = true;
        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
        }
        CassetteDisplayerGameObject.visible = false;
    }
    private static IEnumerator ShowTicketDisplayer(float duration)
    {
        TrackerDisplayerPatch.TicketUI.visible = true;
        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
        }
        TrackerDisplayerPatch.TicketUI.visible = false;
    }
    private static IEnumerator ShowAppleDisplayer(float duration)
    {
        AppleDisplayerUIhider.visible = true;
        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
        }
        AppleDisplayerUIhider.visible = false;
    }
    private static IEnumerator ShowBugDisplayer(float duration)
    {
        BugDisplayerUIhider.visible = true;
        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
        }
        BugDisplayerUIhider.visible = false;
    }
    private static IEnumerator ShowKeyDisplayer(float duration)
    {
        KeyDisplayerUIhider.visible = true;
        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
        }
        KeyDisplayerUIhider.visible = false;
    }
}