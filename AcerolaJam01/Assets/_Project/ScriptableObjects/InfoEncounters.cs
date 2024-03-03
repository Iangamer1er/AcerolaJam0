using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Encounter", menuName = "Map/Encounters")]
public class InfoEncounters : ScriptableObject
{
    [Header("Infos")]
    [SerializeField, TextArea] string DMDescription = "";
    [SerializeField, Range(0, 1)] float chance = 0.1f;
    [SerializeField, TextArea] string DMDescriptionFail = "Failed";
    [SerializeField, TextArea] string DMDescriptionSuccess = "Success";
}
