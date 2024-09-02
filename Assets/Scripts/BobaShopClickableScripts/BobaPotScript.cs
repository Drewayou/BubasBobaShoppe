using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class BobaPotScript : MonoBehaviour
{
    //The Game manager instance of this round (Will automatically be pulled in Start() method.
    private GameManagerScript currentGameManagerInstance;

    //The Game manager instance of this round (Will automatically be pulled in Start() method.
    private BobaShopRoundManagerScript currentRoundManagerInstance;

    //The itemInHandInventory object of this round will automatically be pulled in Start() method.
    private GameObject itemInHandInventory;

    //The Animator Controllor of the boba pot object & itemInHandInventory of this round will automatically be pulled in Start() method.
    private Animator animationController, itemInHandInventoryAnimator;

    //A private int to dictate what stage the cooking pot is at.
    private int bobaPotCookingTimer;

    //A public int to dictate maximum items in pot & how many items are in the pot currently.
    private int bobaToppingsIngredientsInPot, bobaToppingMaximumInPot;

    //The ammount of time (in seconds) to dictate how fast the boba pot cooks topings. Called from player game stats.
    public float bobaPotCookingSpeed;

    //The name of ingredient placed inside the boba pot.
    public string bobaIngredientInPot;

    // Start is called before the first frame update
    void Start()
    {
        //Finds the Game Manager in this instance.
        currentGameManagerInstance = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
        currentRoundManagerInstance = GameObject.Find("BobaShopRoundManager").GetComponent<BobaShopRoundManagerScript>();

        //Finds the Inventory object in this instance & it's animator.
        itemInHandInventory = GameObject.Find("ItemInHand");
        itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();

        //Finds the animator of this bobapot game object.
        animationController = gameObject.GetComponent<Animator>();

        //Pulls the game stats to program the pot's cooking speed.
        bobaPotCookingSpeed = currentGameManagerInstance.ReturnPlayerStats().bobaShopBobaPotCookingSpeed;

        //Pulls the game stats to program the pot's maximum capacity.
        bobaToppingMaximumInPot = currentGameManagerInstance.ReturnPlayerStats().maxCapacityOfBobaPot;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This block of code handles what comes into the Boba Pot and updating the player's data.
    public void PlacedToppingRelatedItemIntoPot(){

        //Check if the pot has an item in it.
        if(bobaToppingsIngredientsInPot == 0){

        interactWithBobaPot();
        //If there is an item in the pot, only allow said item in pot to be placed/pulled out... Or activate pot if
        //The player has nothing in their hand!

        //Else if the pot already has ingredients in it, interact depending on what the player has in their hand if the pot isn't filled.
        }else if(bobaToppingsIngredientsInPot < bobaToppingMaximumInPot && gameObject.transform.childCount != 0 && itemInHandInventory.transform.childCount>2){
            switch(bobaIngredientInPot){
                case "Boba":

                if(itemInHandInventory.transform.GetChild(1).name=="ShopCassavaBall(Clone)"){

                        //Move the item in hand into the boba pot parent.
                        itemInHandInventory.transform.GetChild(1).transform.SetParent(gameObject.transform);
                        bobaToppingsIngredientsInPot += 1;
                        
                    }else if(itemInHandInventory.transform.GetChild(1).name=="BTongHolding(Clone)"){

                        //Move the item in the boba pot into the hand parent object.
                        itemInHandInventory.transform.GetChild(1).transform.SetParent(itemInHandInventory.transform);
                        //Get item in hand inventory, and move the newly placed item in index 2 -> 1 for the tong object to render correctly.
                        itemInHandInventory.transform.GetChild(2).SetSiblingIndex(1);
                        bobaToppingsIngredientsInPot -= 1;
                }

                break;
            }
        }
    }

    //This block of code simply does the boba pot animation and removes the item from the Tongs!
    public void doBobaPotItemAnimationAndExtras(){
        animationController.Play("BobaPotItemPlacedIn");
        animationController.keepAnimatorStateOnDisable = true;
        Destroy(itemInHandInventory.transform.GetChild(1).gameObject);
    }

    //This block of code adjusts an interaction depending on what the player is holding.
    public void InteractWithIngredientsInBobaPot(string desiredPrefabName){

    }

    //This block of code simply does the boba action depending on what's in the pot.
    public void interactWithBobaPot(){
        //Check if the hand inventory has an item.
            if(itemInHandInventory.transform.childCount >= 2){

                //Check if Tongs are holding something to put into the pot.
                if(itemInHandInventory.transform.GetChild(1)!=null){

                    switch(itemInHandInventory.transform.GetChild(1).name){
                        case "ShopCassavaBall(Clone)":
                        bobaToppingsIngredientsInPot += 1;
                        bobaIngredientInPot = "Boba";

                        //Move the item in hand into the boba pot parent.
                        itemInHandInventory.transform.GetChild(1).transform.SetParent(gameObject.transform);
                        break;

                        default:
                        itemInHandInventoryAnimator.Play("IncorrectInteraction");
                        print("Can't put that into the boba pot!");
                        break;
                    }
                }
            }else{itemInHandInventoryAnimator.Play("IncorrectInteraction");}
    }
}
