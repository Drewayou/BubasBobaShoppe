using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInHandScript : MonoBehaviour
{

    //Gets game Object to check what the player is currently holding
    [SerializeField]
    GameObject ItemInHandInventory;
    public bool HasItemInHand;

    // Start is called before the first frame update
    void Start()
    {
        ItemInHandInventory = gameObject.GetComponentInChildren<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
