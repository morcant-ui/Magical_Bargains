using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.IO;

public class GameIntro : MonoBehaviour
{
    [Header("images and text")]
    [SerializeField] private TextAsset introDataJSON;

    [Header("Intro Graphics")]
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject backgroundColor;
    [SerializeField] private GameObject cutsceneImage;
    [SerializeField] private TextMeshProUGUI cutsceneText;

    // for running through the story
    private TextAsset textAsset;
    private Story currentStory;
    private bool isTextPlaying = false;

    // for accessing the images and text of the cutscene
    private Queue<CutsceneImageData> imagesQueue;
    private string currentImageName;

    // where in RESOURCES to find the images and text
    private string introPath = "Cutscene";

    // input to go to next line of text and image
    private string inputKey = "space";

    // keep track of the state of cutscene: mostly to avoid spacebar hijinks with other dialogue managers
    private bool introActivated = false;

    // to exit we store the coroutine in this var to make sure it happens only once
    private Coroutine exitOnlyOnce;


    void Update()
    {
        if (!isTextPlaying)
        {
            return;
        }

        if (introActivated && isTextPlaying && Input.GetKeyDown(inputKey))
        {
            // after first image and text, everytime space is pressed and cutscene is not finished,
            // we both show the next line of text and show the next image
            ContinueStory();
            NextImage();
        }
    }



    public void ShowIntroCutscene() {

        // Called from Game State manager:
        introActivated = true;

        // notify dialogue manager that a cutscene is currently going on and it should listen for spacebar input
        DialogueManager.GetInstance().CutsceneStarted();

        // display the cutscene graphics
        holder.SetActive(true);

        // pull JSON data for intro cutscene
        CutsceneData cutsceneData = JsonUtility.FromJson<CutsceneData>(introDataJSON.text);

        // get the text asset
        string textName = cutsceneData.textName;
        textAsset = Resources.Load<TextAsset>(System.IO.Path.Combine(introPath, textName));

        // make a queue of the images and pop the first one
        imagesQueue = new Queue<CutsceneImageData>(cutsceneData.imageNames);

        // show the first image (text will soon follow)
        NextImage();

        // set the UI image active and enter dialogue mode
        cutsceneImage.SetActive(true);

        // plug the story object into text mode
        currentStory = new Story(textAsset.text);
        EnterTextMode(textAsset);
        
    }

    private void EnterTextMode(TextAsset textAsset) {

        currentStory = new Story(textAsset.text);
        isTextPlaying = true;

        ContinueStory();
    }

    private void ContinueStory() {
        if (currentStory.canContinue)
        {
            string nextLine = currentStory.Continue();
            cutsceneText.text = nextLine;
        }
        else
        {
            // when text is finished we consider the cutscene finished:
            // we make sure the coroutine runs only once
            if (exitOnlyOnce != null) { return; }

            exitOnlyOnce = StartCoroutine(ExitTextMode());
        }
    }

    private void NextImage()
    {
        if (imagesQueue.Count != 0)
        {
            currentImageName = imagesQueue.Dequeue().imageName;
            cutsceneImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(System.IO.Path.Combine(introPath, currentImageName));
        }
    }

    private IEnumerator ExitTextMode()
    {
        yield return new WaitForSeconds(0.2f);

        // update the booleans
        isTextPlaying = false;
        introActivated = false;

        // notify dialogue manager that we're done
        DialogueManager.GetInstance().CutsceneStopped();

        // notify game manager next
        GameStateManager.GetInstance().LoadLevelIntro();
        
        // hide the cutscene graphics and reset text content
        cutsceneText.text = "";
        cutsceneImage.SetActive(false);
        holder.SetActive(false);

        
    }
}
