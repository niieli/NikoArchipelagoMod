using System.Collections.Generic;

namespace NikoArchipelago;

public class Location
{
    public long ID { get; set; }
    public string Flag { get; set; }
    public int Level { get; set; }

    public Location(long id, string flag, int level)
    {
        ID = id;
        Flag = flag;
        Level = level;
    }
}

public class Locations
{
    private const long BaseID = 598_145_444_000;

    public static Dictionary<long, Location> CoinLocations = new()
    {
    // Home
    { BaseID + 0, new Location(BaseID + 0, "Fetch", 0) },

    // Hairball City
    { BaseID + 3, new Location(BaseID + 3, "volley", 1) },
    { BaseID + 4, new Location(BaseID + 4, "Dustan", 1) },
    { BaseID + 5, new Location(BaseID + 5, "flowerPuzzle", 1) },
    { BaseID + 6, new Location(BaseID + 6, "main", 1) },
    { BaseID + 7, new Location(BaseID + 7, "fishing", 1) },
    { BaseID + 8, new Location(BaseID + 8, "bug", 1) },
    // { BaseID + 9, new Location(BaseID + 9, "Nina", 1) },
    // { BaseID + 10, new Location(BaseID + 10, "Moomy", 1) },
    // { BaseID + 11, new Location(BaseID + 11, "giveMitch5Cassettes", 1) },
    // { BaseID + 12, new Location(BaseID + 12, "giveMai5Cassettes", 1) },
    // { BaseID + 13, new Location(BaseID + 13, "gameKid", 1) },
    // { BaseID + 14, new Location(BaseID + 14, "blippyDog", 1) },
    // { BaseID + 15, new Location(BaseID + 15, "blippyCoin", 1) },
    // { BaseID + 16, new Location(BaseID + 16, "SerschelAndLouist", 1) },

    // Turbine Town
    { BaseID + 17, new Location(BaseID + 17, "fishing", 2) },
    { BaseID + 18, new Location(BaseID + 18, "volley", 2) },
    { BaseID + 19, new Location(BaseID + 19, "flowerPuzzle", 2) },
    { BaseID + 20, new Location(BaseID + 20, "main", 2) },
    { BaseID + 21, new Location(BaseID + 21, "bug", 2) },
    { BaseID + 22, new Location(BaseID + 22, "Dustan", 2) },
    // { BaseID + 23, new Location(BaseID + 23, "giveMitch5Cassettes", 2) },
    // { BaseID + 24, new Location(BaseID + 24, "giveMai5Cassettes", 2) },
    // { BaseID + 25, new Location(BaseID + 25, "blippyDog", 2) },
    // { BaseID + 26, new Location(BaseID + 26, "blippyCoin", 2) },
    // { BaseID + 27, new Location(BaseID + 27, "SerschelAndLouist", 2) },

    // Salmon Creek Forest
    { BaseID + 28, new Location(BaseID + 28, "main", 3) },
    { BaseID + 29, new Location(BaseID + 29, "cassetteCoin", 3) },
    { BaseID + 30, new Location(BaseID + 30, "Dustan", 3) },
    { BaseID + 31, new Location(BaseID + 31, "hamsterball", 3) },
    { BaseID + 32, new Location(BaseID + 32, "arcadeBone", 3) },
    { BaseID + 33, new Location(BaseID + 33, "tree", 3) },
    { BaseID + 34, new Location(BaseID + 34, "bug", 3) },
    { BaseID + 35, new Location(BaseID + 35, "behindWaterfall", 3) },
    { BaseID + 36, new Location(BaseID + 36, "volley", 3) },
    { BaseID + 37, new Location(BaseID + 37, "flowerPuzzle", 3) },
    { BaseID + 38, new Location(BaseID + 38, "graffiti", 3) },
    { BaseID + 39, new Location(BaseID + 39, "cassetteCoin2", 3) },
    { BaseID + 40, new Location(BaseID + 40, "fishing", 3) },
    // { BaseID + 41, new Location(BaseID + 41, "gameKid", 3) },
    // { BaseID + 42, new Location(BaseID + 42, "blippyCoin", 3) },
    // { BaseID + 43, new Location(BaseID + 43, "SerschelAndLouist", 3) },

    // Public Pool
    { BaseID + 44, new Location(BaseID + 44, "2D", 4) },
    { BaseID + 45, new Location(BaseID + 45, "cassetteCoin2", 4) },
    { BaseID + 46, new Location(BaseID + 46, "arcadeBone", 4) },
    { BaseID + 47, new Location(BaseID + 47, "arcade", 4) },
    { BaseID + 48, new Location(BaseID + 48, "fishing", 4) },
    { BaseID + 49, new Location(BaseID + 49, "main", 4) },
    { BaseID + 50, new Location(BaseID + 50, "volley", 4) },
    { BaseID + 51, new Location(BaseID + 51, "bug", 4) },
    { BaseID + 52, new Location(BaseID + 52, "cassetteCoin", 4) },
    { BaseID + 53, new Location(BaseID + 53, "flowerPuzzle", 4) },

    // Bathhouse
    { BaseID + 54, new Location(BaseID + 54, "carrynojump", 5) },
    { BaseID + 55, new Location(BaseID + 55, "hamsterball", 5) },
    { BaseID + 56, new Location(BaseID + 56, "main", 5) },
    { BaseID + 57, new Location(BaseID + 57, "graffiti", 5) },
    { BaseID + 58, new Location(BaseID + 58, "cassetteCoin", 5) },
    { BaseID + 59, new Location(BaseID + 59, "cassetteCoin2", 5) },
    { BaseID + 60, new Location(BaseID + 60, "Dustan", 5) },
    { BaseID + 61, new Location(BaseID + 61, "volley", 5) },
    { BaseID + 62, new Location(BaseID + 62, "gamerQuest", 5) },
    { BaseID + 63, new Location(BaseID + 63, "fishing", 5) },
    { BaseID + 64, new Location(BaseID + 64, "bug", 5) },
    { BaseID + 65, new Location(BaseID + 65, "flowerPuzzle", 5) },
    { BaseID + 66, new Location(BaseID + 66, "arcadeBone", 5) },
    { BaseID + 67, new Location(BaseID + 67, "arcade", 5) },

    // Tadpole HQ
    { BaseID + 68, new Location(BaseID + 68, "cassetteCoin2", 6) },
    { BaseID + 69, new Location(BaseID + 69, "cassetteCoin", 6) },
    { BaseID + 70, new Location(BaseID + 70, "main", 6) },
    { BaseID + 71, new Location(BaseID + 71, "volley", 6) },
    { BaseID + 72, new Location(BaseID + 72, "fishing", 6) },
    { BaseID + 73, new Location(BaseID + 73, "flowerPuzzle", 6) },
    { BaseID + 74, new Location(BaseID + 74, "arcade", 6) },
    { BaseID + 75, new Location(BaseID + 75, "bug", 6) },
    { BaseID + 76, new Location(BaseID + 76, "carrynojump", 6) },
    { BaseID + 77, new Location(BaseID + 77, "arcadeBone", 6) }
};

    public static void AddLocation(long id, string flag, int level)
    {
        CoinLocations[id] = new Location(id, flag, level);
    }

    public static Location GetLocation(long id)
    {
        return CoinLocations.ContainsKey(id) ? CoinLocations[id] : null;
    }
}

