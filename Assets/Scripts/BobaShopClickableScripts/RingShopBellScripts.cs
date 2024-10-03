using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingShopBellScripts : MonoBehaviour
{

    //Gets game Object to find the Game Manager.
    [SerializeField]
    [Tooltip("Drag the \"GameManagerObject\" object in here.")]
    GameObject gameManagerObject;

    //Gets Game Manager Script.
    GameManagerScript currentGameManagerInstance;

    //Gets game Object to check what the player is currently holding.
    [SerializeField]
    [Tooltip("Drag the \"itemInHandInventory\" object in here.")]
    GameObject itemInHandInventory;
    Animator itemInHandInventoryAnimator;

    //The SFX prefab to make the bell ring sound.
    [SerializeField]
    [Tooltip("Drag the \"GameManagerObject\" object in here.")]
    GameObject bellSFXRing;

    //Gets the bell's animation.
    Animator bellsAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //Get the game manager objects & round manager objects.
        gameManagerObject = GameObject.Find("GameManagerObject");
        currentGameManagerInstance = gameManagerObject.GetComponent<GameManagerScript>();

        //Gets the item in hand animator to let the player know if they're doing the right interaction or not.
        //(Bell cannot be rung unless empty handed).
        itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();

        //Gets the bell's animator fo the ringning effect to play.
        bellsAnimator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //A script that runs when the bell is pressed.
    public void ringShopBell(){
        if(itemInHandInventory.transform.childCount == 0){
        makeBellNoise();
        //FIXME: Add a way to call the next customer in queue 
        //callCustomer();
        }
    }

    public void makeBellNoise(){
        //FIXME: find a way to store all the SFX sound effects.
        Instantiate(bellSFXRing);
        bellsAnimator.Play("BellRing");
    }
    
}
