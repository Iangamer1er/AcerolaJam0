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
    public float torsoHealth;
    
    private EnemyBase currentState;
    private bool brokenLegs, brokenArms, brokenHead = false;
    public InfoEnemies info;

    private void Start() {
        info = ChoseEnemy();
        InitialiseStats(info);
        ChangeState(info.behavoir);
    }
 
    private List<InfoEnemies> ResoucesScript(string mapNumber){
        return Resources.LoadAll(
            "Map" +
            mapNumber +
            "/Enemies"
        ).Cast<InfoEnemies>().ToList();
    }

    public InfoEnemies ChoseEnemy(){
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
        torsoHealth = info.torsoMaxHealth;
    }

    public void ChangeState(EnemyBase state){
        currentState = state;
        currentState.InitState(this);
    }

    public void TakeDamage(float damage, BodyParts part){
        currentState.TakeDamage(this, damage, part);
        if(info.behavoirLowHealth != null && torsoHealth <= info.lowHealthThreshhold) ChangeState(info.behavoirLowHealth);
    }

    public void Attack(float damage, BodyParts part){
        currentState.TakeDamage(this, damage, part);
    }
}
