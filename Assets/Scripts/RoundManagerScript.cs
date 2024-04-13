using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using System;
using TMPro;
using JetBrains.Annotations;

public class RoundManagerScript : MonoBehaviour
{
    //FIXME:
    
    //The Game's OverallManager Object to pull/put scripts from.
    [Header("GameManager")]
    [Tooltip("Put the game's overall Manager Object to end the game / check if paused / unpaused")]
    GameObject overallGameManager;

    //The Game's OverallManager SCRIPT to pull/put scripts and values from.
    [Header("GameManagerSCRIPT")]
    [Tooltip("Pull Values from this script")]
    GameManagerScript thisGamesOverallInstance;
    
    //Spawnrates and spawners pulled from WorldState, which in turn is pulled from the GameManager.
    private WorldState whichWorldWasSelected;

    //The Game's In-GameUI object to use / move during / after the game has ended.
    [SerializeField]
    [Header("In-GameUIObject")]
    [Tooltip("Put the game's In-GameUI to move for end of round animations and to disable this UI")]
    GameObject inGameUIObject;

    //The above's game animator
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
    [Tooltip("Put the game's End-Of-RoundUI TextObjects to update at the end")]
    TMP_Text OolongDemandTxt, OolongSoldTxt, PandanDemandTxt, PandanSoldTxt, BananaDemandTxt, BananaSoldTxt,
    StrawberryDemandTxt, StrawberrySoldTxt, MangoDemandTxt, MangoSoldTxt, UbeDemandTxt, UbeSoldTxt, TotalNewGoldTxt;

    //FIXME private bool roundIsOver = false;

    //FIXME: Temp PUBLIC value to see the time in game. SET TO PRIVATE AFTER DEBUGGING.
    //Timer for this round.
    public float roundTimer;

    //List of possible drinks that can be made (deppends on the ingredients obtained during the round).
    public List<String> possibleDrinksList;

    //Values to calculate how much of which resources were gained from the round.
    private int CassavaSlimeBalls = 0, PandanLeaves = 0, BananaMinis = 0,
    StrawberryMinis = 0, MangoMinis = 0, UbeMinis = 0;

    //Values to show how many of each drink were sold. Made via RNG at the end of the game, and each drink requires ONE CassavaSlimeBalls resource. 
    //Oolong is default if there are no other resources left. Excess resources are thrown away sadly.
    private int oolongSold = 0, PandanSold = 0, BananaSold = 0,
    StrawberrySold = 0, MangoSold = 0, UbeSold = 0;

    //FIXME: ValuesOfEachDrinkSetPre-round by Overall Game Manager in pre-round UI and RNG.
    private float oolongMultiplier = 1.0f, PandanMultiplier = 1.0f, BananaMultiplier = 1.0f,
    StrawberryMultiplier = 1.0f, MangoMultiplier = 1.0f, UbeMultiplier = 1.0f;

    //Player Increased coind value by how much this round?
    public float playerEarnedCoins = 0f;

    // Start is called before the first frame update.
    void Start()
    {
        //FIXME: ADD THE SETTING OF VALUES FROM THE OVERALLGAME MANAGER (Hint this may be from the JSON serialization of scriptableobject Gamemanager Script?)
        overallGameManager = GameObject.Find("GameManagerObject");
        thisGamesOverallInstance = overallGameManager.GetComponent<GameManagerScript>();

        //Make Sure the ENDOFGAME UI isn't on and the INGAME UI is.
        EndOfRoundUIObject.SetActive(false);
        inGameUIObject.SetActive(true);

        //Other LOC to yeild values
        //LOC to pull animator objects of these UI's for proper use/view/animations
        inGameUIAnimator = inGameUIObject.GetComponentInChildren<Animator>();
        EndOfRoundUIAnimator = EndOfRoundUIObject.GetComponentInChildren <Animator>();
    
        //FIXME: 
        
        //Check which world the player selected to go to. If default, set to world 1.
       
        //Debug.LogWarning(thisGamesOverallInstance.ReturnWorldSelected(worldSelected));

        whichWorldWasSelected = thisGamesOverallInstance.ReturnWorldSelectedViaRound();
        

        //Start the round timer and make sure the timescale is set to 1. Moreover, make sure this round is over bool is not true.
        //FIXME:roundIsOver = false;
        roundTimer = 0f;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(roundTimer<=180){
            roundTimer += Time.deltaTime;
        }else{
            endTheRound();
        }
    }

    public float getRoundTime(){
        return roundTimer;
    }

