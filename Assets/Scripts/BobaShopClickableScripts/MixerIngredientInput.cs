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
    [Tooltip("Input the \"itemInHandInventory\" object in here.")]
    GameObject itemInHandInventory;

    //Drag the shop gameobjects items into the co-responding items.
    [SerializeField]
    [Tooltip("Input the \"Ingredient Item\" objects in each of their gameobjects.")]
    GameObject ShopCassavaBall,ShopPandanLeaf,ShopStrawberryMini,ShopBananaMini,ShopMangoMini,ShopUbeMini;

    //Get the animator for the boba mixer.
    Animator bobaMixerAnimator;

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
        if(itemInHandInventory.transform.childCount != 0 && itemInHandInventory.transform.GetChild(1).tag == "Ingredient" && !gameObject.transform.parent.gameObject.transform.Find("MixerSpill")){
        GameObject itemToPutInMixer = itemInHandInventory.transform.GetChild(1).gameObject;
        bool itemPlacedInMixer = false;
        switch(itemToPutInMixer.name){
            case "ShopCassavaBall(Clone)":
                WrongInteraction();
            break;
            case "ShopPandanLeaf(Clone)":
                itemInHandInventory.transform.GetChild(1).transform.parent =  gameObject.transform.parent;
                itemPlacedInMixer = true;
            break;
            case "ShopStrawberryMini(Clone)":
                itemPlacedInMixer = true;
            break;
            case "ShopBananaMini(Clone)":
                itemPlacedInMixer = true;
            break;
            case "ShopMangoMini(Clone)":
                itemPlacedInMixer = true;
            break;
            case "ShopUbeMini(Clone)":
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
}
