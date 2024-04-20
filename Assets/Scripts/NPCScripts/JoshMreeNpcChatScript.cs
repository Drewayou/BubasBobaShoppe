using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class JoshMreeNpcChatScript : MonoBehaviour
{
    //FIXME: Do this script tomorrow <3 You got this!!!
    //Get 2d Circle collider of this NPC
    public CircleCollider2D thisNPCInteractCollider;

    //Get interaction AlertBubble Object
    [SerializeField]
    [Header("InteractionAlert")]
    [Tooltip("This will be used for showing that this is an interactable NPC")]
    GameObject interactionAlert;

    //Get interaction textbox display area from UI and place it in here
    [SerializeField]
    [Header("DialoguePrefab")]
    [Tooltip("Grab the Dialoge-Box from round UI")]
    GameObject thisDialogeBox;

    //Get interaction textbox display area from UI and place it in here
    [SerializeField]
    [Header("Who is talking Image UI")]
    [Tooltip("Grab the Co-Responding NPC Image Card Prefab")]
    Image thisEntityTalking;

    //Get interaction Entity Image UI and place it in here
    [SerializeField]
    [Header("Who is talking Image UI Anim")]
    [Tooltip("Grab the Co-Responding NPC Image Card Prefab")]
    Animator thisEntityTalkingAnim;

    //Get interaction textbox display area from UI and place it in here
    [SerializeField]
    [Header("TextBox Popup UI Animation")]
    [Tooltip("Grab the Co-Responding Dialoge-Box from round UI to grab Anim")]
    Animator thisTextBoxAnim;

    //The text field from the aboxe Dialoge box prefab
    //Get text field from UI and place it in here
    [SerializeField]
    [Header("TextBox Dialogue")]
    [Tooltip("Grab the Co-Responding Dialoge-Box from round UI to Text")]
    TMP_Text dialogeBoxText;

    public bool inInteractRange = false;

    public bool isChatting = false;

    public bool thisInteractionStartACutScene = false;

    public bool playerInteractingButton = false, hadWentToNextDia = false;

    //Which Npc Chat Interaction Is This? (They're labled in comments in the scripts);
    public int npcChatScriptIndex = 0;

    //What text is next?
    int chatIndex = 0;

    float chatTimer = 1f, chatButtonDelay = 1f;

    NpcDiaStructure thisDialoguePulled;

    //IN BEATA, PULL TEXTS FROM JSON FILE AND PUT THEM INTO AN ARRAY?

    // Start is called before the first frame update
    void Start()
    {
        //thisDialoguePulled = new NpcDiaStructure();
        retrieveDialogueInfo(npcChatScriptIndex);
        thisNPCInteractCollider = this.GetComponent<CircleCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if(isChatting){
            thisDialogeBox.SetActive(true);
            thisTextBoxAnim.Play("TextBoxPopsUp");
            retrieveDialogueInfo(chatIndex);
            //dialogeBoxText.text = getTextFromJsonIndex(chatScriptIndex);
        }else{
            if(thisDialogeBox.activeSelf){
            thisTextBoxAnim.Play("TextBoxRetracts");
            }
            //dialogeBoxText.text = "";
        }
        if(playerInteractingButton){
            chatTimer -= Time.deltaTime;
        }else{
            chatTimer = chatButtonDelay;
        }

        StartInteractionOrContinue();
        
        if(chatTimer > 0f){chatTimer -= Time.deltaTime;}
    }

    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            inInteractRange = true;
            //Popup Interact E  Alert
            Vector2 spawnAboveNPC = new Vector2 (transform.position.x, transform.position.y + .22f);
            Instantiate(interactionAlert, spawnAboveNPC, transform.rotation, transform);
        }
    }

    void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            inInteractRange = false;
            isChatting = false;
            chatIndex = -1;
        }
    }

    public void playerRequestedInteraction(){
        playerInteractingButton = true;
    }

    //Below are used by input actions!
    public void StartInteractionOrContinue(){
        //If E was pressed and player was in InteractionRange ->
        if(playerInteractingButton && inInteractRange && !thisInteractionStartACutScene){
        isChatting = true;
        NextDialogue(chatTimer);
        }
    }

    public void NextDialogue(float chatSkipCooldown){
        if(chatSkipCooldown<=0){
            chatIndex += 1;
            chatTimer = chatButtonDelay;
            playerInteractingButton = false;
        }
    }

    public void EndInteraction(){
        //If Q was pressed and player was in InteractionRange ->
        if(playerInteractingButton && inInteractRange && !thisInteractionStartACutScene){
        isChatting = false;
        playerInteractingButton = false;
        chatIndex = -1;
        }
    }

    //Used for cutscene / stopping time when chatting
    public void StartInteractionNPauseTime(){
        //If X was pressed and player was in InteractionRange ->
        if(playerInteractingButton && inInteractRange && thisInteractionStartACutScene){
        Time.timeScale = 0;
        isChatting = true;
        }
    }

    public void EndtInteractionNResumeTime(){
        //If X was pressed and player was in InteractionRange ->
        if(playerInteractingButton && inInteractRange && thisInteractionStartACutScene){
        Time.timeScale = 1.0f;
        isChatting = false;
        }
    }

    //NOTE : This needs to be connected to a JSON object 
    public void getTextFromJsonIndex(int inxedText){
        
    }

    //FIXME: NOTE; will use this for now...
    public void retrieveDialogueInfo(int thisCharacterInteractionIndexPulled){
        switch(thisCharacterInteractionIndexPulled){
            case 0:
            thisEntityTalkingAnim.Play("JoshMreeCard");
            dialogeBoxText.text = "Hai, I'm Josh Mree!";
            break;
            case 1:
            thisEntityTalkingAnim.Play("BubaCard");
            dialogeBoxText.text = "Ello there... Are you lifting Rocks?";
            break;
            case 2:
            thisEntityTalkingAnim.Play("JoshMreeCard");
            dialogeBoxText.text = "Yes! The local Gym doesn't have heavy enough weights so I come here.";
            break;
            case 3:
            thisEntityTalkingAnim.Play("BubaCard");
            dialogeBoxText.text = "Well be careful out here... The fruit monsters seem to be getting tougher.";
            break;
            case 4:
            thisEntityTalkingAnim.Play("JoshMreeCard");
            dialogeBoxText.text = "Don't worry Mister! I'm good. Even moonwalked away from an Ube monster recently!";
            break;
            case 5:
            thisEntityTalkingAnim.Play("BubaCard");
            dialogeBoxText.text = "Moonwalked? Like Michale Jackleson?";
            break;
            case 6:
            thisEntityTalkingAnim.Play("JoshMreeCard");
            dialogeBoxText.text = "Yeah! I can show you how to! Hahaha.";
            break;
            case 7:
            thisEntityTalkingAnim.Play("JoshMreeCard");
            dialogeBoxText.text = "Press 'm' on keyboard and you'll be moonwalking in no time!";
            break;
            case 8:
            thisEntityTalkingAnim.Play("BubaCard");
            dialogeBoxText.text = "Huh... Alright, thanks Josh. I'll give that a try...";
            break;
            case 9:
            thisEntityTalkingAnim.Play("JoshMreeCard");
            dialogeBoxText.text = "See ya around Sir!";
            break;
            case 10:
            isChatting = false;
            chatIndex = -1;
            break;

            //If int was misnumbered, reset to default
            default:
            isChatting = false;
            chatIndex = -1;
            break;
        }
    }
    
}
