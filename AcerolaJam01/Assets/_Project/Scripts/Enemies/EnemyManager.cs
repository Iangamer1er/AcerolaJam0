using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public string altMapName = "Beta";

    private void Start() {
        List<InfoEnemies> infos;
        Debug.Log(GameManager.instance.level.ToString());
        infos = ResoucesScript(GameManager.instance.level.ToString());
        if(infos.Count == 0){
            infos = ResoucesScript(altMapName);
        }

        Debug.Log(infos);
    }
 
    private List<InfoEnemies> ResoucesScript(string mapNumber){
        return Resources.LoadAll(
            "Map" +
            mapNumber +
            "/Enemies"
        ).Cast<InfoEnemies>().ToList();
    }
}
