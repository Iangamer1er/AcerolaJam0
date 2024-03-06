using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBCore.Refs;

public class Player : ValidatedMonoBehaviour
{
    [SerializeField] private LayerMask maskInteractable;
    [SerializeField] private Transform defaultArmPos;
    [SerializeField, Self] Rigidbody rb;
    [SerializeField, Range(5, 50)] float handSpeed;

    private Coroutine coroutineDisableMouseForFrame;
    private Coroutine mouveHandToPoint;
    private bool canLook = true;
    private bool wannaMove = false;
    

    private void Start() {
        OnEnableMouseControl();
    }

    private void Update() {
        wannaMove = Input.GetMouseButton(0);
    }

    private void FixedUpdate() {
       if(wannaMove) MoveHand();    
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
            mousePos = new Vector3(mousePos.x, mousePos.y + transform.localScale.y * 0.5f, mousePos.z);
            if(mouveHandToPoint != null) StopCoroutine(mouveHandToPoint);
            mouveHandToPoint = StartCoroutine(CoroutineMove(transform.position, mousePos));
        }
    }

    private IEnumerator CoroutineMove(Vector3 posIni , Vector3 posDest){
        bool isThere = false; 
        while (!isThere){
            isThere = MouveWithMoveTowards(posDest);
            yield return new WaitForFixedUpdate(); 
        }
    }

    private bool MouveWithMoveTowards(Vector3 posDest){
        float maxDistance = handSpeed * Time.fixedDeltaTime; 
        Vector3 newPos = Vector3.MoveTowards(transform.position, posDest, maxDistance); 
        rb.MovePosition(newPos);
        Vector3 roundedPos = new Vector3(Round(newPos.x, 2), Round(newPos.y, 2), Round(newPos.z, 2));
        Vector3 roudedDestPos = new Vector3(Round(posDest.x, 2), Round(posDest.y, 2), Round(posDest.z, 2));
        return (roundedPos == roudedDestPos); 

    }

    private float Round(float valeur, int decimale){
        float multiple = Mathf.Pow(10, (float)decimale);
        return Mathf.Round(valeur*multiple)/multiple; 
    }
}
