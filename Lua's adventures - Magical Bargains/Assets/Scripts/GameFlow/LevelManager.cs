using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class LevelManager : MonoBehaviour
{
    [Header("Game Data")]
    [SerializeField] private TextAsset gameDataJSON;

    [Header("Assets Spawners")]
    [SerializeField] private ClientSpawner clientSpawner;
    [SerializeField] private ArtifactSpawner artifactSpawner;

    [Header("black screen UI")]
    [SerializeField] private GameObject blackScreen;

    [Header("Offers")]
    [SerializeField] private OfferManager offerManager;

    [Header("Savings Indication")]
    [SerializeField] private GameObject savingsImage;
    [SerializeField] private TextMeshProUGUI savingsText;

    private double savings;

    private Queue<LevelData> levelQueue; // this queue will let us iterate thru game data
    private LevelData currentLevel; // index to keep track of current client/artifact

    private Queue<ClientData> clientQueue; // this queue will let us iterate thru game data
    private ClientData currentClient; // index to keep track of current client/artifact

    private TextAsset dialogueA;
    private TextAsset dialogueB;

    private string artifactSpriteName;
    private bool hadDefects;

    private string JSONPathName = "JSON";

    private string spritePathName = "Sprites";
    private string dialogueAPathName = "Dialogues/dialogueA";
    private string dialogueBPathName = "Dialogues/dialogueB";
    private string dialogueCPathName = "Dialogues/dialogueC";

    private string menuScene = "SimpleMenu";

    private bool processingClients = true;

    void Start()
    {
        blackScreen.SetActive(false);
        //LoadGameData();
        //LoadLevelIntro();
        //GameStateManager.GetInstance().LoadIntroState();
    }

    void Update()
    {
        if (!processingClients)
        {
            blackScreen.SetActive(true);
            StartCoroutine(GoBackToMenuAfterDelay(0.5f));
        }
    }

    void LoadGameData()
    {
        ListLevels listLevels = JsonUtility.FromJson<ListLevels>(gameDataJSON.text);
        levelQueue = new Queue<LevelData>(listLevels.levelData);

        currentLevel = levelQueue.Dequeue();
    }

    public void LoadLevelIntro()
    {
        //string currentLevelName = currentLevel.listClientsName;
        //TextAsset clientDataJSON = Resources.Load<TextAsset>(Path.Combine(JSONPathName, currentLevelName))

        ListClients listClients = JsonUtility.FromJson<ListClients>(gameDataJSON.text);
        clientQueue = new Queue<ClientData>(listClients.clients);
        LoadNextClient();
    }


    public void LoadNextClient()
    {

        // after every processed client we destroy them and reset dialogueManager
        if (currentClient != null)
        {
            StartCoroutine(DestroyAfterDelay(0.5f));
        }

        // we now check the queue (not dependent on coroutine above): if empty we'll put up a black screen
        if (clientQueue.Count == 0)
        {
            Debug.Log("All clients processed.");
            processingClients = false;
            return;
        }

        // if not done: pop the queue
        currentClient = clientQueue.Dequeue();

        // retreive necessary information from game data
        string clientSpriteName = currentClient.clientSprite;
        string dialogueNameA = currentClient.dialogueA;



        //Color objectColor = ParseColor(currentClient.objectColor);

        // now we call the client and artifact spawner feeding them necessary information
        ///// WARNING: THIS CURRENTLY ONLY WORKS BC OLD OBJ ARE DESTROYED BEFORE THE CALL
        ///// TO SPAWNERS: ideally we should wrap all those instructions inside another coroutine
        ///// and write "yield return StartCoroutine(DestroyAfterDelay(0.5f));
        /// (i think... not sure)

        clientSpawner.SpawnClient(Path.Combine(spritePathName, clientSpriteName));
        

        // retrieve the dialogue from Resources
        dialogueA = Resources.Load<TextAsset>(Path.Combine(dialogueAPathName, dialogueNameA));

        // now we have a new client on our hand, we should load the new Dialogue
        DialogueManager.GetInstance().EnterDialogueMode(dialogueA); // dia not playing and dia not finished
    }

    public void CreateArtifact() {

        string artifactSpriteName = currentClient.artifactSprite;
        string magnifierSpriteName = currentClient.magnifierSprite;
        string cameraSpriteName = currentClient.cameraSprite;
        string thermoColor = currentClient.thermoColor;
        
        //bool isThermostatDefective = currentClient.isThermostatDefective;
        //bool isCameraDefective = currentClient.isCameraDefective;

        artifactSpawner.SpawnObject(artifactSpriteName, magnifierSpriteName, cameraSpriteName, thermoColor);
    }

    public void PrepareBargainState()
    {
        // Destroy Artifact
        GameObject[] oldObjects = GameObject.FindGameObjectsWithTag("currentArtifact");

        foreach (GameObject obj in oldObjects)
        {
            Destroy(obj);
        }


        savings = GameStateManager.GetInstance().CheckMoney();
        savingsText.text = "$" + savings.ToString("00.00");

        savingsImage.SetActive(true);

        string currentOffer = currentClient.artifactOffer;
        Debug.Log(currentOffer);
        offerManager.StartBargain(currentOffer);
    }

    public void FinishBargainState()
    {   
        int finalOffer = offerManager.FinalOffer;
        int minOfferAccepted = int.Parse(currentClient.minOfferAccepted);

        if (finalOffer >= minOfferAccepted && savings >= finalOffer)
        {
            Debug.Log("offer went through !");

            savings = GameStateManager.GetInstance().RetrieveMoney(finalOffer);
            savingsText.text = "$" + savings.ToString("00.00");

            // Load Dialogue B
            string dialogueNameB = currentClient.dialogueB;
            TextAsset dialogueB = Resources.Load<TextAsset>(Path.Combine(dialogueBPathName, dialogueNameB));

            DialogueManager.GetInstance().EnterDialogueMode(dialogueB);

        } else if (finalOffer >= minOfferAccepted && savings < finalOffer) {
            // if this feels order of priority feels weird we can remove the final offer >= min offer condition above to prioritize them bashing you for having no money
            Debug.Log("You are trying to retrieve " + finalOffer + " gold, but you only have " + savings + " in grandpa's bank account !");

            savings = GameStateManager.GetInstance().RetrieveMoney(finalOffer);
            savingsText.text = "$" + savings.ToString("00.00");

            TextAsset NotEnoughMoneyDialogue = Resources.Load<TextAsset>(Path.Combine("Dialogues", "NotEnoughMoney"));

            DialogueManager.GetInstance().EnterDialogueMode(NotEnoughMoneyDialogue);
        }
        else
        {
            Debug.Log("HUM OFFER IS NOT ACCEPTED");
            // Load Dialogue C
            string dialogueNameC = currentClient.dialogueC;
            TextAsset dialogueC = Resources.Load<TextAsset>(Path.Combine(dialogueCPathName, dialogueNameC));

            DialogueManager.GetInstance().EnterDialogueMode(dialogueC);
        }

    }

    // destroy a client and artifact
    IEnumerator DestroyAfterDelay(float delay)
    {

        blackScreen.SetActive(true);

        // to destroy them i use the "delete all object with tag" strategy
        GameObject[] oldObjects = GameObject.FindGameObjectsWithTag("currentClient");

        foreach (GameObject obj in oldObjects)
        {
            Destroy(obj);
        }

        // wait for a few seconds: could use this time for animation maybe instead of blackscreen
        yield return new WaitForSeconds(delay);


        // ugly repeating of code but whatever: its to prevent blackscreen to oscillate at the very end
        if (clientQueue.Count != 0 || processingClients)
        {
            blackScreen.SetActive(false);
        }
    }

    // once client queue is done: blackscreen for a few seconds then go back to menu
    IEnumerator GoBackToMenuAfterDelay(float delay)
    {
        
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(menuScene);
    }







    // helper function to get color from game data
    Color ParseColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out var color);
        return color;
    }
}


