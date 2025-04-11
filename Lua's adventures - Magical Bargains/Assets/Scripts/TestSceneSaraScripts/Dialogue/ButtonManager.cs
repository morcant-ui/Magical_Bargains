using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Buttons")]
    [SerializeField] private Button dialogueButton;
    [SerializeField] private Button inspectButton;

    [Header("Scenes")]
    [SerializeField] private string inspectScene = "TestSceneSara";

    private bool dialogueButtonActivated = false;
    private bool inspectButtonActivated = false;

    private void Start()
    {

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

        // if player presses button, trigger dialogue managment
        if (dialogueButtonActivated && !DialogueManager.GetInstance().dialogueIsPlaying && !DialogueManager.GetInstance().dialogueIsFinished)
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            dialogueButtonActivated = false;
            EventSystem.current.SetSelectedGameObject(null);

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
                Debug.Log("don't know what to do now");
                Debug.Log(currentState);
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
