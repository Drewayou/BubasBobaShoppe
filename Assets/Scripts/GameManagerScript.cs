using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Runtime.CompilerServices;

public class NewGameOverwrite : MonoBehaviour
{

    //Set all JSON to the default base.
    private PlayerDataJson PlayerStats = new PlayerDataJson();
    private WorldState world1 = new WorldState();
    private WorldState world2 = new WorldState();
    private WorldState world3 = new WorldState();
    private WorldState world4 = new WorldState();

    private CassavaBossLevelJson cassavaBossLevel = new CassavaBossLevelJson();
    private Inter_Dataservice DataService = new JsonDataService();

    //Set all JSON to the default base.
    private PlayerDataJson PlayerStatsThisInstance = new PlayerDataJson();
    private WorldState worldInstance = new WorldState();

    private CassavaBossLevelJson cassavaBossLevelInstance = new CassavaBossLevelJson();

    string path = Application.persistentDataPath + "/alalaa";

    /// <summary>
    /// FIXME: will implement encryption in the future
    /// </summary>
    private bool EncryptionEnabled = false;
    
    /// <summary>
    /// StartNewGame erases the current save files on user's local disk and makes the bare bone ones using the set JSON scripts
    /// </summary>
    public void StartNewGame(){

        ClearData();
        PrepareDefaultWorldSettings();
        

        if (File.Exists(path + "/PlayerStats.json"))
        {
            try
            {
                DataService.SaveData("/PlayerStats.json", PlayerStats, EncryptionEnabled);
                DataService.SaveData("/World1Stats.json", world1, EncryptionEnabled);
                DataService.SaveData("/World2Stats.json", world2, EncryptionEnabled);
                DataService.SaveData("/World3Stats.json", world3, EncryptionEnabled);
                DataService.SaveData("/World4Stats.json", world4, EncryptionEnabled);
                DataService.SaveData("/CassavaCastleStats.json", cassavaBossLevel, EncryptionEnabled);
            }
            catch (Exception e)
            {
                Debug.LogError($"Couldn't find files! Game Dev has yet to understand why! (Sorry :<) \n Error:" + e);
            }
        }
        else
        {
            Debug.LogError("Could not start game! Game Dev has yet to understand why! (Sorry :<)");
        }
    }

    public void PrepareDefaultWorldSettings(){

        world1.WorldName = "PandanForest";
        world1.SpawnersAlive = 3;
        world1.LevelDifficulty = 1;
        world1.chanceOfSlime = 1f;
        world1.chanceOfPandan = 0f;
        world1.chanceOfBanana = 0f;
        world1.chanceOfStrawberry = 0f;
        world1.chanceOfMango = 0f;
        world1.chanceOfUbe = 0f;

        world1.WorldName = "PricklyDesert";
        world1.SpawnersAlive = 3;
        world1.LevelDifficulty = 1;
        world1.chanceOfSlime = .25f;
        world1.chanceOfPandan = 0f;
        world1.chanceOfBanana = .25f;
        world1.chanceOfStrawberry = .25f;
        world1.chanceOfMango = 0f;
        world1.chanceOfUbe = 0f;

        world1.WorldName = "MarshMellows";
        world1.SpawnersAlive = 3;
        world1.LevelDifficulty = 1;
        world1.chanceOfSlime = 0f;
        world1.chanceOfPandan = .20f;
        world1.chanceOfBanana = .30f;
        world1.chanceOfStrawberry = 0f;
        world1.chanceOfMango = .50f;
        world1.chanceOfUbe = 0f;

        world1.WorldName = "PandanForest";
        world1.SpawnersAlive = 3;
        world1.LevelDifficulty = 1;
        world1.chanceOfSlime = .30f;
        world1.chanceOfPandan = .25f;
        world1.chanceOfBanana = 0f;
        world1.chanceOfStrawberry = .20f;
        world1.chanceOfMango = 0f;
        world1.chanceOfUbe = .20f;
    }

