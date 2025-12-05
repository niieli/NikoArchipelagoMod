using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NikoArchipelago.Archipelago;

public class ArchipelagoOptions
{
    public enum GoalCompletionMode
    {
        Hired = 0,
        Employee = 1,
        Custom = 2,
        Garden = 3,
        Help = 4
    }
    public enum GardenAccessMode
    {
        Tadpole = 0,
        TadpoleAndGarden = 1,
        Garden = 2
    }
    public enum CassetteMode
    {
        LevelBased = 0,
        Progressive = 1,
        Scattered = 2
    }
    public enum InsanityLevel
    {
        Vanilla = 0,
        Location = 1,
        Insanity = 2
    }
    public enum ChatsanityLevel
    {
        Vanilla = 0,
        LevelBased = 1,
        Global = 2
    }
    public enum AchievementsMode
    {
        All = 0,
        ExceptSnailFashion = 1,
        Disabled = 2
    }
    public enum TextboxLevel
    {
        Vanilla = 0,
        Global = 1,
        Level = 2
    }
    
    public GoalCompletionMode GoalCompletion { get; set; }
    public GardenAccessMode GardenAccess { get; set; }
    public CassetteMode Cassette { get; set; }
    public InsanityLevel Fishsanity { get; set; }
    public InsanityLevel Seedsanity { get; set; }
    public InsanityLevel Flowersanity { get; set; }
    public InsanityLevel Bonesanity { get; set; }
    public ChatsanityLevel Chatsanity { get; set; }
    public AchievementsMode Achievements { get; set; }
    public TextboxLevel Textbox { get; set; }

    public bool GarysGarden { get; set; }
    public bool HandsomeFrog { get; set; }
    public bool Keylevels { get; set; }
    public bool Snailshop { get; set; }
    public bool Applesanity { get; set; }
    public bool Bugsanity { get; set; }
    public bool BonkPermit { get; set; }
    public bool BugNet { get; set; }
    public bool SodaCans { get; set; }
    public bool Parasols { get; set; }
    public bool SwimCourse { get; set; }
    public bool AcRepair { get; set; }
    public bool AppleBasket { get; set; }
    public bool Thoughtsanity { get; set; }
    public int DeathLinkAmnesty { get; set; }
    public int CustomGoalCoinAmount { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder();
        var props = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in props)
        {
            var value = prop.GetValue(this);
            builder.AppendLine($"{prop.Name}: {value}");
        }
        return builder.ToString();
    }
}

public static class ArchipelagoOptionsParser
{
    public static ArchipelagoOptions FromSlotData(Dictionary<string, object> slotData)
    {
        var options = new ArchipelagoOptions();

        int GetInt(string key) =>
            slotData.TryGetValue(key, out var val) && int.TryParse(val.ToString(), out var r) ? r : 0;

        bool GetBool(string key) =>
            slotData.TryGetValue(key, out var val) && int.TryParse(val.ToString(), out var r) && r > 0;

        options.GoalCompletion = (ArchipelagoOptions.GoalCompletionMode)GetInt("goal_completion");
        options.GardenAccess = (ArchipelagoOptions.GardenAccessMode)GetInt("garden_access");
        options.Cassette = (ArchipelagoOptions.CassetteMode)GetInt("cassette_logic");
        options.Fishsanity = (ArchipelagoOptions.InsanityLevel)GetInt("fishsanity");
        options.Seedsanity = (ArchipelagoOptions.InsanityLevel)GetInt("seedsanity");
        options.Flowersanity = (ArchipelagoOptions.InsanityLevel)GetInt("flowersanity");
        options.Bonesanity = (ArchipelagoOptions.InsanityLevel)GetInt("bonesanity");
        options.Chatsanity = (ArchipelagoOptions.ChatsanityLevel)GetInt("chatsanity");
        options.Achievements = (ArchipelagoOptions.AchievementsMode)GetInt("achievements");
        options.Textbox = (ArchipelagoOptions.TextboxLevel)GetInt("textbox");

        options.GarysGarden = GetBool("shuffle_garden");
        options.HandsomeFrog = GetBool("handsome_frog");
        options.Keylevels = GetBool("key_level");
        options.Snailshop = GetBool("snailshop");
        options.Applesanity = GetBool("applessanity");
        options.Bugsanity = GetBool("bugsanity");
        options.BonkPermit = GetBool("bonk_permit");
        options.BugNet = GetBool("bug_net");
        options.SodaCans = GetBool("soda_cans");
        options.Parasols = GetBool("parasols");
        options.SwimCourse = GetBool("swimming");
        options.AcRepair = GetBool("ac_repair");
        options.AppleBasket = GetBool("applebasket");
        options.Thoughtsanity = GetBool("thoughtsanity");
        options.DeathLinkAmnesty = GetInt("death_link_amnesty");
        options.CustomGoalCoinAmount = GetInt("custom_goal_coin_amount");
        
        return options;
    }
}
