using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.PlayerLoop;
using System.Threading;
using JetBrains.Annotations;

public class GameManagerScript : MonoBehaviour
{

    [SerializeField]
    [Header("Intro Video Object")]
    [Tooltip("The Intro video if this is a new game")]
    //Drag the intro video object here
    GameObject introContainer;

    [SerializeField]
    [Header("Intro Music Prefab Object")]
    [Tooltip("The Intro music if this is a new game")]
    //Drag the intro video object here
    GameObject introMusic;

    [SerializeField]
    [Header("Main Menu Music")]
    [Tooltip("Place Main Menu Music Here ")]
    //Drag the intro video object here
    GameObject mainMenuMusic;

    //FIXME: Public Temp var for each world.
    [SerializeField]
    [Header("Which world is this?")]
    [Tooltip("Set this to the number of what world this manager is attached to!")]
    public int worldOfRound;

    //Set all JSON to the default base.
    private PlayerDataJson playerStats = new PlayerDataJson();

    private ShopCostsNEarnings ShopData = new ShopCostsNEarnings();
    private WorldState world1 = new WorldState();
    private WorldState world2 = new WorldState();
    private WorldState world3 = new WorldState();
    private WorldState world4 = new WorldState();

    private DrinkMultiplierScripts drinkMultiplier = new DrinkMultiplierScripts();

     private DrinkMultiplierScripts drinkMultiplierInstance;

    private WorldState cassavaBossLevel = new WorldState();
    private Inter_Dataservice DataService = new JsonDataService();

    //Set all JSON to the default base.
    private PlayerDataJson PlayerStatsThisInstance = new PlayerDataJson();
    private ShopCostsNEarnings thisInstanceOfPlayerShop = new ShopCostsNEarnings();
    private WorldState worldInstance = new WorldState();

    private RoundManagerScript roundConnected;

    //FIXME: TempVars for the MAXIMUM stats a player can grind to in this game
    private int MaxHealthEverThisGame = 100, MaxStaminaEverThisGame = 20, MaxAttackEverThisGame = 20;

    /// <summary>
    /// FIXME: will implement encryption in the future
    /// </summary>
    private bool EncryptionEnabled = false;

    //Set which world is this from current instance and saves
    void Start(){

        //Load the game save from files
        ContinueGame();

        if(mainMenuMusic !=null){
        if(GameObject.Find("MainMenuMusic") == null && GameObject.Find("IntroMusic") == null){
                Instantiate(mainMenuMusic);
            }
        }

        //Start playing the intro if this is a new game save, else make sure it's off
        if(introContainer != null){
            if(PlayerStatsThisInstance.newGame){
                introContainer.SetActive(true);
                if(GameObject.FindWithTag("IntroMusic") == null){
                    Instantiate(introMusic);
                    Destroy(GameObject.Find("MainMenuMusic(Clone)"));
                }
            }else{
                introContainer.SetActive(false);
            }
        }

    }

    void Update(){
    }
    
    /// <summary>
    /// StartNewGame erases the current save files on user's local disk and makes the bare bone ones using the set JSON scripts
    /// </summary>
    public void StartNewGame(){

        Debug.LogWarning("WARNING : Any player save will be overwritten!");
        ClearData();
        PrepareDefaultWorldSettings();
        Debug.LogWarning("Saving Default World Settings...");

        string path = Application.persistentDataPath + "/alalaa";

        if (File.Exists(path + "/PlayerStats.json")){
            Debug.LogWarning("WARNING: Other save files found! Overwriting files...");
        }else{
            Directory.CreateDirectory(path);
            Debug.LogWarning("No other saves found! Creating directory for saves...");
        }

        StartCoroutine(waitForDirectoryCreation(3.5f));
        
        try
        {
            DataService.SaveData("/alalaa/PlayerStats.json", playerStats, EncryptionEnabled);
            DataService.SaveData("/alalaa/PlayerShopData.json", ShopData, EncryptionEnabled);
            DataService.SaveData("/alalaa/World1Stats.json", world1, EncryptionEnabled);
            DataService.SaveData("/alalaa/World2Stats.json", world2, EncryptionEnabled);
            DataService.SaveData("/alalaa/World3Stats.json", world3, EncryptionEnabled);
            DataService.SaveData("/alalaa/World4Stats.json", world4, EncryptionEnabled);
            DataService.SaveData("/alalaa/CassavaCastleStats.json", cassavaBossLevel, EncryptionEnabled);
            DataService.SaveData("/alalaa/DrinkMultiplier.json", drinkMultiplier, EncryptionEnabled);

        }
        catch (Exception e)
        {
            Debug.LogError($"Set new game find files FAILED! Game Dev has yet to understand why! (Sorry :<) \n Error:" + e);
        }
        
        //FIXME: The world will always be set to world1 EVRYTIME you start a new game
        //This may be useful if you ever make a tutorial!
        Debug.LogWarning("World Default Settings Set! \n Starting New Game...");
    
    }

