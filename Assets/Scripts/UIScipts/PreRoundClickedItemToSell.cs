using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreRoundClickedItemToSell : MonoBehaviour
{

    //GameManagerObject for data.
    [SerializeField]
    [Tooltip("Put this scene's GameManagerObject here")]
    public GameObject currentGameManager;

    //Gameobject of the drink page that has a script saves how many items were clicked.
    [SerializeField]
    [Tooltip("Get the ObjectDrinkBreakDown")]
    public GameObject itemInventoryPageOrObjectDrinkBreakDown;

    //Gameobject to get the check UI prefab
    [SerializeField]
    [Tooltip("Get the checkmark object UI")]
    public GameObject checkUIPrefab;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //FIXME: You need to make it so that the player can't click on items that are missing!
    //Use this code for when the UI for the inventory items in pre-round is clicked, if the checkmark prefab is saved in it, add it's count to the sell page item selected script.
    //Else, do the opposite and destroy the checkmark prefab. 
    public void ClickedThisItemToSell(){
        //Do the interaction to spawn/delete a check prefab. In addition, update the connected data points for which ingredient was selected.
            //Scenario if the item was selected already.
            if(gameObject.transform.Find("CheckIcoPrefab(Clone)") != null){
                itemInventoryPageOrObjectDrinkBreakDown.GetComponent<PreRoundItemSelection>().DecreaseHowManyIngredientsWereSlected();
                Destroy(gameObject.transform.Find("CheckIcoPrefab(Clone)").gameObject);
            }else{
                //Scenario if the item is to be selected.
                if(itemInventoryPageOrObjectDrinkBreakDown.GetComponent<PreRoundItemSelection>().HowManyIngredientsWereSelected() <= currentGameManager.GetComponent<GameManagerScript>().ReturnPlayerStats().shopTraysAvailable -1  && playerHasItemSelectedInInventory()){
                    
                    //FIXME: if(ItemSelected == 0 in the game manager player data){
                    GameObject checkMark = Instantiate(checkUIPrefab);
                    checkMark.transform.SetParent(gameObject.transform);
                    checkMark.transform.localPosition = new Vector3(0,-75,0);
                    checkMark.transform.localScale = new Vector3(1,1,1);
                    itemInventoryPageOrObjectDrinkBreakDown.GetComponent<PreRoundItemSelection>().IncreaseIngredientsThatWereSlected();
                //}
                }
            }
    }

    //This code is used by the "RunBobaShop" button to generate what items were selected into the item trays!
    public void SaveTheseItemToSellInShopRound(){
            //Clears the running game data's selected item shop list, then compares the player's shop trays from last round.
            currentGameManager.GetComponent<GameManagerScript>().ReturnPlayerStats().shopTraysItemListArray.Clear();
            int traysFilled = currentGameManager.GetComponent<GameManagerScript>().ReturnPlayerStats().shopTraysAvailable;

            //Iterates through all possible item inventory if they trays aren't full, for specifically marked ones to add to the new shop list.
            foreach(Transform item in itemInventoryPageOrObjectDrinkBreakDown.transform){
            if(traysFilled!=0){
                if(item.transform.Find("CheckIcoPrefab(Clone)") != null){
                String itemSelected = item.name;
                    switch (itemSelected){
                        case "CassavaBallsMini":
                        traysFilled -=1;
                        currentGameManager.GetComponent<GameManagerScript>().ReturnPlayerStats().shopTraysItemListArray.Add(1);
                        break;
                        case "PandanLeafMini":
                        traysFilled -=1;
                        currentGameManager.GetComponent<GameManagerScript>().ReturnPlayerStats().shopTraysItemListArray.Add(2);
                        break;
                        case "BananaMini":
                        traysFilled -=1;
                        currentGameManager.GetComponent<GameManagerScript>().ReturnPlayerStats().shopTraysItemListArray.Add(3);
                        break;
                        case "StrawberryMini":
                        traysFilled -=1;
                        currentGameManager.GetComponent<GameManagerScript>().ReturnPlayerStats().shopTraysItemListArray.Add(4);
                        break;
                        case "MangoMini":
                        traysFilled -=1;
                        currentGameManager.GetComponent<GameManagerScript>().ReturnPlayerStats().shopTraysItemListArray.Add(5);
                        break;
                        case "UbeMini":
                        traysFilled -=1;
                        currentGameManager.GetComponent<GameManagerScript>().ReturnPlayerStats().shopTraysItemListArray.Add(6);
                        break;
                    }
                }
            }
            }
            //Attempts to finalize the selection and saves this selected array to the game save FILES. 
            currentGameManager.GetComponent<GameManagerScript>().SavePlayerDataAfterSelectingIngredientsForShopRound(currentGameManager.GetComponent<GameManagerScript>().ReturnPlayerStats());
    }

    public bool playerHasItemSelectedInInventory(){
        print(gameObject.name.ToString());
            switch (gameObject.name){
                        case "CassavaBallsMini":
                        if (currentGameManager.GetComponent<GameManagerScript>().ReturnPlayerStats().casavaBalls!=0){
                            print("Added Cassava");
                            return true;
                        }else{
                            return false;
                        }
                        case "PandanLeafMini":
                        if (currentGameManager.GetComponent<GameManagerScript>().ReturnPlayerStats().pandanLeaves!=0){
                            return true;
                        }else{
                            return false;
                        }
                        case "BananaMini":
                        if (currentGameManager.GetComponent<GameManagerScript>().ReturnPlayerStats().bananas!=0){
                            return true;
                        }else{
                            return false;
                        }
                        case "StrawberryMini":
                        if (currentGameManager.GetComponent<GameManagerScript>().ReturnPlayerStats().strawberries!=0){
                            return true;
                        }else{
                            return false;
                        }
                        case "MangoMini":
                        if (currentGameManager.GetComponent<GameManagerScript>().ReturnPlayerStats().mangos!=0){
                            return true;
                        }else{
                            return false;
                        }
                        case "UbeMini":
                        if (currentGameManager.GetComponent<GameManagerScript>().ReturnPlayerStats().ube!=0){
                            return true;
                        }else{
                            return false;
                        }
                        default:
                        return false;
                }
    }
}
