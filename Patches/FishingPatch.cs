using System.Collections.Generic;
using System.Reflection;
using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace NikoArchipelago.Patches;

public class FishingPatch
{
    private static string level;
    private static bool blockedLog;
    private static string textureType = "_Texture";
    private static bool stateStart = true;
    private static MyCharacterController characterController;
    private static PlayerCamera camera;
    private static scrTextbox textbox;
    private static scrTextboxTrigger textboxTrigger;
    private static scrWorldSaveDataContainer worldData;
    private static Vector3 characterShadowHomePos;
    private static float hoptimer;
    private static FieldInfo firstTimeTalkedBoolean;
    public static readonly Dictionary<string, string> FischerConversation = new();
    private static bool answerFix1, answerFix2;
    private static string _classification, _itemName, _playerName;
    private static int _currentScoutID;
    
    [HarmonyPatch(typeof(scrFishingMaster), "Start")]
    public static class FishingStartPatch
    {
        private static void Postfix(scrFishingMaster __instance)
        {
          FischerReady();
          var targetType = __instance.GetType();
          firstTimeTalkedBoolean = targetType.GetField("firstTimeTalked", BindingFlags.NonPublic | BindingFlags.Instance);
          if (firstTimeTalkedBoolean != null)
          {
              firstTimeTalkedBoolean.SetValue(__instance, true);
          }
          level = SceneManager.GetActiveScene().name;
          characterController = GameObject.FindGameObjectWithTag("CharacterController").GetComponent<MyCharacterController>();
          camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerCamera>();
          textbox = GameObject.FindGameObjectWithTag("Textbox").GetComponent<scrTextbox>();
          textboxTrigger = __instance.fisherTextbox.GetComponent<scrTextboxTrigger>();
          worldData = GameObject.FindGameObjectWithTag("WorldMaster").GetComponent<scrWorldSaveDataContainer>();
          characterShadowHomePos = __instance.characterShadow.transform.localPosition;
          FischerConversation.Clear();
          FischerConversation.
            Add("FishsanityFinal", $"##nameFischer;##talkerimg1;##nikoimg9;You caught all fish! ##newbox;##nikoimg6;As promised you will get (Item) for (Player)!");
          FischerConversation.
            Add("FishsanityNotEnough", $"##nameFischer;##talkerimg2;##nikoimg4;You need 5 (level name + fish) to get my reward##newbox;##nikoimg7;My reward is (item) for (Player) I heard it's (Classification)");
          FischerConversation.
            Add("FischerLocationIdle", $"##nameFischer;##talkerimg1;##nikoimg3;Catch all 5 fish to get my reward##newbox;##nikoimg7;My reward is (item) for (Player) I heard it's (Classification)");
          FischerConversation.
            Add("FishsanityObtained", $"##nameFischer;##talkerimg1;##nikoimg3;You already obtained my (Item) for (Player)!");
          FischerConversation.
            Add("FishsanityFishing", $"##nameFischer;##talkerimg1;##nikoimg3;You found all 5 fish!\nNow you need to catch the remaining Fish!");
          FischerConversation.
            Add("FishsanityNoSwimming", $"##nameFischer;##talkerimg1;##nikoimg1;Can you help me fishing? ##newbox;Wait##nikoimg5;, you can't swim?! Then you should learn how to swim, I think (Player) could help you.##nikoimg4;");
        }
    }
    private static void FischerReady()
    {
        if (!scrWorldSaveDataContainer.instance.miscFlags.Contains("Fischer talked"))
            scrWorldSaveDataContainer.instance.miscFlags.Add("Fischer talked");
    }

    [HarmonyPatch(typeof(scrFishingMaster), "Update")]
    public static class FishingUpdatePatch
    {
        private static int _currentFishCount;
        private static string _currentLevelName;
        private static bool _canSwim = false;

