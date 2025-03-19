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
    public static Image noteShadowBoxImage;
    private static TextMeshProUGUI _noteText;
    private static Image _noteSprite;
    private static scrUIhider _uiHider;
    private static scrUIhider _uiHiderShadow;
    private static Color DefaultShadowNoteColor;

    public void Start()
    {
        noteBox = transform.Find("ItemSent")?.gameObject;
        noteBoxImage = transform.Find("ItemSent")?.GetComponent<Image>();
        noteShadowBox = transform.Find("ItemSentShadow")?.gameObject;
        noteShadowBoxImage = transform.Find("ItemSentShadow")?.GetComponent<Image>();
        _noteText = transform.Find("ItemSent/Text")?.GetComponent<TextMeshProUGUI>();
        _noteSprite = transform.Find("ItemSent/Sprite")?.GetComponent<Image>();

        if (noteBox == null) Plugin.BepinLogger.LogError("ItemSent is null");
        if (_noteText == null) Plugin.BepinLogger.LogError("Text is null");
        if (noteShadowBox == null) Plugin.BepinLogger.LogError("noteShadowBox is null!");
        if (_noteSprite == null) Plugin.BepinLogger.LogError("Couldn't find Sprite!");
        
        DefaultShadowNoteColor = noteShadowBoxImage.color;
        
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

    private static void Clear()
    {
        if (_noteText != null)
        {
            _noteText.text = "";
            _noteText.fontSize = 26;
            _noteText.enableAutoSizing = true;
        }
        noteShadowBoxImage.color = DefaultShadowNoteColor;
    }

    public static void SentItem(LogMessage message)
    {
        Clear();
        if (!ArchipelagoMenu.itemSent) return;
        switch (message)
        {
            case HintItemSendLogMessage hintLogMessage:
                var receiverHint = hintLogMessage.Receiver;
                var networkItem = hintLogMessage.Item;
                var found = hintLogMessage.IsFound;
                noteBoxImage.color = new Color32(172, 174, 255, 255);
                if (hintLogMessage.IsSenderTheActivePlayer)
                {
                    SetSprite(networkItem.ItemGame, networkItem.ItemName, networkItem.Flags);
                    
                    var obtained = found ? "Found" : "Not Found";
                    _noteText.text = $"[Hint]: {receiverHint.Name}'s {networkItem.ItemName} is at {networkItem.LocationName}.\n({obtained})";
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
                if (itemSendLogMessage.IsSenderTheActivePlayer)
                {
                    SetSprite(itemGame, itemName, itemFlag);

                    _noteText.text = $"You sent {itemName} to {receiver}!\n({itemLocation})";
                    if (itemFlag.HasFlag(ItemFlags.Advancement))
                    {
                        noteShadowBoxImage.color = new Color32(249, 138, 255, 255);
                    } else if (itemFlag.HasFlag(ItemFlags.NeverExclude))
                    {
                        noteShadowBoxImage.color = new Color32(116, 109, 255, 255);
                    } else if (itemFlag.HasFlag(ItemFlags.Trap))
                    {
                        noteShadowBoxImage.color = new Color32(255, 192, 105, 255);
                    } else if (itemFlag.HasFlag(ItemFlags.None))
                    {
                        noteShadowBoxImage.color = new Color32(152, 152, 152, 255);
                    }
                    _uiHider.Show(3.25f);
                    _uiHiderShadow.Show(3.25f);
                }
                break;
        }
    }

    private static void SetSprite(string itemGame, string itemName, ItemFlags itemFlag)
    {
        switch (itemGame)
        {
            case "Here Comes Niko!":
                _noteSprite.sprite = itemName switch
                {
                    "Coin" => Plugin.CoinSprite,
                    "Cassette" => Plugin.CassetteSprite,
                    "Key" => Plugin.KeySprite,
                    "Super Jump" => Plugin.SuperJumpSprite,
                    "Letter" => Plugin.LetterSprite,
                    "Snail Money" or "1000 Snail Dollar" => Plugin.SnailMoneySprite,
                    "Bugs" or "10 Bugs" => Plugin.BugSprite,
                    "Apples" or "25 Apples" => Plugin.ApplesSprite,
                    "Contact List 1" or "Contact List 2" or "Progressive Contact List" => Plugin.ContactListSprite,
                    "Gary's Garden Ticket" => Plugin.GgSprite,
                    "Hairball City Ticket" => Plugin.HcSprite,
                    "Turbine Town Ticket" => Plugin.TtSprite,
                    "Salmon Creek Forest Ticket" => Plugin.SfcSprite,
                    "Public Pool Ticket" => Plugin.PpSprite,
                    "Bathhouse Ticket" => Plugin.BathSprite,
                    "Tadpole HQ Ticket" => Plugin.HqSprite,
                    "Hairball City Fish" => Plugin.HairballFishSprite,
                    "Turbine Town Fish" => Plugin.TurbineFishSprite,
                    "Salmon Creek Forest Fish" => Plugin.SalmonFishSprite,
                    "Public Pool Fish" => Plugin.PoolFishSprite,
                    "Bathhouse Fish" => Plugin.BathFishSprite,
                    "Tadpole HQ Fish" => Plugin.TadpoleFishSprite,
                    "Hairball City Key" => Plugin.HairballKeySprite,
                    "Turbine Town Key" => Plugin.TurbineKeySprite,
                    "Salmon Creek Forest Key" => Plugin.SalmonKeySprite,
                    "Public Pool Key" => Plugin.PoolKeySprite,
                    "Bathhouse Key" => Plugin.BathKeySprite,
                    "Tadpole HQ Key" => Plugin.TadpoleKeySprite,
                    "Hairball City Flower" => Plugin.HairballFlowerSprite,
                    "Turbine Town Flower" => Plugin.TurbineFlowerSprite,
                    "Salmon Creek Forest Flower" => Plugin.SalmonFlowerSprite,
                    "Public Pool Flower" => Plugin.PoolFlowerSprite,
                    "Bathhouse Flower" => Plugin.BathFlowerSprite,
                    "Tadpole HQ Flower" => Plugin.TadpoleFlowerSprite,
                    "Hairball City Cassette" => Plugin.HairballCassetteSprite,
                    "Turbine Town Cassette" => Plugin.TurbineCassetteSprite,
                    "Salmon Creek Forest Cassette" => Plugin.SalmonCassetteSprite,
                    "Public Pool Cassette" => Plugin.PoolCassetteSprite,
                    "Bathhouse Cassette" => Plugin.BathCassetteSprite,
                    "Tadpole HQ Cassette" => Plugin.TadpoleCassetteSprite,
                    "Gary's Garden Cassette" => Plugin.GardenCassetteSprite,
                    "Hairball City Seed" => Plugin.HairballSeedSprite,
                    "Salmon Creek Forest Seed" => Plugin.SalmonSeedSprite,
                    "Bathhouse Seed" => Plugin.BathSeedSprite,
                    _ => Plugin.ApProgressionSprite
                };
                break;
            default:
            {
                switch (itemName)
                {
                    case "Time Piece" when itemGame == "A Hat in Time":

                        break;
                    case "Yarn" when itemGame == "A Hat in Time":
                    {
                        var yarnSprites = new[]
                        {
                            _noteSprite.sprite = Plugin.YarnSprite,
                            _noteSprite.sprite = Plugin.Yarn2Sprite,
                            _noteSprite.sprite = Plugin.Yarn3Sprite,
                            _noteSprite.sprite = Plugin.Yarn4Sprite,
                            _noteSprite.sprite = Plugin.Yarn5Sprite
                        };
                        var randomIndex = Random.Range(0, yarnSprites.Length);
                        _noteSprite.sprite = yarnSprites[randomIndex];
                        break;
                    }
                    default:
                    {
                        if (itemFlag.HasFlag(ItemFlags.Advancement))
                        {
                            _noteSprite.sprite = Plugin.ApProgressionSprite;
                        }
                        else if (itemFlag.HasFlag(ItemFlags.NeverExclude))
                        {
                            _noteSprite.sprite = Plugin.ApUsefulSprite;
                        }
                        else if (itemFlag.HasFlag(ItemFlags.Trap))
                        {
                            var trapSprites = new[]
                            {
                                _noteSprite.sprite = Plugin.ApTrapSprite,
                                _noteSprite.sprite = Plugin.ApTrap2Sprite,
                                _noteSprite.sprite = Plugin.ApTrap3Sprite
                            };
                            var randomIndex = Random.Range(0, trapSprites.Length);
                            _noteSprite.sprite = trapSprites[randomIndex];
                        }
                        else if (itemFlag.HasFlag(ItemFlags.None))
                        {
                            _noteSprite.sprite = Plugin.ApFillerSprite;
                        }
                        else
                        {
                            _noteSprite.sprite = Plugin.ApProgressionSprite;
                        }

                        break;
                    }
                }

                break;
            }
        }
    }
}