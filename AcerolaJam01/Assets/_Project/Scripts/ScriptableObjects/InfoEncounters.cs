using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Encounter", menuName = "Map/Encounters")]
public class InfoEncounters : ScriptableObject
{
    [Header("Infos")]
    [SerializeField, Range(0, 1)] public float chance = 0.1f;
    [SerializeField] public List<string> DMDescription;
    [SerializeField] public List<string> DMDescriptionFail;
    [SerializeField] public List<string> DMDescriptionSuccess;

    [Header("Event")]
    [SerializeField] public List<MyBaseEvent> eventFail;
    [SerializeField] public List<MyBaseEvent> eventWin;
}

[System.Serializable]
public class MyBaseEvent {
    [SerializeReference, SubclassPicker] BaseEvent baseEvent;
}