        [HarmonyPrefix]
        private static bool Prefix(scrFishingMaster __instance)
        {
            if (ArchipelagoData.slotData == null) return true;
            var pullupAnimationStartPoint = (Vector3)AccessTools.Field(typeof(scrFishingMaster), "pullupAnimationStartPoint").GetValue(__instance);
            var currentBoxField = AccessTools.Field(typeof(scrTextbox), "currentBox");
            int currentBox = (int)currentBoxField.GetValue(scrTextbox.instance);
            bool isFishsanity = ArchipelagoData.Options.Fishsanity == ArchipelagoOptions.InsanityLevel.Insanity;
            bool allFishCollected = CheckAllFish(level);
            __instance.pullupAnimationDuration = 4f;

            if (__instance.finalConversation && !__instance.gotCoin)
              textboxTrigger.ChangeIcon(scrTextboxTrigger.IconType.reward, true);
            else if ((textboxTrigger.conversation == __instance.idleConverstation 
                      || textboxTrigger.conversation == "FishsanityNotEnough" || textboxTrigger.conversation == "FishsanityFishing" 
                      || textboxTrigger.conversation == "FischerLocationIdle") &&
                     __instance.state == scrFishingMaster.States.Inactive)
              textboxTrigger.ChangeIcon(scrTextboxTrigger.IconType.quest, true);
            else
              textboxTrigger.ChangeIcon(scrTextboxTrigger.IconType.talk, false);
            if ((bool)firstTimeTalkedBoolean.GetValue(__instance) && (!textbox.isOn || textbox.isDying) &&
                __instance.startTrigger.foundPlayer() && __instance.state == scrFishingMaster.States.Inactive &&
                (textboxTrigger.conversation == __instance.idleConverstation ||
                 textboxTrigger.conversation == __instance.noCatchConversation ||
                 textboxTrigger.conversation == __instance.oldCatchConversation ||
                 textboxTrigger.conversation == "FishsanityObtained" ||
                 textboxTrigger.conversation == "FishsanityNotEnough" ||
                 textboxTrigger.conversation == "FishsanityFinal" ||
                 textboxTrigger.conversation == "FishsanityFishing" ||
                 textboxTrigger.conversation == "FischerLocationIdle"))
            {
              if (__instance.state != scrFishingMaster.States.Fishing)
                stateStart = true;
              __instance.state = scrFishingMaster.States.Fishing;
            }

            if (!textboxTrigger.isNewConversation)
              firstTimeTalkedBoolean.SetValue(__instance, true);
            if (textbox.isOn & !textbox.isDying)
            {
              if (__instance.state == scrFishingMaster.States.Fishing)
              {
                var gameObject = UnityEngine.Object.Instantiate<GameObject>(__instance.audioOneShot);
                gameObject.transform.position = MyCharacterController.position;
                gameObject.GetComponent<scrAudioOneShot>().setup(__instance.audioClipsEnd, 0.4f, Random.Range(0.5f, 1.5f));
                __instance.animator.SetTexture(__instance.txrFisherIdle);
                __instance.animatorArray.SetAnimationStrip(__instance.txrFisherIdle.name);
                __instance.StartCoroutine("HideVignette");
              }

              __instance.state = scrFishingMaster.States.Inactive;
            }

            if (!__instance.areaTrigger.foundPlayer() && characterController.Motor.GroundingStatus.IsStableOnGround)
            {
              if (__instance.state == scrFishingMaster.States.Fishing)
              {
                var gameObject = UnityEngine.Object.Instantiate<GameObject>(__instance.audioOneShot);
                gameObject.transform.position = MyCharacterController.position;
                gameObject.GetComponent<scrAudioOneShot>().setup(__instance.audioClipsEnd, 0.4f, Random.Range(0.5f, 1.5f));
                __instance.animator.SetTexture(__instance.txrFisherIdle);
                __instance.animatorArray.SetAnimationStrip(__instance.txrFisherIdle.name);
                __instance.StartCoroutine("HideVignette");
              }

              __instance.state = scrFishingMaster.States.Inactive;
            }

            if (__instance.state == scrFishingMaster.States.Inactive && stateStart)
            {
              __instance.characterShadow.transform.localPosition = characterShadowHomePos;
              stateStart = false;
            }

            if (__instance.state == scrFishingMaster.States.Fishing)
            {
              if (stateStart)
              {
                var gameObject = UnityEngine.Object.Instantiate<GameObject>(__instance.audioOneShot);
                gameObject.transform.position = MyCharacterController.position;
                gameObject.GetComponent<scrAudioOneShot>().setup(__instance.audioClipsStart, 0.4f, Random.Range(0.5f, 1.5f));
                stateStart = false;
                __instance.HUD.Show(4f);
                __instance.StartCoroutine("ShowVignette");
                __instance.fisherNewFish.GetComponent<Renderer>().sharedMaterial.SetTexture(textureType, __instance.txrEmpty);
              }

              if ((UnityEngine.Object)__instance.animator.GetTexture() != (UnityEngine.Object)__instance.txrFisherFishing)
                __instance.animatorArray.SetAnimationStrip(__instance.txrFisherFishing.name);
              if ((UnityEngine.Object)__instance.animator.GetTexture() != (UnityEngine.Object)__instance.txrFisherFishing)
                __instance.animatorArray.SetAnimationStrip(__instance.txrFisherFishing.name);
              if (characterController.state == MyCharacterController.States.Swimming)
              {
                __instance.currentFish = -1;
                for (var index = 0; index < __instance.fishLocations.Count; ++index)
                  if (__instance.fishLocations[index].foundPlayer())
                    __instance.currentFish = index;
                __instance.state = scrFishingMaster.States.Pullup;
                stateStart = true;
              }
            }

            if (__instance.state == scrFishingMaster.States.Fishing || __instance.state == scrFishingMaster.States.Pullup)
            {
              __instance.line.enabled = true;
              __instance.line.SetPosition(0, __instance.lineStart.position);
              __instance.line.SetPosition(1, MyCharacterController.position + new Vector3(0.0f, 0.5f, 0.0f));
              if (__instance.state == scrFishingMaster.States.Pullup)
                __instance.line.SetPosition(1, __instance.hook.transform.position);
            }
            else
            {
              __instance.line.enabled = false;
            }

            if (__instance.state == scrFishingMaster.States.Pullup)
            {
              var flag = true;
              if (stateStart)
              {
                __instance.Invoke("checkIfLastFish", 0f);
                pullupAnimationStartPoint = MyCharacterController.position;
                __instance.pullupAnimationTimer = 0.0f;
                __instance.animator.SetTexture(__instance.txrFisherPulling);
                __instance.animatorArray.SetAnimationStrip(__instance.txrFisherPulling.name);
                __instance.StartCoroutine("HideVignette");
                var gameObject = UnityEngine.Object.Instantiate<GameObject>(__instance.audioOneShot);
                gameObject.transform.position = MyCharacterController.position;
                gameObject.GetComponent<scrAudioOneShot>().setup(__instance.audioClipsPullUp, 1f, 1f);
                characterController.blockMovementInput = true;
                characterController.state = MyCharacterController.States.Normal;
                characterController.requestNewVelocity(new Vector3(0.0f, 0.0f, 0.0f));
                characterController.Motor.ForceUnground();
                characterController.playerAnimationManager.meshRenderer.enabled = false;
                __instance.hook.SetActive(true);
                __instance.hook.transform.position = pullupAnimationStartPoint;
                camera._targetVerticalAngle = 0.0f;
                camera.RequestSmooth();
                camera.SlerpEverything(0.5f);
                if (__instance.currentFish >= 0)
                {
                  __instance.hookFish.GetComponent<scrAnimateTextureArray>()
                    .SetAnimationStripAndName(__instance.fishTextures[__instance.currentFish].name);
                }
                else
                {
                  __instance.currentNonFish = Random.Range(0, __instance.noCatchTextures.Count - 1);
                  __instance.hookFish.GetComponent<scrAnimateTextureArray>()
                    .SetAnimationStripAndName(__instance.noCatchTextures[__instance.currentNonFish].name);
                }

                stateStart = false;
              }

              __instance.pullupAnimationTimer += Time.deltaTime / __instance.pullupAnimationDuration;
              __instance.hook.transform.position = new Vector3(
                pullupAnimationStartPoint.x +
                (__instance.pullupAnimationEndPoint.transform.position.x - pullupAnimationStartPoint.x) *
                __instance.pullupAnimationLateral.Evaluate(__instance.pullupAnimationTimer),
                pullupAnimationStartPoint.y +
                (__instance.pullupAnimationEndPoint.transform.position.y - pullupAnimationStartPoint.y) *
                __instance.pullupAnimationVertical.Evaluate(__instance.pullupAnimationTimer),
                pullupAnimationStartPoint.z +
                (__instance.pullupAnimationEndPoint.transform.position.z - pullupAnimationStartPoint.z) *
                __instance.pullupAnimationLateral.Evaluate(__instance.pullupAnimationTimer));
              __instance.hook.transform.LookAt(camera.transform);
              characterController.transform.position = __instance.pullupAnimationEndPoint.transform.position;
              characterController.requestNewPosition(__instance.pullupAnimationEndPoint.transform.position);
              characterController.blockMovement = true;
              __instance.characterShadow.transform.position = __instance.hook.transform.position;
              if ((double)__instance.pullupAnimationTimer >= 0)
              {
                __instance.characterShadow.transform.localPosition = characterShadowHomePos;
                characterController.blockMovement = false;
                characterController.requestNewVelocity(new Vector3(0.0f, 1f, 0.0f));
                characterController.playerAnimationManager.DoFlip(1f, 4f, false);
                if (__instance.currentFish >= 0)
                {
                  if (!textbox.isOn)
                    __instance.animatorArray.SetAnimationStrip(__instance.txrFisherGotFish.name);
                  __instance.fisherNewFish.SetActive(true);
                  __instance.fisherNewFish.GetComponent<Renderer>().sharedMaterial
                    .SetTexture(textureType, __instance.fishTextures[__instance.currentFish]);
                  textboxTrigger.conversation = __instance.oldCatchConversation;
                  textboxTrigger.isNewConversation = true;
                  for (var index = 0; index < worldData.fishFlags.Count; ++index)
                    if (worldData.fishFlags[index] == "fish" + __instance.currentFish.ToString())
                      flag = false;
                  if (flag)
                  {
                    worldData.fishFlags.Add("fish" + __instance.currentFish.ToString());
                    worldData.SaveWorld();
                    textboxTrigger.conversation = "fish" + __instance.currentFish.ToString();
                    __instance.seaFish[__instance.currentFish].JumpFreely = false;
                    __instance.HUD.Show(4f);
                    var gameObject = UnityEngine.Object.Instantiate<GameObject>(__instance.audioOneShot);
                    gameObject.transform.position = MyCharacterController.position;
                    gameObject.GetComponent<scrAudioOneShot>().setup(__instance.audioClipsNewFish, 1f, 1f);
                    __instance.animator.SetTexture(__instance.txrFisherGotFish);
                    __instance.animatorArray.SetAnimationStrip(__instance.txrFisherGotFish.name);
                    textboxTrigger.Initiate();
                  }
                }
                else
                {
                  if (!textbox.isOn)
                    __instance.animatorArray.SetAnimationStrip(__instance.txrFisherGotWeird.name);
                  textboxTrigger.conversation = __instance.noCatchConversation;
                  textboxTrigger.isNewConversation = true;
                  __instance.fisherNewFish.SetActive(true);
                  __instance.fisherNewFish.GetComponent<Renderer>().sharedMaterial
                    .SetTexture(textureType, __instance.noCatchTextures[__instance.currentNonFish]);
                }

                if (!textbox.isOn || textbox.isDying)
                  characterController.blockMovementInput = false;
                characterController.state = MyCharacterController.States.Normal;
                characterController.Motor.ForceUnground();
                characterController.playerAnimationManager.meshRenderer.enabled = true;
                __instance.hook.SetActive(false);
                __instance.Invoke("checkIfLastFish", 0f);
                __instance.state = scrFishingMaster.States.Inactive;
                stateStart = true;
              }
            }
            
            if (ArchipelagoData.Options.SwimCourse)
            {
              if (!ArchipelagoClient.SwimmingAcquired)
              {
                textboxTrigger.conversation = "FishsanityNoSwimming";
                __instance.state = scrFishingMaster.States.Inactive;
                stateStart = false;
              }
              else
                _canSwim = true;
            }
            else
              _canSwim = true;

            if (!textboxTrigger.isNewConversation && !textbox.isOn && _canSwim)
            {
              __instance.animator.SetTexture(__instance.txrFisherIdle);
              __instance.animatorArray.SetAnimationStrip(__instance.txrFisherIdle.name);
              __instance.fisherNewFish.SetActive(false);
              if (isFishsanity)
              {
                textboxTrigger.conversation = "FishsanityFishing";
                if (!allFishCollected)
                  textboxTrigger.conversation = "FishsanityNotEnough";
              }
              else
              {
                textboxTrigger.conversation = "FischerLocationIdle";
              }
              textboxTrigger.isNewConversation = true;
              textboxTrigger.conversationStarted = false;
              __instance.gotCoin = false;
            }

            if (!textbox.isOn)
            {
              answerFix1 = false;
              answerFix2 = false;
            }
            
            if (textbox.isOn && textbox.conversation == "FishsanityNotEnough")
            {
              textbox.canWaklaway = true;
              if (currentBox == 0 && !answerFix1)
              {
                textbox.conversationLocalized[0] = $"You need 5 {_currentLevelName} fish to get my reward.\nYou currently found {_currentFishCount}/5!";
                answerFix1 = true;
              }

              if (currentBox == 1 && !answerFix2)
              {
                var scout = ArchipelagoClient.ScoutLocation(_currentScoutID);
                var playerName = scout.Player.Name;
                textbox.conversationLocalized[1] = $"My reward is '{Assets.GetItemName(scout)}' for {playerName} I heard it's {Assets.GetClassification(scout)}!";
                answerFix2 = true;
              }
            }
            
            if (textbox.isOn && textbox.conversation == "FischerLocationIdle")
            {
              textbox.canWaklaway = true;
              if (currentBox == 1 && !answerFix2)
              {
                var scout = ArchipelagoClient.ScoutLocation(_currentScoutID);
                var playerName = scout.Player.Name;
                textbox.conversationLocalized[1] = $"My reward is '{Assets.GetItemName(scout)}' for {playerName} I heard it's {Assets.GetClassification(scout)}!";
                answerFix2 = true;
              }
            }
            
            if (textbox.isOn && textbox.conversation == "FishsanityFinal")
            {
              textbox.canWaklaway = true;
              if (currentBox == 0 && !answerFix1)
              {
                if (isFishsanity)
                  textbox.conversationLocalized[0] = $"You got all 5 {_currentLevelName} fish!";
                answerFix1 = true;
              }

              if (currentBox == 1 && !answerFix2)
              {
                var scout = ArchipelagoClient.ScoutLocation(_currentScoutID, false);
                var playerName = scout.Player.Name;
                textbox.conversationLocalized[1] = $"As promised you will get '{Assets.GetItemName(scout)}' for {playerName}!";
                answerFix2 = true;
              }
            }
            
            if (textbox.isOn && textbox.conversation == "FishsanityObtained")
            {
              textbox.canWaklaway = true;
              if (currentBox == 0 && !answerFix1)
              {
                var scout = ArchipelagoClient.ScoutLocation(_currentScoutID, false);
                var playerName = scout.Player.Name;
                textbox.conversationLocalized[0] = $"You already obtained my '{Assets.GetItemName(scout)}' for {playerName}!";
                answerFix1 = true;
              }
            }
            
            if (textbox.isOn && textbox.conversation == "FishsanityNoSwimming")
            {
              textbox.canWaklaway = true;
              if (currentBox == 1 && !answerFix1)
              {
                // var scout = ArchipelagoClient.ScoutLocation(_currentScoutID, false);
                // var playerName = scout.Player.Name;
                // var game = scout.Player.Game; // Sadly there is no Item Scouting for other slots, so can't hint for swim course.
                textbox.conversationLocalized[1] = $"Wait##nikoimg5;, you can't ##talkerimg2;##sp7;swim##sp0;?!##nikoimg4; Then you should learn how to swim, not sure where you can learn that.";
                answerFix1 = true;
              }
            }

            if (worldData.fishFlags.Count >= __instance.fishLocations.Count && !__instance.fisherNewFish.activeSelf &&
                !textbox.isOn && (allFishCollected || !isFishsanity))
            {
              if (!__instance.finalConversation)
              {
                __instance.fisherNewFish.SetActive(false);
                textboxTrigger.conversation = "FishsanityFinal";
                textboxTrigger.isNewConversation = true;
                textboxTrigger.conversationStarted = false;
                __instance.finalConversation = true;
              }

              for (var index = 0; index < worldData.coinFlags.Count; ++index)
                if (worldData.coinFlags[index] == "fishing")
                  __instance.gotCoin = true;
            }

            if (textbox.isOn && textbox.conversation == "FishsanityFinal" && textbox.isDying)
            {
              if (!__instance.gotCoin)
              {
                UnityEngine.Object.Instantiate<GameObject>(__instance.coinObtainer).GetComponent<scrObtainCoin>().myFlag =
                  "fishing";
                __instance.gotCoin = true;
              }
            }

            if (__instance.gotCoin && (allFishCollected || !isFishsanity))
            {
              textboxTrigger.conversation = "FishsanityObtained";
            }
              
            if (__instance.gotCoin && !textbox.isOn && __instance.state == scrFishingMaster.States.Inactive)
            {
              hoptimer += Time.deltaTime / 1.5f;
              if (hoptimer > 2.0)
              {
                hoptimer = 0.0f;
                __instance.hopper.Hop();
                __instance.animator.SetTexture(__instance.txrFisherDance);
                __instance.animator.SetFrameDuration(1.5f);
                __instance.animatorArray.SetAnimationStrip(__instance.txrFisherDance.name);
                __instance.animatorArray.SetFrameDuration(1.5f);
                if (__instance.animator.frame == 0)
                {
                  __instance.animator.SetFrameNumber(1);
                  __instance.animatorArray.FrameOffset = 1;
                }
                else
                {
                  __instance.animator.SetFrameNumber(0);
                  __instance.animatorArray.FrameOffset = 0;
                }
              }
            }

            __instance.animatorArray.transform.eulerAngles =
              new Vector3(0.0f, camera.transform.eulerAngles.y, 0.0f);
            
            return false;
        }

