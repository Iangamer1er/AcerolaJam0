using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossState : EnemyBase{
    
    public override void InitState(EnemyManager enemy){
        enemy.StartCoroutine(DM.instance.Talk(enemy.info.description));
        Player.instance.isOnBoss = true;
        enemy.StartCoroutine(WaitUntil());
    }

    private IEnumerator WaitUntil(){
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        Player.instance.canInteract = true;
    }

    public override void TakeDamage(EnemyManager enemy, float damage, BodyParts part){
        float damageInflicted;
        switch(part){
            case BodyParts.Arms : 
                damageInflicted = damage * enemy.info.armsHealthRatio;
                enemy.armsHealth -= damageInflicted;
                enemy.torsoHealth -= damageInflicted;
                enemy.StartCoroutine(enemy.CoCheckBrokenArms());
                break;
            case BodyParts.Legs : 
                damageInflicted = damage * enemy.info.legsHealthRatio;
                enemy.legsHealth -= damageInflicted;
                enemy.torsoHealth -= damageInflicted;
                enemy.StartCoroutine(enemy.CoCheckBrokenLegs());
                break;
            case BodyParts.Head : 
                if(Random.Range(0, 1) <= enemy.info.headChance){
                    damageInflicted = damage * enemy.info.headModif;
                    enemy.torsoHealth -= damageInflicted;
                    break;
                }
                enemy.StartCoroutine(DM.instance.Talk(DM.instance.BdodgeHeadTxt));
                break;
            case BodyParts.Torso : 
                damageInflicted = damage * enemy.info.torsoModif;
                enemy.torsoHealth -= damageInflicted;
                break;
            default:
                Debug.Log("Error with parts");
                break;
        }
        enemy.StartCoroutine(enemy.CoCheckHealth());
    }

    public override void Attack(EnemyManager enemy, float damage){
        //todo make multi attack a chance of procs
        //todo make the boss swap into 2nd phase at 30% health
        float newRandom = Random.Range(0, 1f);
        int randomAttack = Random.Range(2, 4);
        if(newRandom < 0.25) enemy.StartCoroutine(MultiAttack(enemy, damage, randomAttack));
        else enemy.StartCoroutine(Player.instance.CoTakeBossDamage(damage, true));
        
    }

    private IEnumerator MultiAttack(EnemyManager enemy, float damage, int numberAttacks){
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        for (int i = 0; i < numberAttacks; i++){
            yield return new WaitUntil(()=>DM.instance.doneTalking);
            enemy.StartCoroutine(Player.instance.CoTakeBossDamage(damage * 0.33f, i == numberAttacks - 1));
        }
    }

    public override void Spare(EnemyManager enemy, bool spared){

    }
}
