using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobaLadelScript : MonoBehaviour
{

    //Gets game Object to check what the player is currently holding
    [SerializeField]
    GameObject itemInHandInventory;
    [SerializeField]
    GameObject bobaLadel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeBobaLadel(){

        if(itemInHandInventory.transform.childCount == 0){
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            Vector3 ladelHeld = new Vector3(0f,-.6f,0f);
            Instantiate(bobaLadel,ladelHeld,Quaternion.identity,itemInHandInventory.transform);
        }else if(itemInHandInventory.transform.GetChild(0).gameObject.name == "BobaLadel(Clone)"){
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Destroy(itemInHandInventory.transform.GetChild(0).gameObject);
        }else{
            //Play wrong interaction hand animation.
            Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
            itemInHandInventoryAnimator.Play("IncorrectInteraction");
        }
    }
}
