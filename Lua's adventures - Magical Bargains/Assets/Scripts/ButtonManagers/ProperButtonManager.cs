using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProperButtonManager : MonoBehaviour
{

    [Header("State Buttons")]
    [SerializeField] private Button inspectButton;
    [SerializeField] private Button bargainButton;
    [SerializeField] private Button introButton;

    [Header("Tools Buttons")]
    [SerializeField] private Button magnifierButton;
    [SerializeField] private Button cameraButton;

    [SerializeField] private GameObject magnifier;
    [SerializeField] private CameraScript cameraScript;

    private bool magnifierButtonActivated = false;
    private bool cameraButtonActivated = false;




    // Start is called before the first frame update
    void Start()
    {
        inspectButton.gameObject.SetActive(true);
        inspectButton.interactable = true;

        bargainButton.gameObject.SetActive(false);
        bargainButton.interactable = false;

        introButton.gameObject.SetActive(false);
        introButton.interactable = false;

        inspectButton.onClick.AddListener(OnInspectButtonClick);
        bargainButton.onClick.AddListener(OnBargainButtonClick);
        introButton.onClick.AddListener(OnIntroButtonClick);

        magnifierButton.onClick.AddListener(OnMagnifierButtonClick);
        magnifier.SetActive(false);
        cameraButton.onClick.AddListener(OnCameraButtonClick);

    }

    public void OnInspectButtonClick() {
        if (DialogueManager.GetInstance().dialogueIsFinished) 
        {
            // load inspect state
            GameStateManager2.GetInstance().LoadInspectState();
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void OnBargainButtonClick() {
        // load bargain state
        GameStateManager2.GetInstance().LoadBargainState();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnIntroButtonClick() {
        if (DialogueManager.GetInstance().dialogueIsFinished) 
        {
            // load intro state
            GameStateManager2.GetInstance().LoadIntroState();
            EventSystem.current.SetSelectedGameObject(null);
        }
    }


    public void OnMagnifierButtonClick()
    {
        if (magnifierButtonActivated)
        {

            magnifier.SetActive(false);
            magnifierButtonActivated = false;
        }
        else
        {

            magnifier.SetActive(true);
            magnifierButtonActivated = true;
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnCameraButtonClick() {
        if (cameraButtonActivated)
        {
            cameraButtonActivated = false;
            cameraScript.RemovePhoto();
        }
        else
        {
            cameraButtonActivated = true;
            cameraScript.StartProcess();
        }
    }

    void Update() {

        string currentState = GameStateManager2.GetInstance().GetState();

        bool isInspectActive = false;
        bool isBargainActive = false;
        bool isIntroActive = false;

        bool isToolsActive = false;

        // if state is intro and dialogue is finished -> show inspect button
        if (currentState == "intro" && DialogueManager.GetInstance().dialogueIsFinished)
        {
            isInspectActive = true;
        }


        if (currentState == "inspect") 
        {
            isBargainActive = true;
            isToolsActive = true;
        }

        if (currentState == "bargain" && DialogueManager.GetInstance().dialogueIsFinished)
        {
            isIntroActive = true;
        }

        inspectButton.gameObject.SetActive(isInspectActive);
        inspectButton.interactable = isInspectActive;

        bargainButton.gameObject.SetActive(isBargainActive);
        bargainButton.interactable = isBargainActive;

        introButton.gameObject.SetActive(isIntroActive);
        introButton.interactable = isIntroActive;

        magnifierButton.gameObject.SetActive(isToolsActive);
        magnifierButton.interactable = isToolsActive;

        cameraButton.gameObject.SetActive(isToolsActive);
        cameraButton.interactable = isToolsActive;
    }
}
