using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WipeClothScript : MonoBehaviour
{

    //Gets game Object to check what the player is currently holding
    [SerializeField]
    GameObject itemInHandInventory;
    Animator itemInHandInventoryAnimator;
    [SerializeField]
    GameObject wipeCloth;

    // Start is called before the first frame update
    void Start()
    {
        itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeWipeCloth(){

        if(itemInHandInventory.transform.childCount == 0){
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            Vector3 clothHeld = new Vector3(0f,-.75f,0f);
            Instantiate(wipeCloth,clothHeld,Quaternion.identity,itemInHandInventory.transform);

            //A switch case to resolve and return wipe cloth if possible and not dirty.
        }else if(itemInHandInventory.transform.GetChild(0).gameObject != null){
        
        switch(itemInHandInventory.transform.GetChild(0).gameObject.name){

                case "WipeCloth(Clone)":
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                Destroy(itemInHandInventory.transform.GetChild(0).gameObject);
                break;

                case "WipeClothDirty(Clone)": 
                //Play wrong interaction hand animation.
                itemInHandInventoryAnimator.Play("IncorrectInteraction");
                print("Cannot Return Dirty Cloth!");
                break;

                default:
                //Play wrong interaction hand animation.
                itemInHandInventoryAnimator.Play("IncorrectInteraction");
                break;
            }
        }
    }
}
