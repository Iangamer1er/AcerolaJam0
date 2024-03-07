using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Map : ValidatedMonoBehaviour
{
    
    [Header("Initializing Tiles")]
    [SerializeField, Range(0, 50)] int maxTileHeight = 10;
    [SerializeField, Range(0, 10)] int nbForks = 1;
    [SerializeField, Range(2, 4)] int maxForkSplits = 3;
    [SerializeField, Range(1, 4)] int maxForkLength = 2;
    [Header("Making Map")]
    [SerializeField, Range(1, 5)] int maxMapHeight = 3;
    [SerializeField, Anywhere] Transform startingPoint;
    [SerializeField, Range(0, 1)] float timeNextPoint;
    [SerializeField, Range(0.01f, 3)] float mapWidth; //currentrly 2.457
    [SerializeField, Range(0.01f, 3)] float heightBetweenSpaces;

    [Header("Line Renderer")]
    [SerializeField, Min(0)] float lineRendererWidth = 0.02f;
    [SerializeField] Color lineColor = Color.black;

    public Dictionary<int, List<Tile>> tilesArrays 
        {get; set;} = new Dictionary<int, List<Tile>>();
    
    private int currentTileHeight = 0;
    private int currentNbForks = 0;
    private int nbTiles = 0;
    private bool hadFork = false;
    private bool canContinueRoutine = true;

    private void Start() {
        MakeMap();
        currentTileHeight = 0;
        StartCoroutine(CoCreateMap());
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
        GameObject newTile = new GameObject();
        List<TileTypes> possibilities = TileTypes.GetValues(typeof(TileTypes)).Cast<TileTypes>().ToList();
        if(currentlyUsed != null) possibilities = possibilities.Except(currentlyUsed).ToList();
        TileTypes type = possibilities[Random.Range(0, possibilities.Count())];
        Tile tileScript = newTile.AddComponent<Tile>();
        tileScript.type = type;
        tileScript.height = currentTileHeight;
        tileScript.lineRendererWidth = lineRendererWidth;
        tileScript.lineColor = lineColor;
        string forkName = "";
        bool isInFork = newFork != null;
        if(isInFork){
            tileScript.forkParent = newFork;
            newTile.transform.parent = newFork; 
            forkName = " Fork " + currentNbForks + " ";
        } 
        else newTile.transform.parent = transform; 
        newTile.name = "H" + currentTileHeight +
            forkName +
            "T" + nbTiles
        ;
        nbTiles++;
        if(!tilesArrays.ContainsKey(currentTileHeight)) tilesArrays[currentTileHeight] = new List<Tile>();
        tilesArrays[currentTileHeight].Add(tileScript);
        AddPossiblePath(newTile, posFork, isInFork);
        return type;
    }

    private void AddPossiblePath(GameObject tile, int posFork, bool isInFork = false){
        if(currentTileHeight == 0) return;
        if(isInFork && tilesArrays[currentTileHeight-1].Count > 1) tilesArrays[currentTileHeight-1][posFork].possiblePath.Add(tile);
        else if(isInFork) tilesArrays[currentTileHeight-1][0].possiblePath.Add(tile);
        else{
            for (int i = 0; i < tilesArrays[currentTileHeight-1].Count(); i++){
                tilesArrays[currentTileHeight-1][i].possiblePath.Add(tile);
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
        newFork.transform.position = transform.position;
        newFork.transform.rotation = transform.rotation;
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

    private IEnumerator CoCreateMap(){
        for(int i = 0; i < maxMapHeight; i++){
            canContinueRoutine = false;
            StartCoroutine(CoInstantiateTile(startingPoint, i));
            yield return new WaitUntil(()=>canContinueRoutine);
            currentTileHeight++;
        }
        currentTileHeight = 0;
        StartCoroutine(CoMakePaths());
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator CoInstantiateTile(Transform StartingPos, float heightOnMap){
        float spaceBetween = 
            mapWidth /
            (tilesArrays[currentTileHeight].Count + 1)
        ;
        for(int i = 0; i < tilesArrays[currentTileHeight].Count; i++){
            Tile currentTileScript = tilesArrays[currentTileHeight][i];
            float tilePosX = -mapWidth * 0.5f;
            tilePosX += spaceBetween * (i+1);
            currentTileScript.gameObject.transform.position = StartingPos.position;
            currentTileScript.gameObject.transform.rotation = startingPoint.rotation;
            currentTileScript.gameObject.transform.localPosition = new Vector3(
                tilePosX, 0, (heightOnMap + 1)*heightBetweenSpaces
            );
            currentTileScript.AddTile();
            yield return new WaitForSeconds(timeNextPoint);
        }
        canContinueRoutine = true;
    }

    private IEnumerator CoMakePaths(){
        canContinueRoutine = false;
        for(int i = 0; i < maxMapHeight - 1; i++){
            canContinueRoutine = false;
            StartCoroutine(CoInstantiatePaths());
            yield return new WaitUntil(()=>canContinueRoutine);
            currentTileHeight++;
        }
    }

    private IEnumerator CoInstantiatePaths(){
        for(int e = 0; e < tilesArrays[currentTileHeight].Count; e++){
            StartCoroutine(tilesArrays[currentTileHeight][e].CoMakePath(timeNextPoint));
            yield return new WaitForSeconds(timeNextPoint);
        }
        canContinueRoutine = true;
    }

    [ContextMenu("Advance Tile")]
    public void ContextMenu(){
        StartCoroutine(CoAdvanceOneTile());
    }

    private IEnumerator CoAdvanceOneTile(){
        int targetMapHeight = currentTileHeight - maxMapHeight;
        for (int i = 0; i < tilesArrays[targetMapHeight].Count; i++){
            Destroy(tilesArrays[targetMapHeight][i]);
            yield return new WaitForSeconds(timeNextPoint);
        }

        for (int i = 0; i < maxMapHeight - 1; i++){
            foreach (Tile currentTileScript in tilesArrays[targetMapHeight + i]){
                currentTileScript.transform.position = new Vector3(
                    currentTileScript.transform.position.x,
                    currentTileScript.transform.position.y,
                    currentTileScript.transform.position.z - heightBetweenSpaces
                );
            }
        }
        currentTileHeight++;
        canContinueRoutine = false;
        StartCoroutine(CoInstantiateTile(startingPoint, maxMapHeight - 1));
        yield return new WaitUntil(()=>canContinueRoutine);
        StartCoroutine(CoMakePaths());
    }

    private bool checkNbTiles(){
        float random = Random.Range(0f, 1f);
        return ((float)currentTileHeight/(float)maxTileHeight >= random ||
            maxTileHeight - currentTileHeight <= maxForkLength * nbForks) &&
            nbForks - currentNbForks > 0
            && !hadFork
        ;
    }
}
