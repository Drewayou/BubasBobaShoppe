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
    [Tooltip("Drag the co-responding prefabs of the flavor level in here.")]
    GameObject mixerFlavorLevelIcon, mixerFlavorLevelLoadedPrefab, mixerFlavorLevelIconDonePrefab, mixerFlavorLevelOutlinePrefab, mixerFlavorLevelToppings;

    //Get the gameobject that takes the input of ingredient from the top of the mixer.
    [SerializeField]
    [Tooltip("Drag the \"ingredientInput\" object in here.")]
    GameObject ingredientInputToMixer;
    
    //Get the mixer button to activate/re-activate.
    [SerializeField]
    [Tooltip("Drag the co-responding prefabs of the flavor level in here.")]
    GameObject mixerButtonInactiveCover;

    //The Bools to check if the mixer is ready to mix.
    public bool mixerHasCupInIt, mixerCupHasBaseInIt, mixerCupHasAddOnInIt;

    //Bool to check if the mixer is even on.
    bool mixerIsMixing = false;

    // Start is called before the first frame update.
    void Start()
    {

    }

    // Update is called once per frame.
    void Update()
    {
        if(!mixerIsMixing){
        //If a boba drink is rendered in this mixer, show that it's done. Else, check if it's an empty cup!
        if(gameObject.transform.childCount != 0 && gameObject.transform.GetChild(0).tag == "BobaDrink" && gameObject.transform.GetChild(0).name != "EmptyCup(Clone)"){
            //Add flavor level done Icon if it's not present. Else, Destroy it if the drink isn't done.
            if(!mixerFlavorLevelIcon.transform.Find(mixerFlavorLevelIconDonePrefab.name + "(Clone)")){
                Instantiate(mixerFlavorLevelIconDonePrefab, mixerFlavorLevelIcon.transform);
            }
            if(!mixerFlavorLevelIcon.transform.Find(mixerFlavorLevelOutlinePrefab.name + "(Clone)")){
                Instantiate(mixerFlavorLevelOutlinePrefab, mixerFlavorLevelIcon.transform);
            }
        }
        
        if(gameObject.transform.childCount == 0 && mixerFlavorLevelIcon.transform.childCount != 0){
            DeleteFlavorLoaded();
        }

        //As long as the mixer has a cup & base liquid in it (Even addons like milk and water count), allow the mixer button to activate.
        if(mixerHasCupInIt && mixerCupHasBaseInIt || mixerCupHasAddOnInIt){
                mixerButtonInactiveCover.SetActive(false);
        }else{
            mixerButtonInactiveCover.SetActive(true);
        }
        }
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
            
        //If Hand Inventory isn't empty and holds an EMPTY CUP ONLY, Put cup into the mixer.
        }else if(handInventory.transform.childCount != 0 && handInventory.transform.GetChild(0).gameObject.name == "EmptyCup(Clone)" && gameObject.transform.childCount == 0 && !mixerSpill){

            //Take Cup in hand and put into mixer.
            handInventory.transform.GetChild(0).gameObject.transform.SetParent(gameObject.transform,false);
            gameObject.transform.GetChild(0).transform.localScale = new Vector3(.12f,.12f,.12f);
            gameObject.transform.GetChild(0).transform.localPosition = new Vector3(0f,0f,0f);

            //Add flavor level cup indicator outline.
            RenderFlavorLevelsCorrectly();

        //Interaction if swapping an empty cup with a current drink in the mixer.
        }else if(handInventory.transform.childCount != 0 && handInventory.transform.GetChild(0).gameObject.name == "EmptyCup(Clone)" && gameObject.transform.childCount != 0 && !mixerSpill){
            swapDoneDrinkWithCup(handInventory);

            //Add flavor level cup indicator outline if it's not already added.
            RenderFlavorLevelsCorrectly();

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
    }

    //Check what tags the empty cup contains, and check if an ingredient is loaded into the mixer.
    public void RenderFlavorLevelsCorrectly(){

        //Reset the flavor level indicator to make sure a new drink is registered correctly.
        DeleteFlavorLoaded();

        if(gameObject.transform.childCount > 0){

            //Set Empty Boba cup outline icon in the back.
                if(gameObject.transform.GetChild(0).name == "EmptyCup(Clone)"){
                    Instantiate(mixerFlavorLevelOutlinePrefab, mixerFlavorLevelIcon.transform);
                    mixerHasCupInIt = true;
                }

            foreach(Transform bobaItemTransform in gameObject.transform.GetChild(0)){

                //Set Flavor Level if a flavor base icon is present in the cup.
                if(bobaItemTransform.tag == "LiquidBase" | bobaItemTransform.tag == "LiquidBaseAddition" && mixerCupHasBaseInIt == false){
                    Instantiate(mixerFlavorLevelLoadedPrefab, mixerFlavorLevelIcon.transform);
                    mixerFlavorLevelIcon.transform.SetAsLastSibling();
                    mixerCupHasBaseInIt = true;
                }

                //Set Toppings in the boba cup icon if they are present.
                if(bobaItemTransform.tag == "BobaToppings"){
                    Instantiate(mixerFlavorLevelToppings, mixerFlavorLevelIcon.transform);
                    mixerFlavorLevelIcon.transform.SetAsLastSibling();
                }
            }
        }
        //Render the main ingredient on top.
        RerenderIngredientFromTop();
    }

    //Makes sure that the right flavors are loaded on the mixer. If not, remove the icons.
    public void DeleteFlavorLoaded(){
        //Delete Empty Boba cup outline icon in the back if the cup isn't placed in.
                if(mixerFlavorLevelIcon.transform.Find("FlavorLevelOutline(Clone)")){
                        Destroy(mixerFlavorLevelIcon.transform.Find("FlavorLevelOutline(Clone)").gameObject);
                }

                //Delete Flavor Level if a flavor base icon isn't present in the cup.
                if(mixerFlavorLevelIcon.transform.Find("FlavorLevelLoaded(Clone)")){
                        Destroy(mixerFlavorLevelIcon.transform.Find("FlavorLevelLoaded(Clone)").gameObject);
                }

                //Delete Toppings in the boba cup icon if they aren't present.
                if(mixerFlavorLevelIcon.transform.Find("FlavorLevelToppingsInCup(Clone)")){
                        Destroy(mixerFlavorLevelIcon.transform.Find("FlavorLevelToppingsInCup(Clone)").gameObject);
                }

                //Delete FlavorLevelLoaded in the boba cup icon if they aren't present.
                if(mixerFlavorLevelIcon.transform.Find("FlavorLevelDone(Clone)")){
                        Destroy(mixerFlavorLevelIcon.transform.Find("FlavorLevelDone(Clone)").gameObject);
                }
            //Reset the mixer to be unable to mix.
            mixerCupHasAddOnInIt = false;
            mixerCupHasBaseInIt = false;
            mixerHasCupInIt = false;
    }

    public void RerenderIngredientFromTop(){
        //Set & check if an ingredient was loaded in.
            foreach(Transform flavorLevelTransform in mixerFlavorLevelIcon.transform){
                if(flavorLevelTransform.tag == "Ingredient"){
                    flavorLevelTransform.transform.localPosition = new Vector3(0,25,0);
                    flavorLevelTransform.transform.localScale = new Vector3(.7f,.7f,.7f);
                    flavorLevelTransform.transform.SetAsLastSibling();
                }
            }
    }
}
