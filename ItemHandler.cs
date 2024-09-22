using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using Newtonsoft.Json;
using NikoArchipelago.Archipelago;
using UnityEngine;
using ArchipelagoClient = NikoArchipelago.Archipelago.ArchipelagoClient;

namespace NikoArchipelago;

public static class ItemHandler
{
    private static ArchipelagoClient archipelagoClient;
    private static Plugin plugin;
    
    private static string jsonFilePath = Path.Combine(Paths.PluginPath, "APTotalCount.json");

    // Structure to hold the data in JSON
    public class CoinData
    {
        public int AddCoinCallCount { get; set; } = 0;
    }

    // Method to load CoinData from JSON
    public static CoinData LoadCoinData()
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonData = File.ReadAllText(jsonFilePath);
            return JsonConvert.DeserializeObject<CoinData>(jsonData);
        }
        return new CoinData();  // If no file exists, return a new CoinData object
    }

    // Method to save CoinData to JSON
    public static void SaveCoinData(CoinData coinData)
    {
        string jsonData = JsonConvert.SerializeObject(coinData, Formatting.Indented);
        File.WriteAllText(jsonFilePath, jsonData);
    }

    public static void AddCoin(int amount = 1, string sender = "")
    {
        scrGameSaveManager.instance.gameData.generalGameData.coinAmount += amount;
        Plugin.APSendNote($"Received Coin from {sender}!", 4f);
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddCassette(int amount = 1, string sender = "")
    {
        scrGameSaveManager.instance.gameData.generalGameData.cassetteAmount += amount;
        Plugin.APSendNote($"Received Cassette from {sender}!", 4f);
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddKey(int amount = 1, string sender = "")
    {
        scrGameSaveManager.instance.gameData.generalGameData.keyAmount += amount;
        Plugin.APSendNote($"Received Key from {sender}!", 4f);
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddLetter(int amount = 1, string sender = "")
    {
        scrGameSaveManager.instance.gameData.generalGameData.bottles += amount;
        Plugin.APSendNote($"Received Letter from {sender}!", 3f);
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddApples(int amount = 25, string sender = "")
    {
        scrGameSaveManager.instance.gameData.generalGameData.appleAmount += amount;
        Plugin.APSendNote($"Received {amount} Apples from {sender}!", 3f);
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddSuperJump(string sender = "")
    {
        scrGameSaveManager.instance.gameData.generalGameData.secretMove = true;
        Plugin.APSendNote($"Received Super Jump from {sender}!", 4f);
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddContactList1(string sender = "")
    {
        scrGameSaveManager.instance.gameData.generalGameData.wave1 = true;
        Plugin.APSendNote($"Received Contact List 1 from {sender}!", 4f);
        scrGameSaveManager.instance.SaveGame();
    }

    public static void AddContactList2(string sender = "")
    {
        scrGameSaveManager.instance.gameData.generalGameData.wave2 = true;
        Plugin.APSendNote($"Received Contact List 2 from {sender}!", 4f);
        scrGameSaveManager.instance.SaveGame();
    }

    /// <summary>
    /// Unlocks the specified level
    /// </summary>
    /// <param name="level">The level in question 1=Home etc.</param>
    /// <param name="sender">The Name of the sender</param>
    public static void AddTicket(int level, string sender)
    {
        scrGameSaveManager.instance.gameData.generalGameData.unlockedLevels[level] = true;
        switch (level)
        {
            case 2:
                Plugin.APSendNote($"Received Hairball City Ticket from {sender}!", 4f);
                break;
            case 3:
                Plugin.APSendNote($"Received Turbine Town Ticket from {sender}!", 4f);
                break;
            case 4:
                Plugin.APSendNote($"Received Salmon Creek Forest Ticket from {sender}!", 4f);
                break;
            case 5:
                Plugin.APSendNote($"Received Public Pool Ticket from {sender}!", 4f);
                break;
            case 6:
                Plugin.APSendNote($"Received Bathhouse Ticket from {sender}!", 4f);
                break;
            case 7:
                Plugin.APSendNote($"Received Tadpole HQ Ticket from {sender}!", 4f);
                break;
        }
        scrGameSaveManager.instance.SaveGame();
    }
}