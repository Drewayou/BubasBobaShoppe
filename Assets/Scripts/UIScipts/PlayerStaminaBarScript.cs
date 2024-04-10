using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerStaminaBarScript : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Drop the player object to track Stamina bar below")]
    [Header("Player Object for the round")]
    GameObject player;

    [SerializeField]
    [Tooltip("Drop the Stamina Bar object to track Stamina bar below")]
    [Header("Player Object for the round")]
    GameObject UIStaminaObject;

    [SerializeField]
    [Tooltip("Drop the Stamina PENALTY Bar object to track Stamina bar below")]
    [Header("Player Object for the round")]
    GameObject UIStaminaPENALTYObject;

    Player_Controller thisPlayerController;

    float playerCurrentStaminaInput, playerMaxStaminaInput, penaltyTimerRemaining, penaltyStartingTimeOut;

    //[Tooltip("Drop the player object to track HP bar below")]
    //[Header("Player Object for the round")]

    UnityEngine.UI.Image thisUIImageStamina;
    UnityEngine.UI.Image thisUIImagePenaltyTimer;

    // Start is called before the first frame update
    void Start()
    {
        //Link all reqired items
        player = GameObject.Find("Player");
        thisPlayerController = player.GetComponent<Player_Controller>();
        playerCurrentStaminaInput = thisPlayerController.currentPlayerStamina;
        playerMaxStaminaInput = thisPlayerController.playerMaxStamina;
        penaltyTimerRemaining = thisPlayerController.penaltyTimer;
        penaltyStartingTimeOut = thisPlayerController.penaltyTimeStaminaOut;

        //Pull the bar UI images to have the fill be animated
        thisUIImageStamina = UIStaminaObject.GetComponent<UnityEngine.UI.Image>();
        thisUIImagePenaltyTimer = UIStaminaPENALTYObject.GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //Set the fill values of the bars according to the required items. 
        //First get updated value from other script, then set.
        playerCurrentStaminaInput = thisPlayerController.returnCurrentStamina();
        thisUIImageStamina.fillAmount = playerCurrentStaminaInput / playerMaxStaminaInput;
        
        //Similar to above, but if bar is "Filled" (Timer hits 0) set to zero.
        penaltyTimerRemaining = thisPlayerController.returnCurrentPenaltyTimer();
        Debug.Log(penaltyTimerRemaining);
        if(penaltyTimerRemaining != 0f){
            thisUIImagePenaltyTimer.fillAmount = 1f - (penaltyTimerRemaining / penaltyStartingTimeOut);
        }else{
            thisUIImagePenaltyTimer.fillAmount = 0f;
        }
            
    }
}
