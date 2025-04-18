using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{

    private string state;

    private static GameStateManager instance;

    private void Awake()
    {



        if (instance != null && instance != this)
        {
            Destroy(this.gameObject); // kill extra copies
        
        } else
        {

            instance = this;
            DontDestroyOnLoad(this.gameObject); // keep across scenes
        }

        
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(state))
        {

            // THE FOLLOWING USE OF SCENE MANAGEMENT LIBRARY IS ONLY FOR TESTING STUFF, 
            // WE SHOULD NOT BE CREATING AN INSTANCE RN FROM INSPECTION SCENE
            string currentSceneName = SceneManager.GetActiveScene().name;
            Debug.Log("Current scene: " + currentSceneName);

            if (string.Equals(currentSceneName, "TestDialogueMode")) {
                state = "dialogue1"; // Default state
            } else if (string.Equals(currentSceneName, "TestInspectMode")) {
                state = "examination";
            }
            
        }
    }

    public static GameStateManager GetInstance() {
        return instance;
    }

    public void SetState(string str) {
        state = str;
        Debug.Log("State set to: " + state);
    }

    public string GetState() {
        return state;
    }
}
