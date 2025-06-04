using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip music;

    [SerializeField] private AudioSource musicSource;
    private float volume = 0.5f;


    // Start is called before the first frame update
    void Start()
    {

        StartMusic(music, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartMusic(AudioClip audio, bool playOnLoop = false)
    {

        musicSource.clip = audio;

        musicSource.loop = playOnLoop;

        musicSource.volume = volume;

        musicSource.Play();


    }

    public void StopMusic()
    {
        musicSource.Stop();

    }


    public float GetMusicVolume()
    {

        return volume;
    }

    public void SetMusicVolume(float vol)
    {

        volume = vol;

        if (musicSource.isPlaying)
        {


            musicSource.volume = volume;

        }
    }
}
