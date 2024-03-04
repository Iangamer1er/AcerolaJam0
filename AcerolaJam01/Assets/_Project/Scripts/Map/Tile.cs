using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int height;
    public TileTypes type = TileTypes.Random;
    public List<Tile> possiblePath = new List<Tile>();
    public bool isForked = false;
}

public enum TileTypes{Encouter, Enemi, Boon, Random}
