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



    private Story currentStory;

    private string inputKey = "space";

    public bool dialogueIsPlaying { get; private set; }
    public bool dialogueIsFinished { get; private set; }
    


    private static DialogueManager instance;

    private void Awake() {
        if (instance != null && instance != this)
        {
            Debug.Log("what's up with this ?");
            Destroy(this.gameObject); // kill extra copies

        }
        else
        {

            instance = this;
        }


        dialogueIsPlaying = false;
        dialogueIsFinished = false;
        dialoguePanel.SetActive(false);
    }

    public static DialogueManager GetInstance() {
        return instance;
    }


    private void Update() {
        if (!dialogueIsPlaying) {
            return;
        }

        if (dialogueIsPlaying && Input.GetKeyDown(inputKey)) {
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
        dialogueIsFinished = true;


        dialoguePanel.SetActive(false);
        dialogueText.text = "";

    }

    public void Reset() {
        dialogueIsPlaying = false;
        dialogueIsFinished = false;
    }

}
