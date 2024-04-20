using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_AttackRange : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Drag The Enemy Prefab here")]
    GameObject thisEnemyMainObject;

    [SerializeField]
    [Header("Projectile")]
    [Tooltip("If the enemy does a ranged attack, use this projectile")]
    GameObject thisEnemyProjectiles;
    Enemy_Controller thisEnemyController;
    
    [SerializeField]
    [Header("RoundProjectileManager")]
    [Tooltip("Drag The EnemyProjectile object here")]
    GameObject thisRoundEnemyProjectile;

    [SerializeField]
    [Header("AttackSfx")]
    [Tooltip("Drag The EnemyAttackSfx object here")]
    GameObject thisRoundEnemyAttackSfx;
    
    [SerializeField]
    [Header("SpecialAttackPrefab")]
    [Tooltip("Drag The Special object attack here")]
    GameObject thisEnemySpecial;

    private RoundManagerScript thisRoundManager;

    //GetEnemyAgroRadius via object
    GameObject EnemyAgroObject;
    [SerializeField]
    Enemy_AgroRadius thisEnemyAgroRadius;
    CircleCollider2D attackRadius;

    PolygonCollider2D thisHitbox;

    Health_Universal thisEnemyHealth;

    [SerializeField]
    [Header("Projectile Speed")]
    [Tooltip("If the enemy does a ranged attack, set the projectile speed here")]
    public float rangedAttackProjectileSpeed;
    public float attackRadiusfloat, tempAttackBuildupSaved;

    public bool isAttacking, inAnAttackAnim, continuosAttackWhenInRange = false, HasCutscenceHappened = false;

    public string whichEnemyAttackIsThis;

    // Start is called before the first frame update
    void Start()
    {
        thisRoundManager = GameObject.Find("RoundManager").GetComponent<RoundManagerScript>();
        //Get enemy controller for this object 
        thisEnemyController = thisEnemyMainObject.GetComponent<Enemy_Controller>();

        thisEnemyHealth = thisEnemyMainObject.GetComponent<Health_Universal>();

        thisHitbox = thisEnemyMainObject.GetComponent<PolygonCollider2D>();

        //Get enemy agroRadius for this object
        //thisEnemyAgroRadius = EnemyAgroObject.GetComponent<Enemy_AgroRadius>();

        //Get enemy Num Val
        whichEnemyAttackIsThis = thisEnemyController.gameObject.name;

        //Set up circle collision trigger for attack radius
        attackRadius = GetComponent<CircleCollider2D>();
        attackRadius.radius = attackRadiusfloat;

        tempAttackBuildupSaved = thisEnemyController.attackBuildUp;

        //Empty GameObject to contain enemy Projectiles
        thisRoundEnemyProjectile = GameObject.Find("EnemyProjectiles");
    }

    // Update is called once per frame
    void Update()
    {
        DoCustomAttack(whichEnemyAttackIsThis);
        /*
        if(thisEnemyController.nextAttackIn <= 0){
            inAnAttackAnim = true;
        }

        if(isAttacking && inAnAttackAnim){
           if(thisEnemyController.attackBuildUp <=0){

            inAnAttackAnim = false;
            thisEnemyController.attackBuildUp = tempAttackBuildupSaved;

            thisEnemyController.agent.isStopped = false;

            }else{
                thisEnemyController.attackBuildUp -= Time.deltaTime;
            }
        }
        */
    }

    public void OnTriggerEnter2D(UnityEngine.Collider2D collision){
        
        if(collision.CompareTag("Player")){
        thisEnemyController.thisAgent.isStopped = true;
        
        isAttacking = true;
        thisEnemyController.isCurrentlyAttacking = isAttacking;
        }
  
    }


    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        //Debug.Log("PlayerLeftAttackRangeOfSlime!");

        if(collision.CompareTag("Player")){
            if(thisEnemyController.thisAgent.enabled){
            thisEnemyController.thisAgent.isStopped = false;

            isAttacking = false;
            thisEnemyController.isCurrentlyAttacking = isAttacking;
            }
        }
        
    }

    private void OnTriggerStay2D(UnityEngine.Collider2D collision)
    {
        //Debug.Log("PlayerLeftAttackRangeOfSlime!");

        if(continuosAttackWhenInRange){
        if(collision.CompareTag("Player")){
            if(thisEnemyController.thisAgent.enabled){
            thisEnemyController.thisAgent.isStopped = true;

            isAttacking = true;
            thisEnemyController.isCurrentlyAttacking = isAttacking;
            }
        }
        }
        
    }

    public bool CheckIfAttacking(){
        return isAttacking;
    }

    public bool CheckIfInAttackingAnimation(){
        return inAnAttackAnim;
    }
    
    public void DoCustomAttack(string enemySelected){

        switch(enemySelected){
        
        case "CassavaSlime(Clone)":

            //THIS IS THE SLIME'S CUSTOM ATTACK
            //
            //Slime winds up to attack player for a quick microsecond
            //

        if(thisEnemyController.nextAttackIn <= 0){
            inAnAttackAnim = true;
            }

        if(isAttacking && inAnAttackAnim){
           if(thisEnemyController.attackBuildUp <=0){

            inAnAttackAnim = false;
            thisEnemyController.attackBuildUp = tempAttackBuildupSaved;

            thisEnemyController.thisAgent.isStopped = false;

            isAttacking = false;
            thisEnemyController.isCurrentlyAttacking = isAttacking;

            }else{
                thisEnemyController.attackBuildUp -= Time.deltaTime;
            }
            }
            break;

            //END OF SLIME CUSTOM ATTACK
            //
            //
            //

        case "CassavaSlimeKnight(Clone)":

            //THIS IS THE CASSAVASLIMEKNIGHT'S CUSTOM ATTACK
            //
            //Slime winds up to attack player for a quick microsecond, very similar to the normal slime,
            //HOWEVER it is perpetually aggro'd to the player
            
        //Tell this knight to perpetually lock unto the player.
        isAttacking = false;
        thisEnemyAgroRadius.LockedOnPlayer = true;
        inAnAttackAnim = false;
        thisEnemyController.thisAgent.isStopped = false;
        thisEnemyController.isCurrentlyAttacking = false;
        thisEnemyController.attackBuildUp = 0;
        
        
            break;

            //END OF CASSAVASLIMEKNIGHT'S CUSTOM ATTACK
            //
            //
            //

        case "CassavaSlimeKing":

            //THIS IS THE CASSAVASLIMEKING'S CUSTOM ATTACK
            //
            //SlimeKing spawns enemies
            //Moreover it is perpetually aggro'd to the player
        if(HasCutscenceHappened){
        continuosAttackWhenInRange = true;
        

        thisEnemyAgroRadius.LockedOnPlayer = true;
    
        if(thisEnemyController.nextAttackIn <= 0){
            //inAnAttackAnim = true;
            }

        if(isAttacking){
           if(thisEnemyController.attackBuildUp <=0){

            inAnAttackAnim = true;
            thisEnemyController.attackBuildUp = tempAttackBuildupSaved;

            thisEnemyController.thisAgent.isStopped = false;

            isAttacking = false;
            thisEnemyController.isCurrentlyAttacking = isAttacking;

            //Below BOC checks if round hit spawn cap. If not, summon slime knights.
            if(!thisRoundManager.CheckIfSpawnCapIsReached()){
            Vector3 rightOfSlimeKing = new Vector3(transform.position.x + .7f, transform.position.y -.1f, transform.position.z);
            Instantiate (thisEnemyProjectiles,rightOfSlimeKing,transform.rotation,thisRoundEnemyProjectile.transform);


            Vector3 leftOfSlimeKing = new Vector3(transform.position.x - .7f, transform.position.y -.1f, transform.position.z);
            Instantiate (thisEnemyProjectiles,leftOfSlimeKing,transform.rotation,thisRoundEnemyProjectile.transform);

            thisRoundManager.enemiesSpawned += 5 ;
            }

            //FIXME: This spawns a collider and sends the player and ALL enemies around the slime king flying. Try to
            //Make it so that it teleports the slime king in a small radius and the player somewhere else in the said radius!
            //Special "Repulse effect?"
        
            //Teleport CassavaSlimeKingHere here
            Vector2 TeleportPointEnemeyHere = (Vector2)transform.position + (Random.insideUnitCircle * thisEnemyAgroRadius.agroRadiusfloat);

            if(checkIfPointOnNavMesh(TeleportPointEnemeyHere,thisEnemyAgroRadius.agroRadiusfloat)){
                
            thisEnemyMainObject.transform.position = TeleportPointEnemeyHere;
            
            };

            //Teleport Player here, with a possible distance of *3 the radius
            Vector2 TeleportPointPlayerHere = (Vector2)transform.position + (Random.insideUnitCircle * thisEnemyAgroRadius.agroRadiusfloat);

            if(checkIfPointOnNavMesh(TeleportPointPlayerHere,thisEnemyAgroRadius.agroRadiusfloat)){
                
            thisEnemyController.Target.transform.position = TeleportPointPlayerHere;

            Instantiate(thisEnemySpecial,thisEnemyController.Target.transform);
            
            };
            
            Instantiate(thisRoundEnemyAttackSfx);

            //Instantiate (thisEnemySpecial,transform.position,transform.rotation,thisRoundEnemyProjectile.transform);

            //Special attack done?

            }else{
                thisEnemyController.attackBuildUp -= Time.deltaTime;
                    }
                }
            }else{
                
                thisEnemyController.thisAgent.isStopped = true;
                
            }

            

            break;

            //END OF CASSAVASLIMEKING'S CUSTOM ATTACK
            //
            //
            //

        case "PandanShooter(Clone)":

            //THIS IS THE PANDAN'S CUSTOM ATTACK
            //
            //PandanShoots a prefab at the character
            //

        continuosAttackWhenInRange = true;

        if(thisEnemyController.nextAttackIn <= 0){
            inAnAttackAnim = true;
            }

        if(isAttacking && inAnAttackAnim){
           if(thisEnemyController.attackBuildUp <=0){

            //Debug.LogWarning(thisEnemyProjectiles.name + "Fired");
            //Debug.LogWarning(transform.position + "Locatiion");

            Instantiate (thisEnemyProjectiles,transform.position,transform.rotation,thisRoundEnemyProjectile.transform);

            /*FIXME: Try to make the pandan shooters run after attacking
            Vector3 directionAwayFromPlayer = (this.transform.position - thisEnemyController.GetPlayerLastSeenPosition()).normalized * -3;
            thisEnemyController.agent.SetDestination(directionAwayFromPlayer);
            StartCoroutine(WaitSeconds(3));
            */
            
            inAnAttackAnim = false;
            thisEnemyController.attackBuildUp = tempAttackBuildupSaved;

            thisEnemyController.thisAgent.isStopped = false;

            isAttacking = false;
            thisEnemyController.isCurrentlyAttacking = isAttacking;

            
            

            }else{
                thisEnemyController.attackBuildUp -= Time.deltaTime;
            }
            }
            break;

            //END OF PANDAN'S CUSTOM ATTACK
            //
            //
            //

        case "BananaShaman(Clone)":

            //THIS IS THE BANANA SHAMAN'S CUSTOM ATTACK
            //
            //Banana Shaman shoots a long-lasting, but slow Orb prefab at the character
            //

        continuosAttackWhenInRange = true;

        if(thisEnemyController.nextAttackIn <= 0){
            inAnAttackAnim = true;
            }

        if(isAttacking && inAnAttackAnim){
           if(thisEnemyController.attackBuildUp <=0){

            //Debug.LogWarning(thisEnemyProjectiles.name + "Fired");
            //Debug.LogWarning(transform.position + "Locatiion");

            Instantiate (thisEnemyProjectiles,transform.position,transform.rotation,thisRoundEnemyProjectile.transform);

            /*FIXME: Try to make the banana shaman try kamakazi Diving at the player after attacking
            Vector3 directionAwayFromPlayer = (this.transform.position - thisEnemyController.GetPlayerLastSeenPosition()).normalized * -3;
            thisEnemyController.agent.SetDestination(directionAwayFromPlayer);
            StartCoroutine(WaitSeconds(3));
            */
            
            inAnAttackAnim = false;
            thisEnemyController.attackBuildUp = tempAttackBuildupSaved;

            thisEnemyController.thisAgent.isStopped = false;

            isAttacking = false;
            thisEnemyController.isCurrentlyAttacking = isAttacking;

            
            

            }else{
                thisEnemyController.attackBuildUp -= Time.deltaTime;
            }
            }
            break;

            //END OF PANDAN'S CUSTOM ATTACK
            //
            //
            //

        case "PricklyStrawberry(Clone)":

            //THIS IS THE PRICKLYSTRAWBERRY'S CUSTOM ATTACK
            //
            //Prickly Strawberry quckly shoots a prickly seeds prefab at the character
            //

        continuosAttackWhenInRange = true;

        if(thisEnemyController.nextAttackIn <= 0){
            inAnAttackAnim = true;
            }

        if(isAttacking && inAnAttackAnim){
           if(thisEnemyController.attackBuildUp <=0){

            //Debug.LogWarning(thisEnemyProjectiles.name + "Fired");
            //Debug.LogWarning(transform.position + "Locatiion");

            Instantiate (thisEnemyProjectiles,transform.position,transform.rotation,thisRoundEnemyProjectile.transform);
            //StartCoroutine(PricklyStrawberryShootBuffer());
            //Instantiate (thisEnemyProjectiles,transform.position,transform.rotation,transform);
            //StartCoroutine(PricklyStrawberryShootBuffer());
            //Instantiate (thisEnemyProjectiles,transform.position,transform.rotation,transform);

            /*FIXME: Try to make the Prickly Straberry Randomly move while shooting at the player.
            Vector3 directionAwayFromPlayer = (this.transform.position - thisEnemyController.GetPlayerLastSeenPosition()).normalized * -3;
            thisEnemyController.agent.SetDestination(directionAwayFromPlayer);
            StartCoroutine(WaitSeconds(3));
            */

            StartCoroutine(PricklyStrawberryShootBuffer());
            
            inAnAttackAnim = false;
            thisEnemyController.attackBuildUp = tempAttackBuildupSaved;

            thisEnemyController.thisAgent.isStopped = false;

            isAttacking = false;
            thisEnemyController.isCurrentlyAttacking = isAttacking;

            }else{
                thisEnemyController.attackBuildUp -= Time.deltaTime;
            }
            }
            break;

            //END OF PRICKLYSTRAWBERRY'S CUSTOM ATTACK
            //
            //
            //

        case "MangoMauler(Clone)":

            //THIS IS THE MANGOMAULER'S CUSTOM ATTACK
            //
            //This Mango winds up to attack player with a heavy punch ;) (Pun intended)
            //

        if(thisEnemyController.nextAttackIn <= 0){
            inAnAttackAnim = true;
            }

        if(isAttacking && inAnAttackAnim){
           if(thisEnemyController.attackBuildUp <=0){

            inAnAttackAnim = false;
            thisEnemyController.attackBuildUp = tempAttackBuildupSaved;

            //thisEnemyController.thisAgent.acceleration = 16f;
            thisEnemyController.thisAgent.speed = .5f;

            isAttacking = false;
            thisEnemyController.isCurrentlyAttacking = isAttacking;
            thisEnemyController.thisAgent.isStopped = false;

            Instantiate(thisRoundEnemyAttackSfx);

            }else{
                thisEnemyController.attackBuildUp -= Time.deltaTime;
                //thisEnemyController.thisAgent.acceleration = 8f;
                thisEnemyController.thisAgent.speed = 1.5f;
                thisEnemyController.thisAgent.isStopped = false;
            }
            }
            break;

            //END OF MANGOMAULER'S CUSTOM ATTACK
            //
            //
            //

        case "UnderGroundUbe(Clone)":

            //THIS IS THE UNDERGROUNDUBE'S CUSTOM ATTACK
            //
            //This Mango winds up to attack player with a heavy punch ;) (Pun intended)
            //

        //continuosAttackWhenInRange = true;

        if(thisEnemyController.nextAttackIn <= 0){
            inAnAttackAnim = true;
            }

        if(isAttacking && inAnAttackAnim){
           if(thisEnemyController.attackBuildUp <=0){

            inAnAttackAnim = true;
            thisEnemyController.attackBuildUp = tempAttackBuildupSaved;

            //thisEnemyController.thisAgent.acceleration = 16f;
            thisEnemyController.thisAgent.speed = 1.0f;
            
            isAttacking = false;
            thisEnemyController.isCurrentlyAttacking = isAttacking;
            thisEnemyController.thisAgent.isStopped = false;

            //FIXME: have an underground bury dig sfx
            //Instantiate(thisRoundEnemyAttackSfx);
            
            }else{
                inAnAttackAnim = false;
                thisEnemyController.attackBuildUp -= Time.deltaTime;
                //thisEnemyController.thisAgent.acceleration = 8f;
                thisEnemyController.thisAgent.speed = 0.25f;
                thisEnemyController.thisAgent.isStopped = false;
                
            }
            //thisEnemyHealth.isInvulnerable = false;
            thisHitbox.isTrigger = false;
            }else{
                //thisEnemyHealth.isInvulnerable = true;
                thisHitbox.isTrigger = true;
            }
            break;

            //END OF UNDERGROUNDUBE'S CUSTOM ATTACK
            //
            //
            //
        
        default:

        break;
        }
    }

    IEnumerator PricklyStrawberryShootBuffer(){
        yield return new WaitForSeconds(.2f);
    }

    //Below code was pulled & edited from unity documentation "AI.NavMesh.SamplePosition.html"
    //This supposedly checks if a point is on the navmesh within a range and returns a bool.
    bool checkIfPointOnNavMesh(Vector3 randomPointSelected, float DesiredRadius)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPointSelected, out hit, DesiredRadius, NavMesh.AllAreas))
        {
            return true;
        }
        return false;
    }
}
