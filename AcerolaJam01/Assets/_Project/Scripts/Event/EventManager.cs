using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour{
    private static EventManager _instance; 
    public static EventManager instance => _instance;

    private BaseEvent currentState;

    private void Awake() => _instance = this;

    public void ChangeState(BaseEvent state){
        currentState = state;
        currentState.EventAction();
    }
}
