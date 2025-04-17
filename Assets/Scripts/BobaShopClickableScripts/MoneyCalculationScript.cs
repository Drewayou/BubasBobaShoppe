using UnityEngine;

public class MoneyCalculationScript : MonoBehaviour
{

    // Get the boba shop game manager script to pull data from.
    BobaShopRoundManagerScript thisRoundOverallInstanceScript;
    
    // This script calculates how much money the player gets according to the drink uuid matching.
    private float drinkCost = 0;

    void Start()
    {
        //Find and load the BobaShopRound data.
        thisRoundOverallInstanceScript = GameObject.Find("BobaShopRoundManager").GetComponent<BobaShopRoundManagerScript>();
    }

    // Check if UUID's of two drinks are identical.
    public void CheckIfDrinkUIDAreSimilar(string UIDOfDrinkInCupholder, string UIDOfWantedCustomerDrink){
        if(UIDOfDrinkInCupholder == UIDOfWantedCustomerDrink){
            //The drinks are the same.
            CalculateExactlySimilarDrinks();
        }
    }

    // This method triggers if the drinks are both EXACTLY the same UID.
    public void CalculateExactlySimilarDrinks(){

    }

    // This method calculates the beginning of the UID for FRUIT INGREDIENT! Pulls the data from the game manager.
    public void CalculateIngredientCost(string UUID){
        switch(UUID.Substring(0,2)){
            case "--":
            //No increase in drink cost.
            drinkCost += thisRoundOverallInstanceScript.baseDrinkMultiplier;
            break;
            case "PD":
            //Increase acording to pandan market cost.
            drinkCost += thisRoundOverallInstanceScript.PandanMultiplier;
            break;
            case "BN":
            //Increase acording to banana market cost.
            drinkCost += thisRoundOverallInstanceScript.BananaMultiplier;
            break;
            case "MB":
            //Increase acording to mango market cost.
            drinkCost += thisRoundOverallInstanceScript.MangoMultiplier;
            break;
            case "UB":
            //Increase acording to ube market cost.
            drinkCost += thisRoundOverallInstanceScript.UbeMultiplier;
            break;
            case "SB":
            //Increase acording to strawberry market cost.
            drinkCost += thisRoundOverallInstanceScript.StrawberryMultiplier;
            break;
        }
    }

    // This method calculates the beginning of the UID for TEA BASE! Pulls the data from the game manager.
    public void CalculateTeaBaseCost(string UUID){
        switch(UUID.Substring(2,2)){
            case "--":
            //No increase in drink cost.
            
            break;
        }
    }
}
