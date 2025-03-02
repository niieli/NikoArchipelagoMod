using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NikoArchipelago;

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
        if (int.Parse(ArchipelagoData.slotData["key_level"].ToString()) != 1) return;
        switch (SceneManager.GetActiveScene().name)
        {
            case "Hairball City":
                keyDisplayer.color = new Color(0f, 0.6196079f, 1f, 1f);
                break;
            case "Trash Kingdom":
                keyDisplayer.color = new Color(0.6705883f, 0.6705883f, 0.6705883f, 1f);
                break;
            case "Salmon Creek Forest":
                keyDisplayer.color = new Color(0.2117647f, 1f, 0f, 1f);
                break;
            case "Public Pool":
                keyDisplayer.color = new Color(0f, 1f, 1f, 1f);
                break;
            case "The Bathhouse":
                keyDisplayer.color = new Color(1f, 0f, 0f, 1f);
                break;
            case "Tadpole inc":
                keyDisplayer.color = new Color(0.6577053f, 1f, 0f, 1f);
                break;
            default:
                keyDisplayer.color = new Color(0.4361057f, 0f, 1f, 1f);
                break;
        }
    }
}