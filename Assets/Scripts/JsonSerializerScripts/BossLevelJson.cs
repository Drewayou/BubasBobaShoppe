[System.Serializable]
public class CassavaBossLevelJson
{
    //WorldName
    public string WorldName = "PandanForest";

    //Note: Although every round ALWAYS has 5 possible spawner spawn points, 1 will ALLWAYS be allocated to a slime cave and the other to the clogged ley geyser
    public int SpawnersAlive = 3;

    //Used in the Level
    public int LevelDifficulty = 1;

    //Below is the chance of a spawner appearing in the level. Measured from 0 -> 1 OUT OF ALL VARIABLES in terms of floats. 
    /// <summary>
    /// (i.e. if cassava slime has a 12% chance, Mango has 78%, and everything else has 0%)
    /// 
    /// World 1 starts at 1 for Slime and 0 for everything else at first.
    /// </summary>
    public float changeOfSlime = 1;
    public float changeOfPandan = 0;
    public float changeOfBanana = 0;
    public float changeOfStrawberry = 0;
    public float changeOfMango = 0;
    public float changeOfUbe = 0;
}