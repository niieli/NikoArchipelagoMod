namespace NikoArchipelago;

public static class ItemHandler
{
    public static void AddCoin(int amount = 1)
    {
        scrGameSaveManager.instance.gameData.generalGameData.coinAmount += amount;
    }

    public static void AddCassette(int amount = 1)
    {
        scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount += amount;
    }

    public static void AddKey(int amount = 1)
    {
        scrGameSaveManager.instance.gameData.generalGameData.keyAmount += amount;
    }

    public static void AddLetter(int amount = 1)
    {
        scrGameSaveManager.instance.gameData.generalGameData.bottles += amount;
    }

    public static void AddApples(int amount = 25)
    {
        scrGameSaveManager.instance.gameData.generalGameData.appleAmount += amount;
    }

    public static void AddSuperJump()
    {
        scrGameSaveManager.instance.gameData.generalGameData.secretMove = true;
    }

    public static void AddContactList1()
    {
        scrGameSaveManager.instance.gameData.generalGameData.wave1 = true;
    }

    public static void AddContactList2()
    {
        scrGameSaveManager.instance.gameData.generalGameData.wave2 = true;
    }
}