using System.Collections.Generic;
using UnityEngine;

public class CustomerDrinkScript : MonoBehaviour
{
    // The GameManager script pulled from the game manager object.
    // Usefull to determine the player stats for what drinks they unlocked.
    GameManagerScript thisGamesOverallInstanceScript;

    // The List of favorite drinks this character wants.
    List<GameObject> characterFavoriteBobaDrinks = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisGamesOverallInstanceScript = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
