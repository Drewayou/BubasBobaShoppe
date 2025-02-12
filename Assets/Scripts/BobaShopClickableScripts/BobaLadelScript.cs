using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobaLadelScript : MonoBehaviour
{
    //NOTE: This script both works for the ladle and the ladle hanger!
    //The Game manager instance of this round (Will automatically be pulled in Start() method.
    private GameManagerScript currentGameManagerInstance;

    //Gets game Object to check what the player is currently holding
    [SerializeField]
    GameObject itemInHandInventory;
    [SerializeField]
    GameObject bobaLadel;

    //How many items the ladle can transport before it gets dirty.
    private int maxcleanCountOfLadle;
    public int cleanCountOfLadle;

    //The max size the ladle can carry directly from the pot.
    private int maxLadleCarrySize;

    //The size the ladle is carrying if it has ingredients.
    public int currentLadleCarrySize = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentGameManagerInstance = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
        //Set to how "non-stick" the ladle is, and how many items it can transport before becoming dirty.
        maxcleanCountOfLadle = currentGameManagerInstance.ReturnPlayerStats().maxCleanCountOfLadle;
        cleanCountOfLadle = maxcleanCountOfLadle;

        //Set the max carry size the ladle has when taking directly from the boba pot.
        maxLadleCarrySize = currentGameManagerInstance.ReturnPlayerStats().maxCarrySizeOfLadle;
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void TakeBobaLadel(){

        if(itemInHandInventory.transform.childCount == 0){
            gameObject.transform.GetChild(0).transform.localScale = new Vector3(.5f, .5f, .5f);
            Vector3 ladelHeld = new Vector3(0f,-.6f,0f);
            gameObject.transform.GetChild(0).transform.position = ladelHeld;
            gameObject.transform.GetChild(0).transform.SetParent(itemInHandInventory.transform);
        }else if(itemInHandInventory.transform.childCount > 0 && itemInHandInventory.transform.GetChild(0).gameObject.name == bobaLadel.name){
            itemInHandInventory.transform.GetChild(0).transform.SetParent(gameObject.transform);
            gameObject.transform.GetChild(0).transform.localScale = new Vector3(0.19f, 0.19f, 0.19f);
            gameObject.transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0);
        }else{
            //Play wrong interaction hand animation.
            Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
            itemInHandInventoryAnimator.Play("IncorrectInteraction");
        }
    }

    //Getters, setters, and other methods that other scripts use for the ladle.
    public bool IsLadleDirty(){
        if(cleanCountOfLadle<=0){
            return true;
        }else return false;
    }

    public void SubtractLadleUses(){
        cleanCountOfLadle -= 1;
    }

    public void SubtractLadleUsesByInput(int ingredientsToMove){
        cleanCountOfLadle -= ingredientsToMove;
    }

    public void SubtractLadleUsesByMaxCarrySize(){
        cleanCountOfLadle -= maxLadleCarrySize;
    }

    public void WashLadleAndResetCleanCount(){
        cleanCountOfLadle = maxcleanCountOfLadle;
    }

    public int GetAmmountOfIngredientsInLadle(){
        return currentLadleCarrySize;
    }
    public int GetMaxCarryLadleAmmount(){
        return maxLadleCarrySize;
    }

    public void Add1AmmountToLadle(){
        currentLadleCarrySize += 1;
    }

    public void Subtract1AmmountToLadle(){
        currentLadleCarrySize -= 1;
    }

    public void SetAmmountOfIngredientsInLadle(int newAmmountToPutOnLadle){
        currentLadleCarrySize = newAmmountToPutOnLadle;
    }

    public void SetAmmountOfIngredientsToLadleMax(){
        currentLadleCarrySize = maxLadleCarrySize;
    }

    public void SetLadleToClean(){
        WashLadleAndResetCleanCount();
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);
        gameObject.transform.GetChild(4).gameObject.SetActive(false);
        gameObject.transform.GetChild(5).gameObject.SetActive(false);
        SetAmmountOfIngredientsInLadle(0);
    }
}
