using UnityEngine;
using UnityEngine.UI;

public class CustomerPatienceUIScript : MonoBehaviour
{

    // This script handles how the customer patience UI is controlled.
    // CONNECTED TO "CustomerPrefabs" directly.

    // Get the boba shop game manager script to pull data from.
    BobaShopRoundManagerScript thisRoundOverallInstanceScript;

    //Customer Game Object.
    GameObject CustomerLoaded;

    // Gameobject list that holds the CustomerWaitingHandlerScript.
    [Tooltip("\"CustomerDrinkWaitQueueHandler\" game object is Identified on customer generation.")]
    CustomerHandlerScript CustomerDrinkWaitQueueHandler;

    //The script from the queu handler to collect patience data and influence it.
    CustomerHandlerScript CustomerDrinkWaitQueueHandlerScript; 

    //FIXME: This field needs to be identified when the bell is rung for patients in the waiting for drink queue.
    // Gameobject list that holds the Customers who picked up their order before deleting them.
    [Tooltip("\"CustomerLeavingHandlerObject\" game object is identified when customer moves to the other queue and is called.")]
    GameObject CustomerLeavingHandlerObject;

    // The UI gameobject for the PatienceMeter.
    [SerializeField]
    [Tooltip("Drag and drop the PatienceMeter UI Object here.")]
    GameObject customerPatienceMeter;

    // The UI gameobject Image for the BottomLayer.
    [SerializeField]
    [Tooltip("Drag and drop the BottomLayer UI Object here.")]
    Image customerPatiencBottomLayerMeter;

    // The UI gameobject Image for the DefaultIco.
    [SerializeField]
    [Tooltip("Drag and drop the DefaultIco UI Object here.")]
    Image customerPatiencDefaultIco;

    // The UI gameobject Image for the DarkerLayerMeter.
    [SerializeField]
    [Tooltip("Drag and drop the DarkerLayerMeter UI Object here.")]
    Image customerPatiencDarkerLayerMeter;

    // The UI gameobject Image for the HappyLayer.
    [SerializeField]
    [Tooltip("Drag and drop the HappyLayer UI Object here.")]
    Image customerPatiencHappyLayerMeter;

    // The UI gameobject Image for the HappyIco.
    [SerializeField]
    [Tooltip("Drag and drop the HappyIco UI Object here.")]
    Image customerPatiencHappyIcoMeter;

    // The UI gameobject Image for the MehLayer.
    [SerializeField]
    [Tooltip("Drag and drop the MehLayer UI Object here.")]
    Image customerPatiencMehLayerMeter;

    // The UI gameobject Image for the MehIco.
    [SerializeField]
    [Tooltip("Drag and drop the MehIco UI Object here.")]
    Image customerPatiencMehIcoMeter;

    // The UI gameobject Image for the SadLayer.
    [SerializeField]
    [Tooltip("Drag and drop the SadLayer UI Object here.")]
    Image customerPatiencSadLayerMeter;

    // The UI gameobject Image for the SadIco.
    [SerializeField]
    [Tooltip("Drag and drop the SadIco UI Object here.")]
    Image customerPatiencSadIcoMeter;

    public bool customerHasInfinitePatience;

    //This saves the last time for an order for patience meter evaluation.
    public float customerStartedWaitingAt = 0f;

    //This saves the customer patience meter evaluation for Order queue. (Default 60s)
    public float customerWaitingInBeginningLineTime = 30f;

    //This saves the customer patience meter evaluation for Order queue. (Default 20s)
    public float customerOrderWaitingTime = 20f;

    //This saves the customer patience meter evaluation for Drink queue. (Default 10s)
    public float customerDrinkWaitingTime = 10f;

    //This saves the customer patience meter evaluation for Special queue. (Default 10s)
    public float customerSpecialWaitingTime = 10f;

    //This is the patience meter over 100% (0.0f->1f)
    public float patienceOver100 = 1;

    public bool customerHadOrderTaken ,customerIsAtFront, customerIsWaitingForDrinks, customerIsWaitingForSomethingElse, customerIsInWaitingInALineNotAtFront, customerIsWalking;

    //This ensures that the customer timer is only started when a customer enteres the line and is clickable(from other scripts).
    //public bool timerHasStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Find and load the BobaShopRound data.
        thisRoundOverallInstanceScript = GameObject.Find("BobaShopRoundManager").GetComponent<BobaShopRoundManagerScript>();

        CustomerLoaded = this.gameObject;
        Debug.Log(CustomerLoaded.name);
        Debug.Log(thisRoundOverallInstanceScript.customerQueueHandlerScript.name);

