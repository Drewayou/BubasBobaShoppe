using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerWaitingHandlerScript : MonoBehaviour
{
    // Get the boba shop game manager script to pull data from.
    BobaShopRoundManagerScript thisRoundOverallInstanceScript;

    // ACTIVE List that holds the waiting-for-order customer Queue. Or customers waiting for their finished drink/product.
    [SerializeField]
    public List<GameObject> waitingForOrderCustomerQueue;

    // Timer for customers picking up their order (Their walk speed and aimations for them "picking up" their boba order at the sell mat).
    public float customerWalkingTimer = 0f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Find and load the BobaShopRound data.
        thisRoundOverallInstanceScript = GameObject.Find("BobaShopRoundManager").GetComponent<BobaShopRoundManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This method is used by the customer handler script to put the customers into this queue.
    public void AddCustomerToThisWatingQueue(GameObject customer){
        customer.transform.SetParent(this.gameObject.transform, false);
        waitingForOrderCustomerQueue.Add(customer);
        customer.GetComponent<Image>().raycastTarget = false;
        VisuallyMoveCustomerAcrossScreen(customer);
    }

    //FIXME: Add animation to this customer line when a new customer is added.
    //This method organizes the visual placement of the customers on the screen and sets their clickable mask on/off depending what place they get in line.
    public void VisuallyMoveCustomerAcrossScreen(GameObject customerWalksToWaitForDrink){
        //Turn off raycast on the customers to prevent clicking on them.
        customerWalksToWaitForDrink.GetComponent<Image>().raycastTarget = false;
        //FIXME: Add animation code here.
    }

    //FIXME: Add animation to method to show the customer "Picking up" their drinks.
    //This method is activated when the bell is pressed.
    public void VisuallyMoveCustomerToPickupOrder(){
        //If no customers are waiting in the queue and no customer is picking up their order (customerWalkingTimer isn't > 0).
        if(waitingForOrderCustomerQueue.Count!=0 && customerWalkingTimer <= 0){
        //Add the code and animations for customer picking up the drink. (Note, make sure to add a timer so that the customer HAS to "touch" and pickup the drinks).
        
        //The code for what happens when the timer hits 0 (customer is at the drink sell mat) is with the "CustomerDrinkScript".
        waitingForOrderCustomerQueue[0].GetComponent<CustomerDrinkScript>().DoCustomerOrderPickupLogic();
        //Remove customer from waiting for drink queue.
        waitingForOrderCustomerQueue.Remove(waitingForOrderCustomerQueue[0]);
        }
    }
}
