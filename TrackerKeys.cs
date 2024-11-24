using NikoArchipelago.Archipelago;
using NikoArchipelago.Patches;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago;

public class TrackerKeys : MonoBehaviour
{
    public GameObject keyPanel;
    public GameObject keyHairball;
    public TextMeshProUGUI hairballAmountText;
    public GameObject keyTurbine;
    public TextMeshProUGUI turbineAmountText;
    public GameObject keySalmon;
    public TextMeshProUGUI salmonAmountText;
    public GameObject keyPool;
    public TextMeshProUGUI poolAmountText;
    public GameObject keyBath;
    public TextMeshProUGUI bathAmountText;
    public GameObject keyTadpole;
    public TextMeshProUGUI tadpoleAmountText;
    private static scrUIhider uiHider;
    private static scrUIhider uiHiderReference;
    public void Start()
    {
        keyPanel = transform.Find("TrackerKey").gameObject;
        keyHairball = keyPanel.transform.Find("KeyHairball").gameObject;
        hairballAmountText = keyHairball.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
        keyTurbine = keyPanel.transform.Find("KeyTurbine").gameObject;
        turbineAmountText = keyTurbine.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
        keySalmon = keyPanel.transform.Find("KeySalmon").gameObject;
        salmonAmountText = keySalmon.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
        keyPool = keyPanel.transform.Find("KeyPool").gameObject;
        poolAmountText = keyPool.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
        keyBath = keyPanel.transform.Find("KeyBath").gameObject;
        bathAmountText = keyBath.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
        keyTadpole = keyPanel.transform.Find("KeyTadpole").gameObject;
        tadpoleAmountText = keyTadpole.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
        
        uiHider = transform.Find("TrackerKey")?.gameObject.AddComponent<scrUIhider>();
        if (uiHider != null)
        {
            uiHiderReference = GameObject.Find("UI/Key Displayer").GetComponent<scrUIhider>();
            uiHider.useAlphaCurve = uiHiderReference.useAlphaCurve;
            uiHider.alphaCurve = uiHiderReference.alphaCurve;
            uiHider.animationCurve = uiHiderReference.animationCurve;
            uiHider.duration = uiHiderReference.duration;
            uiHider.hideOffset = uiHiderReference.hideOffset;
            TrackerDisplayerPatch.KeyUI = uiHider;
        }
    }

    public void Update()
    {
        if (ArchipelagoData.slotData == null) return;
        if (!ArchipelagoData.slotData.ContainsKey("key_level")) return;
        if (int.Parse(ArchipelagoData.slotData["key_level"].ToString()) != 1) return;
        hairballAmountText.text = ItemHandler.HairballKeyAmount.ToString();
        turbineAmountText.text = ItemHandler.TurbineKeyAmount.ToString();
        salmonAmountText.text = ItemHandler.SalmonKeyAmount.ToString();
        poolAmountText.text = ItemHandler.PoolKeyAmount.ToString();
        bathAmountText.text = ItemHandler.BathKeyAmount.ToString();
        tadpoleAmountText.text = ItemHandler.TadpoleKeyAmount.ToString();
        uiHiderReference.gameObject.SetActive(false);
        switch (SceneManager.GetActiveScene().name)
        {
            case "Hairball City":
                keyHairball.SetActive(true);
                keyTurbine.SetActive(false);
                keySalmon.SetActive(false);
                keyPool.SetActive(false);
                keyBath.SetActive(false);
                keyTadpole.SetActive(false);
                break;
            case "Trash Kingdom":
                keyHairball.SetActive(false);
                keyTurbine.SetActive(true);
                keySalmon.SetActive(false);
                keyPool.SetActive(false);
                keyBath.SetActive(false);
                keyTadpole.SetActive(false);
                break;
            case "Salmon Creek Forest":
                keyHairball.SetActive(false);
                keyTurbine.SetActive(false);
                keySalmon.SetActive(true);
                keyPool.SetActive(false);
                keyBath.SetActive(false);
                keyTadpole.SetActive(false);
                break;
            case "Public Pool":
                keyHairball.SetActive(false);
                keyTurbine.SetActive(false);
                keySalmon.SetActive(false);
                keyPool.SetActive(true);
                keyBath.SetActive(false);
                keyTadpole.SetActive(false);
                break;
            case "The Bathhouse":
                keyHairball.SetActive(false);
                keyTurbine.SetActive(false);
                keySalmon.SetActive(false);
                keyPool.SetActive(false);
                keyBath.SetActive(true);
                keyTadpole.SetActive(false);
                break;
            case "Tadpole inc":
                keyHairball.SetActive(false);
                keyTurbine.SetActive(false);
                keySalmon.SetActive(false);
                keyPool.SetActive(false);
                keyBath.SetActive(false);
                keyTadpole.SetActive(true);
                break;
            default:
                keyHairball.SetActive(false);
                keyTurbine.SetActive(false);
                keySalmon.SetActive(false);
                keyPool.SetActive(false);
                keyBath.SetActive(false);
                keyTadpole.SetActive(false);
                break;
        }
    }
}