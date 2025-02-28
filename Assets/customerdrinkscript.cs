using System.Collections.Generic;
using UnityEngine;

public class CustomerDrinkScript : MonoBehaviour
{
    // This is the main script for customer-specific interactions in the boba shop.

    // The GameManager script pulled from the game manager object.
    // Usefull to determine the player stats for what drinks they unlocked.
    GameManagerScript thisGamesOverallInstanceScript;

    // The List of favorite drinks this character wants.
    // Added via UID string.
    [SerializeField]
    [Tooltip("Input the UID's of this character's favorite drink here.")]
    public List<string> characterFavoriteBobaDrinks = new List<string>();

    // Chances the character gets their favorite drink(s) (usually a 50% chance if the drink is available, aka c/10 chance where c is input chance of 5 default).
    // Selection of favorite drink is c < 10, or if 1/10, if the number rand selects 1, the character selects their favorite drink.
    [SerializeField]
    [Tooltip("Input the chance(c) that this character would pick a favorite/new drink [c/10]")]
    public int chanceOfFavoriteDrink = 5; 

    // Chances the character gets multiple drink(s). Default is 3/10, but increases as popularity increases and changes on special occasions.
    // Selection of more drinks ordered is c < 10, or if 1/10, if the number rand selects 1, the character selects orders 2 drinks and does the chance again.
    // Thus, the chance of another drink NOT correlated to the previous chance and has a 2/20 chance for two drinks to be ordered, and 1/20 chance for 3 drinks.
    // HOWEVER, multiplier chance of 11 & 12 cause two and three drinks to be always ordered respectively.
    [SerializeField]
    [Tooltip("Input the chance(c) that this character would order multiple drinks [c/10]")]
    public int chanceOfMultpileDrinks = 3; 

    // This field allows the specific speed for boba shop characters.
    [SerializeField]
    [Tooltip("Input the speed(s) that this character has when walking to the specific shop parts in the shop.")]
    public float characterShopSpeed = 2.5f;

    //FIXME: Edit this when ading seasons / temperature drinks.
    // Check if the temperature of the day changes the selected temp drink if possible.
    bool dayHasANonStandardTempDiff = false;

    // The list of drinks UID's that this character has choosen.
    [Tooltip("Input the UID's of this character's favorite drink here.")]
    public List<string> drinksThisNPCOrdered = new List<string>();

