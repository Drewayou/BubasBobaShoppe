using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class CustomerHandlerScript : MonoBehaviour
{
    // This script handles what happens if the boba shop round handler needs to generate a new customer and add them to the queue.
    // CONNECTED TO "CustomerQueueHandler" Game Object.

    // Get the boba shop game manager script to pull data from.
    BobaShopRoundManagerScript thisRoundOverallInstanceScript;

    // Gameobject list that holds the CustomerWaitingHandlerScript.
    [SerializeField]
    [Tooltip("Drag and drop the \"CustomerDrinkWaitQueueHandler\" game object here.")]
    GameObject CustomerDrinkWaitQueueHandler;

    // Gameobject list that hold the to-order customer Queue. Or customers waiting to order.
    [SerializeField]
    public List<GameObject> toOrderCustomerQueue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    void Start()
    {
        //Find and load the BobaShopRound data.
        thisRoundOverallInstanceScript = GameObject.Find("BobaShopRoundManager").GetComponent<BobaShopRoundManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //FIXME: Patience is acting wonky
        //Timer that checks off the round time to evaluate if the customer in front of this queue looses patience. +5s Patience is added for each dialogue order taken interaction at position 0. 
        if (thisRoundOverallInstanceScript.customerTimerInQO1 < 0.1 && this.gameObject.transform.childCount > 0)
        {
            if (toOrderCustomerQueue[0].GetComponent<CustomerPatienceUIScript>() != null)
            {
                if (toOrderCustomerQueue[0].GetComponent<CustomerPatienceUIScript>().customerIsAtFront)
                {
                    thisRoundOverallInstanceScript.customerTimerInQO1 = 0.1f;
                    CustomerRunsOutOfPatienceForOderTaken(0);
                }
            }
        }
        if (thisRoundOverallInstanceScript.customerTimerInQO2 < 0.1 && this.gameObject.transform.childCount > 1)
        {
            if (this.gameObject.transform.childCount > 1 && toOrderCustomerQueue[1].GetComponent<CustomerPatienceUIScript>() != null)
            {
                if (toOrderCustomerQueue[1].GetComponent<CustomerPatienceUIScript>().customerIsInWaitingInALineNotAtFront)
                {
                    thisRoundOverallInstanceScript.customerTimerInQO2 = 0.1f;
                    CustomerRunsOutOfPatienceForOderTaken(1);
                }
            }
        }
        if (thisRoundOverallInstanceScript.customerTimerInQO3 < 0.1 && this.gameObject.transform.childCount > 2)
        {
            if (this.gameObject.transform.childCount > 2 && toOrderCustomerQueue[2].GetComponent<CustomerPatienceUIScript>() != null)
            {
                if (toOrderCustomerQueue[2].GetComponent<CustomerPatienceUIScript>().customerIsInWaitingInALineNotAtFront)
                {
                    thisRoundOverallInstanceScript.customerTimerInQO3 = 0.1f;
                    CustomerRunsOutOfPatienceForOderTaken(2);
                }
            }
        }

        //Below should be moved to drink handle pickup script.
        if (thisRoundOverallInstanceScript.customerTimerInDO1 < 0.1 && this.gameObject.transform.childCount > 0)
        {
            CustomerRunsOutOfPatienceForOderTaken(0);
        }
        if (thisRoundOverallInstanceScript.customerTimerInDO2 < 0.1 && this.gameObject.transform.childCount > 1)
        {
            CustomerRunsOutOfPatienceForOderTaken(1);
        }
        if (thisRoundOverallInstanceScript.customerTimerInDO3 < 0.1 && this.gameObject.transform.childCount > 2)
        {
            CustomerRunsOutOfPatienceForOderTaken(2);
        }

        //Below should be moved to special request handle script.
        if (thisRoundOverallInstanceScript.customerTimerInSO1 < 0.1 && this.gameObject.transform.childCount > 0)
        {
            CustomerRunsOutOfPatienceForOderTaken(0);
        }
    }

    // Awake is called when this script is on.
    void OnEnable()
    {
        RecheckCustomerVisuals();
    }

    //This method Adds and organizes the visual placement of the customers on the screen and sets their clickable mask on/off depending what place they get in line.
    public void AddCustomerToThisQueue(GameObject customerToAdd){

        //Spawn the new customer object, set it's name and place it off screen.
        GameObject customerObject = Instantiate(customerToAdd);
        customerObject.name = customerToAdd.name;
        customerObject.transform.SetParent(this.gameObject.transform,false);
        customerObject.transform.localPosition = new Vector3(-1200f,0f,0f);
        customerObject.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
        customerObject.transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>().color = new Color32(0, 0, 0, 0);

        toOrderCustomerQueue.Add(customerObject);

        AdjustRayCastsAndPatienceTimers();
        AdjustColorNAnimationOfNewCustomer(customerObject);

    }

    //FIXME: Add animation to this customer line when a customer's order is taken (After all dialogue has been activated by customer's custom scripts).
    //ADD LOGIC TO INCREASE POPULARITY BASED OF HOW MUCH PATIENCE WAS LEFT.
    //This method is activated when an order is truly placed by the customer and it pushes all other customers forward and the main customer to the other queue.
    public void TakeCustomerOrderNAnimateAction(){
        //Check the game object that this script is attached to (the "CustomerQueueHandler" GameObject) to move it's customer to the next queue.
        GameObject customerThatOrderedADrink = toOrderCustomerQueue[0];
        CustomerDrinkWaitQueueHandler.GetComponent<CustomerWaitingHandlerScript>().AddCustomerToThisWatingQueue(customerThatOrderedADrink);

        //If customer has limited patience, reset it for the next timer.
        if (customerThatOrderedADrink.GetComponent<CustomerPatienceUIScript>()!=null)
        {
            customerThatOrderedADrink.GetComponent<CustomerPatienceUIScript>().ResetThisCustomersPatienceAndFrontStatus();
        }

        toOrderCustomerQueue.RemoveAt(0);

        //Does not reset ALL patience, but instead only moves them if nessiscary.
        MoveOrResetPatience(false);

        StartCoroutine(MoveNPCOtherQueuePosition(1200,customerThatOrderedADrink));
        //If the player isn't looking at the front shop, cancel walk in animation.
        AdjustRayCastsAndPatienceTimers();
        foreach(GameObject customer in toOrderCustomerQueue){
            AdjustColorNAnimationOfNewCustomer(customer);
        }
    }

    //FIXME: Add animation to this customer line when a customer looses all patience (with dialogue pop up automated). Add logic to decrease popularity too!
    //This method is activated when a customer looses all patience waiting to have their order taken.
    public void CustomerRunsOutOfPatienceForOderTaken(int lineIndx)
    {

        //Check if the customer even exists or left line already
        if (toOrderCustomerQueue[lineIndx] != null) {
            //Check if the customer even has patience timer
            if (toOrderCustomerQueue[lineIndx].GetComponent<CustomerPatienceUIScript>() == null)
            {
                //Do nothing, this customer has infinite patience.
            }
            else
            {
                ResetOrMovePatience(lineIndx);

                //Pull the game object that this script is attached to (the "CustomerQueueHandler" GameObject) to destroy customer and remove from queue.
                GameObject customerThatWantedToOrderAndLostPatience = toOrderCustomerQueue[lineIndx];
                    toOrderCustomerQueue.RemoveAt(lineIndx);
                    customerThatWantedToOrderAndLostPatience.transform.SetParent(null, false);
                    StartCoroutine(LerpNPCPatienceDestroyer(-3200, customerThatWantedToOrderAndLostPatience));
                    //If the player isn't looking at the front shop, cancel walk in animation.
                    AdjustRayCastsAndPatienceTimers();
                    foreach (GameObject customer in toOrderCustomerQueue)
                    {
                        if (customer == toOrderCustomerQueue[0])
                        {
                            //Do not alter
                        }
                    else
                    {
                        AdjustColorNAnimationOfNewCustomer(customer);
                    }
                        
                    }
            }
        }
    }

    //This method adjusts the raycast and color values of the NPC's in this queue.
    public void AdjustRayCastsAndPatienceTimers(){
        for (int i = 0; i < toOrderCustomerQueue.Count; i++)
        {
            if (i == 0)
            {
                //Check if customer has infinite patience
                toOrderCustomerQueue[i].GetComponent<Image>().raycastTarget = true;
                if (toOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>()==null)
                {
                    //Do not time
                }
                else
                {
                    //Set this customer as the front customer with the patience icon. Pull in customer's patience for giving their order.
                    if (!toOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>().customerIsAtFront) {
                        thisRoundOverallInstanceScript.eCustomerEndTimeAInQO1 = (toOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>().customerOrderWaitingTime + thisRoundOverallInstanceScript.roundTimer);
                        thisRoundOverallInstanceScript.customerStartingTimeInQO1 = thisRoundOverallInstanceScript.roundTimer;
                        toOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>().CustomerStartedWaitingForOrderTaking();
                        toOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>().customerIsAtFront = true;
                        toOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>().customerHadOrderTaken = true;
                        toOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>().customerIsInWaitingInALineNotAtFront = false;
                    }
                }

            }
            else
            {
                if (i == 1)
                {
                    //Check if customer has infinite patience
                    if (toOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>() == null)
                    {
                        //Do not time
                    }
                    else
                    {
                        if (!toOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>().customerIsInWaitingInALineNotAtFront) {
                        //Set this customer's patience for waiting in the initial line.
                        thisRoundOverallInstanceScript.customerStartingTimeInQO2 = thisRoundOverallInstanceScript.roundTimer;
                        thisRoundOverallInstanceScript.eCustomerEndTimeAInQO2 = (toOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>().customerWaitingInBeginningLineTime + thisRoundOverallInstanceScript.roundTimer);
                        toOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>().customerIsInWaitingInALineNotAtFront = true;
                        }
                    }
                }
                if (i == 2)
                {
                    //Check if customer has infinite patience
                    if (toOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>()== null)
                    {
                        //Do not time
                    }
                    else
                    {
                        if (!toOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>().customerIsInWaitingInALineNotAtFront) {
                            //Set this customer's patience for waiting in the initial line.
                            thisRoundOverallInstanceScript.customerStartingTimeInQO3 = thisRoundOverallInstanceScript.roundTimer;
                            thisRoundOverallInstanceScript.eCustomerEndTimeAInQO3 = (toOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>().customerWaitingInBeginningLineTime + thisRoundOverallInstanceScript.roundTimer);
                            toOrderCustomerQueue[i].GetComponent<CustomerPatienceUIScript>().customerIsInWaitingInALineNotAtFront = true;
                        }
                    }
                }

                toOrderCustomerQueue[i].GetComponent<Image>().raycastTarget = false;
            }
        }
    }

    //This method adjusts the animation and color of the new customer coming into the queue.
    public void AdjustColorNAnimationOfNewCustomer(GameObject customer){
        if(toOrderCustomerQueue.Count == 1){
            //If first position customer is already in fully colored, don't alter them.
            if (toOrderCustomerQueue[0].transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>().color != new Color32(255, 255, 255, 255))
            {
                StartCoroutine(LerpNPCQueueColors(255, toOrderCustomerQueue[0].transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>()));
                StartCoroutine(LerpNPCQueuePosition(-550, toOrderCustomerQueue[0]));
            }
            //If the player isn't looking at the front shop, cancel walk in animation.
            if (gameObject.transform.parent.gameObject.activeSelf){
                StartCoroutine(LerpNPCQueueColors(255,toOrderCustomerQueue[0].transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>()));
                StartCoroutine(LerpNPCQueuePosition(-550,toOrderCustomerQueue[0]));
            }else{
                customer.transform.localPosition = new Vector3(-550,-55,0);
                customer.transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>().color = new Color32(255,255,255,255);
            }
        }
        if(toOrderCustomerQueue.Count == 2){
            //If the player isn't looking at the front shop, cancel walk in animation.
            if(gameObject.transform.parent.gameObject.activeSelf){

                //If first position customer is already in fully colored, don't alter them.
                if(toOrderCustomerQueue[0].transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>().color != new Color32(255, 255, 255, 255))
                {
                    StartCoroutine(LerpNPCQueueColors(255, toOrderCustomerQueue[0].transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>()));
                    StartCoroutine(LerpNPCQueuePosition(-550, toOrderCustomerQueue[0]));
                }
                StartCoroutine(LerpNPCQueueColors(155,toOrderCustomerQueue[1].transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>()));
                StartCoroutine(LerpNPCQueuePosition(-650,toOrderCustomerQueue[1]));
            }else{
                toOrderCustomerQueue[0].transform.localPosition = new Vector3(-550,-55,0);
                toOrderCustomerQueue[0].transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>().color = new Color32(255,255,255,255);
                customer.transform.localPosition = new Vector3(-650,-55,0);
                customer.transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>().color = new Color32(155,155,155,255);
            }
            toOrderCustomerQueue[0].transform.SetAsLastSibling();
        }
        if(toOrderCustomerQueue.Count == 3){
            //If the player isn't looking at the front shop, cancel walk in animation.
            if(gameObject.transform.parent.gameObject.activeSelf){
                StartCoroutine(LerpNPCQueueColors(75,toOrderCustomerQueue[2].transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>()));
                StartCoroutine(LerpNPCQueuePosition(-750,toOrderCustomerQueue[2]));
            }else{
                customer.transform.localPosition = new Vector3(-750,-55,0);
                customer.transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>().color = new Color32(75,75,75,255);
            }
            toOrderCustomerQueue[1].transform.SetAsLastSibling();
            toOrderCustomerQueue[0].transform.SetAsLastSibling();
        }
    }

    //This method ensures if the object coroutine was interrupted, customers will still load as normal.
    public void RecheckCustomerVisuals(){
        //Reset the list objects positions and colors if there are customers in it.
        if(toOrderCustomerQueue.Count == 3){
            for (int i = 0; i < toOrderCustomerQueue.Count; i++){
                if(i == 2){
                    toOrderCustomerQueue[2].transform.localPosition = new Vector3(-750,-55,0);
                    toOrderCustomerQueue[2].transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>().color = new Color32(75,75,75,255);
                }
                if(i == 1){
                    toOrderCustomerQueue[1].transform.localPosition = new Vector3(-650,-55,0);
                    toOrderCustomerQueue[1].transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>().color = new Color32(155,155,155,255);
                }
                if(i == 0){
                    toOrderCustomerQueue[0].transform.localPosition = new Vector3(-550,-55,0);
                    toOrderCustomerQueue[0].transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>().color = new Color32(255,255,255,255);
                }
            }
        } 
        if(toOrderCustomerQueue.Count == 2){
            for (int i = 0; i < toOrderCustomerQueue.Count; i++){
            if(i == 1){
                toOrderCustomerQueue[1].transform.localPosition = new Vector3(-650,-55,0);
                toOrderCustomerQueue[1].transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>().color = new Color32(155,155,155,255);
            }
            if(i == 0){
                toOrderCustomerQueue[0].transform.localPosition = new Vector3(-550,-55,0);
                toOrderCustomerQueue[0].transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>().color = new Color32(255,255,255,255);
            }
            }
        }
        if(toOrderCustomerQueue.Count == 1){
            toOrderCustomerQueue[0].transform.localPosition = new Vector3(-550,-55,0);
            toOrderCustomerQueue[0].transform.Find("BobaShopCharacterSprite").GetComponentInChildren<Image>().color = new Color32(255,255,255,255);
        } 
    }

    //This method resets the patience according to when a customer leaves.
    public void ResetOrMovePatience(int customerInxThatLeft)
    {
        //Reset or move Patience timers
        switch (customerInxThatLeft)
        {
            case 0:
                //Reset Q1 timers, as front facing customers have different patience, but reset Q2 as well as future customers need new timers.
                thisRoundOverallInstanceScript.customerTimerInQO1 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInQO1 = 0f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInQO1 = 0f;
                thisRoundOverallInstanceScript.customerTimerInQO2 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInQO2 = 0f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInQO2 = 0f;
                return;
            case 1:
                float q3TimerToMoveUp = thisRoundOverallInstanceScript.customerTimerInQO3;
                float q3TimerSToMoveUp = thisRoundOverallInstanceScript.customerStartingTimeInQO3;
                float q3TimerEToMoveUp = thisRoundOverallInstanceScript.eCustomerEndTimeAInQO3;
                //Reset Q3 timers, and let Q2 have old Q3 timers.
                thisRoundOverallInstanceScript.customerTimerInQO2 = q3TimerToMoveUp;
                thisRoundOverallInstanceScript.customerStartingTimeInQO2 = q3TimerSToMoveUp;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInQO2 = q3TimerEToMoveUp;
                thisRoundOverallInstanceScript.customerTimerInQO3 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInQO3 = 0f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInQO3 = 0f;
                return;
            case 2:
                //Reset Q3 timers
                thisRoundOverallInstanceScript.customerTimerInQO3 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInQO3 = 0f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInQO3 = 0f;
                return;
        }
    }

    //Overloaded method resets the patience anytime the queue changes.
    public void MoveOrResetPatience(bool resetAllPatienceInThisQueue)
    {
        if (resetAllPatienceInThisQueue)
        {
            //Resets ALL Patience timers
            thisRoundOverallInstanceScript.customerTimerInQO1 = 0.1f;
            thisRoundOverallInstanceScript.customerStartingTimeInQO1 = 0f;
            thisRoundOverallInstanceScript.eCustomerEndTimeAInQO1 = 0f;
            thisRoundOverallInstanceScript.customerTimerInQO2 = 0.1f;
            thisRoundOverallInstanceScript.customerStartingTimeInQO2 = 0f;
            thisRoundOverallInstanceScript.eCustomerEndTimeAInQO2 = 0f;
            thisRoundOverallInstanceScript.customerTimerInQO3 = 0.1f;
            thisRoundOverallInstanceScript.customerStartingTimeInQO3 = 0f;
            thisRoundOverallInstanceScript.eCustomerEndTimeAInQO3 = 0f;
        }
        else
        {
            if (toOrderCustomerQueue.Count == 0)
            {
                thisRoundOverallInstanceScript.customerTimerInQO1 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInQO1 = 0f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInQO1 = 0f;
            }
            if (toOrderCustomerQueue.Count == 1)
            {
                thisRoundOverallInstanceScript.customerTimerInQO1 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInQO1 = 0f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInQO1 = 0f;
                thisRoundOverallInstanceScript.customerTimerInQO2 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInQO2 = 0f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInQO2 = 0f;
            }

            if (toOrderCustomerQueue.Count == 2)
            {
                thisRoundOverallInstanceScript.customerTimerInQO1 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInQO1 = 0f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInQO1 = 0f;
                float q3TimerToMoveUp = thisRoundOverallInstanceScript.customerTimerInQO3;
                float q3TimerSToMoveUp = thisRoundOverallInstanceScript.customerStartingTimeInQO3;
                float q3TimerEToMoveUp = thisRoundOverallInstanceScript.eCustomerEndTimeAInQO3;
                //Reset Q3 timers, and let Q2 have old Q3 timers.
                thisRoundOverallInstanceScript.customerTimerInQO2 = q3TimerToMoveUp;
                thisRoundOverallInstanceScript.customerStartingTimeInQO2 = q3TimerSToMoveUp;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInQO2 = q3TimerEToMoveUp;
                thisRoundOverallInstanceScript.customerTimerInQO3 = 0.1f;
                thisRoundOverallInstanceScript.customerStartingTimeInQO3 = 0f;
                thisRoundOverallInstanceScript.eCustomerEndTimeAInQO3 = 0f;
            }
        }

    }

    //This enum is a lerp for the NPC's color.
    public IEnumerator LerpNPCQueueColors(float colorTarget, Image imageToChange)
    {
        float timeElapsed = 0;
        float valueToLerp = 0f;

        while (timeElapsed < 0.5f)
        {
            imageToChange.color = new Color32(((byte)valueToLerp),((byte)valueToLerp),((byte)valueToLerp),255);
            valueToLerp = Mathf.Lerp(0, colorTarget, timeElapsed / 1.5f);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        imageToChange.color = new Color32(((byte)colorTarget),((byte)colorTarget),((byte)colorTarget),255);
    }

    //This enum is a lerp for the NPC's position coming into the line creating the "walking" into the queue animation.
    public IEnumerator LerpNPCQueuePosition(float newPositionDesired, GameObject NPCToMove)
    {
        float timeElapsed = 0;

        while (timeElapsed < NPCToMove.GetComponent<CustomerDrinkScript>().characterShopSpeed)
        {
            float valueToLerp = Mathf.Lerp(NPCToMove.transform.localPosition.x, newPositionDesired, timeElapsed / NPCToMove.GetComponent<CustomerDrinkScript>().characterShopSpeed);
            NPCToMove.transform.localPosition = new Vector3(valueToLerp,math.sin(valueToLerp*math.PI)-55,0);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        NPCToMove.transform.localPosition = new Vector3(NPCToMove.transform.localPosition.x,-55,0);
    }

    //This enum is a lerp for the NPC's position "walking" into the other animation.
    public IEnumerator MoveNPCOtherQueuePosition(float newPositionDesired, GameObject NPCToMove)
    {
        float timeElapsed = 0;

        while (timeElapsed < 10f)
        {
            float valueToLerp = Mathf.Lerp(NPCToMove.transform.localPosition.x, newPositionDesired, timeElapsed / NPCToMove.GetComponent<CustomerDrinkScript>().characterShopSpeed);
            NPCToMove.transform.localPosition = new Vector3(valueToLerp,NPCToMove.transform.localPosition.y, 0);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        NPCToMove.transform.localPosition = new Vector3(NPCToMove.transform.localPosition.x,-55,0);
    }

    //This enum is a lerp for the NPC's position "walking" into the other animation and destroys the NPC.
    public IEnumerator LerpNPCPatienceDestroyer(float newPositionDesired, GameObject NPCToMove)
    {
        float timeElapsed = 0;

        while (timeElapsed < 1.5)
        {
            float valueToLerp = Mathf.Lerp(NPCToMove.transform.localPosition.x, newPositionDesired, timeElapsed / NPCToMove.GetComponent<CustomerDrinkScript>().characterShopSpeed);
            NPCToMove.transform.localPosition = new Vector3(valueToLerp, math.sin(valueToLerp * math.PI) - 55, 0);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        NPCToMove.transform.localPosition = new Vector3(NPCToMove.transform.localPosition.x, -55, 0);
        Destroy(NPCToMove);
    }

    //This method adjusts the animation of the NPC's that enter the scene and makes it so that they appeart to be walking into line.
    //This furthermore adjusts the color values.
}
