using UnityEngine;

namespace NikoArchipelago.Patches;

public class ScuffedSpin : MonoBehaviour
{
    public void Update()
    {
        transform.Rotate(0f, 100f * Time.deltaTime, 0f, Space.Self);
    }
}