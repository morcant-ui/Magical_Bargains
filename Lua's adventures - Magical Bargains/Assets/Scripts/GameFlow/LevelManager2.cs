using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager2 : MonoBehaviour
{
    [Header("Game Data")]
    [SerializeField] private TextAsset levelDataJSON;

    [Header("Assets Spawners")]
    [SerializeField] private ClientSpawner clientSpawner;
    [SerializeField] private ArtifactSpawner artifactSpawner;

    [Header("black screen UI")]
    [SerializeField] private GameObject blackScreen;



    private Queue<ClientData> clientQueue; // this queue will let us iterate thru game data
    private ClientData currentClient; // index to keep track of current client/artifact

    private TextAsset dialogueA;
    private TextAsset dialogueB;

    private string artifactSpriteName;
    private bool hadDefects;

    private string spritePathName = "Sprites";
    private string dialogueAPathName = "Dialogues/dialogueA";
    private string dialogueBPathName = "Dialogues/dialogueB";

    private string menuScene = "SimpleMenu";

    private bool processingClients = true;


    void Start()
    {
        blackScreen.SetActive(false);
        LoadLevelData();
        LoadNextClient();
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

    void LoadLevelData()
    {
        LevelData levelData = JsonUtility.FromJson<LevelData>(levelDataJSON.text);
        clientQueue = new Queue<ClientData>(levelData.clients);
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


        // Load Dialogue B
        string dialogueNameB = currentClient.dialogueB;
        TextAsset dialogueB = Resources.Load<TextAsset>(Path.Combine(dialogueBPathName, dialogueNameB));

        DialogueManager.GetInstance().EnterDialogueMode(dialogueB);
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


