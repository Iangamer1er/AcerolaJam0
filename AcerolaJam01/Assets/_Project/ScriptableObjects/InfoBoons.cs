using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boon", menuName = "Map/Boons")]
public class InfoBoons : ScriptableObject
{
    [Header("Infos")]
    [SerializeField, TextArea] string DMDescription = "";
    [SerializeField, Range(0, 1)] float chanceWarriorGod = 0.1f;
    [SerializeField, TextArea] string DMDescriptionFail = "Failed";
    [SerializeField, TextArea] string DMDescriptionSuccess = "Success";
    [SerializeField, TextArea] string DMDescriptionOtherGod = "Shameful";
}
