using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageChanger : MonoBehaviour
{

    public GameObject pageDesired;

    [SerializeField]
    public List<GameObject> pagesOfThisUI;

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

    public void switchPage(){

        switch(myButtonGameObject.name){

            case "GoldCoinTab":

            pageDesired = pagesOfThisUI[0];
            break;

            case "BobaRatesTab":

            pageDesired = pagesOfThisUI[1];
            break;

            case "AttackMapTab":

            pageDesired = pagesOfThisUI[2];
            break;

            case "HelpTab":

            pageDesired = pagesOfThisUI[3];
            break;
        }

    

        //Hide all pages first
        hideAllPages();

        //enable the selected page
        pageDesired.SetActive(true);
    }

    public void switchPageViaIndex(int index){

        switch(index){

            case 1:

            pageDesired = pagesOfThisUI[0];
            break;

            case 2:

            pageDesired = pagesOfThisUI[1];
            break;

            case 3:

            pageDesired = pagesOfThisUI[2];
            break;

            case 4:

            pageDesired = pagesOfThisUI[3];
            break;
            case 5:

            pageDesired = pagesOfThisUI[4];
            break;
            case 6:

            pageDesired = pagesOfThisUI[5];
            break;
            case 7:

            pageDesired = pagesOfThisUI[6];
            break;
            case 8:

            pageDesired = pagesOfThisUI[7];
            break;
            case 9:

            pageDesired = pagesOfThisUI[8];
            break;

        }

    

        //Hide all pages first
        hideAllPages();

        //enable the selected page
        pageDesired.SetActive(true);
    }

    public void hideAllPages(){
            foreach(GameObject pages in pagesOfThisUI){
            pages.SetActive(false);
        }
    }
}
