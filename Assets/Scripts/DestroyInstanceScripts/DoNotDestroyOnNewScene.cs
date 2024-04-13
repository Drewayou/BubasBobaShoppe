using UnityEngine;

// Using unity's Object.DontDestroyOnLoad example.
//
// This script is for playing audio accross different scenes, and for the overall "GameManager Object" to be kept during the whole game.

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}