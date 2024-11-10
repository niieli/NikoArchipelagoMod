using System.Linq;
using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NikoArchipelago.Patches;

public class APItemOverworld
{
    [HarmonyPatch(typeof(scrCassette), "Start")]
    public static class CassetteTexturePatch
    {
        private static readonly GameObject APProgItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("APProgressive"));
        private static readonly GameObject APUsefulItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("APUseful"));
        private static readonly GameObject APFillerItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("APFiller"));
        private static readonly GameObject APTrapItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("APTrap"));
        private static readonly GameObject APTrap1ItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("APTrap1"));
        private static readonly GameObject APTrap2ItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("APTrap2"));
        private static readonly GameObject CoinItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Coin"));
        private static readonly GameObject CassetteItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Cassette"));
        private static readonly GameObject KeyItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Key"));
        private static readonly GameObject ContactListItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("ContactList"));
        private static readonly GameObject ApplesItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Apples"));
        private static readonly GameObject SnailMoneyItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("SnailMoney"));
        private static readonly GameObject LetterItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Letter"));
        private static readonly GameObject BugsItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Bugs"));
        private static readonly GameObject HairballCityItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("HairballCity"));
        private static readonly GameObject TurbineTownItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("TurbineTown"));
        private static readonly GameObject SalmonCreekForestItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("SalmonCreekForest"));
        private static readonly GameObject PublicPoolItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("PublicPool"));
        private static readonly GameObject BathhouseItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Bathhouse"));
        private static readonly GameObject TadpoleHQItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("TadpoleHQ"));
        private static readonly GameObject GarysGardenItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("GarysGarden"));
        private static void Postfix(scrCassette __instance)
        {
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            var apProg = APProgItemOverworld.transform.Find("Quads").gameObject;
            apProg.transform.position = ogQuads.transform.position;
            apProg.AddComponent<ScuffedSpin>();
            var apUseful = APUsefulItemOverworld.transform.Find("Quads").gameObject;
            apUseful.transform.position = ogQuads.transform.position;
            apUseful.AddComponent<ScuffedSpin>();
            var apFiller = APFillerItemOverworld.transform.Find("Quads").gameObject;
            apFiller.transform.position = ogQuads.transform.position;
            apFiller.AddComponent<ScuffedSpin>();
            var apTrap = APTrapItemOverworld.transform.Find("Quads").gameObject;
            apTrap.transform.position = ogQuads.transform.position;
            apTrap.AddComponent<ScuffedSpin>();
            var apTrap1 = APTrap1ItemOverworld.transform.Find("Quads").gameObject;
            apTrap1.transform.position = ogQuads.transform.position;
            apTrap1.AddComponent<ScuffedSpin>();
            var apTrap2 = APTrap2ItemOverworld.transform.Find("Quads").gameObject;
            apTrap2.transform.position = ogQuads.transform.position;
            apTrap2.AddComponent<ScuffedSpin>();
            var coin = CoinItemOverworld.transform.Find("Quads").gameObject;
            coin.transform.position = ogQuads.transform.position;
            coin.AddComponent<ScuffedSpin>();
            var cassette = CassetteItemOverworld.transform.Find("Quads").gameObject;
            cassette.transform.position = ogQuads.transform.position;
            cassette.AddComponent<ScuffedSpin>();
            var key = KeyItemOverworld.transform.Find("Quads").gameObject;
            key.transform.position = ogQuads.transform.position;
            key.AddComponent<ScuffedSpin>();
            var contactList = ContactListItemOverworld.transform.Find("Quads").gameObject;
            contactList.transform.position = ogQuads.transform.position;
            contactList.AddComponent<ScuffedSpin>();
            var apples = ApplesItemOverworld.transform.Find("Quads").gameObject;
            apples.transform.position = ogQuads.transform.position;
            apples.AddComponent<ScuffedSpin>();
            var snailMoney = SnailMoneyItemOverworld.transform.Find("Quads").gameObject;
            snailMoney.transform.position = ogQuads.transform.position;
            snailMoney.AddComponent<ScuffedSpin>();
            var letter = LetterItemOverworld.transform.Find("Quads").gameObject;
            letter.transform.position = ogQuads.transform.position;
            letter.AddComponent<ScuffedSpin>();
            var bugs = BugsItemOverworld.transform.Find("Quads").gameObject;
            bugs.transform.position = ogQuads.transform.position;
            bugs.AddComponent<ScuffedSpin>();
            var hairballCity = HairballCityItemOverworld.transform.Find("Quads").gameObject;
            hairballCity.transform.position = ogQuads.transform.position;
            hairballCity.AddComponent<ScuffedSpin>();
            var turbineTown = TurbineTownItemOverworld.transform.Find("Quads").gameObject;
            turbineTown.transform.position = ogQuads.transform.position;
            turbineTown.AddComponent<ScuffedSpin>();
            var salmonCreekForest = SalmonCreekForestItemOverworld.transform.Find("Quads").gameObject;
            salmonCreekForest.transform.position = ogQuads.transform.position;
            salmonCreekForest.AddComponent<ScuffedSpin>();
            var publicPool = PublicPoolItemOverworld.transform.Find("Quads").gameObject;
            publicPool.transform.position = ogQuads.transform.position;
            publicPool.AddComponent<ScuffedSpin>();
            var bathhouse = BathhouseItemOverworld.transform.Find("Quads").gameObject;
            bathhouse.transform.position = ogQuads.transform.position;
            bathhouse.AddComponent<ScuffedSpin>();
            var tadpoleHQ = TadpoleHQItemOverworld.transform.Find("Quads").gameObject;
            tadpoleHQ.transform.position = ogQuads.transform.position;
            tadpoleHQ.AddComponent<ScuffedSpin>();
            var garysGarden = GarysGardenItemOverworld.transform.Find("Quads").gameObject;
            garysGarden.transform.position = ogQuads.transform.position;
            garysGarden.AddComponent<ScuffedSpin>();
            ogQuads.SetActive(false);
            
            var flagField = AccessTools.Field(typeof(scrCassette), "flag");
            var flag = (string)flagField.GetValue(__instance);
            
            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    var list = Locations.ScoutHCCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == flag);
                    const int offset = 101;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = apProg;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = apUseful;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    apTrap,
                                    apTrap1,
                                    apTrap2
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = apFiller;
                            }
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => coin,
                                "Cassette" => cassette,
                                "Key" => key,
                                "25 Apples" => apples,
                                "10 Bugs" => bugs,
                                "1000 Snail Dollar" => snailMoney,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => contactList,
                                "Hairball City Ticket" => hairballCity,
                                "Turbine Town Ticket" => turbineTown,
                                "Salmon Creek Forest Ticket" => salmonCreekForest,
                                "Public Pool Ticket" => publicPool,
                                "Bathhouse Ticket" => bathhouse,
                                "Tadpole HQ Ticket" => tadpoleHQ,
                                _ => apProg
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
                case "Trash Kingdom":
                {
                    var list = Locations.ScoutTTCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == flag);
                    const int offset = 111;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = apProg;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = apUseful;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = apFiller;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    apTrap,
                                    apTrap1,
                                    apTrap2
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = apFiller;
                            }
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => coin,
                                "Cassette" => cassette,
                                "Key" => key,
                                "25 Apples" => apples,
                                "10 Bugs" => bugs,
                                "1000 Snail Dollar" => snailMoney,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => contactList,
                                "Hairball City Ticket" => hairballCity,
                                "Turbine Town Ticket" => turbineTown,
                                "Salmon Creek Forest Ticket" => salmonCreekForest,
                                "Public Pool Ticket" => publicPool,
                                "Bathhouse Ticket" => bathhouse,
                                "Tadpole HQ Ticket" => tadpoleHQ,
                                _ => apProg
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSFCCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == flag);
                    const int offset = 121;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = apProg;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = apUseful;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    apTrap,
                                    apTrap1,
                                    apTrap2
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = apFiller;
                            }
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => coin,
                                "Cassette" => cassette,
                                "Key" => key,
                                "25 Apples" => apples,
                                "10 Bugs" => bugs,
                                "1000 Snail Dollar" => snailMoney,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => contactList,
                                "Hairball City Ticket" => hairballCity,
                                "Turbine Town Ticket" => turbineTown,
                                "Salmon Creek Forest Ticket" => salmonCreekForest,
                                "Public Pool Ticket" => publicPool,
                                "Bathhouse Ticket" => bathhouse,
                                "Tadpole HQ Ticket" => tadpoleHQ,
                                _ => apProg
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
                case "Public Pool":
                {
                    var list = Locations.ScoutPPCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == flag);
                    const int offset = 132;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = apProg;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = apUseful;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    apTrap,
                                    apTrap1,
                                    apTrap2
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = apFiller;
                            }
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => coin,
                                "Cassette" => cassette,
                                "Key" => key,
                                "25 Apples" => apples,
                                "10 Bugs" => bugs,
                                "1000 Snail Dollar" => snailMoney,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => contactList,
                                "Hairball City Ticket" => hairballCity,
                                "Turbine Town Ticket" => turbineTown,
                                "Salmon Creek Forest Ticket" => salmonCreekForest,
                                "Public Pool Ticket" => publicPool,
                                "Bathhouse Ticket" => bathhouse,
                                "Tadpole HQ Ticket" => tadpoleHQ,
                                _ => apProg
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == flag);
                    const int offset = 142;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = apProg;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = apUseful;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    apTrap,
                                    apTrap1,
                                    apTrap2
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = apFiller;
                            }
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => coin,
                                "Cassette" => cassette,
                                "Key" => key,
                                "25 Apples" => apples,
                                "10 Bugs" => bugs,
                                "1000 Snail Dollar" => snailMoney,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => contactList,
                                "Hairball City Ticket" => hairballCity,
                                "Turbine Town Ticket" => turbineTown,
                                "Salmon Creek Forest Ticket" => salmonCreekForest,
                                "Public Pool Ticket" => publicPool,
                                "Bathhouse Ticket" => bathhouse,
                                "Tadpole HQ Ticket" => tadpoleHQ,
                                _ => apProg
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutHQCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == flag);
                    const int offset = 152;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = apProg;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = apUseful;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    apTrap,
                                    apTrap1,
                                    apTrap2
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = apFiller;
                            }
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => coin,
                                "Cassette" => cassette,
                                "Key" => key,
                                "25 Apples" => apples,
                                "10 Bugs" => bugs,
                                "1000 Snail Dollar" => snailMoney,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => contactList,
                                "Hairball City Ticket" => hairballCity,
                                "Turbine Town Ticket" => turbineTown,
                                "Salmon Creek Forest Ticket" => salmonCreekForest,
                                "Public Pool Ticket" => publicPool,
                                "Bathhouse Ticket" => bathhouse,
                                "Tadpole HQ Ticket" => tadpoleHQ,
                                _ => apProg
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
                case "GarysGarden":
                {
                    var list = Locations.ScoutGardenCassetteList.ToList();
                    var index = list.FindIndex(pair => pair.Value == flag);
                    const int offset = 162;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = apProg;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = apUseful;
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    apTrap,
                                    apTrap1,
                                    apTrap2
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = apFiller;
                            }
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => coin,
                                "Cassette" => cassette,
                                "Key" => key,
                                "25 Apples" => apples,
                                "10 Bugs" => bugs,
                                "1000 Snail Dollar" => snailMoney,
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => contactList,
                                "Hairball City Ticket" => hairballCity,
                                "Turbine Town Ticket" => turbineTown,
                                "Salmon Creek Forest Ticket" => salmonCreekForest,
                                "Public Pool Ticket" => publicPool,
                                "Bathhouse Ticket" => bathhouse,
                                "Tadpole HQ Ticket" => tadpoleHQ,
                                _ => apProg
                            };
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
            }

            __instance.quads = apProg;
        }
    }
    
    [HarmonyPatch(typeof(scrCoin), "Start")]
    public static class CoinTexturePatch
    {
        private static void Postfix(scrCoin __instance)
        {
            //var t = __instance.transform.Find("Quads/Front").GetComponent<MeshRenderer>();
            //t.enabled = false;
        }
    }
}