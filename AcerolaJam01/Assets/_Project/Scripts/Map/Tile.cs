using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int maskInteractable;
    public float lineRendererWidth = 0.2f;
    public Color lineColor = Color.black;
    public int height;
    public TileTypes type = TileTypes.Random;
    public List<GameObject> possiblePath = new List<GameObject>();
    public List<GameObject> lines = new List<GameObject>();
    public bool isForked = false;
    public Transform forkParent;
    public bool touched = false;

    public void AddTile(){
        GameObject prefabTile;
        prefabTile = (GameObject)Resources.Load("Spaces/" + type.ToString());
        prefabTile = Instantiate(prefabTile, transform);
        prefabTile.layer = maskInteractable;
        foreach (Transform child in prefabTile.GetComponentsInChildren<Transform>()){
            child.gameObject.layer = maskInteractable;
            child.gameObject.tag = type.ToString();
        }
        prefabTile.tag = type.ToString();
    }

    public IEnumerator CoMakePath(float waitTime){
        foreach (GameObject path in possiblePath){
            GameObject lineObj = new GameObject("Line");
            lineObj.transform.position = transform.position;
            lineObj.transform.rotation = transform.rotation;
            lineObj.transform.parent = transform;
            LineRenderer line = lineObj.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            line.startWidth = lineRendererWidth;
            line.endWidth = lineRendererWidth;
            line.positionCount = 2;
            line.startColor = lineColor;
            line.endColor = lineColor;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, path.transform.position);
            lines.Add(lineObj);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void ClearPaths(){
        foreach (GameObject line in lines){
            Destroy(line);
        }
    }
}

public enum TileTypes{Encounter, Enemy, Boon, Random}
