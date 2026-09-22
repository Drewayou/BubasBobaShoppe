using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using System;
using TMPro;

public class BobaShopRoundManagerScript : MonoBehaviour
{
    //FIXME: You need to change this round manager depending if it's a hunt, sell, or city round!
    
    //The Game's OverallManager Object to pull/put scripts from.
    [Header("GameManager")]
    [Tooltip("Put the game's overall Manager Object to end the game / check if paused / unpaused ")]
    [SerializeField]
    GameObject overallGameManager;

    //The Game's OverallManager SCRIPT to pull/put scripts and values from. Gets established automatically from above object.
    [Header("GameManagerSCRIPT")]
    [Tooltip("Pull Values from this script")]
    GameManagerScript thisGamesOverallInstanceScript;

    //Drink rate demands DrinkMultiplierScripts, which in turn is pulled from the GameManager.
    public DrinkMultiplierScripts whatDrinksArePopular;

    //The stats of this players shop is pulled from the GameManager.
    public ShopCostsNEarnings playersCurrentShopStats;

    //The stats of this players data is pulled from the GameManager.
    public PlayerDataJson playersCurrentDataStats;

    //Drag the Trays object that holds all the ingredient trays.
    [SerializeField]
    [Header("Trays")]
    [Tooltip("Put the game's \"Trays\" game object to access how much more ingredients the player has.")]
    public GameObject traysInTheShop;

    //The Money jar object so that the money can be calculated/visualized.
    [SerializeField]
    [Header("Waiting queue")]
    [Tooltip("Put the game's \"MoneyJar\" game object to access how much money is being generated and how the money animations occur.")]
    public MoneyJarScript thisRoundMoneyJar;

    //The Customer queue handler gameObject pulled from the Gameobjects and queue holders.
    [SerializeField]
    [Header("Waiting queue")]
    [Tooltip("Put the game's \"CustomerQueueHandler\" game object to access the customer queue of people waiting to make orders.")]
    public GameObject customerQueueHandlerGameObject;

    //The Customer queue handler scripts pulled from the Gameobjects and queue holders.
    [SerializeField]
    [Header("Waiting queue")]
    [Tooltip("Put the game's \"CustomerQueueHandler\" game object to access the customer queue of people waiting to make orders.")] 
    public CustomerHandlerScript customerQueueHandlerScript;

    [SerializeField]  
    [Header("Drink pending queue GAMEOBJECT")]
    [Tooltip("Put the game's \"CustomerDrinkWaitQueueHandler\" game object to access transform properties of this object.")] 
    public GameObject customerWaitingDrinkHandlerGameObject;

    [SerializeField]  
    [Header("Drink pending queue")]
    [Tooltip("Put the game's \"CustomerDrinkWaitQueueHandler\" game object to access the customer queue of people waiting for their drinks.")] 
    CustomerWaitingForDrinkHandlerScript customerWaitingDrinkHandlerScript;

    [SerializeField]
    [Header("Customer pending drink pickup script")]
    [Tooltip("Put the game's \"CustomerOrderPickupScript\" script to access the customer queue of people waiting for their drinks (also found in customerWaitingDrinkHandlerGameObject).")]
    CustomerOrderPickupScript customerOrderPickupHandlerScript;

    [SerializeField]
    [Header("Boba Sell Mat GAMEOBJECT")]
    [Tooltip("Put the game's \"DrinkPlacementMat\" game object to access transform and script properties of this object.")]
    public GameObject bobaDrinkMat;

    //The script attached to this game object that programs what customers can spawn.
    //Gets set via the Start() method.
    NPCCustomersThatCanSpawnScript customersThatCanSpawnThisRoundScript;

    //The Game's In-GameUI object to use / move during / after the game has ended.
    [SerializeField]
    [Header("In-GameUIObject")]
    [Tooltip("Put the game's In-GameUI to move for end of round animations and to disable this UI")] 
    public GameObject inGameUIObject;

    //The above's game animator.
    private Animator inGameUIAnimator;

    //The Game's End-Of-RoundUI object to use / move during / after the game has ended.
    [SerializeField]
    [Header("Endof-RoundUIObject")]
    [Tooltip("Put the game's End-Of-RoundUI to move for end of round animations and to enable this UI")]
    GameObject EndOfRoundUIObject;

    //The above's game animator
    private Animator EndOfRoundUIAnimator;

    //The Game's End-Of-RoundUI Text Objects to update
    [SerializeField]
    [Header("Endof-RoundUITextObjects")]
    [Tooltip("Put the game's End-Of-RoundUI TextObjects to update at the end, depending on what round this manages.")]
    TMP_Text EndOfRoundToastText, BaseTeaORCassavaFlexTxt, BaseTeaSoldTxt, PandanFlexTxt, PandanSoldTxt, BananaFlexTxt, BananaSoldTxt,
    StrawberryFlexTxt, StrawberrySoldTxt, MangoFlexTxt, MangoSoldTxt, UbeFlexTxt, UbeSoldTxt, ShopLevelNMultiplier, TotalNewGoldTxt;

    //A notif if the player did not hunt cassava slimes this round
    [SerializeField]
    [Header("No Cassava for boba!")]
    [Tooltip("Drag the notif object in the end-of round UI")]
    GameObject NeedSlimeForBobaNotif;

    //FIXME: private bool roundIsOver = false;

    //FIXME: Temp PUBLIC value to see the time in game. SET TO PRIVATE AFTER DEBUGGING.
    //Timer for this round.
    public float roundTimer;

    //Timer cooldown for possible customer spawning. Changes depending on popularity (20s = 1 star, 1s = 5 star).
    public float customerSpawnCooldownTimer = 0.0f;

    //Bool to check if first customer has spawned for special reasons.
    bool firstCustomerSpawned = false;

