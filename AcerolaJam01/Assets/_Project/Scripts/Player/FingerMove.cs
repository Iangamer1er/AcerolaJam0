using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerMove : MonoBehaviour
{
    const int roundPrecision = 4;
    public Transform targetSocket;
    [SerializeField, Min(0)] private float moveTime = 0.5f;

    public IEnumerator CoroutineMoveEnd(Vector3 posIni, Vector3 rotIni, Transform posDest){
        bool isThere = false;
        float actionTime = 0;
        while (!isThere){
            actionTime += Time.fixedDeltaTime;
            MoveWithLerp(posIni, rotIni, posDest, actionTime, moveTime);
            yield return new WaitForFixedUpdate(); 
        }
        transform.parent = targetSocket;
    }

    private bool MoveWithLerp(Vector3 posIni, Vector3 rotIni,Transform posDest, float actionTime, float speed){
        float t = Mathf.SmoothStep(0, 1, actionTime/speed); 
        Vector3 newPos = Vector3.Lerp(posIni, posDest.position, t); 
        Vector3 newRot = Vector3.Lerp(rotIni, posDest.eulerAngles, t); 
        Vector3 roundedPos = new Vector3(Round(newPos.x, roundPrecision), Round(newPos.y, roundPrecision), Round(newPos.z, roundPrecision));
        Vector3 roudedDestPos = new Vector3(Round(posDest.position.x, roundPrecision), Round(posDest.position.y, roundPrecision), Round(posDest.position.z, roundPrecision));
        transform.position = newPos;
        transform.eulerAngles = newRot;
        return (roundedPos == roudedDestPos);
    }

    private float Round(float valeur, int decimale){
        float multiple = Mathf.Pow(10, (float)decimale);
        return Mathf.Round(valeur*multiple)/multiple; 
    }

}
