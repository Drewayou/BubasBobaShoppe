using UnityEngine;

public class CustomerDialogueScript : MonoBehaviour, Inter_BobaShopNPC
{
    //Script to call upon the standard dialogue for drinks.
    StandardDrinkNameDialogueGenerator basicDialogueScript;

    //Script to call upon the NPC to order drinks.
    CustomerDrinkScript customerOrdersDrinkScript;

    public string customerNegatesBobaShop(int negationDialogueIndexBShop)
    {
        throw new System.NotImplementedException();
    }

    public string customerOrdersAtShopBobaShop(int orderNormalIndexBShop)
    {
        switch(orderNormalIndexBShop){
            case 0:
            return "A good day for some drinks!";
            case 1:
            return "Hello there!";
            case 2:
            return "Hello~";
            case 3:
            return "Hiya!";
            case 4:
            return "Hi.";
            default: 
            return "Hello!";
        }
    }

    public string customerOrdersSpecialBobaShop(int orderSpecialIndexBShop)
    {
        switch(orderSpecialIndexBShop){
            case 0:
            return "Can I get my usual?";
            default: 
            return "Mango Milk tea made my way please!";
        }
    }

    public string customerThanksBobaShop(int thanksDialogueIndexBShop)
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        basicDialogueScript = this.gameObject.GetComponent<StandardDrinkNameDialogueGenerator>();
        customerOrdersDrinkScript = this.gameObject.GetComponent<CustomerDrinkScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This method is a TEST method to make drinks, and print out the drink orders that this character has into the console.
    public void DrinkCreationTEST(){
        customerOrdersDrinkScript.GenerateCustomerOrderUIDList();
        foreach(string drinkUID in customerOrdersDrinkScript.drinksThisNPCOrdered){
            print(basicDialogueScript.OrderDrinkByName(drinkUID,1));
        }
    }
}