    //Values to calculate how much of which resources were gained from the round.
    private int CassavaSlimeBalls = 0, PandanLeaves = 0, BananaMinis = 0,
    StrawberryMinis = 0, MangoMinis = 0, UbeMinis = 0;

    //FIXME: This prior logic was used for the alpha! Edit this for the beta!
    //Values to show how many of each drink were sold. Made via RNG at the end of the game, and each drink requires ONE CassavaSlimeBalls resource. 
    //Oolong is default if there are no other resources left. Excess resources are thrown away sadly.
    private int oolongSold = 0, PandanSold = 0, BananaSold = 0,
    StrawberrySold = 0, MangoSold = 0, UbeSold = 0;

    //ValuesOfEachDrinkSetPre-round by Overall Game Manager in pre-round UI and RNG.
    public float baseDrinkMultiplier = 1.0f, PandanMultiplier = 1.0f, BananaMultiplier = 1.0f,
    StrawberryMultiplier = 1.0f, MangoMultiplier = 1.0f, UbeMultiplier = 1.0f;

    //ValuesOfEachDrinkSet by Overall Game Manager in pre-round UI and RNG, and player price.
    public double priceOfCasavaTopping = 1.0f, priceOfPandan = 1.0f, priceOfBanana = 1.0f,
    priceOfStrawberry = 1.0f, priceOfMango = 1.0f, priceOfUbe = 1.0f, priceOfGreenTea = 1.0f, priceOfBlackTea = 1.0f, priceOfOolongTea = 1.0f, priceOfMilk = 1.0f;

    //Bools to see if player set price is higher than expected drink price.
    public bool priceOfCasavaToppingScalped = false, priceOfPandanScalped = false, priceOfBananaScalped = false,
    priceOfStrawberryScalped = false, priceOfMangoScalped = false, priceOfUbeScalped = false, priceOfGreenTeaScalped, priceOfBlackTeaScalped = false, priceOfOolongTeaScalped = false, priceOfMilkScalped = false;

    //Player Increased coin value by how much this round?
    public double playerEarnedCoins = 0;

    //FIXME: These are connected to the Spawner scripts, enemy agro, customer patience, ect. To manage the difficulty of the round. Should be modulated by a "level difficulty" method!
    //The int should be pulled by the GAMEMANAGER script!
    public int levelDifficulty;

    //The chance for enemies to seek out the player no matter if in range or not
    public float playerPerpetualAgroChance;

    //SlowmoTime
    public float slowmoTime = 2f;

    //Possible spawned spawners 
    public float chanceOfSlime, chanceOfPandan, chanceOfBanana, chanceOfStrawberry, chanceOfMango, chanceOfUbe;

    //Saves how many customers were spawned in the round
    [Header("SpawnedCustomers")]
    [Tooltip("Used count how many customers are spawned in the world currently. Does not record any enemies pre-placed into the world!")]
    public int customersSpawned = 0;

    //Saves the base stat of how long a customer would wait on an action (Base of 10seconds)
    [Header("Customer Patience")]
    [Tooltip("This is base patience for customers and should be modulated against other factors.")]
    public float customerOverallPatienceThisRound = 10f;

    //Special flags for customer order patience counters.
    public float customerTimerInQO1 = .01f, customerTimerInQO2, customerTimerInQO3, customerTimerInDO1, customerTimerInDO2, customerTimerInDO3, customerTimerInDO4, customerTimerInDO5, customerTimerInDO6, customerTimerInSO1;
    public float customerStartingTimeInQO1, customerStartingTimeInQO2, customerStartingTimeInQO3, customerStartingTimeInDO1, customerStartingTimeInDO2, customerStartingTimeInDO3, customerStartingTimeInDO4, customerStartingTimeInDO5, customerStartingTimeInDO6, customerStartingTimeInSO1;
    public float eCustomerEndTimeAInQO1, eCustomerEndTimeAInQO2, eCustomerEndTimeAInQO3, eCustomerEndTimeAInDO1, eCustomerEndTimeAInDO2, eCustomerEndTimeAInDO3, eCustomerEndTimeAInDO4, eCustomerEndTimeAInDO5, eCustomerEndTimeAInDO6, eCustomerEndTimeAInSO1;
    public float bellTimer = 0f;

    //NOTE : Player ALWAYS starts with 3 lives!
    public int playerLives;

