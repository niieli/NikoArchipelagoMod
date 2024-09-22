using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Packets;

namespace NikoArchipelago.Archipelago;

public class ArchipelagoClient
{
    public const string APVersion = "0.5.0";
    private const string Game = "Here Comes Niko!";

    public static bool Authenticated;
    private bool attemptingConnection;

    public static ArchipelagoData ServerData = new();
    private DeathLinkHandler DeathLinkHandler;
    private static ArchipelagoSession session;
    public int coinAmount, cassetteAmount, keyAmount;
    public bool superJump, contactList1, contactList2, ticket1, ticket2, ticket3, ticket4, ticket5, ticket6;

    /// <summary>
    /// call to connect to an Archipelago session. Connection info should already be set up on ServerData
    /// </summary>
    /// <returns></returns>
    public void Connect()
    {
        if (Authenticated || attemptingConnection) return;

        try
        {
            session = ArchipelagoSessionFactory.CreateSession(ServerData.Uri);
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
        session.MessageLog.OnMessageReceived += message => ArchipelagoConsole.LogMessage(message.ToString());
        session.Items.ItemReceived += OnItemReceived;
        session.Socket.ErrorReceived += OnSessionErrorReceived;
        session.Socket.SocketClosed += OnSessionSocketClosed;
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
                    session.TryConnectAndLogin(
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

            ServerData.SetupSession(success.SlotData, session.RoomState.Seed);
            Authenticated = true;

            DeathLinkHandler = new(session.CreateDeathLinkService(), ServerData.SlotName);
#if NET35
            session.Locations.CompleteLocationChecksAsync(null, ServerData.CheckedLocations.ToArray());
#else
            session.Locations.CompleteLocationChecksAsync(ServerData.CheckedLocations.ToArray());
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
        session?.Socket.DisconnectAsync();
#endif
        session = null;
        Authenticated = false;
    }

    public void SendMessage(string message)
    {
        session.Socket.SendPacketAsync(new SayPacket { Text = message });
    }

    
    /// <summary>
    /// Synchronizes all item data from Archipelago's DataStorage when the player logs in.
    /// </summary>
    public static async Task SyncItemsFromDataStorage()
    {
        int coinAmount = await GetItemFromStorage("TotalCoins");
        int cassetteAmount = await GetItemFromStorage("TotalCassettes");
        int keyAmount = await GetItemFromStorage("TotalKeys");
        int appleAmount = await GetItemFromStorage("TotalApples");
        //int hasSuperJump = await GetItemFromStorage("SuperJump");
        //int hasContactList1 = await GetItemFromStorage("ContactList1");
        //int hasContactList2 = await GetItemFromStorage("ContactList2");

        // Apply these values to your game data
        if (scrGameSaveManager.instance.gameData.generalGameData.coinAmount > coinAmount)
        {
            scrGameSaveManager.instance.gameData.generalGameData.coinAmount = coinAmount;
        }
        if (scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount > cassetteAmount)
        {
            scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount = cassetteAmount;
        }
        if (scrGameSaveManager.instance.gameData.generalGameData.keyAmount > keyAmount)
        {
            scrGameSaveManager.instance.gameData.generalGameData.keyAmount = keyAmount;
        }
        //scrGameSaveManager.instance.gameData.generalGameData.secretMove = scrGameSaveManager.instance.gameData.generalGameData.secretMove != 1.Equals(hasSuperJump) && hasSuperJump.Equals(1);
        //scrGameSaveManager.instance.gameData.generalGameData.wave1 = scrGameSaveManager.instance.gameData.generalGameData.wave1 != 1.Equals(hasContactList1);
        //scrGameSaveManager.instance.gameData.generalGameData.wave2 = scrGameSaveManager.instance.gameData.generalGameData.wave2 != 1.Equals(hasContactList2);
        //scrGameSaveManager.instance.gameData.generalGameData.appleAmount += appleAmount;
        Plugin.BepinLogger.LogInfo("Sync items from data storage finished.");
    }

    /// <summary>
    /// Retrieves an item count from Archipelago's DataStorage.
    /// </summary>
    /// <param name="key">The key used to store the item count in DataStorage.</param>
    /// <returns>The stored value, or 0 if no value exists.</returns>
    private static async Task<int> GetItemFromStorage(string key)
    {
        var value = await session.DataStorage[key].GetAsync();
        return value?.ToObject<int>() ?? 0;
    }

    /// <summary>
    /// Stores an updated item count to Archipelago's DataStorage.
    /// </summary>
    private static Task StoreItemToStorage(string key, int value)
    {
        session.DataStorage[key].Initialize(value);
        return Task.CompletedTask;
    }
    
    /// <summary>
    /// we received an item so reward it here
    /// </summary>
    /// <param name="helper">item helper which we can grab our item from</param>
    private async void OnItemReceived(ReceivedItemsHelper helper)
    {
        var receivedItem = helper.DequeueItem();
        
        if (helper.Index < ServerData.Index) return;

        ServerData.Index++;
        
        var senderName = session.Players.GetPlayerName(receivedItem.Player);
        
        switch (receivedItem.ItemId)
        {
            case 598_145_444_000:
                ItemHandler.AddCoin(1, senderName);
                coinAmount++;
                await StoreItemToStorage("TotalCoins", scrGameSaveManager.instance.gameData.generalGameData.coinAmount);
                break;
            case 598_145_444_000 + 1: // Cassette
                ItemHandler.AddCassette(1, senderName);
                cassetteAmount++;
                await StoreItemToStorage("TotalCassettes", scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount);
                break;
            case 598_145_444_000 + 2: // Key
                ItemHandler.AddKey(1, senderName);
                keyAmount++;
                await StoreItemToStorage("TotalKeys", scrGameSaveManager.instance.gameData.generalGameData.keyAmount);
                break;
            case 598_145_444_000 + 3: // Apples
                ItemHandler.AddApples(25, senderName);
                await StoreItemToStorage("TotalApples", scrGameSaveManager.instance.gameData.generalGameData.appleAmount);
                break;
            case 598_145_444_000 + 4: // Contact List 1
                ItemHandler.AddContactList1(senderName);
                contactList1 = true;
                await StoreItemToStorage("ContactList1", scrGameSaveManager.instance.gameData.generalGameData.wave1 ? 1 : 0);
                break;
            case 598_145_444_000 + 5: // Contact List 2
                ItemHandler.AddContactList2(senderName);
                contactList2 = true;
                await StoreItemToStorage("ContactList2", scrGameSaveManager.instance.gameData.generalGameData.wave2 ? 1 : 0);
                break;
            case 598_145_444_000 + 6: // Super Jump
                ItemHandler.AddSuperJump(senderName);
                superJump = true;
                await StoreItemToStorage("SuperJump", scrGameSaveManager.instance.gameData.generalGameData.secretMove ? 1 : 0);
                break;
            case 598_145_444_000+7:
                ItemHandler.AddLetter(1, senderName);
                break;
            case 598_145_444_000+8:
                ItemHandler.AddTicket(2, senderName);
                ticket1 = true;
                break;
            case 598_145_444_000+9:
                ItemHandler.AddTicket(3, senderName);
                ticket2 = true;
                break;
            case 598_145_444_000+10:
                ItemHandler.AddTicket(4, senderName);
                ticket3 = true;
                break;
            case 598_145_444_000+11:
                ItemHandler.AddTicket(5, senderName);
                ticket4 = true;
                break;
            case 598_145_444_000+12:
                ItemHandler.AddTicket(6, senderName);
                ticket5 = true;
                break;
            case 598_145_444_000+13:
                ItemHandler.AddTicket(7, senderName);
                ticket6 = true;
                break;
        }
        // if items can be received while in an invalid state for actually handling them, they can be placed in a local
        // queue to be handled later
    }

    public static void SendCompletion()
    {
        var statusUpdatePacket = new StatusUpdatePacket();
        statusUpdatePacket.Status = ArchipelagoClientState.ClientGoal;
        session.Socket.SendPacket(statusUpdatePacket);
    }

    public static void OnLocationChecked(long locationId)
    {
        session.Locations.CompleteLocationChecks(locationId);
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