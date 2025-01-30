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
    List<GameObject> sellableBobaDrinks= new List<GameObject>();

    //A bool to determine if the interaction was sucessfull and play the animation if not.
    bool interactedCorrectly;

    //A bool to determine if the drink is a valid boba drink that one can sell.
    bool isSellableBobaDrink;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InteractWithCupHolder(){

        interactedCorrectly = false;

        //Check if hand inventory has a boba drink, or finished boba drink, and the holder is empty, move it to the cup holder.
        if(itemInHandInventory.transform.childCount != 0 && itemInHandInventory.transform.GetChild(0).gameObject.tag == "BobaDrink" | itemInHandInventory.transform.GetChild(0).gameObject.tag == "FinishedBobaDrink" && gameObject.transform.childCount == 0){
            itemInHandInventory.transform.GetChild(0).gameObject.transform.SetParent(gameObject.transform,false);
            gameObject.transform.GetChild(0).transform.localScale = new Vector3(.7f,.7f,.7f);
            gameObject.transform.GetChild(0).transform.localPosition = new Vector3(1f,10f,0f);
            interactedCorrectly = true;

            }else{
                //FIXME: Cause an overflow spill if there is more than 1 topping in the ladle (You loose the cup and all the toppings!).
            }
            interactedCorrectly = true;
        }

        //Player interaction to swap boba drinks from cup holder and hand inventory.
        if(itemInHandInventory.transform.childCount != 0 && gameObject.transform.childCount != 0 && itemInHandInventory.transform.GetChild(0).gameObject.tag == "BobaDrink" || itemInHandInventory.transform.GetChild(0).gameObject.tag == "FinishedBobaDrink"){
            
            //Swap cup holder drink.
            itemInHandInventory.transform.GetChild(0).gameObject.transform.SetParent(gameObject.transform,false);
            gameObject.transform.GetChild(0).gameObject.transform.SetParent(itemInHandInventory.transform,false);
            itemInHandInventory.transform.GetChild(0).transform.position = new Vector3(0f,-.75f,0f);
            itemInHandInventory.transform.GetChild(0).transform.localScale = new Vector3(3f,3f,3f);
            gameObject.transform.GetChild(0).transform.localScale = new Vector3(.7f,.7f,.7f);
            gameObject.transform.GetChild(0).transform.localPosition = new Vector3(1f,10f,0f);

            //print("Swapping possible boba drinks!");
            interactedCorrectly = true;
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
