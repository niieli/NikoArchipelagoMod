﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Archipelago;

public class ArchipelagoClient
{
    public const string APVersion = "0.5.0";
    private const string Game = "Here Comes Niko!";

    public static bool Authenticated;
    private bool attemptingConnection;

    public static ArchipelagoData ServerData = new();
    private DeathLinkHandler deathLinkHandler;
    public static ArchipelagoSession _session;
    public int CoinAmount, CassetteAmount, KeyAmount, HcKeyAmount, TtKeyAmount, SfcKeyAmount, PpKeyAmount, BathKeyAmount, HqKeyAmount,
        HcFishAmount, TtFishAmount, SfcFishAmount, PpFishAmount, BathFishAmount, HqFishAmount;
    public static bool SuperJump, Ticket1, Ticket2, Ticket3, Ticket4, Ticket5, Ticket6, TicketGary;
    public Task _disconnectTask;

    /// <summary>
    /// call to connect to an Archipelago session. Connection info should already be set up on ServerData
    /// </summary>
    /// <returns></returns>
    public void Connect()
    {
        if (Authenticated || attemptingConnection) return;

        try
        {
            _session = ArchipelagoSessionFactory.CreateSession(ServerData.Uri);
            SetupSession();
        }
        catch (Exception e)
        {
            Plugin.BepinLogger.LogError(e);
        }
        TryConnect();
    }

    /// <summary>
    /// add handlers for Archipelago events
    /// </summary>
    private void SetupSession()
    {
        _session.MessageLog.OnMessageReceived += message => ArchipelagoConsole.LogMessage(message.ToString());
        _session.MessageLog.OnMessageReceived += message => APItemSentNotification.SentItem(message);
        _session.Items.ItemReceived += OnItemReceived;
        _session.Socket.ErrorReceived += OnSessionErrorReceived;
        _session.Socket.SocketClosed += OnSessionSocketClosed;
    }

    /// <summary>
    /// attempt to connect to the server with our connection info
    /// </summary>
    private void TryConnect()
    {
        try
        {
            // it's safe to thread this function call but unity notoriously hates threading so do not use excessively
            ThreadPool.QueueUserWorkItem(
                _ => HandleConnectResult(
                    _session.TryConnectAndLogin(
                        Game,
                        ServerData.SlotName,
                        ItemsHandlingFlags.AllItems,
                        new Version(APVersion),
                        uuid: null,
                        password: ServerData.Password,
                        requestSlotData: true // ServerData.NeedSlotData
                    )));
        }
        catch (Exception e)
        {
            Plugin.BepinLogger.LogError(e);
            HandleConnectResult(new LoginFailure(e.ToString()));
            attemptingConnection = false;
        }
    }

    /// <summary>
    /// handle the connection result and do things
    /// </summary>
    /// <param name="result"></param>
    private void HandleConnectResult(LoginResult result)
    {
        string outText;
        if (result.Successful)
        {
            var success = (LoginSuccessful)result;

            ServerData.SetupSession(success.SlotData, _session.RoomState.Seed);
            Authenticated = true;
            deathLinkHandler = new(_session.CreateDeathLinkService(), ServerData.SlotName);
#if NET35
            session.Locations.CompleteLocationChecksAsync(null, ServerData.CheckedLocations.ToArray());
#else
            _session.Locations.CompleteLocationChecksAsync(ServerData.CheckedLocations.ToArray());
            Scout();
#endif
            outText = $"Successfully connected to {ServerData.Uri} as {ServerData.SlotName}!";

            ArchipelagoConsole.LogMessage(outText);
        }
        else
        {
            var failure = (LoginFailure)result;
            outText = $"Failed to connect to {ServerData.Uri} as {ServerData.SlotName}.";
            outText = failure.Errors.Aggregate(outText, (current, error) => current + $"\n    {error}");

            Plugin.BepinLogger.LogError(outText);

            Authenticated = false;
            Disconnect();
        }

        ArchipelagoConsole.LogMessage(outText);
        attemptingConnection = false;
    }

    /// <summary>
    /// something we wrong or we need to properly disconnect from the server. cleanup and re null our session
    /// </summary>
    public static void Disconnect()
    {
        Plugin.BepinLogger.LogDebug("disconnecting from server...");
        try
        {
#if NET35
        session?.Socket.Disconnect();
#else
            _session?.Socket.DisconnectAsync();
#endif
            _session = null;
            Authenticated = false;
        }
        catch (Exception e)
        {
            Plugin.BepinLogger.LogError($"Error during disconnection: {e.Message}");
        }
    }


