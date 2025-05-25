using System.Collections;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.MessageLog.Parts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NikoArchipelago.Archipelago;

public static class APItemSentNotification
{
    public static bool hintCommand;
    private static string _hintStatus;
    public static void GetHintStatus(HintStatus? hintStatus)
    {
        _hintStatus = hintStatus.ToString();
    }
    public static void SentItem(LogMessage message)
    {
        if (!ArchipelagoMenu.APNotifications) return;
        
        bool isHint = false;
        string itemName = "";
        string playerName = "";
        string locationName = "";
        ItemFlags itemFlags = ItemFlags.None;
        float duration = 3f;
        Color? backgroundColor = null;
        Color? durationColor = null;
        Sprite icon = null;
        string hintState = null;
        switch (message)
        {
            case HintItemSendLogMessage hintItemSendLogMessage:
            {
                if ((message.ToString().Contains("(avoid)") || hintItemSendLogMessage.IsFound) && hintCommand)
                {
                    Plugin.BepinLogger.LogInfo("Skipped Notification for Hint Item Send Log Message: " + message + "");
                    return;
                }
                isHint = true;
                if (message.ToString().Contains("(avoid)"))
                    hintState = "Avoid";                    
                else if (message.ToString().Contains("(unspecified)"))
                    hintState = "Unspecified";
                else if (message.ToString().Contains("(priority)"))
                    hintState = "Priority";
                else if (message.ToString().Contains("(no priority)"))
                    hintState = "Non-Priority";
                else hintState = hintItemSendLogMessage.IsFound ? "Found" : "Not Found";
                //TODO: Change to PrintJsonPacket or wait till hintstatus is actually callable from a HINTMESSAGE?!?!
                //If changed to PrintJsonPacket, remove the bool within ArchipelagoConsole.cs
                
                itemName = hintItemSendLogMessage.Item.ItemName;
                playerName = hintItemSendLogMessage.Receiver.Name;
                locationName = hintItemSendLogMessage.Item.LocationName;
                itemFlags = hintItemSendLogMessage.Item.Flags;
                // var hint = ArchipelagoClient._session.DataStorage.GetHints()[ArchipelagoClient._session.DataStorage.GetHints().Length];
                // Plugin.BepinLogger.LogInfo($"Item Name: {hintItemSendLogMessage.Item.ItemName} " +
                //                            $"| Receiving Player: {hintItemSendLogMessage.Receiver.Name} | " +
                //                            $"Hint Name: {ArchipelagoClient._session.Items.GetItemName(hint.ItemId, 
                //                                ArchipelagoClient._session.Players.GetPlayerInfo(hint.ReceivingPlayer).Game)} | " +
                //                            $"Hint Receiving Player: {ArchipelagoClient._session.Players.GetPlayerInfo(hint.ReceivingPlayer).Name} | " +
                //                            $"Hint Status: {hint.Status.ToString()}");
                duration = 5f;
                if (itemFlags.HasFlag(ItemFlags.Advancement))
                {
                    backgroundColor = new Color(0.976f, 0.54f, 1, 0.72f);
                } else if (itemFlags.HasFlag(ItemFlags.NeverExclude))
                {
                    backgroundColor = new Color(0.46f, 0.427f, 1, 0.72f);
                } else if (itemFlags.HasFlag(ItemFlags.Trap))
                {
                    backgroundColor = new Color(1, 0.75f, 0.41f, 0.72f);
                } else if (itemFlags.HasFlag(ItemFlags.None))
                {
                    backgroundColor = new Color(0.6f, 0.6f, 0.6f, 1);
                }
                else
                {
                    backgroundColor = new Color(1, 0.54f, 0.76f, 0.72f);
                }
                durationColor = new Color(0.486f, 0.60f, 1, 0.79f);
                icon = SetSprite(hintItemSendLogMessage.Item.ItemGame, hintItemSendLogMessage.Item.ItemName, hintItemSendLogMessage.Item.Flags);
                break;
            }
            case ItemSendLogMessage itemSendLogMessage:
                itemName = itemSendLogMessage.Item.ItemName;
                playerName = itemSendLogMessage.Receiver.Name;
                locationName = itemSendLogMessage.Item.LocationName;
                itemFlags = itemSendLogMessage.Item.Flags;
                duration = 3.5f;
                //if(Some Custom Data from SaveData)
                if (itemFlags.HasFlag(ItemFlags.Advancement))
                {
                    backgroundColor = new Color(0.976f, 0.54f, 1, 0.72f);
                } else if (itemFlags.HasFlag(ItemFlags.NeverExclude))
                {
                    backgroundColor = new Color(0.46f, 0.427f, 1, 0.72f);
                } else if (itemFlags.HasFlag(ItemFlags.Trap))
                {
                    backgroundColor = new Color(1, 0.75f, 0.41f, 0.72f);
                } else if (itemFlags.HasFlag(ItemFlags.None))
                {
                    backgroundColor = new Color(0.6f, 0.6f, 0.6f, 1);
                }
                else
                {
                    backgroundColor = new Color(1, 0.54f, 0.76f, 0.72f);
                }
                durationColor = new Color(0.486f, 0.60f, 1, 0.79f);
                icon = SetSprite(itemSendLogMessage.Item.ItemGame, itemSendLogMessage.Item.ItemName, itemSendLogMessage.Item.Flags);
                break;
            default:
                return;
        }
        var notification = new APNotification(isHint, itemName, playerName, locationName, itemFlags, duration, backgroundColor, durationColor, icon, hintState);
        NotificationManager.AddNewNotification.Enqueue(notification);
    }

