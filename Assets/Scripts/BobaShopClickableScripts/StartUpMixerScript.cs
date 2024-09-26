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

    //The gameobject for the drink area that contains the drink pre-mix.
    [SerializeField]
    [Tooltip("Drag the mixer clickable drink area here.")]
    public GameObject drinkInMixerClickableArea;

    //The gameobject for the drinks that can be made.
    [SerializeField]
    [Tooltip("Drag the possible drink outcomes here.")]
    public GameObject FailedDrink, WaterCup, MilkCup, PlainGreenTea, PlainOolongTea, PlainBlackTea;

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

            //Dispense thedrink made from the ingredients.
            DispenseDrinkMade();

            //Open the mixer doors.
            mixerDoorsObject.GetComponent<Animator>().Play("MixerDoorsOpen");
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

    //Check what tags the empty cup contains, and check if an ingredient is loaded into the mixer.
    public void DispenseDrinkMade(){

        if(drinkInMixerClickableArea.transform.childCount > 0){
            
            //A variable gameobject to set the new drink name
            GameObject drinkMade;

            //Set Empty Boba cup outline icon in the back.
            GameObject drinkInMixer = drinkInMixerClickableArea.transform.GetChild(0).gameObject;

                //If the mixer only has one child count, specifically for drinks like water cups, milk cups, ect.
                if(drinkInMixer.transform.childCount == 1){
                    switch(drinkInMixer.transform.GetChild(0).gameObject.tag){
                        case "LiquidBase":
                            switch(drinkInMixer.transform.GetChild(0).name){
                                case "GreenTeaBase(Clone)":
                                EraseDrinkPreMix(drinkInMixer);
                                drinkMade  = Instantiate(PlainGreenTea, drinkInMixerClickableArea.transform);
                                drinkMade.name = PlainGreenTea.name;
                                RescaleNewDrink(drinkMade);
                                break;
                                case "OolongBase(Clone)":
                                EraseDrinkPreMix(drinkInMixer);
                                drinkMade  = Instantiate(PlainOolongTea, drinkInMixerClickableArea.transform);
                                drinkMade.name = PlainOolongTea.name;
                                RescaleNewDrink(drinkMade);
                                break;
                                case "BlackTeaBase(Clone)":
                                EraseDrinkPreMix(drinkInMixer);
                                drinkMade  = Instantiate(PlainBlackTea, drinkInMixerClickableArea.transform);
                                drinkMade.name = PlainBlackTea.name;
                                RescaleNewDrink(drinkMade);
                                break;
                            }
                        break;

                        case "LiquidBaseAddition":
                            switch(drinkInMixer.transform.GetChild(0).name){
                                case "WaterBaseAddition(Clone)":
                                EraseDrinkPreMix(drinkInMixer);
                                drinkMade  = Instantiate(WaterCup, drinkInMixerClickableArea.transform);
                                drinkMade.name = WaterCup.name;
                                RescaleNewDrink(drinkMade);
                                break;
                                case "MilkBaseAddition(Clone)":
                                EraseDrinkPreMix(drinkInMixer);
                                drinkMade  = Instantiate(MilkCup, drinkInMixerClickableArea.transform);
                                drinkMade.name = MilkCup.name;
                                RescaleNewDrink(drinkMade);
                                break;
                            }
                        break;
                    }
                }else{
                    EraseDrinkPreMix(drinkInMixer);
                    drinkMade  = Instantiate(FailedDrink, drinkInMixerClickableArea.transform);
                    drinkMade.name = FailedDrink.name;
                    RescaleNewDrink(drinkMade);
                }
        }
    }

    //Erases the pre-mixed drink in the mixerer to make room for the new drink.
    public void EraseDrinkPreMix(GameObject thisDrink){
        Destroy(thisDrink);
    }

    //A method to rescale the new drink spawned.
    public void RescaleNewDrink(GameObject newDrinkSpawned){
        newDrinkSpawned.transform.localScale = new Vector3(.12f,.12f,.12f);
        newDrinkSpawned.transform.localPosition = new Vector3(0f,0f,0f);
    }
}
