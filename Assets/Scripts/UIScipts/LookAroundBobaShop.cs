using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAroundBobaShop : MonoBehaviour
{

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
}
