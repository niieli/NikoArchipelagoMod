using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NikoArchipelago;

public class TrapManager : MonoBehaviour
{
    public static TrapManager instance;
    public static bool FreezeOn, IronBootsOn, MyTurnOn, WhoopsOn, GravityOn, WideOn, JumpingJacksOn, HomeOn, PhoneOn, TinyOn;
    private readonly GameObject trapFreeze = Plugin.TrapFreeze; 
    private readonly GameObject trapIronBoots = Plugin.TrapIronBoots;
    private readonly GameObject trapMyTurn = Plugin.TrapMyTurn;
    private readonly GameObject trapWhoops = Plugin.TrapWhoops;
    private readonly GameObject trapGravity = Plugin.TrapGravity;
    private readonly GameObject trapHome = Plugin.TrapHome;
    private readonly GameObject trapWide = Plugin.TrapWide;
    private readonly GameObject trapJumpingJacks = Plugin.TrapJumpingJacks;
    private readonly GameObject trapPhoneCall = Plugin.TrapPhoneCall;
    private readonly GameObject trapTiny = Plugin.TrapTiny;
    private Transform trapListUI; 
    private Transform trapListFake;
    private GameObject trapUI;
    private GameObject trapFake;
    private static readonly int Timer = Animator.StringToHash("Timer");
    private readonly Dictionary<string, GameObject> activeTraps = new();
    public static readonly Dictionary<string, string> TrapConversations = new();

    private void Awake()
    {
        trapListUI = transform.Find("TrapPanel");
        trapListFake = transform.Find("VisualPanel");
        if (instance == null) instance = this;
        AddTrapConversations();
        Plugin.BepinLogger.LogInfo($"Loaded {TrapConversations.Count} trap conversations.");
    }

    public void Update()
    {
        if (FreezeOn)
        {
            FreezeOn = false;
            ActivateTrap("Freeze", 8.5f);
        }
        if (IronBootsOn)
        {
            IronBootsOn = false;
            ActivateTrap("Iron Boots", 30f);
        }
        if (MyTurnOn)
        {
            MyTurnOn = false;
            ActivateTrap("My Turn!", Random.Range(25f, 60f));
        }
        if (WhoopsOn)
        {
            WhoopsOn = false;
            ActivateTrap("Whoops!", 5f);
        }
        if (GravityOn)
        {
            GravityOn = false;
            ActivateTrap("Zero Gravity", 12.5f);
        }
        if (WideOn)
        {
            if (activeTraps.ContainsKey("Tiny")) return;
            WideOn = false;
            ActivateTrap("Wide", Random.Range(10f, 60f));
        }
        if (JumpingJacksOn)
        {
            JumpingJacksOn = false;
            ActivateTrap("Jumping Jacks", Random.Range(10f, 45f));
        }
        if (HomeOn)
        {
            HomeOn = false;
            ActivateTrap("Home", 7.5f);
        }
        if (PhoneOn)
        {
            PhoneOn = false;
            ActivateTrap("Phone", 10f);
        }
        if (TinyOn)
        {
            if (activeTraps.ContainsKey("Wide")) return;
            TinyOn = false;
            ActivateTrap("Tiny", Random.Range(10f, 60f));
        }
    }

