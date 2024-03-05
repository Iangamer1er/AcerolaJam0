using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Coroutine coroutineDisableMouseForFrame;
    private bool canLook = true;
    

    private void Start() {
        
    }

    private void Update() {
        
    }

    private void MoveHand(){
        Vector2 mousePos = Input.mousePosition;
        
    }

    private void OnEnableMouseControl(){
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible = false;
        coroutineDisableMouseForFrame = StartCoroutine(DisableMouseForFrame());
    }

    private void OndisableMouseControl(){
        Cursor.lockState= CursorLockMode.None;
        Cursor.visible = true;
        StopCoroutine(coroutineDisableMouseForFrame);
        canLook = true;
    }

    private IEnumerator DisableMouseForFrame(){
        canLook = true;
        yield return new WaitForEndOfFrame();
        canLook = false;
    }
}
