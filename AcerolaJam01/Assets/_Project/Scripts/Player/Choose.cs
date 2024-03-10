using KBCore.Refs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ChoseType {Attack, Yes, Spare, No}

public class Choose : ValidatedMonoBehaviour{
    public int roundPrecision = 4;
    [SerializeField] public ChoseType type = ChoseType.Attack;

    [Header("Rotation")]
    [SerializeField, Min(0)] private float rotationTime = 0.5f;
    [SerializeField, Range(0, 360)] private float flipAmount = 180;
    [SerializeField] private bool canFlip = true;

    [Header("Suspended")]
    [SerializeField, Min(0)] private float elevatedSpeed = 4f;
    [SerializeField, Min(0)] private float elevatedStartingSpeed = 4f;
    [SerializeField, Min(0)] private float elevatedEndingSpeed = 3f;
    [SerializeField, Range(0, 2)] private float elevatedHight = 0.5f;
    [SerializeField, Min(0)] private float animationHightChange = 0.1f;
    [Header("Suspended Lerp")]
    [SerializeField, Min(0)] private float elevationTime = 0.5f;
    [SerializeField, Min(0)] private float elevationStartingTime = 0.5f;
    [SerializeField, Min(0)] private float elevationEndingTime = 0.5f;
    [SerializeField] bool useLerp = false;

    public bool isSupended = false;
    private Coroutine coMouveTowards;
    private Coroutine coFlip;
    private Coroutine coAnimFlip;
    private Coroutine coAnimMouve;
    private Vector3 startingPos;
    public bool isThere = false;

    private void Start() {
        startingPos = transform.position;
        Player.instance.inCombatEvent.AddListener(AnimateFlip);
    }

    public void AnimateFloat(){
        isSupended = true;
        coAnimMouve = StartCoroutine(CoAnimateFloat());
    }

    public void AnimateFlip(){
        if(!canFlip) return;
        if(coAnimFlip != null) StopCoroutine(coAnimFlip);
        coAnimFlip = StartCoroutine(CoAnimateFlip(
            transform.localEulerAngles,
            transform.localEulerAngles + Vector3.right * flipAmount,
            rotationTime
        ));
    }

    private IEnumerator CoAnimateFloat(){
        float direction = 1;
        if(coMouveTowards != null) StopCoroutine(coMouveTowards);
        coMouveTowards = StartCoroutine(CoroutineMove(
            startingPos,
            startingPos + Vector3.up * elevatedHight,
            true
            ));
        yield return new WaitUntil(()=>isThere);
        while (isSupended){
            StopCoroutine(coMouveTowards);
            coMouveTowards = StartCoroutine(CoroutineMove(
                startingPos + Vector3.up * elevatedHight + Vector3.up * animationHightChange * direction, 
                startingPos + Vector3.up * elevatedHight - Vector3.up * animationHightChange * direction
            ));
            direction *= -1;
            yield return new WaitUntil(()=>isThere);
        }
        StopCoroutine(coMouveTowards);
        isThere = false;
        coMouveTowards = StartCoroutine(CoroutineMoveEnd(transform.position, startingPos));
    }

    private IEnumerator CoAnimateFlip(Vector3 rotIni , Vector3 rotEnd, float animeTime){
        bool isRotated = false;
        float actionTime = 0;
        while (!isRotated){
            actionTime += Time.fixedDeltaTime;
            isRotated = FlipWithLerp(rotIni, rotEnd, actionTime, animeTime);
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }   

    private IEnumerator CoroutineMove(Vector3 posIni , Vector3 posDest, bool startingSpeed = false){
        isThere = false;
        float actionTime = 0;
        while (!isThere && isSupended){
            actionTime += Time.fixedDeltaTime;
            if(useLerp){
                float speed = startingSpeed ? elevationStartingTime : elevationTime;
                isThere = MoveWithLerp(posIni, posDest, actionTime, speed);
            }else{
                float speed = startingSpeed ? elevatedStartingSpeed : elevatedSpeed;
                isThere = MouveWithMoveTowards(posDest, speed);
            }
            yield return new WaitForFixedUpdate(); 
        }
        isThere = true;
    }

    private IEnumerator CoroutineMoveEnd(Vector3 posIni , Vector3 posDest){
        isThere = false;
        float actionTime = 0;
        while (!isThere){
            actionTime += Time.fixedDeltaTime;
            isThere = useLerp ? 
                MoveWithLerp(posIni, posDest, actionTime, elevationEndingTime) : 
                MouveWithMoveTowards(posDest, elevatedEndingSpeed)
            ;
            yield return new WaitForFixedUpdate(); 
        }
    }

    private bool MouveWithMoveTowards(Vector3 posDest, float speed){
        float maxDistance = speed * Time.fixedDeltaTime; 
        Vector3 newPos = Vector3.MoveTowards(transform.position, posDest, maxDistance);
        transform.position = newPos;
        Vector3 roundedPos = new Vector3(Round(newPos.x, roundPrecision), Round(newPos.y, roundPrecision), Round(newPos.z, roundPrecision));
        Vector3 roudedDestPos = new Vector3(Round(posDest.x, roundPrecision), Round(posDest.y, roundPrecision), Round(posDest.z, roundPrecision));
        return (roundedPos == roudedDestPos); 
    }

    private bool MoveWithLerp(Vector3 posIni, Vector3 posDest, float actionTime, float speed){
        float t = Mathf.SmoothStep(0, 1, actionTime/speed); 
        Vector3 newPos = Vector3.Lerp(posIni, posDest, t); 
        Vector3 roundedPos = new Vector3(Round(newPos.x, roundPrecision), Round(newPos.y, roundPrecision), Round(newPos.z, roundPrecision));
        Vector3 roudedDestPos = new Vector3(Round(posDest.x, roundPrecision), Round(posDest.y, roundPrecision), Round(posDest.z, roundPrecision));
        transform.position = newPos;
        return (roundedPos == roudedDestPos);
    }

    private bool FlipWithLerp(Vector3 rotIni, Vector3 rotEnd, float actionTime, float speed){
        float t = Mathf.SmoothStep(0, 1, actionTime/speed); 
        Vector3 newPos = Vector3.Lerp(rotIni, rotEnd, t); 
        Vector3 roundedPos = new Vector3(Round(newPos.x, roundPrecision), Round(newPos.y, roundPrecision), Round(newPos.z, roundPrecision));
        Vector3 roudedDestPos = new Vector3(Round(rotEnd.x, roundPrecision), Round(rotEnd.y, roundPrecision), Round(rotEnd.z, roundPrecision));
        transform.localEulerAngles = newPos;
        return (roundedPos == roudedDestPos);
    }

    private float Round(float valeur, int decimale){
        float multiple = Mathf.Pow(10, (float)decimale);
        return Mathf.Round(valeur*multiple)/multiple; 
    }
}
