using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NikoArchipelago.Patches;

public class APMainMenu
{
    //TODO: Make in-game UI
    public static GameObject TitleScreen;
    public static GameObject MainMenuObject;
    static GameObject archipelagoButtonObject = new GameObject("ArchipelagoButton");
    [HarmonyPatch(typeof(MainMenu), "Awake")]
    public class MainMenuPatch
    {
        static void Postfix(MainMenu __instance)
        {
            if (MainMenuObject != null)
            {
                var rectTransform = archipelagoButtonObject.AddComponent<RectTransform>();
                var archipelagoButton = archipelagoButtonObject.AddComponent<CustomButton>();
                archipelagoButton.ButtonImage.sprite = Plugin.APIconSprite;
                archipelagoButtonObject.transform.SetParent(MainMenuObject.transform);
                rectTransform.sizeDelta = new Vector2(200, 60);
                rectTransform.anchoredPosition = new Vector2(0, -300);
                archipelagoButton.OnClickFinished.AddListener(new UnityEngine.Events.UnityAction(() => OnArchipelagoButtonPressed(__instance)));
            }
        }
        static void OnArchipelagoButtonPressed(MainMenu mainMenuInstance)
        {
            if (ArchipelagoMenu.Instance != null)
            {
                MenuHelpers.SetActiveMenu(ArchipelagoMenu.Instance, true);
                mainMenuInstance.Default = (Selectable)ArchipelagoMenu.Instance.ArchipelagoTextInput;
            }
            else
            {
                Plugin.BepinLogger.LogError("ArchipelagoMenu.Instance is null! Ensure the menu is properly initialized.");
            }
        }
    }

    public static void TitleScreenAPLogo()
    {
        if (TitleScreen != null)
        {
            var ApLogo = new GameObject("APLogo");
            ApLogo.AddComponent<Image>().sprite = Plugin.APIconSprite;
            ApLogo.layer = LayerMask.NameToLayer("UI");
            ApLogo.transform.SetParent(TitleScreen.transform);
            ApLogo.transform.position = new Vector3((float)1354.03, (float)137.3608, (float)2.12);
            ApLogo.transform.localPosition = new Vector3((float)1433.787, (float)-2.9115, (float)2.12);
            ApLogo.transform.localScale = new Vector3((float)3.2573, (float)3.0645, (float)7.8854);
        }
    }
}
public class ArchipelagoMenu : Menu
{
    public static ArchipelagoMenu Instance; // Singleton instance
    public InputField ArchipelagoTextInput;
    public Button SubmitButton;

    protected override void Awake()
    {
        Instance = this;
        base.Awake();

        // Ensure the text input field is initialized
        if (ArchipelagoTextInput != null)
        {
            ArchipelagoTextInput.text = string.Empty;
        }

        // Add listener to the submit button to handle text input
        if (SubmitButton != null)
        {
            SubmitButton.onClick.AddListener(OnSubmitText);
        }
    }

    // This function will be called when the user presses the submit button
    public void OnSubmitText()
    {
        string inputText = ArchipelagoTextInput.text;
        if (!string.IsNullOrEmpty(inputText))
        {
            Plugin.BepinLogger.LogInfo("Archipelago Input: " + inputText);
            // Handle the input text (e.g., pass it to another function or system)
        }
        else
        {
            Plugin.BepinLogger.LogError("No input provided.");
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (ArchipelagoTextInput != null)
        {
            ArchipelagoTextInput.Select(); // Automatically focus the input field
        }
    }
}
