using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOrUnPause : MonoBehaviour
{
    [SerializeField]
    [Header("What Gameobject to hide")]
    [Tooltip("What game object will you like to hide / get on pause start / end?")]
    GameObject thisUIStart, thisUIEnd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Unpause(){
        Time.timeScale = 1.0f;
        thisUIEnd.SetActive(false);
    }

    public void Pause(){
        Time.timeScale = 0f;
        thisUIStart.SetActive(true);
    }
}
