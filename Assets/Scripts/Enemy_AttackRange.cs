using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackRange : MonoBehaviour
{
    GameObject thisEnemy;
    Enemy_Controller thisEnemyController;
    CircleCollider2D attackRadius;
    public float attackRadiusfloat, tempAttackBuildupSaved;

    public bool isAttacking, inAnAttackAnim;

    public int whichEnemyAttackIsThis;

    // Start is called before the first frame update
    void Start()
    {
        //Set gameobject memory
        thisEnemy = GetComponent<GameObject>();

        //Get enemy controller for this object 
        thisEnemyController = GetComponentInParent<Enemy_Controller>();

        //Get enemy Num Val
        whichEnemyAttackIsThis = thisEnemyController.whatEnemyValIsThis;

        //Set up circle collision trigger for attack radius
        attackRadius = GetComponent<CircleCollider2D>();
        attackRadius.radius = attackRadiusfloat;

        tempAttackBuildupSaved = thisEnemyController.attackBuildUp;
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
        
        thisEnemyController.agent.isStopped = true;
        
        isAttacking = true;
        thisEnemyController.isCurrentlyAttacking = isAttacking;
  
    }


    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        Debug.Log("PlayerLeftAttackRangeOfSlime!");

        if(thisEnemy != null){
        thisEnemyController.agent.isStopped = false;

        isAttacking = false;
        thisEnemyController.isCurrentlyAttacking = isAttacking;
        }
        
    }

    public bool CheckIfAttacking(){
        return isAttacking;
    }
    
    public IEnumerator WaitSeconds(float num)
    {
        yield return new WaitForSecondsRealtime(num);
        thisEnemyController.agent.isStopped = false;
    }

    public void DoCustomAttack(int enemySelected){

        switch(enemySelected){
        
        case 1:

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

            thisEnemyController.agent.isStopped = false;

            }else{
                thisEnemyController.attackBuildUp -= Time.deltaTime;
            }
        }
            break;

        //END OF SLIME CUSTOM ATTACK
        //
        //
        //
        
        default:

        break;
        }
    }
}
