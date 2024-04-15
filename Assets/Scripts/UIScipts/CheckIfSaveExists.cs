using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckIfThereSASave : MonoBehaviour
{
    GameObject thisGameManager;

    [SerializeField]
    [Header("No Save Alert!")]
    [Tooltip("Alert Prefab if no save files were found!")]
    GameObject noSaveFoundAlert;

    [SerializeField]
    [Header("Another Save Found Alert!")]
    [Tooltip("Alert Prefab if a save file was found, overwrite it?!")]
    GameObject overWritingSaveFoundAlert;

    [SerializeField]
    [Header("New Game Button")]
    [Tooltip("Button for new game")]
    GameObject NewGamePreConfirm;

    [SerializeField]
    [Header("A confirm overwrite new game button")]
    [Tooltip("Confirm the button for a new game save")]
    GameObject NewGameConfirmed;

    //What level to load if continuing level
    [Header("What Scene To Load On Continue")]
    [Tooltip("A cheecky way to have the continue button load the tutorial or Level1")]
    public string StringNameOfLevelToLoad;

    // Start is called before the first frame update
    void Start()
    {
        thisGameManager = GameObject.Find("GameManagerObject");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckIfSaveExists(){
        bool doSavesExist = thisGameManager.GetComponent<GameManagerScript>().CheckingIfSavesExist();
        if(doSavesExist){
            thisGameManager.GetComponent<GameManagerScript>().ContinueGame();
            Time.timeScale = 1;
            SceneManager.LoadScene(StringNameOfLevelToLoad);
        }else{
            noSaveFoundAlert.SetActive(true);
        }
    }

    public void CheckIfSaveExistsBeforeStartingANewGame(){
        bool doSavesExist = thisGameManager.GetComponent<GameManagerScript>().CheckingIfSavesExist();
        if(doSavesExist){
            overWritingSaveFoundAlert.SetActive(true);
            NewGameConfirmed.SetActive(true);
        }else{
            thisGameManager.GetComponent<GameManagerScript>().StartNewGame();
            Time.timeScale = 1;
            SceneManager.LoadScene(StringNameOfLevelToLoad);
        }
    }
}
