using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BobaDrinkHolderScript : MonoBehaviour
{

    //Gets game Object to check what the player is currently holding
    [SerializeField]
    GameObject itemInHandInventory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InteractWithCupHolder(){

        //Check if hand inventory has a boba drink, and the holder is empty, move it to the cup holder.
        if(itemInHandInventory.transform.childCount != 0 && itemInHandInventory.transform.GetChild(0).gameObject.tag == "BobaDrink" && gameObject.transform.childCount == 0){
            itemInHandInventory.transform.GetChild(0).gameObject.transform.SetParent(gameObject.transform,false);
            gameObject.transform.GetChild(0).transform.localScale = new Vector3(.7f,.7f,.7f);
            gameObject.transform.GetChild(0).transform.localPosition = new Vector3(1f,10f,0f);

        //Check if hand inventory is empty, and the holder has a boba drink, then grab it (move to the hand inventory).
        }else if(itemInHandInventory.transform.childCount == 0 && gameObject.transform.childCount != 0){
            gameObject.transform.GetChild(0).gameObject.transform.SetParent(itemInHandInventory.transform,false);
            itemInHandInventory.transform.GetChild(0).transform.position = new Vector3(0f,-.75f,0f);
            itemInHandInventory.transform.GetChild(0).transform.localScale = new Vector3(3f,3f,3f);
        }
        
        //Player interaction if they choose to place something other than a boba drink into the cup holder.
        if(itemInHandInventory.transform.childCount != 0 && itemInHandInventory.transform.GetChild(0).gameObject.tag != "BobaDrink"){
            //Play wrong interaction hand animation.
            Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
            itemInHandInventoryAnimator.Play("IncorrectInteraction");
            print("You can't place other items into the cup holder!");
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

            print("Swapping possible boba drinks!");
        }
    }
}
