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

    [Header("Movement Variables")]
    [Tooltip("The speed at which the player will move.")]
    public float moveSpeed = 2.0f;

    private Player_AnimChanger thisPlayerAnim;

    //More variables for this script
    //Direction the player moves depending on Input Action Event
    private Vector2 movementDirection;

    //The current position of the player
    private Vector2 playerCurrentPosition;

    //Bool if attack button is pressed down
    public bool attackButtonPressed;

void Start(){
    myRigidbody = GetComponent<Rigidbody2D>();
    myHitbox = GetComponent<PolygonCollider2D>();
    thisPlayerAnim = GetComponent<Player_AnimChanger>();
}

void Update(){
   movePlayer();
   giveInpuValuesY();
   giveInpuValuesX();
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

    if(!amIAttackng()){

    playerCurrentPosition = new Vector2(myRigidbody.position.x, myRigidbody.position.y);
    Vector2 velocity = movementDirection * moveSpeed * Time.deltaTime;

    myRigidbody.MovePosition(playerCurrentPosition + velocity);
    }
}

// Gets the player animator of this game object in order to make sure attack / movement anims don't overlap
// Moreover, a key mechanic in your game is the player can't move when attacking.
private bool amIAttackng()
    {
        return thisPlayerAnim.isSequencialAnimationInProgress();
    }

// Anoter public method used by the Player_Anim_Controller to check if an attack button was pressed

public bool attackButtonWasPressed()
    {
        return attackButtonPressed;
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

// Pulls input of left click OR "A" (xbox/gamepad button) to be processed by methods above.
public void processAttack(InputAction.CallbackContext context){
    attackButtonPressed = context.ReadValueAsButton();
    Debug.Log("attackButtonPressed was pressed!");
    Debug.Log("Bool setting :" + attackButtonPressed);
    }

}
