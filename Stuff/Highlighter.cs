using UnityEngine;
using UnityEngine.EventSystems;

namespace NikoArchipelago.Stuff;

public class Highlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject highlightPanel;
    //Maybe make it fancy?
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightPanel != null)
            highlightPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightPanel != null)
            highlightPanel.SetActive(false);
    }
}