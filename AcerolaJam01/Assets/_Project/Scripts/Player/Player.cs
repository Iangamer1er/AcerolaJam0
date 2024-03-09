using System;
using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using OpenCover.Framework.Model;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class Player : ValidatedMonoBehaviour
{
    [Header("Player stats")]
    [SerializeField, Range(0, 1)] public float maxHealth = 1;
    [SerializeField, Range(0, 1)] public float attackPower = 0.2f;
    [SerializeField, Range(0, 1)] public float dodgeChance = 0.01f;
    [SerializeField, Range(0, 1)] public float armor = 0f;
    [SerializeField] private EnemyManager targetEnemy;

    public float currentHealth;
    public bool canChoseMap = false;

    private int levelHeight = 0;
    private Choose lastChosen;

    private UnityEvent _inCombatEvent = new UnityEvent();
    public UnityEvent inCombatEvent => _inCombatEvent;

    private static Player _instance; 
    public static Player instance => _instance;

    private void Awake() {
        _instance = this;
        currentHealth = maxHealth;
    }

    [ContextMenu("Flip")]
    public void ContextMenu(){
        inCombatEvent.Invoke();
    }


    public void Attack(EnemyManager enemy, BodyParts part = BodyParts.Torso){
        if(enemy == null) enemy = targetEnemy;
        targetEnemy.TakeDamage(attackPower, part);
    }

    public void ChangeHealth(float healthChange){
        currentHealth = Mathf.Clamp(currentHealth + healthChange, 0, maxHealth);
    }

    public void ChangeMaxHealth(float MaxhealthChange){
        maxHealth = Mathf.Max(maxHealth + MaxhealthChange, 0.01f);
    }

    public void ChangeDodge(float dodgeChange){
        dodgeChance = Mathf.Clamp(maxHealth + dodgeChange, 0, 1);
    }

    public void ClickedInteractable(GameObject objHit){
        if(!ClickCase(objHit)) ClickAnswer(objHit);
    }

    private bool ClickCase(GameObject objHit){
        if(lastChosen != null) lastChosen.isSupended = false;
        Tile objTile = objHit.gameObject.GetComponentInParent<Tile>();
        if(objTile == null || !canChoseMap) return false;
        bool verifyHeight = objTile.height == levelHeight;
        if(!verifyHeight) return false;
        switch(objHit.tag){
            case "Encounter": 
                if(objTile.touched) break;
                Encounter scriptEncounter = objHit.AddComponent<Encounter>();
                scriptEncounter.info = scriptEncounter.ChoseEncounter();
                objTile.touched = true;
                break;
            case "Enemy": 
                if(objTile.touched) break;
                EnemyManager scriptEnemy = objHit.AddComponent<EnemyManager>();
                scriptEnemy.info = scriptEnemy.ChoseEnemy();
                objTile.touched = true;
                break;
            case "Boon": 
                if(objTile.touched) break;
                Boon scriptBoon = objHit.AddComponent<Boon>();
                scriptBoon.info = scriptBoon.ChoseBoon();
                objTile.touched = true;
                break;
            case "Random": 
                if(objTile.touched) break;
                RandomTile scriptRandom = objHit.AddComponent<RandomTile>();
                objTile.touched = true;
                break;
            default :
                Debug.Log("Something went wrong");
                return false;
        }
        canChoseMap = false;
        return true;
    }

    private void ClickAnswer(GameObject objHit){
        if(lastChosen != null) lastChosen.isSupended = false;
        switch(objHit.tag){
            case "Skip" :
                Debug.Log("Skip");
                break;
            case "Yes_Attack" :
                lastChosen = objHit.GetComponent<Choose>();
                lastChosen.AnimateFloat();
                Debug.Log("Attack");
                break;
            case "No_Spare" :
                Debug.Log("Spare");
                break;
            default :
                Debug.Log("Nothing");
                break;
        }
    }

    public void FinishEncounter(){
        levelHeight++;
        StartCoroutine(Map.instance.CoAdvanceOneTile());
    }
}
