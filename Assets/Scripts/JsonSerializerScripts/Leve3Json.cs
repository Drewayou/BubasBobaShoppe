[System.Serializable]
public class World3Stats
{
    //WorldName
    public string WorldName = "MarshyMellows";

    //Note: Although every round ALWAYS has 5 possible spawner spawn points, 1 will ALLWAYS be allocated to a slime cave and the other to the clogged ley geyser
    public int SpawnersAlive = 3;

    //Used in the Level
    public int LevelDifficulty = 1;

    //Below is the chance of a spawner appearing in the level. Measured from 0 -> 1 OUT OF ALL VARIABLES in terms of floats. 
    /// <summary>
    /// (i.e. if cassava slime has a 12% chance, Mango has 78%, and everything else has 0%)
    /// 
    /// World 3 stats.
    /// </summary>
    public float changeOfSlime = .02f;
    public float changeOfPandan = .05f;
    public float changeOfBanana = .05f;
    public float changeOfStrawberry = .48f;
    public float changeOfMango = .40f;
    public float changeOfUbe = 0f;
}