using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MixerCupAreaScript : MonoBehaviour
{

    //Gets game Object to check what the player is currently holding.
    [SerializeField]
    GameObject itemInHandInventory;
    Animator itemInHandInventoryAnimator;

    //Set the empty cup prefab to analyze if it can go in the mixer or not.
    [SerializeField]
    GameObject emptyCup;

    //The objects to determine the mixer mixing process and how far along the mixer is in the process. Spawns the other mixer level prefabs in it.
    [SerializeField]
    GameObject mixerFlavorLevelIcon, mixerFlavorLevelIconPrefab, mixerFlavorLevelIconDonePrefab, mixerFlavorLevelOutlinePrefab;

    // Start is called before the first frame update.
    void Start()
    {

    }

    // Update is called once per frame.
    void Update()
    {
        
    }

    public void InteractWithMixerCupArea(){

        GameObject handInventory = GameObject.Find("ItemInHand");
        Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
        
        //Check if this mixer has a spill!
        bool mixerSpill;
        if(gameObject.transform.parent.Find("MixerSpill") == null){
            mixerSpill = false;
        }else{
            itemInHandInventoryAnimator.Play("IncorrectInteraction");
            print("You have to clean up this mess first!");
            mixerSpill = true;}
        

        //Do interaction when clicked.
        if(handInventory.transform.childCount == 0 && gameObject.transform.childCount != 0 && !mixerSpill){
            
            //Move Cup in mixer and put into inventory.
            gameObject.transform.GetChild(0).gameObject.transform.SetParent(handInventory.transform,false);
            handInventory.transform.GetChild(0).transform.position = new Vector3(0f,-.75f,0f);
            handInventory.transform.GetChild(0).transform.localScale = new Vector3(3f,3f,3f);

            //Delete flavor level cup indicator.
            if(mixerFlavorLevelIcon.transform.Find("FlavorLevelOutline(Clone)")){
                Destroy(mixerFlavorLevelIcon.transform.Find("FlavorLevelOutline(Clone)").gameObject);
            }
            
        //If Hand Inventory isn't empty and holds an EMPTY CUP ONLY, Put cup into the mixer.
        }else if(handInventory.transform.childCount != 0 && handInventory.transform.GetChild(0).gameObject.name == "EmptyCup(Clone)" && gameObject.transform.childCount == 0 && !mixerSpill){

            //Take Cup in hand and put into mixer.
            handInventory.transform.GetChild(0).gameObject.transform.SetParent(gameObject.transform,false);
            gameObject.transform.GetChild(0).transform.localScale = new Vector3(.12f,.12f,.12f);
            gameObject.transform.GetChild(0).transform.localPosition = new Vector3(0f,0f,0f);

            //Add flavor level cup indicator outline.
            Instantiate(mixerFlavorLevelOutlinePrefab, mixerFlavorLevelIcon.transform);

        //Interaction if swapping an empty cup with a current drink in the mixer.
        }else if(handInventory.transform.childCount != 0 && handInventory.transform.GetChild(0).gameObject.name == "EmptyCup(Clone)" && gameObject.transform.childCount != 0 && !mixerSpill){
            swapDoneDrinkWithCup(handInventory);

        //Interaction if a different object than an empty cup is held while clicked on mixer.
        }else if(handInventory.transform.childCount != 0 && handInventory.transform.GetChild(0).gameObject.name != "EmptyCup(Clone)" && !mixerSpill){
            print("You can only place empty cups (or empty cups with toppings and/or basedrink) into the boba mixer!");
            //Play wrong interaction hand animation.
            itemInHandInventoryAnimator.Play("IncorrectInteraction");
        }
    }

    public void swapDoneDrinkWithCup(GameObject handInventory){
        gameObject.transform.GetChild(0).gameObject.transform.SetParent(handInventory.transform,false);
        handInventory.transform.GetChild(0).gameObject.transform.SetParent(gameObject.transform,false);
            handInventory.transform.GetChild(0).transform.position = new Vector3(0f,-.75f,0f);
            handInventory.transform.GetChild(0).transform.localScale = new Vector3(3f,3f,3f);
            gameObject.transform.GetChild(0).transform.localScale = new Vector3(.12f,.12f,.12f);
            gameObject.transform.GetChild(0).transform.localPosition = new Vector3(0f,0f,0f);

            //Delete flavor level indicators beside cup outline.
            if(mixerFlavorLevelIcon.transform.Find("FlavorLevelLoaded(Clone)")||mixerFlavorLevelIcon.transform.Find("FlavorLevelDone(Clone)")){
            Destroy(mixerFlavorLevelIcon.transform.Find("FlavorLevelLoaded(Clone)").gameObject);
            Destroy(mixerFlavorLevelIcon.transform.Find("FlavorLevelDone(Clone)").gameObject);
            }
    }
}
