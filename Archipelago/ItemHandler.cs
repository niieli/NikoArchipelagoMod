using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net.Models;
using NikoArchipelago.Stuff;

namespace NikoArchipelago.Archipelago;

public static class ItemHandler
{
    public static bool Garden;

    public static int HairballKeyAmount,
        TurbineKeyAmount,
        SalmonKeyAmount,
        PoolKeyAmount,
        BathKeyAmount,
        TadpoleKeyAmount;

    public static int HairballFishAmount, 
        TurbineFishAmount,
        SalmonFishAmount,
        PoolFishAmount,
        BathFishAmount,
        TadpoleFishAmount;

    public static int HairballSeedAmount, SalmonSeedAmount, BathSeedAmount;

    public static int HairballFlowerAmount,
        TurbineFlowerAmount,
        SalmonFlowerAmount,
        PoolFlowerAmount,
        BathFlowerAmount,
        TadpoleFlowerAmount;

    public static int HairballCassetteAmount,
        TurbineCassetteAmount,
        SalmonCassetteAmount,
        PoolCassetteAmount,
        BathCassetteAmount,
        TadpoleCassetteAmount,
        GardenCassetteAmount;
    
    public static int HairballBoneAmount, TurbineBoneAmount, SalmonBoneAmount, PoolBoneAmount, BathBoneAmount, TadpoleBoneAmount;
    
    public static int GarysGardenSeedAmount;
    
