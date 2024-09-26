using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerIngredientInput : MonoBehaviour
{
    //GameObjects that interact with the Mixer
    //Gets game Object to find the Game Manager.
    [SerializeField]
    [Tooltip("Input the \"GameManagerObject\" object in here.")]
    GameObject gameManagerObject;

    //Gets Game Manager Script.
    GameManagerScript currentGameManagerInstance;

    //Gets round Object to find the Game Manager.
    [SerializeField]
    [Tooltip("Input the \"RoundManagerObject\" object in here.")]
    GameObject roundManagerObject;

    //Gets Round Manager Script.
    BobaShopRoundManagerScript roundManagerScript;

    //Gets game Object to check what the player is currently holding.
    [SerializeField]
    [Tooltip("Drag the \"itemInHandInventory\" object in here.")]
    GameObject itemInHandInventory;

    //Drag the shop gameobjects items into the co-responding items.
    [SerializeField]
    [Tooltip("Drag the \"Ingredient Item\" objects in each of their gameobjects.")]
    GameObject ShopCassavaBall,ShopPandanLeaf,ShopStrawberryMini,ShopBananaMini,ShopMangoMini,ShopUbeMini;

    //Gets the game object to get the RenderFlavorLevelCorrectly() script.
    [SerializeField]
    [Tooltip("Drag the \"mixerClickableDrinkArea\" object from the mixer into here.")]
    GameObject mixerClickableDrinkAreaObject;

    //Gets game Object that renders the flavor levels.
    [SerializeField]
    [Tooltip("Drag the \"mixerFlavorLevel\" object from the mixer into here.")]
    GameObject mixerFlavorLevelIcon;

    //Get the animator for the boba mixer.
    Animator bobaMixerAnimator;

    //Makes a bool to check if an item has already been placed in the mixer.
    bool itemPlacedInMixer = false;

    // Start is called before the first frame update.
    void Start()
    {
        //Get the game manager objects & round manager objects.
        gameManagerObject = GameObject.Find("GameManagerObject");
        currentGameManagerInstance = gameManagerObject.GetComponent<GameManagerScript>();
        roundManagerObject = GameObject.Find("BobaShopRoundManager");
        roundManagerScript = roundManagerObject.GetComponent<BobaShopRoundManagerScript>();

        bobaMixerAnimator = gameObject.transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame.
    void Update()
    {
        
    }

    //The method that activated if an ingredient is added to this mixer.
    public void PutIngredientInMixer(){
        //If the user is holding a tong and an ingredient, and mixer doesn't have an ingredient, determine what the tong is holding and add it to the mixer.
        if(itemInHandInventory.transform.childCount != 0 && itemInHandInventory.transform.GetChild(1).tag == "Ingredient" && !gameObject.transform.parent.gameObject.transform.Find("MixerSpill") && !itemPlacedInMixer){
        GameObject itemToPutInMixer = itemInHandInventory.transform.GetChild(1).gameObject;
        switch(itemToPutInMixer.name){
            case "ShopCassavaBall(Clone)":
                WrongInteraction();
            break;
            case "ShopPandanLeaf(Clone)":
                itemInHandInventory.transform.GetChild(1).transform.SetParent(mixerFlavorLevelIcon.transform);
                mixerClickableDrinkAreaObject.GetComponent<MixerCupAreaScript>().RerenderIngredientFromTop();
                itemPlacedInMixer = true;
            break;
            case "ShopStrawberryMini(Clone)":
                itemInHandInventory.transform.GetChild(1).transform.SetParent(mixerFlavorLevelIcon.transform);
                mixerClickableDrinkAreaObject.GetComponent<MixerCupAreaScript>().RerenderIngredientFromTop();
                itemPlacedInMixer = true;
            break;
            case "ShopBananaMini(Clone)":
                itemInHandInventory.transform.GetChild(1).transform.SetParent(mixerFlavorLevelIcon.transform);
                mixerClickableDrinkAreaObject.GetComponent<MixerCupAreaScript>().RerenderIngredientFromTop();
                itemPlacedInMixer = true;
            break;
            case "ShopMangoMini(Clone)":
                itemInHandInventory.transform.GetChild(1).transform.SetParent(mixerFlavorLevelIcon.transform);
                mixerClickableDrinkAreaObject.GetComponent<MixerCupAreaScript>().RerenderIngredientFromTop();
                itemPlacedInMixer = true;
            break;
            case "ShopUbeMini(Clone)":
                itemInHandInventory.transform.GetChild(1).transform.SetParent(mixerFlavorLevelIcon.transform);
                mixerClickableDrinkAreaObject.GetComponent<MixerCupAreaScript>().RerenderIngredientFromTop();
                itemPlacedInMixer = true;
            break;
            }
            if(itemPlacedInMixer){
                bobaMixerAnimator.Play("MixerItemPlacedIn");
            }
        }
    }

    public void WrongInteraction(){
        //Play wrong interaction hand animation.
            Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
            itemInHandInventoryAnimator.Play("IncorrectInteraction");
    }

    
    //Used to reset the mixer ingredient in mixer bool when the mixer blends.
    public void UseIngredientInMixerInput(){
        itemPlacedInMixer = false;
    }

    //Getter to get the bool if an ingredient has been placed in the mixer top.
    public bool HasAnIngredientBeenPLacedInMixerTop(){
        return itemPlacedInMixer;
    }

}
