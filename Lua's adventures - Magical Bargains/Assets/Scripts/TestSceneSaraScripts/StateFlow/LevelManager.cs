using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public TextAsset levelDataJSON;

    public ClientSpawner clientSpawner;
    public ArtifactSpawner artifactSpawner;

    public GameObject blackScreen;
    private bool finished;

    private Queue<ClientData> clientQueue;
    private ClientData currentClient;


    void Start()
    {
        blackScreen.SetActive(false);
        LoadLevelData();
        LoadNextClient();
    }

    void Update() {
        if (finished) {
            blackScreen.SetActive(true);
        }
    }

    void LoadLevelData()
    {
        LevelData levelData = JsonUtility.FromJson<LevelData>(levelDataJSON.text);
        clientQueue = new Queue<ClientData>(levelData.clients);
    }

    public void LoadNextClient()
    {

        if (currentClient != null) {
            Debug.Log("Helloo");
            
            StartCoroutine(DestroyAfterDelay(0.5f));
            
        }

        if (clientQueue.Count == 0)
        {
            Debug.Log("All clients processed.");
            finished = true;
            return;
        }

        currentClient = clientQueue.Dequeue();

        Color clientColor = ParseColor(currentClient.clientColor);
        Color objectColor = ParseColor(currentClient.objectColor);
        

        ///// WARNING: THIS CURRENTLY ONLY WORKS BC OLD OBJ ARE DESTROYED BEFORE THE CALL
        ///// TO SPAWNERS: ideally we should wrap all those instructions inside another coroutine
        ///// and write "yield return StartCoroutine(DestroyAfterDelay(0.5f));
        /// (i think... not sure)
        clientSpawner.SpawnClient(clientColor);
        artifactSpawner.SpawnObject(objectColor, currentClient.hasDefects);

        
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
        yield return new WaitForSeconds(delay);

        
        blackScreen.SetActive(false);
    }
}