    IEnumerator waitForDirectoryCreation(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
    }

    public void PrepareDefaultWorldSettings(){

        Debug.LogWarning("Setting Default World Settings...");

        //Player default settings
        playerStats.newGame = true;
        playerStats.onDayNumber = 0;
        playerStats.playerMaxHealth = 15;
        playerStats.playerMaxStamina = 10f;
        playerStats.playerAttackPoints = 5;

        //Shop default settings
        ShopData.shopLevelAt = 1;
        ShopData.playerShopDrinkSellAmmount = 1.0f;
        ShopData.shopLevelUpCost = 10;
        ShopData.shopPlayerHPUpCost = 10;
        ShopData.shopPlayerAttackUpCost = 10;
        ShopData.shopPlayerStaminaUpCost = 10;
        ShopData.world2Cost = 25;
        ShopData.world3Cost = 50;
        ShopData.world4Cost = 75;

        //Basic world settings
        world1.WorldName = "PandanForest";
        world1.SpawnersAlive = 3;
        world1.LevelDifficulty = 1;
        world1.chanceOfSlime = 1.0f;
        world1.chanceOfPandan = 0f;
        world1.chanceOfBanana = 0f;
        world1.chanceOfStrawberry = 0f;
        world1.chanceOfMango = 0f;
        world1.chanceOfUbe = 0f;

        world2.WorldName = "PricklyDesert";
        world2.SpawnersAlive = 3;
        world2.LevelDifficulty = 1;
        world2.chanceOfSlime = .25f;
        world2.chanceOfPandan = 0f;
        world2.chanceOfBanana = .25f;
        world2.chanceOfStrawberry = .25f;
        world2.chanceOfMango = 0f;
        world2.chanceOfUbe = 0f;

        world3.WorldName = "MarshMellows";
        world3.SpawnersAlive = 3;
        world3.LevelDifficulty = 1;
        world3.chanceOfSlime = 0f;
        world3.chanceOfPandan = .20f;
        world3.chanceOfBanana = .30f;
        world3.chanceOfStrawberry = 0f;
        world3.chanceOfMango = .50f;
        world3.chanceOfUbe = 0f;

        world4.WorldName = "PandanForest";
        world4.SpawnersAlive = 3;
        world4.LevelDifficulty = 1;
        world4.chanceOfSlime = .30f;
        world4.chanceOfPandan = .25f;
        world4.chanceOfBanana = 0f;
        world4.chanceOfStrawberry = .20f;
        world4.chanceOfMango = 0f;
        world4.chanceOfUbe = .20f;

        //Defualt Drink multiplier
        drinkMultiplier.OolongMultiplier = 1.00f;
        drinkMultiplier.PandanMultiplier = 1.50f;
        drinkMultiplier.BananaMultiplier = 2.00f;
        drinkMultiplier.StrawberryMultiplier = 2.00f;
        drinkMultiplier.MangoMultiplier = 3.00f;
        drinkMultiplier.UbeMultiplier = 3.00f;

        //Defualt Basic boss level
        cassavaBossLevel.WorldName = "CassavaCastle";
        cassavaBossLevel.isThereABoss = true;
    }

