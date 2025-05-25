namespace NikoArchipelago.Stuff;

using UnityEngine;
using UnityEngine.UI;

public class ScrollingEffect : MonoBehaviour
{
    public float scrollSpeed = 0.03f;
    private Material _material;

    private void Start()
    {
        _material = GetComponent<Image>().material;
    }

    private void Update()
    {
        var offset = Time.time * scrollSpeed;
        _material.mainTextureOffset = new Vector2(offset, offset);

    }
}
public class FallingSnowflakesBackground : MonoBehaviour
{
    public float scrollSpeed = 0.07f;
    private Material material;

    private void Start()
    {
        material = GetComponent<Image>().material;
    }

    private void Update()
    {
        float offset = Time.time * scrollSpeed;
        float storm = Time.time * 0.005f;
        material.mainTextureOffset = new Vector2(storm, offset);

    }
}