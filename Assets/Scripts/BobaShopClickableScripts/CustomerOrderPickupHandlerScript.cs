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


    //Boba mat GameObject
    [SerializeField]
    [Tooltip("Drag and drop the \"DrinkPlacementMat\" game object here.")]
    GameObject thisBobaMatGameObject;

    //Boba mat script
    [SerializeField]
    [Tooltip("Drag and drop the \"DrinkPlacementMat\" game object here.")]
    BobaSellMattScript thisBobaMatScript;

    //Boba mat script
    [SerializeField]
    [Tooltip("Drag and drop the \"DrinkPlacementMat\" game object here.")]
    CustomerPayment4DrinkScript customerPayment4DrinkScript;

    // Gameobject list that holds the Customers who picked up their order before deleting them.
    [SerializeField]
    [Tooltip("Drag and drop the \"CustomerLeavingHandlerObject\" game object here.")]
    GameObject CustomerLeavingHandlerObject;

    //The queue list for "CustomerWaitingHandlerScript"
    public List<GameObject> thisOrderCustomerQueue;

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
        if (customerIsPickingUpDrinks && thisOrderCustomerQueue[0].GetComponent<CustomerPatienceUIScript>().customerIsAtFront && thisOrderCustomerQueue[0].GetComponent<CustomerPatienceUIScript>().customerIsWaitingForDrinks)
        {
            customerPayment4DrinkScript.customerIsTryingToPay = true;
        }
    }

    public void AttemptCustomerTakesDrinksAndPays()
    {
        Debug.LogWarning("Customer will try to grab RIGHT drinks!");
        if (thisOrderCustomerQueue.Count > 0)
        {
            if (customerIsPickingUpDrinks && thisOrderCustomerQueue[0].GetComponent<CustomerPatienceUIScript>().customerIsAtFront && thisOrderCustomerQueue[0].GetComponent<CustomerPatienceUIScript>().customerIsWaitingForDrinks)
            {
               thisBobaMatScript.GetComponent<CustomerPayment4DrinkScript>().AttemptToPerformPurchase();
            }
        }
    }

    public void AttemptCustomerLoosingPatienceButGrabbingDrinks()
    {
        Debug.LogWarning("Customer will try to grab ANY AND ALL drinks! RAHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
        if (customerIsPickingUpDrinks && thisOrderCustomerQueue[0].GetComponent<CustomerPatienceUIScript>().customerIsAtFront && thisOrderCustomerQueue[0].GetComponent<CustomerPatienceUIScript>().customerIsWaitingForDrinks)
        {
            thisBobaMatScript.GetComponent<CustomerPayment4DrinkScript>().AttemptToPerformPatienceRanOutPurchase();
        }
    }

    //FIXME :
    //If their timer runs out, remove their order from the list of order tabs on the screen. First 3 order tabs are shown, while the next ones in queue (Max depending on player upgrades),
    //are hidden but "shown under" the other tabs. Popularity lost is DOUBLE as compared as loosing patience in first queue (Similar to reality! Prevents player spamming orders).
    //Max of 3 order tabs at first, upgradable to 6 total tabs.
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
