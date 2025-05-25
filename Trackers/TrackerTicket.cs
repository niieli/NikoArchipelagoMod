using NikoArchipelago.Archipelago;
using NikoArchipelago.Patches;
using UnityEngine;
using UnityEngine.UI;

namespace NikoArchipelago.Trackers;

public class TrackerTicket : MonoBehaviour
{
    public GameObject ticketPanel;
    public Image ticketHcImage;
    public Image ticketTtImage;
    public Image ticketSfcImage;
    public Image ticketPpImage;
    public Image ticketBathImage;
    public Image ticketHqImage;
    public Image ticketGgImage;
    public Image ticketCl1Image;
    public Image ticketCl2Image;
    private scrGameSaveManager gameSaveManager;
    private static scrUIhider uiHider;

    private void Start()
    {
        gameSaveManager = scrGameSaveManager.instance;
        ticketPanel = transform.Find("TrackerTicket").gameObject;
        ticketHcImage = ticketPanel.transform.Find("TicketHairball").GetComponent<Image>();
        ticketTtImage = ticketPanel.transform.Find("TicketTurbine").GetComponent<Image>();
        ticketSfcImage = ticketPanel.transform.Find("TicketSalmon").GetComponent<Image>();
        ticketPpImage = ticketPanel.transform.Find("TicketPool").GetComponent<Image>();
        ticketBathImage = ticketPanel.transform.Find("TicketBath").GetComponent<Image>();
        ticketHqImage = ticketPanel.transform.Find("TicketTadpole").GetComponent<Image>();
        ticketGgImage = ticketPanel.transform.Find("TicketGarden").GetComponent<Image>();
        ticketCl1Image = ticketPanel.transform.Find("ContactList1").GetComponent<Image>();
        ticketCl2Image = ticketPanel.transform.Find("ContactList2").GetComponent<Image>();
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
        if (ticketGgImage == null) Plugin.BepinLogger.LogError("TicketGgImage is null");
        
        ticketPanel.SetActive(true);
        
        uiHider = transform.Find("TrackerTicket")?.gameObject.AddComponent<scrUIhider>();
        if (uiHider != null)
        {
            var reference = GameObject.Find("UI/Apple Displayer").GetComponent<scrUIhider>();
            uiHider.useAlphaCurve = reference.useAlphaCurve;
            uiHider.alphaCurve = reference.alphaCurve;
            uiHider.animationCurve = reference.animationCurve;
            uiHider.duration = 0.5f;
            uiHider.hideOffset = new Vector3(-350, 0, 0);
            TrackerDisplayerPatch.TicketUI = uiHider;
        }
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
        if (ArchipelagoData.slotData == null) return;
        if (ArchipelagoData.slotData.ContainsKey("garden_access"))
        {
            if (ItemHandler.Garden || int.Parse(ArchipelagoData.slotData["garden_access"].ToString()) == 0 
                && gameSaveManager.gameData.generalGameData.unlockedLevels[7]) ticketGgImage.color = Color.white;
        }
        else if (gameSaveManager.gameData.generalGameData.unlockedLevels[7])
        {
            ticketGgImage.color = Color.white;
        }
        if (!ArchipelagoMenu.contactList)
        {
            ticketCl1Image.gameObject.SetActive(false); 
            ticketCl2Image.gameObject.SetActive(false);
        }
        if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("APWave1")) ticketCl1Image.color = Color.white;
        if (gameSaveManager.gameData.generalGameData.generalFlags.Contains("APWave2")) ticketCl2Image.color = Color.white;
    }
}