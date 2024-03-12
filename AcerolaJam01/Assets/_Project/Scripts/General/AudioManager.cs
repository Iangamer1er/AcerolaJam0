using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;

public class AudioManager : ValidatedMonoBehaviour
{
    [SerializeField, Range(0.5f,1.5f)] private float pitchMin = 1;
    [SerializeField, Range(0.5f,1.5f)] private float pitchMax = 1;
    private AudioSource _aS; 
    private AudioSource _aSLeft; 
    private AudioSource _aSRight; 
    public AudioSource _aSMusic; 
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
            if(source.gameObject.name == "Left") _aSLeft = source;
            else if(source.gameObject.name == "Right") _aSRight = source;
            else if(source.gameObject.name == "Music") _aSMusic = source;
            else if(source.gameObject.name == "SoundEffects") _aSEffects = source;
        }
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
    public void ChangeVolumeMusic(float newVolume){
        _aSMusic.volume = newVolume;
    }

    /// <summary>
    /// Returns a random pitch between the max and min pitch
    /// </summary>
    /// <returns>Random pitch</returns>
    private float PitchSelect(){
        return Random.Range(pitchMin, pitchMax);
    }
}