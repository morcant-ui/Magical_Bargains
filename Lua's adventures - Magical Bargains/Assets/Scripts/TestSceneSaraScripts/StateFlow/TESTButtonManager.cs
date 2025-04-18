using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class TESTButtonManager : MonoBehaviour
{

    private TextAsset dialogueIntro;
    private TextAsset dialogueOutro;

    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button nextButton;

    [Header("Level Manager")]
    [SerializeField] private LevelManager levelManager;



    private bool startButtonActivated = false;
    private bool nextButtonActivated = false;

    private void Start()
    {
        string currentState = GameStateManager.GetInstance().GetState();



        // Attach a listener to the button
        startButton.onClick.AddListener(OnStartButtonClick);
        nextButton.onClick.AddListener(OnNextButtonClick);
    }

    public void LoadDialogue(TextAsset dialogueA) {

        dialogueIntro = dialogueA;
        startButton.interactable = true;
        
    }

    public void OnStartButtonClick()
    {
        if (!startButtonActivated)
        {
            startButtonActivated = true;
        }
    }

    public void OnNextButtonClick()
    {
        if (!nextButtonActivated && DialogueManager.GetInstance().dialogueIsFinished)
        {
            nextButtonActivated = true;
        }
    }


    private void Update()
    {

        if (startButtonActivated)
        {



            // if player presses button, trigger dialogue managment
            if (!DialogueManager.GetInstance().dialogueIsPlaying && !DialogueManager.GetInstance().dialogueIsFinished)
            {
                DialogueManager.GetInstance().EnterDialogueMode(dialogueIntro);
                startButtonActivated = false;
                EventSystem.current.SetSelectedGameObject(null);

            }


        }



        if (nextButtonActivated && DialogueManager.GetInstance().dialogueIsFinished)
        {



            /////////////////////////////// call to stack
            // for the above to work: goto file -> build settings -> drag and drop desired scene to the build
            levelManager.LoadNextClient();
            EventSystem.current.SetSelectedGameObject(null);

            DialogueManager.GetInstance().Reset();


            // should be receiving event info from level manager for when the queue is empty ?
            // to remove the buttons ? or we just quit the level idk

            startButtonActivated = false;
            nextButtonActivated = false;
            startButton.interactable = true;
            nextButton.interactable = false;


        }

        if (!DialogueManager.GetInstance().dialogueIsFinished)
        {
            if (dialogueIntro != null) {
                startButton.interactable = true;
            } else {
                startButton.interactable = false;
            }
            
            nextButton.interactable = false;
        }
        else
        {
            startButton.interactable = false;
            nextButton.interactable = true;
        }
    }




}
