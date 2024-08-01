using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class MixerCupAreaScript : MonoBehaviour
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

    public void InteractWithMixerCupArea(){

        GameObject handInventory = GameObject.Find("ItemInHand");

        //Check if this mixer has a spill!
        bool mixerSpill;
        if(gameObject.transform.parent.Find("MixerSpill") == null){
            mixerSpill = false;
        }else{
            print("You have to clean up this mess first!");
            mixerSpill = true;}
        

        //Do interaction when clicked.
        if(handInventory.transform.childCount == 0 && gameObject.transform.childCount != 0 && !mixerSpill){
            
            //Move Cup in mixer and put into inventory.
            gameObject.transform.GetChild(0).gameObject.transform.SetParent(handInventory.transform,false);
            handInventory.transform.GetChild(0).transform.position = new Vector3(0f,-.75f,0f);
            handInventory.transform.GetChild(0).transform.localScale = new Vector3(3f,3f,3f);
        
        //If Hand Inventory isn't empty and holds an EMPTY CUP ONLY, Put cup into the mixer.
        }else if(handInventory.transform.childCount != 0 && handInventory.transform.GetChild(0).gameObject.name == "EmptyCup(Clone)" && gameObject.transform.childCount == 0 && !mixerSpill){

            //Move Cup in hand and put into mixer.
            handInventory.transform.GetChild(0).gameObject.transform.SetParent(gameObject.transform,false);
            gameObject.transform.GetChild(0).transform.localScale = new Vector3(.12f,.12f,.12f);
            gameObject.transform.GetChild(0).transform.localPosition = new Vector3(0f,0f,0f);

        //Interaction if swapping an empty cup with a current drink in the mixer.
        }else if(handInventory.transform.childCount != 0 && handInventory.transform.GetChild(0).gameObject.name == "EmptyCup(Clone)" && gameObject.transform.childCount != 0 && !mixerSpill){
            swapdrinks(handInventory);

        //Interaction if a different object than an empty cup is held while clicked on mixer.
        }else if(handInventory.transform.childCount != 0 && handInventory.transform.GetChild(0).gameObject.name != "EmptyCup(Clone)" && !mixerSpill){
            print("You can only place empty cups (or empty cups with toppings) into the boba mixer!");
            
        }
    }

    public void swapdrinks(GameObject handInventory){
        gameObject.transform.GetChild(0).gameObject.transform.SetParent(handInventory.transform,false);
        handInventory.transform.GetChild(0).gameObject.transform.SetParent(gameObject.transform,false);
            handInventory.transform.GetChild(0).transform.position = new Vector3(0f,-.75f,0f);
            handInventory.transform.GetChild(0).transform.localScale = new Vector3(3f,3f,3f);
            gameObject.transform.GetChild(0).transform.localScale = new Vector3(.12f,.12f,.12f);
            gameObject.transform.GetChild(0).transform.localPosition = new Vector3(0f,0f,0f);
    }
}