    public void ContinueGame(){
        string path = Application.persistentDataPath + "/alalaa";
        if (File.Exists(path + "/PlayerStats.json"))
        {
            try
            {
                PlayerStatsThisInstance = DataService.LoadData<PlayerDataJson>("/alalaa/PlayerStats.json", EncryptionEnabled);
                thisInstanceOfPlayerShop = DataService.LoadData<ShopCostsNEarnings>("/alalaa/PlayerShopData.json", EncryptionEnabled);
                //FIXME: Unsure if BOC below should even be called
                
                world1 = DataService.LoadData<WorldState>("/alalaa/World1Stats.json", EncryptionEnabled);
                world2 = DataService.LoadData<WorldState>("/alalaa/World2Stats.json", EncryptionEnabled);
                world3 = DataService.LoadData<WorldState>("/alalaa/World3Stats.json", EncryptionEnabled);
                world4 = DataService.LoadData<WorldState>("/alalaa/World4Stats.json", EncryptionEnabled);
                cassavaBossLevel = DataService.LoadData<WorldState>("/alalaa/CassavaCastleStats.json", EncryptionEnabled);
                
                drinkMultiplierInstance = DataService.LoadData<DrinkMultiplierScripts>("/alalaa/DrinkMultiplier.json", EncryptionEnabled);
            }
            catch (Exception e)
            {
                Debug.LogError($"Couldn't find directory! Game Dev has yet to understand why! (Sorry :<) \n Error:" + e);
            }
            //Set this world instance
            worldInstance = DataService.LoadData<WorldState>("/alalaa" + ReturnThisRoundInstanceWorldPath(ReturnWorldSelectedViaIntRoundVal()), EncryptionEnabled);
        }
        else
        {
            Debug.LogError("Could not save file! Game Dev has yet to understand why! (Sorry :<)");
        }
    }

    //FIXME: This save game method saves the TOTALITY of the game. For use with a future save icon.
    public void SaveGame(){
        string path = Application.persistentDataPath + "/alalaa";
        if (File.Exists(path + "/PlayerStats.json"))
        {
            try
            {
                //FIXME: Unsure if code below should even be called
                /*
                DataService.SaveData("/alalaa/PlayerStats.json", PlayerStatsThisInstance, EncryptionEnabled);
                DataService.SaveData("/alalaa/World1Stats.json", world1, EncryptionEnabled);
                DataService.SaveData("/alalaa/World2Stats.json", world2, EncryptionEnabled);
                DataService.SaveData("/alalaa/World3Stats.json", world3, EncryptionEnabled);
                DataService.SaveData("/alalaa/World4Stats.json", world4, EncryptionEnabled);
                DataService.SaveData("/alalaa/CassavaCastleStats.json", cassavaBossLevel, EncryptionEnabled);
                DataService.SaveData("/alalaa/DrinkMultiplier.json", drinkMultiplier, EncryptionEnabled);
                */
            }
            catch (Exception e)
            {
                Debug.LogError($"Couldn't find files! Game Dev has yet to understand why! (Sorry :<) \n Error:" + e);
            }
        }
        else
        {
            Debug.LogError("Could not save game! Files cannot be found! Game Dev has yet to understand why! (Sorry :<)");
        }
    }

    //FIXME: This save game method saves the game at the end of the DAY/(ROUND) of each game.
    public void SaveGameAfterRound(WorldState worldSelected){
        string path = Application.persistentDataPath + "/alalaa";
        if (File.Exists(path + "/PlayerStats.json"))
        {
            try
            {
                SetNewWorldSpawnRatesIncreased();
                SetNewWorldSpawnRatesDecreased();
                

                //thisTestState.chanceOfSlime = 1000f;
                DataService.SaveData("/alalaa/PlayerStats.json", PlayerStatsThisInstance, EncryptionEnabled);
                DataService.SaveData("/alalaa" + ReturnThisRoundInstanceWorldPath(worldSelected), worldInstance, EncryptionEnabled);
                DataService.SaveData("/alalaa/CassavaCastleStats.json", cassavaBossLevel, EncryptionEnabled);
                DataService.SaveData("/alalaa/DrinkMultiplier.json", drinkMultiplierInstance, EncryptionEnabled);
            }
            catch (Exception e)
            {
                Debug.LogError($"Couldn't find files! Game Dev has yet to understand why! (Sorry :<) \n Error:" + e);
            }
        }
        else
        {
            Debug.LogError("Could not save game! Files cannot be found! Game Dev has yet to understand why! (Sorry :<)");
        }
    }

