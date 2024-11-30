using System;
using System.Linq;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Patches;
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
    public TextMeshProUGUI kioskHomeShadowText;
    public TextMeshProUGUI kioskHcShadowText;
    public TextMeshProUGUI kioskTtShadowText;
    public TextMeshProUGUI kioskSfcShadowText;
    public TextMeshProUGUI kioskPpShadowText;
    public TextMeshProUGUI kioskBathShadowText;
    public TextMeshProUGUI kioskHqShadowText;
    private scrGameSaveManager gameSaveManager;
    private static scrUIhider uiHider;
    
    // Mitch & Mai
    public Image mitchGardenImage;
    public Image maiGardenImage;
    public Image boughtMitchGardenImage;
    public Image boughtMaiGardenImage;
    public TextMeshProUGUI mitchGardenText;
    public TextMeshProUGUI maiGardenText;
    public TextMeshProUGUI mitchGardenShadowText;
    public TextMeshProUGUI maiGardenShadowText;
    public Image mitchHairballImage;
    public Image maiHairballImage;
    public Image boughtMitchHairballImage;
    public Image boughtMaiHairballImage;
    public TextMeshProUGUI mitchHairballText;
    public TextMeshProUGUI maiHairballText;
    public TextMeshProUGUI mitchHairballShadowText;
    public TextMeshProUGUI maiHairballShadowText;
    public Image mitchTurbineImage;
    public Image maiTurbineImage;
    public Image boughtMitchTurbineImage;
    public Image boughtMaiTurbineImage;
    public TextMeshProUGUI mitchTurbineText;
    public TextMeshProUGUI maiTurbineText;
    public TextMeshProUGUI mitchTurbineShadowText;
    public TextMeshProUGUI maiTurbineShadowText;
    public Image mitchSalmonImage;
    public Image maiSalmonImage;
    public Image boughtMitchSalmonImage;
    public Image boughtMaiSalmonImage;
    public TextMeshProUGUI mitchSalmonText;
    public TextMeshProUGUI maiSalmonText;
    public TextMeshProUGUI mitchSalmonShadowText;
    public TextMeshProUGUI maiSalmonShadowText;
    public Image mitchPoolImage;
    public Image maiPoolImage;
    public Image boughtMitchPoolImage;
    public Image boughtMaiPoolImage;
    public TextMeshProUGUI mitchPoolText;
    public TextMeshProUGUI maiPoolText;
    public TextMeshProUGUI mitchPoolShadowText;
    public TextMeshProUGUI maiPoolShadowText;
    public Image mitchBathImage;
    public Image maiBathImage;
    public Image boughtMitchBathImage;
    public Image boughtMaiBathImage;
    public TextMeshProUGUI mitchBathText;
    public TextMeshProUGUI maiBathText;
    public TextMeshProUGUI mitchBathShadowText;
    public TextMeshProUGUI maiBathShadowText;
    public Image mitchTadpoleImage;
    public Image maiTadpoleImage;
    public Image boughtMitchTadpoleImage;
    public Image boughtMaiTadpoleImage;
    public TextMeshProUGUI mitchTadpoleText;
    public TextMeshProUGUI maiTadpoleText;
    public TextMeshProUGUI mitchTadpoleShadowText;
    public TextMeshProUGUI maiTadpoleShadowText;

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
        kioskHomeShadowText = transform.Find("TrackerKiosk/KioskHome/CostHome/CostShadow")?.GetComponent<TextMeshProUGUI>();
        kioskHcShadowText = transform.Find("TrackerKiosk/KioskHairball/CostHairball/CostShadow").GetComponent<TextMeshProUGUI>();
        kioskTtShadowText = transform.Find("TrackerKiosk/KioskTurbine/CostTurbine/CostShadow")?.GetComponent<TextMeshProUGUI>();
        kioskSfcShadowText = transform.Find("TrackerKiosk/KioskSalmon/CostSalmon/CostShadow")?.GetComponent<TextMeshProUGUI>();
        kioskPpShadowText = transform.Find("TrackerKiosk/KioskPool/CostPool/CostShadow")?.GetComponent<TextMeshProUGUI>();
        kioskBathShadowText = transform.Find("TrackerKiosk/KioskBath/CostBath/CostShadow")?.GetComponent<TextMeshProUGUI>();
        kioskHqShadowText = transform.Find("TrackerKiosk/KioskTadpole/CostTadpole/CostShadow")?.GetComponent<TextMeshProUGUI>();
        
        // Mitch & Mai
        mitchGardenImage = kioskPanel.transform.Find("GarysGarden/Mitch").GetComponent<Image>();
        maiGardenImage = kioskPanel.transform.Find("GarysGarden/Mai").GetComponent<Image>();
        boughtMitchGardenImage = mitchGardenImage.transform.Find("Bought").GetComponent<Image>();
        boughtMaiGardenImage = maiGardenImage.transform.Find("Bought").GetComponent<Image>();
        mitchGardenText = mitchGardenImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        mitchGardenShadowText = mitchGardenImage.transform.Find("CostShadow").GetComponent<TextMeshProUGUI>();
        maiGardenText = maiGardenImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        maiGardenShadowText = maiGardenImage.transform.Find("CostShadow").GetComponent<TextMeshProUGUI>();
        
        mitchHairballImage = kioskHcImage.transform.Find("Mitch").GetComponent<Image>();
        maiHairballImage = kioskHcImage.transform.Find("Mai").GetComponent<Image>();
        boughtMitchHairballImage = mitchHairballImage.transform.Find("Bought").GetComponent<Image>();
        boughtMaiHairballImage = maiHairballImage.transform.Find("Bought").GetComponent<Image>();
        mitchHairballText = mitchHairballImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        mitchHairballShadowText = mitchHairballImage.transform.Find("CostShadow").GetComponent<TextMeshProUGUI>();
        maiHairballText = maiHairballImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        maiHairballShadowText = maiHairballImage.transform.Find("CostShadow").GetComponent<TextMeshProUGUI>();
        
        mitchTurbineImage = kioskTtImage.transform.Find("Mitch").GetComponent<Image>();
        maiTurbineImage = kioskTtImage.transform.Find("Mai").GetComponent<Image>();
        boughtMitchTurbineImage = mitchTurbineImage.transform.Find("Bought").GetComponent<Image>();
        boughtMaiTurbineImage = maiTurbineImage.transform.Find("Bought").GetComponent<Image>();
        mitchTurbineText = mitchTurbineImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        mitchTurbineShadowText = mitchTurbineImage.transform.Find("CostShadow").GetComponent<TextMeshProUGUI>();
        maiTurbineText = maiTurbineImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        maiTurbineShadowText = maiTurbineImage.transform.Find("CostShadow").GetComponent<TextMeshProUGUI>();
        
        mitchSalmonImage = kioskSfcImage.transform.Find("Mitch").GetComponent<Image>();
        maiSalmonImage = kioskSfcImage.transform.Find("Mai").GetComponent<Image>();
        boughtMitchSalmonImage = mitchSalmonImage.transform.Find("Bought").GetComponent<Image>();
        boughtMaiSalmonImage = maiSalmonImage.transform.Find("Bought").GetComponent<Image>();
        mitchSalmonText = mitchSalmonImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        mitchSalmonShadowText = mitchSalmonImage.transform.Find("CostShadow").GetComponent<TextMeshProUGUI>();
        maiSalmonText = maiSalmonImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        maiSalmonShadowText = maiSalmonImage.transform.Find("CostShadow").GetComponent<TextMeshProUGUI>();
        
        mitchPoolImage = kioskPpImage.transform.Find("Mitch").GetComponent<Image>();
        maiPoolImage = kioskPpImage.transform.Find("Mai").GetComponent<Image>();
        boughtMitchPoolImage = mitchPoolImage.transform.Find("Bought").GetComponent<Image>();
        boughtMaiPoolImage = maiPoolImage.transform.Find("Bought").GetComponent<Image>();
        mitchPoolText = mitchPoolImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        mitchPoolShadowText = mitchPoolImage.transform.Find("CostShadow").GetComponent<TextMeshProUGUI>();
        maiPoolText = maiPoolImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        maiPoolShadowText = maiPoolImage.transform.Find("CostShadow").GetComponent<TextMeshProUGUI>();
        
        mitchBathImage = kioskBathImage.transform.Find("Mitch").GetComponent<Image>();
        maiBathImage = kioskBathImage.transform.Find("Mai").GetComponent<Image>();
        boughtMitchBathImage = mitchBathImage.transform.Find("Bought").GetComponent<Image>();
        boughtMaiBathImage = maiBathImage.transform.Find("Bought").GetComponent<Image>();
        mitchBathText = mitchBathImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        mitchBathShadowText = mitchBathImage.transform.Find("CostShadow").GetComponent<TextMeshProUGUI>();
        maiBathText = maiBathImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        maiBathShadowText = maiBathImage.transform.Find("CostShadow").GetComponent<TextMeshProUGUI>();
        
        mitchTadpoleImage = kioskHqImage.transform.Find("Mitch").GetComponent<Image>();
        maiTadpoleImage = kioskHqImage.transform.Find("Mai").GetComponent<Image>();
        boughtMitchTadpoleImage = mitchTadpoleImage.transform.Find("Bought").GetComponent<Image>();
        boughtMaiTadpoleImage = maiTadpoleImage.transform.Find("Bought").GetComponent<Image>();
        mitchTadpoleText = mitchTadpoleImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        mitchTadpoleShadowText = mitchTadpoleImage.transform.Find("CostShadow").GetComponent<TextMeshProUGUI>();
        maiTadpoleText = maiTadpoleImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        maiTadpoleShadowText = maiTadpoleImage.transform.Find("CostShadow").GetComponent<TextMeshProUGUI>();
        
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
        // Mitch & Mai
        boughtMitchGardenImage.enabled = false; 
        boughtMaiGardenImage.enabled = false;
        boughtMitchHairballImage.enabled = false; 
        boughtMaiHairballImage.enabled = false;
        boughtMitchTurbineImage.enabled = false; 
        boughtMaiTurbineImage.enabled = false;
        boughtMitchSalmonImage.enabled = false; 
        boughtMaiSalmonImage.enabled = false;
        boughtMitchPoolImage.enabled = false; 
        boughtMaiPoolImage.enabled = false;
        boughtMitchBathImage.enabled = false;
        boughtMaiBathImage.enabled = false;
        boughtMitchTadpoleImage.enabled = false;
        boughtMaiTadpoleImage.enabled = false;

        uiHider = transform.Find("TrackerKiosk")?.gameObject.AddComponent<scrUIhider>();
        if (uiHider != null)
        {
            var reference = GameObject.Find("UI/Apple Displayer").GetComponent<scrUIhider>();
            uiHider.useAlphaCurve = reference.useAlphaCurve;
            uiHider.alphaCurve = reference.alphaCurve;
            uiHider.animationCurve = reference.animationCurve;
            uiHider.duration = 0.5f;
            uiHider.hideOffset = new Vector3(0, -350, 0);
            TrackerDisplayerPatch.KioskUI = uiHider;
        }
    }

    public void Update()
    {
        if (!Plugin.saveReady) return;
        if (ArchipelagoData.slotData == null) return;
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
        kioskHomeShadowText.text = kioskHomeText.text;
        kioskHcShadowText.text = kioskHcText.text;
        kioskTtShadowText.text = kioskTtText.text;
        kioskSfcShadowText.text = kioskSfcText.text;
        kioskPpShadowText.text = kioskPpText.text;
        kioskBathShadowText.text = kioskBathText.text;
        kioskHqShadowText.text = kioskHqText.text;

        if (ArchipelagoMenu.CassetteSpoiler)
        {
            mitchHairballText.text = scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("Hint0") 
                ? (int.Parse(ArchipelagoData.slotData["chc1"].ToString())*5).ToString() : "??";
            maiHairballText.text = scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("Hint1") 
                ? (int.Parse(ArchipelagoData.slotData["chc2"].ToString())*5).ToString() : "??";
            mitchTurbineText.text = scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("Hint2") 
                ? (int.Parse(ArchipelagoData.slotData["ctt1"].ToString())*5).ToString() : "??";
            maiTurbineText.text = scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("Hint3") 
                ? (int.Parse(ArchipelagoData.slotData["ctt2"].ToString())*5).ToString() : "??";
            mitchSalmonText.text = scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("Hint4") 
                ? (int.Parse(ArchipelagoData.slotData["csfc1"].ToString())*5).ToString() : "??";
            maiSalmonText.text = scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("Hint5") 
                ? (int.Parse(ArchipelagoData.slotData["csfc2"].ToString())*5).ToString() : "??";
            maiPoolText.text = scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("Hint6") 
                ? (int.Parse(ArchipelagoData.slotData["cpp2"].ToString())*5).ToString() : "??";
            mitchPoolText.text = scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("Hint7") 
                ? (int.Parse(ArchipelagoData.slotData["cpp1"].ToString())*5).ToString() : "??";
            mitchBathText.text = scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("Hint8") 
                ? (int.Parse(ArchipelagoData.slotData["cbath1"].ToString())*5).ToString() : "??";
            maiBathText.text = scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("Hint9") 
                ? (int.Parse(ArchipelagoData.slotData["cbath2"].ToString())*5).ToString() : "??";
            maiTadpoleText.text = scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("Hint10") 
                ? (int.Parse(ArchipelagoData.slotData["chq2"].ToString())*5).ToString() : "??";
            mitchTadpoleText.text = scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("Hint11") 
                ? (int.Parse(ArchipelagoData.slotData["chq1"].ToString())*5).ToString() : "??";
            maiGardenText.text = scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("Hint12") 
                ? (int.Parse(ArchipelagoData.slotData["cgg2"].ToString())*5).ToString() : "??";
            mitchGardenText.text = scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("Hint13") 
                ? (int.Parse(ArchipelagoData.slotData["cgg1"].ToString())*5).ToString() : "??";
        }
        else
        {
            mitchGardenText.text = (int.Parse(ArchipelagoData.slotData["cgg1"].ToString())*5).ToString();
            maiGardenText.text = (int.Parse(ArchipelagoData.slotData["cgg2"].ToString())*5).ToString();
            mitchTadpoleText.text = (int.Parse(ArchipelagoData.slotData["chq1"].ToString())*5).ToString();
            maiTadpoleText.text = (int.Parse(ArchipelagoData.slotData["chq2"].ToString())*5).ToString();
            mitchBathText.text = (int.Parse(ArchipelagoData.slotData["cbath1"].ToString())*5).ToString();
            maiBathText.text = (int.Parse(ArchipelagoData.slotData["cbath2"].ToString())*5).ToString();
            mitchPoolText.text = (int.Parse(ArchipelagoData.slotData["cpp1"].ToString())*5).ToString();
            maiPoolText.text = (int.Parse(ArchipelagoData.slotData["cpp2"].ToString())*5).ToString();
            mitchSalmonText.text = (int.Parse(ArchipelagoData.slotData["csfc1"].ToString())*5).ToString();
            maiSalmonText.text = (int.Parse(ArchipelagoData.slotData["csfc2"].ToString())*5).ToString();
            mitchTurbineText.text = (int.Parse(ArchipelagoData.slotData["ctt1"].ToString())*5).ToString();
            maiTurbineText.text = (int.Parse(ArchipelagoData.slotData["ctt2"].ToString())*5).ToString();
            mitchHairballText.text = (int.Parse(ArchipelagoData.slotData["chc1"].ToString())*5).ToString();
            maiHairballText.text = (int.Parse(ArchipelagoData.slotData["chc2"].ToString())*5).ToString();
        }
        mitchGardenShadowText.text = mitchGardenText.text; 
        maiGardenShadowText.text = maiGardenText.text;
        mitchHairballShadowText.text = mitchHairballText.text;
        maiHairballShadowText.text = maiHairballText.text;
        mitchTurbineShadowText.text = mitchTurbineText.text; 
        maiTurbineShadowText.text = maiTurbineText.text;
        mitchSalmonShadowText.text = mitchSalmonText.text; 
        maiSalmonShadowText.text = maiSalmonText.text;
        mitchPoolShadowText.text = mitchPoolText.text; 
        maiPoolShadowText.text = maiPoolText.text;
        mitchBathShadowText.text = mitchBathText.text; 
        maiBathShadowText.text = maiBathText.text;
        mitchTadpoleShadowText.text = mitchTadpoleText.text; 
        maiTadpoleShadowText.text = maiTadpoleText.text;
        
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

        for (var i = 1; i < 8; i++)
        {
            if (scrGameSaveManager.instance.gameData.worldsData[i].coinFlags.Contains("cassetteCoin"))
            {
                switch (i)
                {
                    case 1:
                        mitchHairballImage.color = new Color(1f, 1f, 1f, 1f);
                        mitchHairballText.enabled = false;
                        boughtMitchHairballImage.enabled = true;
                        break;
                    case 2:
                        mitchTurbineImage.color = new Color(1f, 1f, 1f, 1f);
                        mitchTurbineText.enabled = false;
                        boughtMitchTurbineImage.enabled = true;
                        break;
                    case 3:
                        mitchSalmonImage.color = new Color(1f, 1f, 1f, 1f);
                        mitchSalmonText.enabled = false;
                        boughtMitchSalmonImage.enabled = true;
                        break;
                    case 4:
                        mitchPoolImage.color = new Color(1f, 1f, 1f, 1f);
                        mitchPoolText.enabled = false;
                        boughtMitchPoolImage.enabled = true;
                        break;
                    case 5:
                        mitchBathImage.color = new Color(1f, 1f, 1f, 1f);
                        mitchBathText.enabled = false;
                        boughtMitchBathImage.enabled = true;
                        break;
                    case 6:
                        mitchTadpoleImage.color = new Color(1f, 1f, 1f, 1f);
                        mitchTadpoleText.enabled = false;
                        boughtMitchTadpoleImage.enabled = true;
                        break;
                    case 7:
                        mitchGardenImage.color = new Color(1f, 1f, 1f, 1f);
                        mitchGardenText.enabled = false;
                        boughtMitchGardenImage.enabled = true;
                        break;
                }
            }
            if (scrGameSaveManager.instance.gameData.worldsData[i].coinFlags.Contains("cassetteCoin2"))
            {
                switch (i)
                {
                    case 1:
                        maiHairballImage.color = new Color(1f, 1f, 1f, 1f);
                        maiHairballText.enabled = false;
                        boughtMaiHairballImage.enabled = true;
                        break;
                    case 2:
                        maiTurbineImage.color = new Color(1f, 1f, 1f, 1f);
                        maiTurbineText.enabled = false;
                        boughtMaiTurbineImage.enabled = true;
                        break;
                    case 3:
                        maiSalmonImage.color = new Color(1f, 1f, 1f, 1f);
                        maiSalmonText.enabled = false;
                        boughtMaiSalmonImage.enabled = true;
                        break;
                    case 4:
                        maiPoolImage.color = new Color(1f, 1f, 1f, 1f);
                        maiPoolText.enabled = false;
                        boughtMaiPoolImage.enabled = true;
                        break;
                    case 5:
                        maiBathImage.color = new Color(1f, 1f, 1f, 1f);
                        maiBathText.enabled = false;
                        boughtMaiBathImage.enabled = true;
                        break;
                    case 6:
                        maiTadpoleImage.color = new Color(1f, 1f, 1f, 1f);
                        maiTadpoleText.enabled = false;
                        boughtMaiTadpoleImage.enabled = true;
                        break;
                    case 7:
                        maiGardenImage.color = new Color(1f, 1f, 1f, 1f);
                        maiGardenText.enabled = false;
                        boughtMaiGardenImage.enabled = true;
                        break;
                }
            }
        }
    }
}