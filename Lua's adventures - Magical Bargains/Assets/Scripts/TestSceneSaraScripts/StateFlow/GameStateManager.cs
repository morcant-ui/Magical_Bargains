using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{

    public string state = "dialogue1";

    private static GameStateManager instance;

    private void Start() {
        if (instance != null) {
            Debug.LogWarning("Found more than one instance of Game State Manager !");
        }
        instance = this;
    }

    public static GameStateManager GetInstance() {
        return instance;
    }

    public void SetState(string str) {
        state = str;
        Debug.Log("state:");
        Debug.Log(state);
    }

    public string GetState() {
        return state;
    }
}
