using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NpcChatScriptCassavaKing : MonoBehaviour
{
    //Place the camera on the boss's position
    [SerializeField]
    [Header("CassavaKingBoss")]
    [Tooltip("Grab the Boss's object to know where to put camera before cutscene")]
    GameObject bossObjectLocation;

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
    [Header("Camera")]
    [Tooltip("Grab the Camera object from the player")]
    GameObject playerCamera;

    //Get interaction textbox display area from UI and place it in here
    [SerializeField]
    [Header("Bubba(Player)")]
    [Tooltip("Grab the Player object to know where to put camera after cutscene")]
    GameObject player;

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

    public bool playerInteractingButton = false, hadWentToNextDia = false, cutSceneHappening = false;

    //Which Npc Chat Interaction Is This? (They're labled in comments in the scripts);
    public int npcChatScriptIndex = 0;

    //What text is next?
    int chatIndex = 0;

    public float chatTimer = 5f, chatButtonDelay = 2f;

    NpcDiaStructure thisDialoguePulled;

    Vector3 playerOriginalCameraPos;

    //IN BEATA, PULL TEXTS FROM JSON FILE AND PUT THEM INTO AN ARRAY?

    // Start is called before the first frame update
    void Start()
    {
        //thisDialoguePulled = new NpcDiaStructure();
        retrieveDialogueInfo(npcChatScriptIndex);
        thisNPCInteractCollider = this.GetComponent<CircleCollider2D>();
        isChatting = false;

        //Save Camera pos
        playerOriginalCameraPos = new Vector3 (player.transform.position.x, -.25f, -10f);
    }

    // Update is called once per frame
    void Update()
    {
        if(chatTimer <= 0){
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
        }

        if(chatTimer >= 0){
        chatTimer -= Time.fixedUnscaledDeltaTime;
        }
        
    }

    void OnTriggerEnter2D(UnityEngine.Collider2D thisNPCInteractCollider)
    {
        if(chatTimer <= 0){
            if(thisInteractionStartACutScene){
                Debug.Log("Collider activated");
                if(thisNPCInteractCollider.gameObject.CompareTag("Player"))
                    inInteractRange = true;
                    isChatting = true;
                    cutSceneHappening = true;
                    Time.timeScale = 0f;
            }
        }

    }

    public void playerRequestedInteraction(){
        if(isChatting){
            playerInteractingButton = true;
            StartInteractionOrContinue();
        }
    }

    public void cutSceneHappens(bool isCutSceneHappening){
        if(isCutSceneHappening){
            Vector3 bossPositionWithPlayerPos = new Vector3(bossObjectLocation.transform.position.x, bossObjectLocation.transform.position.y, playerCamera.transform.position.z);
            if(playerCamera.transform.position != bossPositionWithPlayerPos){
                    playerCamera.transform.position = bossPositionWithPlayerPos;
                }
            }
    }

    //Below are used by input actions!
    public void StartInteractionOrContinue(){
        //If E was pressed and player was in InteractionRange with the boss->
        if(playerInteractingButton && thisInteractionStartACutScene && isChatting){
        cutSceneHappens(cutSceneHappening);
        NextDialogue();
        }
    }

    public void NextDialogue(){
        if(chatTimer<=0){
            chatIndex += 1;
            playerInteractingButton = false;
            chatTimer = chatButtonDelay;
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
            thisEntityTalkingAnim.Play("SlimeKingCard");
            dialogeBoxText.text = "I do not know why you are here...";
            break;
            case 1:
            thisEntityTalkingAnim.Play("SlimeKingCard");
            dialogeBoxText.text = "But i've heard from the whispers of my constituents of what you've done.";
            break;
            case 2:
            thisEntityTalkingAnim.Play("SlimeKingCard");
            dialogeBoxText.text = "If there is truely no other way of detering a battle...";
            break;
            case 3:
            thisEntityTalkingAnim.Play("SlimeKingCard");
            dialogeBoxText.text = "I hope you know what you are doing...";
            break;
            case 4:
            thisEntityTalkingAnim.Play("SlimeKingCard");
            dialogeBoxText.text = "BECAUSE I SHALL DEFEND MY LAND UNTIL MY LAST BREATH!!!";
            break;
            case 5:
            isChatting = false;
            Time.timeScale = 1f;

            playerCamera.transform.position = player.transform.localPosition;
            Vector3 PlayerPos = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
            playerCamera.transform.position = PlayerPos;
            
            chatIndex = 100;
            thisInteractionStartACutScene = false;
            thisNPCInteractCollider.enabled = !thisNPCInteractCollider.enabled;
            cutSceneHappening = false;
            break;

            //If int was misnumbered, reset to default
            default:
            isChatting = false;
            chatIndex = -1;
            break;
        }
    }
    
}
