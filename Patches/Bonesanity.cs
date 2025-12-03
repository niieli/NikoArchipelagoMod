using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class Bonesanity
{
    public static int ID;
    private static bool _bonesanityOn;
    private static string _currentLevelName;
    private static int _currentScoutID;
    private static int _currentBoneCount;
    private static bool _answerFix;
    private static bool _answerFix2;
    private static bool _isInArea;
    private static bool _playedSound;
    [HarmonyPatch(typeof(scrBone), "Start")]
    public static class BonesanityPatch
    {
        private static void Postfix(scrBone __instance)
        {
            if (!ArchipelagoMenu.cacmi) return;
            var flag = __instance.name;
            if (scrWorldSaveDataContainer.instance.miscFlags.Contains(flag) || GameObjectChecker.LoggedInstances.Contains(__instance.GetInstanceID()))
            {
                if (scrWorldSaveDataContainer.instance.miscFlags.Contains(flag))
                    Object.Destroy(__instance.gameObject);
                return;
            }
            if (ArchipelagoData.slotData == null) return;
            if (ArchipelagoData.Options.Bonesanity == ArchipelagoOptions.InsanityLevel.Vanilla) return;
            _bonesanityOn = true;
            GameObjectChecker.LoggedInstances.Add(__instance.GetInstanceID());
            __instance.StartCoroutine(PlaceModelsAfterLoading(__instance));
        }

        private static IEnumerator PlaceModelsAfterLoading(scrBone __instance)
        {
            yield return new WaitUntil(() => GameObjectChecker.PreviousScene != SceneManager.GetActiveScene().name);
            var flag = __instance.name;
            int scoutID;
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    ID = __instance.name switch
                    {
                        "Bone" => 1,
                        "Bone (1)" => 2,
                        "Bone (2)" => 3,
                        "Bone (3)" => 4,
                        "Bone (4)" => 5,
                        _ => ID
                    };
                    scoutID = 2200 + ID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
                case "Trash Kingdom":
                {
                    ID = __instance.name switch
                    {
                        "Bone" => 1,
                        "Bone (1)" => 2,
                        "Bone (2)" => 3,
                        "Bone (3)" => 4,
                        "Bone (4)" => 5,
                        _ => ID
                    };
                    scoutID = 2205 + ID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    ID = __instance.name switch
                    {
                        "Bone" => 1,
                        "Bone (1)" => 2,
                        "Bone (2)" => 3,
                        "Bone (3)" => 4,
                        "Bone (4)" => 5,
                        _ => ID
                    };
                    scoutID = 2210 + ID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
                case "Public Pool":
                {
                    ID = __instance.name switch
                    {
                        "Bone" => 1,
                        "Bone (1)" => 2,
                        "Bone (2)" => 3,
                        "Bone (3)" => 4,
                        "Bone (4)" => 5,
                        _ => ID
                    };
                    scoutID = 2215 + ID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
                case "The Bathhouse":
                {
                    ID = __instance.name switch
                    {
                        "Bone" => 1,
                        "Bone (1)" => 2,
                        "Bone (2)" => 3,
                        "Bone (3)" => 4,
                        "Bone (4)" => 5,
                        _ => ID
                    };
                    scoutID = 2220 + ID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
                case "Tadpole inc":
                {
                    ID = __instance.name switch
                    {
                        "Bone" => 1,
                        "Bone (1)" => 2,
                        "Bone (2)" => 3,
                        "Bone (3)" => 4,
                        "Bone (4)" => 5,
                        _ => ID
                    };
                    scoutID = 2225 + ID;
                    PlaceModelHelper.PlaceModel(scoutID, 0, __instance, true);
                    break;
                }
            }
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            Object.Destroy(ogQuads.gameObject);
            GameObjectChecker.LogBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"ID: {ID}, Flag: {flag}")
                .AppendLine($"Model: {__instance.quads.name}");
        }
        
        private static bool IsTransitioning(bool levelIntroduction = true)
        {
            return scrTrainManager.instance.isLoadingNewScene
                   || scrTransitionManager.instance.state != scrTransitionManager.States.idle
                   || !ArchipelagoClient.IsValidScene()
                   || (scrLevelIntroduction.isOn && levelIntroduction);
        }
    }

    [HarmonyPatch(typeof(scrBone), "Update")]
    public static class BonesanityCollectPatch
    {
        private static bool Prefix(scrBone __instance)
        {
            if (!_bonesanityOn) return true;
            if (__instance.trigger.foundPlayer() &&
                !scrWorldSaveDataContainer.instance.miscFlags.Contains(__instance.name))
            {
                scrWorldSaveDataContainer.instance.miscFlags.Add(__instance.name);
                Object.Instantiate<GameObject>(__instance.effect).transform.position = __instance.transform.position;
                if (__instance.gameObject != null && __instance.gameObject)
                    Object.Destroy((Object) __instance.gameObject);
            }
            // if (__instance.quads != null && __instance.quads)
            //     __instance.quads.transform.localEulerAngles = new Vector3(0.0f, 0.0f, Mathf.Sin(Time.time * 5f) * 10f);
            // __instance.transform.eulerAngles += new Vector3(0.0f, Time.deltaTime * 90f, 0.0f);
            return false;
        }
    }
    
    [HarmonyPatch(typeof(scrBoneQuest), "Update")]
    public static class BonesanityRewardPatch
    {
        private static bool Prefix(scrBoneQuest __instance)
        {
            if (ArchipelagoData.slotData == null) return true;
            if (ArchipelagoData.Options.Bonesanity == ArchipelagoOptions.InsanityLevel.Vanilla) return true;
            bool isBonesanity = ArchipelagoData.Options.Bonesanity == ArchipelagoOptions.InsanityLevel.Insanity;
            bool gotAllBones = scrWorldSaveDataContainer.instance.miscFlags.Count(x => x.StartsWith("Bone")) >= 5;
            var currentBoxField = AccessTools.Field(typeof(scrTextbox), "currentBox");
            int currentBox = (int)currentBoxField.GetValue(scrTextbox.instance);
            if (__instance.areaTrigger.foundPlayer())
            {
                _isInArea = true;
                var totalBones = 5;
                __instance.UIhider.visible = true;
                __instance.UItext.text = $"{scrWorldSaveDataContainer.instance.miscFlags.Count(x => x.StartsWith("Bone")).ToString()} / {totalBones.ToString()}";
            }
            else
            {
                _isInArea = false;
                __instance.UIhider.visible = false;
            }
            
            if (!scrWorldSaveDataContainer.instance.coinFlags.Contains("arcadeBone"))
            {
                if (scrTextbox.instance.isOn && scrTextbox.instance.conversation == "dogeBoneQuest")
                {
                    if (currentBox == 0 && !_answerFix)
                    {
                        var item = ArchipelagoClient.ScoutLocation(_currentScoutID);
                        var itemName = Assets.GetItemName(item);
                        var playerName = item.Player.Name;
                        if (!HasEnough() && isBonesanity)
                        {
                            scrTextbox.instance.conversationLocalized[0] =
                                $"Gimme {5-_currentBoneCount} more {_currentLevelName} B O N E S and I will show you {playerName}'s '{itemName}'! ({GetClassification(item)})";
                        }
                        else if (!gotAllBones)
                        {
                            scrTextbox.instance.conversationLocalized[0] =
                                $"Find all B O N E S and I will show you {playerName}'s '{itemName}'! ({GetClassification(item)}) ##fx0;*Happy dog sounds*";
                        }

                        _answerFix = true;
                    }
                }
                else
                {
                    _answerFix = false;
                }
            }
            
            if ((gotAllBones && !isBonesanity)|| HasEnough() || scrWorldSaveDataContainer.instance.coinFlags.Contains("arcadeBone"))
            {
                if (scrTextbox.instance.isOn && scrTextbox.instance.conversation == "dogeBonePost")
                {
                    if (currentBox == 0 && !_answerFix2)
                    {
                        HasEnough();
                        var item = ArchipelagoClient.ScoutLocation(_currentScoutID);
                        var itemName = Assets.GetItemName(item);
                        var playerName = item.Player.Name;
                        scrTextbox.instance.conversationLocalized[0] =
                            $"Tada! Here is {playerName}'s '{itemName}'! ({GetClassification(item)}) ##fx0;";
                        _answerFix2 = true;
                    }
                }
                else
                {
                    _answerFix2 = false;
                }
            }

            if (!_playedSound && _isInArea && !scrWorldSaveDataContainer.instance.coinFlags.Contains("arcadeBone") && gotAllBones)
            {
                _playedSound = true;
                GameObject gameObject = Object.Instantiate<GameObject>(__instance.audioOneShot);
                gameObject.transform.position = __instance.transform.position;
                gameObject.GetComponent<scrAudioOneShot>().setup(__instance.sndBoneGotAll, 0.7f, 1f);
                Object.Instantiate<GameObject>(__instance.smoke).transform.position = __instance.coin.transform.position;
            }

            if (__instance.coin != null && __instance.coin && (gotAllBones || HasEnough() && isBonesanity))
            {
                __instance.coin.SetActive(true);
                __instance.NPCQuest.SetActive(false);
                __instance.NPCPost.SetActive(true);
            }
            if (_isInArea && scrWorldSaveDataContainer.instance.coinFlags.Contains("arcadeBone"))
                __instance.UIhider.visible = false;
            if (scrWorldSaveDataContainer.instance.miscFlags.Count(x => x.StartsWith("Bone")) < 5)
            {
                if (!_isInArea)
                    __instance.UIhider.visible = false;
            }
            return false;
        }

        private static bool HasEnough()
        {
            var level = SceneManager.GetActiveScene().name;
            int amountOfBones = 0;
            switch (level)
            {
                case "Hairball City":
                    amountOfBones = ItemHandler.HairballBoneAmount;
                    _currentLevelName = "Hairball City";
                    _currentScoutID = 14;
                    break;
                case "Trash Kingdom":
                    amountOfBones = ItemHandler.TurbineBoneAmount;
                    _currentLevelName = "Turbine Town";
                    _currentScoutID = 25;
                    break;
                case "Salmon Creek Forest":
                    amountOfBones = ItemHandler.SalmonBoneAmount;
                    _currentLevelName = "Salmon Creek Forest";
                    _currentScoutID = 32;
                    break;
                case "Public Pool":
                    amountOfBones = ItemHandler.PoolBoneAmount;
                    _currentLevelName = "Public Pool";
                    _currentScoutID = 46;
                    break;
                case "The Bathhouse":
                    amountOfBones = ItemHandler.BathBoneAmount;
                    _currentLevelName = "Bathhouse";
                    _currentScoutID = 66;
                    break;
                case "Tadpole inc":
                    amountOfBones = ItemHandler.TadpoleBoneAmount;
                    _currentLevelName = "Tadpole HQ";
                    _currentScoutID = 77;
                    break;
                default:
                    return false;
            }
            _currentBoneCount = amountOfBones;
            return amountOfBones >= 5;
        }

        private const string RandomSymbols = "#@+*$3%&!";
        private static string GetClassification(ScoutedItemInfo scoutInfo)
        {
            var flags = scoutInfo.Flags;
            var classification = "";
            if (flags.HasFlag(ItemFlags.Advancement))
                classification = "Important";
            else if (flags.HasFlag(ItemFlags.NeverExclude))
                classification = "Useful";
            else if (flags.HasFlag(ItemFlags.Trap))
            {
                var length = Random.Range(8, 11);
                var builder = new StringBuilder(length);
                for (int i = 0; i < length; i++)
                {
                    var index = Random.Range(0, RandomSymbols.Length);
                    builder.Append(RandomSymbols[index]);
                }
                classification = builder.ToString();
            }
            else if (flags.HasFlag(ItemFlags.None))
                classification = "Useless";
            return classification;
        }
    }

    [HarmonyPatch(typeof(scrArcadeManager), "Update")]
    public static class BlippyStayWithUs
    {
        private static bool Prefix(scrArcadeManager __instance)
        {
            var trans = GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<scrTransitionManager>();
            var completed = (bool)AccessTools.Field(typeof(scrArcadeManager), "completed").GetValue(__instance);
            var hasRemoved = (bool)AccessTools.Field(typeof(scrArcadeManager), "hasRemoved").GetValue(__instance);
            if (__instance.insideTrigger.foundPlayer())
            {
                __instance.SecretLevel.SetActive(true);
            }
            else
            {
                int state = (int) trans.state;
            }

            if (!completed)
            {
                foreach (var t in __instance.objectsToTurnOn)
                    t.SetActive(false);
            }

            if (completed)
            {
                for (int index = 0; index < __instance.objectsToTurnOff.Count; ++index)
                {
                    if (index is 2 or 1)
                    {
                        __instance.objectsToTurnOff[index].SetActive(true);
                        continue;
                    }
                        
                    __instance.objectsToTurnOff[index].SetActive(false);
                }
            }
            if (!completed && (Object) __instance.coin == (Object) null && MyCharacterController.instance.state != MyCharacterController.States.Occupied)
            {
                if (__instance.insideTrigger.foundPlayer())
                {
                    __instance.teleporter.Teleport(true);
                    scrAudioMixerMaster.instance.ChangeState(scrAudioMixerMaster.States.normal);
                }
                AccessTools.Field(typeof(scrArcadeManager), "completed").SetValue(__instance, true);
            }
            if (!completed || hasRemoved || __instance.teleporter.isTeleporting)
                return false;
            __instance.Invoke("Complete", 0f);
            return false;
        }
    }
}