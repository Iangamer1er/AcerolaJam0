using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormalState : EnemyBase{
    
    public override void InitState(EnemyManager enemy){
        
    }

    public override void TakeDamage(EnemyManager enemy, float damage, BodyParts part){
        float damageInflicted;
        switch(part){
            case BodyParts.Arms : 
                damageInflicted = damage * enemy.info.armsHealthRatio;
                enemy.armsHealth -= damageInflicted;
                enemy.torsoHealth -= damageInflicted;
                break;
            case BodyParts.Legs : 
                damageInflicted = damage * enemy.info.legsHealthRatio;
                enemy.legsHealth -= damageInflicted;
                enemy.torsoHealth -= damageInflicted;
                break;
            case BodyParts.Head : 
                if(Random.Range(0, 1) >= enemy.info.headChance) break;
                damageInflicted = damage * enemy.info.headModif;
                enemy.torsoHealth -= damageInflicted;
                break;
            case BodyParts.Torso : 
                damageInflicted = damage * enemy.info.armsHealthRatio;
                enemy.torsoHealth -= damageInflicted;
                break;
            default:
                Debug.Log("Error with parts");
                break;
        }
    }

    public override void Attack(EnemyManager enemy, float damage, BodyParts part){
        switch(part){
            case BodyParts.Arms : 

                break;
            case BodyParts.Legs : 

                break;
            case BodyParts.Head : 

                break;
            case BodyParts.Torso : 

                break;
            default:
                Debug.Log("Error with parts");
                break;
        }
    }
}
