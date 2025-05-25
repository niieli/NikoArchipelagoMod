using System.IO;
using BepInEx;
using Newtonsoft.Json;
using UnityEngine;

namespace NikoArchipelago;

public class SavedData
{
    private readonly string jsonFilePath = Path.Combine(Paths.PluginPath, "APSavedSettings.json");
    public static SavedData Instance { get; private set; } = new();
    public string Host { get; set; } = ArchipelagoMenu.Host;
    public string SlotName { get; set; } = ArchipelagoMenu.SlotName;
    public bool RememberMe { get; set; } = ArchipelagoMenu.RememberMe;
    public bool Chat { get; set; } = ArchipelagoMenu.Chat;
    public bool ShopHints { get; set; } = ArchipelagoMenu.ShopHints;
    public bool Hint { get; set; } = ArchipelagoMenu.Hints;
    public bool Ticket { get; set; } = ArchipelagoMenu.Ticket;
    public bool Kiosk { get; set; } = ArchipelagoMenu.Kiosk;
    public bool KioskSpoiler { get; set; } = ArchipelagoMenu.KioskSpoiler;
    public bool CACMI { get; set; } = ArchipelagoMenu.cacmi;
    public bool APNotifications { get; set; } = ArchipelagoMenu.APNotifications;
    public bool ContactList { get; set; } = ArchipelagoMenu.contactList;
    public bool Status { get; set; } = ArchipelagoMenu.status;
    public bool Tooltips { get; set; } = ArchipelagoMenu.Tooltips;
    public bool KALMI { get; set; } = ArchipelagoMenu.kalmi;
    public bool CassetteSpoiler { get; set; } = ArchipelagoMenu.CassetteSpoiler;
    public bool SeasonalThemes { get; set; } = ArchipelagoMenu.SeasonalThemes;
    public bool SkipPickup { get; set; } = ArchipelagoMenu.SkipPickup;
    //public string NotificationBoxColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationBoxColor);
    //public string NotificationBoxHintColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationBoxHintColor);
    //public string NotificationAccentColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationAccentColor);
    public float NotificationDuration { get; set; } = NotificationManager.notificationDuration;
    public string NotificationProgColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationProgColor);
    public string NotificationUsefulColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationUsefulColor);
    public string NotificationTrapColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationTrapColor);
    public string NotificationFillerColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationFillerColor);
    public string NotificationTimerColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationTimerColor);
    public string NotificationPlayerNameColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationPlayerNameColor);
    public string NotificationItemNameColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationItemNameColor);
    //public string NotificationHintPlayerNameColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationHintPlayerNameColor);
    //public string NotificationHintItemNameColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationHintItemNameColor);
    public string NotificationHintStateColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationHintStateColor);
    public string NotificationLocationNameColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationLocationNameColor);

    public void LoadSettings()
    {
        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            var data = JsonConvert.DeserializeObject<SavedData>(json);
            if (data == null) return;
            Instance = data;
            if (json.Contains("ItemSent") || !json.Contains("APNotifications"))
            {
                Host = data.Host;
                SlotName = data.SlotName;
                RememberMe = data.RememberMe;
                Chat = data.Chat;
                ShopHints = data.ShopHints;
                Hint = data.Hint;
                Ticket = data.Ticket;
                Kiosk = data.Kiosk;
                KioskSpoiler = data.KioskSpoiler;
                CACMI = data.CACMI;
                ContactList = data.ContactList;
                Status = data.Status;
                Tooltips = data.Tooltips;
                KALMI = data.KALMI;
                CassetteSpoiler = data.CassetteSpoiler;
                SeasonalThemes = data.SeasonalThemes;
                SkipPickup = data.SkipPickup;
                APNotifications = true;
                //NotificationBoxColor = ColorUtility.ToHtmlStringRGBA(Color.white);
                //NotificationBoxHintColor = ColorUtility.ToHtmlStringRGBA(new Color(0.3624749f, 0.3069153f, 0.5377358f, 1));
                //NotificationAccentColor = ColorUtility.ToHtmlStringRGBA(new Color(1, 0.54f, 0.76f, 0.72f));
                NotificationDuration = 3f;
                NotificationProgColor = ColorUtility.ToHtmlStringRGBA(new Color(0.976f, 0.54f, 1, 0.72f));
                NotificationUsefulColor = ColorUtility.ToHtmlStringRGBA(new Color(0.46f, 0.427f, 1, 0.72f));
                NotificationTrapColor = ColorUtility.ToHtmlStringRGBA(new Color(1, 0.75f, 0.41f, 0.72f));
                NotificationFillerColor = ColorUtility.ToHtmlStringRGBA(new Color(0.6f, 0.6f, 0.6f, 1));
                NotificationTimerColor = ColorUtility.ToHtmlStringRGBA(new Color(0.486f, 0.60f, 1, 0.79f));
                NotificationPlayerNameColor = ColorUtility.ToHtmlStringRGBA(new Color(0.4f, 0.60f, 1));
                NotificationItemNameColor = ColorUtility.ToHtmlStringRGBA(new Color(1f, 0.4f, 0.4f));
                //NotificationHintPlayerNameColor = ColorUtility.ToHtmlStringRGBA(new(0.4f, 0.60f, 1));
                //NotificationHintItemNameColor = ColorUtility.ToHtmlStringRGBA(new(1f, 0.4f, 0.4f));
                NotificationHintStateColor = ColorUtility.ToHtmlStringRGBA(new Color(0.7058824f, 0, 0.7176471f));
                NotificationLocationNameColor = ColorUtility.ToHtmlStringRGBA(new Color(0.5566038f, 0.5566038f, 0.5566038f));
                Plugin.BepinLogger.LogInfo("Found missing settings! Added missing settings!");
                string jsonData = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(jsonFilePath, jsonData);
            }
            Plugin.BepinLogger.LogInfo("Loaded saved settings.");
        }
        else
        {
            Host = "archipelago.gg:";
            SlotName = "Player1";
            RememberMe = true;
            Chat = true;
            ShopHints = true;
            Hint = true;
            Ticket = true;
            Kiosk = true;
            KioskSpoiler = true;
            CACMI = true;
            APNotifications = true;
            ContactList = true;
            Status = true;
            Tooltips = true;
            KALMI = true;
            CassetteSpoiler = true;
            SeasonalThemes = true;
            SkipPickup = true;
            //NotificationBoxColor = ColorUtility.ToHtmlStringRGBA(Color.white);
            //NotificationBoxHintColor = ColorUtility.ToHtmlStringRGBA(new Color(0.3624749f, 0.3069153f, 0.5377358f, 1));
            //NotificationAccentColor = ColorUtility.ToHtmlStringRGBA(new Color(1, 0.54f, 0.76f, 0.72f));
            NotificationDuration = 3f;
            NotificationProgColor = ColorUtility.ToHtmlStringRGBA(new Color(0.976f, 0.54f, 1, 0.72f));
            NotificationUsefulColor = ColorUtility.ToHtmlStringRGBA(new Color(0.46f, 0.427f, 1, 0.72f));
            NotificationTrapColor = ColorUtility.ToHtmlStringRGBA(new Color(1, 0.75f, 0.41f, 0.72f));
            NotificationFillerColor = ColorUtility.ToHtmlStringRGBA(new Color(0.6f, 0.6f, 0.6f, 1));
            NotificationTimerColor = ColorUtility.ToHtmlStringRGBA(new Color(0.486f, 0.60f, 1, 0.79f));
            NotificationPlayerNameColor = ColorUtility.ToHtmlStringRGB(new Color(0.4f, 0.60f, 1));
            NotificationItemNameColor = ColorUtility.ToHtmlStringRGB(new Color(1f, 0.4f, 0.4f));
            //NotificationHintPlayerNameColor = ColorUtility.ToHtmlStringRGBA(new(0.4f, 0.60f, 1));
            //NotificationHintItemNameColor = ColorUtility.ToHtmlStringRGBA(new(1f, 0.4f, 0.4f));
            NotificationHintStateColor = ColorUtility.ToHtmlStringRGB(new Color(0.7058824f, 0, 0.7176471f));
            NotificationLocationNameColor = ColorUtility.ToHtmlStringRGB(new Color(0.5566038f, 0.5566038f, 0.5566038f));
            Plugin.BepinLogger.LogInfo("Created new saved settings.");
            SaveSettings();
        }
    }
    
    public void SaveSettings()
    {
        string jsonData = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(jsonFilePath, jsonData);
    }
}