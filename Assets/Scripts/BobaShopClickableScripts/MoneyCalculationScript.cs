using UnityEngine;

public class MoneyCalculationScript : MonoBehaviour
{

    // This script calculates how much money the player gets according to the drink uuid matching.
    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
