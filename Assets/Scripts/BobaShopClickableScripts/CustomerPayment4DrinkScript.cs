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
                        Debug.LogWarning("Drink price Calculated: " + CalculateDrinkPriceAtValue(drinkThatMatchesInMat.GetComponent<BobaCupUIDSettingsScript>().drinkUID));
                        thisRoundOverallInstanceScript.thisRoundMoneyJar.AddMoneyThisRound(CalculateDrinkPriceAtValue(drinkThatMatchesInMat.GetComponent<BobaCupUIDSettingsScript>().drinkUID));
                        bobaSellMattScript.sellableBobaDrinks.Remove(drinkThatMatchesInMat);
                        thisRoundOverallInstanceScript.AddDrinkSoldToList(drinkThatMatchesInMat.GetComponent<BobaCupUIDSettingsScript>().drinkUID);
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

                        thisRoundOverallInstanceScript.thisRoundMoneyJar.AddMoneyThisRound(CalculateDrinkPriceAtValue(drinkThatMatchesInMat.GetComponent<BobaCupUIDSettingsScript>().drinkUID));
                        bobaSellMattScript.sellableBobaDrinks.Remove(drinkThatMatchesInMat);
                        bobaSellMattScript.sellableBobaDrinksStrings.Remove(drinkThatMatchesInMat.GetComponent<BobaCupUIDSettingsScript>().drinkUID);
                        thisRoundOverallInstanceScript.AddDrinkSoldToList(drinkThatMatchesInMat.GetComponent<BobaCupUIDSettingsScript>().drinkUID);
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

                    thisRoundOverallInstanceScript.thisRoundMoneyJar.AddMoneyThisRound(CalculateAndCompareDrinkPriceAtValue(uidsOrdered[i], nextDrinkInMat.GetComponent<BobaCupUIDSettingsScript>().drinkUID));
                    bobaSellMattScript.sellableBobaDrinks.RemoveAt(0);
                    thisRoundOverallInstanceScript.AddDrinkSoldToList(nextDrinkInMat.GetComponent<BobaCupUIDSettingsScript>().drinkUID);
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

                    thisRoundOverallInstanceScript.thisRoundMoneyJar.AddMoneyThisRound(CalculateAndCompareDrinkPriceAtValue(uidsOrdered[i], nextDrinkInMat.GetComponent<BobaCupUIDSettingsScript>().drinkUID));
                    bobaSellMattScript.sellableBobaDrinks.Remove(nextDrinkInMat);
                    bobaSellMattScript.sellableBobaDrinksStrings.Remove(nextDrinkInMat.GetComponent<BobaCupUIDSettingsScript>().drinkUID);
                    thisRoundOverallInstanceScript.AddDrinkSoldToList(nextDrinkInMat.GetComponent<BobaCupUIDSettingsScript>().drinkUID);
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

    //This compares drink values and returns the lower one.
    public double CalculateAndCompareDrinkPriceAtValue(string UUIDOfDesiredDrink, string UUIDOfDrinkTaken)
    {
        double priceOfDesiredDrink = CalculateDrinkPriceAtValue(UUIDOfDesiredDrink);
        double priceOfTakenDrink = CalculateDrinkPriceAtValue(UUIDOfDrinkTaken);

        if (priceOfDesiredDrink < priceOfTakenDrink) {
            return priceOfDesiredDrink;
        }
        else
        {
            return priceOfTakenDrink;
        }
    }

    //This is used to calculate ONE drink.
    public double CalculateDrinkPriceAtValue(string UUIDOfDrink)
    {
        double costOfDrink = 0f;

        //Break down each drink UID to populate the tab.
        //Tea Ingredient Base
        switch (UUIDOfDrink.Substring(0, 2))
        {
            case "--":
                //Do nothing
                break;
            case "PD":
                costOfDrink += thisRoundOverallInstanceScript.priceOfPandan;
                break;
            case "BN":
                costOfDrink += thisRoundOverallInstanceScript.priceOfBanana;
                break;

            case "SB":
                costOfDrink += thisRoundOverallInstanceScript.priceOfStrawberry;
                break;

            case "MB":
                costOfDrink += thisRoundOverallInstanceScript.priceOfMango;
                break;

            case "UB":
                costOfDrink += thisRoundOverallInstanceScript.priceOfUbe;
                break;
        }
        //TeaBase
        switch (UUIDOfDrink.Substring(2, 2))
        {
            case "--":
                //Do Nothing
                break;
            case "GB":
                costOfDrink += thisRoundOverallInstanceScript.priceOfGreenTea;
                break;
            case "BB":
                costOfDrink += thisRoundOverallInstanceScript.priceOfBlackTea;
                break;
            case "OB":
                costOfDrink += thisRoundOverallInstanceScript.priceOfOolongTea;
                break;
        }
        //Drink overlay
        switch (UUIDOfDrink.Substring(4, 1))
        {
            case "-":
                //Do Nothing.
                break;
            case "M":
                costOfDrink += thisRoundOverallInstanceScript.priceOfMilk;
                break;
            case "W":
                //Enhance customer popularity gain and add tips! For now multiply by current drink price, or give a coin.
                if (costOfDrink > 0)
                {
                    costOfDrink += costOfDrink * Random.Range(0f, 1f);
                }
                else
                {
                    costOfDrink = 1f;
                }
                break;
        }
        //Toppings
        switch (UUIDOfDrink.Substring(5, 2))
        {
            case "*-":
                //Do Nothing.
                break;
            case "*B":
                costOfDrink += thisRoundOverallInstanceScript.priceOfCasavaTopping;
                break;
        }
        //Tempurature
        switch (UUIDOfDrink.Substring(7, 1))
        {
            case "-":
                //Do Nothing.
                break;
        }
        //Sweetness
        switch (UUIDOfDrink.Substring(8, 1))
        {
            case "-":
                //Do Nothing.
                break;
        }
        return costOfDrink;
    }
}