    // Start is called before the first frame update.
    void Start()
    { 
        playerLives = 3;
        overallGameManager = GameObject.Find("GameManagerObject");
        thisGamesOverallInstanceScript = overallGameManager.GetComponent<GameManagerScript>();
        customersThatCanSpawnThisRoundScript = this.gameObject.GetComponent<NPCCustomersThatCanSpawnScript>();
        whatDrinksArePopular = thisGamesOverallInstanceScript.ReturnDrinkRatesThisRound();
        playersCurrentShopStats = thisGamesOverallInstanceScript.ReturnCurrentShopInstance();
        playersCurrentDataStats = thisGamesOverallInstanceScript.ReturnPlayerStats();

        //FIXME: Patience may change due to game events (Like Rain, Storms, Cold, Heat, Festivites, Ect.)
        customerOverallPatienceThisRound = thisGamesOverallInstanceScript.ReturnBobaShopCustomerPatience();

        //Make Sure the ENDOFGAME UI isn't on and the INGAME UI is.
        EndOfRoundUIObject.SetActive(false);
        //FIXME:inGameUIObject.SetActive(true);

        //Set all customer timers to .1
        customerTimerInQO1 = customerTimerInQO2 = customerTimerInQO3 = customerTimerInDO1 = customerTimerInDO2 = customerTimerInDO3 = customerTimerInDO4 = customerTimerInDO5 = customerTimerInDO6 = customerTimerInSO1 = 0.1f;

        //Get the drink demand from this game's manager and apply them to this round
        UpdateThisRoundDrinksDemand();

        //Set the prices of drinks.
        SetDrinkPricesFromGameSave();

        //Update how often the customers can spawn depending on player data (Shop popularity).
        resetCustomerSpawnCooldownTimer();

        //Start the round timer and make sure the timescale is set to 1. Moreover, make sure this round is over bool is not true.
        //FIXME:roundIsOver = false;
        roundTimer = 0f;
        Time.timeScale = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(roundTimer<=360){
            roundTimer += Time.deltaTime;
            EscalateQueueTimers();
        }else{
            EndTheBOBASHOPRound();
        }

        //FIXME: Timer for customer spawning.
        //No customers spawn within the first 10 seconds, then attempt to spawn 1/5th chance for a customer every 5 seconds depending on popularity,
        //Player max queue, and others.
        if(roundTimer > 10 && !firstCustomerSpawned){
            firstCustomerSpawned = true;
            tryToSpawnARoundLoadedCustomer();  
        }

        if(firstCustomerSpawned && customerSpawnCooldownTimer>0){
            customerSpawnCooldownTimer -= Time.deltaTime;
        }

        if (firstCustomerSpawned && customerSpawnCooldownTimer < 0) {
            tryToSpawnARoundLoadedCustomer();
            resetCustomerSpawnCooldownTimer();
        }

        //This calls the script to attempt a customer to pickup their drinks to pay.
        if (eCustomerEndTimeAInDO1 > roundTimer + 1.1f)
        {
            customerOrderPickupHandlerScript.AttemptCustomerTakesDrinksAndPays();
        }
        else
        {
            customerOrderPickupHandlerScript.AttemptCustomerLoosingPatienceButGrabbingDrinks();
        } 
    }

    //Sets the customer cooldown timer for the CHANCE to spawn another NPC at the boba shop to a formula including shop popularity.
    public void resetCustomerSpawnCooldownTimer(){
        customerSpawnCooldownTimer = 25.5f - Mathf.RoundToInt(thisGamesOverallInstanceScript.ReturnPlayerStats().shopPopularity) * 5;
    }

    public float getRoundTime(){
        return roundTimer;
    }

    //FIXME: A method to attempt to spawn a new customer according to shop popularity and if queue line (Order & Waiting queue) is full. Need to add popularity.
    public void tryToSpawnARoundLoadedCustomer(){
        if(customersThatCanSpawnThisRoundScript.thisRoundOfPossibleCustomers.Count!=0)
        {
            if ((customerQueueHandlerScript.toOrderCustomerQueue.Count + customerWaitingDrinkHandlerScript.waitingForOrderCustomerQueue.Count) < thisGamesOverallInstanceScript.ReturnMaxBobaShopLineQueue())
            {
                if (customersThatCanSpawnThisRoundScript.useRandomizedCustomerList)
                {
                    GameObject customerPlannedToSpawn = customersThatCanSpawnThisRoundScript.LoadRandomCustomerFromList();
                    customersThatCanSpawnThisRoundScript.thisRoundOfPossibleCustomers.Remove(customerPlannedToSpawn);
                    customerQueueHandlerScript.AddCustomerToThisQueue(customerPlannedToSpawn);
                    print("Spawned a customer.");
                }
                else
                {
                    GameObject customerPlannedToSpawn = customersThatCanSpawnThisRoundScript.thisRoundOfPossibleCustomers[0];
                    customersThatCanSpawnThisRoundScript.thisRoundOfPossibleCustomers.Remove(customerPlannedToSpawn);
                    customerQueueHandlerScript.AddCustomerToThisQueue(customerPlannedToSpawn);
                    print("Spawned the next customer.");
                }
                
            }
        }
    }

    //A method to adjust how long a customer would wait in line for various reasons.
    public void SetCustomerWaitingSpeeds()
    {
        thisGamesOverallInstanceScript.ReturnBobaShopCustomerPatience();
    }

    //For used by customer scripts to keep track of how many customers are in this round
    public void CustomerSpawned(){
        customersSpawned += 1;
    }

