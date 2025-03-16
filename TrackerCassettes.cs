using NikoArchipelago.Archipelago;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

namespace NikoArchipelago;

public class TrackerCassettes : MonoBehaviour
{
    private static Image cassetteDisplayer;
    public void Start()
    {
        cassetteDisplayer = GameObject.Find("UI/CassetteDisplayer front/Cassette big").GetComponent<Image>();
    }

    public void Update()
    {
        if (ArchipelagoData.slotData == null) return;
        if (int.Parse(ArchipelagoData.slotData["cassette_logic"].ToString()) != 0) return;
        switch (SceneManager.GetActiveScene().name)
        {
            case "Hairball City":
                cassetteDisplayer.sprite = Plugin.HairballCassetteSprite;
                //cassetteDisplayer.color = new Color(0f, 0.6196079f, 1f, 1f);
                break;
            case "Trash Kingdom":
                cassetteDisplayer.sprite = Plugin.TurbineCassetteSprite;
                //cassetteDisplayer.color = new Color(0.6705883f, 0.6705883f, 0.6705883f, 1f);
                break;
            case "Salmon Creek Forest":
                cassetteDisplayer.sprite = Plugin.SalmonCassetteSprite;
                //cassetteDisplayer.color = new Color(0.2117647f, 1f, 0f, 1f);
                break;
            case "Public Pool":
                cassetteDisplayer.sprite = Plugin.PoolCassetteSprite;
                //cassetteDisplayer.color = new Color(0f, 1f, 1f, 1f);
                break;
            case "The Bathhouse":
                cassetteDisplayer.sprite = Plugin.BathCassetteSprite;
                //cassetteDisplayer.color = new Color(1f, 0f, 0f, 1f);
                break;
            case "Tadpole inc":
                cassetteDisplayer.sprite = Plugin.TadpoleCassetteSprite;
                //cassetteDisplayer.color = new Color(0.6577053f, 1f, 0f, 1f);
                break;
            case "GarysGarden":
                cassetteDisplayer.sprite = Plugin.GardenCassetteSprite;
                //cassetteDisplayer.color = new Color(1f, 0.5518868f, 0.8447619f, 1f);
                break;
            default:
                cassetteDisplayer.color = new Color(0.4361057f, 0f, 1f, 1f);
                break;
        }
    }
}