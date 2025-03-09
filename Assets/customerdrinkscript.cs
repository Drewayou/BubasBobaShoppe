using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;

public class CustomerDrinkScript : MonoBehaviour
{
    // This is the main script for customer-specific interactions in the boba shop.

    // The GameManager script pulled from the game manager object.
    // Usefull to determine the player stats for what drinks they unlocked.
    GameManagerScript thisGamesOverallInstanceScript;

    // This chattiness number depends on the customer if they want to pre-talk before ordering a drink or not.
    // The player's relationship score with the NPC also alters this score.
    // Default is 1, ergo one dialogue before ordering.
    // The TEXT of the dialogue is handled by the custom customer script.
    public int chattiness = 1;

    // The script that pulls the basic customer dialogue from Json files and/or hardcoded basic replies.
    CustomerDialogueScript customerDialogue;

    // The script that pulls the basic drink order text dialogue from the Standard generator.
    StandardDrinkNameDialogueGenerator drinkOrderStandardDialogue;

    // The Textbox UI gameobject for the character here to hide/show the bubble.
    [SerializeField]
    [Tooltip("Drag and drop the textbox TMP UI here.")]
    GameObject customerDialogueBoxObject;

    // Added via popup textbox UI TMP component for the character here to change text.
    [SerializeField]
    [Tooltip("Drag and drop the textbox TMP UI here.")]
    TMP_Text customerDialogueBox;

    // The List of favorite drinks this character wants.
    // Added via UID string.
    [SerializeField]
    [Tooltip("Input the UID's of this character's favorite drink here.")]
    public List<string> characterFavoriteBobaDrinks = new List<string>();

    // The List of drinks this character ordered.
    // Added via game generation.
    [Tooltip("The drinks the NPC ordered here.")]
    public List<string> drinksThisNPCOrdered = new List<string>();

    // The int variable that counts if the character ordered n number of drinks.
    public int customerVerballyOrderedNDrinks = 0;

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
        customerDialogue = this.gameObject.GetComponent<CustomerDialogueScript>();
        drinkOrderStandardDialogue = this.gameObject.GetComponent<StandardDrinkNameDialogueGenerator>();
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

        //This makes sure the random drink cannot be water + extras only (topping,sugars,temp adustments with a water cup).
        if(randomDrinkDesired.Substring(4,1)=="W"){
            if(!(randomDrinkDesired.Substring(5,4)=="*---")){
                print("Error, dirty water drink generated, re-generating drink!");
                randomDrinkDesired = GenerateRandomDrinkUIDWithIngredients();
            }
        }

        //This makes sure that if a fruit/veggie ingredient occurs, there HAS to be a liquid like water, milk, or other tea base with it.
        if(!(randomDrinkDesired.Substring(0,2)=="--")){
            if(randomDrinkDesired.Substring(4,1)=="-"){
                if(randomDrinkDesired.Substring(2,2)=="--"){
                    print("Error, ingredient choosen but no liquid, re-generating drink!");
                    randomDrinkDesired = GenerateRandomDrinkUIDWithIngredients();
                }
            }
        }

