using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Map : ValidatedMonoBehaviour
{
    [SerializeField, Range(0, 50)] int maxTileHeight = 10;
    [SerializeField, Range(0, 10)] int nbForks = 1;
    [SerializeField, Range(2, 4)] int maxForkSplits = 3;
    [SerializeField, Range(1, 4)] int maxForkLength = 2;
    [SerializeField] GameObject tile;
    [SerializeField, Range(1, 5)] int maxMapHeight = 3;
    [SerializeField] Transform startingPoint;

    public Dictionary<int, List<Tile>> tilesArrays 
        {get; set;} = new Dictionary<int, List<Tile>>();
    
    private int currentTileHeight = 0;
    private int currentNbForks = 0;
    private int nbTiles = 0;
    bool hadFork = false;

    private void Start() {
        MakeMap();
        currentTileHeight = 0;
    }

    private void MakeMap(){
        for(int e = 0; e < maxTileHeight; e++){
            if(!CalculateForkChance()){
                MakeTile(null);
                hadFork = false;
                currentTileHeight++;
            } 
        }
    }

    private TileTypes MakeTile(List<TileTypes> currentlyUsed, Transform newFork = null, int posFork = 0){
        // GameObject newTile = Instantiate(tile);
        List<TileTypes> possibilities = TileTypes.GetValues(typeof(TileTypes)).Cast<TileTypes>().ToList();
        if(currentlyUsed != null) possibilities = possibilities.Except(currentlyUsed).ToList();
        TileTypes type = possibilities[Random.Range(0, possibilities.Count())];
        // Tile tileScript = newTile.AddComponent<Tile>();
        Tile tileScript = new Tile();
        tileScript.height = currentTileHeight;
        string forkName = "";
        bool isInFork = newFork != null;
        if(isInFork){
            // newTile.transform.parent = newFork; 
            forkName = " Fork " + currentNbForks + " ";
        } 
        // else newTile.transform.parent = transform; 
        // newTile.name = "H" + currentTileHeight +
        //     forkName +
        //     "T" + nbTiles
        // ;
        nbTiles++;
        if(!tilesArrays.ContainsKey(currentTileHeight)) tilesArrays[currentTileHeight] = new List<Tile>();
        tilesArrays[currentTileHeight].Add(tileScript);
        AddPossiblePath(tileScript, posFork, isInFork);
        return type;
    }

    private void AddPossiblePath(Tile tileScript, int posFork, bool isInFork = false){
        if(currentTileHeight == 0) return;
        if(isInFork && tilesArrays[currentTileHeight-1].Count > 1) tilesArrays[currentTileHeight-1][posFork].possiblePath.Add(tileScript);
        else if(isInFork) tilesArrays[currentTileHeight-1][0].possiblePath.Add(tileScript);
        else{
            for (int i = 0; i < tilesArrays[currentTileHeight-1].Count(); i++){
                tilesArrays[currentTileHeight-1][i].possiblePath.Add(tileScript);
            }
        }
    }

    private bool CalculateForkChance(){
        if(!checkNbTiles()) return false;
        hadFork = true;
        int forkSplits = Random.Range(2, maxForkSplits);
        int randLenght = Random.Range(1, maxForkLength);
        int remainingTile = maxTileHeight - currentTileHeight;
        int forkLenght = randLenght <= remainingTile ? randLenght : remainingTile;
        GameObject newFork = new GameObject("H" + currentTileHeight + " Fork " + currentNbForks);
        newFork.transform.parent = transform;
        for(int e = 0; e < forkLenght; e++){
            List<TileTypes> currentlyUsed = new List<TileTypes>();
            for(int i = 0; i < forkSplits; i++){
                currentlyUsed.Add(MakeTile(currentlyUsed, newFork.transform, i));
            }
            currentTileHeight++;
        }
        currentNbForks++;
        return true;
    }

    private IEnumerator CoroutineCreateMap(){
        for (int i = 0; i < maxMapHeight; i++){
            InstantiateTile()
        }
        yield return new WaitForEndOfFrame();
    }

    private void InstantiateTile(Vector3 StartingPos){
            for (int e = 0; e < tilesArrays[currentTileHeight].Count; e++){
                
            }
    }

    private bool checkNbTiles(){
        float random = Random.Range(0f, 1f);
        return ((float)currentTileHeight/(float)maxTileHeight >= random ||
            maxTileHeight - currentTileHeight < 2) &&
            nbForks - currentNbForks > 0
            && !hadFork
        ;
    }
}
