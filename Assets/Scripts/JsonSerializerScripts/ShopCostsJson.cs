using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class ShopCostsNEarnings
{
    public int shopLevelAt { get; set; }
    public int shopLevelUpCost { get; set; }
    public int shopPlayerHPUpCost { get; set; }
    public int shopPlayerStaminaUpCost { get; set; }
    public int shopPlayerAttackUpCost { get; set; }
    public float playerShopDrinkSellAmmount { get; set; }
    public int world2Cost { get; set; }
    public int world3Cost { get; set; }
    public int world4Cost { get; set; }

    //World Game Expected Price for drinks (If player price < expected price, do player price! Otherwise, do [expected price / player price] chance)
    //[expected / player] price makes customers LEAVE if they want a drink with that flavor but the expected price is higher, decreasing customers that buy things.
    public double casavaToppingExpectedPrice { get; set; }
    public double pandanExpectedPrice { get; set; }
    public double bananaExpectedPrice { get; set; }
    public double strawberryExpectedPrice { get; set; }
    public double mangoExpectedPrice { get; set; }
    public double ubeExpectedPrice { get; set; }

    //Price for teas
    public double greenTeaExpectedPrice { get; set; }
    public double blackTeaExpectedPrice { get; set; }
    public double oolongTeaExpectedPrice { get; set; }

    // price for milk
    public double milkExpectedPrice { get; set; }

}