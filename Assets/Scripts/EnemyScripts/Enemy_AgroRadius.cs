using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Extensions;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_AgroRadius : MonoBehaviour
{
    Enemy_Controller thisEnemyController;

    [SerializeField]
    [Tooltip("Place in the attackrange prefab for this enemy")]
    [Header("This Attack Range")]
    Enemy_AttackRange thisEnemyAttackRange;

    //Aggro Radius itself
    CircleCollider2D aggroRadius;

    [SerializeField]
    [Tooltip("The Agro Alert to be summoned when player is in agro range")]
    public GameObject aggroPrefab;

    [SerializeField]
    [Tooltip("The Question Alert to be summoned when player leaves agro range")]
    public GameObject questionPrefab;

    [SerializeField]
    [Tooltip("Place in the Health_Universal prefab for this enemy FROM the parent")]
    [Header("Healthy?")]
    Health_Universal thisEnemyHealthManager;

    //Round manager to set diffiulty of things
    RoundManagerScript thisRoundManager;

    //Pull level navmesh for patrol purposes
    public NavMeshData levelNavMesh;

    //Bool to use for other script to check if there's agro collision
    public bool isIndeedAgro = false;
    public bool playerIsInAgroRange = false;

    //Below will be used by the "Enemy_Despawn" script if they have not interacted with the player within agro range in 20 sec.
    public bool hasInteractedWithPlayer = false;

    [SerializeField]
    [Tooltip("Is the enemy ready to patrol in it's agro radius?")]
    [Header("Patroling")]
    public bool randomPatrol = false;

    [SerializeField]
    [Tooltip("A random point in the enemy agro radius they will patrol to")]
    [Header("Patrol Random Point")]
    private Vector2 randomPatrolPoint;

    [SerializeField]
    [Tooltip("How much MORE BIGGER than the AGGRO RADIUS do you want the patrol radius to fire? This will ADD to the Patrol Radius")]
    [Header("Patrol Radius Multiplier")]
    public float desiredMultiplicityPatrolRadius = 1;

    [SerializeField]
    [Tooltip("How long until the enemy would wander to a new point in it's agro range")]
    [Header("Patrol Timer")]
    public float enemyFidgetTimerCountdown = 15f;

    public float enemyFidgetTimer;

    //Trigger collider radius
    [Header("Agro radius")]
    [Tooltip("Trigger collider for agro radius in float")]
    public float agroRadiusfloat = 2.0f;

    //FOR USE OF HIGHER LEVEL DIFFICULTY.
    //On lower levels, the chance that the enemy will always be following the player is low... Visa versa as the game progresses!
    [Header("Locked On Player?")]
    [Tooltip("Enemy will follow and attack the player no matter how far!")]
    public bool LockedOnPlayer = false;

    //FIXME: Use level difficulty set by JSON files FOR USE OF HIGHER LEVEL DIFFICULTY. DO NOT set above 25%!!!! Too many navAI's slow down the game performance!
    //On lower levels, the chance that the enemy will always be following the player is low... Visa versa as the game progresses! This is the value (% * 100 already done)
    public float lockedOnDifficultyPercentage;

    // Start is called before the first frame update
    void Start()
    {
        thisRoundManager = GameObject.Find("RoundManager").GetComponent<RoundManagerScript>();

        //Set up agro radius
        aggroRadius = GetComponent<CircleCollider2D>();
        aggroRadius.radius = agroRadiusfloat;

        //Set enemy controller
        thisEnemyController = GetComponentInParent<Enemy_Controller>();


        //set possibity of this enemy to forever be agro to player
        lockedOnDifficultyPercentage = thisRoundManager.ProbabilityOfPerpertualPlayerAgro();
    }

    // Update is called once per frame
    void Update()
    {
        if(LockedOnPlayer){
            isIndeedAgro = true;
        }
        if(isIndeedAgro && !thisEnemyAttackRange.isAttacking){
            thisEnemyController.thisAgent.isStopped = false;
            GameObject targetNewLocation = thisEnemyController.Target;
            thisEnemyController.setLocationOfObjectToFollow(targetNewLocation);
            thisEnemyController.SetDestinationTweaked(thisEnemyController.getPlayerLastSeenLocation());
            
        }else{
            if(!playerIsInAgroRange && !thisEnemyAttackRange.isAttacking){
            //Note:UncommentingBelowCausesTheEnemyToStopOutOfRangeAndDoesNotCheckThe
            //Player'sLastLocation
            //thisEnemyController.agent.isStopped = true;
            }
        }

        //Below BOC is for the enemies to patrol or go to player after a set time. The patrol they will go is set to agroradius * desiredMultiplicityPatrolRadius
        if(randomPatrol && !isIndeedAgro && !playerIsInAgroRange && !thisEnemyAttackRange.isAttacking){

            //Attempt to perform a patrol
            float desiredPatrolRadius = aggroRadius.radius + desiredMultiplicityPatrolRadius;
            randomPatrolPoint = (Vector2)transform.position + (Random.insideUnitCircle * desiredPatrolRadius);
            if(checkSpawnPointOnNavMesh(randomPatrolPoint)){

                thisEnemyController.SetIsNotFreshlySpawned();
                thisEnemyController.thisAgent.isStopped = false;
                thisEnemyController.thisAgent.SetDestination(randomPatrolPoint);
                randomPatrol = false;
                enemyFidgetTimer = enemyFidgetTimerCountdown;

                //Attempt to perpetually lock on the player
                tryForeverAgro();
            }
        }
        else{FidgetyTimer();}
    }

    public void FidgetyTimer(){
        if(enemyFidgetTimer>0){
        enemyFidgetTimer -= Time.deltaTime;
        }else{
            randomPatrol = true;
        }
    }

    //Below BOC makes it so that by set chance (out of 100%) depending on level difficulty (lockedOnDifficulty), the enemy weill perpetually hunt the player
    public void tryForeverAgro(){
        if(UnityEngine.Random.Range(1,100)<=lockedOnDifficultyPercentage){
            LockedOnPlayer = true;
            hasInteractedWithPlayer = true;
        }
    }

    private void OnTriggerStay2D(UnityEngine.Collider2D collision){
        if(collision.CompareTag("Player")){
            GameObject targetNewLocation = thisEnemyController.Target;
            thisEnemyController.setLocationOfObjectToFollow(targetNewLocation);
            thisEnemyController.SetDestinationTweaked(thisEnemyController.getPlayerLastSeenLocation());
        }
    }

    //Code block below to check if this enemy is in range for attacking
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        //Debug.Log("PlayerInAgroRangeOfSlime!");

        if(thisEnemyController.thisAgent!=null){
            if(collision.CompareTag("Player")){
                thisEnemyController.isFreshlySpawned = false;
                playerIsInAgroRange = true;
                isIndeedAgro = true;
                hasInteractedWithPlayer = true;
                thisEnemyController.thisAgent.isStopped = false;
                thisEnemyController.SetDestinationTweaked(thisEnemyController.getPlayerLastSeenLocation());

                //Popup Agro Alert
                Vector2 spawnAboveEnemy = new Vector2 (transform.position.x, transform.position.y + .22f);
                Instantiate(aggroPrefab, spawnAboveEnemy, transform.rotation, transform);
            }
        }

    }

    //Check if a point selected is on the navmesh
    bool checkSpawnPointOnNavMesh(Vector3 randomPointSelected)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPointSelected, out hit, aggroRadius.radius, NavMesh.AllAreas))
        {
            return true;
        }
        return false;
    }

    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        //Debug.Log("PlayerLeftAgroRangeOfSlime!");
        
        if(thisEnemyController.thisAgent!=null){
            if(collision.CompareTag("Player")){
            isIndeedAgro = false;
        
               
                playerIsInAgroRange = false;
                thisEnemyController.SetDestinationTweaked(thisEnemyController.getPlayerLastSeenLocation());
                
                //Popup question Alert
                Vector2 spawnAboveEnemy = new Vector2 (transform.position.x, transform.position.y + .22f);

                //Make the question alert effect
                Instantiate(questionPrefab, spawnAboveEnemy, transform.rotation, transform);
            }
        }
        

    }

    public bool HasInteractedWithPlayer(){
        return hasInteractedWithPlayer;
    }

    public bool CheckIfAgro(){
        return isIndeedAgro;
    }
    
}
