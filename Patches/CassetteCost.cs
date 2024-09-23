using HarmonyLib;
using KinematicCharacterController.Core;
using UnityEngine;

namespace NikoArchipelago.Patches;

public class CassetteCost
{
    [HarmonyPatch(typeof(scrCassetteBuyer), "Update")]
    public static class PatchCassetteBuyer
    {
        private static void Postfix(scrCassetteBuyer __instance)
        {
            int count = 0;

            for (int i = 0; i < scrWorldSaveDataContainer.instance.coinFlags.Count; i++)
            {
                if (scrWorldSaveDataContainer.instance.coinFlags[i].Contains("cassetteCoin") || scrWorldSaveDataContainer.instance.coinFlags[i].Contains("cassetteCoin2"))
                {
                    count++;
                }
            }

            // Adjust the price based on how many cassette flags were found
            __instance.price = 5 * count;

            //Plugin.BepinLogger.LogInfo($"CassetteBuyer price updated to: {__instance.price} (based on {count} cassette coins found).");
        }
    }
}