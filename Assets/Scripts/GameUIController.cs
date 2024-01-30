using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayBGM("AudioClip/Play");
    }

    void Update()
    {
        
    }
}