    public void ActivateTrap(string trapName, float duration)
    {
        if (activeTraps.ContainsKey(trapName)) return;

        Plugin.BepinLogger.LogInfo("Received Trap: " + trapName);
        Plugin.BepinLogger.LogInfo($"Activating trap {trapName} for {duration} seconds.");
        switch (trapName)
        {
            case "Freeze":
                Plugin.BepinLogger.LogInfo("Instantiating Freeze Trap");
                trapUI = Instantiate(trapFreeze, trapListFake);
                StartCoroutine(Freeze(duration));
                trapUI.transform.Find("TrapBox/Background").gameObject.AddComponent<FallingSnowflakesBackground>();
                break;
            case "Iron Boots":
                Plugin.BepinLogger.LogInfo("Instantiating Iron Boots Trap");
                trapUI = Instantiate(trapIronBoots, trapListFake);
                StartCoroutine(IronBoots(duration));
                break;
            case "My Turn!":
                Plugin.BepinLogger.LogInfo("Instantiating MyTurnTrap");
                trapUI = Instantiate(trapMyTurn, trapListFake);
                StartCoroutine(MyTurn(duration));
                break;
            case "Whoops!":
                Plugin.BepinLogger.LogInfo("Instantiating WhoopsTrap");
                trapUI = Instantiate(trapWhoops, trapListFake);
                StartCoroutine(Whoops(duration));
                break;
            case "Zero Gravity":
                Plugin.BepinLogger.LogInfo("Instantiating GravityTrap");
                trapUI = Instantiate(trapGravity, trapListFake);
                StartCoroutine(ZeroGravity(duration));
                break;
            case "Wide":
                Plugin.BepinLogger.LogInfo("Instantiating WideTrap");
                trapUI = Instantiate(trapWide, trapListFake);
                StartCoroutine(Wide(duration));
                break;
            case "Jumping Jacks":
                Plugin.BepinLogger.LogInfo("Instantiating JumpingJacksTrap");
                trapUI = Instantiate(trapJumpingJacks, trapListFake);
                StartCoroutine(JumpingJacks(duration));
                break;
            case "Home":
                Plugin.BepinLogger.LogInfo("Instantiating HomeTrap");
                trapUI = Instantiate(trapHome, trapListFake);
                StartCoroutine(Home(duration));
                break;
            case "Phone":
                Plugin.BepinLogger.LogInfo("Instantiating PhoneCallTrap");
                trapUI = Instantiate(trapPhoneCall, trapListFake);
                StartCoroutine(PhoneCall(duration));
                break;
            case "Tiny":
                Plugin.BepinLogger.LogInfo("Instantiating TinyTrap");
                trapUI = Instantiate(trapTiny, trapListFake);
                StartCoroutine(Tiny(duration));
                break;
        }
        if (trapUI != null)
            trapFake = Instantiate(trapUI, trapListUI);
        if (trapUI.name != "Freeze")
            trapUI.transform.Find("TrapBox/Background").gameObject.AddComponent<ScrollingEffect>();
        var animator = trapUI.GetComponent<Animator>();
        animator.SetFloat(Timer, duration);
        StartCoroutine(UpdateTrapTimer(trapUI, trapFake, trapName, duration));

        activeTraps[trapName] = trapUI;
    }
    
