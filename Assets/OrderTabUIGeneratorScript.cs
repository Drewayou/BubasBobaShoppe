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
    GameObject pandanIco,bananaIco,strawberryIco,mangoIco,ubeIco;

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
                switch(drinkUIDToScan.Substring(0, 2)){
                    case "--":
                        //Do Nothing.
                    break;
                    case "PD":
                        Instantiate(pandanIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        infoIndexNext++;
                    break;
                    case "BN":
                        Instantiate(bananaIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        infoIndexNext++;
                    break;
                    case "SB":
                        Instantiate(strawberryIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        infoIndexNext++;
                    break;
                    case "MB":
                        Instantiate(mangoIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        infoIndexNext++;
                    break;
                    case "UB":
                        Instantiate(ubeIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        infoIndexNext++;
                    break;
                }
                //TeaBase
                switch(drinkUIDToScan.Substring(2, 2)){
                    case "--":
                        //Do Nothing
                    break;
                    case "GB":
                        Instantiate(ubeIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        infoIndexNext++;
                    break;
                    case "BB":
                        Instantiate(ubeIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        infoIndexNext++;
                    break;
                    case "OB":
                        Instantiate(ubeIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        infoIndexNext++;
                    break;
                }
                //Drink overlay
                switch(drinkUIDToScan.Substring(4, 1)){
                    case "-":
                        //Do Nothing.
                    break;
                    case "M":
                        Instantiate(ubeIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        infoIndexNext++;
                    break;
                    case "W":
                        Instantiate(ubeIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
                        infoIndexNext++;
                    break;
                }
                //Toppings
                switch(drinkUIDToScan.Substring(5, 2)){
                    case "*-":
                        //Do Nothing.
                    break;
                    case "*B":
                        Instantiate(ubeIco,this.gameObject.transform.GetChild(drinkIndexNext).transform.GetChild(infoIndexNext));
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
