using System.Collections.Generic;
using UnityEngine;

public class BobaSellMattScript : MonoBehaviour
{
    //FIXME: This script was pulled from the boba cup holder script! Fix this for the sell script by
    //having the drinks ordered and shown via a stack! =^-^=
    //Gets game Object to check what the player is currently holding
    [SerializeField]
    GameObject itemInHandInventory;

    //List of the game objects in the sell tray.
    List<GameObject> sellableBobaDrinks = new List<GameObject>();

    //A bool to determine if the interaction was sucessfull and play the animation if not.
    bool interactedCorrectly;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void popSellableBobaDrinks(){
        if(sellableBobaDrinks.Count == 0){
            //Do nothing
            print("Error : No boba drink in the sell mat!");
            interactedCorrectly = false;
        }else{
            gameObject.transform.GetChild(0).transform.SetParent(itemInHandInventory.transform,false);
            itemInHandInventory.transform.GetChild(0).transform.position = new Vector3(0f,-.75f,0f);
            itemInHandInventory.transform.GetChild(0).transform.localScale = new Vector3(3f,3f,3f);
            sellableBobaDrinks.RemoveAt(0);
            print("Attempted to move drink");
            generateProperDrinkVisuals();
        }
    }

    public void pushSellableBobaDrinks(GameObject drinkToAdd){
        if(drinkToAdd.tag != "FinishedBobaDrink"){
            //Do nothing
            print("Error : No valid boba drink to add in the mat!");
            interactedCorrectly = false;
        }else{
            drinkToAdd.transform.SetParent(gameObject.transform,false);
            drinkToAdd.transform.transform.localScale = new Vector3(.7f,.7f,.7f);
            drinkToAdd.transform.transform.localPosition = new Vector3(1f,10f,0f);
            interactedCorrectly = true;
            sellableBobaDrinks.Add(drinkToAdd);
            print(sellableBobaDrinks.Count.ToString() + " : Drinks on mat.");
            generateProperDrinkVisuals();
        }
    }

    public void generateProperDrinkVisuals(){
        foreach(GameObject drink in sellableBobaDrinks){
            if(sellableBobaDrinks.Count == 1){
                sellableBobaDrinks[0].transform.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
                sellableBobaDrinks[0].transform.localPosition = new Vector3(0f,10f,0f);
            }
            else if(sellableBobaDrinks.Count == 2){
                sellableBobaDrinks[0].transform.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
                sellableBobaDrinks[0].transform.localPosition = new Vector3(10f,10f,0f);
                sellableBobaDrinks[1].transform.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
                sellableBobaDrinks[1].transform.localPosition = new Vector3(-10f,10f,0f); 
            }
            else if(sellableBobaDrinks.Count == 3){
                sellableBobaDrinks[0].transform.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
                sellableBobaDrinks[0].transform.localPosition = new Vector3(20f,10f,0f);
                sellableBobaDrinks[1].transform.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
                sellableBobaDrinks[1].transform.localPosition = new Vector3(0f,10f,0f);
                sellableBobaDrinks[2].transform.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
                sellableBobaDrinks[2].transform.localPosition = new Vector3(-20f,10f,0f);  
            }
        }
    }

    public void InteractWithDrinkMat(){

        interactedCorrectly = false;

        //Check if hand inventory has a boba drink, or finished boba drink, and the sellableBobaDrinks mat is empty or <3 full of other drinks, move it to the mat.
        if(itemInHandInventory.transform.childCount != 0 && itemInHandInventory.transform.GetChild(0).gameObject.tag == "FinishedBobaDrink" && sellableBobaDrinks.Count <3){
            pushSellableBobaDrinks(itemInHandInventory.transform.GetChild(0).gameObject);   
        }

        //Player interaction to swap boba drinks from sell mat to hand inventory.
        else if(itemInHandInventory.transform.childCount == 0 && sellableBobaDrinks.Count >=0){
            //Swap first mat drink into hand.
            popSellableBobaDrinks();
            //print("Swapping possible boba drinks!");
            interactedCorrectly = true;
        }

        
        //If hand is holding and mat is full of 3 drinks, perform a swap.
        if(itemInHandInventory.transform.childCount != 0 && sellableBobaDrinks.Count == 3){
            pushSellableBobaDrinks(itemInHandInventory.transform.GetChild(0).gameObject);
            popSellableBobaDrinks();
        }

        //Player interaction if they interacted wrongly with the cup holder.
        if(!interactedCorrectly){
            //Play wrong interaction hand animation.
            Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
            itemInHandInventoryAnimator.Play("IncorrectInteraction");
            print("You can't place other items into the cup holder!");
        }
    }
}
