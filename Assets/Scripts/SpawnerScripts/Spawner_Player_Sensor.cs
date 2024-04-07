using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Player_Sensor : MonoBehaviour
{

    [SerializeField]
    [Header("Spawner")]
    [Tooltip("Put in parent spawner to link deactivation boundary")]
    public GameObject parentSpawner;
    private float timeTillSpawnerDeactivates, timeTillReactivation; 

    private CircleCollider2D thisCollider;
    public float timerTillDeactivate, timerTillReactivate;

    private bool isSpawnerActive = true, playerInSensor = false;

    // Start is called before the first frame update
    void Start()
    {

        //FIXME:
        //Set spawnerSensor level
        setSensorLevel(1);

        thisCollider = GetComponent<CircleCollider2D>();

        isSpawnerActive = true;

        //Set active sensors
        timerTillDeactivate = timeTillSpawnerDeactivates; 
        timerTillReactivate = timeTillReactivation;


    }

    // Update is called once per frame
    void Update()
    {
        customOnTriggerStay2D();
        tryRecoverSpawnerResources();
    }

    private void tryRecoverSpawnerResources(){
        if(!playerInSensor && timerTillDeactivate<=timeTillSpawnerDeactivates){
            timerTillDeactivate += Time.deltaTime;
       }
    }

    public void OnTriggerEnter2D(){
        playerInSensor = true;
    }

    public void OnTriggerExit2D(){
        playerInSensor = false;
    }

    public void customOnTriggerStay2D(){
        
        if(playerInSensor && isSpawnerActive){
            if(timerTillDeactivate<=0f){
            parentSpawner.GetComponent<Spawner_Scripts>().setSpawnerDeactive();
            
                isSpawnerActive = false;
                }else{
                    timerTillDeactivate -= Time.deltaTime;
                }
        }else{
            if(!isSpawnerActive){
            if(timerTillReactivate>=0f){
                timerTillReactivate -= Time.deltaTime;
            }else{
                //Turn on "sensor" for player and reset REactivation time for next instance
                timerTillReactivate = timeTillReactivation;
                timerTillDeactivate = timeTillSpawnerDeactivates;
                parentSpawner.GetComponent<Spawner_Scripts>().setSpawnerActive();
                isSpawnerActive = true;
                }
            }
        }
        
    }

    public void setSensorLevel(int thisSpawnerLevel){
        //FIXME: adda switch statement using the stats from the parent spawner to change this
        //spawner's difficulty

        switch(thisSpawnerLevel){

            case 1:
            timeTillSpawnerDeactivates = 15f;
            timeTillReactivation = 20f;
            break;

            default:
            timeTillSpawnerDeactivates = 20f;
            timeTillReactivation = 35f;
            break;
        }
    }
}
