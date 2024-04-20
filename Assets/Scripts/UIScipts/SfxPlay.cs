using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SfxPlay : MonoBehaviour
{
    [SerializeField]
    [Header("Sfx Object")]
    [Tooltip("Drag an sfx to play here")]
    GameObject sfxPrefabToPlay;
    
    public void PlaySfx(){
        Instantiate(sfxPrefabToPlay);
    }
}
