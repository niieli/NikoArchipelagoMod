using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Converters;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Exceptions;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using Newtonsoft.Json.Linq;
using NikoArchipelago.Patches;
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
        HcFishAmount, TtFishAmount, SfcFishAmount, PpFishAmount, BathFishAmount, HqFishAmount,
        HcSeedAmount, SfcSeedAmount, BathSeedAmount,
        HcFlowerAmount, TtFlowerAmount, SfcFlowerAmount, PpFlowerAmount, BathFlowerAmount, HqFlowerAmount,
        HcCassetteAmount, TtCassetteAmount, SfcCassetteAmount, PpCassetteAmount, BathCassetteAmount, HqCassetteAmount, GgCassetteAmount,
        HcBoneAmount, TtBoneAmount, SfcBoneAmount, PpBoneAmount, BathBoneAmount, HqBoneAmount, GarysGardenSeedAmount;

    public static int SpeedBoostAmount;
    public static bool SuperJump, Ticket1, Ticket2, Ticket3, Ticket4, Ticket5, Ticket6, TicketGary, TicketParty,
        HcNPCs, TtNPCs, SfcNPCs, PpNPCs, BathNPCs, HqNPCs, Keysanity, ElevatorRepaired, 
        BonkPermitAcquired, BugnetAcquired, SodaRepairAcquired, ParasolRepairAcquired, SwimmingAcquired, TextboxAcquired, acRepairAcquired, AppleBasketAcquired,
        HomeTextboxAcquired, HairballTextboxAcquired, TurbineTextboxAcquired, SalmonTextboxAcquired, 
        PoolTextboxAcquired, BathTextboxAcquired, TadpoleTextboxAcquired, GardenTextboxAcquired;

    private static int savedItemIndex;
    private static bool stopIt;
    public Task _disconnectTask;
    public static bool TrapLink { get; private set; }
    public static bool RingLink { get; private set; }

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
        _session.MessageLog.OnMessageReceived += OnMessageReceived;
        _session.Items.ItemReceived += OnItemReceived;
        _session.Socket.ErrorReceived += OnSessionErrorReceived;
        _session.Socket.SocketClosed += OnSessionSocketClosed;
        _session.Socket.PacketReceived += OnPacketReceived;
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
            ArchipelagoMenu.pressedConnect = false;
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
            deathLinkHandler = new(_session.CreateDeathLinkService(), ServerData.SlotName, IsDeathLink());
            if (IsTrapLink())
                ToggleTrapLink();
#if NET35
            session.Locations.CompleteLocationChecksAsync(null, ServerData.CheckedLocations.ToArray());
#else
            _session.Locations.CompleteLocationChecksAsync(ServerData.CheckedLocations.ToArray());
            Scout();
