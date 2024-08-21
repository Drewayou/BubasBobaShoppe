using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongScript : MonoBehaviour
{

    //Gets game Object to check what the player is currently holding
    [SerializeField]
    GameObject itemInHandInventory;
    [SerializeField]
    GameObject tongHeldF, tongHeldB;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeTong(){

        if(itemInHandInventory.transform.childCount == 0){
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            Vector3 clothHeld = new Vector3(0f,-.55f,0f);
            Instantiate(tongHeldF,clothHeld,Quaternion.identity,itemInHandInventory.transform);
            Instantiate(tongHeldB,clothHeld,Quaternion.identity,itemInHandInventory.transform);

            //A switch case to resolve and return tong if possible doesn't hold an ingredient item.
        }else if(itemInHandInventory.transform.GetChild(0).gameObject != null && itemInHandInventory.transform.childCount == 2){
        
        switch(itemInHandInventory.transform.GetChild(0).gameObject.name){

                case "FTongHolding(Clone)":
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                Destroy(itemInHandInventory.transform.GetChild(0).gameObject);
                Destroy(itemInHandInventory.transform.GetChild(1).gameObject);
                break;
            }
        }else{
            //Play wrong interaction hand animation.
            Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
            itemInHandInventoryAnimator.Play("IncorrectInteraction");
        }
    }
}
