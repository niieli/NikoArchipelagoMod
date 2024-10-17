using System.Collections;
using System.IO;
using BepInEx;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
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
    public Button connectButton;
    private scrGameSaveManager gameSaveManager;
    private static ArchipelagoClient _archipelagoClient;

    public void Start()
    {
        gameSaveManager = scrGameSaveManager.instance;
        formPanel = transform.Find("Panel")?.gameObject;
        openFormButton = transform.Find("APButton")?.gameObject.GetComponent<Button>();
        serverAddressField = transform.Find("Panel/ServerAdress")?.GetComponent<InputField>();
        slotNameField = transform.Find("Panel/SlotName")?.GetComponent<InputField>();
        passwordField = transform.Find("Panel/Password")?.GetComponent<InputField>();
        rememberMeToggle = transform.Find("Panel/Remember")?.gameObject.GetComponent<Toggle>();
        chatToggle = transform.Find("Panel/Chat")?.gameObject.GetComponent<Toggle>();
        hintsToggle = transform.Find("Panel/Hints")?.gameObject.GetComponent<Toggle>();
        shopHintsToggle = transform.Find("Panel/ShopHints")?.gameObject.GetComponent<Toggle>();
        connectButton = transform.Find("Panel/Button")?.gameObject.GetComponent<Button>();
        
        serverAddressField.text = "archipelago.gg:";
        slotNameField.text = "Player1";

        if (serverAddressField == null)
            Plugin.BepinLogger.LogError("Server Address Field is null!");
        if (slotNameField == null)
            Plugin.BepinLogger.LogError("Slot Name Field is null!");
        if (passwordField == null)
            Plugin.BepinLogger.LogError("Password Field is null!");
        if (formPanel == null)
            Plugin.BepinLogger.LogError("Form Panel is null!");
        if (openFormButton == null)
            Plugin.BepinLogger.LogError("APButton is null!");
        if (rememberMeToggle == null)
            Plugin.BepinLogger.LogError("Remember is null!");
        if (chatToggle == null)
            Plugin.BepinLogger.LogError("Chat is null!");
        if (hintsToggle == null)
            Plugin.BepinLogger.LogError("Hints is null!");
        if (shopHintsToggle == null)
            Plugin.BepinLogger.LogError("ShopHints is null!");

        formPanel.SetActive(false);
        openFormButton.onClick.AddListener(ToggleFormVisibility);
        connectButton.onClick.AddListener(Connect);
    }

    public void ToggleFormVisibility()
    {
        bool isActive = formPanel.activeSelf;
        formPanel.SetActive(!isActive);
    }

    public void Connect()
    {
        string serverAddress = serverAddressField.text;
        string slotName = slotNameField.text;
        string password = passwordField.text;
        bool rememberMe = rememberMeToggle.isOn;
        bool chat = chatToggle.isOn;
        bool hints = hintsToggle.isOn;
        bool shopHints = shopHintsToggle.isOn;

        Plugin.BepinLogger.LogInfo($"Server Address: {serverAddress}");
        Plugin.BepinLogger.LogInfo($"Slot Name: {slotName}");
        Plugin.BepinLogger.LogInfo($"Password: {password}");
        Plugin.BepinLogger.LogInfo($"Remember Me: {rememberMe}");
        Plugin.BepinLogger.LogInfo($"Chat: {chat}");
        Plugin.BepinLogger.LogInfo($"Hints: {hints}");
        Plugin.BepinLogger.LogInfo($"Shop Hints: {shopHints}");

        if (!serverAddress.IsNullOrWhiteSpace() && !slotName.IsNullOrWhiteSpace())
        {
            Plugin.BepinLogger.LogMessage("Connecting...");
            StartCoroutine(Connecting());
            if (scrGameSaveManager.saveName != Plugin.saveName)
            {
                scrGameSaveManager.saveName = Plugin.saveName;
                var savePath = Path.Combine(Plugin.ArchipelagoFolderPath, Plugin.saveName + "_" + Plugin.Seed + ".json");
                if (File.Exists(savePath))
                {
                    scrGameSaveManager.dataPath = savePath;
                    Plugin.BepinLogger.LogInfo("Found a SaveFile with the current SlotName & Port!");
                    //ArchipelagoConsole.LogMessage("Found a SaveFile with the current SlotName & Port!");
                    gameSaveManager.LoadGame();
                }
                else
                {
                    Plugin.newFile = true;
                    scrGameSaveManager.dataPath = savePath;
                    Plugin.BepinLogger.LogWarning("No SaveFile found. Creating a new one!");
                    //ArchipelagoConsole.LogMessage("No SaveFile found. Creating a new one!");
                    gameSaveManager.SaveGame();
                    gameSaveManager.LoadGame();
                    gameSaveManager.ClearSaveData();
                }
                scrTrainManager.instance.UseTrain(!Plugin.newFile ? gameSaveManager.gameData.generalGameData.currentLevel : 1, false);
                if (Plugin.newFile)
                {
                    ArchipelagoClient.Disconnect();
                    StartCoroutine(Plugin.FirstLoginFix());
                }
                Plugin.APSendNote($"Connected to {ArchipelagoClient.ServerData.Uri} successfully", 10F);
            }
        }
    }

    private IEnumerator Connecting()
    {
        _archipelagoClient.Connect();
        yield return new WaitForSeconds(3f);
    }
}