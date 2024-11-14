using UnityEngine;
using UnityEngine.EventSystems;

namespace NikoArchipelago.Stuff;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 _originalScale;
    private Vector3 _targetScale;
    private bool _isHovered;

    private void Start()
    {
        _originalScale = new Vector3(1f, 1f, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _targetScale = _originalScale * 1.1f;
        _isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _targetScale = _originalScale;
        _isHovered = false;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _isHovered ? _targetScale : _originalScale, Time.deltaTime * 10f);
    }
}