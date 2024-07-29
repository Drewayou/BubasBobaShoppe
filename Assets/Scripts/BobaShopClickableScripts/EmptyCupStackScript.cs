using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCupStackScript : MonoBehaviour
{

    //Gets game Object to check what the player is currently holding
    [SerializeField]
    GameObject itemInHandInventory;
    [SerializeField]
    GameObject emptyCup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeCup(){

        if(itemInHandInventory.transform.childCount == 0){
            Instantiate(emptyCup,itemInHandInventory.transform);
        }else if(itemInHandInventory.transform.GetChild(0).gameObject.name == "EmptyCup(Clone)"){
            Destroy(itemInHandInventory.transform.GetChild(0).gameObject);
        }
    }
}
