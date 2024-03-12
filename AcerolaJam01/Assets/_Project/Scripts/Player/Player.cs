// using System;
using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using OpenCover.Framework.Model;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Linq;

public class Player : ValidatedMonoBehaviour
{
    [Header("Player stats")]
    [SerializeField, Range(0, 1)] public float maxHealth = 1;
    [SerializeField, Range(0, 1)] public float attackPower = 0.2f;
    [SerializeField, Range(1, 10)] public float attackPowerModifier = 1f;
    [SerializeField, Range(0, 1)] public float dodgeChance = 0.01f;
    [SerializeField, Range(0, 1)] public float armor = 0f;
    [SerializeField, Range(0, 1)] public float vioarrFavor = 0.5f;

    [Header("Stats")]
    [SerializeField, Anywhere] private TextMeshProUGUI armorTxt;
    [SerializeField, Anywhere] private TextMeshProUGUI maxHealthTxt;
    [SerializeField, Anywhere] private TextMeshProUGUI attackPowerTxt;
    [SerializeField, Anywhere] private TextMeshProUGUI dodgeTxt;

    [Header("HandParts")]
    [SerializeField, Anywhere] private GameObject currentHand;
    [SerializeField, Anywhere] private GameObject prefabFinger;
    [SerializeField, Anywhere] private GameObject[] handsStates;
    [SerializeField, Anywhere] public Transform[] handsCuts;
    [SerializeField, Anywhere] public Transform[] DMSockets;

    [Header("Events")]
    [SerializeField, Anywhere] private EnemyManager targetEnemy;
    [SerializeField, Anywhere] private Encounter encounter;

    [Header("Animation")]
    [SerializeField, Anywhere] private Animator[] DMAnimators;
    [SerializeField, Self] private Animator animator;


    public float currentHealth;
    public bool canChoseMap = false;
    public bool inCombat = false;
    public bool canInteract = false;

    public int levelHeight = 0;
    public Transform cutFingerT;

    private Choose lastChosen;
    private Choose partChosen;
    private BodyParts part;

    private int nbFingerTaken = 0;

    private UnityEvent _inCombatEvent = new UnityEvent();
    public UnityEvent inCombatEvent => _inCombatEvent;

    private static Player _instance; 
    public static Player instance => _instance;

    private void Awake() {
        _instance = this;
        currentHealth = maxHealth;
    }

    [ContextMenu("Start anims")]
    public void ContextMenu(){
        StartCoroutine(CoRemoveFingerAnim());
    }

    private void Start() {
        // Instantiate(prefabHand, transform);
        UpdateStats();
    }

    public void UpdateStats(){
        armorTxt.text = Mathf.RoundToInt(armor * 100) + "";
        maxHealthTxt.text = Mathf.RoundToInt(maxHealth/currentHealth * 100) + "";
        attackPowerTxt.text = Mathf.RoundToInt(attackPower * 100) + "";
        dodgeTxt.text = Mathf.RoundToInt(dodgeChance * 100) + "";
    }

    public void RemoveFinger(){
        AudioManager.instance.PlayEffect(AudioManager.instance.playerCutFinger);
        AudioManager.instance.PlayEffect(AudioManager.instance.fingerFly);
        Destroy(currentHand);
        currentHand = Instantiate(handsStates[nbFingerTaken], transform);
        cutFingerT = Instantiate(prefabFinger, handsCuts[nbFingerTaken].position, prefabFinger.transform.rotation, transform).transform;
        cutFingerT.parent = DMSockets[nbFingerTaken];
        StartCoroutine(cutFingerT.GetComponent<FingerMove>().CoroutineMoveEnd(
            cutFingerT.position, 
            cutFingerT.eulerAngles, 
            DMSockets[nbFingerTaken]
        ));
        nbFingerTaken ++;
        if(nbFingerTaken >= handsStates.Length){
          StartCoroutine(CoDies());
        }else StartCoroutine(PassEncounter());
    }

    public IEnumerator PassEncounter(){
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        currentHealth = maxHealth;
        UpdateStats();
        DM.instance.Talk(DM.instance.PFingerRemoved);
    }

    public IEnumerator CoRemoveFingerAnim(){
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        canInteract = false;
        canChoseMap = false;
        //todo make player look at DM'S hand and not be able to look
        foreach (Animator DMAnimator in DMAnimators){
            DMAnimator.SetTrigger("StartAnim" + nbFingerTaken);
        }
        animator.SetTrigger("StartAnim");
    }

    

    public void Attack(EnemyManager enemy, BodyParts part = BodyParts.Torso){
        if(enemy == null) enemy = targetEnemy;
        StartCoroutine(targetEnemy.CoTakeDamage(attackPower * attackPowerModifier, part));
        canInteract = false;
    }

    public IEnumerator CoTakeDamage(float damage){
        Debug.Log("Gotten Here");
        yield return new WaitUntil(()=>DM.instance.doneTalking);
        if(Random.Range(0, 1) > dodgeChance){
            DM.instance.Talk(DM.instance.PhitTxt);
            ChangeHealth(-damage + armor);
            AudioManager.instance.PlayEffect(AudioManager.instance.swordHit);
            yield return new WaitUntil(()=>DM.instance.doneTalking);
            canInteract = true;
        } else{
            DM.instance.Talk(DM.instance.PdodgeTxt);
            AudioManager.instance.PlayEffect(AudioManager.instance.swordMiss);
            yield return new WaitUntil(()=>DM.instance.doneTalking);
            canInteract = true;
        }
    }



    public IEnumerator CoDies(){
        // todo
        yield return new WaitUntil(()=>DM.instance.doneTalking);
    }

