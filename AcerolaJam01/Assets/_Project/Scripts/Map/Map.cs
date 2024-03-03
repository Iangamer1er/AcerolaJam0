using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;

public class Map : ValidatedMonoBehaviour
{
    [SerializeField, Range(0, 50)] int nbEncounters = 10;
    [SerializeField, Range(0, 10)] int nbForks = 1;
    [SerializeField, Range(2, 4)] int maxForkSplits = 3;
    private int currentNbEncounters;
    private int currentNbForks;

    private void Awake() {
        currentNbEncounters = nbEncounters;
        currentNbForks = nbForks;
    }

    private void MakeEncounters(){
        for (int i = 0; i < nbEncounters; i++){
            
        }
    }

    private void MakeForksPath(){
        if(currentNbForks < 1) return;

    }

    private void CalculateForkChance(){
        if(!checkNbEncounters()) return;
        currentNbForks--;
        int forkSplits = Random.Range(2, maxForkSplits);
    }

    private bool checkNbEncounters(){
        return (float)currentNbEncounters/(float)nbEncounters>= Random.Range(0, 1) ||
            nbEncounters - currentNbEncounters < 3;
    }
}
