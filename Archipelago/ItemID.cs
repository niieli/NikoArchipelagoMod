using System.Collections.Generic;

namespace NikoArchipelago.Archipelago;

public static class ItemID
{
    private const long BaseID = 598_145_444_000;
    
    // General Items
    public const long Coin = BaseID + 0;
    public const long Cassette = BaseID + 1;
    public const long Key = BaseID + 2;
    public const long Letter = BaseID + 7;
    public const long Apples = BaseID + 3;
    public const long ContactList1 = BaseID + 4;
    public const long ContactList2 = BaseID + 5;
    public const long ProgressiveContactList = BaseID + 15;
    public const long SuperJump = BaseID + 6;
    public const long Bugs = BaseID + 14;
    public const long SnailMoney = BaseID + 16;
    public const long SpeedBoost = BaseID + 18;
    public const long GarysGardenSeed = BaseID + 250;
    
    // Traps
    public const long FreezeTrap = BaseID + 70;
    public const long IronBootsTrap = BaseID + 71;
    public const long WhoopsTrap = BaseID + 72;
    public const long MyTurnTrap = BaseID + 73;
    public const long GravityTrap = BaseID + 74;
    public const long HomeTrap = BaseID + 75;
    public const long WideTrap = BaseID + 76;
    public const long PhoneTrap = BaseID + 77;
    public const long TinyTrap = BaseID + 78;
    public const long JumpingJacksTrap = BaseID + 79;

    public static readonly HashSet<long> TrapIDs = new()
    {
        FreezeTrap,
        IronBootsTrap,
        WhoopsTrap,
        MyTurnTrap,
        GravityTrap,
        HomeTrap,
        WideTrap,
        PhoneTrap,
        TinyTrap,
        JumpingJacksTrap,
    };
    
    // Fishsanity
    public const long HairballCityFish = BaseID + 20;
    public const long TurbineTownFish = BaseID + 21;
    public const long SalmonCreekForestFish = BaseID + 22;
    public const long PublicPoolFish = BaseID + 23;
    public const long BathhouseFish = BaseID + 24;
    public const long TadpoleHqFish = BaseID + 25;
    
    // Keys
    public const long HairballCityKey = BaseID + 30;
    public const long TurbineTownKey = BaseID + 31;
    public const long SalmonCreekForestKey = BaseID + 32;
    public const long PublicPoolKey = BaseID + 33;
    public const long BathhouseKey = BaseID + 34;
    public const long TadpoleHqKey = BaseID + 35;
    
    // Seeds
    public const long HairballCitySeed = BaseID + 36;
    public const long SalmonCreekForestSeed = BaseID + 37;
    public const long BathhouseSeed = BaseID + 38;
    
    // Flowers
    public const long HairballCityFlower = BaseID + 40;
    public const long TurbineTownFlower = BaseID + 41;
    public const long SalmonCreekForestFlower = BaseID + 42;
    public const long PublicPoolFlower = BaseID + 43;
    public const long BathhouseFlower = BaseID + 44;
    public const long TadpoleHqFlower = BaseID + 45;
    
    // NPCs
    public const long HairballCityNPCs = BaseID + 46;
    public const long TurbineTownNPCs = BaseID + 47;
    public const long SalmonCreekForestNPCs = BaseID + 48;
    public const long PublicPoolNPCs = BaseID + 49;
    public const long BathhouseNPCs = BaseID + 50;
    public const long TadpoleHqNPCs = BaseID + 51;
    
    // Level Based Cassettes
    public const long HairballCityCassette = BaseID + 52;
    public const long TurbineTownCassette = BaseID + 53;
    public const long SalmonCreekForestCassette = BaseID + 54;
    public const long PublicPoolCassette = BaseID + 55;
    public const long BathhouseCassette = BaseID + 56;
    public const long TadpoleHqCassette = BaseID + 57;
    public const long GarysGardenCassette = BaseID + 58;
    
    // Permits
    public const long SafetyHelmet = BaseID + 101;
    public const long BugNet = BaseID + 102;
    public const long SodaRepair = BaseID + 103;
    public const long ParasolRepair = BaseID + 104;
    public const long SwimCourse = BaseID + 105;
    public const long AcRepair = BaseID + 107;
    public const long AppleBasket = BaseID + 108;
    
    // Textbox
    public const long Textbox = BaseID + 106;
    public const long HomeTextbox = BaseID + 140;
    public const long HairballCityTextbox = BaseID + 141;
    public const long TurbineTownTextbox = BaseID + 142;
    public const long SalmonCreekForestTextbox = BaseID + 143;
    public const long PublicPoolTextbox = BaseID + 144;
    public const long BathhouseTextbox = BaseID + 145;
    public const long TadpoleHqTextbox = BaseID + 146;
    public const long GarysGardenTextbox = BaseID + 147;
    
    // Bonesanity
    public const long HairballCityBone = BaseID + 111;
    public const long TurbineTownBone = BaseID + 112;
    public const long SalmonCreekForestBone = BaseID + 113;
    public const long PublicPoolBone = BaseID + 114;
    public const long BathhouseBone = BaseID + 115;
    public const long TadpoleHqBone = BaseID + 116;
    
    // Levels
    public const long HairballCityTicket = BaseID + 8;
    public const long TurbineTownTicket = BaseID + 9;
    public const long SalmonCreekForestTicket = BaseID + 10;
    public const long PublicPoolTicket = BaseID + 11;
    public const long BathhouseTicket = BaseID + 12;
    public const long TadpoleHqTicket = BaseID + 13;
    public const long GarysGardenTicket = BaseID + 17;
    public const long PartyInvitation = BaseID + 80;
}