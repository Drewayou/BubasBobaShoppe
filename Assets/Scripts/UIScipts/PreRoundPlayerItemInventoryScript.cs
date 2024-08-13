using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PreRoundPlayerItemInventoryScript : MonoBehaviour
{
    //The Game's OverallManager Object to pull/put scripts from.
    [Header("GameManager")]
    [Tooltip("Put the game's overall Manager Object to end the game / check if paused / unpaused")]
    GameObject overallGameManager;

    //The Game's OverallManager SCRIPT to pull/put scripts and values from.
    [Header("GameManagerSCRIPT")]
    [Tooltip("Pull Values from this script")]
    GameManagerScript thisGamesOverallInstance;

    //The player's inventory UI Pre-Round
    [SerializeField]
    [Header("Player's Item Inventory Ammounts")]
    [Tooltip("Put the game's Pre-RoundUI TextObjects to update player inverntory visual values")]
    TMP_Text cassavaItems, pandanItems, bananaItems, strawberryItems, mangoItems, ubeItems;

    // Start is called before the first frame update
    void Start()
    {
        
        // BOC below gets the overall game manager, focuses on the script component, then pulls
        // the drink data from the last known save and loads it to the menu for the next round.
        overallGameManager = GameObject.Find("GameManagerObject");
        thisGamesOverallInstance = overallGameManager.GetComponent<GameManagerScript>();
        DisplayInventoryValues();
    }

    //Gets the player save from /alalla to produce the UI inventory screen.
    public void DisplayInventoryValues(){
        cassavaItems.text = thisGamesOverallInstance.ReturnPlayerStats().casavaBalls.ToString();
        pandanItems.text = thisGamesOverallInstance.ReturnPlayerStats().pandanLeaves.ToString();
        bananaItems.text = thisGamesOverallInstance.ReturnPlayerStats().bananas.ToString();
        strawberryItems.text = thisGamesOverallInstance.ReturnPlayerStats().strawberries.ToString();
        mangoItems.text = thisGamesOverallInstance.ReturnPlayerStats().mangos.ToString();
        ubeItems.text = thisGamesOverallInstance.ReturnPlayerStats().ube.ToString();
    }
}
