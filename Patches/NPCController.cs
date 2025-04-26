using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KinematicCharacterController.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class NpcController : MonoBehaviour
{
    private string currentScene;
    public GameObject NpcGameObject;
    private string[] home, hairball, turbine, salmon, pool, bathhouse, tadpole, garden, niko;
    private static readonly HashSet<string> LoggedConversations = [];
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        LoggedConversations.Clear();
        home = ["CHATpepperMeet", "CHATmataMeet", "CHATFetchInfo", "CHATFetchQuest", 
            "CHATBlastFrog", "CHATKiosk",
            "CHATpepperParty", "CHATMoomyParty", "CHATBlessleyParty", "CHATVlogFrogParty",
            "CHATLouistParty", "CHATSerschelParty", "CHATDustanParty", "CHATTravisParty",
            "CHATTrixieParty", "CHATMitchParty", "CHATMaiParty", "CHATGunterParty", "CHATGabiParty",
            "CHATBlippyParty", "CHATGameKidParty", "CHATNinaParty", "CHATStijnParty", "CHATMelissaParty",
            "CHATFischerParty", "CHATFrogKingParty", "CHATPoppyParty", "CHATPaulParty", "CHATFrogtectiveParty",
            "CHATMataParty", "CHATPelicanParty", "CHATMinoesParty", "CHATMaggieParty", "CHATCarrotParty",
            "CHATDispatchedParty", "CHATMahjongFrog", "CHATCoastGaurd"
        ];
        
        hairball = ["CHATBritney", "CHATfrogFloaty", "CHATfrogSmallTalk", "CHATtrainFrog", 
            "CHATofficeCat", "CHAThbcNervousFrog", "CHAThbcToughFrog", 
            "CHAThbcImpatientFrog", "CHATVlogFrog", "CHATScarefrog", 
            "CHAThbcJiji", "CHAThbcMaggie", "CHAThbcMinoes", "CHATbreakFrog", 
            "CHAThbcSimon", "CHATbrooklynFrog", "CHAThbcCarrot", "CHATDustan", "CHATTravis",
            "CHATFlowerShovel", "CHATFlowerPlant", "CHATfrogHUD", "CHATpepperMain", "CHATGunter",
            "CHATArcadeBone", "CHATArcade", "CHATMoomy", "CHATBlessley", "CHATFischer",
            "CHATMelissa", "CHATStijn", "CHATNina", "CHATGamer", "CHATSerschel", "CHATLouist", "CHATGabi",
            "CHATMitch", "CHATMai", "CHATKiosk", "CHAThbcHandsomeFrog", "CHATCoastGaurd"
        ];
        
        turbine = ["CHATtrainFrog", "CHATtrainFrog2", "CHATVlogFrog", 
            "CHATForestGull", "CHATHQGull", "CHATtownGullNoline", "CHATbabyGull", 
            "CHATBritney", "CHATcultureGull", "CHATMelissaStijn", "CHATtipGull1", 
            "CHATtipGull2", "CHATtipGull3", "CHATlockBird", "CHATpepperMain",
            "CHATtownGull2", "CHATtownGull3", "CHATButtonBird", "CHATPostman", "CHATTravis",
            "CHATdragon", "CHATArcadeBone", "CHATGabi", "CHATBlessley", "CHATFischer",
            "CHATArcade", "CHATPelly", "CHATSerschel", "CHATLouist", "CHATDustan",
            "CHATMitch", "CHATMai", "CHATKiosk", "CHAThbcHandsomeFrog", "CHATCoastGaurd"
        ];
        
        salmon = ["CHATstijnsDad", "CHATstijnsMom", "CHATtrainFrog", "CHATtreeFrog", "CHATMysteriousDoe", "CHATTurbineStag", 
            "CHATSteamyStag", "CHATPoolDoe", "CHATLoudStag", "CHATVlogFrog", "CHATFearDeer", "CHATDiveDoe", "CHATcaveDoe", 
            "CHATpoppy", "CHATpaul", "CHATflippy", "CHATjippy", "CHATmippy", "CHATskippy", "CHATtippy", "CHATbunkid1", "CHATbunkid2", 
            "CHATbunkid3", "CHATbunkid4", "CHATbundad", "CHATbunmom", "CHATWoodisch", "CHATScarefrog", "CHATTreeMan", 
            "CHATMelissa", "CHATStijn", "CHATpepperMain", "CHATArcadeBone", "CHATArcade",
            "CHATSerschel", "CHATLouist", "CHATGabi", "CHATBlessley", "CHATFischer", "CHATDustan", "CHATTravis",
            "CHATMitch", "CHATMai", "CHATKiosk", "CHAThbcHandsomeFrog", "CHATCoastGaurd"
        ];
        
        pool = ["CHATculley", "CHAThatkid", "CHATfrogFloaty", "CHATFizzy", "CHATtownGull2", "CHATtownGull3",
            "CHATbabyGull", "CHATbabyGull2", "CHATBritney", "CHATbunkid1", "CHATbunkid2", "CHATbunkid4", 
            "CHATbundad", "CHATbunmom", "CHATdirk", "CHATtrainfrog", "CHATpepperMain", 
            "CHATfrogFlowerHint1", "CHATfrogFlowerHint2", "CHATVlogFrog", "CHATArcadeBone", "CHATArcade", 
            "CHATpoppy", "CHATpaul", "CHATflippy", "CHATjippy", "CHATmippy", "CHATskippy", "CHATtippy", 
            "CHATDetective", "CHATGabi", "CHATBlessley", "CHATFischer", "CHATTravis",
            "CHATMitch", "CHATMai", "CHATKiosk", "CHAThandsomeFrog", "CHATCoastGaurd"
        ];
        
        bathhouse = ["CHATMelissa", "CHATStijn", "CHATmonkey1", "CHATmonkey2", "CHATmonkey3", "CHATsteamyStag", 
            "CHATsteamyDoe", "CHATsnowFrog", "CHATfrogBoil", "CHATbunkid1", "CHATbunkid2", "CHATpepperMain",
            "CHATbunkid3", "CHATbunkid4", "CHATbundad", "CHATbunmom", "CHATpenny", "CHATgashadokuro", 
            "CHATpaul", "CHATtippy", "CHATtrainFrog", "CHATtbhCarl", "CHATtbhJess", "CHATtbhMiki", "CHATtbhBiki", "CHATtbhWess", 
            "CHATmahjongFrog", "CHATVlogFrog", "CHATtbhMonty", "CHATArcadeBone", "CHATArcade", "CHATUnderhero",
            "CHATTravis", "CHATSerschel", "CHATLouist", "CHATGabi", 
            "CHATBlessley", "CHATFischer", "CHATDustan",
            "CHATMitch", "CHATMai", "CHATKiosk", "CHAThbcHandsomeFrog", "CHATCoastGaurd"
        ];
        
        tadpole = ["CHATmonthFrog", "CHATMelissaStijn", "CHATfrogSushi", "CHATricky", 
            "CHATsweatFrog", "CHATgameFrog", "CHATtaxFrog", "CHATRNDfrog", "CHATRNDFrog2", "CHATRNDFrog3", 
            "CHATslackFrog", "CHATfrogacc", "CHATRoboFrog", "CHATAlice", "CHATpepperMain", "CHATGabi",
            "CHATVRfrog", "CHATFrogbucks", "CHATFrogMagic", "CHATArcadeBone", "CHATArcade", "CHATVlogFrog", 
            "CHATassistant", "CHATtrainFrog", "CHATcoffeeFrog", "CHATclassicNiko", "CHATBorbie", 
            "CHATSerschel", "CHATLouist", "CHATKing",
            "CHATBlessley", "CHATFischer", "CHATMaster", "CHATTravis",
            "CHATMitch", "CHATMai", "CHATElevator", "CHATCoastGaurd"
        ];
        
        garden = [
            "CHATGary", "CHATGaryFrog1", "CHATGaryFrog2", "CHATGaryFrog3", "CHATGaryFrog4",
            "CHATGaryFrog5", "CHATGaryFrog6", "CHATGaryFrog7", "CHATGaryFrog8", "CHATGaryFrog9",
            "CHATGaryFrog10", "CHATGaryFrog11", "CHATGunter", "CHATMitch", "CHATMai", "CHATCoastGaurd"
        ];
        
        niko = [
            "inspectTPIPond", "nikoFish", "nikoStatue", "nikoTPINightmare",
            "nikoBHMaintain", "nikoBHNinja", "nikoBHEcho", "nikoBHGold", "nikoBHHaiku", "nikoBHMainBuilding", "nikoBHButton",
            "nikoPPVape","nikoBall" , "nikoPPBath", "nikoPP2D", "nikoPink",
            "nikoSCRocks", "nikoMoneySmell", "nikoSCFall",
            "nikoTTHaring","nikoSCF", "nikoTTTown", "nikoTTCollider", "nikoSpacefrogs", "nikoTTGate",
            "nikoHBCStatue", "nikoHBCSnack", "nikoHBCSticky", "nikoHBCHarbor", "nikoHBCBench"
        ];
    }

    private void Update()
    {
        switch (currentScene)
        {
            case "Home":
                if (scrTextbox.instance.isOn)
                {
                    var conversation = scrTextbox.instance.conversation;
                    if (conversation is "FetchInfo" or "FetchInfoPost")
                        conversation = "FetchInfo";
                    if (conversation is "FetchPost" or "FetchQuest" or "FetchReward")
                        conversation = "FetchQuest";
                    if (conversation is "kioskNomoney" or "kioskBuy" or "kioskBought")
                        conversation = "Kiosk";
                    
                    if (!niko.Contains(conversation))
                        conversation = "CHAT"+conversation;

                    if ((home.Contains(conversation) || niko.Contains(conversation)) 
                        && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
                    {
                        scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
                    }
                    else if (!home.Contains(conversation) && !LoggedConversations.Contains(conversation))
                    {
                        Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
                        LoggedConversations.Add(conversation);
                    }
                }
                break;
            case "Hairball City":
                //NpcGameObject.SetActive(ArchipelagoClient.HcNPCs);
                if (scrTextbox.instance.isOn)
                {
                    var conversation = scrTextbox.instance.conversation;
                    if (conversation is "ArcadeBoneQuest" or "ArcadeBonePost")
                        conversation = "ArcadeBone";
                    if (conversation is "ArcadeQuest" or "ArcadePost")
                        conversation = "Arcade";
                    if (conversation is "SerschelSearch" or "SerschelHappy")
                        conversation = "Serschel";
                    if (conversation is "LouistLost" or "LouistReward" or "LouistPost")
                        conversation = "Louist";
                    if (conversation is "MoomyQuest" or "MoomyReward" or "MoomyPost")
                        conversation = "Moomy";
                    if (conversation is "graffitiQuestMelissa" or "graffitiRewardMelissa")
                        conversation = "Melissa";
                    if (conversation is "graffitiQuestStijn" or "graffitiRewardStijn")
                        conversation = "Stijn";
                    if (conversation is "graffitiQuestNina" or "graffitiPostNina" 
                        or "graffitiTooFewNina" or "graffitiRewardNina")
                        conversation = "Nina";
                    if (conversation is "GamerQuest" or "GamerReward" or "GamerPost")
                        conversation = "Gamer";
                    if (conversation is "FlowerQuest" or "FlowerPost" or "FlowerReward")
                        conversation = "Gabi";
                    if (conversation is "BugQuest" or "BugPost" or "BugReward")
                        conversation = "Blessley";
                    if (conversation is "FischerAll" or "fish0" 
                        or "fish1" or "fish2" or "fish3" or "fish4"
                        or "FischerIdle" or "FischerNonFish" or "FischerOldFish")
                        conversation = "Fischer";
                    if (conversation is "DustanPost" or "DustanReward" or "DustanFirstTime")
                        conversation = "Dustan";
                    if (conversation is "hbcTravis" or "hbcTravisWin" or "hbcTravisWon")
                        conversation = "Travis";
                    if (conversation is "hbcGunter" or "hbcGunter1" or "hbcGunter2"
                        or "hbcGunter3" or "hbcGunterEnd" or "hbcGunterDown")
                        conversation = "Gunter";
                    if (conversation is "kioskNomoney" or "kioskBuy" or "kioskBought")
                        conversation = "Kiosk";
                    if (conversation is "CassetteCoinCantBuy" 
                        or "CassetteCoinNotBought" 
                        or "CassetteCoinBought")
                        conversation = "Mitch";
                    if (conversation is "CassetteCoinCantBuy2" 
                        or "CassetteCoinNotBought2" 
                        or "CassetteCoinBought2")
                        conversation = "Mai";
                    
                    if (!niko.Contains(conversation))
                        conversation = "CHAT"+conversation;
                    
                    if ((hairball.Contains(conversation)  || niko.Contains(conversation))
                        && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
                    {
                        scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
                    }
                    else if (!hairball.Contains(conversation) && niko.Contains(conversation) && !LoggedConversations.Contains(conversation))
                    {
                        Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
                        LoggedConversations.Add(conversation);
                    }
                }
                break;
            case "Trash Kingdom":
                //NpcGameObject.SetActive(ArchipelagoClient.TtNPCs);
                if (scrTextbox.instance.isOn)
                {
                    var conversation = scrTextbox.instance.conversation;
                    if (conversation is "dragonPost")
                        conversation = "dragon";
                    if (conversation is "pellyPost" or "pellyQuest" or "pellyReward")
                        conversation = "Pelly";
                    if (conversation is "ArcadeBoneQuest" or "ArcadeBonePost")
                        conversation = "ArcadeBone";
                    if (conversation is "ArcadeQuest" or "ArcadePost")
                        conversation = "Arcade";
                    if (conversation is "SerschelSearch" or "SerschelHappy")
                        conversation = "Serschel";
                    if (conversation is "LouistLost" or "LouistReward" or "LouistPost")
                        conversation = "Louist";
                    if (conversation is "FlowerQuest" or "FlowerPost" or "FlowerReward")
                        conversation = "Gabi";
                    if (conversation is "BugQuest" or "BugPost" or "BugReward")
                        conversation = "Blessley";
                    if (conversation is "FischerAll" or "fish0" 
                        or "fish1" or "fish2" or "fish3" or "fish4"
                        or "FischerIdle" or "FischerNonFish" or "FischerOldFish")
                        conversation = "Fischer";
                    if (conversation is "DustanPost" or "DustanReward" or "DustanFirstTime")
                        conversation = "Dustan";
                    if (conversation is "hbcTravis" or "hbcTravisWin" or "hbcTravisWon")
                        conversation = "Travis";
                    if (conversation is "kioskNomoney" or "kioskBuy" or "kioskBought")
                        conversation = "Kiosk";
                    if (conversation is "CassetteCoinCantBuy" 
                        or "CassetteCoinNotBought" 
                        or "CassetteCoinBought")
                        conversation = "Mitch";
                    if (conversation is "CassetteCoinCantBuy2" 
                        or "CassetteCoinNotBought2" 
                        or "CassetteCoinBought2")
                        conversation = "Mai";
                    
                    if (!niko.Contains(conversation))
                        conversation = "CHAT"+conversation;

                    if ((turbine.Contains(conversation) || niko.Contains(conversation))
                        && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
                    {
                        scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
                    }
                    else if (!turbine.Contains(conversation) && !niko.Contains(conversation) && !LoggedConversations.Contains(conversation))
                    {
                        Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
                        LoggedConversations.Add(conversation);
                    }
                }
                break;
            case "Salmon Creek Forest":
                //NpcGameObject.SetActive(ArchipelagoClient.SfcNPCs);
                if (scrTextbox.instance.isOn)
                {
                    var conversation = scrTextbox.instance.conversation;
                    if (conversation is "melissaCarry" or "melissaPre")
                        conversation = "Melissa";
                    if (conversation is "stijnQuest" or "stijnPost")
                        conversation = "Stijn";
                    if (conversation is "tree1" or "tree2" or "tree3" 
                        or "tree4" or "tree5" or "treeEnd" or "treePost")
                        conversation = "TreeMan";
                    if (conversation is "ArcadeBoneQuest" or "ArcadeBonePost")
                        conversation = "ArcadeBone";
                    if (conversation is "ArcadeQuest" or "ArcadePost")
                        conversation = "Arcade";
                    if (conversation is "SerschelSearch" or "SerschelHappy")
                        conversation = "Serschel";
                    if (conversation is "LouistLost" or "LouistReward" or "LouistPost")
                        conversation = "Louist";
                    if (conversation is "GamerQuest" or "GamerReward" or "GamerPost")
                        conversation = "Gamer";
                    if (conversation is "FlowerQuest" or "FlowerPost" or "FlowerReward")
                        conversation = "Gabi";
                    if (conversation is "BugQuest" or "BugPost" or "BugReward")
                        conversation = "Blessley";
                    if (conversation is "FischerAll" or "fish0" 
                        or "fish1" or "fish2" or "fish3" or "fish4"
                        or "FischerIdle" or "FischerNonFish" or "FischerOldFish")
                        conversation = "Fischer";
                    if (conversation is "DustanPost" or "DustanReward" or "DustanFirstTime")
                        conversation = "Dustan";
                    if (conversation is "hbcTravis" or "hbcTravisWin" or "hbcTravisWon")
                        conversation = "Travis";
                    if (conversation is "kioskNomoney" or "kioskBuy" or "kioskBought")
                        conversation = "Kiosk";
                    if (conversation is "CassetteCoinCantBuy" 
                        or "CassetteCoinNotBought" 
                        or "CassetteCoinBought")
                        conversation = "Mitch";
                    if (conversation is "CassetteCoinCantBuy2" 
                        or "CassetteCoinNotBought2" 
                        or "CassetteCoinBought2")
                        conversation = "Mai";
                    
                    if (!niko.Contains(conversation))
                        conversation = "CHAT"+conversation;

                    if ((salmon.Contains(conversation) || niko.Contains(conversation))
                        && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
                    {
                        scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
                    }
                    else if (!salmon.Contains(conversation) && !niko.Contains(conversation) && !LoggedConversations.Contains(conversation))
                    {
                        Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
                        LoggedConversations.Add(conversation);
                    }
                }
                break;
            case "Public Pool":
                //NpcGameObject.SetActive(ArchipelagoClient.PpNPCs);
                if (scrTextbox.instance.isOn)
                {
                    var conversation = scrTextbox.instance.conversation;
                    if (conversation is "culleyPost")
                        conversation = "culley";
                    if (conversation is "ArcadeBoneQuest" or "ArcadeBonePost")
                        conversation = "ArcadeBone";
                    if (conversation is "ArcadeQuest" or "ArcadePost")
                        conversation = "Arcade";
                    if (conversation is "detectiveQuest" or "detectivePost" or "detectiveReward")
                        conversation = "Detective";
                    if (conversation is "FlowerQuest" or "FlowerPost" or "FlowerReward")
                        conversation = "Gabi";
                    if (conversation is "BugQuest" or "BugPost" or "BugReward")
                        conversation = "Blessley";
                    if (conversation is "FischerAll" or "fish0" 
                        or "fish1" or "fish2" or "fish3" or "fish4"
                        or "FischerIdle" or "FischerNonFish" or "FischerOldFish")
                        conversation = "Fischer";
                    if (conversation is "hbcTravis" or "hbcTravisWin" or "hbcTravisWon")
                        conversation = "Travis";
                    if (conversation is "kioskNomoney" or "kioskBuy" or "kioskBought")
                        conversation = "Kiosk";
                    if (conversation is "CassetteCoinCantBuy" 
                        or "CassetteCoinNotBought" 
                        or "CassetteCoinBought")
                        conversation = "Mitch";
                    if (conversation is "CassetteCoinCantBuy2" 
                        or "CassetteCoinNotBought2" 
                        or "CassetteCoinBought2")
                        conversation = "Mai";
                    
                    if (!niko.Contains(conversation))
                        conversation = "CHAT"+conversation;

                    if ((pool.Contains(conversation) || niko.Contains(conversation))
                        && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
                    {
                        scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
                    }
                    else if (!pool.Contains(conversation) && !niko.Contains(conversation) && !LoggedConversations.Contains(conversation))
                    {
                        Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
                        LoggedConversations.Add(conversation);
                    }
                }
                break;
            case "The Bathhouse":
                //NpcGameObject.SetActive(ArchipelagoClient.BathNPCs);
                if (scrTextbox.instance.isOn)
                {
                    var conversation = scrTextbox.instance.conversation;
                    if (conversation is "graffitiQuestMelissa" or "graffitiRewardMelissa")
                        conversation = "Melissa";
                    if (conversation is "graffitiQuestStijn" or "graffitiRewardStijn")
                        conversation = "Stijn";
                    if (conversation is "graffitiQuestNina" or "graffitiPostNina" 
                        or "graffitiTooFewNina" or "graffitiRewardNina")
                        conversation = "Nina";
                    if (conversation is "snowFrogPost")
                        conversation = "snowFrog";
                    if (conversation is "pennyQuest" or "pennyPost" or "pennyPre")
                        conversation = "penny";
                    if (conversation is "paulQuest" or "paulPost" or "paulPre" 
                        or "paulPanic" or "paulReward")
                        conversation = "paul";
                    if (conversation is "tippyQuest" or "tippyPost")
                        conversation = "tippy";
                    if (conversation is "poppyQuest" or "poppyPost")
                        conversation = "poppy";
                    if (conversation is "gashadokuroPost" or "gashadokuroQuest")
                        conversation = "gashadokuro";
                    if (conversation is "ArcadeBoneQuest" or "ArcadeBonePost")
                        conversation = "ArcadeBone";
                    if (conversation is "ArcadeQuest" or "ArcadePost")
                        conversation = "Arcade";
                    if (conversation is "GamerQuest" or "GamerReward" or "GamerPost")
                        conversation = "Gamer";
                    if (conversation is "SerschelSearch" or "SerschelHappy")
                        conversation = "Serschel";
                    if (conversation is "LouistLost" or "LouistReward" or "LouistPost")
                        conversation = "Louist";
                    if (conversation is "FlowerQuest" or "FlowerPost" or "FlowerReward")
                        conversation = "Gabi";
                    if (conversation is "BugQuest" or "BugPost" or "BugReward")
                        conversation = "Blessley";
                    if (conversation is "FischerAll" or "fish0" 
                        or "fish1" or "fish2" or "fish3" or "fish4"
                        or "FischerIdle" or "FischerNonFish" or "FischerOldFish")
                        conversation = "Fischer";
                    if (conversation is "DustanPost" or "DustanReward" or "DustanFirstTime")
                        conversation = "Dustan";
                    if (conversation is "tbhTravis" or "tbhTravisWin" or "tbhTravisWon")
                        conversation = "Travis";
                    if (conversation is "kioskNomoney" or "kioskBuy" or "kioskBought")
                        conversation = "Kiosk";
                    if (conversation is "CassetteCoinCantBuy" 
                        or "CassetteCoinNotBought" 
                        or "CassetteCoinBought")
                        conversation = "Mitch";
                    if (conversation is "CassetteCoinCantBuy2" 
                        or "CassetteCoinNotBought2" 
                        or "CassetteCoinBought2")
                        conversation = "Mai";
                    
                    if (!niko.Contains(conversation))
                        conversation = "CHAT"+conversation;

                    if ((bathhouse.Contains(conversation) || niko.Contains(conversation))
                        && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
                    {
                        scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
                    }
                    else if (!bathhouse.Contains(conversation) && !niko.Contains(conversation) && !LoggedConversations.Contains(conversation))
                    {
                        Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
                        LoggedConversations.Add(conversation);
                    }
                }
                break;
            case "Tadpole inc":
                //NpcGameObject.SetActive(ArchipelagoClient.HqNPCs);
                if (scrTextbox.instance.isOn)
                {
                    var conversation = scrTextbox.instance.conversation;
                    if (conversation is "monthFrogPre" or "monthFrogPost")
                        conversation = "monthFrog";
                    if (conversation is "ArcadeBoneQuest" or "ArcadeBonePost")
                        conversation = "ArcadeBone";
                    if (conversation is "ArcadeQuest" or "ArcadePost")
                        conversation = "Arcade";
                    if (conversation is "king1" or "king2" or "kingPost")
                        conversation = "King";
                    if (conversation is "SerschelSearch" or "SerschelHappy")
                        conversation = "Serschel";
                    if (conversation is "LouistLost" or "LouistReward" or "LouistPost")
                        conversation = "Louist";
                    if (conversation is "FlowerQuest" or "FlowerPost" or "FlowerReward")
                        conversation = "Gabi";
                    if (conversation is "BugQuest" or "BugPost" or "BugReward")
                        conversation = "Blessley";
                    if (conversation is "FischerAll" or "fish0" 
                        or "fish1" or "fish2" or "fish3" or "fish4"
                        or "FischerIdle" or "FischerNonFish" or "FischerOldFish")
                        conversation = "Fischer";
                    if (conversation is "hbcTravis" or "hbcTravisWin" or "hbcTravisWon")
                        conversation = "Travis";
                    if (conversation is "masterNotEnough" or "masterBuy" or "masterBought")
                        conversation = "Master";
                    if (conversation is "elevatorNoMoney" or "elevatorBuy" or "elevatorBought")
                        conversation = "Elevator";
                    if (conversation is "CassetteCoinCantBuy" 
                        or "CassetteCoinNotBought" 
                        or "CassetteCoinBought")
                        conversation = "Mitch";
                    if (conversation is "CassetteCoinCantBuy2" 
                        or "CassetteCoinNotBought2" 
                        or "CassetteCoinBought2")
                        conversation = "Mai";
                    if (conversation is "VlogFrogMonth")
                        conversation = "VlogFrog";
                    
                    if (!niko.Contains(conversation))
                        conversation = "CHAT"+conversation;

                    if ((tadpole.Contains(conversation) || niko.Contains(conversation))
                        && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
                    {
                        scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
                    }
                    else if (!tadpole.Contains(conversation) && !niko.Contains(conversation) && !LoggedConversations.Contains(conversation))
                    {
                        Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
                        LoggedConversations.Add(conversation);
                    }
                }
                break;
            case "GarysGarden":
                if (scrTextbox.instance.isOn)
                {
                    var conversation = scrTextbox.instance.conversation;
                    if (conversation is "CassetteCoinCantBuy" 
                        or "CassetteCoinNotBought" 
                        or "CassetteCoinBought")
                        conversation = "Mitch";
                    if (conversation is "CassetteCoinCantBuy2" 
                        or "CassetteCoinNotBought2" 
                        or "CassetteCoinBought2")
                        conversation = "Mai";
                    if (conversation is "Gary0" or "Gary1" 
                        or "Gary2" or "Gary3" or "Gary4" 
                        or "Gary5" or "Gary6" or "Gary7" 
                        or "Gary8" or "Gary9")
                        conversation = "Gary";
                    if (conversation is "Gary10")
                        conversation = "Gunter";
                    
                    conversation = "CHAT"+conversation;

                    if (garden.Contains(conversation)
                        && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
                    {
                        scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
                    }
                    else if (!garden.Contains(conversation) && !LoggedConversations.Contains(conversation))
                    {
                        Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
                        LoggedConversations.Add(conversation);
                    }
                }
                break;
        }
    }
}