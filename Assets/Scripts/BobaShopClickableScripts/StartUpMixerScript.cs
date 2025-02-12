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

    //The gameobject for the mixer's top that contains the ingredient pre-mix.
    [SerializeField]
    [Tooltip("Drag the mixer's \"FlavorLevelIcon\" prefab object here.")]
    public GameObject mixerFlavorLevel;

    //The gameobject for the drinks that can be made.
    [SerializeField]
    [Tooltip("Drag the possible drink outcomes here.")]
    public GameObject FailedDrink, WaterCup, MilkCup, PlainGreenTea, PlainOolongTea, PlainBlackTea, MilkOverlay, WaterOverlay, GreenTeaOverlay, OolongOverlay, BlackTeaOverlay,
    BananaBase, PandanBase, StrawberryBase, MangoBase, UbeBase;

    //The possible levels of temperature for the drinks (FUTURE IMPLEMENTATION) [0 is default, 1 is hot, 2 is iced, 3 is frosty]!
    public int drinkTempMixSetting = 0;

    //The possible levels of sweetness for the drinks (FUTURE IMPLEMENTATION) [0 is default, 1 is 125%, 2 is 150%, 3 is 200%]!
    public int drinkSweetness = 0;

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
    //NOTE : This also gives the drink it's UUID!
    public void DispenseDrinkMade(){

        if(drinkInMixerClickableArea.transform.childCount > 0){
            
            //A temp variable gameobject to set the new drink name
            GameObject drinkMade;

            //A temp variable gameobject to save the old drink in the mixer.
            GameObject drinkInMixer = drinkInMixerClickableArea.transform.GetChild(0).gameObject;

                //BELOW BOC IS IF THE MIXER HAS INGREDIENTS IN THE TOP MIXER!
                //If the mixer has an ingredient in the top, and more than one ingredient overall, make the specific drink.
                if(mixerIngredientInput.GetComponent<MixerIngredientInput>().HasAnIngredientBeenPlacedInMixerTop()){

                    foreach(Transform mixerFlavorLevelItem in mixerFlavorLevel.transform){
                        if(mixerFlavorLevelItem.tag == "Ingredient"){
                            switch(mixerFlavorLevelItem.name){

                                case "ShopPandanLeaf(Clone)":
                                    baseOfNewDrink  = Instantiate(PandanBase, drinkInMixerClickableArea.transform);
                                    baseOfNewDrink.name = PandanBase.name;
                                    RescaleNewDrink(baseOfNewDrink);
                                break;
                                case "ShopStrawberryMini(Clone)":
                                    baseOfNewDrink  = Instantiate(StrawberryBase, drinkInMixerClickableArea.transform);
                                    baseOfNewDrink.name = StrawberryBase.name;
                                    RescaleNewDrink(baseOfNewDrink);
                                break;
                                case "ShopBananaMini(Clone)":
                                    baseOfNewDrink  = Instantiate(BananaBase, drinkInMixerClickableArea.transform);
                                    baseOfNewDrink.name = BananaBase.name;
                                    RescaleNewDrink(baseOfNewDrink);
                                break;
                                case "ShopMangoMini(Clone)":
                                    baseOfNewDrink  = Instantiate(MangoBase, drinkInMixerClickableArea.transform);
                                    baseOfNewDrink.name = MangoBase.name;
                                    RescaleNewDrink(baseOfNewDrink);
                                break;
                                case "ShopUbeMini(Clone)":
                                    baseOfNewDrink  = Instantiate(UbeBase, drinkInMixerClickableArea.transform);
                                    baseOfNewDrink.name = UbeBase.name;
                                    RescaleNewDrink(baseOfNewDrink);
                                break;
                            }
                            //Erase this ingredient in the mixer since it's used to make the boba drink.
                            Destroy(mixerFlavorLevelItem.gameObject);
                            
                        }
                    }

                    //Get base object BobaCupUIDSettingsScript
                    BobaCupUIDSettingsScript drinkUIDScript = baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>();
                    
                    foreach(Transform ingredientInCup in drinkInMixer.transform){
                        
                        if(ingredientInCup.tag == "LiquidBase"){
                        //If a liquid base is present, instantiate it first.
                        drinkUIDScript.hasTeaOverlay = true;

                        switch(drinkInMixer.transform.GetChild(0).name){
                                case "GreenTeaBase(Clone)":
                                drinkMade  = Instantiate(GreenTeaOverlay, baseOfNewDrink.transform);
                                drinkMade.name = PlainGreenTea.name;
                                RescaleNewDrink(baseOfNewDrink);
                                drinkUIDScript.drinkUID += "GB";
                                drinkUIDScript.teaOverlaySelectionNumber = 0;
                                break;
                                case "OolongBase(Clone)":
                                drinkMade  = Instantiate(OolongOverlay, baseOfNewDrink.transform);
                                drinkMade.name = PlainOolongTea.name;
                                RescaleNewDrink(baseOfNewDrink);
                                drinkUIDScript.drinkUID += "OB";
                                drinkUIDScript.teaOverlaySelectionNumber = 1;
                                break;
                                case "BlackTeaBase(Clone)":
                                drinkMade  = Instantiate(BlackTeaOverlay, baseOfNewDrink.transform);
                                drinkMade.name = PlainBlackTea.name;
                                RescaleNewDrink(baseOfNewDrink);
                                drinkUIDScript.drinkUID += "BB";
                                drinkUIDScript.teaOverlaySelectionNumber = 2;
                                break;
                            }
                    }

                        //If a liquid base addition is present, & base was made, instantiate it. Else, make it the new base.
                        if(ingredientInCup.tag == "LiquidBaseAddition"){
                            drinkUIDScript.hasAdditionalFlavorOverlay = true;
                            switch(ingredientInCup.name){

                                case "WaterBaseAddition(Clone)":
                            
                                    drinkMade = Instantiate(WaterOverlay, baseOfNewDrink.transform);
                                    drinkMade.name = WaterOverlay.name;
                                    RescaleOverlay(drinkMade);

                                    //Check if there is/are ingredients blended in, or a Tea base, and adjust UID acordingly.
                                    if(drinkUIDScript.GetComponent<BobaCupUIDSettingsScript>().hasTeaOverlay == true){
                                        drinkUIDScript.GetComponent<BobaCupUIDSettingsScript>().drinkUID += "W";
                                    }
                                    else{
                                        drinkUIDScript.GetComponent<BobaCupUIDSettingsScript>().drinkUID += "--W";
                                    }
                                    
                                    drinkUIDScript.additionalOverlaySelectionNumber = 1;
                                
                                break;

                                case "MilkBaseAddition(Clone)":
                                
                                    drinkMade = Instantiate(MilkOverlay, baseOfNewDrink.transform);
                                    drinkMade.name = MilkOverlay.name;
                                    RescaleOverlay(drinkMade);

                                    //Check if there is/are ingredients blended in, or a Tea base, and adjust UID acordingly.
                                    if(drinkUIDScript.GetComponent<BobaCupUIDSettingsScript>().hasTeaOverlay == true){
                                        drinkUIDScript.GetComponent<BobaCupUIDSettingsScript>().drinkUID += "M";
                                    }
                                    else{
                                        drinkUIDScript.GetComponent<BobaCupUIDSettingsScript>().drinkUID += "--M";
                                    }
                                    drinkUIDScript.additionalOverlaySelectionNumber = 0;

                                break;
                            }
                        }

                        //FIXME: Add other toppings when they're made here. If a boba topping is present, instantiate it.
                        if(ingredientInCup.tag == "BobaToppings" && baseOfNewDrink.name != null){
                            drinkUIDScript.hasToppingsOverlay = true;
                            switch(ingredientInCup.name){
                                case "BobaToppingInCup(Clone)":
                                GameObject bobaOverlayGenerated = randomizeWhichBobaOverLayToMake();
                                drinkMade = Instantiate(bobaOverlayGenerated, baseOfNewDrink.transform);
                                RescaleOverlay(drinkMade);
                                //Check if the drink needs to add a space for the drink overlay.
                                if(drinkUIDScript.hasAdditionalFlavorOverlay == true){
                                    drinkUIDScript.drinkUID += "*B";
                                }
                                drinkUIDScript.additionalOverlaySelectionNumber = 0;
                                break;
                            }
                        }
                    }
                    //Erase the other drink in the mixer.
                    EraseDrinkPreMix(drinkInMixer);
                    //Reset the ingredient in the mixer to empty since it was blended into the drink.
                    mixerIngredientInput.GetComponent<MixerIngredientInput>().ConsumeIngredientInMixerInput();
                
                //Establish any faulty UID after * (These include toppings, temp, and sweetness UID adjustments.)
                MakeDrinkUIDFinalization(drinkUIDScript);
                }


                //If the cup in the mixer ONLY has ONE child count, specifically for drinks like water cups, milk cups, ect, and no ingredient has been placed in the top of the mixer.
                //BELOW BOC IS IF THE MIXER ***DOES NOT*** HAVE INGREDIENTS IN THE TOP MIXER, AND ONLY ONE LIQUID IN IT!
                else if(drinkInMixer.transform.childCount == 1 && !mixerIngredientInput.GetComponent<MixerIngredientInput>().HasAnIngredientBeenPlacedInMixerTop()){
                    //Set base object BobaCupUIDSettingsScript
                    BobaCupUIDSettingsScript drinkUIDScript;
                    switch(drinkInMixer.transform.GetChild(0).gameObject.tag){
                        
                        case "LiquidBase":   
                        
                            switch(drinkInMixer.transform.GetChild(0).name){
                                case "GreenTeaBase(Clone)":
                                EraseDrinkPreMix(drinkInMixer);
                                drinkMade  = Instantiate(PlainGreenTea, drinkInMixerClickableArea.transform);
                                drinkMade.name = PlainGreenTea.name;
                                drinkUIDScript = drinkMade.GetComponent<BobaCupUIDSettingsScript>();
                                RescaleNewDrink(drinkMade);
                                drinkUIDScript.drinkUID += "--GB";
                                drinkUIDScript.teaOverlaySelectionNumber = 0;
                                drinkUIDScript.hasTeaOverlay = true;
                                //Establish any faulty UID after * (These include toppings, temp, and sweetness UID adjustments.)
                                MakeDrinkUIDFinalization(drinkUIDScript.GetComponent<BobaCupUIDSettingsScript>());
                                break;
                                case "OolongBase(Clone)":
                                EraseDrinkPreMix(drinkInMixer);
                                drinkMade  = Instantiate(PlainOolongTea, drinkInMixerClickableArea.transform);
                                drinkMade.name = PlainOolongTea.name;
                                RescaleNewDrink(drinkMade);
                                drinkUIDScript = drinkMade.GetComponent<BobaCupUIDSettingsScript>();
                                drinkUIDScript.drinkUID += "--OB";
                                drinkUIDScript.teaOverlaySelectionNumber = 1;
                                drinkUIDScript.hasTeaOverlay = true;
                                //Establish any faulty UID after * (These include toppings, temp, and sweetness UID adjustments.)
                                MakeDrinkUIDFinalization(drinkUIDScript.GetComponent<BobaCupUIDSettingsScript>());
                                break;
                                case "BlackTeaBase(Clone)":
                                EraseDrinkPreMix(drinkInMixer);
                                drinkMade  = Instantiate(PlainBlackTea, drinkInMixerClickableArea.transform);
                                drinkMade.name = PlainBlackTea.name;
                                RescaleNewDrink(drinkMade);
                                drinkUIDScript = drinkMade.GetComponent<BobaCupUIDSettingsScript>();
                                drinkUIDScript.drinkUID += "--BB";
                                drinkUIDScript.teaOverlaySelectionNumber = 2;
                                drinkUIDScript.hasTeaOverlay = true;
                                //Establish any faulty UID after * (These include toppings, temp, and sweetness UID adjustments.)
                                MakeDrinkUIDFinalization(drinkUIDScript.GetComponent<BobaCupUIDSettingsScript>());
                                break;
                            }
                        break;

                        case "LiquidBaseAddition":
                            
                            switch(drinkInMixer.transform.GetChild(0).name){
                                case "WaterBaseAddition(Clone)":
                                EraseDrinkPreMix(drinkInMixer);
                                drinkMade  = Instantiate(WaterCup, drinkInMixerClickableArea.transform);
                                drinkMade.name = WaterCup.name;
                                drinkUIDScript = drinkMade.GetComponent<BobaCupUIDSettingsScript>();
                                RescaleNewDrink(drinkMade);
                                drinkUIDScript.drinkUID += "----W";
                                drinkUIDScript.additionalOverlaySelectionNumber = 1;
                                drinkUIDScript.hasAdditionalFlavorOverlay = true;
                                //Establish any faulty UID after * (These include toppings, temp, and sweetness UID adjustments.)
                                MakeDrinkUIDFinalization(drinkUIDScript.GetComponent<BobaCupUIDSettingsScript>());
                                break;
                                case "MilkBaseAddition(Clone)":
                                EraseDrinkPreMix(drinkInMixer);
                                drinkMade  = Instantiate(MilkCup, drinkInMixerClickableArea.transform);
                                drinkMade.name = MilkCup.name;
                                drinkUIDScript = drinkMade.GetComponent<BobaCupUIDSettingsScript>();
                                RescaleNewDrink(drinkMade);
                                drinkUIDScript.drinkUID += "----M";
                                drinkUIDScript.additionalOverlaySelectionNumber = 0;
                                drinkUIDScript.hasAdditionalFlavorOverlay = true;
                                //Establish any faulty UID after * (These include toppings, temp, and sweetness UID adjustments.)
                                MakeDrinkUIDFinalization(drinkUIDScript.GetComponent<BobaCupUIDSettingsScript>());
                                break;
                            }
                        break;
                    }

                //Else, if the cup in the mixer has MORE than 1 item in it, and no ingredient has been placed in the top of the mixer.
                //BELOW BOC IS IF THE MIXER ***DOES NOT*** HAVE INGREDIENTS IN THE TOP MIXER, BUT HAS MORE THAN ONE LIQUID IN IT!
                }else if(drinkInMixer.transform.childCount > 1 && !mixerIngredientInput.GetComponent<MixerIngredientInput>().HasAnIngredientBeenPlacedInMixerTop()){
                    
                    foreach(Transform ingredientInCup in drinkInMixer.transform){
                        
                        //If a liquid base is present, instantiate it first.
                        if(ingredientInCup.tag == "LiquidBase"){
                        switch(drinkInMixer.transform.GetChild(0).name){
                                case "GreenTeaBase(Clone)":
                                baseOfNewDrink  = Instantiate(PlainGreenTea, drinkInMixerClickableArea.transform);
                                baseOfNewDrink.name = PlainGreenTea.name;
                                RescaleNewDrink(baseOfNewDrink);
                                baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().drinkUID += "--GB";
                                baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().teaOverlaySelectionNumber = 0;
                                baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().hasTeaOverlay = true;
                                break;
                                case "OolongBase(Clone)":
                                baseOfNewDrink  = Instantiate(PlainOolongTea, drinkInMixerClickableArea.transform);
                                baseOfNewDrink.name = PlainOolongTea.name;
                                RescaleNewDrink(baseOfNewDrink);
                                baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().drinkUID += "--OB";
                                baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().teaOverlaySelectionNumber = 1;
                                baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().hasTeaOverlay = true;
                                break;
                                case "BlackTeaBase(Clone)":
                                baseOfNewDrink  = Instantiate(PlainBlackTea, drinkInMixerClickableArea.transform);
                                baseOfNewDrink.name = PlainBlackTea.name;
                                RescaleNewDrink(baseOfNewDrink);
                                baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().drinkUID += "--BB";
                                baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().teaOverlaySelectionNumber = 2;
                                baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().hasTeaOverlay = true;
                                break;
                            }
                    }

                        //If a liquid base addition is present, & base was made, instantiate it. Else, make it the new base. Else add "-" to UUID.
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
                                baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().hasAdditionalFlavorOverlay = true;
                                
                                //Check if there is/are ingredients blended in, or a Tea base, and adjust UID acordingly.
                                    if(baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().hasTeaOverlay == true){
                                        baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().drinkUID += "W";
                                    }
                                    else{
                                        baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().drinkUID += "--W";
                                    }

                                baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().additionalOverlaySelectionNumber = 1;
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
                                baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().hasAdditionalFlavorOverlay = true;

                                //Check if there is/are ingredients blended in, or a Tea base, and adjust UID acordingly.
                                    if(baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().hasTeaOverlay == true){
                                        baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().drinkUID += "M";
                                    }
                                    else{
                                        baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().drinkUID += "----M";
                                    }

                                baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().additionalOverlaySelectionNumber = 0;
                                break;
                            }
                        }

                        //FIXME: Add other toppings when they're made here. If a boba topping is present, instantiate it.
                        if(ingredientInCup.tag == "BobaToppings" && baseOfNewDrink.name != null){

                            baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().hasToppingsOverlay = true;

                            switch(ingredientInCup.name){
                                case "BobaToppingInCup(Clone)":
                                GameObject bobaOverlayGenerated = randomizeWhichBobaOverLayToMake();
                                drinkMade = Instantiate(bobaOverlayGenerated, baseOfNewDrink.transform);
                                RescaleOverlay(drinkMade);
                                //Check if the drink needs to add a space for the drink overlay.
                                if(baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().hasAdditionalFlavorOverlay == true){
                                    baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().drinkUID += "*B";
                                }else{
                                    baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().drinkUID += "-*B";
                                }
                                baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>().additionalOverlaySelectionNumber = 0;
                                break;
                            }
                        }
                    }
                    //Establish any faulty UID after * (These include toppings, temp, and sweetness UID adjustments.)
                    MakeDrinkUIDFinalization(baseOfNewDrink.GetComponent<BobaCupUIDSettingsScript>());
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
            spill.transform.localPosition = new Vector3(-3.5f,-39f,0f);
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

    //A method to quick check and establish the final drink UID adjustments (Toppings + Temp + Sweetness)
    public void MakeDrinkUIDFinalization(BobaCupUIDSettingsScript drinkUIDScriptToEdit){
        //If the drink has been made and there is indeed no topping ingredient in it, check if there's a flavor overlay, and adjust the UID accordingly.
        if(drinkUIDScriptToEdit.hasToppingsOverlay == false){
            if(drinkUIDScriptToEdit.hasAdditionalFlavorOverlay == false){
                            drinkUIDScriptToEdit.GetComponent<BobaCupUIDSettingsScript>().drinkUID += "-";  
                        }
            drinkUIDScriptToEdit.drinkUID += "*-";
        }
        //If the drink has a different temp than default, adjust or leve it "-".
        if(drinkTempMixSetting == 0){
            drinkUIDScriptToEdit.drinkUID += "-";
        }
        //If the drink has a different sweetness than default 100% (normal), adjust or leve it "-".
        if(drinkSweetness == 0){
            drinkUIDScriptToEdit.drinkUID += "-";
        }
    }

    //A method to rescale the new drink spawned.
    public void RescaleNewDrink(GameObject newDrinkSpawned){
        newDrinkSpawned.transform.localScale = new Vector3(.12f,.12f,.12f);
        newDrinkSpawned.transform.localPosition = new Vector3(0f,0f,0f);
    }

    //A method to rescale the overlays of the new drink.
    public void RescaleOverlay(GameObject newOverlaySpawned){
        newOverlaySpawned.transform.localScale = new Vector3(2f,2f,2f);
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
