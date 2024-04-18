using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoshMree : MonoBehaviour
{
    //FIXME: Do this script tomorrow <3 You got this!!!
    //Get 2d Circle collider of this NPC
    private CircleCollider2D thisNPCInteractCollider;

    //Get interaction AlertBubble Object
    [SerializeField]
    [Header("InteractionAlert")]
    [Tooltip("This will be used for showing that this is an interactable NPC")]
    GameObject interactionAlert;

    //Get interaction textbox display area
    [SerializeField]
    [Header("UI Display Area Object")]
    [Tooltip("Grab the object that hosts the Dialoge")]
    GameObject uIPlaceToDisplay;

    //Get interaction textbox display area
    [SerializeField]
    [Header("DialoguePrefab")]
    [Tooltip("Grab the prefab Dialoge-Box")]
    GameObject thisDialogeBox;

    //The text field from the aboxe Dialoge box prefab
    TMP_Text dialogeBoxText;

    //IN BEATA, PULL TEXTS FROM JSON FILE AND PUT THEM INTO AN ARRAY?

    // Start is called before the first frame update
    void Start()
    {
        dialogeBoxText = thisDialogeBox.GetComponent<TMP_Text>();
        thisNPCInteractCollider = this.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        
    }

    public void StartInteraction(){
        Time.timeScale = 0;
        Instantiate(thisDialogeBox, uIPlaceToDisplay.transform);
        GameObject.Find("TextBox(Clone)");
    }
}
