using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BobaDrinkHolderScript : MonoBehaviour
{

    //Gets game Object to check what the player is currently holding
    [SerializeField]
    GameObject itemInHandInventory;

    //Boba toppings in cup prefab.
    [SerializeField]
    [Tooltip("Drag the \"Boba topping in cup prefab\" here!")]
    GameObject bobaToppingsInCupPrefab;

    //Empty cup prefab.
    [SerializeField]
    [Tooltip("Drag the \"Empty cup prefab\" here!")]
    GameObject emptyCup;

    //Empty cup prefab.
    [SerializeField]
    [Tooltip("Drag the \"bobaLadle prefab\" here!")]
    GameObject bobaladle;

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

    public void InteractWithCupHolder(){

        interactedCorrectly = false;

        //Check if hand inventory has a boba drink, and the holder is empty, move it to the cup holder.
        if(itemInHandInventory.transform.childCount != 0 && itemInHandInventory.transform.GetChild(0).gameObject.tag == "BobaDrink" && gameObject.transform.childCount == 0){
            itemInHandInventory.transform.GetChild(0).gameObject.transform.SetParent(gameObject.transform,false);
            gameObject.transform.GetChild(0).transform.localScale = new Vector3(.7f,.7f,.7f);
            gameObject.transform.GetChild(0).transform.localPosition = new Vector3(1f,10f,0f);
            interactedCorrectly = true;

        //Check if hand inventory is empty, and the holder has a boba drink, then grab it (move to the hand inventory).
        }else if(itemInHandInventory.transform.childCount == 0 && gameObject.transform.childCount != 0){
            gameObject.transform.GetChild(0).gameObject.transform.SetParent(itemInHandInventory.transform,false);
            itemInHandInventory.transform.GetChild(0).transform.position = new Vector3(0f,-.75f,0f);
            itemInHandInventory.transform.GetChild(0).transform.localScale = new Vector3(3f,3f,3f);
            interactedCorrectly = true;
        }

        //Check if hand inventory has a ladle with toppings, and if the cup holder has an EMPTY cup, then move the correct toppings to the cup inside the cup holder.
        if(itemInHandInventory.transform.childCount != 0 && itemInHandInventory.transform.GetChild(0).gameObject.name == bobaladle.name && gameObject.transform.childCount != 0 && gameObject.transform.GetChild(0).name == emptyCup.name + "(Clone)" && gameObject.transform.GetChild(0).transform.childCount == 0){
            //Check what topping the ladle holds to change it's settings.

            //FIXME: Check if ladle has only 1 TOPPING (If more, cause a spill).
            if(itemInHandInventory.transform.GetChild(0).GetComponent<BobaLadelScript>().GetAmmountOfIngredientsInLadle() == 1){
            //Check if the BOBA topping is in the ladle.
                if(itemInHandInventory.transform.GetChild(0).transform.GetChild(1).gameObject.activeSelf){
                    //Put boba toppings prefab into the cup.
                    Instantiate(bobaToppingsInCupPrefab, gameObject.transform.GetChild(0).gameObject.transform.position, Quaternion.identity, gameObject.transform.GetChild(0).gameObject.transform);
                    //Reset the ladle active topping to empty, and reduce the items in the ladle to 0.
                    itemInHandInventory.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                    itemInHandInventory.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                    itemInHandInventory.transform.GetChild(0).GetComponent<BobaLadelScript>().SetAmmountOfIngredientsInLadle(0);
                }
                //Check if the LYCHEE toppings is in the ladle.
                if(itemInHandInventory.transform.GetChild(0).transform.GetChild(2).gameObject.activeSelf){
                    //Put boba toppings prefab into the cup.
                        //FIXME: Instantiate(lycheeToppingsPrefab, new Vector3(0,0,0), Quaternion.identity, emptyCup.transform);
                    //Reset the ladle active topping to empty, and reduce the items in the ladle to 0.
                    itemInHandInventory.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
                    itemInHandInventory.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                    itemInHandInventory.transform.GetChild(0).GetComponent<BobaLadelScript>().SetAmmountOfIngredientsInLadle(0);
                }
                //Check if the REDBEAN toppings is in the ladle.
                if(itemInHandInventory.transform.GetChild(0).transform.GetChild(3).gameObject.activeSelf){
                    //Put boba toppings prefab into the cup.
                        //FIXME: Instantiate(redBeanToppingsPrefab, new Vector3(0,0,0), Quaternion.identity, emptyCup.transform);
                    //Reset the ladle active topping to empty, and reduce the items in the ladle to 0.
                    itemInHandInventory.transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(false);
                    itemInHandInventory.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                    itemInHandInventory.transform.GetChild(0).GetComponent<BobaLadelScript>().SetAmmountOfIngredientsInLadle(0);
                }
            }else{
                //FIXME: Cause an overflow spill if there is more than 1 topping in the ladle (You loose the cup and all the toppings!).
            }
            interactedCorrectly = true;
        }

        //Player interaction to swap boba drinks from mixer and hand inventory.
        if(itemInHandInventory.transform.childCount != 0 && gameObject.transform.childCount != 0 && itemInHandInventory.transform.GetChild(0).gameObject.tag == "BobaDrink"){
            
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
