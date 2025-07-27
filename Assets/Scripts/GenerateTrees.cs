using UnityEngine;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;
using System.Linq;
using System.Collections;

public class NewMonoBehaviourScript : MonoBehaviour
{
    //Note: This is a temp code that may/may not be in use within the main game.
    //This was made for a university project.
    //Struct to perform the job of randomply finding transform places for trees via Parallelism.
    public struct GenerationParallelJob : IJobParallelForTransform
    {
        // Jobs declare all data that will be accessed in the job
        // By declaring it as read only, multiple jobs are allowed to access the data in parallel
        [ReadOnly]
        public NativeArray<Vector3> treeNewPositionArrayInJob;

        // The main thread waits for the job same frame or next frame, but the job should do work deterministically
        // independent on when the job happens to run on the worker threads.
        // The code actually running on the job is below. The job will access all transform positions for the object array.
        public void Execute(int index, TransformAccess transform)
        {
            // Move the transforms based on random generation.
            Vector3 position = treeNewPositionArrayInJob[index];
            transform.position = position;
        }
    }

    // Variables to put the trees and tree transforms into for the job.
    // Gameobject var to put what one want to instantiate.
    [SerializeField]
    public GameObject treeObject;

    // Gameobject var to put the generated objects into for the hierarchy. An empty "WorldGeneration" object may be needed.
    [SerializeField]
    public GameObject WorldGenerationObjectHolder;

    // Input how many trees this code will generate (higher/more needs more CPU).
    public int treesToGenerate;

    //Bool to decide to use parallelism or not.
    public bool useParallelismToGenerate;
    public Transform[] treeTransformFromGameObject;
    public Vector3[] randomizedPositionsForTreesArray;
    TransformAccessArray treePositionArray;

    // Variables that saves the time to process the code selected.
    private float timeInMSToProcessCode = 0.0f;

    // The TRUE process timer of this script that saves how long this script has been running.
    private float overAllTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Checks if the dev wants to generate the trees utilizing sequential code or parallel code.
        if(!useParallelismToGenerate){
            GenerateTreesSequentially();
        }else{
            GenerateTreesViaParallelism();
        }
    }

    // Update is called once per frame
    void Update()
    {
        overAllTimer += Time.deltaTime;
    }

    // Method to randomply generate a Vector3 to place the new trees.
    public Vector3 RandomPlaceForTree(){
        Vector3 newPosition = new Vector3(Random.Range(-10f,10f),Random.Range(-10f,10f),0);
        return newPosition;
    }

    // Method to generate trees NOT via Parallelism.
    public void GenerateTreesSequentially(){

        for(int processes = 1; processes < treesToGenerate; processes++){
            //Generate a new tree via random() methods which does take some CPU processes.
            Vector3 treePosition = RandomPlaceForTree();
            Instantiate(treeObject,treePosition, Quaternion.identity, WorldGenerationObjectHolder.transform);
        }

        //Replies to the console the time it takes to process this code.
        Debug.Log(Time.realtimeSinceStartup);
    }

    public void GenerateTreesViaParallelism(){

        // Sets the size for the all the trees to be generated in the game object holder List
        treeTransformFromGameObject = new Transform[treesToGenerate-1];

        // Sets the size for the random position vector3 List
        randomizedPositionsForTreesArray = new Vector3[treesToGenerate-1];

        for(int processes = 1; processes < treesToGenerate; processes++){
            //Generate a new tree depending how many the dev wants.
            treeTransformFromGameObject[processes-1] = Instantiate(treeObject,WorldGenerationObjectHolder.transform).transform;
            //Generate Vector3 coordinates randomly for each tree and add it to the Vector3 list sequentially.
            randomizedPositionsForTreesArray[processes-1] = RandomPlaceForTree();
        }

        treePositionArray = new TransformAccessArray(treeTransformFromGameObject);

        // Initialize the job data.
        NativeArray<Vector3> treeNewPositions = new NativeArray<Vector3>(randomizedPositionsForTreesArray, Allocator.Persistent);
        //Saves the needed vector data to calculate and render the tree's true transforms.
        var job = new GenerationParallelJob(){
            treeNewPositionArrayInJob = treeNewPositions
        };

        JobHandle jobHandle = job.Schedule(treePositionArray);

        //StartCoroutine(WaitForJobs(2f));
        // Ensure the job has completed.
        jobHandle.Complete();
        // Native arrays must be disposed manually.
        treePositionArray.Dispose();
        treeNewPositions.Dispose();

        //Replies to the console the time it takes to process this code.
        Debug.Log(Time.realtimeSinceStartup);
    }

    // Coroutine to ensure the jobs to finish
    public IEnumerator WaitForJobs(float num)
    {
        yield return new WaitForSecondsRealtime(num);
    
    }
}
