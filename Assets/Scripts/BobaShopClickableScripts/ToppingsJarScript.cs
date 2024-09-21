using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    //The object that shows the GUI of how many more toppings are left in the jar.
    GameObject inJarAmmountofToppingsObject;

    [SerializeField]
    [Tooltip("Input your dirty wipe cloth prefab in here.")]
    //Input the dirfty wipe cloth prefab in here.
    GameObject wipeClothDirty;

    //A public int to dictate maximum items in jars & how many items are in the pot currently. Another one for any transfers of ingredients.
    public int toppingsIngredientsInJarCurrentAmount, bobaToppingMaximumInJar, ammountOfNewToppingsToAddToTheJar;

    [SerializeField]
    [Tooltip("Drag the possible boba ladel's prefab here.")]
    GameObject bobaLadlePrefab;

    [Tooltip("Is this jar dirty? (Hit 0 toppings in jar)")]
    bool isTheJarDirty;

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

        inJarAmmountofToppingsObject = gameObject.transform.GetChild(4).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        //Instantly change the jar state to dirty.
        if(isTheJarDirty){
            gameObject.transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    public void InteractWithLadleOrPlayer(){
        //If the player is holding a ladle, reguardless of what state, do something.
            //Test if the user has a ladle in their hands & the jar is CLEAN.
            if(itemInHandInventory.gameObject.transform.childCount >= 0 && itemInHandInventory.transform.GetChild(0).name == bobaLadlePrefab.name && !isTheJarDirty){
            //Note, to get the children objects inside the ladle, this must be done.
            GameObject ladleInInventoryObject = itemInHandInventory.transform.GetChild(0).gameObject;
                if(TryToGetJarToppings(ladleInInventoryObject)){
                    Debug.Log("We got jar toppings!");
                }else{
                    TryToPourStuffBackInJar(ladleInInventoryObject);
                    Debug.Log("We Poured jar toppings!");
                }
            }

            //If the player has a CLEAN cloth, they can clean the dirty jar.
            if(isTheJarDirty && itemInHandInventory.gameObject.transform.childCount == 1 && itemInHandInventory.gameObject.transform.GetChild(0).gameObject.name == "WipeCloth(Clone)"){
                TryToClearJar();
            }
    }

    //Method to pull stuff from the topping jar using the ladle.
    public bool TryToGetJarToppings(GameObject ladleInInventoryObjectPassthrough){

        //If the jar has toppings & the user holds an empty-CLEAN ladle, and the jar isn't out of the selected ingredient, do actions.
        if(itemInHandInventory.transform.childCount > 0 && ladleInInventoryObjectPassthrough.transform.GetChild(0).gameObject.activeSelf && !ladleInInventoryObjectPassthrough.GetComponent<BobaLadelScript>().IsLadleDirty() && toppingsIngredientsInJarCurrentAmount != 0){
            Debug.Log("Jar has topings!");
            toppingsIngredientsInJarCurrentAmount -= 1;
            //If this action makes the jar hit 0 items, make the jar dirty.
            if(toppingsIngredientsInJarCurrentAmount == 0){
                isTheJarDirty = true;
            }
            itemInHandInventory.transform.GetChild(0).GetComponent<BobaLadelScript>().SetAmmountOfIngredientsInLadle(1);
            Debug.Log("Ladle has" + itemInHandInventory.transform.GetChild(0).GetComponent<BobaLadelScript>().GetAmmountOfIngredientsInLadle() +"topings!");

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
            return true;
        }else{
            return false;
        }
    }

    //Method to put stuff into the topping jar using the ladle, IF it has the correct ingedient.
    public void TryToPourStuffBackInJar(GameObject ladleInInventoryObjectPassthrough){
        ammountOfNewToppingsToAddToTheJar = 0;
        //Get hand animator for playing interaction feedback if it's wrong.
        Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
        
        if(itemInHandInventory.transform.GetChild(0).name == bobaLadlePrefab.name && ladleInInventoryObjectPassthrough.GetComponent<BobaLadelScript>().GetAmmountOfIngredientsInLadle() != 0 && toppingsIngredientsInJarCurrentAmount < bobaToppingMaximumInJar){
            Debug.Log("Jar can have toppings dumped into it.");
            //First check if there would be an overflow of toppings and how many are being transfered to the jar..
            if(toppingsIngredientsInJarCurrentAmount + bobaLadlePrefab.GetComponent<BobaLadelScript>().GetAmmountOfIngredientsInLadle() > bobaToppingMaximumInJar){
                toppingsIngredientsInJarCurrentAmount = bobaToppingMaximumInJar;
                //FIXME: If there's an overflow, have it actually cause a mess as well!
            }else{
                ammountOfNewToppingsToAddToTheJar += itemInHandInventory.transform.GetChild(0).gameObject.GetComponent<BobaLadelScript>().GetAmmountOfIngredientsInLadle();
                itemInHandInventory.transform.GetChild(0).gameObject.GetComponent<BobaLadelScript>().SetAmmountOfIngredientsInLadle(0);
                Debug.Log("Ammount of toppings to be transfered: " + itemInHandInventory.transform.GetChild(0).gameObject.GetComponent<BobaLadelScript>().GetAmmountOfIngredientsInLadle());
            }
            //If jar isn't empty, try to pour ladle item back into the jar.
            if(jarsCurrentStateOrTopping != null){
            switch(jarsCurrentStateOrTopping.name){
                case"ToppingBobaInJar":
                    if(ladleInInventoryObjectPassthrough.transform.GetChild(1).gameObject.activeSelf){
                        toppingsIngredientsInJarCurrentAmount += ammountOfNewToppingsToAddToTheJar;
                        ladleInInventoryObjectPassthrough.gameObject.GetComponent<BobaLadelScript>().SetAmmountOfIngredientsInLadle(0);
                        ladleInInventoryObjectPassthrough.transform.GetChild(1).gameObject.SetActive(false);
                        ladleInInventoryObjectPassthrough.transform.GetChild(0).gameObject.SetActive(true);
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
            }else{Debug.Log("Jar IS empty");
                //Check using if statements what ladle state is active.

                //Ladle in Boba state
                if(ladleInInventoryObjectPassthrough.transform.GetChild(1).gameObject.activeSelf){
                    jarsCurrentStateOrTopping = this.gameObject.transform.GetChild(0).gameObject;
                    jarsCurrentStateOrTopping.SetActive(true);
                    toppingsIngredientsInJarCurrentAmount += ammountOfNewToppingsToAddToTheJar;

                    //Reset the active ladle to use the empty ladle object.
                    ladleInInventoryObjectPassthrough.transform.GetChild(1).gameObject.SetActive(false);
                    ladleInInventoryObjectPassthrough.transform.GetChild(0).gameObject.SetActive(true);
                    bobaLadlePrefab.GetComponent<BobaLadelScript>().SetAmmountOfIngredientsInLadle(0);
                }
                //Ladle in Lychee state
                if(ladleInInventoryObjectPassthrough.transform.GetChild(2).gameObject.activeSelf){
                    jarsCurrentStateOrTopping = this.gameObject.transform.GetChild(1).gameObject;
                    jarsCurrentStateOrTopping.SetActive(true);
                    toppingsIngredientsInJarCurrentAmount += ammountOfNewToppingsToAddToTheJar;
                    //Reset the active ladle to use the empty ladle object.
                    ladleInInventoryObjectPassthrough.transform.GetChild(2).gameObject.SetActive(false);
                    ladleInInventoryObjectPassthrough.transform.GetChild(0).gameObject.SetActive(true);
                    bobaLadlePrefab.GetComponent<BobaLadelScript>().SetAmmountOfIngredientsInLadle(0);
                }
                //Ladle in RedBean state
                if(ladleInInventoryObjectPassthrough.transform.GetChild(3).gameObject.activeSelf){
                    jarsCurrentStateOrTopping = this.gameObject.transform.GetChild(2).gameObject;
                    jarsCurrentStateOrTopping.SetActive(true);
                    toppingsIngredientsInJarCurrentAmount += ammountOfNewToppingsToAddToTheJar;
                    //Reset the active ladle to use the empty ladle object.
                    ladleInInventoryObjectPassthrough.transform.GetChild(3).gameObject.SetActive(false);
                    ladleInInventoryObjectPassthrough.transform.GetChild(0).gameObject.SetActive(true);
                    bobaLadlePrefab.GetComponent<BobaLadelScript>().SetAmmountOfIngredientsInLadle(0);
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
        //Reset the ammount to add into the jar to 0 for next interaction.
        ammountOfNewToppingsToAddToTheJar = 0;
    }

    //Method to CLEAN a jar EMPTY.
    public void TryToClearJar(){
        if(isTheJarDirty){
            jarsCurrentStateOrTopping = null;
            toppingsIngredientsInJarCurrentAmount = 0;
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
            gameObject.transform.GetChild(3).gameObject.SetActive(false);
            isTheJarDirty = false;

            //Replace the cloth in yout hand with a dirty cloth.
            Destroy(itemInHandInventory.transform.GetChild(0).gameObject);
            Vector3 clothHeldPosition = new Vector3(0f,-.75f,0f);
            Instantiate(wipeClothDirty,clothHeldPosition,Quaternion.identity,itemInHandInventory.transform);
        }
    }

    //These scripts all are connected to teh ToppingsJarGUI scripts.
    //Hover scripts to show the value of the jars in text format as well.
    public void OnMouseEnter(){
        inJarAmmountofToppingsObject.SetActive(true);
    }

    public void OnMouseOver(){
        inJarAmmountofToppingsObject.GetComponent<TMP_Text>().text = toppingsIngredientsInJarCurrentAmount.ToString() + "/" + bobaToppingMaximumInJar.ToString();
    }

    public void OnMouseExit(){
        inJarAmmountofToppingsObject.SetActive(false);
    }
}
