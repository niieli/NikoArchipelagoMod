﻿using System;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace NikoArchipelago.Archipelago;

public class APItemSentNotification : MonoBehaviour
{
    public static GameObject noteBox;
    public static GameObject noteShadowBox;
    public static Image noteBoxImage;
    public static RawImage _noteImage;
    private static RawImage _noteProgImage;
    private static RawImage _noteUsefulImage;
    private static RawImage _noteFillerImage;
    private static RawImage _noteTrapImage;
    private static RawImage _noteTrap2Image;
    private static RawImage _noteTrap3Image;
    private static RawImage _noteCoinImage;
    private static RawImage _noteCassetteImage;
    private static RawImage _noteContactListImage;
    private static RawImage _noteKeyImage;
    private static RawImage _noteApplesImage;
    private static RawImage _noteFishImage;
    private static RawImage _noteLetterImage;
    private static RawImage _noteSnailMoneyImage;
    private static RawImage _noteBugImage;
    private static RawImage _noteGardenImage;
    private static RawImage _noteHairballImage;
    private static RawImage _noteTurbineImage;
    private static RawImage _noteSalmonImage;
    private static RawImage _notePoolImage;
    private static RawImage _noteBathhouseImage;
    private static RawImage _noteTadpoleImage;
    private static RawImage _noteSuperJumpImage;
    private static TextMeshProUGUI _noteText;
    private static scrUIhider _uiHider;
    private static scrUIhider _uiHiderShadow;

