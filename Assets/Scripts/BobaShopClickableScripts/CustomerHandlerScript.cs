using System.Collections.Generic;
using UnityEngine;

public class CustomerHandlerScript : MonoBehaviour
{
    // Gameobject list that hold the to-order customer Queue. Or customers waiting to order.
    public List<GameObject> toOrderCustomerQueue;

    // List that hold the waiting-for-order customer Queue. Or customers waiting for their finished drink/product.
    public List<GameObject> waitingForOrderCustomerQueue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
