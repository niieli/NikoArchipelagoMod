using System.Collections;
using System.IO;
using BepInEx;
using Newtonsoft.Json;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;

namespace NikoArchipelago;

public class ArchipelagoMenu : MonoBehaviour
{
    public GameObject formPanel; 
    public GameObject connectionPanel;
    public Button openFormButton; 
    public InputField serverAddressField;
    public InputField slotNameField;
    public InputField passwordField;
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
    public Toggle itemSentToggle;
    public Tooltip itemSentTooltip;
    public TooltipTrigger itemSentTrigger;
    public GameObject itemSentHighlight;
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
    private static bool _cacmi;
    private static bool _itemSent;
    private static bool _contactList;
    private static bool _status;
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
    public static bool contactList;
    public static bool itemSent;
    public static bool status;
    
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
    public Button debugButton;
    public TooltipTrigger debugTrigger;
    public Tooltip debugTooltip;
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
    public TextMeshProUGUI keyHCCountBackText;
    public TextMeshProUGUI keyHCCountFrontText;
    public TextMeshProUGUI keyTTCountBackText;
    public TextMeshProUGUI keyTTCountFrontText;
    public TextMeshProUGUI keySFCCountBackText;
    public TextMeshProUGUI keySFCCountFrontText;
    public TextMeshProUGUI keyPPCountBackText;
    public TextMeshProUGUI keyPPCountFrontText;
    public TextMeshProUGUI keyBATHCountBackText;
    public TextMeshProUGUI keyBATHCountFrontText;
    public TextMeshProUGUI keyHQCountBackText;
    public TextMeshProUGUI keyHQCountFrontText;
    public static bool forceDebug;
    
