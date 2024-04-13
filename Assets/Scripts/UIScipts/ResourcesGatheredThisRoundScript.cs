using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesGatheredThisRoundScript : MonoBehaviour
{

    [SerializeField]
    public GameObject CassavaObject,PandanObject,BananaObject,StrawberryObject,MangoObject,UbeObject;

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
        if(Cassava.text != "x0"){
            CassavaObject.SetActive(true);
        }
        Cassava.text = "x"+ thisGamesRoundManager.GetCassavaSlimeBallsThisRound();
        if(Pandan.text != "x0"){
            PandanObject.SetActive(true);
        }
        Pandan.text = "x"+ thisGamesRoundManager.GetPandanLeavesThisRound();
        if(Banana.text != "x0"){
            BananaObject.SetActive(true);
        }
        Banana.text = "x"+ thisGamesRoundManager.GetBananaMinisThisRound();
        if(Strawberry.text != "x0"){
            StrawberryObject.SetActive(true);
        }
        Strawberry.text = "x"+ thisGamesRoundManager.GetStrawberryMinisThisRound();
        if(Mango.text != "x0"){
            MangoObject.SetActive(true);
        }
        Mango.text = "x"+ thisGamesRoundManager.GetMangoMinisThisRound();
        if(Ube.text != "x0"){
            UbeObject.SetActive(true);
        }
        Ube.text = "x"+ thisGamesRoundManager.GetUbeMinisThisRound();
    }   
}
