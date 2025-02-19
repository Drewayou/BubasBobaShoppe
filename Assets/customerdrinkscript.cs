using UnityEngine;

public class CustomerDrinkScript : MonoBehaviour
{
    // The GameManager script pulled from the game manager object.
    // Usefull to determine the player stats for what drinks they unlocked.
    GameManagerScript thisGamesOverallInstanceScript;

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
