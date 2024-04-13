using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeScript : MonoBehaviour
{

    [SerializeField]
    [Header("InteractableGameObject")]
    [Tooltip("To Get the game object this script is tied to")]
    GameObject thisObject;

    [SerializeField]
    [Header("Damage points")]
    [Tooltip("How many damage points does this do to things that toutch it's collision?")]
    public int damagepoints;

    [SerializeField]
    [Header("ThisCollider")]
    [Tooltip("What is the collider for this collision?")]
    PolygonCollider2D thisCollider;

    // Start is called before the first frame update
    void Start()
    {
        thisCollider = thisObject.GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy")) {
            collision.gameObject.GetComponent<IDamageable>().damageThis(damagepoints);
        }
    }
}