    public void Start()
    {
        gameSaveManager = scrGameSaveManager.instance;
        
        formPanel = transform.Find("Panel").gameObject;
        openFormButton = transform.Find("APButton").gameObject.GetComponent<Button>();
        apButtonGameObject = transform.Find("APButton").gameObject;
        connectionPanel = formPanel.transform.Find("Connection").gameObject;
        serverAddressField = connectionPanel.transform.Find("ServerAdress").GetComponent<InputField>();
        slotNameField = connectionPanel.transform.Find("SlotName").GetComponent<InputField>();
        passwordField = connectionPanel.transform.Find("Password").GetComponent<InputField>();
        rememberMeToggle = formPanel.transform.Find("Remember").gameObject.GetComponent<Toggle>();
        rememberMeTrigger = rememberMeToggle.gameObject.AddComponent<TooltipTrigger>();
        rememberMeTooltip = rememberMeToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        connectButton = connectionPanel.transform.Find("Button").gameObject.GetComponent<Button>();
        versionText = formPanel.transform.Find("Version").gameObject.GetComponent<TMP_Text>();
        
        // Information, when logged in
        informationPanel = formPanel.transform.Find("InformationScreen").gameObject;
        debugButton = informationPanel.transform.Find("DEBUG").gameObject.GetComponent<Button>();
        debugTooltip = debugButton.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        debugTrigger = debugButton.gameObject.AddComponent<TooltipTrigger>();
        // Home
        kioskHomeImage = informationPanel.transform.Find("KioskHome").GetComponent<Image>();
        boughtHomeImage = kioskHomeImage.transform.Find("Bought").GetComponent<Image>();
        kioskHomeCostImage = kioskHomeImage.transform.Find("CostHome").GetComponent<Image>();
        kioskHomeText = kioskHomeCostImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        boughtHomeBackImage = informationPanel.transform.Find("KioskHomeBack/Bought").GetComponent<Image>();
        kioskHomeCostBackImage = informationPanel.transform.Find("KioskHomeBack/CostHome").GetComponent<Image>();
        kioskHomeBackText = informationPanel.transform.Find("KioskHomeBack/CostHome/Cost").GetComponent<TextMeshProUGUI>();
        // Hairball
        ticketHcImage = informationPanel.transform.Find("TicketHairball").GetComponent<Image>();
        boughtHcImage = ticketHcImage.transform.Find("Bought").GetComponent<Image>();
        kioskHcCostImage = ticketHcImage.transform.Find("CostHairball").GetComponent<Image>();
        kioskHcText = kioskHcCostImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        boughtHcBackImage = informationPanel.transform.Find("TicketHairballBack/Bought").GetComponent<Image>();
        kioskHcCostBackImage = informationPanel.transform.Find("TicketHairballBack/CostHairball").GetComponent<Image>();
        kioskHcBackText = informationPanel.transform.Find("TicketHairballBack/CostHairball/Cost").GetComponent<TextMeshProUGUI>();
        // Turbine
        ticketTtImage = informationPanel.transform.Find("TicketTurbine").GetComponent<Image>();
        boughtTtImage = ticketTtImage.transform.Find("Bought").GetComponent<Image>();
        kioskTtCostImage = ticketTtImage.transform.Find("CostTurbine").GetComponent<Image>();
        kioskTtText = kioskTtCostImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        boughtTtBackImage = informationPanel.transform.Find("TicketTurbineBack/Bought").GetComponent<Image>();
        kioskTtCostBackImage = informationPanel.transform.Find("TicketTurbineBack/CostTurbine").GetComponent<Image>();
        kioskTtBackText = informationPanel.transform.Find("TicketTurbineBack/CostTurbine/Cost").GetComponent<TextMeshProUGUI>();
        // Salmon
        ticketSfcImage = informationPanel.transform.Find("TicketSalmon").GetComponent<Image>();
        boughtSfcImage = ticketSfcImage.transform.Find("Bought").GetComponent<Image>();
        kioskSfcCostImage = ticketSfcImage.transform.Find("CostSalmon").GetComponent<Image>();
        kioskSfcText = kioskSfcCostImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        boughtSfcBackImage = informationPanel.transform.Find("TicketSalmonBack/Bought").GetComponent<Image>();
        kioskSfcCostBackImage = informationPanel.transform.Find("TicketSalmonBack/CostSalmon").GetComponent<Image>();
        kioskSfcBackText = informationPanel.transform.Find("TicketSalmonBack/CostSalmon/Cost").GetComponent<TextMeshProUGUI>();
        // Pool
        ticketPpImage = informationPanel.transform.Find("TicketPool").GetComponent<Image>();
        boughtPpImage = ticketPpImage.transform.Find("Bought").GetComponent<Image>();
        kioskPpCostImage = ticketPpImage.transform.Find("CostPool").GetComponent<Image>();
        kioskPpText = kioskPpCostImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        boughtPpBackImage = informationPanel.transform.Find("TicketPoolBack/Bought").GetComponent<Image>();
        kioskPpCostBackImage = informationPanel.transform.Find("TicketPoolBack/CostPool").GetComponent<Image>();
        kioskPpBackText = informationPanel.transform.Find("TicketPoolBack/CostPool/Cost").GetComponent<TextMeshProUGUI>();
        // Bath
        ticketBathImage = informationPanel.transform.Find("TicketBath").GetComponent<Image>();
        boughtBathImage = ticketBathImage.transform.Find("Bought").GetComponent<Image>();
        kioskBathCostImage = ticketBathImage.transform.Find("CostBath").GetComponent<Image>();
        kioskBathText = kioskBathCostImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        boughtBathBackImage = informationPanel.transform.Find("TicketBathBack/Bought").GetComponent<Image>();
        kioskBathCostBackImage = informationPanel.transform.Find("TicketBathBack/CostBath").GetComponent<Image>();
        kioskBathBackText = informationPanel.transform.Find("TicketBathBack/CostBath/Cost").GetComponent<TextMeshProUGUI>();
        // Tadpole
        ticketHqImage = informationPanel.transform.Find("TicketTadpole").GetComponent<Image>();
        boughtHqImage = ticketHqImage.transform.Find("Bought").GetComponent<Image>();
        kioskHqCostImage = ticketHqImage.transform.Find("CostTadpole").GetComponent<Image>();
        kioskHqText = kioskHqCostImage.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
        boughtHqBackImage = informationPanel.transform.Find("TicketTadpoleBack/Bought").GetComponent<Image>();
        kioskHqCostBackImage = informationPanel.transform.Find("TicketTadpoleBack/CostTadpole").GetComponent<Image>();
        kioskHqBackText = informationPanel.transform.Find("TicketTadpoleBack/CostTadpole/Cost").GetComponent<TextMeshProUGUI>();
        // Misc
        ticketGgImage = informationPanel.transform.Find("TicketGarden").GetComponent<Image>();
        ticketCl1Image = informationPanel.transform.Find("ContactList1").GetComponent<Image>();
        ticketCl2Image = informationPanel.transform.Find("ContactList2").GetComponent<Image>();
        
        keyCountBackText = informationPanel.transform.Find("KeyCount/Back").GetComponent<TextMeshProUGUI>();
        keyCountFrontText = informationPanel.transform.Find("KeyCount/Front").GetComponent<TextMeshProUGUI>();
        coinCountBackText = informationPanel.transform.Find("CoinCount/Back").GetComponent<TextMeshProUGUI>();
        coinCountFrontText = informationPanel.transform.Find("CoinCount/Front").GetComponent<TextMeshProUGUI>();
        cassetteCountBackText = informationPanel.transform.Find("CassetteCount/Back").GetComponent<TextMeshProUGUI>();
        cassetteCountFrontText = informationPanel.transform.Find("CassetteCount/Front").GetComponent<TextMeshProUGUI>();
        keyHCCountBackText = informationPanel.transform.Find("HCKeyCount/Back").GetComponent<TextMeshProUGUI>();
        keyHCCountFrontText = informationPanel.transform.Find("HCKeyCount/Front").GetComponent<TextMeshProUGUI>();
        keyTTCountBackText = informationPanel.transform.Find("TTKeyCount/Back").GetComponent<TextMeshProUGUI>();
        keyTTCountFrontText = informationPanel.transform.Find("TTKeyCount/Front").GetComponent<TextMeshProUGUI>();
        keySFCCountBackText = informationPanel.transform.Find("SFCKeyCount/Back").GetComponent<TextMeshProUGUI>();
        keySFCCountFrontText = informationPanel.transform.Find("SFCKeyCount/Front").GetComponent<TextMeshProUGUI>();
        keyPPCountBackText = informationPanel.transform.Find("PPKeyCount/Back").GetComponent<TextMeshProUGUI>();
        keyPPCountFrontText = informationPanel.transform.Find("PPKeyCount/Front").GetComponent<TextMeshProUGUI>();
        keyBATHCountBackText = informationPanel.transform.Find("BATHKeyCount/Back").GetComponent<TextMeshProUGUI>();
        keyBATHCountFrontText = informationPanel.transform.Find("BATHKeyCount/Front").GetComponent<TextMeshProUGUI>();
        keyHQCountBackText = informationPanel.transform.Find("HQKeyCount/Back").GetComponent<TextMeshProUGUI>();
        keyHQCountFrontText = informationPanel.transform.Find("HQKeyCount/Front").GetComponent<TextMeshProUGUI>();
        
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
        
        // QOL
        qolButton = formPanel.transform.Find("Tabs/QOLButton").gameObject.GetComponent<Button>();
        qolButton.gameObject.AddComponent<ButtonHoverEffect>();
        qolTrigger = qolButton.gameObject.AddComponent<TooltipTrigger>();
        
        cacmiToggle = qolPanel.transform.Find("CACMI").gameObject.GetComponent<Toggle>();
        cacmiTooltip = cacmiToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        cacmiTrigger = cacmiToggle.gameObject.AddComponent<TooltipTrigger>();
        cacmiHighlight = cacmiToggle.transform.Find("Highlight").gameObject;
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
        itemSentTrigger.tooltip = itemSentTooltip;
        contactListTrigger.tooltip = contactListTooltip;
        statusTrigger.tooltip = statusTooltip;
        debugTrigger.tooltip = debugTooltip;
        
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
        
        settingsPanelCanvasGroup = formPanel.transform.Find("settingsPanel").gameObject.GetComponent<CanvasGroup>();
        trackersPanelCanvasGroup = formPanel.transform.Find("trackersPanel").gameObject.GetComponent<CanvasGroup>();
        qolPanelCanvasGroup = formPanel.transform.Find("qolPanel").gameObject.GetComponent<CanvasGroup>();
        _activePanel = settingsPanelCanvasGroup;
        SetActivePanel(settingsPanelCanvasGroup);
    }
    