    private IEnumerator UpdateTrapTimer(GameObject trap, GameObject fake, string trapName, float duration)
    {
        var rt = fake.GetComponent<RectTransform>();
        var timerTextShadow = trap.transform.Find("Timer/TextShadow").GetComponent<TextMeshProUGUI>();
        var timerText = trap.transform.Find("Timer/TextFront").GetComponent<TextMeshProUGUI>();

        if (trapName == "Phone")
        {
            var phoneDescription = RandomPhoneDescription();
            trap.transform.Find("TrapDescription/TextShadow").GetComponent<TextMeshProUGUI>().text = phoneDescription;
            trap.transform.Find("TrapDescription/TextFront").GetComponent<TextMeshProUGUI>().text = phoneDescription;
        }

        float timeRemaining = duration;
        while (timeRemaining > 0)
        {
            timerText.text = timeRemaining.ToString("F1") + "s";
            timerTextShadow.text = timeRemaining.ToString("F1") + "s";
            yield return null;
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);

            Vector3 center = (corners[0] + corners[2]) / 2;

            trap.transform.position = center;
            timeRemaining -= Time.deltaTime;
            trap.GetComponent<Animator>().SetFloat(Timer, timeRemaining);
            if (IsTransitioning() && trapName != "Home")
            {
                yield return new WaitUntil(() => !IsTransitioning());
            }
        }
        timerText.text = "0.0s";
        timerTextShadow.text = "0.0s";
        RemoveTrap(trapName, fake);
    }

    public void RemoveTrap(string trapName, GameObject fake)
    {
        if (!activeTraps.TryGetValue(trapName, out GameObject trap)) return;
        Destroy(fake);
        Destroy(trap);
        activeTraps.Remove(trapName);
    }
    
    private static bool IsTransitioning(bool levelIntroduction = true)
    {
        return scrTrainManager.instance.isLoadingNewScene
               || scrTransitionManager.instance.state != scrTransitionManager.States.idle
               || !ArchipelagoClient.IsValidScene()
               || (scrLevelIntroduction.isOn && levelIntroduction);
    }
    
    private static IEnumerator Whoops(float duration)
    {
        MyCharacterController.instance.requestNewPosition(new Vector3(0, 450, 0));
        Plugin.BepinLogger.LogInfo("Whoopsed Player");
        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
        }
    }
    
    private static IEnumerator Freeze(float duration)
    {
        MyCharacterController.instance.blockMovement = true;
        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
            if (IsTransitioning())
            {
                yield return new WaitUntil(() => !IsTransitioning());
            }
        }
        MyCharacterController.instance.blockMovement = false;
        Plugin.BepinLogger.LogInfo("Freeze Trap ended.");
    }

    private IEnumerator IronBoots(float duration)
    {
        var diveCancelHopSpeed = MyCharacterController.instance.DiveCancelHopSpeed;
        var diveSpeed = MyCharacterController.instance.DiveSpeed;
        var diveUpwardsSpeed = MyCharacterController.instance.DiveUpwardsSpeed;
        var maxAirMoveSpeed = MyCharacterController.instance.MaxAirMoveSpeed;
        var jumpSpeed = MyCharacterController.instance.JumpSpeed;
        var maxStableMoveSpeed = MyCharacterController.instance.MaxStableMoveSpeed;
        var wallJumpSpeed = MyCharacterController.instance.WallJumpSpeed;
        var wallUpwardsJumpSpeed = MyCharacterController.instance.WallUpwardsJumpSpeed;

        MyCharacterController.instance.DiveSpeed = 7.5f;
        MyCharacterController.instance.DiveUpwardsSpeed = 6f;
        MyCharacterController.instance.DiveCancelHopSpeed = 6f;
        MyCharacterController.instance.MaxAirMoveSpeed = 6f;
        MyCharacterController.instance.JumpSpeed = 8f;
        MyCharacterController.instance.MaxStableMoveSpeed = 6f;
        MyCharacterController.instance.WallJumpSpeed = 8.75f;
        MyCharacterController.instance.WallUpwardsJumpSpeed = 8.75f;

        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
            if (!IsTransitioning()) continue;
            yield return new WaitUntil(() => !IsTransitioning());
            MyCharacterController.instance.DiveSpeed = 7.5f;
            MyCharacterController.instance.DiveUpwardsSpeed = 6f;
            MyCharacterController.instance.DiveCancelHopSpeed = 6f;
            MyCharacterController.instance.MaxAirMoveSpeed = 6f;
            MyCharacterController.instance.JumpSpeed = 8f;
            MyCharacterController.instance.MaxStableMoveSpeed = 6f;
            MyCharacterController.instance.WallJumpSpeed = 8.75f;
            MyCharacterController.instance.WallUpwardsJumpSpeed = 8.75f;
        }

        MyCharacterController.instance.DiveSpeed = diveSpeed;
        MyCharacterController.instance.DiveUpwardsSpeed = diveUpwardsSpeed;
        MyCharacterController.instance.DiveCancelHopSpeed = diveCancelHopSpeed;
        MyCharacterController.instance.MaxAirMoveSpeed = maxAirMoveSpeed;
        MyCharacterController.instance.JumpSpeed = jumpSpeed;
        MyCharacterController.instance.MaxStableMoveSpeed = maxStableMoveSpeed;
        MyCharacterController.instance.WallJumpSpeed = wallJumpSpeed;
        MyCharacterController.instance.WallUpwardsJumpSpeed = wallUpwardsJumpSpeed;
        Plugin.BepinLogger.LogInfo("Iron Boots Trap ended.");
        if (!activeTraps.ContainsKey("Zero Gravity"))
        {
            MovementSpeed.MovementSpeedMultiplier();
        }
    }
    private static IEnumerator MyTurn(float duration)
    {
        var waitTime = 0f;
        while (duration > 0)
        {
            if (waitTime < 0)
            {
                float randomX = Random.Range(-35f, 35f);
                float randomZ = Random.Range(-35f, 35f);
                Vector3 randomVelocity = new Vector3(randomX, 0, randomZ);

                MyCharacterController.instance.requestAddVelocity(randomVelocity);
                Plugin.BepinLogger.LogInfo($"Applied Velocity: {randomVelocity}");
            
                var shouldJump = Random.value > 0.4f;
                if (shouldJump)
                {
                    var _jumpRequestedField = typeof(MyCharacterController).GetField("_jumpRequested", BindingFlags.NonPublic | BindingFlags.Instance);
                    _jumpRequestedField.SetValue(MyCharacterController.instance, true);
                    Plugin.BepinLogger.LogInfo("Player Jumped!");
                }
            
                var shouldDive = Random.value > 0.25f;
                if (shouldDive)
                {
                    MyCharacterController.instance._diveRequested = true;
                    Plugin.BepinLogger.LogInfo("Player Dived!");
                }
            
                waitTime = Random.Range(0f, 3.5f);
            }
            yield return null;
            duration -= Time.deltaTime;
            waitTime -= Time.deltaTime;
            if (IsTransitioning())
            {
                yield return new WaitUntil(() => !IsTransitioning(false));
            }
        }
        Plugin.BepinLogger.LogInfo("My Turn! Trap ended.");
    }
    
    private IEnumerator ZeroGravity(float duration)
    {
        var jump = MyCharacterController.instance.JumpSpeed;
        var airMove = MyCharacterController.instance.MaxAirMoveSpeed;
        var airAcc = MyCharacterController.instance.AirAccelerationSpeed;
        var gravityField = typeof(MyCharacterController).GetField("Gravity", BindingFlags.NonPublic | BindingFlags.Instance);
        gravityField.SetValue(MyCharacterController.instance, new Vector3(0f, 0.5f, 0f));
        MyCharacterController.instance.JumpSpeed = 0.15f;
        MyCharacterController.instance.MaxAirMoveSpeed = 1f;
        MyCharacterController.instance.AirAccelerationSpeed = 0.15f;
        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
            if (!IsTransitioning()) continue;
            yield return new WaitUntil(() => !IsTransitioning());
            MyCharacterController.instance.JumpSpeed = 0.15f;
            MyCharacterController.instance.MaxAirMoveSpeed = 1f;
            MyCharacterController.instance.AirAccelerationSpeed = 0.15f;
        }
        MyCharacterController.instance.JumpSpeed = jump;
        MyCharacterController.instance.AirAccelerationSpeed = airAcc;
        MyCharacterController.instance.MaxAirMoveSpeed = airMove;
        gravityField.SetValue(MyCharacterController.instance, new Vector3(0f, -30f, 0f));
        Plugin.BepinLogger.LogInfo("Zero Gravity Trap ended.");
        if (!activeTraps.ContainsKey("Iron Boots"))
        {
            MovementSpeed.MovementSpeedMultiplier();
        }
    }
    
    private static IEnumerator BadEyeSight(float duration)
    {
        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
            if (IsTransitioning())
            {
                yield return new WaitUntil(() => !IsTransitioning());
            }
        }
        Plugin.BepinLogger.LogInfo("Bad Eye Sight ended.");
    }
    
    private static IEnumerator Wide(float duration)
    {
        var otherAnim = MyCharacterController.instance.directionObject.transform;
        var squisher = MyCharacterController.instance.squisher.gameObject.GetComponent<Squisher>();
        squisher.SuperSquish();
        squisher.enabled = false;
        while (duration > 0)
        {
            if (IsTransitioning())
            {
                yield return new WaitUntil(() => !IsTransitioning(false));
                squisher = MyCharacterController.instance.squisher.gameObject.GetComponent<Squisher>();
                squisher.enabled = false;
                otherAnim = MyCharacterController.instance.directionObject.transform;
            }
            squisher.gameObject.transform.localScale = new Vector3(3.5f, 0.4f, 3.5f);
            otherAnim.localScale = new Vector3(3.5f, 0.4f, 1f);
            yield return null;
            duration -= Time.deltaTime;
        }
        squisher.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        otherAnim.localScale = new Vector3(1f, 1f, 1f);
        squisher.enabled = true;
        squisher.MinorSquish();
        Plugin.BepinLogger.LogInfo("Wide Trap ended.");
    }
    
    private static IEnumerator Home(float duration)
    {
        Plugin.BepinLogger.LogInfo("Send Player to Home");
        scrTrainManager.instance.UseTrain(1, false);
        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
        }
    }
    
    private static IEnumerator JumpingJacks(float duration)
    {
        while (duration > 0)
        {
            var _jumpRequestedField = typeof(MyCharacterController).GetField("_jumpRequested", BindingFlags.NonPublic | BindingFlags.Instance);
            _jumpRequestedField.SetValue(MyCharacterController.instance, true);
            yield return null;
            duration -= Time.deltaTime;
            if (IsTransitioning())
            {
                yield return new WaitUntil(() => !IsTransitioning(false));
            }
        }
        Plugin.BepinLogger.LogInfo("Jumping Jacks Trap ended.");
    }
    
    private static IEnumerator PhoneCall(float duration)
    {
        var msg = "trapConv"+Random.Range(0, TrapConversations.Count);
        Plugin.BepinLogger.LogInfo($"Phone Call Trap: {msg}");
        scrTextbox.instance.TurnOn(msg);
        if (scrTextbox.instance.conversationLocalized.Count >= 30)
        {
            var letterDurationField = typeof(scrTextbox).GetField("letterDuration", BindingFlags.NonPublic | BindingFlags.Instance);
            letterDurationField.SetValue(scrTextbox.instance, 0.005f);
            scrTextbox.instance.typeSpeedMod = 0f;
            Plugin.BepinLogger.LogInfo("Phone Call Trap: Long Conversation, slight text speed up!");
        }
        else
        {
            var letterDurationField = typeof(scrTextbox).GetField("letterDuration", BindingFlags.NonPublic | BindingFlags.Instance);
            letterDurationField.SetValue(scrTextbox.instance, 0.02f);
        }
        scrTextbox.instance.canWaklaway = false;
        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
            if (IsTransitioning())
            {
                yield return new WaitUntil(() => !IsTransitioning());
                scrTextbox.instance.TurnOn(msg);
            }
            var currentBoxField = AccessTools.Field(typeof(scrTextbox), "currentBox");
            int _currentBox = (int)currentBoxField.GetValue(scrTextbox.instance);
        }
        Plugin.BepinLogger.LogInfo("Phone Call Trap ended.");
    }

    private static string RandomPhoneDescription()
    {
        var descriptions = new[]
        {
            "Congrats! You've won a conversation you can't skip!",
            "Forced conversation! Better hear them out.",
            "Someone’s calling. Pick up and listen to them.",
            "You got a phone call, let's see who it is...",
            "Mysterious number is calling you..."
        };
        var randomIndex = Random.Range(0, descriptions.Length);
        return descriptions[randomIndex];
    }
    
    private static IEnumerator Tiny(float duration)
    {
        var otherAnim = MyCharacterController.instance.directionObject.transform;
        var squisher = MyCharacterController.instance.squisher.gameObject.GetComponent<Squisher>();
        squisher.SuperSquish();
        squisher.enabled = false;
        while (duration > 0)
        {
            if (IsTransitioning())
            {
                yield return new WaitUntil(() => !IsTransitioning(false));
                squisher = MyCharacterController.instance.squisher.gameObject.GetComponent<Squisher>();
                squisher.enabled = false;
                otherAnim = MyCharacterController.instance.directionObject.transform;
            }
            squisher.gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            otherAnim.localScale = new Vector3(0.2f, 0.2f, 1f);
            yield return null;
            duration -= Time.deltaTime;
        }
        squisher.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        otherAnim.localScale = new Vector3(1f, 1f, 1f);
        squisher.enabled = true;
        squisher.MinorSquish();
        Plugin.BepinLogger.LogInfo("Tiny Trap ended.");
    }
    
    private static IEnumerator FreezeCamera(float duration)
    {
        var pos = PlayerCamera.instance._currentFollowPosition;
        while (duration > 0)
        {
            PlayerCamera.instance._currentFollowPosition = pos;
            yield return null;
            duration -= Time.deltaTime;
            if (IsTransitioning())
            {
                yield return new WaitUntil(() => !IsTransitioning(false));
                pos = PlayerCamera.instance._currentFollowPosition;
            }
        }
        Plugin.BepinLogger.LogInfo("Jumping Jacks Trap ended.");
    }

    // For the time beings use this method
    private void AddTrapConversations()
    {
        TrapConversations.Add("trapConv0",
            "##nameSandwich Monarch;Have you ever wondered why they call it \"BK Mode\"? Well, it's a long story, and you're about to find out. ##newbox;So there was this person playing in a multiworld with their friends, but they ran out of things to do because they lacked progression items. ##newbox;So they thought, \"I'm actually kind of hungry. Why don't I get some food?\", and that's what they did. ##newbox;They took a break from the randomizer, went to their favorite fast food restaurant, and got themselves a delicious meal. ##newbox;Keep in mind, the randomizer was still going. The other people were still playing and getting stuff for the person that was getting food. ##newbox;So that person, the one getting food, got back. When they checked their game, they had plenty of new items. ##newbox;And do you want to know what the kicker of all this is? ##addinput:Not really;skip0; ##addinput:Of course!;skip1; ##newbox;Well too bad, because I'm telling you anyway! ##newbox;Not a single one of them was progression! They received so many items, but not a single one of them helped them get further in their game! ##newbox;So they were stuck there, eating their food, STILL waiting to receive an item that would help them get further in their game. ##newbox;And from then on, whenever you aren't able to do anything in a randomizer because you need to receive items from others, ##newbox;that is called \"BK Mode\".");
        TrapConversations.Add("trapConv1",
            "##nameDaft Punk;##nikoimg2;Around the world ##newbox;##nikoimg1;Around the world!!! ##newbox;##nikoimg2;Around the world ##newbox;##nikoimg1;Around the world!!! ##newbox;##nikoimg2;Around the world ##newbox;##nikoimg1;Around the world!!! ##newbox;##nikoimg2;Around the world ##newbox;##nikoimg1;Around the world!!! ##newbox;##nikoimg2;Around the world ##newbox;##nikoimg1;Around the world!!! ##newbox;##nikoimg2;Around the world ##newbox;##nikoimg1;Around the world!!! ##newbox;##nikoimg2;Around the world ##newbox;##nikoimg1;Around the world!!!");
        TrapConversations.Add("trapConv2",
            "##nameDesperate Trainer;I'm looking for a super important item, have you seen it? ##newbox;It's like some kind of parcel, and the guy who needs it has the same name as a tree. ##newbox;If you find it, please let me know ASAP. Thanks!");
        TrapConversations.Add("trapConv3",
            "##nameUnknown Caller;##nikoimg2;Why do they call it oven when you of in the cold food of out hot eat the food? ##addinput:Yes;skip0; ##addinput:No;skip0; ##addinput:What?;skip0; ##newbox;##nikoimg4;...Nevermind.");
    }
}