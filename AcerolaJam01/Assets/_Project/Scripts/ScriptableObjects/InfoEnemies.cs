using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemie", menuName = "Map/Enemies")]
public class InfoEnemies : ScriptableObject
{
    [Header("Infos")]
    [SerializeField] public string eName = "";
    [SerializeField, Range(0, 1)] public float damage = 0.1f; 
    [SerializeField, Range(0, 1)] public float Brokendamage = 0.05f; 
    [SerializeField, Range(0, 1)] public float dodgeChance = 0.1f; 

    [SerializeField, Range(0, 1)] public float armsMaxHealth = 1; 
    [SerializeField, Range(0, 1)] public float armsHealthRatio = 1f;
    [SerializeField, Range(0, 1)] public float legsMaxHealth = 1; 
    [SerializeField, Range(0, 1)] public float legsHealthRatio = 1f;
    [SerializeField, Range(0, 1)] public float headChance = 0.2f;
    [SerializeField, Range(0, 3)] public float headModif = 1.5f;
    [SerializeField, Range(0, 5)] public float torsoMaxHealth = 1f;
    [SerializeField, Range(0, 1)] public float torsoModif = 1f;
    [SerializeField, Range(0, 1)] public float spareAskChance = 0;

    [Header("Textes")]
    [SerializeField, TextArea] public List<string> description;
    [SerializeField, TextArea] public List<string> attackTxt;

    [Header("States")]
    [SerializeReference, SubclassPicker] public EnemyBase behavoir;
    [SerializeReference, SubclassPicker] public EnemyBase behavoirLowHealth;
    [SerializeField,Range(0, 1)] public float lowHealthThreshhold = 0.33f;
    [SerializeField, TextArea] public List<string> descriptionLowHealthChange;

    [Header("Reward")]
    [SerializeField] public List<MyBaseEvent> combatRewards;
}