    private void CheckNullReferences()
    {
        // General UI Elements
        LogIfNull(serverAddressField, "ServerAddress Field");
        LogIfNull(slotNameField, "SlotName Field");
        LogIfNull(passwordField, "Password Field");
        LogIfNull(formPanel, "Form Panel");
        LogIfNull(openFormButton, "APButton");
        LogIfNull(rememberMeToggle, "Remember Toggle");
        LogIfNull(chatToggle, "Chat Toggle");
        LogIfNull(hintsToggle, "Hints Toggle");
        LogIfNull(shopHintsToggle, "ShopHints Toggle");
        LogIfNull(connectButton, "Connect Button");
        LogIfNull(versionText, "Version Text");

        // Settings, Trackers, and QOL Panels
        LogIfNull(settingsPanel, "Settings Panel");
        LogIfNull(trackersPanel, "Trackers Panel");
        LogIfNull(qolPanel, "QOL Panel");
    }

    private void LogIfNull(Object obj, string name)
    {
        if (obj == null)
        {
            Plugin.BepinLogger.LogError($"{name} is null!");
        }
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
        if (Plugin.loggedIn)
        {
            informationPanel.SetActive(true);
            connectionPanel.SetActive(false);
            trackersButton.interactable = false;
            qolButton.interactable = false;
            keyCountBackText.text = scrGameSaveManager.instance.gameData.generalGameData.keyAmount.ToString();
            keyCountFrontText.text = scrGameSaveManager.instance.gameData.generalGameData.keyAmount.ToString();
            coinCountBackText.text = scrGameSaveManager.instance.gameData.generalGameData.coinAmount.ToString();
            coinCountFrontText.text = scrGameSaveManager.instance.gameData.generalGameData.coinAmount.ToString();
            cassetteCountBackText.text = scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount.ToString();
            cassetteCountFrontText.text = scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount.ToString();
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
            if (ItemHandler.Garden || int.Parse(ArchipelagoData.slotData["garden_access"].ToString()) == 0 
                && gameSaveManager.gameData.generalGameData.unlockedLevels[7]) ticketGgImage.color = Color.white;
            if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("APWave1")) ticketCl1Image.color = Color.white;
            if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("APWave2")) ticketCl2Image.color = Color.white;
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

    public void Connect()
    {
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
        Hints = _hints;
        Chat = _chat;
        ShopHints = _shopHints;
        Ticket = _ticket;
        Kiosk = _kiosk;
        KioskSpoiler = _kioskSpoiler;
        cacmi = _cacmi;
        itemSent = _itemSent;
        contactList = _contactList;
        status = _status;
        
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
            ItemSent = _itemSent,
            ContactList = _contactList,
            Status = _status,
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
            itemSentToggle.isOn = savedData.ItemSent;
            contactListToggle.isOn = savedData.ContactList;
            statusToggle.isOn = savedData.Status;
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
