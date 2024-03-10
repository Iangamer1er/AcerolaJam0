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
    public float damage;
    
    private EnemyBase currentState;
    
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
        damage = info.damage;
    }

    public IEnumerator CheckBrokenArms(){
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        if(armsHealth <= 0){
            damage = info.Brokendamage;
            
        }
    }

    public IEnumerator CheckBrokenLegs(){
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        if(legsHealth <= 0){
            
        }
    }

    public void ChangeState(EnemyBase state){
        currentState = state;
        currentState.InitState(this);
    }

    public IEnumerator CoTakeDamage(float damage, BodyParts part){
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        if(Random.Range(0f, 1f) < legsHealth/info.legsMaxHealth){
            StartCoroutine(DM.instance.Talk(DM.instance.EdodgeTxt));
            yield return new WaitUntil(()=>DM.instance.doneTalking);
        }else{
            currentState.TakeDamage(this, damage, part);
            if(info.behavoirLowHealth != null && torsoHealth <= info.lowHealthThreshhold) ChangeState(info.behavoirLowHealth);
            yield return null;
        }
    }

    public IEnumerator CoCheckHealth(){
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        if(torsoHealth <= 0){
            StartCoroutine(DM.instance.Talk(DM.instance.EDeadTxt));
            yield return new WaitUntil(()=>DM.instance.doneTalking);
            Player.instance.canInteract = true;
        }else{
            StartCoroutine(DM.instance.Talk(info.attackTxt));
            yield return new WaitUntil(()=>DM.instance.doneTalking);
            Attack(info.damage);
        }
    }

    public void Attack(float damage){
        currentState.Attack(this, damage);
    }

    public IEnumerator CoSpare(){
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        if(Random.Range(0f, legsHealth/info.legsMaxHealth) < info.spareAskChance){

        }
    }
}
