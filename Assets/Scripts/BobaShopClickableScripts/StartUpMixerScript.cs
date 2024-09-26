using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpMixerScript : MonoBehaviour
{

    //The Game manager instance of this round (Will automatically be pulled in Start() method.
    private GameManagerScript currentGameManagerInstance;

    //The Game manager instance of this round (Will automatically be pulled in Start() method.
    private BobaShopRoundManagerScript currentRoundManagerInstance;

    //The gameobject for the mixer's top that contains the ingredient pre-mix.
    [SerializeField]
    [Tooltip("Drag the mixer's \"Prefab\" object here (The main prefab object).")]
    public GameObject mixerOverallPrefabObject;

    //Get the animator from this gameobject.
    Animator thisBobaMixerAnimator;

    //The mixer doors in this mixer prefab that overlays the drinks in the mixer.
    private GameObject mixerDoorsObject;

    //The gameobject for the drink area that contains the drink pre-mix.
    [SerializeField]
    [Tooltip("Drag the mixer clickable drink area here.")]
    public GameObject drinkInMixerClickableArea;

    //The gameobject for the mixer's top that contains the ingredient pre-mix.
    [SerializeField]
    [Tooltip("Drag the mixer's \"IngredientInput\" object here.")]
    public GameObject mixerIngredientInput;

    //The gameobject for the drinks that can be made.
    [SerializeField]
    [Tooltip("Drag the possible drink outcomes here.")]
    public GameObject FailedDrink, WaterCup, MilkCup, PlainGreenTea, PlainOolongTea, PlainBlackTea, MilkOverlay, WaterOverlay, GreenTeaOverlay, OolongOverlay, BlackTeaOverlay;

    //The gameobject for the possible toppings that can be made.
    [SerializeField]
    [Tooltip("Drag the possible drink outcomes here.")]
    public GameObject BobaToppingsInDrinkOverlay1, BobaToppingsInDrinkOverlay2, BobaToppingsInDrinkOverlay3, BobaToppingsInDrinkOverlay4, BobaToppingsInDrinkOverlay5;

    //A game object to temporarily save the base of the new drink made.
    private GameObject baseOfNewDrink;

    //The gameobject for the mixer's top that contains the ingredient pre-mix.
    [SerializeField]
    [Tooltip("Drag the mixer spill prefab object here.")]
    public GameObject mixerSpillObject;

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

    //FIXME: Add a timer for the mixing process.
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
            
            //A temp variable gameobject to set the new drink name
            GameObject drinkMade;

            //A temp variable gameobject to save the old drink in the mixer.
            GameObject drinkInMixer = drinkInMixerClickableArea.transform.GetChild(0).gameObject;

                //If the cup in the mixer only has one child count, specifically for drinks like water cups, milk cups, ect, and no ingredient has been placed in the top of the mixer.
                if(drinkInMixer.transform.childCount == 1 && !mixerIngredientInput.GetComponent<MixerIngredientInput>().HasAnIngredientBeenPLacedInMixerTop()){
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

                //Else, if the cup in the mixer has more than 1 item in it, and no ingredient has been placed in the top of the mixer.
                }else if(drinkInMixer.transform.childCount > 1 && !mixerIngredientInput.GetComponent<MixerIngredientInput>().HasAnIngredientBeenPLacedInMixerTop()){
                    
                    foreach(Transform ingredientInCup in drinkInMixer.transform){
                        
                        //If a liquid base is present, instantiate it first.
                        if(ingredientInCup.tag == "LiquidBase"){
                        switch(drinkInMixer.transform.GetChild(0).name){
                                case "GreenTeaBase(Clone)":
                                baseOfNewDrink  = Instantiate(PlainGreenTea, drinkInMixerClickableArea.transform);
                                baseOfNewDrink.name = PlainGreenTea.name;
                                RescaleNewDrink(baseOfNewDrink);
                                break;
                                case "OolongBase(Clone)":
                                baseOfNewDrink  = Instantiate(PlainOolongTea, drinkInMixerClickableArea.transform);
                                baseOfNewDrink.name = PlainOolongTea.name;
                                RescaleNewDrink(baseOfNewDrink);
                                break;
                                case "BlackTeaBase(Clone)":
                                baseOfNewDrink  = Instantiate(PlainBlackTea, drinkInMixerClickableArea.transform);
                                baseOfNewDrink.name = PlainBlackTea.name;
                                RescaleNewDrink(baseOfNewDrink);
                                break;
                            }
                    }

                        //If a liquid base addition is present, & base was made, instantiate it. Else, make it the new base.
                        if(ingredientInCup.tag == "LiquidBaseAddition"){
                            switch(ingredientInCup.name){

                                case "WaterBaseAddition(Clone)":
                                if(baseOfNewDrink == null){
                                     baseOfNewDrink  = Instantiate(WaterCup, drinkInMixerClickableArea.transform);
                                     baseOfNewDrink.name = WaterCup.name;
                                     RescaleNewDrink(baseOfNewDrink);
                                }else{
                                    drinkMade = Instantiate(WaterOverlay, baseOfNewDrink.transform);
                                    RescaleOverlay(drinkMade);
                                }
                                break;

                                case "MilkBaseAddition(Clone)":
                                if(baseOfNewDrink == null){
                                     baseOfNewDrink  = Instantiate(MilkCup, drinkInMixerClickableArea.transform);
                                     baseOfNewDrink.name = MilkCup.name;
                                     RescaleNewDrink(baseOfNewDrink);
                                }else{
                                    drinkMade = Instantiate(MilkOverlay, baseOfNewDrink.transform);
                                    RescaleOverlay(drinkMade);
                                }
                                break;
                            }
                        }

                        //FIXME: Add other toppings when they're made here. If a boba topping is present, instantiate it.
                        if(ingredientInCup.tag == "BobaToppings" && baseOfNewDrink.name != null){
                            switch(ingredientInCup.name){
                                case "BobaToppingInCup(Clone)":
                                GameObject bobaOverlayGenerated = randomizeWhichBobaOverLayToMake();
                                drinkMade = Instantiate(bobaOverlayGenerated, baseOfNewDrink.transform);
                                RescaleOverlay(drinkMade);
                                break;
                            }
                        }
                    }
                    //Erase the other drink in the mixer.
                    EraseDrinkPreMix(drinkInMixer);
                }else{
                    makeFailedDrink();
                }
                
        }

        //Check if drink is valid, and use the possibility rate of machine failure.
        attemptPossibleDrinkFailure();
    }

    //A method to check if the drink is valid (WHO PUTS BOBA IN WATER?!), or if the machine unfortunatly does a fauilire (Dictated by the PlayerData if they have a high quality mixer or not).
    public void attemptPossibleDrinkFailure(){
        //FIXME: Currently, the mixer has a 20% chance of making a failed drink if the failure rate is default at 5.
        int failureCheck = Random.Range(1,currentGameManagerInstance.GetComponent<GameManagerScript>().ReturnPlayerStats().mixerFailureRate);

        if(failureCheck == 1){
            foreach(Transform drinksInMixer in drinkInMixerClickableArea.transform){
                Destroy(drinksInMixer.gameObject);
            }
            makeFailedDrink();
            GameObject spill = Instantiate(mixerSpillObject, mixerOverallPrefabObject.transform);
            spill.transform.localPosition = new Vector3(-3.5f,-40f,0f);
            spill.transform.localScale = new Vector3(.6f,.6f,.6f);
        }
    }

    //A method to make a failed drink.
    public void makeFailedDrink(){
        GameObject drinkToMake  = Instantiate(FailedDrink, drinkInMixerClickableArea.transform);
        drinkToMake.name = FailedDrink.name;
        RescaleNewDrink(drinkToMake);
    }


    //Erases the pre-mixed drink in the mixerer to make room for the new drink.
    public void EraseDrinkPreMix(GameObject thisDrink){
        baseOfNewDrink = null;
        Destroy(thisDrink);
    }

    //A method to rescale the new drink spawned.
    public void RescaleNewDrink(GameObject newDrinkSpawned){
        newDrinkSpawned.transform.localScale = new Vector3(.12f,.12f,.12f);
        newDrinkSpawned.transform.localPosition = new Vector3(0f,0f,0f);
    }

    //A method to rescale the overlays of the new drink.
    public void RescaleOverlay(GameObject newOverlaySpawned){
        newOverlaySpawned.transform.localScale = new Vector3(1f,1f,1f);
        newOverlaySpawned.transform.localPosition = new Vector3(0f,0f,0f);
    }

    //A method to randomly generate a boba overlay topping.
    public GameObject randomizeWhichBobaOverLayToMake(){
        //Randomply picks a number from 1-10.
        int randomIntGenerated = Random.Range(1, 11);
        
        if(randomIntGenerated < 3){
            return BobaToppingsInDrinkOverlay1;
        }

        if(randomIntGenerated < 5 && randomIntGenerated > 2){
            return BobaToppingsInDrinkOverlay2;
        }

        if(randomIntGenerated < 7 && randomIntGenerated > 4){
            return BobaToppingsInDrinkOverlay3;
        }

        if(randomIntGenerated == 7){
            return BobaToppingsInDrinkOverlay4;
        }

        if(randomIntGenerated == 8){
            return BobaToppingsInDrinkOverlay5;
        }

        if(randomIntGenerated > 8){
            return BobaToppingsInDrinkOverlay1;
        }
        return BobaToppingsInDrinkOverlay2;
    }
}
