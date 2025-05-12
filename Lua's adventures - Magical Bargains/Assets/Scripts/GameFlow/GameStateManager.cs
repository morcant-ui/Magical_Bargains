using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameObject grandpa;

    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameIntro gameIntro;

    private static GameStateManager instance;

    private bool playIntroCutscene = true;

    private static double savings = 80.0;

    private string state;

    // ALL STATES:
        // "game intro"
            // "level intro"
                // "client intro"
                // "inspect"
                // "bargain"
            // "client outro"
        // "level outro"
    // "game outro"



    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject); // kill extra copies
        
        } else
        {

            instance = this;
        }
    }

    public static GameStateManager GetInstance() {
        return instance;
    }

    private void Start() {
        LoadGameIntro();
    }


    ///////////////////////////////////////////////

    public void LoadGameIntro() {
        state = "game intro";

        levelManager.LoadGameData();

        if (playIntroCutscene)
        {
            gameIntro.ShowIntroCutscene();
        } else {
            LoadLevelIntro();
        }

        
    }

    public void LoadLevelIntro() {
        state = "level intro";
        DialogueManager.GetInstance().Reset();

        grandpa.SetActive(true);
        levelManager.LoadNextLevel();
    }

    public void LoadClientIntro() {
        state = "client intro";
        grandpa.SetActive(false);

        DialogueManager.GetInstance().Reset();
        levelManager.LoadNextClient();
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



    /////////////////////////////////

    public string GetState()
    {
        return state;
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
