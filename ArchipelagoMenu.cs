using System.Collections;
using System.IO;
using BepInEx;
using KinematicCharacterController.Core;
using Newtonsoft.Json;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;

namespace NikoArchipelago;

public class ArchipelagoMenu : MonoBehaviour
{
    public GameObject formPanel; 
    public GameObject connectionPanel;
    public Button openFormButton; 
    public TMP_InputField serverAddressField;
    public TMP_InputField slotNameField;
    public Graphic slotNamePlaceholder;
    public TMP_Text slotNameTextComponent;
    public TMP_InputField passwordField;
    public Toggle rememberMeToggle;
    public Tooltip rememberMeTooltip;
    public TooltipTrigger rememberMeTrigger;
    public Toggle chatToggle;
    public Tooltip chatTooltip;
    public TooltipTrigger chatTrigger;
    public GameObject chatHighlight;
    public Toggle hintsToggle;
    public Tooltip hintsTooltip;
    public TooltipTrigger hintsTrigger;
    public GameObject hintsHighlight;
    public Toggle shopHintsToggle;
    public Tooltip shopHintsTooltip;
    public TooltipTrigger shopHintsTrigger;
    public GameObject shopHintsHighlight;
    public Toggle statusToggle;
    public Tooltip statusTooltip;
    public TooltipTrigger statusTrigger;
    public GameObject statusHighlight;
    public Toggle ticketToggle;
    public Tooltip ticketTooltip;
    public TooltipTrigger ticketTrigger;
    public GameObject ticketHighlight;
    public Toggle kioskToggle;
    public Tooltip kioskTooltip;
    public TooltipTrigger kioskTrigger;
    public GameObject kioskHighlight;
    public Toggle kioskSpoilerToggle;
    public Tooltip kioskSpoilerTooltip;
    public TooltipTrigger kioskSpoilerTrigger;
    public GameObject kioskSpoilerHighlight;
    public Toggle contactListToggle;
    public Tooltip contactListTooltip;
    public TooltipTrigger contactListTrigger;
    public GameObject contactListHighlight;
    public Toggle cacmiToggle;
    public Tooltip cacmiTooltip;
    public TooltipTrigger cacmiTrigger;
    public GameObject cacmiHighlight;
    public Toggle kalmiToggle;
    public Tooltip kalmiTooltip;
    public TooltipTrigger kalmiTrigger;
    public GameObject kalmiHighlight;
    public Toggle itemSentToggle;
    public Tooltip itemSentTooltip;
    public TooltipTrigger itemSentTrigger;
    public GameObject itemSentHighlight;
    public Toggle cassetteSpoilerToggle;
    public Tooltip cassetteSpoilerTooltip;
    public TooltipTrigger cassetteSpoilerTrigger;
    public GameObject cassetteSpoilerHighlight;
    //public Toggle trackerKeyToggle;
    //public Tooltip trackerKeyTooltip;
    //public TooltipTrigger trackerKeyTrigger;
    //public GameObject trackerKeyHighlight;
    public Toggle seasonalThemesToggle;
    public Tooltip seasonalThemesTooltip;
    public TooltipTrigger seasonalThemesTrigger;
    public GameObject seasonalThemesHighlight;
    public Toggle skipPickupToggle;
    public Tooltip skipPickupTooltip;
    public TooltipTrigger skipPickupTrigger;
    public GameObject skipPickupHighlight;
    public Button connectButton;
    public TMP_Text versionText;
    private static scrGameSaveManager gameSaveManager;
    private static ArchipelagoClient ArchipelagoClient;
    private static string _serverAddress;
    private static string _slotName;
    private string password;
    private static bool _rememberMe;
    private static bool _chat;
    private static bool _hints;
    private static bool _shopHints;
    private static bool _ticket;
    private static bool _kiosk;
    private static bool _kioskSpoiler;
    private static bool _cassetteSpoiler;
    private static bool _cacmi;
    private static bool _kalmi;
    private static bool _itemSent;
    private static bool _contactList;
    private static bool _status;
    //private static bool _trackerKey;
    private static bool _tooltips;
    private static bool _seasonalThemes;
    private static bool _skipPickup;
    private readonly string jsonFilePath = Path.Combine(Paths.PluginPath, "APSavedSettings.json");
    private GameObject apButtonGameObject;
    public static string Seed;
    public static bool Hints;
    public static bool ShopHints;
    public static bool Chat;
    public static bool Ticket;
    public static bool Kiosk;
    public static bool KioskSpoiler;
    public static bool cacmi;
    public static bool kalmi;
    public static bool contactList;
    public static bool itemSent;
    public static bool status;
    public static bool CassetteSpoiler;
    //public static bool TrackerKey;
    public static bool Tooltips;
    public static bool SeasonalThemes;
    public static bool SkipPickup;
    
    // New Menu stuff
    public GameObject settingsPanel;
    public Tooltip settingsTooltip;
    public GameObject trackersPanel;
    public Tooltip trackersTooltip;
    public GameObject qolPanel;
    public Tooltip qolTooltip;
    public CanvasGroup settingsPanelCanvasGroup;
    public CanvasGroup trackersPanelCanvasGroup;
    public CanvasGroup qolPanelCanvasGroup;
    public float fadeDuration = 0.5f;

    private CanvasGroup _activePanel;    
    public Button settingsButton;
    public TooltipTrigger settingsTrigger;
    public Button trackersButton;
    public TooltipTrigger trackersTrigger;
    public Button qolButton;
    public TooltipTrigger qolTrigger;
    
    // Information stuff
    public GameObject informationPanel;
    public GameObject kioskAndTicketPanel;
    public GameObject keysPanel;
    public GameObject fishPanel;
    public Button debugButton;
    public TooltipTrigger debugTrigger;
    public Tooltip debugTooltip;
    public Button reloadButton;
    public TooltipTrigger reloadTrigger;
    public Tooltip reloadTooltip;
    public Image ticketHcImage;
    public Image ticketTtImage;
    public Image ticketSfcImage;
    public Image ticketPpImage;
    public Image ticketBathImage;
    public Image ticketHqImage;
    public Image ticketGgImage;
    public Image ticketCl1Image;
    public Image ticketCl2Image;
    public Image kioskHomeImage;
    public Image kioskHomeCostImage;
    public Image kioskHomeCostBackImage;
    public Image kioskHcCostImage;
    public Image kioskHcCostBackImage;
    public Image kioskTtCostImage;
    public Image kioskTtCostBackImage;
    public Image kioskSfcCostImage;
    public Image kioskSfcCostBackImage;
    public Image kioskPpCostImage;
    public Image kioskPpCostBackImage;
    public Image kioskBathCostImage;
    public Image kioskBathCostBackImage;
    public Image kioskHqCostImage;
    public Image kioskHqCostBackImage;
    public Image boughtHomeImage;
    public Image boughtHomeBackImage;
    public Image boughtHcImage;
    public Image boughtHcBackImage;
    public Image boughtTtImage;
    public Image boughtTtBackImage;
    public Image boughtSfcImage;
    public Image boughtSfcBackImage;
    public Image boughtPpImage;
    public Image boughtPpBackImage;
    public Image boughtBathImage;
    public Image boughtBathBackImage;
    public Image boughtHqImage;
    public Image boughtHqBackImage;
    public TextMeshProUGUI kioskHomeText;
    public TextMeshProUGUI kioskHomeBackText;
    public TextMeshProUGUI kioskHcText;
    public TextMeshProUGUI kioskHcBackText;
    public TextMeshProUGUI kioskTtText;
    public TextMeshProUGUI kioskTtBackText;
    public TextMeshProUGUI kioskSfcText;
    public TextMeshProUGUI kioskSfcBackText;
    public TextMeshProUGUI kioskPpText;
    public TextMeshProUGUI kioskPpBackText;
    public TextMeshProUGUI kioskBathText;
    public TextMeshProUGUI kioskBathBackText;
    public TextMeshProUGUI kioskHqText;
    public TextMeshProUGUI kioskHqBackText;
    public TextMeshProUGUI keyCountBackText;
    public TextMeshProUGUI keyCountFrontText;
    public TextMeshProUGUI coinCountBackText;
    public TextMeshProUGUI coinCountFrontText;
    public TextMeshProUGUI cassetteCountBackText;
    public TextMeshProUGUI cassetteCountFrontText;
    public TextMeshProUGUI keyHcCountBackText;
    public TextMeshProUGUI keyHcCountFrontText;
    public TextMeshProUGUI keyTtCountBackText;
    public TextMeshProUGUI keyTtCountFrontText;
    public TextMeshProUGUI keySfcCountBackText;
    public TextMeshProUGUI keySfcCountFrontText;
    public TextMeshProUGUI keyPpCountBackText;
    public TextMeshProUGUI keyPpCountFrontText;
    public TextMeshProUGUI keyBathCountBackText;
    public TextMeshProUGUI keyBathCountFrontText;
    public TextMeshProUGUI keyHqCountBackText;
    public TextMeshProUGUI keyHqCountFrontText;
    public TextMeshProUGUI fishHcCountBackText;
    public TextMeshProUGUI fishHcCountFrontText;
    public TextMeshProUGUI fishTtCountBackText;
    public TextMeshProUGUI fishTtCountFrontText;
    public TextMeshProUGUI fishSfcCountBackText;
    public TextMeshProUGUI fishSfcCountFrontText;
    public TextMeshProUGUI fishPpCountBackText;
    public TextMeshProUGUI fishPpCountFrontText;
    public TextMeshProUGUI fishBathCountBackText;
    public TextMeshProUGUI fishBathCountFrontText;
    public TextMeshProUGUI fishHqCountBackText;
    public TextMeshProUGUI fishHqCountFrontText;
    public Image keysDisabledImage;
    public Image fishDisabledImage;
    public TextMeshProUGUI goalText;
    public TooltipTrigger goalTrigger;
    public Tooltip goalTooltip;
    public TextMeshProUGUI goalTooltipText;
    public TooltipTrigger keysDisabledTrigger;
    public Tooltip keysDisabledTooltip;
    public TooltipTrigger fishDisabledTrigger;
    public Tooltip fishDisabledTooltip;
    public TooltipTrigger keysTrigger;
    public Tooltip keysTooltip;
    public TooltipTrigger fishTrigger;
    public Tooltip fishTooltip;
    public Image boughtHcFishImage;
    public Image boughtTtFishImage;
    public Image boughtScfFishImage;
    public Image boughtPpFishImage;
    public Image boughtBathFishImage;
    public Image boughtHqFishImage;
    public Toggle tooltipsToggle;
    public Tooltip tooltipsTooltip;
    public TooltipTrigger tooltipsTrigger;
    public static bool forceDebug;
    public static bool hideOnce;
    public Image mitchHairballImage;
    public Image maiHairballImage;
    public Image mitchTurbineImage;
    public Image maiTurbineImage;
    public Image mitchSalmonImage;
    public Image maiSalmonImage;
    public Image mitchPoolImage;
    public Image maiPoolImage;
    public Image mitchBathImage;
    public Image maiBathImage;
    public Image mitchTadpoleImage;
    public Image maiTadpoleImage;
    public Image mitchGardenImage;
    public Image maiGardenImage;
    public Image gardenDisabledImage;
    public static bool pressedConnect;
    
