using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// i followed a tuto for this: https://www.youtube.com/watch?v=DU7cgVsU2rM

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource obj;

    [SerializeField] private AudioSource musicSource;


    [SerializeField] private AudioClip musicLvl1;
    [SerializeField] private AudioClip musicLvl2;
    [SerializeField] private AudioClip musicLvl3;


    private bool lvl1Played = false;
    private bool lvl2Played = false;


    private float volume = 0.1f;


    private static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public static AudioManager GetInstance() { return instance; }


    public void PlayClip(AudioClip audioClip, Transform spawnTransform, float volume, float startingTime = 0f) {
        //spawn a new gameobject
        AudioSource audioSource = Instantiate(obj, spawnTransform.position, Quaternion.identity);

        // assign the audioclip
        audioSource.clip = audioClip;

        // assign volume
        audioSource.volume = volume;

        // if startingTime specified start at that time:
        if (startingTime != 0f) {
            audioSource.time = startingTime;
        }

        // play sound
        audioSource.Play();

        // get length of clip
        float clipLength = audioSource.clip.length;

        // destroy clip after it's done playing
        Destroy(audioSource.gameObject, clipLength);
    }


    public void StartLevelMusic() {

        if (!lvl1Played)
        {
            StartMusic(musicLvl1, true);
            lvl1Played = true;

        }
        else if (!lvl2Played)
        {
            StartMusic(musicLvl2, true);
            lvl2Played = true;
        }
        else if (lvl1Played && lvl2Played) {

            StartMusic(musicLvl3, true);

        }
    
    
    }


    public void StartMusic(AudioClip audio, bool playOnLoop = false) {

        musicSource.clip = audio;

        musicSource.loop = playOnLoop;

        musicSource.volume = volume;

        musicSource.Play();


    }

    public void StopMusic() {
        musicSource.Stop();

    }


    public float GetMusicVolume() {

        return volume;
    }

    public void SetMusicVolume(float vol) {

        volume = vol;
        GameLaunchOptions.MusicVolume = vol;

        if (musicSource.isPlaying) {

            
            musicSource.volume = volume;

        }
    }
}
