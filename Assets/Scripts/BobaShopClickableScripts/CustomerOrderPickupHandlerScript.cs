using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using System.Linq;
using UnityEngine;

public class CustomerOrderPickupScript : MonoBehaviour
{
    // This script handles what happens if the bell is rung and there's a customer in the waiting queue wanting to pick up their order.
    // CONNECTED TO "CustomerDrinkWaitQueueHandler" Game Object.

    // Gameobject list that holds the Customers who picked up their order before deleting them.
    [SerializeField]
    [Tooltip("Drag and drop the \"CustomerLeavingHandlerObject\" game object here.")]
    GameObject CustomerLeavingHandlerObject;

    //This is for where the customers stop to pick up the order. Generally the position of the boba sell mat.
    float positionOfSellMatt = 477f;

    // Gameobject list that holds the Sellmat.
    [SerializeField]
    [Tooltip("Drag and drop the \"DrinkPlacementMat\" game object here.")]
    GameObject DrinkPlacementMat;

    //FIXME : IF CUSTOMERS are NOT in the queue and are off screen waiting for the bell to be rung, have their timer match the "Waiting in line".
    //If their timer runs out, remove their order from the list of order tabs on the screen. First 3 order tabs are shown, while the next ones in queue (Max depending on player upgrades),
    //are hidden but "shown under" the other tabs. Popularity lost is DOUBLE as compared as loosing patience in first queue (Similar to reality! Prevents player spamming orders).
    //Max of 3 order tabs at first, upgradable to 6 total tabs.
    //This method below generally runs AFTER the player calls for customers to pick up their drinks.
    //Checks if there's customers waiting to pick up their drink orders to start VISUAL patience and sell off processes.
    public void CheckIfCustomersAreWaitingForDrinks()
    {
        if (this.gameObject.GetComponent<CustomerWaitingHandlerScript>().waitingForOrderCustomerQueue.Count > 0)
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
            Debug.Log("No customer is waiting for a drink!");
        }
    }

    //FIXME: Trigger this method to also trigger the sell of the drinks
    // This method does the UI interactions and initiates the "CustomerPays4Drink" Script.
    public void NextCustomerPicksUpOrder()
    {
        StartCoroutine(LerpNPCPositionAnimation(positionOfSellMatt, this.gameObject.GetComponent<CustomerWaitingHandlerScript>().waitingForOrderCustomerQueue[0]));
        // Check the script list and remove the customer up next to move up the "line".
        this.gameObject.GetComponent<CustomerWaitingHandlerScript>().waitingForOrderCustomerQueue.RemoveAt(0);
        // Move the game object itself into the handler object to ensure the next customer waiting for their drink is up next.
        GameObject CustomerInQueue = this.gameObject.transform.GetChild(0).gameObject;
        CustomerInQueue.transform.SetParent(CustomerLeavingHandlerObject.gameObject.transform, false);
    }
    
    //This enum is a lerp for the NPC's position from the right side of the screen to pick up thier drink.
    public IEnumerator LerpNPCPositionAnimation(float newPositionDesired, GameObject NPCToMove)
    {
        float timeElapsed = 0;

        while (timeElapsed < NPCToMove.GetComponentInParent<CustomerDrinkScript>().characterShopSpeed)
        {
            float valueToLerp = Mathf.Lerp(NPCToMove.transform.localPosition.x, newPositionDesired, timeElapsed / NPCToMove.GetComponent<CustomerDrinkScript>().characterShopSpeed);
            NPCToMove.transform.localPosition = new Vector3(valueToLerp,-55,0);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        NPCToMove.transform.localPosition = new Vector3(NPCToMove.transform.localPosition.x,-55,0);
    }
}
