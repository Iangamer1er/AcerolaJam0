using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public string altMapName = "Beta";

    private void Awake() {
        List<InfoEnemies> infos = new List<InfoEnemies>();
        var resources = ResoucesScript(GameManager.instance.level.ToString());
        if(resources.Length == null || resources.Length == 0){
            resources = ResoucesScript(altMapName);
        }
        foreach (InfoEnemies info in resources){
            infos.Add(info);
        }

        Debug.Log(infos);
    }
 
    private dynamic ResoucesScript(string mapNumber){
        return Resources.LoadAll(
            "Map" +
            mapNumber +
            "/Enemies"
        );
    }
}
