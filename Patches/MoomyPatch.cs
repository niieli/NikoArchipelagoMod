using System.Reflection;
using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class MoomyPatch
{
    private static string level;
    private static bool blockedLog;
    private static scrActionButtonPromt actionButtonPromt;
    private static AudioSource myMusic;
    private static long currentLocationID;
    private static scrHopOnBump NPCRewardHopper;
    private static MethodInfo startHamsterball;
    private static MethodInfo handleEffects;
    private static MethodInfo handleHamsterballControls;
    [HarmonyPatch(typeof(scrHamsterballMaster), "Start")]
    public static class MoomyStartPatch
    {
        private static void Postfix(scrHamsterballMaster __instance)
        {
            level = SceneManager.GetActiveScene().name;
            actionButtonPromt = GameObject.FindGameObjectWithTag("ActionButtonPromt").GetComponent<scrActionButtonPromt>();
            myMusic = __instance.GetComponent<AudioSource>();
            if (level == "Hairball City")
                currentLocationID = Locations.BaseID + 10;
            else if (level == "Salmon Creek Forest")
                currentLocationID = Locations.BaseID + 31;
            else if (level == "The Bathhouse")
                currentLocationID = Locations.BaseID + 55;
            NPCRewardHopper = __instance.NPCReward.GetComponent<scrHopOnBump>();
            startHamsterball = typeof(scrHamsterballMaster).GetMethod("StartHamsterball", BindingFlags.Instance | BindingFlags.NonPublic);
            handleEffects = typeof(scrHamsterballMaster).GetMethod("HandleEffects", BindingFlags.Instance | BindingFlags.NonPublic);
            handleHamsterballControls = typeof(scrHamsterballMaster).GetMethod("HandleHamsterballControls", BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
    
    [HarmonyPatch(typeof(scrHamsterballMaster), "FixedUpdate")]
    public static class MoomyUpdatePatch
    {
        private static int _currentSeedCount;
        private static string _currentLevelName;
        private static bool _answerFix, _answerFix2;

        [HarmonyPrefix]
        private static bool Prefix(scrHamsterballMaster __instance)
        {
            if (ArchipelagoData.slotData == null) return true;
            if (!ArchipelagoData.slotData.ContainsKey("seedsanity")) return true;
            if (int.Parse(ArchipelagoData.slotData["seedsanity"].ToString()) != 2) return true;
            
            var textbox = scrTextbox.instance;
            var questComplete = (bool)AccessTools.Field(typeof(scrHamsterballMaster), "questComplete").GetValue(__instance);
            var moomyDanceTimer = (float)AccessTools.Field(typeof(scrHamsterballMaster), "moomyDanceTimer").GetValue(__instance);
            bool allSeedsCollected = CheckAllSeeds(level);
            
            if (__instance.hamsterballStartTrigger.foundPlayer() && MyCharacterController.instance.state == MyCharacterController.States.Diving)
                startHamsterball.Invoke(__instance, null);
            handleEffects.Invoke(__instance, null);
            if (__instance.hamsterballStart.activeSelf)
                myMusic.Stop();
            if (__instance.hamsterball.activeSelf)
            {
                GameObjectChecker.IsHamsterball = true;
                handleHamsterballControls.Invoke(__instance, null);
                actionButtonPromt.Show("hamsterballStop", true);
            }

            if (!__instance.hamsterball.activeSelf)
            {
                GameObjectChecker.IsHamsterball = false;
                if (!allSeedsCollected && scrTextbox.instance.isOn && scrTextbox.instance.conversation == "MoomyQuest")
                {
                    if (!_answerFix)
                    {

                        textbox.conversationLocalized[0] =
                            $"##nikoimg4;You need all 10 seeds of {_currentLevelName} to obtain a reward!" +
                            $"\nYou have {_currentSeedCount}/10 for this level!";
                        _answerFix = true;
                    }
                    if (!_answerFix2)
                    {
                        var item = ArchipelagoClient._session.Locations.ScoutLocationsAsync(
                            HintCreationPolicy.CreateAndAnnounce, currentLocationID);
                        var itemName = item.Result[currentLocationID].ItemName;
                        var itemPlayer = item.Result[currentLocationID].Player;
                        var itemFlag = item.Result[currentLocationID].Flags;
                        string classification = "";
                        if (itemFlag.HasFlag(ItemFlags.Advancement))
                            classification = "Important";
                        else if (itemFlag.HasFlag(ItemFlags.NeverExclude))
                            classification = "Useful";
                        else if (itemFlag.HasFlag(ItemFlags.Trap))
                            classification = "Super Duper Important :)";
                        else if (itemFlag.HasFlag(ItemFlags.None))
                            classification = "Useless";
                        textbox.conversationLocalized[1] = 
                            $"I think <color=#6699FF>{itemPlayer}</color> would like their '<color=#FF6666>{itemName}</color>'!" +
                            $"\nMy expertise tells me it's {classification}##end;";
                        _answerFix2 = true;
                    }
                }
                else
                {
                    _answerFix = false;
                    _answerFix2 = false;
                }
                if (!questComplete && __instance.sunflowerSeedsParent.transform.childCount == 0 && allSeedsCollected)
                    __instance.Invoke("CompleteQuest", 0f);
            }
            if (__instance.NPC.activeSelf)
                __instance.NPCTrigger.ChangeIcon(scrTextboxTrigger.IconType.quest, true);
            if (!__instance.NPCReward.activeSelf)
                return false;
            moomyDanceTimer += Time.deltaTime;
            if (moomyDanceTimer <= 0.6000000238418579)
                return false;
            NPCRewardHopper.Hop();
            moomyDanceTimer = 0.0f;
            return false;
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