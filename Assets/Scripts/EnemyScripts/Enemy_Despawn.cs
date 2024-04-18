using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Despawn : MonoBehaviour
{
    [SerializeField]
    [Header("Enemy Prefab")]
    [Tooltip("The top game object prefab to despawn")]
    GameObject parentEnemyObject;

    [SerializeField]
    [Header("Enemy AgroObject")]
    [Tooltip("The Agro Radius of this enemy prefab")]
    GameObject enemyAgroObject;
    private Enemy_AgroRadius attachedAgroRadius;

    private RoundManagerScript gamesRoundManager;

    //Below are two floats to count down an enemy despawn. Let's aim for 20 secs of no activity / no agro / not in player radius.
    public float timeTillDespawnIsPossible = 20f, despawnCountDownStartsAt = 20f;

    // Start is called before the first frame update
    void Start()
    {
        despawnCountDownStartsAt = 20f;
        timeTillDespawnIsPossible = 20f;
        attachedAgroRadius = enemyAgroObject.GetComponent<Enemy_AgroRadius>();
        gamesRoundManager = GameObject.Find("RoundManager").GetComponent<RoundManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(!attachedAgroRadius.HasInteractedWithPlayer()){
            if(timeTillDespawnIsPossible >= 0){
                timeTillDespawnIsPossible -= Time.deltaTime;
            }else{
                timeTillDespawnIsPossible = despawnCountDownStartsAt;
                Despawn();
            }
        }
    }

    public void Despawn(){
        Destroy(parentEnemyObject);
        gamesRoundManager.EnemyKilled();
    }
}
