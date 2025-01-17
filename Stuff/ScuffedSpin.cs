using UnityEngine;
using UnityEngine.Serialization;

namespace NikoArchipelago.Stuff;

public class ScuffedSpin : MonoBehaviour
{
    public float spinSpeed = 100f;
    public void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);
    }
}