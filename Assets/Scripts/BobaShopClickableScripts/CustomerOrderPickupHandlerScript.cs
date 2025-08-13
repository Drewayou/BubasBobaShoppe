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


    //Checks if there's customers waiting to pick up their drink orders.
    public void CheckIfCustomersAreWaitingForDrinks()
    {
        if (this.gameObject.GetComponent<CustomerWaitingHandlerScript>().waitingForOrderCustomerQueue.Count > 0)
        {
            NextCustomerPicksUpOrder();
        }
        else
        {
            Debug.Log("No customer is waiting for a drink!");
        }
    }

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

        while (timeElapsed < 60)
        {
            float valueToLerp = Mathf.Lerp(NPCToMove.transform.localPosition.x, newPositionDesired, timeElapsed / NPCToMove.GetComponent<CustomerDrinkScript>().characterShopSpeed);
            NPCToMove.transform.localPosition = new Vector3(valueToLerp,math.sin(valueToLerp*math.PI)-55,0);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        NPCToMove.transform.localPosition = new Vector3(NPCToMove.transform.localPosition.x,-55,0);
    }
}
