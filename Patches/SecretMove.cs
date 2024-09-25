using HarmonyLib;

namespace NikoArchipelago.Patches;

public class SecretMove
{
    [HarmonyPatch(typeof(scrObtainSecretMove), "Start")]
    public static class PatchSecretMove
    {
        private static void Postfix(scrObtainSecretMove __instance)
        {
            if (scrGameSaveManager.instance.gameData.generalGameData.appleAmount >= __instance.appleCost)
            {
                if (!scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("SecretMove Obtained"))
                {
                    scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Add("SecretMove Obtained");
                }
            }
        }
    }
    [HarmonyPatch(typeof(scrAppleSwitch), "Update")]
    public static class PatchSecretMove2
    {
        private static void Postfix(scrAppleSwitch __instance)
        {
            if (scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("SecretMove Obtained"))
            {
                __instance.secretMovePost.SetActive(true);
            }
        }
    }
}