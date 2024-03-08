using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Boon", menuName = "Map/Boons")]
public class InfoBoons : ScriptableObject
{
    [Header("Infos")]
    [SerializeField, TextArea] public string DMDescription = "";
    [SerializeField, Range(0, 1)] public float chanceWarriorGod = 0.1f;
    [SerializeField, TextArea] public string DMDescriptionFail = "Failed";
    [SerializeField, TextArea] public string DMDescriptionSuccess = "Success";
    [SerializeField, TextArea] public string DMDescriptionOtherGod = "Shameful";
    
}
