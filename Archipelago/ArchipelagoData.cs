using System.Collections.Generic;
using Newtonsoft.Json;

namespace NikoArchipelago.Archipelago;

public class ArchipelagoData
{
    public string Uri;
    public string SlotName;
    public string Password;
    public int Index;

    public List<long> CheckedLocations;

    /// <summary>
    /// seed for this archipelago data. Can be used when loading a file to verify the session the player is trying to
    /// load is valid to the room it's connecting to.
    /// </summary>
    private string seed;

    public static Dictionary<string, object> slotData;
    public static int FishsanitySetting { private set; get; } = 0;
    public static int SeedsanitySetting { private set; get; } = 0;
    public static int FlowersanitySetting { private set; get; } = 0;
    public static int BonesanitySetting { private set; get; } = 0;
    public static int ApplesanitySetting { private set; get; } = 0;
    public static int BugsanitySetting { private set; get; } = 0;
    public static int ChatsanitySetting { private set; get; } = 0;
    public static int ThoughtsanitySetting { private set; get; } = 0;
    public static int SnailshopSetting { private set; get; } = 0;
    public static int AchievementsSetting { private set; get; } = 0;
    public static bool HandsomeFrogSetting { private set; get; } = false;
    public static bool KeylevelSetting { private set; get; } = false;
    public static bool BonkPermitSetting { private set; get; } = false;
    public static bool BugNetSetting { private set; get; } = false;
    public static bool SodaCansSetting { private set; get; } = false;
    public static bool ParasolsSetting { private set; get; } = false;
    public static bool SwimCourseSetting { private set; get; } = false;
    public static bool TextboxSetting { private set; get; } = false;
    public static bool AcRepairSetting { private set; get; } = false;
    public static bool AppleBasketSetting { private set; get; } = false;

    public bool NeedSlotData => slotData == null;

    public ArchipelagoData()
    {
        Uri = "archipelago.gg:";
        SlotName = "Player1";
        CheckedLocations = [];
    }

    public ArchipelagoData(string uri, string slotName, string password)
    {
        Uri = uri;
        SlotName = slotName;
        Password = password;
        CheckedLocations = [];
    }

    /// <summary>
    /// assigns the slot data and seed to our data handler. any necessary setup using this data can be done here.
    /// </summary>
    /// <param name="roomSlotData">slot data of your slot from the room</param>
    /// <param name="roomSeed">seed name of this session</param>
    public void SetupSession(Dictionary<string, object> roomSlotData, string roomSeed)
    {
        slotData = roomSlotData;
        seed = roomSeed;
        Plugin.Seed = seed;
        ArchipelagoMenu.Seed = seed;
        Plugin.SlotData = slotData;
    }

    /// <summary>
    /// returns the object as a json string to be written to a file which you can then load
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

    private void WhatIsRandomized()
    {
        if (slotData == null) return;
        if (slotData.TryGetValue("fishsanity", out var fishsanity))
            FishsanitySetting = int.Parse(fishsanity.ToString());
        if (slotData.TryGetValue("fishsanity", out var seedsanity))
            FishsanitySetting = int.Parse(seedsanity.ToString());
        if (slotData.TryGetValue("fishsanity", out var flowersanity))
            FishsanitySetting = int.Parse(flowersanity.ToString());
        if (slotData.TryGetValue("fishsanity", out var applesanity))
            FishsanitySetting = int.Parse(applesanity.ToString());
        if (slotData.TryGetValue("fishsanity", out var bugsanity))
            FishsanitySetting = int.Parse(bugsanity.ToString());
        if (slotData.TryGetValue("fishsanity", out var value5))
            FishsanitySetting = int.Parse(value5.ToString());
        if (slotData.TryGetValue("fishsanity", out var value6))
            FishsanitySetting = int.Parse(value6.ToString());
        if (slotData.TryGetValue("fishsanity", out var value7))
            FishsanitySetting = int.Parse(value7.ToString());
        if (slotData.TryGetValue("fishsanity", out var value8))
            FishsanitySetting = int.Parse(value8.ToString());
        if (slotData.TryGetValue("fishsanity", out var value9))
            FishsanitySetting = int.Parse(value9.ToString());
        if (slotData.TryGetValue("fishsanity", out var value10))
            FishsanitySetting = int.Parse(value10.ToString());
        if (slotData.TryGetValue("fishsanity", out var value11))
            FishsanitySetting = int.Parse(value11.ToString());
    }
}