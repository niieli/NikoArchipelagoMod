using System.Reflection;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class MoomyPatch
{
    private static string level;
    private static bool blockedLog;
    [HarmonyPatch(typeof(scrHamsterballMaster), "Start")]
    public static class MoomyStartPatch
    {
        private static void Postfix(scrHamsterballMaster __instance)
        {
            level = SceneManager.GetActiveScene().name;
        }
    }
    
    [HarmonyPatch(typeof(scrHamsterballMaster), "FixedUpdate")]
    public static class MoomyUpdatePatch
    {
        private static int _currentSeedCount;
        private static string _currentLevelName;

        [HarmonyPrefix]
        private static bool Prefix(scrHamsterballMaster __instance)
        {
            if (ArchipelagoData.slotData == null) return true;
            if (!ArchipelagoData.slotData.ContainsKey("seedsanity")) return true;
            if (int.Parse(ArchipelagoData.slotData["seedsanity"].ToString()) != 2) return true;
            var textbox = scrTextbox.instance;
            var questComplete = Traverse.Create(__instance).Field("questComplete").GetValue<bool>();
            bool allSeedsCollected = CheckAllSeeds(level);
            if (__instance.sunflowerSeedsParent.transform.childCount == 0 && !textbox.isOn && !questComplete)
            {
                if (!allSeedsCollected)
                {
                    __instance.NPCReward.SetActive(false);
                    __instance.NPC.SetActive(true);
                    if (blockedLog) return false;
                    Plugin.BepinLogger.LogInfo($"Reward blocked: Need all 10 seeds of {level}");
                    blockedLog = true;
                    return false;
                }
            }
            else if (!allSeedsCollected && __instance.sunflowerSeedsParent.transform.childCount == 0 && textbox.isOn && textbox.characterName == "Moomy" && !questComplete)
            {
                textbox.textMesh.text = $"You need all 10 seeds of {_currentLevelName} to obtain my reward!\nYou have {_currentSeedCount}/10 for this level!";
                textbox.textMesh.maxVisibleCharacters = 99;
                if (GameInput.GetButtonDown("Action"))
                    textbox.EndConversation();
                return false;
            } else if (!allSeedsCollected && __instance.sunflowerSeedsParent.transform.childCount == 0 && __instance.NPCReward.activeSelf && !__instance.nikoInBall.activeSelf)
            {
                Traverse.Create(__instance).Field("questComplete").SetValue(false);
                __instance.NPCReward.SetActive(false);
                __instance.NPC.SetActive(true);
                textbox.textMesh.text = $"You need all 10 seeds of {_currentLevelName} to obtain my reward!\nYou have {_currentSeedCount}/10 for this level!";
                textbox.textMesh.maxVisibleCharacters = 99;
                if (GameInput.GetButtonDown("Action"))
                    textbox.EndConversation();
            }
            return true;
        }

    private static bool CheckAllSeeds(string currentLevel)
        {
            int amountOfSeeds = 0;
            switch (currentLevel)
            {
                case "Hairball City":
                    amountOfSeeds = ItemHandler.HairballSeedAmount;
                    _currentLevelName = "Hairball City";
                    break;
                case "Salmon Creek Forest":
                    amountOfSeeds = ItemHandler.SalmonSeedAmount;
                    _currentLevelName = "Salmon Creek Forest";
                    break;
                case "The Bathhouse":
                    amountOfSeeds = ItemHandler.BathSeedAmount;
                    _currentLevelName = "Bathhouse";
                    break;
                default:
                    return false;
            }
            _currentSeedCount = amountOfSeeds;
            return amountOfSeeds >= 10;
        }
    }
}