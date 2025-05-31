using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;


public class LevelManager : MonoBehaviour
{
    [Header("Data OF each Level")]
    [SerializeField] private TextAsset gameDataJSON;

    [Header("Assets Spawners")]
    [SerializeField] private ClientSpawner clientSpawner;
    [SerializeField] private ArtifactSpawner artifactSpawner;

    [Header("Offers")]
    [SerializeField] private OfferManager offerManager;
    [SerializeField] private DialogueLoader dialogueLoader;

    [Header("Savings Indication")]
    [SerializeField] private GameObject savingsImage;
    [SerializeField] private TextMeshProUGUI savingsText;

    [Header("Transitional blackscreen")]
    [SerializeField] private GameObject blackScreen;

    [Header("WarningNoMoney Graphics")]
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject backgroundColor;
     // message if skipped last client
    public GameObject warningNoMoreMoneyMessage;


    // BOOLEANS
    private bool processingClients = false;
    private bool tutoActivated = false;

    // savings kept track of by state manager
    private double savings;
    // bool to check if it was last client before skip
    private bool lastClientGotSkipped = false;
   
   
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
    private string tutoPathName = Path.Combine("Dialogues", "grandpa", "Tuto");

    // to be deleted
    private Coroutine destroyClientCoroutine;
    private Coroutine endGameCoroutine;

    private Coroutine startBargainTutoCoroutine;


    private string menuScene = "SimpleMenu";

    private bool triedBargaining = false;


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
        // simply pull the list of levels �nto the queue
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
        if (levelQueue.Count == 0)
        {

            if (endGameCoroutine != null) { return; }

            endGameCoroutine = StartCoroutine(LoadGameOutroAfterDelay(0.5f));
            return;
        }

        // 1)
        currentLevel = levelQueue.Dequeue();
        string currentLevelName = currentLevel.listClientsName;
        int currentMaxTime = tutoActivated ? 0 : currentLevel.maxTime;

        Debug.Log("----------TEST: " + currentLevel.dailyExpenses);


        GameStateManager.GetInstance().UpdateMaxTime(currentMaxTime);

        // 2)
        TextAsset clientDataJSON = Resources.Load<TextAsset>(Path.Combine(JSONPathName, currentLevelName));

        ListClients listClients = JsonUtility.FromJson<ListClients>(clientDataJSON.text);
        clientQueue = new Queue<ClientData>(listClients.clients);

        processingClients = true;
        nbProcessedClients = 0;

        // 3)
        

        // if tuto: play tuto and return. another dialogue gets played from state manager normally
        if (tutoActivated) { return; }

        string grandpaDialogueName = currentLevel.grandpaIntroDialogue;
        TextAsset grandpaIntroDialogue = Resources.Load<TextAsset>(Path.Combine(grandpaDialoguesPathName, grandpaDialogueName));
        
        if (lastLevelAppreciation != "") {

            Debug.Log("APPRECIATION:" + lastLevelAppreciation);

            TextAsset grandpaAppreciationDialogue;
            string dialogueName = "";

            if (lastLevelAppreciation == "bankrupt") { dialogueName = "appreciationBankrupt"; }
            if (lastLevelAppreciation == "haunted") { dialogueName = "appreciationHaunted"; }
            if (lastLevelAppreciation == "junk") { dialogueName = "appreciationJunk"; }
            if (lastLevelAppreciation == "bad") { dialogueName = "appreciationBad"; }
            if (lastLevelAppreciation == "ok") { dialogueName = "appreciationOk"; }

            if (dialogueName != "")
            {

                grandpaAppreciationDialogue = Resources.Load<TextAsset>(Path.Combine(grandpaDialoguesPathName, "Appreciations", dialogueName));

                if (grandpaAppreciationDialogue != null) {
                    DialogueManager.GetInstance().EnterDialogueMode(grandpaAppreciationDialogue, grandpaIntroDialogue);
                    return;
                }  
            }
        }

