using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private OfferManager offerManager;

    [Header("State Buttons")]
    [SerializeField] private Button openShopButton;
    [SerializeField] private Button inspectButton;
    [SerializeField] private Button bargainButton;
    [SerializeField] private Button introButton;

    [Header("Bargain Buttons")]
    [SerializeField] private GameObject bargainFrog;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button refuseButton;
    [SerializeField] private Button addButton;
    [SerializeField] private Button minusButton;

    [Header("Bargain AGAIN Buttons")]
    [SerializeField] private Button problemThermoButton;
    [SerializeField] private Button problemCameraButton;
    [SerializeField] private Button problemMagniButton;
    [SerializeField] private Button moreMoneyButton;

    

    [Header("Tools Buttons")]
    [SerializeField] private Button magnifierButton;
    [SerializeField] private Button cameraButton;
    [SerializeField] private Button thermometerButton;

    [Header("Tools Themselves")]
    [SerializeField] private GameObject magnifier;
    [SerializeField] private GameObject magnifierHandle;
    [SerializeField] private CameraScript cameraScript;
    [SerializeField] private GameObject fishingGame;
    [SerializeField] private GameObject thermometer;
    [SerializeField] private ShakingThermo thermometerScript;

    [Header("TUTO")]
    [SerializeField] private string endDialogueName = "tuto4";


    private Vector3 magnifierPosition = new Vector3(5.0f, -2.6f, -3.96f);

    private bool magnifierButtonActivated = false;
    private bool cameraButtonActivated = false;
    private bool thermometerButtonActivated = false;

    private bool offerAccepted = false;


    private bool isTutoActivated = false;
    private bool magnifierTutoSeen = false;
    private bool cameraTutoSeen = false;
    private bool thermoTutoSeen = false;

    private bool endDialogueReady = false;
    private bool isTutoToolsFinished = false;

    private Coroutine endOfToolsTutoCoroutine;
    private Coroutine endOfLastToolTutoCoroutine;

    
    private string tutoPathName = Path.Combine("Dialogues", "grandpa", "Tuto");




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

        refuseButton.gameObject.SetActive(false);
        refuseButton.interactable = false;

        addButton.gameObject.SetActive(false);
        addButton.interactable = false;

        minusButton.gameObject.SetActive(false);
        minusButton.interactable = false;

        problemCameraButton.gameObject.SetActive(false);
        problemCameraButton.interactable = false;

        problemMagniButton.gameObject.SetActive(false);
        problemMagniButton.interactable = false;

        problemThermoButton.gameObject.SetActive(false);
        problemThermoButton.interactable = false;

        moreMoneyButton.gameObject.SetActive(false);
        moreMoneyButton.interactable = false;

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
        refuseButton.onClick.AddListener(OnRefuseOfferButtonClick);
        addButton.onClick.AddListener(OnAddOfferButtonClick);
        minusButton.onClick.AddListener(OnReduceOfferButtonClick);

        problemCameraButton.onClick.AddListener(OnProblemCameraButtonClick);
        problemMagniButton.onClick.AddListener(OnProblemMagniButtonClick);
        problemThermoButton.onClick.AddListener(OnProblemThermoButtonClick);
        moreMoneyButton.onClick.AddListener(OnMoreMoneyButtonClick);

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

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnReduceOfferButtonClick(){
        offerManager.ChangeOffer(-10);

        magnifier.gameObject.SetActive(false);
        cameraScript.Abort();
        cameraButtonActivated = false;
        magnifierButtonActivated = false;
        thermometer.gameObject.SetActive(false);
        thermometerButtonActivated = false;

        EventSystem.current.SetSelectedGameObject(null);
    }
    public void OnAcceptOfferButtonClick()
    {

        offerAccepted = true;

        offerManager.AcceptOffer();

        GameStateManager.GetInstance().LoadBargainDoneState(false);


        magnifier.gameObject.SetActive(false);
        cameraScript.Abort();
        cameraButtonActivated = false;
        magnifierButtonActivated = false;
        thermometer.gameObject.SetActive(false);
        thermometerButtonActivated = false;

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnProblemCameraButtonClick()
    {
        // Debug.Log("trying to bargain CAMERA");

        offerManager.ChoiceActionDone();
        GameStateManager.GetInstance().LoadSecondBargainDoneState("camera");

        magnifier.gameObject.SetActive(false);
        cameraScript.Abort();
        cameraButtonActivated = false;
        magnifierButtonActivated = false;
        thermometer.gameObject.SetActive(false);
        thermometerButtonActivated = false;
    }

    public void OnProblemMagniButtonClick()
    {
        // Debug.Log("trying to bargain MAGNI");

        offerManager.ChoiceActionDone();
        GameStateManager.GetInstance().LoadSecondBargainDoneState("magnifier");

        magnifier.gameObject.SetActive(false);
        cameraScript.Abort();
        cameraButtonActivated = false;
        magnifierButtonActivated = false;
        thermometer.gameObject.SetActive(false);
        thermometerButtonActivated = false;
    }

    public void OnProblemThermoButtonClick()
    {
        // Debug.Log("trying to bargain THERMO");

        offerManager.ChoiceActionDone();
        GameStateManager.GetInstance().LoadSecondBargainDoneState("thermometer");

        magnifier.gameObject.SetActive(false);
        cameraScript.Abort();
        cameraButtonActivated = false;
        magnifierButtonActivated = false;
        thermometer.gameObject.SetActive(false);
        thermometerButtonActivated = false;
    }

    public void OnMoreMoneyButtonClick()
    {
        GameStateManager.GetInstance().LoadBargainState();

        magnifier.gameObject.SetActive(false);
        cameraScript.Abort();
        cameraButtonActivated = false;
        magnifierButtonActivated = false;
        thermometer.gameObject.SetActive(false);
        thermometerButtonActivated = false;


    }
    
    public void OnRefuseOfferButtonClick()
    {

        offerAccepted = true;

        offerManager.RefuseOffer();

        GameStateManager.GetInstance().LoadBargainDoneState(true);


        magnifier.gameObject.SetActive(false);
        cameraScript.Abort();
        cameraButtonActivated = false;
        magnifierButtonActivated = false;
        thermometer.gameObject.SetActive(false);
        thermometerButtonActivated = false;

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnIntroButtonClick()
    {
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

        EventSystem.current.SetSelectedGameObject(null);
    }


    public void OnMagnifierButtonClick()
    {
        if (magnifierButtonActivated)
        {

            magnifier.SetActive(false);
            magnifierButtonActivated = false;

            if (endDialogueReady && !isTutoToolsFinished)
            {
                LoadEndDialogue();
            }
        }
        else
        {
            Debug.Log("Magnifier position: " + magnifierHandle.transform.position);

            magnifierHandle.transform.position = magnifierPosition;
            magnifier.SetActive(true);
            magnifierButtonActivated = true;

            if (isTutoActivated && !magnifierTutoSeen && !isTutoToolsFinished) {

                // tuto: grandpa explains first you wait for it to finish and then it activates
                TextAsset tutoDialogue = Resources.Load<TextAsset>(Path.Combine(tutoPathName, "tutoMagnifier"));

                DialogueManager.GetInstance().EnterDialogueMode(tutoDialogue);

                if (cameraTutoSeen && thermoTutoSeen)
                {
                    //LoadEndDialogue(tutoDialogue);
                    endDialogueReady = true;
                }


                magnifierTutoSeen = true;
            }
            
            
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnCameraButtonClick() {
        if (cameraButtonActivated)
        {
            cameraButtonActivated = false;
            fishingGame.SetActive(false);
            cameraScript.Abort();

            if (endDialogueReady && !isTutoToolsFinished)
            {
                LoadEndDialogue();
            }
        }
        else
        {
            cameraButtonActivated = true;
            fishingGame.SetActive(true);
            cameraScript.StartProcess();

            if (isTutoActivated && !cameraTutoSeen && !isTutoToolsFinished)
            {

                // tuto: grandpa explains first you wait for it to finish and then it activates
                TextAsset tutoDialogue = Resources.Load<TextAsset>(Path.Combine(tutoPathName, "tutoCamera"));


                DialogueManager.GetInstance().EnterDialogueMode(tutoDialogue);

                if (magnifierTutoSeen && thermoTutoSeen)
                {
                    //LoadEndDialogue(tutoDialogue);
                    endDialogueReady = true;
                }

                cameraTutoSeen = true;
            }
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnThermometerButtonClick()
    {
        if (thermometerButtonActivated)
        {

            thermometer.SetActive(false);
            thermometerButtonActivated = false;

            if (endDialogueReady && !isTutoToolsFinished) {
                LoadEndDialogue();
            }
        }
        else
        {

            thermometer.SetActive(true);
            thermometerButtonActivated = true;
            thermometerScript.StartProcess();

            if (isTutoActivated && !thermoTutoSeen && !isTutoToolsFinished)
            {

                // tuto: grandpa explains first you wait for it to finish and then it activates
                TextAsset tutoDialogue = Resources.Load<TextAsset>(Path.Combine(tutoPathName, "tutoThermo"));

                DialogueManager.GetInstance().EnterDialogueMode(tutoDialogue);

                if (cameraTutoSeen && magnifierTutoSeen)
                {

                    //LoadEndDialogue(tutoDialogue);
                    endDialogueReady = true;
                }

                thermoTutoSeen = true;
            }
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    void Update() {

        string currentState = GameStateManager.GetInstance().GetState();

        bool dialoguePlaying = DialogueManager.GetInstance().dialogueIsPlaying;
        bool additionalCheck = true;
        bool bargainDoubleCheck = true;
        

        bool isOpenShopActive = false;
        bool isInspectActive = false;
        bool isBargainActive = false;
        bool isIntroActive = false;

        bool isToolsActive = false;

        bool bargainInProgress = false;

        bool choosingNewAction = false;

        if (currentState == "level intro" && !dialoguePlaying)
        {
            isOpenShopActive = true;
        }

        // if state is intro and dialogue is finished -> show inspect button
        if (currentState == "client intro" && !dialoguePlaying)
        {
            isInspectActive = true;

            if (isTutoActivated && dialoguePlaying) { additionalCheck = false; }
        }

        if (currentState == "inspect" ) 
        {       
            isBargainActive = true;
            isToolsActive = true;

            if ( isTutoActivated && dialoguePlaying ) { additionalCheck = false; }
            if (isTutoActivated && !isTutoToolsFinished) { bargainDoubleCheck = false; }
        }

        if (currentState == "bargain" && !offerAccepted){
           
            bargainInProgress = true;
            choosingNewAction = false;

            if (isTutoActivated && dialoguePlaying) { additionalCheck = false; }
        }

        if (currentState == "bargainAGAIN")
        {
            choosingNewAction = true;
            offerAccepted = false;
        }

        if (currentState == "client outro" && !dialoguePlaying)
        {
            offerAccepted = false;
            bargainInProgress = false;
            choosingNewAction = false;
            isIntroActive = true;
        }

        openShopButton.gameObject.SetActive(isOpenShopActive);
        openShopButton.interactable = isOpenShopActive;

        inspectButton.gameObject.SetActive(isInspectActive && additionalCheck);
        inspectButton.interactable = isInspectActive && additionalCheck;

        bargainButton.gameObject.SetActive(isBargainActive);
        bargainButton.interactable = isBargainActive && additionalCheck && bargainDoubleCheck;

        

        introButton.gameObject.SetActive(isIntroActive && additionalCheck);
        introButton.interactable = isIntroActive && additionalCheck;

        bargainFrog.SetActive(bargainInProgress);

        acceptButton.gameObject.SetActive(bargainInProgress);
        refuseButton.gameObject.SetActive(bargainInProgress);
        addButton.gameObject.SetActive(bargainInProgress);
        minusButton.gameObject.SetActive(bargainInProgress);
        acceptButton.interactable = bargainInProgress && additionalCheck;
        refuseButton.interactable = bargainInProgress && additionalCheck;
        addButton.interactable = bargainInProgress && additionalCheck;
        minusButton.interactable = bargainInProgress && additionalCheck;

        problemCameraButton.gameObject.SetActive(choosingNewAction);
        problemMagniButton.gameObject.SetActive(choosingNewAction);
        problemThermoButton.gameObject.SetActive(choosingNewAction);
        moreMoneyButton.gameObject.SetActive(choosingNewAction);
        problemCameraButton.interactable = choosingNewAction;
        problemMagniButton.interactable = choosingNewAction;
        problemThermoButton.interactable = choosingNewAction;
        moreMoneyButton.interactable = choosingNewAction;


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



    public void SetTutoStatus(bool tutoActivated)
    {
        isTutoActivated = tutoActivated;
    }

    private void LoadEndDialogue() {

        


        if (endOfLastToolTutoCoroutine != null) { Debug.Log("button manager: test"); StopCoroutine(endOfLastToolTutoCoroutine); }

        endOfLastToolTutoCoroutine = StartCoroutine(WaitForEndOfDialogueToFinish());
    }

    private IEnumerator WaitForEndOfDialogueToFinish() {
        yield return new WaitForSeconds(0.1f);

        while (DialogueManager.GetInstance().dialogueIsPlaying)
        {

            yield return null;
        }


        TextAsset EndTutoDialogue = Resources.Load<TextAsset>(Path.Combine(tutoPathName, endDialogueName));

        if (magnifierButtonActivated)
        {
            OnMagnifierButtonClick();
        }
        else if (cameraButtonActivated)
        {
            OnCameraButtonClick();
        }
        else if (thermometerButtonActivated) {
            OnThermometerButtonClick();
        }

        DialogueManager.GetInstance().EnterDialogueMode(EndTutoDialogue);

        if (endOfToolsTutoCoroutine != null) { Debug.Log("button manager: test"); StopCoroutine(endOfToolsTutoCoroutine); }

        endOfToolsTutoCoroutine = StartCoroutine(WaitToActivateBargainButton());

    }

    private IEnumerator WaitToActivateBargainButton() {

        yield return new WaitForSeconds(1f);

        while (DialogueManager.GetInstance().dialogueIsPlaying)
        {

            yield return null;
        }

        isTutoToolsFinished = true;
    }
}