        //This makes sure the rest of the drink isn't just toppings/temp/sugar adjustments.
        if(randomDrinkDesired.Substring(0,5)=="-----"){
            print("Error, UID generated a non-drink, re-generating drink!");
            randomDrinkDesired = GenerateRandomDrinkUIDWithIngredients();
        }

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
    //If the character is done chatting and orders all the drinks, move them to the other queue.
    public void DoCustomerDialogueLogic(){
        customerDialogueBoxObject.SetActive(true);
        if(chattiness > 0){
            customerDialogueBox.text = customerDialogue.customerOrdersAtShopBobaShop(Random.Range(0,5));
            chattiness -= 1;   
            //Actually generate the drinks that the customer wants if they are done chatting.
            if(chattiness == 0){
                GenerateCustomerOrderUIDList();
            }

        //Checks if there are dubplicate UID orders and change them via order by name and ammount, and if the customer ordered any more drinks.
        }else if(customerVerballyOrderedNDrinks > 0){

            //FIXME: There should be a quicker and more efficient way to make this.
            //If all drinks are distinct.
            if(drinksThisNPCOrdered.Distinct().Count() == drinksThisNPCOrdered.Count){
                //If 3 distinct drinks are not yet ordered.
                if(customerVerballyOrderedNDrinks == 3){
                    customerDialogueBox.text = drinkOrderStandardDialogue.OrderDrinkByName(drinksThisNPCOrdered[customerVerballyOrderedNDrinks-1],1);
                    customerVerballyOrderedNDrinks -= 1;
                    return;
                }
                //If two distinct drinks are not yet ordered.
                if(customerVerballyOrderedNDrinks == 2){
                    string prefixDia = "";
                    int oneDrinkOrderedDia = Random.Range(0,3);
                    if(oneDrinkOrderedDia==0){
                        prefixDia = "Also: ";
                    }
                    if(oneDrinkOrderedDia==1){
                        prefixDia = "&: ";
                    }
                    if(oneDrinkOrderedDia==2){
                        prefixDia = "And... ";
                    }
                    customerDialogueBox.text = prefixDia + drinkOrderStandardDialogue.OrderDrinkByName(drinksThisNPCOrdered[customerVerballyOrderedNDrinks-1],1);
                    customerVerballyOrderedNDrinks -= 1;
                    return;
                }
                //If the last distinct drinks is not yet ordered.
                if(customerVerballyOrderedNDrinks == 1){
                    string prefixDia = "";
                    string suffixfixDia = "";
                    int oneDrinkOrderedDia = Random.Range(0,3);
                    if(oneDrinkOrderedDia==0){
                        prefixDia = "Also: ";
                        suffixfixDia = " To wrap it all up!";
                    }
                    if(oneDrinkOrderedDia==1){
                        prefixDia = "& Finally, ";
                    }
                    if(oneDrinkOrderedDia==2){
                        prefixDia = "Lastly, ";
                    }
                    customerDialogueBox.text = prefixDia + drinkOrderStandardDialogue.OrderDrinkByName(drinksThisNPCOrdered[customerVerballyOrderedNDrinks-1],1) + suffixfixDia;
                    customerVerballyOrderedNDrinks -= 1;
                    return;
                }
            }else{
                //If 3 drinks that were ordered have the same UID's, order them and finish the dialogue.
                if(customerVerballyOrderedNDrinks == 3 && drinksThisNPCOrdered.Distinct().Count() == 1){
                    customerDialogueBox.text = drinkOrderStandardDialogue.OrderDrinkByName(drinksThisNPCOrdered[customerVerballyOrderedNDrinks-1],3);
                    customerVerballyOrderedNDrinks = 0;
                }
                //If there are 2 similar drinks only, order them.
                if(customerVerballyOrderedNDrinks == 2 && drinksThisNPCOrdered.Distinct().Count() == 1){
                    customerDialogueBox.text = drinkOrderStandardDialogue.OrderDrinkByName(drinksThisNPCOrdered[customerVerballyOrderedNDrinks-1],2);
                    customerVerballyOrderedNDrinks = 0;
                }
                //If there are 3 total drinks to order left, 2 similar drinks but 1 other UID drink, order the single drink first.
                if(customerVerballyOrderedNDrinks == 3 && drinksThisNPCOrdered.Distinct().Count() == 2){
                    //Check all elements for the unique drink UID.
                    //temp var to save UID of the unique drink.
                    string uniqueDrink;
                    if(drinksThisNPCOrdered[0] == drinksThisNPCOrdered[1]){
                        uniqueDrink = drinksThisNPCOrdered[2];
                    }else if(drinksThisNPCOrdered[0] == drinksThisNPCOrdered[2]){
                        uniqueDrink = drinksThisNPCOrdered[1];
                    }else{
                        uniqueDrink = drinksThisNPCOrdered[0];
                    }
                        customerDialogueBox.text = drinkOrderStandardDialogue.OrderDrinkByName(uniqueDrink,1);
                        customerVerballyOrderedNDrinks = 2;
                        return;
                    }
                //If there are 2 total drinks to order left, 2 similar drinks but 1 other UID drink total, order the double drinks last.
                if(customerVerballyOrderedNDrinks == 2 && drinksThisNPCOrdered.Distinct().Count() == 2){
                    //Check all elements for the duplicate drink UID.
                    //temp var to save UID of the unique drink.
                    string dupeDrink;
                    if(drinksThisNPCOrdered[0] == drinksThisNPCOrdered[1]){
                        dupeDrink = drinksThisNPCOrdered[0];
                    }else if(drinksThisNPCOrdered[0] == drinksThisNPCOrdered[2]){
                        dupeDrink = drinksThisNPCOrdered[0];
                    }else{
                        dupeDrink = drinksThisNPCOrdered[1];
                    }
                        int doubleDrinkOrderedDia = Random.Range(0,3);
                        string prefixDia = "";
                        if(doubleDrinkOrderedDia==0){
                        prefixDia = "Also: ";
                        }
                        if(doubleDrinkOrderedDia==1){
                        prefixDia = "And finally, ";
                        }
                        if(doubleDrinkOrderedDia==2){
                        prefixDia = "Lastly, ";
                        }
                        customerDialogueBox.text = prefixDia + drinkOrderStandardDialogue.OrderDrinkByName(dupeDrink,2);
                        customerVerballyOrderedNDrinks = 0;
                        return;
                    }
                }
            }
            if(customerVerballyOrderedNDrinks == 0 && chattiness == 0){
                customerDialogueBoxObject.SetActive(false);
            }
        }

    //This method is called by other scripts to have customers do pickup orders and adds coins to the boba shop game manager.
    public void DoCustomerOrderPickupLogic(){
        
    }

    //This method actually generates the drinks the NPC orders via the methods above and saves them into the "drinksThisNPCOrdered" list variable.
    public void GenerateCustomerOrderUIDList(){
        if(chanceOfMultpileDrinks != 11 && chanceOfMultpileDrinks != 12){
            drinksThisNPCOrdered.Add(CharacterOrdersDrink());
            customerVerballyOrderedNDrinks = 1;
            //If character decides to order 2 drinks.
            if(chanceOfMultpileDrinks > Random.Range(1,11)){
                drinksThisNPCOrdered.Add(CharacterOrdersDrink());
                customerVerballyOrderedNDrinks = 2;
                //If character decides to order 3 drinks.
                    if(chanceOfMultpileDrinks > Random.Range(1,11)){
                    drinksThisNPCOrdered.Add(CharacterOrdersDrink());
                    customerVerballyOrderedNDrinks = 3;
                }
            }
        }
        

        //Only order two drinks
        if(chanceOfMultpileDrinks == 11){
            drinksThisNPCOrdered.Add(CharacterOrdersDrink());
            drinksThisNPCOrdered.Add(CharacterOrdersDrink());
            customerVerballyOrderedNDrinks = 2;
        }

        //Only order three drinks
        if(chanceOfMultpileDrinks == 12){
            drinksThisNPCOrdered.Add(CharacterOrdersDrink());
            drinksThisNPCOrdered.Add(CharacterOrdersDrink());
            drinksThisNPCOrdered.Add(CharacterOrdersDrink());
            customerVerballyOrderedNDrinks = 3;
        }
    }
}
