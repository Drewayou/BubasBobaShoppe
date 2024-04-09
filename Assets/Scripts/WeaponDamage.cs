using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{

    //Below is used for the attack of Bubba's Staff
    [SerializeField]
    [Tooltip("(Int) What's the attack of this object?")]
    public int attackPoints;

    //Below is used for collision detection to animate deatheffects with the ridgedbody velocity of this target object
    [Header("Collision detector")]
    [SerializeField]
    [Tooltip("This is the RidgedBody2d of this object/entity depending on what they are")]
    public Rigidbody2D thisBody;

    //Below is used for the Polygon Collider 2D for the HITBOX / Collision of the object.
    [Header("Hitbox")]
    [SerializeField]
    [Tooltip("This is the Polygon collider of this object FOR THE HITBOX (Not a trigger) depending on what they are")]
    public CompositeCollider2D thisHitbox;

    // Start is called before the first frame update
    void Start()
    {
        thisHitbox = GetComponent<CompositeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*Check what THIS object collides with and what "team" they are on
    Then, does respective actions*/
    void OnCollisionEnter2D(Collision2D collision){
        //Debug.Log("Collision Started!");

        if(collision.gameObject.CompareTag("Enemy")) {

                        //Debug.Log(gameObject.name + "PlayerLanded a Meele attack!");
                        collision.gameObject.GetComponent<IDamageable>().damageThis(attackPoints);
                }

        }

        
    }

