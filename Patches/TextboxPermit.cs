using System.Collections.Generic;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            if (textbox.conversation == "waveTrigger") continue;
            _textboxes.Add(textbox.gameObject);
        }
    }

    private void Update()
    {
        if (ArchipelagoData.Options.Textbox == ArchipelagoOptions.TextboxLevel.Global)
            GlobalTextbox();
        else
            LevelTextbox();
    }

    private void GlobalTextbox()
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

    private void LevelTextbox()
    {
        var home = SceneManager.GetActiveScene().name == "Home" && ArchipelagoClient.HomeTextboxAcquired;
        var hairball = SceneManager.GetActiveScene().name == "Hairball City" && ArchipelagoClient.HairballTextboxAcquired;
        var turbine = SceneManager.GetActiveScene().name == "Trash Kingdom" && ArchipelagoClient.TurbineTextboxAcquired;
        var salmon = SceneManager.GetActiveScene().name == "Salmon Creek Forest" && ArchipelagoClient.SalmonTextboxAcquired;
        var pool = SceneManager.GetActiveScene().name == "Public Pool" && ArchipelagoClient.PoolTextboxAcquired;
        var bath = SceneManager.GetActiveScene().name == "The Bathhouse" && ArchipelagoClient.BathTextboxAcquired;
        var tadpole = SceneManager.GetActiveScene().name == "Tadpole inc" && ArchipelagoClient.TadpoleTextboxAcquired;
        var garden = SceneManager.GetActiveScene().name == "GarysGarden" && ArchipelagoClient.GardenTextboxAcquired;
        if (home || hairball || turbine || salmon || pool || bath || tadpole || garden)
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