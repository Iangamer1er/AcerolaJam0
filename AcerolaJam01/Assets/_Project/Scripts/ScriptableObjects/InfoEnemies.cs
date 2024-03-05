using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemie", menuName = "Map/Enemies")]
public class InfoEnemies : ScriptableObject
{
    [Header("Infos")]
    [SerializeField] public string eName = "";
    [SerializeField, TextArea] public string description = "";

    [SerializeField, Range(0, 1)] public float armsMaxHealth = 1; 
    [SerializeField, Range(0, 1)] public float armsHealthRatio = 0.2f;
    [SerializeField, Range(0, 1)] public float legsMaxHealth = 1; 
    [SerializeField, Range(0, 1)] public float legsHealthRatio = 0.2f;
    [SerializeField, Range(0, 1)] public float headMaxHealth = 1;
    [SerializeField, Range(0, 3)] public float headModif = 1;
    [SerializeField, Range(0, 1)] public float torsoMaxHealth = 1f;
    [SerializeField, Range(0,1)] public float spareAskChance = 0;
}
