using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerPayment4DrinkScript : MonoBehaviour
{
    //The Game's OverallManager SCRIPT to pull/put scripts and values from. Gets established automatically from above object.
    GameManagerScript thisGamesOverallInstanceScript;

    // Get the boba shop game manager script to pull data from.
    BobaShopRoundManagerScript thisRoundOverallInstanceScript;

    [SerializeField]
    [Header("Drink pending queue GAMEOBJECT")]
    [Tooltip("Put the game's \"CustomerDrinkWaitQueueHandler\" game object to access transform properties of this object.")]
    public GameObject customerWaitingDrinkHandlerGameObject;

    [SerializeField]
    [Header("Drink pending queue")]
    [Tooltip("Put the game's \"CustomerDrinkWaitQueueHandler\" game object to access the customer queue of people waiting for their drinks.")]
    CustomerWaitingForDrinkHandlerScript customerWaitingDrinkHandlerScript;

    // Script for customer waiting to pickup drinks.
    [SerializeField]
    [Tooltip("Drag and drop the \"CustomerOrderPickupScript\" from the \"CustomerDrinkWaitQueueHandler\" game object here.")]
    public CustomerOrderPickupScript customerWaitingOrderPickupScript;

    //Script of the sell tray.
    BobaSellMattScript bobaSellMattScript;

    //List of the game objects in the sell tray.
    List<GameObject> sellableBobaDrinks;

    // Bool to see if all drinks match the customer's order.
    public bool allDrinksMatch = false;

    // float for how much the customer will pay.
    public float totalPayment = 0f;

    public bool customerIsTryingToPay = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisGamesOverallInstanceScript = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
        thisRoundOverallInstanceScript = GameObject.Find("BobaShopRoundManager").GetComponent<BobaShopRoundManagerScript>();
        bobaSellMattScript = this.gameObject.GetComponent<BobaSellMattScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Call the purchase timer if there is a customer waiting to pick up drinks.
    //Wrong drinks per customer causes them to pay WAY less (If if the value of the drink they ordered is higher), or they will pay "At their Order" cost if the taken drink has more value.
    //Tips are NOT given if customer reaches 0 patience.
    //Patience should - INCREASE POPULARITY BASED OF HOW MUCH PATIENCE WAS LEFT. Sad->no patience left (1/4th or lower) grants nothing.
    
    //This method simply checks if the customer has matching drinks in the boba mat, along with other checks.
    public void AttemptToPerformPurchase()
    {
        totalPayment = 0f;
        
        // Iterate through the related customer waiting for drink queue if they exist.
        if (customerWaitingDrinkHandlerScript.waitingForOrderCustomerQueue[0] != null && customerIsTryingToPay)
        {
            GameObject customerToSellDrinkTo = customerWaitingDrinkHandlerScript.waitingForOrderCustomerQueue[0];

            //checks if the customer has the exact drinks in the mat they requested.
            if (customerToSellDrinkTo.GetComponent<CustomerDrinkScript>().drinksThisNPCOrdered.All(drinkUID => bobaSellMattScript.sellableBobaDrinksStrings.Contains(drinkUID)))
            {
                List<string> uidsOrdered = customerToSellDrinkTo.GetComponent<CustomerDrinkScript>().drinksThisNPCOrdered;

                //If the boba mat has the same ammount of drinks in the order.
                if (uidsOrdered.Count == bobaSellMattScript.sellableBobaDrinksStrings.Count)
                {
                    //Itereate through each drink the customer wanted
                    for (int i = 0; i < uidsOrdered.Count; i++)
                    {
                        //Get the gameobject that matches the UID being ordered.
                        GameObject drinkThatMatchesInMat = bobaSellMattScript.sellableBobaDrinks.Find(obj => obj.GetComponent<BobaCupUIDSettingsScript>().drinkUID == uidsOrdered[i]);
                        bobaSellMattScript.ShowCustomerTakesDrinkInDrinkMat(drinkThatMatchesInMat, customerToSellDrinkTo, i);

                        //FIXME: Still need to make payments go through.
                        totalPayment += 10;
                        bobaSellMattScript.sellableBobaDrinks.Remove(drinkThatMatchesInMat);
                    }
                    bobaSellMattScript.sellableBobaDrinksStrings.Clear();
                }
                else
                {
                    //Itereate through each drink the customer wanted
                    for (int i = 0; i < uidsOrdered.Count; i++)
                    {
                        //Get the gameobject that matches the UID being ordered.
                        GameObject drinkThatMatchesInMat = bobaSellMattScript.sellableBobaDrinks.Find(obj => obj.GetComponent<BobaCupUIDSettingsScript>().drinkUID == uidsOrdered[i]);
                        bobaSellMattScript.ShowCustomerTakesDrinkInDrinkMat(drinkThatMatchesInMat, customerToSellDrinkTo, i);

                        //FIXME: Still need to make payments go through.
                        totalPayment += 10;
                        bobaSellMattScript.sellableBobaDrinks.Remove(drinkThatMatchesInMat);
                        bobaSellMattScript.sellableBobaDrinksStrings.Remove(drinkThatMatchesInMat.GetComponent<BobaCupUIDSettingsScript>().drinkUID);
                    }
                }

                customerWaitingDrinkHandlerScript.waitingForOrderCustomerQueue.Remove(customerToSellDrinkTo);
                thisRoundOverallInstanceScript.customerTimerInDO1 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInDO1 = 0.1f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInDO1 = 0.1f;

                customerWaitingDrinkHandlerScript.RemoveCustomerWithDrinks(customerToSellDrinkTo);
                customerIsTryingToPay = false;
            }
        }
    }

    //This method runs if the customer ran out of patience waiting for drinks they ordered at the mat.
    public void AttemptToPerformPatienceRanOutPurchase()
    {
        totalPayment = 0f;
        // Iterate through the related customer waiting for drink queue if they exist.
        if (customerWaitingDrinkHandlerScript.waitingForOrderCustomerQueue[0] != null && customerIsTryingToPay)
        {
            GameObject customerToSellDrinkTo = customerWaitingDrinkHandlerScript.waitingForOrderCustomerQueue[0];

            List<string> uidsOrdered = customerToSellDrinkTo.GetComponent<CustomerDrinkScript>().drinksThisNPCOrdered;

            //If the boba mat has the same ammount of drinks in the order.
            if (uidsOrdered.Count == bobaSellMattScript.sellableBobaDrinksStrings.Count)
            {
                //Itereate through each drink the customer wanted
                for (int i = 0; i < uidsOrdered.Count; i++)
                {
                    //Get the next drink in the mat.
                    GameObject nextDrinkInMat = bobaSellMattScript.sellableBobaDrinks[0];
                    bobaSellMattScript.ShowCustomerTakesDrinkInDrinkMat(nextDrinkInMat, customerToSellDrinkTo, i);

                    //FIXME: Still need to make payments go through.
                    totalPayment += 10;
                    bobaSellMattScript.sellableBobaDrinks.RemoveAt(0);
                }
                bobaSellMattScript.sellableBobaDrinksStrings.Clear();
            }
            else
            {
                //Itereate through each drink the customer wanted
                for (int i = 0; i < uidsOrdered.Count; i++)
                {
                    //Get the gameobject that matches the UID being ordered.
                    GameObject nextDrinkInMat = bobaSellMattScript.sellableBobaDrinks[0];
                    bobaSellMattScript.ShowCustomerTakesDrinkInDrinkMat(nextDrinkInMat, customerToSellDrinkTo, i);

                    //FIXME: Still need to make payments go through.
                    totalPayment += 10;
                    bobaSellMattScript.sellableBobaDrinks.Remove(nextDrinkInMat);
                    bobaSellMattScript.sellableBobaDrinksStrings.Remove(nextDrinkInMat.GetComponent<BobaCupUIDSettingsScript>().drinkUID);
                }
            }

            customerWaitingDrinkHandlerScript.waitingForOrderCustomerQueue.Remove(customerToSellDrinkTo);
            thisRoundOverallInstanceScript.customerTimerInDO1 = 0.1f;
            thisRoundOverallInstanceScript.customerStartingTimeInDO1 = 0.1f;
            thisRoundOverallInstanceScript.eCustomerEndTimeAInDO1 = 0.1f;

            customerWaitingDrinkHandlerScript.RemoveCustomerWithDrinks(customerToSellDrinkTo);
            customerIsTryingToPay = false;
        }
    }

    public float CalculateDrinkPriceAtValue(string UUIDOfDrink)
    {
        float costOfDrink = 10f;

        Debug.LogWarning("Sold a drink! " + costOfDrink);
        return costOfDrink;
    }

    //foreach (string drinkUIDToScan in customerWaitingDrinkHandlerScript.waitingForOrderCustomerQueue[0].GetComponent<CustomerDrinkScript>().drinksThisNPCOrdered)
    //        {
    //            //Temp index to save the info to make sure it spaces evenly.
    //            int totalPayment = 0;

    //            //Break down each drink UID to populate the tab.
    //            //Tea Ingredient Base
    //            switch (drinkUIDToScan.Substring(0, 2))
    //            {
    //                case "--":
    //                    //Do Nothing.
    //                    break;
    //                case "PD":


    //                    break;
    //                case "BN":


    //                    break;
    //                case "SB":


    //                    break;
    //                case "MB":


    //                    break;
    //                case "UB":


    //                    break;
    //            }
    //            //TeaBase
    //            switch (drinkUIDToScan.Substring(2, 2))
    //            {
    //                case "--":
    //                    //Do Nothing
    //                    break;
    //                case "GB":

    //                    break;
    //                case "BB":


    //                    break;
    //                case "OB":

    //                    break;
    //            }
    //            //Drink overlay
    //            switch (drinkUIDToScan.Substring(4, 1))
    //            {
    //                case "-":
    //                    //Do Nothing.
    //                    break;
    //                case "M":


    //                    break;
    //                case "W":


    //                    break;
    //            }
    //            //Toppings
    //            switch (drinkUIDToScan.Substring(5, 2))
    //            {
    //                case "*-":
    //                    //Do Nothing.
    //                    break;
    //                case "*B":


    //                    break;
    //            }
    //            //Tempurature
    //            switch (drinkUIDToScan.Substring(7, 1))
    //            {
    //                case "-":
    //                    //Do Nothing.
    //                    break;
    //            }
    //            //Sweetness
    //            switch (drinkUIDToScan.Substring(8, 1))
    //            {
    //                case "-":
    //                    //Do Nothing.
    //                    break;
    //            }
    //        }

}
