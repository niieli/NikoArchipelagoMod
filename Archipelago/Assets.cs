using System;
using System.Collections.Generic;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NikoArchipelago.Archipelago;

public class Assets
{
    public static AssetBundle AssetBundle, AssetBundleXmas;

    public static GameObject TrapFreeze,
        TrapIronBoots,
        TrapMyTurn,
        TrapWhoops,
        TrapGravity,
        ItemNotification,
        HintNotification,
        TrapWide,
        TrapHome,
        TrapJumpingJacks,
        TrapPhoneCall,
        TrapTiny,
        TrapFast,
        NoticeBonkHelmet,
        NoticeBonkHelmetTree,
        NoticeSodaCan,
        NoticeParasol,
        NoticeAC,
        NoticeSwimCourse,
        DeathLinkNotice,
        NoticeBugNet,
        NoticePartyTicket,
        NoticeAppleBasket,
        SparksParticleSystem,
        DeathLinkPopup;

    public static Sprite APSprite,
        BandanaSprite,
        BowtieSprite,
        CapSprite,
        CatSprite,
        ClownSprite,
        FlowerSprite,
        GlassesSprite,
        KingSprite,
        MahjongSprite,
        MotorSprite,
        MouseSprite,
        SmallHatSprite,
        StarsSprite,
        SwordSprite,
        TopHatSprite,
        SunglassesSprite,
        APLogoSprite,
        APIconSprite,
        CoinSprite,
        CassetteSprite,
        FishSprite,
        HairballFishSprite,
        TurbineFishSprite,
        SalmonFishSprite,
        PoolFishSprite,
        BathFishSprite,
        TadpoleFishSprite,
        KeySprite,
        HairballKeySprite,
        TurbineKeySprite,
        SalmonKeySprite,
        PoolKeySprite,
        BathKeySprite,
        TadpoleKeySprite,
        ContactListSprite,
        BottledSprite,
        EmployeeSprite,
        FrogFanSprite,
        HandsomeSprite,
        LostSprite,
        SuperJumpSprite,
        SnailFashionSprite,
        VolleyDreamsSprite,
        ApplesSprite,
        LetterSprite,
        HcSprite,
        TtSprite,
        SfcSprite,
        PpSprite,
        BathSprite,
        HqSprite,
        SnailMoneySprite,
        BugsSprite,
        GgSprite,
        GoalBadSprite,
        ApProgressionSprite,
        ApUsefulSprite,
        ApFillerSprite,
        ApTrapSprite,
        ApTrap2Sprite,
        ApTrap3Sprite,
        TimePieceSprite,
        YarnSprite,
        HairballFlowerSprite,
        TurbineFlowerSprite,
        SalmonFlowerSprite,
        PoolFlowerSprite,
        BathFlowerSprite,
        TadpoleFlowerSprite,
        HairballSeedSprite,
        SalmonSeedSprite,
        BathSeedSprite,
        HairballCassetteSprite,
        TurbineCassetteSprite,
        SalmonCassetteSprite,
        PoolCassetteSprite,
        BathCassetteSprite,
        TadpoleCassetteSprite,
        GardenCassetteSprite,
        FischerNoteSprite,
        GabiNoteSprite,
        MoomyNoteSprite,
        BlessleyNoteSprite,
        FreezeTrapSprite,
        IronBootsTrapSprite,
        MyTurnTrapSprite,
        WhoopsTrapSprite,
        SpeedBoostSprite,
        GravityTrapSprite,
        PhoneCallTrapSprite,
        JumpingJacksTrapSprite,
        WideTrapSprite,
        HomeTrapSprite,
        TinyTrapSprite,
        PartyTicketSprite,
        BonkHelmetSprite,
        BugNetSprite,
        SodaRepairSprite,
        ParasolRepairSprite,
        SwimCourseSprite,
        TextboxItemSprite,
        ACRepairSprite,
        AppleBasketSprite,
        DeathLinkSprite,
        HairballBoneSprite,
        TurbineBoneSprite,
        SalmonBoneSprite,
        PoolBoneSprite,
        BathBoneSprite,
        TadpoleBoneSprite,
        HomeTextboxSprite,
        HairballTextboxSprite,
        TurbineTextboxSprite,
        SalmonTextboxSprite,
        PoolTextboxSprite,
        BathTextboxSprite,
        TadpoleTextboxSprite,
        GardenTextboxSprite;

