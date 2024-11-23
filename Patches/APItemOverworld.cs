using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net.Enums;
using HarmonyLib;
using KinematicCharacterController.Core;
using NikoArchipelago.Archipelago;
using NikoArchipelago.Stuff;
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
        private static GameObject CreateItemPrefab(string prefabName, scrCassette instance)
        {
            // Load prefab
            GameObject prefab = Plugin.AssetBundle.LoadAsset<GameObject>(prefabName);
            if (prefab == null)
            {
                Plugin.BepinLogger.LogError($"Prefab '{prefabName}' not found in AssetBundle.");
                return null;
            }
            GameObject itemOverworld = Object.Instantiate(prefab);
            var ogQuads = instance.transform.Find("Quads").gameObject;
            var itemQuads = itemOverworld.transform.Find("Quads").gameObject;

            itemQuads.transform.position = ogQuads.transform.position;
            itemQuads.AddComponent<ScuffedSpin>();
            itemQuads.transform.SetParent(instance.transform);

            return itemQuads;
        }

        private static GameObject CreateItemOverworld(string itemName, scrCassette instance)
        {
            // Map item names to prefab names.
            var prefabMap = new Dictionary<string, string>
            {
                { "apProg", "APProgressive" },
                { "apUseful", "APUseful" },
                { "apFiller", "APFiller" },
                { "apTrap", "APTrap" },
                { "apTrap1", "APTrap1" },
                { "apTrap2", "APTrap2" },
                { "coin", "Coin" },
                { "cassette", "Cassette" },
                { "key", "Key" },
                { "contactList", "ContactList" },
                { "apples", "Apples" },
                { "snailMoney", "SnailMoney" },
                { "letter", "Letter" },
                { "bugs", "Bugs" },
                { "hcfish", "HairballFish" },
                { "ttfish", "TurbineFish" },
                { "scffish", "SalmonFish" },
                { "ppfish", "PoolFish" },
                { "bathfish", "BathFish" },
                { "hqfish", "TadpoleFish" },
                { "superJump", "SuperJump" },
                { "hairballCity", "HairballCity" },
                { "turbineTown", "TurbineTown" },
                { "salmonCreekForest", "SalmonCreekForest" },
                { "publicPool", "PublicPool" },
                { "bathhouse", "Bathhouse" },
                { "tadpoleHQ", "TadpoleHQ" },
                { "garysGarden", "GarysGarden" },
            };
            if (prefabMap.TryGetValue(itemName, out string prefabName))
            {
                return CreateItemPrefab(prefabName, instance);
            }
            Plugin.BepinLogger.LogError($"Item name '{itemName}' not recognized.");
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

                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
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
                
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
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
                
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
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
                
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
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
                
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
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
                
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
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
                
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
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
         private static GameObject CreateItemPrefab(string prefabName, scrCoin instance)
        {
            // Load prefab
            GameObject prefab = Plugin.AssetBundle.LoadAsset<GameObject>(prefabName);
            if (prefab == null)
            {
                Plugin.BepinLogger.LogError($"Prefab '{prefabName}' not found in AssetBundle.");
                return null;
            }
            GameObject itemOverworld = Object.Instantiate(prefab);
            var ogQuads = instance.transform.Find("Quads").gameObject;
            var itemQuads = itemOverworld.transform.Find("Quads").gameObject;

            itemQuads.transform.position = ogQuads.transform.position;
            itemQuads.AddComponent<ScuffedSpin>();
            itemQuads.transform.SetParent(instance.transform);

            return itemQuads;
        }

        private static GameObject CreateItemOverworld(string itemName, scrCoin instance)
        {
            // Map item names to prefab names.
            var prefabMap = new Dictionary<string, string>
            {
                { "apProg", "APProgressive" },
                { "apUseful", "APUseful" },
                { "apFiller", "APFiller" },
                { "apTrap", "APTrap" },
                { "apTrap1", "APTrap1" },
                { "apTrap2", "APTrap2" },
                { "coin", "Coin" },
                { "cassette", "Cassette" },
                { "key", "Key" },
                { "contactList", "ContactList" },
                { "apples", "Apples" },
                { "snailMoney", "SnailMoney" },
                { "letter", "Letter" },
                { "bugs", "Bugs" },
                { "hcfish", "HairballFish" },
                { "ttfish", "TurbineFish" },
                { "scffish", "SalmonFish" },
                { "ppfish", "PoolFish" },
                { "bathfish", "BathFish" },
                { "hqfish", "TadpoleFish" },
                { "superJump", "SuperJump" },
                { "hairballCity", "HairballCity" },
                { "turbineTown", "TurbineTown" },
                { "salmonCreekForest", "SalmonCreekForest" },
                { "publicPool", "PublicPool" },
                { "bathhouse", "Bathhouse" },
                { "tadpoleHQ", "TadpoleHQ" },
                { "garysGarden", "GarysGarden" },
            };
            if (prefabMap.TryGetValue(itemName, out string prefabName))
            {
                return CreateItemPrefab(prefabName, instance);
            }
            Plugin.BepinLogger.LogError($"Item name '{itemName}' not recognized.");
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

                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
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
                
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
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
                
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
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
                
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
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
                
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
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
                    const int offset = 92;
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
                
                            Plugin.BepinLogger.LogInfo("Item: "+ArchipelagoClient.ScoutedLocations[index + offset].ItemName 
                                                               + "\nLocation: "+ ArchipelagoClient.ScoutedLocations[index + offset].LocationName 
                                                               + "\nLocationID: " + ArchipelagoClient.ScoutedLocations[index + offset].LocationId);
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