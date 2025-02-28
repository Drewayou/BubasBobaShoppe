using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class CustomerHandlerScript : MonoBehaviour
{
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
        
    }

    // Awake is called when this script is on.
    void OnEnable()
    {
        RecheckCustomerVisuals();
    }

    //FIXME: Add animation to this customer line when a new customer is added.
    //This method Adds and organizes the visual placement of the customers on the screen and sets their clickable mask on/off depending what place they get in line.
    public void AddCustomerToThisQueue(GameObject customerToAdd){

        //Spawn the new customer object, set it's name and place it off screen.
        GameObject customerObject = Instantiate(customerToAdd);
        customerObject.name = customerToAdd.name;
        customerObject.transform.SetParent(this.gameObject.transform,false);
        customerObject.transform.localPosition = new Vector3(-1200f,0f,0f);
        customerObject.transform.localScale = new Vector3(0.6f,0.6f,0.6f);

        toOrderCustomerQueue.Add(customerObject);

        AdjustRayCasts();
        AdjustColorNAnimationOfNewCustomer(customerObject);
    }

    //FIXME: Add animation to this customer line when a customer's order is taken (After all dialogue has been activated by customer's custom scripts).
    //This method is activated when an order is truly placed by the customer and it pushes all other customers forward and the main customer to the other queue.
    public void TakeCustomerOrderNAnimateAction(){
        //Check the game object that this script is attached to (the "CustomerQueueHandler" GameObject) to move it's customer to the next queue.
        GameObject customerThatOrderedADrink = this.gameObject.transform.GetChild(0).gameObject;
        CustomerDrinkWaitQueueHandler.GetComponent<CustomerWaitingHandlerScript>().AddCustomerToThisWatingQueue(customerThatOrderedADrink);
        AdjustRayCasts();
    }

    //This method adjusts the raycast and color values of the NPC's in this queue.
    public void AdjustRayCasts(){
        for(int i = 0; i < toOrderCustomerQueue.Count; i++){
            if(i == 0){
                toOrderCustomerQueue[i].GetComponent<Image>().raycastTarget = true;
            }else{
                toOrderCustomerQueue[i].GetComponent<Image>().raycastTarget = false;
            }
        }
    }

    //This method adjusts the animation and color of the new customer coming into the queue.
    public void AdjustColorNAnimationOfNewCustomer(GameObject customer){
        if(toOrderCustomerQueue.Count == 1){
            //If the player isn't looking at the front shop, cancel walk in animation.
            if(gameObject.transform.parent.gameObject.activeSelf){
                StartCoroutine(LerpNPCQueueColors(255,toOrderCustomerQueue[0].GetComponent<Image>()));
                StartCoroutine(LerpNPCQueuePosition(-550,toOrderCustomerQueue[0]));
            }else{
                customer.transform.localPosition = new Vector3(-550,-55,0);
                customer.GetComponent<Image>().color = new Color32(255,255,255,255);
            }
        }
        if(toOrderCustomerQueue.Count == 2){
            //If the player isn't looking at the front shop, cancel walk in animation.
            if(gameObject.transform.parent.gameObject.activeSelf){
                StartCoroutine(LerpNPCQueueColors(155,toOrderCustomerQueue[1].GetComponent<Image>()));
                StartCoroutine(LerpNPCQueuePosition(-650,toOrderCustomerQueue[1]));
            }else{
                customer.transform.localPosition = new Vector3(-650,-55,0);
                customer.GetComponent<Image>().color = new Color32(155,155,155,255);
            }
            toOrderCustomerQueue[0].transform.SetAsLastSibling();
        }
        if(toOrderCustomerQueue.Count == 3){
            //If the player isn't looking at the front shop, cancel walk in animation.
            if(gameObject.transform.parent.gameObject.activeSelf){
                StartCoroutine(LerpNPCQueueColors(75,toOrderCustomerQueue[2].GetComponent<Image>()));
                StartCoroutine(LerpNPCQueuePosition(-750,toOrderCustomerQueue[2]));
            }else{
                customer.transform.localPosition = new Vector3(-750,-55,0);
                customer.GetComponent<Image>().color = new Color32(75,75,75,255);
            }
            toOrderCustomerQueue[1].transform.SetAsLastSibling();
            toOrderCustomerQueue[0].transform.SetAsLastSibling();
        }
    }

    //This method ensures if the object coroutine was interrupted, customers will still load as normal.
    public void RecheckCustomerVisuals(){
        //Reset the list objects positions and colors if there are customers in it.
        print(toOrderCustomerQueue.Count);
        if(toOrderCustomerQueue.Count == 3){
            for (int i = 0; i < toOrderCustomerQueue.Count; i++){
                if(i == 2){
                    toOrderCustomerQueue[2].transform.localPosition = new Vector3(-750,-55,0);
                    toOrderCustomerQueue[2].GetComponent<Image>().color = new Color32(75,75,75,255);
                }
                if(i == 1){
                    toOrderCustomerQueue[1].transform.localPosition = new Vector3(-650,-55,0);
                    toOrderCustomerQueue[1].GetComponent<Image>().color = new Color32(155,155,155,255);
                }
                if(i == 0){
                    toOrderCustomerQueue[0].transform.localPosition = new Vector3(-550,-55,0);
                    toOrderCustomerQueue[0].GetComponent<Image>().color = new Color32(255,255,255,255);
                }
            }
        } 
        if(toOrderCustomerQueue.Count == 2){
            for (int i = 0; i < toOrderCustomerQueue.Count; i++){
            if(i == 1){
                toOrderCustomerQueue[1].transform.localPosition = new Vector3(-650,-55,0);
                toOrderCustomerQueue[1].GetComponent<Image>().color = new Color32(155,155,155,255);
            }
            if(i == 0){
                toOrderCustomerQueue[0].transform.localPosition = new Vector3(-550,-55,0);
                toOrderCustomerQueue[0].GetComponent<Image>().color = new Color32(255,255,255,255);
            }
            }
        }
        if(toOrderCustomerQueue.Count == 1){
            toOrderCustomerQueue[0].transform.localPosition = new Vector3(-550,-55,0);
            toOrderCustomerQueue[0].GetComponent<Image>().color = new Color32(255,255,255,255);
        } 
    }

    //This enum is a lerp for the NPC's color.
    public IEnumerator LerpNPCQueueColors(float colorTarget, Image imageToChange)
    {
        float timeElapsed = 0;
        float valueToLerp = 0f;

        while (timeElapsed < 1.5f)
        {
            imageToChange.GetComponent<Image>().color = new Color32(((byte)valueToLerp),((byte)valueToLerp),((byte)valueToLerp),255);
            valueToLerp = Mathf.Lerp(0, colorTarget, timeElapsed / 1.5f);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        imageToChange.GetComponent<Image>().color = new Color32(((byte)colorTarget),((byte)colorTarget),((byte)colorTarget),255);
    }

    //This enum is a lerp for the NPC's position coming into the line creating the "walking" into the queue animation.
    public IEnumerator LerpNPCQueuePosition(float newPositionDesired, GameObject NPCToMove)
    {
        float timeElapsed = 0;

        while (timeElapsed < 1.5f)
        {
            float valueToLerp = Mathf.Lerp(NPCToMove.transform.localPosition.x, newPositionDesired, timeElapsed / 1.5f);
            NPCToMove.transform.localPosition = new Vector3(valueToLerp,math.sin(valueToLerp*math.PI)-55,0);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        NPCToMove.transform.localPosition = new Vector3(NPCToMove.transform.localPosition.x,-55,0);
    }

    //This method adjusts the animation of the NPC's that enter the scene and makes it so that they appeart to be walking into line.
    //This furthermore adjusts the color values.
}
