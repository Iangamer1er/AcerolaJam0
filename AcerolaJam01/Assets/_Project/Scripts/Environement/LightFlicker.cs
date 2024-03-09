using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;

public class LightFlicker : ValidatedMonoBehaviour
{
    [SerializeField, Self] private Light myLight;
    [SerializeField, Range(0, 1)] float maxDisplacement = 0.25f;
    [SerializeField, Range(0, 5)] float intensityMaxFlicker = 1;
    [SerializeField, Range(0, 1)] float maxInterval = 1f;
    [SerializeField, Range(0, 1)] float minInterval = 1f;

    private float targetIntensity;
    private float lastIntensity;
    private float interval;
    private float timer = 0;
    private float startingIntensity;

    private Vector3 targetPosition;
    private Vector3 lastPosition;
    private Vector3 origin;

    private void Awake() {
        origin = transform.position;
        lastPosition = origin;
        startingIntensity = myLight.intensity;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval){
            lastIntensity = myLight.intensity;
            targetIntensity = Random.Range(-intensityMaxFlicker, intensityMaxFlicker);
            timer = 0;
            interval = Random.Range(minInterval, maxInterval);
            targetPosition = origin + Random.insideUnitSphere * maxDisplacement;
            lastPosition = myLight.transform.position;
        }

        myLight.intensity = Mathf.Lerp(lastIntensity, targetIntensity + startingIntensity, timer / interval);
        myLight.transform.position = Vector3.Lerp(lastPosition, targetPosition, timer / interval);
    }
}
