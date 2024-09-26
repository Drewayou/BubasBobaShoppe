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

    //Player dropable items data
    public int casavaBalls { get; set; }
    public int pandanLeaves { get; set; }
    public int bananas { get; set; }
    public int strawberries { get; set; }
    public int mangos { get; set; }
    public int ube { get; set; }

    //Player shop save data. May need to move to Shop JSON in the future?
    public int shopTraysAvailable { get; set; }
    public int shopCupHoldersAvailable { get; set; }
    public int mixerAvailable { get; set; }
    public float shopPopularity { get; set; }

    public List<int> shopTraysItemListArray { get; set; }

    public float bobaShopBobaPotCookingSpeed { get; set; }

    public int maxCapacityOfBobaPot { get; set; }

    public int maxCapacityOfToppingJars { get; set; }

    public int maxCleanCountOfLadle { get; set; }

    public int maxCarrySizeOfLadle { get; set; }

    public float speedOfMixer { get; set; }

    public int mixerFailureRate { get; set; }

}