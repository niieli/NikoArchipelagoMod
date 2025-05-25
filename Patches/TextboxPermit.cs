using System;
using System.Collections.Generic;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class TextboxPermit : MonoBehaviour
{
    private List<GameObject> _textboxes = new();
    private bool _setActive = false;
    private void Start()
    {
        scrTextboxTrigger[] textboxes = FindObjectsOfType<scrTextboxTrigger>(includeInactive: true);
    
        foreach (var textbox in textboxes)
        {
            _textboxes.Add(textbox.gameObject);
        }
    }

    private void Update()
    {
        if (ArchipelagoClient.TextboxAcquired)
        {
            if (_setActive) return;
            foreach (var textbox in _textboxes)
            {
                textbox.SetActive(true);
            }
            _setActive = true;
            return;
        }
        
        foreach (var textbox in _textboxes)
        {
            textbox.SetActive(false);
        }
    }
}