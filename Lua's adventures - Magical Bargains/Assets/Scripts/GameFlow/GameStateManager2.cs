using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager2 : MonoBehaviour
{

    private static double savings = 60.0;

    private string state;

    private static GameStateManager2 instance;

    [SerializeField] private LevelManager2 levelManager;

    private void Awake()
    {

        state = "intro";

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject); // kill extra copies
        
        } else
        {

            instance = this;
        }

        
    }

    public static GameStateManager2 GetInstance() {
        return instance;
    }


    public string GetState() {
        return state;
    }


    public void LoadIntroState() 
    {
        state = "intro";
        DialogueManager.GetInstance().Reset();
        levelManager.LoadNextClient();
        
        //buttonManager.Reset();
    }

    public void LoadInspectState()
    {
        state = "inspect";
        // move Desk and other stuff for layout change
        levelManager.CreateArtifact();
        // spawn tools as well
    }

    public void LoadBargainState()
    {
        state = "bargain";
        // move Desk and stuff for layout change
        //hide tools + show bargaining buttons
        DialogueManager.GetInstance().Reset();
        levelManager.PrepareBargainState();

    }

    public void LoadBargainDoneState()
    {
        state = "bargainDone";
        levelManager.FinishBargainState();
    }


    public double RetrieveMoney(double amount) {
        if (savings >= amount)
        {

            savings -= amount;
            return savings;

        }
        else {
            return savings;
        }

    }

    public double CheckMoney() {
        return savings;
    }
}
