using System.Collections;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using TMPro;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class DeathAmnesty : MonoBehaviour
{
    public static bool DeathLinkEnabled;
    public static int DeathCounterInt; //TODO: Add to the companion save file, when ever that is implemented
    private static int _oldDeathCounterInt;
    private static GameObject _popupObject;
    private static float _timer;
    private static bool _playerTouchedHazard;
    private static Hazard _lastHazard;
    private static readonly GameObject Popup = Assets.DeathLinkPopup;

    private enum Hazard
    {
        Water = 1,
        Scissor = 2
    }
    
    private void Update()
    {
        if (!DeathLinkEnabled) return;
        if (_playerTouchedHazard && _timer < 2.5f)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            _timer = 0;
            _playerTouchedHazard = false;
        }
        
        if (DeathCounterInt >= ArchipelagoData.Options.DeathLinkAmnesty)
        {
            SendDeath();
            return;
        }
        
        IsTouchingWater();

        while (DeathCounterInt > _oldDeathCounterInt && DeathCounterInt < ArchipelagoData.Options.DeathLinkAmnesty)
        {
            _oldDeathCounterInt++;
            Plugin.BepinLogger.LogInfo($"Deathcounter: {DeathCounterInt}");
            DeathPopup();
        }
    }

    private static void IsTouchingWater()
    {
        if (!ArchipelagoClient.SwimmingAcquired)
        {
            if (MyCharacterController.instance.isTouchingWater && !_playerTouchedHazard)
            {
                DeathCounterInt++;
                _playerTouchedHazard = true;
                _lastHazard = Hazard.Water;
            }
        }
    }

    private static void SendDeath()
    {
        string cause = _lastHazard switch
        {
            Hazard.Water => "tried swimming without knowing how to even swim",
            Hazard.Scissor => "cut themselves with scissors",
            _ => "died... somehow"
        };
        Plugin.ArchipelagoClient.SendDeathLink(cause);
        Plugin.BepinLogger.LogWarning("You... and everyone else are dead!");
        DeathCounterInt = 0;
        _oldDeathCounterInt = 0;
    }

    private void DeathPopup()
    {
        if (_popupObject != null)
        {
            StopAllCoroutines();
            Destroy(_popupObject);
        }
        StartCoroutine(ShowPopup());
    }

    private static IEnumerator ShowPopup()
    {
        _popupObject = Instantiate(Popup, Plugin.NotifcationCanvas.transform);
        _popupObject.transform.Find("Touches").GetComponent<TextMeshProUGUI>().text = $"<color=#ff0000>{ArchipelagoData.Options.DeathLinkAmnesty - DeathCounterInt}</color> touches until DeathLink!";
        if (ArchipelagoData.Options.DeathLinkAmnesty - DeathCounterInt == 1)
            _popupObject.transform.Find("Touches").GetComponent<TextMeshProUGUI>().text = $"<color=#ff0000>{ArchipelagoData.Options.DeathLinkAmnesty-DeathCounterInt}</color> touch until DeathLink!";
        _popupObject.transform.Find("Amnesty").GetComponent<TextMeshProUGUI>().text = $"DeathLink Amnesty is set to {ArchipelagoData.Options.DeathLinkAmnesty} touches";
        var timeRemaining = 3f;
        while (timeRemaining > 0)
        {
            yield return null;
            timeRemaining -= Time.deltaTime;
        }
        Destroy(_popupObject);
    }

    [HarmonyPatch(typeof(PlayerHitDetector), "OnTriggerEnter")]
    public static class PlayerHitDetectorPostfix
    {
        private static void Postfix(PlayerHitDetector __instance, ref Collider other)
        {
            if (other.CompareTag("Pain") && !_playerTouchedHazard)
            {
                DeathCounterInt++;
                _playerTouchedHazard = true;
                _lastHazard = Hazard.Scissor;
            }
        }
    }
}