using System.Collections;
using TMPro;
using UnityEngine;

public class ShopManagerScript : MonoBehaviour
{
    GameManagerScript thisGamesCurrentManagerScript;
    ShopCostsNEarnings thisGamesCurrentShopDataInstance;
    PlayerDataJson thisPlayersCurrentData;

    [SerializeField]
    //FIXME: add desc
    GameObject playShopBuySfx, playShopfailedSfx;

    //Sprite renderers to display the progress of the player from the maxed out settings.
    [SerializeField]
    [Header("Add the Image object of the meters")]
    [Tooltip("Connect the fields to this to co-respond fill to the GameManager saved stats for max player stats")]
    UnityEngine.UI.Image hpMaxUpgradeMeter, staminaMaxUpgradeMeter, attackMaxUpgradeMeter;

    [SerializeField]
    [Header("Add the TXT field to update costs of OBJ")]
    [Tooltip("Connect the fields to this to co-respond to the GameManager saved stats")]
    TMP_Text PlayerTotalGameGold, PlayerTotalGameDays, UpgradeStoreCostTxt,StoreCurrentLevelNMultiplierTxt,HPCostTxt,PlayerCurrentHp,StaminaCostTxt,PlayerCurrentSp,AttackCostTxt,PlayerCurrentAtck,
    world2CostTxt,world3CostTxt,world4CostTxt;

    [SerializeField]
    [Header("BuyLevelButton")]
    [Tooltip("Connect buttons to this to co-respond the levels theplayer can buy")]
    GameObject Level2Buy, Level3Buy, Level4Buy, nextPage, CanvasMainMenu;

    [SerializeField]
    [Header("NotEnoughMoneyAlert")]
    [Tooltip("What Pre-Fab to display if the player doesn't have enough money")]
    GameObject NotEnoughMoneyAlert;

