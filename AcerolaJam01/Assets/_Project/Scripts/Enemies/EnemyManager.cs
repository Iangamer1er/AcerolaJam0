using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public string altMapName = "Beta";

    [Header("Current Stats")]
    public float armsHealth;
    public float legsHealth;
    public float headHealth;
    public float torsoHealth;
    
    private EnemyBase currentState;

    private void Start() {
        InfoEnemies info = ChoseEnemy();
        InitialiseStats(info);
    }
 
    private List<InfoEnemies> ResoucesScript(string mapNumber){
        return Resources.LoadAll(
            "Map" +
            mapNumber +
            "/Enemies"
        ).Cast<InfoEnemies>().ToList();
    }

    private InfoEnemies ChoseEnemy(){
        List<InfoEnemies> infos;
        infos = ResoucesScript(GameManager.instance.level.ToString());
        if(infos.Count == 0){
            infos = ResoucesScript(altMapName);
        }
        return infos[Random.Range(0, infos.Count)];
    }

    private void InitialiseStats(InfoEnemies info){
        armsHealth = info.armsMaxHealth;
        legsHealth = info.legsMaxHealth;
        headHealth = info.headMaxHealth;
        torsoHealth = info.torsoMaxHealth;
    }

    public void ChangeState(EnemyBase state, InfoEnemies infos){
        currentState = state;
        currentState.InitState(this, infos);
    }

    
}
