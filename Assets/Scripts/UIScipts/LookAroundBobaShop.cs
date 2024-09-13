using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LookAroundBobaShop : MonoBehaviour
{
    private int ShopGuiShowing = 0;
    public int ShopAreaIndexDesired;

    [SerializeField]
    public List<GameObject> shopUIList;

    Button myButtonGameObject;

    // Start is called before the first frame update
    void Start()
    {
        myButtonGameObject = gameObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This method is for the manual switch shop buttons.
    public void SwitchShopUI(){

        //Hide the whole shop before enabling the right UI to show.
        HideAllShopUI();

        //Switch to selected shop co-responding to the button pressed.
        switch(ShopAreaIndexDesired){
            case 0:
            shopUIList[0].SetActive(true);

            break;
            case 1:
            shopUIList[1].SetActive(true);

            break;
            case 2:
            shopUIList[2].SetActive(true);

            break;
            case 3:
            shopUIList[3].SetActive(true);

            break;

            default:
            shopUIList[0].SetActive(true);

            break;

        }
    }

    public void HideAllShopUI(){
            foreach(GameObject pages in shopUIList){
            pages.SetActive(false);
        }
    }

    //
    //NOTE: Below are for the input of player to switch sides of the boba shop. Scenes are indexed from 0->3.
    //Where going right means turing CLOCKWISE, and left means turning COUNTERCLOCKWISE.
    //This method is for the callback context player input of switching scenes Right.
    //
    public void SwitchShopUIRight(InputAction.CallbackContext context){
        if(context.started){
        if(ShopGuiShowing<3){
            ShopGuiShowing += 1;
        }else{
            ShopGuiShowing = 0;
        }
        HideAllShopUI();
        shopUIList[ShopGuiShowing].SetActive(true);
        }
    }

    //This method is for the callback context player input of switching scenes Left.
    public void SwitchShopUILeft(InputAction.CallbackContext context){
        if(context.started){
        if(ShopGuiShowing>0){
            ShopGuiShowing -= 1;
        }else{
            ShopGuiShowing = 3;
        }
        HideAllShopUI();
        shopUIList[ShopGuiShowing].SetActive(true);
        }
    }
}
