using System.Linq;
using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace NikoArchipelago.Patches;

public class APItemOverworld
{
    [HarmonyPatch(typeof(scrCassette), "Start")]
    public static class CassetteTexturePatch
    {
        private static GameObject _apProgItemOverworld;
        private static GameObject _apUsefulItemOverworld;
        private static GameObject _apFillerItemOverworld;
        private static GameObject _apTrapItemOverworld;
        private static GameObject _apTrap1ItemOverworld;
        private static GameObject _apTrap2ItemOverworld;
        private static GameObject _coinItemOverworld;
        private static GameObject _cassetteItemOverworld;
        private static GameObject _keyItemOverworld;
        private static GameObject _contactListItemOverworld;
        private static GameObject _applesItemOverworld;
        private static GameObject _snailMoneyItemOverworld;
        private static GameObject _letterItemOverworld;
        private static GameObject _bugsItemOverworld;
        private static GameObject _hairballCityItemOverworld;
        private static GameObject _turbineTownItemOverworld;
        private static GameObject _salmonCreekForestItemOverworld;
        private static GameObject _publicPoolItemOverworld;
        private static GameObject _bathhouseItemOverworld;
        private static GameObject _tadpoleHqItemOverworld;
        private static GameObject _garysGardenItemOverworld;
        private static GameObject _superJumpItemOverworld;

