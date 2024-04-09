using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileDoDamage : MonoBehaviour, IDamageable
{
    [Header("Health Points")]
    [SerializeField]
    [Tooltip("(Int) This is the health set for this object/entity and should be variable depending on what they are")]
    public int thisProjectileHealth;

    [Header("Attack Points")]
    [SerializeField]
    [Tooltip("(Int) What's the attack/Damage of this projectile?")]
    public int thisProjectileDamagePoints;

    //Below is used for the Polygon Collider 2D for the HITBOX / Collision of the object.
    [Header("Hitbox")]
    [SerializeField]
    [Tooltip("This is the Polygon collider of this object FOR THE HITBOX (Not a trigger) depending on what they are")]
    public PolygonCollider2D thisHitbox;

    //Below is used for the Polygon Collider 2D for the HITBOX / Collision of the object.
    [SerializeField]
    [Header("Is This Player Projectile?")]
    [Tooltip("If this is the player's projectile or enemies. Used for propper collision detection")]
    public bool playersProjectile = false;

    // Start is called before the first frame update
    void Start()
    {
        thisHitbox = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (thisProjectileHealth<=0){
            Destroy(this.gameObject);
        }
    }

    /*Check what THIS object collides with and what "team" they are on
    Then, does respective actions*/
    void OnCollisionEnter2D(Collision2D collision){

        if(collision.gameObject.CompareTag("Player") && !playersProjectile) {
            collision.gameObject.GetComponent<IDamageable>().damageThis(thisProjectileDamagePoints);
        }

        /*if(collision.gameObject.CompareTag("Enemy") && playersProjectile) {
            collision.gameObject.GetComponent<IDamageable>().damageThis(thisProjectileDamagePoints);
        }*/

        //Destroy(this.gameObject);
    }

    //Used so that the projectile itself can be destroyed or killed with a well-timed strike
    public void damageThis(int damage)
    {
        thisProjectileHealth = thisProjectileHealth-damage;
    }
}
