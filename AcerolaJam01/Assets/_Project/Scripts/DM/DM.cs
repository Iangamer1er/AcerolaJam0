using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DM : MonoBehaviour
{
    [SerializeField] private InfoDialogue introDialogue;
    
    private static GameManager _instance; 
    public static GameManager instance => _instance;


    private void Awake() {
        
    }
}
