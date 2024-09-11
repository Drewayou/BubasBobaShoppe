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

    [SerializeField]
    [Tooltip("Drag the boba ladle object prefab in here.")]   
    //Drag the boba ladle object PREFAB in here.
    private GameObject bobaLadle;

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

    //The name of ingredient placed inside the boba pot.
    public string bobaIngredientInPotStringType;

    //A public int to dictate maximum items in pot & how many items are in the pot currently.
    public int bobaToppingsIngredientsInPotCurrentAmount, bobaToppingMaximumInPot;

    //The ammount of time (in seconds) to dictate how fast the boba pot cooks topings. Called from player game stats.
    public float bobaPotCookingSpeed;

    //The variables to judge the ammount of time currently held by the boba pot if it's done cooking or not.
    public float bobaPotCookingEndTime;
    public float bobaPotRoundTimerDifference;

    //The phase times and phase time divisable for each animation loading for the boba pot.
    public float phase1Time, phase2Time, phase3Time, phase4Time, phase5Time, calculatedPhaseDivide;

    //The boba pot bool if it's cooking/donecooking/dirty.
    public bool isPotCooking = false, isPotDoneCooking = false, isPotDirty = false;

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

        //Sets cooking timer difference to 0.
        bobaPotRoundTimerDifference = 0;

        //Pulls the game stats to program the pot's maximum capacity.
        bobaToppingMaximumInPot = currentGameManagerInstance.ReturnPlayerStats().maxCapacityOfBobaPot;

        //Pulls the objects of the boba pot's UI that appears when the user hovers over the pot.
        bobaPotItemsAmmountText = bobaPotItemsAmmountObject.GetComponent<TMP_Text>();
        IngredientInPotPopupUI = bobaPotItemsAmmountObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Set PotTimerDifference to sync with the round timer.
        bobaPotRoundTimerDifference = currentRoundManagerInstance.roundTimer;

        if(isPotCooking){
        //Check if the round timer is greater than the time required to finish cooking boba once activated.
        if(bobaPotRoundTimerDifference<=bobaPotCookingEndTime){
            if(phase1Time > bobaPotRoundTimerDifference && !animationController.GetCurrentAnimatorStateInfo(0).IsName("BobaPotCookingPhase1_5")){
                animationController.Play("BobaPotCookingPhase1");
            }else if(phase2Time > bobaPotRoundTimerDifference){
                animationController.Play("BobaPotCookingPhase2");
            }else if(phase3Time > bobaPotRoundTimerDifference){
                animationController.Play("BobaPotCookingPhase3");
            }else if(phase4Time > bobaPotRoundTimerDifference){
                animationController.Play("BobaPotCookingPhase4");
            }else if(phase5Time > bobaPotRoundTimerDifference){
                animationController.Play("BobaPotCookingPhase5");
            }
        }
        //When boba pot hits the timer, toppings are cooked.
            if(bobaPotCookingEndTime < bobaPotRoundTimerDifference){
                isPotDoneCooking = true;
            }
        }
    }

    //NOTE: MAIN INTERACTION METHOD. -----------------------------------------------------------------------------------
    //This block of code handles what comes into the Boba Pot and updating the player's data.
    public void InteractOrPlaceToppingRelatedItemIntoPot(){

        //Check if the pot is dirty or cooking. Stop interactions if it is.
        if(!isPotDirty && !isPotCooking){

        //Check if the pot has an item in it.
        if(bobaToppingsIngredientsInPotCurrentAmount == 0){

        InteractWithEmptyBobaPot();
        //If there is an item in the pot, only allow said item in pot to be placed/pulled out... Or activate pot if
        //The player has nothing in their hand!

        //Else if the pot already has ingredients in it, interact depending on what the player has in their hand if the pot isn't filled.
        }else if(bobaToppingsIngredientsInPotCurrentAmount < bobaToppingMaximumInPot && gameObject.transform.childCount != 0 && itemInHandInventory.transform.childCount >= 2){
            
            //Play the "Item Placed In" animation
            DropCookingItemInBobaPotAnimationAndExtras();

            switch(bobaIngredientInPotStringType){
                case "Boba":

                if(itemInHandInventory.transform.GetChild(1).name=="ShopCassavaBall(Clone)"){

                        //Move the item in hand into the boba pot parent.
                        itemInHandInventory.transform.GetChild(1).transform.SetParent(gameObject.transform);
                        bobaToppingsIngredientsInPotCurrentAmount += 1;
                        
                }else if(itemInHandInventory.transform.GetChild(1).name=="BTongHolding(Clone)"){
                        //Move the item in the boba pot into the hand parent object.
                        gameObject.transform.GetChild(0).transform.SetParent(itemInHandInventory.transform);
                        //Get item in hand inventory, and move the newly placed item in index 2 -> 1 for the tong object to render correctly.
                        itemInHandInventory.transform.GetChild(2).SetSiblingIndex(1);
                        bobaToppingsIngredientsInPotCurrentAmount -= 1;

                        //Play the animation of taking out the boba and resize the item. Moreover, check if the pot is empty & do actions if it is.
                        TakeCookingItemFromBobaPotAnimationAndExtras();
                }
                break;
            }
        //If the player clicks the boba pot without anything in their hand, and if it has items in it, set the pot timers to start cooking!
        }else if(gameObject.transform.childCount != 0 && itemInHandInventory.transform.childCount == 0){
            bobaPotCookingEndTime = currentRoundManagerInstance.roundTimer + bobaPotCookingSpeed;

            calculatedPhaseDivide = bobaPotCookingSpeed / 5;

            phase5Time = bobaPotRoundTimerDifference + calculatedPhaseDivide * 5;
            phase4Time = bobaPotRoundTimerDifference + calculatedPhaseDivide * 4;
            phase3Time = bobaPotRoundTimerDifference + calculatedPhaseDivide * 3;
            phase2Time = bobaPotRoundTimerDifference + calculatedPhaseDivide * 2;
            phase1Time = bobaPotRoundTimerDifference + calculatedPhaseDivide * 1;

            isPotCooking = true;
            CookingIngredientsToBeRemoveFromPlayerOverallTotalInventory();
            }
        }

    //If the boba pot is done cooking, allow interactions with the boba ladle.
    if(isPotDoneCooking){
        //Check if the player is holding an empty ladle, if they are, do action according to what item is in the pot.
        GameObject ladleInInventory = itemInHandInventory.transform.GetChild(0).gameObject;
        
        if(ladleInInventory.transform.GetChild(0).gameObject.activeSelf){
            switch(bobaIngredientInPotStringType){
                case "Boba":
                
                ladleInInventory.transform.Find("BobaLadelEmpty").gameObject.SetActive(false);
                ladleInInventory.transform.Find("BobaLadelBobaHeld").gameObject.SetActive(true);
                break;

                case "Lychee":
                ladleInInventory.transform.Find("BobaLadelEmpty").gameObject.SetActive(false);
                ladleInInventory.transform.Find("BobaLadelLycheeHeld").gameObject.SetActive(true);
                break;

                case "RedBean":
                ladleInInventory.transform.Find("BobaLadelEmpty").gameObject.SetActive(false);
                ladleInInventory.transform.Find("BobaLadelRedBeanHeld").gameObject.SetActive(true);
                break;
            }
            //Transfer the topping ammounts to the ladle accordingly.
            TransferAmmountReadjustment(ladleInInventory);
            }
        }
    }

    //A method to make sure that the ladle gets at much from the pot as possible.
    public void TransferAmmountReadjustment(GameObject ladleInInventoryObjectPassthrough ){
        //If the boba ladle has a greater capacity than the pot, transfer the rest of the pot to the ladle.
        if(ladleInInventoryObjectPassthrough.GetComponent<BobaLadelScript>().GetMaxCarryLadleAmmount()>bobaToppingsIngredientsInPotCurrentAmount){
            ladleInInventoryObjectPassthrough.GetComponent<BobaLadelScript>().SetAmmountOfIngredientsInLadle(bobaToppingsIngredientsInPotCurrentAmount);
            ladleInInventoryObjectPassthrough.GetComponent<BobaLadelScript>().SubtractLadleUsesByInput(bobaToppingsIngredientsInPotCurrentAmount);
            bobaToppingsIngredientsInPotCurrentAmount = 0; 
            isPotCooking = false;
            isPotDoneCooking = false;
            //FIXME: Eventually have a way for the pot to be dirty as well after each use.
            //isPotDirty = false;

        //Else, max out the ladle and take as much from the pot as possible.
        }else{
            ladleInInventoryObjectPassthrough.GetComponent<BobaLadelScript>().SetAmmountOfIngredientsToLadleMax();
            ladleInInventoryObjectPassthrough.GetComponent<BobaLadelScript>().SubtractLadleUsesByMaxCarrySize();
            bobaToppingsIngredientsInPotCurrentAmount -= ladleInInventoryObjectPassthrough.GetComponent<BobaLadelScript>().GetMaxCarryLadleAmmount();
        }
    }

    //FIXME: This block of code simply does the boba pot animation and removes the item from the Tongs!
    public void StartBobaPotCookingItemAnimationAndExtras(){
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
        if(bobaToppingsIngredientsInPotCurrentAmount == 0){
            bobaIngredientInPotStringType = "";
            IngredientInPotPopupUI.SetActive(false);

            //Iterate through all the possible boba pot ingredients UI and turn them off.
            for(int i = 0; i < bobaPotItemsAmmountObject.transform.childCount; i++){
                bobaPotItemsAmmountObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    //This block of code simply does the boba action, and ONLY ACTIVATES when the pot is empty!.
    public void InteractWithEmptyBobaPot(){
        //Check if the hand inventory has an item, and if pot is clean/not cookling.
            if(itemInHandInventory.transform.childCount >= 2){

                //Check if Tongs are holding something to put into the pot.
                if(itemInHandInventory.transform.GetChild(1)!=null){

                    switch(itemInHandInventory.transform.GetChild(1).name){
                        case "ShopCassavaBall(Clone)":
                        bobaToppingsIngredientsInPotCurrentAmount += 1;
                        bobaIngredientInPotStringType = "Boba";
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

    //FIXME: add the other toppings here in the future when boba pot is done cooking.
    public void CookingIngredientsToBeRemoveFromPlayerOverallTotalInventory(){
        switch(bobaIngredientInPotStringType){
            case "Boba": 
            currentGameManagerInstance.ReturnPlayerStats().casavaBalls -= bobaToppingsIngredientsInPotCurrentAmount;
            break;
        }
    }

    public void OnMouseEnter(){
        bobaPotItemsAmmountObject.SetActive(true);
        print("HoveringOverTheBobaPot");
    }

    public void OnMouseOver(){
        bobaPotItemsAmmountText.text = bobaToppingsIngredientsInPotCurrentAmount.ToString() + "/" + bobaToppingMaximumInPot.ToString();
    }

    public void OnMouseExit(){
        bobaPotItemsAmmountObject.SetActive(false);
        print("NOT-HoveringOverTheBobaPot");
    }

    //Getter to obtain how many items are in the pot.
    public string GetBobaPotIngredientType(){
        return bobaIngredientInPotStringType;
    }

    //Getter to obtain what string of item the pot has.
    public string GetBobaPotIngredientsInPot(){
        return bobaIngredientInPotStringType;
    }
}