    public void Start()
    {
        gameSaveManager = scrGameSaveManager.instance;
        formPanel = transform.Find("Panel").gameObject;
        openFormButton = transform.Find("APButton").gameObject.GetComponent<Button>();
        apButtonGameObject = transform.Find("APButton").gameObject;
        connectionPanel = formPanel.transform.Find("Connection").gameObject;
        serverAddressField = connectionPanel.transform.Find("ServerAddress").gameObject.AddComponent<TMP_InputField>();
        serverAddressField.textViewport = serverAddressField.transform.Find("Text Area").GetComponent<RectTransform>();
        serverAddressField.placeholder = serverAddressField.transform.Find("Text Area/Placeholder").GetComponent<Graphic>();
        serverAddressField.textComponent = serverAddressField.transform.Find("Text Area/Text").GetComponent<TMP_Text>();
        slotNameField = connectionPanel.transform.Find("SlotName").gameObject.AddComponent<TMP_InputField>();
        slotNameField.textViewport = slotNameField.transform.Find("Text Area").GetComponent<RectTransform>();
        slotNameField.placeholder = slotNameField.transform.Find("Text Area/Placeholder").GetComponent<Graphic>();
        slotNameField.textComponent = slotNameField.transform.Find("Text Area/Text").GetComponent<TMP_Text>();
        passwordField = connectionPanel.transform.Find("Password").gameObject.AddComponent<TMP_InputField>();
        passwordField.contentType = TMP_InputField.ContentType.Password;
        passwordField.textViewport = passwordField.transform.Find("Text Area").GetComponent<RectTransform>();
        passwordField.placeholder = passwordField.transform.Find("Text Area/Placeholder").GetComponent<Graphic>();
        passwordField.textComponent = passwordField.transform.Find("Text Area/Text").GetComponent<TMP_Text>();
        rememberMeToggle = formPanel.transform.Find("Remember").gameObject.GetComponent<Toggle>();
        rememberMeTrigger = rememberMeToggle.gameObject.AddComponent<TooltipTrigger>();
        rememberMeTooltip = rememberMeToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        connectButton = connectionPanel.transform.Find("Button").gameObject.GetComponent<Button>();
        versionText = formPanel.transform.Find("Version").gameObject.GetComponent<TMP_Text>();
        tooltipsToggle = formPanel.transform.Find("Tooltips").gameObject.GetComponent<Toggle>();
        tooltipsTrigger = tooltipsToggle.gameObject.AddComponent<TooltipTrigger>();
        tooltipsTooltip = tooltipsToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        seasonalThemesToggle = formPanel.transform.Find("Seasonal").gameObject.GetComponent<Toggle>();
        seasonalThemesTrigger = seasonalThemesToggle.gameObject.AddComponent<TooltipTrigger>();
        seasonalThemesTooltip = seasonalThemesToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();

        serverAddressField.selectionColor = new Color(1,(float)0.5801887,(float)0.9300315,(float)0.7529412);
        slotNameField.selectionColor = new Color(1,(float)0.5801887,(float)0.9300315,(float)0.7529412);
        passwordField.selectionColor = new Color(1,(float)0.5801887,(float)0.9300315,(float)0.7529412);
        
        // Information, when logged in
        informationPanel = formPanel.transform.Find("InformationScreen").gameObject;
        kioskAndTicketPanel = informationPanel.transform.Find("KioskAndTicketScreen").gameObject;
        keysPanel = informationPanel.transform.Find("KeysScreen").gameObject;
        fishPanel = informationPanel.transform.Find("FishScreen").gameObject;
        debugButton = informationPanel.transform.Find("DEBUG").gameObject.GetComponent<Button>();
        debugTooltip = informationPanel.transform.Find("DEBUGTooltip").gameObject.AddComponent<Tooltip>();
        debugTrigger = debugButton.gameObject.AddComponent<TooltipTrigger>();
        reloadButton = informationPanel.transform.Find("ReloadButton").gameObject.GetComponent<Button>();
        reloadTooltip = informationPanel.transform.Find("TooltipReload").gameObject.AddComponent<Tooltip>();
        reloadTrigger = reloadButton.gameObject.AddComponent<TooltipTrigger>();
        keysDisabledImage = informationPanel.transform.Find("KeysDisabled").gameObject.GetComponent<Image>();
        keysDisabledTrigger = keysDisabledImage.gameObject.AddComponent<TooltipTrigger>();
        keysDisabledTooltip = keysDisabledImage.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        fishDisabledImage = informationPanel.transform.Find("FishDisabled").gameObject.GetComponent<Image>();
        fishDisabledTrigger = fishDisabledImage.gameObject.AddComponent<TooltipTrigger>();
        fishDisabledTooltip = fishDisabledImage.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        goalText = informationPanel.transform.Find("Goal/Text").GetComponent<TextMeshProUGUI>();
        goalTrigger = informationPanel.transform.Find("Goal").gameObject.AddComponent<TooltipTrigger>();
        goalTooltip = informationPanel.transform.Find("TooltipGoal").gameObject.AddComponent<Tooltip>();
        goalTooltipText = goalTooltip.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        keysTrigger = keysPanel.gameObject.AddComponent<TooltipTrigger>();
        keysTooltip = keysPanel.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        fishTrigger = fishPanel.gameObject.AddComponent<TooltipTrigger>();
        fishTooltip = fishPanel.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        // Home
        kioskHomeImage = kioskAndTicketPanel.transform.Find("KioskHome").GetComponent<Image>();
        boughtHomeImage = kioskHomeImage.transform.Find("Bought").GetComponent<Image>();
        kioskHomeCostImage = kioskHomeImage.transform.Find("CostHome").GetComponent<Image>();
        kioskHomeText = kioskHomeCostImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        boughtHomeBackImage = kioskAndTicketPanel.transform.Find("KioskHomeBack/Bought").GetComponent<Image>();
        kioskHomeCostBackImage = kioskAndTicketPanel.transform.Find("KioskHomeBack/CostHome").GetComponent<Image>();
        kioskHomeBackText = kioskAndTicketPanel.transform.Find("KioskHomeBack/CostHome/Cost").GetComponent<TextMeshProUGUI>();
        // Hairball
        ticketHcImage = kioskAndTicketPanel.transform.Find("TicketHairball").GetComponent<Image>();
        boughtHcImage = ticketHcImage.transform.Find("Bought").GetComponent<Image>();
        kioskHcCostImage = ticketHcImage.transform.Find("CostHairball").GetComponent<Image>();
        kioskHcText = kioskHcCostImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        boughtHcBackImage = kioskAndTicketPanel.transform.Find("TicketHairballBack/Bought").GetComponent<Image>();
        kioskHcCostBackImage = kioskAndTicketPanel.transform.Find("TicketHairballBack/CostHairball").GetComponent<Image>();
        kioskHcBackText = kioskAndTicketPanel.transform.Find("TicketHairballBack/CostHairball/Cost").GetComponent<TextMeshProUGUI>();
        keyHcCountBackText = keysPanel.transform.Find("HCKeyCount/Back").GetComponent<TextMeshProUGUI>();
        keyHcCountFrontText = keysPanel.transform.Find("HCKeyCount/Front").GetComponent<TextMeshProUGUI>();
        fishHcCountBackText = fishPanel.transform.Find("HCFishCount/Back").GetComponent<TextMeshProUGUI>();
        fishHcCountFrontText = fishPanel.transform.Find("HCFishCount/Front").GetComponent<TextMeshProUGUI>();
        boughtHcFishImage = fishPanel.transform.Find("HCComplete").gameObject.GetComponent<Image>();
        mitchHairballImage = ticketHcImage.transform.Find("Mitch").GetComponent<Image>();
        maiHairballImage = ticketHcImage.transform.Find("Mai").GetComponent<Image>();
        // Turbine
        ticketTtImage = kioskAndTicketPanel.transform.Find("TicketTurbine").GetComponent<Image>();
        boughtTtImage = ticketTtImage.transform.Find("Bought").GetComponent<Image>();
        kioskTtCostImage = ticketTtImage.transform.Find("CostTurbine").GetComponent<Image>();
        kioskTtText = kioskTtCostImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        boughtTtBackImage = kioskAndTicketPanel.transform.Find("TicketTurbineBack/Bought").GetComponent<Image>();
        kioskTtCostBackImage = kioskAndTicketPanel.transform.Find("TicketTurbineBack/CostTurbine").GetComponent<Image>();
        kioskTtBackText = kioskAndTicketPanel.transform.Find("TicketTurbineBack/CostTurbine/Cost").GetComponent<TextMeshProUGUI>();
        keyTtCountBackText = keysPanel.transform.Find("TTKeyCount/Back").GetComponent<TextMeshProUGUI>();
        keyTtCountFrontText = keysPanel.transform.Find("TTKeyCount/Front").GetComponent<TextMeshProUGUI>();
        fishTtCountBackText = fishPanel.transform.Find("TTFishCount/Back").GetComponent<TextMeshProUGUI>();
        fishTtCountFrontText = fishPanel.transform.Find("TTFishCount/Front").GetComponent<TextMeshProUGUI>();
        boughtTtFishImage = fishPanel.transform.Find("TTComplete").gameObject.GetComponent<Image>();
        mitchTurbineImage = ticketTtImage.transform.Find("Mitch").GetComponent<Image>();
        maiTurbineImage = ticketTtImage.transform.Find("Mai").GetComponent<Image>();
        // Salmon
        ticketSfcImage = kioskAndTicketPanel.transform.Find("TicketSalmon").GetComponent<Image>();
        boughtSfcImage = ticketSfcImage.transform.Find("Bought").GetComponent<Image>();
        kioskSfcCostImage = ticketSfcImage.transform.Find("CostSalmon").GetComponent<Image>();
        kioskSfcText = kioskSfcCostImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        boughtSfcBackImage = kioskAndTicketPanel.transform.Find("TicketSalmonBack/Bought").GetComponent<Image>();
        kioskSfcCostBackImage = kioskAndTicketPanel.transform.Find("TicketSalmonBack/CostSalmon").GetComponent<Image>();
        kioskSfcBackText = kioskAndTicketPanel.transform.Find("TicketSalmonBack/CostSalmon/Cost").GetComponent<TextMeshProUGUI>();
        keySfcCountBackText = keysPanel.transform.Find("SFCKeyCount/Back").GetComponent<TextMeshProUGUI>();
        keySfcCountFrontText = keysPanel.transform.Find("SFCKeyCount/Front").GetComponent<TextMeshProUGUI>();
        fishSfcCountBackText = fishPanel.transform.Find("SFCFishCount/Back").GetComponent<TextMeshProUGUI>();
        fishSfcCountFrontText = fishPanel.transform.Find("SFCFishCount/Front").GetComponent<TextMeshProUGUI>();
        boughtScfFishImage = fishPanel.transform.Find("SCFComplete").gameObject.GetComponent<Image>();
        mitchSalmonImage = ticketSfcImage.transform.Find("Mitch").GetComponent<Image>();
        maiSalmonImage = ticketSfcImage.transform.Find("Mai").GetComponent<Image>();
        // Pool
        ticketPpImage = kioskAndTicketPanel.transform.Find("TicketPool").GetComponent<Image>();
        boughtPpImage = ticketPpImage.transform.Find("Bought").GetComponent<Image>();
        kioskPpCostImage = ticketPpImage.transform.Find("CostPool").GetComponent<Image>();
        kioskPpText = kioskPpCostImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        boughtPpBackImage = kioskAndTicketPanel.transform.Find("TicketPoolBack/Bought").GetComponent<Image>();
        kioskPpCostBackImage = kioskAndTicketPanel.transform.Find("TicketPoolBack/CostPool").GetComponent<Image>();
        kioskPpBackText = kioskAndTicketPanel.transform.Find("TicketPoolBack/CostPool/Cost").GetComponent<TextMeshProUGUI>();
        keyPpCountBackText = keysPanel.transform.Find("PPKeyCount/Back").GetComponent<TextMeshProUGUI>();
        keyPpCountFrontText = keysPanel.transform.Find("PPKeyCount/Front").GetComponent<TextMeshProUGUI>();
        fishPpCountBackText = fishPanel.transform.Find("PPFishCount/Back").GetComponent<TextMeshProUGUI>();
        fishPpCountFrontText = fishPanel.transform.Find("PPFishCount/Front").GetComponent<TextMeshProUGUI>();
        boughtPpFishImage = fishPanel.transform.Find("PPComplete").gameObject.GetComponent<Image>();
        mitchPoolImage = ticketPpImage.transform.Find("Mitch").GetComponent<Image>();
        maiPoolImage = ticketPpImage.transform.Find("Mai").GetComponent<Image>();
        // Bath
        ticketBathImage = kioskAndTicketPanel.transform.Find("TicketBath").GetComponent<Image>();
        boughtBathImage = ticketBathImage.transform.Find("Bought").GetComponent<Image>();
        kioskBathCostImage = ticketBathImage.transform.Find("CostBath").GetComponent<Image>();
        kioskBathText = kioskBathCostImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        boughtBathBackImage = kioskAndTicketPanel.transform.Find("TicketBathBack/Bought").GetComponent<Image>();
        kioskBathCostBackImage = kioskAndTicketPanel.transform.Find("TicketBathBack/CostBath").GetComponent<Image>();
        kioskBathBackText = kioskAndTicketPanel.transform.Find("TicketBathBack/CostBath/Cost").GetComponent<TextMeshProUGUI>();
        keyBathCountBackText = keysPanel.transform.Find("BATHKeyCount/Back").GetComponent<TextMeshProUGUI>();
        keyBathCountFrontText = keysPanel.transform.Find("BATHKeyCount/Front").GetComponent<TextMeshProUGUI>();
        fishBathCountBackText = fishPanel.transform.Find("BATHFishCount/Back").GetComponent<TextMeshProUGUI>();
        fishBathCountFrontText = fishPanel.transform.Find("BATHFishCount/Front").GetComponent<TextMeshProUGUI>();
        boughtBathFishImage = fishPanel.transform.Find("BATHComplete").gameObject.GetComponent<Image>();
        mitchBathImage = ticketBathImage.transform.Find("Mitch").GetComponent<Image>();
        maiBathImage = ticketBathImage.transform.Find("Mai").GetComponent<Image>();
        // Tadpole
        ticketHqImage = kioskAndTicketPanel.transform.Find("TicketTadpole").GetComponent<Image>();
        boughtHqImage = ticketHqImage.transform.Find("Bought").GetComponent<Image>();
        kioskHqCostImage = ticketHqImage.transform.Find("CostTadpole").GetComponent<Image>();
        kioskHqText = kioskHqCostImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        boughtHqBackImage = kioskAndTicketPanel.transform.Find("TicketTadpoleBack/Bought").GetComponent<Image>();
        kioskHqCostBackImage = kioskAndTicketPanel.transform.Find("TicketTadpoleBack/CostTadpole").GetComponent<Image>();
        kioskHqBackText = kioskAndTicketPanel.transform.Find("TicketTadpoleBack/CostTadpole/Cost").GetComponent<TextMeshProUGUI>();
        keyHqCountBackText = keysPanel.transform.Find("HQKeyCount/Back").GetComponent<TextMeshProUGUI>();
        keyHqCountFrontText = keysPanel.transform.Find("HQKeyCount/Front").GetComponent<TextMeshProUGUI>();
        fishHqCountBackText = fishPanel.transform.Find("HQFishCount/Back").GetComponent<TextMeshProUGUI>();
        fishHqCountFrontText = fishPanel.transform.Find("HQFishCount/Front").GetComponent<TextMeshProUGUI>();
        boughtHqFishImage = fishPanel.transform.Find("HQComplete").gameObject.GetComponent<Image>();
        mitchTadpoleImage = ticketHqImage.transform.Find("Mitch").GetComponent<Image>();
        maiTadpoleImage = ticketHqImage.transform.Find("Mai").GetComponent<Image>();
        // Misc
        ticketGgImage = kioskAndTicketPanel.transform.Find("TicketGarden").GetComponent<Image>();
        mitchGardenImage = ticketGgImage.transform.Find("Mitch").GetComponent<Image>();
        maiGardenImage = ticketGgImage.transform.Find("Mai").GetComponent<Image>();
        gardenDisabledImage = ticketGgImage.transform.Find("Disabled").GetComponent<Image>();
        ticketCl1Image = informationPanel.transform.Find("ContactList1").GetComponent<Image>();
        ticketCl2Image = informationPanel.transform.Find("ContactList2").GetComponent<Image>();
        
        keyCountBackText = keysPanel.transform.Find("KeyCount/Back").GetComponent<TextMeshProUGUI>();
        keyCountFrontText = keysPanel.transform.Find("KeyCount/Front").GetComponent<TextMeshProUGUI>();
        coinCountBackText = informationPanel.transform.Find("CoinCount/Back").GetComponent<TextMeshProUGUI>();
        coinCountFrontText = informationPanel.transform.Find("CoinCount/Front").GetComponent<TextMeshProUGUI>();
        cassetteCountBackText = informationPanel.transform.Find("CassetteCount/Back").GetComponent<TextMeshProUGUI>();
        cassetteCountFrontText = informationPanel.transform.Find("CassetteCount/Front").GetComponent<TextMeshProUGUI>();
        
        // Wowwwie
        formPanel.transform.Find("Nya").gameObject.AddComponent<FloatingAnimation>();
        formPanel.transform.Find("Flowers").gameObject.AddComponent<FloatingAnimation>().floatSpeed = -1f;
        formPanel.transform.Find("Frog").gameObject.AddComponent<FloatingAnimation>().floatSpeed = 0.4f;
        formPanel.transform.Find("ScrollingBackground/Image").gameObject.AddComponent<ScrollingEffect>();
        
        // Tabs
        settingsPanel = formPanel.transform.Find("settingsPanel").gameObject;
        settingsTooltip = formPanel.transform.Find("Tabs/TooltipSettings").gameObject.AddComponent<Tooltip>();
        trackersPanel = formPanel.transform.Find("trackersPanel").gameObject;
        trackersTooltip = formPanel.transform.Find("Tabs/TooltipTrackers").gameObject.AddComponent<Tooltip>();
        qolPanel = formPanel.transform.Find("qolPanel").gameObject;
        qolTooltip = formPanel.transform.Find("Tabs/TooltipQOL").gameObject.AddComponent<Tooltip>();
        
        // Settings
        settingsButton = formPanel.transform.Find("Tabs/SettingsButton").gameObject.GetComponent<Button>();
        settingsButton.gameObject.AddComponent<ButtonHoverEffect>();
        settingsTrigger = settingsButton.gameObject.AddComponent<TooltipTrigger>();
        
        chatToggle = settingsPanel.transform.Find("Chat").gameObject.GetComponent<Toggle>();
        chatTooltip = chatToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        chatTrigger = chatToggle.gameObject.AddComponent<TooltipTrigger>();
        chatHighlight = chatToggle.transform.Find("Highlight").gameObject;
        hintsToggle = settingsPanel.transform.Find("Hints").gameObject.GetComponent<Toggle>();
        hintsTooltip = hintsToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        hintsTrigger = hintsToggle.gameObject.AddComponent<TooltipTrigger>();
        hintsHighlight = hintsToggle.transform.Find("Highlight").gameObject;
        shopHintsToggle = settingsPanel.transform.Find("ShopHints").gameObject.GetComponent<Toggle>();
        shopHintsTooltip = shopHintsToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        shopHintsTrigger = shopHintsToggle.gameObject.AddComponent<TooltipTrigger>();
        shopHintsHighlight = shopHintsToggle.transform.Find("Highlight").gameObject;
        itemSentToggle = settingsPanel.transform.Find("ItemSent").gameObject.GetComponent<Toggle>();
        itemSentTooltip = itemSentToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        itemSentTrigger = itemSentToggle.gameObject.AddComponent<TooltipTrigger>();
        itemSentHighlight = itemSentToggle.transform.Find("Highlight").gameObject;
        statusToggle = settingsPanel.transform.Find("Status").gameObject.GetComponent<Toggle>();
        statusTooltip = statusToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        statusTrigger = statusToggle.gameObject.AddComponent<TooltipTrigger>();
        statusHighlight = statusToggle.transform.Find("Highlight").gameObject;
        
        // Trackers
        trackersButton = formPanel.transform.Find("Tabs/TrackersButton").gameObject.GetComponent<Button>();
        trackersButton.gameObject.AddComponent<ButtonHoverEffect>();
        trackersTrigger = trackersButton.gameObject.AddComponent<TooltipTrigger>();
        
        ticketToggle = trackersPanel.transform.Find("Ticket").gameObject.GetComponent<Toggle>();
        ticketTooltip = ticketToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        ticketTrigger = ticketToggle.gameObject.AddComponent<TooltipTrigger>();
        ticketHighlight = ticketToggle.transform.Find("Highlight").gameObject;
        kioskToggle = trackersPanel.transform.Find("Kiosk").gameObject.GetComponent<Toggle>();
        kioskTooltip = kioskToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        kioskTrigger = kioskToggle.gameObject.AddComponent<TooltipTrigger>();
        kioskHighlight = kioskToggle.transform.Find("Highlight").gameObject;
        kioskSpoilerToggle = trackersPanel.transform.Find("Spoiler").gameObject.GetComponent<Toggle>();
        kioskSpoilerTooltip = kioskSpoilerToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        kioskSpoilerTrigger = kioskSpoilerToggle.gameObject.AddComponent<TooltipTrigger>();
        kioskSpoilerHighlight = kioskSpoilerToggle.transform.Find("Highlight").gameObject;
        contactListToggle = trackersPanel.transform.Find("ContactList").gameObject.GetComponent<Toggle>();
        contactListTooltip = contactListToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        contactListTrigger = contactListToggle.gameObject.AddComponent<TooltipTrigger>();
        contactListHighlight = contactListToggle.transform.Find("Highlight").gameObject;
        //trackerKeyToggle = trackersPanel.transform.Find("Key").gameObject.GetComponent<Toggle>();
        //trackerKeyTooltip = trackerKeyToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        //trackerKeyTrigger = trackerKeyToggle.gameObject.AddComponent<TooltipTrigger>();
        //trackerKeyHighlight = trackerKeyToggle.transform.Find("Highlight").gameObject;
        cassetteSpoilerToggle = trackersPanel.transform.Find("CassetteSpoiler").gameObject.GetComponent<Toggle>();
        cassetteSpoilerTooltip = cassetteSpoilerToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        cassetteSpoilerTrigger = cassetteSpoilerToggle.gameObject.AddComponent<TooltipTrigger>();
        cassetteSpoilerHighlight = cassetteSpoilerToggle.transform.Find("Highlight").gameObject;
        
        // QOL
        qolButton = formPanel.transform.Find("Tabs/QOLButton").gameObject.GetComponent<Button>();
        qolButton.gameObject.AddComponent<ButtonHoverEffect>();
        qolTrigger = qolButton.gameObject.AddComponent<TooltipTrigger>();
        
        cacmiToggle = qolPanel.transform.Find("CACMI").gameObject.GetComponent<Toggle>();
        cacmiTooltip = cacmiToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        cacmiTrigger = cacmiToggle.gameObject.AddComponent<TooltipTrigger>();
        cacmiHighlight = cacmiToggle.transform.Find("Highlight").gameObject;
        kalmiToggle = qolPanel.transform.Find("KALMI").gameObject.GetComponent<Toggle>();
        kalmiTooltip = kalmiToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        kalmiTrigger = kalmiToggle.gameObject.AddComponent<TooltipTrigger>();
        kalmiHighlight = kalmiToggle.transform.Find("Highlight").gameObject;
        skipPickupToggle = qolPanel.transform.Find("SkipAnimation").gameObject.GetComponent<Toggle>();
        skipPickupTooltip = skipPickupToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        skipPickupTrigger = skipPickupToggle.gameObject.AddComponent<TooltipTrigger>();
        skipPickupHighlight = skipPickupToggle.transform.Find("Highlight").gameObject;
        // itemSentToggle = qolPanel.transform.Find("ItemSent").gameObject.GetComponent<Toggle>();
        // itemSentTooltip = itemSentToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        // itemSentTrigger = itemSentToggle.gameObject.AddComponent<TooltipTrigger>();
        
        if (serverAddressField == null) Plugin.BepinLogger.LogError("Server Address Field is null!");
        if (slotNameField == null) Plugin.BepinLogger.LogError("Slot Name Field is null!");
        if (passwordField == null) Plugin.BepinLogger.LogError("Password Field is null!");
        if (formPanel == null) Plugin.BepinLogger.LogError("Form Panel is null!");
        if (openFormButton == null) Plugin.BepinLogger.LogError("APButton is null!");
        if (rememberMeToggle == null) Plugin.BepinLogger.LogError("Remember is null!");
        if (chatToggle == null) Plugin.BepinLogger.LogError("Chat is null!");
        if (hintsToggle == null) Plugin.BepinLogger.LogError("Hints is null!");
        if (shopHintsToggle == null) Plugin.BepinLogger.LogError("ShopHints is null!");
        if (connectButton == null) Plugin.BepinLogger.LogError("ConnectButton is null!");
        if (versionText == null) Plugin.BepinLogger.LogError("VersionText is null!");

        _serverAddress = serverAddressField.text;
        _slotName = slotNameField.text;
        password = passwordField.text;
        _rememberMe = rememberMeToggle.isOn;
        _chat = chatToggle.isOn;
        _hints = hintsToggle.isOn;
        _shopHints = shopHintsToggle.isOn;
        _ticket = ticketToggle.isOn;
        _kiosk = kioskToggle.isOn;
        _kioskSpoiler = kioskSpoilerToggle.isOn;
        _cacmi = cacmiToggle.isOn;
        _itemSent = itemSentToggle.isOn;
        _contactList = contactListToggle.isOn;
        _status = statusToggle.isOn;
        //_trackerKey = trackerKeyToggle.isOn;
        _tooltips = tooltipsToggle.isOn;
        _seasonalThemes = seasonalThemesToggle.isOn;
        _skipPickup = skipPickupToggle.isOn;
        hideOnce = false;
        LoadData();

        versionText.text = "Version "+Plugin.PluginVersion;
        formPanel.SetActive(false);
        apButtonGameObject.SetActive(true);
        openFormButton.onClick.AddListener(ToggleFormVisibility);
        connectButton.onClick.AddListener(Connect);
        settingsButton.onClick.AddListener(ShowSettings);
        trackersButton.onClick.AddListener(ShowTrackers);
        qolButton.onClick.AddListener(ShowQOL);
        debugButton.onClick.AddListener(ToggleDebugMode);
        reloadButton.onClick.AddListener(ReloadSettings);
        
        // Tooltips
        settingsTrigger.tooltip = settingsTooltip;
        trackersTrigger.tooltip = trackersTooltip;
        qolTrigger.tooltip = qolTooltip;
        rememberMeTrigger.tooltip = rememberMeTooltip;
        chatTrigger.tooltip = chatTooltip;
        hintsTrigger.tooltip = hintsTooltip;
        shopHintsTrigger.tooltip = shopHintsTooltip;
        ticketTrigger.tooltip = ticketTooltip;
        kioskTrigger.tooltip = kioskTooltip;
        kioskSpoilerTrigger.tooltip = kioskSpoilerTooltip;
        cacmiTrigger.tooltip = cacmiTooltip;
        kalmiTrigger.tooltip = kalmiTooltip;
        itemSentTrigger.tooltip = itemSentTooltip;
        contactListTrigger.tooltip = contactListTooltip;
        statusTrigger.tooltip = statusTooltip;
        debugTrigger.tooltip = debugTooltip;
        reloadTrigger.tooltip = reloadTooltip;
        goalTrigger.tooltip = goalTooltip;
        keysDisabledTrigger.tooltip = keysDisabledTooltip;
        fishDisabledTrigger.tooltip = fishDisabledTooltip;
        keysTrigger.tooltip = keysTooltip;
        fishTrigger.tooltip = fishTooltip;
        //trackerKeyTrigger.tooltip = trackerKeyTooltip;
        tooltipsTrigger.tooltip = tooltipsTooltip;
        cassetteSpoilerTrigger.tooltip = cassetteSpoilerTooltip;
        seasonalThemesTrigger.tooltip = seasonalThemesTooltip;
        skipPickupTrigger.tooltip = skipPickupTooltip;
        
        // Highlights
        chatToggle.gameObject.AddComponent<Highlighter>().highlightPanel = chatHighlight;
        hintsToggle.gameObject.AddComponent<Highlighter>().highlightPanel = hintsHighlight;
        shopHintsToggle.gameObject.AddComponent<Highlighter>().highlightPanel = shopHintsHighlight;
        itemSentToggle.gameObject.AddComponent<Highlighter>().highlightPanel = itemSentHighlight;
        statusToggle.gameObject.AddComponent<Highlighter>().highlightPanel = statusHighlight;
        ticketToggle.gameObject.AddComponent<Highlighter>().highlightPanel = ticketHighlight;
        kioskToggle.gameObject.AddComponent<Highlighter>().highlightPanel = kioskHighlight;
        kioskSpoilerToggle.gameObject.AddComponent<Highlighter>().highlightPanel = kioskSpoilerHighlight;
        contactListToggle.gameObject.AddComponent<Highlighter>().highlightPanel = contactListHighlight;
        cacmiToggle.gameObject.AddComponent<Highlighter>().highlightPanel = cacmiHighlight;
        kalmiToggle.gameObject.AddComponent<Highlighter>().highlightPanel = kalmiHighlight;
        //trackerKeyToggle.gameObject.AddComponent<Highlighter>().highlightPanel = trackerKeyHighlight;
        cassetteSpoilerToggle.gameObject.AddComponent<Highlighter>().highlightPanel = cassetteSpoilerHighlight;
        skipPickupToggle.gameObject.AddComponent<Highlighter>().highlightPanel = skipPickupHighlight;
        
        // Information Tracker stuff
        boughtHomeImage.enabled = false;
        boughtHomeBackImage.enabled = false;
        boughtHcImage.enabled = false;
        boughtHcBackImage.enabled = false;
        boughtTtImage.enabled = false;
        boughtTtBackImage.enabled = false;
        boughtSfcImage.enabled = false;
        boughtSfcBackImage.enabled = false;
        boughtPpImage.enabled = false;
        boughtPpBackImage.enabled = false;
        boughtBathImage.enabled = false;
        boughtBathBackImage.enabled = false;
        boughtHqImage.enabled = false;
        boughtHqBackImage.enabled = false;
        
        boughtHcFishImage.enabled = false;
        boughtTtFishImage.enabled = false;
        boughtScfFishImage.enabled = false;
        boughtPpFishImage.enabled = false;
        boughtBathFishImage.enabled = false;
        boughtHqFishImage.enabled = false;
        
        // Lights
        if (Plugin.ChristmasEvent && !Plugin.NoXmasEvent)
        {
            slotNameField.selectionColor = new Color(1, (float)0.2877358, (float)0.3696053, (float)0.7529412);
            serverAddressField.selectionColor = new Color(1, (float)0.2877358, (float)0.3696053, (float)0.7529412);
            passwordField.selectionColor = new Color(1, (float)0.2877358, (float)0.3696053, (float)0.7529412);
            var menuCandles = formPanel.transform.Find("MenuGlowCandles");
            var menuCandles2 = formPanel.transform.Find("MenuGlowCandles2");
            var menuCandlesBlink = formPanel.transform.Find("MenuGlowCandles1Blink");
            var menuCandlesBlink2 = formPanel.transform.Find("MenuGlowCandles2Blink");
            var settingsLights = settingsButton.transform.Find("GlowLightsSettings");
            var trackersLights = trackersButton.transform.Find("GlowLightsTrackers");
            var qolLights = qolButton.transform.Find("GlowLightsQOL");
            Color[] glowColors =
            [
                new Color(1f, 0f, 0f), 
                new Color(1f, 0.5f, 0f), 
                new Color(0f, 1f, 0f), 
                new Color(0f, 0.9062204f, 1f),
                new Color(1f, 0.5529412f, 0.788992f),
                new Color(0.2071452f, 0f, 1f), 
                new Color(1f, 0f, 0.6565428f), 
                new Color(1f, 1f, 0f), 
                new Color(0f, 1f, 0.4670312f), 
                new Color(0f, 0f, 1f), 
                new Color(0.9038821f, 0.5518868f, 1f), 
                new Color(1f, 1f, 1f), 
                new Color(0f, 0.444962f, 1f), 
                new Color(1f, 0f, 0.9598556f), 
                new Color(0.5898412f, 0.5882353f, 1f), 
                new Color(1f, 0.7369196f, 0f), 
            ];
            menuCandles.gameObject.AddComponent<LightController>().glowColors = glowColors;
            Color[] glowColors2 =
            [
                new(1f, 0.7369196f, 0f),
                new(1f, 0f, 0f), 
                new(1f, 0.5f, 0f), 
                new(0f, 1f, 0f), 
                new(0f, 0.9062204f, 1f),
                new(1f, 0.5529412f, 0.788992f),
                new(0.2071452f, 0f, 1f), 
                new(1f, 0f, 0.6565428f), 
                new(1f, 1f, 0f), 
                new(0f, 1f, 0.4670312f), 
                new(0f, 0f, 1f), 
                new(0.9038821f, 0.5518868f, 1f), 
                new(1f, 1f, 1f), 
                new(0f, 0.444962f, 1f), 
                new(1f, 0f, 0.9598556f), 
                new(0.5898412f, 0.5882353f, 1f), 
            ];
            menuCandles2.gameObject.AddComponent<LightController>().glowColors = glowColors2;
            menuCandlesBlink.gameObject.AddComponent<LightBlinking>();
            menuCandlesBlink2.gameObject.AddComponent<LightBlinking>().alpha = 0f;
            settingsLights.gameObject.AddComponent<LightController>();
            trackersLights.gameObject.AddComponent<LightController>();
            qolLights.gameObject.AddComponent<LightController>();
        }
        
        settingsPanelCanvasGroup = formPanel.transform.Find("settingsPanel").gameObject.GetComponent<CanvasGroup>();
        trackersPanelCanvasGroup = formPanel.transform.Find("trackersPanel").gameObject.GetComponent<CanvasGroup>();
        qolPanelCanvasGroup = formPanel.transform.Find("qolPanel").gameObject.GetComponent<CanvasGroup>();
        _activePanel = settingsPanelCanvasGroup;
        SetActivePanel(settingsPanelCanvasGroup);
    }

