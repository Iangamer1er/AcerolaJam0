using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Boon : MonoBehaviour
{
    public InfoBoons info;
    public string altMapName = "Beta";

    private List<InfoBoons> ResoucesScript(string mapNumber){
        return Resources.LoadAll(
            "Map" +
            mapNumber +
            "/Boons"
        ).Cast<InfoBoons>().ToList();
    }

    public InfoBoons ChoseBoon(){
        List<InfoBoons> infos;
        infos = ResoucesScript(GameManager.instance.level.ToString());
        if(infos.Count == 0){
            infos = ResoucesScript(altMapName);
        }
        return infos[Random.Range(0, infos.Count)];
    }
}
