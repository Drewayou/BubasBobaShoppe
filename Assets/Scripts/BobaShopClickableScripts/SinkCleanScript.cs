using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkCleanScript : MonoBehaviour
{

    //Gets game Object to check what the player is currently holding
    [SerializeField]
    GameObject itemInHandInventory;
    [SerializeField]
    GameObject sink,wipeCloth;

    Animator sinkAnimation;

    // Start is called before the first frame update
    void Start()
    {
        sinkAnimation = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CleanObjectInHand(){

        if(itemInHandInventory.transform.childCount != 0){
        
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
            }
        }else{
            //Play wrong interaction hand animation.
            Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
            itemInHandInventoryAnimator.Play("IncorrectInteraction");
        }
    }
}
