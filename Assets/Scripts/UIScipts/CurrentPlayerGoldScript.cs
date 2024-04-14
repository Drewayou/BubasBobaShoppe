using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentPlayerGoldScript : MonoBehaviour
{

    GameManagerScript overAllGameManagerScript;

    //[SerializeField]
    //GameObject thisUIGoldTextOBJECT;

    [SerializeField]
    TMP_Text thisUIGoldText;

    int goldCollectedAllTime;

    // Start is called before the first frame update
    void Start()
    {
        overAllGameManagerScript = GameObject.Find("GameManagerObject").GetComponent<GameManagerScript>();
        goldCollectedAllTime = overAllGameManagerScript.ReturnPlayerCoinStats();
        //thisUIGoldTextOBJECT = GetComponent<GameObject>();
        thisUIGoldText = gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(overAllGameManagerScript.ReturnPlayerCoinStats());
        thisUIGoldText.text = goldCollectedAllTime + "c";
    }
}