    public void ChangeHealth(float healthChange){
        currentHealth = Mathf.Clamp(currentHealth + healthChange, 0, maxHealth);
        UpdateStats();
        if(currentHealth <= 0) CoRemoveFingerAnim();
    }

    public void ChangeMaxHealth(float MaxhealthChange){
        maxHealth = Mathf.Max(maxHealth + MaxhealthChange, 0.01f);
        UpdateStats();
    }

    public void ChangeDodge(float dodgeChange){
        dodgeChance = Mathf.Clamp(maxHealth + dodgeChange, 0, 1);
        UpdateStats();
    }

    public void ChangeArmor(float armorChange){
        armor = armorChange;
        UpdateStats();
    }

    public void ChangeWeapon(float weaponChange){
        attackPower = weaponChange;
        UpdateStats();
    }

    public void ChangeDamage(float damageChange){
        attackPower = Mathf.Clamp(attackPowerModifier + damageChange, 0.3f, 10);
        UpdateStats();
    }

    public void ChangeFavor(float favorChange) => Player.instance.vioarrFavor = Mathf.Clamp01(Player.instance.vioarrFavor + favorChange);

    private void StartCombat(){
        targetEnemy.ChoseEnemy();
        Map.instance.ChangeStartingTile(TileTypes.Enemy);
        inCombat = true;
        targetEnemy.ChangeState(targetEnemy.info.behavoir);
        inCombatEvent.Invoke();
    }

    public void ClickedInteractable(GameObject objHit){
        if(objHit.tag == "Skip"){
            if(lastChosen != null) lastChosen.isSupended = false;
            Debug.Log("Skip!");
            DM.instance.wannaSkip = true;
        }else if(!canInteract) return;
        if(!ClickCase(objHit) && inCombat) ClickAnswer(objHit);
        else if(!inCombat) ClickAnswerOutCombat(objHit);
    }

    private bool ClickCase(GameObject objHit){
        Tile objTile = objHit.gameObject.GetComponentInParent<Tile>();
        if(objTile == null || !canChoseMap) return false;
        bool verifyHeight = objTile.height == levelHeight;
        if(!verifyHeight) return false;
        Map.instance.startPossiblePaths = objTile.GetComponent<Tile>().possiblePath;
        Map.instance.RemoveStartingPath();
        switch(objHit.tag){
            case "Encounter": 
                if(objTile.touched) break;
                encounter.isBoon = false;
                StartCoroutine(encounter.StartEncounter());
                objTile.touched = true;
                Map.instance.ChangeStartingTile(TileTypes.Encounter);
                break;
            case "Enemy": 
                if(objTile.touched) break;
                objTile.touched = true;
                StartCombat();
                break;
            case "Boon": 
                if(objTile.touched) break;
                encounter.isBoon = true;
                StartCoroutine(encounter.StartEncounter());
                Map.instance.ChangeStartingTile(TileTypes.Boon);
                objTile.touched = true;
                break;
            case "Random": 
                if(objTile.touched) break;
                switch(Random.Range(0, 3)){
                    case 0 :
                        encounter.isBoon = false;
                        StartCoroutine(encounter.StartEncounter());
                        break;
                    case 1 :
                        StartCombat();
                        break;
                    case 2 :
                        encounter.isBoon = true;
                        StartCoroutine(encounter.StartEncounter());
                        break;
                }
                Map.instance.ChangeStartingTile(TileTypes.Random);
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
        switch(objHit.tag){
            case "Yes_Attack" :
                if(lastChosen != null) lastChosen.isSupended = false;
                lastChosen = objHit.GetComponent<Choose>();
                lastChosen.AnimateFloat();
                break;
            case "No_Spare" :
                if(lastChosen != null) lastChosen.isSupended = false;
                StartCoroutine(targetEnemy.CoSpare());
                break;
            case "Head" :
                if(partChosen != null) partChosen.isSupended = false;
                partChosen = objHit.GetComponent<Choose>();
                partChosen.AnimateFloat();
                part = BodyParts.Head;
                break;
            case "Arms" :
                if(partChosen != null) partChosen.isSupended = false;
                partChosen = objHit.GetComponent<Choose>();
                partChosen.AnimateFloat();
                part = BodyParts.Arms;
                break;
            case "Legs" :
                Debug.Log("Legs");
                if(partChosen != null) partChosen.isSupended = false;
                partChosen = objHit.GetComponent<Choose>();
                partChosen.AnimateFloat();
                part = BodyParts.Legs;
                break;
            case "Body" :
                if(partChosen != null) partChosen.isSupended = false;
                partChosen = objHit.GetComponent<Choose>();
                partChosen.AnimateFloat();
                part = BodyParts.Torso;
                break;
            default :
                if(lastChosen != null) lastChosen.isSupended = false;
                if(partChosen != null) partChosen.isSupended = false;
                break;
        }
        if(lastChosen == null || partChosen == null) return; 
        if(lastChosen.isSupended && partChosen.isSupended){
           Attack(null, part);
           lastChosen.isSupended = false;
           partChosen.isSupended = false;
        } 
    }

    private void ClickAnswerOutCombat(GameObject objHit){
        Debug.Log("Worked out of combat");
        switch (objHit.tag){
            case "No_Spare": 
                if(encounter.isBoon) StartCoroutine(encounter.CoPlayerChoseBoon(false));
                else StartCoroutine(encounter.CoPlayerTakeEncounter(false));
                break;
            case "Yes_Attack": 
                if(encounter.isBoon) StartCoroutine(encounter.CoPlayerChoseBoon(true));
                else StartCoroutine(encounter.CoPlayerTakeEncounter(true));
                break;
            default:
                break;
        }
    }

    public void FinishEncounter(){
        canChoseMap = true;
        canInteract = true;
        inCombat = false;
        StartCoroutine(Map.instance.CoMakeStartingPath());
    }
}
