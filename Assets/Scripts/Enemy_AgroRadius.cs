using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Extensions;
using UnityEngine;

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
    [Tooltip("Place in the Health_Universal prefab for this enemy FROM the parent")]
    [Header("Healthy?")]
    Health_Universal thisEnemyHealthManager;

    //Bool to use for other script to check if there's agro collision
    public bool isIndeedAgro = false;
    public bool playerIsInAgroRange = false;

    //Trigger collider radius
    [Header("Agro radius")]
    [Tooltip("Trigger collider for agro radius in float")]
    public float agroRadiusfloat = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Set up agro radius
        aggroRadius = GetComponent<CircleCollider2D>();
        aggroRadius.radius = agroRadiusfloat;

        thisEnemyController = GetComponentInParent<Enemy_Controller>();
        //thisEnemyHealthManager = GetComponentInParent<Health_Universal>();
        //thisEnemyAttackRange = GetComponent<Enemy_AttackRange>();
    }

    // Update is called once per frame
    void Update()
    {
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
        thisEnemyController.thisAgent.isStopped = false;
        thisEnemyController.SetDestinationTweaked(thisEnemyController.getPlayerLastSeenLocation());
        }
        }

    }

    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        //Debug.Log("PlayerLeftAgroRangeOfSlime!");
        
        if(thisEnemyController.thisAgent!=null){
            if(collision.CompareTag("Player")){
            isIndeedAgro = false;
        
               
                playerIsInAgroRange = false;
                thisEnemyController.SetDestinationTweaked(thisEnemyController.getPlayerLastSeenLocation());
                
            }
        }
        

    }

    public bool CheckIfAgro(){
        return isIndeedAgro;
    }
}
