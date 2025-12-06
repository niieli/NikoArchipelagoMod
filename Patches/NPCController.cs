using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class NpcController : MonoBehaviour
{
    private string currentScene;
    public GameObject NpcGameObject;
    private string[] home, hairball, turbine, salmon, pool, bathhouse, tadpole, garden, niko, global;
    private static readonly HashSet<string> LoggedConversations = [];
    public static bool IsGlobal = false;
    public static bool Thoughtsanity = false;
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        LoggedConversations.Clear();
        home = ["CHATPepper", "CHATMata", "CHATFetchInfo", "CHATFetchQuest", 
            "CHATBlastFrog", "CHATKiosk", "CHATMoomy", "CHATBlessley", "CHATVlogFrog",
            "CHATLouist", "CHATSerschel", "CHATDustan", "CHATTravis",
            "CHATTrixie", "CHATMitch", "CHATMai", "CHATGunter", "CHATGabi",
            "CHATBlippy", "CHATGameKid", "CHATNina", "CHATStijn", "CHATMelissa",
            "CHATFischer", "CHATFrogKing", "CHATPoppy", "CHATPaul", "CHATFrogtective",
            "CHATPelican", "CHATMinoes", "CHATMaggie", "CHATCarrot", "CHATMahjongFrog", "CHATCoastGaurd", "CHATmonthFrog"
        ];
        
        hairball = ["CHATBritney", "CHATfrogFloaty", "CHATfrogSmallTalk", "CHATtrainFrog", 
            "CHATofficeCat", "CHAThbcNervousFrog", "CHAThbcToughFrog", 
            "CHAThbcImpatientFrog", "CHATVlogFrog", "CHATScarefrog", 
            "CHAThbcJiji", "CHAThbcMaggie", "CHAThbcMinoes", "CHATbreakFrog", 
            "CHAThbcSimon", "CHATbrooklynFrog", "CHAThbcCarrot", "CHATDustan", "CHATTravis",
            "CHATFlowerShovel", "CHATFlowerPlant", "CHATfrogHUD", "CHATpepperMain", "CHATGunter",
            "CHATArcadeBone", "CHATArcade", "CHATMoomy", "CHATBlessley", "CHATFischer",
            "CHATMelissa", "CHATStijn", "CHATNina", "CHATGamer", "CHATSerschel", "CHATLouist", "CHATGabi",
            "CHATMitch", "CHATMai", "CHATKiosk", "CHAThbcHandsomeFrog", "CHATCoastGaurd", "CHATBoneDog", "CHATkappa", "CHATdog", "CHATdog2"
        ];
        
        turbine = ["CHATtrainFrog", "CHATtrainFrog2", "CHATVlogFrog", 
            "CHATForestGull", "CHATHQGull", "CHATtownGullNoline", "CHATbabyGull", 
            "CHATBritney", "CHATcultureGull", "CHATMelissaStijn", "CHATtipGull1", 
            "CHATtipGull2", "CHATtipGull3", "CHATlockBird", "CHATpepperMain",
            "CHATtownGull2", "CHATtownGull3", "CHATButtonBird", "CHATPostman", "CHATTravis",
            "CHATdragon", "CHATArcadeBone", "CHATGabi", "CHATBlessley", "CHATFischer",
            "CHATArcade", "CHATPelly", "CHATSerschel", "CHATLouist", "CHATDustan",
            "CHATMitch", "CHATMai", "CHATKiosk", "CHAThbcHandsomeFrog", "CHATCoastGaurd", "CHATBoneDog", "CHATkappa", "CHATdog", "CHATdog2"
        ];
        
        salmon = ["CHATstijnsDad", "CHATstijnsMom", "CHATtrainFrog", "CHATtreeFrog", "CHATMysteriousDoe", "CHATTurbineStag", 
            "CHATSteamyStag", "CHATPoolDoe", "CHATLoudStag", "CHATVlogFrog", "CHATFearDeer", "CHATDiveDoe", "CHATcaveDoe", 
            "CHATpoppy", "CHATpaul", "CHATflippy", "CHATjippy", "CHATmippy", "CHATskippy", "CHATtippy", "CHATbunkid1", "CHATbunkid2", 
            "CHATbunkid3", "CHATbunkid4", "CHATbundad", "CHATbunmom", "CHATWoodisch", "CHATScarefrog", "CHATTreeMan", 
            "CHATMelissa", "CHATStijn", "CHATpepperMain", "CHATArcadeBone", "CHATArcade",
            "CHATSerschel", "CHATLouist", "CHATGabi", "CHATBlessley", "CHATFischer", "CHATDustan", "CHATTravis",
            "CHATMitch", "CHATMai", "CHATKiosk", "CHAThbcHandsomeFrog", "CHATCoastGaurd", "CHATNina", "CHATGamer", "CHATMoomy", "CHATBoneDog"
        ];
        
        pool = ["CHATculley", "CHAThatkid", "CHATfrogFloaty", "CHATFizzy", "CHATtownGull2", "CHATtownGull3",
            "CHATbabyGull", "CHATbabyGull2", "CHATBritney", "CHATbunkid1", "CHATbunkid2", "CHATbunkid4", 
            "CHATbundad", "CHATbunmom", "CHATdirk", "CHATtrainfrog", "CHATpepperMain", 
            "CHATfrogFlowerHint1", "CHATfrogFlowerHint2", "CHATVlogFrog", "CHATArcadeBone", "CHATArcade", 
            "CHATpoppy", "CHATpaul", "CHATflippy", "CHATjippy", "CHATmippy", "CHATskippy", "CHATtippy", 
            "CHATDetective", "CHATGabi", "CHATBlessley", "CHATFischer", "CHATTravis",
            "CHATMitch", "CHATMai", "CHATKiosk", "CHAThandsomeFrog", "CHATCoastGaurd", "CHATBoneDog"
        ];
        
        bathhouse = ["CHATMelissa", "CHATStijn", "CHATmonkey1", "CHATmonkey2", "CHATmonkey3", "CHATsteamyStag", 
            "CHATsteamyDoe", "CHATsnowFrog", "CHATfrogBoil", "CHATbunkid1", "CHATbunkid2", "CHATpepperMain",
            "CHATbunkid3", "CHATbunkid4", "CHATbundad", "CHATbunmom", "CHATpenny", "CHATgashadokuro", 
            "CHATpaul", "CHATtippy", "CHATtrainFrog", "CHATtbhCarl", "CHATtbhJess", "CHATtbhMiki", "CHATtbhBiki", "CHATtbhWess", 
            "CHATmahjongFrog", "CHATVlogFrog", "CHATtbhMonty", "CHATArcadeBone", "CHATArcade", "CHATUnderhero",
            "CHATTravis", "CHATSerschel", "CHATLouist", "CHATGabi", 
            "CHATBlessley", "CHATFischer", "CHATDustan", "CHATMelissaStijn",
            "CHATMitch", "CHATMai", "CHATKiosk", "CHAThbcHandsomeFrog", "CHATCoastGaurd", "CHATNina", "CHATGamer", "CHATMoomy", "CHATBoneDog"
        ];
        
        tadpole = ["CHATmonthFrog", "CHATMelissaStijn", "CHATfrogSushi", "CHATricky", 
            "CHATsweatFrog", "CHATgameFrog", "CHATtaxFrog", "CHATRNDfrog", "CHATRNDFrog2", "CHATRNDFrog3", 
            "CHATslackFrog", "CHATfrogacc", "CHATRoboFrog", "CHATAlice", "CHATpepperMain", "CHATGabi",
            "CHATVRfrog", "CHATFrogbucks", "CHATFrogMagic", "CHATArcadeBone", "CHATArcade", "CHATVlogFrog", 
            "CHATassistant", "CHATtrainFrog", "CHATcoffeeFrog", "CHATclassicNiko", "CHATBorbie", 
            "CHATSerschel", "CHATLouist", "CHATKing",
            "CHATBlessley", "CHATFischer", "CHATMaster", "CHATTravis",
            "CHATMitch", "CHATMai", "CHATElevator", "CHATCoastGaurd", "CHATBoneDog", "CHATdog"
        ];
        
        garden = [
            "CHATGary", "CHATGaryFrog1", "CHATGaryFrog2", "CHATGaryFrog3", "CHATGaryFrog4",
            "CHATGaryFrog5", "CHATGaryFrog6", "CHATGaryFrog7", "CHATGaryFrog8", "CHATGaryFrog9",
            "CHATGaryFrog10", "CHATGaryFrog11", "CHATGunter", "CHATMitch", "CHATMai", "CHATCoastGaurd"
        ];
        
        niko = [
            "inspectTPIPond", "nikoFish", "nikoStatue", "nikoTPINightmare",
            "nikoBHMaintain", "nikoBHNinja", "nikoBHEcho", "nikoBHGold", "nikoBHHaiku", "nikoBHMainBuilding", "nikoBHButton",
            "nikoPPVape","nikoBall" , "nikoPPBath", "nikoPP2D", "nikoPink", "nikoCrime",
            "nikoSCRocks", "nikoMoneySmell", "nikoSCFall",
            "nikoTTHaring","nikoSCF", "nikoTTTown", "nikoTTCollider", "nikoSpacefrogs", "nikoTTGate",
            "nikoHBCStatue", "nikoHBCSnack", "nikoHBCSticky", "nikoHBCHarbor", "nikoHBCBench"
        ];

        global =
        [
            "CHATPepper",                                           // Pepper
            "CHATMata",                                             // Mata
            "CHATFetchInfo",                                        // Low Frog
            "CHATFetchQuest",                                       // High Frog
            "CHATBlastFrog",                                        // Blast Frog
            "CHATKiosk",                                            // Dispatcher
            "CHATMoomy",                                            // Moomy
            "CHATBlessley",                                         // Blessley
            "CHATVlogFrog",                                         // Vlog Frog
            "CHATLouist",                                           // Louist
            "CHATSerschel",                                         // Serschel
            "CHATDustan",                                           // Dustan
            "CHATTravis",                                           // Travis
            "CHATTrixie",                                           // Trixie
            "CHATMitch",                                            // Mitch
            "CHATMai",                                              // Mai
            "CHATGunter",                                           // Gunter
            "CHATGabi",                                             // Little Gabi
            "CHATGamer",                                            // Game Kid
            "CHATNina",                                             // Nina
            "CHATStijn",                                            // Stijn
            "CHATMelissa",                                          // Melissa
            "CHATFischer",                                          // Fischer
            "CHATKing",                                             // Frog king
            "CHATPoppy",                                            // Poppy
            "CHATPaul",                                             // Paul
            "CHATFrogtective",                                      // Frogtective
            "CHATPelly",                                            // Pelly the Engineer
            "CHATMinoes",                                           // Minoes
            "CHATMaggie",                                           // Maggie
            "CHATCarrot",                                           // Carrot
            "CHATMahjongFrog",                                      // Mahjong Frog (Party, singular)
            "CHATCoastGaurd",                                       // Hasselhop
            "CHATBritney",                                          // Britney
            "CHATfrogFloaty",                                       // Salty Frog
            "CHATfrogSmallTalk",                                    // Small Talk Frog
            "CHATofficeCat",                                        // Bobby
            "CHAThbcNervousFrog",                                   // Nervous Frog
            "CHAThbcToughFrog",                                     // Tough Frog
            "CHAThbcImpatientFrog",                                 // Impatient Frog
            "CHAThbcJiji",                                          // Jiji
            "CHATbreakFrog",                                        // Frog of Destruction
            "CHAThbcSimon",                                         // Simon
            "CHATbrooklynFrog",                                     // Brooklyn Frog
            "CHATFlowerShovel",                                     // Shovelin' Frog
            "CHATFlowerPlant",                                      // Flowery Frog
            "CHATfrogHUD",                                          // HUD Frog
            "CHATArcadeBone",                                       // Blippy (Bone)
            "CHATArcade",                                           // Blippy
            "CHATHandsomeFrog",                                     // Handsome Frog
            "CHATTrainFrog",                                        // Train Frog
            "CHATForestGull",                                       // Gull Friend
            "CHATHQGull",                                           // Gull Friend
            "CHATtownGullNoline",                                   // Fry loving Gull
            "CHATbabyGull",                                         // Baby Gull (TT)
            "CHATcultureGull",                                      // Culture Gull
            "CHATtipGull1",                                         // Knowledgeable Gull
            "CHATtipGull2",                                         // Superstitious Gull
            "CHATtipGull3",                                         // Mythology Gull
            "CHATlockBird",                                         // Lock Gull
            "CHATtownGull1",                                        // Fry Gull
            "CHATtownGull2",                                        // Friendly Gull
            "CHATtownGull3",                                        // AC Gull
            "CHATButtonBird",                                       // Button Bird
            "CHATPostman",                                          // Noah
            "CHATdragon",                                           // Wind Dragon
            "CHATstijnsDad",                                        // Stijn's Dad
            "CHATstijnsMom",                                        // Stijn's Mom
            "CHATtreeFrog",                                         // Pine Frog
            "CHATMysteriousDoe",                                    // Mysterious Doe
            "CHATTurbineStag",                                      // Turbine Stag
            "CHATSteamyStag",                                       // Steamy Stag
            "CHATPoolDoe",                                          // Swimming Doe
            "CHATLoudStag",                                         // Loud Stag
            "CHATFearDeer",                                         // Fear Deer
            "CHATDiveDoe",                                          // Divin' Doe
            "CHATcaveDoe",                                          // Doe of Darkness
            "CHATflippy",                                           // Flippy
            "CHATjippy",                                            // Jippy
            "CHATmippy",                                            // Mippy
            "CHATskippy",                                           // Skippy
            "CHATtippy",                                            // Tippy
            "CHATbunkid1",                                          // Clint
            "CHATbunkid2",                                          // Coco
            "CHATbunkid3",                                          // Culley
            "CHATbunkid4",                                          // Clover
            "CHATbundad",                                           // David D. Carota
            "CHATbunmom",                                           // Marry D. Carota
            "CHATWoodisch",                                         // Woodisch
            "CHATScarefrog",                                        // Scare Frog
            "CHATTreeMan",                                          // Treeman
            "CHAThatkid",                                           // Hat Kid
            "CHATVacationFrog",                                     // Vacation Frog
            "CHATFizzy",                                            // Fizzy the Frog
            "CHATMomGull",                                          // Mom Gull (TT)
            "CHATMomGull2",                                         // Mom Gull (PP)
            "CHATbabyGull2",                                        // Baby Gull (PP)
            "CHATdirk",                                             // Dirk
            "CHATmonkey1",                                          // Moe
            "CHATmonkey2",                                          // Mickey
            "CHATmonkey3",                                          // Marshal
            "CHATsteamyStag",                                       // Big Sis
            "CHATsteamyDoe",                                        // Lil Sis
            "CHATSnowFrog",                                         // Snow Frog Frog
            "CHATfrogBoil",                                         // Steamy Frog
            "CHATPenny",                                            // Penny
            "CHATGashadokuro",                                      // Gashadokuro
            "CHATtbhCarl",                                          // Carl
            "CHATtbhJess",                                          // Jess
            "CHATtbhMiki",                                          // Miki
            "CHATtbhBiki",                                          // Biki
            "CHATtbhWess",                                          // Wess
            "CHATmahjongFrog",                                      // Mahjong Frogs (Bath)
            "CHATtbhMonty",                                         // Monty
            "CHATUnderhero",                                        // Elizabeth IV
            "CHATmonthFrog",                                        // (Ex) Employee of the month
            "CHATfrogSushi",                                        // Sushi Frog
            "CHATricky",                                            // Ricky
            "CHATsweatFrog",                                        // Code Frog
            "CHATgameFrog",                                         // Gamedev Frog
            "CHATtaxFrog",                                          // Tax Frog
            "CHATRNDFrog2",                                         // R&D Frog
            "CHATRNDFrog3",                                         // R&D Frog
            "CHATRNDfrog",                                          // R&D Frog
            "CHATslackFrog",                                        // Slack Frog
            "CHATfrogacc",                                          // Accountant Frog
            "CHATRoboFrog",                                         // Robo Fr0g
            "CHATAlice",                                            // Alice
            "CHATVRfrog",                                           // VR Frog
            "CHATFrogbucks",                                        // Frog (Frogbucks)
            "CHATFrogMagic",                                        // Frogucus the Green
            "CHATassistant",                                        // Assistant Frog
            "CHATcoffeeFrog",                                       // Accountant Frog
            "CHATclassicNiko",                                      // Niko a0.45
            "CHATBorbie",                                           // Borbie
            "CHATMaster",                                           // Master/Sensei
            "CHATElevator",                                         // Fix Frog
            "CHATGary",                                             // Gary
            "CHATGaryFrog1",                                        // Danger Frog
            "CHATGaryFrog2",                                        // Bird
            "CHATGaryFrog3",                                        // Dream Frog
            "CHATGaryFrog4",                                        // Snip Frog
            "CHATGaryFrog5",                                        // Fear Frog
            "CHATGaryFrog6",                                        // Dance Frog
            "CHATGaryFrog7",                                        // Tourist Frog
            "CHATGaryFrog8",                                        // Hungry Frog
            "CHATGaryFrog10",                                       // Flower Frog
            "CHATGaryFrog11",                                       // Conspiracy Frog
            "CHATTipFrog",                                          // Tip Frog
            "CHATBoneDog",                                          // Bone Dog
            "CHATkappa",                                            // Kappa
            "CHATdog",                                              // Dog
            "CHATdog2",                                             // Dog
        ];
    }

    private void GlobalChat()
    {
        if (!scrTextbox.instance.isOn) return;
        var conversation = scrTextbox.instance.conversation;
        if (conversation is "CassetteCoinCantBuy" or "CassetteCoinNotBought" or "CassetteCoinBought" or "MitchParty")
            conversation = "Mitch";
        if (conversation is "CassetteCoinCantBuy2" or "CassetteCoinNotBought2" or "CassetteCoinBought2" or "MaiParty")
            conversation = "Mai";
        if (conversation is "Gary0" or "Gary1" or "Gary2" or "Gary3" or "Gary4" or "Gary5" or "Gary6" or "Gary7"
            or "Gary8" or "Gary9")
            conversation = "Gary";
        if (conversation is "Gary10" or "hbcGunter" or "hbcGunter1" or "hbcGunter2" or "hbcGunter3" or "hbcGunterEnd"
            or "hbcGunterDown" or "GunterParty") conversation = "Gunter";
        if (conversation is "monthFrogPre" or "monthFrogPost") conversation = "monthFrog"; //Employee of the month
        if (conversation is "ArcadeBoneQuest" or "ArcadeBonePost" or "BlippyParty") conversation = "ArcadeBone";
        if (conversation is "ArcadeQuest" or "ArcadePost" or "BlippyParty") conversation = "Arcade";
        if (conversation is "king1" or "king2" or "kingPost" or "FrogKingParty") conversation = "King";
        if (conversation is "SerschelSearch" or "SerschelHappy" or "SerschelParty") conversation = "Serschel";
        if (conversation is "LouistLost" or "LouistReward" or "LouistPost" or "LouistParty") conversation = "Louist";
        if (conversation is "FlowerQuest" or "FlowerPost" or "FlowerReward" or "GabiParty") conversation = "Gabi";
        if (conversation is "BugQuest" or "BugPost" or "BugReward" or "BlessleyParty") conversation = "Blessley";
        if (conversation is "FischerAll" or "FishsanityNotEnough" or "FishsanityFishing"
            or "FishsanityFinal" or "FishsanityObtained" or "FischerLocationIdle" or "FishsanityNoSwimming" 
            or "fish0" or "fish1" or "fish2" or "fish3" or "fish4" or "FischerIdle"
            or "FischerNonFish" or "FischerOldFish" or "FischerParty")
            conversation = "Fischer";
        if (conversation is "masterNotEnough" or "masterBuy" or "masterBought") conversation = "Master"; //Name:Master/Sensei
        if (conversation is "elevatorNoMoney" or "elevatorBuy" or "elevatorBought") conversation = "Elevator"; //Name:Fix Frog
        if (conversation is "VlogFrogMonth" or "VlogFrogParty") conversation = "VlogFrog";
        if (conversation is "graffitiQuestMelissa" or "graffitiRewardMelissa" or "melissaCarry" or "melissaPre"
            or "MelissaParty" or "MelissaStijn") conversation = "Melissa";
        if (conversation is "graffitiQuestStijn" or "graffitiRewardStijn" or "stijnQuest" or "stijnPost" 
            or "StijnParty" or "MelissaStijn") conversation = "Stijn";
        if (conversation is "graffitiQuestNina" or "graffitiPostNina" or "graffitiTooFewNina"
            or "graffitiRewardNina" or "NinaParty" or "NinaParty2")
            conversation = "Nina";
        if (conversation is "snowFrogPost" or "snowFrog") conversation = "SnowFrog";  //Name:Snow Frog Frog
        if (conversation is "pennyQuest" or "pennyPost" or "pennyPre") conversation = "Penny";
        if (conversation is "paulQuest" or "paulPost" or "paulPre" or "paulPanic" or "paulReward" or "PaulParty")
            conversation = "Paul";
        if (conversation is "poppyQuest" or "poppyPost" or "PoppyParty") conversation = "Poppy";
        if (conversation is "tippyQuest" or "tippyPost") conversation = "tippy";
        if (conversation is "gashadokuroPost" or "gashadokuroQuest") conversation = "gashadokuro";
        if (conversation is "GamerQuest" or "GamerReward" or "GamerPost" or "GameKidParty") conversation = "Gamer"; //Name:Game Kid
        if (conversation is "DustanPost" or "DustanReward" or "DustanFirstTime" or "DustanParty"
            or "tbhDustaFound" or "tbhDustanPostFound") conversation = "Dustan";
        if ((conversation is "hbcTravis" or "hbcTravisWin" or "hbcTravisWon" 
            && currentScene is "Public Pool" or "Salmon Creek Forest" or "Trash Kingdom") 
            || conversation=="TrixieParty") conversation = "Trixie";
        if (conversation is "tbhTravis" or "tbhTravisWin" or "tbhTravisWon" or 
            "hbcTravis" or "hbcTravisWin" or "hbcTravisWon" or "TravisParty") conversation = "Travis";
        if (conversation is "kioskNomoney" or "kioskBuy" or "kioskBought" or "DispatchedParty") conversation = "Kiosk";  //Name:Dispatcher
        if (conversation is "MoomyQuest" or "MoomyReward" or "MoomyPost" or "MoomyParty") conversation = "Moomy";
        if (conversation is "culleyPost" or "culley") conversation = "bunkid3";
        if (conversation is "detectiveQuest" or "detectivePost" or "detectiveReward" or "FrogtectiveParty") conversation = "Frogtective";  //Name:Frogtective
        if (conversation is "tree1" or "tree2" or "tree3" or "tree4" or "tree5" or "treeEnd" or "treePost")
            conversation = "TreeMan";
        if (conversation is "dragonPost") conversation = "dragon";
        if (conversation is "pellyPost" or "pellyQuest" or "pellyReward" or "PelicanParty") conversation = "Pelly";
        if (conversation is "hbcGunter" or "hbcGunter1" or "hbcGunter2" or "hbcGunter3" or "hbcGunterEnd"
            or "hbcGunterDown")
            conversation = "Gunter";
        if (conversation is "FetchInfo" or "FetchInfoPost") conversation = "FetchInfo";
        if (conversation is "FetchPost" or "FetchQuest" or "FetchReward") conversation = "FetchQuest";
        if (conversation is "pepperMeet" or "pepperMain" or "pepperParty" or "pepperGotAllCoins"
            or "pepperRandom1" or "pepperRandom2" or "pepperRandom3" or "pepperRandom4" or "pepperRandom5" 
            or "pepperRandom6" or "pepperRandom7" or "pepperRandom8" or "pepperRandom9" or "interview") conversation = "Pepper";
        if (conversation is "mataMeet" or "MataParty") conversation = "Mata";
        if (conversation is "hbcHandsomeFrog" or "handsomeFrog" or "GaryFrog9") conversation = "HandsomeFrog";
        if (conversation is "hbcMaggie" or "MaggieParty") conversation = "Maggie";
        if (conversation is "hbcMinoes" or "MinoesParty") conversation = "Minoes";
        if (conversation is "hbcCarrot" or "CarrotParty") conversation = "Carrot";
        if (conversation is "trainFrog" && currentScene == "Trash Kingdom") conversation = "TipFrog";
        if (conversation is "trainFrog2" or "trainFrog" or "trainfrog") conversation = "TrainFrog";
        if (conversation is "frogFloaty" && currentScene == "Public Pool") conversation = "VacationFrog";
        if (conversation is "monthFrogParty") conversation = "InfoFrog";
        if (conversation is "townGull2" && currentScene == "Public Pool") conversation = "MomGull";
        if (conversation is "townGull3" && currentScene == "Public Pool") conversation = "MomGull2";
        if (conversation is "mahjongFrog") conversation = "MahjongFrog";
        if (conversation is "dogeBoneQuest" or "dogeBonePost") conversation = "BoneDog";
        if (conversation is "dog1") conversation = "dog";
        if (!conversation.StartsWith("trapConv") && !niko.Contains(conversation))
            conversation = "CHAT" + conversation;
        //if (conversation.EndsWith("Party"))
        //    conversation = conversation.Replace("Party", "");
        if (global.Contains(conversation) && !scrGameSaveManager.instance.gameData.worldsData[0].miscFlags.Contains(conversation))
        {
            //Plugin.BepinLogger.LogInfo($"Found Coversation: {conversation}");
            scrGameSaveManager.instance.gameData.worldsData[0].miscFlags.Add(conversation);
            if (conversation == "CHATMelissaStijn")
            {
                scrGameSaveManager.instance.gameData.worldsData[0].miscFlags.Add("CHATStijn");
                scrGameSaveManager.instance.gameData.worldsData[0].miscFlags.Add("CHATMelissa");
            }
        }
        else if (!global.Contains(conversation) && !LoggedConversations.Contains(conversation)
                 && !conversation.StartsWith("trapConv") && !niko.Contains(conversation))
        {
            Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
            LoggedConversations.Add(conversation);
        }
    }

    private void LevelChat()
    {
        var isLevelTextbox = ArchipelagoData.Options.Textbox == ArchipelagoOptions.TextboxLevel.Level;
        switch (currentScene)
        {
            case "Home":
                if (isLevelTextbox && !ArchipelagoClient.HomeTextboxAcquired) return;
                if (scrTextbox.instance.isOn)
                {
                    var conversation = scrTextbox.instance.conversation;
                    if (conversation is "FetchInfo" or "FetchInfoPost")
                        conversation = "FetchInfo";
                    if (conversation is "FetchPost" or "FetchQuest" or "FetchReward")
                        conversation = "FetchQuest";
                    if (conversation is "kioskNomoney" or "kioskBuy" or "kioskBought" or "DispatchedParty")
                        conversation = "Kiosk";
                    if (conversation is "mataMeet" or "MataParty")
                        conversation = "Mata";
                    if (conversation is "pepperMeet" or "pepperParty")
                        conversation = "Pepper";
                    if (conversation.EndsWith("Party"))
                        conversation = conversation.Replace("Party", "");

                    if (!niko.Contains(conversation) && !conversation.StartsWith("trapConv"))
                        conversation = "CHAT" + conversation;

                    if ((home.Contains(conversation) || niko.Contains(conversation))
                        && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
                    {
                        scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
                    }
                    else if (!home.Contains(conversation) && !LoggedConversations.Contains(conversation) &&
                             !conversation.StartsWith("trapConv"))
                    {
                        Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
                        LoggedConversations.Add(conversation);
                    }
                }

                break;
            case "Hairball City":
                //NpcGameObject.SetActive(ArchipelagoClient.HcNPCs);
                if (isLevelTextbox && !ArchipelagoClient.HairballTextboxAcquired) return;
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
                    if (conversation is "FischerAll" or "FishsanityNotEnough" or "FishsanityFishing"
                        or "FishsanityFinal" or "FishsanityObtained" or "FischerLocationIdle" or "FishsanityNoSwimming" or "fish0"
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
                    if (conversation is "pepperGotAllCoins"
                        or "pepperRandom1" or "pepperRandom2" or "pepperRandom3" or "pepperRandom4" or "pepperRandom5" 
                        or "pepperRandom6" or "pepperRandom7" or "pepperRandom8" or "pepperRandom9")
                        conversation = "pepperMain";
                    if (conversation is "dogeBoneQuest" or "dogeBonePost") conversation = "BoneDog";
                    if (conversation is "dog1") conversation = "dog";
                    
                    if (!niko.Contains(conversation) && !conversation.StartsWith("trapConv"))
                        conversation = "CHAT" + conversation;

                    if ((hairball.Contains(conversation) || niko.Contains(conversation))
                        && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
                    {
                        scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
                    }
                    else if (!hairball.Contains(conversation) && niko.Contains(conversation) &&
                             !LoggedConversations.Contains(conversation) && !conversation.StartsWith("trapConv"))
                    {
                        Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
                        LoggedConversations.Add(conversation);
                    }
                }

                break;
            case "Trash Kingdom":
                //NpcGameObject.SetActive(ArchipelagoClient.TtNPCs);
                if (isLevelTextbox && !ArchipelagoClient.TurbineTextboxAcquired) return;
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
                    if (conversation is "FischerAll" or "FishsanityNotEnough" or "FishsanityFishing"
                        or "FishsanityFinal" or "FishsanityObtained" or "FischerLocationIdle" or "FishsanityNoSwimming" or "fish0"
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
                    if (conversation is "pepperGotAllCoins"
                        or "pepperRandom1" or "pepperRandom2" or "pepperRandom3" or "pepperRandom4" or "pepperRandom5" 
                        or "pepperRandom6" or "pepperRandom7" or "pepperRandom8" or "pepperRandom9")
                        conversation = "pepperMain";
                    if (conversation is "dogeBoneQuest" or "dogeBonePost") conversation = "BoneDog";
                    
                    if (!niko.Contains(conversation) && !conversation.StartsWith("trapConv"))
                        conversation = "CHAT" + conversation;

                    if ((turbine.Contains(conversation) || niko.Contains(conversation))
                        && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
                    {
                        scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
                    }
                    else if (!turbine.Contains(conversation) && !niko.Contains(conversation) &&
                             !conversation.StartsWith("trapConv") && !LoggedConversations.Contains(conversation))
                    {
                        Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
                        LoggedConversations.Add(conversation);
                    }
                }

                break;
            case "Salmon Creek Forest":
                //NpcGameObject.SetActive(ArchipelagoClient.SfcNPCs);
                if (isLevelTextbox && !ArchipelagoClient.SalmonTextboxAcquired) return;
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
                    if (conversation is "FischerAll" or "FishsanityNotEnough" or "FishsanityFishing"
                        or "FishsanityFinal" or "FishsanityObtained" or "FischerLocationIdle" or "FishsanityNoSwimming" or "fish0"
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
                    if (conversation is "MoomyQuest" or "MoomyReward" or "MoomyPost")
                        conversation = "Moomy";
                    if (conversation is "pepperGotAllCoins"
                        or "pepperRandom1" or "pepperRandom2" or "pepperRandom3" or "pepperRandom4" or "pepperRandom5" 
                        or "pepperRandom6" or "pepperRandom7" or "pepperRandom8" or "pepperRandom9")
                        conversation = "pepperMain";
                    if (conversation is "graffitiQuestNina" or "graffitiPostNina"
                        or "graffitiTooFewNina" or "graffitiRewardNina")
                        conversation = "Nina";
                    if (conversation is "dogeBoneQuest" or "dogeBonePost") conversation = "BoneDog";
                    
                    if (!niko.Contains(conversation) && !conversation.StartsWith("trapConv"))
                        conversation = "CHAT" + conversation;

                    if ((salmon.Contains(conversation) || niko.Contains(conversation))
                        && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
                    {
                        scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
                    }
                    else if (!salmon.Contains(conversation) && !niko.Contains(conversation) &&
                             !conversation.StartsWith("trapConv") && !LoggedConversations.Contains(conversation))
                    {
                        Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
                        LoggedConversations.Add(conversation);
                    }
                }

                break;
            case "Public Pool":
                //NpcGameObject.SetActive(ArchipelagoClient.PpNPCs);
                if (isLevelTextbox && !ArchipelagoClient.PoolTextboxAcquired) return;
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
                    if (conversation is "FischerAll" or "FishsanityNotEnough" or "FishsanityFishing"
                        or "FishsanityFinal" or "FishsanityObtained" or "FischerLocationIdle" or "FishsanityNoSwimming" or "fish0"
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
                    if (conversation is "pepperGotAllCoins"
                        or "pepperRandom1" or "pepperRandom2" or "pepperRandom3" or "pepperRandom4" or "pepperRandom5" 
                        or "pepperRandom6" or "pepperRandom7" or "pepperRandom8" or "pepperRandom9")
                        conversation = "pepperMain";
                    if (conversation is "dogeBoneQuest" or "dogeBonePost") conversation = "BoneDog";
                    
                    if (!niko.Contains(conversation) && !conversation.StartsWith("trapConv"))
                        conversation = "CHAT" + conversation;

                    if ((pool.Contains(conversation) || niko.Contains(conversation))
                        && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
                    {
                        scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
                    }
                    else if (!pool.Contains(conversation) && !niko.Contains(conversation) &&
                             !conversation.StartsWith("trapConv") && !LoggedConversations.Contains(conversation))
                    {
                        Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
                        LoggedConversations.Add(conversation);
                    }
                }

                break;
            case "The Bathhouse":
                //NpcGameObject.SetActive(ArchipelagoClient.BathNPCs);
                if (isLevelTextbox && !ArchipelagoClient.BathTextboxAcquired) return;
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
                    if (conversation is "FischerAll" or "FishsanityNotEnough" or "FishsanityFishing"
                        or "FishsanityFinal" or "FishsanityObtained" or "FischerLocationIdle" or "FishsanityNoSwimming" or "fish0"
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
                    if (conversation is "MoomyQuest" or "MoomyReward" or "MoomyPost")
                        conversation = "Moomy";
                    if (conversation is "pepperGotAllCoins"
                        or "pepperRandom1" or "pepperRandom2" or "pepperRandom3" or "pepperRandom4" or "pepperRandom5" 
                        or "pepperRandom6" or "pepperRandom7" or "pepperRandom8" or "pepperRandom9")
                        conversation = "pepperMain";
                    if (conversation is "dogeBoneQuest" or "dogeBonePost") conversation = "BoneDog";
                    
                    if (!niko.Contains(conversation) && !conversation.StartsWith("trapConv"))
                        conversation = "CHAT" + conversation;

                    if ((bathhouse.Contains(conversation) || niko.Contains(conversation))
                        && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
                    {
                        scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
                        if (conversation == "CHATMelissaStijn")
                        {
                            scrWorldSaveDataContainer.instance.miscFlags.Add("CHATStijn");
                            scrWorldSaveDataContainer.instance.miscFlags.Add("CHATMelissa");
                        }
                    }
                    else if (!bathhouse.Contains(conversation) && !niko.Contains(conversation) &&
                             !conversation.StartsWith("trapConv") && !LoggedConversations.Contains(conversation))
                    {
                        Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
                        LoggedConversations.Add(conversation);
                    }
                }

                break;
            case "Tadpole inc":
                //NpcGameObject.SetActive(ArchipelagoClient.HqNPCs);
                if (isLevelTextbox && !ArchipelagoClient.TadpoleTextboxAcquired) return;
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
                    if (conversation is "FischerAll" or "FishsanityNotEnough" or "FishsanityFishing"
                        or "FishsanityFinal" or "FishsanityObtained" or "FischerLocationIdle" or "FishsanityNoSwimming" or "fish0"
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
                    if (conversation is "pepperGotAllCoins"
                        or "pepperRandom1" or "pepperRandom2" or "pepperRandom3" or "pepperRandom4" or "pepperRandom5" 
                        or "pepperRandom6" or "pepperRandom7" or "pepperRandom8" or "pepperRandom9" or "interview")
                        conversation = "pepperMain";
                    if (conversation is "dogeBoneQuest" or "dogeBonePost") conversation = "BoneDog";
                    
                    if (!niko.Contains(conversation) && !conversation.StartsWith("trapConv"))
                        conversation = "CHAT" + conversation;

                    if ((tadpole.Contains(conversation) || niko.Contains(conversation))
                        && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
                    {
                        scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
                    }
                    else if (!tadpole.Contains(conversation) && !niko.Contains(conversation) &&
                             !conversation.StartsWith("trapConv") && !LoggedConversations.Contains(conversation))
                    {
                        Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
                        LoggedConversations.Add(conversation);
                    }
                }

                break;
            case "GarysGarden":
                if (isLevelTextbox && !ArchipelagoClient.GardenTextboxAcquired) return;
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

                    if (!conversation.StartsWith("trapConv"))
                        conversation = "CHAT" + conversation;

                    if (garden.Contains(conversation)
                        && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
                    {
                        scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
                    }
                    else if (!garden.Contains(conversation) && !LoggedConversations.Contains(conversation) &&
                             !conversation.StartsWith("trapConv"))
                    {
                        Plugin.BepinLogger.LogInfo($"Conversation not found: {conversation}");
                        LoggedConversations.Add(conversation);
                    }
                }

                break;
        }
    }

    private void NikoThoughts()
    {
        var conversation = scrTextbox.instance.conversation;
        if (niko.Contains(conversation) && !scrWorldSaveDataContainer.instance.miscFlags.Contains(conversation))
            scrWorldSaveDataContainer.instance.miscFlags.Add(conversation);
    }

    private void Update()
    {
        if (scrTrainManager.instance.isLoadingNewScene) return; // Maybe this fixes the home trap sending home conversations?
        if (ArchipelagoData.Options.Textbox == ArchipelagoOptions.TextboxLevel.Global &&
            !ArchipelagoClient.TextboxAcquired) return;
        if (Thoughtsanity)
            NikoThoughts();
        if (!IsGlobal)
        {
            LevelChat();
        }
        else
        {
            GlobalChat();
        }
    }
}