    /// <summary>
    /// This method is used to calculate the player's earnings at the end of the round.
    /// 
    /// Again, the drinks that are sold are made via RNG at the end of the game, and each drink requires ONE CassavaSlimeBalls resource. 
    /// Oolong is default if there are no other resources left. Excess resources are thrown away sadly.
    /// 
    /// </summary>
    private void CalculateEndOfRoundScoreYields(){

        //NOTE: This was the old logic newer logic below: For however how many cassavaslimeball resources the player has earned, make a drink randomly and decrement until there are no more cassavaslimeballs left.
        //Use these values for the next step below.

        //~~~~~~~~~~~~~~~~~~~~~~~~~~OLD LOGIC USED THAT MAY BE RECYCLED!~~~~~~~~~~~~~~~~~~~~~~~~~~
        /*EXPAND BELOW
        
        for (int drinksToMakeLeft = CassavaSlimeBalls; drinksToMakeLeft > 0; drinksToMakeLeft =-1){
            
            makePossibleDrinkList();

            //Pick random string from possibleDrinks list, and do the co-responding actions : add the drink made to their counter, subtract cassavaslimeballs to escape loop,
            //and finally, re-update list to do it again if there's still resources left. This could maybe be made via recursion, but idk how yet.
            string selectedDrinkToMake = possibleDrinksList[UnityEngine.Random.Range(0,possibleDrinksList.Count)];

            //FIXME:

            switch(selectedDrinkToMake){

                case "Oolong": 

                oolongSold += 1;
                CassavaSlimeBalls -= 1;
                break;

                case "Pandan": 

                PandanSold +=1;
                CassavaSlimeBalls -= 1;
                break;

                case "Banana":

                BananaSold +=1;
                CassavaSlimeBalls -= 1;
                break;

                case "Strawberry":

                StrawberrySold +=1;
                CassavaSlimeBalls -= 1;
                break;

                case "Mango":

                MangoSold += 1;
                CassavaSlimeBalls -= 1;
                break;

                case "Ube":

                UbeSold +=1;
                CassavaSlimeBalls -= 1;
                break;

                default:

                oolongSold += 1;
                CassavaSlimeBalls -= 1;
                break;
            } 
        } 
        
        //FIXME: Use this to make rng drink orders?
    private void makePossibleDrinkList(){

        //Reset the possible drinks
        possibleDrinksList = new List<string>{};

        //Add Oolong
        if(CassavaSlimeBalls != 0){
            possibleDrinksList.Add("Oolong");
        }

        //Add Pandan
        if(PandanLeaves != 0){
            possibleDrinksList.Add("Pandan");
        }

        //Add Banana
        if(BananaMinis != 0){
            possibleDrinksList.Add("Banana");
        }

        //Add Strawberry
        if(StrawberryMinis != 0){
            possibleDrinksList.Add("Strawberry");
        }

        //Add Mango
        if(MangoMinis != 0){
            possibleDrinksList.Add("Mango");
        }

        //Add Ube
        if(UbeMinis != 0){
            possibleDrinksList.Add("Ube");
        }
    }


        //Actually calculate the ammount of coin the user gained this round.
        //Math goes like this = Player shop multiplier * (Total drinks sold * Their Multipliers)
        playerEarnedCoins = thisGamesOverallInstance.ReturnPlayerShopCoinMultiplier() * ((oolongSold * oolongMultiplier) + (PandanSold * PandanMultiplier) + 
        (BananaSold * BananaMultiplier) + (StrawberrySold * StrawberryMultiplier) + (MangoSold * MangoMultiplier) + (UbeSold * UbeMultiplier));

        //Debug.Log(playerEarnedCoins);

        */
        //~~~~~~~~~~~~~~~~~~~~~~~~~~NEW LOGIC : Shop was revised!~~~~~~~~~~~~~~~~~~~~~~~~~~
        
    }

    private void UpdateAndSaveBobaShopInventory(){
        CalculateItemsUsed();
        thisGamesOverallInstanceScript.UpdatePlayerHuntInventoryLossed(CassavaSlimeBalls, PandanLeaves, BananaMinis, StrawberryMinis, MangoMinis, UbeMinis);
    }

    private void UpdateThisRoundDrinksDemand(){
        baseDrinkMultiplier = thisGamesOverallInstanceScript.ReturnDrinkRatesThisRound().OolongMultiplier;
        PandanMultiplier = thisGamesOverallInstanceScript.ReturnDrinkRatesThisRound().PandanMultiplier;
        BananaMultiplier = thisGamesOverallInstanceScript.ReturnDrinkRatesThisRound().BananaMultiplier;
        StrawberryMultiplier = thisGamesOverallInstanceScript.ReturnDrinkRatesThisRound().StrawberryMultiplier;
        MangoMultiplier = thisGamesOverallInstanceScript.ReturnDrinkRatesThisRound().MangoMultiplier;
        UbeMultiplier = thisGamesOverallInstanceScript.ReturnDrinkRatesThisRound().UbeMultiplier;
    }

    private void UpdateEndRoundUI(){
        BaseTeaORCassavaFlexTxt.text = baseDrinkMultiplier.ToString("F2");
        BaseTeaSoldTxt.text = "x" +oolongSold.ToString(); 
        PandanFlexTxt.text = PandanMultiplier.ToString("F2");
        PandanSoldTxt.text = "x" +PandanSold.ToString(); 
        BananaFlexTxt.text = BananaMultiplier.ToString("F2"); 
        BananaSoldTxt.text = "x" +BananaSold.ToString(); 
        StrawberryFlexTxt.text = StrawberryMultiplier.ToString("F2"); 
        StrawberrySoldTxt.text = "x" +StrawberrySold.ToString(); 
        MangoFlexTxt.text = MangoMultiplier.ToString("F2"); 
        MangoSoldTxt.text = "x" +MangoSold.ToString(); 
        UbeFlexTxt.text = UbeMultiplier.ToString("F2"); 
        UbeSoldTxt.text = "x" +UbeSold.ToString();
        ShopLevelNMultiplier.text = "Lvl" +thisGamesOverallInstanceScript.ReturnCurrentShopInstance().shopLevelAt.ToString() 
        + " x " +thisGamesOverallInstanceScript.ReturnCurrentShopInstance().playerShopDrinkSellAmmount.ToString("F2");
        TotalNewGoldTxt.text = playerEarnedCoins.ToString("F2");

    //Notify the player if they forgot to hunt cassava slimes for this round!
        if(oolongSold == 0){
            NeedSlimeForBobaNotif.SetActive(true);
        }else{NeedSlimeForBobaNotif.SetActive(false);}
    }

    private void UpdateEndOfHUNTRoundUI(){
        BaseTeaORCassavaFlexTxt.text = CassavaSlimeBalls.ToString("F2");
        PandanFlexTxt.text = PandanLeaves.ToString("F2");
        BananaFlexTxt.text = BananaMinis.ToString("F2"); 
        StrawberryFlexTxt.text = StrawberryMinis.ToString("F2"); 
        MangoFlexTxt.text = MangoMinis.ToString("F2"); 
        UbeFlexTxt.text = UbeMinis.ToString("F2"); 
    }

