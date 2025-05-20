using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    private Texture2D screenCapture;
    private bool viewingPhoto;

    [Header("CameraCanvas Objects")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;
    [SerializeField] private GameObject cameraFlash;
    [SerializeField] private float flashTime;

    [Header("Photo Camera Setup")]
    [SerializeField] private Camera photoCamera;
    [SerializeField] private RenderTexture photoRenderTexture;

    [Header("Shutter Sound Clip")]
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private float volume = 0.1f;

    private bool isActivated = false;

    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    public void StartProcess()
    {
        //Debug.Log("are you here");
        isActivated = true;
        if (!viewingPhoto)
        {
            StartCoroutine(HandleFishingSequence());
            //StartCoroutine(CapturePhoto());
        }
    }

    private IEnumerator HandleFishingSequence()
    {
        if (isActivated)
        {
            viewingPhoto = true; // Prevent retriggering

            FishingGame minigame = GameObject.Find("FishingGame").GetComponent<FishingGame>();
            //minigame.gameObject.SetActive(true);

            // Wait for the minigame to finish
            yield return StartCoroutine(minigame.PlayFishingGame());

            //minigame.gameObject.SetActive(false);

            // Now take the photo
            yield return StartCoroutine(CapturePhoto());
        }

    }


    // only capture the screen after everything else is rendered
    IEnumerator CapturePhoto()
    {
        if (isActivated)
        {
            viewingPhoto = true;

            yield return new WaitForEndOfFrame();

            photoCamera.Render();

            screenCapture = new Texture2D(photoRenderTexture.width, photoRenderTexture.height, TextureFormat.RGB24, false);
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = photoRenderTexture;

            screenCapture.ReadPixels(new Rect(0, 0, photoRenderTexture.width, photoRenderTexture.height), 0, 0);
            screenCapture.Apply();

            RenderTexture.active = currentRT;

            // create sprite out of texture:
            ShowPhoto();
        }

        yield return null;
    }

    void ShowPhoto()
    {
        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);

        photoDisplayArea.sprite = photoSprite;

        StartCoroutine(CameraFlashEffect());

        photoFrame.SetActive(true);

    }


    // IEnumerator to create a coroutine
    IEnumerator CameraFlashEffect()
    {
        if (isActivated)
        {
            // play audio
            Transform position = photoCamera.GetComponent<Transform>();
            AudioManager.instance.PlayClip(audioClip, position, volume);


            cameraFlash.SetActive(true);

            // Fade out the image to transparent
            float timeElapsed = 0f;
            while (timeElapsed < flashTime)
            {
                cameraFlash.GetComponent<Image>().color = Color.Lerp(cameraFlash.GetComponent<Image>().color, new Color(1f, 1f, 1f, 0f), timeElapsed / flashTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            cameraFlash.SetActive(false);
            cameraFlash.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void Abort()
    {
        isActivated = false;

        viewingPhoto = false;
        photoFrame.SetActive(false);
        // Destroy minigame...

        // cameraUI true
    }
}
