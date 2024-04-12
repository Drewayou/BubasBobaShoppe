using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

public class TransparencyCollisionScript : MonoBehaviour
{
    [SerializeField]
    [Header("Object Tied To this Script")]
    [Tooltip("What Object should this transparency script pull")]
    public GameObject thisObject;

    [SerializeField]
    [Header("Object Tied To this Script")]
    [Tooltip("What Material should this transparency script put?")]
    public Material materialOfThisObject;

    private PolygonCollider2D thisObjectsPhysicsCollider;

    private SpriteRenderer thisSpriteRenderer;

    private Color colorSaved;

    // Start is called before the first frame update
    void Start()
    {
        thisObjectsPhysicsCollider = thisObject.GetComponent<PolygonCollider2D>();
        thisSpriteRenderer = thisObject.GetComponentInParent<SpriteRenderer>();
        colorSaved = thisSpriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay2D(){
        thisSpriteRenderer.color = new Color(255f, 255f, 255f, 0.75f);
        thisSpriteRenderer.sortingLayerName = "ForegroundFoliage";
    }

    public void OnTriggerExit2D(){
        thisSpriteRenderer.color = colorSaved;
        thisSpriteRenderer.sortingLayerName = "BackgroundFoliage";
    }
}
