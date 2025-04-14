using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraUiTest : MonoBehaviour
{
    private Texture2D screenCapture;
    private bool viewingPhoto;
    private bool activated = false;

    public GameObject highlight;
    public Image photoDisplayArea;
    public GameObject photoFrame;
    public GameObject cameraFlash;
    public float flashTime;

    // Start is called before the first frame update
    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        highlight.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (activated)
        {
            highlight.GetComponent<SpriteRenderer>().enabled = true;

            if (!viewingPhoto)
            {
                StartCoroutine(HandleFishingSequence());
                //StartCoroutine(CapturePhoto());
            }

        }
    }

    private IEnumerator HandleFishingSequence()
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

    void OnMouseUp()
    {
        if (activated == true)
        {
            activated = false;
            highlight.GetComponent<SpriteRenderer>().enabled = false;
            removePhoto();
        }
        else
        {
            activated = true;
        }
    }

    // only capture the screen after everything else is rendered
    IEnumerator CapturePhoto() {

        // cameraUI false
        viewingPhoto = true;

        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();

        ShowPhoto();
    }
    
    void ShowPhoto() {
        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);

        photoDisplayArea.sprite = photoSprite;

        StartCoroutine(CameraFlashEffect());

        photoFrame.SetActive(true);

    }


    // IEnumerator to create a coroutine
    IEnumerator CameraFlashEffect() {
        // play audio
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

    void removePhoto() {
        viewingPhoto = false;
        photoFrame.SetActive(false);

        // cameraUI true
    }
}
