using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class CustomerWaitingHandlerScript : MonoBehaviour
{
    // This script handles the player interaction and a couple UI tabs to recieve orders from the customer and move them to the waiting handler game object.
    // CONNECTED TO "CustomerQueueHandler" Game Object.

    // Get the boba shop game manager script to pull data from.
    BobaShopRoundManagerScript thisRoundOverallInstanceScript;

    // ACTIVE List that holds the waiting-for-order customer Queue. Or customers waiting for their finished drink/product.
    [SerializeField]
    public List<GameObject> waitingForOrderCustomerQueue;

    // Timer for customers picking up their order (Their walk speed and aimations for them "picking up" their boba order at the sell mat).
    public float customerWalkingTimer = 0f;

    // Timer for customers in order queue (Generally double the base time in player stats)
    public float customerPatienceForGettingOrder;

    // Script for the drink mat to be pulled.
    [SerializeField]
    [Tooltip("Drag and drop the boba sell mat here to get it's script code.")]
    private BobaSellMattScript bobaSellMatScript;

    [SerializeField]
    [Tooltip("Drag and drop the order tabs to use their scripts and populate the order tabs.")]
    OrderTabUIGeneratorScript OrderTab1Script,OrderTab2Script,OrderTab3Script;

    [SerializeField]
    [Tooltip("Drag and drop the order tabs GAMEOJECTS to use their scripts and populate the order tabs.")]
    GameObject OrderTab1GameObject,OrderTab2GameObject,OrderTab3GameObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Find and load the BobaShopRound data.
        thisRoundOverallInstanceScript = GameObject.Find("BobaShopRoundManager").GetComponent<BobaShopRoundManagerScript>();
        customerPatienceForGettingOrder = thisRoundOverallInstanceScript.customerOverallPatienceThisRound * 2;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPatienceOfWaitingCustomers();
    }

    // This is called when this script is on. Makes sure the customers in this queue are off screen.
    void OnEnable()
    {
        RecheckCustomerVisuals();
    }

    //This method is used by the customer handler script to put the customers into this queue.
    public void AddCustomerToThisWatingQueue(GameObject customer){
        customer.transform.SetParent(this.gameObject.transform, false);
        customer.transform.SetSiblingIndex(0);
        waitingForOrderCustomerQueue.Add(customer);
        SetCorespondingTimer(customer);
        customer.GetComponent<Image>().raycastTarget = false;
        VisuallyMoveCustomerAcrossScreen(customer);
        UpdateOrderTabs();
    }

    //FIXME: Add animation to this customer line when a new customer is added.
    //This method organizes the visual placement of the customers on the screen and sets their clickable mask on/off depending what place they get in line.
    public void VisuallyMoveCustomerAcrossScreen(GameObject customerWalksToWaitForDrink){
        //Turn off raycast on the customers to prevent clicking on them.
        customerWalksToWaitForDrink.GetComponent<Image>().raycastTarget = false;
        //FIXME: Add animation code here.
    }

    //Trigger the order tabs to update.
    public void UpdateOrderTabs(){
        if(waitingForOrderCustomerQueue.Count == 1){
            OrderTab1GameObject.SetActive(true);
            OrderTab1Script.GenerateDrinkTabUI();
        }
        if(waitingForOrderCustomerQueue.Count == 2){
            OrderTab2GameObject.SetActive(true);
            OrderTab2Script.GenerateDrinkTabUI();
        }
        if(waitingForOrderCustomerQueue.Count == 3){
            OrderTab3GameObject.SetActive(true);
            OrderTab3Script.GenerateDrinkTabUI();
        }
    }

    //Place all the customers in this queue off screen.
    public void RecheckCustomerVisuals(){
        foreach(GameObject customer in waitingForOrderCustomerQueue){
            Vector3 offScreenParams = new Vector3(-1200f,0f,0f);
            customer.transform.localPosition = offScreenParams;
        }
    }

    //Set the timer in this waiting for drinks made queue when customer joins
    public void SetCorespondingTimer(GameObject customerToAdjustTimer)
    {
        switch (waitingForOrderCustomerQueue.Count)
        {
            case 1:
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO1 = (customerToAdjustTimer.GetComponent<CustomerPatienceUIScript>().customerOrderWaitingTime * 2) + thisRoundOverallInstanceScript.roundTimer;
                thisRoundOverallInstanceScript.customerStartingTimeInDO1 = thisRoundOverallInstanceScript.roundTimer;
                return;
            case 2:
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO2 = (customerToAdjustTimer.GetComponent<CustomerPatienceUIScript>().customerOrderWaitingTime * 2) + thisRoundOverallInstanceScript.roundTimer;
                thisRoundOverallInstanceScript.customerStartingTimeInDO2 = thisRoundOverallInstanceScript.roundTimer;
                return;
            case 3:
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO3 = (customerToAdjustTimer.GetComponent<CustomerPatienceUIScript>().customerOrderWaitingTime * 2) + thisRoundOverallInstanceScript.roundTimer;
                thisRoundOverallInstanceScript.customerStartingTimeInDO3 = thisRoundOverallInstanceScript.roundTimer;
                return;
            case 4:
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO4 = (customerToAdjustTimer.GetComponent<CustomerPatienceUIScript>().customerOrderWaitingTime * 2) + thisRoundOverallInstanceScript.roundTimer;
                thisRoundOverallInstanceScript.customerStartingTimeInDO4 = thisRoundOverallInstanceScript.roundTimer;
                return;
            case 5:
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO5 = (customerToAdjustTimer.GetComponent<CustomerPatienceUIScript>().customerOrderWaitingTime * 2) + thisRoundOverallInstanceScript.roundTimer;
                thisRoundOverallInstanceScript.customerStartingTimeInDO5 = thisRoundOverallInstanceScript.roundTimer;
                return;
            case 6:
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO6 = (customerToAdjustTimer.GetComponent<CustomerPatienceUIScript>().customerOrderWaitingTime * 2) + thisRoundOverallInstanceScript.roundTimer;
                thisRoundOverallInstanceScript.customerStartingTimeInDO6 = thisRoundOverallInstanceScript.roundTimer;
                return;
        }
    }

    //Check the customers patience in this queue
    public void CheckPatienceOfWaitingCustomers()
    {
        if (thisRoundOverallInstanceScript.customerTimerInDO1 < 0.1)
        {
            RemoveImpatienceCustomer(0);
        }
        if (thisRoundOverallInstanceScript.customerTimerInDO2 < 0.1)
        {
            RemoveImpatienceCustomer(1);
        }
        if (thisRoundOverallInstanceScript.customerTimerInDO3 < 0.1)
        {
            RemoveImpatienceCustomer(2);
        }
        if (thisRoundOverallInstanceScript.customerTimerInDO4 < 0.1)
        {
            RemoveImpatienceCustomer(3);
        }
        if (thisRoundOverallInstanceScript.customerTimerInDO5 < 0.1)
        {
            RemoveImpatienceCustomer(4);
        }
        if (thisRoundOverallInstanceScript.customerTimerInDO6 < 0.1)
        {
            RemoveImpatienceCustomer(5);
        }
    }

    //FIX ME: Make the popularity hit x2 worse.
    //Remove customer from the queue with their order.
    public void RemoveImpatienceCustomer(int indexForTheCustomer)
    {
        //SaveQ1Q2AndResetAllPatience.
        float tempT2E = thisRoundOverallInstanceScript.eCustomerEndTimeAInDO2;
        float tempT2S = thisRoundOverallInstanceScript.customerStartingTimeInDO2;
        float tempT3E = thisRoundOverallInstanceScript.eCustomerEndTimeAInDO3;
        float tempT3S = thisRoundOverallInstanceScript.customerStartingTimeInDO3;
        float tempT4E = thisRoundOverallInstanceScript.eCustomerEndTimeAInDO4;
        float tempT4S = thisRoundOverallInstanceScript.customerStartingTimeInDO4;
        float tempT5E = thisRoundOverallInstanceScript.eCustomerEndTimeAInDO5;
        float tempT5S = thisRoundOverallInstanceScript.customerStartingTimeInDO5;
        float tempT6E = thisRoundOverallInstanceScript.eCustomerEndTimeAInDO6;
        float tempT6S = thisRoundOverallInstanceScript.customerStartingTimeInDO6;

        //Check if there's anyone in the queue at all.
        if (waitingForOrderCustomerQueue.Count > 0) {
            //FIXME:Add animations for customers already at the front.
            endCustomer(indexForTheCustomer);

            //This updates timers by moving them n+1. This coud 100% be optimized.
            if (indexForTheCustomer == 0)
            {
                //Resets ALL Patience timers
                thisRoundOverallInstanceScript.customerStartingTimeInDO1 = tempT2S;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO1 = tempT2E;

                thisRoundOverallInstanceScript.customerStartingTimeInDO2 = 0;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO2 = 0;
                thisRoundOverallInstanceScript.customerTimerInDO2 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO2 = tempT3S;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO2 = tempT3E;

                thisRoundOverallInstanceScript.customerStartingTimeInDO3 = 0;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO3 = 0;
                thisRoundOverallInstanceScript.customerTimerInDO3 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO3 = tempT4S;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO3 = tempT4E;

                thisRoundOverallInstanceScript.customerStartingTimeInDO4 = 0;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO4 = 0;
                thisRoundOverallInstanceScript.customerTimerInDO4 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO4 = tempT5S;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO4 = tempT5E;

                thisRoundOverallInstanceScript.customerStartingTimeInDO5 = 0;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO5 = 0;
                thisRoundOverallInstanceScript.customerTimerInDO5 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO5 = tempT6S;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO5 = tempT6E;

                thisRoundOverallInstanceScript.customerTimerInDO6 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO6 = 0f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO6 = 0f;
            }

            if (indexForTheCustomer == 1)
            {
                //Resets ALL Patience timers

                thisRoundOverallInstanceScript.customerStartingTimeInDO2 = 0;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO2 = 0;
                thisRoundOverallInstanceScript.customerTimerInDO2 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO2 = tempT3S;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO2 = tempT3E;

                thisRoundOverallInstanceScript.customerStartingTimeInDO3 = 0;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO3 = 0;
                thisRoundOverallInstanceScript.customerTimerInDO3 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO3 = tempT4S;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO3 = tempT4E;

                thisRoundOverallInstanceScript.customerStartingTimeInDO4 = 0;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO4 = 0;
                thisRoundOverallInstanceScript.customerTimerInDO4 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO4 = tempT5S;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO4 = tempT5E;

                thisRoundOverallInstanceScript.customerStartingTimeInDO5 = 0;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO5 = 0;
                thisRoundOverallInstanceScript.customerTimerInDO5 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO5 = tempT6S;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO5 = tempT6E;

                thisRoundOverallInstanceScript.customerTimerInDO6 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO6 = 0f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO6 = 0f;
            }

            if (indexForTheCustomer == 2)
            {
                //Resets ALL Patience timers

                thisRoundOverallInstanceScript.customerStartingTimeInDO3 = 0;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO3 = 0;
                thisRoundOverallInstanceScript.customerTimerInDO3 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO3 = tempT4S;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO3 = tempT4E;

                thisRoundOverallInstanceScript.customerStartingTimeInDO4 = 0;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO4 = 0;
                thisRoundOverallInstanceScript.customerTimerInDO4 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO4 = tempT5S;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO4 = tempT5E;

                thisRoundOverallInstanceScript.customerStartingTimeInDO5 = 0;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO5 = 0;
                thisRoundOverallInstanceScript.customerTimerInDO5 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO5 = tempT6S;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO5 = tempT6E;

                thisRoundOverallInstanceScript.customerTimerInDO6 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO6 = 0f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO6 = 0f;
            }
            
            if (indexForTheCustomer == 3)
            {
            //Resets ALL Patience timers

            thisRoundOverallInstanceScript.customerStartingTimeInDO4 = 0;
            thisRoundOverallInstanceScript.eCustomerEndTimeAInDO4 = 0;
            thisRoundOverallInstanceScript.customerTimerInDO4 = 0.1f;
            thisRoundOverallInstanceScript.customerStartingTimeInDO4 = tempT5S;
            thisRoundOverallInstanceScript.eCustomerEndTimeAInDO4 = tempT5E;

            thisRoundOverallInstanceScript.customerStartingTimeInDO5 = 0;
            thisRoundOverallInstanceScript.eCustomerEndTimeAInDO5 = 0;
            thisRoundOverallInstanceScript.customerTimerInDO5 = 0.1f;
            thisRoundOverallInstanceScript.customerStartingTimeInDO5 = tempT6S;
            thisRoundOverallInstanceScript.eCustomerEndTimeAInDO5 = tempT6E;

            thisRoundOverallInstanceScript.customerTimerInDO6 = 0.1f;
            thisRoundOverallInstanceScript.customerStartingTimeInDO6 = 0f;
            thisRoundOverallInstanceScript.eCustomerEndTimeAInDO6 = 0f;
            }

            if (indexForTheCustomer == 4)
            {
            //Resets ALL Patience timers

            thisRoundOverallInstanceScript.customerStartingTimeInDO5 = 0;
            thisRoundOverallInstanceScript.eCustomerEndTimeAInDO5 = 0;
            thisRoundOverallInstanceScript.customerTimerInDO5 = 0.1f;
            thisRoundOverallInstanceScript.customerStartingTimeInDO5 = tempT6S;
            thisRoundOverallInstanceScript.eCustomerEndTimeAInDO5 = tempT6E;

            thisRoundOverallInstanceScript.customerTimerInDO6 = 0.1f;
            thisRoundOverallInstanceScript.customerStartingTimeInDO6 = 0f;
            thisRoundOverallInstanceScript.eCustomerEndTimeAInDO6 = 0f;
            }

            if (indexForTheCustomer == 5)
            {
                //Resets ALL Patience timers
                thisRoundOverallInstanceScript.customerTimerInDO6 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO6 = 0f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO6 = 0f;
            }
            Destroy(this.gameObject.transform.GetChild(indexForTheCustomer).gameObject);
            Debug.Log("Killed a customer");
        }
    }

    //This actually removes the customer from the list and destroys them
    public void endCustomer(int customerToEnd)
    {
        //Remove the customer and reset it's timers.
        switch (customerToEnd)
        {
            case 0:
                waitingForOrderCustomerQueue.RemoveAt(0);
                thisRoundOverallInstanceScript.customerTimerInDO1 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO1 = 0.1f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO1 = 0.1f;
                return;
            case 1:
                waitingForOrderCustomerQueue.RemoveAt(1);
                thisRoundOverallInstanceScript.customerTimerInDO2 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO2 = 0.1f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO2 = 0.1f;
                return;
            case 2:
                waitingForOrderCustomerQueue.RemoveAt(2);
                thisRoundOverallInstanceScript.customerTimerInDO3 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO3 = 0.1f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO3 = 0.1f;
                return;
            case 3:
                waitingForOrderCustomerQueue.RemoveAt(3);
                thisRoundOverallInstanceScript.customerTimerInDO4 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO4 = 0.1f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO4 = 0.1f;
                return;
            case 4:
                waitingForOrderCustomerQueue.RemoveAt(4);
                thisRoundOverallInstanceScript.customerTimerInDO5 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO5 = 0.1f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO5 = 0.1f;
                return;
            case 5:
                waitingForOrderCustomerQueue.RemoveAt(5);
                thisRoundOverallInstanceScript.customerTimerInDO6 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO6 = 0.1f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO6 = 0.1f;
                return;

        }
    }
}
