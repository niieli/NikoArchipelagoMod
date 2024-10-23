using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NikoArchipelago;

public class TrackerKiosk : MonoBehaviour
{
    public GameObject kioskPanel;
    public Image kioskHomeImage;
    public Image kioskHcImage;
    public Image kioskTtImage;
    public Image kioskSfcImage;
    public Image kioskPpImage;
    public Image kioskBathImage;
    public Image kioskHqImage;
    public Image kioskHomeCostImage;
    public Image kioskHcCostImage;
    public Image kioskTtCostImage;
    public Image kioskSfcCostImage;
    public Image kioskPpCostImage;
    public Image kioskBathCostImage;
    public Image kioskHqCostImage;
    public Image boughtHomeImage;
    public Image boughtHcImage;
    public Image boughtTtImage;
    public Image boughtSfcImage;
    public Image boughtPpImage;
    public Image boughtBathImage;
    public Image boughtHqImage;
    public TextMeshProUGUI kioskHomeText;
    public TextMeshProUGUI kioskHcText;
    public TextMeshProUGUI kioskTtText;
    public TextMeshProUGUI kioskSfcText;
    public TextMeshProUGUI kioskPpText;
    public TextMeshProUGUI kioskBathText;
    public TextMeshProUGUI kioskHqText;
    private scrGameSaveManager gameSaveManager;

    public void Start()
    {
        gameSaveManager = scrGameSaveManager.instance;
        kioskPanel = transform.Find("TrackerKiosk")?.gameObject;
        kioskHomeImage = transform.Find("TrackerKiosk/KioskHome")?.GetComponent<Image>();
        boughtHomeImage = transform.Find("TrackerKiosk/KioskHome/Bought")?.GetComponent<Image>();
        kioskHomeCostImage = transform.Find("TrackerKiosk/KioskHome/CostHome")?.GetComponent<Image>();
        kioskHomeText = transform.Find("TrackerKiosk/KioskHome/CostHome/Cost")?.GetComponent<TextMeshProUGUI>();
        kioskHcImage = transform.Find("TrackerKiosk/KioskHairball")?.GetComponent<Image>();
        boughtHcImage = transform.Find("TrackerKiosk/KioskHairball/Bought")?.GetComponent<Image>();
        kioskHcCostImage = transform.Find("TrackerKiosk/KioskHairball/CostHairball")?.GetComponent<Image>();
        kioskHcText = transform.Find("TrackerKiosk/KioskHairball/CostHairball/Cost").GetComponent<TextMeshProUGUI>();
        kioskTtImage = transform.Find("TrackerKiosk/KioskTurbine")?.GetComponent<Image>();
        boughtTtImage = transform.Find("TrackerKiosk/KioskTurbine/Bought")?.GetComponent<Image>();
        kioskTtCostImage = transform.Find("TrackerKiosk/KioskTurbine/CostTurbine")?.GetComponent<Image>();
        kioskTtText = transform.Find("TrackerKiosk/KioskTurbine/CostTurbine/Cost")?.GetComponent<TextMeshProUGUI>();
        kioskSfcImage = transform.Find("TrackerKiosk/KioskSalmon")?.GetComponent<Image>();
        boughtSfcImage = transform.Find("TrackerKiosk/KioskSalmon/Bought")?.GetComponent<Image>();
        kioskSfcCostImage = transform.Find("TrackerKiosk/KioskSalmon/CostSalmon")?.GetComponent<Image>();
        kioskSfcText = transform.Find("TrackerKiosk/KioskSalmon/CostSalmon/Cost")?.GetComponent<TextMeshProUGUI>();
        kioskPpImage = transform.Find("TrackerKiosk/KioskPool")?.GetComponent<Image>();
        boughtPpImage = transform.Find("TrackerKiosk/KioskPool/Bought")?.GetComponent<Image>();
        kioskPpCostImage = transform.Find("TrackerKiosk/KioskPool/CostPool")?.GetComponent<Image>();
        kioskPpText = transform.Find("TrackerKiosk/KioskPool/CostPool/Cost")?.GetComponent<TextMeshProUGUI>();
        kioskBathImage = transform.Find("TrackerKiosk/KioskBath")?.GetComponent<Image>();
        boughtBathImage = transform.Find("TrackerKiosk/KioskBath/Bought")?.GetComponent<Image>();
        kioskBathCostImage = transform.Find("TrackerKiosk/KioskBath/CostBath")?.GetComponent<Image>();
        kioskBathText = transform.Find("TrackerKiosk/KioskBath/CostBath/Cost")?.GetComponent<TextMeshProUGUI>();
        kioskHqImage = transform.Find("TrackerKiosk/KioskTadpole")?.GetComponent<Image>();
        boughtHqImage = transform.Find("TrackerKiosk/KioskTadpole/Bought")?.GetComponent<Image>();
        kioskHqCostImage = transform.Find("TrackerKiosk/KioskTadpole/CostTadpole")?.GetComponent<Image>();
        kioskHqText = transform.Find("TrackerKiosk/KioskTadpole/CostTadpole/Cost")?.GetComponent<TextMeshProUGUI>();
        
        if (kioskPanel == null)
        {
            Plugin.BepinLogger.LogError("KioskPanel is null");
            return;
        }
        if (kioskHomeImage == null) Plugin.BepinLogger.LogError("kioskHomeImage is null");
        if (kioskHcImage == null) Plugin.BepinLogger.LogError("kioskHcImage is null");
        if (kioskTtImage == null) Plugin.BepinLogger.LogError("kioskTtImage is null");
        if (kioskSfcImage == null) Plugin.BepinLogger.LogError("kioskSfcImage is null");
        if (kioskPpImage == null) Plugin.BepinLogger.LogError("kioskPpImage is null");
        if (kioskBathImage == null) Plugin.BepinLogger.LogError("kioskBathImage is null");
        if (kioskHomeText == null) Plugin.BepinLogger.LogError("kioskHomeText is null");
        if (kioskHqText == null) Plugin.BepinLogger.LogError("kioskHqText is null");
        if (kioskHcText == null) Plugin.BepinLogger.LogError("kioskHcText is null");
        if (kioskTtText == null) Plugin.BepinLogger.LogError("kioskTtText is null");
        if (kioskSfcText == null) Plugin.BepinLogger.LogError("kioskSfcText is null");
        if (kioskPpText == null) Plugin.BepinLogger.LogError("kioskPpText is null");
        if (kioskBathText == null) Plugin.BepinLogger.LogError("kioskBathText is null");
        if (kioskHqText == null) Plugin.BepinLogger.LogError("kioskHqText is null");
        
        kioskPanel.SetActive(true);
        boughtHomeImage.enabled = false;
        boughtHcImage.enabled = false;
        boughtTtImage.enabled = false;
        boughtSfcImage.enabled = false;
        boughtPpImage.enabled = false;
        boughtBathImage.enabled = false;
        boughtHqImage.enabled = false;
        //var uIhider = kioskPanel.AddComponent<scrUIhider>();
        var cGroup = kioskPanel.AddComponent<CanvasGroup>();
        cGroup.alpha = 0;
        //uIhider.visible = false;
        //uIhider.useAlphaCurve = true;
    }

