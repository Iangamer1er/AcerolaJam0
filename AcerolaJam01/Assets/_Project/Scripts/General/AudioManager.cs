using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using KBCore.Refs;
using UnityEngine;

public enum MusicSources{CombatMusic, AmbianceMusic, BoonsMusic, EventsMusic}

public class AudioManager : ValidatedMonoBehaviour
{
    [SerializeField, Range(0.5f,1.5f)] private float pitchMin = 1;
    [SerializeField, Range(0.5f,1.5f)] private float pitchMax = 1;
    [SerializeField, Range(0f,1f)] private float maxVolume = 0.5f;
    [SerializeField, Range(0f, 5f)] private float transitionTime = 1f;
    public AudioClip combatMusic;
    public AudioClip eventMusic;
    public AudioClip boonsMusic;
    public AudioClip ambianceMusic;
    public AudioClip swordHit;
    public AudioClip swordMiss;
    public AudioClip startCombat;

    private AudioSource currentMusic;
    private AudioSource aSCombatMusic;
    private AudioSource aSEventMusic;
    private AudioSource aSBoonsMusic;
    private AudioSource aSAmbianceMusic;
    private AudioSource _aS; 
    private AudioSource _aSLeft; 
    private AudioSource _aSRight; 
    private AudioSource _aSEffects; 


    private static AudioManager _instance;
    public static AudioManager instance => _instance; 

    private void Awake() {
        if(_instance == null) _instance = this; 
        else{
           Destroy(gameObject);
           return;
        }
        AudioSource[] sources = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource source in sources){
            switch (source.gameObject.name){
                case "Left":
                    _aSLeft = source;
                    break;
                case "Right":
                    _aSRight = source;
                    break;
                case "SoundEffects":
                    _aSEffects = source;
                    break;
                case "CombatMusic":
                    aSCombatMusic = source;
                    aSCombatMusic.clip = combatMusic;
                    aSCombatMusic.Play();
                    break;
                case "AmbianceMusic":
                    aSAmbianceMusic = source;
                    aSAmbianceMusic.volume = maxVolume;
                    aSAmbianceMusic.clip = ambianceMusic;
                    aSAmbianceMusic.Play();
                    break;
                case "BoonsMusic":
                    aSBoonsMusic = source;
                    aSBoonsMusic.clip = boonsMusic;
                    aSBoonsMusic.Play();
                    break;
                case "EventsMusic":
                    aSEventMusic = source;
                    aSEventMusic.clip = eventMusic;
                    aSEventMusic.Play();
                    break;
                default:
                    break;
            }
        }
        currentMusic = aSAmbianceMusic;
        DontDestroyOnLoad(gameObject);
    }

        /// <summary>
    /// Function to play sounds with option to play with pitch on the effects audioclip Component
    /// </summary>
    /// <param name="leSon">Audioclip to play</param>
    public void PlayEffect(AudioClip leSon, bool usePitch = false){
        _aSEffects.pitch = 1;
        if(usePitch) _aSEffects.pitch = PitchSelect();
        _aSEffects.PlayOneShot(leSon); // jouer le son
    }

    /// <summary>
    /// Function to play sounds with specific pitch on the effects audioclip Component
    /// </summary>
    /// <param name="leSon">Audioclip to play</param>
    /// <param name="pitch"></param>
    public void PlayEffect(AudioClip leSon, float pitch){
        _aSEffects.pitch = pitch;
        _aSEffects.PlayOneShot(leSon); // jouer le son
    }

    /// <summary>
    /// Function to play sounds with option to play with pitch on the left channel audioclip Component
    /// </summary>
    /// <param name="leSon">Audioclip to play</param>
    public void PlayLeft(AudioClip leSon, bool usePitch = false){
        _aSLeft.pitch = 1;
        if(usePitch) _aSLeft.pitch = PitchSelect();
        _aSLeft.PlayOneShot(leSon); // jouer le son
    }

    /// <summary>
    /// Function to play sounds with option to play with pitch on the right channel audioclip Component
    /// </summary>
    /// <param name="leSon">Audioclip to play</param>
    public void PlayRight(AudioClip leSon, bool usePitch = false){
        _aSRight.pitch = 1;
        if(usePitch) _aSRight.pitch = PitchSelect();
        _aSRight.PlayOneShot(leSon); // jouer le son
    }

    /// <summary>
    /// Function to play sounds with option to play with pitch on the effects audioclip Component
    /// </summary>
    /// <param name="leSon">Audioclip to play</param>
    public void JouerSon(AudioClip leSon, bool usePitch = false){
        _aS.pitch = 1;
        if(usePitch) _aS.pitch = PitchSelect();
        _aS.PlayOneShot(leSon); // jouer le son
    }

    /// <summary>
    /// Function to change the music volume
    /// </summary>
    /// <param name="newVolume"></param>
    public void ChangeVolumeMusic(float newVolume, AudioSource music){
        music.volume = Mathf.Min(newVolume, maxVolume);
    }

    [ContextMenu("ChangeMusic to fighting")]
    public void ContextMenu(){
        TransitionMusic(MusicSources.CombatMusic);
    }

    public void TransitionMusic(MusicSources sourceName){
        AudioSource toSource = currentMusic;
        switch(sourceName){
            case MusicSources.CombatMusic:
                toSource = aSCombatMusic;
                break;
            case MusicSources.AmbianceMusic:
                toSource = aSAmbianceMusic;
                break;
            case MusicSources.BoonsMusic:
                toSource = aSBoonsMusic;
                break;
            case MusicSources.EventsMusic:
                toSource = aSEventMusic;
                break;
        }
        if(toSource == currentMusic) return;
        StartCoroutine(CoTransitionMusic(currentMusic, toSource));
    }

    private IEnumerator CoTransitionMusic(AudioSource fromAs, AudioSource toAs){
        float actionTime = 0;
        while (actionTime < transitionTime){
            actionTime += Time.fixedDeltaTime;
            float t = Mathf.SmoothStep(0, maxVolume, actionTime/transitionTime);
            toAs.volume = Mathf.Lerp(0, maxVolume, t);
            fromAs.volume = Mathf.Lerp(maxVolume, 0, t);
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Returns a random pitch between the max and min pitch
    /// </summary>
    /// <returns>Random pitch</returns>
    private float PitchSelect(){
        return Random.Range(pitchMin, pitchMax);
    }
}