    public void SendMessage(string message)
    {
        _session.Socket.SendPacketAsync(new SayPacket { Text = message });
    }
    
    public List<ItemInfo> queuedItems = [];
    public List<ItemInfo> queuedItems2 = [];
    private static readonly string[] validScenes = ["Public Pool", "Hairball City", "Salmon Creek Forest", "Trash Kingdom", "Tadpole inc", "Home", "The Bathhouse", "GarysGarden"];

    /// <summary>
    /// we received an item so reward it here
    /// </summary>
    /// <param name="helper">item helper which we can grab our item from</param>
    private void OnItemReceived(ReceivedItemsHelper helper)
    {
        var receivedItem = helper.DequeueItem();
        
        if (helper.Index < ServerData.Index) return;

        ServerData.Index++;
        if (IsValidScene() && Plugin.loggedIn)
        {
            GiveItem(receivedItem);
        }
        else
        {
            queuedItems.Add(receivedItem);
            Plugin.BepinLogger.LogInfo($"Added Item '{receivedItem.ItemName}' , ID:'{receivedItem.ItemId}' to queue");
        }
    }

    public void GiveItem(ItemInfo item, bool notify = true)
    {
        var senderName = _session.Players.GetPlayerName(item.Player);
        switch (item.ItemId)
            {
                case 598_145_444_000:
                    ItemHandler.AddCoin(1, senderName, notify);
                    CoinAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Coin");
                    break;
                case 598_145_444_000 + 1: // Cassette
                    ItemHandler.AddCassette(1, senderName, notify);
                    CassetteAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Cassette");
                    break;
                case 598_145_444_000 + 2: // Key
                    ItemHandler.AddKey(1, senderName, notify);
                    KeyAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Key");
                    break;
                case 598_145_444_000 + 3: // Apples
                    ItemHandler.AddApples(25, senderName, notify);
                    break;
                case 598_145_444_000 + 4: // Contact List 1
                    ItemHandler.AddContactList1(senderName, notify);
                    break;
                case 598_145_444_000 + 5: // Contact List 2
                    ItemHandler.AddContactList2(senderName, notify);
                    break;
                case 598_145_444_000 + 6: // Super Jump
                    ItemHandler.AddSuperJump(senderName, notify);
                    SuperJump = _session.Items.AllItemsReceived.Contains(item);
                    break;
                case 598_145_444_000+7:
                    ItemHandler.AddLetter(1, senderName, notify);
                    break;
                case 598_145_444_000+8:
                    ItemHandler.AddTicket(2, senderName, notify);
                    Ticket1 = true;
                    break;
                case 598_145_444_000+9:
                    ItemHandler.AddTicket(3, senderName, notify);
                    Ticket2 = true;
                    break;
                case 598_145_444_000+10:
                    ItemHandler.AddTicket(4, senderName, notify);
                    Ticket3 = true;
                    break;
                case 598_145_444_000+11:
                    ItemHandler.AddTicket(5, senderName, notify);
                    Ticket4 = true;
                    break;
                case 598_145_444_000+12:
                    ItemHandler.AddTicket(6, senderName, notify);
                    Ticket5 = true;
                    break;
                case 598_145_444_000+13:
                    ItemHandler.AddTicket(7, senderName, notify);
                    Ticket6 = true;
                    break;
                case 598_145_444_000+17:
                    ItemHandler.AddGarden(senderName, notify);
                    TicketGary = true;
                    break;
                case 598_145_444_000+14:
                    ItemHandler.AddBugs(10, senderName, notify);
                    break;
                case 598_145_444_000+16:
                    ItemHandler.AddMoney(1000, senderName, notify);
                    break;
                case 598_145_444_000+15:
                    var real = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Progressive Contact List");
                    if (real == 2)
                    {
                        ItemHandler.AddContactList2(senderName, notify);
                    }
                    ItemHandler.AddContactList1(senderName, notify);
                    break;
                case 598_145_444_000+20: // HCFish
                    ItemHandler.AddHcFish(senderName, notify);
                    HcFishAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Hairball City Fish");
                    break;
                case 598_145_444_000+21: // TTFish
                    ItemHandler.AddTtFish(senderName, notify);
                    TtFishAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Turbine Town Fish");
                    break;
                case 598_145_444_000+22: // SFCFish
                    ItemHandler.AddSfcFish(senderName, notify);
                    SfcFishAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Salmon Creek Forest Fish");
                    break;
                case 598_145_444_000+23: // PPFish
                    ItemHandler.AddPpFish(senderName, notify);
                    PpFishAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Public Pool Fish");
                    break;
                case 598_145_444_000+24: // BathFish
                    ItemHandler.AddBathFish(senderName, notify);
                    BathFishAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Bathhouse Fish");
                    break;
                case 598_145_444_000+25: // HQFish
                    ItemHandler.AddHqFish(senderName, notify);
                    HqFishAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Tadpole HQ Fish");
                    break;
                case 598_145_444_000+30: // HCKey
                    ItemHandler.AddHCKey(senderName, notify);
                    HcKeyAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Hairball City Key");
                    break;
                case 598_145_444_000+31: // TTKey
                    ItemHandler.AddTTKey(senderName, notify);
                    TtKeyAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Turbine Town Key");
                    break;
                case 598_145_444_000+32: // SFCKey
                    ItemHandler.AddSFCKey(senderName, notify);
                    SfcKeyAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Salmon Creek Forest Key");
                    break;
                case 598_145_444_000+33: // PPKey
                    ItemHandler.AddPPKey(senderName, notify);
                    PpKeyAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Public Pool Key");
                    break;
                case 598_145_444_000+34: // BathKey
                    ItemHandler.AddBathKey(senderName, notify);
                    BathKeyAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Bathhouse Key");
                    break;
                case 598_145_444_000+35: // HQKey
                    ItemHandler.AddHQKey(senderName, notify);
                    HqKeyAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Tadpole HQ Key");
                    break;
            }
    }

