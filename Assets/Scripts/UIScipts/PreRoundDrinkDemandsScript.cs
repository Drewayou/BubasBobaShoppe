using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PreRoundDrinkDemandsScript : MonoBehaviour
{
    //The Game's OverallManager Object to pull/put scripts from.
    [Header("GameManager")]
    [Tooltip("Put the game's overall Manager Object to end the game / check if paused / unpaused")]
    GameObject overallGameManager;

    //The Game's OverallManager SCRIPT to pull/put scripts and values from.
    [Header("GameManagerSCRIPT")]
    [Tooltip("Pull Values from this script")]
    GameManagerScript thisGamesOverallInstance;

    //Drink rate demands DrinkMultiplierScripts, which in turn is pulled from the GameManager.
    private DrinkMultiplierScripts whatDrinksArePopular;

    //The Game's Demand Text Objects to update
    [SerializeField]
    [Header("Boba Drink TextObjects")]
    [Tooltip("Put the game's End-Of-RoundUI TextObjects to update at the end")]
    TMP_Text OolongDemandTxt, PandanDemandTxt, BananaDemandTxt, StrawberryDemandTxt, MangoDemandTxt, UbeDemandTxt;

    // ValuesOfEachDrinkSetPre-round by Overall Game Manager in pre-round UI and RNG.
    private float oolongMultiplier, PandanMultiplier, BananaMultiplier,
    StrawberryMultiplier, MangoMultiplier, UbeMultiplier;
    // Start is called before the first frame update
    void Start()
    {
        
        // BOC below gets the overall game manager, focuses on the script component, then pulls
        // the drink data from the last known save and loads it to the menu for the next round.
        overallGameManager = GameObject.Find("GameManagerObject");
        thisGamesOverallInstance = overallGameManager.GetComponent<GameManagerScript>();
        whatDrinksArePopular = thisGamesOverallInstance.ReturnDrinkRatesThisRound();
        ShowDrinkDemandsForNextRound();
    }

    // Update is called once per frame
    void Update()
    {
        ShowDrinkDemandsForNextRound();
    }

    public void ShowDrinkDemandsForNextRound(){

        //NOTE: It's more efficient to simply display the data directly from the JSON,
        //BUT I'd rather have the variables of the multipliers also saved incase we 
        //May have a future game feature to be able to "Advertise" to temp increase a flavor demand
    
        //Set the drink data to what was loaded
        oolongMultiplier =  whatDrinksArePopular.OolongMultiplier;
        //Debug.Log(oolongMultiplier);
        PandanMultiplier = whatDrinksArePopular.PandanMultiplier;
        BananaMultiplier = whatDrinksArePopular.BananaMultiplier;
        StrawberryMultiplier = whatDrinksArePopular.StrawberryMultiplier;
        MangoMultiplier = whatDrinksArePopular.MangoMultiplier;
        UbeMultiplier = whatDrinksArePopular.UbeMultiplier;

        //Show the drink data to what was loaded to the TXT's in UI
        OolongDemandTxt.text = "x"+oolongMultiplier.ToString();
        PandanDemandTxt.text = "x"+PandanMultiplier.ToString();
        BananaDemandTxt.text = "x"+BananaMultiplier.ToString();
        StrawberryDemandTxt.text = "x"+StrawberryMultiplier.ToString();
        MangoDemandTxt.text = "x"+MangoMultiplier.ToString();
        UbeDemandTxt.text = "x"+UbeMultiplier.ToString();
    }
}
