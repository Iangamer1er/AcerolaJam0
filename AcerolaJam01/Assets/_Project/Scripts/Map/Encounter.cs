using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Encounter : MonoBehaviour
{
    public InfoEncounters infoEncounter;
    public InfoBoons infoBoon;
    public string altMapName = "Beta";
    private List<InfoEncounters> possibleEncounters;
    private List<InfoBoons> possibleBoons;

    private void Start() {
        possibleEncounters = ResoucesScriptEncounter(altMapName);
        possibleBoons = ResoucesScriptBoon(altMapName);    
    }

    public void CheckEncounters(){
        if(possibleEncounters.Count == 0) ResetEncounters();
        else ChoseEncounter();
    }

    public void CheckBoons(){
        if(possibleBoons.Count == 0) ResetBoons();
        else ChoseBoon();
    }

    private void ResetEncounters(){
        possibleEncounters = ResoucesScriptEncounter(GameManager.instance.level.ToString());
        if(possibleEncounters.Count == 0){
            possibleEncounters = ResoucesScriptEncounter(altMapName);
        }
        infoEncounter = ChoseEncounter();
    }

    private void ResetBoons(){
        possibleBoons = ResoucesScriptBoon(GameManager.instance.level.ToString());
        if(possibleBoons.Count == 0){
            possibleBoons = ResoucesScriptBoon(altMapName);
        } 
        infoBoon = ChoseBoon();
    }

    private InfoEncounters ChoseEncounter() => possibleEncounters[Random.Range(0, possibleEncounters.Count)];
    private InfoBoons ChoseBoon() => possibleBoons[Random.Range(0, possibleBoons.Count)];

    private List<InfoEncounters> ResoucesScriptEncounter(string mapNumber){
        return Resources.LoadAll(
            "Map" +
            mapNumber +
            "/Encounters"
        ).Cast<InfoEncounters>().ToList();
    }


    private List<InfoBoons> ResoucesScriptBoon(string mapNumber){
        return Resources.LoadAll(
            "Map" +
            mapNumber +
            "/Boons"
        ).Cast<InfoBoons>().ToList();
    }


    public IEnumerator CoPlayerChoseEncounter(bool choseChance){
        if(choseChance == false){
            DM.instance.Talk(DM.instance.EventSkipTxt);
            yield return new WaitUntil(()=>DM.instance.doneTalking);
            Player.instance.FinishEncounter();
        }
        List<MyBaseEvent> events;
        if(infoEncounter.chance <= Random.Range(0f, 1f)) events = infoEncounter.eventWin;
        else events = infoEncounter.eventFail;
        StartCoroutine(CoGiveEvent(events));
    }

    private IEnumerator CoGiveEvent(List<MyBaseEvent> events){
        foreach (MyBaseEvent baseEvent in events){
            yield return new WaitUntil(()=>DM.instance.doneTalking);
            EventManager.instance.ChangeState(baseEvent.baseEvent);
            yield return new WaitUntil(()=>DM.instance.doneTalking);
        }
        Player.instance.FinishEncounter();
    }
}
