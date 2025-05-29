using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// i followed a tuto for this: https://www.youtube.com/watch?v=DU7cgVsU2rM

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource obj;

    [SerializeField] private AudioSource musicSource;


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


    public void PlayClip(AudioClip audioClip, Transform spawnTransform, float volume) {
        //spawn a new gameobject
        AudioSource audioSource = Instantiate(obj, spawnTransform.position, Quaternion.identity);

        // assign the audioclip
        audioSource.clip = audioClip;

        // assign volume
        audioSource.volume = volume;

        // play sound
        audioSource.Play();

        // get length of clip
        float clipLength = audioSource.clip.length;

        // destroy clip after it's done playing
        Destroy(audioSource.gameObject, clipLength);
    }


    public void StartMusic(AudioClip audio, float volume) {

        musicSource.clip = audio;

        musicSource.loop = true;

        musicSource.volume = volume;

        musicSource.Play();


    }

    public void StopMusic() {
        musicSource.Stop();

    }
}
