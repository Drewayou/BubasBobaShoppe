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
}