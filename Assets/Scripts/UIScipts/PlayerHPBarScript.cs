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

    Player_Controller thePlayersController;

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
        thePlayersController = player.GetComponent<Player_Controller>();

        playerMaxHealthOverInput = thePlayersController.getPlayerMaxHealth();
    }

    // Update is called once per frame
    void Update()
    {
        thisUIHealthText.text = thePlayersHealth.health + "/" + thePlayersController.getPlayerMaxHealth().ToString();
        thisUIImage.fillAmount = thePlayersHealth.health / playerMaxHealthOverInput;
    }
}