using TMPro;
using UnityEngine;

public class OrderTabUIGeneratorScript : MonoBehaviour
{
    // This int is to progam what customer this tab relays to.
    [Tooltip("Input which drink tab this is (Co-responds to the customer in queue).")]
    public int drinkTabIndexThisIs = 0;

    // This script pulls data from what customers are in the list to get their desired drink UID's.
    [SerializeField]
    [Tooltip("Drag and drop the CustomerWaitingHandlerScript from the waiting for ordered drink queue gameobject.")]
    CustomerWaitingHandlerScript customerWaitingDrinkQueue;

    // These gameobjects are the ingredient Icons to populate the drink tabs.
    [SerializeField]
    [Tooltip("Drag and drop the ingredient Icon gameobjects to populate the drink tabs.")]
    GameObject textIco,pandanIco,bananaIco,strawberryIco,mangoIco,ubeIco,bobaIco;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateDrinkTabUI(){
        // Clear out the drink tab just incase.
        EraseAllItemsInThisDrinkTab();

        // Iterate through the related customer waiting for drink queue if they exist.
        if(drinkTabIndexThisIs <= customerWaitingDrinkQueue.waitingForOrderCustomerQueue.Count-1){
            //Itereate through each drink this co-responding customer ordered.
            int drinkIndexNext = 0;
            foreach(string drinkUIDToScan in customerWaitingDrinkQueue.waitingForOrderCustomerQueue[drinkTabIndexThisIs].GetComponent<CustomerDrinkScript>().drinksThisNPCOrdered){
                //Temp index to save the info to make sure it spaces evenly.
                int infoIndexNext = 0;
                
                //Break down each drink UID to populate the tab.
                //Tea Ingredient Base
                GameObject teaIngredientBase;
                switch(drinkUIDToScan.Substring(0, 2)){
                    case "--":
                        //Do Nothing.
                    break;
                    case "PD":
                        teaIngredientBase = Instantiate(pandanIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        teaIngredientBase.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
                        teaIngredientBase.transform.localPosition = new Vector3(0f, 20f, 0f);
                        infoIndexNext++;
                    break;
                    case "BN":
                        teaIngredientBase = Instantiate(bananaIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        teaIngredientBase.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
                        teaIngredientBase.transform.localPosition = new Vector3(0f, 20f, 0f);
                        infoIndexNext++;
                    break;
                    case "SB":
                        teaIngredientBase = Instantiate(strawberryIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        teaIngredientBase.transform.localScale = new Vector3(0.55f, 0.55f, 0.25f);
                        teaIngredientBase.transform.localPosition = new Vector3(0f, 20f, 0f);
                        infoIndexNext++;
                    break;
                    case "MB":
                        teaIngredientBase = Instantiate(mangoIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        teaIngredientBase.transform.localScale = new Vector3(0.55f, 0.55f, 0.25f);
                        teaIngredientBase.transform.localPosition = new Vector3(0f, 20f, 0f);
                        infoIndexNext++;
                    break;
                    case "UB":
                        teaIngredientBase = Instantiate(ubeIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        teaIngredientBase.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
                        teaIngredientBase.transform.localPosition = new Vector3(0f, 20f, 0f);
                        infoIndexNext++;
                    break;
                }
                //TeaBase
                GameObject textTeaBase;
                switch(drinkUIDToScan.Substring(2, 2)){
                    case "--":
                        //Do Nothing
                    break;
                    case "GB":
                        textTeaBase = Instantiate(textIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        textTeaBase.GetComponent<TMP_Text>().SetText("G");
                        infoIndexNext++;
                    break;
                    case "BB":
                        textTeaBase = Instantiate(textIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        textTeaBase.GetComponent<TMP_Text>().SetText("B");
                        infoIndexNext++;
                    break;
                    case "OB":
                        textTeaBase = Instantiate(textIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        textTeaBase.GetComponent<TMP_Text>().SetText("O");
                        infoIndexNext++;
                    break;
                }
                //Drink overlay
                GameObject textFlavorOverlay;
                switch(drinkUIDToScan.Substring(4, 1)){
                    case "-":
                        //Do Nothing.
                    break;
                    case "M":
                        textFlavorOverlay = Instantiate(textIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        textFlavorOverlay.GetComponent<TMP_Text>().SetText("M");
                        infoIndexNext++;
                    break;
                    case "W":
                        textFlavorOverlay = Instantiate(textIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        textFlavorOverlay.GetComponent<TMP_Text>().SetText("W");
                        infoIndexNext++;
                    break;
                }
                //Toppings
                GameObject bobaToppings;
                switch(drinkUIDToScan.Substring(5, 2)){
                    case "*-":
                        //Do Nothing.
                    break;
                    case "*B":
                        bobaToppings = Instantiate(bobaIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        bobaToppings.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                        infoIndexNext++;
                    break;
                }
                //Tempurature
                switch(drinkUIDToScan.Substring(7, 1)){
                    case "-":
                        //Do Nothing.
                    break;
                }
                //Sweetness
                switch(drinkUIDToScan.Substring(8, 1)){
                    case "-":
                        //Do Nothing.
                    break;
                }
                drinkIndexNext++;
            }
        }
    }

    public void EraseAllItemsInThisDrinkTab(){
        foreach(Transform drinkTransform in this.gameObject.transform){
            foreach(Transform infoTransform in drinkTransform){
                if(infoTransform.childCount > 0){
                    Destroy(infoTransform.GetChild(0).gameObject);
                }
            }
        }

    }
}