    private static bool CheckAllFish(string currentLevel)
        {
            int amountOfFish = 0;
            switch (currentLevel)
            {
                case "Hairball City":
                    amountOfFish = ItemHandler.HairballFishAmount;
                    _currentLevelName = "Hairball City";
                    _currentScoutID = 7;
                    break;
                case "Trash Kingdom":
                    amountOfFish = ItemHandler.TurbineFishAmount;
                    _currentLevelName = "Turbine Town";
                    _currentScoutID = 17;
                    break;
                case "Salmon Creek Forest":
                    amountOfFish = ItemHandler.SalmonFishAmount;
                    _currentLevelName = "Salmon Creek Forest";
                    _currentScoutID = 40;
                    break;
                case "Public Pool":
                    amountOfFish = ItemHandler.PoolFishAmount;
                    _currentLevelName = "Public Pool";
                    _currentScoutID = 48;
                    break;
                case "The Bathhouse":
                    amountOfFish = ItemHandler.BathFishAmount;
                    _currentLevelName = "Bathhouse";
                    _currentScoutID = 63;
                    break;
                case "Tadpole inc":
                    amountOfFish = ItemHandler.TadpoleFishAmount;
                    _currentLevelName = "Tadpole HQ";
                    _currentScoutID = 72;
                    break;
                default:
                    return false;
            }
            _currentFishCount = amountOfFish;
            return amountOfFish >= 5;
        }
    }
}