#endif
            outText = $"Successfully connected to {ServerData.Uri} as {ServerData.SlotName}!";

            ArchipelagoConsole.LogMessage(outText);
            Plugin.loggedIn = true;
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

    private bool IsDeathLink()
    {
        return int.Parse(ArchipelagoData.slotData["death_link"].ToString()) == 1;
    }

    private bool IsTrapLink()
    {
        if (ArchipelagoData.slotData.ContainsKey("trap_link"))
            return int.Parse(ArchipelagoData.slotData["trap_link"].ToString()) == 1;
        ArchipelagoConsole.LogMessage("apworld does not contain TrapLink.");
        return false;
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
            ArchipelagoMenu.pressedConnect = false;
        }
        catch (Exception e)
        {
            Plugin.BepinLogger.LogError($"Error during disconnection: {e.Message}");
        }
    }

    private void OnMessageReceived(LogMessage message)
    {
        ArchipelagoConsole.LogMessage(message.ToString());
        APItemSentNotification.SentItem(message);
    }

    private void OnPacketReceived(ArchipelagoPacketBase archipelagoPacketBase)
    {
        if (archipelagoPacketBase is BouncedPacket bouncedPacket && bouncedPacket.Tags.Contains("TrapLink"))
        {
            ReceivedTrapLink(bouncedPacket);
        }
        if (Plugin.DebugMode)
            Plugin.BepinLogger.LogInfo($"Received packet: {archipelagoPacketBase.GetType().Name}");
    }

    public static void ToggleTrapLink()
    {
        TrapLink = !TrapLink;
        if (TrapLink)
        {
            _session.ConnectionInfo.UpdateConnectionOptions(_session.ConnectionInfo.Tags.Concat(["TrapLink"]).ToArray());
        }
        else
        {
            _session.ConnectionInfo.UpdateConnectionOptions(_session.ConnectionInfo.Tags.Where(tag => tag != "TrapLink").ToArray());
        }
    }

    private void ReceivedTrapLink(BouncedPacket trapLinkPacket)
    {
        var time = DateTime.Now;
        var nameOfTrap = trapLinkPacket.Data["trap_name"].ToString();
        var source = trapLinkPacket.Data["source"].ToString();
        if (source == ServerData.SlotName) return;
        if (TrapManager.TrapLinkMapping.ContainsKey(nameOfTrap))
        {
            TrapManager.TrapLinkQueue.Enqueue((nameOfTrap, source, (DateTime)time));
        }
    }

    public static void SendTrapLink(string trapName)
    {
        BouncePacket trapLinkPacket = new BouncePacket
        {
            Tags = ["TrapLink"],
            Data = new Dictionary<string, JToken>
            {
                { "time", (float)DateTime.Now.ToUnixTimeStamp() },
                { "source", ServerData.SlotName },
                { "trap_name", trapName }
            }
        };
        _session.Socket.SendPacket(trapLinkPacket);
        Plugin.BepinLogger.LogInfo("Sent TrapLink!");
    }
    
    public static void ToggleRingLink()
    {
        RingLink = !RingLink;
        if (RingLink)
        {
            _session.ConnectionInfo.UpdateConnectionOptions(_session.ConnectionInfo.Tags.Concat(["RingLink"]).ToArray());
        }
        else
        {
            _session.ConnectionInfo.UpdateConnectionOptions(_session.ConnectionInfo.Tags.Where(tag => tag != "RingLink").ToArray());
        }
    }

    public void SendDeathLink(string cause)
    {
        deathLinkHandler.SendDeathLink(cause);
    }

    private void SendPacket()
    {
        
    }

    public static void SendMessage(string message)
    {
        _session.Socket.SendPacketAsync(new SayPacket { Text = message });
    }
    
    public List<ItemInfo> queuedItems = [];
    public List<ItemInfo> InvalidSceneItemQueue = [];
    private static readonly string[] validScenes = ["Public Pool", "Hairball City", "Salmon Creek Forest", "Trash Kingdom", "Tadpole inc", "Home", "The Bathhouse", "GarysGarden"];

    // Maybe useful for later or when I am bored and rework the whole scout thing (ScoutID Array)
    public static ScoutedItemInfo ScoutLocation(long locationID, bool hint = true)
    {
        var scout = _session.Locations.ScoutLocationsAsync(ArchipelagoMenu.Hints && hint ? HintCreationPolicy.CreateAndAnnounce : HintCreationPolicy.None, 598_145_444_000+locationID);
        return scout.Result[598_145_444_000+locationID];
    }
    
    /// <summary>
    /// we received an item so reward it here
    /// </summary>
    /// <param name="helper">item helper which we can grab our item from</param>
    private void OnItemReceived(ReceivedItemsHelper helper)
    {
        while (!Plugin.worldReady)
        {
            if (stopIt) continue;
            Plugin.BepinLogger.LogError("Waiting for save to finish loading...");
            stopIt = true;
        }
        var receivedItem = helper.DequeueItem();
        if (helper.Index <= ServerData.Index) return;
        ServerData.Index++;

        if (ServerData.Index <= GetItemIndex())
        {
            GameObjectChecker.LogPastItemsBatch.AppendLine("-------------------------------------------------")
                .AppendLine($"helper index: {helper.Index}")
                .AppendLine($"Server index: {ServerData.Index}")
                .AppendLine($"Flag index: {GetItemIndex()}")
                .AppendLine($"Current Item: {receivedItem.ItemName}");
            if (receivedItem.ItemId is ItemID.Apples or ItemID.Bugs or ItemID.Letter or ItemID.SnailMoney
                    or ItemID.FreezeTrap or ItemID.IronBootsTrap or ItemID.WhoopsTrap or ItemID.MyTurnTrap
                    or ItemID.GravityTrap or ItemID.HomeTrap or ItemID.WideTrap or ItemID.PhoneTrap or ItemID.TinyTrap or ItemID.JumpingJacksTrap
                    or ItemID.CameraStuckTrap or ItemID.InvertedCameraTrap or ItemID.ThereGoesNikoTrap)
            {
                GameObjectChecker.LogPastItemsBatch.AppendLine($"Skipping Item: {receivedItem.ItemName}");
            }
            else
            {
                GiveItem(receivedItem, false);
            }
            return;
        }
        
        savedItemIndex = GetItemIndex()+1;
        SaveItemIndex(savedItemIndex);
        
        if (IsValidScene() && Plugin.loggedIn && scrTransitionManager.instance.state == scrTransitionManager.States.idle)
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
                case ItemID.Coin:
                    ItemHandler.AddCoin(1, senderName, notify);
                    CoinAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.Coin);
                    break;
                case ItemID.Cassette: // Cassette
                    ItemHandler.AddCassette(1, senderName, notify);
                    CassetteAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.Cassette);
                    break;
                case ItemID.Key: // Key
                    ItemHandler.AddKey(1, senderName, notify);
                    KeyAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.Key);
                    break;
                case ItemID.Apples: // Apples
                    ItemHandler.AddApples(25, senderName, notify);
                    break;
                case ItemID.ContactList1: // Contact List 1
                    ItemHandler.AddContactList1(senderName, notify);
                    break;
                case ItemID.ContactList2: // Contact List 2
                    ItemHandler.AddContactList2(senderName, notify);
                    break;
                case ItemID.SuperJump: // Super Jump
                    ItemHandler.AddSuperJump(senderName, notify);
                    SuperJump = _session.Items.AllItemsReceived.Contains(item);
                    break;
                case ItemID.Letter:
                    ItemHandler.AddLetter(1, senderName, notify);
                    break;
                case ItemID.HairballCityTicket:
                    ItemHandler.AddTicket(2, senderName, notify);
                    Ticket1 = true;
                    break;
                case ItemID.TurbineTownTicket:
                    ItemHandler.AddTicket(3, senderName, notify);
                    Ticket2 = true;
                    break;
                case ItemID.SalmonCreekForestTicket:
                    ItemHandler.AddTicket(4, senderName, notify);
                    Ticket3 = true;
                    break;
                case ItemID.PublicPoolTicket:
                    ItemHandler.AddTicket(5, senderName, notify);
                    Ticket4 = true;
                    break;
                case ItemID.BathhouseTicket:
                    ItemHandler.AddTicket(6, senderName, notify);
                    Ticket5 = true;
                    break;
                case ItemID.TadpoleHqTicket:
                    ItemHandler.AddTicket(7, senderName, notify);
                    Ticket6 = true;
                    break;
                case ItemID.GarysGardenTicket:
                    ItemHandler.AddGarden(senderName, notify);
                    TicketGary = true;
                    break;
                case ItemID.Bugs:
                    ItemHandler.AddBugs(10, senderName, notify);
                    break;
                case ItemID.SnailMoney:
                    ItemHandler.AddMoney(7500, senderName, notify);
                    break;
                case ItemID.ProgressiveContactList:
                    var real = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.ProgressiveContactList);
                    if (real == 2)
                    {
                        ItemHandler.AddContactList2(senderName, notify);
                    }
                    ItemHandler.AddContactList1(senderName, notify);
                    break;
                case ItemID.HairballCityFish: // HCFish
                    ItemHandler.AddItemNote(item, notify);
                    HcFishAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.HairballCityFish);
                    break;
                case ItemID.TurbineTownFish: // TTFish
                    ItemHandler.AddItemNote(item, notify);
                    TtFishAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.TurbineTownFish);
                    break;
                case ItemID.SalmonCreekForestFish: // SFCFish
                    ItemHandler.AddItemNote(item, notify);
                    SfcFishAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.SalmonCreekForestFish);
                    break;
                case ItemID.PublicPoolFish: // PPFish
                    ItemHandler.AddItemNote(item, notify);
                    PpFishAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.PublicPoolFish);
                    break;
                case ItemID.BathhouseFish: // BathFish
                    ItemHandler.AddItemNote(item, notify);
                    BathFishAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.BathhouseFish);
                    break;
                case ItemID.TadpoleHqFish: // HQFish
                    ItemHandler.AddItemNote(item, notify);
                    HqFishAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.TadpoleHqFish);
                    break;
                case ItemID.HairballCityKey: // HCKey
                    ItemHandler.AddItemNote(item, notify);
                    HcKeyAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.HairballCityKey);
                    break;
                case ItemID.TurbineTownKey: // TTKey
                    ItemHandler.AddItemNote(item, notify);
                    TtKeyAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.TurbineTownKey);
                    break;
                case ItemID.SalmonCreekForestKey: // SFCKey
                    ItemHandler.AddItemNote(item, notify);
                    SfcKeyAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.SalmonCreekForestKey);
                    break;
                case ItemID.PublicPoolKey: // PPKey
                    ItemHandler.AddItemNote(item, notify);
                    PpKeyAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.PublicPoolKey);
                    break;
                case ItemID.BathhouseKey: // BathKey
                    ItemHandler.AddItemNote(item, notify);
                    BathKeyAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.BathhouseKey);
                    break;
                case ItemID.TadpoleHqKey: // HQKey
                    ItemHandler.AddItemNote(item, notify);
                    HqKeyAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.TadpoleHqKey);
                    break;
                case ItemID.HairballCitySeed: // HCSeed
                    ItemHandler.AddItemNote(item, notify);
                    HcSeedAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.HairballCitySeed);
                    break;
                case ItemID.SalmonCreekForestSeed: // SFCSeed
                    ItemHandler.AddItemNote(item, notify);
                    SfcSeedAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.SalmonCreekForestSeed);
                    break;
                case ItemID.BathhouseSeed: // BathSeed
                    ItemHandler.AddItemNote(item, notify);
                    BathSeedAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.BathhouseSeed);
                    break;
                case ItemID.HairballCityFlower: // HCFlower
                    ItemHandler.AddItemNote(item, notify);
                    HcFlowerAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.HairballCityFlower);
                    break;
                case ItemID.TurbineTownFlower: // TTFlower
                    ItemHandler.AddItemNote(item, notify);
                    TtFlowerAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.TurbineTownFlower);
                    break;
                case ItemID.SalmonCreekForestFlower: // SFCFlower
                    ItemHandler.AddItemNote(item, notify);
                    SfcFlowerAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.SalmonCreekForestFlower);
                    break;
                case ItemID.PublicPoolFlower: // PPFlower
                    ItemHandler.AddItemNote(item, notify);
                    PpFlowerAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.PublicPoolFlower);
                    break;
                case ItemID.BathhouseFlower: // BathFlower
                    ItemHandler.AddItemNote(item, notify);
                    BathFlowerAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.BathhouseFlower);
                    break;
                case ItemID.TadpoleHqFlower: // HQFlower
                    ItemHandler.AddItemNote(item, notify);
                    HqFlowerAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.TadpoleHqFlower);
                    break;
                case ItemID.HairballCityCassette: // HCCassette
                    ItemHandler.AddHcCassette(senderName, notify);
                    HcCassetteAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.HairballCityCassette);
                    break;
                case ItemID.TurbineTownCassette: // TTCassette
                    ItemHandler.AddTtCassette(senderName, notify);
                    TtCassetteAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.TurbineTownCassette);
                    break;
                case ItemID.SalmonCreekForestCassette: // SFCCassette
                    ItemHandler.AddSfcCassette(senderName, notify);
                    SfcCassetteAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.SalmonCreekForestCassette);
                    break;
                case ItemID.PublicPoolCassette: // PPCassette
                    ItemHandler.AddPpCassette(senderName, notify);
                    PpCassetteAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.PublicPoolCassette);
                    break;
                case ItemID.BathhouseCassette: // BathCassette
                    ItemHandler.AddBathCassette(senderName, notify);
                    BathCassetteAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.BathhouseCassette);
                    break;
                case ItemID.TadpoleHqCassette: // HQCassette
                    ItemHandler.AddHqCassette(senderName, notify);
                    HqCassetteAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.TadpoleHqCassette);
                    break;
                case ItemID.GarysGardenCassette: // GGCassette
                    ItemHandler.AddGgCassette(senderName, notify);
                    GgCassetteAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.GarysGardenCassette);
                    break;
                case ItemID.SpeedBoost: // Speed Boost
                    SpeedBoostAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.SpeedBoost);
                    ItemHandler.AddSpeedBoost(senderName, notify);
                    break;
                case ItemID.FreezeTrap: // Freeze Trap
                    ItemHandler.AddFreezeTrap(senderName, notify);
                    break;
                case ItemID.IronBootsTrap: // Iron Boots Trap
                    ItemHandler.AddIronBootsTrap(senderName, notify);
                    break;
                case ItemID.WhoopsTrap: // Whoops! Trap
                    ItemHandler.AddWhoopsTrap(senderName, notify);
                    break;
                case ItemID.MyTurnTrap: // My Turn! Trap
                    ItemHandler.AddMyTurnTrap(senderName, notify);
                    break;
                case ItemID.GravityTrap: // Gravity Trap
                    ItemHandler.AddGravityTrap(senderName, notify);
                    break;
                case ItemID.HomeTrap: // Home Trap
                    ItemHandler.AddHomeTrap(senderName, notify);
                    break;
                case ItemID.WideTrap: // W I D E Trap
                    ItemHandler.AddWideTrap(senderName, notify);
                    break;
                case ItemID.PhoneTrap: // Phone Trap
                    ItemHandler.AddPhoneTrap(senderName, notify);
                    break;
                case ItemID.TinyTrap: // Tiny Trap
                    ItemHandler.AddTinyTrap(senderName, notify);
                    break;
                case ItemID.JumpingJacksTrap: // Jumping Jacks Trap
                    ItemHandler.AddJumpingJacksTrap(senderName, notify);
                    break;
                case ItemID.CameraStuckTrap: // Camera Stuck Trap
                    ItemHandler.AddCameraStuckTrap(senderName, notify);
                    break;
                case ItemID.InvertedCameraTrap: // InvertedCamera Trap
                    ItemHandler.AddInvertedCameraTrap(senderName, notify);
                    break;
                case ItemID.ThereGoesNikoTrap: // There Goes Niko Trap
                    ItemHandler.AddThereGoesNikoTrap(senderName, notify);
                    break;
                case ItemID.PartyInvitation: // Party Invitation
                    ItemHandler.AddPartyInvitation(item, notify);
                    TicketParty = true;
                    break;
                case ItemID.SafetyHelmet: // Bonk Helmet
                    ItemHandler.AddItemNote(item, notify);
                    BonkPermitAcquired = true;
                    break;
                case ItemID.BugNet: // Bug net
                    ItemHandler.AddItemNote(item, notify);
                    BugnetAcquired = true;
                    break;
                case ItemID.SodaRepair: // Soda Repair
                    ItemHandler.AddItemNote(item, notify);
                    SodaRepairAcquired = true;
                    break;
                case ItemID.ParasolRepair: // Parasol Repair
                    ItemHandler.AddItemNote(item, notify);
                    ParasolRepairAcquired = true;
                    break;
                case ItemID.SwimCourse: // Swim Course
                    ItemHandler.AddItemNote(item, notify);
                    SwimmingAcquired = true;
                    break;
                case ItemID.Textbox: // Textbox
                    ItemHandler.AddItemNote(item, notify);
                    TextboxAcquired = true;
                    break;
                case ItemID.HomeTextbox: // Textbox
                    ItemHandler.AddItemNote(item, notify);
                    HomeTextboxAcquired = true;
                    break;
                case ItemID.HairballCityTextbox: // Textbox
                    ItemHandler.AddItemNote(item, notify);
                    HairballTextboxAcquired = true;
                    break;
                case ItemID.TurbineTownTextbox: // Textbox
                    ItemHandler.AddItemNote(item, notify);
                    TurbineTextboxAcquired = true;
                    break;
                case ItemID.SalmonCreekForestTextbox: // Textbox
                    ItemHandler.AddItemNote(item, notify);
                    SalmonTextboxAcquired = true;
                    break;
                case ItemID.PublicPoolTextbox: // Textbox
                    ItemHandler.AddItemNote(item, notify);
                    PoolTextboxAcquired = true;
                    break;
                case ItemID.BathhouseTextbox: // Textbox
                    ItemHandler.AddItemNote(item, notify);
                    BathTextboxAcquired = true;
                    break;
                case ItemID.TadpoleHqTextbox: // Textbox
                    ItemHandler.AddItemNote(item, notify);
                    TadpoleTextboxAcquired = true;
                    break;
                case ItemID.GarysGardenTextbox: // Textbox
                    ItemHandler.AddItemNote(item, notify);
                    GardenTextboxAcquired = true;
                    break;
                case ItemID.AcRepair: // AC Repair
                    ItemHandler.AddItemNote(item, notify);
                    acRepairAcquired = true;
                    break;
                case ItemID.AppleBasket: // Apple Basket
                    ItemHandler.AddItemNote(item, notify);
                    AppleBasketAcquired = true;
                    break;
                case ItemID.HairballCityBone: // Hairball City Bone
                    ItemHandler.AddItemNote(item, notify);
                    HcBoneAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.HairballCityBone);
                    break;
                case ItemID.TurbineTownBone: // Turbine Town Bone
                    ItemHandler.AddItemNote(item, notify);
                    TtBoneAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.TurbineTownBone);
                    break;
                case ItemID.SalmonCreekForestBone: // Salmon Creek Forest Bone
                    ItemHandler.AddItemNote(item, notify);
                    SfcBoneAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.SalmonCreekForestBone);
                    break;
                case ItemID.PublicPoolBone: // Public Pool Bone
                    ItemHandler.AddItemNote(item, notify);
                    PpBoneAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.PublicPoolBone);
                    break;
                case ItemID.BathhouseBone: // Bathhouse Bone
                    ItemHandler.AddItemNote(item, notify);
                    BathBoneAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.BathhouseBone);
                    break;
                case ItemID.TadpoleHqBone: // Tadpole HQ Bone
                    ItemHandler.AddItemNote(item, notify);
                    HqBoneAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.TadpoleHqBone);
                    break;
                case ItemID.GarysGardenSeed: // Gary's Garden Seed
                    ItemHandler.AddGarysGardenSeed(item, notify);
                    GarysGardenSeedAmount = _session.Items.AllItemsReceived.Count(t => t.ItemId == ItemID.GarysGardenSeed);
                    if (GarysGardenSeedAmount >= 10)
                        GarysGardenSeedAmount = 10;
                    Plugin.BepinLogger.LogInfo($"Got a new seed! Current Count: {GarysGardenSeedAmount}");
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
                InvalidSceneItemQueue.Add(item);
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
        foreach (var location in checkedLocations)
        {
            if (ServerData.CheckedLocations.Contains(location)) continue;
            Location matchedLocation = null;
            string flagType = null;

            if (Locations.LetterLocations.TryGetValue(location, out var letterLocation))
            {
               matchedLocation = letterLocation; 
               flagType = "Letter";
            }
                
            else if (Locations.AchievementsLocations.TryGetValue(location, out var achievementLocation))
            {
                matchedLocation = achievementLocation;
                flagType = "Achievement";
            }
            else if (Locations.CoinLocations.TryGetValue(location, out var coinLocation))
            {
                matchedLocation = coinLocation;
                flagType = "Coin";
            }
            else if (Locations.CassetteLocations.TryGetValue(location, out var cassetteLocation))
            {
                matchedLocation = cassetteLocation;
                flagType = "Cassette";
            }
            else if (Locations.FishsanityLocations.TryGetValue(location, out var fishsanityLocation))
            {
                matchedLocation = fishsanityLocation;
                flagType = "Fish";
            }
            else if (Locations.GeneralLocations.TryGetValue(location, out var generalLocation))
            {
                matchedLocation = generalLocation;
                flagType = "General";
            }
            else if (Locations.HandsomeLocations.TryGetValue(location, out var handsomeLocation))
            {
                matchedLocation = handsomeLocation;
                flagType = "Handsome";
            }
            else if (Locations.KeyLocations.TryGetValue(location, out var keyLocation))
            {
                matchedLocation = keyLocation;
                flagType = "Key";
            }
            else if (Locations.KioskLocations.TryGetValue(location, out var kioskLocation))
            {
                matchedLocation = kioskLocation;
                flagType = "Kiosk";
            }
            else if (Locations.SnailShopLocations.TryGetValue(location, out var shopLocation))
            {
                matchedLocation = shopLocation;
                flagType = "SnailShop";
            }
            else if (Locations.GaryGardenCoinLocations.TryGetValue(location, out var gardenCoinLocation))
            {
                matchedLocation = gardenCoinLocation;
                flagType = "GardenCoin";
            }
            else if (Locations.GaryGardenCassetteLocations.TryGetValue(location, out var gardenCassetteLocation))
            {
                matchedLocation = gardenCassetteLocation;
                flagType = "GardenCassette";
            }
            else if (Locations.SunflowerSeedsLocations.TryGetValue(location, out var sunflowerSeedLocation))
            {
                matchedLocation = sunflowerSeedLocation;
                flagType = "SunflowerSeed";
            }
            else if (Locations.FlowerbedsLocations.TryGetValue(location, out var flowerbedsLocation))
            {
                matchedLocation = flowerbedsLocation;
                flagType = "Flowerbed";
            }
            else if (Locations.ApplesanityLocations.TryGetValue(location, out var appleLocation))
            {
                matchedLocation = appleLocation;
                flagType = "Apple";
            }
            else if (Locations.ProgressiveMitchMaiLocations.TryGetValue(location, out var mimaLocation))
            {
                matchedLocation = mimaLocation;
                flagType = "MiMa";
            }
            else if (Locations.ChatsanityLevelLocations.TryGetValue(location, out var chatLevelLocation))
            {
                matchedLocation = chatLevelLocation;
                flagType = "ChatLevel";
            }
            else if (Locations.ChatsanityLevelGarysGardenLocations.TryGetValue(location, out var gardenChatLevelLocation))
            {
                matchedLocation = gardenChatLevelLocation;
                flagType = "GardenChatLevel";
            }
            else if (Locations.ChatsanityNikoThoughtsLocations.TryGetValue(location, out var thougtLocation))
            {
                matchedLocation = thougtLocation;
                flagType = "Thought";
            } else if (Locations.ChatsanityGlobalLocations.TryGetValue(location, out var chatGlobalLocation))
            {
                matchedLocation = chatGlobalLocation;
                flagType = "ChatGlobal";
            }
            else if (Locations.BugsanityLocations.TryGetValue(location, out var bugLocation))
            {
                matchedLocation = bugLocation;
                flagType = "Bug";
            }
            else if (Locations.BonesanityLocations.TryGetValue(location, out var boneLocation))
            {
                matchedLocation = boneLocation;
                flagType = "Bone";
            }
            else if (Locations.GaryGardenSeedLocations.TryGetValue(location, out var gardenSeedLocation))
            {
                matchedLocation = gardenSeedLocation;
                flagType = "GardenSeed";
            }
            
            ServerData.CheckedLocations.Add(location);
            if (matchedLocation != null)
            {
                int level = matchedLocation.Level;
                string flag = matchedLocation.Flag;

                AddFlag(flagType, flag, level);
                if (Plugin.DebugMode)
                    Plugin.BepinLogger.LogInfo($"Processed Location: {location}, Level: {level}, Flag: {flag}");
            }
            else
            {
                Plugin.BepinLogger.LogInfo($"Location not found in dictionaries: {location}");
            }
        }
    }
    
    private static void AddFlag(string flagType, string flag, int level)
    {
        var worldsData = scrGameSaveManager.instance.gameData.worldsData;
        var world = scrWorldSaveDataContainer.instance;
        var genFlag = scrGameSaveManager.instance.gameData.generalGameData.generalFlags;
        switch (flagType)
        {
            case "Coin":
                if (!worldsData[level].coinFlags.Contains(flag))
                    worldsData[level].coinFlags.Add(flag);
                break;
            case "Cassette":
                if (!worldsData[level].cassetteFlags.Contains(flag))
                    worldsData[level].cassetteFlags.Add(flag);
                break;
            case "SunflowerSeed" or "Flowerbed" or "Apple" or "Thought" or "ChatLevel" or "Bug" or "Bone":
                if (!worldsData[level].miscFlags.Contains(flag))
                    worldsData[level].miscFlags.Add(flag);
                break;
            case "Key":
                if (!worldsData[level].miscFlags.Contains(flag))
                {
                    worldsData[level].miscFlags.Add(flag);
                    if (world.worldIndex == level)
                    {
                        world.keyAmount++;
                    }
                    else
                    {
                        worldsData[level].keyAmount++;
                    }
                }
                break;
            case "Letter":
                if (!worldsData[level].letterFlags.Contains(flag))
                    worldsData[level].letterFlags.Add(flag);
                break;
            case "General" or "Kiosk" or "Handsome" or "Achievement" or "MiMa":
                if (!scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains(flag))
                    scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Add(flag);
                break;
            case "SnailShop":
                if (!scrGameSaveManager.instance.gameData.generalGameData.snailBoughtClothes[level])
                    scrGameSaveManager.instance.gameData.generalGameData.snailBoughtClothes[level] = true;
                break;
            case "Fish":
                if (!worldsData[level].fishFlags.Contains(flag))
                    worldsData[level].fishFlags.Add(flag);
                break;
            case "ChatGlobal":
                if (!worldsData[0].miscFlags.Contains(flag))
                    worldsData[0].miscFlags.Add(flag);
                break;
            case "GardenCoin":
                try
                {
                    if (!worldsData[7].coinFlags.Contains(flag))
                        worldsData[7].coinFlags.Add(flag);
                }
                catch (Exception e)
                {
                    Plugin.BepinLogger.LogInfo($"Failed to add flag {flag} to Garden: {e.Message}");
                }
                break;
            case "GardenCassette":
                try
                {
                    if (!worldsData[7].cassetteFlags.Contains(flag))
                        worldsData[7].cassetteFlags.Add(flag);
                }
                catch (Exception e)
                {
                    Plugin.BepinLogger.LogInfo($"Failed to add flag {flag} to Garden: {e.Message}");
                }
                break;
            case "GardenChatLevel" or "GardenSeed":
                try
                {
                    if (!worldsData[7].miscFlags.Contains(flag))
                        worldsData[7].miscFlags.Add(flag);
                }
                catch (Exception e)
                {
                    Plugin.BepinLogger.LogInfo($"Failed to add flag {flag} to Garden: {e.Message}");
                }
                break;
            default:
                Plugin.BepinLogger.LogError($"Unknown flag type: {flagType}");
                break;
        }
        scrWorldSaveDataContainer.instance.SaveWorld();
    }

    private static void SaveItemIndex(int itemIndex)
    {
        Plugin.BepinLogger.LogInfo($"Saving ItemIndex: {itemIndex}");

        scrGameSaveManager.instance.gameData.generalGameData.generalFlags.RemoveAll(flag =>
            flag.StartsWith("ItemIndex."));

        scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Add($"ItemIndex.{itemIndex}");
        scrGameSaveManager.IsDirty = true;
    }

    public static int GetItemIndex()
    {
        var itemFlag = scrGameSaveManager.instance.gameData.generalGameData.generalFlags
            .FirstOrDefault(flag => flag.StartsWith("ItemIndex."));

        if (!string.IsNullOrEmpty(itemFlag))
        {
            if (int.TryParse(itemFlag.Split('.')[1], out var index))
            {
                return index;
            }
        }

        return 0;
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