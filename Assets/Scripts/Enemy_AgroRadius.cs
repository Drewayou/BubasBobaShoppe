using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AgroRadius : MonoBehaviour
{

    Enemy_Controller thisEnemyController;

    Enemy_AttackRange thisEnemyAttackRange;

    //Aggro Radius itself
    CircleCollider2D aggroRadius;

    //Bool to use for other script to check if there's agro collision
    public bool isIndeedAgro = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(isIndeedAgro ){
            GameObject targetNewLocation = thisEnemyController.Target;
            thisEnemyController.setLocationOfObjectToFollow(targetNewLocation);
            thisEnemyController.SetDestinationTweaked(thisEnemyController.getPlayerLastSeenLocation());
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
        Debug.Log("PlayerInAgroRangeOfSlime!");

        thisEnemyController.isFreshlySpawned = false;
        isIndeedAgro = true;
        thisEnemyController.agent.isStopped = false;
        thisEnemyController.SetDestinationTweaked(thisEnemyController.getPlayerLastSeenLocation());

    }

    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        //Debug.Log("PlayerLeftAgroRangeOfSlime!");
        isIndeedAgro = false;

    }

    public bool CheckIfAgro(){
        return isIndeedAgro;
    }
}
