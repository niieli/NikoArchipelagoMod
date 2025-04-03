using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using TMPro;
using UnityEngine;

namespace NikoArchipelago;

public class TrapManager : MonoBehaviour
{
    public static TrapManager instance;
    public static bool FreezeOn, IronBootsOn, MyTurnOn, WhoopsOn, ReversedControlsOn;
    public GameObject trapFreeze = Plugin.FreezeTrap; 
    public GameObject trapIronBoots = Plugin.IronBootsTrap;
    public GameObject trapMyTurn = Plugin.MyTurnTrap;
    public GameObject trapWhoops = Plugin.WhoopsTrap;
    public Transform trapListUI; 
    private Dictionary<string, GameObject> activeTraps = new Dictionary<string, GameObject>();
    private GameObject trapUI;
    private static scrUIhider uiHider;

    private void Awake()
    {
        //if (!ArchipelagoClient.IsValidScene() && !Plugin.SaveEstablished) return;
        trapListUI = transform.Find("TrapPanel");
        if (instance == null) instance = this;
    }

    private void Start()
    {
        
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
            ActivateTrap("My Turn!", 60f);
        }
        if (WhoopsOn)
        {
            WhoopsOn = false;
            ActivateTrap("Whoops!", 5f);
        }
        if (ReversedControlsOn)
        {
            ReversedControlsOn = false;
            ActivateTrap("Reverse Controls", 50f);
        }
    }

    public void ActivateTrap(string trapName, float duration)
    {
        //if (!ArchipelagoClient.IsValidScene() || !Plugin.SaveEstablished) return;
        if (activeTraps.ContainsKey(trapName)) return;

        Plugin.BepinLogger.LogInfo("Received Trap: " + trapName);
        switch (trapName)
        {
            case "Freeze":
                Plugin.BepinLogger.LogInfo("Instantiating Freeze Trap");
                trapUI = Instantiate(trapFreeze, trapListUI);
                StartCoroutine(Freeze(duration));
                break;
            case "Iron Boots":
                Plugin.BepinLogger.LogInfo("Instantiating Iron Boots Trap");
                trapUI = Instantiate(trapIronBoots, trapListUI);
                StartCoroutine(IronBoots(duration));
                break;
            case "My Turn!":
                Plugin.BepinLogger.LogInfo("Instantiating MyTurnTrap");
                trapUI = Instantiate(trapMyTurn, trapListUI);
                StartCoroutine(MyTurn(duration));
                break;
            case "Whoops!":
                Plugin.BepinLogger.LogInfo("Instantiating WhoopsTrap");
                trapUI = Instantiate(trapWhoops, trapListUI);
                break;
            case "Reverse Controls":
                Plugin.BepinLogger.LogInfo("Instantiating ReverseControlsTrap");
                trapUI = Instantiate(trapWhoops, trapListUI);
                StartCoroutine(ReverseControls(duration));
                break;
        }
        StartCoroutine(UpdateTrapTimer(trapUI, trapName, duration));

        activeTraps[trapName] = trapUI;
    }

    private IEnumerator UpdateTrapTimer(GameObject trap, string trapName, float duration)
    {
        // var hider = trap.gameObject.AddComponent<scrUIhider>();
        // if (hider != null)
        // {
        //     var reference = GameObject.Find("UI/Apple Displayer").GetComponent<scrUIhider>();
        //     hider.useAlphaCurve = reference.useAlphaCurve;
        //     hider.alphaCurve = reference.alphaCurve;
        //     hider.animationCurve = reference.animationCurve;
        //     hider.duration = 0.75f;
        //     hider.hideOffset = new Vector3(-225, 0, 0);
        // }
        // hider.Show(duration-.25f);
        var timerTextShadow = trap.transform.Find("Timer/TextShadow").GetComponent<TextMeshProUGUI>();
        var timerText = trap.transform.Find("Timer/TextFront").GetComponent<TextMeshProUGUI>();

        float timeRemaining = duration;
        while (timeRemaining > 0)
        {
            timerText.text = timeRemaining.ToString("F1") + "s";
            timerTextShadow.text = timeRemaining.ToString("F1") + "s";
            yield return null;
            timeRemaining -= Time.deltaTime;
        }
        timerText.text = "0.0s";
        timerTextShadow.text = "0.0s";
        RemoveTrap(trapName);
    }

    public void RemoveTrap(string trapName)
    {
        if (activeTraps.TryGetValue(trapName, out GameObject trap))
        {
            Destroy(trap);
            activeTraps.Remove(trapName);
        }
    }
    
    // public static void WhoopsTrap()
    // {
    //     var instance = scrGameSaveManager.instance;
    //     instance.StartCoroutine(Whoops());
    // }
    
    private static IEnumerator Freeze(float duration)
    {
        MyCharacterController.instance.blockMovement = true;
        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
        }
        MyCharacterController.instance.blockMovement = false;
        Plugin.BepinLogger.LogInfo("Freeze Trap ended.");
    }

    private static IEnumerator IronBoots(float duration)
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
    }
    private static IEnumerator MyTurn(float duration)
    {
        var waitTime = 0f;
        while (duration > 0)
        {
            if (waitTime < 0)
            {
                float randomX = UnityEngine.Random.Range(-35f, 35f);
                float randomZ = UnityEngine.Random.Range(-35f, 35f);
                Vector3 randomVelocity = new Vector3(randomX, 0, randomZ);

                MyCharacterController.instance.requestAddVelocity(randomVelocity);
                Plugin.BepinLogger.LogInfo($"Applied Velocity: {randomVelocity}");
            
                var shouldJump = UnityEngine.Random.value > 0.4f;
                if (shouldJump)
                {
                    var _jumpRequestedField = typeof(MyCharacterController).GetField("_jumpRequested", BindingFlags.NonPublic | BindingFlags.Instance);
                    _jumpRequestedField.SetValue(MyCharacterController.instance, true);
                    Plugin.BepinLogger.LogInfo("Player Jumped!");
                }
            
                var shouldDive = UnityEngine.Random.value > 0.25f;
                if (shouldDive)
                {
                    MyCharacterController.instance._diveRequested = true;
                    Plugin.BepinLogger.LogInfo("Player Dived!");
                }
            
                waitTime = UnityEngine.Random.Range(0f, 3.5f);
            }
            yield return null;
            duration -= Time.deltaTime;
            waitTime -= Time.deltaTime;
        }
        Plugin.BepinLogger.LogInfo("My Turn! Trap ended.");
    }
    
    private static IEnumerator ReverseControls(float duration)
    {
        PlayerCharacterInputs playerInputs;
        while (duration > 0)
        {
            playerInputs.MoveAxisRight = 0;
            if (playerInputs.MoveAxisRight > 0)
            {
                Plugin.BepinLogger.LogInfo("Left or Right has been pressed");
            }
            float waitTime = 0.1f;
            duration -= waitTime;
        
            yield return new WaitForSeconds(waitTime);
        }
        Plugin.BepinLogger.LogInfo("Reversed Controls Trap ended.");
    }
    
}