using System.Collections;
using NikoArchipelago.Patches;
using UnityEngine;

namespace NikoArchipelago.Stuff;

public class ShowDisplayers
{
    public static scrCoinDisplayer CoinDisplayerGameObject; 
    public static scrCassetteDisplayer CassetteDisplayerGameObject;
    public static scrUIhider KeyDisplayerUIhider, AppleDisplayerUIhider, BugDisplayerUIhider;
    
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
        yield return new WaitForSeconds(duration);
        CoinDisplayerGameObject.visible = false;
    }
    private static IEnumerator ShowCassetteDisplayer(float duration)
    {
        CassetteDisplayerGameObject.visible = true;
        yield return new WaitForSeconds(duration);
        CassetteDisplayerGameObject.visible = false;
    }
    private static IEnumerator ShowTicketDisplayer(float duration)
    {
        TrackerDisplayerPatch.TicketUI.visible = true;
        yield return new WaitForSeconds(duration);
        TrackerDisplayerPatch.TicketUI.visible = false;
    }
    private static IEnumerator ShowAppleDisplayer(float duration)
    {
        AppleDisplayerUIhider.visible = true;
        yield return new WaitForSeconds(duration);
        AppleDisplayerUIhider.visible = false;
    }
    private static IEnumerator ShowBugDisplayer(float duration)
    {
        BugDisplayerUIhider.visible = true;
        yield return new WaitForSeconds(duration);
        BugDisplayerUIhider.visible = false;
    }
    private static IEnumerator ShowKeyDisplayer(float duration)
    {
        KeyDisplayerUIhider.visible = true;
        yield return new WaitForSeconds(duration);
        KeyDisplayerUIhider.visible = false;
    }
}