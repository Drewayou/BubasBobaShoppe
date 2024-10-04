using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanThrowTrashScript : MonoBehaviour
{

    //Gets game Object to check what the player is currently holding.
    [SerializeField]
    GameObject itemInHandInventory;
    
    //Gets the SFX object holder in the scene that keeps the sfx of the game loaded!
    [SerializeField]
    [Tooltip("Drag the object that holds all the sfx here!")]
    GameObject sfxHolderObject;

    //Gets possible Trash SFX Objects.
    [SerializeField]
    [Tooltip("Drag all the possible trash can sfx here!")]
    GameObject trashSfx1, trashSfx2, trashSfx3, trashSfx4;

    public void ThrowAwayTrashInHand(){

        if(itemInHandInventory.transform.childCount > 0 && itemInHandInventory.transform.GetChild(0).gameObject.tag == "Trash"){
            ThrowTrash();
        }else{
            //Play wrong interaction hand animation.
            Animator itemInHandInventoryAnimator = itemInHandInventory.GetComponent<Animator>();
            itemInHandInventoryAnimator.Play("IncorrectInteraction");
        }
    }

    public void ThrowTrash(){
        //Destroy the trash.
        Destroy(itemInHandInventory.transform.GetChild(0).gameObject);
        
        //Makes a randomm selection for a random trash sfx to play! Why are there so many random sfx? Idk LMAO.
        switch(Random.Range(1,5)){
            case 1:
                Instantiate(trashSfx1,sfxHolderObject.transform);
            break;
            case 2:
                Instantiate(trashSfx2,sfxHolderObject.transform);
            break;
            case 3:
                Instantiate(trashSfx3,sfxHolderObject.transform);
            break;
            case 4:
                Instantiate(trashSfx4,sfxHolderObject.transform);
            break;

        }
    }
}
