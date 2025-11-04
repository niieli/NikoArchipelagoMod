using NikoArchipelago.Archipelago;
using NikoArchipelago.Patches;
using TMPro;
using UnityEngine;

namespace NikoArchipelago.Trackers;

public class TrackerGardenSeeds : MonoBehaviour
{
    public GameObject seedPanel;
    public TextMeshProUGUI seedCounter;
    private static scrUIhider uiHider;
    
    private void Start()
    {
        seedPanel = transform.Find("SeedPanel").gameObject;
        seedCounter = seedPanel.transform.Find("Count").GetComponent<TextMeshProUGUI>();
        
        if (seedPanel == null)
        {
            Plugin.BepinLogger.LogError("SeedPanel is null");
            return;
        }
        
        seedPanel.SetActive(true);
        
        uiHider = transform.Find("SeedPanel")?.gameObject.AddComponent<scrUIhider>();
        if (uiHider != null)
        {
            var reference = GameObject.Find("UI/Apple Displayer").GetComponent<scrUIhider>();
            var referenceFont = GameObject.Find("UI/Apple Displayer/Amount").GetComponent<TextMeshProUGUI>();
            uiHider.useAlphaCurve = reference.useAlphaCurve;
            uiHider.alphaCurve = reference.alphaCurve;
            uiHider.animationCurve = reference.animationCurve;
            uiHider.duration = 0.625f;
            uiHider.hideOffset = new Vector3(425, 0, 0);
            TrackerDisplayerPatch.GardenSeedUI = uiHider;
            seedCounter.font = referenceFont.font;
            seedCounter.fontSize = referenceFont.fontSize;
            seedCounter.color = referenceFont.color;
        }
    }

    public void Update()
    {
        if (!Plugin.saveReady) return;
        if (ArchipelagoData.slotData == null) return;
        seedCounter.text = ItemHandler.GarysGardenSeedAmount.ToString();
    }
}