using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_AnimChanger : MonoBehaviour
{
    //Find object's animator and controller to programatically change states / animations
    Animator animatorThisObject;
    Player_Controller controllerThisObject;

    //Prepare player attack pre-fab for meele weapons
    [Header("Meele Weapon Prefab")]
    [Tooltip("The Prefab to instantiate an attack")]
    public GameObject Player_Weapon;

    //Meele pre-fab Temp Clone var
    private GameObject Player_Weapon_Attacks;

    //Vurrent animation playing for the player character
    string currentAnimationThisObject;

    float movmentInputX, movementInputY, sequentialAnimationTimeLeft = 0;

    [Header("Delay before idle animation starts Variables")]
    [Tooltip("The speed at which the player will move.")]
    public float idleDelayAnimTime;

    public bool playerWantsToMove = true, playerIsAttackingAnim = false, didAttackCostStamina = false;
    bool playerLookingN = false, 
    playerLookingS = true, playerLookingE = false, playerLookingW = false,
    playerLookingNW = false, playerLookingNE = false, playerLookingSW = false,
    playerLookingSE = false;

    // Start is called before the first frame update
    void Start()
    {
        animatorThisObject = gameObject.GetComponent<Animator>();
        controllerThisObject = gameObject.GetComponent<Player_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        changeAnimation();
        playDownSequencialAnimTimer();
    }

    public void changeAnimation(){

        if(controllerThisObject.attackButtonWasPressed()){
            sequentialAnimationTimeLeft = .10f;
        }

        saveWherePlayerIsLooking();

        //Check if there is player input, if not, 
        
        //
        //TO DO : co-responding idle animations after set time.
        //

        if(sequentialAnimationTimeLeft == 0){

        //Save movement input values
        movmentInputX = controllerThisObject.giveInpuValuesX();
        movementInputY = controllerThisObject.giveInpuValuesY();

        if (movmentInputX != 0 | movementInputY != 0){
            playerWantsToMove = true;
        }else{
            playerWantsToMove = false;
        }

        
        if(playerWantsToMove){

            //Walking facing NORTH player animation
            if (playerLookingN && currentAnimationThisObject != "BubbaDiagWalkNorthEast"){
                    currentAnimationThisObject = "BubbaDiagWalkNorthEast";
                    animatorThisObject.Play("BubbaDiagWalkNorthEast",0,0);

                    //FlipVectorCharacter
                    if(transform.localScale.x == -1){
                    Vector3 FlipVector = new Vector3(1, 1, 1);
                    transform.localScale = FlipVector;
                }
            }

            //Walking facing SOUTH player animation
            if (playerLookingS && currentAnimationThisObject != "BubbaDiagWalkSouthEast"){
                    currentAnimationThisObject = "BubbaDiagWalkSouthEast";
                    animatorThisObject.Play("BubbaDiagWalkSouthEast",0,0);

                    //FlipVectorCharacter
                    if(transform.localScale.x == -1){
                    Vector3 FlipVector = new Vector3(1, 1, 1);
                    transform.localScale = FlipVector;
                }
            }

            //Walking facing EAST player animation
            if (playerLookingE && currentAnimationThisObject != "BubbaDiagWalkSouthEast" && currentAnimationThisObject != "BubbaDiagWalkSouthEast" && currentAnimationThisObject != "BubbaDiagWalkNorthEast"){
                    currentAnimationThisObject = "BubbaDiagWalkSouthEast";
                    animatorThisObject.Play("BubbaDiagWalkSouthEast",0,0);

                    //FlipVectorCharacter
                    if(transform.localScale.x == -1){
                    Vector3 FlipVector = new Vector3(1, 1, 1);
                    transform.localScale = FlipVector;
                }
            }

            //Walking facing WEST player animation
            if (playerLookingW && currentAnimationThisObject != "BubbaDiagWalkSouthWest" && currentAnimationThisObject != "BubbaDiagWalkSouthWest" && currentAnimationThisObject != "BubbaDiagWalkNorthWest"){
                    currentAnimationThisObject = "BubbaDiagWalkSouthWest";
                    animatorThisObject.Play("BubbaDiagWalkSouthWest",0,0);

                    //FlipVectorCharacterWEST
                    if(transform.localScale.x == 1){
                    Vector3 FlipVector = new Vector3(-1, 1, 1);
                    transform.localScale = FlipVector;
                }
            }

            //Walking facing NORTHEAST player animation
            if (playerLookingNE && currentAnimationThisObject != "BubbaDiagWalkNorthEast"){
                    currentAnimationThisObject = "BubbaDiagWalkNorthEast";
                    animatorThisObject.Play("BubbaDiagWalkNorthEast",0,0);

                    //FlipVectorCharacter
                    if(transform.localScale.x == -1){
                    Vector3 FlipVector = new Vector3(1, 1, 1);
                    transform.localScale = FlipVector;
                }
            }

            //Walking facing NORTHWEST player animation
            if (playerLookingNW && currentAnimationThisObject != "BubbaDiagWalkNorthWest"){
                    currentAnimationThisObject = "BubbaDiagWalkNorthWest";
                    animatorThisObject.Play("BubbaDiagWalkNorthWest",0,0);

                    //FlipVectorCharacterWEST
                    if(transform.localScale.x == 1){
                    Vector3 FlipVector = new Vector3(-1, 1, 1);
                    transform.localScale = FlipVector;
                }
            }

            //Walking facing SOUTHEAST player animation
            if (playerLookingSE && currentAnimationThisObject != "BubbaDiagWalkSouthEast"){
                    currentAnimationThisObject = "BubbaDiagWalkSouthEast";
                    animatorThisObject.Play("BubbaDiagWalkSouthEast",0,0);

                    //FlipVectorCharacter
                    if(transform.localScale.x == -1){
                    Vector3 FlipVector = new Vector3(1, 1, 1);
                    transform.localScale = FlipVector;
                }
            }

            //Walking facing SOUTHWEST player animation
            if (playerLookingSW && currentAnimationThisObject != "BubbaDiagWalkSouthWest"){
                    currentAnimationThisObject = "BubbaDiagWalkSouthWest";
                    animatorThisObject.Play("BubbaDiagWalkSouthWest",0,0);

                    //FlipVectorCharacterWEST
                    if(transform.localScale.x == 1){
                    Vector3 FlipVector = new Vector3(-1, 1, 1);
                    transform.localScale = FlipVector;
                }
            }

        }else{

            ///
            /// BELOW ARE IDLE ANIMATIONS WHEN MOVEMENT VECTOR = 0 
            /// ON BOTH X & Y AXIS!
            ///
            ///

            //Idle facing NORTH/NORTHEAST player animation
            if (playerLookingN || playerLookingNE && currentAnimationThisObject != "BubbaIdleNorthEast"){
                currentAnimationThisObject = "BubbaIdleNorthEast";
                animatorThisObject.Play("BubbaIdleNorthEast",0,0);

                //FlipVectorCharacter
                    if(transform.localScale.x == -1){
                    Vector3 FlipVector = new Vector3(1, 1, 1);
                    transform.localScale = FlipVector;
                }
            }       

            //Idle facing NORTHWEST player animation
            if (playerLookingNW && currentAnimationThisObject != "BubbaIdleNorthWest"){
                currentAnimationThisObject = "BubbaIdleNorthWest";
                animatorThisObject.Play("BubbaIdleNorthWest",0,0);

                //FlipVectorCharacterWEST
                    if(transform.localScale.x == 1){
                    Vector3 FlipVector = new Vector3(-1, 1, 1);
                    transform.localScale = FlipVector;
                }
            }

            //Idle facing SOUTH/SOUTHEAST animation
            if (playerLookingS || playerLookingE || playerLookingSE && currentAnimationThisObject != "BubbaIdleSouthEast"){
                currentAnimationThisObject = "BubbaIdleSouthEast";
                animatorThisObject.Play("BubbaIdleSouthEast",0,0);

                //FlipVectorCharacter
                    if(transform.localScale.x == -1){
                    Vector3 FlipVector = new Vector3(1, 1, 1);
                    transform.localScale = FlipVector;
                }
            }   

            //Idle facing SOUTHWEST animation
            if (playerLookingW || playerLookingSW && currentAnimationThisObject != "BubbaIdleSouthWest"){
                currentAnimationThisObject = "BubbaIdleSouthWest";
                animatorThisObject.Play("BubbaIdleSouthWest",0,0);

                //FlipVectorCharacterWEST
                    if(transform.localScale.x == 1){
                    Vector3 FlipVector = new Vector3(-1, 1, 1);
                    transform.localScale = FlipVector;
                    }
                }   
            }
        }else{

        /* NOTE : ABOVE CODE IS PURELY FOR PLAYER MOVEMENT.
        IF IN ANY OTHER ANIMATION AND THE PLAYER WANTS TO MOVE, THEY CANNOT.
        BELOW ARE OTHER ANIMATIONS IN SPECIFIC CASES LIKE ATTACK ANIMS.
        */

        //Meele Attack Animation
        if(controllerThisObject.attackButtonWasPressed()){

            attackStaminaCostApplied(controllerThisObject.wasMeeleAttack);

            //ATTACK facing NORTHEAST player animation
            if (playerLookingN || playerLookingNE && currentAnimationThisObject != "BubbaNorthEastAttack"){
                currentAnimationThisObject = "BubbaNorthEastAttack";
                animatorThisObject.Play("BubbaNorthEastAttack",0,0);

                if(!playerIsAttackingAnim){

                Vector3 offsetAttackPosition 
                = new Vector3(controllerThisObject.transform.position.x+0.05f,
                controllerThisObject.transform.position.y+0.05f, 
                controllerThisObject.transform.position.z);

                Player_Weapon_Attacks = Instantiate (Player_Weapon,offsetAttackPosition,controllerThisObject.transform.rotation);
                
                Player_Weapon_Attacks.GetComponent<Animator>().Play("AttackNorthEast");

                playerIsAttackingAnim = true;}

                //FlipVectorCharacter
                    if(transform.localScale.x == -1){
                    Vector3 FlipVector = new Vector3(1, 1, 1);
                    transform.localScale = FlipVector;
                }
            }       

            //ATTACK facing NORTHWEST player animation
            if (playerLookingNW && currentAnimationThisObject != "BubbaNorthWestAttack"){
                currentAnimationThisObject = "BubbaNorthWestAttack";
                animatorThisObject.Play("BubbaNorthWestAttack",0,0);

                if(!playerIsAttackingAnim){

                Vector3 offsetAttackPosition 
                = new Vector3(controllerThisObject.transform.position.x-0.05f,
                controllerThisObject.transform.position.y+0.05f, 
                controllerThisObject.transform.position.z);
                
                Player_Weapon_Attacks = Instantiate (Player_Weapon,offsetAttackPosition,controllerThisObject.transform.rotation);
                
                Vector3 FlipVector = new Vector3(-1, 1, 1);
                Player_Weapon_Attacks.transform.localScale = FlipVector;
                Player_Weapon_Attacks.GetComponent<Animator>().Play("AttackNorthWest");

                playerIsAttackingAnim = true;}

                //FlipVectorCharacterWEST
                    if(transform.localScale.x == 1){
                    Vector3 FlipVector = new Vector3(-1, 1, 1);
                    transform.localScale = FlipVector;
                }
            }

            //ATTACK facing SOUTH/SOUTHEAST animation
            if (playerLookingS || playerLookingE || playerLookingSE && currentAnimationThisObject != "BubbaSouthEastAttack"){
                currentAnimationThisObject = "BubbaSouthEastAttack";
                animatorThisObject.Play("BubbaSouthEastAttack",0,0);
                

                if(!playerIsAttackingAnim){

                Vector3 offsetAttackPosition 
                = new Vector3(controllerThisObject.transform.position.x+0.12f,
                controllerThisObject.transform.position.y-0.12f, 
                controllerThisObject.transform.position.z);
                

                Player_Weapon_Attacks = Instantiate (Player_Weapon,offsetAttackPosition,controllerThisObject.transform.rotation);
            
                Player_Weapon_Attacks.GetComponent<Animator>().Play("AttackSouthEast");

                playerIsAttackingAnim = true;}

                //FlipVectorCharacter
                    if(transform.localScale.x == -1){
                    Vector3 FlipVector = new Vector3(1, 1, 1);
                    transform.localScale = FlipVector;
                }
            }   

            //ATTACK facing SOUTHWEST animation
            if (playerLookingW || playerLookingSW && currentAnimationThisObject != "BubbaSouthWestAttack"){
                currentAnimationThisObject = "BubbaSouthWestAttack";
                animatorThisObject.Play("BubbaSouthWestAttack",0,0);

                if(!playerIsAttackingAnim){

                Vector3 offsetAttackPosition 
                = new Vector3(controllerThisObject.transform.position.x-0.12f,
                controllerThisObject.transform.position.y-0.12f, 
                controllerThisObject.transform.position.z);
                

                Player_Weapon_Attacks = Instantiate (Player_Weapon,offsetAttackPosition,controllerThisObject.transform.rotation);
                
                Vector3 FlipVector = new Vector3(-1, 1, 1);
                Player_Weapon_Attacks.transform.localScale = FlipVector;
                Player_Weapon_Attacks.GetComponent<Animator>().Play("AttackSouthWest");

                playerIsAttackingAnim = true;}

                //FlipVectorCharacterWEST
                    if(transform.localScale.x == 1){
                    Vector3 FlipVector = new Vector3(-1, 1, 1);
                    transform.localScale = FlipVector;
                    }
                }

            //Refund the bool to former state if the attack button was released
            }else{didAttackCostStamina=false;}
        }
    }

    public void playDownSequencialAnimTimer(){
        if(sequentialAnimationTimeLeft>=0){
        sequentialAnimationTimeLeft -= Time.deltaTime;
        //Debug.LogWarning("DECREASING ANIM TIME");
        }else{
            sequentialAnimationTimeLeft = 0;

            endAllSeqencialAnims();

        }
    }

    public void endAllSeqencialAnims(){
        playerIsAttackingAnim = false;
    }

    public void saveWherePlayerIsLooking(){

        //Debug.Log("Player looking Inputrecorded");
        //Save movement input values

        //Check if player is looking NORTH and save value
            if(movmentInputX == 0 && movementInputY > 0){
                resetLookingDirection();
                playerLookingN = true;
            }
        
        //Check if player is looking SOUTH and save value
            if(movmentInputX == 0 && movementInputY < 0){
                resetLookingDirection();
                playerLookingS = true;
            }

        //Check if player is looking EAST and save value
            if(movmentInputX > 0 && movementInputY == 0){
                resetLookingDirection();
                playerLookingE = true;
            }

        //Check if player is looking WEST and save value
            if(movmentInputX < 0 && movementInputY == 0){
                resetLookingDirection();
                playerLookingW = true;
            }
        
        //Check if player is looking NORTHWEST and save value
        
            if(movmentInputX < 0 && movementInputY > 0){
                resetLookingDirection();
                playerLookingNW = true;
            }
        
        //Check if player is looking NORTHEAST and save value
        
            if(movmentInputX > 0 && movementInputY > 0){
                resetLookingDirection();
                playerLookingNE = true;
            }
        
        //Check if player is looking SOUTHWEST and save value
            if(movmentInputX < 0 && movementInputY < 0){
                resetLookingDirection();
                playerLookingSW = true;
            }

        //Check if player is looking SOUTHEAST and save value
            if(movmentInputX > 0 && movementInputY < 0){
                resetLookingDirection();
                playerLookingSE = true;
            }
        }

        public void resetLookingDirection(){
            playerLookingN = false;
            playerLookingS = false;
            playerLookingE = false;
            playerLookingW = false;
            playerLookingNW = false;
            playerLookingSW = false;
            playerLookingNE = false;
            playerLookingSE = false;
        }

    public string getCurrentAnimation(){
        return currentAnimationThisObject;
    }

    // Will be used in "Player Weapon Script", useful public method that allows other
    // scripts to know this player looking value.
    public string whereIsPlayerLooking(){
        if(playerLookingN){
            return "playerLookingN";
        }
        if(playerLookingS){
            return "playerLookingS";
        } 
        if(playerLookingE){
            return "playerLookingE";
        } 
        if(playerLookingW){
            return "playerLookingW";
        } 
        if(playerLookingSW){
            return "playerLookingSW";
        } 
        if(playerLookingNE){
            return "playerLookingNE";
        } 
        if(playerLookingNW){
            return "playerLookingNW";
        } 
        if(playerLookingSE){
            return "playerLookingSE";
        }
        else{
            return "noRecordedPlayerLookingVal";
        }

    }

    // Used in Player_Controller to prevent spamming of attack / extra movements
    //during special sequential animations (Death, Revive, Attack, Etc.)
    public bool isSequencialAnimationInProgress(){

        if(sequentialAnimationTimeLeft!=0){

        return true;

        }else{
            return false;
        };
    }

    public void attackStaminaCostApplied(bool meeleAttack){
        if(meeleAttack && !didAttackCostStamina){
        controllerThisObject.currentPlayerStamina -= controllerThisObject.meeleAttackCost;
        didAttackCostStamina = true;
        }
    }
}