    public void Start()
    {
        noteBox = transform.Find("ItemSent")?.gameObject;
        noteBoxImage = transform.Find("ItemSent")?.GetComponent<Image>();
        noteShadowBox = transform.Find("ItemSentShadow")?.gameObject;
        _noteImage = transform.Find("ItemSent/Icon")?.GetComponent<RawImage>();
        _noteProgImage = transform.Find("ItemSent/Prog")?.GetComponent<RawImage>();
        _noteUsefulImage = transform.Find("ItemSent/Useful")?.GetComponent<RawImage>();
        _noteFillerImage = transform.Find("ItemSent/Filler")?.GetComponent<RawImage>();
        _noteTrapImage = transform.Find("ItemSent/Trap")?.GetComponent<RawImage>();
        _noteTrap2Image = transform.Find("ItemSent/Trap2")?.GetComponent<RawImage>();
        _noteTrap3Image = transform.Find("ItemSent/Trap3")?.GetComponent<RawImage>();
        _noteCoinImage = transform.Find("ItemSent/Coin")?.GetComponent<RawImage>();
        _noteCassetteImage = transform.Find("ItemSent/Cassette")?.GetComponent<RawImage>();
        _noteContactListImage = transform.Find("ItemSent/ContactList")?.GetComponent<RawImage>();
        _noteKeyImage = transform.Find("ItemSent/Key")?.GetComponent<RawImage>();
        _noteApplesImage = transform.Find("ItemSent/Apples")?.GetComponent<RawImage>();
        _noteFishImage = transform.Find("ItemSent/Fish")?.GetComponent<RawImage>();
        _noteLetterImage = transform.Find("ItemSent/Letter")?.GetComponent<RawImage>();
        _noteSnailMoneyImage = transform.Find("ItemSent/SnailMoney")?.GetComponent<RawImage>();
        _noteBugImage = transform.Find("ItemSent/Bug")?.GetComponent<RawImage>();
        _noteGardenImage = transform.Find("ItemSent/Garden")?.GetComponent<RawImage>();
        _noteHairballImage = transform.Find("ItemSent/Hairball")?.GetComponent<RawImage>();
        _noteTurbineImage = transform.Find("ItemSent/Turbine")?.GetComponent<RawImage>();
        _noteSalmonImage = transform.Find("ItemSent/Salmon")?.GetComponent<RawImage>();
        _notePoolImage = transform.Find("ItemSent/Pool")?.GetComponent<RawImage>();
        _noteBathhouseImage = transform.Find("ItemSent/Bathhouse")?.GetComponent<RawImage>();
        _noteTadpoleImage = transform.Find("ItemSent/Tadpole")?.GetComponent<RawImage>();
        _noteSuperJumpImage = transform.Find("ItemSent/SuperJump")?.GetComponent<RawImage>();
        _noteText = transform.Find("ItemSent/Text")?.GetComponent<TextMeshProUGUI>();

        if (noteBox == null) Plugin.BepinLogger.LogError("ItemSent is null");
        if (_noteText == null) Plugin.BepinLogger.LogError("Text is null");
        if (_noteImage == null) Plugin.BepinLogger.LogError("Icon is null");
        if (_noteCoinImage == null) Plugin.BepinLogger.LogError("_noteCoinImage is null!");
        if (_noteCassetteImage == null) Plugin.BepinLogger.LogError("_noteCassetteImage is null!");
        if (_noteKeyImage == null) Plugin.BepinLogger.LogError("_noteKeyImage is null!");
        if (_noteLetterImage == null) Plugin.BepinLogger.LogError("_noteLetterImage is null!");
        if (_noteSnailMoneyImage == null) Plugin.BepinLogger.LogError("_noteSnailMoneyImage is null!");
        if (_noteBugImage == null) Plugin.BepinLogger.LogError("_noteBugImage is null!");
        if (_noteApplesImage == null) Plugin.BepinLogger.LogError("_noteApplesImage is null!");
        if (_noteContactListImage == null) Plugin.BepinLogger.LogError("_noteContactListImage is null!");
        if (_noteGardenImage == null) Plugin.BepinLogger.LogError("_noteGardenImage is null!");
        if (_noteHairballImage == null) Plugin.BepinLogger.LogError("_noteHairballImage is null!");
        if (_noteTurbineImage == null) Plugin.BepinLogger.LogError("_noteTurbineImage is null!");
        if (_noteSalmonImage == null) Plugin.BepinLogger.LogError("_noteSalmonImage is null!");
        if (_notePoolImage == null) Plugin.BepinLogger.LogError("_notePoolImage is null!");
        if (_noteBathhouseImage == null) Plugin.BepinLogger.LogError("_noteBathhouseImage is null!");
        if (_noteTadpoleImage == null) Plugin.BepinLogger.LogError("_noteTadpoleImage is null!");
        if (_noteSuperJumpImage == null) Plugin.BepinLogger.LogError("_noteSuperJumpImage is null!");
        if (noteShadowBox == null) Plugin.BepinLogger.LogError("noteShadowBox is null!");
        
        _noteCoinImage.gameObject.SetActive(false);
        _noteCassetteImage.gameObject.SetActive(false);
        _noteKeyImage.gameObject.SetActive(false);
        _noteSuperJumpImage.gameObject.SetActive(false);
        _noteLetterImage.gameObject.SetActive(false);
        _noteSnailMoneyImage.gameObject.SetActive(false);
        _noteBugImage.gameObject.SetActive(false);
        _noteApplesImage.gameObject.SetActive(false);
        _noteContactListImage.gameObject.SetActive(false);
        _noteGardenImage.gameObject.SetActive(false);
        _noteHairballImage.gameObject.SetActive(false);
        _noteTurbineImage.gameObject.SetActive(false);
        _noteSalmonImage.gameObject.SetActive(false);
        _notePoolImage.gameObject.SetActive(false);
        _noteBathhouseImage.gameObject.SetActive(false);
        _noteTadpoleImage.gameObject.SetActive(false);
        _noteProgImage.gameObject.SetActive(false);
        _noteUsefulImage.gameObject.SetActive(false);
        _noteFillerImage.gameObject.SetActive(false);
        _noteFishImage.gameObject.SetActive(false);
        _noteImage.gameObject.SetActive(false);
        _noteTrapImage.gameObject.SetActive(false);
        _noteTrap2Image.gameObject.SetActive(false);
        _noteTrap3Image.gameObject.SetActive(false);
        _noteImage.gameObject.SetActive(false);
        
        _uiHider = transform.Find("ItemSent")?.gameObject.AddComponent<scrUIhider>();
        _uiHiderShadow = transform.Find("ItemSentShadow")?.gameObject.AddComponent<scrUIhider>();
        if (_uiHider != null)
        {
            var reference = GameObject.Find("UI/Apple Displayer").GetComponent<scrUIhider>();
            _uiHider.useAlphaCurve = reference.useAlphaCurve;
            _uiHider.alphaCurve = reference.alphaCurve;
            _uiHider.animationCurve = reference.animationCurve;
            _uiHider.duration = 0.75f;
            _uiHider.hideOffset = new Vector3(0, -150, 0);
            _uiHiderShadow.useAlphaCurve = _uiHider.useAlphaCurve;
            _uiHiderShadow.alphaCurve = _uiHider.alphaCurve;
            _uiHiderShadow.animationCurve = _uiHider.animationCurve;
            _uiHiderShadow.duration = 0.75f;
            _uiHiderShadow.hideOffset = new Vector3(0, -150, 0);
        }
    }

