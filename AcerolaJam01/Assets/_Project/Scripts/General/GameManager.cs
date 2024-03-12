using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBCore.Refs;
using UnityEngine.SceneManagement;

public class GameManager : ValidatedMonoBehaviour
{
    private static GameManager _instance; 
    public static GameManager instance => _instance;
    public bool normalTime = false;
    public int level = 0;
    public bool didTuto = false;

    public float fixedDeltaTime;

    void Awake()
    {
        if(_instance == null) _instance = this; 
        else{
           Destroy(gameObject); 
           return;
        } 
        DontDestroyOnLoad(gameObject);  
        fixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape)) Application.Quit();
    }

    public void ChangeTimeScale(float percent){
        Time.timeScale = percent;
        Time.fixedDeltaTime = fixedDeltaTime * percent;
    }

    public void ChangeScene(int sceneIndex){
        SceneManager.LoadScene(sceneIndex);
    }
}
