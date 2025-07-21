using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Patches;
using NikoArchipelago.Stuff;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace NikoArchipelago;

public class TrapManager : MonoBehaviour
{
    public static TrapManager instance;
    public static bool FreezeOn, IronBootsOn, MyTurnOn, WhoopsOn, GravityOn, WideOn, JumpingJacksOn, HomeOn, PhoneOn, TinyOn, FastTimeOn;
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
    private readonly GameObject trapFast = Plugin.TrapFast;
    private Transform trapListUI; 
    private Transform trapListFake;
    private GameObject trapUI;
    private GameObject trapFake;
    private CanvasGroup canvasGroup;
    public static bool IsUiOnScreen;
    private static readonly int Timer = Animator.StringToHash("Timer");
    private readonly Dictionary<string, GameObject> activeTraps = new();
    public static readonly Dictionary<string, string> TrapConversations = new();
    private static string lastPhoneCall;
    private static bool stillCalling;
    private static bool canSendHome;
    public static int DebugPhone = -1;
    public static bool DebugPhoneBool;
    
    // TrapLink
    public static readonly Queue<(string, string, DateTime)> TrapLinkQueue = new();
    public static readonly Dictionary<string, (string, float)> TrapLinkMapping = new()
    {
        { "Banana Trap", ("Whoops!", 5f) },
        { "Chaos Control Trap", ("Freeze", 5f) },
        { "Confuse Trap", ("My Turn!", 25f) },
        { "Confusion Trap", ("My Turn!", 25f) },
        { "Controller Drift Trap", ("My Turn!", 25f) },
        { "Cutscene Trap", ("Phone", 10f) },
        { "Eject Ability", ("Whoops!", 5f) },
        { "Exposition Trap", ("Phone", 10f) },
        { "Fast Trap", ("Fast", Random.Range(25f, 50f)) },
        { "Freeze Trap", ("Freeze", 5f) },
        { "Frozen Trap", ("Freeze", 5f) },
        { "Gravity Trap", ("Gravity", 7.5f) },
        { "Hiccup Trap", ("Jumping Jacks", 20f) },
        { "Honey Trap", ("Iron Boots", 15f) },
        { "Ice Trap", ("Freeze", 5f) },
        { "Instant Death Trap", ("Home", 5f) },
        { "Jump Trap", ("Jumping Jacks", 15f) },
        { "Literature Trap", ("Phone", 10f) },
        { "Paralyze Trap", ("Freeze", 5f) },
        { "Push Trap", ("My Turn!", 15f) },
        { "Slow Trap", ("Iron Boots", 15f) },
        { "Slowness Trap", ("Iron Boots", 15f) },
        { "Tiny Trap", ("Tiny", 25f) },
    };

    private void Awake()
    {
        trapListUI = transform.Find("TrapPanel");
        trapListFake = transform.Find("VisualPanel");
        canvasGroup = trapListFake.GetComponent<CanvasGroup>();
        if (instance == null) instance = this;
        AddTrapConversations();
        Plugin.BepinLogger.LogInfo($"Loaded {TrapConversations.Count} trap conversations.");
    }
    
    private static Dictionary<string, bool> GetEnabledTraps()
    {
        return new Dictionary<string, bool>
        {
            { "Freeze", ArchipelagoData.FreezeTrapEnabled },
            { "Iron Boots", ArchipelagoData.IronBootsTrapEnabled },
            { "Whoops!", ArchipelagoData.WhoopsTrapEnabled },
            { "My Turn!", ArchipelagoData.MyTurnTrapEnabled },
            { "Gravity", ArchipelagoData.GravityTrapEnabled },
            { "Home", ArchipelagoData.HomeTrapEnabled },
            { "W I D E", ArchipelagoData.WideTrapEnabled },
            { "Phone", ArchipelagoData.PhoneTrapEnabled },
            { "Tiny", ArchipelagoData.TinyTrapEnabled },
            { "Jumping Jacks", ArchipelagoData.JumpingJacksTrapEnabled },
        };
    }

