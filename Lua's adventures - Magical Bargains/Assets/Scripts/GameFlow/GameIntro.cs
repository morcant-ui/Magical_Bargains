using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.IO;

public class GameIntro : MonoBehaviour
{

    [SerializeField] private TextAsset introDataJSON;

    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject backgroundColor;
    [SerializeField] private GameObject cutsceneImage;
    [SerializeField] private TextMeshProUGUI cutsceneText;

    private TextAsset textAsset;
    private Story currentStory;
    private bool isTextPlaying = false;

    private Queue<CutsceneImageData> imagesQueue; // this queue will let us iterate thru game data
    private string currentImageName; // index to keep track of current client/artifact
    private string introPath = "Cutscene";

    private string inputKey = "space";

    public bool introActivated = false;

    private Coroutine exitOnlyOnce;


    void Update()
    {
        if (!isTextPlaying)
        {
            return;
        }

        if (introActivated && isTextPlaying && Input.GetKeyDown(inputKey))
        {
            ContinueStory();
            NextImage();
        }
    }



    public void ShowIntroCutscene() {

        introActivated = true;
        DialogueManager.GetInstance().CutsceneStarted();

        holder.SetActive(true);

        // pull JSON data for intro cutscene
        CutsceneData cutsceneData = JsonUtility.FromJson<CutsceneData>(introDataJSON.text);

        // get the text asset
        string textName = cutsceneData.textName;
        textAsset = Resources.Load<TextAsset>(System.IO.Path.Combine(introPath, textName));

        // make a queue of the images and pop the first one
        imagesQueue = new Queue<CutsceneImageData>(cutsceneData.imageNames);

        NextImage();

        // set the UI image active and enter dialogue mode
        cutsceneImage.SetActive(true);

        currentStory = new Story(textAsset.text);

        EnterTextMode(textAsset);
        
    }

    private void NextImage()
    {

        if (imagesQueue.Count != 0)
        {
            currentImageName = imagesQueue.Dequeue().imageName;
            cutsceneImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(System.IO.Path.Combine(introPath, currentImageName));
        }
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
            //Debug.Log(nextLine);
            cutsceneText.text = nextLine;
        }
        else
        {
            if (exitOnlyOnce != null) { return; }

            exitOnlyOnce = StartCoroutine(ExitTextMode());
        }
    }

    private IEnumerator ExitTextMode()
    {
        yield return new WaitForSeconds(0.2f);

        isTextPlaying = false;
        introActivated = false;


        DialogueManager.GetInstance().CutsceneStopped();
        GameStateManager.GetInstance().LoadLevelIntro();
        
        cutsceneText.text = "";
        cutsceneImage.SetActive(false);
        holder.SetActive(false);

        
    }
}
