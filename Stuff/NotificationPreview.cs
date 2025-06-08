using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NikoArchipelago.Stuff;

public class NotificationPreview : MonoBehaviour
{
    private Slider durationSlider;
    private Slider rSlider, gSlider, bSlider;
    private Image previewBoxColor, previewTimerColor, previewHintBoxColor, previewHintTimerColor;
    private TextMeshProUGUI previewText, previewHintText;
    private TMP_Dropdown dropdown, dropdownNotification;
    private TextMeshProUGUI redValue, redValueShadow, greenValue, 
        greenValueShadow, blueValue, blueValueShadow, durationValue, durationValueShadow;
    private Color tempItemColor, tempPlayerColor, tempHintItemColor, tempHintPlayerColor, tempHintStateColor,
        tempProgColor, tempUsefulColor, tempTrapColor, tempFillerColor, tempTimerColor, tempHintSenderColor;
    private Button applyButton, closeButton, openButton, resetButton;
    private GameObject panel, panelPrevention;
    private Image noteMaterial, noteHintMaterial, noteIcon, noteHintIcon;
    private bool isHint;
    private ScrollingEffect noteScrollEffect, noteHintScrollEffect;
    private readonly SavedData.NotificationColors tempColors = new();
    private float tempDuration;

    private void Start()
    {
        panel = transform.Find("NoteSettingsPanel").gameObject;
        panelPrevention = transform.Find("NoteSettingsPrevention").gameObject;
        previewBoxColor = panel.transform.Find("ItemNotification/BoxClassification").GetComponent<Image>();
        previewTimerColor = panel.transform.Find("ItemNotification/Timer").GetComponent<Image>();
        previewText = panel.transform.Find("ItemNotification/Text").GetComponent<TextMeshProUGUI>();

        previewHintBoxColor = panel.transform.Find("HintNotification/BoxClassification").GetComponent<Image>();
        previewHintTimerColor = panel.transform.Find("HintNotification/Timer").GetComponent<Image>();
        previewHintText = panel.transform.Find("HintNotification/Text").GetComponent<TextMeshProUGUI>();
        dropdown = panel.transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
        dropdownNotification = panel.transform.Find("DropdownNotification").GetComponent<TMP_Dropdown>();
        
        durationSlider = panel.transform.Find("DurationSlider").GetComponent<Slider>();
        rSlider = panel.transform.Find("RSlider").GetComponent<Slider>();
        gSlider = panel.transform.Find("GSlider").GetComponent<Slider>();
        bSlider = panel.transform.Find("BSlider").GetComponent<Slider>();

        noteScrollEffect = panel.transform.Find("ItemNotification/BackgroundBox/NoteMaterial").gameObject.AddComponent<ScrollingEffect>();
        noteScrollEffect.scrollSpeed = 0.1f;
        noteMaterial = noteScrollEffect.gameObject.GetComponent<Image>();

        noteHintScrollEffect = panel.transform.Find("HintNotification/BackgroundBox/NoteMaterial").gameObject.AddComponent<ScrollingEffect>();
        noteHintScrollEffect.scrollSpeed = 0.1f;
        noteHintMaterial = noteHintScrollEffect.gameObject.GetComponent<Image>();

        noteIcon = panel.transform.Find("ItemNotification/ItemIcon").gameObject.GetComponent<Image>();
        noteHintIcon = panel.transform.Find("HintNotification/ItemIcon").gameObject.GetComponent<Image>();

        applyButton = panel.transform.Find("ApplyButton").GetComponent<Button>();
        closeButton = panel.transform.Find("CloseButton").GetComponent<Button>();
        openButton = transform.Find("NoteSettingsButton").GetComponent<Button>();
        resetButton = panel.transform.Find("ResetButton").GetComponent<Button>();

        applyButton.onClick.AddListener(ApplySettings);
        closeButton.onClick.AddListener(CloseSettings);
        openButton.onClick.AddListener(OpenSettings);
        resetButton.onClick.AddListener(ResetSettings);

        redValue = rSlider.transform.Find("Value").GetComponent<TextMeshProUGUI>();
        greenValue = gSlider.transform.Find("Value").GetComponent<TextMeshProUGUI>();
        blueValue = bSlider.transform.Find("Value").GetComponent<TextMeshProUGUI>();
        redValueShadow = rSlider.transform.Find("ValueShadow").GetComponent<TextMeshProUGUI>();
        greenValueShadow = gSlider.transform.Find("ValueShadow").GetComponent<TextMeshProUGUI>();
        blueValueShadow = bSlider.transform.Find("ValueShadow").GetComponent<TextMeshProUGUI>();
        durationValueShadow = durationSlider.transform.Find("ValueShadow").GetComponent<TextMeshProUGUI>();
        durationValue = durationSlider.transform.Find("Value").GetComponent<TextMeshProUGUI>();

        durationSlider.onValueChanged.AddListener(UpdatePreview);
        rSlider.onValueChanged.AddListener(UpdateColorPreview);
        gSlider.onValueChanged.AddListener(UpdateColorPreview);
        bSlider.onValueChanged.AddListener(UpdateColorPreview);
        dropdown.onValueChanged.AddListener(UpdateOptions);
        dropdownNotification.onValueChanged.AddListener(UpdateNotification);
        
        isHint = false;
        panel.transform.Find("HintNotification").gameObject.SetActive(false);

        LoadSavedSettings();
        CloseSettings();
        rSlider.value = tempProgColor.r;
        gSlider.value = tempProgColor.g;
        bSlider.value = tempProgColor.b;
        previewTimerColor.color = tempColors.timerColor;
        previewText.text = $"<color=#B400B7></color>You sent <color=#{ColorUtility.ToHtmlStringRGB(tempColors.itemColor)}>Important Item</color> to " +
                           $"<color=#{ColorUtility.ToHtmlStringRGB(tempColors.playerNameColor)}>Frog King</color>";
        previewHintText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(tempColors.playerNameColor)}>Frog King</color>'s " +
                               $"<color=#{ColorUtility.ToHtmlStringRGB(tempColors.itemColor)}>Important Item</color> " +
                               $"<size=22>at</size> <color=#{ColorUtility.ToHtmlStringRGB(tempColors.hintSenderColor)}>Assistant #22</color>'s <size=22>world</size>\n" +
                               $"<color=#{ColorUtility.ToHtmlStringRGB(tempColors.hintStateColor)}>(Priority)</color>";
    }

    private void UpdateColorPreview(float _)
    {
        redValue.text = rSlider.value.ToString("0.00");
        greenValue.text = gSlider.value.ToString("0.00");
        blueValue.text = bSlider.value.ToString("0.00");
        redValueShadow.text = rSlider.value.ToString("0.00");
        greenValueShadow.text = gSlider.value.ToString("0.00");
        blueValueShadow.text = bSlider.value.ToString("0.00");

        var c = new Color(rSlider.value, gSlider.value, bSlider.value);

        switch (dropdown.value)
        {
            case 0:
                //Accent(Prog) Color
                c.a = 0.7215f;
                previewHintBoxColor.color = c;
                tempProgColor = c;
                previewBoxColor.color = c;
                break;
            case 1:
                //Accent(Useful) Color
                c.a = 0.7215f;
                previewHintBoxColor.color = c;
                tempUsefulColor = c;
                previewBoxColor.color = c;
                break;
            case 2:
                //Accent(Trap) Color
                c.a = 0.7215f;
                previewHintBoxColor.color = c;
                tempTrapColor = c;
                previewBoxColor.color = c;
                break;
            case 3:
                //Accent(Filler) Color
                c.a = 0.7215f;
                previewHintBoxColor.color = c;
                tempFillerColor = c;
                previewBoxColor.color = c;
                break;
            case 4:
                //Timer Color
                c.a = 0.79f;
                previewHintTimerColor.color = c;
                tempTimerColor = c;
                previewTimerColor.color = c;
                UpdatePreview(tempDuration);
                break;
            // case 5 when isHint: // For the time being not needed
            //     dropdownNotification.value = 0;
            //     break;
            case 5:
            {
                //Item Color
                tempItemColor = c;
                previewText.text = $"<color=#B400B7></color>You sent <color=#{ColorUtility.ToHtmlStringRGB(c)}>Important Item</color> to " +
                                   $"<color=#{ColorUtility.ToHtmlStringRGB(tempPlayerColor)}>Frog King</color>";
                break;
            }
            // case 6 when isHint: // For the time being not needed
            //     dropdownNotification.value = 0;
            //     break;
            case 6:
            {
                //Player Color
                tempPlayerColor = c;
                previewText.text = $"<color=#B400B7></color>You sent <color=#{ColorUtility.ToHtmlStringRGB(tempItemColor)}>Important Item</color> to " +
                                   $"<color=#{ColorUtility.ToHtmlStringRGB(c)}>Frog King</color>";
                break;
            }
            //HintState Color
            case 7 when isHint:
                tempHintStateColor = c;
                previewHintText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(tempPlayerColor)}>Frog King</color>'s " +
                                       $"<color=#{ColorUtility.ToHtmlStringRGB(tempItemColor)}>Important Item</color> " +
                                       $"<size=22>at</size> <color=#{ColorUtility.ToHtmlStringRGB(tempHintSenderColor)}>Assistant #22</color>'s <size=22>world</size>\n" +
                                       $"<color=#{ColorUtility.ToHtmlStringRGB(c)}>(Priority)</color>";
                break;
            case 7:
                dropdownNotification.value = 1;
                break;
            //HintSender Color
            case 8 when isHint:
                tempHintSenderColor = c;
                previewHintText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(tempPlayerColor)}>Frog King</color>'s " +
                                       $"<color=#{ColorUtility.ToHtmlStringRGB(tempItemColor)}>Important Item</color> " +
                                       $"<size=22>at</size> <color=#{ColorUtility.ToHtmlStringRGB(c)}>Assistant #22</color>'s <size=22>world</size>\n" +
                                       $"<color=#{ColorUtility.ToHtmlStringRGB(tempHintStateColor)}>(Priority)</color>";
                break;
            case 8:
                dropdownNotification.value = 1;
                break;
        }
        tempColors.SetByIndex(dropdown.value, c);
    }

    private void UpdateOptions(int _)
    {
        switch (dropdown.value)
        {
            case 0:
                noteMaterial.material = Plugin.ProgNotificationTexture;
                noteHintMaterial.material = Plugin.ProgNotificationTexture;
                noteIcon.sprite = Plugin.ApProgressionSprite;
                noteHintIcon.sprite = Plugin.ApProgressionSprite;
                noteScrollEffect.Invoke("Start",0f);
                noteHintScrollEffect.Invoke("Start",0f);
                break;
            case 1:
                noteMaterial.material = Plugin.UsefulNotificationTexture;
                noteHintMaterial.material = Plugin.UsefulNotificationTexture;
                noteIcon.sprite = Plugin.ApUsefulSprite;
                noteHintIcon.sprite = Plugin.ApUsefulSprite;
                noteScrollEffect.Invoke("Start",0f);
                noteHintScrollEffect.Invoke("Start",0f);
                break;
            case 2:
                noteMaterial.material = Plugin.TrapNotificationTexture;
                noteHintMaterial.material = Plugin.TrapNotificationTexture;
                noteIcon.sprite = Plugin.ApTrap2Sprite;
                noteHintIcon.sprite = Plugin.ApTrap2Sprite;
                noteScrollEffect.Invoke("Start",0f);
                noteHintScrollEffect.Invoke("Start",0f);
                break;
            case 3:
                noteMaterial.material = Plugin.FillerNotificationTexture;
                noteHintMaterial.material = Plugin.FillerNotificationTexture;
                noteIcon.sprite = Plugin.ApFillerSprite;
                noteHintIcon.sprite = Plugin.ApFillerSprite;
                noteScrollEffect.Invoke("Start",0f);
                noteHintScrollEffect.Invoke("Start",0f);
                break;
        }
        Color c = tempColors.GetByIndex(_);
        rSlider.value = c.r;
        gSlider.value = c.g;
        bSlider.value = c.b;
        UpdatePreview(tempDuration);
    }

    private void UpdateNotification(int _)
    {
        switch (dropdownNotification.value)
        {
            case 0:
                //Sent Notification
                isHint = false;
                panel.transform.Find("ItemNotification").gameObject.SetActive(true);
                panel.transform.Find("HintNotification").gameObject.SetActive(false);
                break;
            case 1:
                //Hint Notification
                isHint = true;
                panel.transform.Find("HintNotification").gameObject.SetActive(true);
                panel.transform.Find("ItemNotification").gameObject.SetActive(false);
                break;
        }
    }

    private void UpdatePreview(float duration)
    {
        durationValueShadow.text = durationSlider.value.ToString("0.00")+ "s";
        durationValue.text = durationSlider.value.ToString("0.00")+ "s";
        StopAllCoroutines();
        StartCoroutine(PreviewTimer(duration));
        tempDuration = duration;
    }

    private IEnumerator PreviewTimer(float duration)
    {
        var timeRemaining = duration;
        previewTimerColor.fillAmount = 1;
        previewHintTimerColor.fillAmount = 1;
        while (timeRemaining > 0)
        {
            previewTimerColor.fillAmount = Mathf.Clamp01(timeRemaining / duration);
            previewHintTimerColor.fillAmount = Mathf.Clamp01(timeRemaining / duration);
            yield return null;
            timeRemaining -= Time.deltaTime;
        }
    }

    public void ApplySettings()
    {
        tempProgColor.a = 0.7215f;
        tempUsefulColor.a = 0.7215f;
        tempTrapColor.a = 0.7215f;
        tempFillerColor.a = 0.7215f;
        tempTimerColor.a = 0.79f;
        SavedData.Instance.NotificationDuration = durationSlider.value;
        SavedData.Instance.NotificationProgColor = ColorUtility.ToHtmlStringRGB(tempColors.progColor);
        SavedData.Instance.NotificationUsefulColor = ColorUtility.ToHtmlStringRGB(tempColors.usefulColor);
        SavedData.Instance.NotificationTrapColor = ColorUtility.ToHtmlStringRGB(tempColors.trapColor);
        SavedData.Instance.NotificationFillerColor = ColorUtility.ToHtmlStringRGB(tempColors.fillerColor);
        SavedData.Instance.NotificationTimerColor = ColorUtility.ToHtmlStringRGB(tempColors.timerColor);
        SavedData.Instance.NotificationItemNameColor = ColorUtility.ToHtmlStringRGB(tempColors.itemColor);
        SavedData.Instance.NotificationPlayerNameColor = ColorUtility.ToHtmlStringRGB(tempColors.playerNameColor);
        SavedData.Instance.NotificationHintSenderColor = ColorUtility.ToHtmlStringRGB(tempColors.hintSenderColor);
        SavedData.Instance.NotificationHintStateColor = ColorUtility.ToHtmlStringRGB(tempColors.hintStateColor);
        SavedData.Instance.SaveSettings();
    }

    public void CloseSettings()
    {
        panel.SetActive(false);
        panelPrevention.gameObject.SetActive(false);
        gameObject.GetComponent<Toggle>().interactable = true;
        gameObject.transform.parent.Find("Chat").gameObject.SetActive(true);
        gameObject.transform.parent.Find("Hints").gameObject.SetActive(true);
        gameObject.transform.parent.Find("ShopHints").gameObject.SetActive(true);
    }

    public void OpenSettings()
    {
        panel.SetActive(true);
        panelPrevention.gameObject.SetActive(true);
        gameObject.GetComponent<Toggle>().interactable = false;
        gameObject.transform.parent.Find("Chat").gameObject.SetActive(false);
        gameObject.transform.parent.Find("Hints").gameObject.SetActive(false);
        gameObject.transform.parent.Find("ShopHints").gameObject.SetActive(false);
        LoadSavedSettings();
    }

    private void LoadSavedSettings()
    {
        dropdown.value = 0;
        durationSlider.value = SavedData.Instance.NotificationDuration;
        if (ColorUtility.TryParseHtmlString("#" + SavedData.Instance.NotificationProgColor, out var progColor))
        {
            tempColors.progColor = progColor;
            tempProgColor.r = progColor.r;
            tempProgColor.g = progColor.g;
            tempProgColor.b = progColor.b;
            rSlider.value = progColor.r;
            gSlider.value = progColor.g;
            bSlider.value = progColor.b;
        }

        if (ColorUtility.TryParseHtmlString("#" + SavedData.Instance.NotificationUsefulColor, out var usefulColor))
        {
            tempColors.usefulColor = usefulColor;
            tempUsefulColor.r = usefulColor.r;
            tempUsefulColor.g = usefulColor.g;
            tempUsefulColor.b = usefulColor.b;
        }

        if (ColorUtility.TryParseHtmlString("#" + SavedData.Instance.NotificationTrapColor, out var trapColor))
        {
            tempColors.trapColor = trapColor;
            tempTrapColor.r = trapColor.r;
            tempTrapColor.g = trapColor.g;
            tempTrapColor.b = trapColor.b;
        }

        if (ColorUtility.TryParseHtmlString("#" + SavedData.Instance.NotificationFillerColor, out var fillerColor))
        {
            tempColors.fillerColor = fillerColor;
            tempFillerColor.r = fillerColor.r;
            tempFillerColor.g = fillerColor.g;
            tempFillerColor.b = fillerColor.b;
        }

        if (ColorUtility.TryParseHtmlString("#" + SavedData.Instance.NotificationTimerColor, out var timerColor))
        {
            tempColors.timerColor = timerColor;
            tempTimerColor.r = timerColor.r;
            tempTimerColor.g = timerColor.g;
            tempTimerColor.b = timerColor.b;
        }
        
        if (ColorUtility.TryParseHtmlString("#" + SavedData.Instance.NotificationItemNameColor, out var itemNameColor))
        {
            tempColors.itemColor = itemNameColor;
            tempItemColor.r = itemNameColor.r;
            tempItemColor.g = itemNameColor.g;
            tempItemColor.b = itemNameColor.b;
        }

        if (ColorUtility.TryParseHtmlString("#" + SavedData.Instance.NotificationPlayerNameColor,
                out var playerNameColor))
        {
            tempColors.playerNameColor = playerNameColor;
            tempPlayerColor.r = playerNameColor.r;
            tempPlayerColor.g = playerNameColor.g;
            tempPlayerColor.b = playerNameColor.b;
        }

        if (ColorUtility.TryParseHtmlString("#" + SavedData.Instance.NotificationHintStateColor,
                out var hintStateColor))
        {
            tempColors.hintStateColor = hintStateColor;
            tempHintStateColor.r = hintStateColor.r;
            tempHintStateColor.g = hintStateColor.g;
            tempHintStateColor.b = hintStateColor.b;
        }
        
        if (ColorUtility.TryParseHtmlString("#" + SavedData.Instance.NotificationHintSenderColor,
                out var hintSenderColor))
        {
            tempColors.hintSenderColor = hintSenderColor;
            tempHintSenderColor.r = hintSenderColor.r;
            tempHintSenderColor.g = hintSenderColor.g;
            tempHintSenderColor.b = hintSenderColor.b;
        }
        tempProgColor.a = 0.7215f;
        tempUsefulColor.a = 0.7215f;
        tempTrapColor.a = 0.7215f;
        tempFillerColor.a = 0.7215f;
        tempTimerColor.a = 0.79f;
    }

    private void ResetSettings()
    {
        durationSlider.value = 3f;
        tempProgColor = new Color(0.976f, 0.54f, 1, 0.72f);
        tempUsefulColor = new Color(0.46f, 0.427f, 1, 0.72f);
        tempTrapColor = new Color(1, 0.75f, 0.41f, 0.72f);
        tempFillerColor = new Color(0.6f, 0.6f, 0.6f, 1);
        tempTimerColor = new Color(0.486f, 0.60f, 1, 0.79f);
        tempPlayerColor = new Color(0.4f, 0.60f, 1);
        tempItemColor = new Color(1f, 0.4f, 0.4f);
        tempHintPlayerColor = new Color(0.4f, 0.60f, 1);
        tempHintItemColor = new Color(1f, 0.4f, 0.4f);
        tempHintSenderColor = new Color(0.85f, 0.94f, 0.4f);
        tempHintStateColor = new Color(0.7058824f, 0, 0.7176471f);
        
        tempColors.progColor = tempProgColor;
        tempColors.usefulColor = tempUsefulColor;
        tempColors.trapColor = tempTrapColor;
        tempColors.fillerColor = tempFillerColor;
        tempColors.timerColor = tempTimerColor;
        tempColors.playerNameColor = tempPlayerColor;
        tempColors.itemColor = tempItemColor;
        tempColors.hintStateColor = tempHintStateColor;
        tempColors.hintSenderColor = tempHintSenderColor;

        if (dropdown.value == 0)
        {
            dropdown.value = 1;
            tempColors.SetByIndex(dropdown.value, tempUsefulColor);
        }
        else
        {
            dropdown.value = 0;
            tempColors.SetByIndex(dropdown.value, tempProgColor);
        }
        
        previewTimerColor.color = tempColors.timerColor;
        previewText.text = $"<color=#B400B7></color>You sent <color=#{ColorUtility.ToHtmlStringRGB(tempColors.itemColor)}>Important Item</color> to " +
                           $"<color=#{ColorUtility.ToHtmlStringRGB(tempColors.playerNameColor)}>Frog King</color>";
        previewHintText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(tempColors.playerNameColor)}>Frog King</color>'s " +
                               $"<color=#{ColorUtility.ToHtmlStringRGB(tempColors.itemColor)}>Important Item</color> " +
                               $"<size=22>at</size> <color=#{ColorUtility.ToHtmlStringRGB(tempColors.hintSenderColor)}>Assistant #22</color>'s <size=22>world</size>\n" +
                               $"<color=#{ColorUtility.ToHtmlStringRGB(tempColors.hintStateColor)}>(Priority)</color>";
        dropdown.value = 0;
        //notificationLocationNameColor = new Color(0.5566038f, 0.5566038f, 0.5566038f);
    }
}
