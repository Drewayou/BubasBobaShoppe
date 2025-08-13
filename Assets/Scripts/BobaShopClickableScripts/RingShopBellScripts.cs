using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingShopBellScripts : MonoBehaviour
{

    //This script handles the bell ringing interaction with the player, and initiates other scripts to respond to it.
    //CONNECTED TO "BellPrefab" Game Object.

    //Gets game Object to find the Game Manager.
    [SerializeField]
    [Tooltip("Drag the \"GameManagerObject\" object in here.")]
    GameObject gameManagerObject;

    //Gets Game Manager Script.
    GameManagerScript currentGameManagerInstance;

    //Gets game Object to check what the player is currently holding.
    [SerializeField]
    [Tooltip("Drag the \"itemInHandInventory\" object in here.")]
    GameObject itemInHandInventory;
    Animator itemInHandInventoryAnimator;

    //Gets game Object to tell the customers waiting for their drink in this queue to pickup thier order if possible.
    [SerializeField]
    [Tooltip("Drag the \"CustomerDrinkWaitQueueHandler\" object in here.")]
    GameObject customerWait4DrinkHandler;

    //The SFX prefab to make the bell ring sound.
    [SerializeField]
    [Tooltip("Drag the \"GameManagerObject\" object in here.")]
    GameObject bellSFXRing;

    //Gets the bell's animation.
    Animator bellsAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //Get the game manager objects & round manager objects.
        gameManagerObject = GameObject.Find("GameManagerObject");
        currentGameManagerInstance = gameManagerObject.GetComponent<GameManagerScript>();

        //Gets the item in hand animator to let the player know if they're doing the right interaction or not.
        //(Bell cannot be rung unless empty handed).
        itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();

        //Gets the bell's animator fo the ringning effect to play.
        bellsAnimator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //A script that runs when the bell is pressed.
    public void ringShopBell()
    {
        if (itemInHandInventory.transform.childCount == 0)
        {
            MakeBellNoise();
            //FIXME: Add a way to call the next customer in queue 
            CallCustomer();
        }
    }

    public void MakeBellNoise()
    {
        //FIXME: find a way to store all the SFX sound effects.
        Instantiate(bellSFXRing);
        bellsAnimator.Play("BellRing");
    }

    //This method contacts OrderPickupHandler script to start the customer drink pickup process.
    public void CallCustomer()
    {
        customerWait4DrinkHandler.GetComponent<CustomerOrderPickupScript>().CheckIfCustomersAreWaitingForDrinks();
    }
    
}
