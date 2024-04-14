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

}