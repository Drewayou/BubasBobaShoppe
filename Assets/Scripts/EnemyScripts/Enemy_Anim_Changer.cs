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
    [Tooltip("The Enemy object")]
    [Header("This Enemy Object")]
    GameObject enemyMainObject;

    Health_Universal thisEnemyHealthManager;

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

    [Header("NameOfEnemy")]
    [Tooltip("This is used to actuvate switch statements and co-responding animations")]
    public string whatEnemyIsThis = null;

    //Added a navmesh agent thanks to some smart people that found out how to use it in 2D games
    [SerializeField]
    [Tooltip("The Enemy Navmesh Agent for this object")]
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        thisAttackController = GetComponentInChildren<Enemy_AttackRange>();

        whatEnemyIsThis = gameObject.name;

        thisEnemyHealthManager = gameObject.GetComponent<Health_Universal>();

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
            
            //Switch for attack animations
            if(thisAttackController.CheckIfInAttackingAnimation()){

            switch(whatEnemyIsThis){

                case "CassavaSlime(Clone)":

                    if(currentAnimationThisObject != "CassavaSlimeAttack"){
                        animatorThisObject.Play("CassavaSlimeAttack");
                        currentAnimationThisObject = "CassavaSlimeAttack";
                        thisEnemyController.isCurrentlyAttacking = true;
                    }

                break;

                case "CassavaSlimeKnight(Clone)":

                    if(currentAnimationThisObject != "SlimeKnightPerpetualHunt"){
                        animatorThisObject.Play("SlimeKnightPerpetualHunt");
                        currentAnimationThisObject = "SlimeKnightPerpetualHunt";
                        thisEnemyController.isCurrentlyAttacking = false;
                    }

                break;

                case "CassavaSlimeKing":

                    if(!thisEnemyHealthManager.bossIsDead && currentAnimationThisObject != "CassavaSlimeKingSpawnsSlimes"){
                        animatorThisObject.Play("CassavaSlimeKingSpawnsSlimes");
                        currentAnimationThisObject = "CassavaSlimeKingSpawnsSlimes";
                        thisEnemyController.isCurrentlyAttacking = false;
                    }

                break;

                case "PandanShooter(Clone)":

                    if(currentAnimationThisObject != "PandanShooterShoots"){
                        animatorThisObject.Play("PandanShooterShoots");
                        currentAnimationThisObject = "PandanShooterShoots";
                        thisEnemyController.isCurrentlyAttacking = true;
                    }

                break;

                case "BananaShaman(Clone)":

                    if(currentAnimationThisObject != "BananaShamanShoots"){
                        animatorThisObject.Play("BananaShamanShoots");
                        currentAnimationThisObject = "BananaShamanShoots";
                        thisEnemyController.isCurrentlyAttacking = true;
                    }

                break;

                case "PricklyStrawberry(Clone)":

                    if(currentAnimationThisObject != "PricklyStrawberryAttacks"){
                        animatorThisObject.Play("PricklyStrawberryAttacks");
                        currentAnimationThisObject = "PricklyStrawberryAttacks";
                        thisEnemyController.isCurrentlyAttacking = true;
                    }

                break;

                case "MangoMauler(Clone)":

                    if(currentAnimationThisObject != "MangoMaulerAttack"){
                        animatorThisObject.Play("MangoMaulerAttack");
                        currentAnimationThisObject = "MangoMaulerAttack";
                        thisEnemyController.isCurrentlyAttacking = true;
                    }

                break;

                case "UnderGroundUbe(Clone)":

                    if(currentAnimationThisObject != "UnderGroundUbeAttacks"){
                        animatorThisObject.Play("UnderGroundUbeAttacks");
                        currentAnimationThisObject = "UnderGroundUbeAttacks";
                        thisEnemyController.isCurrentlyAttacking = true;
                    }

                break;
            }
            }

        }else{ 

            //Switch for movement animations

            switch(whatEnemyIsThis){

                case "CassavaSlime(Clone)":

                    if(isThisMoving && currentAnimationThisObject != "CassavaSlimeAboutToMove"){
                        animatorThisObject.Play("CassavaSlimeAboutToMove");
                        currentAnimationThisObject = "CassavaSlimeAboutToMove";
                    
                    }else{ 

                    if(isThisMoving != true && currentAnimationThisObject != "CassavaSlimeJiggleEat"){
                        animatorThisObject.Play("CassavaSlimeJiggleEat");
                        currentAnimationThisObject = "CassavaSlimeJiggleEat";
                        }
                    }
            
                break;

                case "CassavaSlimeKnight(Clone)":

                    if(isThisMoving && currentAnimationThisObject != "SlimeKnightPerpetualHunt"){
                        animatorThisObject.Play("SlimeKnightPerpetualHunt");
                        currentAnimationThisObject = "SlimeKnightPerpetualHunt";
                    
                    }else{ 

                    if(isThisMoving != true && currentAnimationThisObject != "SlimeKnightPerpetualHunt"){
                        animatorThisObject.Play("SlimeKnightPerpetualHunt");
                        currentAnimationThisObject = "SlimeKnightPerpetualHunt";
                        }
                    }
            
                break;

                case "CassavaSlimeKing":

                    if(isThisMoving && !thisEnemyHealthManager.bossIsDead && currentAnimationThisObject != "CassavaSlimeKingMoving"){
                        animatorThisObject.Play("CassavaSlimeKingMoving");
                        currentAnimationThisObject = "CassavaSlimeKingMoving";
                    
                    }else{ 

                    if(isThisMoving != true && !thisEnemyHealthManager.bossIsDead && currentAnimationThisObject != "CassavaSlimeKingIdle"){
                        animatorThisObject.Play("CassavaSlimeKingIdle");
                        currentAnimationThisObject = "CassavaSlimeKingIdle";
                        }
                    }
            
                break;

                case "PandanShooter(Clone)":

                    if(isThisMoving && currentAnimationThisObject != "PandanShooterAboutToMove"){
                        animatorThisObject.Play("PandanShooterAboutToMove");
                        currentAnimationThisObject = "PandanShooterAboutToMove";
                    
                    }else{ 

                    if(isThisMoving != true && currentAnimationThisObject != "PandanShooterIdle"){
                        animatorThisObject.Play("PandanShooterIdle");
                        currentAnimationThisObject = "PandanShooterIdle";
                        }
                    }

                break;

                case "BananaShaman(Clone)":

                    if(isThisMoving && currentAnimationThisObject != "BananaShamanAboutToMove"){
                        animatorThisObject.Play("BananaShamanAboutToMove");
                        currentAnimationThisObject = "BananaShamanAboutToMove";
                    
                    }else{ 

                    if(isThisMoving != true && currentAnimationThisObject != "BananaShamanIdle"){
                        animatorThisObject.Play("BananaShamanIdle");
                        currentAnimationThisObject = "BananaShamanIdle";
                        }
                    }

                break;

                case "PricklyStrawberry(Clone)":

                    if(isThisMoving && currentAnimationThisObject != "PricklyStrawberryAboutToMove"){
                        animatorThisObject.Play("PricklyStrawberryAboutToMove");
                        currentAnimationThisObject = "PricklyStrawberryAboutToMove";
                    
                    }else{ 

                    if(isThisMoving != true && currentAnimationThisObject != "PricklyStrawberryrIdle"){
                        animatorThisObject.Play("PricklyStrawberryrIdle");
                        currentAnimationThisObject = "PricklyStrawberryrIdle";
                        }
                    }

                break;

                case "MangoMauler(Clone)":

                    if(isThisMoving && currentAnimationThisObject != "MangoMaulerAboutToMove"){
                        animatorThisObject.Play("MangoMaulerAboutToMove");
                        currentAnimationThisObject = "MangoMaulerAboutToMove";
                    
                    }else{ 

                    if(isThisMoving != true && currentAnimationThisObject != "MangoMaulerIdle"){
                        animatorThisObject.Play("MangoMaulerIdle");
                        currentAnimationThisObject = "MangoMaulerIdle";
                        }
                    }

                break;

                case "UnderGroundUbe(Clone)":

                    if(isThisMoving && currentAnimationThisObject != "UnderGroundUbeAboutToMove"){
                        animatorThisObject.Play("UnderGroundUbeAboutToMove");
                        currentAnimationThisObject = "UnderGroundUbeAboutToMove";
                    
                    }else{ 

                    if(isThisMoving != true && currentAnimationThisObject != "UnderGroundUbeAboutToIdle"){
                        animatorThisObject.Play("UnderGroundUbeAboutToIdle");
                        currentAnimationThisObject = "UnderGroundUbeAboutToIdle";
                        }
                    }

                break;
            }

            
        }
        if(whatEnemyIsThis == "CassavaSlimeKing"){
        //Switch for attack animations
            if(thisEnemyHealthManager.bossIsDead){
                animatorThisObject.Play("CassavaSlimeKingDies");
                currentAnimationThisObject = "CassavaSlimeKingDies";
            }
        }
    }
}
