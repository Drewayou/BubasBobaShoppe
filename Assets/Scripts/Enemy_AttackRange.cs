using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackRange : MonoBehaviour
{
    GameObject thisEnemyMainObject;

    [SerializeField]
    [Header("Projectile")]
    [Tooltip("If the enemy does a ranged attack, use this projectile")]
    GameObject thisEnemyProjectiles;
    Enemy_Controller thisEnemyController;
    CircleCollider2D attackRadius;

    Health_Universal thisEnemyHealth;

    [SerializeField]
    [Header("Projectile Speed")]
    [Tooltip("If the enemy does a ranged attack, set the projectile speed here")]
    public float rangedAttackProjectileSpeed;
    public float attackRadiusfloat, tempAttackBuildupSaved;

    public bool isAttacking, inAnAttackAnim;

    public string whichEnemyAttackIsThis;

    // Start is called before the first frame update
    void Start()
    {
        //Set gameobject memory
        //thisEnemyMainObject = GetComponentInParent<GameObject>();

        //Get enemy controller for this object 
        thisEnemyController = GetComponentInParent<Enemy_Controller>();

        //Get enemy Num Val
        whichEnemyAttackIsThis = thisEnemyController.gameObject.name;

        //Set up circle collision trigger for attack radius
        attackRadius = GetComponent<CircleCollider2D>();
        attackRadius.radius = attackRadiusfloat;

        tempAttackBuildupSaved = thisEnemyController.attackBuildUp;

        thisEnemyMainObject = GameObject.Find("PandanShooter");
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
            if(thisEnemyMainObject != null){
            thisEnemyController.thisAgent.isStopped = false;

            isAttacking = false;
            thisEnemyController.isCurrentlyAttacking = isAttacking;
        }
        }
        
    }

    public bool CheckIfAttacking(){
        return isAttacking;
    }

    public bool CheckIfInAttackingAnimation(){
        return inAnAttackAnim;
    }
    
    public IEnumerator WaitSeconds(float num)
    {
        yield return new WaitForSecondsRealtime(num);
        thisEnemyController.thisAgent.isStopped = false;
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

        case "PandanShooter(Clone)":

            //THIS IS THE PANDAN'S CUSTOM ATTACK
            //
            //PandanShoots a prefab at the character
            //

        if(thisEnemyController.nextAttackIn <= 0){
            inAnAttackAnim = true;
            }

        if(isAttacking && inAnAttackAnim){
           if(thisEnemyController.attackBuildUp <=0){

            //Debug.LogWarning(thisEnemyProjectiles.name + "Fired");
            //Debug.LogWarning(transform.position + "Locatiion");

            Instantiate (thisEnemyProjectiles,transform.position,transform.rotation,transform);

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

        if(thisEnemyController.nextAttackIn <= 0){
            inAnAttackAnim = true;
            }

        if(isAttacking && inAnAttackAnim){
           if(thisEnemyController.attackBuildUp <=0){

            //Debug.LogWarning(thisEnemyProjectiles.name + "Fired");
            //Debug.LogWarning(transform.position + "Locatiion");

            Instantiate (thisEnemyProjectiles,transform.position,transform.rotation,transform);

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
        
        default:

        break;
        }
    }
}
