using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Controller : MonoBehaviour
{
    private float agentDrift = 0.0001f; // minimal drift to keep 2d Navmesh bug from happening

    [Tooltip ("What object to follow, in this case the player")]
    [SerializeField]
    public GameObject Target;

    [Tooltip ("What Agro radius to follow, in this case the childobject")]
    [SerializeField]
    public GameObject AgroRadius;

    [Tooltip ("What agrescript to call, in this case, in the childobject AgroRadius")]
    [SerializeField]
    public Enemy_AgroRadius thisEnemyAgroRange;

    private Health_Universal thisEnemyHealthManager;

    [Tooltip ("Get enemy Val of this Enemy")]
    [SerializeField]
    public int whatEnemyValIsThis;

    //Trigger collider for animation to change
    //private CircleCollider2D attackRadius;

    //Trigger collider radius
    [Header("Attack radius")]
    [Tooltip("Trigger collider for attack radius in float")]
    public float attackRadiusfloat = 0.25f;

    [Header("Movement Variables")]
    [Tooltip("The speed at which this enemy will move.")]
    public float moveSpeed = 2.0f;

    [Header("AttackCooldown Variables")]
    [Tooltip("The speed at which this enemy attempt another attack.")]
    public float attackSpeed = 2.0f;

    [Header("AttackWindup Variables")]
    [Tooltip("The speed at which this enemy will wind up an attack.")]
    public float attackBuildUp = 0.5f;

    //Timer variable to count down till next attack
    public float nextAttackIn = 0f;

    //Added a navmesh agent thanks to some smart people that found out how to use it in 2D games
    [SerializeField]
    [Tooltip("The Enemy Navmesh Agent for this object")]
    public NavMeshAgent thisAgent;

    //More variables for this script

    public bool isFreshlySpawned = true, isCurrentlyAttacking = false, amIMoving = false;

    //Direction this enemy moves
    public bool movementDirectionEast;

    // Vector to know where to go for Navmesh
    private Vector3 ObjectToFollowPosition;

    // Temp Vector to know where to go for Navmesh attack
    private Vector3 playerLastSeenPosition;

    //The current position of this enemy
    private Vector2 enemyCurrentPosition;

    // Start is called before the first frame update
    void Start()
    {
        //Find Agro Radius Prefab Script Object
        //AgroRadius = GameObject(AgroRadius);
    
        //Set up player target, find their instance on the scene
        Target = GameObject.Find("Player");
        setUpEnemySettings();
    }

    // Update is called once per frame
    void Update()
    {
        if(AmIMoving()){
        directionOfMovement();
        amIMoving = AmIMoving();
        }else{
        amIMoving = AmIMoving();
        }
    }

    //Set up Enemy settings
    private void setUpEnemySettings(){

        //Set up agent settings
        thisAgent = GetComponent<NavMeshAgent>();
		thisAgent.updateRotation = false;
		thisAgent.updateUpAxis = false;
        thisAgent.speed = moveSpeed;
        SetDestinationTweaked(getPlayerLastSeenLocation());
        thisAgent.isStopped = true;

        //Set up agro radius
        AgroRadius = GetComponent<GameObject>();
        //thisEnemyAgroRange = AgroRadius.GetComponentInChildren<Enemy_AgroRadius>();

        //Set up health
        thisEnemyHealthManager = GetComponent<Health_Universal>();
        
    }

    /// <summary>
    /// This is the main BOC that allows the enemy to move
    /// </summary>
    /// 
    

    //Set the current position of this Enemy
    public void getThisEnemyLocation(){
        enemyCurrentPosition = new Vector2(transform.position.x, transform.position.y);
    }

    //Set the last SEEN position of this Enemy (Useful for when out of range/attacks)
    public Vector2 getPlayerLastSeenLocation(){
        return playerLastSeenPosition = new Vector2(Target.transform.position.x, Target.transform.position.y);
    }

    //BOC to find where the player is to follow
    public void getLocationOfObjectToFollow(){
        ObjectToFollowPosition = new Vector2 (Target.transform.position.x, Target.transform.position.y);
    }

    public void setLocationOfObjectToFollow(GameObject targetsLocation){
        playerLastSeenPosition = new Vector2 (targetsLocation.transform.position.x, targetsLocation.transform.position.y);
    }

    //BOC used to minorly adjust the player location on the X-axis so the 2d Navmesh bug doesn't stop it
    public void SetDestinationTweaked(Vector3 targetToBeTweaked)
    {   
        //if(!thisEnemyHealthManager.amIDeadYet){
		if(Mathf.Abs(enemyCurrentPosition.x - targetToBeTweaked.x) < agentDrift)
        playerLastSeenPosition = targetToBeTweaked + new Vector3(agentDrift, 0f, 0f);
        

        if(thisAgent.enabled){
            
        thisAgent.SetDestination(playerLastSeenPosition);
        }
        
    }

    //Public methods below for other scripts to pull values of this controller
    
    //Check direction this enemy is moving
    public void directionOfMovement(){
        
        if(thisAgent.velocity.x > 0){
            if(transform.localScale.x == -1){
                    Vector3 FlipVector = new Vector3(1, 1, 1);
                    transform.localScale = FlipVector;
            }
            movementDirectionEast = true;
            //Debug.Log("Velocity vector is facing East!");

        }else{

        if(thisAgent.velocity.x < 0){
            if(transform.localScale.x == 1){
                    Vector3 FlipVector = new Vector3(-1, 1, 1);
                    transform.localScale = FlipVector;
            }
        }
            movementDirectionEast = false;
            //Debug.Log("Velocity vector is facing West!");

        }
    }

    /// <summary>
    /// Below is an editable method for each enemy on how they do their attacks
    /// </summary>

    //Public method to Check if this enemy is moving, used by Enemy_Anim_Changer script;
    public bool AmIMoving(){
        Vector3 notMoving = new Vector3(0,0,0);
        if(thisAgent.velocity!=notMoving){
            //Debug.Log("Velocity vector is moving!");
            return true;
        }else{
            //Debug.Log("Velocity vector is still!");
            return false;
        }
    }

    public void SetIsNotFreshlySpawned(){
        isFreshlySpawned = false;
    }

    public float ReturnAttackTime(){
        return nextAttackIn;
    }

    public Vector3 GetPlayerLastSeenPosition(){
        return playerLastSeenPosition;
    }
}
