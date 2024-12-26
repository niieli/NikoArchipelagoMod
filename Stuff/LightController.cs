using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NikoArchipelago.Stuff;

public class LightController : MonoBehaviour
{
    private Image glowImage;
    public Color[] glowColors = [
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
    public float glowSpeed = 1f;

    private int _currentColorIndex;
    private float _timer;

    private void Start()
    {
        glowImage = GetComponent<Image>();
    }

    private void Update()
    {
        _timer += Time.deltaTime * glowSpeed;
        if (!(_timer >= 0.85f)) return;
        _timer = 0f;
        _currentColorIndex = (_currentColorIndex + 1) % glowColors.Length;
        glowImage.color = glowColors[_currentColorIndex];
    }
}
public class LightBlinking : MonoBehaviour
{
    public float flickerInterval = 1f;
    private Image _blinkImage;
    private float _timer;
    public float alpha = 1f;
    
    void Start()
    {
        _blinkImage = GetComponent<Image>();
    }

    private void Update()
    {
        _timer += Time.deltaTime * flickerInterval;
        if (!(_timer >= 0.85f)) return;
        _timer = 0f;
        if (alpha == 1f)
        {
            _blinkImage.color = _blinkImage.color with { a = 0f };
        }
        else
        {
            _blinkImage.color = _blinkImage.color with { a = 1f };
        }
        alpha = _blinkImage.color.a;
    }
}
