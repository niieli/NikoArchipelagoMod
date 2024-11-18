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
    public Toggle hintsToggle;
    public Tooltip hintsTooltip;
    public TooltipTrigger hintsTrigger;
    public Toggle shopHintsToggle;
    public Tooltip shopHintsTooltip;
    public TooltipTrigger shopHintsTrigger;
    public Toggle ticketToggle;
    public Tooltip ticketTooltip;
    public TooltipTrigger ticketTrigger;
    public Toggle kioskToggle;
    public Tooltip kioskTooltip;
    public TooltipTrigger kioskTrigger;
    public Toggle kioskSpoilerToggle;
    public Tooltip kioskSpoilerTooltip;
    public TooltipTrigger kioskSpoilerTrigger;
    public Toggle contactListToggle;
    public Tooltip contactListTooltip;
    public TooltipTrigger contactListTrigger;
    public Toggle cacmiToggle;
    public Tooltip cacmiTooltip;
    public TooltipTrigger cacmiTrigger;
    public Toggle itemSentToggle;
    public Tooltip itemSentTooltip;
    public TooltipTrigger itemSentTrigger;
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
    
    //New Menu stuff
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

    public void Start()
    {
        gameSaveManager = scrGameSaveManager.instance;
        
        formPanel = transform.Find("Panel").gameObject;
        openFormButton = transform.Find("APButton").gameObject.GetComponent<Button>();
        apButtonGameObject = transform.Find("APButton").gameObject;
        serverAddressField = formPanel.transform.Find("ServerAdress").GetComponent<InputField>();
        slotNameField = formPanel.transform.Find("SlotName").GetComponent<InputField>();
        passwordField = formPanel.transform.Find("Password").GetComponent<InputField>();
        rememberMeToggle = formPanel.transform.Find("Remember").gameObject.GetComponent<Toggle>();
        rememberMeTrigger = rememberMeToggle.gameObject.AddComponent<TooltipTrigger>();
        rememberMeTooltip = rememberMeToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        connectButton = formPanel.transform.Find("Button").gameObject.GetComponent<Button>();
        versionText = formPanel.transform.Find("Version").gameObject.GetComponent<TMP_Text>();
        
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
        hintsToggle = settingsPanel.transform.Find("Hints").gameObject.GetComponent<Toggle>();
        hintsTooltip = hintsToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        hintsTrigger = hintsToggle.gameObject.AddComponent<TooltipTrigger>();
        shopHintsToggle = settingsPanel.transform.Find("ShopHints").gameObject.GetComponent<Toggle>();
        shopHintsTooltip = shopHintsToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        shopHintsTrigger = shopHintsToggle.gameObject.AddComponent<TooltipTrigger>();
        itemSentToggle = settingsPanel.transform.Find("ItemSent").gameObject.GetComponent<Toggle>();
        itemSentTooltip = itemSentToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        itemSentTrigger = itemSentToggle.gameObject.AddComponent<TooltipTrigger>();
        
        // Trackers
        trackersButton = formPanel.transform.Find("Tabs/TrackersButton").gameObject.GetComponent<Button>();
        trackersButton.gameObject.AddComponent<ButtonHoverEffect>();
        trackersTrigger = trackersButton.gameObject.AddComponent<TooltipTrigger>();
        
        ticketToggle = trackersPanel.transform.Find("Ticket").gameObject.GetComponent<Toggle>();
        ticketTooltip = ticketToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        ticketTrigger = ticketToggle.gameObject.AddComponent<TooltipTrigger>();
        kioskToggle = trackersPanel.transform.Find("Kiosk").gameObject.GetComponent<Toggle>();
        kioskTooltip = kioskToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        kioskTrigger = kioskToggle.gameObject.AddComponent<TooltipTrigger>();
        kioskSpoilerToggle = trackersPanel.transform.Find("Spoiler").gameObject.GetComponent<Toggle>();
        kioskSpoilerTooltip = kioskSpoilerToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        kioskSpoilerTrigger = kioskSpoilerToggle.gameObject.AddComponent<TooltipTrigger>();
        contactListToggle = trackersPanel.transform.Find("ContactList").gameObject.GetComponent<Toggle>();
        contactListTooltip = contactListToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        contactListTrigger = contactListToggle.gameObject.AddComponent<TooltipTrigger>();
        
        // QOL
        qolButton = formPanel.transform.Find("Tabs/QOLButton").gameObject.GetComponent<Button>();
        qolButton.gameObject.AddComponent<ButtonHoverEffect>();
        qolTrigger = qolButton.gameObject.AddComponent<TooltipTrigger>();
        
        cacmiToggle = qolPanel.transform.Find("CACMI").gameObject.GetComponent<Toggle>();
        cacmiTooltip = cacmiToggle.transform.Find("Tooltip").gameObject.AddComponent<Tooltip>();
        cacmiTrigger = cacmiToggle.gameObject.AddComponent<TooltipTrigger>();
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
        LoadData();

        versionText.text = "Version "+Plugin.PluginVersion;
        formPanel.SetActive(false);
        apButtonGameObject.SetActive(true);
        openFormButton.onClick.AddListener(ToggleFormVisibility);
        connectButton.onClick.AddListener(Connect);
        settingsButton.onClick.AddListener(ShowSettings);
        trackersButton.onClick.AddListener(ShowTrackers);
        qolButton.onClick.AddListener(ShowQOL);
        
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
            formPanel.SetActive(false);
            apButtonGameObject.SetActive(false);
        }
        else
        {
            apButtonGameObject.SetActive(true);
        }
    }

    public void ToggleFormVisibility()
    {
        bool isActive = formPanel.activeSelf;
        formPanel.SetActive(!isActive);
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
        Hints = _hints;
        Chat = _chat;
        ShopHints = _shopHints;
        Ticket = _ticket;
        Kiosk = _kiosk;
        KioskSpoiler = _kioskSpoiler;
        cacmi = _cacmi;
        itemSent = _itemSent;
        contactList = _contactList;
        
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
