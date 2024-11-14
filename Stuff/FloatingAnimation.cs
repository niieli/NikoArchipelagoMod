using UnityEngine;

namespace NikoArchipelago.Stuff;

public class FloatingAnimation : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float floatHeight = 5f;

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = _startPosition + new Vector3(0, Mathf.Sin(Time.time * floatSpeed) * floatHeight, 0);
    }
}