    public void ShowSettings()
    {
        SetActivePanel(settingsPanelCanvasGroup);
    }

    public void ShowTrackers()
    {
        SetActivePanel(trackersPanelCanvasGroup);
    }

    public void ShowQOL()
    {
        SetActivePanel(qolPanelCanvasGroup);
    }

    public void ToggleTooltips()
    {
        if (tooltipsToggle.isOn && hideOnce) return;
        settingsTooltip.gameObject.SetActive(false);
        trackersTooltip.gameObject.SetActive(false);
        qolTooltip.gameObject.SetActive(false);
        rememberMeTooltip.gameObject.SetActive(false);
        chatTooltip.gameObject.SetActive(false);
        hintsTooltip.gameObject.SetActive(false);
        shopHintsTooltip.gameObject.SetActive(false);
        ticketTooltip.gameObject.SetActive(false);
        kioskTooltip.gameObject.SetActive(false);
        kioskSpoilerTooltip.gameObject.SetActive(false);
        cacmiTooltip.gameObject.SetActive(false);
        kalmiTooltip.gameObject.SetActive(false);
        itemSentTooltip.gameObject.SetActive(false);
        contactListTooltip.gameObject.SetActive(false);
        statusTooltip.gameObject.SetActive(false);
        reloadTooltip.gameObject.SetActive(false);
        keysDisabledTooltip.gameObject.SetActive(false);
        fishDisabledTooltip.gameObject.SetActive(false);
        //trackerKeyTooltip.gameObject.SetActive(false);
        tooltipsTooltip.gameObject.SetActive(false);
        cassetteSpoilerTooltip.gameObject.SetActive(false);
        skipPickupTooltip.gameObject.SetActive(false);
    }