    private void TrapLinkActivation()
    {
        if (TrapLinkQueue.Count == 0) return;
        var trap = TrapLinkQueue.Dequeue();
        if (trap.Item3 + TimeSpan.FromSeconds(5f) > DateTime.Now)
        {
            var nikoTrap = TrapLinkMapping[trap.Item1].Item1;
            var enabledTraps = GetEnabledTraps();
            enabledTraps.TryGetValue(nikoTrap, out var value);
            if (!value)
            {
                Plugin.BepinLogger.LogInfo($"TrapLink: {nikoTrap} is disabled | {ArchipelagoData.FreezeTrapEnabled}");
                return;
            }
            var timer = TrapLinkMapping[trap.Item1].Item2;
            ActivateTrap(nikoTrap, timer+0.5f, trap.Item2);
            Plugin.BepinLogger.LogInfo($"Activated {nikoTrap} via TrapLink");
        }
    }

    public void Update()
    {
        TrapLinkActivation();
        if (FreezeOn)
        {
            FreezeOn = false;
            ActivateTrap("Freeze", Random.Range(4f, 8f));
        }
        if (IronBootsOn)
        {
            IronBootsOn = false;
            ActivateTrap("Iron Boots", Random.Range(8f, 30f));
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
            ActivateTrap("Gravity", 12.5f);
        }
        if (WideOn)
        {
            if (activeTraps.ContainsKey("Tiny")) return;
            WideOn = false;
            ActivateTrap("W I D E", Random.Range(10f, 60f));
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

        if (FastTimeOn)
        {
            FastTimeOn = false;
            ActivateTrap("Fast", Random.Range(40f, 80f));
        }
        canvasGroup.alpha = IsUiOnScreen ? 0.35f : 1f;
    }

    public void ActivateTrap(string trapName, float duration, string source = null)
    {
        if (activeTraps.ContainsKey(trapName))
        { 
            //Plugin.BepinLogger.LogInfo($"{trapName} already active, so no extra.");
            return;
        }
        if (source == null && ArchipelagoClient.TrapLink)
            ArchipelagoClient.SendTrapLink(trapName+" Trap");

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
            case "Gravity":
                Plugin.BepinLogger.LogInfo("Instantiating GravityTrap");
                trapUI = Instantiate(trapGravity, trapListFake);
                StartCoroutine(ZeroGravity(duration));
                break;
            case "W I D E":
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
            case "Fast":
                Plugin.BepinLogger.LogInfo("Instantiating FastTrap");
                trapUI = Instantiate(trapFast, trapListFake);
                StartCoroutine(Fast(duration));
                break;
        }
        if (trapUI != null)
            trapFake = Instantiate(trapUI, trapListUI);
        if (trapUI.name != "Freeze")
            trapUI.transform.Find("TrapBox/Background").gameObject.AddComponent<ScrollingEffect>();
        var animator = trapUI.GetComponent<Animator>();
        animator.SetFloat(Timer, duration);
        StartCoroutine(UpdateTrapTimer(trapUI, trapFake, trapName, duration, source));

        activeTraps[trapName] = trapUI;
    }
    
    private IEnumerator UpdateTrapTimer(GameObject trap, GameObject fake, string trapName, float duration, string source = null)
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

        if (source != null)
        {
            if (trap.transform.Find("TrapDescription/TextShadow") != null)
                trap.transform.Find("TrapDescription/TextShadow").GetComponent<TextMeshProUGUI>().text = $"from {source}";
            if (trap.transform.Find("TrapDescription/TextFront") != null)
                trap.transform.Find("TrapDescription/TextFront").GetComponent<TextMeshProUGUI>().text = $"from {source}";
            trap.transform.Find("TrapLink").gameObject.SetActive(true);
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
            if (IsTransitioning() && trapName == "Home" && !canSendHome)
            {
                yield return new WaitUntil(() => canSendHome);
            }
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
               || (scrLevelIntroduction.isOn && levelIntroduction)
               || GameObjectChecker.IsHamsterball;
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
        yield return new WaitUntil(() => !IsTransitioning());
        canSendHome = true;
        Plugin.BepinLogger.LogInfo("Send Player to Home");
        scrTrainManager.instance.UseTrain(1, false);
        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
        }
        canSendHome = false;
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
        while (msg == lastPhoneCall)
            msg = "trapConv"+Random.Range(0, TrapConversations.Count);
        if (DebugPhone != -1 && DebugPhoneBool)
            msg = "trapConv"+DebugPhone;
        lastPhoneCall = msg;
        Plugin.BepinLogger.LogInfo($"Phone Call Trap: {msg}");
        scrTextbox.instance.TurnOn(msg);
        if (scrTextbox.instance.conversationLocalized.Count >= 30 && msg != "trapConv9")
        {
            var letterDurationField = typeof(scrTextbox).GetField("letterDuration", BindingFlags.NonPublic | BindingFlags.Instance);
            letterDurationField.SetValue(scrTextbox.instance, 0.01f);
            scrTextbox.instance.typeSpeedMod = 0f;
            Plugin.BepinLogger.LogInfo("Phone Call Trap: Long Conversation, slight text speed up!");
        }
        else
        {
            var letterDurationField = typeof(scrTextbox).GetField("letterDuration", BindingFlags.NonPublic | BindingFlags.Instance);
            letterDurationField.SetValue(scrTextbox.instance, 0.02f);
        }
        if (msg == "trapConv9")
        {
            var letterDurationField = typeof(scrTextbox).GetField("letterDuration", BindingFlags.NonPublic | BindingFlags.Instance);
            letterDurationField.SetValue(scrTextbox.instance, 0.025f);
            Plugin.BepinLogger.LogInfo("Phone Call Trap: trapConv9, slowing down to prevent textbox from breaking");
        }
        stillCalling = true;
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
        stillCalling = false;
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
    
