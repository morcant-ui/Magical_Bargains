using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class LevelManager : MonoBehaviour
{
    [Header("Data OF each Level")]
    [SerializeField] private TextAsset gameDataJSON;

    [Header("Assets Spawners")]
    [SerializeField] private ClientSpawner clientSpawner;
    [SerializeField] private ArtifactSpawner artifactSpawner;

    [Header("Offers")]
    [SerializeField] private OfferManager offerManager;
    [SerializeField] private OfferDecisionManager offerDecision;

    [Header("Savings Indication")]
    [SerializeField] private GameObject savingsImage;
    [SerializeField] private TextMeshProUGUI savingsText;

    [Header("Transitional blackscreen")]
    [SerializeField] private GameObject blackScreen;

    // boolean to keep track of if we're processing a client
    private bool processingClients = false;

    // savings kept track of by state manager
    private double savings;

    // keep track of number of clients processed
    private int nbProcessedClients = 0;

    // queue for List of levels
    private Queue<LevelData> levelQueue;
    private LevelData currentLevel;

    // queue for list of clients per level
    private Queue<ClientData> clientQueue;
    private ClientData currentClient;

    // vars that i keep track of
    // ARTIFACT
    private string artifactSpriteName;
    private bool hadDefects;

    // DIALOGUES
    private TextAsset dialogueA;
    private TextAsset dialogueB;

    // PATH NAMES
    private string JSONPathName = "ClientLists";
    private string spritePathName = "Sprites";
    private string dialogueAPathName = Path.Combine("Dialogues", "dialogueA");
    private string grandpaDialoguesPathName = Path.Combine("Dialogues", "grandpa");

    // to be deleted
    private Coroutine destroyClientCoroutine;
    private Coroutine endGameCoroutine;


    private string menuScene = "SimpleMenu";


    void Start()
    {
        blackScreen.SetActive(false);
    }

    void Update(){ }

    /////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////// Handling lists /////////////////////////////////////////////////

    // THIS NEEDS TO RUN ONCE AT THE BEGINNING OF THE GAME
    public void LoadGameData()
    {
        // simply pull the list of levels înto the queue
        ListLevels listLevels = JsonUtility.FromJson<ListLevels>(gameDataJSON.text);
        levelQueue = new Queue<LevelData>(listLevels.levelData);
        
    }

    // THIS NEEDS TO RUN ONCE FOR EACH LEVEL
    // Steps:   1. pull a new level from the queue
    //          2. pull a new list of clients to go through
    //          3. run grandpa dialogue(s)
    //              3.1 if there is a last level, show grandpa appreciation dialogue
    //              3.2 then show the grandpa dialogue from current level
    //          N. when the level queue is empty we start the game outro process
    public void LoadNextLevel(string lastLevelAppreciation)
    {

        // N) when level queue is empty we enter game outro
        if (levelQueue.Count == 0) {

            if (endGameCoroutine != null) { return; }

            endGameCoroutine = StartCoroutine(LoadGameOutroAfterDelay(0.5f));
            return;
        }

        // 1)
        currentLevel = levelQueue.Dequeue();
        string currentLevelName = currentLevel.listClientsName;
        int currentMaxTime = currentLevel.maxTime;

        GameStateManager.GetInstance().UpdateMaxTime(currentMaxTime);

        // 2)
        TextAsset clientDataJSON = Resources.Load<TextAsset>(Path.Combine(JSONPathName, currentLevelName));

        ListClients listClients = JsonUtility.FromJson<ListClients>(clientDataJSON.text);
        clientQueue = new Queue<ClientData>(listClients.clients);

        processingClients = true;
        nbProcessedClients = 0;

        // 3)
        //TextAsset finalGrandpaDialogue = new TextAsset();

        Debug.Log("Arrived here..");

        string grandpaDialogueName = currentLevel.grandpaIntroDialogue;
        TextAsset grandpaIntroDialogue = Resources.Load<TextAsset>(Path.Combine(grandpaDialoguesPathName, grandpaDialogueName));


        if (lastLevelAppreciation != "") {

            Debug.Log("--- last level appreciation is not null");

            TextAsset grandpaAppreciationDialogue;
            string dialogueName = "";

            if (lastLevelAppreciation == "haunted") { dialogueName = "appreciationHaunted"; }
            if (lastLevelAppreciation == "junk") { dialogueName = "appreciationJunk"; }
            if (lastLevelAppreciation == "bad") { dialogueName = "appreciationbad"; }
            if (lastLevelAppreciation == "ok") { dialogueName = "appreciationOk"; }

            if (dialogueName != "")
            {
                Debug.Log("dialogueName: " + dialogueName);

                grandpaAppreciationDialogue = Resources.Load<TextAsset>(Path.Combine(grandpaDialoguesPathName, dialogueName));

                if (grandpaAppreciationDialogue != null) {
                    DialogueManager.GetInstance().EnterDialogueMode(grandpaAppreciationDialogue, grandpaIntroDialogue);
                    return;
                }
                
                
            }

            Debug.Log("unsuccessful search");

            //finalGrandpaDialogue.text = grandpaAppreciationDialogue.text + grandpaIntroDialogue.text;
            
        }

        //finalGrandpaDialogue = grandpaIntroDialogue;
        Debug.Log("loading just one dialogue");
        DialogueManager.GetInstance().EnterDialogueMode(grandpaIntroDialogue);
    }



    // THIS NEEDS TO RUN ONCE FOR EACH CLIENT FOR EACH LEVEL 
    // Steps:   1. pull a new client from the queue
    //          2. retrieve the sprite and dialogue and make the client spawn
    //          3. retrieve the dialogue and call dialogue manager
    //          4. once we load a new client, we need to destroy the previous one
    //          N. when the client queue is empty OR timer ran out, we start the level outro process
    public void LoadNextClient(bool timerEnded)
    {

        // 4) after every processed client we delete client and reset dialogue
        DialogueManager.GetInstance().Reset();
        if (currentClient != null)
        {
            //if (destroyClientCoroutine != null) { return; }
            //destroyClientCoroutine = StartCoroutine(DestroyAfterDelay(0.5f));
            DestroyClient(0.5f);
            nbProcessedClients += 1;
        }


        // N) when queue is empty or timer ran out, we go to level outro
        if (clientQueue.Count == 0 || timerEnded)
        {
            if (timerEnded) { Debug.Log("Max timer has ended"); } else {  Debug.Log("All clients processed."); }

            processingClients = false;

            currentClient = null;
            currentLevel = null;

            StartCoroutine(NextLevelAfterDelay(0.5f));
            return;
        }

        // 1)
        currentClient = clientQueue.Dequeue();

        // 2)
        CreateClient();

        // 3)
        string dialogueNameA = currentClient.dialogueA;
        dialogueA = Resources.Load<TextAsset>(Path.Combine(dialogueAPathName, dialogueNameA)); 
        DialogueManager.GetInstance().EnterDialogueMode(dialogueA); 
    }

    //////////////////////////////////// Handling lists ///////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////// Handle Bargain State /////////////////////////////////////////////

    // Steps:   1. destroy the artifact
    //          2. show the savings
    //          3. retrieve the current offer and give it to offer Manager
    public void PrepareBargainState()
    {
        // 1)
        DestroyArtifact();

        // 2)
        savings = GameStateManager.GetInstance().CheckMoney();
        savingsText.text = "$" + savings.ToString("00.00");

        savingsText.color = Color.white;
        savingsImage.SetActive(true);

        // 3)
        string currentOffer = currentClient.artifactOffer;
        offerManager.StartBargain(currentOffer);
    }

    // Steps:   1. retrieve final offer from offer manager and min offer from the current client
    //          2. determine the outcome of the offer
    //          3. for any outcome, call offer dialogue manager to obtain a working dialogue
    //          4. feed the dialogue to dialogue Manager
    public void FinishBargainState()
    {   
        // 1)
        int finalOffer = offerManager.FinalOffer;
        int minOfferAccepted = int.Parse(currentClient.minOfferAccepted);
        TextAsset dialogue;

        // 2)
        // Outcome B: offer accepted
        if (finalOffer >= minOfferAccepted && savings >= finalOffer)
        {
            Debug.Log("Finish Bargain State: Outcome B");

            // update savings
            savings = GameStateManager.GetInstance().RetrieveMoney(finalOffer);
            savingsText.text = "$" + savings.ToString("00.00");

            // add new purchase info to list in state manager
            currentClient.finalPrice = finalOffer.ToString();
            GameStateManager.GetInstance().AddToListPurchases(currentClient);

            // 3)
            string dialogueNameB = currentClient.dialogueB;
            dialogue = offerDecision.LoadDialogue(dialogueNameB, "dialogueB");

        // Outcome D: not enough money
        } else if (finalOffer >= minOfferAccepted && savings < finalOffer) {

            Debug.Log("Finish Bargain State: Outcome D");

            // make savings flicker for some visual cue
            DialogueManager.GetInstance().TriggerSavingsFlickering();

            // 3)
            string dialogueNameD = currentClient.dialogueD;
            dialogue = offerDecision.LoadDialogue(dialogueNameD, "dialogueD");

        // Outcome C: client refuses
        } else {

            Debug.Log("Finish Bargain State: Outcome C");

            // 3)
            string dialogueNameC = currentClient.dialogueC;
            dialogue = offerDecision.LoadDialogue(dialogueNameC, "dialogueC");
        }

        if (dialogue == null) {
            Debug.Log("finishBargainState: dialogue is null");
            SceneManager.LoadScene(menuScene);
            return;
        }

        // 4)
        DialogueManager.GetInstance().EnterDialogueMode(dialogue);
    }

    //////////////////////////////////// Handle Bargain State /////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////// Use the Spawners /////////////////////////////////////////////////
    
    public void CreateArtifact()
    {

        string artifactSpriteName = currentClient.artifactSprite;
        string magnifierSpriteName = currentClient.magnifierSprite;
        string cameraSpriteName = currentClient.cameraSprite;
        string thermoColor = currentClient.thermoColor;

        artifactSpawner.SpawnObject(artifactSpriteName, magnifierSpriteName, cameraSpriteName, thermoColor);
    }

    private void CreateClient()
    {
        string clientSpriteName = currentClient.clientSprite;

        clientSpawner.SpawnClient(Path.Combine(spritePathName, clientSpriteName));
    }

    private void DestroyArtifact() 
    {
        GameObject[] oldObjects = GameObject.FindGameObjectsWithTag("currentArtifact");

        foreach (GameObject obj in oldObjects) { Destroy(obj); }
    }

    private void DestroyClient(float delay)
    {
        blackScreen.SetActive(true);
        GameObject[] oldObjects = GameObject.FindGameObjectsWithTag("currentClient");

        foreach (GameObject obj in oldObjects) { Destroy(obj); }

        // wait for a few seconds: could use this time for animation maybe instead of blackscreen
        //yield return new WaitForSeconds(delay);

        if (clientQueue.Count != 0 || processingClients)
        {
            blackScreen.SetActive(false);
        }
    }
    //////////////////////////////////// Use the Spawners /////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////// Next Step to Take ////////////////////////////////////////////////

    
    //private void GoBackToMenuWithoutDelay() { SceneManager.LoadScene(menuScene); }

    IEnumerator LoadGameOutroAfterDelay(float delay)
    {
        blackScreen.SetActive(true);

        yield return new WaitForSeconds(delay);

        GameStateManager.GetInstance().LoadGameOutro();

        blackScreen.SetActive(false);
    }

    IEnumerator NextLevelAfterDelay(float delay)
    {
        blackScreen.SetActive(true);

        yield return new WaitForSeconds(delay);

        if (levelQueue.Count != 0)
        {
            blackScreen.SetActive(false);
            GameStateManager.GetInstance().LoadLevelOutro(nbProcessedClients);
        } 
        else 
        {
            blackScreen.SetActive(true);
            GameStateManager.GetInstance().LoadLevelIntro();
        }
        
        
    }
    //////////////////////////////////// Next Step to Take ////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////

    // helper function to get color from game data
    Color ParseColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out var color);
        return color;
    }

    
}


