using System;
using System.Collections.Generic;
using System.Linq;
using NikoArchipelago.Patches;
using NikoArchipelago.Stuff;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace NikoArchipelago;

public class ArrowTrackerManager : MonoBehaviour
{
    public List<Transform> activeLocations = new();
    public ArrowIndicator arrowIndicator;
    public Image iconImage;
    private Transform currentTarget;
    private int currentTargetIndex = 0;

    public void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Home")
        {
            //locations = GameObject.FindObjectsOfType<Transform>();
        }
        var player = GameObject.Find("PlayerCharacter/CharacterController");
        LoadActiveLocations();
        arrowIndicator = gameObject.AddComponent<ArrowIndicator>();
        arrowIndicator.player = player.transform;
    }
    
    private void LoadActiveLocations()
    {
        activeLocations.Clear(); // Alte Locations löschen

        var gameData = scrWorldSaveDataContainer.instance;

        foreach (var location in Locations.GetTrackableLocations(gameData))
        {
            GameObject locObject = FindClosestObject(location.Position);

            if (locObject != null)
            {
                activeLocations.Add(locObject.transform); // Füge gefundene Transform hinzu
            }

        }

        currentTargetIndex = 0;
        SetNextTarget(); // Erstes Ziel setzen
    }

    private void SetNextTarget()
    {
        if (activeLocations.Count == 0) return; // Abbruch, wenn keine Ziele verfügbar

        currentTarget = activeLocations[currentTargetIndex]; // Ziel setzen
        arrowIndicator.target = currentTarget;               // An ArrowIndicator übergeben
    }

    private GameObject FindClosestObject(Vector3 targetPosition)
    {
        float maxDistance = 1.0f; // Maximal erlaubte Distanz für Übereinstimmung
        GameObject closestObject = null;

        // Suche alle möglichen Objekte in der Szene
        foreach (var obj in GameObject.FindObjectsOfType<Transform>())
        {
            if (Vector3.Distance(obj.position, targetPosition) <= maxDistance)
            {
                closestObject = obj.gameObject;
                break; // Abbruch, wenn ein passendes Objekt gefunden wurde
            }
        }

        return closestObject;
    }

    
    private void Update()
    {
        if (currentTarget == null && activeLocations.Count > 0)
        {
            SetNextTarget(); // Bei fehlendem Ziel neu setzen
        }

        //locations = GameObject.FindObjectsOfType<Transform>();
    }
    
    public void MoveToNextTarget()
    {
        if (activeLocations.Count == 0) return;

        currentTargetIndex = (currentTargetIndex + 1) % activeLocations.Count; // Zum nächsten Ziel wechseln 
        SetNextTarget();
    }

}