        private static GameObject CreateItemOverworld(string itemName, scrCassette __instance)
        {
            switch (itemName)
            {
                case "apProg":
                {
                    _apProgItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("APProgressive"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var apProg = _apProgItemOverworld.transform.Find("Quads").gameObject;
                    apProg.transform.position = ogQuads.transform.position;
                    apProg.AddComponent<ScuffedSpin>();
                    apProg.transform.SetParent(__instance.transform);
                    return apProg;
                }
                case "apUseful":
                {
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var apUseful = _apUsefulItemOverworld.transform.Find("Quads").gameObject;
                    apUseful.transform.position = ogQuads.transform.position;
                    apUseful.AddComponent<ScuffedSpin>();
                    apUseful.transform.SetParent(__instance.transform);
                    return apUseful;
                }
                case "apFiller":
                {
                    _apFillerItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("APFiller"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var apFiller = _apFillerItemOverworld.transform.Find("Quads").gameObject;
                    apFiller.transform.position = ogQuads.transform.position;
                    apFiller.AddComponent<ScuffedSpin>();
                    apFiller.transform.SetParent(__instance.transform);
                    return apFiller;
                }
                case "apTrap":
                {
                    _apTrapItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("APTrap"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var apTrap = _apTrapItemOverworld.transform.Find("Quads").gameObject;
                    apTrap.transform.position = ogQuads.transform.position;
                    apTrap.AddComponent<ScuffedSpin>();
                    apTrap.transform.SetParent(__instance.transform);
                    return apTrap;
                }
                case "apTrap1":
                {
                    _apTrap1ItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("APTrap1"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var apTrap1 = _apTrap1ItemOverworld.transform.Find("Quads").gameObject;
                    apTrap1.transform.position = ogQuads.transform.position;
                    apTrap1.AddComponent<ScuffedSpin>();
                    apTrap1.transform.SetParent(__instance.transform);
                    return apTrap1;
                }
                case "apTrap2":
                {
                    _apTrap2ItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("APTrap2"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var apTrap2 = _apTrap2ItemOverworld.transform.Find("Quads").gameObject;
                    apTrap2.transform.position = ogQuads.transform.position;
                    apTrap2.AddComponent<ScuffedSpin>();
                    apTrap2.transform.SetParent(__instance.transform);
                    return apTrap2;
                }
                case "coin":
                {
                    _coinItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Coin"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var coin = _coinItemOverworld.transform.Find("Quads").gameObject;
                    coin.transform.position = ogQuads.transform.position;
                    coin.AddComponent<ScuffedSpin>();
                    coin.transform.SetParent(__instance.transform);
                    return coin;
                }
                case "cassette":
                {
                    _cassetteItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Cassette"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var cassette = _cassetteItemOverworld.transform.Find("Quads").gameObject;
                    cassette.transform.position = ogQuads.transform.position;
                    cassette.AddComponent<ScuffedSpin>();
                    cassette.transform.SetParent(__instance.transform);
                    return cassette;
                }
                case "key":
                {
                    _keyItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Key"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var key = _keyItemOverworld.transform.Find("Quads").gameObject;
                    key.transform.position = ogQuads.transform.position;
                    key.AddComponent<ScuffedSpin>();
                    key.transform.SetParent(__instance.transform);
                    return key;
                }
                case "contactList":
                {
                    _contactListItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("ContactList"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var contactList = _contactListItemOverworld.transform.Find("Quads").gameObject;
                    contactList.transform.position = ogQuads.transform.position;
                    contactList.AddComponent<ScuffedSpin>();
                    contactList.transform.SetParent(__instance.transform);
                    return contactList;
                }
                case "apples":
                {
                    _applesItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Apples"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var apples = _applesItemOverworld.transform.Find("Quads").gameObject;
                    apples.transform.position = ogQuads.transform.position;
                    apples.AddComponent<ScuffedSpin>();
                    apples.transform.SetParent(__instance.transform);
                    return apples;
                }
                case "snailMoney":
                {
                    _snailMoneyItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("SnailMoney"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var snailMoney = _snailMoneyItemOverworld.transform.Find("Quads").gameObject;
                    snailMoney.transform.position = ogQuads.transform.position;
                    snailMoney.AddComponent<ScuffedSpin>();
                    snailMoney.transform.SetParent(__instance.transform);
                    return snailMoney;
                }
                case "letter":
                {
                    _letterItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Letter"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var letter = _letterItemOverworld.transform.Find("Quads").gameObject;
                    letter.transform.position = ogQuads.transform.position;
                    letter.AddComponent<ScuffedSpin>();
                    letter.transform.SetParent(__instance.transform);
                    return letter;
                }
                case "bugs":
                {
                    _bugsItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Bugs"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var bugs = _bugsItemOverworld.transform.Find("Quads").gameObject;
                    bugs.transform.position = ogQuads.transform.position;
                    bugs.AddComponent<ScuffedSpin>();
                    bugs.transform.SetParent(__instance.transform);
                    return bugs;
                }
                case "superJump":
                {
                    _superJumpItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("SuperJump"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var super = _superJumpItemOverworld.transform.Find("Quads").gameObject;
                    super.transform.position = ogQuads.transform.position;
                    super.AddComponent<ScuffedSpin>();
                    super.transform.SetParent(__instance.transform);
                    return super;
                }
                case "hairballCity":
                {
                    _hairballCityItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("HairballCity"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var hairballCity = _hairballCityItemOverworld.transform.Find("Quads").gameObject;
                    hairballCity.transform.position = ogQuads.transform.position;
                    hairballCity.AddComponent<ScuffedSpin>();
                    hairballCity.transform.SetParent(__instance.transform);
                    return hairballCity;
                }
                case "turbineTown":
                {
                    _turbineTownItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("TurbineTown"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var turbineTown = _turbineTownItemOverworld.transform.Find("Quads").gameObject;
                    turbineTown.transform.position = ogQuads.transform.position;
                    turbineTown.AddComponent<ScuffedSpin>();
                    turbineTown.transform.SetParent(__instance.transform);
                    return turbineTown;
                }
                case "salmonCreekForest":
                {
                    _salmonCreekForestItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("SalmonCreekForest"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var salmonCreekForest = _salmonCreekForestItemOverworld.transform.Find("Quads").gameObject;
                    salmonCreekForest.transform.position = ogQuads.transform.position;
                    salmonCreekForest.AddComponent<ScuffedSpin>();
                    salmonCreekForest.transform.SetParent(__instance.transform);
                    return salmonCreekForest;
                }
                case "publicPool":
                {
                    _publicPoolItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("PublicPool"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var publicPool = _publicPoolItemOverworld.transform.Find("Quads").gameObject;
                    publicPool.transform.position = ogQuads.transform.position;
                    publicPool.AddComponent<ScuffedSpin>();
                    publicPool.transform.SetParent(__instance.transform);
                    return publicPool;
                }
                case "bathhouse":
                {
                    _bathhouseItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Bathhouse"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var bathhouse = _bathhouseItemOverworld.transform.Find("Quads").gameObject;
                    bathhouse.transform.position = ogQuads.transform.position;
                    bathhouse.AddComponent<ScuffedSpin>();
                    bathhouse.transform.SetParent(__instance.transform);
                    return bathhouse;
                }
                case "tadpoleHQ":
                {
                    _tadpoleHqItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("TadpoleHQ"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var tadpoleHQ = _tadpoleHqItemOverworld.transform.Find("Quads").gameObject;
                    tadpoleHQ.transform.position = ogQuads.transform.position;
                    tadpoleHQ.AddComponent<ScuffedSpin>();
                    tadpoleHQ.transform.SetParent(__instance.transform);
                    return tadpoleHQ;
                }
                case "garysGarden":
                {
                    _garysGardenItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("GarysGarden"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var garysGarden = _garysGardenItemOverworld.transform.Find("Quads").gameObject;
                    garysGarden.transform.position = ogQuads.transform.position;
                    garysGarden.AddComponent<ScuffedSpin>();
                    garysGarden.transform.SetParent(__instance.transform);
                    return garysGarden;
                }
            }
            return null;
        }
        private static void Postfix(scrCassette __instance)
        {
            if (!ArchipelagoMenu.cacmi) return;
            var ogQuads = __instance.transform.Find("Quads").gameObject;
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
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    CreateItemOverworld("apTrap", __instance),
                                    CreateItemOverworld("apTrap1", __instance),
                                    CreateItemOverworld("apTrap2", __instance)
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }

                            Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                        {
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => CreateItemOverworld("contactList", __instance),
                                "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                                "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                                "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                                "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                                "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                                "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                                "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                                "Super Jump" => CreateItemOverworld("superJump", __instance),
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                        }
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
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    CreateItemOverworld("apTrap", __instance),
                                    CreateItemOverworld("apTrap1", __instance),
                                    CreateItemOverworld("apTrap2", __instance),
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }
                
                            Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                        {
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => CreateItemOverworld("contactList", __instance),
                                "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                                "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                                "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                                "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                                "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                                "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                                "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                                "Super Jump" => CreateItemOverworld("superJump", __instance),
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                        }
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
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    CreateItemOverworld("apTrap", __instance),
                                    CreateItemOverworld("apTrap1", __instance),
                                    CreateItemOverworld("apTrap2", __instance),
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }
                
                            Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                        {
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => CreateItemOverworld("contactList", __instance),
                                "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                                "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                                "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                                "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                                "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                                "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                                "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                                "Super Jump" => CreateItemOverworld("superJump", __instance),
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                        }
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
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    CreateItemOverworld("apTrap", __instance),
                                    CreateItemOverworld("apTrap1", __instance),
                                    CreateItemOverworld("apTrap2", __instance),
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }
                
                            Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                        {
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => CreateItemOverworld("contactList", __instance),
                                "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                                "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                                "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                                "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                                "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                                "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                                "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                                "Super Jump" => CreateItemOverworld("superJump", __instance),
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                        }
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
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    CreateItemOverworld("apTrap", __instance),
                                    CreateItemOverworld("apTrap1", __instance),
                                    CreateItemOverworld("apTrap2", __instance),
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }
                
                            Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                        {
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => CreateItemOverworld("contactList", __instance),
                                "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                                "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                                "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                                "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                                "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                                "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                                "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                                "Super Jump" => CreateItemOverworld("superJump", __instance),
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                        }
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
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    CreateItemOverworld("apTrap", __instance),
                                    CreateItemOverworld("apTrap1", __instance),
                                    CreateItemOverworld("apTrap2", __instance),
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }
                
                            Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                        {
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => CreateItemOverworld("contactList", __instance),
                                "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                                "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                                "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                                "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                                "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                                "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                                "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                                "Super Jump" => CreateItemOverworld("superJump", __instance),
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                        }
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
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    CreateItemOverworld("apTrap", __instance),
                                    CreateItemOverworld("apTrap1", __instance),
                                    CreateItemOverworld("apTrap2", __instance),
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }
                
                            Plugin.BepinLogger.LogInfo("Item: " +
                                                       ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                        {
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => CreateItemOverworld("contactList", __instance),
                                "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                                "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                                "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                                "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                                "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                                "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                                "Super Jump" => CreateItemOverworld("superJump", __instance),
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                        }
                    }
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
            }
        }
    }

    [HarmonyPatch(typeof(scrCoin), "Start")]
    public static class CoinTexturePatch
    {
        private static GameObject _apProgItemOverworld;
        private static GameObject _apUsefulItemOverworld;
        private static GameObject _apFillerItemOverworld;
        private static GameObject _apTrapItemOverworld;
        private static GameObject _apTrap1ItemOverworld;
        private static GameObject _apTrap2ItemOverworld;
        private static GameObject _coinItemOverworld;
        private static GameObject _cassetteItemOverworld;
        private static GameObject _keyItemOverworld;
        private static GameObject _contactListItemOverworld;
        private static GameObject _applesItemOverworld;
        private static GameObject _snailMoneyItemOverworld;
        private static GameObject _letterItemOverworld;
        private static GameObject _bugsItemOverworld;
        private static GameObject _hairballCityItemOverworld;
        private static GameObject _turbineTownItemOverworld;
        private static GameObject _salmonCreekForestItemOverworld;
        private static GameObject _publicPoolItemOverworld;
        private static GameObject _bathhouseItemOverworld;
        private static GameObject _tadpoleHqItemOverworld;
        private static GameObject _garysGardenItemOverworld;
        private static GameObject _superJumpItemOverworld;

        private static GameObject CreateItemOverworld(string itemName, scrCoin __instance)
        {
            switch (itemName)
            {
                case "apProg":
                {
                    _apProgItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("APProgressive"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var apProg = _apProgItemOverworld.transform.Find("Quads").gameObject;
                    apProg.transform.position = ogQuads.transform.position;
                    apProg.AddComponent<ScuffedSpin>();
                    apProg.transform.SetParent(__instance.transform);
                    return apProg;
                }
                case "apUseful":
                {
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var apUseful = _apUsefulItemOverworld.transform.Find("Quads").gameObject;
                    apUseful.transform.position = ogQuads.transform.position;
                    apUseful.AddComponent<ScuffedSpin>();
                    apUseful.transform.SetParent(__instance.transform);
                    return apUseful;
                }
                case "apFiller":
                {
                    _apFillerItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("APFiller"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var apFiller = _apFillerItemOverworld.transform.Find("Quads").gameObject;
                    apFiller.transform.position = ogQuads.transform.position;
                    apFiller.AddComponent<ScuffedSpin>();
                    apFiller.transform.SetParent(__instance.transform);
                    return apFiller;
                }
                case "apTrap":
                {
                    _apTrapItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("APTrap"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var apTrap = _apTrapItemOverworld.transform.Find("Quads").gameObject;
                    apTrap.transform.position = ogQuads.transform.position;
                    apTrap.AddComponent<ScuffedSpin>();
                    apTrap.transform.SetParent(__instance.transform);
                    return apTrap;
                }
                case "apTrap1":
                {
                    _apTrap1ItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("APTrap1"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var apTrap1 = _apTrap1ItemOverworld.transform.Find("Quads").gameObject;
                    apTrap1.transform.position = ogQuads.transform.position;
                    apTrap1.AddComponent<ScuffedSpin>();
                    apTrap1.transform.SetParent(__instance.transform);
                    return apTrap1;
                }
                case "apTrap2":
                {
                    _apTrap2ItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("APTrap2"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var apTrap2 = _apTrap2ItemOverworld.transform.Find("Quads").gameObject;
                    apTrap2.transform.position = ogQuads.transform.position;
                    apTrap2.AddComponent<ScuffedSpin>();
                    apTrap2.transform.SetParent(__instance.transform);
                    return apTrap2;
                }
                case "coin":
                {
                    _coinItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Coin"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var coin = _coinItemOverworld.transform.Find("Quads").gameObject;
                    coin.transform.position = ogQuads.transform.position;
                    coin.AddComponent<ScuffedSpin>();
                    coin.transform.SetParent(__instance.transform);
                    return coin;
                }
                case "cassette":
                {
                    _cassetteItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Cassette"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var cassette = _cassetteItemOverworld.transform.Find("Quads").gameObject;
                    cassette.transform.position = ogQuads.transform.position;
                    cassette.AddComponent<ScuffedSpin>();
                    cassette.transform.SetParent(__instance.transform);
                    return cassette;
                }
                case "key":
                {
                    _keyItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Key"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var key = _keyItemOverworld.transform.Find("Quads").gameObject;
                    key.transform.position = ogQuads.transform.position;
                    key.AddComponent<ScuffedSpin>();
                    key.transform.SetParent(__instance.transform);
                    return key;
                }
                case "contactList":
                {
                    _contactListItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("ContactList"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var contactList = _contactListItemOverworld.transform.Find("Quads").gameObject;
                    contactList.transform.position = ogQuads.transform.position;
                    contactList.AddComponent<ScuffedSpin>();
                    contactList.transform.SetParent(__instance.transform);
                    return contactList;
                }
                case "apples":
                {
                    _applesItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Apples"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var apples = _applesItemOverworld.transform.Find("Quads").gameObject;
                    apples.transform.position = ogQuads.transform.position;
                    apples.AddComponent<ScuffedSpin>();
                    apples.transform.SetParent(__instance.transform);
                    return apples;
                }
                case "snailMoney":
                {
                    _snailMoneyItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("SnailMoney"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var snailMoney = _snailMoneyItemOverworld.transform.Find("Quads").gameObject;
                    snailMoney.transform.position = ogQuads.transform.position;
                    snailMoney.AddComponent<ScuffedSpin>();
                    snailMoney.transform.SetParent(__instance.transform);
                    return snailMoney;
                }
                case "letter":
                {
                    _letterItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Letter"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var letter = _letterItemOverworld.transform.Find("Quads").gameObject;
                    letter.transform.position = ogQuads.transform.position;
                    letter.AddComponent<ScuffedSpin>();
                    letter.transform.SetParent(__instance.transform);
                    return letter;
                }
                case "bugs":
                {
                    _bugsItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Bugs"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var bugs = _bugsItemOverworld.transform.Find("Quads").gameObject;
                    bugs.transform.position = ogQuads.transform.position;
                    bugs.AddComponent<ScuffedSpin>();
                    bugs.transform.SetParent(__instance.transform);
                    return bugs;
                }
                case "superJump":
                {
                    _superJumpItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("SuperJump"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var super = _superJumpItemOverworld.transform.Find("Quads").gameObject;
                    super.transform.position = ogQuads.transform.position;
                    super.AddComponent<ScuffedSpin>();
                    super.transform.SetParent(__instance.transform);
                    return super;
                }
                case "hairballCity":
                {
                    _hairballCityItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("HairballCity"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var hairballCity = _hairballCityItemOverworld.transform.Find("Quads").gameObject;
                    hairballCity.transform.position = ogQuads.transform.position;
                    hairballCity.AddComponent<ScuffedSpin>();
                    hairballCity.transform.SetParent(__instance.transform);
                    return hairballCity;
                }
                case "turbineTown":
                {
                    _turbineTownItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("TurbineTown"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var turbineTown = _turbineTownItemOverworld.transform.Find("Quads").gameObject;
                    turbineTown.transform.position = ogQuads.transform.position;
                    turbineTown.AddComponent<ScuffedSpin>();
                    turbineTown.transform.SetParent(__instance.transform);
                    return turbineTown;
                }
                case "salmonCreekForest":
                {
                    _salmonCreekForestItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("SalmonCreekForest"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var salmonCreekForest = _salmonCreekForestItemOverworld.transform.Find("Quads").gameObject;
                    salmonCreekForest.transform.position = ogQuads.transform.position;
                    salmonCreekForest.AddComponent<ScuffedSpin>();
                    salmonCreekForest.transform.SetParent(__instance.transform);
                    return salmonCreekForest;
                }
                case "publicPool":
                {
                    _publicPoolItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("PublicPool"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var publicPool = _publicPoolItemOverworld.transform.Find("Quads").gameObject;
                    publicPool.transform.position = ogQuads.transform.position;
                    publicPool.AddComponent<ScuffedSpin>();
                    publicPool.transform.SetParent(__instance.transform);
                    return publicPool;
                }
                case "bathhouse":
                {
                    _bathhouseItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Bathhouse"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var bathhouse = _bathhouseItemOverworld.transform.Find("Quads").gameObject;
                    bathhouse.transform.position = ogQuads.transform.position;
                    bathhouse.AddComponent<ScuffedSpin>();
                    bathhouse.transform.SetParent(__instance.transform);
                    return bathhouse;
                }
                case "tadpoleHQ":
                {
                    _tadpoleHqItemOverworld = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("TadpoleHQ"));
                    var ogQuads = __instance.transform.Find("Quads").gameObject;
                    var tadpoleHQ = _tadpoleHqItemOverworld.transform.Find("Quads").gameObject;
                    tadpoleHQ.transform.position = ogQuads.transform.position;
                    tadpoleHQ.AddComponent<ScuffedSpin>();
                    tadpoleHQ.transform.SetParent(__instance.transform);
                    return tadpoleHQ;
                }
            }
            return null;
        }
        private static void Postfix(scrCoin __instance)
        {
            if (!ArchipelagoMenu.cacmi) return;
            var ogQuads = __instance.transform.Find("Quads").gameObject;
            ogQuads.SetActive(false);

            var currentscene = SceneManager.GetActiveScene().name;
            switch (currentscene)
            {
                case "Hairball City":
                {
                    var list = Locations.ScoutHCCoinList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    const int offset = 36;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    CreateItemOverworld("apTrap", __instance),
                                    CreateItemOverworld("apTrap1", __instance),
                                    CreateItemOverworld("apTrap2", __instance)
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }

                            Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                        {
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => CreateItemOverworld("contactList", __instance),
                                "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                                "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                                "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                                "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                                "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                                "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                                "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                                "Super Jump" => CreateItemOverworld("superJump", __instance),
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                        }
                    }

                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
                case "Trash Kingdom":
                {
                    var list = Locations.ScoutTTCoinList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    const int offset = 49;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    CreateItemOverworld("apTrap", __instance),
                                    CreateItemOverworld("apTrap1", __instance),
                                    CreateItemOverworld("apTrap2", __instance),
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }
                
                            Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                        {
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => CreateItemOverworld("contactList", __instance),
                                "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                                "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                                "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                                "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                                "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                                "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                                "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                                "Super Jump" => CreateItemOverworld("superJump", __instance),
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                        }
                    }
                
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
                case "Salmon Creek Forest":
                {
                    var list = Locations.ScoutSFCCoinList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    const int offset = 58;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    CreateItemOverworld("apTrap", __instance),
                                    CreateItemOverworld("apTrap1", __instance),
                                    CreateItemOverworld("apTrap2", __instance),
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }
                
                            Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                        {
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => CreateItemOverworld("contactList", __instance),
                                "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                                "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                                "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                                "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                                "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                                "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                                "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                                "Super Jump" => CreateItemOverworld("superJump", __instance),
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                        }
                    }
                
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
                case "Public Pool":
                {
                    var list = Locations.ScoutPPCoinList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    const int offset = 72;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    CreateItemOverworld("apTrap", __instance),
                                    CreateItemOverworld("apTrap1", __instance),
                                    CreateItemOverworld("apTrap2", __instance),
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }
                
                            Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                        {
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => CreateItemOverworld("contactList", __instance),
                                "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                                "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                                "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                                "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                                "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                                "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                                "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                                "Super Jump" => CreateItemOverworld("superJump", __instance),
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                        }
                    }
                
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
                case "The Bathhouse":
                {
                    var list = Locations.ScoutBathCoinList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    const int offset = 80;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    CreateItemOverworld("apTrap", __instance),
                                    CreateItemOverworld("apTrap1", __instance),
                                    CreateItemOverworld("apTrap2", __instance),
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }
                
                            Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                        {
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => CreateItemOverworld("contactList", __instance),
                                "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                                "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                                "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                                "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                                "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                                "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                                "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                                "Super Jump" => CreateItemOverworld("superJump", __instance),
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                        }
                    }
                
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
                case "Tadpole inc":
                {
                    var list = Locations.ScoutHQCoinList.ToList();
                    var index = list.FindIndex(pair => pair.Value == __instance.myFlag);
                    const int offset = 93;
                    if (index + offset <= ArchipelagoClient.ScoutedLocations.Count)
                    {
                        if (ArchipelagoClient.ScoutedLocations[index + offset].ItemGame != "Here Comes Niko!")
                        {
                            if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Advancement))
                            {
                                __instance.quads = CreateItemOverworld("apProg", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.NeverExclude))
                            {
                                __instance.quads = CreateItemOverworld("apUseful", __instance);
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.Trap))
                            {
                                var trapTextures = new[]
                                {
                                    CreateItemOverworld("apTrap", __instance),
                                    CreateItemOverworld("apTrap1", __instance),
                                    CreateItemOverworld("apTrap2", __instance),
                                };
                                var randomIndex = Random.Range(0, trapTextures.Length);
                                __instance.quads = trapTextures[randomIndex];
                            }
                            else if (ArchipelagoClient.ScoutedLocations[index + offset].Flags.HasFlag(ItemFlags.None))
                            {
                                __instance.quads = CreateItemOverworld("apFiller", __instance);
                            }
                
                            Plugin.BepinLogger.LogInfo("Item: " + ArchipelagoClient.ScoutedLocations[index + offset].ItemName);
                        }
                        else
                        {
                            __instance.quads = ArchipelagoClient.ScoutedLocations[index + offset].ItemName switch
                            {
                                "Coin" => CreateItemOverworld("coin", __instance),
                                "Cassette" => CreateItemOverworld("cassette", __instance),
                                "Key" => CreateItemOverworld("key", __instance),
                                "25 Apples" => CreateItemOverworld("apples", __instance),
                                "10 Bugs" => CreateItemOverworld("bugs", __instance),
                                "1000 Snail Dollar" => CreateItemOverworld("snailMoney", __instance),
                                "Contact List 1" or "Contact List 2" or "Progressive Contact List" => CreateItemOverworld("contactList", __instance),
                                "Hairball City Ticket" => CreateItemOverworld("hairballCity", __instance),
                                "Turbine Town Ticket" => CreateItemOverworld("turbineTown", __instance),
                                "Salmon Creek Forest Ticket" => CreateItemOverworld("salmonCreekForest", __instance),
                                "Public Pool Ticket" => CreateItemOverworld("publicPool", __instance),
                                "Bathhouse Ticket" => CreateItemOverworld("bathhouse", __instance),
                                "Tadpole HQ Ticket" => CreateItemOverworld("tadpoleHQ", __instance),
                                "Gary's Garden Ticket" => CreateItemOverworld("garysGarden", __instance),
                                "Super Jump" => CreateItemOverworld("superJump", __instance),
                                _ => CreateItemOverworld("apProg", __instance)
                            };
                        }
                    }
                
                    Plugin.BepinLogger.LogInfo("Index: " + index + ", Offset: " + offset);
                    break;
                }
            }
        }
    }
}