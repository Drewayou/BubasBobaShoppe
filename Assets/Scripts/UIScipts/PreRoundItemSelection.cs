using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PreRoundItemSelection : MonoBehaviour
{

    //GameManagerScript to pull from.
    [SerializeField]
    public GameManagerScript thisGamesCurrentManagerScript;

    //Gameobject UI for buttons to start shop.
    [SerializeField]
    [Tooltip("Put the UI Object Shop round's UI buttons/info text.")]
    public GameObject menuItemsLeftToSelect;
    public GameObject RunBobaShopButton;

    //Gameobject prefab to spawn the checkmark of the object to sell
    [SerializeField]
    [Tooltip("Put the checkmark UI prefab here.")]
    public GameObject checkMarkItemSelectedPrefabUI;

    //Int of items selected for the shop round.
    public int ingredientsSlected;

    // Start is called before the first frame update.
    void Start()
    {
        thisGamesCurrentManagerScript = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
        ingredientsSlected = 0;
    }

    // Update is called once per frame.
    void Update()
    {
        //Check how many items were selected to sell in a shop round.
        if(thisGamesCurrentManagerScript.ReturnPlayerStats().shopTraysAvailable-1 >= ingredientsSlected){
            RunBobaShopButton.SetActive(false);
            menuItemsLeftToSelect.SetActive(true);
            menuItemsLeftToSelect.GetComponentInChildren<TMP_Text>().SetText("Pick " + (thisGamesCurrentManagerScript.ReturnPlayerStats().shopTraysAvailable - ingredientsSlected) + " \nitems to sell");
        }else{
            menuItemsLeftToSelect.SetActive(false);
            RunBobaShopButton.SetActive(true);
        }
    }

    //Setters, Getters, and Return for Items Selected on inventory page(s).
    public void DecreaseHowManyIngredientsWereSlected(){
       ingredientsSlected -= 1;
    }

    public void IncreaseIngredientsThatWereSlected(){
        ingredientsSlected += 1;
    }

    public int HowManyIngredientsWereSelected(){
        return ingredientsSlected;
    }
}
