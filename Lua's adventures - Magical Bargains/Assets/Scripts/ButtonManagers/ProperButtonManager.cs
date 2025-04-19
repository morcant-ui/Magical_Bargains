using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProperButtonManager : MonoBehaviour
{

    [Header("Buttons")]
    [SerializeField] private Button inspectButton;
    [SerializeField] private Button bargainButton;
    [SerializeField] private Button introButton;



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

    void Update() {

        string currentState = GameStateManager2.GetInstance().GetState();

        bool isInspectActive = false;
        bool isBargainActive = false;
        bool isIntroActive = false;

        // if state is intro and dialogue is finished -> show inspect button
        if (currentState == "intro" && DialogueManager.GetInstance().dialogueIsFinished)
        {
            isInspectActive = true;
        }


        if (currentState == "inspect") 
        {
            isBargainActive = true;
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

    }
}
