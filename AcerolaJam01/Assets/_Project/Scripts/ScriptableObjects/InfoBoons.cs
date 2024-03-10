using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Boon", menuName = "Map/Boons")]
public class InfoBoons : ScriptableObject
{
    [Header("Infos")]
    [SerializeField] public List<string> DMDescription;
    [SerializeField, Range(0, 1)] public float chanceWarriorGod = 0.1f;
    [SerializeField] public List<string> DMDescriptionFail;
    [SerializeField] public List<string> DMDescriptionSuccess;
    [SerializeField] public List<string> DMDescriptionOtherGod;
    
}
