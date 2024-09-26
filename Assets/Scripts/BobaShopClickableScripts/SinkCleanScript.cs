using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkCleanScript : MonoBehaviour
{

    //Gets game Object to check what the player is currently holding.
    [SerializeField]
    GameObject itemInHandInventory;
    [SerializeField]
    GameObject sink,wipeCloth;

    //Gameobject to initialize the used cup.
    [SerializeField]
    [Tooltip("Drag the \"UsedCup\" prefab object here.")] 
    GameObject usedCupPrefab;

    Animator sinkAnimation;

    // Start is called before the first frame update.
    void Start()
    {
        sinkAnimation = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame.
    void Update()
    {
        
    }

    public void CleanObjectInHand(){

        if(itemInHandInventory.transform.childCount != 0 && itemInHandInventory.transform.GetChild(0).gameObject.tag != "FinishedBobaDrink"){
        
        switch(itemInHandInventory.transform.GetChild(0).gameObject.name){

                case "WipeCloth(Clone)":
                sinkAnimation.Play("SinkUsed");
                break;

                case "WipeClothDirty(Clone)":
                sinkAnimation.Play("SinkUsed");
                Destroy(itemInHandInventory.transform.GetChild(0).gameObject);
                Vector3 clothHeldPosition = new Vector3(0f,-.75f,0f);
                Instantiate(wipeCloth,clothHeldPosition,Quaternion.identity,itemInHandInventory.transform);
                break;

                case "BobaLadleObject":
                sinkAnimation.Play("SinkUsed");
                itemInHandInventory.transform.GetChild(0).GetComponent<BobaLadelScript>().SetLadleToClean();
                break;
            }
        }else if(itemInHandInventory.transform.GetChild(0).gameObject.tag == "FinishedBobaDrink" | itemInHandInventory.transform.GetChild(0).gameObject.tag == "BobaDrink"){
            WashCup();
        }else{
            //Play wrong interaction hand animation.
            Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
            itemInHandInventoryAnimator.Play("IncorrectInteraction");
        }
    }

    public void WashCup(){
        //Destroy the cup and instantiate a used cup.
        Destroy(itemInHandInventory.transform.GetChild(0).gameObject);
        
        //Instantiate an empty cup.
        GameObject usedCupClone = Instantiate(usedCupPrefab, itemInHandInventory.transform);
        usedCupClone.transform.position = new Vector3(0f,-.75f,0f);
        usedCupClone.transform.localScale = new Vector3(3f,3f,3f);
    }
}
