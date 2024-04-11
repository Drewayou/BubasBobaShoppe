using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesGatheredThisRoundScript : MonoBehaviour
{
    [SerializeField]
    public TMP_Text Cassava,Pandan,Banana,Strawberry,Mango,Ube;

    //Place this game round's object
    [SerializeField]
    [Header("Set RoundObject")]
    [Tooltip("This is for the Round Manager to pull it's scripts and calculate round essentials")]
    private GameObject thisRoundGameObject;
    private RoundManagerScript thisGamesRoundManager;

    // Start is called before the first frame update
    void Start()
    {

        thisGamesRoundManager = thisRoundGameObject.GetComponent<RoundManagerScript>();

        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(thisGamesRoundManager.GetCassavaSlimeBallsThisRound());
        Cassava.text = "x"+ thisGamesRoundManager.GetCassavaSlimeBallsThisRound();
        Pandan.text = "x"+ thisGamesRoundManager.GetPandanLeavesThisRound();
        Banana.text = "x"+ thisGamesRoundManager.GetBananaMinisThisRound();
        Strawberry.text = "x"+ thisGamesRoundManager.GetStrawberryMinisThisRound();
        Mango.text = "x"+ thisGamesRoundManager.GetMangoMinisThisRound();
        Ube.text = "x"+ thisGamesRoundManager.GetUbeMinisThisRound();
    }   
}
