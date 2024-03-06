using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Encounter : MonoBehaviour
{
    public InfoEncounters info;
    public string altMapName = "Beta";

    private List<InfoEncounters> ResoucesScript(string mapNumber){
        return Resources.LoadAll(
            "Map" +
            mapNumber +
            "/Encounters"
        ).Cast<InfoEncounters>().ToList();
    }

    private InfoEncounters ChoseEncounter(){
        List<InfoEncounters> infos;
        infos = ResoucesScript(GameManager.instance.level.ToString());
        if(infos.Count == 0){
            infos = ResoucesScript(altMapName);
        }
        return infos[Random.Range(0, infos.Count)];
    }
}
