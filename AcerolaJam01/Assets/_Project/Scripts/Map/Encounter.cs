using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Encounter : MonoBehaviour
{
    [SerializeField, Min(0)] private int minEncounters = 3;
    public InfoEncounters infoEncounter;
    public InfoBoons infoBoon;
    public string altMapName = "Beta";
    public bool isBoon = false;
    private List<InfoEncounters> possibleEncounters;
    private List<InfoBoons> possibleBoons;

    private void Start() {
        possibleEncounters = ResoucesScriptEncounter(altMapName);
        possibleBoons = ResoucesScriptBoon(altMapName);    
    }

    public IEnumerator StartEncounter(){
        if(isBoon){
            CheckBoons();
            StartCoroutine(DM.instance.Talk(infoBoon.DMDescription));
        }else{
            CheckEncounters(); 
            StartCoroutine(DM.instance.Talk(infoEncounter.DMDescription));
        }
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        Player.instance.canInteract = true;
    }

    private void CheckEncounters(){
        if(possibleEncounters.Count <= minEncounters) ResetEncounters();
        infoEncounter = ChoseEncounter();
        possibleEncounters.Remove(infoEncounter);
    }

    private void CheckBoons(){
        if(possibleBoons.Count <= minEncounters) ResetBoons();
        infoBoon = ChoseBoon();
        possibleBoons.Remove(infoBoon);
    }

    private void ResetEncounters(){
        possibleEncounters = ResoucesScriptEncounter(GameManager.instance.level.ToString());
        if(possibleEncounters.Count == 0) possibleEncounters = ResoucesScriptEncounter(altMapName);
    }

    private void ResetBoons(){
        possibleBoons = ResoucesScriptBoon(GameManager.instance.level.ToString());
        if(possibleBoons.Count == 0) possibleBoons = ResoucesScriptBoon(altMapName);
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


    public IEnumerator CoPlayerTakeEncounter(bool choseChance){
        if(choseChance == false){
            DM.instance.Talk(DM.instance.EventSkipTxt);
            yield return new WaitUntil(()=>DM.instance.doneTalking);
            Player.instance.FinishEncounter();
        }else{
            List<MyBaseEvent> events;
            if(infoEncounter.chance <= Random.Range(0f, 1f)){
                events = infoEncounter.eventWin; 
                DM.instance.Talk(infoEncounter.DMDescriptionSuccess);
            }else{
                events = infoEncounter.eventFail;  
                DM.instance.Talk(infoEncounter.DMDescriptionFail);
            } 
            yield return new WaitUntil(()=>DM.instance.doneTalking);
            StartCoroutine(CoGiveEvent(events));
        }
    }

    public IEnumerator CoPlayerChoseBoon(bool choseWarriorGod){
        List<MyBaseEvent> events;
        if(choseWarriorGod == false){
            DM.instance.Talk(infoBoon.DMDescriptionOtherGod);
            events = infoBoon.eventOuterGod;
        }else if(infoBoon.chance <= Random.Range(0f, 1f)){
            DM.instance.Talk(infoBoon.DMDescriptionSuccess);
            events = infoBoon.eventWarriorGodSuccess; 
        }else{
            DM.instance.Talk(infoBoon.DMDescriptionFail);
            events = infoBoon.eventWarriorGodFail; 
        } 
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        StartCoroutine(CoGiveEvent(events));
    }

    public IEnumerator CoGiveEvent(List<MyBaseEvent> events){
        foreach (MyBaseEvent baseEvent in events){
            yield return new WaitUntil(()=>DM.instance.doneTalking);
            EventManager.instance.ChangeState(baseEvent.baseEvent);
            yield return new WaitUntil(()=>DM.instance.doneTalking);
        }
        Player.instance.FinishEncounter();
    }
}
