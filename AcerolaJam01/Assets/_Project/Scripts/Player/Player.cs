using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBCore.Refs;

public class Player : ValidatedMonoBehaviour
{
    [SerializeField] private LayerMask maskInteractable;
    [SerializeField] private Transform defaultArmPos;
    [SerializeField, Self] Rigidbody rb;
    [SerializeField, Range(0.01f,1)] float handSpeed;

    private Coroutine coroutineDisableMouseForFrame;
    private Coroutine mouveHandToPoint;
    private bool canLook = true;
    

    private void Start() {
        OnEnableMouseControl();
    }

    private void Update() {
        
    }

    private void FixedUpdate() {
        MoveHand();    
    }

    private void OnEnableMouseControl(){
        Cursor.lockState= CursorLockMode.Confined;
        // Cursor.visible = false;
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

    private void MoveHand(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mousePos = new Vector3();
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, maskInteractable)){
            mousePos = raycastHit.point;
            Debug.Log(mousePos);
            if(mouveHandToPoint != null) StopCoroutine(mouveHandToPoint);
            mouveHandToPoint = StartCoroutine(CoroutineMove(transform.position, mousePos));
        }
    }

    private IEnumerator CoroutineMove(Vector3 posIni , Vector3 posDest){
        bool isThere = false; 
        float actionTime = 0; 
        Vector3 posInit = transform.position; 
        while (!isThere){
            isThere = MouveWithLerp(posIni, posDest, actionTime);
            actionTime += Time.fixedDeltaTime; 
            yield return new WaitForFixedUpdate(); 
        }
    }

    private bool MouveWithLerp(Vector3 posIni, Vector3 posDest, float actionTime){
        float t = Mathf.SmoothStep(0, 1, actionTime/handSpeed);
        Vector3 newPos = Vector3.Lerp(posIni, posDest, t);
        float distance = Vector3.Distance(transform.position, newPos);
        Vector3 roudedPos = new Vector3(Round(newPos.x, 2), Round(newPos.y, 2), Round(newPos.z, 2)); 
        Vector3 roudedDestPos = new Vector3(Round(posDest.x, 2), Round(posDest.y, 2), Round(posDest.z, 2));
        rb.MovePosition(newPos);
        return (roudedPos == roudedDestPos);
    }

    private float Round(float valeur, int decimale){
        float multiple = Mathf.Pow(10, (float)decimale);
        return Mathf.Round(valeur*multiple)/multiple; 
    }
}