    private static IEnumerator Fast(float duration)
    {
        while (duration > 0)
        {
            Time.timeScale = 2;
            yield return null;
            duration -= Time.deltaTime;
        }
        Time.timeScale = 1;
        Plugin.BepinLogger.LogInfo("Fast Trap ended.");
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
        TrapConversations.Add("trapConv4",
            "##nameMedia Enjoyer;##nikoimg2;Niko, you're not gonna believe this... ##newbox;I watched the next episode of this show I've been binging... ##newbox;and this guy turned himself into a PICKLE!  ##newbox;##nikoimg4;Literally the funniest thing I've ever seen. ##newbox;Anyway, you should probably get back to work. Somebody in BK is waiting on you.");
        TrapConversations.Add("trapConv5",
            "##nameSnatcher;##nikoimg5;AHHHHAHAHAHAHAHAHAHA!! ##newbox;FOOOOOOOOOOooo... oh wait, wrong number.");
        TrapConversations.Add("trapConv6",
            "##nameEgg Frog;##nikoimg2;I've called to make an announcement. ##newbox;##nikoimg9;Handsome Frog is a dirty-faced liar. ##newbox;##nikoimg5;He jumped over my freaking wife! ##newbox;##nikoimg8;That's right, he took pathetic little legs out and jumped over my freaking wife! ##newbox;##nikoimg8;And he said his jump was *t h i s   b i g*. ##newbox;##nikoimg4;And I said that was embarrassing. ##newbox;##nikoimg4;So I'm making a callout post on my frogsky dot net. ##newbox;##nikoimg4;Handsome Frog, you have a small jump. ##newbox;##nikoimg4;It's the height of piece of paper except way smaller. ##newbox;##nikoimg8;And guess what, here's what my jump looks like. ##newbox;##morenikoimg2; ##newbox;##morenikoimg2;That's right baby, all height, no fear, no pillows. ##newbox;##morenikoimg2;Look at that it looks like a super high jump. ##newbox;##morenikoimg1;He jumped over my wife so guess what? ##newbox;##nikoimg5;I'm gonna jump over the earth. ##newbox;##nikoimg5;THAT'S RIGHT THIS IS WHAT YOU GET, MY SUPER JUMP! ##newbox;##nikoimg4;Except I'm not gonna jump over the earth, I'm gonna go higher. ##newbox;##nikoimg5;I'M JUMPING OVER THE MOOOOOOON! ##newbox;##morenikoimg2;HOW DO YOU LIKE THAT PEPPER? I JUMPED OVER THE MOON YOU IDIOT! ##newbox;##morenikoimg3;YOU HAVE 23 HOURS BEFORE THE GRAVITY HITS TADPOLE HQ. ##newbox;##morenikoimg1;NOW GET OUT OF MY SIGHT BEFORE I JUMP OVER YOU TOO.");
        TrapConversations.Add("trapConv7",
            "##nameFred;Hey Niko! Guess what! ##newbox;##nikoimg2;I uhhhhhhhhhh... ##newbox;Uhhhhhhhhhhh.... ##newbox;Uhmmm.......hmmmm...... ##newbox;##nikoimg9;I uh....erm.... ##newbox;Hold on......uh...............  ##newbox;##nikoimg8;.................................................................................................. ##newbox;##nikoimg4;I forgot. ##newbox;Talk to you later!!");
        TrapConversations.Add("trapConv8",
            "##name?????;##nikoimg2;Crazy? ##newbox;I was crazy once. ##newbox;They locked me in a room. ##newbox;An Archipelago room. ##newbox;An Archipelago room with rats. ##newbox;And rats drive me crazy. ##newbox;##morenikoimg1;Crazy? ##newbox;I was crazy once. ##newbox;They locked me in a room. ##newbox;An Archipelago room. ##newbox;An Archipelago room with rats. ##newbox;And rats drive me crazy. ##newbox;##nikoimg5;Crazy? ##newbox;I was crazy once. ##newbox;They locked me in a room. ##newbox;An Archipelago room. ##newbox;An Archipelago room with rats. ##newbox;And rats drive me crazy. ##newbox;Crazy? ##newbox;I was crazy once. ##newbox;They locked me in a room. ##newbox;An Archipelago room. ##newbox;An Archipelago room with rats. ##newbox;And rats drive me crazy. ##newbox;Crazy? ##newbox;I was crazy once. ##newbox;They locked me in a room. ##newbox;An Archipelago room. ##newbox;An Archipelago room with rats. ##newbox;And rats drive me crazy.");
        TrapConversations.Add("trapConv9",
            "##nameMadeline;##nikoimg6;Comf! ##newbox;.... ##newbox;##nikoimg2;You were warned. ##newbox;##morenikoimg1;I'm now going to say every word beginning with Z! ##newbox;##nikoimg5;ZA ##newbox;ZABAGLIONE ##newbox;ZABAGLIONES ##newbox;ZABIONE ##newbox;ZABIONES ##newbox;ZABAJONE ##newbox;ZABAJONES ##newbox;ZABETA ##newbox;ZABETAS ##newbox;ZABRA ##newbox;ZABRAS ##newbox;ZABTIEH ##newbox;ZABTIEHS ##newbox;ZACATON ##newbox;ZACATONS ##newbox;ZACK ##newbox;ZACKS ##newbox;ZADDICK ##newbox;ZADDIK ##newbox;ZADDIKIM ##newbox;ZADDIKS ##newbox;ZAFFAR ##newbox;ZAFFARS ##newbox;ZAFFER ##newbox;ZAFFERS ##newbox;ZAFFIR ##newbox;.... ##newbox;.... ##newbox;.... ##newbox;I'll let you off easy. ##newbox;This Time.");
        TrapConversations.Add("trapConv10",
            "##namePepper;Niko, if you want to be a real professional friend, you need to take this position more seriously. ##newbox;##morenikoimg2;Too serious. Stop that.");
        TrapConversations.Add("trapConv11",
            "Rock, Paper, Scissors? ##addinput:Rock;skip0; ##addinput:Paper;skip1; ##addinput:Scissors;skip0; ##newbox;You lost! ##end; ##newbox;You win!");
        TrapConversations.Add("trapConv12",
            "##nameNonkey Jong;Nonkey Jong Try To Call Red And White Lady ##newbox; ##sp7; ##sp0;                                  ##sp7; ##sp0; ##newbox;Nonkey Jong Have Wrong Number");
        TrapConversations.Add("trapConv13",
            "##nameGaius Van Baelsar;Tell me, for whom do you fight? ##newbox;... Hmmph. How very glib. And do you believe in Eorzea? ##newbox;Eorzea's unity is forged of falsehoods.  ##newbox;Its city-states are built on deceit. ##newbox;And its faith is an instrument of deception. ##newbox;It is naught but a cobweb of lies. ##newbox;To believe in Eorzea is to believe in nothing. ##newbox;In Eorzea, the beast tribes often summon gods to fight in their stead-- ##newbox;though your comrades only rarely respond in kind. ##newbox;Which is strange, is it not? ##newbox;Are the \"Twelve\" otherwise engaged? ##newbox;I was given to understand they were your protectors. ##newbox;Nor is this unknown to your masters. ##newbox;Which prompts the question: why do they cling to these false deities? ##newbox;What drives even men of learning - even the great Louisoix - to grovel at their feet? ##newbox;The answer: your masters lack the strength to do otherwise! ##newbox;For the word of man to mean anything, man must own the world. ##newbox;To this end, he hath fought ever to raise himself through conflict - to grow rich through conquest. ##newbox;And when the dust of battle settles, it is ever the strong who dictate the fate of the weak. ##newbox;Knowing this, but a single path is open to the impotent ruler: that of false worship. ##newbox;A path which leads to enervation and death. ##newbox;Only a man of power can rightly steer the course of civilization. ##newbox;And in this land of creeping mendacity, that one truth will prove its salvation. ##newbox;Come, champion of Eorzea, face me! ##newbox;Your defeat shall serve as proof of my readiness to rule! ##newbox;It is only right that I should take your realm. ##newbox;For none among you has the power to stop me!");
        TrapConversations.Add("trapConv14",
            "##morenikoimg3;Ack--! My disguise wore off!");
        TrapConversations.Add("trapConv15",
            "##nameFlamel;##nikoimg2;Hey Niko, it's been a while hasn't it? ##newbox;I just wanted to call and check in on you. All this running and jumping can be dangerous! ##newbox;And I hear now you're working with people from other realities? That's dangerous too!  ##newbox;Just remember to stay safe. I keep seeing these weird rings of colored dots around the place. ##newbox;They kinda taste like salt and pop rocks.");
        TrapConversations.Add("trapConv16",
            "##nameOnion Boy;Hey, I just wanted to double check something. ##newbox;You don't still have me as \"Onion Boy\" in your contacts, do you?");
        TrapConversations.Add("trapConv17",
            "##nameEvil Niko;Hey, it's me. Niko from the Evil Universe. ##newbox;This weird frog named Salt is helping me be a professional enemy, which apparently involves giving people a bunch of my own money. ##newbox;Not to mention throwing fish back into the water, ripping plants out of their pots, stealing bones from dogs -- ##newbox;sorry, I'm getting a call from my mom who loves me very much. We'll catch up some other time, okay?");
        TrapConversations.Add("trapConv18",
            "##nameNetlia;##nikoimg2;Hello! My name's Netlia. What's yours? ##newbox;##nikoimg3;Niko? Well hi Niko! I just wanted to call you and remind you to plant flowers for Gabi!  ##newbox;She's got a really good reason for doing it. She bought seeds from me. Special seeds! ##newbox;See, they grow as soon as they're planted! And if you unplant them, they turn back to seeds! ##newbox;Hope this helps you! Byeee!~");
        TrapConversations.Add("trapConv19",
            "Is your refrigerator running? ##addinput:Yes;skip0; ##addinput:No;skip1; ##newbox;Well, you better go catch it!##end; ##newbox;Oh. Okay.");
        TrapConversations.Add("trapConv20",
            "##nameDetermined Child;Hello, have you by chance seen my ITEM item? ##newbox;You see, the ITEM item is an item that allows me to use items in battle. ##newbox;When I have the ITEM item, it allows me to use the ITEM button to access my items. Without the ITEM item, most items are useless. ##newbox;Of course, I could have chosen not to shuffle the ITEM button, in which I would just start with the ITEM item. ##newbox;In any case, I cannot use items until my ITEM item has been found. If you find an item that looks like the ITEM item, please let me know.");
        TrapConversations.Add("trapConv21",
            "Boy, I sure do enjoy playing AIR ##sp7; ##sp0;VOLLEY ##sp7; ##sp0;without any items! ##newbox;##morenikoimg2;NOTICE: You need the Item AC Repair to use ACs ##newbox;NOTICE: You need the Item AC Repair to use ACs ##newbox;NOTICE: You need the Item AC Repair to use ACs ##newbox;NOTICE: You need the Item AC Repair to use ACs ##newbox;NOTICE: You need the Item AC Repair to use ACs ##newbox;NOTICE: You need the Item AC Repair to use ACs ##newbox;NOTICE: You need the Item AC Repair to use ACs ##newbox;NOTICE: You need the Item AC Repair to use ACs ##newbox;NOTICE: You need the Item AC Repair to use ACs ##newbox;NOTICE: You need the Item AC Repair to use ACs ##newbox;NOTICE: You need the Item AC Repair to use ACs ##newbox;NOTICE: You need the Item AC Repair to use ACs ##newbox;NOTICE: You need the Item AC Repair to use ACs ##newbox;NOTICE: You need the Item AC Repair to use ACs ##newbox;##nikoimg0;NOTICE: You need the Item AC Repair to-  I think I've taken this joke far enough.");
        // TrapConversations.Add("trapConvXX",
        //     "");
    }
}