using UnityEngine;

// Using unity's Object.DontDestroyOnLoad example.
//
// This script is for playing audio accross different scenes, and for the overall "GameManager Object" to be kept during the whole game.

public class DontDestroy : MonoBehaviour
{
    [SerializeField]
    [Header("MainMenuMusicPrefab")]
    [Tooltip("Put the MainMenuMusicPrefab to play here")]
    GameObject MainMenuMusicPrefab;

    [SerializeField]
    [Header("IntroMusicPrefab")]
    [Tooltip("Put the IntroMusicPrefab to play here")]
    GameObject IntroMusicPrefab;
    
    void Awake()
    {
 
    }
    void Update(){
        
    }
}