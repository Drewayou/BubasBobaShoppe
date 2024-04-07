using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Anim_Changer : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The Enemy Animator for this object")]
    [Header("Spawn Effect")]
    GameObject spawnInLevelEffect;

    [SerializeField]
    [Tooltip("The Enemy Animator for this object")]
    public Animator animatorThisObject;

    [Tooltip("The Current Enemy Animator in Stringform")]
    string currentAnimationThisObject;

    [SerializeField]
    [Tooltip("The Enemy Controller for when they're about to move")]
    public Enemy_Controller thisEnemyController;

    [SerializeField]
    [Tooltip("The Enemy Controller for when they're about to attack")]
    public Enemy_AttackRange thisAttackController;

    [Tooltip("The Rigidbody2D component to use for collision detection.")]
    public Rigidbody2D thisRigidbody = null;

    [Header("Movement Variables")]
    [Tooltip("The speed at which this enemy will move.")]
    public float moveSpeed = 2.0f;

    //Added a navmesh agent thanks to some smart people that found out how to use it in 2D games
    [SerializeField]
    [Tooltip("The Enemy Navmesh Agent for this object")]
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        thisAttackController = GetComponentInChildren<Enemy_AttackRange>();
        
        Instantiate (spawnInLevelEffect, transform.position, transform.rotation, transform);
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateAnimation Depending on movement
        changeAnimation(thisEnemyController.AmIMoving());
    }

    public void changeAnimation(bool isThisMoving){

        //BOC below is for when enemy decided to attack, and animates it
        if(thisAttackController.CheckIfAttacking()){
            if(thisEnemyController.movementDirectionEast){
                    //FlipVectorCharacterAttackEast
                    if(transform.localScale.x == -1){
                    Vector3 FlipVector = new Vector3(1, 1, 1);
                    transform.localScale = FlipVector;
                }
            }else{
                    //FlipVectorCharacterAttackWest
                    if(transform.localScale.x == -1){
                    Vector3 FlipVector = new Vector3(-1, 1, 1);
                    transform.localScale = FlipVector;
                }

            }
            if(currentAnimationThisObject != "CassavaSlimeAttack"){
                animatorThisObject.Play("CassavaSlimeAttack");
                currentAnimationThisObject = "CassavaSlimeAttack";
                thisEnemyController.isCurrentlyAttacking = true;
        }
        }else{ 

            if(isThisMoving && currentAnimationThisObject != "CassavaSlimeAboutToMove"){
                animatorThisObject.Play("CassavaSlimeAboutToMove");
                currentAnimationThisObject = "CassavaSlimeAboutToMove";
            }

            else{ 
                if(isThisMoving != true && currentAnimationThisObject != "CassavaSlimeJiggleEat"){
                animatorThisObject.Play("CassavaSlimeJiggleEat");
                currentAnimationThisObject = "CassavaSlimeJiggleEat";
                }
            }
        }
    }
}
