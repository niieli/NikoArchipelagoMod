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

    public static Dictionary<long, Location> LocationNames = new Dictionary<long, Location>
    {
        { BaseID, new Location(BaseID, "Fetch", 0) },
        { BaseID+3, new Location(BaseID+3,"volley", 1)},
        { BaseID+4, new Location(BaseID+4, "Dustan",1)},
        { BaseID+5, new Location(BaseID+5,"flowerPuzzle", 1)},
        { BaseID+6, new Location(BaseID+6, "main",1)},
        { BaseID+7, new Location(BaseID+7,"fishing", 1)},
        { BaseID+8, new Location(BaseID+8, "bug",1)},
        // { BaseID+9, new Location(BaseID+9,"", 1)},
        // { BaseID+10, new Location(BaseID+10, "",1)},
        // { BaseID+11, new Location(BaseID+11,"", 1)},
        // { BaseID+12, new Location(BaseID+12, "",1)},
        // { BaseID+13, new Location(BaseID+13,"", 1)},
        // { BaseID+14, new Location(BaseID+14, "",1)},
        // { BaseID+15, new Location(BaseID+15,"", 1)},
        // { BaseID+16, new Location(BaseID+16, "",1)},
        { BaseID+17, new Location(BaseID+17, "fishing",2)},

    };

    public static void AddLocation(long id, string flag, int level)
    {
        LocationNames[id] = new Location(id, flag, level);
    }

    public static Location GetLocation(long id)
    {
        return LocationNames.ContainsKey(id) ? LocationNames[id] : null;
    }
}

