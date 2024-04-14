using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BobaLIvesScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Drag and drop the player Object here")]
    [Header("Player To Connect To Lives Meter")]
    GameObject BubbaPlayer;

    [SerializeField]
    [Tooltip("Drag and drop the boba lives co-responding to left-> right here")]
    [Header("BobaLivesUI")]
    GameObject BobaLives1, BobaLives2, BobaLives3;

    [SerializeField]
    [Tooltip("Drag and drop the boba lives co-responding to left-> right here")]
    [Header("BobaLivesMeterUI")]
    UnityEngine.UI.Image BobaLivesMeter1, BobaLivesMeter2, BobaLivesMeter3;

    RoundManagerScript thisRoundManager;

    Player_Controller thePlayersController;

    Health_Universal thePlayersHealth;

    // Start is called before the first frame update
    void Start()
    {
        thePlayersController = BubbaPlayer.GetComponent<Player_Controller>();
        thePlayersHealth = BubbaPlayer.GetComponent<Health_Universal>();
        thisRoundManager = GameObject.Find("RoundManager").GetComponent<RoundManagerScript>();;
    }

    // Update is called once per frame
    void Update()
    {
        if(thisRoundManager.ReturnPlayerLives() == 3){
            BobaLivesMeter1.fillAmount = thePlayersHealth.health / thePlayersController.getPlayerMaxHealth();
        }

        if(thisRoundManager.ReturnPlayerLives() == 2){
            BobaLives1.SetActive(false);
            BobaLivesMeter2.fillAmount = thePlayersHealth.health / thePlayersController.getPlayerMaxHealth();
        }

        if(thisRoundManager.ReturnPlayerLives() == 1){
            BobaLives2.SetActive(false);
            BobaLivesMeter3.fillAmount = thePlayersHealth.health / thePlayersController.getPlayerMaxHealth();
        }
        if(thisRoundManager.ReturnPlayerLives() == 0){
            BobaLives1.SetActive(false);
        }
    }
}
