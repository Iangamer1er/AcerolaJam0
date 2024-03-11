using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormalState : EnemyBase{
    
    public override void InitState(EnemyManager enemy){
        enemy.StartCoroutine(DM.instance.Talk(enemy.info.description));
    }

    public override void TakeDamage(EnemyManager enemy, float damage, BodyParts part){
        float damageInflicted;
        switch(part){
            case BodyParts.Arms : 
                damageInflicted = damage * enemy.info.armsHealthRatio;
                enemy.armsHealth -= damageInflicted;
                enemy.torsoHealth -= damageInflicted;
                // enemy.StartCoroutine(enemy.coche)
                Debug.Log(part +" health : " + enemy.armsHealth);
                Debug.Log(BodyParts.Torso +" health : " + enemy.torsoHealth);
                break;
            case BodyParts.Legs : 
                damageInflicted = damage * enemy.info.legsHealthRatio;
                enemy.legsHealth -= damageInflicted;
                enemy.torsoHealth -= damageInflicted;
                Debug.Log(part +" health : " + enemy.legsHealth);
                Debug.Log(BodyParts.Torso +" health : " + enemy.torsoHealth);
                break;
            case BodyParts.Head : 
                if(Random.Range(0, 1) >= enemy.info.headChance){
                    DM.instance.Talk(DM.instance.EdodgeTxt);
                    break;
                }
                damageInflicted = damage * enemy.info.headModif;
                enemy.torsoHealth -= damageInflicted;
                Debug.Log("Damage inflicted : " + damageInflicted);
                Debug.Log(BodyParts.Torso +" health : " + enemy.torsoHealth);
                break;
            case BodyParts.Torso : 
                damageInflicted = damage * enemy.info.torsoModif;
                enemy.torsoHealth -= damageInflicted;
                Debug.Log(BodyParts.Torso +" health : " + enemy.torsoHealth);
                break;
            default:
                Debug.Log("Error with parts");
                break;
        }
        enemy.StartCoroutine(enemy.CoCheckHealth());
    }

    public override void Attack(EnemyManager enemy, float damage){
        enemy.StartCoroutine(Player.instance.CoTakeDamage(damage));
    }

    public override void Spare(EnemyManager enemy, bool spared){
        if(!spared){
            Attack(enemy, enemy.info.damage);
            return;
        }
        // todo spared reward and end of combat
    }
}
