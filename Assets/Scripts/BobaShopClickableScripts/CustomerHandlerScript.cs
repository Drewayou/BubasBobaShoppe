using System.Collections.Generic;
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

    //This method is used by the boba shop game manager to put the customers into this queue.
    public void AddCustomerToThisQueue(GameObject customerToAdd){
        toOrderCustomerQueue.Add(customerToAdd);
        VisuallyAddNewCustomerToTheLine(customerToAdd);
    }

    //FIXME: Add animation to this customer line when a new customer is added.
    //This method organizes the visual placement of the customers on the screen and sets their clickable mask on/off depending what place they get in line.
    public void VisuallyAddNewCustomerToTheLine(GameObject customerToAdd){

        //Spawn the new customer object, set it's name and place it off screen.
        GameObject customerObject = Instantiate(customerToAdd);
        customerObject.name = customerToAdd.name;
        customerObject.transform.SetParent(this.gameObject.transform,false);
        customerObject.transform.localPosition = new Vector3(0f,0f,0f);
        customerObject.transform.localScale = new Vector3(0.6f,0.6f,0.6f);

        if(toOrderCustomerQueue.Count == 1){

            //FIXME: Temp placement.
            customerObject.GetComponent<Image>().raycastTarget = true;
            
            //Add animation...
        }else if(toOrderCustomerQueue.Count == 2){

            customerObject.GetComponent<Image>().raycastTarget = false;
            //Add animation...
        }else{

            customerObject.GetComponent<Image>().raycastTarget = false;
            //Add animation...
        }
    }

    //FIXME: Add animation to this customer line when a customer's order is taken (After all dialogue has been activated by customer's custom scripts).
    //This method is activated when an order is truly placed by the customer and it pushes all other customers forward and the main customer to the other queue.
    public void TakeCustomerOrderNAnimateAction(){
        //Check the game object that this script is attached to (the "CustomerQueueHandler" GameObject) to move it's customer to the next queue.
        GameObject customerThatOrderedADrink = this.gameObject.transform.GetChild(0).gameObject;
        CustomerDrinkWaitQueueHandler.GetComponent<CustomerWaitingHandlerScript>().AddCustomerToThisWatingQueue(customerThatOrderedADrink);
    }
}
