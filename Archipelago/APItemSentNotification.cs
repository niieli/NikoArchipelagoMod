using System;
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
        string senderName = "";
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
                if (!hintItemSendLogMessage.IsRelatedToActivePlayer) return;
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
                if (itemName == null)
                    itemName = hintItemSendLogMessage.Item.ItemId.ToString();
                playerName = hintItemSendLogMessage.Receiver.Name;
                senderName = hintItemSendLogMessage.Sender.Name;
                locationName = hintItemSendLogMessage.Item.LocationName;
                if (locationName == null)
                    locationName = hintItemSendLogMessage.Item.LocationId.ToString();
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
                icon = SetSprite(hintItemSendLogMessage.Item.ItemGame, hintItemSendLogMessage.Item.ItemName, hintItemSendLogMessage.Item.ItemId, hintItemSendLogMessage.Item.Flags);
                break;
            }
            case ItemSendLogMessage itemSendLogMessage:
                if (!itemSendLogMessage.IsSenderTheActivePlayer) return;
                itemName = itemSendLogMessage.Item.ItemName;
                if (itemName == null)
                    itemName = itemSendLogMessage.Item.ItemId.ToString();
                playerName = itemSendLogMessage.Receiver.Name;
                locationName = itemSendLogMessage.Item.LocationName;
                if (locationName == null)
                    locationName = itemSendLogMessage.Item.LocationId.ToString();
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
                icon = SetSprite(itemSendLogMessage.Item.ItemGame, itemSendLogMessage.Item.ItemName, itemSendLogMessage.Item.ItemId, itemSendLogMessage.Item.Flags);
                break;
            default:
                return;
        }
        var notification = new APNotification(isHint, itemName, playerName, senderName, locationName, itemFlags, duration, backgroundColor, durationColor, icon, hintState);
        NotificationManager.AddNewNotification.Enqueue(notification);
    }

    private static Sprite SetSprite(string itemGame, string itemName, long itemId, ItemFlags itemFlag)
    {
        Sprite sprite = null;
        switch (itemGame)
        {
            case "Here Comes Niko!":
                sprite = itemId switch
                {
                    ItemID.Coin => Plugin.CoinSprite,
                    ItemID.Cassette => Plugin.CassetteSprite,
                    ItemID.Key => Plugin.KeySprite,
                    ItemID.SuperJump => Plugin.SuperJumpSprite,
                    ItemID.Letter => Plugin.LetterSprite,
                    ItemID.SnailMoney => Plugin.SnailMoneySprite,
                    ItemID.Bugs => Plugin.BugsSprite,
                    ItemID.Apples => Plugin.ApplesSprite,
                    ItemID.ContactList1 or ItemID.ContactList2 or ItemID.ProgressiveContactList => Plugin.ContactListSprite,
                    ItemID.GarysGardenTicket => Plugin.GgSprite,
                    ItemID.HairballCityTicket => Plugin.HcSprite,
                    ItemID.TurbineTownTicket => Plugin.TtSprite,
                    ItemID.SalmonCreekForestTicket => Plugin.SfcSprite,
                    ItemID.PublicPoolTicket => Plugin.PpSprite,
                    ItemID.BathhouseTicket => Plugin.BathSprite,
                    ItemID.TadpoleHqTicket => Plugin.HqSprite,
                    ItemID.HairballCityFish => Plugin.HairballFishSprite,
                    ItemID.TurbineTownFish => Plugin.TurbineFishSprite,
                    ItemID.SalmonCreekForestFish => Plugin.SalmonFishSprite,
                    ItemID.PublicPoolFish => Plugin.PoolFishSprite,
                    ItemID.BathhouseFish => Plugin.BathFishSprite,
                    ItemID.TadpoleHqFish => Plugin.TadpoleFishSprite,
                    ItemID.HairballCityKey => Plugin.HairballKeySprite,
                    ItemID.TurbineTownKey => Plugin.TurbineKeySprite,
                    ItemID.SalmonCreekForestKey => Plugin.SalmonKeySprite,
                    ItemID.PublicPoolKey => Plugin.PoolKeySprite,
                    ItemID.BathhouseKey => Plugin.BathKeySprite,
                    ItemID.TadpoleHqKey => Plugin.TadpoleKeySprite,
                    ItemID.HairballCityFlower => Plugin.HairballFlowerSprite,
                    ItemID.TurbineTownFlower => Plugin.TurbineFlowerSprite,
                    ItemID.SalmonCreekForestFlower => Plugin.SalmonFlowerSprite,
                    ItemID.PublicPoolFlower => Plugin.PoolFlowerSprite,
                    ItemID.BathhouseFlower => Plugin.BathFlowerSprite,
                    ItemID.TadpoleHqFlower => Plugin.TadpoleFlowerSprite,
                    ItemID.HairballCityCassette => Plugin.HairballCassetteSprite,
                    ItemID.TurbineTownCassette => Plugin.TurbineCassetteSprite,
                    ItemID.SalmonCreekForestCassette => Plugin.SalmonCassetteSprite,
                    ItemID.PublicPoolCassette => Plugin.PoolCassetteSprite,
                    ItemID.BathhouseCassette => Plugin.BathCassetteSprite,
                    ItemID.TadpoleHqCassette => Plugin.TadpoleCassetteSprite,
                    ItemID.GarysGardenCassette => Plugin.GardenCassetteSprite,
                    ItemID.HairballCitySeed => Plugin.HairballSeedSprite,
                    ItemID.SalmonCreekForestSeed => Plugin.SalmonSeedSprite,
                    ItemID.BathhouseSeed => Plugin.BathSeedSprite,
                    ItemID.FreezeTrap => Plugin.FreezeTrapSprite,
                    ItemID.IronBootsTrap => Plugin.IronBootsTrapSprite,
                    ItemID.WhoopsTrap => Plugin.WhoopsTrapSprite,
                    ItemID.MyTurnTrap => Plugin.MyTurnTrapSprite,
                    ItemID.SpeedBoost => Plugin.SpeedBoostSprite,
                    ItemID.HomeTrap => Plugin.HomeTrapSprite,
                    ItemID.WideTrap => Plugin.WideTrapSprite,
                    ItemID.PhoneTrap => Plugin.PhoneCallTrapSprite,
                    ItemID.TinyTrap => Plugin.TinyTrapSprite,
                    ItemID.GravityTrap => Plugin.GravityTrapSprite,
                    ItemID.JumpingJacksTrap => Plugin.JumpingJacksTrapSprite,
                    ItemID.PartyInvitation => Plugin.PartyTicketSprite,
                    ItemID.SafetyHelmet => Plugin.BonkHelmetSprite,
                    ItemID.BugNet => Plugin.BugNetSprite,
                    ItemID.SodaRepair => Plugin.SodaRepairSprite,
                    ItemID.ParasolRepair => Plugin.ParasolRepairSprite,
                    ItemID.SwimCourse => Plugin.SwimCourseSprite,
                    ItemID.Textbox => Plugin.TextboxItemSprite,
                    ItemID.AcRepair => Plugin.ACRepairSprite,
                    ItemID.AppleBasket => Plugin.AppleBasketSprite,
                    ItemID.HairballCityBone => Plugin.HairballBoneSprite,
                    ItemID.TurbineTownBone => Plugin.TurbineBoneSprite,
                    ItemID.SalmonCreekForestBone => Plugin.SalmonBoneSprite,
                    ItemID.PublicPoolBone => Plugin.PoolBoneSprite,
                    ItemID.BathhouseBone => Plugin.BathBoneSprite,
                    ItemID.TadpoleHqBone => Plugin.TadpoleBoneSprite,
                    _ => Plugin.ApProgressionSprite
                };
                break;
            case "A Hat in Time":
                if (itemName == "Time Piece")
                    sprite = Plugin.TimePieceSprite;
                if (itemName == "Yarn")
                    sprite = Plugin.YarnSprite;
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
                break;
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
        return sprite;
    }
}