    private void SetActivePanel(CanvasGroup newPanel)
    {
        if (_activePanel == newPanel) return;
        
        StopAllCoroutines();
        StartCoroutine(FadeOut(_activePanel));
        StartCoroutine(FadeIn(newPanel));
        _activePanel = newPanel;
    }

    private IEnumerator FadeIn(CanvasGroup panel)
    {
        panel.alpha = 0;
        panel.gameObject.SetActive(true);
        
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            panel.alpha = t / fadeDuration;
            yield return null;
        }
        panel.alpha = 1;
    }

    private IEnumerator FadeOut(CanvasGroup panel)
    {
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            panel.alpha = 1 - (t / fadeDuration);
            yield return null;
        }
        panel.alpha = 0;
        panel.gameObject.SetActive(false);
    }

    private void Update()
    {
        ToggleTooltips();
        Plugin.ChristmasEvent = !Plugin.NoXmasEvent && SeasonalThemes;
        if (Plugin.loggedIn)
        {
            informationPanel.SetActive(true);
            connectionPanel.SetActive(false);
            //trackersButton.interactable = false;
            //qolButton.interactable = false;
            if (int.Parse(ArchipelagoData.slotData["goal_completion"].ToString()) == 0)
            {
                goalText.text = "Goal: Get Hired";
                goalTooltipText.text = "Repair the elevator in Tadpole HQ to reach Pepper and get hired!";
            }
            else
            {
                goalText.text = "Goal: Employee Of The Month";
                goalTooltipText.text = "Get 76 Coins to be considered the 'Employee Of The Month'!";
            }

            if (ArchipelagoData.slotData.ContainsKey("key_level"))
            {
                if (int.Parse(ArchipelagoData.slotData["key_level"].ToString()) == 1)
                {
                    keysDisabledImage.gameObject.SetActive(false);
                    keyCountBackText.text = "NO";
                    keyCountFrontText.text = "NO";
                    keyHcCountBackText.text = Plugin.ArchipelagoClient.HcKeyAmount + "/1";
                    keyHcCountFrontText.text = Plugin.ArchipelagoClient.HcKeyAmount + "/1";
                    keyTtCountBackText.text = Plugin.ArchipelagoClient.TtKeyAmount + "/1";
                    keyTtCountFrontText.text = Plugin.ArchipelagoClient.TtKeyAmount + "/1";
                    keySfcCountBackText.text = Plugin.ArchipelagoClient.SfcKeyAmount + "/1";
                    keySfcCountFrontText.text = Plugin.ArchipelagoClient.SfcKeyAmount + "/1";
                    keyPpCountBackText.text = Plugin.ArchipelagoClient.PpKeyAmount + "/1";
                    keyPpCountFrontText.text = Plugin.ArchipelagoClient.PpKeyAmount + "/1";
                    keyBathCountBackText.text = Plugin.ArchipelagoClient.BathKeyAmount + "/2";
                    keyBathCountFrontText.text = Plugin.ArchipelagoClient.BathKeyAmount + "/2";
                    keyHqCountBackText.text = Plugin.ArchipelagoClient.HqKeyAmount + "/1";
                    keyHqCountFrontText.text = Plugin.ArchipelagoClient.HqKeyAmount + "/1";
                }
                else
                {
                    keysDisabledImage.gameObject.SetActive(true);
                    keyCountBackText.text = Plugin.ArchipelagoClient.KeyAmount + "/9";
                    keyCountFrontText.text = Plugin.ArchipelagoClient.KeyAmount + "/9";
                    keyHcCountBackText.text = "X";
                    keyHcCountFrontText.text = "X";
                    keyTtCountBackText.text = "X";
                    keyTtCountFrontText.text = "X";
                    keySfcCountBackText.text = "X";
                    keySfcCountFrontText.text = "X";
                    keyPpCountBackText.text = "X";
                    keyPpCountFrontText.text = "X";
                    keyBathCountBackText.text = "X";
                    keyBathCountFrontText.text = "X";
                    keyHqCountBackText.text = "X";
                    keyHqCountFrontText.text = "X";
                }
            }
            else
            {
                keysDisabledImage.gameObject.SetActive(true);
                keyCountBackText.text = Plugin.ArchipelagoClient.KeyAmount + "/9";
                keyCountFrontText.text = Plugin.ArchipelagoClient.KeyAmount + "/9";
                keyHcCountBackText.text = "X";
                keyHcCountFrontText.text = "X";
                keyTtCountBackText.text = "X";
                keyTtCountFrontText.text = "X";
                keySfcCountBackText.text = "X";
                keySfcCountFrontText.text = "X";
                keyPpCountBackText.text = "X";
                keyPpCountFrontText.text = "X";
                keyBathCountBackText.text = "X";
                keyBathCountFrontText.text = "X";
                keyHqCountBackText.text = "X";
                keyHqCountFrontText.text = "X";
            }

            if (ArchipelagoData.slotData.ContainsKey("fishsanity"))
            {
                if (int.Parse(ArchipelagoData.slotData["fishsanity"].ToString()) == 2)
                {
                    fishDisabledImage.gameObject.SetActive(false);
                    fishHcCountBackText.text = ItemHandler.HairballFishAmount + "/5";
                    fishHcCountFrontText.text = ItemHandler.HairballFishAmount + "/5";
                    if (scrGameSaveManager.instance.gameData.worldsData[1].coinFlags.Contains("fishing"))
                        boughtHcFishImage.enabled = true;
                    fishTtCountBackText.text = ItemHandler.TurbineFishAmount + "/5";
                    fishTtCountFrontText.text = ItemHandler.TurbineFishAmount + "/5";
                    if (scrGameSaveManager.instance.gameData.worldsData[2].coinFlags.Contains("fishing"))
                        boughtTtFishImage.enabled = true;
                    fishSfcCountBackText.text = ItemHandler.SalmonFishAmount + "/5";
                    fishSfcCountFrontText.text = ItemHandler.SalmonFishAmount + "/5";
                    if (scrGameSaveManager.instance.gameData.worldsData[3].coinFlags.Contains("fishing"))
                        boughtScfFishImage.enabled = true;
                    fishPpCountBackText.text = ItemHandler.PoolFishAmount + "/5";
                    fishPpCountFrontText.text = ItemHandler.PoolFishAmount + "/5";
                    if (scrGameSaveManager.instance.gameData.worldsData[4].coinFlags.Contains("fishing"))
                        boughtPpFishImage.enabled = true;
                    fishBathCountBackText.text = ItemHandler.BathFishAmount + "/5";
                    fishBathCountFrontText.text = ItemHandler.BathFishAmount + "/5";
                    if (scrGameSaveManager.instance.gameData.worldsData[5].coinFlags.Contains("fishing"))
                        boughtBathFishImage.enabled = true;
                    fishHqCountBackText.text = ItemHandler.TadpoleFishAmount + "/5";
                    fishHqCountFrontText.text = ItemHandler.TadpoleFishAmount + "/5";
                    if (scrGameSaveManager.instance.gameData.worldsData[6].coinFlags.Contains("fishing"))
                        boughtHqFishImage.enabled = true;
                }
                else
                {
                    fishDisabledImage.gameObject.SetActive(true);
                    fishHcCountBackText.text = "X";
                    fishHcCountFrontText.text = "X";
                    fishTtCountBackText.text = "X";
                    fishTtCountFrontText.text = "X";
                    fishSfcCountBackText.text = "X";
                    fishSfcCountFrontText.text = "X";
                    fishPpCountBackText.text = "X";
                    fishPpCountFrontText.text = "X";
                    fishBathCountBackText.text = "X";
                    fishBathCountFrontText.text = "X";
                    fishHqCountBackText.text = "X";
                    fishHqCountFrontText.text = "X";
                }
            }
            else
            {
                fishDisabledImage.gameObject.SetActive(true);
                fishHcCountBackText.text = "X";
                fishHcCountFrontText.text = "X";
                fishTtCountBackText.text = "X";
                fishTtCountFrontText.text = "X";
                fishSfcCountBackText.text = "X";
                fishSfcCountFrontText.text = "X";
                fishPpCountBackText.text = "X";
                fishPpCountFrontText.text = "X";
                fishBathCountBackText.text = "X";
                fishBathCountFrontText.text = "X";
                fishHqCountBackText.text = "X";
                fishHqCountFrontText.text = "X";
            }

            if (ArchipelagoData.slotData.ContainsKey("shuffle_garden"))
            {
                if (int.Parse(ArchipelagoData.slotData["shuffle_garden"].ToString()) == 1)
                {
                    coinCountBackText.text = scrGameSaveManager.instance.gameData.generalGameData.coinAmount + "/79";
                    coinCountFrontText.text = scrGameSaveManager.instance.gameData.generalGameData.coinAmount + "/79";
                    cassetteCountBackText.text = scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount + "/71";
                    cassetteCountFrontText.text = scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount + "/71";
                    gardenDisabledImage.gameObject.SetActive(false);
                }
                else
                {
                    coinCountBackText.text = scrGameSaveManager.instance.gameData.generalGameData.coinAmount + "/76";
                    coinCountFrontText.text = scrGameSaveManager.instance.gameData.generalGameData.coinAmount + "/76";
                    cassetteCountBackText.text = scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount + "/61";
                    cassetteCountFrontText.text = scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount + "/61";
                    gardenDisabledImage.gameObject.SetActive(true);
                }
            }
            else
            {
                coinCountBackText.text = scrGameSaveManager.instance.gameData.generalGameData.coinAmount + "/79";
                coinCountFrontText.text = scrGameSaveManager.instance.gameData.generalGameData.coinAmount + "/79";
                cassetteCountBackText.text = scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount + "/71";
                cassetteCountFrontText.text = scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount + "/71";
                gardenDisabledImage.gameObject.SetActive(true);
            }
            
            if (KioskSpoiler)
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
            kioskHomeBackText.text = kioskHomeText.text;
            kioskHcBackText.text = kioskHcText.text;
            kioskTtBackText.text = kioskTtText.text;
            kioskSfcBackText.text = kioskSfcText.text;
            kioskPpBackText.text = kioskPpText.text;
            kioskBathBackText.text = kioskBathText.text;
            kioskHqBackText.text = kioskHqText.text;
            if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("KioskHome"))
            {
                kioskHomeImage.color = new Color(1f, 1f, 1f, 1f);
                kioskHomeText.enabled = false;
                kioskHomeBackText.enabled = false;
                kioskHomeCostImage.enabled = false;
                kioskHomeCostBackImage.enabled = false;
                boughtHomeImage.enabled = true;
                boughtHomeBackImage.enabled = true;
            }

            if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("KioskHairball City"))
            {
                kioskHcText.enabled = false;
                kioskHcBackText.enabled = false;
                kioskHcCostImage.enabled = false;
                kioskHcCostBackImage.enabled = false;
                boughtHcImage.enabled = true;
                boughtHcBackImage.enabled = true;
            }

            if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("KioskTrash Kingdom"))
            {
                kioskTtText.enabled = false;
                kioskTtBackText.enabled = false;
                kioskTtCostImage.enabled = false;
                kioskTtCostBackImage.enabled = false;
                boughtTtImage.enabled = true;
                boughtTtBackImage.enabled = true;
            }

            if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("KioskSalmon Creek Forest"))
            {
                kioskSfcText.enabled = false;
                kioskSfcBackText.enabled = false;
                kioskSfcCostImage.enabled = false;
                kioskSfcCostBackImage.enabled = false;
                boughtSfcImage.enabled = true;
                boughtSfcBackImage.enabled = true;
            }

            if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("KioskPublic Pool"))
            {
                kioskPpText.enabled = false;
                kioskPpBackText.enabled = false;
                kioskPpCostImage.enabled = false;
                kioskPpCostBackImage.enabled = false;
                boughtPpImage.enabled = true;
                boughtPpBackImage.enabled = true;
            }

            if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("KioskThe Bathhouse"))
            {
                kioskBathText.enabled = false;
                kioskBathBackText.enabled = false;
                kioskBathCostImage.enabled = false;
                kioskBathCostBackImage.enabled = false;
                boughtBathImage.enabled = true;
                boughtBathBackImage.enabled = true;
            }

            if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("KioskTadpole inc"))
            {
                kioskHqText.enabled = false;
                kioskHqBackText.enabled = false;
                kioskHqCostImage.enabled = false;
                kioskHqCostBackImage.enabled = false;
                boughtHqImage.enabled = true;
                boughtHqBackImage.enabled = true;
            }
            if (gameSaveManager.gameData.generalGameData.unlockedLevels[2]) ticketHcImage.color = Color.white;
            if (gameSaveManager.gameData.generalGameData.unlockedLevels[3]) ticketTtImage.color = Color.white;
            if (gameSaveManager.gameData.generalGameData.unlockedLevels[4]) ticketSfcImage.color = Color.white;
            if (gameSaveManager.gameData.generalGameData.unlockedLevels[5]) ticketPpImage.color = Color.white;
            if (gameSaveManager.gameData.generalGameData.unlockedLevels[6]) ticketBathImage.color = Color.white;
            if (gameSaveManager.gameData.generalGameData.unlockedLevels[7]) ticketHqImage.color = Color.white;
            if (ArchipelagoData.slotData.ContainsKey("garden_access"))
            {
                if (ItemHandler.Garden || int.Parse(ArchipelagoData.slotData["garden_access"].ToString()) == 0 
                    && gameSaveManager.gameData.generalGameData.unlockedLevels[7]) ticketGgImage.color = Color.white;
            }
            else if (gameSaveManager.gameData.generalGameData.unlockedLevels[7])
            {
                ticketGgImage.color = Color.white;
            }
            if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("APWave1")) ticketCl1Image.color = Color.white;
            if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("APWave2")) ticketCl2Image.color = Color.white;
            for (var i = 1; i < 8; i++)
            {
                if (scrGameSaveManager.instance.gameData.worldsData[i].coinFlags.Contains("cassetteCoin"))
                {
                    switch (i)
                    {
                        case 1:
                            mitchHairballImage.color = new Color(1f, 1f, 1f, 1f);
                            // mitchHairballText.enabled = false;
                            // mitchHairballShadowText.enabled = false;
                            // boughtMitchHairballImage.enabled = true;
                            // mitchHairballCassetteImage.enabled = false;
                            break;
                        case 2:
                            mitchTurbineImage.color = new Color(1f, 1f, 1f, 1f);
                            // mitchTurbineText.enabled = false;
                            // mitchTurbineShadowText.enabled = false;
                            // boughtMitchTurbineImage.enabled = true;
                            // mitchTurbineCassetteImage.enabled = false;
                            break;
                        case 3:
                            mitchSalmonImage.color = new Color(1f, 1f, 1f, 1f);
                            // mitchSalmonText.enabled = false;
                            // mitchSalmonShadowText.enabled = false;
                            // boughtMitchSalmonImage.enabled = true;
                            // mitchSalmonCassetteImage.enabled = false;
                            break;
                        case 4:
                            mitchPoolImage.color = new Color(1f, 1f, 1f, 1f);
                            // mitchPoolText.enabled = false;
                            // mitchPoolShadowText.enabled = false;
                            // boughtMitchPoolImage.enabled = true;
                            // mitchPoolCassetteImage.enabled = false;
                            break;
                        case 5:
                            mitchBathImage.color = new Color(1f, 1f, 1f, 1f);
                            // mitchBathText.enabled = false;
                            // mitchBathShadowText.enabled = false;
                            // boughtMitchBathImage.enabled = true;
                            // mitchBathCassetteImage.enabled = false;
                            break;
                        case 6:
                            mitchTadpoleImage.color = new Color(1f, 1f, 1f, 1f);
                            // mitchTadpoleText.enabled = false;
                            // mitchTadpoleShadowText.enabled = false;
                            // boughtMitchTadpoleImage.enabled = true;
                            // mitchTadpoleCassetteImage.enabled = false;
                            break;
                        case 7:
                            mitchGardenImage.color = new Color(1f, 1f, 1f, 1f);
                            // mitchGardenText.enabled = false;
                            // mitchGardenShadowText.enabled = false;
                            // boughtMitchGardenImage.enabled = true;
                            // mitchGardenCassetteImage.enabled = false;
                            break;
                    }
                }
                if (scrGameSaveManager.instance.gameData.worldsData[i].coinFlags.Contains("cassetteCoin2"))
                {
                    switch (i)
                    {
                        case 1:
                            maiHairballImage.color = new Color(1f, 1f, 1f, 1f);
                            // maiHairballText.enabled = false;
                            // maiHairballShadowText.enabled = false;
                            // boughtMaiHairballImage.enabled = true;
                            // maiHairballCassetteImage.enabled = false;
                            break;
                        case 2:
                            maiTurbineImage.color = new Color(1f, 1f, 1f, 1f);
                            // maiTurbineText.enabled = false;
                            // maiTurbineShadowText.enabled = false;
                            // boughtMaiTurbineImage.enabled = true;
                            // maiTurbineCassetteImage.enabled = false;
                            break;
                        case 3:
                            maiSalmonImage.color = new Color(1f, 1f, 1f, 1f);
                            // maiSalmonText.enabled = false;
                            // maiSalmonShadowText.enabled = false;
                            // boughtMaiSalmonImage.enabled = true;
                            // maiSalmonCassetteImage.enabled = false;
                            break;
                        case 4:
                            maiPoolImage.color = new Color(1f, 1f, 1f, 1f);
                            // maiPoolText.enabled = false;
                            // maiPoolShadowText.enabled = false;
                            // boughtMaiPoolImage.enabled = true;
                            // maiPoolCassetteImage.enabled = false;
                            break;
                        case 5:
                            maiBathImage.color = new Color(1f, 1f, 1f, 1f);
                            // maiBathText.enabled = false;
                            // maiBathShadowText.enabled = false;
                            // boughtMaiBathImage.enabled = true;
                            // maiBathCassetteImage.enabled = false;
                            break;
                        case 6:
                            maiTadpoleImage.color = new Color(1f, 1f, 1f, 1f);
                            // maiTadpoleText.enabled = false;
                            // maiTadpoleShadowText.enabled = false;
                            // boughtMaiTadpoleImage.enabled = true;
                            // maiTadpoleCassetteImage.enabled = false;
                            break;
                        case 7:
                            maiGardenImage.color = new Color(1f, 1f, 1f, 1f);
                            // maiGardenText.enabled = false;
                            // maiGardenShadowText.enabled = false;
                            // boughtMaiGardenImage.enabled = true;
                            // maiGardenCassetteImage.enabled = false;
                            break;
                    }
                }
            }
        }
        else
        {
            informationPanel.SetActive(false);
            connectionPanel.SetActive(true);
        }
    }

    public void ToggleFormVisibility()
    {
        bool isActive = formPanel.activeSelf;
        formPanel.SetActive(!isActive);
    }

    public void ToggleDebugMode()
    {
        forceDebug = !forceDebug;
    }

    public void ReloadSettings()
    {
        _rememberMe = rememberMeToggle.isOn;
        _chat = chatToggle.isOn;
        _hints = hintsToggle.isOn;
        _shopHints = shopHintsToggle.isOn;
        _ticket = ticketToggle.isOn;
        _kiosk = kioskToggle.isOn;
        _kioskSpoiler = kioskSpoilerToggle.isOn;
        _cacmi = cacmiToggle.isOn;
        _kalmi = kalmiToggle.isOn;
        _itemSent = itemSentToggle.isOn;
        _contactList = contactListToggle.isOn;
        _status = statusToggle.isOn;
        //_trackerKey = trackerKeyToggle.isOn;
        _tooltips = tooltipsToggle.isOn;
        _cassetteSpoiler = cassetteSpoilerToggle.isOn;
        _seasonalThemes = seasonalThemesToggle.isOn;
        _skipPickup = skipPickupToggle.isOn;
        Hints = _hints;
        Chat = _chat;
        ShopHints = _shopHints;
        Ticket = _ticket;
        Kiosk = _kiosk;
        KioskSpoiler = _kioskSpoiler;
        cacmi = _cacmi;
        kalmi = _kalmi;
        itemSent = _itemSent;
        contactList = _contactList;
        status = _status;
        //TrackerKey = _trackerKey;
        Tooltips = _tooltips;
        CassetteSpoiler = _cassetteSpoiler;
        hideOnce = _tooltips;
        SeasonalThemes = _seasonalThemes;
        SkipPickup = _skipPickup;
        
        SavedData data = new SavedData
        {
            Host = ArchipelagoClient.ServerData.Uri,
            SlotName = ArchipelagoClient.ServerData.SlotName,
            RememberMe = _rememberMe,
            Chat = _chat,
            Hint = _hints,
            ShopHints = _shopHints,
            Ticket = _ticket,
            Kiosk = _kiosk,
            KioskSpoiler = _kioskSpoiler,
            CACMI = _cacmi,
            KALMI = _kalmi,
            ItemSent = _itemSent,
            ContactList = _contactList,
            Status = _status,
            //Key = _trackerKey,
            Tooltips = _tooltips,
            CassetteSpoiler = _cassetteSpoiler,
            SeasonalThemes =  _seasonalThemes,
            SkipPickup = _skipPickup,
        };
        if (_rememberMe)
        {
            SaveSettings(data);
        }
        scrTrainManager.instance.UseTrain(scrGameSaveManager.instance.gameData.generalGameData.currentLevel, false);
    }

    public void Connect()
    {
        if (pressedConnect) return;
        pressedConnect = true;
        _serverAddress = serverAddressField.text;
        _slotName = slotNameField.text;
        password = passwordField.text;
        _rememberMe = rememberMeToggle.isOn;
        _chat = chatToggle.isOn;
        _hints = hintsToggle.isOn;
        _shopHints = shopHintsToggle.isOn;
        _ticket = ticketToggle.isOn;
        _kiosk = kioskToggle.isOn;
        _kioskSpoiler = kioskSpoilerToggle.isOn;
        _cacmi = cacmiToggle.isOn;
        _kalmi = kalmiToggle.isOn;
        _itemSent = itemSentToggle.isOn;
        _contactList = contactListToggle.isOn;
        _status = statusToggle.isOn;
        //_trackerKey = trackerKeyToggle.isOn;
        _skipPickup = skipPickupToggle.isOn;
        _tooltips = tooltipsToggle.isOn;
        _cassetteSpoiler = cassetteSpoilerToggle.isOn;
        Hints = _hints;
        Chat = _chat;
        ShopHints = _shopHints;
        Ticket = _ticket;
        Kiosk = _kiosk;
        KioskSpoiler = _kioskSpoiler;
        cacmi = _cacmi;
        kalmi = _kalmi;
        itemSent = _itemSent;
        contactList = _contactList;
        status = _status;
        //TrackerKey = _trackerKey;
        Tooltips = _tooltips;
        CassetteSpoiler = _cassetteSpoiler;
        _seasonalThemes = seasonalThemesToggle.isOn;
        SeasonalThemes = _seasonalThemes;
        SkipPickup = _skipPickup;
        
        ArchipelagoClient.ServerData.Uri = _serverAddress;
        ArchipelagoClient.ServerData.SlotName = _slotName;
        ArchipelagoClient.ServerData.Password = password;

        Plugin.BepinLogger.LogInfo($"Server Address: {_serverAddress}");
        Plugin.BepinLogger.LogInfo($"Slot Name: {_slotName}");
        Plugin.BepinLogger.LogInfo($"Password: {password}");
        Plugin.BepinLogger.LogInfo($"Remember Me: {_rememberMe}");
        Plugin.BepinLogger.LogInfo($"Chat: {_chat}");
        Plugin.BepinLogger.LogInfo($"Hints: {_hints}");
        Plugin.BepinLogger.LogInfo($"Shop Hints: {_shopHints}");
        Plugin.BepinLogger.LogInfo($"Ticket Tracker: {_ticket}");
        Plugin.BepinLogger.LogInfo($"Kiosk Tracker: {_kiosk}");
        Plugin.BepinLogger.LogInfo($"Hide Cost: {_kioskSpoiler}");
        Plugin.BepinLogger.LogInfo($"CACMI: {_cacmi}");
        Plugin.BepinLogger.LogInfo($"Item Sent: {_itemSent}");
        Plugin.BepinLogger.LogInfo($"Contact List: {_contactList}");
        Plugin.BepinLogger.LogInfo($"Status: {_status}");
        
        SavedData data = new SavedData
        {
            Host = _serverAddress,
            SlotName = _slotName,
            RememberMe = _rememberMe,
            Chat = _chat,
            Hint = _hints,
            ShopHints = _shopHints,
            Ticket = _ticket,
            Kiosk = _kiosk,
            KioskSpoiler = _kioskSpoiler,
            CACMI = _cacmi,
            KALMI = _kalmi,
            ItemSent = _itemSent,
            ContactList = _contactList,
            Status = _status,
            //Key = _trackerKey,
            Tooltips = _tooltips,
            CassetteSpoiler = _cassetteSpoiler,
            SeasonalThemes = _seasonalThemes,
            SkipPickup = _skipPickup,
        };
        if (_rememberMe)
        {
            SaveSettings(data);
        }

        Plugin.ArchipelagoClient.Connect();
        Plugin.BepinLogger.LogInfo("Checking Saves...");
        // var saveName = "APSave" + "_" + ArchipelagoClient.ServerData.SlotName + "_" + ArchipelagoClient.ServerData.Uri.Replace(":", ".");
        // if (scrGameSaveManager.saveName != saveName && ArchipelagoClient.Authenticated)
        // {
        //     scrGameSaveManager.saveName = saveName;
        //     var savePath = Path.Combine(Plugin.ArchipelagoFolderPath, saveName + "_" + Seed + ".json");
        //     if (File.Exists(savePath))
        //     {
        //         scrGameSaveManager.dataPath = savePath;
        //         Plugin.BepinLogger.LogInfo("Found a SaveFile with the current SlotName & Port!");
        //         //ArchipelagoConsole.LogMessage("Found a SaveFile with the current SlotName & Port!");
        //         gameSaveManager.LoadGame();
        //     }
        //     else
        //     {
        //         Plugin.newFile = true;
        //         scrGameSaveManager.dataPath = savePath;
        //         Plugin.BepinLogger.LogWarning("No SaveFile found. Creating a new one!");
        //         //ArchipelagoConsole.LogMessage("No SaveFile found. Creating a new one!");
        //         gameSaveManager.SaveGame();
        //         gameSaveManager.LoadGame();
        //         gameSaveManager.ClearSaveData();
        //     }
        //     scrTrainManager.instance.UseTrain(!Plugin.newFile ? gameSaveManager.gameData.generalGameData.currentLevel : 1, false);
        //     if (Plugin.newFile)
        //     {
        //         ArchipelagoClient.Disconnect();
        //         StartCoroutine(FirstLoginFix());
        //     }
        // }
    }

    private static IEnumerator FirstLoginFix()
    {
        yield return new WaitUntil(ArchipelagoClient.IsValidScene);
        //ArchipelagoClient.Connect();
        Plugin.APSendNote($"Connected to {ArchipelagoClient.ServerData.Uri} successfully", 6F);
        Plugin.loggedIn = true; 
        //ArchipelagoClient.CheckReceivedItems();
    }

    private class SavedData
    {
        public string Host { get; set; } = _serverAddress;
        public string SlotName { get; set; } = _slotName;
        public bool RememberMe { get; set; } = _rememberMe;
        public bool Chat { get; set; } = _chat;
        public bool ShopHints { get; set; } = _shopHints;
        public bool Hint { get; set; } = _hints;
        public bool Ticket { get; set; } = _ticket;
        public bool Kiosk { get; set; } = _kiosk;
        public bool KioskSpoiler { get; set; } = _kioskSpoiler;
        public bool CACMI { get; set; } = _cacmi;
        public bool ItemSent { get; set; } = _itemSent;
        public bool ContactList { get; set; } = _contactList;
        public bool Status { get; set; } = _status;
        //public bool Key { get; set; } = _trackerKey;
        public bool Tooltips { get; set; } = _tooltips;
        public bool KALMI { get; set; } = _kalmi;
        public bool CassetteSpoiler { get; set; } = _cassetteSpoiler;
        public bool SeasonalThemes { get; set; } = _seasonalThemes;
        public bool SkipPickup { get; set; } = _skipPickup;
    }

    private void LoadData()
    {
        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            SavedData savedData = JsonConvert.DeserializeObject<SavedData>(json);
            serverAddressField.text = savedData.Host;
            slotNameField.text = savedData.SlotName;
            rememberMeToggle.isOn = savedData.RememberMe;
            chatToggle.isOn = savedData.Chat;
            hintsToggle.isOn = savedData.Hint;
            shopHintsToggle.isOn = savedData.ShopHints;
            kioskToggle.isOn = savedData.Kiosk;
            ticketToggle.isOn = savedData.Ticket;
            kioskSpoilerToggle.isOn = savedData.KioskSpoiler;
            cacmiToggle.isOn = savedData.CACMI;
            kalmiToggle.isOn = savedData.KALMI;
            itemSentToggle.isOn = savedData.ItemSent;
            contactListToggle.isOn = savedData.ContactList;
            statusToggle.isOn = savedData.Status;
            //trackerKeyToggle.isOn = savedData.Key;
            tooltipsToggle.isOn = savedData.Tooltips;
            cassetteSpoilerToggle.isOn = savedData.CassetteSpoiler;
            hideOnce = savedData.Tooltips;
            seasonalThemesToggle.isOn = savedData.SeasonalThemes;
            skipPickupToggle.isOn = savedData.SkipPickup;
            Plugin.BepinLogger.LogInfo("Loaded saved settings.");
        }
        else
        {
            serverAddressField.text = "archipelago.gg:";
            slotNameField.text = "Player1";
            rememberMeToggle.isOn = true;
            chatToggle.isOn = true;
            hintsToggle.isOn = true;
            shopHintsToggle.isOn = true;
            kioskToggle.isOn = true;
            ticketToggle.isOn = true;
            kioskSpoilerToggle.isOn = true;
            cacmiToggle.isOn = true;
            itemSentToggle.isOn = true;
            contactListToggle.isOn = true;
            statusToggle.isOn = true;
            //trackerKeyToggle.isOn = true;
            tooltipsToggle.isOn = false;
            hideOnce = false;
            kalmiToggle.isOn = true;
            cassetteSpoilerToggle.isOn = true;
            seasonalThemesToggle.isOn = true;
            skipPickupToggle.isOn = true;
        }
    }

    private void SaveSettings(SavedData data)
    {
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(jsonFilePath, jsonData);
    }
}

public class Tooltip : MonoBehaviour
{
    void Start()
    {
        HideTooltip();
    }

    public void ShowTooltip()
    {
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Tooltip tooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
