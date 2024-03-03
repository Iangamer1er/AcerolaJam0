using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemie", menuName = "Game Enemies")]
public class InfoEnemies : ScriptableObject
{
    [Header("Infos")]
    [SerializeField] string eName = "";

    [SerializeField, Range(0, 1)] float armsMaxHealth = 1; 
    [SerializeField, Range(0, 1)] float armsHealthRatio = 0.2f;
    [SerializeField, Range(0, 1)] float legsMaxHealth = 1; 
    [SerializeField, Range(0, 1)] float legsHealthRatio = 0.2f;
    [SerializeField, Range(0, 1)] float HeadMaxHealth = 1;
    [SerializeField, Range(0, 3)] float HeadModif = 1;
    [SerializeField, Range(0, 1)] float TorsoMaxHealth = 1f;
    [SerializeField, Range(0,1)] float spareAskChance = 0;
}
