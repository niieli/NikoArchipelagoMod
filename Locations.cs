using System.Collections.Generic;
using NikoArchipelago.Archipelago;

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
    { BaseID + 9, new Location(BaseID + 9, "graffiti", 1) },
    { BaseID + 10, new Location(BaseID + 10, "hamsterball", 1) },
    { BaseID + 11, new Location(BaseID + 11, "cassetteCoin", 1) },
    { BaseID + 12, new Location(BaseID + 12, "cassetteCoin2", 1) },
    { BaseID + 13, new Location(BaseID + 13, "gamerQuest", 1) },
    { BaseID + 14, new Location(BaseID + 14, "arcadeBone", 1) },
    { BaseID + 15, new Location(BaseID + 15, "arcade", 1) },
    { BaseID + 16, new Location(BaseID + 16, "carrynojump", 1) },

    // Turbine Town
    { BaseID + 17, new Location(BaseID + 17, "fishing", 2) },
    { BaseID + 18, new Location(BaseID + 18, "volley", 2) },
    { BaseID + 19, new Location(BaseID + 19, "flowerPuzzle", 2) },
    { BaseID + 20, new Location(BaseID + 20, "main", 2) },
    { BaseID + 21, new Location(BaseID + 21, "bug", 2) },
    { BaseID + 22, new Location(BaseID + 22, "Dustan", 2) },
    { BaseID + 23, new Location(BaseID + 23, "cassetteCoin", 2) },
    { BaseID + 24, new Location(BaseID + 24, "cassetteCoin2", 2) },
    { BaseID + 25, new Location(BaseID + 25, "arcadeBone", 2) },
    { BaseID + 26, new Location(BaseID + 26, "arcade", 2) },
    { BaseID + 27, new Location(BaseID + 27, "carrynojump", 2) },

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
    { BaseID + 41, new Location(BaseID + 41, "gamerQuest", 3) },
    { BaseID + 42, new Location(BaseID + 42, "arcade", 3) },
    { BaseID + 43, new Location(BaseID + 43, "carrynojump", 3) },

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
    { BaseID + 77, new Location(BaseID + 77, "arcadeBone", 6) },
};

    public static Dictionary<long, Location> CassetteLocations = new()
    { 
    // Hairball City
    { BaseID + 100, new Location(BaseID + 100, "casHairballCity0", 1) },
    { BaseID + 101, new Location(BaseID + 101, "casHairballCity1", 1) },
    { BaseID + 102, new Location(BaseID + 102, "casHairballCity2", 1) },
    { BaseID + 103, new Location(BaseID + 103, "casHairballCity3", 1) },
    { BaseID + 104, new Location(BaseID + 104, "casHairballCity4", 1) },
    { BaseID + 105, new Location(BaseID + 105, "casHairballCity5", 1) },
    { BaseID + 106, new Location(BaseID + 106, "casHairballCity6", 1) },
    { BaseID + 107, new Location(BaseID + 107, "casHairballCity7", 1) },
    { BaseID + 108, new Location(BaseID + 108, "casHairballCity8", 1) },
    { BaseID + 109, new Location(BaseID + 109, "casHairballCity9", 1) },

    // Turbine Town
    { BaseID + 111, new Location(BaseID + 111, "Cassette", 2) },
    { BaseID + 112, new Location(BaseID + 112, "Cassette (1)", 2) },
    { BaseID + 113, new Location(BaseID + 113, "Cassette (2)", 2) },
    { BaseID + 114, new Location(BaseID + 114, "Cassette (3)", 2) },
    { BaseID + 115, new Location(BaseID + 115, "Cassette (4)", 2) },
    { BaseID + 116, new Location(BaseID + 116, "Cassette (5)", 2) },
    { BaseID + 117, new Location(BaseID + 117, "Cassette (6)", 2) },
    { BaseID + 118, new Location(BaseID + 118, "Cassette (7)", 2) },
    { BaseID + 119, new Location(BaseID + 119, "Cassette (8)", 2) },
    { BaseID + 120, new Location(BaseID + 120, "Cassette (9)", 2) },

    // Salmon Creek Forest
    { BaseID + 122, new Location(BaseID + 122, "Cassette", 3) },
    { BaseID + 123, new Location(BaseID + 123, "Cassette (1)", 3) },
    { BaseID + 124, new Location(BaseID + 124, "Cassette (2)", 3) },
    { BaseID + 125, new Location(BaseID + 125, "Cassette (3)", 3) },
    { BaseID + 126, new Location(BaseID + 126, "Cassette (4)", 3) },
    { BaseID + 127, new Location(BaseID + 127, "Cassette (5)", 3) },
    { BaseID + 128, new Location(BaseID + 128, "Cassette (6)", 3) },
    { BaseID + 129, new Location(BaseID + 129, "Cassette (7)", 3) },
    { BaseID + 130, new Location(BaseID + 130, "Cassette (8)", 3) },
    { BaseID + 131, new Location(BaseID + 131, "Cassette (9)", 3) },
    { BaseID + 132, new Location(BaseID + 132, "Cassette (10)", 3) },
    
    // Public Pool
    { BaseID + 134, new Location(BaseID + 134, "Cassette", 4) },
    { BaseID + 135, new Location(BaseID + 135, "Cassette (1)", 4) },
    { BaseID + 136, new Location(BaseID + 136, "Cassette (2)", 4) },
    { BaseID + 137, new Location(BaseID + 137, "Cassette (3)", 4) },
    { BaseID + 138, new Location(BaseID + 138, "Cassette (4)", 4) },
    { BaseID + 139, new Location(BaseID + 139, "Cassette (5)", 4) },
    { BaseID + 140, new Location(BaseID + 140, "Cassette (6)", 4) },
    { BaseID + 141, new Location(BaseID + 141, "Cassette (7)", 4) },
    { BaseID + 142, new Location(BaseID + 142, "Cassette (8)", 4) },
    { BaseID + 143, new Location(BaseID + 143, "Cassette (9)", 4) },

    // Bathhouse
    { BaseID + 145, new Location(BaseID + 145, "Cassette", 5) },
    { BaseID + 146, new Location(BaseID + 146, "Cassette (1)", 5) },
    { BaseID + 147, new Location(BaseID + 147, "Cassette (2)", 5) },
    { BaseID + 148, new Location(BaseID + 148, "Cassette (3)", 5) },
    { BaseID + 149, new Location(BaseID + 149, "Cassette (4)", 5) },
    { BaseID + 150, new Location(BaseID + 150, "Cassette (5)", 5) },
    { BaseID + 151, new Location(BaseID + 151, "Cassette (6)", 5) },
    { BaseID + 152, new Location(BaseID + 152, "Cassette (7)", 5) },
    { BaseID + 153, new Location(BaseID + 153, "Cassette (8)", 5) },
    { BaseID + 154, new Location(BaseID + 154, "Cassette (9)", 5) },

    // Tadpole HQ
    { BaseID + 156, new Location(BaseID + 156, "Cassette", 6) },
    { BaseID + 157, new Location(BaseID + 157, "Cassette (1)", 6) },
    { BaseID + 158, new Location(BaseID + 158, "Cassette (2)", 6) },
    { BaseID + 159, new Location(BaseID + 159, "Cassette (3)", 6) },
    { BaseID + 160, new Location(BaseID + 160, "Cassette (4)", 6) },
    { BaseID + 161, new Location(BaseID + 161, "Cassette (5)", 6) },
    { BaseID + 162, new Location(BaseID + 162, "Cassette (6)", 6) },
    { BaseID + 163, new Location(BaseID + 163, "Cassette (7)", 6) },
    { BaseID + 164, new Location(BaseID + 164, "Cassette (8)", 6) },
    { BaseID + 165, new Location(BaseID + 165, "Cassette (9)", 6) },
}; 
    public static Dictionary<long, Location> LetterLocations = new()
    {
        { BaseID + 80, new Location(BaseID + 80, "letter12", 0) },
        { BaseID + 81, new Location(BaseID + 81, "letter7", 1) },
        { BaseID + 82, new Location(BaseID + 82, "letter1", 1) },
        { BaseID + 83, new Location(BaseID + 83, "letter2", 2) },
        { BaseID + 84, new Location(BaseID + 84, "letter8", 2) },
        { BaseID + 85, new Location(BaseID + 85, "letter9", 3) },
        { BaseID + 86, new Location(BaseID + 86, "letter3", 3) },
        { BaseID + 87, new Location(BaseID + 87, "letter10", 4) },
        { BaseID + 88, new Location(BaseID + 88, "letter4", 4) }, 
        { BaseID + 89, new Location(BaseID + 89, "letter11", 5) },
        { BaseID + 90, new Location(BaseID + 90, "letter5", 5) }, 
    };
    
    public static Dictionary<long, Location> KeyLocations = new()
    {
        { BaseID + 91, new Location(BaseID + 91, "containerKey", 2) },
        { BaseID + 92, new Location(BaseID + 92, "parasolKey", 2) },
        { BaseID + 93, new Location(BaseID + 93, "1Key", 3) },
        { BaseID + 94, new Location(BaseID + 94, "2Key", 3) },
        { BaseID + 95, new Location(BaseID + 95, "3Key", 3) },
        { BaseID + 96, new Location(BaseID + 96, "testKey", 4) },
        { BaseID + 97, new Location(BaseID + 97, "ontoriiKey", 5) },
        { BaseID + 98, new Location(BaseID + 98, "inpuzzleKey", 5) },
        { BaseID + 99, new Location(BaseID + 99, "underfloorKey", 5) }, 
    };
    
    public static Dictionary<long, Location> GeneralLocations = new()
    {
        { BaseID + 166, new Location(BaseID + 166, "SecretMove Obtained", 6) },
        { BaseID + 167, new Location(BaseID + 167, "CL1 Obtained", 3) },
        { BaseID + 168, new Location(BaseID + 168, "CL2 Obtained", 6) },
    };
    
    public static Dictionary<long, Location> AchievementsLocations = new()
    {
        { BaseID + 176, new Location(BaseID + 176, "FROG_FAN", 6) },
        { BaseID + 177, new Location(BaseID + 177, "EMLOYEE_OF_THE_MONTH", 6) },
        { BaseID + 178, new Location(BaseID + 178, "BOTTLED_UP", 6) },
        { BaseID + 179, new Location(BaseID + 179, "SNAIL_FASHION_SHOW", 6) },
        { BaseID + 180, new Location(BaseID + 180, "VOLLEY_DREAMS", 6) },
        { BaseID + 181, new Location(BaseID + 181, "HOPELESS_ROMANTIC", 6) },
        { BaseID + 182, new Location(BaseID + 182, "LOST_AT_SEA", 6) },
        { BaseID + 202, new Location(BaseID + 202, "Dustan", 6) }
    };
    
    public static Dictionary<long, Location> HandsomeLocations = new()
    {
        { BaseID + 193, new Location(BaseID + 193, "Froggy Hairball City", 1) },
        { BaseID + 194, new Location(BaseID + 194, "Froggy Trash Kingdom", 2) },
        { BaseID + 195, new Location(BaseID + 195, "Froggy Salmon Creek Forest", 3) }, 
        { BaseID + 196, new Location(BaseID + 196, "Froggy Public Pool", 4) },
        { BaseID + 197, new Location(BaseID + 197, "Froggy The Bathhouse", 5) },
        //{ BaseID + 201, new Location(BaseID + 201, "true", 23) },

    };
    
    //Gary's Garden
    public static Dictionary<long, Location> GaryGardenCoinLocations = new()
    {
        //Coins
        { BaseID + 198, new Location(BaseID + 198, "Gary", 23) },
        { BaseID + 199, new Location(BaseID + 199, "cassetteCoin2", 23) },
        { BaseID + 200, new Location(BaseID + 200, "cassetteCoin", 23) },
    };
    
    public static Dictionary<long, Location> GaryGardenCassetteLocations = new()
    {
        //Cassettes
        { BaseID + 183, new Location(BaseID + 183, "Cassette", 23) },
        { BaseID + 184, new Location(BaseID + 184, "Cassette (1)", 23) },
        { BaseID + 185, new Location(BaseID + 185, "Cassette (2)", 23) },
        { BaseID + 186, new Location(BaseID + 186, "Cassette (3)", 23) },
        { BaseID + 187, new Location(BaseID + 187, "Cassette (4)", 23) },
        { BaseID + 188, new Location(BaseID + 188, "Cassette (5)", 23) },
        { BaseID + 189, new Location(BaseID + 189, "Cassette (6)", 23) },
        { BaseID + 190, new Location(BaseID + 190, "Cassette (7)", 23) },
        { BaseID + 191, new Location(BaseID + 191, "Cassette (8)", 23) },
        { BaseID + 192, new Location(BaseID + 192, "Cassette (9)", 23) },
    };
    
    public static Dictionary<long, Location> FishsanityLocations = new()
    {
        //FISH
        { BaseID + 203, new Location(BaseID + 203, "fish0", 1) },
        { BaseID + 204, new Location(BaseID + 204, "fish1", 1) },
        { BaseID + 205, new Location(BaseID + 205, "fish2", 1) },
        { BaseID + 206, new Location(BaseID + 206, "fish3", 1) },
        { BaseID + 207, new Location(BaseID + 207, "fish4", 1) },
        { BaseID + 208, new Location(BaseID + 208, "fish0", 2) },
        { BaseID + 209, new Location(BaseID + 209, "fish1", 2) },
        { BaseID + 210, new Location(BaseID + 210, "fish2", 2) },
        { BaseID + 211, new Location(BaseID + 211, "fish3", 2) },
        { BaseID + 212, new Location(BaseID + 212, "fish4", 2) },
        { BaseID + 213, new Location(BaseID + 213, "fish0", 3) },
        { BaseID + 214, new Location(BaseID + 214, "fish1", 3) },
        { BaseID + 215, new Location(BaseID + 215, "fish2", 3) },
        { BaseID + 216, new Location(BaseID + 216, "fish3", 3) },
        { BaseID + 217, new Location(BaseID + 217, "fish4", 3) },
        { BaseID + 218, new Location(BaseID + 218, "fish0", 4) },
        { BaseID + 219, new Location(BaseID + 219, "fish1", 4) },
        { BaseID + 220, new Location(BaseID + 220, "fish2", 4) },
        { BaseID + 221, new Location(BaseID + 221, "fish3", 4) },
        { BaseID + 222, new Location(BaseID + 222, "fish4", 4) },
        { BaseID + 223, new Location(BaseID + 223, "fish0", 5) },
        { BaseID + 224, new Location(BaseID + 224, "fish1", 5) },
        { BaseID + 225, new Location(BaseID + 225, "fish2", 5) },
        { BaseID + 226, new Location(BaseID + 226, "fish3", 5) },
        { BaseID + 227, new Location(BaseID + 227, "fish4", 5) },
        { BaseID + 228, new Location(BaseID + 228, "fish0", 6) },
        { BaseID + 229, new Location(BaseID + 229, "fish1", 6) },
        { BaseID + 230, new Location(BaseID + 230, "fish2", 6) },
        { BaseID + 231, new Location(BaseID + 231, "fish3", 6) },
        { BaseID + 232, new Location(BaseID + 232, "fish4", 6) },
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

