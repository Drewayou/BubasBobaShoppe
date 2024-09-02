using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameClockTimeScriptBobaShop : MonoBehaviour
{

    //This game object's timer object to put the text value of it's time.
    [SerializeField]
    [Header("This ClockTimer")]
    [Tooltip("The Game Object for this clock timer")]
    GameObject thisClockTimerObject;

    //This clock timer text
    TMP_Text thisClockTimerTXMP;

    //This round's object's value of it's time.
    [SerializeField]
    [Header("RoundManager")]
    [Tooltip("This Round Manager's Game Object to pull current rount time")]
    GameObject thisGameRoundManager;

    BobaShopRoundManagerScript thisGameRoundManagerScript;

    private float currentInRoundTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        thisClockTimerTXMP = thisClockTimerObject.GetComponent<TMP_Text>();
        thisClockTimerTXMP.text= "7:00am";
        
        thisGameRoundManagerScript = thisGameRoundManager.GetComponent<BobaShopRoundManagerScript>();
        currentInRoundTime = thisGameRoundManagerScript.getRoundTime();
    }

    // Update is called once per frame
    void Update()
    {
       
        currentInRoundTime = thisGameRoundManagerScript.getRoundTime();
        currentInRoundTime = Mathf.Round(currentInRoundTime * 10.0f) * 0.1f;
        
        // If round timer is NOT greater than 3min (180s) update the time for other scripts use. Otherwise, don't increase the timer.

        //Debug.Log(currentInRoundTime);

        if(currentInRoundTime < 180f){
            if(15f<currentInRoundTime && currentInRoundTime<30f){
            thisClockTimerTXMP.text="8:00am";
        }
        if(currentInRoundTime>=30f && currentInRoundTime<45f){
            thisClockTimerTXMP.text="9:00am";
        }
        if(currentInRoundTime>=45f && currentInRoundTime<60f){
            thisClockTimerTXMP.text="10:00am";
        }
        if(currentInRoundTime>=60f && currentInRoundTime<80f){
            thisClockTimerTXMP.text="11:00am";
        }
        if(currentInRoundTime>=80f && currentInRoundTime<90f){
            thisClockTimerTXMP.text="12:00pm";
        }
        if(currentInRoundTime>=90f && currentInRoundTime<95f){
            thisClockTimerTXMP.text="1:00pm";
        }
        if(currentInRoundTime>=95f && currentInRoundTime<120f){
            thisClockTimerTXMP.text="1:00pm";
        }
        if(currentInRoundTime>=120f && currentInRoundTime<130f){
            thisClockTimerTXMP.text="2:00pm";
        }
        if(currentInRoundTime>=130f && currentInRoundTime<140f){
            thisClockTimerTXMP.text="3:00pm";
        }
        if(currentInRoundTime>=140f && currentInRoundTime<150f){
            thisClockTimerTXMP.text="4:00pm";
        }
        if(currentInRoundTime>=150f && currentInRoundTime<160f){
            thisClockTimerTXMP.text="5:00pm";
        }
        if(currentInRoundTime>=160f && currentInRoundTime<170f){
            thisClockTimerTXMP.text="6:00pm";
        }
        if(currentInRoundTime>=170f){
            thisClockTimerTXMP.text="7:00pm";
        }
        }

        
    }
}
