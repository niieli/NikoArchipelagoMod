using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class SwimCourse : MonoBehaviour
{
    private void Update()
    {
        if (ArchipelagoClient.SwimmingAcquired) return;
        if (MyCharacterController.instance.isTouchingWater)
            MyCharacterController.instance.BackToDaisy();
    }
}