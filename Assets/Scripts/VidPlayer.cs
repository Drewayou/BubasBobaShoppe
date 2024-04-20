using UnityEngine;
using UnityEngine.Video;

public class VidPlayer : MonoBehaviour
{
    [SerializeField]
    string VidName;

    // Start is called before the first frame update
    void PlayVideo()
    {
        VideoPlayer videoplayer = GetComponent<VideoPlayer>();

        if(videoplayer){
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, VidName);
            Debug.Log(videoPath);
            videoplayer.url = videoPath;
            videoplayer.Play();
        }
    }

    // Update is called once per frame
    void Start()
    {
        PlayVideo();
    }
}
