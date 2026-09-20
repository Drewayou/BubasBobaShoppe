using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CustomerOrderPickupScript : MonoBehaviour
{
    // This script handles what happens if the bell is rung and there's a customer in the waiting queue wanting to pick up their order.
    // CONNECTED TO "CustomerDrinkWaitQueueHandler" Game Object.

    // Get the boba shop game manager script to pull data from.
    BobaShopRoundManagerScript thisRoundOverallInstanceScript;

    // Gameobject list that holds the Customers who picked up their order before deleting them.
    [SerializeField]
    [Tooltip("Drag and drop the \"CustomerLeavingHandlerObject\" game object here.")]
    GameObject CustomerLeavingHandlerObject;

    //The queue list for "CustomerWaitingHandlerScript"
    List<GameObject> thisOrderCustomerQueue;

    //The script for "CustomerWaitingHandlerScript"
    CustomerWaitingForDrinkHandlerScript thisOrderCustomerOtherScript;

    //This is for where the customers stop to pick up the order. Generally the position of the boba sell mat.
    public float xPositionOfSellMatt = 477f;

    // Bool to ensure if a customer is ready to pick up a drink.
    public bool customerIsPickingUpDrinks = false;

    // Gameobject list that holds the Sellmat.
    [SerializeField]
    [Tooltip("Drag and drop the \"DrinkPlacementMat\" game object here.")]
    GameObject DrinkPlacementMat;

    void Start()
    {
        //Find and load the BobaShopRound data.
        thisRoundOverallInstanceScript = GameObject.Find("BobaShopRoundManager").GetComponent<BobaShopRoundManagerScript>();
        thisOrderCustomerQueue = this.gameObject.GetComponent<CustomerWaitingForDrinkHandlerScript>().waitingForOrderCustomerQueue;
        thisOrderCustomerOtherScript = this.gameObject.GetComponent<CustomerWaitingForDrinkHandlerScript>();
    }

    private void Update()
    {
        //Call the purchase timer if there is a customer waiting to pick up drinks.
        if (this.gameObject.transform.childCount > 1 && (thisOrderCustomerQueue[0].GetComponent<CustomerPatienceUIScript>() == null))
        {
            if (customerIsPickingUpDrinks)
            {
                //
            }
        }
    }

    //FIXME : IF CUSTOMERS are NOT in the queue and are off screen waiting for the bell to be rung, have their timer match the "Waiting in line".
    //If their timer runs out, remove their order from the list of order tabs on the screen. First 3 order tabs are shown, while the next ones in queue (Max depending on player upgrades),
    //are hidden but "shown under" the other tabs. Popularity lost is DOUBLE as compared as loosing patience in first queue (Similar to reality! Prevents player spamming orders).
    //Max of 3 order tabs at first, upgradable to 6 total tabs.
    //This method below generally runs AFTER the player calls for customers to pick up their drinks.
    //Checks if there's customers waiting to pick up their drink orders to start VISUAL patience and sell off processes.
    public void CheckIfCustomersAreWaitingForDrinks()
    {
        bool calledNextOrder = false;

        foreach (Transform customer in this.gameObject.transform)
        {
            if (!calledNextOrder) { 
                calledNextOrder = true;
                for (int i = 0; i < this.gameObject.transform.childCount; i++)
                {
                    if (i == 0)
                    {
                        thisOrderCustomerQueue[i].GetComponent<Image>().raycastTarget = false;
                        //Check if customer has infinite patience
                        if (thisOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>() == null)
                        {
                            if (!customerIsPickingUpDrinks)
                            {
                                NextCustomerPicksUpOrder();
                            }
                        }
                        else
                        {
                            //Set this customer as the front customer with the patience icon. Pull in customer's patience for giving their order.
                            if (!thisOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>().customerIsAtFront && !customerIsPickingUpDrinks)
                            {
                                thisOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>().customerIsWaitingForDrinks = true;
                                NextCustomerPicksUpOrder();
                            }
                        }
                    }
                    Debug.Log("Lots more customers waiting for a drink pickup!");
                }
            }
        }

        if (this.gameObject.GetComponent<CustomerWaitingForDrinkHandlerScript>().waitingForOrderCustomerQueue.Count == 1)
        {

            //FIXME: Customer starts patience and it runs all over again.
            //Patience will run until they see ALL their drinks in the drink mat.
            //If patience runs out while they're at the mat, they will take (up to their order limit) as many drinks on the mat as possible.
            //Wrong drinks per customer causes them to pay WAY less (If if the value of the drink they ordered is higher), or they will pay "At their Order" cost if the taken drink has more value.
            //Tips are NOT given if customer reaches 0 patience.
            //Patience should - INCREASE POPULARITY BASED OF HOW MUCH PATIENCE WAS LEFT. Sad->no patience left (1/4th or lower) grants nothing.

            //FIXME: Trigger this method to also trigger the sell of the drinks
            //NextCustomerPicksUpOrder();
        }
        else
        {
            Debug.Log("No customers are waiting for a drink!");
        }
    }

    //FIXME: Trigger this method to also trigger the sell of the drinks
    // This method does the UI interactions and initiates the "CustomerPays4Drink" Script.
    public void NextCustomerPicksUpOrder()
    {
        customerIsPickingUpDrinks = true;
        StartCoroutine(thisRoundOverallInstanceScript.LerpNPCPositionAnimation(xPositionOfSellMatt, this.gameObject.GetComponent<CustomerWaitingForDrinkHandlerScript>().waitingForOrderCustomerQueue[0]));
    }

    public void CustomerWaitingForDrinksLeft()
    {
        customerIsPickingUpDrinks = false;
    }


}