    public static Material ProgNotificationTexture,
        UsefulNotificationTexture,
        FillerNotificationTexture,
        TrapNotificationTexture;

    private static void CheckForChristmas()
    {
        var now = DateTime.Now;
        var currentYear = now.Month == 1 ? now.Year - 1 : now.Year;
        var christmasTime = new DateTime(currentYear, 12, 25);
        var startChrismas = christmasTime.AddDays(-24);
        var endChrismas = christmasTime.AddDays(18);
        if (DateTime.Now.Ticks > startChrismas.Ticks && DateTime.Now.Ticks < endChrismas.Ticks)
        {
            Plugin.ChristmasEvent = true;
            Plugin.BepinLogger.LogInfo($"Christmas Event Active.");
            AssetBundleXmas = AssetBundleLoader.LoadEmbeddedAssetBundle("apxmas");
        }
        else
        {
            Plugin.NoXmasEvent = true;
        }
    }
    
    public static void AssignSprites()
    {
        CheckForChristmas();
        AssetBundle = AssetBundleLoader.LoadEmbeddedAssetBundle("apassets");
        if (AssetBundle == null) return;
        APSprite = AssetBundle.LoadAsset<Sprite>("apLogo");
        BandanaSprite = AssetBundle.LoadAsset<Sprite>("BandanaAP");
        BowtieSprite = AssetBundle.LoadAsset<Sprite>("BowtieAP");
        CapSprite = AssetBundle.LoadAsset<Sprite>("CapAP");
        CatSprite = AssetBundle.LoadAsset<Sprite>("CatAP");
        ClownSprite = AssetBundle.LoadAsset<Sprite>("ClownFaceAP");
        FlowerSprite = AssetBundle.LoadAsset<Sprite>("FlowerAP");
        GlassesSprite = AssetBundle.LoadAsset<Sprite>("GlassesAP");
        KingSprite = AssetBundle.LoadAsset<Sprite>("KingStaffAP");
        MahjongSprite = AssetBundle.LoadAsset<Sprite>("MahjongAP");
        MotorSprite = AssetBundle.LoadAsset<Sprite>("MotorcycleAP");
        MouseSprite = AssetBundle.LoadAsset<Sprite>("MouseAP");
        SmallHatSprite = AssetBundle.LoadAsset<Sprite>("SmallHatAP");
        StarsSprite = AssetBundle.LoadAsset<Sprite>("StarsAP");
        SwordSprite = AssetBundle.LoadAsset<Sprite>("SwordAP");
        TopHatSprite = AssetBundle.LoadAsset<Sprite>("TophatAP");
        SunglassesSprite = AssetBundle.LoadAsset<Sprite>("SunglassesAP");
        APLogoSprite = AssetBundle.LoadAsset<Sprite>("HereComesNikoAPLogo");
        APIconSprite = AssetBundle.LoadAsset<Sprite>("nikoApLogo");
        CoinSprite = AssetBundle.LoadAsset<Sprite>("sprCoinLit");
        CassetteSprite = AssetBundle.LoadAsset<Sprite>("txrCassette");
        ContactListSprite = AssetBundle.LoadAsset<Sprite>("txrList");
        BottledSprite = AssetBundle.LoadAsset<Sprite>("BOTTLED_UP");
        EmployeeSprite = AssetBundle.LoadAsset<Sprite>("EMLOYEE_OF_THE_MONTH");
        FrogFanSprite = AssetBundle.LoadAsset<Sprite>("FROG_FAN");
        HandsomeSprite = AssetBundle.LoadAsset<Sprite>("HOPELESS_ROMANTIC");
        LostSprite = AssetBundle.LoadAsset<Sprite>("LOST_AT_SEA");
        SnailFashionSprite = AssetBundle.LoadAsset<Sprite>("SNAIL_FASHION_SHOW");
        VolleyDreamsSprite = AssetBundle.LoadAsset<Sprite>("VOLLEY_DREAMS");
        LetterSprite = AssetBundle.LoadAsset<Sprite>("txrLetter");
        ApplesSprite = AssetBundle.LoadAsset<Sprite>("AppleBundle");
        KeySprite = AssetBundle.LoadAsset<Sprite>("txrKey");
        HcSprite = AssetBundle.LoadAsset<Sprite>("TrainHairball");
        TtSprite = AssetBundle.LoadAsset<Sprite>("TrainTurbine");
        SfcSprite = AssetBundle.LoadAsset<Sprite>("TrainSalmon");
        PpSprite = AssetBundle.LoadAsset<Sprite>("TrainPool");
        BathSprite = AssetBundle.LoadAsset<Sprite>("TrainBath");
        HqSprite = AssetBundle.LoadAsset<Sprite>("TrainTadpole");
        GgSprite = AssetBundle.LoadAsset<Sprite>("GarysGarden");
        SnailMoneySprite = AssetBundle.LoadAsset<Sprite>("SnailMoney");
        BugsSprite = AssetBundle.LoadAsset<Sprite>("BugBundle");
        ApProgressionSprite = AssetBundle.LoadAsset<Sprite>("ApProgression");
        ApUsefulSprite = AssetBundle.LoadAsset<Sprite>("ApUseful");
        ApFillerSprite = AssetBundle.LoadAsset<Sprite>("ApFiller");
        ApTrapSprite = AssetBundle.LoadAsset<Sprite>("ApTrap");
        ApTrap2Sprite = AssetBundle.LoadAsset<Sprite>("ApTrap2");
        ApTrap3Sprite = AssetBundle.LoadAsset<Sprite>("ApTrap3");
        GoalBadSprite = AssetBundle.LoadAsset<Sprite>("goalBad");
        SuperJumpSprite = AssetBundle.LoadAsset<Sprite>("SuperJump");
        FishSprite = AssetBundle.LoadAsset<Sprite>("imgMapfish");
        HairballFishSprite = AssetBundle.LoadAsset<Sprite>("HairballFish");
        TurbineFishSprite = AssetBundle.LoadAsset<Sprite>("TurbineFish");
        SalmonFishSprite = AssetBundle.LoadAsset<Sprite>("SalmonFish");
        PoolFishSprite = AssetBundle.LoadAsset<Sprite>("PoolFish");
        BathFishSprite = AssetBundle.LoadAsset<Sprite>("BathFish");
        TadpoleFishSprite = AssetBundle.LoadAsset<Sprite>("TadpoleFish");
        HairballKeySprite = AssetBundle.LoadAsset<Sprite>("txrHairballKey");
        TurbineKeySprite = AssetBundle.LoadAsset<Sprite>("txrTurbineKey");
        SalmonKeySprite = AssetBundle.LoadAsset<Sprite>("txrSalmonKey");
        PoolKeySprite = AssetBundle.LoadAsset<Sprite>("txrPoolKey");
        BathKeySprite = AssetBundle.LoadAsset<Sprite>("txrBathKey");
        TadpoleKeySprite = AssetBundle.LoadAsset<Sprite>("txrTadpoleKey");
        TimePieceSprite = AssetBundle.LoadAsset<Sprite>("timepiece2D");
        YarnSprite = AssetBundle.LoadAsset<Sprite>("Yarn");
        HairballFlowerSprite = AssetBundle.LoadAsset<Sprite>("HairballFlower");
        TurbineFlowerSprite = AssetBundle.LoadAsset<Sprite>("TurbineFlower");
        SalmonFlowerSprite = AssetBundle.LoadAsset<Sprite>("SalmonFlower");
        PoolFlowerSprite = AssetBundle.LoadAsset<Sprite>("PoolFlower");
        BathFlowerSprite = AssetBundle.LoadAsset<Sprite>("BathFlower");
        TadpoleFlowerSprite = AssetBundle.LoadAsset<Sprite>("TadpoleFlower");
        HairballSeedSprite = AssetBundle.LoadAsset<Sprite>("HairballSeed");
        SalmonSeedSprite = AssetBundle.LoadAsset<Sprite>("SalmonSeed");
        BathSeedSprite = AssetBundle.LoadAsset<Sprite>("BathSeed");
        HairballCassetteSprite = AssetBundle.LoadAsset<Sprite>("HairballCassette");
        TurbineCassetteSprite = AssetBundle.LoadAsset<Sprite>("TurbineCassette");
        SalmonCassetteSprite = AssetBundle.LoadAsset<Sprite>("SalmonCassette");
        PoolCassetteSprite = AssetBundle.LoadAsset<Sprite>("PoolCassette");
        BathCassetteSprite = AssetBundle.LoadAsset<Sprite>("BathCassette");
        TadpoleCassetteSprite = AssetBundle.LoadAsset<Sprite>("TadpoleCassette");
        GardenCassetteSprite = AssetBundle.LoadAsset<Sprite>("GardenCassette");
        FischerNoteSprite = AssetBundle.LoadAsset<Sprite>("sprFischerHappyNote");
        GabiNoteSprite = AssetBundle.LoadAsset<Sprite>("sprFlowerNote");
        MoomyNoteSprite = AssetBundle.LoadAsset<Sprite>("sprHamsterNote");
        BlessleyNoteSprite = AssetBundle.LoadAsset<Sprite>("sprBugNote");
        TrapFreeze = AssetBundle.LoadAsset<GameObject>("TrapFreeze");
        TrapIronBoots = AssetBundle.LoadAsset<GameObject>("TrapIronBoots");
        TrapMyTurn = AssetBundle.LoadAsset<GameObject>("TrapMyTurn");
        TrapWhoops = AssetBundle.LoadAsset<GameObject>("TrapWhoops");
        FreezeTrapSprite = AssetBundle.LoadAsset<Sprite>("Schneeflocken1");
        IronBootsTrapSprite = AssetBundle.LoadAsset<Sprite>("TrapIronBoots");
        MyTurnTrapSprite = AssetBundle.LoadAsset<Sprite>("imgPepper");
        WhoopsTrapSprite = AssetBundle.LoadAsset<Sprite>("TrapWhoops");
        SpeedBoostSprite = AssetBundle.LoadAsset<Sprite>("SpeedBoost");
        TrapGravity = AssetBundle.LoadAsset<GameObject>("TrapGravity");
        GravityTrapSprite = AssetBundle.LoadAsset<Sprite>("BuzzNote");
        ProgNotificationTexture = AssetBundle.LoadAsset<Material>("APProgressionNotificationMaterial");
        UsefulNotificationTexture = AssetBundle.LoadAsset<Material>("APUsefulNotificationMaterial");
        FillerNotificationTexture = AssetBundle.LoadAsset<Material>("APFillerNotificationMaterial");
        TrapNotificationTexture = AssetBundle.LoadAsset<Material>("APTrapNotificationMaterial");
        ItemNotification = AssetBundle.LoadAsset<GameObject>("ItemNotification");
        HintNotification = AssetBundle.LoadAsset<GameObject>("HintNotification");
        TrapHome = AssetBundle.LoadAsset<GameObject>("TrapHome");
        TrapWide = AssetBundle.LoadAsset<GameObject>("TrapWideHappy");
        TrapJumpingJacks = AssetBundle.LoadAsset<GameObject>("TrapJumpingJacks");
        TrapPhoneCall = AssetBundle.LoadAsset<GameObject>("TrapPhoneCall");
        HomeTrapSprite = AssetBundle.LoadAsset<Sprite>("TrainHome");
        WideTrapSprite = AssetBundle.LoadAsset<Sprite>("WideTrap");
        JumpingJacksTrapSprite = AssetBundle.LoadAsset<Sprite>("JumpingJacksTrap");
        PhoneCallTrapSprite = AssetBundle.LoadAsset<Sprite>("NikoPhone");
        TinyTrapSprite = AssetBundle.LoadAsset<Sprite>("TinyTrap");
        SparksParticleSystem = AssetBundle.LoadAsset<GameObject>("Sparks");
        PartyTicketSprite = AssetBundle.LoadAsset<Sprite>("PartyInvitation");
        BonkHelmetSprite = AssetBundle.LoadAsset<Sprite>("BonkHelmet");
        BugNetSprite = AssetBundle.LoadAsset<Sprite>("BugNet");
        SodaRepairSprite = AssetBundle.LoadAsset<Sprite>("SodaCanRepair");
        ParasolRepairSprite = AssetBundle.LoadAsset<Sprite>("ParasolRepair");
        SwimCourseSprite = AssetBundle.LoadAsset<Sprite>("SwimCourse");
        TextboxItemSprite = AssetBundle.LoadAsset<Sprite>("TextboxItem");
        ACRepairSprite = AssetBundle.LoadAsset<Sprite>("ACRepair");
        TrapTiny = AssetBundle.LoadAsset<GameObject>("TrapTiny");
        NoticeBonkHelmet = AssetBundle.LoadAsset<GameObject>("NoticeBonkHelmet");
        NoticeSodaCan = AssetBundle.LoadAsset<GameObject>("NoticeSodaCan");
        NoticeParasol = AssetBundle.LoadAsset<GameObject>("NoticeParasol");
        NoticeSwimCourse = AssetBundle.LoadAsset<GameObject>("NoticeSwimCourse");
        NoticeAC = AssetBundle.LoadAsset<GameObject>("NoticeAC");
        DeathLinkSprite = AssetBundle.LoadAsset<Sprite>("DeathlinkSprite");
        DeathLinkNotice = AssetBundle.LoadAsset<GameObject>("NoticeDeathLink");
        NoticeBugNet = AssetBundle.LoadAsset<GameObject>("NoticeBugNet");
        NoticePartyTicket = AssetBundle.LoadAsset<GameObject>("NoticePartyTicket");
        AppleBasketSprite = AssetBundle.LoadAsset<Sprite>("AppleBasket");
        HairballBoneSprite = AssetBundle.LoadAsset<Sprite>("HairballBone");
        TurbineBoneSprite = AssetBundle.LoadAsset<Sprite>("TurbineBone");
        SalmonBoneSprite = AssetBundle.LoadAsset<Sprite>("SalmonBone");
        PoolBoneSprite = AssetBundle.LoadAsset<Sprite>("PoolBone");
        BathBoneSprite = AssetBundle.LoadAsset<Sprite>("BathhouseBone");
        TadpoleBoneSprite = AssetBundle.LoadAsset<Sprite>("TadpoleBone");
        NoticeAppleBasket = AssetBundle.LoadAsset<GameObject>("NoticeAppleBasket");
        TrapFast = AssetBundle.LoadAsset<GameObject>("TrapFast");
        DeathLinkPopup = AssetBundle.LoadAsset<GameObject>("DeathLinkPopup");
        HomeTextboxSprite = AssetBundle.LoadAsset<Sprite>("HomeTextboxItem");
        HairballTextboxSprite = AssetBundle.LoadAsset<Sprite>("HairballTextboxItem");
        TurbineTextboxSprite = AssetBundle.LoadAsset<Sprite>("TurbineTextboxItem");
        SalmonTextboxSprite = AssetBundle.LoadAsset<Sprite>("SalmonTextboxItem");
        PoolTextboxSprite = AssetBundle.LoadAsset<Sprite>("PoolTextboxItem");
        BathTextboxSprite = AssetBundle.LoadAsset<Sprite>("BathTextboxItem");
        TadpoleTextboxSprite = AssetBundle.LoadAsset<Sprite>("TadpoleTextboxItem");
        GardenTextboxSprite = AssetBundle.LoadAsset<Sprite>("GardenTextboxItem");
        NoticeBonkHelmetTree =  AssetBundle.LoadAsset<GameObject>("NoticeBonkHelmetTree");
    }

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
        { "homeTextbox", "HomeTextbox" },
        { "hcTextbox", "HairballTextbox" },
        { "ttTextbox", "TurbineTextbox" },
        { "scfTextbox", "SalmonTextbox" },
        { "ppTextbox", "PoolTextbox" },
        { "bathTextbox", "BathTextbox" },
        { "hqTextbox", "TadpoleTextbox" },
        { "ggTextbox", "GardenTextbox" },
    };

    private static readonly List<string> _progItems =
    [
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
        "homeTextbox",
        "hcTextbox",
        "ttTextbox",
        "scfTextbox",
        "ppTextbox",
        "bathTextbox",
        "hqTextbox",
        "ggTextbox",
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

        if (itemName == null)
            itemName = "Item: " + itemInfo.ItemId;
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
                "a very funny item",
                "needed for 100% completion!"
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

    public static Sprite SetSprite(long itemId)
    {
        var sprite = itemId switch
        {
            ItemID.Coin => CoinSprite,
            ItemID.Cassette => CassetteSprite,
            ItemID.Key => KeySprite,
            ItemID.SuperJump => SuperJumpSprite,
            ItemID.Letter => LetterSprite,
            ItemID.SnailMoney => SnailMoneySprite,
            ItemID.Bugs => BugsSprite,
            ItemID.Apples => ApplesSprite,
            ItemID.ContactList1 or ItemID.ContactList2 or ItemID.ProgressiveContactList => ContactListSprite,
            ItemID.GarysGardenTicket => GgSprite,
            ItemID.HairballCityTicket => HcSprite,
            ItemID.TurbineTownTicket => TtSprite,
            ItemID.SalmonCreekForestTicket => SfcSprite,
            ItemID.PublicPoolTicket => PpSprite,
            ItemID.BathhouseTicket => BathSprite,
            ItemID.TadpoleHqTicket => HqSprite,
            ItemID.HairballCityFish => HairballFishSprite,
            ItemID.TurbineTownFish => TurbineFishSprite,
            ItemID.SalmonCreekForestFish => SalmonFishSprite,
            ItemID.PublicPoolFish => PoolFishSprite,
            ItemID.BathhouseFish => BathFishSprite,
            ItemID.TadpoleHqFish => TadpoleFishSprite,
            ItemID.HairballCityKey => HairballKeySprite,
            ItemID.TurbineTownKey => TurbineKeySprite,
            ItemID.SalmonCreekForestKey => SalmonKeySprite,
            ItemID.PublicPoolKey => PoolKeySprite,
            ItemID.BathhouseKey => BathKeySprite,
            ItemID.TadpoleHqKey => TadpoleKeySprite,
            ItemID.HairballCityFlower => HairballFlowerSprite,
            ItemID.TurbineTownFlower => TurbineFlowerSprite,
            ItemID.SalmonCreekForestFlower => SalmonFlowerSprite,
            ItemID.PublicPoolFlower => PoolFlowerSprite,
            ItemID.BathhouseFlower => BathFlowerSprite,
            ItemID.TadpoleHqFlower => TadpoleFlowerSprite,
            ItemID.HairballCityCassette => HairballCassetteSprite,
            ItemID.TurbineTownCassette => TurbineCassetteSprite,
            ItemID.SalmonCreekForestCassette => SalmonCassetteSprite,
            ItemID.PublicPoolCassette => PoolCassetteSprite,
            ItemID.BathhouseCassette => BathCassetteSprite,
            ItemID.TadpoleHqCassette => TadpoleCassetteSprite,
            ItemID.GarysGardenCassette => GardenCassetteSprite,
            ItemID.HairballCitySeed => HairballSeedSprite,
            ItemID.SalmonCreekForestSeed => SalmonSeedSprite,
            ItemID.BathhouseSeed => BathSeedSprite,
            ItemID.FreezeTrap => FreezeTrapSprite,
            ItemID.IronBootsTrap => IronBootsTrapSprite,
            ItemID.WhoopsTrap => WhoopsTrapSprite,
            ItemID.MyTurnTrap => MyTurnTrapSprite,
            ItemID.SpeedBoost => SpeedBoostSprite,
            ItemID.HomeTrap => HomeTrapSprite,
            ItemID.WideTrap => WideTrapSprite,
            ItemID.PhoneTrap => PhoneCallTrapSprite,
            ItemID.TinyTrap => TinyTrapSprite,
            ItemID.GravityTrap => GravityTrapSprite,
            ItemID.JumpingJacksTrap => JumpingJacksTrapSprite,
            ItemID.PartyInvitation => PartyTicketSprite,
            ItemID.SafetyHelmet => BonkHelmetSprite,
            ItemID.BugNet => BugNetSprite,
            ItemID.SodaRepair => SodaRepairSprite,
            ItemID.ParasolRepair => ParasolRepairSprite,
            ItemID.SwimCourse => SwimCourseSprite,
            ItemID.Textbox => TextboxItemSprite,
            ItemID.AcRepair => ACRepairSprite,
            ItemID.AppleBasket => AppleBasketSprite,
            ItemID.HairballCityBone => HairballBoneSprite,
            ItemID.TurbineTownBone => TurbineBoneSprite,
            ItemID.SalmonCreekForestBone => SalmonBoneSprite,
            ItemID.PublicPoolBone => PoolBoneSprite,
            ItemID.BathhouseBone => BathBoneSprite,
            ItemID.TadpoleHqBone => TadpoleBoneSprite,
            ItemID.HomeTextbox => HomeTextboxSprite,
            ItemID.HairballCityTextbox => HairballTextboxSprite,
            ItemID.TurbineTownTextbox => TurbineTextboxSprite,
            ItemID.SalmonCreekForestTextbox => SalmonTextboxSprite,
            ItemID.PublicPoolTextbox => PoolTextboxSprite,
            ItemID.BathhouseTextbox => BathTextboxSprite,
            ItemID.TadpoleHqTextbox => TadpoleTextboxSprite,
            ItemID.GarysGardenTextbox => GardenTextboxSprite,
            _ => ApProgressionSprite
        };
        return sprite;
    }
}