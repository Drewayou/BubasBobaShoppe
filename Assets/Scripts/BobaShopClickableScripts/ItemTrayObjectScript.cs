using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrayObjectScript : MonoBehaviour
{
    //The Game manager instance of this round (Will automatically be pulled in Start() method.
    private GameManagerScript currentGameManagerInstance;

    //Int identification of what item tray this is.
    [SerializeField]
    [Tooltip("What Item Tray is this? Give it a number starting from the 0th index!")]
    public int itemTrayNumber;

    //Int identification of how MANY items the player has left in their inventory.
    public int itemSelectedLeftInPlayerInventory;

    //The Pre-fabs for all the possible Ingredient Items.
    [SerializeField]
    [Tooltip("Add all the pre-fabs for the possible ingredients below!")]
    public GameObject CassavaBall,PandanLeaf,BananaMini,StrawberryMini,MangoMini,UbeMini;

    //The selected Pre-fab to spawn.
    public GameObject selectedIngredientItemPrefab;

    //Selected item index (saves what index item is placed inside this tray).
    int selectedItemIndexThatWillBeInThisTray;

    //The psudo game object inventory object for the boba shop round.
    [SerializeField]
    [Tooltip("Drag the player inventory object \"ItemInHand\" here!")]
    public GameObject itemInHandInventory;

    // Start is called before the first frame update.
    void Start()
    {
        //Finds the Game Manager in this instance.
        currentGameManagerInstance = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();

        //Finds the Inventory object in this instance.
        itemInHandInventory = GameObject.Find("ItemInHand");

        //Sets the item that were selected in the round prior to correlate with whatever particular tray this item is on.
        selectedItemIndexThatWillBeInThisTray = currentGameManagerInstance.ReturnPlayerStats().shopTraysItemListArray[itemTrayNumber];

        //Finds out what item would be in this tray according to what number tray this script is on, and what items were selected in the round prior.
        switch(selectedItemIndexThatWillBeInThisTray){
            case 1:
            selectedIngredientItemPrefab = CassavaBall;
            itemSelectedLeftInPlayerInventory = currentGameManagerInstance.ReturnPlayerStats().casavaBalls;
            break;
            
            case 2:
            selectedIngredientItemPrefab = PandanLeaf;
            itemSelectedLeftInPlayerInventory = currentGameManagerInstance.ReturnPlayerStats().pandanLeaves;            
            break;

            case 3:
            selectedIngredientItemPrefab = BananaMini;
            itemSelectedLeftInPlayerInventory = currentGameManagerInstance.ReturnPlayerStats().bananas;
            break;    

            case 4:
            selectedIngredientItemPrefab = StrawberryMini;
            itemSelectedLeftInPlayerInventory = currentGameManagerInstance.ReturnPlayerStats().strawberries;
            break;

            case 5:
            selectedIngredientItemPrefab = MangoMini;
            itemSelectedLeftInPlayerInventory = currentGameManagerInstance.ReturnPlayerStats().mangos;
            break;

            case 6:
            selectedIngredientItemPrefab = UbeMini;
            itemSelectedLeftInPlayerInventory = currentGameManagerInstance.ReturnPlayerStats().ube;
            break;
        }

        //Spawns the items in the tray iteratively.
        AttemptToSpawnIngredientsInTrayAtTheStart();
    }

    // Update is called once per frame.
    void Update()
    {
        
    }

    //The method will spawn up to 10 items of the selected ingredient in the tray if there are that many items in the player's save file.
    public void AttemptToSpawnIngredientsInTrayAtTheStart(){

        //If the player has at least 10 of the items in their overall game inventory, spawn 10. Else, spawn as many as it can.
        if(itemSelectedLeftInPlayerInventory >= 10){
            for(int i = 0; i < 10; i++){
                GameObject newIngredientSpawned = Instantiate(selectedIngredientItemPrefab,gameObject.transform);
                normalizeTheLookOfIngredientsInTray(newIngredientSpawned);
                //print(i);
            }
        }else{
            for(int i = 0; i < itemSelectedLeftInPlayerInventory; i++){
                GameObject newIngredientSpawned = Instantiate(selectedIngredientItemPrefab,gameObject.transform);
                normalizeTheLookOfIngredientsInTray(newIngredientSpawned);
                //print(i);
            }
        }
        
    }

    //The method will spawn an ingredient into the tray.
    public void AttemptToSpawnONEIngredientsInTray(){

        //If the player has at least 1 of the items in their overall game inventory, spawn it to replenish the tray visual.
        if(itemSelectedLeftInPlayerInventory >= 11){
            GameObject newIngredientSpawned = Instantiate(selectedIngredientItemPrefab,gameObject.transform);
            normalizeTheLookOfIngredientsInTray(newIngredientSpawned);
        }
    }

    //This method is to randomly set an area locally for the item to appear randomly dropped at inside the tray.
    public void normalizeTheLookOfIngredientsInTray(GameObject ingredientSpawned){
        ingredientSpawned.transform.localPosition = new Vector3(Random.Range(-10f, 10f),Random.Range(-1f, 1f),0);
        ingredientSpawned.transform.localScale = new Vector3(0.15f,0.15f,0.15f);
    }

    //This method is used specifically if the player has ladle, for Cassava slime balls and other toppings.
    //Method Made for cleaner code and higher level abstraction.
    public void PutBackNLadleItemIngredientToTray(GameObject ladleObject){
        for (int i = ladleObject.GetComponent<BobaLadelScript>().GetAmmountOfIngredientsInLadle(); i !=0; --i){
                        //Replace the ingredients back in the tray.
                        if(gameObject.transform.childCount < 11){
                            GameObject newIngredientSpawned = Instantiate(selectedIngredientItemPrefab,gameObject.transform);
                            normalizeTheLookOfIngredientsInTray(newIngredientSpawned);
                            ladleObject.GetComponent<BobaLadelScript>().Subtract1AmmountToLadle();
                            ladleObject.GetComponent<BobaLadelScript>().SubtractLadleUses();
                        }
                        //If the ladle is dirty, change it's state/look.
                        if(ladleObject.GetComponent<BobaLadelScript>().IsLadleDirty()){
                            ladleObject.transform.Find("BobaLadelEmpty").gameObject.SetActive(false);
                            ladleObject.transform.Find("BobaLadelDirty").gameObject.SetActive(true);
                        }
                    }
        itemSelectedLeftInPlayerInventory +=1;
        }

    //MAIN METHOD : This method is used if the player has tong or ladle.
    public void TakeOrDropItemIngredientFromTray(){

        //If the player holds an EMPTY Tong (No item between the tong objects), trigger a grab of the item to the tong/inventory.
        if(itemInHandInventory.transform.childCount != 0 && (itemInHandInventory.transform.GetChild(0).name == "FTongHolding(Clone)") && (itemInHandInventory.transform.GetChild(1).name == "BTongHolding(Clone)")){
            
            //Move item from tray into your hand.
            gameObject.transform.GetChild(0).transform.SetParent(itemInHandInventory.transform);

            //Get item in hand inventory, and move the newly placed item in index 2 -> 1 for the tong object to render correctly.
            itemInHandInventory.transform.GetChild(2).SetSiblingIndex(1);

            //Get item in hand inventory, and set the position / scale correctly.
            itemInHandInventory.transform.GetChild(1).position = new Vector3(0,-.75f,0);
            itemInHandInventory.transform.GetChild(1).localScale = new Vector3(3,3,3);

            //Take from overall player inventory.
            itemSelectedLeftInPlayerInventory -=1;
            AttemptToSpawnONEIngredientsInTray();

        }
        //If the player holds an Tong WITH an ingredient (Item between the tong objects in inventory), drop it into the tray IF it matches it's ingredient tray type.
        else if(itemInHandInventory.transform.childCount != 0 && (itemInHandInventory.transform.GetChild(0).name == "FTongHolding(Clone)") && (itemInHandInventory.transform.GetChild(1).name == selectedIngredientItemPrefab.name + "(Clone)")){
            
            //Preform this visual trickery if and ONLY if there are enough items left in player save data.
            if(itemSelectedLeftInPlayerInventory > 11 & gameObject.transform.childCount<10){
                //Destroy the item in your hand & the tray for visual trickery reasons above.
                Destroy(itemInHandInventory.transform.GetChild(1).gameObject);
                //If the item tray has >=10 items, destroy 1 to make the illusion. 
                if(gameObject.transform.childCount<10){
                    Destroy(gameObject.transform.GetChild(0).gameObject);
                }
                //Place back to overall player inventory & item tray.
                itemSelectedLeftInPlayerInventory +=1;
                AttemptToSpawnONEIngredientsInTray();
            }else{
                Destroy(itemInHandInventory.transform.GetChild(1).gameObject);
                //Assure the spawn of another object in the tray from the dropped Tong item.
                GameObject newIngredientSpawned = Instantiate(selectedIngredientItemPrefab,gameObject.transform);
                normalizeTheLookOfIngredientsInTray(newIngredientSpawned);
                //Place back to overall player inventory.
                itemSelectedLeftInPlayerInventory +=1;
            }
        }

        //If the player holds an EMPTY Ladle (No item in the ladle), trigger a grab of the item to the ladle/inventory.
        else if(itemInHandInventory.transform.childCount != 0 && itemInHandInventory.transform.GetChild(0).name == "BobaLadleObject"){
        //Since the player is holding an empty ladle, do action according to what item is in the pot.
        GameObject ladleInInventory = itemInHandInventory.transform.GetChild(0).gameObject;
        
            if(ladleInInventory.transform.GetChild(0).gameObject.activeSelf && gameObject.transform.childCount > 0){
                switch(selectedItemIndexThatWillBeInThisTray){
                    case 1:
                
                    ladleInInventory.transform.Find("BobaLadelEmpty").gameObject.SetActive(false);
                    ladleInInventory.transform.Find("BobaLadelRawSlimeHeld").gameObject.SetActive(true);
                    break;

                    /* To be added when lychee/redbean are added
                    case n:
                    ladleInInventory.transform.Find("BobaLadelEmpty").gameObject.SetActive(false);
                    ladleInInventory.transform.Find("BobaLadelRedBeanHeld").gameObject.SetActive(true);
                    break;*/
                }
                //NOTE : LADLE ALWAYS CARRIES ONLY THE MAX OUT FROM THE ITEM TRAY. It cannot get items < ladle max carry.
                //Transfer the topping ammounts to the ladle and remove from item tray accordingly.
                //Preform this visual trickery if and ONLY if there are enough items left in player save data, and the player got a valid topping.
                if(ladleInInventory.transform.Find("BobaLadelEmpty").gameObject.activeSelf != true){
                    //Destroy n items in the tray for visual trickery reasons above, depending on how many items the ladle took and were in the item tray.
                    if(ladleInInventory.GetComponent<BobaLadelScript>().GetMaxCarryLadleAmmount() > gameObject.transform.childCount){
                        ladleInInventory.GetComponent<BobaLadelScript>().SetAmmountOfIngredientsInLadle(gameObject.transform.childCount);
                        for(int i = gameObject.transform.childCount ; i > 0; --i){
                            itemSelectedLeftInPlayerInventory -=1;
                            Destroy(gameObject.transform.GetChild(i-1).gameObject);
                        }
                    }else{
                        for (int i = ladleInInventory.GetComponent<BobaLadelScript>().GetMaxCarryLadleAmmount(); i > 0; --i){
                            ladleInInventory.GetComponent<BobaLadelScript>().Add1AmmountToLadle();
                            itemSelectedLeftInPlayerInventory -=1;
                            Destroy(gameObject.transform.GetChild(i-1).gameObject);
                        if(itemSelectedLeftInPlayerInventory > 11 && gameObject.transform.childCount < 10){
                            AttemptToSpawnONEIngredientsInTray();
                        }
                    }
                    }
                }
            //If the player holds a FILLED Ladle (Topping items in the ladle), trigger a put back the item(s) to the item tray/inventory.
            }else if(!ladleInInventory.transform.GetChild(0).gameObject.activeSelf){
                if(selectedItemIndexThatWillBeInThisTray == 1 && ladleInInventory.transform.Find("BobaLadelRawSlimeHeld").gameObject.activeSelf == true){
                    ladleInInventory.transform.Find("BobaLadelEmpty").gameObject.SetActive(true);
                    ladleInInventory.transform.Find("BobaLadelRawSlimeHeld").gameObject.SetActive(false);
                    PutBackNLadleItemIngredientToTray(ladleInInventory);
                }
                /* FIXME: Alt if statments to be added when lychee/redbean are added
                if(selectedItemIndexThatWillBeInThisTray == 1 && ladleInInventory.transform.Find("BobaLadelRawRedbean").gameObject.activeSelf == true){
                 
                }
                if(selectedItemIndexThatWillBeInThisTray == 1 && ladleInInventory.transform.Find("BobaLadelLychee").gameObject.activeSelf == true){
                 
                }*/
                    //Transfer the topping ammounts to the ladle and remove from item tray accordingly.
                    //Preform this visual trickery to put back toppings into item tray if valid.
                    //Destroy n items in the tray for visual trickery reasons above, depending on max size of the ladle.
            }else{
                //Play wrong interaction hand animation.
                Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
                itemInHandInventoryAnimator.Play("IncorrectInteraction");
            }
        }else{
            //Play wrong interaction hand animation.
            Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
            itemInHandInventoryAnimator.Play("IncorrectInteraction");
        }
    }
}
