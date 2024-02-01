using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : Singleton<SceneController>
{
    public int maxLevel = 13;
    public int currentStage;

    public void LoadStage(string name)
    {
        if (name == "MainMenu") { currentStage = 0; }
        else currentStage = Convert.ToInt32(name.Replace("Stage", string.Empty));
        SceneManager.LoadScene(name);
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T _instance;
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();
                if (_instance == null)
                {
                    var go = new GameObject();
                    go.name = typeof(T).Name;
                    _instance = go.AddComponent<T>();
                }
            }
            return _instance;
        }
    }
}