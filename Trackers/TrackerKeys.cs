using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NikoArchipelago.Trackers;

public class TrackerKeys : MonoBehaviour
{
    private static Image keyDisplayer;
    public void Start()
    {
        keyDisplayer = GameObject.Find("Key Displayer/keyImage").GetComponent<Image>();
    }

    public void Update()
    {
        if (ArchipelagoData.slotData == null) return;
        if (!ArchipelagoData.slotData.ContainsKey("key_level")) return;
        if (!ArchipelagoData.Options.Keylevels) return;
        switch (SceneManager.GetActiveScene().name)
        {
            case "Hairball City":
                keyDisplayer.sprite = Plugin.HairballKeySprite;
                //keyDisplayer.color = new Color(0f, 0.6196079f, 1f, 1f);
                break;
            case "Trash Kingdom":
                keyDisplayer.sprite = Plugin.TurbineKeySprite;
                //keyDisplayer.color = new Color(0.6705883f, 0.6705883f, 0.6705883f, 1f);
                break;
            case "Salmon Creek Forest":
                keyDisplayer.sprite = Plugin.SalmonKeySprite;
                //keyDisplayer.color = new Color(0.2117647f, 1f, 0f, 1f);
                break;
            case "Public Pool":
                keyDisplayer.sprite = Plugin.PoolKeySprite;
                //keyDisplayer.color = new Color(0f, 1f, 1f, 1f);
                break;
            case "The Bathhouse":
                keyDisplayer.sprite = Plugin.BathKeySprite;
                //keyDisplayer.color = new Color(1f, 0f, 0f, 1f);
                break;
            case "Tadpole inc":
                keyDisplayer.sprite = Plugin.TadpoleKeySprite;
                //keyDisplayer.color = new Color(0.6577053f, 1f, 0f, 1f);
                break;
            default:
                keyDisplayer.color = new Color(0.4361057f, 0f, 1f, 1f);
                break;
        }
    }
}