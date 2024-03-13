using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public string altMapName = "Beta";
    [SerializeField] InfoEnemies infoBoss;

    [Header("Current Stats")]
    public float armsHealth;
    public float legsHealth;
    public float torsoHealth;
    public float damage;
    
    private EnemyBase currentState;
    
    public InfoEnemies info;

    // private void Start() {
    //     info = ChoseEnemy();
    //     InitialiseStats(info);
    //     ChangeState(info.behavoir);
    // }
 
    private List<InfoEnemies> ResoucesScript(string mapNumber){
        return Resources.LoadAll(
            "Map" +
            mapNumber +
            "/Enemies"
        ).Cast<InfoEnemies>().ToList();
    }

    public InfoEnemies ChoseEnemy(){
        if(Player.instance.isOnBoss) return infoBoss;
        List<InfoEnemies> infos;
        infos = ResoucesScript(GameManager.instance.level.ToString());
        if(infos.Count == 0){
            infos = ResoucesScript(altMapName);
        }
        return infos[Random.Range(0, infos.Count)];
    }

    [ContextMenu("Kill")]
    public void ContextMenu(){
        torsoHealth =0;
        StartCoroutine(CoCheckHealth());
    }

    public void InitialiseStats(InfoEnemies info){
        armsHealth = info.armsMaxHealth;
        legsHealth = info.legsMaxHealth;
        torsoHealth = info.torsoMaxHealth;
        damage = info.damage;
    }

    public IEnumerator CoCheckBrokenArms(){
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        if(armsHealth <= 0){
            damage = info.Brokendamage;
            StartCoroutine(DM.instance.Talk(DM.instance.EArmsBrokenTxt));
        }
    }

    public IEnumerator CoCheckBrokenLegs(){
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        if(legsHealth <= 0){
            StartCoroutine(DM.instance.Talk(DM.instance.ELegsBrokenTxt));
        }
    }

    public void ChangeState(EnemyBase state){
        if(state == null) state = new EnemyNormalState();
        currentState = state;
        InitialiseStats(info);
        currentState.InitState(this);
    }

    public IEnumerator CoTakeDamage(float damage, BodyParts part){
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        StartCoroutine(DM.instance.Talk(DM.instance.PAttackTxt));
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        if(Random.Range(0f, legsHealth/info.legsMaxHealth) > 1 - info.dodgeChance){
            AudioManager.instance.PlayEffect(AudioManager.instance.swordMiss, true);
            StartCoroutine(DM.instance.Talk(DM.instance.EdodgeTxt));
            yield return new WaitUntil(()=>DM.instance.doneTalking);
            StartCoroutine(DM.instance.Talk(info.attackTxt));
            yield return new WaitUntil(()=>DM.instance.doneTalking);
            Attack();
        }else{
            currentState.TakeDamage(this, damage, part);
            AudioManager.instance.PlayEffect(AudioManager.instance.swordHit, true);
            if(info.behavoirLowHealth != null && torsoHealth <= info.lowHealthThreshhold) ChangeState(info.behavoirLowHealth);
            yield return null;
        }
    }

    public IEnumerator CoCheckHealth(){
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        if(torsoHealth <= 0){
            AudioManager.instance.PlayEffect(AudioManager.instance.enemyDead, true);
            StartCoroutine(DM.instance.Talk(DM.instance.EDeadTxt));
            yield return new WaitUntil(()=>DM.instance.doneTalking);
            Player.instance.ChangeFavor(-0.1f);
            Player.instance.canInteract = true;
            List<List<MyBaseEvent>> possibleRewards = new List<List<MyBaseEvent>>();
            if(CheckCombatReward(info.lowCombatRewards)) possibleRewards.Add(info.lowCombatRewards);
            if(CheckCombatReward(info.midCombatRewards)) possibleRewards.Add(info.midCombatRewards);
            if(CheckCombatReward(info.highCombatRewards)) possibleRewards.Add(info.highCombatRewards);
            List<MyBaseEvent> chosenReward = possibleRewards[Random.Range(0, possibleRewards.Count)];
            foreach (MyBaseEvent reward in chosenReward){
                EventManager.instance.ChangeState(reward.baseEvent);
            }
            yield return new WaitUntil(()=>DM.instance.doneTalking);
            Player.instance.FinishEncounter();
            Player.instance.inCombatEvent.Invoke();
        }else{
            StartCoroutine(DM.instance.Talk(info.attackTxt));
            yield return new WaitUntil(()=>DM.instance.doneTalking);
            Attack();
        }
    }

    private bool CheckCombatReward(List<MyBaseEvent> reward){
        if(reward == null) return false;
        if(reward.Count == 0) return false;
        return true;
    }

    public void Attack(){
        float chosenDamage = armsHealth > 0 ? info.damage : info.Brokendamage;
        currentState.Attack(this, chosenDamage);
    }

    public IEnumerator CoSpare(){
        bool spared;
        if(Random.Range(0f, torsoHealth/info.torsoMaxHealth) <= info.spareAskChance){
            StartCoroutine(DM.instance.Talk(DM.instance.ESparedTxt));
            spared = true;
        }else{
            StartCoroutine(DM.instance.Talk(DM.instance.ESpareFailTxt));
            spared = false;
        } 
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        currentState.Spare(this, spared);
    }
}
