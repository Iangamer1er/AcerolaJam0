using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;
using TMPro;
using System.Threading;
using Utilities;

public class DM : ValidatedMonoBehaviour
{
    [Header("Talk infos")]
    [SerializeField, Anywhere] private TextMeshProUGUI dialogue;
    [SerializeField, Min(0)] private float timeBetweenLetters = 0.1f;
    [SerializeField, Min(0)] private float timeBetweenSentences = 3f;
    [SerializeField, TextArea] private List<string> skipTutoTxt;

    [Header("Tuto txt")]
    [SerializeField, TextArea] private List<string> introTxt0;
    [SerializeField, TextArea] private List<string> introTxt1;
    [SerializeField, TextArea] private List<string> introTxt2;
    [SerializeField, TextArea] private List<string> makingMapTxt;
    [SerializeField, TextArea] private List<string> firstCombatTxt;
    

    [Header("Player txt")]
    [SerializeField, TextArea] public List<string> PdodgeTxt;
    [SerializeField, TextArea] public List<string> PAttackTxt;
    [SerializeField, TextArea] public List<string> PhitTxt;
    [SerializeField, TextArea] public List<string> PFingerRemoved;
    [SerializeField, TextArea] public List<string> PDeadTxt;

    [Header("Enemy txt")]
    [SerializeField, TextArea] public List<string> EdodgeTxt;
    [SerializeField, TextArea] public List<string> EdodgeHeadTxt;
    [SerializeField, TextArea] public List<string> EDeadTxt;
    [SerializeField, TextArea] public List<string> ESparedTxt;
    [SerializeField, TextArea] public List<string> ESpareFailTxt;
    [SerializeField, TextArea] public List<string> ELegsBrokenTxt;
    [SerializeField, TextArea] public List<string> EArmsBrokenTxt;
    [SerializeField, TextArea] public List<string> EventSkipTxt;

    [Header("Boss txt")]
    [SerializeField, TextArea] public List<string> BFluryAttacks;
    [SerializeField, TextArea] public List<string> BdodgeHeadTxt;
    [SerializeField, TextArea] public List<string> BDeadTxt;
    [SerializeField, TextArea] public List<string> BSparedTxt;
    [SerializeField, TextArea] public List<string> BRemovesFingerTxt;

    [Header("Sounds")]
    [SerializeField] private bool usePitch = false;
    [SerializeField] private List<AudioClip> clipsTalk;

    public bool wannaSkip = false;
    public bool doneTalking = false;

    private bool goNext = false;
    private string currentTxt = "";
    private CountdownTimer timerSentences; 
    private bool timerDone = false;
    
    private static DM _instance; 
    public static DM instance => _instance;


    private void Awake() {
        _instance = this;
    }

    private void Start() {
        timerSentences = new CountdownTimer(timeBetweenSentences);
        timerSentences.OnTimerStop = ()=> timerDone = true;
        if(GameManager.instance.didTuto) StartCoroutine(SkipTuto());
        else StartCoroutine(Tuto());
    }

    private IEnumerator SkipTuto(){
        yield return new WaitForSeconds(1);
        StartCoroutine(Talk(skipTutoTxt));
        Map.instance.StartGame();
        yield return new WaitUntil(()=>doneTalking);
        Player.instance.canChoseMap = true;
        Player.instance.canInteract = true;
    }

    private IEnumerator Tuto(){
        yield return new WaitForSeconds(1);
        StartCoroutine(Talk(introTxt0));
        yield return new WaitUntil(()=>doneTalking);
        //todo animation of giving skip, and answers
        StartCoroutine(Talk(introTxt1));
        yield return new WaitUntil(()=>doneTalking);
        Player.instance.inCombatEvent.Invoke();
        StartCoroutine(Talk(introTxt2));
        yield return new WaitUntil(()=>doneTalking);
        //todo make the tiles flip and the camera pan to it
        Player.instance.inCombatEvent.Invoke();
        yield return new WaitForSeconds(1);
        //todo make camera accessible to the player
        StartCoroutine(Talk(makingMapTxt));
        Map.instance.StartGame();
        yield return new WaitUntil(()=>doneTalking);
        StartCoroutine(Talk(firstCombatTxt));
        yield return new WaitUntil(()=>doneTalking);
        //todo make the player able to interract with the map
        Player.instance.canChoseMap = true;
        Player.instance.canInteract = true;
        yield return null;
    }

    private void Update() {
        timerSentences.Tick(Time.deltaTime);
    }

    public IEnumerator Talk(InfoDialogue info){
        doneTalking = false;
        Player.instance.canInteract = false;
        foreach (MyListDialogues listDialogue in info.Act){
            foreach (string text in listDialogue.dialogue){
                StartCoroutine(WriteText(text));
                yield return new WaitUntil(()=>goNext);
                goNext = false;
            }
        }
        doneTalking = true;
        dialogue.text = "";
    }
    
    public IEnumerator Talk(List<string> list){
        doneTalking = false;
        Player.instance.canInteract = false;
        foreach (string text in list){
            StartCoroutine(WriteText(text));
            yield return new WaitUntil(()=>goNext);
            goNext = false;
        }
        doneTalking = true;
        dialogue.text = "";
    }

    private IEnumerator WriteText(string text){
        currentTxt = "";
        wannaSkip = false;
        for (int i = 0; i < text.Length; i++){
            currentTxt += text[i];
            dialogue.text = currentTxt;
            if(!wannaSkip){
               if(text[i] != ' ') AudioManager.instance.PlayEffect(ChoseSound(), usePitch);  
               yield return new WaitForSeconds(timeBetweenLetters); 
            }
        }
        timerSentences.Start();
        wannaSkip = false;
        yield return new WaitUntil(()=>timerDone || wannaSkip);
        timerSentences.Stop();
        timerDone = false;
        goNext = true;
    }

    private AudioClip ChoseSound(){
        int randomInt = Random.Range(0, clipsTalk.Count);
        return clipsTalk[randomInt];
    }
}   
