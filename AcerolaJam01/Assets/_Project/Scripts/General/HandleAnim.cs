using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandleAnim : MonoBehaviour
{
    [SerializeField] public UnityEvent myEvent;
    [SerializeField] private Transform transformChild;
    [SerializeField] private Transform transformParent;
    
    private Transform transFormOriginalParent;
    
    public void StartEvents(){
        myEvent.Invoke();
    }

    public void SetAsParent(){
        transFormOriginalParent = transformChild.parent;
        transformChild.parent = transformParent;
    }

    public void SetOriginalParent(){
        transformChild.parent = transFormOriginalParent;
    }
}
