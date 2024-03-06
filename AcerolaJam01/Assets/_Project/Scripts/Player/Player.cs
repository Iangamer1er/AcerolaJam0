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


    private float currentHealth;

    private void Awake() {
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
}
