using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;

public class Player : ValidatedMonoBehaviour
{
    [Header("Player stats")]
    [SerializeField, Range(0, 1)] public float maxHealth = 1;
    [SerializeField, Range(0, 1)] public float attackPower = 0.2f;
    [SerializeField] private EnemyManager targetEnemy;

    private static Player _instance; 
    public static Player instance => _instance;
    private float currentHealth;

    private void Awake() {
        _instance = this;
        currentHealth = maxHealth;
    }

    [ContextMenu("Attack")]
    public void ContextMenu(){
        Attack(null);
    }

    public void Attack(EnemyManager enemy, BodyParts part = BodyParts.Torso){
        if(enemy == null) enemy = targetEnemy;
        targetEnemy.TakeDamage(attackPower, part);
    }

    public void ClickCase(GameObject objHit){
        Tile objTile;
        switch(objHit.tag){
            case "Encounter": 
                objTile = objHit.gameObject.GetComponentInParent<Tile>();
                if(objTile.touched) break;
                Encounter scriptEncounter = objHit.AddComponent<Encounter>();
                scriptEncounter.info = scriptEncounter.ChoseEncounter();
                objTile.touched = true;
                break;
            case "Enemy": 
                objTile = objHit.gameObject.GetComponentInParent<Tile>();
                if(objTile.touched) break;
                EnemyManager scriptEnemy = objHit.AddComponent<EnemyManager>();
                scriptEnemy.info = scriptEnemy.ChoseEnemy();
                objTile.touched = true;
                break;
            case "Boon": 
                objTile = objHit.gameObject.GetComponentInParent<Tile>();
                if(objTile.touched) break;
                Boon scriptBoon = objHit.AddComponent<Boon>();
                scriptBoon.info = scriptBoon.ChoseBoon();
                objTile.touched = true;
                break;
            case "Random": 
                objTile = objHit.gameObject.GetComponentInParent<Tile>();
                if(objTile.touched) break;
                RandomTile scriptRandom = objHit.AddComponent<RandomTile>();
                objTile.touched = true;
                break;
            default :
                Debug.Log("Nothing");
                break;
        }
    }
}