    // The parts of the UID string for the drinks that the player can order are saved here.
    List<string> possibleFruitVeggieIngredient = new List<string>();
    List<string> possibleTeaBases = new List<string>();
    List<string> possibleFlavorOverlays = new List<string>();
    List<string> possibleToppings = new List<string>();
    List<string> possibleTemperature = new List<string>();
    List<string> possibleSweetness = new List<string>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisGamesOverallInstanceScript = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
    }

    //THIS IS THE MAIN METHOD USED BY NPC'S TO ORDER A DRINK.
    //This method is used to determine if the drinks possible in the list can be made by the player. Used by other code.
    public string CharacterOrdersDrink(){
        
        //Checks what drinks are available and adds them to the list.
        EstablishAvailableDrinkUIDList();

        //Make customer decide what drinks they want.
        if(Random.Range(1,11)<=chanceOfFavoriteDrink){
            //If the int selected was < "chanceOfFavoriteDrink", check if the character has a favorite drink list. 
            //If so, attempt to order such drink if available. Lest, check if there's another favorite drink, or generate a random one with the available 
            //possible drink lists.
            if(characterFavoriteBobaDrinks.Count != 0){
                //Check all favorite drinks of the character if it can be made.
                foreach(string drinkUID in characterFavoriteBobaDrinks){
                    if(CheckIfDrinkCanBeMade(drinkUID)){
                        return drinkUID;
                    }
                }
            }  
        }
        //Make a random drink with player's available ingredients if above checks fail.
        return GenerateRandomDrinkUIDWithIngredients();
    }

    //This method creates the propper lists of possible drink settings pulled from the user data.
    public void EstablishAvailableDrinkUIDList(){

        //Checks what fruits/veggie ingredients the player has availabe in the shop.
        //Add "nulls" to toppings and ingredients.
        possibleFruitVeggieIngredient.Add("--");
        possibleToppings.Add("*-");
        foreach(int ingredient in thisGamesOverallInstanceScript.ReturnPlayerStats().shopTraysItemListArray){

        //Finds out what item would be in this tray according to what number tray this script is on, and what items were selected in the round prior.
        switch(ingredient){

            //CassavaBall
            case 1:
            //Add boba to possible topping if there are any cassava balls left.
            if(thisGamesOverallInstanceScript.ReturnPlayerStats().casavaBalls>0){
                possibleToppings.Add("*B");
            }
            break;
            
            //PandanLeaf
            case 2:
            //Add PandanLeaf to possible ingredient if there are any left.
            if(thisGamesOverallInstanceScript.ReturnPlayerStats().pandanLeaves>0){
                possibleFruitVeggieIngredient.Add("PD");
            }    
            break;

            //BananaMini
            case 3:
            //Add BananaMini to possible ingredient if there are any left.
            if(thisGamesOverallInstanceScript.ReturnPlayerStats().bananas>0){
                possibleFruitVeggieIngredient.Add("BN");
            }
            break;    

            //StrawberryMini
            case 4:
            //Add StrawberryMini to possible ingredient if there are any left.
            if(thisGamesOverallInstanceScript.ReturnPlayerStats().strawberries>0){
                possibleFruitVeggieIngredient.Add("SB");
            }
            break;

            //MangoMini
            case 5:
            //Add MangoMini to possible ingredient if there are any left.
            if(thisGamesOverallInstanceScript.ReturnPlayerStats().mangos>0){
                possibleFruitVeggieIngredient.Add("MB");
            }
            break;

            //UbeMini
            case 6:
            //Add UbeMini to possible ingredient if there are any left.
            if(thisGamesOverallInstanceScript.ReturnPlayerStats().ube>0){
                possibleFruitVeggieIngredient.Add("UB");
            }
            break;
            }
        }

        //Checks what tea bases are available.
        //Add "null".
        possibleTeaBases.Add("--");
        if(thisGamesOverallInstanceScript.ReturnPlayerStats().greenTeaAmmount > 0){
            possibleTeaBases.Add("GB");
        }
        if(thisGamesOverallInstanceScript.ReturnPlayerStats().blackTeaUnlocked && thisGamesOverallInstanceScript.ReturnPlayerStats().blackTeaAmmount> 0){
            possibleTeaBases.Add("BB");
        }
        if(thisGamesOverallInstanceScript.ReturnPlayerStats().oolongTeaUnlocked && thisGamesOverallInstanceScript.ReturnPlayerStats().oolongTeaAmmount> 0){
            possibleTeaBases.Add("OB");
        }

        //Checks what flavor overlays are available.
        //Add "null".
        possibleFlavorOverlays.Add("-");
        if(thisGamesOverallInstanceScript.ReturnPlayerStats().milkUnlocked && thisGamesOverallInstanceScript.ReturnPlayerStats().milkAmmount> 0){
            possibleFlavorOverlays.Add("M");
        }
        //Add water.
        possibleFlavorOverlays.Add("W");

        //Checks what drink temperatures are available.
        //Add "null".
        possibleTemperature.Add("-");
        if(thisGamesOverallInstanceScript.ReturnPlayerStats().tempModifierUnlocked){
            //If the day has a different temp than normal, add a chance to switch drinks to weather type.
            if(dayHasANonStandardTempDiff){
            //Add "Hot".
            possibleTemperature.Add("H");
            //Add "Iced".
            possibleTemperature.Add("I");
            //Add "Slushy".
            possibleTemperature.Add("S");
            } 
        }
        //Checks what drink sweetnesses are available.
        //Add "null".
        possibleSweetness.Add("-");
        if(thisGamesOverallInstanceScript.ReturnPlayerStats().tempModifierUnlocked){
            //Add "Hot".
            possibleSweetness.Add("1");
            //Add "Iced".
            possibleSweetness.Add("2");
            //Add "Slushy".
            possibleSweetness.Add("3");
        }
    }

    //This method checks out and returns a possible drink via UID and above list of ingredients. 
    public string GenerateRandomDrinkUIDWithIngredients(){
        string randomDrinkDesired = "";

        //Add possibleFruitVeggieIngredient
        randomDrinkDesired += possibleFruitVeggieIngredient[Random.Range(0,possibleFruitVeggieIngredient.Count)];
        //Add possibleTeaBases
        randomDrinkDesired += possibleTeaBases[Random.Range(0,possibleTeaBases.Count)];
        //Add possibleFlavorOverlays
        randomDrinkDesired += possibleFlavorOverlays[Random.Range(0,possibleFlavorOverlays.Count)];
        //Add possibleToppings
        randomDrinkDesired += possibleToppings[Random.Range(0,possibleToppings.Count)];
        //Add possibleTemperature
        randomDrinkDesired += possibleTemperature[Random.Range(0,possibleTemperature.Count)];
        //Add possibleSweetness
        randomDrinkDesired += possibleSweetness[Random.Range(0,possibleSweetness.Count)];

        print("Random Drink ordered!: " + randomDrinkDesired);
        return randomDrinkDesired;
    }

    //This method checks if a drink UID can be made with the possible list of ingredients/player items.
    //NOTE: This may be a VERY expensive method so possibly add a timer of 2 seconds to allow the game to catch up and visually
    //show the player the NPC is "thinking" about ordering a drink.
    public bool CheckIfDrinkCanBeMade(string drinkUIDInput){
        
        //Checks string literal in parts if it is in the list of coresponding ingredients.
        //Check first two letters in UID that coresponds to available ingredients exists.
        if(possibleFruitVeggieIngredient.Contains(drinkUIDInput.Substring(0,2))){
            //Check next two letters in UID that coresponds to available possibleTeaBases exists.
            if(possibleTeaBases.Contains(drinkUIDInput.Substring(2,2))){
                //Check next letter in UID that coresponds to available possibleFlavorOverlays exists.
                if(possibleFlavorOverlays.Contains(drinkUIDInput.Substring(4,1))){
                    //Check next two letters in UID that coresponds to available possibleToppings exists.
                    if(possibleToppings.Contains(drinkUIDInput.Substring(5,2))){
                        //Check next letter in UID that coresponds to available possibleTemperature exists.
                        if(possibleTemperature.Contains(drinkUIDInput.Substring(7,1))){
                            //Check next letter in UID that coresponds to available possibleSweetness exists.
                            if(possibleSweetness.Contains(drinkUIDInput.Substring(8,1))){
                                print("The favorite drink this customer wants can be made!");
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    //This method is called by other scripts to have customers do custom order dialogues.
    public void DoCustomerDialogueLogic(){

    }

    //This method is called by other scripts to have customers do pickup orders and adds coins to the boba shop game manager.
    public void DoCustomerOrderPickupLogic(){
        
    }

    //This method actually generates the drinks the NPC orders via the methods above and saves them into the "drinksThisNPCOrdered" list variable.
    public void GenerateCustomerOrderUIDList(){
        drinksThisNPCOrdered.Add(CharacterOrdersDrink());

        //If character decides to order 2 drinks.
        if(chanceOfMultpileDrinks < Random.Range(1,11)){
            drinksThisNPCOrdered.Add(CharacterOrdersDrink());
        }

        //If character decides to order 3 drinks.
        if(chanceOfMultpileDrinks < Random.Range(1,11)){
            drinksThisNPCOrdered.Add(CharacterOrdersDrink());
        }

        //Only order two drinks
        if(chanceOfMultpileDrinks == 11){
            drinksThisNPCOrdered.Add(CharacterOrdersDrink());
        }

        //Only order three drinks
        if(chanceOfMultpileDrinks == 12){
            drinksThisNPCOrdered.Add(CharacterOrdersDrink());
            drinksThisNPCOrdered.Add(CharacterOrdersDrink());
        }
    }
}
