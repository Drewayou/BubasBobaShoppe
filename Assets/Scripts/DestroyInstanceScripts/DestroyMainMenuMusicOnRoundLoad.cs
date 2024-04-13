using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Start()
    {
        Destroy(GameObject.Find("MainMenuMusic"));
    }
}