    public void Update()
    {
        if (!Plugin.saveReady) return;
        if (ArchipelagoMenu.KioskSpoiler)
        {
            kioskHomeText.text = scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[1] 
                ? levelData.levelPrices[2].ToString() : "??";
            kioskHcText.text = scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[2] 
                ? levelData.levelPrices[3].ToString() : "??";
            kioskTtText.text = scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[3] 
                ? levelData.levelPrices[4].ToString() : "??";
            kioskSfcText.text = scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[4] 
                ? levelData.levelPrices[5].ToString() : "??";
            kioskPpText.text = scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[5] 
                ? levelData.levelPrices[6].ToString() : "??";
            kioskBathText.text = scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[6] 
                ? levelData.levelPrices[7].ToString() : "??";
            kioskHqText.text = scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[7] 
                ? levelData.levelPrices[8].ToString() : "??";
        }
        else
        {
            kioskHomeText.text = levelData.levelPrices[2].ToString();
            kioskHcText.text = levelData.levelPrices[3].ToString();
            kioskTtText.text = levelData.levelPrices[4].ToString();
            kioskSfcText.text = levelData.levelPrices[5].ToString();
            kioskPpText.text = levelData.levelPrices[6].ToString();
            kioskBathText.text = levelData.levelPrices[7].ToString();
            kioskHqText.text = levelData.levelPrices[8].ToString();
        }
        if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("KioskHome"))
        {
            kioskHomeImage.color = new Color(1f, 1f, 1f, 1f);
            kioskHomeText.enabled = false;
            kioskHomeCostImage.enabled = false;
            boughtHomeImage.enabled = true;
        }
        if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("KioskHairball City"))
        {
            kioskHcImage.color = new Color(1f, 1f, 1f, 1f);
            kioskHcText.enabled = false;
            kioskHcCostImage.enabled = false;
            boughtHcImage.enabled = true;
        }
        if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("KioskTrash Kingdom"))
        {
            kioskTtImage.color = new Color(1f, 1f, 1f, 1f);
            kioskTtText.enabled = false;
            kioskTtCostImage.enabled = false;
            boughtTtImage.enabled = true;
        }
        if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("KioskSalmon Creek Forest"))
        {
            kioskSfcImage.color = new Color(1f, 1f, 1f, 1f);
            kioskSfcText.enabled = false;
            kioskSfcCostImage.enabled = false;
            boughtSfcImage.enabled = true;
        }
        if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("KioskPublic Pool"))
        {
            kioskPpImage.color = new Color(1f, 1f, 1f, 1f);
            kioskPpText.enabled = false;
            kioskPpCostImage.enabled = false;
            boughtPpImage.enabled = true;
        }
        if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("KioskThe Bathhouse"))
        {
            kioskBathImage.color = new Color(1f, 1f, 1f, 1f);
            kioskBathText.enabled = false;
            kioskBathCostImage.enabled = false;
            boughtBathImage.enabled = true;
        }
        if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("KioskTadpole inc"))
        {
            kioskHqImage.color = new Color(1f, 1f, 1f, 1f);
            kioskHqText.enabled = false;
            kioskHqCostImage.enabled = false;
            boughtHqImage.enabled = true;
        }
    }
}