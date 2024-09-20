using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpMixerScript : MonoBehaviour
{

    //The Game manager instance of this round (Will automatically be pulled in Start() method.
    private GameManagerScript currentGameManagerInstance;

    //The Game manager instance of this round (Will automatically be pulled in Start() method.
    private BobaShopRoundManagerScript currentRoundManagerInstance;

    //Get the animator from this gameobject.
    Animator thisBobaMixerAnimator;

    //The mixer doors in this mixer prefab that overlays the drinks in the mixer.
    private GameObject mixerDoorsObject;

    // Start is called before the first frame update
    void Start()
    {
        //Finds the Game Manager in this instance.
        currentGameManagerInstance = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
        currentRoundManagerInstance = GameObject.Find("BobaShopRoundManager").GetComponent<BobaShopRoundManagerScript>();


        //Get the mixer doors in this prefab.
        mixerDoorsObject = gameObject.transform.parent.transform.GetChild(2).gameObject;

        //Get the animator from this gameobject.
        thisBobaMixerAnimator = gameObject.transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMixingADrink(){

        //Start mixing the drink!
        if(MixerIsClean()/*&& MixerHasIngredients()*/){
            thisBobaMixerAnimator.Play("MixerDoorsClose");
            mixerDoorsObject.GetComponent<Animator>().Play("MixerDoorsClose");
        }
        
    }

    //Check if this mixer has a spill!
    public bool MixerIsClean(){
        if(gameObject.transform.parent.Find("MixerSpill") == null){
            return true;
        }else{
                print("You have to clean up this mess first!");
                return false;
            }
    }

    //FIXME: Check if mixer has the minimum ammount of ingredients needed.
    public bool MixerHasIngredients(){
        return true;
    }
}
