using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{

    [Header("State Buttons")]
    [SerializeField] private Button inspectButton;
    [SerializeField] private Button bargainButton;
    [SerializeField] private Button introButton;

    [Header("Bargain Buttons")]
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button addButton;
    [SerializeField] private Button minusButton;

    [SerializeField] private OfferManager offerManager;

    [Header("Tools Buttons")]
    [SerializeField] private Button magnifierButton;
    [SerializeField] private Button cameraButton;
    [SerializeField] private Button thermometerButton;


    [SerializeField] private GameObject magnifier;
    [SerializeField] private CameraScript cameraScript;
    [SerializeField] private GameObject thermometer;
    [SerializeField] private ShakingThermo thermometerScript;

    private bool magnifierButtonActivated = false;
    private bool cameraButtonActivated = false;
    private bool thermometerButtonActivated = false;

    private bool offerAccepted = false;




    // Start is called before the first frame update
    void Start()
    {
        inspectButton.gameObject.SetActive(true);
        inspectButton.interactable = true;

        bargainButton.gameObject.SetActive(false);
        bargainButton.interactable = false;

        introButton.gameObject.SetActive(false);
        introButton.interactable = false;

        acceptButton.gameObject.SetActive(false);
        acceptButton.interactable = false;
        addButton.gameObject.SetActive(false);
        addButton.interactable = false;
        minusButton.gameObject.SetActive(false);
        minusButton.interactable = false;

        inspectButton.onClick.AddListener(OnInspectButtonClick);
        bargainButton.onClick.AddListener(OnBargainButtonClick);
        introButton.onClick.AddListener(OnIntroButtonClick);

        magnifierButton.onClick.AddListener(OnMagnifierButtonClick);
        magnifier.SetActive(false);
        cameraButton.onClick.AddListener(OnCameraButtonClick);
        thermometerButton.onClick.AddListener(OnThermometerButtonClick);
        thermometer.SetActive(false);

        acceptButton.onClick.AddListener(OnAcceptOfferButtonClick);
        addButton.onClick.AddListener(OnAddOfferButtonClick);
        minusButton.onClick.AddListener(OnReduceOfferButtonClick);

    }

    public void OnInspectButtonClick() {
        if (DialogueManager.GetInstance().dialogueIsFinished) 
        {
            // load inspect state
            GameStateManager.GetInstance().LoadInspectState();
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void OnBargainButtonClick() {
        // load bargain state
        GameStateManager.GetInstance().LoadBargainState();
        EventSystem.current.SetSelectedGameObject(null);

        magnifier.gameObject.SetActive(false);
        cameraScript.Abort();
        cameraButtonActivated = false;
        magnifierButtonActivated = false;
        thermometer.gameObject.SetActive(false);
        thermometerButtonActivated = false;
    }

    public void OnAddOfferButtonClick(){
        offerManager.ChangeOffer(10);

        magnifier.gameObject.SetActive(false);
        cameraScript.Abort();
        cameraButtonActivated = false;
        magnifierButtonActivated = false;
        thermometer.gameObject.SetActive(false);
        thermometerButtonActivated = false;
    }

    public void OnReduceOfferButtonClick(){
        offerManager.ChangeOffer(-10);

        magnifier.gameObject.SetActive(false);
        cameraScript.Abort();
        cameraButtonActivated = false;
        magnifierButtonActivated = false;
        thermometer.gameObject.SetActive(false);
        thermometerButtonActivated = false;
    }
    public void OnAcceptOfferButtonClick(){
        
        offerAccepted = true;

        offerManager.AcceptOffer();

        GameStateManager.GetInstance().LoadBargainDoneState();
        

        magnifier.gameObject.SetActive(false);
        cameraScript.Abort();
        cameraButtonActivated = false;
        magnifierButtonActivated = false;
        thermometer.gameObject.SetActive(false);
        thermometerButtonActivated = false;
    }

    public void OnIntroButtonClick() {
        if (DialogueManager.GetInstance().dialogueIsFinished) 
        {
            // load intro state
            GameStateManager.GetInstance().LoadIntroState();
            EventSystem.current.SetSelectedGameObject(null);

            magnifier.gameObject.SetActive(false);
            cameraScript.Abort();
            cameraButtonActivated = false;
            magnifierButtonActivated = false;
            thermometer.gameObject.SetActive(false);
            thermometerButtonActivated = false;
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
            cameraScript.Abort();
        }
        else
        {
            cameraButtonActivated = true;
            cameraScript.StartProcess();
        }
    }

    public void OnThermometerButtonClick()
    {
        if (thermometerButtonActivated)
        {

            thermometer.SetActive(false);
            thermometerButtonActivated = false;
        }
        else
        {

            thermometer.SetActive(true);
            thermometerButtonActivated = true;
            thermometerScript.StartProcess();
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    void Update() {

        string currentState = GameStateManager.GetInstance().GetState();

        bool isInspectActive = false;
        bool isBargainActive = false;
        bool isIntroActive = false;

        bool isToolsActive = false;

        bool bargainInProgress = false;

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

        if (currentState == "bargain" && !offerAccepted){
           
            bargainInProgress = true;
        }

        if (currentState == "bargainDone" && DialogueManager.GetInstance().dialogueIsFinished)
        {
            offerAccepted = false;
            bargainInProgress = false;
            isIntroActive = true;
            

        }

        inspectButton.gameObject.SetActive(isInspectActive);
        inspectButton.interactable = isInspectActive;

        bargainButton.gameObject.SetActive(isBargainActive);
        bargainButton.interactable = isBargainActive;

        introButton.gameObject.SetActive(isIntroActive);
        introButton.interactable = isIntroActive;

        acceptButton.gameObject.SetActive(bargainInProgress);
        addButton.gameObject.SetActive(bargainInProgress);
        minusButton.gameObject.SetActive(bargainInProgress);
        acceptButton.interactable = bargainInProgress;
        addButton.interactable = bargainInProgress;
        minusButton.interactable = bargainInProgress;
         

        magnifierButton.gameObject.SetActive(isToolsActive);
        magnifierButton.interactable = !cameraButtonActivated;
        magnifierButton.interactable = !thermometerButtonActivated;

        cameraButton.gameObject.SetActive(isToolsActive);
        cameraButton.interactable = !magnifierButtonActivated;
        cameraButton.interactable = !thermometerButtonActivated;

        thermometerButton.gameObject.SetActive(isToolsActive);
        thermometerButton.interactable = !magnifierButtonActivated;
        thermometerButton.interactable = !cameraButtonActivated;

        if (!magnifierButtonActivated)
        {
            magnifier.gameObject.SetActive(false);
        }
        if (!cameraButtonActivated) 
        {
            cameraScript.Abort();
        }
        if (!thermometerButtonActivated)
        {
            thermometer.gameObject.SetActive(false);
        }
    }
}