    //This method is used by the "Continue" button at the end of the "HUNT" round and updates
    //the overall inventory the player has, and saves new data! (Produces this levels states
    //into the save as well).
    public void EndTheBOBASHOPRound(){

        //Enable end of game UI
        EndOfRoundUIObject.SetActive(true);
        
        //Morescript to pull end of game UI
        inGameUIAnimator.Play("EndOfGameMoveOutOfTheWay");
        EndOfRoundUIAnimator.Play("MoveInEndOfRoundUI");

        //Used to freeze game and MOSTLY everything
        Time.timeScale = 0;

        //FIXME: use this second variable incase of other scripts still running after Time.timescale = 0
        //FIXME: roundIsOver = true;

        //Turn off other UI after these many seconds
        StartCoroutine(TurnOffInGameUIAfterNSeconds(5f));

        //Use RNG and other values from the round start pulled via "getRoundSettingData()";
        CalculateEndOfRoundScoreYields();

        //Call GameManager to change drink demand rates for next round
        thisGamesOverallInstanceScript.SetNewDrinkDemandRates(thisGamesOverallInstanceScript.ReturnDrinkRatesThisRound());

        EndOfRoundToastText.text = "The Day Has Ended!";

        //Update End of Round UI
        UpdateEndOfHUNTRoundUI();
    }

    public void endTheRoundViaBossKill(string bossName){

        //Enable end of game UI
        EndOfRoundUIObject.SetActive(true);
        
        //Morescript to pull end of game UI
        inGameUIAnimator.Play("EndOfGameMoveOutOfTheWay");
        EndOfRoundUIAnimator.Play("MoveInEndOfRoundUI");

        //Used to freeze game and MOSTLY everything
        Time.timeScale = 0;

        //FIXME: use this second variable incase of other scripts still running after Time.timescale = 0
        //FIXME: roundIsOver = true;

        //Turn off other UI after these many seconds
        StartCoroutine(TurnOffInGameUIAfterNSeconds(5f));

        //Use RNG and other values from the round start pulled via "getRoundSettingData()";
        CalculateEndOfRoundScoreYields();

        EndOfRoundToastText.fontSize = 100;
        EndOfRoundToastText.text = "You have killed the " + bossName +"!";

        //Update End of Round UI
        UpdateEndRoundUI();
    }

    //FIXME: Will be called if the round is ended via the pause menu - you have yet to connect this to the main menu
    public void endTheRoundEarly(){

        //Enable end of game UI
        EndOfRoundUIObject.SetActive(true);
        
        //FIXME: make custom early round end UI & animations
        inGameUIAnimator.Play("EndOfGameMoveOutOfTheWay");
        EndOfRoundUIAnimator.Play("MoveInEndOfRoundUI");

        //Used to freeze game and MOSTLY everything
        Time.timeScale = 0;

        //FIXME: use this second variable incase of other scripts still running after Time.timescale = 0
        //FIXME: roundIsOver = true;

        //Turn off other UI after these many seconds
        StartCoroutine(TurnOffInGameUIAfterNSeconds(5f));

        //Use RNG and other values from the round start pulled via "getRoundSettingData()";
        CalculateEndOfRoundScoreYields();

        //playerEarnedCoins /= 2;

        //Update End of Round UI
        UpdateEndRoundUI();
    }

    //FIXME: Will be called if the round is ended via players lives being all taken. - You have yet to test this out.
    public void endTheRoundDueToDeath(){

        //Enable end of game UI
        EndOfRoundUIObject.SetActive(true);
        
        //FIXME: make custom death UI & animations Morescript to pull end of game UI
        inGameUIAnimator.Play("EndOfGameMoveOutOfTheWay");
        EndOfRoundUIAnimator.Play("MoveInEndOfRoundUI");

        //Used to freeze game and MOSTLY everything
        Time.timeScale = 0;

        //FIXME: use this second variable incase of other scripts still running after Time.timescale = 0
        //FIXME: roundIsOver = true;

        //Turn off other UI after these many seconds
        StartCoroutine(TurnOffInGameUIAfterNSeconds(5f));

        //Use RNG and other values from the round start pulled via "getRoundSettingData()";
        CalculateEndOfRoundScoreYields();

        EndOfRoundToastText.text = "You have died!";

        ///
        ///NOTE: MAKE AN IN-GAME UI THT SHOWS THIS /2 PENALTY OF COINS EARNED DUE TO PLAYER DEATH
        ///
        playerEarnedCoins /= 2;

        //Update End of Round UI
        UpdateEndRoundUI();
    }

    //Below is to activate by Uni_Health to take a player life away upon hp = 0
    public void TakePlayerLives(){
        playerLives -= 1;
    }

     //Below is to activate by Uni_Health to check if the player is actually dead
    public int ReturnPlayerLives(){
        return playerLives;
    }

    //This will be connected to the button for the "Continue" at the end of game UI.
    public void saveBeforeContinuingBackToMainMenuButton(){

        thisGamesOverallInstanceScript.ReturnPlayerStats().onDayNumber += 1;

        UpdateAndSaveBobaShopInventory();

        //Update overall coin stats
        MatchMoneyJar();
        thisGamesOverallInstanceScript.UpdatePlayerCoinStats(Math.Round(playerEarnedCoins, 2, MidpointRounding.AwayFromZero));

        //Call GameManager to change spawn rates for next round
        //FIXME:thisGamesOverallInstance.SetNewWorldSpawnRatesState(whichWorldWasSelected);

        //Call GameManager to change drink demand rates for next round
        thisGamesOverallInstanceScript.SetNewDrinkDemandRates(thisGamesOverallInstanceScript.ReturnDrinkRatesThisRound());
    }

    //FIXME: Make the method to pull scripts / data rom JSON serialized object? This is for the current round level, spawner settings / availability, etc.
    /* public void getRoundSettingData(){

    } */

