using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class BobaPotScript : MonoBehaviour
{
    //The Game manager instance of this round (Will automatically be pulled in Start() method.
    private GameManagerScript currentGameManagerInstance;

    //The itemInHandInventory object of this round will automatically be pulled in Start() method.
    private GameObject itemInHandInventory;

    //The Animator Controllor of the boba pot object & itemInHandInventory of this round will automatically be pulled in Start() method.
    private Animator animationController, itemInHandInventoryAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //Finds the Game Manager in this instance.
        currentGameManagerInstance = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();

        //Finds the Inventory object in this instance & it's animator.
        itemInHandInventory = GameObject.Find("ItemInHand");
        itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();

        //Finds the animator of this bobapot game object.
        animationController = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This block of code handles what comes into the Boba Pot and updating the player's data.
    public void placedToppingRelatedItemIntoPot(){

        //Check if the hand inventory has an item.
        if(itemInHandInventory.transform.childCount != 0){

            //Check if Tongs are holding something.
            if(itemInHandInventory.transform.GetChild(1)!=null){

                switch(itemInHandInventory.transform.GetChild(1).name){
                    case "ShopCassavaBall(Clone)":
                    doBobaPotItemAnimationAndExtras();
                    currentGameManagerInstance.ReturnPlayerStats().casavaBalls -= 1;
                    //BobaPotItemPlacedIn
                    break;

                    default:
                    itemInHandInventoryAnimator.Play("IncorrectInteraction");
                    print("Can't put that into the boba pot!");
                    break;
                }
            }
        }
    }

    //This block of code simply does the boba pot animation and removes the item from the Tongs!
    public void doBobaPotItemAnimationAndExtras(){
        animationController.speed = currentGameManagerInstance.ReturnPlayerStats().bobaShopBobaPotCookingSpeed;
                animationController.Play("BobaPotItemPlacedIn");
                animationController.keepAnimatorStateOnDisable = true;
                Destroy(itemInHandInventory.transform.GetChild(1).gameObject);
    }
}
