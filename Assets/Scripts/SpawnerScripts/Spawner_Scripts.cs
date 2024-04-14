using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.AI;

public class Spawner_Scripts : MonoBehaviour
{
    //Where to put clones of enemy prefabs
    [SerializeField]
    [Header("EnemiesOfLevel")]
    [Tooltip("Where to put gameobject in hirarchy for level")]
    public GameObject enemiesInLevel;

    // Drag a pre-fab to spawn this selected monster
    [SerializeField]
    [Header("FruitMonsterToSpawn")]
    [Tooltip("This is the pre-fab enemy that this spawner will spawn")]
    public GameObject monsterToSpawn;

    // Spawn Radius set for this Pre-fab
    [Header("SpawnRadiusCollider")]
    [Tooltip("Used to check if the player is too close")]
    public CircleCollider2D enemySpawnRadiusCollider;

    // Spawn Radius set for this Pre-fab
    [Header("SpawnRadius")]
    [Tooltip("This is the area the spawner will spawn enemies, put into the spawn collider & trySpawn()")]
    float enemySpawnRadius;

    // Spawn Radius set for this Pre-fab
    [Header("NavMesh")]
    [Tooltip("Put in global navmesh to find spawn points for monsters")]
    public NavMeshData levelNavMesh;

    private Animator thisSpawnerAnimator;

    private Spawner_Player_Sensor attatchedPlayerSensor;

    // This round's level setting
    private RoundManagerScript thisRoundsSettings;


/// <summary>
/// Use below settings to set via other game objects / Game manager object
/// </summary>
/// 

// FIXME:
    //Below needs to be added after the round manager object is made
    //int spawnerLevel = 1, maxMonsterAmmountCap = 20;

    /*
    [Header("RoundManager")]
    [Tooltip("Used to check CurrentGameSettings")]
    GameObject currentRound;
    */

    // Timer to set for spawner cooldown
    [Header("Spawner Cooldown")]
    [Tooltip("Used reset spawn timer each time a new enemy spawns")]
    public float coolDownPerNewSpawn;

    public float timeleftToSpawnNextEnemy;

    Vector2 spawnPointEnemeyHere;

    private bool playerNearSpawner = false, canSpawnEnemyTimerDone = true, boostedSpawnRate = false;

    // Start is called before the first frame update
    void Start()
    {

        enemiesInLevel = GameObject.Find("Enemies");
        //Set spawn radius to collider radius
        enemySpawnRadius = enemySpawnRadiusCollider.radius;

        //Get this spawner animator
        thisSpawnerAnimator = GetComponent<Animator>();
        thisSpawnerAnimator.Play("SpawnerReactivate");

        attatchedPlayerSensor = GetComponentInChildren<Spawner_Player_Sensor>();

        //Get the round manager to set the settings of this spawner
        thisRoundsSettings = GameObject.Find("RoundManager").GetComponent<RoundManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //Count down timer to next spawn of this spawner
        tickDownSpawnTimer();

        //Spawn monsters if this spawner can
        if(!IsLevelOverPopulated() && !playerNearSpawner && canSpawnEnemyTimerDone){
            //Debug.LogWarning("Spawning Slime");
            trySpawn();
        }

        if(attatchedPlayerSensor.playerInSensor){
        boostedSpawnRate = true;
        }else{
        boostedSpawnRate = false;
        }

    }

    public bool IsLevelOverPopulated(){
        if(thisRoundsSettings.CheckIfSpawnCapIsReached()){
            return true;
        }else{
            return false;
        }
    }

    void tickDownSpawnTimer(){
        if(timeleftToSpawnNextEnemy>=0){
            timeleftToSpawnNextEnemy -= Time.deltaTime;
        }else{
            canSpawnEnemyTimerDone = true;
        }
    }

    void trySpawn(){
        
        spawnPointEnemeyHere = (Vector2)transform.position + (Random.insideUnitCircle * enemySpawnRadius);

        if(checkSpawnPointOnNavMesh(spawnPointEnemeyHere)){
            Vector3 spawnPointSet = new Vector3(spawnPointEnemeyHere.x,spawnPointEnemeyHere.y,transform.position.z);
            Instantiate (monsterToSpawn,spawnPointSet,transform.rotation,enemiesInLevel.transform);

            resetSpawnTimer();
            canSpawnEnemyTimerDone = false;

            thisRoundsSettings.EnemySpawned();
        };

    }

    public void resetSpawnTimer(){
        if(boostedSpawnRate){
            float tempNewCooldown = coolDownPerNewSpawn - 3;
            timeleftToSpawnNextEnemy = tempNewCooldown;
            }else{
                timeleftToSpawnNextEnemy = coolDownPerNewSpawn;
            }
    }

    /* FIXME:
    public void checkLevelPopulation(){

    */

    //Below code was pulled & edited from unity documentation "AI.NavMesh.SamplePosition.html"
    //This supposedly checks if a point is on the navmesh within a range and returns a bool.
    bool checkSpawnPointOnNavMesh(Vector3 randomPointSelected)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPointSelected, out hit, enemySpawnRadius, NavMesh.AllAreas))
        {
            return true;
        }
        return false;
    }

    public void setSpawnerDeactive(){

        //Deactivate spawner and play the animation
        playerNearSpawner = true;
        thisSpawnerAnimator.Play("SpawnerDeactivate");
        //Debug.LogWarning("Player near the spawner! Can't summon monster!");
    }

    public void setSpawnerActive(){

        //Reactivate spawner and play the animation
        thisSpawnerAnimator.Play("SpawnerReactivate");
        playerNearSpawner = false;
        //Debug.LogWarning("Player left the spawner! Monsters will be summoned!");
    }

    public float getSpawnerSpawnRadius(){
        return enemySpawnRadius;
    }
    
}
