using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEvent{
   public abstract void EventAction();
}

public class ChangeCurrentHealth : BaseEvent{
    [SerializeField, Range(-1, 1)] public float healthChange;
    public override void EventAction(){
        Player.instance.ChangeHealth(healthChange);
    }
}

public class GainMaxHealth : BaseEvent{
    [SerializeField, Range(-1, 1)] public float maxHealthChange;
    public override void EventAction(){
        Player.instance.ChangeMaxHealth(maxHealthChange);
    }
}

public class ChangeArmor : BaseEvent{
    [SerializeField, Range(0, 1)] public float armorValue;
    public override void EventAction(){
        Player.instance.armor = armorValue;
    }
}

public class ChangeDodge : BaseEvent{
    [SerializeField, Range(-1, 1)] public float dodgeChance;
    public override void EventAction(){
        Player.instance.ChangeDodge(dodgeChance);
    }
}

public class ChangeAttackModif : BaseEvent{
    [SerializeField, Range(-1, 1)] public float attackModifChange;
    public override void EventAction(){
        Player.instance.ChangeDamage(attackModifChange);
    }
}

public class ChangeVioarrFavor : BaseEvent{
    [SerializeField, Range(-1, 1)] public float vioarrFavorChange;
    public override void EventAction(){
        Player.instance.ChangeFavor(vioarrFavorChange);
    }
}

public class ChangeWeapon : BaseEvent{
    [SerializeField, Range(0.1f, 1)] public float weaponChange;
    public override void EventAction(){
        Player.instance.ChangeWeapon(weaponChange);
    }
}

public class Talk : BaseEvent{
    [SerializeField] public List<string> dialogue;
    public override void EventAction(){
        DM.instance.Talk(dialogue);
    }
}