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
    [Header("Dialogues")]
    [SerializeField, TextArea] private List<string> introDialogue;
    [SerializeField, TextArea] public List<string> EdodgeTxt;
    [SerializeField, TextArea] public List<string> PdodgeTxt;
    [SerializeField, TextArea] public List<string> EDeadTxt;
    [SerializeField, TextArea] public List<string> ESparedTxt;
    [SerializeField, TextArea] public List<string> ESpareFailTxt;
    [SerializeField, TextArea] public List<string> ELegsBrokenTxt;
    [SerializeField, TextArea] public List<string> EArmsBrokenTxt;
    [SerializeField, TextArea] public List<string> EventSkipTxt;

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
        StartCoroutine(Talk(introDialogue));
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
            yield return new WaitForSeconds(wannaSkip ? 0 : timeBetweenLetters);
        }
        timerSentences.Start();
        wannaSkip = false;
        yield return new WaitUntil(()=>timerDone || wannaSkip);
        timerSentences.Stop();
        timerDone = false;
        goNext = true;
    }
}
