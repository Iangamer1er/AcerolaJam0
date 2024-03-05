using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "DM/Dialogue")]
public class InfoDialogue : ScriptableObject
{
    [Header("Infos")]
    [SerializeField] string eName = "";
    [SerializeField] MyListDialogues[] Act;
}

[System.Serializable]
public class MyListDialogues {
    [SerializeField, TextArea] public List<string> dialogue;
}