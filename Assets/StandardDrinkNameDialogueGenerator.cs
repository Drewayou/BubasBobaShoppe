using System.Runtime.CompilerServices;
using UnityEngine;

public class StandardDrinkNameDialogueGenerator : MonoBehaviour
{
    // THIS SCRIPT IS ONLY FOR PRODUCING STANDARD CUSTOMER ORDER DIALOGUES!

    // Get the player data to make sure the player isn't informed of different drink types they cannot possibly have (Temp/sweetness).
    //The Game manager instance of this round (Will automatically be pulled in Start() method).
    private GameManagerScript currentGameManagerInstance;
    
    //The Round manager instance of this round (Will automatically be pulled in Start() method).
    private BobaShopRoundManagerScript currentRoundManagerInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Pull the player data to ensure the dialogue doesn't say something they shouldn't. (i.e temp/sweetness).
        currentGameManagerInstance = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
        //Pull the round instance to change states of the round.
        currentRoundManagerInstance = GameObject.Find("BobaShopRoundManager").GetComponent<BobaShopRoundManagerScript>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This method creates the dialogue depending on what UID is inputed.
    public string OrderDrinkByName(string drinkUID,int numberOfThisDrinkOrder){

        string basicOrderScript = "";

        //Special string to change numbers of drink ordered.
        string numOfDrinksOrderedPrefix = "";

        //Special string to change adjectives of drink.
        string adjectivePrefix = "";

        string teaIngredientBase = "";
        string teaBase = "";
        string drinkOverlayBase = "";
        string topping = "";
        string temperature = "";
        string sweetness = "";

        //Ellaborates number of similar drinks ordered if two or more of the same drinks are ordered.
        switch(numberOfThisDrinkOrder){
            case 1:
                int oneDrinkOrderedDia = Random.Range(0,4);
                    if(oneDrinkOrderedDia==0){
                        numOfDrinksOrderedPrefix = "An order of";
                    }
                    if(oneDrinkOrderedDia==1){
                        numOfDrinksOrderedPrefix = "Can I get one";
                    }
                    if(oneDrinkOrderedDia==2){
                        numOfDrinksOrderedPrefix = "One";
                    }
                    if(oneDrinkOrderedDia==3){
                        numOfDrinksOrderedPrefix = "A";
                    }
            break;
            case 2:
                int twoDrinkOrderedDia = Random.Range(0,3);
                    if(twoDrinkOrderedDia==0){
                        numOfDrinksOrderedPrefix = "Two orders of";
                    }
                    if(twoDrinkOrderedDia==1){
                        numOfDrinksOrderedPrefix = "Can I get two";
                    }
                    if(twoDrinkOrderedDia==2){
                        numOfDrinksOrderedPrefix = "Two";
                    }
            break;
            case 3:
                int threeDrinkOrderedDia = Random.Range(0,3);
                    if(threeDrinkOrderedDia==0){
                        numOfDrinksOrderedPrefix = "Three orders of";
                    }
                    if(threeDrinkOrderedDia==1){
                        numOfDrinksOrderedPrefix = "Can I get three";
                    }
                    if(threeDrinkOrderedDia==2){
                        numOfDrinksOrderedPrefix = "Three";
                    }
            break;

            default:
                numOfDrinksOrderedPrefix = "An order of";
            break;
        }basicOrderScript += numOfDrinksOrderedPrefix;

        //FIXME: Future implementation of player-named drinks.
        //Check if the drink UID has a player-dedicated name.
        //if(!drinkUID.HasCustomName()){

        //FIXME: Add more when more flavors are made. TeaBase overlay
        switch(drinkUID.Substring(0,2)){
            case "--":
            break;

            case "PD":
                    teaIngredientBase = " Pandan";
            break;

            case "SB":
                    teaIngredientBase = " Strawberry";
            break;

            case "BN":
                    teaIngredientBase = " Banana";
                
            break;

            case "UB":
                    teaIngredientBase = " Ube";
                
            break;

            case "MB":
                    teaIngredientBase = " Mango";
                
            break;

            default:
            break;
        }basicOrderScript+= teaIngredientBase;

        //TeaBase overlay
        switch(drinkUID.Substring(2,2)){
            case "--":
            break;

            case "GB":
                if(teaIngredientBase == ""){
                    teaBase = " Green";
                }else{
                    if(Random.Range(0, 2) == 0){
                        teaBase = ", Green";
                    }else{
                        teaBase = "-Green";
                    }
                }
            break;

            case "OB":
                if(teaIngredientBase == ""){
                    teaBase = " Oolong";
                }else{
                    if(Random.Range(0, 2) == 0){
                        teaBase = ", Oolong";
                    }else{
                        teaBase = "-Oolong";
                    }
                }
            break;

            case "BB":
                if(teaIngredientBase == ""){
                    teaBase = " Black";
                }else{
                    if(Random.Range(0, 2) == 0){
                        teaBase = ", Black";
                    }else{
                        teaBase = "-Black";
                    }
                }
            break;

            default:
            break;
        }basicOrderScript+= teaBase;

        //Drink overlay
        switch(drinkUID.Substring(4,1)){
            case "-":
            //If it's just a tea with no ingredient, call it "plain [tea type] tea".
                if(drinkUID.Substring(0,2)=="--"){
                        adjectivePrefix = " plain";
                        basicOrderScript = numOfDrinksOrderedPrefix + adjectivePrefix + teaIngredientBase + teaBase + " tea";
                //Else, it's a hybrid drink like "[Ingredient type] [tea type] tea".
                }else{
                    drinkOverlayBase = " tea";
                }

            break;

            case "M":
                if(drinkUID.Substring(2,2) == "--" && drinkUID.Substring(0,2) == "--"){
                    if(numberOfThisDrinkOrder >1){
                        drinkOverlayBase = " Milk cups";
                    }else{
                        drinkOverlayBase = " Milk Cup";
                    }
                }else{
                    //Check if this is an ingredient blended without a tea, but with milk.
                    if(drinkUID.Substring(2,2) == "--"){
                        int blendedMilkNum = Random.Range(0, 3);
                        if(blendedMilkNum == 0){
                            drinkOverlayBase = " mixed with milk";
                        }
                        if(blendedMilkNum == 1){
                            drinkOverlayBase = " blended with milk";
                        }
                        if(blendedMilkNum == 2){
                            adjectivePrefix = " creamy";
                            basicOrderScript = numOfDrinksOrderedPrefix + adjectivePrefix + teaIngredientBase + teaBase + " drink";
                        }
                    }else{
                        int addMilkNum = Random.Range(0, 5);
                        if(addMilkNum == 0){
                            drinkOverlayBase = " Milk tea";
                        }
                        if(addMilkNum == 1){
                            drinkOverlayBase = " tea, with cream";
                        }
                        if(addMilkNum == 2){
                            drinkOverlayBase = " tea, add milk";
                        }
                        if(addMilkNum == 3){
                            drinkOverlayBase = " Milk tea";
                        }
                        if(addMilkNum == 4){
                            adjectivePrefix = " creamy";
                            basicOrderScript = numOfDrinksOrderedPrefix + adjectivePrefix + teaIngredientBase + teaBase + " tea";
                        }
                    }
                }
            break;

            case "W":
                if(drinkUID.Substring(2,2) == "--" && drinkUID.Substring(0,2) == "--"){
                    if(numberOfThisDrinkOrder >1){
                        return numOfDrinksOrderedPrefix + " Water cups";
                    }else{
                        int waterCupDia = Random.Range(0, 2);
                        if(waterCupDia == 0){
                            return "May I get a water cup?";
                        }
                        if(waterCupDia == 1){
                            return "Just a water cup.";
                        }
                        return "Water cups";
                    }
                }else{
                    //Check if this is an ingredient blended without a tea, but with water.
                    if(drinkUID.Substring(2,2) == "--"){
                        int blendWaterNum = Random.Range(0, 3);
                        if(blendWaterNum == 0){
                            adjectivePrefix = " watery";
                            basicOrderScript = numOfDrinksOrderedPrefix + adjectivePrefix + teaIngredientBase + " puree";
                        }
                        if(blendWaterNum == 1){
                            drinkOverlayBase = " puree watered down";
                        }
                        if(blendWaterNum == 2){
                            adjectivePrefix = " diluted";
                            basicOrderScript = numOfDrinksOrderedPrefix + adjectivePrefix + teaIngredientBase +" drink";
                        }
                    }else{
                        if(Random.Range(0, 2) == 0){
                            drinkOverlayBase = " tea, extra water";
                        }else{
                            drinkOverlayBase = " tea, dilute with water";
                        }
                    }
                }
            break;

            default:
            break;
        }basicOrderScript+= drinkOverlayBase;

        //FIXME: Add other toppings in the future
        switch(drinkUID.Substring(5,2)){
            case "*-":
                if(Random.Range(0, 3) == 0){
                    topping = ", no toppings";
                }else{
                    topping = "";
                }
            break;

            case "*B":
            int bobaDiaSel = Random.Range(0, 3);
                if(bobaDiaSel == 0){
                    topping = ", with boba toppings";
                }else if(bobaDiaSel == 1){
                    topping = ", with boba";
                }else{
                    topping = ", add boba";
                }
            break;
            default:
            break;
        }basicOrderScript+= topping;
    
        // Temperature
        if(currentGameManagerInstance.ReturnPlayerStats().tempModifierUnlocked){
            switch(drinkUID.Substring(7,1)){
                case "-":
                    if(Random.Range(0, 2) == 0){
                        temperature = ", room-temp";
                    }else{
                        temperature = "";
                    }
                break;
                case "H":
                    temperature = ", hot";
                break;
                case "I":
                    temperature = ", iced";
                break;
                case "S":
                    if(Random.Range(0, 2) == 0){
                        temperature = ", slushied";
                    }else{
                        temperature = ", frozen";
                    }
                break;
                default:
                break;
            }
        }basicOrderScript+= temperature;

        // Sugar
        if(currentGameManagerInstance.ReturnPlayerStats().sugarModifierUnlocked){
            switch(drinkUID.Substring(8,1)){
                case "-":
                    int sweetDiaChosen = Random.Range(0, 3);
                    if(sweetDiaChosen == 0){
                        sweetness = ", 100% sweetness";
                    }
                    if(sweetDiaChosen == 1){
                        sweetness = ", normal sweetness";
                    }
                    if(sweetDiaChosen == 2){
                        sweetness = "";
                    }
                break;
                case "1":
                    if(Random.Range(0, 2) == 0){
                        sweetness = ", 125% sweetness";
                    }else{
                        sweetness = ", slightly sweeter";
                    }
                break;
                case "2":
                    if(Random.Range(0, 2) == 0){
                        sweetness = ", 150% sweetness";
                    }else{
                        sweetness = ", very Sweet";
                    }
                break;
                case "3":
                    if(Random.Range(0, 2) == 0){
                        sweetness = ", 200% sweetness";
                    }else{
                        sweetness = ", extremely sweet";
                    }
                break;
                default:
                break;
            }
        }basicOrderScript+= sweetness;

        basicOrderScript += ".";
        return basicOrderScript;
       // This bracket is for the above implementation of custom drink names.
       //}else{
       //}return pullDrinkNameFromData();
    }

    //FIXME:
    //This method is a future implementation to add custom names to drinks.
    public string pullDrinkNameFromData(){
        //Add a way to pull customer player drinks via JSON hashmaps?
        return "";
    }

    
}
