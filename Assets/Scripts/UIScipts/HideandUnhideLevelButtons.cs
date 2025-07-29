using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideandUnhideShopBoughtButtons : MonoBehaviour
{
    //Initialize game manager object
    GameManagerScript thisGameManagerScript;

    [SerializeField]
    [Header("What Buttons can show up after buying them?")]
    GameObject Level2Button,Level3Button,Level4Button,GoToBossPageButton;
    // Start is called before the first frame update
    void Start()
    {
        thisGameManagerScript = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(thisGameManagerScript.ReturnPlayerStats().unlockedLeve2){
            Level2Button.SetActive(true);
        }
        if(thisGameManagerScript.ReturnPlayerStats().unlockedLeve3){
            Level3Button.SetActive(true);
        }
        if(thisGameManagerScript.ReturnPlayerStats().unlockedLeve4){
            Level4Button.SetActive(true);
        }
        if(thisGameManagerScript.ReturnPlayerStats().unlockedLeve2 && thisGameManagerScript.ReturnPlayerStats().unlockedLeve3 && thisGameManagerScript.ReturnPlayerStats().unlockedLeve4){
            thisGameManagerScript.setPlayerAndBossFight1Available();
        }
        if(thisGameManagerScript.ReturnPlayerStats().unlockedBossFight){
            GoToBossPageButton.SetActive(true);
        }
    }
}
