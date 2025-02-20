using TMPro;
using UnityEngine;

namespace NikoArchipelago.Stuff;

public class ArrowIndicator : MonoBehaviour
{
    public Transform player;
    public Transform target;
    public Transform arrowObject;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI distanceTextShadow;

    void Update()
    {
        if (target == null || player == null || arrowObject == null) return;

        Vector3 direction = target.position - player.position;

        //direction.y = 0;

        if (direction.sqrMagnitude > 0.01f) 
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            float rotationSpeed = 5f;
            arrowObject.rotation = Quaternion.Slerp(arrowObject.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        //Debug.Log("Direction: " + direction);
        //Debug.Log("Arrow Rotation: " + arrow3D.rotation.eulerAngles);
        Debug.DrawRay(player.position, direction, Color.red);

        if (distanceText != null)
        {
            float distance = direction.magnitude; 
            distanceText.text = $"{distance:F1} m";
            distanceTextShadow.text = $"{distance:F1} m";
        }

    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget; // Ziel dynamisch setzen
    }


}