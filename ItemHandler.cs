using ArchipelagoClient = NikoArchipelago.Archipelago.ArchipelagoClient;

namespace NikoArchipelago;

public static class ItemHandler
{
    private static ArchipelagoClient archipelagoClient;
    private static Plugin plugin;
    private static bool prog;
    public static bool Garden;
    public static void AddCoin(int amount = 1, string sender = "", bool notify = true)
    {
        scrGameSaveManager.instance.gameData.generalGameData.coinAmount += amount;
        if (notify)
        {
            Plugin.APSendNote(
                sender != ArchipelagoClient.ServerData.SlotName ? $"Received Coin from {sender}!" : "You found your Coin!",
                3f, Plugin.CoinSprite);
        }
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
        scrGameSaveManager.instance.SaveGame();
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
                3f);
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
    }

    public static void AddGarden(string sender, bool notify = true)
    {
        if (!notify) return;
        Plugin.APSendNote(
            sender != ArchipelagoClient.ServerData.SlotName ? $"Received Gary's Garden Ticket from {sender}!" : "You found your Gary's Garden Ticket!",
            4f, Plugin.ApProgressionSprite);
        Garden = true;
    }
}