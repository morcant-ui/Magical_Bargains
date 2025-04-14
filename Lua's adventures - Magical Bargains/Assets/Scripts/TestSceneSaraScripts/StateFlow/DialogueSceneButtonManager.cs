using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class DialogueSceneButtonManager : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset dialogue1;
    [SerializeField] private TextAsset dialogue2;

    [Header("Buttons")]
    [SerializeField] private Button dialogueButton;
    [SerializeField] private Button inspectButton;

    [Header("Scenes")]
    [SerializeField] private string inspectScene = "TestInspectScene";

    private bool dialogueButtonActivated = false;
    private bool inspectButtonActivated = false;

    private void Start()
    {
        string currentState = GameStateManager.GetInstance().GetState();

        if (currentState == "dialogue2") {
            inspectButton.interactable = false;
        }

        // Attach a listener to the button
        dialogueButton.onClick.AddListener(OnDialogueButtonClick);
        inspectButton.onClick.AddListener(OnInspectButtonClick);
    }


    public void OnDialogueButtonClick()
    {
        if (!dialogueButtonActivated)
        {
            dialogueButtonActivated = true;
        }
    }

    public void OnInspectButtonClick()
    {
        if (!inspectButtonActivated && DialogueManager.GetInstance().dialogueIsFinished)
        {
            inspectButtonActivated = true;
        }
    }


    private void Update() {

        if (dialogueButtonActivated)
        {
            // distinction between scenes:
            string currentState = GameStateManager.GetInstance().GetState();

            if (currentState == "dialogue1")
            {

                // if player presses button, trigger dialogue managment
                if (!DialogueManager.GetInstance().dialogueIsPlaying && !DialogueManager.GetInstance().dialogueIsFinished)
                {
                    DialogueManager.GetInstance().EnterDialogueMode(dialogue1);
                    dialogueButtonActivated = false;
                    EventSystem.current.SetSelectedGameObject(null);

                }

            }
            else if (currentState == "dialogue2")
            {
                if (!DialogueManager.GetInstance().dialogueIsPlaying && !DialogueManager.GetInstance().dialogueIsFinished)
                {
                    DialogueManager.GetInstance().EnterDialogueMode(dialogue2);
                    dialogueButtonActivated = false;
                    EventSystem.current.SetSelectedGameObject(null);

                }
            }
        }



        if (inspectButtonActivated && DialogueManager.GetInstance().dialogueIsFinished) {

            string currentState = GameStateManager.GetInstance().GetState();

            if (currentState == "dialogue1")
            {

                GameStateManager.GetInstance().SetState("examination");
                SceneManager.LoadScene(inspectScene);
                // for the above to work: goto file -> build settings -> drag and drop desired scene to the build

            }
            else
            {
                Debug.Log("We should currently be in the following state: " + currentState);
                Debug.Log("At this point the client should leave");
            }
        }

        if (!DialogueManager.GetInstance().dialogueIsFinished)
        {
            dialogueButton.interactable = true;
            inspectButton.interactable = false;
        }
        else {
            dialogueButton.interactable = false;
            inspectButton.interactable = true;
        }
    }




}
