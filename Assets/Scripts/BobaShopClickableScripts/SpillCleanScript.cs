using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class SpillCleanScript : MonoBehaviour
{

    //Gets game Object to check what the player is currently holding
    [SerializeField]
    GameObject itemInHandInventory, wipeClothDirty;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CleanUpMess(){

        //If player holds a clean wipe cloth, clean (destroy) this spill and replace with a wipe cloth that's dirty.
        if(itemInHandInventory.transform.childCount != 0 && itemInHandInventory.transform.GetChild(0).gameObject.name == "WipeCloth(Clone)"){
            Destroy(itemInHandInventory.transform.GetChild(0).gameObject);
            Vector3 clothHeldPosition = new Vector3(0f,-.75f,0f);
            Instantiate(wipeClothDirty,clothHeldPosition,Quaternion.identity,itemInHandInventory.transform);
            Destroy(gameObject);
        }else{
            //Play wrong interaction hand animation.
            Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
            itemInHandInventoryAnimator.Play("IncorrectInteraction");
        }
    }
}
