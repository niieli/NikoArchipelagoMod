using System;
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
    public int CoinAmount, CassetteAmount, KeyAmount;
    public bool SuperJump, ContactList1, ContactList2, Ticket1, Ticket2, Ticket3, Ticket4, Ticket5, Ticket6, isRunning;
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
            isRunning = true;
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
    public void Disconnect()
    {
        Plugin.BepinLogger.LogDebug("disconnecting from server...");
#if NET35
        session?.Socket.Disconnect();
#else
        if (_session != null && _session.Socket != null)
        {
            // Store the disconnect async task
            _disconnectTask = _session.Socket.DisconnectAsync();
        }
#endif
        _session = null;
        Authenticated = false;
        isRunning = false;
    }

    public void SendMessage(string message)
    {
        _session.Socket.SendPacketAsync(new SayPacket { Text = message });
    }

    public static ItemInfo SyncInventory()
    {
        foreach(ItemInfo item in _session.Items.AllItemsReceived)
        {
            return item;
        }
        return null;
    }
    
    public List<ItemInfo> queuedItems = [];
    private readonly string[] validScenes = ["Public Pool", "Hairball City", "Salmon Creek Forest", "Trash Kingdom", "Tadpole inc", "Home", "The Bathhouse", "GarysGarden"];
    
    /// <summary>
    /// we received an item so reward it here
    /// </summary>
    /// <param name="helper">item helper which we can grab our item from</param>
    private void OnItemReceived(ReceivedItemsHelper helper)
    {
        var receivedItem = helper.DequeueItem();
        
        if (helper.Index < ServerData.Index) return;

        ServerData.Index++;
        
        if (IsValidScene())
        {
            GiveItem(receivedItem);
        }
        else
        {
            queuedItems.Add(receivedItem);
            Plugin.BepinLogger.LogInfo($"Added Item '{receivedItem.ItemName}' , ID:'{receivedItem.ItemId}' to queue");
        }
    }

    public void GiveItem(ItemInfo item)
    {
        var senderName = _session.Players.GetPlayerName(item.Player);
        switch (item.ItemId)
            {
                case 598_145_444_000:
                    ItemHandler.AddCoin(1, senderName);
                    CoinAmount++;
                    break;
                case 598_145_444_000 + 1: // Cassette
                    ItemHandler.AddCassette(1, senderName);
                    CassetteAmount++;
                    break;
                case 598_145_444_000 + 2: // Key
                    ItemHandler.AddKey(1, senderName);
                    KeyAmount++;
                    break;
                case 598_145_444_000 + 3: // Apples
                    ItemHandler.AddApples(25, senderName);
                    break;
                case 598_145_444_000 + 4: // Contact List 1
                    ItemHandler.AddContactList1(senderName);
                    ContactList1 = true;
                    break;
                case 598_145_444_000 + 5: // Contact List 2
                    ItemHandler.AddContactList2(senderName);
                    ContactList2 = true;
                    break;
                case 598_145_444_000 + 6: // Super Jump
                    ItemHandler.AddSuperJump(senderName);
                    SuperJump = true;
                    break;
                case 598_145_444_000+7:
                    ItemHandler.AddLetter(1, senderName);
                    break;
                case 598_145_444_000+8:
                    ItemHandler.AddTicket(2, senderName);
                    Ticket1 = true;
                    break;
                case 598_145_444_000+9:
                    ItemHandler.AddTicket(3, senderName);
                    Ticket2 = true;
                    break;
                case 598_145_444_000+10:
                    ItemHandler.AddTicket(4, senderName);
                    Ticket3 = true;
                    break;
                case 598_145_444_000+11:
                    ItemHandler.AddTicket(5, senderName);
                    Ticket4 = true;
                    break;
                case 598_145_444_000+12:
                    ItemHandler.AddTicket(6, senderName);
                    Ticket5 = true;
                    break;
                case 598_145_444_000+13:
                    ItemHandler.AddTicket(7, senderName);
                    Ticket6 = true;
                    break;
                case 598_145_444_000+14:
                    ItemHandler.AddBugs(10, senderName);
                    break;
                case 598_145_444_000+15:
                    ItemHandler.AddProgressiveContactList(senderName);
                    if (!ContactList2 && ContactList1)
                    {
                        ContactList2 = true;
                    }
                    else
                    {
                        ContactList1 = true;
                    }
                    break;
            }
    }
    
    public bool IsValidScene()
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