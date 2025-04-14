using System.Collections.Generic;
using System.Linq;
using KinematicCharacterController.Core;
using NikoArchipelago.Patches;
using NikoArchipelago.Stuff;
using UnityEngine;
using ArchipelagoClient = NikoArchipelago.Archipelago.ArchipelagoClient;

namespace NikoArchipelago;

public static class ItemHandler
{
    private static ArchipelagoClient archipelagoClient;
    private static Plugin plugin;
    private static bool prog;
    public static bool Garden;
    public static int HairballKeyAmount, TurbineKeyAmount, SalmonKeyAmount, PoolKeyAmount, BathKeyAmount, TadpoleKeyAmount,
        HairballFishAmount, TurbineFishAmount, SalmonFishAmount, PoolFishAmount, BathFishAmount, TadpoleFishAmount,
        HairballSeedAmount, SalmonSeedAmount, BathSeedAmount,
        HairballFlowerAmount, TurbineFlowerAmount, SalmonFlowerAmount, PoolFlowerAmount, BathFlowerAmount, TadpoleFlowerAmount,
        HairballCassetteAmount, TurbineCassetteAmount, SalmonCassetteAmount, PoolCassetteAmount, BathCassetteAmount, TadpoleCassetteAmount, GardenCassetteAmount;
    public static void AddCoin(int amount = 1, string sender = "", bool notify = true)
    {
        scrGameSaveManager.instance.gameData.generalGameData.coinAmount += amount;
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received Coin from {sender}!" : "You found your Coin!",
                3f, Plugin.CoinSprite);
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
                3f, Plugin.CassetteSprite);
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
                3f, Plugin.KeySprite);
        }
        ShowDisplayers.KeyDisplayer();
        scrGameSaveManager.instance.SaveGame();
    }
    
    public static void AddHCKey(string sender = "", bool notify = true)
    {
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received Hairball City Key from {sender}!" : "You found your Hairball City Key!",
                3f, Plugin.HairballKeySprite);
        }
    }
    
    public static void AddTTKey(string sender = "", bool notify = true)
    {
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received Turbine Town Key from {sender}!" : "You found your Turbine Town Key!",
                3f, Plugin.TurbineKeySprite);
        }
    }
    
    public static void AddSFCKey(string sender = "", bool notify = true)
    {
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received Salmon Creek Forest Key from {sender}!" : "You found your Salmon Creek Forest Key!",
                3f, Plugin.SalmonKeySprite);
        }
    }
    
    public static void AddPPKey(string sender = "", bool notify = true)
    {
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received Public Pool Key from {sender}!" : "You found your Public Pool Key!",
                3f, Plugin.PoolKeySprite);
        }
    }
    
    public static void AddBathKey(string sender = "", bool notify = true)
    {
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received Bathhouse Key from {sender}!" : "You found your Bathhouse Key!",
                3f, Plugin.BathKeySprite);
        }
    }
    
    public static void AddHQKey(string sender = "", bool notify = true)
    {
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received Tadpole HQ Key from {sender}!" : "You found your Tadpole HQ Key!",
                3f, Plugin.TadpoleKeySprite);
        }
    }

    public static void AddLetter(int amount = 1, string sender = "", bool notify = true)
    {
        scrGameSaveManager.instance.gameData.generalGameData.bottles += amount;
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received Letter from {sender}!" : "You found your Letter!",
                1.75f, Plugin.LetterSprite);
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
                1.75f, Plugin.ApplesSprite);
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
                1.75f, Plugin.BugSprite);
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
                1.75f, Plugin.SnailMoneySprite);
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
                3f, Plugin.SuperJumpSprite);
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
                3f, Plugin.ContactListSprite);
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
                3f, Plugin.ContactListSprite);
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
                    4f, Plugin.HcSprite);
                break;
            case 3:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Turbine Town Ticket from {sender}!" : "You found your Turbine Town Ticket!",
                    4f, Plugin.TtSprite);
                break;
            case 4:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Salmon Creek Forest Ticket from {sender}!" : "You found your Salmon Creek Forest Ticket!",
                    4f, Plugin.SfcSprite);
                break;
            case 5:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Public Pool Ticket from {sender}!" : "You found your Public Pool Ticket!",
                    4f, Plugin.PpSprite);
                break;
            case 6:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Bathhouse Ticket from {sender}!" : "You found your Bathhouse Ticket!",
                    4f, Plugin.BathSprite);
                break;
            case 7:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Tadpole HQ Ticket from {sender}!" : "You found your Tadpole HQ Ticket!",
                    4f, Plugin.HqSprite);
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
            4f, Plugin.GgSprite);
        Garden = true;
        ShowDisplayers.TicketDisplayer();
        scrGameSaveManager.instance.SaveGame();
    }
    
    public static void AddHcFish(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Hairball City Fish from {sender}!" : "You found your Hairball City Fish!",
            3f, Plugin.HairballFishSprite);
    }
    
    public static void AddTtFish(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Turbine Town Fish from {sender}!" : "You found your Turbine Town Fish!",
            3f, Plugin.TurbineFishSprite);
    }
    
    public static void AddSfcFish(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Salmon Creek Forest Fish from {sender}!" : "You found your Salmon Creek Forest Fish!",
            3f, Plugin.SalmonFishSprite);
    }
    
    public static void AddPpFish(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Public Pool Fish from {sender}!" : "You found your Public Pool Fish!",
            3f, Plugin.PoolFishSprite);
    }
    
    public static void AddBathFish(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Bathhouse Fish from {sender}!" : "You found your Bathhouse Fish!",
            3f, Plugin.BathFishSprite);
    }
    
    public static void AddHqFish(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Tadpole HQ Fish from {sender}!" : "You found your Tadpole HQ Fish!",
            3f, Plugin.TadpoleFishSprite);
    }
    
    public static void AddHcSeed(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Hairball City Seed from {sender}!" : "You found your Hairball City Seed!",
            3f, Plugin.HairballSeedSprite);
    }
    
    public static void AddSfcSeed(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Salmon Creek Forest Seed from {sender}!" : "You found your Salmon Creek Forest Seed!",
            3f, Plugin.SalmonSeedSprite);
    }
    
    public static void AddBathSeed(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Bathhouse Seed from {sender}!" : "You found your Bathhouse Seed!",
            3f, Plugin.BathSeedSprite);
    }
    
    public static void AddHcFlower(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Hairball City Flower from {sender}!" : "You found your Hairball City Flower!",
            3f, Plugin.HairballFlowerSprite);
    }
    
    public static void AddTtFlower(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Turbine Town Flower from {sender}!" : "You found your Turbine Town Flower!",
            3f, Plugin.TurbineFlowerSprite);
    }
    
    public static void AddSfcFlower(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Salmon Creek Forest Flower from {sender}!" : "You found your Salmon Creek Forest Flower!",
            3f, Plugin.SalmonFlowerSprite);
    }
    
    public static void AddPpFlower(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Public Pool Flower from {sender}!" : "You found your Public Pool Flower!",
            3f, Plugin.PoolFlowerSprite);
    }
    
    public static void AddBathFlower(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Bathhouse Flower from {sender}!" : "You found your Bathhouse Flower!",
            3f, Plugin.BathFlowerSprite);
    }
    
    public static void AddHqFlower(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Tadpole HQ Flower from {sender}!" : "You found your Tadpole HQ Flower!",
            3f, Plugin.TadpoleFlowerSprite);
    }
    
    // Level Based Cassettes
    public static void AddHcCassette(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Hairball City Cassette from {sender}!" : "You found your Hairball City Cassette!",
            3f, Plugin.HairballCassetteSprite);
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
            3f, Plugin.TurbineCassetteSprite);
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
            3f, Plugin.SalmonCassetteSprite);
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
            3f, Plugin.PoolCassetteSprite);
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
            3f, Plugin.BathCassetteSprite);
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
            3f, Plugin.TadpoleCassetteSprite);
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
            3f, Plugin.GardenCassetteSprite);
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
            3f, Plugin.SpeedBoostSprite);
        Plugin.MovementSpeedMultiplier();
    }
    
    public static void AddFreezeTrap(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Freeze Trap from {sender}!" : "You found your Freeze Trap!",
            3f, Plugin.FreezeTrapSprite);
        TrapManager.FreezeOn = true;
    }
    
    public static void AddIronBootsTrap(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Iron Boots Trap from {sender}!" : "You found your Iron Boots Trap!",
            3f, Plugin.IronBootsTrapSprite);
        TrapManager.IronBootsOn = true;
    }
    
    public static void AddWhoopsTrap(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Whoops! Trap from {sender}!" : "You found your Whoops Trap!",
            3f, Plugin.WhoopsTrapSprite);
        TrapManager.WhoopsOn = true;
        MyCharacterController.instance.requestNewPosition(new Vector3(0, 450, 0));
    }
    
    public static void AddMyTurnTrap(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received My Turn! Trap from {sender}!" : "You found your My Turn Trap!",
            3f, Plugin.MyTurnTrapSprite);
        TrapManager.MyTurnOn = true;
    }
    public static void AddGravityTrap(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Gravity Trap from {sender}!" : "You found your Gravity Trap!",
            3f, Plugin.GravityTrapSprite);
        TrapManager.GravityOn = true;
    }
    // NPCs
    // public static void AddHcNPCs(string sender, bool notify = true)
    // {
    //     if (!notify) return;
    //     Plugin.APSendNote(
    //         sender != ArchipelagoClient.ServerData.SlotName ? $"Received Hairball City NPCs from {sender}!" : "You found your Hairball City NPCs!",
    //         3f, Plugin.HairballFishSprite);
    // }
    //
    // public static void AddTtNPCs(string sender, bool notify = true)
    // {
    //     if (!notify) return;
    //     Plugin.APSendNote(
    //         sender != ArchipelagoClient.ServerData.SlotName ? $"Received Turbine Town NPCs from {sender}!" : "You found your Turbine Town NPCs!",
    //         3f, Plugin.TurbineFishSprite);
    // }
    //
    // public static void AddSfcNPCs(string sender, bool notify = true)
    // {
    //     if (!notify) return;
    //     Plugin.APSendNote(
    //         sender != ArchipelagoClient.ServerData.SlotName ? $"Received Salmon Creek Forest NPCs from {sender}!" : "You found your Salmon Creek Forest NPCs!",
    //         3f, Plugin.SalmonFishSprite);
    // }
    //
    // public static void AddPpNPCs(string sender, bool notify = true)
    // {
    //     if (!notify) return;
    //     Plugin.APSendNote(
    //         sender != ArchipelagoClient.ServerData.SlotName ? $"Received Public Pool NPCs from {sender}!" : "You found your Public Pool NPCs!",
    //         3f, Plugin.PoolFishSprite);
    // }
    //
    // public static void AddBathNPCs(string sender, bool notify = true)
    // {
    //     if (!notify) return;
    //     Plugin.APSendNote(
    //         sender != ArchipelagoClient.ServerData.SlotName ? $"Received Bathhouse NPCs from {sender}!" : "You found your Bathhouse NPCs!",
    //         3f, Plugin.BathFishSprite);
    // }
    //
    // public static void AddHqNPCs(string sender, bool notify = true)
    // {
    //     if (!notify) return;
    //     Plugin.APSendNote(
    //         sender != ArchipelagoClient.ServerData.SlotName ? $"Received Tadpole HQ NPCs from {sender}!" : "You found your Tadpole HQ NPCs!",
    //         3f, Plugin.TadpoleFishSprite);
    // }
}