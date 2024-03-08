using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Encounter", menuName = "Map/Encounters")]
public class InfoEncounters : ScriptableObject
{
    [Header("Infos")]
    [SerializeField, TextArea] public string DMDescription = "";
    [SerializeField, Range(0, 1)] public float chance = 0.1f;
    [SerializeField, TextArea] public string DMDescriptionFail = "Failed";
    [SerializeField, TextArea] public string DMDescriptionSuccess = "Success";

    [Header("Event")]
    [SerializeReference, SubclassPicker] public BaseEvent baseEvent;
}
