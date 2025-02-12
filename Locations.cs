using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
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
    public const long BaseID = 598_145_444_000;

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
        { BaseID + 250, new Location(BaseID + 250, "letter6", 6) },
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
        { BaseID + 201, new Location(BaseID + 201, "Froggy GarysGarden", 23) },

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
    
    public static Dictionary<long, Location> KioskLocations = new()
    {
        //Coins
        { BaseID + 170, new Location(BaseID + 170, "KioskHome", 0) },
        { BaseID + 171, new Location(BaseID + 171, "KioskHairball City", 1) },
        { BaseID + 172, new Location(BaseID + 172, "KioskTrash Kingdom", 2) },
        { BaseID + 173, new Location(BaseID + 173, "KioskSalmon Creek Forest", 3) },
        { BaseID + 174, new Location(BaseID + 174, "KioskPublic Pool", 4) },
        { BaseID + 175, new Location(BaseID + 175, "KioskThe Bathhouse", 5) },
    };
    
    public static Dictionary<long, Location> SnailShopLocations = new()
    {
        //Shop
        { BaseID + 233, new Location(BaseID + 233, "true", 0) },
        { BaseID + 234, new Location(BaseID + 234, "true", 1) },
        { BaseID + 235, new Location(BaseID + 235, "true", 2) },
        { BaseID + 236, new Location(BaseID + 236, "true", 3) },
        { BaseID + 237, new Location(BaseID + 237, "true", 4) },
        { BaseID + 238, new Location(BaseID + 238, "true", 5) },
        { BaseID + 239, new Location(BaseID + 239, "true", 6) },
        { BaseID + 240, new Location(BaseID + 240, "true", 7) },
        { BaseID + 241, new Location(BaseID + 241, "true", 8) },
        { BaseID + 242, new Location(BaseID + 242, "true", 9) },
        { BaseID + 243, new Location(BaseID + 243, "true", 10) },
        { BaseID + 244, new Location(BaseID + 244, "true", 11) },
        { BaseID + 245, new Location(BaseID + 245, "true", 12) },
        { BaseID + 246, new Location(BaseID + 246, "true", 13) },
        { BaseID + 247, new Location(BaseID + 247, "true", 14) },
        { BaseID + 248, new Location(BaseID + 248, "true", 15) },
    };
    
    public static Dictionary<long, Location> SunflowerSeedsLocations = new()
    {
        //Seeds
        { BaseID + 260, new Location(BaseID + 260, "Seed", 1) },
        { BaseID + 261, new Location(BaseID + 261, "Seed(1)", 1) },
        { BaseID + 262, new Location(BaseID + 262, "Seed(2)", 1) },
        { BaseID + 263, new Location(BaseID + 263, "Seed(3)", 1) },
        { BaseID + 264, new Location(BaseID + 264, "Seed(4)", 1) },
        { BaseID + 265, new Location(BaseID + 265, "Seed(5)", 1) },
        { BaseID + 266, new Location(BaseID + 266, "Seed(6)", 1) },
        { BaseID + 267, new Location(BaseID + 267, "Seed(7)", 1) },
        { BaseID + 268, new Location(BaseID + 268, "Seed(8)", 1) },
        { BaseID + 269, new Location(BaseID + 269, "Seed(9)", 1) },
        { BaseID + 270, new Location(BaseID + 270, "Seed", 3) },
        { BaseID + 271, new Location(BaseID + 271, "Seed(1)", 3) },
        { BaseID + 272, new Location(BaseID + 272, "Seed(2)", 3) },
        { BaseID + 273, new Location(BaseID + 273, "Seed(3)", 3) },
        { BaseID + 274, new Location(BaseID + 274, "Seed(4)", 3) },
        { BaseID + 275, new Location(BaseID + 275, "Seed(5)", 3) },
        { BaseID + 276, new Location(BaseID + 276, "Seed(6)", 3) },
        { BaseID + 277, new Location(BaseID + 277, "Seed(7)", 3) },
        { BaseID + 278, new Location(BaseID + 278, "Seed(8)", 3) },
        { BaseID + 279, new Location(BaseID + 279, "Seed(9)", 3) },
        { BaseID + 280, new Location(BaseID + 280, "Seed", 5) },
        { BaseID + 281, new Location(BaseID + 281, "Seed(1)", 5) },
        { BaseID + 282, new Location(BaseID + 282, "Seed(2)", 5) },
        { BaseID + 283, new Location(BaseID + 283, "Seed(3)", 5) },
        { BaseID + 284, new Location(BaseID + 284, "Seed(4)", 5) },
        { BaseID + 285, new Location(BaseID + 285, "Seed(5)", 5) },
        { BaseID + 286, new Location(BaseID + 286, "Seed(6)", 5) },
        { BaseID + 287, new Location(BaseID + 287, "Seed(7)", 5) },
        { BaseID + 288, new Location(BaseID + 288, "Seed(8)", 5) },
        { BaseID + 289, new Location(BaseID + 289, "Seed(9)", 5) },
    };
    
    public static Dictionary<long, Location> FlowerbedsLocations = new()
    {
        //Flowerbeds
        { BaseID + 300, new Location(BaseID + 300, "FPuzzle", 1) },
        { BaseID + 301, new Location(BaseID + 301, "FPuzzle (1)", 1) },
        { BaseID + 302, new Location(BaseID + 302, "FPuzzle (2)", 1) },
        { BaseID + 303, new Location(BaseID + 303, "FPuzzle", 2) },
        { BaseID + 304, new Location(BaseID + 304, "FPuzzle (1)", 2) },
        { BaseID + 305, new Location(BaseID + 305, "FPuzzle (2)", 2) },
        { BaseID + 306, new Location(BaseID + 306, "FPuzzle", 3) },
        { BaseID + 307, new Location(BaseID + 307, "FPuzzle (1)", 3) },
        { BaseID + 308, new Location(BaseID + 308, "FPuzzle (2)", 3) },
        { BaseID + 309, new Location(BaseID + 309, "FPuzzle (3)", 3) },
        { BaseID + 310, new Location(BaseID + 310, "FPuzzle (4)", 3) },
        { BaseID + 311, new Location(BaseID + 311, "FPuzzle (5)", 3) },
        { BaseID + 312, new Location(BaseID + 312, "FPuzzle", 4) },
        { BaseID + 313, new Location(BaseID + 313, "FPuzzle (1)", 4) },
        { BaseID + 314, new Location(BaseID + 314, "FPuzzle (2)", 4) },
        { BaseID + 315, new Location(BaseID + 315, "FPuzzle", 5) },
        { BaseID + 316, new Location(BaseID + 316, "FPuzzle (1)", 5) },
        { BaseID + 317, new Location(BaseID + 317, "FPuzzle (2)", 5) },
        { BaseID + 318, new Location(BaseID + 318, "FPuzzle", 6) },
        { BaseID + 319, new Location(BaseID + 319, "FPuzzle (1)", 6) },
        { BaseID + 320, new Location(BaseID + 320, "FPuzzle (2)", 6) },
        { BaseID + 321, new Location(BaseID + 321, "FPuzzle (3)", 6) },
    };
    
    public static Dictionary<long, Location> ApplesanityLocations = new()
    {
        //Applesanity
        { BaseID + 322, new Location(BaseID + 322, "Apple Float", 1) },
        { BaseID + 323, new Location(BaseID + 323, "Apple Float (1)", 1) },
        { BaseID + 324, new Location(BaseID + 324, "Apple Float (2)", 1) },
        { BaseID + 325, new Location(BaseID + 325, "Apple Float (3)", 1) },
        { BaseID + 326, new Location(BaseID + 326, "Apple Float (4)", 1) }, 
        { BaseID + 327, new Location(BaseID + 327, "Apple Float (5)", 1) },
        { BaseID + 328, new Location(BaseID + 328, "Apple Float (6)", 1) }, 
        { BaseID + 329, new Location(BaseID + 329, "Apple Float (7)", 1) },
        { BaseID + 330, new Location(BaseID + 330, "Apple Float (8)", 1) }, 
        { BaseID + 331, new Location(BaseID + 331, "Apple Float (9)", 1) },
        { BaseID + 332, new Location(BaseID + 332, "Apple Float (10)", 1) }, 
        { BaseID + 333, new Location(BaseID + 333, "Apple Float (11)", 1) },
        { BaseID + 334, new Location(BaseID + 334, "Apple Float (12)", 1) }, 
        { BaseID + 335, new Location(BaseID + 335, "Apple Float (13)", 1) },
        { BaseID + 336, new Location(BaseID + 336, "Apple Float (14)", 1) }, 
        { BaseID + 337, new Location(BaseID + 337, "Apple Float (15)", 1) },
        { BaseID + 338, new Location(BaseID + 338, "Apple Float (16)", 1) }, 
        { BaseID + 339, new Location(BaseID + 339, "Apple Float (17)", 1) },
        { BaseID + 340, new Location(BaseID + 340, "Apple Float (18)", 1) }, 
        { BaseID + 341, new Location(BaseID + 341, "Apple Float (19)", 1) },
        { BaseID + 342, new Location(BaseID + 342, "Apple Float (20)", 1) }, 
        { BaseID + 343, new Location(BaseID + 343, "Apple Float (21)", 1) },
        { BaseID + 344, new Location(BaseID + 344, "Apple Float (22)", 1) }, 
        { BaseID + 345, new Location(BaseID + 345, "Apple Float (23)", 1) },
        { BaseID + 346, new Location(BaseID + 346, "Apple Float (24)", 1) }, 
        { BaseID + 347, new Location(BaseID + 347, "Apple Float (25)", 1) },
        { BaseID + 348, new Location(BaseID + 348, "Apple Float (26)", 1) }, 
        { BaseID + 349, new Location(BaseID + 349, "Apple Float (27)", 1) },
        { BaseID + 350, new Location(BaseID + 350, "Apple Float (28)", 1) }, 
        { BaseID + 351, new Location(BaseID + 351, "Apple Float (29)", 1) },
        { BaseID + 352, new Location(BaseID + 352, "Apple Float (30)", 1) }, 
        { BaseID + 353, new Location(BaseID + 353, "Apple Float (31)", 1) },
        
        { BaseID + 354, new Location(BaseID + 354, "Apple Float", 2) },
        { BaseID + 355, new Location(BaseID + 355, "Apple Float (1)", 2) },
        { BaseID + 356, new Location(BaseID + 356, "Apple Float (2)", 2) },
        { BaseID + 357, new Location(BaseID + 357, "Apple Float (3)", 2) },
        { BaseID + 358, new Location(BaseID + 358, "Apple Float (4)", 2) }, 
        { BaseID + 359, new Location(BaseID + 359, "Apple Float (5)", 2) },
        { BaseID + 360, new Location(BaseID + 360, "Apple Float (6)", 2) }, 
        { BaseID + 361, new Location(BaseID + 361, "Apple Float (7)", 2) },
        { BaseID + 362, new Location(BaseID + 362, "Apple Float (8)", 2) }, 
        { BaseID + 363, new Location(BaseID + 363, "Apple Float (9)", 2) },
        { BaseID + 364, new Location(BaseID + 364, "Apple Float (10)", 2) }, 
        { BaseID + 365, new Location(BaseID + 365, "Apple Float (11)", 2) },
        { BaseID + 366, new Location(BaseID + 366, "Apple Float (12)", 2) }, 
        { BaseID + 367, new Location(BaseID + 367, "Apple Float (13)", 2) },
        { BaseID + 368, new Location(BaseID + 368, "Apple Float (14)", 2) }, 
        { BaseID + 369, new Location(BaseID + 369, "Apple Float (15)", 2) },
        { BaseID + 370, new Location(BaseID + 370, "Apple Float (16)", 2) }, 
        { BaseID + 371, new Location(BaseID + 371, "Apple Float (17)", 2) },
        { BaseID + 372, new Location(BaseID + 372, "Apple Float (18)", 2) }, 
        { BaseID + 373, new Location(BaseID + 373, "Apple Float (19)", 2) },
        { BaseID + 374, new Location(BaseID + 374, "Apple Float (20)", 2) }, 
        { BaseID + 375, new Location(BaseID + 375, "Apple Float (21)", 2) },
        { BaseID + 376, new Location(BaseID + 376, "Apple Float (22)", 2) }, 
        { BaseID + 377, new Location(BaseID + 377, "Apple Float (23)", 2) },
        { BaseID + 378, new Location(BaseID + 378, "Apple Float (24)", 2) }, 
        { BaseID + 379, new Location(BaseID + 379, "Apple Float (25)", 2) },
        { BaseID + 380, new Location(BaseID + 380, "Apple Float (26)", 2) }, 
        { BaseID + 381, new Location(BaseID + 381, "Apple Float (27)", 2) },
        { BaseID + 382, new Location(BaseID + 382, "Apple Float (28)", 2) }, 
        { BaseID + 383, new Location(BaseID + 383, "Apple Float (29)", 2) },
        { BaseID + 384, new Location(BaseID + 384, "Apple Float (30)", 2) }, 
        { BaseID + 385, new Location(BaseID + 385, "Apple Float (31)", 2) },
        
        { BaseID + 386, new Location(BaseID + 386, "Apple Float", 3) },
        { BaseID + 387, new Location(BaseID + 387, "Apple Float (1)", 3) },
        { BaseID + 388, new Location(BaseID + 388, "Apple Float (2)", 3) },
        { BaseID + 389, new Location(BaseID + 389, "Apple Float (3)", 3) },
        { BaseID + 390, new Location(BaseID + 390, "Apple Float (4)", 3) }, 
        { BaseID + 391, new Location(BaseID + 391, "Apple Float (5)", 3) },
        { BaseID + 392, new Location(BaseID + 392, "Apple Float (6)", 3) }, 
        { BaseID + 393, new Location(BaseID + 393, "Apple Float (7)", 3) },
        { BaseID + 394, new Location(BaseID + 394, "Apple Float (8)", 3) }, 
        { BaseID + 395, new Location(BaseID + 395, "Apple Float (9)", 3) },
        { BaseID + 396, new Location(BaseID + 396, "Apple Float (10)", 3) }, 
        { BaseID + 397, new Location(BaseID + 397, "Apple Float (11)", 3) },
        { BaseID + 398, new Location(BaseID + 398, "Apple Float (12)", 3) }, 
        { BaseID + 399, new Location(BaseID + 399, "Apple Float (13)", 3) },
        { BaseID + 400, new Location(BaseID + 400, "Apple Float (14)", 3) }, 
        { BaseID + 401, new Location(BaseID + 401, "Apple Float (15)", 3) },
        { BaseID + 402, new Location(BaseID + 402, "Apple Float (16)", 3) }, 
        { BaseID + 403, new Location(BaseID + 403, "Apple Float (17)", 3) },
        { BaseID + 404, new Location(BaseID + 404, "Apple Float (18)", 3) }, 
        { BaseID + 405, new Location(BaseID + 405, "Apple Float (19)", 3) },
        { BaseID + 406, new Location(BaseID + 406, "Apple Float (20)", 3) }, 
        { BaseID + 407, new Location(BaseID + 407, "Apple Float (21)", 3) },
        { BaseID + 408, new Location(BaseID + 408, "Apple Float (22)", 3) }, 
        { BaseID + 409, new Location(BaseID + 409, "Apple Float (23)", 3) },
        { BaseID + 410, new Location(BaseID + 410, "Apple Float (24)", 3) }, 
        { BaseID + 411, new Location(BaseID + 411, "Apple Float (25)", 3) },
        { BaseID + 412, new Location(BaseID + 412, "Apple Float (26)", 3) }, 
        { BaseID + 413, new Location(BaseID + 413, "Apple Float (27)", 3) },
        { BaseID + 414, new Location(BaseID + 414, "Apple Float (28)", 3) }, 
        { BaseID + 415, new Location(BaseID + 415, "Apple Float (29)", 3) },
        { BaseID + 416, new Location(BaseID + 416, "Apple Float (30)", 3) }, 
        { BaseID + 417, new Location(BaseID + 417, "Apple Float (31)", 3) },
        { BaseID + 418, new Location(BaseID + 418, "Apple Float (32)", 3) },
        { BaseID + 419, new Location(BaseID + 419, "Apple Float (33)", 3) },
        { BaseID + 420, new Location(BaseID + 420, "Apple Float (34)", 3) },
        { BaseID + 421, new Location(BaseID + 421, "Apple Float (35)", 3) }, 
        { BaseID + 422, new Location(BaseID + 422, "Apple Float (36)", 3) },
        { BaseID + 423, new Location(BaseID + 423, "Apple Float (37)", 3) }, 
        { BaseID + 424, new Location(BaseID + 424, "Apple Float (38)", 3) },
        { BaseID + 425, new Location(BaseID + 425, "Apple Float (39)", 3) }, 
        { BaseID + 426, new Location(BaseID + 426, "Apple Float (40)", 3) },
        { BaseID + 427, new Location(BaseID + 427, "Apple Float (41)", 3) }, 
        { BaseID + 428, new Location(BaseID + 428, "Apple Float (42)", 3) },
        { BaseID + 429, new Location(BaseID + 429, "Apple Float (43)", 3) }, 
        { BaseID + 430, new Location(BaseID + 430, "Apple Float (44)", 3) },
        { BaseID + 431, new Location(BaseID + 431, "Apple Float (45)", 3) }, 
        { BaseID + 432, new Location(BaseID + 432, "Apple Float (46)", 3) },
        { BaseID + 433, new Location(BaseID + 433, "Apple Float (47)", 3) }, 
        { BaseID + 434, new Location(BaseID + 434, "Apple Float (48)", 3) },
        { BaseID + 435, new Location(BaseID + 435, "Apple Float (49)", 3) }, 
        { BaseID + 436, new Location(BaseID + 436, "Apple Float (50)", 3) },
        { BaseID + 437, new Location(BaseID + 437, "Apple Float (51)", 3) }, 
        { BaseID + 438, new Location(BaseID + 438, "Apple Float (52)", 3) },
        { BaseID + 439, new Location(BaseID + 439, "Apple Float (53)", 3) }, 
        { BaseID + 440, new Location(BaseID + 440, "Apple Float (54)", 3) },
        { BaseID + 441, new Location(BaseID + 441, "Apple Float (55)", 3) }, 
        { BaseID + 442, new Location(BaseID + 442, "Apple Float (56)", 3) },
        { BaseID + 443, new Location(BaseID + 443, "Apple Float (57)", 3) }, 
        { BaseID + 444, new Location(BaseID + 444, "Apple Float (58)", 3) },
        { BaseID + 445, new Location(BaseID + 445, "Apple Float (59)", 3) }, 
        { BaseID + 446, new Location(BaseID + 446, "Apple Float (60)", 3) },
        { BaseID + 447, new Location(BaseID + 447, "Apple Float (61)", 3) }, 
        { BaseID + 448, new Location(BaseID + 448, "Apple Float (62)", 3) },
        { BaseID + 449, new Location(BaseID + 449, "Apple Float (63)", 3) },
        { BaseID + 450, new Location(BaseID + 450, "Apple Float (64)", 3) }, 
        { BaseID + 451, new Location(BaseID + 451, "Apple Float (65)", 3) },
        { BaseID + 452, new Location(BaseID + 452, "Apple Float (66)", 3) }, 
        { BaseID + 453, new Location(BaseID + 453, "Apple Float (67)", 3) },
        { BaseID + 454, new Location(BaseID + 454, "Apple Float (68)", 3) }, 
        { BaseID + 455, new Location(BaseID + 455, "Apple Float (69)", 3) },
        { BaseID + 456, new Location(BaseID + 456, "Apple Float (70)", 3) }, 
        { BaseID + 457, new Location(BaseID + 457, "Apple Float (71)", 3) },
        { BaseID + 458, new Location(BaseID + 458, "Apple Float (72)", 3) }, 
        { BaseID + 459, new Location(BaseID + 459, "Apple Float (73)", 3) },
        { BaseID + 460, new Location(BaseID + 460, "Apple Float (74)", 3) }, 
        { BaseID + 461, new Location(BaseID + 461, "Apple Float (75)", 3) },
        { BaseID + 462, new Location(BaseID + 462, "Apple Float (76)", 3) }, 
        { BaseID + 463, new Location(BaseID + 463, "Apple Float (77)", 3) },
        { BaseID + 464, new Location(BaseID + 464, "Apple Float (78)", 3) }, 
        { BaseID + 465, new Location(BaseID + 465, "Apple Float (79)", 3) },
        { BaseID + 466, new Location(BaseID + 466, "Apple Float (80)", 3) }, 
        { BaseID + 467, new Location(BaseID + 467, "Apple Float (81)", 3) },
        { BaseID + 468, new Location(BaseID + 468, "Apple Float (82)", 3) }, 
        { BaseID + 469, new Location(BaseID + 469, "Apple Float (83)", 3) },
        { BaseID + 470, new Location(BaseID + 470, "Apple Float (84)", 3) }, 
        { BaseID + 471, new Location(BaseID + 471, "Apple Float (85)", 3) },
        
        { BaseID + 472, new Location(BaseID + 472, "Apple Float", 4) }, // (67)
        { BaseID + 473, new Location(BaseID + 473, "Apple Float (1)", 4) },
        { BaseID + 474, new Location(BaseID + 474, "Apple Float (2)", 4) },
        { BaseID + 475, new Location(BaseID + 475, "Apple Float (3)", 4) },
        { BaseID + 476, new Location(BaseID + 476, "Apple Float (4)", 4) }, 
        { BaseID + 477, new Location(BaseID + 477, "Apple Float (5)", 4) },
        { BaseID + 478, new Location(BaseID + 478, "Apple Float (6)", 4) }, 
        { BaseID + 479, new Location(BaseID + 479, "Apple Float (7)", 4) },
        { BaseID + 480, new Location(BaseID + 480, "Apple Float (8)", 4) }, 
        { BaseID + 481, new Location(BaseID + 481, "Apple Float (9)", 4) },
        { BaseID + 482, new Location(BaseID + 482, "Apple Float (10)", 4) }, 
        { BaseID + 483, new Location(BaseID + 483, "Apple Float (11)", 4) },
        { BaseID + 484, new Location(BaseID + 484, "Apple Float (12)", 4) }, 
        { BaseID + 485, new Location(BaseID + 485, "Apple Float (13)", 4) },
        { BaseID + 486, new Location(BaseID + 486, "Apple Float (14)", 4) }, 
        { BaseID + 487, new Location(BaseID + 487, "Apple Float (15)", 4) },
        { BaseID + 488, new Location(BaseID + 488, "Apple Float (16)", 4) }, 
        { BaseID + 489, new Location(BaseID + 489, "Apple Float (17)", 4) },
        { BaseID + 490, new Location(BaseID + 490, "Apple Float (18)", 4) }, 
        { BaseID + 491, new Location(BaseID + 491, "Apple Float (19)", 4) },
        { BaseID + 492, new Location(BaseID + 492, "Apple Float (20)", 4) }, 
        { BaseID + 493, new Location(BaseID + 493, "Apple Float (21)", 4) },
        { BaseID + 494, new Location(BaseID + 494, "Apple Float (22)", 4) }, 
        { BaseID + 495, new Location(BaseID + 495, "Apple Float (23)", 4) },
        { BaseID + 496, new Location(BaseID + 496, "Apple Float (24)", 4) }, 
        { BaseID + 497, new Location(BaseID + 497, "Apple Float (25)", 4) },
        { BaseID + 498, new Location(BaseID + 498, "Apple Float (26)", 4) }, 
        { BaseID + 499, new Location(BaseID + 499, "Apple Float (27)", 4) },
        { BaseID + 500, new Location(BaseID + 500, "Apple Float (28)", 4) },
        { BaseID + 501, new Location(BaseID + 501, "Apple Float (29)", 4) },
        { BaseID + 502, new Location(BaseID + 502, "Apple Float (30)", 4) },
        { BaseID + 503, new Location(BaseID + 503, "Apple Float (31)", 4) }, 
        { BaseID + 504, new Location(BaseID + 504, "Apple Float (32)", 4) },
        { BaseID + 505, new Location(BaseID + 505, "Apple Float (33)", 4) }, 
        { BaseID + 506, new Location(BaseID + 506, "Apple Float (34)", 4) },
        { BaseID + 507, new Location(BaseID + 507, "Apple Float (35)", 4) }, 
        { BaseID + 508, new Location(BaseID + 508, "Apple Float (36)", 4) },
        { BaseID + 509, new Location(BaseID + 509, "Apple Float (37)", 4) }, 
        { BaseID + 510, new Location(BaseID + 510, "Apple Float (38)", 4) },
        { BaseID + 511, new Location(BaseID + 511, "Apple Float (39)", 4) }, 
        { BaseID + 512, new Location(BaseID + 512, "Apple Float (40)", 4) },
        { BaseID + 513, new Location(BaseID + 513, "Apple Float (41)", 4) }, 
        { BaseID + 514, new Location(BaseID + 514, "Apple Float (42)", 4) },
        { BaseID + 515, new Location(BaseID + 515, "Apple Float (43)", 4) }, 
        { BaseID + 516, new Location(BaseID + 516, "Apple Float (44)", 4) },
        { BaseID + 517, new Location(BaseID + 517, "Apple Float (45)", 4) }, 
        { BaseID + 518, new Location(BaseID + 518, "Apple Float (46)", 4) },
        { BaseID + 519, new Location(BaseID + 519, "Apple Float (47)", 4) }, 
        { BaseID + 520, new Location(BaseID + 520, "Apple Float (48)", 4) },
        { BaseID + 521, new Location(BaseID + 521, "Apple Float (49)", 4) }, 
        { BaseID + 522, new Location(BaseID + 522, "Apple Float (50)", 4) },
        { BaseID + 523, new Location(BaseID + 523, "Apple Float (51)", 4) }, 
        { BaseID + 524, new Location(BaseID + 524, "Apple Float (52)", 4) },
        { BaseID + 525, new Location(BaseID + 525, "Apple Float (53)", 4) }, 
        { BaseID + 526, new Location(BaseID + 526, "Apple Float (54)", 4) },
        { BaseID + 527, new Location(BaseID + 527, "Apple Float (55)", 4) }, 
        { BaseID + 528, new Location(BaseID + 528, "Apple Float (56)", 4) },
        { BaseID + 529, new Location(BaseID + 529, "Apple Float (57)", 4) }, 
        { BaseID + 530, new Location(BaseID + 530, "Apple Float (58)", 4) },
        { BaseID + 531, new Location(BaseID + 531, "Apple Float (59)", 4) },
        { BaseID + 532, new Location(BaseID + 532, "Apple Float (60)", 4) }, 
        { BaseID + 533, new Location(BaseID + 533, "Apple Float (61)", 4) },
        { BaseID + 534, new Location(BaseID + 534, "Apple Float (62)", 4) }, 
        { BaseID + 535, new Location(BaseID + 535, "Apple Float (63)", 4) },
        { BaseID + 536, new Location(BaseID + 536, "Apple Float (64)", 4) },
        { BaseID + 537, new Location(BaseID + 537, "Apple Float (65)", 4) }, 
        { BaseID + 538, new Location(BaseID + 538, "Apple Float (66)", 4) },
        { BaseID + 539, new Location(BaseID + 539, "Apple Float (67)", 4) }, 
        
        { BaseID + 540, new Location(BaseID + 540, "Apple Float", 5) }, // (66)
        { BaseID + 541, new Location(BaseID + 541, "Apple Float (1)", 5) },
        { BaseID + 542, new Location(BaseID + 542, "Apple Float (2)", 5) },
        { BaseID + 543, new Location(BaseID + 543, "Apple Float (3)", 5) },
        { BaseID + 544, new Location(BaseID + 544, "Apple Float (4)", 5) }, 
        { BaseID + 545, new Location(BaseID + 545, "Apple Float (5)", 5) },
        { BaseID + 546, new Location(BaseID + 546, "Apple Float (6)", 5) }, 
        { BaseID + 547, new Location(BaseID + 547, "Apple Float (7)", 5) },
        { BaseID + 548, new Location(BaseID + 548, "Apple Float (8)", 5) }, 
        { BaseID + 549, new Location(BaseID + 549, "Apple Float (9)", 5) },
        { BaseID + 550, new Location(BaseID + 550, "Apple Float (10)", 5) }, 
        { BaseID + 551, new Location(BaseID + 551, "Apple Float (11)", 5) },
        { BaseID + 552, new Location(BaseID + 552, "Apple Float (12)", 5) }, 
        { BaseID + 553, new Location(BaseID + 553, "Apple Float (13)", 5) },
        { BaseID + 554, new Location(BaseID + 554, "Apple Float (14)", 5) }, 
        { BaseID + 555, new Location(BaseID + 555, "Apple Float (15)", 5) },
        { BaseID + 556, new Location(BaseID + 556, "Apple Float (16)", 5) }, 
        { BaseID + 557, new Location(BaseID + 557, "Apple Float (17)", 5) },
        { BaseID + 558, new Location(BaseID + 558, "Apple Float (18)", 5) }, 
        { BaseID + 559, new Location(BaseID + 559, "Apple Float (19)", 5) },
        { BaseID + 560, new Location(BaseID + 560, "Apple Float (20)", 5) }, 
        { BaseID + 561, new Location(BaseID + 561, "Apple Float (21)", 5) },
        { BaseID + 562, new Location(BaseID + 562, "Apple Float (22)", 5) }, 
        { BaseID + 563, new Location(BaseID + 563, "Apple Float (23)", 5) },
        { BaseID + 564, new Location(BaseID + 564, "Apple Float (24)", 5) }, 
        { BaseID + 565, new Location(BaseID + 565, "Apple Float (25)", 5) },
        { BaseID + 566, new Location(BaseID + 566, "Apple Float (26)", 5) }, 
        { BaseID + 567, new Location(BaseID + 567, "Apple Float (27)", 5) },
        { BaseID + 568, new Location(BaseID + 568, "Apple Float (28)", 5) },
        { BaseID + 569, new Location(BaseID + 569, "Apple Float (29)", 5) },
        { BaseID + 570, new Location(BaseID + 570, "Apple Float (30)", 5) },
        { BaseID + 571, new Location(BaseID + 571, "Apple Float (31)", 5) }, 
        { BaseID + 572, new Location(BaseID + 572, "Apple Float (32)", 5) },
        { BaseID + 573, new Location(BaseID + 573, "Apple Float (33)", 5) }, 
        { BaseID + 574, new Location(BaseID + 574, "Apple Float (34)", 5) },
        { BaseID + 575, new Location(BaseID + 575, "Apple Float (35)", 5) }, 
        { BaseID + 576, new Location(BaseID + 576, "Apple Float (36)", 5) },
        { BaseID + 577, new Location(BaseID + 577, "Apple Float (37)", 5) }, 
        { BaseID + 578, new Location(BaseID + 578, "Apple Float (38)", 5) },
        { BaseID + 579, new Location(BaseID + 579, "Apple Float (39)", 5) }, 
        { BaseID + 580, new Location(BaseID + 580, "Apple Float (40)", 5) },
        { BaseID + 581, new Location(BaseID + 581, "Apple Float (41)", 5) }, 
        { BaseID + 582, new Location(BaseID + 582, "Apple Float (42)", 5) },
        { BaseID + 583, new Location(BaseID + 583, "Apple Float (43)", 5) }, 
        { BaseID + 584, new Location(BaseID + 584, "Apple Float (44)", 5) },
        { BaseID + 585, new Location(BaseID + 585, "Apple Float (45)", 5) }, 
        { BaseID + 586, new Location(BaseID + 586, "Apple Float (46)", 5) },
        { BaseID + 587, new Location(BaseID + 587, "Apple Float (47)", 5) }, 
        { BaseID + 588, new Location(BaseID + 588, "Apple Float (48)", 5) },
        { BaseID + 589, new Location(BaseID + 589, "Apple Float (49)", 5) }, 
        { BaseID + 590, new Location(BaseID + 590, "Apple Float (50)", 5) },
        { BaseID + 591, new Location(BaseID + 591, "Apple Float (51)", 5) }, 
        { BaseID + 592, new Location(BaseID + 592, "Apple Float (52)", 5) },
        { BaseID + 593, new Location(BaseID + 593, "Apple Float (53)", 5) }, 
        { BaseID + 594, new Location(BaseID + 594, "Apple Float (54)", 5) },
        { BaseID + 595, new Location(BaseID + 595, "Apple Float (55)", 5) }, 
        { BaseID + 596, new Location(BaseID + 596, "Apple Float (56)", 5) },
        { BaseID + 597, new Location(BaseID + 597, "Apple Float (57)", 5) }, 
        { BaseID + 598, new Location(BaseID + 598, "Apple Float (58)", 5) },
        { BaseID + 599, new Location(BaseID + 599, "Apple Float (59)", 5) },
        { BaseID + 600, new Location(BaseID + 600, "Apple Float (60)", 5) }, 
        { BaseID + 601, new Location(BaseID + 601, "Apple Float (61)", 5) },
        { BaseID + 602, new Location(BaseID + 602, "Apple Float (62)", 5) }, 
        { BaseID + 603, new Location(BaseID + 603, "Apple Float (63)", 5) },
        { BaseID + 604, new Location(BaseID + 604, "Apple Float (64)", 5) },
        { BaseID + 605, new Location(BaseID + 605, "Apple Float (65)", 5) }, 
        { BaseID + 606, new Location(BaseID + 606, "Apple Float (66)", 5) },
        
        { BaseID + 607, new Location(BaseID + 607, "Apple Float", 6) }, // (10)
        { BaseID + 608, new Location(BaseID + 608, "Apple Float (1)", 6) },
        { BaseID + 609, new Location(BaseID + 609, "Apple Float (2)", 6) },
        { BaseID + 610, new Location(BaseID + 610, "Apple Float (3)", 6) },
        { BaseID + 611, new Location(BaseID + 611, "Apple Float (4)", 6) }, 
        { BaseID + 612, new Location(BaseID + 612, "Apple Float (5)", 6) },
        { BaseID + 613, new Location(BaseID + 613, "Apple Float (6)", 6) }, 
        { BaseID + 614, new Location(BaseID + 614, "Apple Float (7)", 6) },
        { BaseID + 615, new Location(BaseID + 615, "Apple Float (8)", 6) }, 
        { BaseID + 616, new Location(BaseID + 616, "Apple Float (9)", 6) },
        { BaseID + 617, new Location(BaseID + 617, "Apple Float (10)", 6) },
    };
    
    public static Dictionary<long, Location> ProgressiveMitchMaiLocations = new()
    {
        //Coins
        { BaseID + 620, new Location(BaseID + 620, "MiMa1", 0) },
        { BaseID + 621, new Location(BaseID + 621, "MiMa2", 0) },
        { BaseID + 622, new Location(BaseID + 622, "MiMa3", 0) },
        { BaseID + 623, new Location(BaseID + 623, "MiMa4", 0) },
        { BaseID + 624, new Location(BaseID + 624, "MiMa5", 0) },
        { BaseID + 625, new Location(BaseID + 625, "MiMa6", 0) },
        { BaseID + 626, new Location(BaseID + 626, "MiMa7", 0) },
        { BaseID + 627, new Location(BaseID + 627, "MiMa8", 0) },
        { BaseID + 628, new Location(BaseID + 628, "MiMa9", 0) },
        { BaseID + 629, new Location(BaseID + 629, "MiMa10", 0) },
        { BaseID + 630, new Location(BaseID + 630, "MiMa11", 0) },
        { BaseID + 631, new Location(BaseID + 631, "MiMa12", 0) },
        { BaseID + 632, new Location(BaseID + 632, "MiMa13", 0) },
        { BaseID + 633, new Location(BaseID + 633, "MiMa14", 0) },
    };

    private static readonly List<long> ScoutList = new()
    {
        //Mitch & Mai
        BaseID + 11, // Mitch
        BaseID + 12, // Mai
        BaseID + 23, // Mitch
        BaseID + 24, // Mai
        BaseID + 29, // Mitch
        BaseID + 39, // Mai
        BaseID + 45, // Mai
        BaseID + 52, // Mitch
        BaseID + 58, // Mitch
        BaseID + 59, // Mai
        BaseID + 68, // Mai
        BaseID + 69, // Mitch
        BaseID + 199, // Mai
        BaseID + 200, // Mitch

        //Kiosk
        BaseID + 170, //Home
        BaseID + 171, //Hairball City
        BaseID + 172, //Turbine Town
        BaseID + 173, //Salmon Creek Forest
        BaseID + 174, //Public Pool
        BaseID + 175, //Bathhouse

        //Snail Shop
        BaseID + 233,
        BaseID + 234,
        BaseID + 235,
        BaseID + 236,
        BaseID + 237,
        BaseID + 238,
        BaseID + 239,
        BaseID + 240,
        BaseID + 241,
        BaseID + 242,
        BaseID + 243,
        BaseID + 244,
        BaseID + 245,
        BaseID + 246,
        BaseID + 247,
        BaseID + 248,

        //All IDs
        
        //Coins
        BaseID + 0, // Index: 36

        //Hairball City
        BaseID + 3,
        BaseID + 4,
        BaseID + 5,
        BaseID + 6,
        BaseID + 7,
        BaseID + 8,
        BaseID + 9,
        BaseID + 10,
        //BaseID + 11,
        //BaseID + 12,
        BaseID + 13,
        BaseID + 14,
        BaseID + 15,
        BaseID + 16,

        //Turbine Town
        BaseID + 17, // Index: 49
        BaseID + 18,
        BaseID + 19,
        BaseID + 20,
        BaseID + 21,
        BaseID + 22,
        //BaseID + 23,
        //BaseID + 24,
        BaseID + 25,
        BaseID + 26,
        BaseID + 27,

        //Salmon Creek Forest
        BaseID + 28, // Index: 58
        //BaseID + 29,
        BaseID + 30,
        BaseID + 31,
        BaseID + 32,
        BaseID + 33,
        BaseID + 34,
        BaseID + 35,
        BaseID + 36,
        BaseID + 37,
        BaseID + 38,
        //BaseID + 39,
        BaseID + 40,
        BaseID + 41,
        BaseID + 42,
        BaseID + 43,

        // Public Pool
        BaseID + 44, // Index: 72
        //BaseID + 45,
        BaseID + 46,
        BaseID + 47,
        BaseID + 48,
        BaseID + 49,
        BaseID + 50,
        BaseID + 51,
        //BaseID + 52,
        BaseID + 53,
        // Bathhouse
        BaseID + 54, // Index: 80
        BaseID + 55,
        BaseID + 56,
        BaseID + 57,
        //BaseID + 58,
        //BaseID + 59,
        BaseID + 60,
        BaseID + 61,
        BaseID + 62,
        BaseID + 63,
        BaseID + 64,
        BaseID + 65,
        BaseID + 66,
        BaseID + 67,

        // Tadpole HQ
        //BaseID + 68,
        //BaseID + 69,
        BaseID + 70, // Index: 93
        BaseID + 71,
        BaseID + 72,
        BaseID + 73,
        BaseID + 74,
        BaseID + 75,
        BaseID + 76,
        BaseID + 77,
        
        //Gary's Garden
        BaseID + 198,
        //BaseID + 199,
        //BaseID + 200,
        
        //Cassettes
        // Hairball City
        BaseID + 100, // Index: 101
        BaseID + 101,
        BaseID + 102,
        BaseID + 103,
        BaseID + 104,
        BaseID + 105,
        BaseID + 106,
        BaseID + 107,
        BaseID + 108,
        BaseID + 109,

        // Turbine Town
        BaseID + 111, // Index: 111
        BaseID + 112,
        BaseID + 113,
        BaseID + 114,
        BaseID + 115,
        BaseID + 116,
        BaseID + 117,
        BaseID + 118,
        BaseID + 119,
        BaseID + 120,

        // Salmon Creek Forest
        BaseID + 122, // Index: 121
        BaseID + 123,
        BaseID + 124,
        BaseID + 125,
        BaseID + 126,
        BaseID + 127,
        BaseID + 128,
        BaseID + 129,
        BaseID + 130,
        BaseID + 131,
        BaseID + 132,

        // Public Pool
        BaseID + 134, // Index: 132
        BaseID + 135,
        BaseID + 136,
        BaseID + 137,
        BaseID + 138,
        BaseID + 139,
        BaseID + 140,
        BaseID + 141,
        BaseID + 142,
        BaseID + 143,

        // Bathhouse
        BaseID + 145, // Index: 142
        BaseID + 146,
        BaseID + 147,
        BaseID + 148,
        BaseID + 149,
        BaseID + 150,
        BaseID + 151,
        BaseID + 152,
        BaseID + 153,
        BaseID + 154,

        // Tadpole HQ
        BaseID + 156, // Index: 152
        BaseID + 157,
        BaseID + 158,
        BaseID + 159,
        BaseID + 160,
        BaseID + 161,
        BaseID + 162,
        BaseID + 163,
        BaseID + 164,
        BaseID + 165,
        
        // Gary's Garden
        BaseID + 183, // Index: 162
        BaseID + 184,
        BaseID + 185,
        BaseID + 186,
        BaseID + 187,
        BaseID + 188,
        BaseID + 189,
        BaseID + 190,
        BaseID + 191,
        BaseID + 192,
        
        // Keys
        BaseID + 91, // Index: 172
        BaseID + 92,
        BaseID + 93,
        BaseID + 94,
        BaseID + 95,
        BaseID + 96,
        BaseID + 97,
        BaseID + 98,
        BaseID + 99,
        // Letters
        BaseID + 80, // Index: 181
        BaseID + 81,
        BaseID + 82,
        BaseID + 83,
        BaseID + 84,
        BaseID + 85,
        BaseID + 86,
        BaseID + 87,
        BaseID + 88,
        BaseID + 89,
        BaseID + 90,
        BaseID + 250,
        
        // Contact Lists
        BaseID + 167, // Index: 193
        BaseID + 168, 
        
        // Seeds
        BaseID + 260, // Index: 195 | Hairball City
        BaseID + 261,
        BaseID + 262,
        BaseID + 263,
        BaseID + 264,
        BaseID + 265,
        BaseID + 266,
        BaseID + 267,
        BaseID + 268,
        BaseID + 269,
        BaseID + 270, // Index: 205 | Salmon Creek Forest
        BaseID + 271,
        BaseID + 272,
        BaseID + 273,
        BaseID + 274,
        BaseID + 275,
        BaseID + 276,
        BaseID + 277,
        BaseID + 278,
        BaseID + 279,
        BaseID + 280, // Index: 215 | Bathhouse
        BaseID + 281,
        BaseID + 282,
        BaseID + 283,
        BaseID + 284,
        BaseID + 285,
        BaseID + 286,
        BaseID + 287,
        BaseID + 288,
        BaseID + 289,
        
        // Applesanity
        BaseID + 322, // Index: 225
        BaseID + 323,
        BaseID + 324,
        BaseID + 325,
        BaseID + 326,
        BaseID + 327,
        BaseID + 328,
        BaseID + 329,
        BaseID + 330,
        BaseID + 331,
        BaseID + 332, 
        BaseID + 333,
        BaseID + 334,
        BaseID + 335,
        BaseID + 336,
        BaseID + 337,
        BaseID + 338,
        BaseID + 339,
        BaseID + 340,
        BaseID + 341,
        BaseID + 342, 
        BaseID + 343,
        BaseID + 344,
        BaseID + 345,
        BaseID + 346,
        BaseID + 347,
        BaseID + 348,
        BaseID + 349,
        BaseID + 350,
        BaseID + 351,
        BaseID + 352, 
        BaseID + 353,
        
        // Turbine Town
        BaseID + 354, // Index: 257
        BaseID + 355,
        BaseID + 356,
        BaseID + 357,
        BaseID + 358,
        BaseID + 359,
        BaseID + 360,
        BaseID + 361,
        BaseID + 362, //265
        BaseID + 363,
        BaseID + 364,
        BaseID + 365,
        BaseID + 366,
        BaseID + 367,
        BaseID + 368,
        BaseID + 369,
        BaseID + 370,
        BaseID + 371,
        BaseID + 372, //275
        BaseID + 373,
        BaseID + 374,
        BaseID + 375,
        BaseID + 376,
        BaseID + 377,
        BaseID + 378,
        BaseID + 379,
        BaseID + 380,
        BaseID + 381,
        BaseID + 382, //285
        BaseID + 383,
        BaseID + 384,
        BaseID + 385,
        
        // Salmon Creek Forest
        BaseID + 386, // Index: 289
        BaseID + 387,
        BaseID + 388,
        BaseID + 389,
        BaseID + 390,
        BaseID + 391,
        BaseID + 392, //295
        BaseID + 393,
        BaseID + 394,
        BaseID + 395,
        BaseID + 396,
        BaseID + 397,
        BaseID + 398,
        BaseID + 399,
        BaseID + 400,
        BaseID + 401,
        BaseID + 402, //305
        BaseID + 403,
        BaseID + 404,
        BaseID + 405,
        BaseID + 406,
        BaseID + 407,
        BaseID + 408,
        BaseID + 409,
        BaseID + 410,
        BaseID + 411,
        BaseID + 412, //315
        BaseID + 413,
        BaseID + 414,
        BaseID + 415,
        BaseID + 416,
        BaseID + 417,
        BaseID + 418,
        BaseID + 419,
        BaseID + 420,
        BaseID + 421,
        BaseID + 422, //325
        BaseID + 423,
        BaseID + 424,
        BaseID + 425,
        BaseID + 426,
        BaseID + 427,
        BaseID + 428,
        BaseID + 429,
        BaseID + 430,
        BaseID + 431,
        BaseID + 432, //335
        BaseID + 433,
        BaseID + 434,
        BaseID + 435,
        BaseID + 436,
        BaseID + 437,
        BaseID + 438,
        BaseID + 439,
        BaseID + 440,
        BaseID + 441,
        BaseID + 442, //345
        BaseID + 443,
        BaseID + 444,
        BaseID + 445,
        BaseID + 446,
        BaseID + 447,
        BaseID + 448,
        BaseID + 449,
        BaseID + 450,
        BaseID + 451,
        BaseID + 452, //355
        BaseID + 453,
        BaseID + 454,
        BaseID + 455,
        BaseID + 456,
        BaseID + 457,
        BaseID + 458,
        BaseID + 459,
        BaseID + 460,
        BaseID + 461,
        BaseID + 462, //365
        BaseID + 463,
        BaseID + 464,
        BaseID + 465,
        BaseID + 466,
        BaseID + 467,
        BaseID + 468,
        BaseID + 469,
        BaseID + 470,
        BaseID + 471,
        
        // Public Pool
        BaseID + 472,  // Index: 375
        BaseID + 473,
        BaseID + 474,
        BaseID + 475,
        BaseID + 476,
        BaseID + 477,
        BaseID + 478,
        BaseID + 479,
        BaseID + 480,
        BaseID + 481,
        BaseID + 482, //385
        BaseID + 483,
        BaseID + 484,
        BaseID + 485,
        BaseID + 486,
        BaseID + 487,
        BaseID + 488,
        BaseID + 489,
        BaseID + 490,
        BaseID + 491,
        BaseID + 492, //395
        BaseID + 493,
        BaseID + 494,
        BaseID + 495,
        BaseID + 496,
        BaseID + 497,
        BaseID + 498,
        BaseID + 499,
        BaseID + 500,
        BaseID + 501,
        BaseID + 502, //405
        BaseID + 503,
        BaseID + 504,
        BaseID + 505,
        BaseID + 506,
        BaseID + 507,
        BaseID + 508,
        BaseID + 509,
        BaseID + 510,
        BaseID + 511,
        BaseID + 512, //415
        BaseID + 513,
        BaseID + 514,
        BaseID + 515,
        BaseID + 516,
        BaseID + 517,
        BaseID + 518,
        BaseID + 519,
        BaseID + 520,
        BaseID + 521,
        BaseID + 522, //425
        BaseID + 523,
        BaseID + 524,
        BaseID + 525,
        BaseID + 526,
        BaseID + 527,
        BaseID + 528,
        BaseID + 529,
        BaseID + 530,
        BaseID + 531,
        BaseID + 532, //435
        BaseID + 533,
        BaseID + 534,
        BaseID + 535,
        BaseID + 536,
        BaseID + 537,
        BaseID + 538,
        BaseID + 539,
        
        // Bathhouse
        BaseID + 540, // Index: 443
        BaseID + 541,
        BaseID + 542, //445
        BaseID + 543,
        BaseID + 544,
        BaseID + 545,
        BaseID + 546,
        BaseID + 547,
        BaseID + 548,
        BaseID + 549,
        BaseID + 550,
        BaseID + 551,
        BaseID + 552, //455
        BaseID + 553,
        BaseID + 554,
        BaseID + 555,
        BaseID + 556,
        BaseID + 557,
        BaseID + 558,
        BaseID + 559,
        BaseID + 560,
        BaseID + 561,
        BaseID + 562, //465
        BaseID + 563,
        BaseID + 564,
        BaseID + 565,
        BaseID + 566,
        BaseID + 567,
        BaseID + 568,
        BaseID + 569,
        BaseID + 570,
        BaseID + 571,
        BaseID + 572, //475
        BaseID + 573,
        BaseID + 574,
        BaseID + 575,
        BaseID + 576,
        BaseID + 577,
        BaseID + 578,
        BaseID + 579,
        BaseID + 580,
        BaseID + 581,
        BaseID + 582, //485
        BaseID + 583,
        BaseID + 584,
        BaseID + 585,
        BaseID + 586,
        BaseID + 587,
        BaseID + 588,
        BaseID + 589,
        BaseID + 590,
        BaseID + 591,
        BaseID + 592, //495
        BaseID + 593,
        BaseID + 594,
        BaseID + 595,
        BaseID + 596,
        BaseID + 597,
        BaseID + 598,
        BaseID + 599,
        BaseID + 600,
        BaseID + 601,
        BaseID + 602, //505
        BaseID + 603,
        BaseID + 604,
        BaseID + 605,
        BaseID + 606,
        
        // Tadpole HQ
        BaseID + 607, // Index: 510
        BaseID + 608,
        BaseID + 609,
        BaseID + 610,
        BaseID + 611,
        BaseID + 612, //515
        BaseID + 613,
        BaseID + 614,
        BaseID + 615,
        BaseID + 616,
        BaseID + 617, //520
        
        // Progressive MiMa
        BaseID + 620,
        BaseID + 621,
        BaseID + 622,
        BaseID + 623,
        BaseID + 624, //525
        BaseID + 625,
        BaseID + 626,
        BaseID + 627,
        BaseID + 628,
        BaseID + 629, // 530
        BaseID + 630,
        BaseID + 631,
        BaseID + 632,
        BaseID + 633, // 534
    };
    
    public static readonly long[] ScoutIDs = ScoutList.ToArray();
    
    public static readonly Dictionary<long, string> ScoutHCCoinList = new()
    {
        { BaseID + 0, "Fetch" },
        // Hairball City
        { BaseID + 3, "volley" },
        { BaseID + 4, "Dustan" },
        { BaseID + 5, "flowerPuzzle" },
        { BaseID + 6, "main" },
        { BaseID + 7, "fishing" },
        { BaseID + 8, "bug" },
        { BaseID + 9, "graffiti" },
        { BaseID + 10, "hamsterball" },
        //{ BaseID + 11, "cassetteCoin" },
        //{ BaseID + 12, "cassetteCoin2" },
        { BaseID + 13, "gamerQuest" },
        { BaseID + 14, "arcadeBone" },
        { BaseID + 15, "arcade" },
        { BaseID + 16, "carrynojump" },
    };
    
    public static readonly Dictionary<long, string> ScoutTTCoinList = new()
    {
        // Turbine Town
        { BaseID + 17, "fishing" },
        { BaseID + 18, "volley" },
        { BaseID + 19, "flowerPuzzle" },
        { BaseID + 20, "main" },
        { BaseID + 21, "bug" },
        { BaseID + 22, "Dustan" },
        //{ BaseID + 23, "cassetteCoin" },
        //{ BaseID + 24, "cassetteCoin2" },
        { BaseID + 25, "arcadeBone" },
        { BaseID + 26, "arcade" },
        { BaseID + 27, "carrynojump" },
    };
    
    public static readonly Dictionary<long, string> ScoutSFCCoinList = new()
    {
        // Salmon Creek Forest
        { BaseID + 28, "main" },
        //{ BaseID + 29, "cassetteCoin" },
        { BaseID + 30, "Dustan" },
        { BaseID + 31, "hamsterball" },
        { BaseID + 32, "arcadeBone" },
        { BaseID + 33, "tree" },
        { BaseID + 34, "bug" },
        { BaseID + 35, "behindWaterfall" },
        { BaseID + 36, "volley" },
        { BaseID + 37, "flowerPuzzle" },
        { BaseID + 38, "graffiti" },
        //{ BaseID + 39, "cassetteCoin2" },
        { BaseID + 40, "fishing" },
        { BaseID + 41, "gamerQuest" },
        { BaseID + 42, "arcade" },
        { BaseID + 43, "carrynojump" },
    };
    
    public static readonly Dictionary<long, string> ScoutPPCoinList = new()
    {
        // Public Pool
        { BaseID + 44, "2D" },
        //{ BaseID + 45, "cassetteCoin2" },
        { BaseID + 46, "arcadeBone" },
        { BaseID + 47, "arcade" },
        { BaseID + 48, "fishing" },
        { BaseID + 49, "main" },
        { BaseID + 50, "volley" },
        { BaseID + 51, "bug" },
        //{ BaseID + 52, "cassetteCoin" },
        { BaseID + 53, "flowerPuzzle" },
    };
    
    public static readonly Dictionary<long, string> ScoutBathCoinList = new()
    {
        // Bathhouse
        { BaseID + 54, "carrynojump" },
        { BaseID + 55, "hamsterball" },
        { BaseID + 56, "main" },
        { BaseID + 57, "graffiti" },
        //{ BaseID + 58, "cassetteCoin" },
        //{ BaseID + 59, "cassetteCoin2" },
        { BaseID + 60, "Dustan" },
        { BaseID + 61, "volley" },
        { BaseID + 62, "gamerQuest" },
        { BaseID + 63, "fishing" },
        { BaseID + 64, "bug" },
        { BaseID + 65, "flowerPuzzle" },
        { BaseID + 66, "arcadeBone" },
        { BaseID + 67, "arcade" },
    };
    
    public static readonly Dictionary<long, string> ScoutHQCoinList = new()
    {
        // Tadpole HQ
        //{ BaseID + 68, "cassetteCoin2" },
        //{ BaseID + 69, "cassetteCoin" },
        { BaseID + 70, "main" },
        { BaseID + 71, "volley" },
        { BaseID + 72, "fishing" },
        { BaseID + 73, "flowerPuzzle" },
        { BaseID + 74, "arcade" },
        { BaseID + 75, "bug" },
        { BaseID + 76, "carrynojump" },
        { BaseID + 77, "arcadeBone" },
        { BaseID + 198, "Gary" }
    };
    
    public static readonly Dictionary<long, string> ScoutMiMaList = new()
    {
        //Mitch & Mai
        { BaseID + 11, "cassetteCoin"}, // Mitch
        { BaseID + 12, "cassetteCoin2"}, // Mai
        { BaseID + 23, "cassetteCoin"}, // Mitch
        { BaseID + 24, "cassetteCoin2"}, // Mai
        { BaseID + 29, "cassetteCoin"}, // Mitch
        { BaseID + 39, "cassetteCoin2"}, // Mai
        { BaseID + 45, "cassetteCoin2"}, // Mai
        { BaseID + 52, "cassetteCoin"}, // Mitch
        { BaseID + 58, "cassetteCoin"}, // Mitch
        { BaseID + 59, "cassetteCoin2"}, // Mai
        { BaseID + 68, "cassetteCoin2"}, // Mai
        { BaseID + 69, "cassetteCoin"}, // Mitch
        { BaseID + 199, "cassetteCoin2"}, // Mai
        { BaseID + 200, "cassetteCoin"}, // Mitch
    };
    
    public static readonly Dictionary<long, string> ScoutHCCassetteList = new()
    {
        // Hairball City
        { BaseID + 100, "casHairballCity0" },
        { BaseID + 101, "casHairballCity1" },
        { BaseID + 102, "casHairballCity2" },
        { BaseID + 103, "casHairballCity3" },
        { BaseID + 104, "casHairballCity4" },
        { BaseID + 105, "casHairballCity5" },
        { BaseID + 106, "casHairballCity6" },
        { BaseID + 107, "casHairballCity7" },
        { BaseID + 108, "casHairballCity8" },
        { BaseID + 109, "casHairballCity9" },
    };
    
    public static readonly Dictionary<long, string> ScoutTTCassetteList = new()
    {
        // Turbine Town
        { BaseID + 111, "Cassette" },
        { BaseID + 112, "Cassette (1)" },
        { BaseID + 113, "Cassette (2)" },
        { BaseID + 114, "Cassette (3)" },
        { BaseID + 115, "Cassette (4)" },
        { BaseID + 116, "Cassette (5)" },
        { BaseID + 117, "Cassette (6)" },
        { BaseID + 118, "Cassette (7)" },
        { BaseID + 119, "Cassette (8)" },
        { BaseID + 120, "Cassette (9)" },
    };
    
    public static readonly Dictionary<long, string> ScoutSFCCassetteList = new()
    {
        // Salmon Creek Forest
        { BaseID + 122, "Cassette" },
        { BaseID + 123, "Cassette (1)" },
        { BaseID + 124, "Cassette (2)" },
        { BaseID + 125, "Cassette (3)" },
        { BaseID + 126, "Cassette (4)" },
        { BaseID + 127, "Cassette (5)" },
        { BaseID + 128, "Cassette (6)" },
        { BaseID + 129, "Cassette (7)" },
        { BaseID + 130, "Cassette (8)" },
        { BaseID + 131, "Cassette (9)" },
        { BaseID + 132, "Cassette (10)" },
    };
    
    public static readonly Dictionary<long, string> ScoutPPCassetteList = new()
    {
        // Public Pool
        { BaseID + 134, "Cassette" },
        { BaseID + 135, "Cassette (1)" },
        { BaseID + 136, "Cassette (2)" },
        { BaseID + 137, "Cassette (3)" },
        { BaseID + 138, "Cassette (4)" },
        { BaseID + 139, "Cassette (5)" },
        { BaseID + 140, "Cassette (6)" },
        { BaseID + 141, "Cassette (7)" },
        { BaseID + 142, "Cassette (8)" },
        { BaseID + 143, "Cassette (9)" },
    };
    
    public static readonly Dictionary<long, string> ScoutBathCassetteList = new()
    {
        // Bathhouse
        { BaseID + 145, "Cassette" },
        { BaseID + 146, "Cassette (1)" },
        { BaseID + 147, "Cassette (2)" },
        { BaseID + 148, "Cassette (3)" },
        { BaseID + 149, "Cassette (4)" },
        { BaseID + 150, "Cassette (5)" },
        { BaseID + 151, "Cassette (6)" },
        { BaseID + 152, "Cassette (7)" },
        { BaseID + 153, "Cassette (8)" },
        { BaseID + 154, "Cassette (9)" },
    };
    
    public static readonly Dictionary<long, string> ScoutHQCassetteList = new()
    {
        // Tadpole HQ
        { BaseID + 156, "Cassette" },
        { BaseID + 157, "Cassette (1)" },
        { BaseID + 158, "Cassette (2)" },
        { BaseID + 159, "Cassette (3)" },
        { BaseID + 160, "Cassette (4)" },
        { BaseID + 161, "Cassette (5)" },
        { BaseID + 162, "Cassette (6)" },
        { BaseID + 163, "Cassette (7)" },
        { BaseID + 164, "Cassette (8)" },
        { BaseID + 165, "Cassette (9)" },
    };
    
    public static readonly Dictionary<long, string> ScoutGardenCassetteList = new()
    {
        // Gary's Garden
        { BaseID + 183, "Cassette" },
        { BaseID + 184, "Cassette (1)" },
        { BaseID + 185, "Cassette (2)" },
        { BaseID + 186, "Cassette (3)" },
        { BaseID + 187, "Cassette (4)" },
        { BaseID + 188, "Cassette (5)" },
        { BaseID + 189, "Cassette (6)" },
        { BaseID + 190, "Cassette (7)" },
        { BaseID + 191, "Cassette (8)" },
        { BaseID + 192, "Cassette (9)" },
    };
    
    public static readonly Dictionary<long, string> ScoutKeyList = new()
    {
        // Keys
        { BaseID + 91, "containerKey" },
        { BaseID + 92, "parasolKey" },
        { BaseID + 93, "1Key" },
        { BaseID + 94, "2Key" },
        { BaseID + 95, "3Key" },
        { BaseID + 96, "testKey" },
        { BaseID + 97, "ontoriiKey" },
        { BaseID + 98, "inpuzzleKey" },
        { BaseID + 99, "underfloorKey" },
    };
    
    public static readonly Dictionary<long, string> ScoutLetterList = new()
    {
        // Keys
        { BaseID + 80, "letter12" },
        { BaseID + 81, "letter7" },
        { BaseID + 82, "letter1" },
        { BaseID + 83, "letter2" },
        { BaseID + 84, "letter8" },
        { BaseID + 85, "letter9" },
        { BaseID + 86, "letter3" },
        { BaseID + 87, "letter10" },
        { BaseID + 88, "letter4" },
        { BaseID + 89, "letter11" },
        { BaseID + 90, "letter5" },
        { BaseID + 250, "letter6" },
    };
    
    public static readonly Dictionary<long, string> ScoutContactList = new()
    {
        // Contact Lists
        { BaseID + 167, "CL1 Obtained" },
        { BaseID + 168, "CL2 Obtained" },
    };
    
    public static readonly Dictionary<long, string> ScoutHCSeedsList = new()
    {
        // Seeds
        { BaseID + 260, "Seed" },
        { BaseID + 261, "Seed(1)" },
        { BaseID + 262, "Seed(2)" },
        { BaseID + 263, "Seed(3)" },
        { BaseID + 264, "Seed(4)" },
        { BaseID + 265, "Seed(5)" },
        { BaseID + 266, "Seed(6)" },
        { BaseID + 267, "Seed(7)" },
        { BaseID + 268, "Seed(8)" },
        { BaseID + 269, "Seed(9)" },
    };
    
    public static readonly Dictionary<long, string> ScoutSFCSeedsList = new()
    {
        // Seeds
        { BaseID + 270, "Seed" },
        { BaseID + 271, "Seed(1)" },
        { BaseID + 272, "Seed(2)" },
        { BaseID + 273, "Seed(3)" },
        { BaseID + 274, "Seed(4)" },
        { BaseID + 275, "Seed(5)" },
        { BaseID + 276, "Seed(6)" },
        { BaseID + 277, "Seed(7)" },
        { BaseID + 278, "Seed(8)" },
        { BaseID + 279, "Seed(9)" },
    };
    
    public static readonly Dictionary<long, string> ScoutBathSeedsList = new()
    {
        // Seeds
        { BaseID + 280, "Seed" },
        { BaseID + 281, "Seed(1)" },
        { BaseID + 282, "Seed(2)" },
        { BaseID + 283, "Seed(3)" },
        { BaseID + 284, "Seed(4)" },
        { BaseID + 285, "Seed(5)" },
        { BaseID + 286, "Seed(6)" },
        { BaseID + 287, "Seed(7)" },
        { BaseID + 288, "Seed(8)" },
        { BaseID + 289, "Seed(9)" }
    };
    
    public static readonly Dictionary<long, string> ScoutHCFlowersList = new()
    {
        // Flowerbeds
        { BaseID + 300, "FPuzzle" },
        { BaseID + 301, "FPuzzle (1)" },
        { BaseID + 302, "FPuzzle (2)" },
        // { BaseID + 263, "FPuzzle (3)" },
        // { BaseID + 264, "FPuzzle (4)" },
        // { BaseID + 265, "FPuzzle (5)" },
        // { BaseID + 266, "FPuzzle (6)" },
    };
    
    public static Dictionary<long, string> ScoutHCApplesList = new()
    {
        //Applesanity
        { BaseID + 322, "Apple Float" },
        { BaseID + 323, "Apple Float (1)" },
        { BaseID + 324, "Apple Float (2)" },
        { BaseID + 325, "Apple Float (3)" },
        { BaseID + 326, "Apple Float (4)" }, 
        { BaseID + 327, "Apple Float (5)" },
        { BaseID + 328, "Apple Float (6)" }, 
        { BaseID + 329, "Apple Float (7)" },
        { BaseID + 330, "Apple Float (8)" }, 
        { BaseID + 331, "Apple Float (9)" },
        { BaseID + 332, "Apple Float (10)" }, 
        { BaseID + 333, "Apple Float (11)" },
        { BaseID + 334, "Apple Float (12)" }, 
        { BaseID + 335, "Apple Float (13)" },
        { BaseID + 336, "Apple Float (14)" }, 
        { BaseID + 337, "Apple Float (15)" },
        { BaseID + 338, "Apple Float (16)" }, 
        { BaseID + 339, "Apple Float (17)" },
        { BaseID + 340, "Apple Float (18)" }, 
        { BaseID + 341, "Apple Float (19)" },
        { BaseID + 342, "Apple Float (20)" }, 
        { BaseID + 343, "Apple Float (21)" },
        { BaseID + 344, "Apple Float (22)" }, 
        { BaseID + 345, "Apple Float (23)" },
        { BaseID + 346, "Apple Float (24)" }, 
        { BaseID + 347, "Apple Float (25)" },
        { BaseID + 348, "Apple Float (26)" }, 
        { BaseID + 349, "Apple Float (27)" },
        { BaseID + 350, "Apple Float (28)" }, 
        { BaseID + 351, "Apple Float (29)" },
        { BaseID + 352, "Apple Float (30)" }, 
        { BaseID + 353, "Apple Float (31)" },
    };
    
    public static Dictionary<long, string> ScoutTTApplesList = new()
    {
        //Applesanity
        { BaseID + 354, "Apple Float" },
        { BaseID + 355, "Apple Float (1)" },
        { BaseID + 356, "Apple Float (2)" },
        { BaseID + 357, "Apple Float (3)" },
        { BaseID + 358, "Apple Float (4)" }, 
        { BaseID + 359, "Apple Float (5)" },
        { BaseID + 360, "Apple Float (6)" }, 
        { BaseID + 361, "Apple Float (7)" },
        { BaseID + 362, "Apple Float (8)" }, 
        { BaseID + 363, "Apple Float (9)" },
        { BaseID + 364, "Apple Float (10)" }, 
        { BaseID + 365, "Apple Float (11)" },
        { BaseID + 366, "Apple Float (12)" }, 
        { BaseID + 367, "Apple Float (13)" },
        { BaseID + 368, "Apple Float (14)" }, 
        { BaseID + 369, "Apple Float (15)" },
        { BaseID + 370, "Apple Float (16)" }, 
        { BaseID + 371, "Apple Float (17)" },
        { BaseID + 372, "Apple Float (18)" }, 
        { BaseID + 373, "Apple Float (19)" },
        { BaseID + 374, "Apple Float (20)" }, 
        { BaseID + 375, "Apple Float (21)" },
        { BaseID + 376, "Apple Float (22)" }, 
        { BaseID + 377, "Apple Float (23)" },
        { BaseID + 378, "Apple Float (24)" }, 
        { BaseID + 379, "Apple Float (25)" },
        { BaseID + 380, "Apple Float (26)" }, 
        { BaseID + 381, "Apple Float (27)" },
        { BaseID + 382, "Apple Float (28)" }, 
        { BaseID + 383, "Apple Float (29)" },
        { BaseID + 384, "Apple Float (30)" }, 
        { BaseID + 385, "Apple Float (31)" },
    };
    
    public static Dictionary<long, string> ScoutSFCApplesList = new()
    {
        //Applesanity
        { BaseID + 386, "Apple Float" },
        { BaseID + 387, "Apple Float (1)" },
        { BaseID + 388, "Apple Float (2)" },
        { BaseID + 389, "Apple Float (3)" },
        { BaseID + 390, "Apple Float (4)" }, 
        { BaseID + 391, "Apple Float (5)" },
        { BaseID + 392, "Apple Float (6)" }, 
        { BaseID + 393, "Apple Float (7)" },
        { BaseID + 394, "Apple Float (8)" }, 
        { BaseID + 395, "Apple Float (9)" },
        { BaseID + 396, "Apple Float (10)" }, 
        { BaseID + 397, "Apple Float (11)" },
        { BaseID + 398, "Apple Float (12)" }, 
        { BaseID + 399, "Apple Float (13)" },
        { BaseID + 400, "Apple Float (14)" }, 
        { BaseID + 401, "Apple Float (15)" },
        { BaseID + 402, "Apple Float (16)" }, 
        { BaseID + 403, "Apple Float (17)" },
        { BaseID + 404, "Apple Float (18)" }, 
        { BaseID + 405, "Apple Float (19)" },
        { BaseID + 406, "Apple Float (20)" }, 
        { BaseID + 407, "Apple Float (21)" },
        { BaseID + 408, "Apple Float (22)" }, 
        { BaseID + 409, "Apple Float (23)" },
        { BaseID + 410, "Apple Float (24)" }, 
        { BaseID + 411, "Apple Float (25)" },
        { BaseID + 412, "Apple Float (26)" }, 
        { BaseID + 413, "Apple Float (27)" },
        { BaseID + 414, "Apple Float (28)" }, 
        { BaseID + 415, "Apple Float (29)" },
        { BaseID + 416, "Apple Float (30)" }, 
        { BaseID + 417, "Apple Float (31)" },
        { BaseID + 418, "Apple Float (32)" },
        { BaseID + 419, "Apple Float (33)" },
        { BaseID + 420, "Apple Float (34)" },
        { BaseID + 421, "Apple Float (35)" }, 
        { BaseID + 422, "Apple Float (36)" },
        { BaseID + 423, "Apple Float (37)" }, 
        { BaseID + 424, "Apple Float (38)" },
        { BaseID + 425, "Apple Float (39)" }, 
        { BaseID + 426, "Apple Float (40)" },
        { BaseID + 427, "Apple Float (41)" }, 
        { BaseID + 428, "Apple Float (42)" },
        { BaseID + 429, "Apple Float (43)" }, 
        { BaseID + 430, "Apple Float (44)" },
        { BaseID + 431, "Apple Float (45)" }, 
        { BaseID + 432, "Apple Float (46)" },
        { BaseID + 433, "Apple Float (47)" }, 
        { BaseID + 434, "Apple Float (48)" },
        { BaseID + 435, "Apple Float (49)" }, 
        { BaseID + 436, "Apple Float (50)" },
        { BaseID + 437, "Apple Float (51)" }, 
        { BaseID + 438, "Apple Float (52)" },
        { BaseID + 439, "Apple Float (53)" }, 
        { BaseID + 440, "Apple Float (54)" },
        { BaseID + 441, "Apple Float (55)" }, 
        { BaseID + 442, "Apple Float (56)" },
        { BaseID + 443, "Apple Float (57)" }, 
        { BaseID + 444, "Apple Float (58)" },
        { BaseID + 445, "Apple Float (59)" }, 
        { BaseID + 446, "Apple Float (60)" },
        { BaseID + 447, "Apple Float (61)" }, 
        { BaseID + 448, "Apple Float (62)" },
        { BaseID + 449, "Apple Float (63)" },
        { BaseID + 450, "Apple Float (64)" }, 
        { BaseID + 451, "Apple Float (65)" },
        { BaseID + 452, "Apple Float (66)" }, 
        { BaseID + 453, "Apple Float (67)" },
        { BaseID + 454, "Apple Float (68)" }, 
        { BaseID + 455, "Apple Float (69)" },
        { BaseID + 456, "Apple Float (70)" }, 
        { BaseID + 457, "Apple Float (71)" },
        { BaseID + 458, "Apple Float (72)" }, 
        { BaseID + 459, "Apple Float (73)" },
        { BaseID + 460, "Apple Float (74)" }, 
        { BaseID + 461, "Apple Float (75)" },
        { BaseID + 462, "Apple Float (76)" }, 
        { BaseID + 463, "Apple Float (77)" },
        { BaseID + 464, "Apple Float (78)" }, 
        { BaseID + 465, "Apple Float (79)" },
        { BaseID + 466, "Apple Float (80)" }, 
        { BaseID + 467, "Apple Float (81)" },
        { BaseID + 468, "Apple Float (82)" }, 
        { BaseID + 469, "Apple Float (83)" },
        { BaseID + 470, "Apple Float (84)" }, 
        { BaseID + 471, "Apple Float (85)" },
    };
    
    public static Dictionary<long, string> ScoutPPApplesList = new()
    {
        //Applesanity
        { BaseID + 472, "Apple Float" },
        { BaseID + 473, "Apple Float (1)" },
        { BaseID + 474, "Apple Float (2)" },
        { BaseID + 475, "Apple Float (3)" },
        { BaseID + 476, "Apple Float (4)" }, 
        { BaseID + 477, "Apple Float (5)" },
        { BaseID + 478, "Apple Float (6)" }, 
        { BaseID + 479, "Apple Float (7)" },
        { BaseID + 480, "Apple Float (8)" }, 
        { BaseID + 481, "Apple Float (9)" },
        { BaseID + 482, "Apple Float (10)" }, 
        { BaseID + 483, "Apple Float (11)" },
        { BaseID + 484, "Apple Float (12)" }, 
        { BaseID + 485, "Apple Float (13)" },
        { BaseID + 486, "Apple Float (14)" }, 
        { BaseID + 487, "Apple Float (15)" },
        { BaseID + 488, "Apple Float (16)" }, 
        { BaseID + 489, "Apple Float (17)" },
        { BaseID + 490, "Apple Float (18)" }, 
        { BaseID + 491, "Apple Float (19)" },
        { BaseID + 492, "Apple Float (20)" }, 
        { BaseID + 493, "Apple Float (21)" },
        { BaseID + 494, "Apple Float (22)" }, 
        { BaseID + 495, "Apple Float (23)" },
        { BaseID + 496, "Apple Float (24)" }, 
        { BaseID + 497, "Apple Float (25)" },
        { BaseID + 498, "Apple Float (26)" }, 
        { BaseID + 499, "Apple Float (27)" },
        { BaseID + 500, "Apple Float (28)" },
        { BaseID + 501, "Apple Float (29)" },
        { BaseID + 502, "Apple Float (30)" },
        { BaseID + 503, "Apple Float (31)" }, 
        { BaseID + 504, "Apple Float (32)" },
        { BaseID + 505, "Apple Float (33)" }, 
        { BaseID + 506, "Apple Float (34)" },
        { BaseID + 507, "Apple Float (35)" }, 
        { BaseID + 508, "Apple Float (36)" },
        { BaseID + 509, "Apple Float (37)" }, 
        { BaseID + 510, "Apple Float (38)" },
        { BaseID + 511, "Apple Float (39)" }, 
        { BaseID + 512, "Apple Float (40)" },
        { BaseID + 513, "Apple Float (41)" }, 
        { BaseID + 514, "Apple Float (42)" },
        { BaseID + 515, "Apple Float (43)" }, 
        { BaseID + 516, "Apple Float (44)" },
        { BaseID + 517, "Apple Float (45)" }, 
        { BaseID + 518, "Apple Float (46)" },
        { BaseID + 519, "Apple Float (47)" }, 
        { BaseID + 520, "Apple Float (48)" },
        { BaseID + 521, "Apple Float (49)" }, 
        { BaseID + 522, "Apple Float (50)" },
        { BaseID + 523, "Apple Float (51)" }, 
        { BaseID + 524, "Apple Float (52)" },
        { BaseID + 525, "Apple Float (53)" }, 
        { BaseID + 526, "Apple Float (54)" },
        { BaseID + 527, "Apple Float (55)" }, 
        { BaseID + 528, "Apple Float (56)" },
        { BaseID + 529, "Apple Float (57)" }, 
        { BaseID + 530, "Apple Float (58)" },
        { BaseID + 531, "Apple Float (59)" },
        { BaseID + 532, "Apple Float (60)" }, 
        { BaseID + 533, "Apple Float (61)" },
        { BaseID + 534, "Apple Float (62)" }, 
        { BaseID + 535, "Apple Float (63)" },
        { BaseID + 536, "Apple Float (64)" },
        { BaseID + 537, "Apple Float (65)" }, 
        { BaseID + 538, "Apple Float (66)" },
        { BaseID + 539, "Apple Float (67)" }, 
    };
    
    public static Dictionary<long, string> ScoutBathApplesList = new()
    {
        //Applesanity
        { BaseID + 540, "Apple Float" },
        { BaseID + 541, "Apple Float (1)" },
        { BaseID + 542, "Apple Float (2)" },
        { BaseID + 543, "Apple Float (3)" },
        { BaseID + 544, "Apple Float (4)" }, 
        { BaseID + 545, "Apple Float (5)" },
        { BaseID + 546, "Apple Float (6)" }, 
        { BaseID + 547, "Apple Float (7)" },
        { BaseID + 548, "Apple Float (8)" }, 
        { BaseID + 549, "Apple Float (9)" },
        { BaseID + 550, "Apple Float (10)" }, 
        { BaseID + 551, "Apple Float (11)" },
        { BaseID + 552, "Apple Float (12)" }, 
        { BaseID + 553, "Apple Float (13)" },
        { BaseID + 554, "Apple Float (14)" }, 
        { BaseID + 555, "Apple Float (15)" },
        { BaseID + 556, "Apple Float (16)" }, 
        { BaseID + 557, "Apple Float (17)" },
        { BaseID + 558, "Apple Float (18)" }, 
        { BaseID + 559, "Apple Float (19)" },
        { BaseID + 560, "Apple Float (20)" }, 
        { BaseID + 561, "Apple Float (21)" },
        { BaseID + 562, "Apple Float (22)" }, 
        { BaseID + 563, "Apple Float (23)" },
        { BaseID + 564, "Apple Float (24)" }, 
        { BaseID + 565, "Apple Float (25)" },
        { BaseID + 566, "Apple Float (26)" }, 
        { BaseID + 567, "Apple Float (27)" },
        { BaseID + 568, "Apple Float (28)" },
        { BaseID + 569, "Apple Float (29)" },
        { BaseID + 570, "Apple Float (30)" },
        { BaseID + 571, "Apple Float (31)" }, 
        { BaseID + 572, "Apple Float (32)" },
        { BaseID + 573, "Apple Float (33)" }, 
        { BaseID + 574, "Apple Float (34)" },
        { BaseID + 575, "Apple Float (35)" }, 
        { BaseID + 576, "Apple Float (36)" },
        { BaseID + 577, "Apple Float (37)" }, 
        { BaseID + 578, "Apple Float (38)" },
        { BaseID + 579, "Apple Float (39)" }, 
        { BaseID + 580, "Apple Float (40)" },
        { BaseID + 581, "Apple Float (41)" }, 
        { BaseID + 582, "Apple Float (42)" },
        { BaseID + 583, "Apple Float (43)" }, 
        { BaseID + 584, "Apple Float (44)" },
        { BaseID + 585, "Apple Float (45)" }, 
        { BaseID + 586, "Apple Float (46)" },
        { BaseID + 587, "Apple Float (47)" }, 
        { BaseID + 588, "Apple Float (48)" },
        { BaseID + 589, "Apple Float (49)" }, 
        { BaseID + 590, "Apple Float (50)" },
        { BaseID + 591, "Apple Float (51)" }, 
        { BaseID + 592, "Apple Float (52)" },
        { BaseID + 593, "Apple Float (53)" }, 
        { BaseID + 594, "Apple Float (54)" },
        { BaseID + 595, "Apple Float (55)" }, 
        { BaseID + 596, "Apple Float (56)" },
        { BaseID + 597, "Apple Float (57)" }, 
        { BaseID + 598, "Apple Float (58)" },
        { BaseID + 599, "Apple Float (59)" },
        { BaseID + 600, "Apple Float (60)" }, 
        { BaseID + 601, "Apple Float (61)" },
        { BaseID + 602, "Apple Float (62)" }, 
        { BaseID + 603, "Apple Float (63)" },
        { BaseID + 604, "Apple Float (64)" },
        { BaseID + 605, "Apple Float (65)" }, 
        { BaseID + 606, "Apple Float (66)" },
    };
    
    public static Dictionary<long, string> ScoutHQApplesList = new()
    {
        //Applesanity
        { BaseID + 607, "Apple Float" },
        { BaseID + 608, "Apple Float (1)" },
        { BaseID + 609, "Apple Float (2)" },
        { BaseID + 610, "Apple Float (3)" },
        { BaseID + 611, "Apple Float (4)" }, 
        { BaseID + 612, "Apple Float (5)" },
        { BaseID + 613, "Apple Float (6)" }, 
        { BaseID + 614, "Apple Float (7)" },
        { BaseID + 615, "Apple Float (8)" }, 
        { BaseID + 616, "Apple Float (9)" },
        { BaseID + 617, "Apple Float (10)" },
    };
    
    public static Dictionary<long, string> ScoutProgressiveMitchMaiList = new()
    {
        //Coins
        { BaseID + 620, "MiMa1" },
        { BaseID + 621, "MiMa2" },
        { BaseID + 622, "MiMa3" },
        { BaseID + 623, "MiMa4" },
        { BaseID + 624, "MiMa5" },
        { BaseID + 625, "MiMa6" },
        { BaseID + 626, "MiMa7" },
        { BaseID + 627, "MiMa8" },
        { BaseID + 628, "MiMa9" },
        { BaseID + 629, "MiMa10" },
        { BaseID + 630, "MiMa11" },
        { BaseID + 631, "MiMa12" },
        { BaseID + 632, "MiMa13" },
        { BaseID + 633, "MiMa14" },
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

