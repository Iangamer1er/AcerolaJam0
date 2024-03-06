using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int height;
    public TileTypes type = TileTypes.Random;
    public List<Tile> possiblePath = new List<Tile>();
    public bool isForked = false;
    public Transform forkParent;

    private dynamic tileObject;

    private void Start() {
        AddTile(type);
    }

    private void AddTile(TileTypes tileType){
        switch(tileType){
            case TileTypes.Encounter : 
            
                break;
            case TileTypes.Enemi : 

                break;
            case TileTypes.Boon : 

                break;
            case TileTypes.Random : 

                break;
        }
    }


}

public enum TileTypes{Encounter, Enemi, Boon, Random}
