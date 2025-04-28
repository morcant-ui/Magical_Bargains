using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Game Data")]
    [SerializeField] private TextAsset levelDataJSON;

    [Header("Assets Spawners")]
    [SerializeField] private ClientSpawner clientSpawner;
    [SerializeField] private ArtifactSpawner artifactSpawner;

    [Header("Button Manager")]
    [SerializeField] private TentativeButtonManager buttonManager;

    [Header("black screen UI")]
    [SerializeField] private GameObject blackScreen;



    private Queue<ClientData> clientQueue; // this queue will let us iterate thru game data
    private ClientData currentClient; // index to keep track of current client/artifact

    private string spritePathName = "Sprites";
    private string dialoguePathName = "Dialogues/dialogueA";

    private bool processingClients = true;


    void Start()
    {
        blackScreen.SetActive(false);
        LoadLevelData();
        LoadNextClient();
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

        if (currentClient != null)
        {

            StartCoroutine(DestroyAfterDelay(0.5f));

        }

        if (clientQueue.Count == 0)
        {
            Debug.Log("All clients processed.");
            processingClients = false;
            return;
        }

        currentClient = clientQueue.Dequeue();

        string clientSpriteName = currentClient.clientSprite;
        string dialogueNameA = currentClient.dialogueA;

        string artifactSpriteName = currentClient.artifactSprite;


        ///// WARNING: THIS CURRENTLY ONLY WORKS BC OLD OBJ ARE DESTROYED BEFORE THE CALL
        ///// TO SPAWNERS: ideally we should wrap all those instructions inside another coroutine
        ///// and write "yield return StartCoroutine(DestroyAfterDelay(0.5f));
        /// (i think... not sure)
        clientSpawner.SpawnClient(Path.Combine(spritePathName, clientSpriteName));
        artifactSpawner.SpawnObject(Path.Combine(spritePathName, artifactSpriteName), "");

        var dialogueA = Resources.Load<TextAsset>(Path.Combine(dialoguePathName, dialogueNameA));
        buttonManager.LoadDialogue(dialogueA);
    }

    Color ParseColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out var color);
        return color;
    }


    IEnumerator DestroyAfterDelay(float delay)
    {
        blackScreen.SetActive(true);



        GameObject[] oldObjects = GameObject.FindGameObjectsWithTag("currentClient");

        foreach (GameObject obj in oldObjects)
        {
            Destroy(obj);
        }

        GameObject[] oldObjects2 = GameObject.FindGameObjectsWithTag("currentArtifact");

        foreach (GameObject obj in oldObjects2)
        {
            Destroy(obj);
        }


        yield return new WaitForSeconds(delay);


        // ugly repeating of code but whatever: its to prevent blackscreen to oscillate
        if (clientQueue.Count != 0 || processingClients)
        {
            blackScreen.SetActive(false);
        }
    }

    IEnumerator GoBackToMenuAfterDelay(float delay)
    {
        
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("SimpleMenu");
    }
}


