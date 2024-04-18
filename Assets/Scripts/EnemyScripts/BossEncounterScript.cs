using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEncounterScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The Enemy Controller radius for when they're about to attack")]
    public Enemy_AttackRange thisAttackController;

    private CircleCollider2D thisCutSceneStartRadius;

    void Start(){
        thisCutSceneStartRadius = GetComponent<CircleCollider2D>();
    }

    //When Player enter's boss range, enable the boss to move via the attack script
    public void OnTriggerEnter2D(UnityEngine.Collider2D collision){
    if(collision.CompareTag("Player")){
            thisAttackController.HasCutscenceHappened = true;
        }
    }
}