    // This method ensures that the timers for the queues are managed by an inactavatablie game object (Round Manager)
    public void EscalateQueueTimers()
    {
        if (eCustomerEndTimeAInQO1 > roundTimer)
        {
            customerTimerInQO1 = eCustomerEndTimeAInQO1 - roundTimer;
        }
        else if(customerQueueHandlerGameObject.transform.childCount > 0)
        {
            if (customerQueueHandlerScript.toOrderCustomerQueue[0].GetComponent<CustomerPatienceUIScript>() != null)
            {
                if (customerQueueHandlerScript.toOrderCustomerQueue[0].GetComponent<CustomerPatienceUIScript>().customerIsAtFront)
                {
                    customerTimerInQO1 = 0.1f;
                    customerQueueHandlerScript.CustomerRunsOutOfPatienceForOderTaken(0);
                }
            }
        }
        if (eCustomerEndTimeAInQO2 > roundTimer)
        {
            customerTimerInQO2 = eCustomerEndTimeAInQO2 - roundTimer;
        }
        else if (customerQueueHandlerGameObject.transform.childCount > 1)
        {
            if (customerQueueHandlerScript.toOrderCustomerQueue[1].GetComponent<CustomerPatienceUIScript>() != null)
            {
                if (customerQueueHandlerScript.toOrderCustomerQueue[1].GetComponent<CustomerPatienceUIScript>().customerIsInWaitingInALineNotAtFront)
                {
                    customerTimerInQO2 = 0.1f;
                    customerQueueHandlerScript.CustomerRunsOutOfPatienceForOderTaken(1);
                }
            }
        }
        if (eCustomerEndTimeAInQO3 > roundTimer)
        {
            customerTimerInQO3 = eCustomerEndTimeAInQO3 - roundTimer;
        }
        else if (customerQueueHandlerGameObject.transform.childCount > 2)
        {
            if (customerQueueHandlerScript.toOrderCustomerQueue[2].GetComponent<CustomerPatienceUIScript>() != null)
            {
                if (customerQueueHandlerScript.toOrderCustomerQueue[2].GetComponent<CustomerPatienceUIScript>().customerIsInWaitingInALineNotAtFront)
                {
                    customerTimerInQO3 = 0.1f;
                    customerQueueHandlerScript.CustomerRunsOutOfPatienceForOderTaken(2);
                }
            }
        }
        if (eCustomerEndTimeAInDO1 > roundTimer)
        {
            customerTimerInDO1 = eCustomerEndTimeAInDO1 - roundTimer;
        }
        else
        {
            customerWaitingDrinkHandlerScript.CheckPatienceOfWaitingCustomers();
        }
        if (eCustomerEndTimeAInDO2 > roundTimer)
        {
            customerTimerInDO2 = eCustomerEndTimeAInDO2 - roundTimer;
        }
        else
        {
            customerWaitingDrinkHandlerScript.CheckPatienceOfWaitingCustomers();
        }
        if (eCustomerEndTimeAInDO3 > roundTimer)
        {
            customerTimerInDO3 = eCustomerEndTimeAInDO3 - roundTimer;
        }
        else
        {
            customerWaitingDrinkHandlerScript.CheckPatienceOfWaitingCustomers();
        }
        if (eCustomerEndTimeAInDO4 > roundTimer)
        {
            customerTimerInDO4 = eCustomerEndTimeAInDO4 - roundTimer;
        }
        else
        {
            customerWaitingDrinkHandlerScript.CheckPatienceOfWaitingCustomers();
        }
        if (eCustomerEndTimeAInDO5 > roundTimer)
        {
            customerTimerInDO5 = eCustomerEndTimeAInDO5 - roundTimer;
        }
        else
        {
            customerWaitingDrinkHandlerScript.CheckPatienceOfWaitingCustomers();
        }
        if (eCustomerEndTimeAInDO6 > roundTimer)
        {
            customerTimerInDO6 = eCustomerEndTimeAInDO6 - roundTimer;
        }
        else
        {
            customerWaitingDrinkHandlerScript.CheckPatienceOfWaitingCustomers();
        }
        if (eCustomerEndTimeAInSO1 > roundTimer)
        {
            customerTimerInSO1 = eCustomerEndTimeAInSO1 - roundTimer;
        }
        if (bellTimer > 0)
        {
            bellTimer = bellTimer - roundTimer;
        }
    }

    //customerWaitingInQO1, customerWaitingInQO2, customerWaitingInQO3, customerWaitingInDO1, customerWaitingInDO2, customerWaitingInDO3, customerWaitingInSO1;
    //// This method ensures that the timers are reset depending on i input
    //public void ResetTimerofQueueN(int Index1to7)
    //{
    //    switch (Index1to7)
    //    {
    //        case 0:
    //            return;
    //        case 1:
    //            return;
    //        case 2:
    //            return;
    //    }
    //}