    public static void AddCoin(int amount = 1, string sender = "", bool notify = true)
    {
        scrGameSaveManager.instance.gameData.generalGameData.coinAmount += amount;
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received Coin from {sender}!" : "You found your Coin!",
                3f, Assets.CoinSprite);
        }
        ShowDisplayers.CoinDisplayer();
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddCassette(int amount = 1, string sender = "", bool notify = true)
    {
        scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount += amount;
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received Cassette from {sender}!" : "You found your Cassette!",
                3f, Assets.CassetteSprite);
        }
        ShowDisplayers.CassetteDisplayer();
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddKey(int amount = 1, string sender = "", bool notify = true)
    {
        scrGameSaveManager.instance.gameData.generalGameData.keyAmount += amount;
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received Key from {sender}!" : "You found your Key!",
                3f, Assets.KeySprite);
        }
        ShowDisplayers.KeyDisplayer();
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddLetter(int amount = 1, string sender = "", bool notify = true)
    {
        scrGameSaveManager.instance.gameData.generalGameData.bottles += amount;
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received Letter from {sender}!" : "You found your Letter!",
                1.75f, Assets.LetterSprite);
        }
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddApples(int amount = 25, string sender = "", bool notify = true)
    {
        scrGameSaveManager.instance.gameData.generalGameData.appleAmount += amount;
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received {amount} Apples from {sender}!" : $"You found your {amount} Apples!",
                1.75f, Assets.ApplesSprite);
        }
        ShowDisplayers.AppleDisplayer();
        scrGameSaveManager.instance.SaveGame();
    }
    
    public static void AddBugs(int amount = 10, string sender = "", bool notify = true)
    {
        var w = scrGameSaveManager.instance.gameData.worldsData;
        scrWorldSaveDataContainer.instance.bugAmount += amount;
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received {amount} Bugs from {sender}!" : $"You found your {amount} Bugs!",
                1.75f, Assets.BugsSprite);
        }
        ShowDisplayers.BugDisplayer();
        scrGameSaveManager.instance.SaveGame();
    }
    
    public static void AddMoney(int amount = 500, string sender = "", bool notify = true)
    {
        var w = scrGameSaveManager.instance.gameData.worldsData;
        scrGameSaveManager.instance.gameData.generalGameData.snailSteps += amount;
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received {amount} Snail Money from {sender}!" : $"You found your {amount} Snail Money!",
                1.75f, Assets.SnailMoneySprite);
        }
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddSuperJump(string sender = "", bool notify = true)
    {
        scrGameSaveManager.instance.gameData.generalGameData.secretMove = true;
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received Super Jump from {sender}!" : "You found your Super Jump!",
                3f, Assets.SuperJumpSprite);
        }
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddContactList1(string sender = "", bool notify = true)
    {
        if (!scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("APWave1"))
        {
            scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Add("APWave1");
            scrGameSaveManager.instance.gameData.generalGameData.newIconLevels[1] = true;
            scrGameSaveManager.instance.gameData.generalGameData.newIconLevels[2] = true;
            scrGameSaveManager.instance.gameData.generalGameData.newIconLevels[3] = true;
        }
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received Contact List 1 from {sender}!" : "You found your Contact List 1!",
                3f, Assets.ContactListSprite);
        }
        scrWaveCheck.doCheck = true;
        ShowDisplayers.TicketDisplayer();
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddContactList2(string sender = "", bool notify = true)
    {
        if (!scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("APWave2"))
        {
            scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Add("APWave2");
            scrGameSaveManager.instance.gameData.generalGameData.newIconLevels[1] = true;
            scrGameSaveManager.instance.gameData.generalGameData.newIconLevels[2] = true;
            scrGameSaveManager.instance.gameData.generalGameData.newIconLevels[3] = true;
            scrGameSaveManager.instance.gameData.generalGameData.newIconLevels[4] = true;
            scrGameSaveManager.instance.gameData.generalGameData.newIconLevels[5] = true;
        }
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received Contact List 2 from {sender}!" : "You found your Contact List 2!",
                3f, Assets.ContactListSprite);
        }
        scrWaveCheck.doCheck = true;
        ShowDisplayers.TicketDisplayer();
        scrGameSaveManager.instance.SaveGame();
    }

    /// <summary>
    /// Unlocks the specified level
    /// </summary>
    /// <param name="level">The level in question 1=Home etc.</param>
    /// <param name="sender">The Name of the sender</param>
    public static void AddTicket(int level, string sender, bool notify = true)
    {
        if (sender == "Server")
        {
            sender = "Starting Inventory";
        }
        scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[level] = true;
        scrGameSaveManager.instance.SaveGame();
        if (!notify) return;
        switch (level)
        {
            case 2:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Hairball City Ticket from {sender}!" : "You found your Hairball City Ticket!",
                    4f, Assets.HcSprite);
                break;
            case 3:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Turbine Town Ticket from {sender}!" : "You found your Turbine Town Ticket!",
                    4f, Assets.TtSprite);
                break;
            case 4:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Salmon Creek Forest Ticket from {sender}!" : "You found your Salmon Creek Forest Ticket!",
                    4f, Assets.SfcSprite);
                break;
            case 5:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Public Pool Ticket from {sender}!" : "You found your Public Pool Ticket!",
                    4f, Assets.PpSprite);
                break;
            case 6:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Bathhouse Ticket from {sender}!" : "You found your Bathhouse Ticket!",
                    4f, Assets.BathSprite);
                break;
            case 7:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Tadpole HQ Ticket from {sender}!" : "You found your Tadpole HQ Ticket!",
                    4f, Assets.HqSprite);
                break;
        }
        ShowDisplayers.TicketDisplayer();
    }

    public static int UsedKeys()
    {
        var worldsData = scrGameSaveManager.instance.gameData.worldsData;
        var keyFlags = new List<string>
        {
            "lock1",
            "mahjonglock",
            "Officelock",
            "lock2",
            "TurbineLock",
            "1"
        };
        return worldsData.Sum(world => world.miscFlags.Count(flag => keyFlags.Any(flag.Equals)));
    }
    
    public static int UsedKeysHairball()
    {
        var worldsData = scrGameSaveManager.instance.gameData.worldsData;
        var keyFlags = new List<string>
        {
            "lock1",
            "mahjonglock",
            "Officelock",
            "lock2",
            "TurbineLock",
            "1"
        };
        return worldsData[1].miscFlags.Count(flag => keyFlags.Any(flag.Equals));
    }
    
    public static int UsedKeysTurbine()
    {
        var worldsData = scrGameSaveManager.instance.gameData.worldsData;
        var keyFlags = new List<string>
        {
            "lock1",
            "mahjonglock",
            "Officelock",
            "lock2",
            "TurbineLock",
            "1"
        };
        return worldsData[2].miscFlags.Count(flag => keyFlags.Any(flag.Equals));
    }
    
    public static int UsedKeysSalmon()
    {
        var worldsData = scrGameSaveManager.instance.gameData.worldsData;
        var keyFlags = new List<string>
        {
            "lock1",
            "mahjonglock",
            "Officelock",
            "lock2",
            "TurbineLock",
            "1"
        };
        return worldsData[3].miscFlags.Count(flag => keyFlags.Any(flag.Equals));
    }
    
    public static int UsedKeysPool()
    {
        var worldsData = scrGameSaveManager.instance.gameData.worldsData;
        var keyFlags = new List<string>
        {
            "lock1",
            "mahjonglock",
            "Officelock",
            "lock2",
            "TurbineLock",
            "1"
        };
        return worldsData[4].miscFlags.Count(flag => keyFlags.Any(flag.Equals));
    }
    
    public static int UsedKeysBath()
    {
        var worldsData = scrGameSaveManager.instance.gameData.worldsData;
        var keyFlags = new List<string>
        {
            "lock1",
            "mahjonglock",
            "Officelock",
            "lock2",
            "TurbineLock",
            "1"
        };
        return worldsData[5].miscFlags.Count(flag => keyFlags.Any(flag.Equals));
    }
    
    public static int UsedKeysTadpole()
    {
        var worldsData = scrGameSaveManager.instance.gameData.worldsData;
        var keyFlags = new List<string>
        {
            "lock1",
            "mahjonglock",
            "Officelock",
            "lock2",
            "TurbineLock",
            "1"
        };
        return worldsData[6].miscFlags.Count(flag => keyFlags.Any(flag.Equals));
    }

    public static void AddGarden(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Gary's Garden Ticket from {sender}!" : "You found your Gary's Garden Ticket!",
            4f, Assets.GgSprite);
        Garden = true;
        ShowDisplayers.TicketDisplayer();
        scrGameSaveManager.instance.SaveGame();
    }
    
    // Level Based Cassettes
    public static void AddHcCassette(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Hairball City Cassette from {sender}!" : "You found your Hairball City Cassette!",
            3f, Assets.HairballCassetteSprite);
        if (scrGameSaveManager.instance.gameData.generalGameData.currentLevel == 2)
        {
            ShowDisplayers.CassetteDisplayer();
        }
    }
    
    public static void AddTtCassette(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Turbine Town Cassette from {sender}!" : "You found your Turbine Town Cassette!",
            3f, Assets.TurbineCassetteSprite);
        if (scrGameSaveManager.instance.gameData.generalGameData.currentLevel == 3)
        {
            ShowDisplayers.CassetteDisplayer();
        }
    }
    
    public static void AddSfcCassette(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Salmon Creek Forest Cassette from {sender}!" : "You found your Salmon Creek Forest Cassette!",
            3f, Assets.SalmonCassetteSprite);
        if (scrGameSaveManager.instance.gameData.generalGameData.currentLevel == 4)
        {
            ShowDisplayers.CassetteDisplayer();
        }
    }
    
    public static void AddPpCassette(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Public Pool Cassette from {sender}!" : "You found your Public Pool Cassette!",
            3f, Assets.PoolCassetteSprite);
        if (scrGameSaveManager.instance.gameData.generalGameData.currentLevel == 5)
        {
            ShowDisplayers.CassetteDisplayer();
        }
    }
    
    public static void AddBathCassette(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Bathhouse Cassette from {sender}!" : "You found your Bathhouse Cassette!",
            3f, Assets.BathCassetteSprite);
        if (scrGameSaveManager.instance.gameData.generalGameData.currentLevel == 6)
        {
            ShowDisplayers.CassetteDisplayer();
        }
    }
    
    public static void AddHqCassette(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Tadpole HQ Cassette from {sender}!" : "You found your Tadpole HQ Cassette!",
            3f, Assets.TadpoleCassetteSprite);
        if (scrGameSaveManager.instance.gameData.generalGameData.currentLevel == 7)
        {
            ShowDisplayers.CassetteDisplayer();
        }
    }
    public static void AddGgCassette(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Gary's Garden Cassette from {sender}!" : "You found your Gary's Garden Cassette!",
            3f, Assets.GardenCassetteSprite);
        if (scrGameSaveManager.instance.gameData.generalGameData.currentLevel == 24)
        {
            ShowDisplayers.CassetteDisplayer();
        }
    }
    
    public static void AddSpeedBoost(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Speed Boost from {sender}!" : "You found your Speed Boost!",
            3f, Assets.SpeedBoostSprite);
        MovementSpeed.MovementSpeedMultiplier();
    }
    
    public static void AddFreezeTrap(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Freeze Trap from {sender}!" : "You found your Freeze Trap!",
            3f, Assets.FreezeTrapSprite);
        TrapManager.FreezeOn = true;
    }
    
    public static void AddIronBootsTrap(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Iron Boots Trap from {sender}!" : "You found your Iron Boots Trap!",
            3f, Assets.IronBootsTrapSprite);
        TrapManager.IronBootsOn = true;
    }
    
    public static void AddWhoopsTrap(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Whoops! Trap from {sender}!" : "You found your Whoops Trap!",
            3f, Assets.WhoopsTrapSprite);
        TrapManager.WhoopsOn = true;
    }
    
    public static void AddMyTurnTrap(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received My Turn! Trap from {sender}!" : "You found your My Turn Trap!",
            3f, Assets.MyTurnTrapSprite);
        TrapManager.MyTurnOn = true;
    }
    public static void AddGravityTrap(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Gravity Trap from {sender}!" : "You found your Gravity Trap!",
            3f, Assets.GravityTrapSprite);
        TrapManager.GravityOn = true;
    }
    public static void AddWideTrap(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Wide Trap from {sender}!" : "You found your Wide Trap!",
            3f, Assets.WideTrapSprite);
        TrapManager.WideOn = true;
    }
    public static void AddHomeTrap(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Home Trap from {sender}!" : "You found your Home Trap!",
            3f, Assets.HomeTrapSprite);
        TrapManager.HomeOn = true;
    }
    public static void AddPhoneTrap(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Phone Trap from {sender}!" : "You found your Phone Trap!",
            3f, Assets.PhoneCallTrapSprite);
        TrapManager.PhoneOn = true;
    }
    public static void AddTinyTrap(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Tiny Trap from {sender}!" : "You found your Tiny Trap!",
            3f, Assets.TinyTrapSprite);
        TrapManager.TinyOn = true;
    }
    public static void AddJumpingJacksTrap(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Jumping Jacks Trap from {sender}!" : "You found your Jumping Jacks Trap!",
            3f, Assets.JumpingJacksTrapSprite);
        TrapManager.JumpingJacksOn = true;
    }
    public static void AddPartyInvitation(ItemInfo itemInfo, bool notify = true)
    {
        if (!notify) return;
        var sender = itemInfo.Player.Name;
        var itemName = itemInfo.ItemName ?? "Item: " + itemInfo.ItemId;
        var itemId = itemInfo.ItemId;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received {itemName} from {sender}!" : $"You found your {itemName}!",
            3f, Assets.SetSprite(itemId));
        NotificationManager.ShowParty = true;
    }
    public static void AddGarysGardenSeed(ItemInfo itemInfo, bool notify = true)
    {
        if (!notify) return;
        var sender = itemInfo.Player.Name;
        var itemName = itemInfo.ItemName ?? "Item: " + itemInfo.ItemId;
        var itemId = itemInfo.ItemId;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received {itemName} from {sender}!" : $"You found your {itemName}!",
            3f, Assets.SetSprite(itemId));
        if (scrGameSaveManager.instance.gameData.generalGameData.currentLevel == 24)
        {
            scrMiscFlagCountSwitch.instance.Rise();
        }
        ShowDisplayers.GardenSeedDisplayer();
    }
    public static void AddItemNote(ItemInfo itemInfo, bool notify = true)
    {
        if (!notify) return;
        var sender = itemInfo.Player.Name;
        var itemName = itemInfo.ItemName ?? "Item: " + itemInfo.ItemId;
        var itemId = itemInfo.ItemId;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received {itemName} from {sender}!" : $"You found your {itemName}!",
            3f, Assets.SetSprite(itemId));
    }
}