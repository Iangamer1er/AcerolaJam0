using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;
using TMPro;
using System.Threading;
using Utilities;

public class DM : ValidatedMonoBehaviour
{
    [SerializeField] private InfoDialogue introDialogue;
    [SerializeField, Anywhere] private TextMeshProUGUI dialogue;
    [SerializeField, Min(0)] private float timeBetweenLetters = 0.1f;
    [SerializeField, Min(0)] private float timeBetweenSentences = 3f;
    [SerializeField] private InfoDialogue textStart;

    public bool wannaSkip = true;

    private bool goNext = false;
    private string currentTxt = "";
    private CountdownTimer timerSentences; 
    
    private static DM _instance; 
    public static DM instance => _instance;


    private void Awake() {
        _instance = this;
    }

    private void Start() {
        timerSentences = new CountdownTimer(timeBetweenSentences);
        timerSentences.OnTimerStop = ()=> wannaSkip = true;
        Talk(textStart);
    }

    private void Update() {
        timerSentences.Tick(Time.deltaTime);
    }

    private IEnumerator Talk(InfoDialogue info){
        foreach (MyListDialogues listDialogue in info.Act){
            foreach (string text in listDialogue.dialogue){
                StartCoroutine(WriteText(text));
                yield return new WaitUntil(()=>goNext);
            }
        }
    }

    public IEnumerator WriteText(string text){
        currentTxt = "";
        for (int i = 0; i < text.Length; i++){
            currentTxt += text[i];
            dialogue.text = currentTxt;
            yield return new WaitForSeconds(wannaSkip ? 0 : timeBetweenLetters);
        }
        wannaSkip = false;
        timerSentences.Start();
        yield return new WaitUntil(()=>wannaSkip);
        goNext = true;
    }
}