        //Get the queue handler and script by going to the root parent holding this gameobject.
        CustomerDrinkWaitQueueHandler = thisRoundOverallInstanceScript.customerQueueHandlerScript;
        ;
        if (CustomerDrinkWaitQueueHandler.name != "CustomerQueueHandler")
        {
            Debug.LogError("Tried to connect customer patience to the order queue handler object but found something else! Found: " + CustomerDrinkWaitQueueHandler.name);
        }
        else
        {
            CustomerDrinkWaitQueueHandlerScript = CustomerDrinkWaitQueueHandler.GetComponent<CustomerHandlerScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        //Updates the viewable patience timer if the customer is at the foreground, check the timer in round manager for generating these patience items.
        if (!customerHasInfinitePatience && customerIsAtFront) {
            
            //Evaluate the patience level according to round manager.
            patienceOver100 = (1.0f - (thisRoundOverallInstanceScript.roundTimer - (thisRoundOverallInstanceScript.customerStartingTimeInQO1 - 0.5f)) / (thisRoundOverallInstanceScript.eCustomerEndTimeAInQO1 - thisRoundOverallInstanceScript.customerStartingTimeInQO1));
            Debug.Log("Patience Val: " + patienceOver100);

            customerPatiencBottomLayerMeter.enabled = true;

            if (patienceOver100 >= .5)
            {
                //Disable the default knob.
                customerPatiencDefaultIco.enabled = false;

                //Enable Dark patience layer and lag behind the loading by .02
                customerPatiencDarkerLayerMeter.enabled = true;
                customerPatiencDarkerLayerMeter.fillAmount = (float)(patienceOver100 + .02);

                //Enable Happy patience layer the loading
                customerPatiencHappyLayerMeter.enabled = true;
                customerPatiencHappyLayerMeter.fillAmount = patienceOver100;

                //Enable Happy Knob
                customerPatiencHappyIcoMeter.enabled = true;
            }
            if (patienceOver100 > .25 && patienceOver100 < .50)
            {
                //Have Dark patience lag behind the loading by .02
                customerPatiencDarkerLayerMeter.fillAmount = (float)(patienceOver100 + .02);

                //Disable the Happy layer.
                customerPatiencHappyLayerMeter.enabled = false;
                //Disable the Happy knob.
                customerPatiencHappyIcoMeter.enabled = false;

                //Enable Meh patience layer the loading
                customerPatiencMehLayerMeter.enabled = true;
                customerPatiencMehLayerMeter.fillAmount = patienceOver100;

                //Enable Meh Knob
                customerPatiencMehIcoMeter.enabled = true;
            }
            if (patienceOver100 <= .25)
            {
                //Have Dark patience lag behind the loading by .02
                customerPatiencDarkerLayerMeter.fillAmount = (float)(patienceOver100 + .02);

                //Disable the Meh layer.
                customerPatiencMehLayerMeter.enabled = false;
                //Disable the Meh knob.
                customerPatiencMehLayerMeter.enabled = false;

                //Enable Sad patience layer the loading
                customerPatiencSadLayerMeter.enabled = true;
                customerPatiencSadLayerMeter.fillAmount = patienceOver100;

                //Enable Sad Knob
                customerPatiencSadIcoMeter.enabled = true;
            }
        }
    }

    public void CustomerStartedWaitingForOrderTaking()
    {
        if (thisRoundOverallInstanceScript != null)
        {
            thisRoundOverallInstanceScript.customerStartingTimeInQO1 = thisRoundOverallInstanceScript.roundTimer + 0.5f;
            customerIsAtFront = true;
        }
        
    }

    public bool CheckIfCustomerHasBeenFlaggedAsFront()
    {
        return customerIsAtFront;
    }

    //Method to reset the customer's patience and icons if needed.
    public void ResetThisCustomersPatienceAndFrontStatus()
    {
        customerIsAtFront = false;

        //Disable the other patience layers.
        customerPatiencDarkerLayerMeter.enabled = false;
        customerPatiencBottomLayerMeter.enabled = false;
        customerPatiencDefaultIco.enabled = false;
        //Disable the Happy layer.
        customerPatiencHappyLayerMeter.enabled = false;
        //Disable the Meh layer.
        customerPatiencMehLayerMeter.enabled = false;
        //Disable the Sad layer.
        customerPatiencSadLayerMeter.enabled = false;

        //Whatever Knob the customer had will be kept to show the player how much patience the customer had left.
        //The happier the knob, the better the rating increases!
        //Red/Sad faces have a 1/4 chance to nagatively decrease rating of restaurant, 2/4 no change (but still gives money with no tip chance), or 1/4 acts like a "meh" customer.
    }
}
