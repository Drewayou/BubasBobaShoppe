using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BossHPValAndUIScript : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Drop the Boss's object to track HP bar below")]
    [Header("Boss Object for the round")]
    GameObject Boss;

    [SerializeField]
    [Tooltip("Drop the Boss's text object to track HP bar below")]
    [Header("Boss text object for the round")]
    GameObject TextObject;

    [SerializeField]
    [Tooltip("Drop the Boss's object to track HP bar below")]
    [Header("Boss Object for the round")]
    GameObject UIHealthObject;
    
    [SerializeField]
    [Tooltip("Drop the Boss's object to track HP stats")]
    [Header("BossHealth Object for the round")]
    Health_Universal bossHealthScript;

    GameManagerScript thisOverallGameData;

    float bossMaxHealth, bossCurrentHealth;


    //[Tooltip("Drop the player object to track HP bar below")]
    //[Header("Player Object for the round")]
    TMP_Text thisUIHealthText;

    //float healthBarMultiplier = 1f;

    UnityEngine.UI.Image thisUIImage;

    // Start is called before the first frame update
    void Start()
    {
        bossMaxHealth = bossHealthScript.ReturnThisHealth();
        thisUIHealthText = TextObject.GetComponent<TMP_Text>();
        thisUIImage = UIHealthObject.GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Boss != null){
        bossCurrentHealth = bossHealthScript.ReturnThisHealth();
        thisUIHealthText.text = Boss.name + " | " + bossCurrentHealth + "/" + bossMaxHealth;
        thisUIImage.fillAmount = bossCurrentHealth / bossMaxHealth;
        }else{
            Destroy(TextObject);
            Destroy(UIHealthObject);
        }
    }
}
