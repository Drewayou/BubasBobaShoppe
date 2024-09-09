using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    //The object that contains the UI Object to update the user on how many items the boba pot holds.
    [SerializeField]
    [Tooltip("Input the \"BobaPotAmmountUI\" item in here to allow the user to see how many ingredients are in the pot")]    
    public GameObject bobaPotItemsAmmountObject;

    //The two objects in the pot item UI to show what item is in the pot.
    public GameObject IngredientInPotPopupUI;

    //The itemInHandInventory object of this round will automatically be pulled in Start() method.   
    private TMP_Text bobaPotItemsAmmountText;

    //The Animator Controllor of the boba pot object & itemInHandInventory of this round will automatically be pulled in Start() method.
    private Animator animationController, itemInHandInventoryAnimator;

    //A private int to dictate what stage the cooking pot is at.
    private int bobaPotCookingTimer;

    //A public int to dictate maximum items in pot & how many items are in the pot currently.
    public int bobaToppingsIngredientsInPot, bobaToppingMaximumInPot;

    //The ammount of time (in seconds) to dictate how fast the boba pot cooks topings. Called from player game stats.
    public float bobaPotCookingSpeed;

    //The name of ingredient placed inside the boba pot.
    public string bobaIngredientInPot;

    //The Pre-fabs for all the possible Ingredient Items.
    [SerializeField]
    [Tooltip("Add all the gameobject UI's for the possible BOBA POT ingredients below!")]
    public GameObject IngredientsInPotCassavaBall,IngredientsInPotMango,IngredientsInPotRedBean;

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

        //Pulls the objects of the boba pot's UI that appears when the user hovers over the pot.
        bobaPotItemsAmmountText = bobaPotItemsAmmountObject.GetComponent<TMP_Text>();
        IngredientInPotPopupUI = bobaPotItemsAmmountObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This block of code handles what comes into the Boba Pot and updating the player's data.
    public void PlacedToppingRelatedItemIntoPot(){

        //Check if the pot has an item in it.
        if(bobaToppingsIngredientsInPot == 0){

        InteractWithEmptyBobaPot();
        //If there is an item in the pot, only allow said item in pot to be placed/pulled out... Or activate pot if
        //The player has nothing in their hand!

        //Else if the pot already has ingredients in it, interact depending on what the player has in their hand if the pot isn't filled.
        }else if(bobaToppingsIngredientsInPot < bobaToppingMaximumInPot && gameObject.transform.childCount != 0 && itemInHandInventory.transform.childCount>=2){
            
            //Play the "Item Placed In" animation
            DropCookingItemInBobaPotAnimationAndExtras();

            switch(bobaIngredientInPot){
                case "Boba":

                if(itemInHandInventory.transform.GetChild(1).name=="ShopCassavaBall(Clone)"){

                        //Move the item in hand into the boba pot parent.
                        itemInHandInventory.transform.GetChild(1).transform.SetParent(gameObject.transform);
                        bobaToppingsIngredientsInPot += 1;
                        
                }else if(itemInHandInventory.transform.GetChild(1).name=="BTongHolding(Clone)"){
                        //Move the item in the boba pot into the hand parent object.
                        gameObject.transform.GetChild(0).transform.SetParent(itemInHandInventory.transform);
                        //Get item in hand inventory, and move the newly placed item in index 2 -> 1 for the tong object to render correctly.
                        itemInHandInventory.transform.GetChild(2).SetSiblingIndex(1);
                        bobaToppingsIngredientsInPot -= 1;

                        //Play the animation of taking out the boba and resize the item. Moreover, check if the pot is empty & do actions if it is.
                        TakeCookingItemFromBobaPotAnimationAndExtras();
                }

                break;
            }
        }
    }

    //FIXME: This block of code simply does the boba pot animation and removes the item from the Tongs!
    public void StartBobaPotCookingItemAnimationAndExtras(){
        animationController.Play("BobaPotCooking");
        animationController.keepAnimatorStateOnDisable = true;
        Destroy(itemInHandInventory.transform.GetChild(1).gameObject);
    }

    //This block of code simply does the boba pot ingredient placed in animation and removes the item from the Tongs!
    public void DropCookingItemInBobaPotAnimationAndExtras(){

        //Play the boba pot jiggle animation from item being placed in it.
        animationController.Play("BobaPotItemPlacedIn");
        animationController.keepAnimatorStateOnDisable = true;

        //transform the item from the hand into the area of the boba put.
        if(itemInHandInventory.transform.childCount>2){
            itemInHandInventory.transform.GetChild(1).position = new Vector3(0,-75f,0);
            itemInHandInventory.transform.GetChild(1).localScale = new Vector3(.5f,.5f,.5f);
            itemInHandInventory.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    //This block of code simply pulls back the boba pot ingredient take out in animation and removes the item from the Pot!
    public void TakeCookingItemFromBobaPotAnimationAndExtras(){

        //Play the boba pot jiggle animation from item being placed in it.
        animationController.Play("BobaPotItemPlacedIn");
        animationController.keepAnimatorStateOnDisable = true;

        //transform the item from the hand into the area of the boba put.
        if(itemInHandInventory.transform.childCount>2 && itemInHandInventory.transform.GetChild(2).name=="BTongHolding(Clone)"){
            itemInHandInventory.transform.GetChild(1).position = new Vector3(0,-.75f,0);
            itemInHandInventory.transform.GetChild(1).localScale = new Vector3(3,3,3);
            itemInHandInventory.transform.GetChild(1).gameObject.SetActive(true);
        }

        //Checks if the boba pot now has become empty, and resets it's stringtag, and hide item in pot ingredient popup.
        if(bobaToppingsIngredientsInPot == 0){
            bobaIngredientInPot = "";
            IngredientInPotPopupUI.SetActive(false);

            //Iterate through all the possible boba pot ingredients UI and turn them off.
            for(int i = 0; i < bobaPotItemsAmmountObject.transform.childCount; i++){
                bobaPotItemsAmmountObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    //This block of code simply does the boba action, and ONLY ACTIVATES when the pot is empty!.
    public void InteractWithEmptyBobaPot(){
        //Check if the hand inventory has an item.
            if(itemInHandInventory.transform.childCount >= 2){

                //Check if Tongs are holding something to put into the pot.
                if(itemInHandInventory.transform.GetChild(1)!=null){

                    switch(itemInHandInventory.transform.GetChild(1).name){
                        case "ShopCassavaBall(Clone)":
                        bobaToppingsIngredientsInPot += 1;
                        bobaIngredientInPot = "Boba";
                        IngredientsInPotCassavaBall.SetActive(true);
                        IngredientInPotPopupUI.SetActive(true);

                        //Play the "Item Placed In" animation
                        DropCookingItemInBobaPotAnimationAndExtras();

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

    public void OnMouseEnter(){
        bobaPotItemsAmmountObject.SetActive(true);
        print("HoveringOverTheBobaPot");
    }

    public void OnMouseOver(){
        bobaPotItemsAmmountText.text = bobaToppingsIngredientsInPot.ToString() + "/" + bobaToppingMaximumInPot.ToString();
    }

    public void OnMouseExit(){
        bobaPotItemsAmmountObject.SetActive(false);
        print("NOT-HoveringOverTheBobaPot");
    }
}