    private static Sprite SetSprite(string itemGame, string itemName, ItemFlags itemFlag)
    {
        Sprite sprite;
        switch (itemGame)
        {
            case "Here Comes Niko!":
                sprite = itemName switch
                {
                    "Coin" => Plugin.CoinSprite,
                    "Cassette" => Plugin.CassetteSprite,
                    "Key" => Plugin.KeySprite,
                    "Super Jump" => Plugin.SuperJumpSprite,
                    "Letter" => Plugin.LetterSprite,
                    "Snail Money" or "1000 Snail Dollar" => Plugin.SnailMoneySprite,
                    "Bugs" or "10 Bugs" => Plugin.BugsSprite,
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
                    "Freeze Trap" => Plugin.FreezeTrapSprite,
                    "Iron Boots Trap" => Plugin.IronBootsTrapSprite,
                    "Whoops! Trap" => Plugin.WhoopsTrapSprite,
                    "My Turn! Trap" => Plugin.MyTurnTrapSprite,
                    "Speed Boost" => Plugin.SpeedBoostSprite,
                    "Home Trap" => Plugin.HomeTrapSprite,
                    "W I D E Trap" => Plugin.WideTrapSprite,
                    "Phone Trap" => Plugin.PhoneCallTrapSprite,
                    "Tiny Trap" => Plugin.TinyTrapSprite,
                    "Gravity Trap" => Plugin.GravityTrapSprite,
                    "Party Invitation" => Plugin.PartyTicketSprite,
                    "Safety Helmet" => Plugin.BonkHelmetSprite,
                    "Bug Net" => Plugin.BugNetSprite,
                    "Soda Repair" => Plugin.SodaRepairSprite,
                    "Parasol Repair" => Plugin.ParasolRepairSprite,
                    "Swim Course" => Plugin.SwimCourseSprite,
                    "Textbox" => Plugin.TextboxItemSprite,
                    "AC Repair" => Plugin.ACRepairSprite,
                    _ => Plugin.ApProgressionSprite
                };
                break;
            default:
            {
                switch (itemName)
                {
                    case "Time Piece" when itemGame == "A Hat in Time":
                        sprite = Plugin.TimePieceSprite;
                        break;
                    case "Yarn" when itemGame == "A Hat in Time":
                    {
                        sprite = Plugin.YarnSprite;
                        break;
                    }
                    default:
                    {
                        if (itemFlag.HasFlag(ItemFlags.Advancement))
                        {
                            sprite = Plugin.ApProgressionSprite;
                        }
                        else if (itemFlag.HasFlag(ItemFlags.NeverExclude))
                        {
                            sprite = Plugin.ApUsefulSprite;
                        }
                        else if (itemFlag.HasFlag(ItemFlags.Trap))
                        {
                            var trapSprites = new[]
                            {
                                Plugin.ApTrapSprite,
                                Plugin.ApTrap2Sprite,
                                Plugin.ApTrap3Sprite
                            };
                            var randomIndex = Random.Range(0, trapSprites.Length);
                            sprite = trapSprites[randomIndex];
                        }
                        else if (itemFlag.HasFlag(ItemFlags.None))
                        {
                            sprite = Plugin.ApFillerSprite;
                        }
                        else
                        {
                            sprite = Plugin.ApProgressionSprite;
                        }

                        break;
                    }
                }

                break;
            }
        }

        return sprite;
    }
}