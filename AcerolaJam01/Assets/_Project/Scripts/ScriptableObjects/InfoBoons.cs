using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Boon", menuName = "Map/Boons")]
public class InfoBoons : ScriptableObject
{
    [Header("Infos")]
    [SerializeField, TextArea] public List<string> DMDescription;
    [SerializeField, Range(0, 1)] public float chanceWarriorGod = 0.1f;
    [SerializeField, TextArea] public List<string> DMDescriptionFail;
    [SerializeField, TextArea] public List<string> DMDescriptionSuccess;
    [SerializeField, TextArea] public List<string> DMDescriptionOtherGod;

    [Header("Event")]
    [SerializeField] public List<MyBaseEvent> eventWarriorGodFail;
    [SerializeField] public List<MyBaseEvent> eventWarriorGodSuccess;
    [SerializeField] public List<MyBaseEvent> eventOuterGod;
    
}