    public void CheckReceivedItems()
    {
        foreach (var item in _session.Items.AllItemsReceived)
        {
            if (IsValidScene())
            {
                GiveItem(item, false);
            }
            else
            {
                queuedItems2.Add(item);
            }
        }
    }
    
    public static bool IsValidScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        foreach (var validScene in validScenes)
        {
            if (currentScene == validScene)
            {
                return true;
            }
        }
        return false;
    }

    public static void CheckLocationState()
    {
        var checkedLocations = _session.Locations.AllLocationsChecked;
        if (ServerData.CheckedLocations.Count == checkedLocations.Count) return;
    }

    public static int TicketCount()
    {
        var count = 0;
        if (Ticket1) count++;
        if (Ticket2) count++;
        if (Ticket3) count++;
        if (Ticket4) count++;
        if (Ticket5) count++;
        if (Ticket6) count++;
        if (TicketGary) count++;
        return count;
    }
    
    public static readonly List<ScoutedItemInfo> ScoutedLocations = [];
    private static void Scout()
    {
        _session.Locations.ScoutLocationsAsync(Locations.ScoutIDs).ContinueWith(locationInfoPacket => {
            foreach (var itemInfo in locationInfoPacket.Result.Values) {
                ScoutedLocations.Add(itemInfo);
            }
        });
        Plugin.BepinLogger.LogInfo("Scouted locations.");
    }
    
    public static void SendCompletion()
    {
        _session.SetGoalAchieved();
        var statusUpdatePacket = new StatusUpdatePacket();
        statusUpdatePacket.Status = ArchipelagoClientState.ClientGoal;
        _session.Socket.SendPacket(statusUpdatePacket);
    }

    public static void OnLocationChecked(long locationId)
    {
        _session.Locations.CompleteLocationChecks(locationId);
    }

    /// <summary>
    /// something went wrong with our socket connection
    /// </summary>
    /// <param name="e">thrown exception from our socket</param>
    /// <param name="message">message received from the server</param>
    private void OnSessionErrorReceived(Exception e, string message)
    {
        Plugin.BepinLogger.LogError(e);
        ArchipelagoConsole.LogMessage(message);
    }

    /// <summary>
    /// something went wrong closing our connection. disconnect and clean up
    /// </summary>
    /// <param name="reason"></param>
    private void OnSessionSocketClosed(string reason)
    {
        Plugin.BepinLogger.LogError($"Connection to Archipelago lost: {reason}");
        Disconnect();
    }
}