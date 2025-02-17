using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class PlayerDataJson 
{
    public bool newGame { get; set; }
    public float playerCoins { get; set; }
    public int onDayNumber { get; set; }
    public int playerMaxHealth { get; set; }
    public float playerMaxStamina { get; set; }
    public int playerAttackPoints { get; set; }
    public bool unlockedLeve2 { get; set; }
    public bool unlockedLeve3 { get; set; }
    public bool unlockedLeve4 { get; set; }
    public bool unlockedBossFight { get; set; }

    //Player dropable monster items data
    public int casavaBalls { get; set; }
    public int pandanLeaves { get; set; }
    public int bananas { get; set; }
    public int strawberries { get; set; }
    public int mangos { get; set; }
    public int ube { get; set; }

    //Player boba shop items data
    public int greenTeaAmmount { get; set; }
    public int oolongTeaAmmount { get; set; }
    public int blackTeaAmmount { get; set; }
    public int sugarAmmount { get; set; }
    public int milkAmmount { get; set; }

    //Player BOBA shop save data. 

    //Shop BOOLS for unlocks in the shop (Story / npc shop based)
    // This data value dictates if the player has the black tea unlocked.
    public bool blackTeaUnlocked { get; set; }

    // This data value dictates if the player has the oolong tea unlocked.
    public bool oolongTeaUnlocked { get; set; }

    // This data value dictates if the player has the Milk unlocked.
    public bool milkUnlocked { get; set; }

    // This data value dictates if the player has the sugar unlocked.
    public bool sugarModifierUnlocked { get; set; }

    // This data value dictates if the player has the drink temperature modifier unlocked.
    public bool tempModifierUnlocked { get; set; }

    //Note : Things/Costs that the player can BUY via NPC SHOPS are located/pulled from "ShopCosts.Json"!
    public int shopTraysAvailable { get; set; }

    // This data value dictates how many cupholders the player can use/have in their shop in the RIGHT side.
    public int shopCupHoldersAvailableRightSide { get; set; }

    // This data value dictates how many cupholders the player can use/have in their shop in the LEFT side.
    public int shopCupHoldersAvailableLeftSide { get; set; }

    // This data value dictates how many topping jars the player can use/have in their shop.
    public int shopToppingJarsAvailable { get; set; }

    // This data value dictates how many mixers the player can use/have in their shop.
    public int mixerAvailable { get; set; }

    // This data value is important for setting the popularity of the 
    public float shopPopularity { get; set; }

    // This data list is for the items selected to be in the trays of the shop. This is usually set before a boba shop round.
    public List<int> shopTraysItemListArray { get; set; }

    // This data value dictates the speed the boba pot cooks.
    public float bobaShopBobaPotCookingSpeed { get; set; }

    // This data value dictates the max ammount of items that can be put in a boba pot.
    public int maxCapacityOfBobaPot { get; set; }

    // This data value dictates the max size of the topping jar. 
    public int maxCapacityOfToppingJars { get; set; }

    // This data value dictates the max size of the GeenTea jug. 
    public int maxCapacityOfGeenTeaJug { get; set; }

    // This data value dictates the max size of the BlackTea jug. 
    public int maxCapacityOfBlackTeaJug { get; set; }

    // This data value dictates the max size of the OolongTea jug. 
    public int maxCapacityOfOolongTeaJug { get; set; }

    // This data value dictates the max size of the Milk jug. 
    public int maxCapacityOfMilkJug { get; set; }

    // This data value dictates the max size of the Sugar jar. 
    public int maxCapacityOfSugarJar { get; set; }

    // This data value dictates how many "uses" the ladle has. Uses equates to each item it can carry. I.E. the ladle picking up 5 items will 
    public int maxCleanCountOfLadle { get; set; }

    // This data value dictates the max value the ladle can carry.
    public int maxCarrySizeOfLadle { get; set; }

    // This data value dictates how long it takes for the mixer to mix.
    public float speedOfMixer { get; set; }

    // This data value dictates how often the mixer can break.
    public int mixerFailureRate { get; set; }

    // This data value dictates how many cusomters the player can deal with at a time during a boba shop round. Maximum of 6 (3 waiting to order and 3 already ordered).
    // Customer Queue max size is dictated by the ammount of tables/chairs the player has bought & boba shop popularity. Again, This INCLUDES customers waiting for their order to be done.
    public int maxBobaShopLineQueue { get; set; }

}