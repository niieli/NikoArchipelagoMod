using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Archipelago.MultiClient.Net.Enums;
using BepInEx;
using KinematicCharacterController.Core;
using NikoArchipelago.Stuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NikoArchipelago;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager instance;
    private static readonly int Timer = Animator.StringToHash("Timer");
    private readonly GameObject hintNotification = Plugin.HintNotification;
    private readonly GameObject normalNotification = Plugin.ItemNotification;
    private static Transform _notificationListUI;
    private static Transform _notificationListFake;
    private static CanvasGroup _notificationListUICanvasGroup;
    private readonly List<GameObject> _activeNotifications = new();
    private readonly Queue<APNotification> _excessNotifications = new();
    public static readonly Queue<APNotification> AddNewNotification = new();
    private readonly string jsonFilePath = Path.Combine(Paths.PluginPath, "APSavedSettings.json");
    public static bool ShowDeathLink;
    public static string DeathLinkCause;
    public static bool ShowParty;
    public static bool IsUiOnScreen;
    
    // Notification Customization
    public static float notificationDuration = 3f;
    public static Color notificationBoxColor = Color.white;
    public static Color notificationBoxHintColor = new (0.3624749f, 0.3069153f, 0.5377358f, 1);
    public static Color notificationAccentColor = new(1, 0.54f, 0.76f, 0.72f);
    public static Color notificationProgColor = new(0.976f, 0.54f, 1, 0.72f);
    public static Color notificationUsefulColor = new(0.46f, 0.427f, 1, 0.72f);
    public static Color notificationTrapColor = new(1, 0.75f, 0.41f, 0.72f);
    public static Color notificationFillerColor = new(0.6f, 0.6f, 0.6f, 1);
    public static Color notificationTimerColor = new(0.486f, 0.60f, 1, 0.79f);
    public static Color notificationPlayerNameColor = new(0.4f, 0.60f, 1);
    public static Color notificationItemNameColor = new(1f, 0.4f, 0.4f);
    public static Color notificationHintPlayerNameColor = new(0.4f, 0.60f, 1);
    public static Color notificationHintItemNameColor = new(1f, 0.4f, 0.4f);
    public static Color notificationHintSenderColor = new(0.85f, 0.94f, 0.4f);
    public static Color notificationHintStateColor = new(0.7058824f, 0, 0.7176471f);
    public static Color notificationLocationNameColor = new(0.5566038f, 0.5566038f, 0.5566038f);

    public void Awake()
    {
        _notificationListUI = transform.Find("NotificationPanelUI");
        _notificationListFake = transform.Find("FakePanel");
        _notificationListUICanvasGroup = _notificationListUI.gameObject.GetComponent<CanvasGroup>();
        instance = this;
        LoadSettings();
    }

    public static void DefaultColors()
    {
        notificationProgColor = new Color(0.976f, 0.54f, 1, 0.72f);
        notificationUsefulColor = new Color(0.46f, 0.427f, 1, 0.72f);
        notificationTrapColor = new Color(1, 0.75f, 0.41f, 0.72f);
        notificationFillerColor = new Color(0.6f, 0.6f, 0.6f, 1);
        notificationTimerColor = new Color(0.486f, 0.60f, 1, 0.79f);
        notificationPlayerNameColor = new Color(0.4f, 0.60f, 1);
        notificationItemNameColor = new Color(1f, 0.4f, 0.4f);
        notificationHintPlayerNameColor = new(0.4f, 0.60f, 1);
        notificationHintItemNameColor = new(1f, 0.4f, 0.4f);
        notificationHintSenderColor = new(0.85f, 0.94f, 0.4f);
        notificationHintStateColor = new Color(0.7058824f, 0, 0.7176471f);
        notificationLocationNameColor = new Color(0.5566038f, 0.5566038f, 0.5566038f);
    }
    
    private void LoadSettings()
    {
        if (File.Exists(jsonFilePath))
        {
            notificationDuration = SavedData.Instance.NotificationDuration;
            if (ColorUtility.TryParseHtmlString("#"+SavedData.Instance.NotificationProgColor, out var progColor))
                notificationProgColor = progColor;
            if (ColorUtility.TryParseHtmlString("#"+SavedData.Instance.NotificationUsefulColor, out var usefulColor))
                notificationUsefulColor = usefulColor;
            if (ColorUtility.TryParseHtmlString("#"+SavedData.Instance.NotificationTrapColor, out var trapColor))
                notificationTrapColor = trapColor;
            if (ColorUtility.TryParseHtmlString("#"+SavedData.Instance.NotificationFillerColor, out var fillerColor))
                notificationFillerColor = fillerColor;
            if (ColorUtility.TryParseHtmlString("#"+SavedData.Instance.NotificationTimerColor, out var timerColor))
                notificationTimerColor = timerColor;
            if (ColorUtility.TryParseHtmlString("#"+SavedData.Instance.NotificationPlayerNameColor, out var playerNameColor))
                notificationPlayerNameColor = playerNameColor;
            if (ColorUtility.TryParseHtmlString("#"+SavedData.Instance.NotificationItemNameColor, out var itemNameColor))
                notificationItemNameColor = itemNameColor;
            if (ColorUtility.TryParseHtmlString("#"+SavedData.Instance.NotificationHintSenderColor, out var hintSenderColor))
                notificationHintSenderColor = hintSenderColor;
            if (ColorUtility.TryParseHtmlString("#"+SavedData.Instance.NotificationHintStateColor, out var hintStateColor))
                notificationHintStateColor = hintStateColor;
            if (ColorUtility.TryParseHtmlString("#"+SavedData.Instance.NotificationLocationNameColor, out var locationNameColor))
                notificationLocationNameColor = locationNameColor;
            Plugin.BepinLogger.LogInfo("Loaded saved Colors.");
        }
        else
        {
            DefaultColors();
            Plugin.BepinLogger.LogInfo("Loaded default Colors.");
        }
    }

    private void Update()
    {
        while (AddNewNotification.Count > 0)
            AddNotification(AddNewNotification.Dequeue());
        if (ShowDeathLink)
            StartCoroutine(ShowDeathLinkNotice(DeathLinkCause));
        if (ShowParty)
            StartCoroutine(ShowPartyNotice());
        _notificationListUICanvasGroup.alpha = IsUiOnScreen ? 0.35f : 1f;
    }

    public void AddNotification(APNotification notification)
    {
        if (_activeNotifications.Count >= 6)
        {
            _excessNotifications.Enqueue(notification);
        }
        else
        {
            ShowNotification(notification);
        }
    }

    private void TryShowNextFromQueue()
    {
        if (_activeNotifications.Count < 6 && _excessNotifications.Count > 0)
        {
            ShowNotification(_excessNotifications.Dequeue());
        }
    }

    private void ShowNotification(APNotification notification)
    {
        var isHint = notification.IsHint;

        GameObject fakeNotification = null;
        var notificationGameObject = Instantiate(isHint ? hintNotification : normalNotification, _notificationListUI);
        if (notificationGameObject != null)
            fakeNotification = Instantiate(notificationGameObject, _notificationListFake);
        Animator animator = notificationGameObject.GetComponent<Animator>();
        var time = notificationDuration;
        if (isHint)
            time *= 2.25f;
        if (animator != null)
            animator.SetFloat(Timer, time);
        StartCoroutine(UpdateNotificationTimer(notificationGameObject, notification, fakeNotification));

        _activeNotifications.Add(notificationGameObject);
    }

    private IEnumerator UpdateNotificationTimer(GameObject notificationObject, APNotification note, GameObject fake)
    {
        LoadSettings();
        var rt = fake.GetComponent<RectTransform>();
        
        var materialObject = notificationObject.transform.Find("BackgroundBox/NoteMaterial").gameObject;
        var materialImage = materialObject.GetComponent<Image>();
        var bgColor = note.BackgroundColor;
        if (note.ItemFlags.HasFlag(ItemFlags.Advancement))
        {
            bgColor = notificationProgColor;
            materialImage.material = Plugin.ProgNotificationTexture;
        }
        else if (note.ItemFlags.HasFlag(ItemFlags.NeverExclude))
        {
            bgColor = notificationUsefulColor;
            materialImage.material = Plugin.UsefulNotificationTexture;
        }
        else if (note.ItemFlags.HasFlag(ItemFlags.Trap))
        {
            bgColor = notificationTrapColor;
            materialImage.material = Plugin.TrapNotificationTexture;
        }
        else if (note.ItemFlags.HasFlag(ItemFlags.None))
        {
            bgColor = notificationFillerColor;
            materialImage.material = Plugin.FillerNotificationTexture;
        }
        else
            materialImage.material = Plugin.ProgNotificationTexture;
        materialObject.AddComponent<ScrollingEffect>().scrollSpeed = 0.1f;
        
        var duration = notificationDuration;
        var itemName = note.ItemName;
        var itemIcon = note.ItemIcon;
        var playerName = note.PlayerName;
        var senderName = note.SenderName;
        var locationName = note.LocationName;
        var hintState = note.HintState;
        var isHint = note.IsHint;
        
        var timerColor = notificationTimerColor;
        var playerColor = "#"+SavedData.Instance.NotificationPlayerNameColor;
        var itemColor = "#"+SavedData.Instance.NotificationItemNameColor;
        var senderColor = "#"+SavedData.Instance.NotificationHintSenderColor;
        var hintStateColor = "#"+SavedData.Instance.NotificationHintStateColor;

        var icon = notificationObject.transform.Find("ItemIcon").GetComponent<Image>();
        var text = notificationObject.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        var location = notificationObject.transform.Find("LocationName").GetComponent<TextMeshProUGUI>();
        var colorBox = notificationObject.transform.Find("BoxClassification").GetComponent<Image>();
        var timer = notificationObject.transform.Find("Timer").GetComponent<Image>();
        text.text = isHint ? $"<color={playerColor}>{playerName}</color>'s <color={itemColor}>{itemName}</color> <size=22>at</size> <color={senderColor}>{senderName}</color>'s <size=22>world</size>\n<color={hintStateColor}>({hintState})</color>" 
            : $"<color=#B400B7></color>You sent <color={itemColor}>{itemName}</color> to <color={playerColor}>{playerName}</color>";
        
        icon.sprite = itemIcon;
        location.text = $"({locationName})";
        colorBox.color = bgColor;
        timer.color = timerColor;
        float timeRemaining = duration;
        while (timeRemaining > 0)
        {
            timer.fillAmount = Mathf.Clamp01(timeRemaining / duration);
            yield return null;
            
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);

            Vector3 center = (corners[0] + corners[2]) / 2;
            
            notificationObject.transform.position = center;
            timeRemaining -= Time.deltaTime;
            notificationObject.GetComponent<Animator>().SetFloat(Timer, timeRemaining);
        }
        RemoveNotification(notificationObject, fake);
    }

    public void RemoveNotification(GameObject notification, GameObject fake)
    {
        if (!_activeNotifications.Contains(notification)) return;
        Destroy(fake);
        Destroy(notification);
        _activeNotifications.Remove(notification);
        TryShowNextFromQueue();
    }
    
    private static IEnumerator ShowDeathLinkNotice(string cause)
    {
        ShowDeathLink = false;
        var notice = Instantiate(Plugin.DeathLinkNotice, Plugin.NotifcationCanvas.transform);
        notice.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = cause;
        //yield return new WaitUntil(() => scrTransitionManager.instance.state == scrTransitionManager.States.idle);
        yield return new WaitForSeconds(10f);
        Destroy(notice);
    }
    
    private static IEnumerator ShowPartyNotice()
    {
        ShowParty = false;
        var confettiObjects = Resources.FindObjectsOfTypeAll<GameObject>()
                .Where(go => go.name == "Confetti");
        var confettiOg = confettiObjects.First();
        if (confettiOg != null)
        {
            var confetti = Instantiate(confettiOg);
            var pos = MyCharacterController.position;
            pos += new Vector3(0, 2f, -3f);
            confetti.transform.position = pos;
        }        
        var t = Instantiate(Plugin.NoticePartyTicket, Plugin.NotifcationCanvas.transform);
        yield return new WaitForSeconds(12.5f);
        Destroy(t);
    }
}