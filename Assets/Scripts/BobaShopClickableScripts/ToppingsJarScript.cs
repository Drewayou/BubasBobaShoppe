using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingsJarScript : MonoBehaviour
{

    //The Game manager instance of this round (Will automatically be pulled in Start() method.
    private GameManagerScript currentGameManagerInstance;

    //Gets game Object to check what the player is currently holding
    [SerializeField]
    GameObject itemInHandInventory;

    //The Animator for the item in hand inventory object.
    Animator itemInHandInventoryAnimator;

    //What toppings are in the jar? Set this when you put something inside the jar, and re-set when you wash the jar.
    GameObject jarsCurrentStateOrTopping;

    //A public int to dictate maximum items in jars & how many items are in the pot currently. Another one for any transfers of ingredients.
    public int toppingsIngredientsInJarCurrentAmount, bobaToppingMaximumInJar, ammountOfNewToppingsToAddToTheJar;

    [SerializeField]
    [Tooltip("Drag the possible boba ladel's prefab here.")]
    GameObject bobaLadle;

    // Start is called before the first frame update
    void Start()
    {
        //Finds the Inventory object in this instance & it's animator.
        itemInHandInventory = GameObject.Find("ItemInHand");
        itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();

        //Finds the Game Manager in this instance.
        currentGameManagerInstance = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();

        //Pulls the game stats to program the jar's maximum capacity.
        bobaToppingMaximumInJar = currentGameManagerInstance.ReturnPlayerStats().maxCapacityOfToppingJars;

        jarsCurrentStateOrTopping = null;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InteractWithLadle(){
        //If the player is holding a ladle, reguardless of what state, do something.
        //FIXME: if(itemInHandInventory.name == bobaLadle.name + "(Clone)"){

            //Note, to get the children objects inside the ladle, this must be done.
            GameObject ladleInInventoryObject = itemInHandInventory.transform.GetChild(0).gameObject;
            TryToGetJarToppings(ladleInInventoryObject);
            TryToPourStuffBackInJar(ladleInInventoryObject);
        //}
    }

    //Method to pull stuff from the topping jar using the ladle.
    public void TryToGetJarToppings(GameObject ladleInInventoryObjectPassthrough){

        Debug.Log("Testing if Jar has topings.");
        //If the jar has toppings & the user holds an empty-CLEAN ladle, and the jar isn't out of the selected ingredient, do actions.
        if(jarsCurrentStateOrTopping != null && ladleInInventoryObjectPassthrough.transform.GetChild(0).gameObject.activeSelf && !ladleInInventoryObjectPassthrough.GetComponent<BobaLadelScript>().IsLadleDirty() && toppingsIngredientsInJarCurrentAmount != 0){
            Debug.Log("Jar has topings!");
            toppingsIngredientsInJarCurrentAmount -= 1;
            bobaLadle.GetComponent<BobaLadelScript>().SubtractLadleUses();
            bobaLadle.GetComponent<BobaLadelScript>().SetAmmountOfIngredientsInLadle(1);

            //Since we are sure the player is holding the ladle, turn off the empty version and enable the co-responging ingredient in it's hierarchy.
            switch(jarsCurrentStateOrTopping.name){
                case"ToppingBobaInJar":
                ladleInInventoryObjectPassthrough.transform.GetChild(0).gameObject.SetActive(false);
                ladleInInventoryObjectPassthrough.transform.GetChild(1).gameObject.SetActive(true);
                break;

                //FIXME: Add all the other ingredients down the line here below in this LOC. When you make the topping jars for them.

            default:
                //Play wrong interaction hand animation.
                itemInHandInventoryAnimator.Play("IncorrectInteraction");
            break;
            }
        }
    }

    //Method to put stuff into the topping jar using the ladle, IF it has the correct ingedient.
    public void TryToPourStuffBackInJar(GameObject ladleInInventoryObjectPassthrough){
        Debug.Log("Testing if Jar Isn't Empty.");
        //Get hand animator for playing interaction feedback if it's wrong.
        Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
        
        if(itemInHandInventory.transform.GetChild(0).name == bobaLadle.name + "(Clone)" && !ladleInInventoryObjectPassthrough.transform.GetChild(0).gameObject.activeSelf && toppingsIngredientsInJarCurrentAmount < bobaToppingMaximumInJar){
            Debug.Log("Jar has topings x2.");
            //First check if there would be an overflow of toppings and how many are being transfered to the jar..
            if(toppingsIngredientsInJarCurrentAmount + bobaLadle.GetComponent<BobaLadelScript>().GetAmmountOfIngredientsInLadle() > bobaToppingMaximumInJar){
                ammountOfNewToppingsToAddToTheJar = bobaToppingMaximumInJar;
                //FIXME: If there's an overflow, have it actually cause a mess as well!
            }else{
                ammountOfNewToppingsToAddToTheJar = bobaLadle.GetComponent<BobaLadelScript>().GetAmmountOfIngredientsInLadle();
            }
            //If jar isn't empty, try to pour ladle items into the jar.
            if(jarsCurrentStateOrTopping != null){Debug.Log("Jar isn't empty");
            switch(jarsCurrentStateOrTopping.name){
                case"ToppingBobaInJar":
                    if(ladleInInventoryObjectPassthrough.transform.GetChild(1).gameObject.activeSelf){
                        toppingsIngredientsInJarCurrentAmount += ammountOfNewToppingsToAddToTheJar;
                    }else{
                        //Play wrong interaction hand animation.
                        itemInHandInventoryAnimator.Play("IncorrectInteraction");
                    }
                break;
                
                case"ToppingLycheeInJar":
                    if(ladleInInventoryObjectPassthrough.transform.GetChild(2).gameObject.activeSelf){
                        toppingsIngredientsInJarCurrentAmount += 1;
                    }else{
                        //Play wrong interaction hand animation.
                        itemInHandInventoryAnimator.Play("IncorrectInteraction");
                    }
                break;

                case"ToppingRedBeanInJar":
                    if(ladleInInventoryObjectPassthrough.transform.GetChild(3).gameObject.activeSelf){
                        toppingsIngredientsInJarCurrentAmount += 1;
                    }else{
                        //Play wrong interaction hand animation.
                        itemInHandInventoryAnimator.Play("IncorrectInteraction");
                    }
                break;

                default:
                    //Play wrong interaction hand animation.
                    itemInHandInventoryAnimator.Play("IncorrectInteraction");
                break;
            }
            //If the jar is actually empty, but the ladle has items, then set the jar to a specific topping via the ladle.
            }else{Debug.Log("Jar isn't empty");
                //Check using if statements what ladle state is active.
                //Ladle in Boba state
                if(ladleInInventoryObjectPassthrough.transform.GetChild(1)){
                    jarsCurrentStateOrTopping = this.gameObject.transform.GetChild(0).gameObject;
                    jarsCurrentStateOrTopping.SetActive(true);
                    toppingsIngredientsInJarCurrentAmount += ammountOfNewToppingsToAddToTheJar;
                    bobaLadle.GetComponent<BobaLadelScript>().SetAmmountOfIngredientsInLadle(0);
                }
                //Ladle in Lychee state
                if(ladleInInventoryObjectPassthrough.transform.GetChild(2)){
                    jarsCurrentStateOrTopping = this.gameObject.transform.GetChild(1).gameObject;
                    jarsCurrentStateOrTopping.SetActive(true);
                    toppingsIngredientsInJarCurrentAmount += ammountOfNewToppingsToAddToTheJar;
                    bobaLadle.GetComponent<BobaLadelScript>().SetAmmountOfIngredientsInLadle(0);
                }
                //Ladle in RedBean state
                if(ladleInInventoryObjectPassthrough.transform.GetChild(3)){
                    jarsCurrentStateOrTopping = this.gameObject.transform.GetChild(2).gameObject;
                    jarsCurrentStateOrTopping.SetActive(true);
                    toppingsIngredientsInJarCurrentAmount += ammountOfNewToppingsToAddToTheJar;
                    bobaLadle.GetComponent<BobaLadelScript>().SetAmmountOfIngredientsInLadle(0);
                }
            }
        }
        //Decrease the change of ladle cleanliness by 1.
        itemInHandInventory.transform.GetChild(0).GetComponent<BobaLadelScript>().SubtractLadleUses();
        //Check if the ladle is now dirty. If it is, change it's visual state.
        if(itemInHandInventory.transform.GetChild(0).GetComponent<BobaLadelScript>().IsLadleDirty()){
            ladleInInventoryObjectPassthrough.transform.GetChild(0).gameObject.SetActive(false);
            ladleInInventoryObjectPassthrough.transform.GetChild(4).gameObject.SetActive(true);
        }
    }

    //Method to put stuff into an EMPTY topping jar using the ladle. Sets the ingredients of the jar.
}
