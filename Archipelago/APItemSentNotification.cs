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
    public static bool HintCommand;
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
                if ((message.ToString().Contains("(avoid)") || hintItemSendLogMessage.IsFound) && HintCommand)
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
                    itemName = "Item: "+hintItemSendLogMessage.Item.ItemId;
                playerName = hintItemSendLogMessage.Receiver.Name;
                senderName = hintItemSendLogMessage.Sender.Name;
                locationName = hintItemSendLogMessage.Item.LocationName;
                if (locationName == null)
                    locationName = "Location: "+hintItemSendLogMessage.Item.LocationId;
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
                if (itemSendLogMessage.IsReceiverTheActivePlayer && !SavedData.Instance.NotificationShowSelfSent)
                {
                    Plugin.BepinLogger.LogInfo("Skipped Notification for Send Log Message: " + message + "");
                    return;
                }
                itemName = itemSendLogMessage.Item.ItemName;
                if (itemName == null)
                    itemName = "Item: "+itemSendLogMessage.Item.ItemId;
                playerName = itemSendLogMessage.Receiver.Name;
                locationName = itemSendLogMessage.Item.LocationName;
                if (locationName == null)
                    locationName = "Location: "+itemSendLogMessage.Item.LocationId;
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
                    if (!SavedData.Instance.NotificationShowJunk)
                    {
                        Plugin.BepinLogger.LogInfo("Skipped Notification for Send Log Message: " + message + "");
                        return;
                    }
                    backgroundColor = new Color(1, 0.75f, 0.41f, 0.72f);
                } else if (itemFlags.HasFlag(ItemFlags.None))
                {
                    if (!SavedData.Instance.NotificationShowJunk)
                    {
                        Plugin.BepinLogger.LogInfo("Skipped Notification for Send Log Message: " + message + "");
                        return;
                    }
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
                sprite = Assets.SetSprite(itemId);
                break;
            case "A Hat in Time":
                if (itemName == "Time Piece")
                    sprite = Assets.TimePieceSprite;
                if (itemName == "Yarn")
                    sprite = Assets.YarnSprite;
                if (itemFlag.HasFlag(ItemFlags.Advancement))
                {
                    sprite = Assets.ApProgressionSprite;
                }
                else if (itemFlag.HasFlag(ItemFlags.NeverExclude))
                {
                    sprite = Assets.ApUsefulSprite;
                }
                else if (itemFlag.HasFlag(ItemFlags.Trap))
                {
                    var trapSprites = new[]
                    {
                        Assets.ApTrapSprite,
                        Assets.ApTrap2Sprite,
                        Assets.ApTrap3Sprite
                    };
                    var randomIndex = Random.Range(0, trapSprites.Length);
                    sprite = trapSprites[randomIndex];
                }
                else if (itemFlag.HasFlag(ItemFlags.None))
                {
                    sprite = Assets.ApFillerSprite;
                }
                break;
            default:
            {
                if (itemFlag.HasFlag(ItemFlags.Advancement))
                {
                    sprite = Assets.ApProgressionSprite;
                }
                else if (itemFlag.HasFlag(ItemFlags.NeverExclude))
                {
                    sprite = Assets.ApUsefulSprite;
                }
                else if (itemFlag.HasFlag(ItemFlags.Trap))
                {
                    var trapSprites = new[]
                    {
                        Assets.ApTrapSprite,
                        Assets.ApTrap2Sprite,
                        Assets.ApTrap3Sprite
                    };
                    var randomIndex = Random.Range(0, trapSprites.Length);
                    sprite = trapSprites[randomIndex];
                }
                else if (itemFlag.HasFlag(ItemFlags.None))
                {
                    sprite = Assets.ApFillerSprite;
                }
                else
                {
                    sprite = Assets.ApProgressionSprite;
                }
                break;
            }
        }
        return sprite;
    }
}