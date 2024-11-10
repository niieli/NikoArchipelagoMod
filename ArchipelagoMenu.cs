using System.Collections;
using System.IO;
using BepInEx;
using Newtonsoft.Json;
using NikoArchipelago.Archipelago;
using TMPro;
using UnityEngine;
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
    public Toggle chatToggle;
    public Toggle hintsToggle;
    public Toggle shopHintsToggle;
    public Toggle ticketToggle;
    public Toggle kioskToggle;
    public Toggle kioskSpoilerToggle;
    public Toggle cacmiToggle;
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
    private readonly string jsonFilePath = Path.Combine(Paths.PluginPath, "APSavedSettings.json");
    private GameObject apButtonGameObject;
    public static string Seed;
    public static bool Hints;
    public static bool ShopHints;
    public static bool Chat;
    public static bool Ticket;
    public static bool Kiosk;
    public static bool KioskSpoiler;
    public static bool CACMI;

    public void Start()
    {
        gameSaveManager = scrGameSaveManager.instance;
        
        formPanel = transform.Find("Panel")?.gameObject;
        openFormButton = transform.Find("APButton")?.gameObject.GetComponent<Button>();
        apButtonGameObject = transform.Find("APButton")?.gameObject;
        serverAddressField = transform.Find("Panel/ServerAdress")?.GetComponent<InputField>();
        slotNameField = transform.Find("Panel/SlotName")?.GetComponent<InputField>();
        passwordField = transform.Find("Panel/Password")?.GetComponent<InputField>();
        rememberMeToggle = transform.Find("Panel/Remember")?.gameObject.GetComponent<Toggle>();
        chatToggle = transform.Find("Panel/Chat")?.gameObject.GetComponent<Toggle>();
        hintsToggle = transform.Find("Panel/Hints")?.gameObject.GetComponent<Toggle>();
        shopHintsToggle = transform.Find("Panel/ShopHints")?.gameObject.GetComponent<Toggle>();
        ticketToggle = transform.Find("Panel/Ticket")?.gameObject.GetComponent<Toggle>();
        kioskToggle = transform.Find("Panel/Kiosk")?.gameObject.GetComponent<Toggle>();
        kioskSpoilerToggle = transform.Find("Panel/Spoiler")?.gameObject.GetComponent<Toggle>();
        cacmiToggle = transform.Find("Panel/CACMI")?.gameObject.GetComponent<Toggle>();
        connectButton = transform.Find("Panel/Button")?.gameObject.GetComponent<Button>();
        versionText = transform.Find("Panel/Version")?.gameObject.GetComponent<TMP_Text>();
        
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
        LoadData();

        versionText.text = "Version "+Plugin.PluginVersion;
        formPanel.SetActive(false);
        apButtonGameObject.SetActive(true);
        openFormButton.onClick.AddListener(ToggleFormVisibility);
        connectButton.onClick.AddListener(Connect);
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
        Hints = _hints;
        Chat = _chat;
        ShopHints = _shopHints;
        Ticket = _ticket;
        Kiosk = _kiosk;
        KioskSpoiler = _kioskSpoiler;
        CACMI = _cacmi;
        
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
        }
    }

    private void SaveSettings(SavedData data)
    {
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(jsonFilePath, jsonData);
    }
}