    //Get prices of drinks and set them. If player's asking price is lower, set the bool for another script to deter customers from queue!
    public void SetDrinkPricesFromGameSave()
    {
        if (playersCurrentDataStats.casavaToppingPlayerPrice <= playersCurrentShopStats.casavaToppingExpectedPrice)
        {
            priceOfCasavaTopping = playersCurrentDataStats.pandanPlayerPrice;
            priceOfCasavaToppingScalped = false;
        }
        else
        {
            priceOfCasavaTopping = playersCurrentDataStats.pandanPlayerPrice;
            priceOfCasavaToppingScalped = true;
        }
        if (playersCurrentDataStats.pandanPlayerPrice <= playersCurrentShopStats.pandanExpectedPrice)
        {
            priceOfPandan = playersCurrentDataStats.pandanPlayerPrice;
            priceOfPandanScalped = false;
        }
        else
        {
            priceOfPandan = playersCurrentDataStats.pandanPlayerPrice;
            priceOfPandanScalped = true;
        }
        if (playersCurrentDataStats.bananaPlayerPrice <= playersCurrentShopStats.bananaExpectedPrice)
        {
            priceOfBanana = playersCurrentDataStats.bananaPlayerPrice;
            priceOfBananaScalped = false;
        }
        else
        {
            priceOfBanana = playersCurrentDataStats.bananaPlayerPrice;
            priceOfBananaScalped = true;
        }
        if (playersCurrentDataStats.strawberryPlayerPrice <= playersCurrentShopStats.strawberryExpectedPrice)
        {
            priceOfStrawberry = playersCurrentDataStats.strawberryPlayerPrice;
            priceOfStrawberryScalped = false;
        }
        else
        {
            priceOfStrawberry = playersCurrentDataStats.strawberryPlayerPrice;
            priceOfStrawberryScalped = true;
        }

        if (playersCurrentDataStats.mangoPlayerPrice <= playersCurrentShopStats.mangoExpectedPrice)
        {
            priceOfMango = playersCurrentDataStats.mangoPlayerPrice;
            priceOfMangoScalped = false;
        }
        else
        {
            priceOfMango = playersCurrentDataStats.mangoPlayerPrice;
            priceOfMangoScalped = true;
        }

        if (playersCurrentDataStats.ubePlayerPrice <= playersCurrentShopStats.ubeExpectedPrice)
        {
            priceOfUbe = playersCurrentDataStats.ubePlayerPrice;
            priceOfUbeScalped = false;
        }
        else
        {
            priceOfUbe = playersCurrentDataStats.ubePlayerPrice;
            priceOfUbeScalped = true;
        }
        if (playersCurrentDataStats.greenTeaPlayerPrice <= playersCurrentShopStats.greenTeaExpectedPrice)
        {
            priceOfGreenTea = playersCurrentDataStats.greenTeaPlayerPrice;
            priceOfGreenTeaScalped = false;
        }
        else
        {
            priceOfGreenTea = playersCurrentDataStats.greenTeaPlayerPrice;
            priceOfGreenTeaScalped = true;
        }

        if (playersCurrentDataStats.blackTeaPlayerPrice <= playersCurrentShopStats.blackTeaExpectedPrice)
        {
            priceOfBlackTea = playersCurrentDataStats.blackTeaPlayerPrice;
            priceOfBlackTeaScalped = false;
        }
        else
        {
            priceOfBlackTea = playersCurrentDataStats.blackTeaPlayerPrice;
            priceOfBlackTeaScalped = true;
        }

        if (playersCurrentDataStats.oolongTeaPlayerPrice <= playersCurrentShopStats.oolongTeaExpectedPrice)
        {
            priceOfOolongTea = playersCurrentDataStats.oolongTeaPlayerPrice;
            priceOfOolongTeaScalped = false;
        }
        else
        {
            priceOfOolongTea = playersCurrentDataStats.oolongTeaPlayerPrice;
            priceOfOolongTeaScalped = true;
        }

        if (playersCurrentDataStats.milkPlayerPrice <= playersCurrentShopStats.milkExpectedPrice)
        {
            priceOfMilk = playersCurrentDataStats.milkPlayerPrice;
            priceOfMilkScalped = false;
        }
        else
        {
            priceOfMilk = playersCurrentDataStats.milkPlayerPrice;
            priceOfMilkScalped = true;
        }
    }

    //Chattiness adds (x * 5) seconds buffer when talking with front customers.
    public void CustomerInterationAddsPatienceToFrontCustomer(float chattiness,string QueueOfInteraction)
    {
        if (eCustomerEndTimeAInQO1 >= roundTimer && QueueOfInteraction == "CustomerQueueHandler")
        {
            eCustomerEndTimeAInQO1 = (chattiness * 5f) + eCustomerEndTimeAInQO1;
        }

        if (eCustomerEndTimeAInDO1 >= roundTimer && QueueOfInteraction == "CustomerDrinkWaitQueueHandler")
        {
            eCustomerEndTimeAInDO1 = (chattiness * 5f) + eCustomerEndTimeAInDO1;
        }

        if (eCustomerEndTimeAInSO1 >= roundTimer && QueueOfInteraction == "CustomerSpecialWaitQueueHandler")
        {
            eCustomerEndTimeAInSO1 = (chattiness * 5f) + eCustomerEndTimeAInSO1;
        }
    }

    public void CalculateItemsUsed()
    {
        foreach (GameObject Tray in traysInTheShop.transform)
        {
            switch (Tray.GetComponent<ItemTrayObjectScript>().selectedItemIndexThatWillBeInThisTray)
            {
                case 1:
                    thisGamesOverallInstanceScript.ReturnPlayerStats().casavaBalls = Tray.GetComponent<ItemTrayObjectScript>().itemSelectedLeftInPlayerInventory;
                    break;

                case 2:
                    thisGamesOverallInstanceScript.ReturnPlayerStats().pandanLeaves = Tray.GetComponent<ItemTrayObjectScript>().itemSelectedLeftInPlayerInventory;;
                    break;

                case 3:
                    thisGamesOverallInstanceScript.ReturnPlayerStats().bananas = Tray.GetComponent<ItemTrayObjectScript>().itemSelectedLeftInPlayerInventory;;
                    break;

                case 4:
                    thisGamesOverallInstanceScript.ReturnPlayerStats().strawberries = Tray.GetComponent<ItemTrayObjectScript>().itemSelectedLeftInPlayerInventory;;
                    break;

                case 5:
                    thisGamesOverallInstanceScript.ReturnPlayerStats().mangos = Tray.GetComponent<ItemTrayObjectScript>().itemSelectedLeftInPlayerInventory;;
                    break;

                case 6:
                    thisGamesOverallInstanceScript.ReturnPlayerStats().ube = Tray.GetComponent<ItemTrayObjectScript>().itemSelectedLeftInPlayerInventory; ;
                    break;
            }
        }
        
    }

    public void MatchMoneyJar()
    {
        playerEarnedCoins = thisRoundMoneyJar.GetComponent<MoneyJarScript>().ReturnMoneyThisRound();
    }

