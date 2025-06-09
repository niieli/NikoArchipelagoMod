using System.Collections.Generic;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using UnityEngine;

namespace NikoArchipelago.Archipelago;

public class Assets
{
    public static readonly Dictionary<string, GameObject> CreatedItemsCache = new();
    public static readonly Dictionary<MonoBehaviour, Dictionary<string, GameObject>> InstanceItemsCache = new();
    public static readonly Dictionary<string, string> PrefabMapping = new()
    {
        { "apProg", "APProgressive" },
        { "apUseful", "APUseful" },
        { "apFiller", "APFiller" },
        { "apTrap", "APTrap" },
        { "apTrap1", "APTrap1" },
        { "apTrap2", "APTrap2" },
        { "coin", "Coin" },
        { "cassette", "Cassette" },
        { "key", "Key" },
        { "contactList", "ContactList" },
        { "apples", "Apples" },
        { "snailMoney", "SnailMoney" },
        { "letter", "Letter" },
        { "bugs", "Bugs" },
        { "hcfish", "HairballFish" },
        { "ttfish", "TurbineFish" },
        { "scffish", "SalmonFish" },
        { "ppfish", "PoolFish" },
        { "bathfish", "BathFish" },
        { "hqfish", "TadpoleFish" },
        { "hckey", "HairballKey" },
        { "ttkey", "TurbineKey" },
        { "scfkey", "SalmonKey" },
        { "ppkey", "PoolKey" },
        { "bathkey", "BathKey" },
        { "hqkey", "TadpoleKey" },
        { "hcflower", "HairballFlower" },
        { "ttflower", "TurbineFlower" },
        { "scfflower", "SalmonFlower" },
        { "ppflower", "PoolFlower" },
        { "bathflower", "BathFlower" },
        { "hqflower", "TadpoleFlower" },
        { "hccassette", "HairballCassette" },
        { "ttcassette", "TurbineCassette" },
        { "scfcassette", "SalmonCassette" },
        { "ppcassette", "PoolCassette" },
        { "bathcassette", "BathCassette" },
        { "hqcassette", "TadpoleCassette" },
        { "gardencassette", "GardenCassette" },
        { "hcseed", "HairballSeed" },
        { "scfseed", "SalmonSeed" },
        { "bathseed", "BathSeed" },
        { "superJump", "SuperJump" },
        { "hairballCity", "HairballCity" },
        { "turbineTown", "TurbineTown" },
        { "salmonCreekForest", "SalmonCreekForest" },
        { "publicPool", "PublicPool" },
        { "bathhouse", "Bathhouse" },
        { "tadpoleHQ", "TadpoleHQ" },
        { "garysGarden", "GarysGarden" },
        { "timepiece", "TimePieceHiT" },
        { "yarn", "YarnHiT" },
        { "yarn2", "Yarn2HiT" },
        { "yarn3", "Yarn3HiT" },
        { "yarn4", "Yarn4HiT" },
        { "yarn5", "Yarn5HiT" },
        { "speedboost", "SpeedBoost" },
        { "partyTicket", "PartyTicket" },
        { "bonkHelmet", "BonkHelmet" },
        { "bugNet", "BugNet" },
        { "sodaRepair", "SodaRepair" },
        { "parasolRepair", "ParasolRepair" },
        { "swimCourse", "SwimCourse" },
        { "textbox", "Textbox" },
        { "acRepair", "ACRepair" },
        { "applebasket", "AppleBasket" },
        { "hcbone", "HairballBone" },
        { "ttbone", "TurbineBone" },
        { "scfbone", "SalmonBone" },
        { "ppbone", "PoolBone" },
        { "bathbone", "BathBone" },
        { "hqbone", "TadpoleBone" },
    };

    private static List<string> _progItems = [
        "coin",
        "cassette",
        "key",
        "contactList",
        "hcfish",
        "ttfish",
        "scffish",
        "ppfish",
        "bathfish",
        "hqfish",
        "hckey",
        "ttkey",
        "scfkey",
        "ppkey",
        "bathkey",
        "hqkey",
        "hcflower",
        "ttflower",
        "scfflower",
        "ppflower",
        "bathflower",
        "hqflower",
        "hccassette",
        "ttcassette",
        "scfcassette",
        "ppcassette",
        "bathcassette",
        "hqcassette",
        "gardencassette",
        "hcseed",
        "scfseed",
        "bathseed",
        "hairballCity",
        "turbineTown",
        "salmonCreekForest",
        "publicPool",
        "bathhouse",
        "tadpoleHQ",
        "garysGarden",
        "partyTicket",
        "bonkHelmet",
        "bugNet",
        "sodaRepair",
        "parasolRepair",
        "swimCourse",
        "textbox",
        "acRepair",
        "applebasket",
        "hcbone",
        "ttbone",
        "scfbone",
        "ppbone",
        "bathbone",
        "hqbone",
    ];

    public static string RandomProgTrap()
    {
        return _progItems[Random.Range(0, _progItems.Count)];
    }

    public static string GetItemName(ScoutedItemInfo itemInfo)
    {
        var itemName = "";
        if (itemInfo.IsReceiverRelatedToActivePlayer && itemInfo.Flags.HasFlag(ItemFlags.Trap))
        {
            var fakeNames = new[]
            {
                "Coin ?",
                "Coin :)",
                "Shiny Object",
                "Pon",
                "Cassette ?",
                "Coin",
                "Rupee",
                "Coin >:(",
                "A fabulous flower",
                "COIN!",
                "CASSETTE!",
                "REDACTED",
                "Cool Item(insert cool smiley)",
                "A",
                "Noic",
                "Mixtape",
                "Home Cassette",
                "Tickets for a concert"
            };
            var randomFakeName = Random.Range(0, fakeNames.Length);
            itemName = fakeNames[randomFakeName];
        }
        else
        {
            itemName = itemInfo.ItemName;
        }

        return itemName;
    }

    public static string GetClassification(ScoutedItemInfo itemInfo)
    {
        var classification = "";
        if (itemInfo.Flags.HasFlag(ItemFlags.Advancement))
        {
            classification = "Important";
        }
        else if (itemInfo.Flags.HasFlag(ItemFlags.NeverExclude))
        {
            classification = "Useful";
        }
        else if (itemInfo.Flags.HasFlag(ItemFlags.Trap))
        {
            var trapStrings = new[]
            {
                "SUPER IMPORTANT",
                "like a good deal",
                "very important trust me",
                "like the best item",
                "a 1-Time Offer!",
                "one of those 'You Need This!' items",
                "RARE LOOT!",
                "Legendary!",
                "very helpful... I promise!",
                "Absolutely NOT a trap",
                "A MUST PICK UP!",
                "loved among collector's... hehe",
                "a very funny item"
            };
            var randomIndex = Random.Range(0, trapStrings.Length);
            classification = trapStrings[randomIndex];
        }
        else if (itemInfo.Flags.HasFlag(ItemFlags.None))
        {
            classification = "Useless";
        }
        else
        {
            classification = "Unknown";
        }

        return classification;
    }
}