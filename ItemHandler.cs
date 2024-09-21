using NikoArchipelago.Archipelago;

namespace NikoArchipelago;

public static class ItemHandler
{
    private static ArchipelagoClient archipelagoClient;
    public static int TotalCoins;
    public static void AddCoin(int amount = 1)
    {
        scrGameSaveManager.instance.gameData.generalGameData.coinAmount += amount;
        TotalCoins++;
        scrGameSaveManager.instance.SaveGame();
        //scrGameSaveManager.instance.gameData.generalGameData.coinAmount = archipelagoClient.Coins;
    }

    public static void AddCassette(int amount = 1)
    {
        scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount += amount;
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddKey(int amount = 1)
    {
        scrGameSaveManager.instance.gameData.generalGameData.keyAmount += amount;
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddLetter(int amount = 1)
    {
        scrGameSaveManager.instance.gameData.generalGameData.bottles += amount;
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddApples(int amount = 25)
    {
        scrGameSaveManager.instance.gameData.generalGameData.appleAmount += amount;
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddSuperJump()
    {
        scrGameSaveManager.instance.gameData.generalGameData.secretMove = true;
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddContactList1()
    {
        scrGameSaveManager.instance.gameData.generalGameData.wave1 = true;
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddContactList2()
    {
        scrGameSaveManager.instance.gameData.generalGameData.wave2 = true;
        scrGameSaveManager.instance.SaveGame();
    }

    /// <summary>
    /// Unlocks the specified level
    /// </summary>
    /// <param name="level">The level in question 1=Home etc.</param>
    public static void AddTicket(int level)
    {
        scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[level] = true;
        scrGameSaveManager.instance.SaveGame();
    }
}