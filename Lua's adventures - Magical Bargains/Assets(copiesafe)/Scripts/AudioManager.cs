using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// i followed a tuto for fun: https://www.youtube.com/watch?v=DU7cgVsU2rM

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource obj;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) {
            instance = this;
        }   
    }


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
}
