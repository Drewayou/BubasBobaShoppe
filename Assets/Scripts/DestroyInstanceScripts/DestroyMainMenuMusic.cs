using UnityEngine;

public class DestroyMainMenuMusic : MonoBehaviour
{
    GameObject MainMenuMusic;

    void Awake(){
        MainMenuMusic = GameObject.Find("MainMenuMusic(Clone)");
        Destroy(MainMenuMusic);
    }
}