    public IEnumerator TurnOffInGameUIAfterNSeconds(float num)
    {
        yield return new WaitForSecondsRealtime(num);
    
    }

    //This enum is a lerp for the NPC's position from the right side of the screen to pick up thier drink.
    public IEnumerator LerpNPCPositionAnimation(float newPositionDesired, GameObject NPCToMove)
    {
        float timeElapsed = 0;

        while (timeElapsed < NPCToMove.GetComponentInParent<CustomerDrinkScript>().characterShopSpeed)
        {
            float valueToLerp = Mathf.Lerp(NPCToMove.transform.localPosition.x, newPositionDesired, timeElapsed / 100f);
            NPCToMove.transform.localPosition = new Vector3(valueToLerp, -55, 0);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        NPCToMove.transform.localPosition = new Vector3(newPositionDesired, -55, 0);

        //If this NPC being moved is the customer picking up drinks and the boba mat movement is the new position, add the Patience timer if the customer has one.
        if (customerWaitingDrinkHandlerScript.waitingForOrderCustomerQueue[0] == NPCToMove && newPositionDesired == 477f)
        {
            if (NPCToMove.GetComponent<CustomerPatienceUIScript>() != null)
            {
                eCustomerEndTimeAInDO1 = NPCToMove.GetComponent<CustomerPatienceUIScript>().customerDrinkWaitingTime + roundTimer;
                customerStartingTimeInDO1 = roundTimer;
                NPCToMove.GetComponent<CustomerPatienceUIScript>().TimerStartedWaitingForPickingUpOrder();
                NPCToMove.GetComponent<CustomerPatienceUIScript>().customerIsAtFront = true;
                NPCToMove.GetComponent<CustomerPatienceUIScript>().customerHadOrderTaken = true;
                bobaDrinkMat.GetComponent<CustomerPayment4DrinkScript>().customerIsTryingToPay = true;
                NPCToMove.GetComponent<CustomerPatienceUIScript>().customerIsInWaitingInALineNotAtFront = false;
            }
        }
    }

    public IEnumerator LerpNPCPatienceDestroyerO(float newPositionDesired, GameObject NPCToMove)
    {
        if (customerWaitingDrinkHandlerScript.isActiveAndEnabled)
        {
            float timeElapsed = 0;

            while (timeElapsed < 5f)
            {
                float valueToLerp = Mathf.Lerp(NPCToMove.transform.localPosition.x, newPositionDesired, timeElapsed / 100f);
                NPCToMove.transform.localPosition = new Vector3(valueToLerp, NPCToMove.transform.localPosition.y, 0);
                timeElapsed += Time.deltaTime;

                yield return null;
            }
        }

        NPCToMove.transform.localPosition = new Vector3(newPositionDesired, -55, 0);
        Destroy(NPCToMove);
    }

    //This enum is a lerp for the NPC's position coming into the line creating the "walking" into the queue animation.
    public IEnumerator LerpNPCQueuePosition(float newPositionDesired, GameObject NPCToMove)
    {
        float timeElapsed = 0;

        while (timeElapsed < NPCToMove.GetComponent<CustomerDrinkScript>().characterShopSpeed)
        {
            float valueToLerp = Mathf.Lerp(NPCToMove.transform.localPosition.x, newPositionDesired, timeElapsed / 100f);
            NPCToMove.transform.localPosition = new Vector3(valueToLerp, NPCToMove.transform.localPosition.y, 0);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        NPCToMove.transform.localPosition = new Vector3(NPCToMove.transform.localPosition.x, -55, 0);
    }

    //This enum is a lerp for the NPC's position "walking" into the other animation.
    public IEnumerator MoveNPCOtherQueuePosition(float newPositionDesired, GameObject NPCToMove)
    {
        float timeElapsed = 0;

        while (timeElapsed < NPCToMove.GetComponent<CustomerDrinkScript>().characterShopSpeed)
        {
            float valueToLerp = Mathf.Lerp(NPCToMove.transform.localPosition.x, newPositionDesired, timeElapsed / 100f);
            NPCToMove.transform.localPosition = new Vector3(valueToLerp, NPCToMove.transform.localPosition.y, 0);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        NPCToMove.transform.localPosition = new Vector3(NPCToMove.transform.localPosition.x, -55, 0);
    }

    //This enum is a lerp for the NPC's position "walking" into the other animation and destroys the NPC.
    public IEnumerator LerpNPCPatienceDestroyer(float newPositionDesired, GameObject NPCToMove)
    {
        float timeElapsed = 0;

        while (timeElapsed < NPCToMove.GetComponent<CustomerDrinkScript>().characterShopSpeed)
        {
            float valueToLerp = Mathf.Lerp(NPCToMove.transform.localPosition.x, newPositionDesired, timeElapsed / NPCToMove.GetComponent<CustomerDrinkScript>().characterShopSpeed);
            NPCToMove.transform.localPosition = new Vector3(valueToLerp, NPCToMove.transform.localPosition.y, 0);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        NPCToMove.transform.localPosition = new Vector3(-1400, -55, 0);
        Destroy(NPCToMove);
    }

    //This enum is a lerp for the NPC's color.
    public IEnumerator LerpNPCQueueColors(float colorTarget, Image imageToChange)
    {
        float timeElapsed = 0;
        float valueToLerp = 0f;

        while (timeElapsed < 0.5f)
        {
            imageToChange.color = new Color32(((byte)valueToLerp), ((byte)valueToLerp), ((byte)valueToLerp), 255);
            valueToLerp = Mathf.Lerp(0, colorTarget, timeElapsed / 1.5f);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        imageToChange.color = new Color32(((byte)colorTarget), ((byte)colorTarget), ((byte)colorTarget), 255);
    }
}
