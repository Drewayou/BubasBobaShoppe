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
    private int itemSelectedLeftInPlayerInventory;

    //The Pre-fabs for all the possible Ingredient Items.
    [SerializeField]
    [Tooltip("Add all the pre-fabs for the possible ingredients below!")]
    public GameObject CassavaBall,PandanLeaf,BananaMini,StrawberryMini,MangoMini,UbeMini;

    //The selected Pre-fab to spawn.
    public GameObject selectedIngredientItemPrefab;

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
        int selectedItemIndexThatWillBeInThisTray = currentGameManagerInstance.ReturnPlayerStats().shopTraysItemListArray[itemTrayNumber];

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
            for(int i = 0; i < 11; i++){
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
        if(itemSelectedLeftInPlayerInventory > 10){
            GameObject newIngredientSpawned = Instantiate(selectedIngredientItemPrefab,gameObject.transform);
            normalizeTheLookOfIngredientsInTray(newIngredientSpawned);
        }
    }

    //This method is to randomly set an area locally for the item to appear randomly dropped at inside the tray.
    public void normalizeTheLookOfIngredientsInTray(GameObject ingredientSpawned){
        ingredientSpawned.transform.localPosition = new Vector3(Random.Range(-10f, 10f),Random.Range(-1f, 1f),0);
        ingredientSpawned.transform.localScale = new Vector3(.15f,.15f,.15f);
    }

    //This method is used if the player has tongs.
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

            AttemptToSpawnONEIngredientsInTray();

        }

        //If the player holds an Tong WITH an ingredient (Item between the tong objects in ionventory), drop it into the tray IF it matches it's ingredient tray type.
        else if(itemInHandInventory.transform.childCount != 0 && (itemInHandInventory.transform.GetChild(0).name == "FTongHolding(Clone)") && (itemInHandInventory.transform.GetChild(1).name == selectedIngredientItemPrefab.name + "(Clone)")){
            
            //Preform this visual trickery if and ONLY if there are enough items left in player save data.
            if(itemSelectedLeftInPlayerInventory > 11){
                //Destroy the item in your hand & the tray for visual trickery reasons above.
                Destroy(itemInHandInventory.transform.GetChild(1).gameObject);
                Destroy(gameObject.transform.GetChild(0).gameObject);
                AttemptToSpawnONEIngredientsInTray();
            }else{
                Destroy(itemInHandInventory.transform.GetChild(1).gameObject);
                //Assure the spawn of another object in the tray from the dropped Tong item.
                GameObject newIngredientSpawned = Instantiate(selectedIngredientItemPrefab,gameObject.transform);
                normalizeTheLookOfIngredientsInTray(newIngredientSpawned);
            }
        }else{
            //Play wrong interaction hand animation.
            Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
            itemInHandInventoryAnimator.Play("IncorrectInteraction");
        }
    }
}
