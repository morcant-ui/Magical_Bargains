using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private OfferManager offerManager;

    [Header("State Buttons")]
    [SerializeField] private Button openShopButton;
    [SerializeField] private Button inspectButton;
    [SerializeField] private Button bargainButton;
    [SerializeField] private Button introButton;

    [Header("Bargain Buttons")]
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button addButton;
    [SerializeField] private Button minusButton;

    

    [Header("Tools Buttons")]
    [SerializeField] private Button magnifierButton;
    [SerializeField] private Button cameraButton;
    [SerializeField] private Button thermometerButton;

    [Header("Tools Themselves")]
    [SerializeField] private GameObject magnifier;
    [SerializeField] private CameraScript cameraScript;
    [SerializeField] private GameObject fishingGame;
    [SerializeField] private GameObject thermometer;
    [SerializeField] private ShakingThermo thermometerScript;

    private bool magnifierButtonActivated = false;
    private bool cameraButtonActivated = false;
    private bool thermometerButtonActivated = false;

    private bool offerAccepted = false;




    // Start is called before the first frame update
    void Start()
    {
        openShopButton.gameObject.SetActive(true);
        openShopButton.interactable = true;

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

        openShopButton.onClick.AddListener(OnOpenShopClick);
        inspectButton.onClick.AddListener(OnInspectButtonClick);
        bargainButton.onClick.AddListener(OnBargainButtonClick);
        introButton.onClick.AddListener(OnIntroButtonClick);

        magnifierButton.onClick.AddListener(OnMagnifierButtonClick);
        magnifier.SetActive(false);

        cameraButton.onClick.AddListener(OnCameraButtonClick);
        fishingGame.SetActive(false);

        thermometerButton.onClick.AddListener(OnThermometerButtonClick);
        thermometer.SetActive(false);

        acceptButton.onClick.AddListener(OnAcceptOfferButtonClick);
        addButton.onClick.AddListener(OnAddOfferButtonClick);
        minusButton.onClick.AddListener(OnReduceOfferButtonClick);

    }

    public void OnOpenShopClick() {
        if (DialogueManager.GetInstance().dialogueIsFinished) {
            GameStateManager.GetInstance().LoadClientIntro();
            EventSystem.current.SetSelectedGameObject(null);
        }
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
            GameStateManager.GetInstance().LoadClientIntro();
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
            fishingGame.SetActive(false);
            cameraScript.Abort();
        }
        else
        {
            cameraButtonActivated = true;
            fishingGame.SetActive(true);
            cameraScript.StartProcess();
        }
        EventSystem.current.SetSelectedGameObject(null);
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

        bool dialogueFinished = DialogueManager.GetInstance().dialogueIsFinished;
        bool tutoActive = GameStateManager.GetInstance().tutoActivated;
        bool additionalCheck = true;
        

        bool isOpenShopActive = false;
        bool isInspectActive = false;
        bool isBargainActive = false;
        bool isIntroActive = false;

        bool isToolsActive = false;

        bool bargainInProgress = false;

        if (currentState == "level intro" && dialogueFinished) {
            isOpenShopActive = true;
        }

        // if state is intro and dialogue is finished -> show inspect button
        if (currentState == "client intro" && dialogueFinished)
        {
            isInspectActive = true;

            if (tutoActive && !dialogueFinished) { additionalCheck = false; }
        }

        if (currentState == "inspect" ) 
        {       
            isBargainActive = true;
            isToolsActive = true;

            if ( tutoActive && !dialogueFinished ) { additionalCheck = false; }
        }

        if (currentState == "bargain" && !offerAccepted){
           
            bargainInProgress = true;

            if (tutoActive && !dialogueFinished) { additionalCheck = false; }
        }

        if (currentState == "client outro" && dialogueFinished)
        {
            offerAccepted = false;
            bargainInProgress = false;
            isIntroActive = true;
        }

        openShopButton.gameObject.SetActive(isOpenShopActive);
        openShopButton.interactable = isOpenShopActive;

        inspectButton.gameObject.SetActive(isInspectActive && additionalCheck);
        inspectButton.interactable = isInspectActive && additionalCheck;

        bargainButton.gameObject.SetActive(isBargainActive);
        bargainButton.interactable = isBargainActive && additionalCheck;

        introButton.gameObject.SetActive(isIntroActive && additionalCheck);
        introButton.interactable = isIntroActive && additionalCheck;

        acceptButton.gameObject.SetActive(bargainInProgress);
        addButton.gameObject.SetActive(bargainInProgress);
        minusButton.gameObject.SetActive(bargainInProgress);
        acceptButton.interactable = bargainInProgress;
        addButton.interactable = bargainInProgress;
        minusButton.interactable = bargainInProgress;


        thermometerButton.gameObject.SetActive(isToolsActive);
        thermometerButton.interactable = (!(magnifierButtonActivated || cameraButtonActivated)) && additionalCheck;

        magnifierButton.gameObject.SetActive(isToolsActive);
        magnifierButton.interactable = (!(cameraButtonActivated || thermometerButtonActivated)) && additionalCheck;

        cameraButton.gameObject.SetActive(isToolsActive);
        cameraButton.interactable = (!(magnifierButtonActivated || thermometerButtonActivated)) && additionalCheck;

        if (!magnifierButtonActivated)
        {
            magnifier.gameObject.SetActive(false);
        }
        if (!cameraButtonActivated) 
        {
            cameraScript.Abort();
            fishingGame.SetActive(false);
        }
        if (!thermometerButtonActivated)
        {
            thermometer.gameObject.SetActive(false);
        }
    }
}
