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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisGamesOverallInstanceScript = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
        thisRoundOverallInstanceScript = GameObject.Find("BobaShopRoundManager").GetComponent<BobaShopRoundManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AttemptToPerformPurchase()
    {
        // Iterate through the related customer waiting for drink queue if they exist.
        //if (drinkTabIndexThisIs <= customerWaitingDrinkQueue.waitingForOrderCustomerQueue.Count - 1)
        //{
        //    //Itereate through each drink this co-responding customer ordered.
        //    int drinkIndexNext = 0;
        //    foreach (string drinkUIDToScan in customerWaitingDrinkQueue.waitingForOrderCustomerQueue[drinkTabIndexThisIs].GetComponent<CustomerDrinkScript>().drinksThisNPCOrdered)
        //    {
        //        //Temp index to save the info to make sure it spaces evenly.
        //        int infoIndexNext = 0;

        //        //Break down each drink UID to populate the tab.
        //        //Tea Ingredient Base
        //        GameObject teaIngredientBase;
        //        switch (drinkUIDToScan.Substring(0, 2))
        //        {
        //            case "--":
        //                //Do Nothing.
        //                break;
        //            case "PD":
        //                teaIngredientBase = Instantiate(pandanIco, this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
        //                teaIngredientBase.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        //                teaIngredientBase.transform.localPosition = new Vector3(0f, 20f, 0f);
        //                infoIndexNext++;
        //                break;
        //            case "BN":
        //                teaIngredientBase = Instantiate(bananaIco, this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
        //                teaIngredientBase.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        //                teaIngredientBase.transform.localPosition = new Vector3(0f, 20f, 0f);
        //                infoIndexNext++;
        //                break;
        //            case "SB":
        //                teaIngredientBase = Instantiate(strawberryIco, this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
        //                teaIngredientBase.transform.localScale = new Vector3(0.55f, 0.55f, 0.25f);
        //                teaIngredientBase.transform.localPosition = new Vector3(0f, 20f, 0f);
        //                infoIndexNext++;
        //                break;
        //            case "MB":
        //                teaIngredientBase = Instantiate(mangoIco, this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
        //                teaIngredientBase.transform.localScale = new Vector3(0.55f, 0.55f, 0.25f);
        //                teaIngredientBase.transform.localPosition = new Vector3(0f, 20f, 0f);
        //                infoIndexNext++;
        //                break;
        //            case "UB":
        //                teaIngredientBase = Instantiate(ubeIco, this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
        //                teaIngredientBase.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        //                teaIngredientBase.transform.localPosition = new Vector3(0f, 20f, 0f);
        //                infoIndexNext++;
        //                break;
        //        }
        //        //TeaBase
        //        GameObject textTeaBase;
        //        switch (drinkUIDToScan.Substring(2, 2))
        //        {
        //            case "--":
        //                //Do Nothing
        //                break;
        //            case "GB":
        //                textTeaBase = Instantiate(textIco, this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
        //                textTeaBase.GetComponent<TMP_Text>().SetText("G");
        //                infoIndexNext++;
        //                break;
        //            case "BB":
        //                textTeaBase = Instantiate(textIco, this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
        //                textTeaBase.GetComponent<TMP_Text>().SetText("B");
        //                infoIndexNext++;
        //                break;
        //            case "OB":
        //                textTeaBase = Instantiate(textIco, this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
        //                textTeaBase.GetComponent<TMP_Text>().SetText("O");
        //                infoIndexNext++;
        //                break;
        //        }
        //        //Drink overlay
        //        GameObject textFlavorOverlay;
        //        switch (drinkUIDToScan.Substring(4, 1))
        //        {
        //            case "-":
        //                //Do Nothing.
        //                break;
        //            case "M":
        //                textFlavorOverlay = Instantiate(textIco, this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
        //                textFlavorOverlay.GetComponent<TMP_Text>().SetText("M");
        //                infoIndexNext++;
        //                break;
        //            case "W":
        //                textFlavorOverlay = Instantiate(textIco, this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
        //                textFlavorOverlay.GetComponent<TMP_Text>().SetText("W");
        //                infoIndexNext++;
        //                break;
        //        }
        //        //Toppings
        //        GameObject bobaToppings;
        //        switch (drinkUIDToScan.Substring(5, 2))
        //        {
        //            case "*-":
        //                //Do Nothing.
        //                break;
        //            case "*B":
        //                bobaToppings = Instantiate(bobaIco, this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
        //                bobaToppings.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        //                infoIndexNext++;
        //                break;
        //        }
        //        //Tempurature
        //        switch (drinkUIDToScan.Substring(7, 1))
        //        {
        //            case "-":
        //                //Do Nothing.
        //                break;
        //        }
        //        //Sweetness
        //        switch (drinkUIDToScan.Substring(8, 1))
        //        {
        //            case "-":
        //                //Do Nothing.
        //                break;
        //        }
        //        drinkIndexNext++;
        //    }
        //}
    }
}
