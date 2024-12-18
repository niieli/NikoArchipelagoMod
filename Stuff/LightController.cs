using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace NikoArchipelago.Stuff;

public class LightController : MonoBehaviour
{
    private Image glowImage;
    public Color[] glowColors;
    public float glowSpeed = 1.0f;

    private int _currentColorIndex;
    private float _timer;

    private void Start()
    {
        glowImage = GetComponent<Image>();
        glowColors =
        [
            new Color(1f, 0f, 0f), 
            new Color(1f, 0.5f, 0f), 
            new Color(0f, 1f, 0f), 
            new Color(0f, 0.9062204f, 1f),
            new Color(1f, 0.5529412f, 0.788992f),
            new Color(0.2071452f, 0f, 1f), 
            new Color(1f, 0f, 0.6565428f), 
            new Color(1f, 1f, 0f), 
            new Color(0f, 1f, 0.4670312f), 
            new Color(0f, 0f, 1f), 
            new Color(0.9038821f, 0.5518868f, 1f), 
            new Color(1f, 1f, 1f), 
            new Color(0f, 0.444962f, 1f), 
            new Color(1f, 0f, 0.9598556f), 
            new Color(0.5898412f, 0.5882353f, 1f), 
            new Color(1f, 0.7369196f, 0f), 
        ];
    }

    private void Update()
    {
        _timer += Time.deltaTime * glowSpeed;
        if (!(_timer >= 1f)) return;
        _timer = 0f;
        _currentColorIndex = (_currentColorIndex + 1) % glowColors.Length;
        glowImage.color = glowColors[_currentColorIndex];
    }
}
