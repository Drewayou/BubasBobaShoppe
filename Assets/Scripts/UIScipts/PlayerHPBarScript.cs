using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerHPValAndUIScript : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Drop the player object to track HP bar below")]
    [Header("Player Object for the round")]
    GameObject player;

    [SerializeField]
    [Tooltip("Drop the player object to track HP bar below")]
    [Header("Player Object for the round")]
    GameObject TextObject;

    [SerializeField]
    [Tooltip("Drop the player object to track HP bar below")]
    [Header("Player Object for the round")]
    GameObject UIHealthObject;
    
    Health_Universal thePlayersHealth;

    GameManagerScript thisOverallGameData;

    float playerMaxHealthOverInput;


    //[Tooltip("Drop the player object to track HP bar below")]
    //[Header("Player Object for the round")]
    TMP_Text thisUIHealthText;

    //float healthBarMultiplier = 1f;

    UnityEngine.UI.Image thisUIImage;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        thePlayersHealth = player.GetComponent<Health_Universal>();
        thisUIHealthText = TextObject.GetComponent<TMP_Text>();
        thisUIImage = UIHealthObject.GetComponent<UnityEngine.UI.Image>();

        //Pull player's currently saved max HP
        thisOverallGameData = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
        
    }

    // Update is called once per frame
    void Update()
    {
        playerMaxHealthOverInput = thisOverallGameData.ReturnPlayerStats().playerMaxHealth;
        thisUIHealthText.text = thePlayersHealth.ReturnThisHealth() + "/" + playerMaxHealthOverInput;
        thisUIImage.fillAmount = thePlayersHealth.ReturnThisHealth() / playerMaxHealthOverInput;
    }
}
