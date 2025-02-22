using System.Collections.Generic;
using UnityEngine;

public class CustomerDrinkScript : MonoBehaviour
{
    // The GameManager script pulled from the game manager object.
    // Usefull to determine the player stats for what drinks they unlocked.
    GameManagerScript thisGamesOverallInstanceScript;

    // The List of favorite drinks this character wants.
    List<GameObject> characterFavoriteBobaDrinks = new List<GameObject>();

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

    // Update is called once per frame
    void Update()
    {
        
    }

    //This method creates the propper lists of possible drink settings pulled from the user data.
    public void checkListDrinkUIDList(){

        //Checks what fruits/veggie ingredients the player has availabe in the shop.
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
        if(thisGamesOverallInstanceScript.ReturnPlayerStats().greenTeaAmmount > 0){

        }
    }

    //This method is used to determine if the drinks possible in the list can be made by the player.
    public void prepareDrinkSelection(){
        
        //Checks what drinks are available and adds them to the list.
        checkListDrinkUIDList();
    }
}
