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
    [SerializeField] private bool skipTypingAsAWhole = false;
    [SerializeField] private bool typingActivatedButSkippable = false;
    [SerializeField] private float normalSpeed = 0.05f;
    [SerializeField] private float fasterSpeed = 0.001f;

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
    private bool stopTyping = false;

    private static DialogueManager instance;
    // name and name color
    private string speakerName;
    private string speakerColor;
    // dialogs options
    public Button[] choiceButtons;
    

    // to set the speaker name and color
    public void SetSpeaker(string speakerNameSet, string speakerColorSet)
    {
        speakerName = speakerNameSet;
        speakerColor = speakerColorSet;
    }

    private void Awake()
    {
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

            if (isCurrentlyTyping)
            {
                spaceBarIcon.color = Color.white;

                if (Input.GetKeyDown(inputKey)) {

                    if (typingActivatedButSkippable)
                    {
                        stopTyping = true;
                    }
                    else {
                        typingSpeed = fasterSpeed;
                    }
                    
                    
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
    
    public void ContinueStory()
    {
        HideChoices();
        if (currentStory.canContinue)
        {
            string nextLine = currentStory.Continue();
            string nextLineTrim = nextLine.TrimStart();
            //check if already has speaker tag (for grandpa)
            // bool checkTag = nextLineTrim.StartsWith("Grandpa");
            if (nextLineTrim.StartsWith("avô"))
            {
                // Debug.Log("THIS IS GRANDPA");
                // Find the index of the colon after "avô Pedro"
                int colonIndex = nextLineTrim.IndexOf(':');
                if (colonIndex >= 0)
                {
                    // Extract "avô Pedro:" including colon
                    string speakerTag = nextLineTrim.Substring(0, colonIndex + 1);
                    string restOfLine = nextLineTrim.Substring(colonIndex + 1);

                    // Color only the speaker tag
                    nextLine = $"<color=#db1226>{speakerTag}</color>{restOfLine}";
                }
                else
                {
                    Debug.Log("yo you forgot to put avô Pedro in front of line ");
                }
            }
            else if (nextLineTrim.StartsWith("\""))
            {
                // Debug.Log("THIS IS LUA");
                string optionText = nextLineTrim.Trim();

                if (optionText.Length >= 2 && optionText.StartsWith("\"") && optionText.EndsWith("\""))
                {
                    optionText = optionText.Substring(1, optionText.Length - 2);
                }
                
                nextLine = $"<color=#db12a5>Lua</color>: {optionText}";
            }
            else
            {
                // Debug.Log("THIS IS CLIENT");
                string speakerInfo = $"<color={speakerColor}>{speakerName}:</color> ";
                nextLine = speakerInfo + nextLine;

            }

            // display new line using a typing effect:
            if (typingCoroutine != null) { StopCoroutine(typingCoroutine); }

            typingCoroutine = StartCoroutine(TypeLine(nextLine));
        }
        else if (currentStory.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
        else
        {
            if (ExitDialogueCoroutine != null) { return; }

            ExitDialogueCoroutine = StartCoroutine(ExitDialogueMode());
        }
    }

    void DisplayChoices()
    {

        int choiceCount = currentStory.currentChoices.Count;

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choiceCount)
            {
                choiceButtons[i].gameObject.SetActive(true);

                TMP_Text buttonText = choiceButtons[i].GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                {
                    buttonText.text = currentStory.currentChoices[i].text;
                }
                else
                {
                    Debug.LogError($"Button {i} is missing a TMP_Text component!");
                }

                // Clear previous listeners to avoid stacking
                choiceButtons[i].onClick.RemoveAllListeners();

                int choiceIndex = i; // capture index for the lambda
                choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(choiceIndex));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }
 
    public void OnChoiceSelected(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        HideChoices();
        ContinueStory();
    }

    void HideChoices()
    {
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }
    }


    private IEnumerator TypeLine(string line)
    {

        if (!skipTypingAsAWhole )
        {

            isCurrentlyTyping = true;

            dialogueText.maxVisibleCharacters = 0;
            dialogueText.text = line;

            for (int i = 0; i <= line.Length; i++)
            {
                if (stopTyping) {
                    dialogueText.maxVisibleCharacters = line.Length;
                    break;
                }

                dialogueText.maxVisibleCharacters = i;
                yield return new WaitForSeconds(typingSpeed);
            }

            isCurrentlyTyping = false;
            typingSpeed = normalSpeed;
        } else
        {
            isCurrentlyTyping = false;
            dialogueText.text = line;
        }
        stopTyping = false;

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
