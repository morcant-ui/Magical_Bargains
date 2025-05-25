using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    // "manage and display the dialogue that's written to the UI"

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private GameObject savingsImage;
    [SerializeField] private TextMeshProUGUI savingsText;
    [SerializeField] private Image spaceBarIcon;

    [Header("Typing Speed")]
    [SerializeField] private float normalSpeed = 0.05f;
    [SerializeField] private float fasterSpeed = 0.05f;

    [Header("Flutter Effect")]
    [SerializeField] private float flutterSpeed = 0.5f;

    // Booleans
    public bool dialogueIsPlaying { get; private set; }
    public bool dialogueIsFinished { get; private set; }

    private bool isCurrentlyTyping;

    private bool isCutscenePlaying;

    // user input to get next line of dialogue
    private string inputKey = "space";

    // to keep track of Story
    private Story currentStory;

    private TextAsset secondInk;


    // coroutines to avoid having them called more than once at a time
    private Coroutine ExitDialogueCoroutine;
    private Coroutine typingCoroutine;
    
    private Coroutine SpaceBarFlutterCoroutine;
    private Coroutine FlickerSavingsCoroutine;

    // typing speed variable
    private float typingSpeed;

    private static DialogueManager instance;

    private void Awake() {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject); 
        }
        else
        {
            instance = this;
        }


        // initialize the booleans
        dialogueIsPlaying = false;
        dialogueIsFinished = false;
        isCutscenePlaying = GameStateManager.GetInstance().WillIntroPlay();

        dialoguePanel.SetActive(!isCutscenePlaying);

        typingSpeed = normalSpeed;
    }

    public static DialogueManager GetInstance() {
        return instance;
    }


    private void Update() {
        if (!dialogueIsPlaying) {
            return;
        }


        if (dialogueIsPlaying && !isCutscenePlaying) {

            if (isCurrentlyTyping )
            {
                spaceBarIcon.color = Color.white;

                if (Input.GetKeyDown(inputKey)) {
                    typingSpeed = fasterSpeed;
                }
            }

            else if (!isCurrentlyTyping) {

                // make space bar icon's color flutter when dialogue is ready to go on
                if (SpaceBarFlutterCoroutine == null) {
                    SpaceBarFlutterCoroutine = StartCoroutine(makeSpaceBarFlutter()); 
                }

                if (Input.GetKeyDown(inputKey))
                {        

                    ContinueStory();
                    
                }
            }
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON, TextAsset secondInkJSON = null) {

        if (inkJSON == null) {
            Debug.Log("EnterDialogueMode: NO DIALOGUE TO READ");
            return;
        }
        
        if (secondInkJSON != null)
        {
            secondInk = secondInkJSON;
        }

        if (isCutscenePlaying) {
            Debug.Log("EnterDialogueMode: you cannot do that !");
        }

        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;

        dialoguePanel.SetActive(true);
        ExitDialogueCoroutine = null;

        ContinueStory();
    }

    public void ContinueStory() {
        if (currentStory.canContinue)
        {
            string nextLine = currentStory.Continue();

            // display new line using a typing effect:
            if (typingCoroutine != null) { StopCoroutine(typingCoroutine); }

            typingCoroutine = StartCoroutine(TypeLine(nextLine));
        }
        else
        {
            if (ExitDialogueCoroutine != null) { return; }

            ExitDialogueCoroutine = StartCoroutine(ExitDialogueMode());
        }
    }


    private IEnumerator TypeLine(string line) {

        isCurrentlyTyping = true;

        dialogueText.maxVisibleCharacters = 0;
        dialogueText.text = line;

        for (int i = 0; i <= line.Length; i++) {

            dialogueText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typingSpeed);
        }

        isCurrentlyTyping = false;
        typingSpeed = normalSpeed;
    }


    private IEnumerator ExitDialogueMode() {
        yield return new WaitForSeconds(0.2f);

        if (secondInk != null)
        {

            EnterDialogueMode(secondInk);
            secondInk = null;
        }
        else
        {

            dialogueIsPlaying = false;
            dialogueIsFinished = true;

            dialoguePanel.SetActive(false);
            savingsImage.SetActive(false);
            dialogueText.text = "";

            StopFluttering();

            StopFlickering();

        }
    }

    public void Reset() {
        dialogueIsPlaying = false;
        dialogueIsFinished = false;
        isCurrentlyTyping = false;
        secondInk = null;
    }


    private IEnumerator makeSpaceBarFlutter() {

        Color flutterColor = new Color (0.9f, 0.9f, 0.9f);

        while ( dialogueIsPlaying && !isCurrentlyTyping ) { 
        
            if ( spaceBarIcon.color == Color.white ) {
                spaceBarIcon.color = flutterColor;
            } else if (spaceBarIcon.color == flutterColor) {
                spaceBarIcon.color = Color.white;
            }

            yield return new WaitForSeconds( flutterSpeed );

        }

        StopFluttering();
    }

    private void StopFluttering() {
        if (SpaceBarFlutterCoroutine != null)
        {
            StopCoroutine(SpaceBarFlutterCoroutine);
            SpaceBarFlutterCoroutine = null;
        }

        spaceBarIcon.color = Color.white;

    }

    public void TriggerSavingsFlickering() {
        if (FlickerSavingsCoroutine != null)
        {
            StopFlickering();
        }

        FlickerSavingsCoroutine = StartCoroutine(FlickerSavings());
    }

    IEnumerator FlickerSavings()
    {

        while ( !dialogueIsFinished )
        {
            if (savingsText.color == Color.red)
            {
                savingsText.color = Color.white;
            }
            else if (savingsText.color == Color.white)
            {
                savingsText.color = Color.red;
            }

            yield return new WaitForSeconds(flutterSpeed);

        }
    }

    private void StopFlickering() {
        if (FlickerSavingsCoroutine != null)
        {
            StopCoroutine(FlickerSavingsCoroutine);
            FlickerSavingsCoroutine = null;
        }

        savingsText.color = Color.white;
    }


    public void CutsceneStarted() {

        isCutscenePlaying = true;
    }

    public void CutsceneStopped() {

        isCutscenePlaying = false;
    }
}
