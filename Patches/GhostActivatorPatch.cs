using System;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class GhostActivatorPatch : MonoBehaviour
{
    private GameObject _ghost;
    public void Start()
    {
        _ghost = transform.Find("GaryGhost").gameObject;
    }

    public void Update()
    {
        if (ArchipelagoData.slotData == null) return;
        if (!ArchipelagoData.slotData.ContainsKey("garden_access")) return;
        if (ArchipelagoData.Options.GardenAccess == ArchipelagoOptions.GardenAccessMode.Tadpole) return;
        _ghost.SetActive(ItemHandler.Garden);
    }
}