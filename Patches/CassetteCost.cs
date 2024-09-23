using KinematicCharacterController.Core;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class CassetteCost
{
    private static scrCassetteBuyer buyer;
    private int count;
    public void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            if (scrWorldSaveDataContainer.instance.coinFlags[i].Contains("cassetteCoin") || scrWorldSaveDataContainer.instance.coinFlags[i].Contains("cassetteCoin2"))
            {
                count++;
                buyer.price = 5 * count;
            }
        }
    }
}