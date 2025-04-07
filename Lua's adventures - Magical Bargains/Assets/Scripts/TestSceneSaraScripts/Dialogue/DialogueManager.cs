using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{

    // "manage and display the dialogue that's written to the UI"

    [Header("Dialogue UI")]

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Examination Button")]
    [SerializeField] private GameObject examinationButton;



    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }


    private static DialogueManager instance;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("Found more than one Dialogue Manager !");
        }
        instance = this;
    }

    public static DialogueManager GetInstance() {
        return instance;
    }

    private void Start() {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        if (GameStateManager.GetInstance().GetState() != "examination") {
            examinationButton.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void Update() {
        if (!dialogueIsPlaying) {
            return;
        }

        if (dialogueIsPlaying && Input.GetKeyDown("space")) {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON) {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    public void ContinueStory() {
        if (currentStory.canContinue)
        {
            string nextLine = currentStory.Continue();
            Debug.Log(nextLine);
            dialogueText.text = nextLine;
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator ExitDialogueMode() {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        string currentState = GameStateManager.GetInstance().GetState() ;

        if (currentState == "dialogue1") {

            examinationButton.GetComponent<SpriteRenderer>().enabled = true;

        } else {
            Debug.Log("don't know what to do now");
            Debug.Log(currentState);
        }
    }

}