    [Header("NotEnoughMoneyAlertAnimController")]
    [Tooltip("Calls the anim if the player doesn't have enough money")]
    Animator NotEnoughMoneyAnim;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        NotEnoughMoneyAnim = NotEnoughMoneyAlert.GetComponent<Animator>();
        thisGamesCurrentManagerScript = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
        thisGamesCurrentShopDataInstance = thisGamesCurrentManagerScript.ReturnCurrentShopInstance();
        thisPlayersCurrentData = thisGamesCurrentManagerScript.ReturnStatsOfThisPlayer();
        UpdateShopDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if(thisPlayersCurrentData.unlockedLeve2){
            Level2Buy.SetActive(false);
        }
        if(thisPlayersCurrentData.unlockedLeve3){
            Level3Buy.SetActive(false);
        }
        if(thisPlayersCurrentData.unlockedLeve4){
            Level4Buy.SetActive(false);
        }
        if(thisPlayersCurrentData.unlockedBossFight){
            nextPage.SetActive(true);
        }
        if(thisPlayersCurrentData.unlockedLeve2 && thisPlayersCurrentData.unlockedLeve3 && thisPlayersCurrentData.unlockedLeve4){
            thisPlayersCurrentData.unlockedBossFight = true;
        }
    }

    public void UpdateShopDisplay(){
        //Set main pre-round menu bars & texts to compare to max stats the player can reach in this game
        PlayerTotalGameDays.text = "Day " + thisGamesCurrentManagerScript.ReturnPlayerStats().onDayNumber.ToString();
        PlayerTotalGameGold.text = thisGamesCurrentManagerScript.ReturnPlayerStats().playerCoins.ToString();
        hpMaxUpgradeMeter.fillAmount = (float)thisGamesCurrentManagerScript.ReturnPlayerStats().playerMaxHealth / (float)thisGamesCurrentManagerScript.ReturnMaxHealthStasThisGameCanHandle();
        staminaMaxUpgradeMeter.fillAmount = thisGamesCurrentManagerScript.ReturnPlayerStats().playerMaxStamina / thisGamesCurrentManagerScript.ReturnMaxStaminaStasThisGameCanHandle();
        attackMaxUpgradeMeter.fillAmount = (float)thisGamesCurrentManagerScript.ReturnPlayerStats().playerAttackPoints / (float)thisGamesCurrentManagerScript.ReturnMaxAttackStasThisGameCanHandle();
        PlayerCurrentHp.text = thisPlayersCurrentData.playerMaxHealth.ToString();
        PlayerCurrentSp.text = thisPlayersCurrentData.playerMaxStamina.ToString();
        PlayerCurrentAtck.text = thisPlayersCurrentData.playerAttackPoints.ToString();

        //Display current shop level / sellcost & cost to upgrade
        UpgradeStoreCostTxt.text = thisGamesCurrentShopDataInstance.shopLevelUpCost.ToString() + "c";
        StoreCurrentLevelNMultiplierTxt.text = "Level: " +thisGamesCurrentShopDataInstance.shopLevelAt.ToString() 
        + " | " +thisGamesCurrentShopDataInstance.playerShopDrinkSellAmmount.ToString("F2");

        //Display current player stats / max possible stats in the game
        HPCostTxt.text = thisGamesCurrentShopDataInstance.shopPlayerHPUpCost.ToString() + "c";
        StaminaCostTxt.text = thisGamesCurrentShopDataInstance.shopPlayerStaminaUpCost.ToString() + "c";
        AttackCostTxt.text = thisGamesCurrentShopDataInstance.shopPlayerAttackUpCost.ToString() + "c";
        world2CostTxt.text = "Buy: " + thisGamesCurrentShopDataInstance.world2Cost.ToString() + "c";
        world3CostTxt.text = "Buy: " + thisGamesCurrentShopDataInstance.world3Cost.ToString() + "c";
        world4CostTxt.text = "Buy: " + thisGamesCurrentShopDataInstance.world4Cost.ToString() + "c";
    }

    public void playerTriesToUpgradeShop(){
        //BOC below checks if the player overall coins is > than the current cost of the shop upgrade
        //If true, does action
        if(thisPlayersCurrentData.playerCoins > thisGamesCurrentShopDataInstance.shopLevelUpCost){
            thisPlayersCurrentData.playerCoins -= thisGamesCurrentShopDataInstance.shopLevelUpCost;
            thisGamesCurrentShopDataInstance.shopLevelUpCost += 10;
            thisGamesCurrentShopDataInstance.playerShopDrinkSellAmmount += 0.10f;
            thisGamesCurrentShopDataInstance.shopLevelAt += 1;
            UpdateNSaveShop();
        }else{
            PlayerIsBroke();
        }
    }

    public void playerTriesToUpgradeHP(){
        //BOC below checks if the player overall coins is > than the current cost of the HP upgrade
        //If true, does action. Player hp icreases by 1 each buy but +10 cost (10th buy = 1000 coins but +1hp)
        if(thisPlayersCurrentData.playerCoins > thisGamesCurrentShopDataInstance.shopPlayerHPUpCost){
            thisPlayersCurrentData.playerCoins -= thisGamesCurrentShopDataInstance.shopPlayerHPUpCost;
            thisGamesCurrentShopDataInstance.shopPlayerHPUpCost += 10;
            thisPlayersCurrentData.playerMaxHealth += 1;
            UpdateNSaveShop();
        }else{
            PlayerIsBroke();
        }
    }

    public void playerTriesToUpgradeATTACK(){
        //BOC below checks if the player overall coins is > than the current cost of the ATTACK upgrade
        //If true, does action. Player hp icreases by 1 each buy but +100 cost (10th buy = 1000 coins but +1attck)
        if(thisPlayersCurrentData.playerCoins > thisGamesCurrentShopDataInstance.shopPlayerAttackUpCost){
            thisPlayersCurrentData.playerCoins -= thisGamesCurrentShopDataInstance.shopPlayerAttackUpCost;
            thisGamesCurrentShopDataInstance.shopPlayerAttackUpCost += 100;
            thisPlayersCurrentData.playerAttackPoints += 1;
            UpdateNSaveShop();
        }else{
            PlayerIsBroke();
        }
    }

    public void playerTriesToUpgradeSTAMINA(){
        //BOC below checks if the player overall coins is > than the current cost of the Stamina upgrade
        //If true, does action. Player hp icreases by 1 each buy but +100 cost (10th buy = 1000 coins but +1Stamina)
        if(thisPlayersCurrentData.playerCoins > thisGamesCurrentShopDataInstance.shopPlayerStaminaUpCost){
            thisPlayersCurrentData.playerCoins -= thisGamesCurrentShopDataInstance.shopPlayerStaminaUpCost;
            thisGamesCurrentShopDataInstance.shopPlayerStaminaUpCost += 100;
            thisPlayersCurrentData.playerMaxStamina += 1;
            UpdateNSaveShop();
        }else{
            PlayerIsBroke();
        }
    }

    //Player tries to buy world 2 - connect to the button on the page 2Pre-Game UI
    public void playerTriesToBuyWorld2(){
        //BOC below checks if the player overall coins is > than the current cost of the Stamina upgrade
        //If true, does action. Player hp icreases by 1 each buy but +100 cost (10th buy = 1000 coins but +1Stamina)
        if(thisPlayersCurrentData.playerCoins > thisGamesCurrentShopDataInstance.world2Cost){
            thisPlayersCurrentData.playerCoins -= thisGamesCurrentShopDataInstance.world2Cost;
            thisPlayersCurrentData.unlockedLeve2 = true;
            // NOTE: No need to UpdateNSaveShop() after level unlock calls because data saved
            // for level unlocks are in PlayerStats instead!;
            Level2Buy.SetActive(false);
            UpdateNSaveShop();
        }else{
            PlayerIsBroke(); 
        }
    }

    //Player tries to buy world 3 - connect to the button on the page 2Pre-Game UI
    public void playerTriesToBuyWorld3(){
        //BOC below checks if the player overall coins is > than the current cost of the Stamina upgrade
        //If true, does action. Player hp icreases by 1 each buy but +100 cost (10th buy = 1000 coins but +1Stamina)
        if(thisPlayersCurrentData.playerCoins > thisGamesCurrentShopDataInstance.world3Cost){
            thisPlayersCurrentData.playerCoins -= thisGamesCurrentShopDataInstance.world3Cost;
            thisPlayersCurrentData.unlockedLeve3 = true;
            // NOTE: No need to UpdateNSaveShop() after level unlock calls because data saved
            // for level unlocks are in PlayerStats instead!;
            Level3Buy.SetActive(false);
            UpdateNSaveShop();
        }else{
            PlayerIsBroke();
        }
    }

    //Player tries to buy world 3 - connect to the button on the page 2Pre-Game UI
    public void playerTriesToBuyWorld4(){
        //BOC below checks if the player overall coins is > than the current cost of the Stamina upgrade
        //If true, does action. Player hp icreases by 1 each buy but +100 cost (10th buy = 1000 coins but +1Stamina)
        if(thisPlayersCurrentData.playerCoins > thisGamesCurrentShopDataInstance.world4Cost){
            thisPlayersCurrentData.playerCoins -= thisGamesCurrentShopDataInstance.world4Cost;
            thisPlayersCurrentData.unlockedLeve4 = true;
            // NOTE: No need to UpdateNSaveShop() after level unlock calls because data saved
            // for level unlocks are in PlayerStats instead!;
            Level4Buy.SetActive(false);
            UpdateNSaveShop();
        }else{
            PlayerIsBroke();
        }
    }

    //BOC to make code cleaner to call the save shop from GameManagerScript after each buy.
    public void UpdateNSaveShop(){
        Instantiate(playShopBuySfx);
        thisGamesCurrentManagerScript.SaveShopAndPlayerDataAfterPurchase(thisGamesCurrentShopDataInstance, thisPlayersCurrentData);
        UpdateShopDisplay();
    }

    public void PlayerIsBroke(){
        Instantiate(playShopfailedSfx);
        //Debug.LogWarning("Player is broke!\n"
        //+ "Dev has yet to put a UI to show they cannot purchase this!");

        //FIXME: Instantiate the pop-up to tell the player is broke!
        Instantiate(NotEnoughMoneyAlert,CanvasMainMenu.transform);
    }
}
