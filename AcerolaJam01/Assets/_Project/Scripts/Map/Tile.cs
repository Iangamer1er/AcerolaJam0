using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float lineRendererWidth = 0.2f;
    public Color lineColor = Color.black;
    public int height;
    public TileTypes type = TileTypes.Random;
    public List<Tile> possiblePath = new List<Tile>();
    public bool isForked = false;
    public Transform forkParent;

    private dynamic tileObject;

    public void AddTile(){
        GameObject prefabTile;
        switch(type){
            case TileTypes.Encounter : 
                prefabTile = (GameObject)Resources.Load("Spaces/encounter");
                break;
            case TileTypes.Enemy : 
                prefabTile = (GameObject)Resources.Load("Spaces/enemy");
                break;
            case TileTypes.Boon : 
                prefabTile = (GameObject)Resources.Load("Spaces/boon");
                break;
            case TileTypes.Random : 
                prefabTile = (GameObject)Resources.Load("Spaces/random");
                break;
            default :
                prefabTile = (GameObject)Resources.Load("Spaces/random");
                break;
        }
        Instantiate(prefabTile, transform);
    }

    private void MakePath(){
        foreach (Tile path in possiblePath){
            LineRenderer line = gameObject.AddComponent<LineRenderer>();
            line.startWidth = lineRendererWidth;
            line.endWidth = lineRendererWidth;
            line.positionCount = 2;
            line.startColor = lineColor;
            line.endColor = lineColor;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, path.transform.position);
        }
    }


}

public enum TileTypes{Encounter, Enemy, Boon, Random}
