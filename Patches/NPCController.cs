﻿using NikoArchipelago.Archipelago;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikoArchipelago.Patches;

public class NpcController : MonoBehaviour
{
    private string currentScene;
    public GameObject NpcGameObject;
    private string[] hairball, turbine, salmon, pool, bathhouse, tadpole;
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        hairball = ["Britney", "frogFloaty", "frogSmallTalk", "trainFrog", 
            "officeCat", "hbcNervousFrog", "hbcToughFrog", "hbcImpatientFrog", "VlogFrog", "Scarefrog",
            "hbcToffee", "hbcJiji", "hbcMaggie", "hbcMinoes", "breakFrog", "hbcSimon", "brooklynFrog"];
        turbine = ["", ""];
        salmon = ["", ""];
        pool = ["", ""];
        bathhouse = ["", ""];
        tadpole = ["", ""];
    }

    private void Update()
    {
        switch (currentScene)
        {
            case "Hairball City":
                NpcGameObject.SetActive(ArchipelagoClient.HcNPCs);
                break;
            case "Trash Kingdom":
                NpcGameObject.SetActive(ArchipelagoClient.TtNPCs);
                break;
            case "Salmon Creek Forest":
                NpcGameObject.SetActive(ArchipelagoClient.SfcNPCs);
                break;
            case "Public Pool":
                NpcGameObject.SetActive(ArchipelagoClient.PpNPCs);
                break;
            case "The Bathhouse":
                NpcGameObject.SetActive(ArchipelagoClient.BathNPCs);
                break;
            case "Tadpole inc":
                NpcGameObject.SetActive(ArchipelagoClient.HqNPCs);
                break;
        } 
    }
}