    //Call this save update after the player sucessfully purchases something from the shop
    public void SaveShopAndPlayerDataAfterPurchase(ShopCostsNEarnings anInstanceOfShop, PlayerDataJson anInstanceOfPlayer){
        string path = Application.persistentDataPath + "/alalaa";
        if (File.Exists(path + "/PlayerShopData.json"))
        {
            try
            {
                DataService.SaveData("/alalaa/PlayerShopData.json", anInstanceOfShop, EncryptionEnabled);
                DataService.SaveData("/alalaa/PlayerStats.json", anInstanceOfPlayer, EncryptionEnabled);
            }
            catch (Exception e)
            {
                Debug.LogError($"Couldn't find files! Game Dev has yet to understand why! (Sorry :<) \n Error:" + e);
            }
        }
        else
        {
            Debug.LogError("Could not save game! Files cannot be found! Game Dev has yet to understand why! (Sorry :<)");
        }
    }

    

    public void ClearData()
    {
        Debug.LogWarning("Checking if files exist...");
        string path = Application.persistentDataPath + "/alalaa";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.LogWarning("Files found! Eating them up! OHMNOMNOMNOM They're gone now, bye bye saves teehee :3");
        }
        Debug.LogWarning("No Files found... OHH Is this a new game?!");
    }

    //BELOW BOC picks a random fruit monster to increase spawnrates of
    public void SetNewWorldSpawnRatesIncreased(){

        int selectedSpawnToIncreaseSpawn = UnityEngine.Random.Range(0,5);

        switch(selectedSpawnToIncreaseSpawn){
            case 0:
            if(worldInstance.chanceOfSlime < 0.90f){worldInstance.chanceOfSlime +=.10f;}
            else{SetNewWorldSpawnRatesIncreased();}
            break;
            
            case 1:
            if(worldInstance.chanceOfPandan < 0.90f){worldInstance.chanceOfPandan +=.10f;}
            else{SetNewWorldSpawnRatesIncreased();}
            break;

            case 2:
            if(worldInstance.chanceOfBanana < 0.90f){worldInstance.chanceOfBanana +=.10f;}
            else{SetNewWorldSpawnRatesIncreased();}
            break;
        
            case 3:
            if(worldInstance.chanceOfStrawberry < 0.90f){worldInstance.chanceOfStrawberry +=.10f;}
            else{SetNewWorldSpawnRatesIncreased();}
            break;

            case 4:
            if(worldInstance.chanceOfMango < 0.90f){worldInstance.chanceOfMango +=.10f;}
            else{SetNewWorldSpawnRatesIncreased();}
            break;

            case 5:
            if(worldInstance.chanceOfUbe < 0.90f){worldInstance.chanceOfUbe +=.10f;}
            else{SetNewWorldSpawnRatesIncreased();}
            break;
            }
        
    }

    //BELOW BOC picks and chooses another to decrease spawnrates of.
    public void SetNewWorldSpawnRatesDecreased(){

        if(worldInstance.chanceOfSlime + worldInstance.chanceOfPandan + worldInstance.chanceOfBanana + worldInstance.chanceOfStrawberry + worldInstance.chanceOfMango + worldInstance.chanceOfUbe > .99f){
        int selectedSpawnToDecreaseSpawn = UnityEngine.Random.Range(0,5);

        switch(selectedSpawnToDecreaseSpawn){
            case 0:
            if(worldInstance.chanceOfSlime > 0.10f){worldInstance.chanceOfSlime -=.10f;
            SetNewWorldSpawnRatesDecreased();}
            break;
            
            case 1:
            if(worldInstance.chanceOfPandan > 0.10f){worldInstance.chanceOfPandan -=.10f;
            SetNewWorldSpawnRatesDecreased();}
            break;

            case 2:
            if(worldInstance.chanceOfBanana > 0.10f){worldInstance.chanceOfBanana -=.10f;
            SetNewWorldSpawnRatesDecreased();}
            break;
        
            case 3:
            if(worldInstance.chanceOfStrawberry > 0.10f){worldInstance.chanceOfStrawberry -=.10f;
            SetNewWorldSpawnRatesDecreased();}
            break;

            case 4:
            if(worldInstance.chanceOfMango > 0.10f){worldInstance.chanceOfMango -=.10f;
            SetNewWorldSpawnRatesDecreased();}
            break;

            case 5:
            if(worldInstance.chanceOfUbe > 0.10f){worldInstance.chanceOfUbe -=.10f;
            SetNewWorldSpawnRatesDecreased();}
            break;
            }
        }

    }

    public void SetNewDrinkDemandRates(DrinkMultiplierScripts drinkMultiplier){

        int selectedDrinkDemandToIncreaseSpawn = UnityEngine.Random.Range(0,5);
        int selectedDrinkDemandToDecreaseSpawn = UnityEngine.Random.Range(0,5);

        switch(selectedDrinkDemandToIncreaseSpawn){
            case 0:
            drinkMultiplierInstance.OolongMultiplier -= 0.10f;
            break;

            case 1:
            drinkMultiplierInstance.PandanMultiplier -= 0.10f;
            break;

            case 2:
            drinkMultiplierInstance.BananaMultiplier -= 0.10f;
            break;
    
            case 3:
            drinkMultiplierInstance.StrawberryMultiplier -= 0.10f;
            break;

            case 4:
            drinkMultiplierInstance.MangoMultiplier -= 0.10f;
            break;

            case 5:
            drinkMultiplierInstance.UbeMultiplier -= 0.10f;
            break;
        }

        switch(selectedDrinkDemandToDecreaseSpawn){
            case 0:
            drinkMultiplierInstance.OolongMultiplier += 0.10f;
            break;

            case 1:
            drinkMultiplierInstance.PandanMultiplier += 0.10f;
            break;

            case 2:
            drinkMultiplierInstance.BananaMultiplier += 0.10f;
            break;
    
            case 3:
            drinkMultiplierInstance.StrawberryMultiplier += 0.10f;
            break;

            case 4:
            drinkMultiplierInstance.MangoMultiplier += 0.10f;
            break;

            case 5:
            drinkMultiplierInstance.UbeMultiplier += 0.10f;
            break;
        }
    }

    public PlayerDataJson ReturnStatsOfThisPlayer(){
        return PlayerStatsThisInstance;
    }

    public WorldState ReturnWorldStateSelected(WorldState worldSelected){
        
        switch(worldSelected.WorldName){
            case "PandanForest":
            return world1;

            case "PricklyDesert":
            return world2;

            case "MarshMellows":
            return world3;
    
            case "CrystalLeyCaves":
            return world4;

            case "CassavaCastle":
            return cassavaBossLevel;

            default:
            return null;
        }
    }

    public WorldState ReturnWorldSelectedViaIntRoundVal(){
        
        switch(worldOfRound){
            case 1:
            return world1;

            case 2:
            return world2;

            case 3:
            return world3;
    
            case 4:
            return world4;

            case 5:
            return cassavaBossLevel;

            default:
            return null;
        }
    }

    public void WhichWorldIsThis(int whichWorldIsThisSetting){
        switch(whichWorldIsThisSetting){
            case 1:
            setWorldInstanceToWorld1();
            break;

            case 2:
            setWorldInstanceToWorld2();
            break;

            case 3:
            setWorldInstanceToWorld3();
            break;

            case 4:
            setWorldInstanceToWorld4();
            break;
        }  
    }

    public void setWorldInstanceToWorld1(){
        worldInstance = world1;
    }

    public void setWorldInstanceToWorld2(){
        worldInstance = world2;
    }

    public void setWorldInstanceToWorld3(){
        worldInstance = world3;
    }

    public void setWorldInstanceToWorld4(){
        worldInstance = world4;
    }

    public void setWorldInstanceToCassavaSlimeBoss(){
        worldInstance = cassavaBossLevel;
    }

    public WorldState ReturnGameManagerWorldInstance(){
        return worldInstance;
    }

    public string ReturnThisRoundInstanceWorldPath(WorldState worldSelected){
        switch(worldSelected.WorldName){
            case "PandanForest":
            return "/World1Stats.json";

            case "PricklyDesert":
            return "/World2Stats.json";

            case "MarshMellows":
            return "/World3Stats.json";
    
            case "CrystalLeyCaves":
            return "/World4Stats.json";

            case "CassavaCastle":
            return "/CassavaCastleStats.json";

            default:
            return "/World1Stats.json";
        }
    }

    public void SetBossWorldState(int EndingLevel){
        switch(EndingLevel){
            case 1:
                worldInstance.bossHP=30;
                worldInstance.LevelDifficulty=1;
                break;
            case 2:
                worldInstance.bossHP=250;
                worldInstance.LevelDifficulty=2;
                break;
            case 3:
                worldInstance.bossHP=500;
                worldInstance.LevelDifficulty=3;
                break;
        }
    }

    public WorldState ReturnStateOfCassavaBossCastle(){
        return cassavaBossLevel;
    }

    public void UpdateHPStatsOfThisPlayer(int additionalHP){
        PlayerStatsThisInstance.playerMaxHealth += additionalHP;
    }

    public void UpdateStaminaStatsOfThisPlayer(float additionalStamina){
        PlayerStatsThisInstance.playerMaxStamina += additionalStamina;
    }

    public void UpdateAttackStatsOfThisPlayer(int additionalAttack){
        PlayerStatsThisInstance.playerAttackPoints += additionalAttack;
    }

    public int ReturnPlayerCoinStats(){
        return (int)PlayerStatsThisInstance.playerCoins;
    }

    public void UpdatePlayerCoinStats(int additionalGoldCoins){
        PlayerStatsThisInstance.playerCoins += additionalGoldCoins;
    }

    public float ReturnPlayerShopCoinMultiplier(){
        return thisInstanceOfPlayerShop.playerShopDrinkSellAmmount;
    }

    public void increasePlayerShopCoinMultiplier(){
        thisInstanceOfPlayerShop.playerShopDrinkSellAmmount += .10f;
    }

    public void setPlayerAndBossFight1Available(){
        playerStats.unlockedBossFight = true;
    }

    public ShopCostsNEarnings ReturnCurrentShopInstance(){
        return thisInstanceOfPlayerShop;
    }
    
    public DrinkMultiplierScripts ReturnDrinkRatesThisRound(){
        return drinkMultiplierInstance;
    }

    public PlayerDataJson ReturnPlayerStats(){
        return PlayerStatsThisInstance;
    }

    public int ReturnMaxHealthStasThisGameCanHandle(){
        return MaxHealthEverThisGame;
    }
    public int ReturnMaxStaminaStasThisGameCanHandle(){
        return MaxStaminaEverThisGame;
    }
    public int ReturnMaxAttackStasThisGameCanHandle(){
        return MaxAttackEverThisGame;
    }

    public bool CheckingIfSavesExist(){
        string path = Application.persistentDataPath + "/alalaa";
        if (File.Exists(path + "/PlayerStats.json")){return true;}
        else{return false;};
    }

    public void doneWithNewGameIntro(){
        introContainer.SetActive(false);
        PlayerStatsThisInstance.newGame = false;
        Destroy(GameObject.Find("IntroMusic(Clone)"));
        Instantiate(mainMenuMusic);
        SaveShopAndPlayerDataAfterPurchase(thisInstanceOfPlayerShop,PlayerStatsThisInstance);
    }
}
