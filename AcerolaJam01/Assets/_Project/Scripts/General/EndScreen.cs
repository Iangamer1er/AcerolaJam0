using System;
using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;
using TMPro;
using Utilities;
using UnityEditor;

public class EndScreen : ValidatedMonoBehaviour
{
    [SerializeField, Self] AudioSource aS;
    [SerializeField, TextArea] List<String> endCredits;
    [SerializeField, Anywhere] private TextMeshProUGUI dialogue;
    [SerializeField, Anywhere] private Canvas UITile;
    [SerializeField, Min(0)] private float timeBetweenLetters = 0.1f;
    [SerializeField, Min(0)] private float timeBetweenSentences = 3f;
    [SerializeField, Min(0)] private float timeBeforeEndScreen = 3f;
    [SerializeField] string casesText = "You made it through ";
    [SerializeField] string options = "Press escape to quit or left mouse button to continue";

    private bool goNext = false;
    private bool canClick = false;
    private bool timerDone = false;
    private string currentTxt = "";
    private CountdownTimer timerSentences; 

    private void Start() {
        AudioManager.instance.currentMusic.volume = 0;
        timerSentences = new CountdownTimer(timeBetweenSentences);
        timerSentences.OnTimerStop = ()=> timerDone = true;
        StartCoroutine(CoEndScreen(endCredits));
    }

    private void Update(){
        timerSentences.Tick(Time.deltaTime); 
        if(canClick && Input.GetMouseButtonDown(0)){
                
        }
    } 

    public IEnumerator CoEndScreen(List<string> list){

        yield return new WaitForSeconds(timeBeforeEndScreen);
        StartCoroutine(AudioManager.instance.CoTransitionInMusic(AudioManager.instance.currentMusic));
        UITile.gameObject.SetActive(false);
        foreach (string text in list){
            StartCoroutine(WriteText(text));
            yield return new WaitUntil(()=>goNext);
            goNext = false;
        }
        StartCoroutine(WriteText(casesText + GameManager.instance.level + " events"));
        yield return new WaitUntil(()=>goNext);
        StartCoroutine(WriteText(options));
        canClick = true;
    }

    private IEnumerator WriteText(string text){
        currentTxt = "";
        for (int i = 0; i < text.Length; i++){
            currentTxt += text[i];
            dialogue.text = currentTxt;  
            yield return new WaitForSeconds(timeBetweenLetters); 
        }
        timerSentences.Start();
        yield return new WaitUntil(()=>timerDone);
        timerSentences.Stop();
        timerDone = false;
        goNext = true;
    }


}
