using KinematicCharacterController.Core;

namespace NikoArchipelago.Patches;

public class CassetteCost
{
    private static scrCassetteBuyer buyer;
    public static void Update()
    {
        var level = scrWorldSaveDataContainer.instance.gameSaveManager.gameData.generalGameData.currentLevel - 1;
        switch (level)
        {
            case 1 when !scrWorldSaveDataContainer.instance.coinFlags[level].Contains("cassetteCoin") || !scrWorldSaveDataContainer.instance.coinFlags[level].Contains("cassetteCoin2"):
                buyer.price = 5;
                break;
            case 1:
                buyer.price = 10;
                break;
            case 2 when !scrWorldSaveDataContainer.instance.coinFlags[level].Contains("cassetteCoin") || !scrWorldSaveDataContainer.instance.coinFlags[level].Contains("cassetteCoin2"):
                buyer.price = 15;
                break;
            case 2:
                buyer.price = 20;
                break;
            case 3 when !scrWorldSaveDataContainer.instance.coinFlags[level].Contains("cassetteCoin") || !scrWorldSaveDataContainer.instance.coinFlags[level].Contains("cassetteCoin2"):
                buyer.price = 25;
                break;
            case 3:
                buyer.price = 30;
                break;
            case 4 when !scrWorldSaveDataContainer.instance.coinFlags[level].Contains("cassetteCoin") || !scrWorldSaveDataContainer.instance.coinFlags[level].Contains("cassetteCoin2"):
                buyer.price = 35;
                break;
            case 4:
                buyer.price = 40;
                break;
            case 5 when !scrWorldSaveDataContainer.instance.coinFlags[level].Contains("cassetteCoin") || !scrWorldSaveDataContainer.instance.coinFlags[level].Contains("cassetteCoin2"):
                buyer.price = 45;
                break;
            case 5:
                buyer.price = 50;
                break;
            case 6 when !scrWorldSaveDataContainer.instance.coinFlags[level].Contains("cassetteCoin") || !scrWorldSaveDataContainer.instance.coinFlags[level].Contains("cassetteCoin2"):
                buyer.price = 55;
                break;
            case 6:
                buyer.price = 60;
                break;
        }
    }
}