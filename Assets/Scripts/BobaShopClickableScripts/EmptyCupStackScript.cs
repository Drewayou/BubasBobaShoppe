using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyCupStackScript : MonoBehaviour
{

    //Gets game Object to check what the player is currently holding
    [SerializeField]
    GameObject itemInHandInventory;
    
    [SerializeField]
    GameObject emptyCup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeCup(){

        if(itemInHandInventory.transform.childCount == 0){
            Vector3 emptyCupLocation = new Vector3(0f,-.75f,0f);

            GameObject emptyCupInHand = Instantiate(emptyCup,emptyCupLocation,Quaternion.identity,itemInHandInventory.transform);
            emptyCupInHand.transform.localScale = new Vector3(3,3,3);
            
        }else if(itemInHandInventory.transform.GetChild(0).gameObject.name == "EmptyCup(Clone)"){
            Destroy(itemInHandInventory.transform.GetChild(0).gameObject);
        }else{
            //Play wrong interaction hand animation.
            Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
            itemInHandInventoryAnimator.Play("IncorrectInteraction");
        }
    }
}