    public static void SentItem(LogMessage message)
    {
        if (!ArchipelagoMenu.itemSent) return;
        _noteCoinImage.gameObject.SetActive(false);
        _noteCassetteImage.gameObject.SetActive(false);
        _noteKeyImage.gameObject.SetActive(false);
        _noteSuperJumpImage.gameObject.SetActive(false);
        _noteLetterImage.gameObject.SetActive(false);
        _noteSnailMoneyImage.gameObject.SetActive(false);
        _noteBugImage.gameObject.SetActive(false);
        _noteApplesImage.gameObject.SetActive(false);
        _noteContactListImage.gameObject.SetActive(false);
        _noteGardenImage.gameObject.SetActive(false);
        _noteHairballImage.gameObject.SetActive(false);
        _noteTurbineImage.gameObject.SetActive(false);
        _noteSalmonImage.gameObject.SetActive(false);
        _notePoolImage.gameObject.SetActive(false);
        _noteBathhouseImage.gameObject.SetActive(false);
        _noteTadpoleImage.gameObject.SetActive(false);
        _noteProgImage.gameObject.SetActive(false);
        _noteUsefulImage.gameObject.SetActive(false);
        _noteFillerImage.gameObject.SetActive(false);
        _noteFishImage.gameObject.SetActive(false);
        _noteImage.gameObject.SetActive(false);
        _noteTrapImage.gameObject.SetActive(false);
        _noteTrap2Image.gameObject.SetActive(false);
        _noteTrap3Image.gameObject.SetActive(false);
        _noteImage.gameObject.SetActive(false);
        _noteText.fontSize = 26;
        _noteText.enableAutoSizing = true;
        switch (message)
        {
            case HintItemSendLogMessage hintLogMessage:
                var receiverHint = hintLogMessage.Receiver;
                var networkItem = hintLogMessage.Item;
                var found = hintLogMessage.IsFound;
                noteBoxImage.color = new Color32(172, 174, 255, 255);
                if (hintLogMessage.IsSenderTheActivePlayer)
                {
                    if (networkItem.ItemGame == "Here Comes Niko!")
                    {
                        switch (networkItem.ItemName)
                        {
                            case "Coin":
                                _noteCoinImage.gameObject.SetActive(true);
                                break;
                            case "Cassette":
                                _noteCassetteImage.gameObject.SetActive(true);
                                break;
                            case "Key":
                                _noteKeyImage.gameObject.SetActive(true);
                                break;
                            case "Super Jump":
                                _noteSuperJumpImage.gameObject.SetActive(true);
                                break;
                            case "Letter":
                                _noteLetterImage.gameObject.SetActive(true);
                                break;
                            case "1000 Snail Dollar":
                                _noteSnailMoneyImage.gameObject.SetActive(true);
                                break;
                            case "10 Bugs":
                                _noteBugImage.gameObject.SetActive(true);
                                break;
                            case "25 Apples":
                                _noteApplesImage.gameObject.SetActive(true);
                                break;
                            case "Contact List 1" or "Contact List 2" or "Progressive Contact List":
                                _noteContactListImage.gameObject.SetActive(true);
                                break;
                            case "Gary's Garden Ticket":
                                _noteGardenImage.gameObject.SetActive(true);
                                break;
                            case "Hairball City Ticket":
                                _noteHairballImage.gameObject.SetActive(true);
                                break;
                            case "Turbine Town Ticket":
                                _noteTurbineImage.gameObject.SetActive(true);
                                break;
                            case "Salmon Creek Forest Ticket":
                                _noteSalmonImage.gameObject.SetActive(true);
                                break;
                            case "Public Pool Ticket":
                                _notePoolImage.gameObject.SetActive(true);
                                break;
                            case "Bathhouse Ticket":
                                _noteBathhouseImage.gameObject.SetActive(true);
                                break;
                            case "Tadpole HQ Ticket":
                                _noteTadpoleImage.gameObject.SetActive(true);
                                break;
                        }
                    }
                    else
                    {
                        if (networkItem.Flags.HasFlag(ItemFlags.Advancement))
                        {
                            _noteProgImage.gameObject.SetActive(true);
                        }
                        else if (networkItem.Flags.HasFlag(ItemFlags.NeverExclude))
                        {
                            _noteUsefulImage.gameObject.SetActive(true);
                        }
                        else if (networkItem.Flags.HasFlag(ItemFlags.Trap))
                        {
                            var trapTextures = new[]
                            {
                                _noteTrapImage,
                                _noteTrap2Image,
                                _noteTrap3Image
                            };
                            var randomIndex = Random.Range(0, trapTextures.Length);
                            trapTextures[randomIndex].gameObject.SetActive(true);
                        }
                        else if (networkItem.Flags.HasFlag(ItemFlags.None))
                        {
                            _noteFillerImage.gameObject.SetActive(true);
                        }
                        else
                        {
                            _noteImage.gameObject.SetActive(true);
                        }
                    }
                    _noteText.text = $"[Hint]: {receiverHint.Name}'s {networkItem.ItemName} is at {networkItem.LocationName}.\n({found})";
                    _uiHider.Show(3.25f);
                    _uiHiderShadow.Show(3.25f);
                }
                break;
            case ItemSendLogMessage itemSendLogMessage: 
                var receiver = itemSendLogMessage.Receiver.Name;
                var itemName = itemSendLogMessage.Item.ItemName;
                var itemGame = itemSendLogMessage.Item.ItemGame;
                var itemFlag = itemSendLogMessage.Item.Flags;
                var itemLocation = itemSendLogMessage.Item.LocationName;
                noteBoxImage.color = Color.white;
                if (itemSendLogMessage.IsSenderTheActivePlayer && !itemSendLogMessage.IsReceiverTheActivePlayer)
                {
                    if (itemGame == "Here Comes Niko!")
                    {
                        switch (itemName)
                        {
                            case "Coin":
                                _noteCoinImage.gameObject.SetActive(true);
                                break;
                            case "Cassette":
                                _noteCassetteImage.gameObject.SetActive(true);
                                break;
                            case "Key":
                                _noteKeyImage.gameObject.SetActive(true);
                                break;
                            case "Super Jump":
                                _noteSuperJumpImage.gameObject.SetActive(true);
                                break;
                            case "Letter":
                                _noteLetterImage.gameObject.SetActive(true);
                                break;
                            case "1000 Snail Dollar":
                                _noteSnailMoneyImage.gameObject.SetActive(true);
                                break;
                            case "10 Bugs":
                                _noteBugImage.gameObject.SetActive(true);
                                break;
                            case "25 Apples":
                                _noteApplesImage.gameObject.SetActive(true);
                                break;
                            case "Contact List 1" or "Contact List 2" or "Progressive Contact List":
                                _noteContactListImage.gameObject.SetActive(true);
                                break;
                            case "Gary's Garden Ticket":
                                _noteGardenImage.gameObject.SetActive(true);
                                break;
                            case "Hairball City Ticket":
                                _noteHairballImage.gameObject.SetActive(true);
                                break;
                            case "Turbine Town Ticket":
                                _noteTurbineImage.gameObject.SetActive(true);
                                break;
                            case "Salmon Creek Forest Ticket":
                                _noteSalmonImage.gameObject.SetActive(true);
                                break;
                            case "Public Pool Ticket":
                                _notePoolImage.gameObject.SetActive(true);
                                break;
                            case "Bathhouse Ticket":
                                _noteBathhouseImage.gameObject.SetActive(true);
                                break;
                            case "Tadpole HQ Ticket":
                                _noteTadpoleImage.gameObject.SetActive(true);
                                break;
                        }
                    }
                    else
                    {
                        if (itemFlag.HasFlag(ItemFlags.Advancement))
                        {
                            _noteProgImage.gameObject.SetActive(true);
                        }
                        else if (itemFlag.HasFlag(ItemFlags.NeverExclude))
                        {
                            _noteUsefulImage.gameObject.SetActive(true);
                        }
                        else if (itemFlag.HasFlag(ItemFlags.Trap))
                        {
                            var trapTextures = new[]
                            {
                                _noteTrapImage,
                                _noteTrap2Image,
                                _noteTrap3Image
                            };
                            var randomIndex = Random.Range(0, trapTextures.Length);
                            trapTextures[randomIndex].gameObject.SetActive(true);
                        }
                        else if (itemFlag.HasFlag(ItemFlags.None))
                        {
                            _noteFillerImage.gameObject.SetActive(true);
                        }
                        else
                        {
                            _noteImage.gameObject.SetActive(true);
                        }
                    }

                    _noteText.text = $"You sent {itemName} to {receiver}!\n({itemLocation})";
                    _uiHider.Show(3.25f);
                    _uiHiderShadow.Show(3.25f);
                }
                break;
        }
    }
}