using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugScript : MonoBehaviour
{

    //Drag and drop the drink liquid that will be dispensed into the empty cups.
    [SerializeField]
    [Tooltip("Drag and drop the drink liquid that will be dispensed into the empty cups.")]
    GameObject DrinkToBeDispensed;

    //Check the iteminhandinventory game object.
    [SerializeField]
    [Tooltip("Drag and drop the \"ItemInHand\" object prefab.")]
    GameObject itemInhandGameObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DispenseDrink(){
        if(itemInhandGameObject.transform.GetChild(0).name == "EmptyCup(Clone)"){
            Debug.Log("Clicked On a jug!");
            //Check if the empty cup already has a base/addon base depending on what this jug dispenses.
            bool EmptyCupAlreadyHasABaseOrAddon = false;
            foreach(Transform itemInCupTransform in itemInhandGameObject.transform.GetChild(0).transform) {
                if(itemInCupTransform.CompareTag(DrinkToBeDispensed.tag)) {
                    EmptyCupAlreadyHasABaseOrAddon = true;
                    Debug.Log("Drink already has a base/addon");
                    break;
                }
            }
                
            //If the empty cup doesn't have this dispenser's possible tag input (A base or addon base), dispense this jug's item into the empty cup!
            if(EmptyCupAlreadyHasABaseOrAddon == false){
                Debug.Log("Dispensing liquid into empty cup!!");
                Instantiate(DrinkToBeDispensed,itemInhandGameObject.transform.GetChild(0).gameObject.transform);
            }
            
            //Check if there are add-on bases (Milk/Water) in the boba cup, and if so, layer them on top for correct graphic generations.
            foreach(Transform itemInCupTransform in itemInhandGameObject.transform.GetChild(0).transform) {
                if(itemInCupTransform.CompareTag("LiquidBaseAddition")) {
                    itemInCupTransform.SetAsLastSibling();
                    break;
                }
            }

            //Check if there are toppings in the boba cup, and if so, layer them on top for correct graphic generations.
            foreach(Transform itemInCupTransform in itemInhandGameObject.transform.GetChild(0).transform) {
                if(itemInCupTransform.CompareTag("BobaToppings")) {
                    itemInCupTransform.SetAsLastSibling();
                    break;
                }
            }
        }
    }
}
