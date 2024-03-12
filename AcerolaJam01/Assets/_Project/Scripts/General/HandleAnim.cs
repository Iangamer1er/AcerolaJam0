using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public void Dies(){
        Camera.main.GetComponentInParent<CinemachineVirtualCamera>().LookAt = null;
        AudioManager.instance.PlayEffect(AudioManager.instance.playerStabbed);
    }

    public void ChangeScene(){
        GameManager.instance.StartCoroutine(ChangeSceneFixed()); // need to wait for animation to finish to be able to load scene
    }

    private IEnumerator ChangeSceneFixed(){
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(1);
    }
}
