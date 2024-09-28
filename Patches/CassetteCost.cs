using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class CassetteCost
{
    [HarmonyPatch(typeof(scrCassetteBuyer), "Update")]
    public static class PatchCassetteBuyer
    {
        static bool _changed = false;

        private static void Postfix(scrCassetteBuyer __instance)
        {
            //TODO: Get Option value and use the corresponding method
            int count = 0;
            for (int i = 0; i < scrWorldSaveDataContainer.instance.coinFlags.Count; i++)
            {
                if (scrWorldSaveDataContainer.instance.coinFlags[i].Contains("cassetteCoin") || scrWorldSaveDataContainer.instance.coinFlags[i].Contains("cassetteCoin2"))
                {
                    count++;
                }
            }
            string currentScene = SceneManager.GetActiveScene().name;
            
            switch (currentScene)
            {
                case "Hairball City":
                    __instance.price = 5 + (5*count);
                    break;
                case "Trash Kingdom":
                    __instance.price = 15 + (5*count); 
                    break;
                case "Salmon Creek Forest":
                    __instance.price = 25 + (5*count); 
                    break;
                case "Public Pool":
                    __instance.price = 35 + (5*count); 
                    break;
                case "The Bathhouse":
                    __instance.price = 45 + (5*count); 
                    break;
                case "Tadpole inc":
                    __instance.price = 55 + (5*count); 
                    break;
                case "GarysGarden":
                    __instance.price = 65 + (5*count); 
                    break;
            }
            if (!_changed)
            {
                Plugin.BepinLogger.LogInfo($"CassetteBuyer price updated to: {__instance.price} (Scene: {currentScene}, based on {count} cassette coins found).");
                _changed = true;
            }
            // int count = 1;
            // var list = scrGameSaveManager.instance.gameData.worldsData;
            // for (int i = 0; i < list.Count ; i++)
            // {
            //     if (list[i].coinFlags.Contains("cassetteCoin") || list[i].coinFlags.Contains("cassetteCoin2"))
            //     {
            //         count++;
            //     }
            // }
            // __instance.price = 5 * count;
            //
            // if (!_changed)
            // {
            //     Plugin.BepinLogger.LogInfo($"CassetteBuyer price updated to: {__instance.price} (based on {count} cassette coins found).");
            //     _changed = true;
            // }
        }
    }
}