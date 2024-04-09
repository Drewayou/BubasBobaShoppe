using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootThisProjectilePrefab : MonoBehaviour
{
        private GameObject playerObjectForLocation;

        private Enemy_AttackRange thisEnemyAttackRange;

        private Vector3 directionTowardsPlayer;

        [Header("Projectile Speed")]
        [Tooltip("Speed of which this projectile will go along a vector")]
        public float projectileSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerObjectForLocation = GameObject.Find("Player");
        directionTowardsPlayer = (this.transform.position - playerObjectForLocation.transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= directionTowardsPlayer * projectileSpeed * Time.fixedDeltaTime;
    }
}
