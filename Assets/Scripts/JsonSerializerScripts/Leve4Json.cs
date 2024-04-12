[System.Serializable]
public class World4Stats
{
    //WorldName
    public string WorldName = "CrystalingLeyCaves";

    //Note: Although every round ALWAYS has 5 possible spawner spawn points, 1 will ALLWAYS be allocated to a slime cave and the other to the clogged ley geyser
    public int SpawnersAlive = 3;

    //Used in the Level
    public int LevelDifficulty = 1;

    //Below is the chance of a spawner appearing in the level. Measured from 0 -> 1 OUT OF ALL VARIABLES in terms of floats. 
    /// <summary>
    /// (i.e. if cassava slime has a 12% chance, Mango has 78%, and everything else has 0%)
    /// 
    /// World 4 stats.
    /// </summary>
    public float changeOfSlime = 0.08f;
    public float changeOfPandan = 0.01f;
    public float changeOfBanana = 0.01f;
    public float changeOfStrawberry = 0.30f;
    public float changeOfMango = 0.30f;
    public float changeOfUbe = 0.30f;
}