    public void ContinueGame(){
        if (File.Exists(path + "/PlayerStats.json"))
        {
            try
            {
                PlayerStatsThisInstance = DataService.LoadData<PlayerDataJson>("/PlayerStats.json", EncryptionEnabled);
                world1 = DataService.LoadData<WorldState>("/World1json", EncryptionEnabled);
                world2 = DataService.LoadData<WorldState>("/World2Stats.json", EncryptionEnabled);
                world3 = DataService.LoadData<WorldState>("/World3Stats.json", EncryptionEnabled);
                world4 = DataService.LoadData<WorldState>("/World4Stats.json", EncryptionEnabled);
                cassavaBossLevelInstance = DataService.LoadData<CassavaBossLevelJson>("/CassavaCastleStats.json", EncryptionEnabled);
            }
            catch (Exception e)
            {
                Debug.LogError($"Couldn't find directory! Game Dev has yet to understand why! (Sorry :<) \n Error:" + e);
            }
        }
        else
        {
            Debug.LogError("Could not save file! Game Dev has yet to understand why! (Sorry :<)");
        }
    }

    public void SaveGame(){
         if (File.Exists(path + "/PlayerStats.json"))
        {
            try
            {
                DataService.SaveData("/PlayerStats.json", PlayerStatsThisInstance, EncryptionEnabled);
                DataService.SaveData("/World1Stats.json", world1, EncryptionEnabled);
                DataService.SaveData("/World2Stats.json", world2, EncryptionEnabled);
                DataService.SaveData("/World3Stats.json", world3, EncryptionEnabled);
                DataService.SaveData("/World4Stats.json", world4, EncryptionEnabled);
                DataService.SaveData("/CassavaCastleStats.json", cassavaBossLevelInstance, EncryptionEnabled);
            }
            catch (Exception e)
            {
                Debug.LogError($"Couldn't find files! Game Dev has yet to understand why! (Sorry :<) \n Error:" + e);
            }
        }
        else
        {
            Debug.LogError("Could not start game! Game Dev has yet to understand why! (Sorry :<)");
        }
    }

    public void ClearData()
    {
        Debug.LogWarning("Checking if files exist...");
        string path = Application.persistentDataPath + "/alalaa";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.LogWarning("Files found! Eating them up! OHMNOMNOMNOM They're gone now teehee :3");
        }
    }

    public PlayerDataJson ReturnStatsOfThisPlayer(){
        return PlayerStatsThisInstance;
    }

    public WorldState ReturnWorldSelected(string worldStateName){
        switch(worldStateName){
            case "PandanForest":
            return world1;

            case "PricklyDesert":
            return world2;

            case "MarshMellows":
            return world3;
    
            case "CrystalLeyCaves":
            return world4;
        }
        return world1;
    }

    public void SetBossWorldState(int EndingLevel){
        switch(EndingLevel){
            case 1:
                cassavaBossLevelInstance.bossHP=30;
                cassavaBossLevelInstance.LevelDifficulty=1;
                break;
            case 2:
                cassavaBossLevelInstance.bossHP=250;
                cassavaBossLevelInstance.LevelDifficulty=2;
                break;
            case 3:
                cassavaBossLevelInstance.bossHP=500;
                cassavaBossLevelInstance.LevelDifficulty=3;
                break;
        }
    }

    public CassavaBossLevelJson ReturnStateOfCassavaBossCastle(){
        return cassavaBossLevelInstance;
    }

    public void updateHPStatsOfThisPlayer(int additionalHP){
        PlayerStatsThisInstance.playerMaxHealth += additionalHP;
    }

    public void updateStaminaStatsOfThisPlayer(float additionalStamina){
        PlayerStatsThisInstance.playerMaxStamina += additionalStamina;
    }

    public void updateAttackStatsOfThisPlayer(int additionalAttack){
        PlayerStatsThisInstance.playerAttackPoints += additionalAttack;
    }

}
