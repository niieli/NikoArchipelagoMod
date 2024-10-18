using UnityEngine;
using UnityEngine.UI;

namespace NikoArchipelago;

public class TrackerTicket : MonoBehaviour
{
    public GameObject ticketPanel;
    public Image ticketHcImage;
    public Image ticketTtImage;
    public Image ticketSfcImage;
    public Image ticketPpImage;
    public Image ticketBathImage;
    public Image ticketHqImage;
    private scrGameSaveManager gameSaveManager;

    private void Start()
    {
        gameSaveManager = scrGameSaveManager.instance;
        ticketPanel = transform.Find("TrackerTicket")?.gameObject;
        ticketHcImage = transform.Find("TrackerTicket/TicketHairball")?.GetComponent<Image>();
        ticketTtImage = transform.Find("TrackerTicket/TicketTurbine")?.GetComponent<Image>();
        ticketSfcImage = transform.Find("TrackerTicket/TicketSalmon")?.GetComponent<Image>();
        ticketPpImage = transform.Find("TrackerTicket/TicketPool")?.GetComponent<Image>();
        ticketBathImage = transform.Find("TrackerTicket/TicketBath")?.GetComponent<Image>();
        ticketHqImage = transform.Find("TrackerTicket/TicketTadpole")?.GetComponent<Image>();

        if (ticketPanel == null)
        {
            Plugin.BepinLogger.LogError("TicketPanel is null");
            return;
        }

        if (ticketHcImage == null) Plugin.BepinLogger.LogError("TicketHcImage is null");
        if (ticketTtImage == null) Plugin.BepinLogger.LogError("TicketTtImage is null");
        if (ticketSfcImage == null) Plugin.BepinLogger.LogError("TicketSfcImage is null");
        if (ticketPpImage == null) Plugin.BepinLogger.LogError("TicketPpImage is null");
        if (ticketBathImage == null) Plugin.BepinLogger.LogError("TicketBathImage is null");
        if (ticketHqImage == null) Plugin.BepinLogger.LogError("TicketHqImage is null");
        
        ticketPanel.SetActive(true);
        //var uIhider = ticketPanel.AddComponent<scrUIhider>();
        var cGroup = ticketPanel.AddComponent<CanvasGroup>();
        cGroup.alpha = 0f;
        //uIhider.visible = false;
        //uIhider.useAlphaCurve = true;
        
    }

    public void Update()
    {
        if (!Plugin.saveReady) return;
        if (gameSaveManager.gameData.generalGameData.unlockedLevels[2]) ticketHcImage.color = Color.white;
        if (gameSaveManager.gameData.generalGameData.unlockedLevels[3]) ticketTtImage.color = Color.white;
        if (gameSaveManager.gameData.generalGameData.unlockedLevels[4]) ticketSfcImage.color = Color.white;
        if (gameSaveManager.gameData.generalGameData.unlockedLevels[5]) ticketPpImage.color = Color.white;
        if (gameSaveManager.gameData.generalGameData.unlockedLevels[6]) ticketBathImage.color = Color.white;
        if (gameSaveManager.gameData.generalGameData.unlockedLevels[7]) ticketHqImage.color = Color.white;
    }
}