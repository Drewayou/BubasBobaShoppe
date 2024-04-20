using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health_Universal : MonoBehaviour, IDamageable
{
    GameObject thisEnemy;

    [Header("Health Points")]
    [Tooltip("(Int) This is the health set for this object/entity and should be variable depending on what they are")]
    public int health;

    [Header("Attack Points")]
    [SerializeField]
    [Tooltip("(Int) What's the attack of this object?")]
    public int attackPoints;

    [Header("Attack Speed")]
    [SerializeField]
    [Tooltip("(Int) What's the attack of this object?")]
    public float attackSpeed;

    [Header("After Hit Invulnerability")]
    [Tooltip("(Int) Time this object can be invulnerable when hit and not killed")]
    public float tempInvTime;

    //Temp var for saving cooldown for dmg taken
    private float tempInvCountdown;

    [Header("Death Effect")]
    [SerializeField]
    [Tooltip("This is the death effect of this object /entity depending on what they are")]
    public GameObject deathEffect;

    [Header("Can Drop Loot")]
    [SerializeField]
    [Tooltip("If hit by the player or environment, this will insure the monster will drop loot")]
    public bool willDropLoot = false;

    [Header("Got Hit Effect")]
    [SerializeField]
    [Tooltip("This is the taken damage effect of this object /entity depending on what they are")]
    public GameObject gotHitEffect;

    [SerializeField]
    [Header("")]
    [Tooltip("")]
    public GameObject playerRevivesSfx;

    [SerializeField]
    [Header("")]
    [Tooltip("")]
    public GameObject playerReallyDiesSfx;

    [Header("Got Hit Sprite Flash")]
    [SerializeField]
    [Tooltip("This is for the 'flash' of dmg effect of this object /entity depending on what they are")]
    public SpriteRenderer thisSpriteRenderer;

    [Header("Set RoundObject")]
    [Tooltip("This is for the Round Manager to pull it's scripts and calculate round essentials")]
    private GameObject thisRoundCalculatorObject;
    private RoundManagerScript thisRoundManagerScript;

    private GameManagerScript thisGlobalGameManager;

    [SerializeField]
    public Player_Controller thisPlayerController;

    // Private variables to let this sprite flash a chosen color (Red) when taken damage
    private Color chosenRedColor = new Color(255,0,0,1);
    private Color colorChangeVelocity;
    private float tempColorChangetime;

    //Below is used for collision detection to animate deatheffects with the ridgedbody velocity of this target object
    [Header("Collision detector")]
    [SerializeField]
    [Tooltip("This is the RidgedBody2d of this object/entity depending on what they are")]
    public Rigidbody2D thisBody;

    //Below is used for the Polygon Collider 2D for the HITBOX / Collision of the object.
    [Header("Hitbox")]
    [SerializeField]
    [Tooltip("This is the Polygon collider of this object FOR THE HITBOX (Not a trigger) depending on what they are")]
    public PolygonCollider2D thisHitbox;

    [Header("(bool) isThisPlayer")]
    [SerializeField]
    [Tooltip("Is Health script for the player/Neutral OR an enemy NPC?")]
    public bool isThisThePlayer;

    [Header("(bool) isThisABoss")]
    [SerializeField]
    [Tooltip("Allows for the round to end/make a cool end")]
    public bool isThisABoss;

    //NOTE: "isUnhittable is different from isInvulnerable! You can hit invulnerable stuff!"
    [Header("(bool) isInvulnerable")]
    [SerializeField]
    [Tooltip("Is this Invulnerable to death & Damage?")]
    public bool isInvulnerable;

    [Header("(bool) isUnhittable")]
    [SerializeField]
    [Tooltip("Is this isUnhittable to Damage?")]
    public bool isUnhittable;

    [Header("canAttackRanged")]
    [SerializeField]
    [Tooltip("Can this be a ranged or on-contact meele character")]
    public bool canAttackRanged;

    // Hidden location where this object dies to spawn the death effect
    private Vector2 locationUponDeath;

    private NavMeshAgent thisAgent;

    /* To evaluate if the this object/entity is indeed dead. Useful in cases
    where the attack damage taken != death.
    */
    public bool amIDeadYet;

    /* To limit the SPAM of CollisionStay(); This allows a once change instead
    of constant event updates that there's a stay collision.
    */
    private bool amITakingDamage = false;

    //This values below are only in use IF&OIF this is the player and there's lives remaining
    private bool playerReviving = false;
    private float timeForRevive = 5f;

    public bool bossIsDead = false;

    // Start is called before the first frame update
    void Start()
    {
        thisEnemy = this.gameObject;

        thisGlobalGameManager = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
        //Set up round manager script and record this enemy instance
        thisRoundCalculatorObject = GameObject.Find("RoundManager");
        thisRoundManagerScript = thisRoundCalculatorObject.GetComponent<RoundManagerScript>();

        thisHitbox = GetComponent<PolygonCollider2D>();
        thisSpriteRenderer = GetComponent<SpriteRenderer>();

        colorChangeVelocity = Color.white - thisSpriteRenderer.material.color; 
        tempColorChangetime = tempInvTime;

        //If this is an enemy, set up the navmesh, else set up HP
        if(!isThisThePlayer){
        thisAgent = GetComponent<NavMeshAgent>();
        }else{
            health = thisGlobalGameManager.ReturnPlayerStats().playerMaxHealth;
        }

        //Make sure this object can be hit and is not invulnerable.
        isUnhittable = false;
        isInvulnerable = false;
    }

    // Update is called once per frame
    void Update()
    {
        stillAliveChecker();
        resetNormalSpriteColor(thisSpriteRenderer.material.color);
        RevivingPlayer(playerReviving);
    }

    void stillAliveChecker(){


        //Enemy death condition
        if(!isThisThePlayer && health <= 0){
            amIDeadYet = true;
            commitDeath();
        }

        if(isThisThePlayer && health <= 0 && thisRoundManagerScript.ReturnPlayerLives() > 0){
            playerReviving = true;
        }
        
        //Player death condition no more lives
        if(!playerReviving && isThisThePlayer && thisRoundManagerScript.ReturnPlayerLives() == 0){
            health = -100;
            thisRoundManagerScript.endTheRoundDueToDeath();
        }
    }

    private void RevivingPlayer(bool playerHadDied){
        //FIXME: Add player Revive effect?
        if(playerHadDied){
            float tempTimeForRevive = timeForRevive; 
            if(tempTimeForRevive > 0 & health < thisGlobalGameManager.ReturnPlayerStats().playerMaxHealth){
                timeForRevive -= Time.deltaTime;
                isInvulnerable = true;
                health = thisGlobalGameManager.ReturnPlayerStats().playerMaxHealth;
            }else{
                thisRoundManagerScript.TakePlayerLives();
                if(GameObject.Find("playerRevivesSfx(Clone)")==null){Instantiate(playerRevivesSfx);}
                isInvulnerable = false;
                playerReviving = false;
            }
        }
    }


    void commitDeath(){
        
        //What happens when a boss dies
        if(isThisABoss){

            Time.timeScale = 0.25f;

            Instantiate(deathEffect, locationUponDeath, Quaternion.identity);

            bossIsDead = true;
            
            BossKilledRoundEnd(thisEnemy.name);

        }

        //What happens when a normal enemy dies
        if(!isThisThePlayer){
        thisAgent.enabled = false;
        }
        
        if(!isThisABoss){
        //NOTE: You NEED to implement this codeblock for sfx!
        locationUponDeath = new Vector2(transform.position.x, transform.position.y);

        //Tell the round manager an enemy was killed to make room for spawn cap!
        thisRoundManagerScript.EnemyKilled();

        DropLoot(willDropLoot);

        Instantiate(deathEffect, locationUponDeath, Quaternion.identity);

        DestroyNDetachChildren();

        Destroy(this.gameObject);
        }

    }

    public void DropLoot(bool canDropLoot){
        if(canDropLoot){
            
            switch(gameObject.name){
                case "CassavaSlime(Clone)":
                thisRoundManagerScript.AddCassavaSlimeBallsThisRound(1);
                break;
                case "CassavaSlimeKnight(Clone)":
                thisRoundManagerScript.AddCassavaSlimeBallsThisRound(1);
                break;
                case "CassavaSlimeKing":
                thisRoundManagerScript.AddCassavaSlimeBallsThisRound(1);
                break;
                case "PandanShooter(Clone)":
                thisRoundManagerScript.AddPandanLeavesThisRound(1);
                break;
                case "BananaShaman(Clone)":
                thisRoundManagerScript.AddBananaMinisThisRound(1);
                break;
                case "PricklyStrawberry(Clone)":
                thisRoundManagerScript.AddStrawberryMinisThisRound(1);
                break;
                case "MangoMauler(Clone)":
                thisRoundManagerScript.AddMangoMinisThisRound(1);
                break;
                case "UnderGroundUbe(Clone)":
                thisRoundManagerScript.AddtUbeMinisThisRound(1);
                break;
            }
        }
    }

    public int RetrieveMaxHealthFromHealth_Universal(){
        return health;
    }

    /*Below are methods that check what THIS object collides with and what "team" they are on
    Then, does respective actions*/ 
    /// <summary>
    /// THE SCRIPTS BELOW ARE MORE FOR DEALING DAMAGE TO a different entity
    /// </summary>

    void OnCollisionEnter2D(Collision2D collision){
        //Debug.Log("Collision Started!");

        tempInvCountdown = tempInvTime;
        
        //NOTE: "isUnhittable is used for spawners / enemies with special conditions"
        if(!isUnhittable){
        if(collision.gameObject.CompareTag("PlayerWeapon")){
            //If this collided with the player's weapons, trigger possible loot drop upon death.
                    willDropLoot = true;
        }
        }

        if(collision.gameObject.CompareTag("Player")) {

                //Temp Attack block
                    if(!canAttackRanged){
                        //Debug.Log(gameObject.name + "Sending damage!");
                        collision.gameObject.GetComponent<IDamageable>().damageThis(attackPoints);
                }

        }else{
            if(collision.gameObject.CompareTag("Enemy")) {
            collision.gameObject.GetComponent<IDamageable>().damageThis(attackPoints);
            }
        }
        
    }

    void OnCollisionStay2D(Collision2D collision){
        //Debug.Log("Collision Continues!");

        if(collision.gameObject.CompareTag("Player")) {

                //Temp Attack block
                    if(!canAttackRanged){
                        //Debug.Log(gameObject.name + "Sending damage!");
                        collision.gameObject.GetComponent<IDamageable>().damageThis(attackPoints);
                }

        }
    }

    void OnCollisionExit2D(Collision2D collision){
        //Debug.Log("Collision is gone!");
    }

    /*What "team" is this entity on? Using Damage interface,
    does respective actions*/
    public void damageThis(int damage)
    {
         // If there's even damage in this collision
        if (damage>0){

        // If the time is below the threshold, add the delta time
        if(!amITakingDamage){
			if (tempInvCountdown < tempInvTime) {
				tempInvCountdown += Time.deltaTime;
                //Debug.Log("The time passed each frame: " + Time.deltaTime.ToString());
                
                if(tempInvCountdown >= tempInvTime){
                    thisSpriteRenderer.material.color = Color.white;
                }
                
			}else{
                
                if (isThisThePlayer && !isInvulnerable){
                    
                    thisSpriteRenderer.material.color = chosenRedColor;
                    //Debug.Log(gameObject.name + "getting damage!");
                    Instantiate(gotHitEffect, gameObject.transform);
                    health -= damage;
                    stillAliveChecker();
                    
                }

                if(!isThisThePlayer && !isInvulnerable){

                    willDropLoot = true;
                    thisSpriteRenderer.material.color = chosenRedColor;
                    //Debug.Log(gameObject.name + "getting damage!");
                    Instantiate(gotHitEffect, gameObject.transform);
                    health -= damage;
                    stillAliveChecker();
                    
                }

                //Reset timer 
                tempInvCountdown = 0;
            }    
        }
    }
    }

    void resetNormalSpriteColor(Color thisSpriteColor){
        
            if(thisSpriteColor != Color.white){
                amITakingDamage = true;
                //Debug.Log(colorChangeVelocity.ToString());
                colorChangeVelocity = Color.white - Color.red; 
                thisSpriteRenderer.material.color += colorChangeVelocity * Time.deltaTime;

                if(tempColorChangetime>=0){
                    tempColorChangetime -= Time.deltaTime;
                    //Debug.Log(tempColorChangetime);
                }else{
                    amITakingDamage = false;
                    tempColorChangetime = tempInvTime;
                    thisSpriteRenderer.material.color = Color.white;
                }
            }

    }

    //Detatches Children Prefabs to avoid update errors that the children still exist 
    //Despite parent being destroyed.
    private void DestroyNDetachChildren()
    {
        if (Application.isPlaying)
        {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                GameObject childObject = transform.GetChild(i).gameObject;
                if (childObject.activeSelf)
                {
                    Destroy(childObject);
                }
            }
        }
        transform.DetachChildren();
    }

    public int ReturnThisHealth(){
        return health;
    }

    //FIXME: edit this for when the round ends due to boss kill
    // (ADD UI)
    public void BossKilledRoundEnd(string thisEnemyName){
        thisRoundManagerScript.slowmoTime -= Time.unscaledDeltaTime;

        //FIXME: Note - there is a bug where the loot is MASSIVELY increaing
        //Lest you have a timer?
        
        DropLoot(willDropLoot);
        if(thisRoundManagerScript.slowmoTime <= 0){
            if(thisRoundManagerScript.slowmoTime <= 20){
                willDropLoot = false;
            }
            Time.timeScale = 1;
            thisRoundManagerScript.endTheRoundViaBossKill(thisEnemyName);
        }
    }
}
