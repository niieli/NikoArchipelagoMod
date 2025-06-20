using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Converters;
using Archipelago.MultiClient.Net.Enums;
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
        HcBoneAmount, TtBoneAmount, SfcBoneAmount, PpBoneAmount, BathBoneAmount, HqBoneAmount;

    public static int SpeedBoostAmount;
    public static bool SuperJump, Ticket1, Ticket2, Ticket3, Ticket4, Ticket5, Ticket6, TicketGary, TicketParty,
        HcNPCs, TtNPCs, SfcNPCs, PpNPCs, BathNPCs, HqNPCs, Keysanity, ElevatorRepaired, 
        BonkPermitAcquired, BugnetAcquired, SodaRepairAcquired, ParasolRepairAcquired, SwimmingAcquired, TextboxAcquired, acRepairAcquired, AppleBasketAcquired;

    private static int savedItemIndex;
    private static bool stopIt;
    public Task _disconnectTask;
    public static bool TrapLink { get; private set; }

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
        BouncedPacket trapLinkPacket = new BouncedPacket
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

    private void SendPacket()
    {
        
    }

    public static void SendMessage(string message)
    {
        _session.Socket.SendPacketAsync(new SayPacket { Text = message });
    }
    
    public List<ItemInfo> queuedItems = [];
    public List<ItemInfo> queuedItems2 = [];
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
            if (receivedItem.ItemName is "Apples" or "25 Apples" or "10 Bugs" or "Bugs" or "Letter" or "Snail Money"
                    or "1000 Snail Money" or "Freeze Trap" or "Iron Boots Trap" or "Whoops! Trap" or "My Turn! Trap"
                    or "Gravity Trap" or "Home Trap" or "W I D E Trap" or "Phone Trap" or "Tiny Trap" or "Jumping Jacks Trap")
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
                    ItemHandler.AddMoney(7500, senderName, notify);
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
                case 598_145_444_000+36: // HCSeed
                    ItemHandler.AddHcSeed(senderName, notify);
                    HcSeedAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Hairball City Seed");
                    break;
                case 598_145_444_000+37: // SFCSeed
                    ItemHandler.AddSfcSeed(senderName, notify);
                    SfcSeedAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Salmon Creek Forest Seed");
                    break;
                case 598_145_444_000+38: // BathSeed
                    ItemHandler.AddBathSeed(senderName, notify);
                    BathSeedAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Bathhouse Seed");
                    break;
                case 598_145_444_000+40: // HCFlower
                    ItemHandler.AddHcFlower(senderName, notify);
                    HcFlowerAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Hairball City Flower");
                    break;
                case 598_145_444_000+41: // TTFlower
                    ItemHandler.AddTtFlower(senderName, notify);
                    TtFlowerAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Turbine Town Flower");
                    break;
                case 598_145_444_000+42: // SFCFlower
                    ItemHandler.AddSfcFlower(senderName, notify);
                    SfcFlowerAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Salmon Creek Forest Flower");
                    break;
                case 598_145_444_000+43: // PPFlower
                    ItemHandler.AddPpFlower(senderName, notify);
                    PpFlowerAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Public Pool Flower");
                    break;
                case 598_145_444_000+44: // BathFlower
                    ItemHandler.AddBathFlower(senderName, notify);
                    BathFlowerAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Bathhouse Flower");
                    break;
                case 598_145_444_000+45: // HQFlower
                    ItemHandler.AddHqFlower(senderName, notify);
                    HqFlowerAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Tadpole HQ Flower");
                    break;
                case 598_145_444_000+52: // HCCassette
                    ItemHandler.AddHcCassette(senderName, notify);
                    HcCassetteAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Hairball City Cassette");
                    break;
                case 598_145_444_000+53: // TTCassette
                    ItemHandler.AddTtCassette(senderName, notify);
                    TtCassetteAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Turbine Town Cassette");
                    break;
                case 598_145_444_000+54: // SFCCassette
                    ItemHandler.AddSfcCassette(senderName, notify);
                    SfcCassetteAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Salmon Creek Forest Cassette");
                    break;
                case 598_145_444_000+55: // PPCassette
                    ItemHandler.AddPpCassette(senderName, notify);
                    PpCassetteAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Public Pool Cassette");
                    break;
                case 598_145_444_000+56: // BathCassette
                    ItemHandler.AddBathCassette(senderName, notify);
                    BathCassetteAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Bathhouse Cassette");
                    break;
                case 598_145_444_000+57: // HQCassette
                    ItemHandler.AddHqCassette(senderName, notify);
                    HqCassetteAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Tadpole HQ Cassette");
                    break;
                case 598_145_444_000+58: // GGCassette
                    ItemHandler.AddGgCassette(senderName, notify);
                    GgCassetteAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Gary's Garden Cassette");
                    break;
                case 598_145_444_000+18: // Speed Boost
                    SpeedBoostAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Speed Boost");
                    ItemHandler.AddSpeedBoost(senderName, notify);
                    break;
                case 598_145_444_000+70: // Freeze Trap
                    ItemHandler.AddFreezeTrap(senderName, notify);
                    break;
                case 598_145_444_000+71: // Iron Boots Trap
                    ItemHandler.AddIronBootsTrap(senderName, notify);
                    break;
                case 598_145_444_000+72: // Whoops! Trap
                    ItemHandler.AddWhoopsTrap(senderName, notify);
                    break;
                case 598_145_444_000+73: // My Turn! Trap
                    ItemHandler.AddMyTurnTrap(senderName, notify);
                    break;
                case 598_145_444_000+74: // Gravity Trap
                    ItemHandler.AddGravityTrap(senderName, notify);
                    break;
                case 598_145_444_000+75: // Home Trap
                    ItemHandler.AddHomeTrap(senderName, notify);
                    break;
                case 598_145_444_000+76: // W I D E Trap
                    ItemHandler.AddWideTrap(senderName, notify);
                    break;
                case 598_145_444_000+77: // Phone Trap
                    ItemHandler.AddPhoneTrap(senderName, notify);
                    break;
                case 598_145_444_000+78: // Tiny Trap
                    ItemHandler.AddTinyTrap(senderName, notify);
                    break;
                case 598_145_444_000+79: // Jumping Jacks Trap
                    ItemHandler.AddJumpingJacksTrap(senderName, notify);
                    break;
                case 598_145_444_000+80: // Party Invitation
                    ItemHandler.AddPartyInvitation(item, notify);
                    TicketParty = true;
                    break;
                case 598_145_444_000+101: // Bonk Helmet
                    ItemHandler.AddSafetyHelmet(item, notify);
                    BonkPermitAcquired = true;
                    break;
                case 598_145_444_000+102: // Bug net
                    ItemHandler.AddBugNet(item, notify);
                    BugnetAcquired = true;
                    break;
                case 598_145_444_000+103: // Soda Repair
                    ItemHandler.AddSodaRepair(item, notify);
                    SodaRepairAcquired = true;
                    break;
                case 598_145_444_000+104: // Parasol Repair
                    ItemHandler.AddParasolRepair(item, notify);
                    ParasolRepairAcquired = true;
                    break;
                case 598_145_444_000+105: // Swim Course
                    ItemHandler.AddSwimCourse(item, notify);
                    SwimmingAcquired = true;
                    break;
                case 598_145_444_000+106: // Textbox
                    ItemHandler.AddTextboxItem(item, notify);
                    TextboxAcquired = true;
                    break;
                case 598_145_444_000+107: // AC Repair
                    ItemHandler.AddACRepair(item, notify);
                    acRepairAcquired = true;
                    break;
                case 598_145_444_000+108: // Apple Basket
                    ItemHandler.AddAppleBasket(item, notify);
                    AppleBasketAcquired = true;
                    break;
                case 598_145_444_000+111: // Hairball City Bone
                    ItemHandler.AddHairballBone(item, notify);
                    HcBoneAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Hairball City Bone");
                    break;
                case 598_145_444_000+112: // Turbine Town Bone
                    ItemHandler.AddTurbineBone(item, notify);
                    TtBoneAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Turbine Town Bone");
                    break;
                case 598_145_444_000+113: // Salmon Creek Forest Bone
                    ItemHandler.AddSalmonBone(item, notify);
                    SfcBoneAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Salmon Creek Forest Bone");
                    break;
                case 598_145_444_000+114: // Public Pool Bone
                    ItemHandler.AddPoolBone(item, notify);
                    PpBoneAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Public Pool Bone");
                    break;
                case 598_145_444_000+115: // Bathhouse Bone
                    ItemHandler.AddBathBone(item, notify);
                    BathBoneAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Bathhouse Bone");
                    break;
                case 598_145_444_000+116: // Tadpole HQ Bone
                    ItemHandler.AddTadpoleBone(item, notify);
                    HqBoneAmount = _session.Items.AllItemsReceived.Count(t => t.ItemName == "Tadpole HQ Bone");
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
            case "GardenChatLevel":
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