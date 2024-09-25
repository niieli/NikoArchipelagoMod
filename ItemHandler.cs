using ArchipelagoClient = NikoArchipelago.Archipelago.ArchipelagoClient;

namespace NikoArchipelago;

public static class ItemHandler
{
    private static ArchipelagoClient archipelagoClient;
    private static Plugin plugin;

    public static void AddCoin(int amount = 1, string sender = "")
    {
        scrGameSaveManager.instance.gameData.generalGameData.coinAmount += amount;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Coin from {sender}!" : "You found your Coin!",
            3f);
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddCassette(int amount = 1, string sender = "")
    {
        scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount += amount;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Cassette from {sender}!" : "You found your Cassette!",
            3f);
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddKey(int amount = 1, string sender = "")
    {
        scrGameSaveManager.instance.gameData.generalGameData.keyAmount += amount;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Key from {sender}!" : "You found your Key!",
            3f);
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddLetter(int amount = 1, string sender = "")
    {
        scrGameSaveManager.instance.gameData.generalGameData.bottles += amount;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Letter from {sender}!" : "You found your Letter!",
            1.75f);
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddApples(int amount = 25, string sender = "")
    {
        scrGameSaveManager.instance.gameData.generalGameData.appleAmount += amount;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received {amount} Apples from {sender}!" : $"You found your {amount} Apples!",
            1.75f);
        scrGameSaveManager.instance.SaveGame();
    }
    
    public static void AddBugs(int amount = 10, string sender = "")
    {
        var w = scrGameSaveManager.instance.gameData.worldsData;
        w[scrGameSaveManager.instance.gameData.generalGameData.currentLevel-1].bugAmount += amount;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received {amount} Bugs from {sender}!" : $"You found your {amount} Bugs!",
            1.75f);
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddSuperJump(string sender = "")
    {
        scrGameSaveManager.instance.gameData.generalGameData.secretMove = true;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Super Jump from {sender}!" : "You found your Super Jump!",
            3f);
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddContactList1(string sender = "")
    {
        scrGameSaveManager.instance.gameData.generalGameData.wave1 = true;
        if (!scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("APWave1"))
        {
            scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Add("APWave1");
        }        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Contact List 1 from {sender}!" : "You found your Contact List 1!",
            3f);
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddContactList2(string sender = "")
    {
        scrGameSaveManager.instance.gameData.generalGameData.wave2 = true;
        if (!scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Contains("APWave2"))
        {
            scrGameSaveManager.instance.gameData.generalGameData.generalFlags.Add("APWave2");
        }
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Contact List 2 from {sender}!" : "You found your Contact List 2!",
            3f);
        scrGameSaveManager.instance.SaveGame();
    }

    /// <summary>
    /// Unlocks the specified level
    /// </summary>
    /// <param name="level">The level in question 1=Home etc.</param>
    /// <param name="sender">The Name of the sender</param>
    public static void AddTicket(int level, string sender)
    {
        if (sender == "Server")
        {
            sender = "Starting Inventory";
        }
        scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[level] = true;
        switch (level)
        {
            case 2:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Hairball City Ticket from {sender}!" : "You found your Hairball City Ticket!",
                    4f);
                break;
            case 3:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Turbine Town Ticket from {sender}!" : "You found your Turbine Town Ticket!",
                    4f);
                break;
            case 4:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Salmon Creek Forest Ticket from {sender}!" : "You found your Salmon Creek Forest Ticket!",
                    4f);
                break;
            case 5:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Public Pool Ticket from {sender}!" : "You found your Public Pool Ticket!",
                    4f);
                break;
            case 6:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Bathhouse Ticket from {sender}!" : "You found your Bathhouse Ticket!",
                    4f);
                break;
            case 7:
                Plugin.APSendNote(
                    sender != ArchipelagoClient.ServerData.SlotName ? $"Received Tadpole HQ Ticket from {sender}!" : "You found your Tadpole HQ Ticket!",
                    4f);
                break;
        }
        scrGameSaveManager.instance.SaveGame();
    }

    // public static void SycnDataFromServer()
    // {
    //     scrGameSaveManager.instance.gameData.generalGameData.coinAmount = 0;
    //     scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount = 0;
    //     scrGameSaveManager.instance.gameData.generalGameData.keyAmount = 0;
    //     scrGameSaveManager.instance.gameData.generalGameData.wave1 = false;
    //     scrGameSaveManager.instance.gameData.generalGameData.wave2 = false;
    //     scrGameSaveManager.instance.gameData.generalGameData.secretMove = false;
    //     scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[2] = false;
    //     scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[3] = false;
    //     scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[4] = false;
    //     scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[5] = false;
    //     scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[6] = false;
    //     scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[7] = false;
    //     switch (ArchipelagoClient.SyncInventory())
    //     {
    //         case 598_145_444_000: // Coin
    //             scrGameSaveManager.instance.gameData.generalGameData.coinAmount++;
    //             break;
    //         case 598_145_444_000 + 1: // Cassette
    //             scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount++;
    //             break;
    //         case 598_145_444_000 + 2: // Key
    //             scrGameSaveManager.instance.gameData.generalGameData.keyAmount++;
    //             break;
    //         case 598_145_444_000 + 3: // Apples
    //             scrGameSaveManager.instance.gameData.generalGameData.appleAmount+=25;
    //             break;
    //         case 598_145_444_000 + 4: // Contact List 1
    //             scrGameSaveManager.instance.gameData.generalGameData.wave1 = true;
    //             break;
    //         case 598_145_444_000 + 5: // Contact List 2
    //             scrGameSaveManager.instance.gameData.generalGameData.wave2 = true;
    //             break;
    //         case 598_145_444_000 + 6: // Super Jump
    //             scrGameSaveManager.instance.gameData.generalGameData.secretMove = true;
    //             break;
    //         case 598_145_444_000+7:
    //             //ItemHandler.AddLetter(1, senderName);
    //             break;
    //         case 598_145_444_000+8:
    //             scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[2] = true;
    //             break;
    //         case 598_145_444_000+9:
    //             scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[3] = true;
    //             break;
    //         case 598_145_444_000+10:
    //             scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[4] = true;
    //             break;
    //         case 598_145_444_000+11:
    //             scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[5] = true;
    //             break;
    //         case 598_145_444_000+12:
    //             scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[6] = true;
    //             break;
    //         case 598_145_444_000+13:
    //             scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[7] = true;
    //             break;
    //     }
    //}
}