    //Setter Methods for this round resources used in other scripts.
    public void AddCassavaSlimeBallsThisRound(int addedLoot){
        CassavaSlimeBalls += addedLoot;
    }
    public void AddPandanLeavesThisRound(int addedLoot){
        PandanLeaves += addedLoot;
    }
    public void AddCBananaMinisThisRound(int addedLoot){
        BananaMinis += addedLoot;
    }
    public void AddStrawberryMinisThisRound(int addedLoot){
        StrawberryMinis += addedLoot;
    }
    public void AddMangoMinisThisRound(int addedLoot){
        MangoMinis += addedLoot;
    }
    public void AddtUbeMinisThisRound(int addedLoot){
        UbeMinis += addedLoot;
    }

    //Getter Methods for this round resources for use in other scripts.
    // I.E. In game UI pulls this into the UI values for the loot gathered!
    public int GetCassavaSlimeBallsThisRound(){
        return CassavaSlimeBalls;
    }
    public int GetPandanLeavesThisRound(){
        return PandanLeaves;
    }
    public int GetBananaMinisThisRound(){
        return BananaMinis;
    }
    public int GetStrawberryMinisThisRound(){
        return StrawberryMinis;
    }
    public int GetMangoMinisThisRound(){
        return MangoMinis;
    }
    public int GetUbeMinisThisRound(){
        return UbeMinis;
    }

    /// <summary>
    /// This method is used to calculate the player's earnings at the end of the round.
    /// 
    /// Again, the drinks that are sold are made via RNG at the end of the game, and each drink requires ONE CassavaSlimeBalls resource. 
    /// Oolong is default if there are no other resources left. Excess resources are thrown away sadly.
    /// 
    /// </summary>
    private void CalculateEndOfRoundScoreYields(){

        //For however how many cassavaslimeball resources the player has earned, make a drink randomly and decrement until there are no more cassavaslimeballs left.
        //Use these values for the next step below.
        
        for (int drinksToMakeLeft = CassavaSlimeBalls; drinksToMakeLeft > 0; drinksToMakeLeft =-1){
            
            makePossibleDrinkList();

            //Pick random string from possibleDrinks list, and do the co-responding actions : add the drink made to their counter, subtract cassavaslimeballs to escape loop,
            //and finally, re-update list to do it again if there's still resources left. This could maybe be made via recursion, but idk how yet.
            string selectedDrinkToMake = possibleDrinksList[UnityEngine.Random.Range(0,possibleDrinksList.Count)];

            //FIXME:
            //Debug.LogWarning(selectedDrinkToMake);

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

        //Actually calculate the ammount of coin the user gained this round.

        playerEarnedCoins = (oolongSold * oolongMultiplier) + (PandanSold * PandanMultiplier) + 
        (BananaSold * BananaMultiplier) + (StrawberrySold * StrawberryMultiplier) + (MangoSold * MangoMultiplier) + (UbeSold * UbeMultiplier);

        Debug.Log(playerEarnedCoins);
    }

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

    private void UpdateEndRoundUI(){
        OolongDemandTxt.text = oolongMultiplier.ToString("F2");
        OolongSoldTxt.text = "x" +oolongSold.ToString(); 
        PandanDemandTxt.text = PandanMultiplier.ToString("F2");
        PandanSoldTxt.text = "x" +PandanSold.ToString(); 
        BananaDemandTxt.text = BananaMultiplier.ToString("F2"); 
        BananaSoldTxt.text = "x" +BananaSold.ToString(); 
        StrawberryDemandTxt.text = StrawberryMultiplier.ToString("F2"); 
        StrawberrySoldTxt.text = "x" +StrawberrySold.ToString(); 
        MangoDemandTxt.text = MangoMultiplier.ToString("F2"); 
        MangoSoldTxt.text = "x" +MangoSold.ToString(); 
        UbeDemandTxt.text = UbeMultiplier.ToString("F2"); 
        UbeSoldTxt.text = "x" +UbeSold.ToString();
        TotalNewGoldTxt.text = playerEarnedCoins.ToString();
    }

    //This method is used by the "Continue" button at the end of the round and updates
    //the overall gold the player has, and saves new data (Produces this levels states
    //into the save as well).
    private void endTheRound(){

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

        //Update End of Round UI
        UpdateEndRoundUI();
    }

    //This will be connected to the button for the "Continue" at the end of game UI.
    public void saveBeforeContinuingBackToMainMenuButton(){

        //Call GameManager to change spawn rates for next round
        thisGamesOverallInstance.SetNewWorldSpawnRatesState(whichWorldWasSelected);

        //Update overall coin stats
        thisGamesOverallInstance.updatePlayerCoinStats((int)playerEarnedCoins);

        //Save new states
        thisGamesOverallInstance.SaveGameAfterRound(whichWorldWasSelected);
    }
    

    //FIXME: Make the method to pull scripts / data rom JSON serialized object? This is for the current round level, spawner settings / availability, etc.
    /* public void getRoundSettingData(){

    } */

    public IEnumerator TurnOffInGameUIAfterNSeconds(float num)
    {
        yield return new WaitForSecondsRealtime(num);
    
    }

}
