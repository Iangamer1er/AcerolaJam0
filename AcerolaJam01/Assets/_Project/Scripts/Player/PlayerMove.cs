using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBCore.Refs;
using Cinemachine;
using System.Threading;
using Utilities;

public class PlayerMove : ValidatedMonoBehaviour
{
    [Header("Hand controls")]
    [SerializeField] private LayerMask maskInteractable;
    [SerializeField] private Transform defaultArmPos;
    [SerializeField, Range(0, 10)] private float timeBeforeHandReturns = 2;
    [SerializeField, Range(5, 50)] float handSpeed;
    // [SerializeField, Self] Rigidbody rb;

    [Header("Camera controls")]
    [SerializeField] private float lookYClamp = 35f;
    [SerializeField] private float lookXClamp = 85f;
    [SerializeField, Min(0)] private float lookSpeed;
    [SerializeField, Range(0.01f, 1)] private float screenWidthPercent = 0.1f;
    [SerializeField, Scene] public CinemachineVirtualCamera cam;

    private CountdownTimer returnTimer;
    private Coroutine coroutineDisableMouseForFrame;
    private Coroutine mouveHandToPoint;
    private bool canLook = true;
    private bool wannaMove = false;
    public bool controlsCam = true;
    public bool controlsHand = true;
    private Vector2 lookRotation = Vector2.zero;

    private void Start() {
        OnEnableMouseControl();
        returnTimer = new CountdownTimer(timeBeforeHandReturns);
        returnTimer.OnTimerStop = ReturnPosition;
    }

    private void Update() {
        wannaMove |= Input.GetMouseButtonDown(0);
        Look();
        returnTimer.Tick(Time.deltaTime);
    }

    private void FixedUpdate() {
       if(wannaMove && controlsHand) MoveHand();    
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

    private void Look(){
        if(!controlsCam) return;
        float lookX = 0;
        if(Input.mousePosition.x < Screen.width * screenWidthPercent) lookX = -1;
        else if(Input.mousePosition.x > Screen.width - Screen.width * screenWidthPercent) lookX = 1;
        float lookY = 0;
        if(Input.mousePosition.y < Screen.height * screenWidthPercent) lookY = -1;
        else if(Input.mousePosition.y > Screen.height - Screen.height * screenWidthPercent) lookY = 1;
        lookRotation -= new Vector2(lookX, lookY) * lookSpeed * Time.deltaTime;
        lookRotation = new Vector2(
            Mathf.Clamp(lookRotation.x, -lookXClamp, lookXClamp),
            Mathf.Clamp(lookRotation.y, -lookYClamp, lookYClamp)
        );
        Vector3 targetRotation = new Vector3(lookRotation.y, -lookRotation.x, cam.transform.localEulerAngles.z);
        cam.transform.localEulerAngles = targetRotation;

    }

    private void MoveHand(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mousePos = new Vector3();
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, maskInteractable)){
            mousePos = raycastHit.point;
            mousePos = new Vector3(mousePos.x, mousePos.y, mousePos.z);
            if(mouveHandToPoint != null) StopCoroutine(mouveHandToPoint);
            mouveHandToPoint = StartCoroutine(CoroutineMove(transform.position, mousePos, raycastHit));
        }
        wannaMove = false;
        returnTimer.Start();
    }

    private void ReturnPosition() => mouveHandToPoint = StartCoroutine(CoroutineMove(transform.position, defaultArmPos.position));

    private IEnumerator CoroutineMove(Vector3 posIni , Vector3 posDest){
        bool isThere = false; 
        while (!isThere){
            isThere = MouveWithMoveTowards(posDest);
            yield return new WaitForFixedUpdate(); 
        }
    }

    private IEnumerator CoroutineMove(Vector3 posIni , Vector3 posDest, RaycastHit raycastHit){
        bool isThere = false; 
        while (!isThere){
            isThere = MouveWithMoveTowards(posDest);
            yield return new WaitForFixedUpdate(); 
        }
        Player.instance.ClickedInteractable(raycastHit.transform.gameObject);
    }

    private bool MouveWithMoveTowards(Vector3 posDest){
        float maxDistance = handSpeed * Time.fixedDeltaTime; 
        Vector3 newPos = Vector3.MoveTowards(transform.position, posDest, maxDistance); 
        transform.position = newPos;
        Vector3 roundedPos = new Vector3(Round(newPos.x, 2), Round(newPos.y, 2), Round(newPos.z, 2));
        Vector3 roudedDestPos = new Vector3(Round(posDest.x, 2), Round(posDest.y, 2), Round(posDest.z, 2));
        return (roundedPos == roudedDestPos); 
    }

    private float Round(float valeur, int decimale){
        float multiple = Mathf.Pow(10, (float)decimale);
        return Mathf.Round(valeur*multiple)/multiple; 
    }
}
