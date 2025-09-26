using System;
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
    public bool Notices { get; set; } = ArchipelagoMenu.Notices;
    public bool VerboseLogging { get; set; } = ArchipelagoMenu.VerboseLogging;
    public float NotificationDuration { get; set; } = NotificationManager.notificationDuration;
    public string NotificationProgColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationProgColor);
    public string NotificationUsefulColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationUsefulColor);
    public string NotificationTrapColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationTrapColor);
    public string NotificationFillerColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationFillerColor);
    public string NotificationTimerColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationTimerColor);
    public string NotificationPlayerNameColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationPlayerNameColor);
    public string NotificationItemNameColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationItemNameColor);
    public string NotificationHintSenderColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationHintSenderColor);
    public string NotificationHintStateColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationHintStateColor);
    public string NotificationLocationNameColor { get; set; } = ColorUtility.ToHtmlStringRGBA(NotificationManager.notificationLocationNameColor);
    public bool NotificationShowJunk { get; set; } = true;
    public bool NotificationShowSelfSent { get; set; } = true;

    public void LoadSettings()
    {
        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            try
            {
                JsonConvert.PopulateObject(json, this);
                Plugin.BepinLogger.LogInfo("Loaded saved settings.");
            }
            catch (Exception e)
            {
                Plugin.BepinLogger.LogWarning("Failed to load settings, using defaults.");
                Plugin.BepinLogger.LogError(e);
            }
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
            NotificationHintSenderColor = ColorUtility.ToHtmlStringRGBA(new Color(0.85f, 0.94f, 0.4f));
            NotificationHintStateColor = ColorUtility.ToHtmlStringRGB(new Color(0.7058824f, 0, 0.7176471f));
            NotificationLocationNameColor = ColorUtility.ToHtmlStringRGB(new Color(0.5566038f, 0.5566038f, 0.5566038f));
            NotificationShowJunk = true;
            NotificationShowSelfSent = true;
            Plugin.BepinLogger.LogInfo("Created new saved settings.");
            SaveSettings();
        }
    }
    
    public void SaveSettings()
    {
        string jsonData = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(jsonFilePath, jsonData);
    }
    
    [Serializable]
    public class NotificationColors {
        public Color progColor;
        public Color usefulColor;
        public Color trapColor;
        public Color fillerColor;
        public Color timerColor;
        public Color itemColor;
        public Color playerNameColor;
        public Color hintStateColor;
        public Color hintSenderColor;
        
        public Color GetByIndex(int index)
        {
            return index switch
            {
                0 => progColor,
                1 => usefulColor,
                2 => trapColor,
                3 => fillerColor,
                4 => timerColor,
                5 => itemColor,
                6 => playerNameColor,
                7 => hintStateColor,
                8 => hintSenderColor,
                _ => Color.white,
            };
        }

        public void SetByIndex(int index, Color color)
        {
            switch (index)
            {
                case 0: progColor = color; break;
                case 1: usefulColor = color; break;
                case 2: trapColor = color; break;
                case 3: fillerColor = color; break;
                case 4: timerColor = color; break;
                case 5: itemColor = color; break;
                case 6: playerNameColor = color; break;
                case 7: hintStateColor = color; break;
                case 8: hintSenderColor = color; break;
            }
        }
    }
}