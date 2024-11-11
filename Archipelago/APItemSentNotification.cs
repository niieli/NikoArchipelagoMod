using System;
using System.Drawing;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using TMPro;
using UnityEngine;

namespace NikoArchipelago.Archipelago;

public class APItemSentNotification : MonoBehaviour
{
    public GameObject noteBox;
    private static Image _noteImage;
    private static Image _noteProgImage;
    private static Image _noteUsefulImage;
    private static Image _noteFillerImage;
    private static Image _noteTrapImage;
    private static Image _noteTrap2Image;
    private static Image _noteTrap3Image;
    private static Image _noteCoinImage;
    private static Image _noteCassetteImage;
    private static Image _noteContactListImage;
    private static Image _noteKeyImage;
    private static Image _noteApplesImage;
    private static Image _noteFishImage;
    private static Image _noteLetterImage;
    private static Image _noteSnailMoneyImage;
    private static Image _noteBugImage;
    private static Image _noteGardenImage;
    private static Image _noteHairballImage;
    private static Image _noteTurbineImage;
    private static Image _noteSalmonImage;
    private static Image _notePoolImage;
    private static Image _noteBathhouseImage;
    private static Image _noteTadpoleImage;
    private static TextMeshProUGUI _noteText;
    private static scrUIhider _uiHider;

    public void Start()
    {
        noteBox = transform.Find("ItemSent")?.gameObject;
        _noteImage = transform.Find("ItemSent/Icon")?.GetComponent<Image>();
        _noteProgImage = transform.Find("ItemSent/Prog")?.GetComponent<Image>();
        _noteUsefulImage = transform.Find("ItemSent/Useful")?.GetComponent<Image>();
        _noteFillerImage = transform.Find("ItemSent/Filler")?.GetComponent<Image>();
        _noteTrapImage = transform.Find("ItemSent/Trap")?.GetComponent<Image>();
        _noteTrap2Image = transform.Find("ItemSent/Trap2")?.GetComponent<Image>();
        _noteTrap3Image = transform.Find("ItemSent/Trap3")?.GetComponent<Image>();
        _noteCoinImage = transform.Find("ItemSent/Coin")?.GetComponent<Image>();
        _noteCassetteImage = transform.Find("ItemSent/Cassette")?.GetComponent<Image>();
        _noteContactListImage = transform.Find("ItemSent/ContactList")?.GetComponent<Image>();
        _noteKeyImage = transform.Find("ItemSent/Key")?.GetComponent<Image>();
        _noteApplesImage = transform.Find("ItemSent/Apples")?.GetComponent<Image>();
        _noteFishImage = transform.Find("ItemSent/Fish")?.GetComponent<Image>();
        _noteLetterImage = transform.Find("ItemSent/Letter")?.GetComponent<Image>();
        _noteSnailMoneyImage = transform.Find("ItemSent/SnailMoney")?.GetComponent<Image>();
        _noteBugImage = transform.Find("ItemSent/Bug")?.GetComponent<Image>();
        _noteGardenImage = transform.Find("ItemSent/Garden")?.GetComponent<Image>();
        _noteHairballImage = transform.Find("ItemSent/Hairball")?.GetComponent<Image>();
        _noteTurbineImage = transform.Find("ItemSent/Turbine")?.GetComponent<Image>();
        _noteSalmonImage = transform.Find("ItemSent/Salmon")?.GetComponent<Image>();
        _notePoolImage = transform.Find("ItemSent/Pool")?.GetComponent<Image>();
        _noteBathhouseImage = transform.Find("ItemSent/Bathhouse")?.GetComponent<Image>();
        _noteTadpoleImage = transform.Find("ItemSent/Tadpole")?.GetComponent<Image>();
        _noteText = transform.Find("ItemSent/Text")?.GetComponent<TextMeshProUGUI>();

        if (noteBox == null) Plugin.BepinLogger.LogError("ItemSent is null");
        if (_noteText == null) Plugin.BepinLogger.LogError("Text is null");
        if (_noteImage == null) Plugin.BepinLogger.LogError("Icon is null");
        
        _uiHider = transform.Find("ItemSent")?.gameObject.AddComponent<scrUIhider>();
        if (_uiHider != null)
        {
            var reference = GameObject.Find("UI/Apple Displayer").GetComponent<scrUIhider>();
            _uiHider.useAlphaCurve = reference.useAlphaCurve;
            _uiHider.alphaCurve = reference.alphaCurve;
            _uiHider.animationCurve = reference.animationCurve;
            _uiHider.duration = 0.5f;
            _uiHider.hideOffset = new Vector3(0, -100, 0);
        }
    }

    public static void SentItem(LogMessage message, Enum noteType=null)
    {
        //if (var t = message.GetType() != MessagePartType.Item)
        _noteText.fontSize = 26;
        _noteText.text = message.ToString();
        _uiHider.Show(2.5f);
    }

    public enum NoteType
    {
        Prog,
        Useful,
        Trap,
        Filler,
        Coin,
        Cassette,
        ContactList,
        Key,
        Apples,
        Fish,
        Letter,
        SnailMoney,
        Bug,
        Garden,
        Hairball,
        Turbine,
        Salmon,
        Pool,
        Bathhouse,
        Tadpole,
    }

}