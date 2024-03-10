using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Encounter", menuName = "Map/Encounters")]
public class InfoEncounters : ScriptableObject
{
    [Header("Infos")]
    [SerializeField] public MyListDialogues DMDescription;
    [SerializeField, Range(0, 1)] public float chance = 0.1f;
    [SerializeField] public MyListDialogues DMDescriptionFail;
    [SerializeField] public MyListDialogues DMDescriptionSuccess;

    [Header("Event")]
    [SerializeField] public List<MyBaseEvent> eventFail;
    [SerializeField] public List<MyBaseEvent> eventWin;
}

[System.Serializable]
public class MyBaseEvent {
    [SerializeReference, SubclassPicker] BaseEvent baseEvent;
}