        // if no appreciation text just load this one
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
            if (destroyClientCoroutine != null) { StopCoroutine(destroyClientCoroutine); }
            destroyClientCoroutine = StartCoroutine(DestroyClient(0.5f));
            //DestroyClient(0.5f);
            nbProcessedClients += 1;
        }

        // N) when queue is empty or timer ran out, we go to level outro
        if (clientQueue.Count == 0 || timerEnded)
        {
            if (timerEnded) { Debug.Log("Max timer has ended"); } else { Debug.Log("All clients processed."); }

            double baseDailyExpenses = currentLevel.dailyExpenses;


            processingClients = false;

            currentClient = null;
            currentLevel = null;

            StartCoroutine(NextLevelAfterDelay(0.5f, baseDailyExpenses));
            return;
        }

        // 1)
        currentClient = clientQueue.Dequeue();

        // skip client if too poor, otherwise keep going as normal
        if (SkipClientWhenPoor(currentClient, timerEnded)){
            return;
        }

        // 2)
        CreateClient();

        // 3)
       
        DialogueManager.GetInstance().SetSpeaker(currentClient.clientName, currentClient.clientColor);

        string dialogueName = currentClient.fullDialogue;

        TextAsset dialogue = dialogueLoader.LoadDialogue(dialogueName, "dialogueA");

        string knotName = "a";

        if (tutoActivated) {

            TextAsset tutoDialogue = Resources.Load<TextAsset>(Path.Combine(tutoPathName, "tuto2"));

            DialogueManager.GetInstance().EnterDialogueMode(dialogue, tutoDialogue, knotName);
            return;
        } 
        DialogueManager.GetInstance().EnterDialogueMode(dialogue, null, knotName); 
    }

    // check if I can still afford the goods of the client that will be loaded
    private bool SkipClientWhenPoor(ClientData currClient, bool timerEnded)
    {
        
        int minOfferAccepted = int.Parse(currClient.minOfferAccepted);
        savings = GameStateManager.GetInstance().CheckMoney();
        if (savings < minOfferAccepted)
        {
            // case where you should not have been able to buy the artifact
            if (currClient.offerIsImpossible == "true")
            {
                // continue normally
                return false;
            }
            // case where you don't have enough money
            //skip to next client
            if (clientQueue.Count == 0)  // This was the last client
            {
                Debug.Log($"savings are {savings}");
                lastClientGotSkipped = true;
            }
            LoadNextClient(timerEnded);
            return true;
        }
        else
        {
            // case where you have enough to buy
            return false;
        }
        
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
        string maxSavings = currentOffer;
        if (double.Parse(currentOffer) > savings)
        {

            // Debug.Log("offer can't be bigger");
            maxSavings = $"{savings}";

        }

        if (tutoActivated)
        {

            TextAsset tutoDialogue = Resources.Load<TextAsset>(Path.Combine(tutoPathName, "tuto5"));

            DialogueManager.GetInstance().EnterDialogueMode(tutoDialogue);

            if (startBargainTutoCoroutine != null) { StopCoroutine(startBargainTutoCoroutine); }

            startBargainTutoCoroutine = StartCoroutine(WaitForGrandpaToFinish(currentOffer, maxSavings));

        }
        else
        {
            offerManager.StartBargain(currentOffer, maxSavings);
        } 
                
    }

    // Steps:   1. show the savings
    //          2. retrieve the current offer and give it to offer Manager
    public void BargainAgain()
    {
        // 1)
        savings = GameStateManager.GetInstance().CheckMoney();
        savingsText.text = "$" + savings.ToString("00.00");

        savingsText.color = Color.white;
        savingsImage.SetActive(true);


        // 2)
        string currentOffer = currentClient.artifactOffer;
        string maxSavings = currentOffer;
        if (double.Parse(currentOffer) > savings)
        {

            // Debug.Log("offer can't be bigger");
            maxSavings = $"{savings}";

        }
        offerManager.ChooseAction(currentOffer, maxSavings);
    }

    // Steps:   1. retrieve final offer from offer manager and min offer from the current client
    //          2. determine the outcome of the offer
    //          3. for any outcome, call offer dialogue manager to obtain a working dialogue
    //          4. feed the dialogue to dialogue Manager
    public void FinishBargainState(bool refused)
    {
        // 1)
        int finalOffer = offerManager.FinalOffer;
        string knotName = "";
        string dialogueName = currentClient.fullDialogue;

        if (refused)
        {
            finalOffer = 0;
            // Debug.Log("Offer refused!!!");
        }
        int minOfferAccepted = int.Parse(currentClient.minOfferAccepted);
        TextAsset dialogue;

        // 2)
        if (refused)
        {
            // Outcome E: Lua did not accept the offer
            Debug.Log("Finish Bargain State: Outcome E");

            dialogue = dialogueLoader.LoadDialogue(dialogueName, "dialogueE");
            knotName = "e";
            refused = false;
            // just to be sure
            triedBargaining = false;
        }

        else if (finalOffer >= minOfferAccepted && savings >= finalOffer)
        {
            // Outcome B: offer accepted
            Debug.Log("Finish Bargain State: Outcome B");

            // update savings
            savings = GameStateManager.GetInstance().RetrieveMoney(finalOffer);
            savingsText.text = "$" + savings.ToString("00.00");

            // add new purchase info to list in state manager
            currentClient.finalPrice = finalOffer.ToString();
            GameStateManager.GetInstance().AddToListPurchases(currentClient);

            // 3)
            dialogue = dialogueLoader.LoadDialogue(dialogueName, "dialogueB");
            knotName = "b";
            // just to be sure
            triedBargaining = false;
        }

        else if (finalOffer >= minOfferAccepted && savings < finalOffer)
        {
            // Outcome D: not enough money
            Debug.Log("Finish Bargain State: Outcome D");

            // make savings flicker for some visual cue
            DialogueManager.GetInstance().TriggerSavingsFlickering();

            // 3)
            dialogue = dialogueLoader.LoadDialogue(dialogueName, "dialogueD");
            knotName = "d";
            // just to be sure
            triedBargaining = false;

            // Outcome C: client refuses
        }
        else
        {

            //this is situation of offer not good for client - can still try to bargain
            Debug.Log("Finish Bargain State: Outcome C");
            // all is not lost, try to bargain ! (except for impossibleOffer ok)
            if (currentClient.canBargain != null && triedBargaining == false)
            {
                // Debug.Log("Way to bargain");
                
                dialogue = dialogueLoader.LoadDialogue(dialogueName, "dialogueF");
                knotName = "f";
                triedBargaining = true;

            }
            else
            {
                // Debug.Log("No way you can bargain bro");
                // 3)
                dialogue = dialogueLoader.LoadDialogue(dialogueName, "dialogueC");
                knotName = "c";
                // false again for next client
                triedBargaining = false;
            }


        }

        if (dialogue == null)
        {
            Debug.Log("finishBargainState: dialogue is null");
            SceneManager.LoadScene(menuScene);
            return;
        }

        // 4)
        if (tutoActivated)
        {

            TextAsset tutoDialogue = Resources.Load<TextAsset>(Path.Combine(tutoPathName, "tuto6"));

            DialogueManager.GetInstance().EnterDialogueMode(dialogue, tutoDialogue, knotName);
            return;
        }

        DialogueManager.GetInstance().EnterDialogueMode(dialogue, null, knotName);
    }

    public void FinishBargainStateAgain(string action)
    {
        triedBargaining = false;

        int finalOffer = offerManager.FinalOffer;
        string knotName = "";
        string dialogueName = currentClient.fullDialogue;

        TextAsset dialogue;

        if (currentClient.canBargain.Split(',').Select(s => s.Trim()).Contains(action)) //accepts "thermometer", "thermometer, magnifier" type of strings
        {
            // congrats you found the sus element!
            
            // update savings
            savings = GameStateManager.GetInstance().RetrieveMoney(finalOffer);
            savingsText.text = "$" + savings.ToString("00.00");

            // add new purchase info to list in state manager
            currentClient.finalPrice = finalOffer.ToString();
            GameStateManager.GetInstance().AddToListPurchases(currentClient);

            // 3)
            dialogue = dialogueLoader.LoadDialogue(dialogueName, "dialogueB");
            knotName = "b";
        }
        else if (currentClient.canBargain != "false" && currentClient.canBargain != action)
        {
            // there was a way to bargain but you missed (oups)
            dialogue = dialogueLoader.LoadDialogue(dialogueName, "dialogueG");
            knotName = "g";

        }
        else if (currentClient.canBargain == "false")
        {
            // there was no way to bargain for a decreased offer (but could maybe offer more money)
            dialogue = dialogueLoader.LoadDialogue(dialogueName, "dialogueH");
            knotName = "h";
        }
        else
        {
            dialogue = null;
            //problemm
        }
        
        if (dialogue == null)
        {
            Debug.Log("finishBargainStateAGAIN: dialogue is null");
            SceneManager.LoadScene(menuScene);
            return;
        }
        DialogueManager.GetInstance().EnterDialogueMode(dialogue, null, knotName);
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
        float thermoIntensity = currentClient.thermoIntensity;

        //Color finalThermoColor = ParseColor(thermoColor, thermoIntensity);

        artifactSpawner.SpawnObject(artifactSpriteName, magnifierSpriteName, cameraSpriteName, thermoColor, thermoIntensity);
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

    private IEnumerator DestroyClient(float delay)
    {
        blackScreen.SetActive(true);
        GameObject[] oldObjects = GameObject.FindGameObjectsWithTag("currentClient");

        foreach (GameObject obj in oldObjects) { Destroy(obj); }

        // wait for a few seconds: could use this time for animation maybe instead of blackscreen
        yield return new WaitForSeconds(delay);

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

    IEnumerator NextLevelAfterDelay(float delay, double baseDailyExpenses)
    {
        yield return new WaitForSeconds(delay);

        if (lastClientGotSkipped)
        {
            // Show the warning message before continuing
            holder.SetActive(true); 
            while (!Input.GetKeyDown(KeyCode.Space))
                {
                    yield return null;
                }
            // yield return new WaitForSeconds(4f); // Let the player read it
            holder.SetActive(false);
            lastClientGotSkipped = false;
        }
        blackScreen.SetActive(true);


        if (levelQueue.Count != 0)
        {
           

            blackScreen.SetActive(false);
            GameStateManager.GetInstance().LoadLevelOutro(nbProcessedClients, baseDailyExpenses);
        }
        else
        {
            blackScreen.SetActive(true);
            GameStateManager.GetInstance().LoadLevelIntro();
        }
        
        
    }
    //////////////////////////////////// Next Step to Take ////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator WaitForGrandpaToFinish(string currentOffer, string maxSavings) {
        while (DialogueManager.GetInstance().dialogueIsPlaying)
        {

            yield return null;
        }

        offerManager.StartBargain(currentOffer, maxSavings);
    }


    public void SetTutoStatus(bool isTutoActivated)
    {
        tutoActivated = isTutoActivated;
    }


}


