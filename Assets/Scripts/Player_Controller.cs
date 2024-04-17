using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// This class controls player movement
/// </summary>
public class Player_Controller : MonoBehaviour
{
    [Header("GameObject/Component References")]
    [Tooltip("The animator controller used to animate the player.")]
    public RuntimeAnimatorController animator = null;
    [Tooltip("The Rigidbody2D component to use for collision detection.")]
    public Rigidbody2D myRigidbody = null;
    [Tooltip("The Polygon hitbox to use for collision detection.")]
    public PolygonCollider2D myHitbox = null;

    public Health_Universal attachedHealthUni;

    private GameManagerScript thisGameData;

    [Header("Movement Variables")]
    [Tooltip("The speed at which the player will move.")]
    public float moveSpeed = 1.5f;

    //attackPoints Taken from JSONGameScript
    public int playerAttackPoints;

    //playerMaxHealth Taken from JSONGameScript
    public int playerMaxHealth;

    //playerMaxStamina is taken from JSONGameScript
    public float playerMaxStamina;

    public float sprintingSpeed = 2.0f;

    [Header("Stamina Variables")]
    [Tooltip("The stamina at which the player cost to do actions.")]
    public float meeleAttackCost = 2f, penaltyTimeStaminaOut = 5f, penaltyTimer;

    public float currentPlayerStamina , playerIdleStaminaRecoverBoost;

    private Player_AnimChanger thisPlayerAnim;

    //More variables for this script
    //Direction the player moves depending on Input Action Event
    private Vector2 movementDirection;

    //The current position of the player
    private Vector2 playerCurrentPosition;

    //Bool if attack button is pressed down
    public bool attackButtonPressed = true;

    //Bool if sprint button is pressed down
    public bool sprintingButtonPressed = false;

    //Bool if standard meele attack or throwing bamboo are used
    public bool wasMeeleAttack = true;

    public bool inPenalty = false;

    public bool isPlayerMoving = false;

void Start(){

    thisGameData = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
    attachedHealthUni = GetComponent<Health_Universal>();
    attachedHealthUni.health = playerMaxHealth;
    myRigidbody = GetComponent<Rigidbody2D>();
    myHitbox = GetComponent<PolygonCollider2D>();
    thisPlayerAnim = GetComponent<Player_AnimChanger>();
    currentPlayerStamina = thisGameData.ReturnPlayerStats().playerMaxStamina;
    playerAttackPoints = thisGameData.ReturnPlayerStats().playerAttackPoints;

    playerIdleStaminaRecoverBoost = 2f;
    sprintingButtonPressed = false;
}

void Update(){
   movePlayer();
   giveInpuValuesY();
   giveInpuValuesX();
    staminaRegen();
    processPenaltyTime();
}

//Gets player current location
public Vector2 getPlayerLocation(){
    return playerCurrentPosition;
}

//Below BOC is for other scripts to use (Anim Script) for X values of inputs from the player
public float giveInpuValuesX(){
    return movementDirection.x;
}

//Below BOC is for other scripts to use (Anim Script) for Y values of inputs from the player
public float giveInpuValuesY(){
    return movementDirection.y;
}

//Another BOC to give (Enemy_Controller Script) the collider of this player
public PolygonCollider2D giveCollider(){
    return myHitbox;
}

//Actual BOC to move the player in the game using vectors pulled from method "processPlayerMovement()" below
public void movePlayer(){

    if(movementDirection.x != 0 || movementDirection.y != 0){
        isPlayerMoving = true;
    }else{
        isPlayerMoving = false;
    }

    if(!amIAttackng()){

    playerCurrentPosition = new Vector2(myRigidbody.position.x, myRigidbody.position.y);
    
    if(sprintingButtonPressed && !inPenalty){
        Vector2 velocitySprinting = movementDirection * sprintingSpeed * Time.fixedDeltaTime;
        myRigidbody.MovePosition(playerCurrentPosition + velocitySprinting);
        currentPlayerStamina -= 3f * Time.deltaTime;
    }else{
        Vector2 velocity = movementDirection * moveSpeed * Time.fixedDeltaTime;
        myRigidbody.MovePosition(playerCurrentPosition + velocity);
    }
    }
}

// Gets the player animator of this game object in order to make sure attack / movement anims don't overlap
// Moreover, a key mechanic in your game is the player can't move when attacking.
private bool amIAttackng()
    {
        return thisPlayerAnim.isSequencialAnimationInProgress();
    }

// Another public method used by the Player_Anim_Controller to check if an attack button was pressed

public bool attackButtonWasPressed()
    {
        if(currentPlayerStamina>=0){
        return attackButtonPressed;
        }
        else{
        return false;
        }
    }


//Another public method used to check if player is sprinting.
    
public bool sprintingPlayer(){
    if(currentPlayerStamina>=0){
        return sprintingButtonPressed;
        }
        else{
        return false;
        }
    }

public void processPenaltyTime(){
    if(inPenalty){
        if(!isPlayerMoving){
        penaltyTimer += playerIdleStaminaRecoverBoost * Time.deltaTime;
        }else{
            penaltyTimer += Time.deltaTime;
        }
        if(penaltyTimer>=penaltyTimeStaminaOut){
            inPenalty =  false;
            penaltyTimer = 0f;
            currentPlayerStamina = 0.1f;
        }
    }
}

public float returnCurrentStamina(){
    return currentPlayerStamina;
}

public float returnCurrentPenaltyTimer(){
    return penaltyTimer;
}

public bool returnIfPlayerIsMoving(){
    return isPlayerMoving;
}

    
public void staminaRegen(){

    //Recover More Stamina if the player is not moving, otherwise, recover normally.
    if(currentPlayerStamina<=playerMaxStamina && !inPenalty){
        if(!isPlayerMoving){
            currentPlayerStamina += playerIdleStaminaRecoverBoost * Time.deltaTime;
        }
        if(!isPlayerMoving && !sprintingButtonPressed){
            currentPlayerStamina += Time.deltaTime;
        }
        if(isPlayerMoving){
            currentPlayerStamina += Time.deltaTime * 0.5f;
        }
    }

    //FIXME: Basically, if the player spends more cooldown than they should for an attack
    //Sprint, then they have a penalty of a 5 second wait. ADD a different UI Animation for the
    //Image of the StaminaBar too!?
    if(currentPlayerStamina<0){
        inPenalty = true;
    }
}

/// <summary>
/// 
/// NOTE: ALL INPUT EVENT SYSTEM CALL BACKS ARE BELOW
/// 
/// </summary>
/// 
// Input Action Callback events to use in order to read the input systems

// Pulls input vectors from WASD or Joystick movement to be processed by methods above.
public void processPlayerMovement(InputAction.CallbackContext context){

    movementDirection = context.ReadValue<Vector2>();
    //Debug.Log("Input movemen sensed :" + movementDirection.x + "," + movementDirection.y);
    
}

// Pulls input of left click OR "Right Trigger" (xbox/gamepad button) to be processed by methods above.
public void processAttack(InputAction.CallbackContext context){
    attackButtonPressed = context.ReadValueAsButton();
    }
    
// Pulls input of "Shift" click OR "A" (xbox/gamepad button) to be processed by methods above.
public void processSprint(InputAction.CallbackContext context){
    sprintingButtonPressed = context.ReadValueAsButton();
    }

public int getPlayerMaxHealth(){
    return playerMaxHealth;
}
}




