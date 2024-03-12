using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandleAnim : MonoBehaviour
{
    [SerializeField] public UnityEvent[] myEvents;
    
    public void StartEvents(){
        foreach (UnityEvent myEvent in myEvents){
            myEvent.Invoke();
        }
    }
}
