using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [Header("SEE OR SKIP CUTSCENE ?")]
    [SerializeField] private bool playIntroCutscene = true;

    [Header("Grandpa sprite")]
    [SerializeField] private GameObject grandpa;

    [Header("Managers")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameIntro gameIntro;
    [SerializeField] private LevelOutro levelOutro;
    [SerializeField] private Timer timer;

    [SerializeField] private float maxTime;

    private static GameStateManager instance;

    public bool timerEnded = false;

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

        if (!timerEnded)
        {
            state = "client intro";
            grandpa.SetActive(false);

            DialogueManager.GetInstance().Reset();
            levelManager.LoadNextClient();

        }
        else {
            LoadLevelOutro();
        }
    }

    // to delete (make sure it isnt refered anywhere before)
    public void LoadIntroState() 
    {
        state = "intro";
        DialogueManager.GetInstance().Reset();
        levelManager.LoadNextClient();
    }

    public void LoadInspectState()
    {
        state = "inspect";

        // start timer
        timer.StartTimer( maxTime );

        // move Desk and other stuff for layout change
        levelManager.CreateArtifact();
        // spawn tools as well
    }

    public void LoadBargainState()
    {
        state = "bargain";

        // stop timer
        timer.PauseTimer();

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


    public void LoadLevelOutro() {
        state = "level outro";

        float elapsedTime = timer.CheckTimer();

        Debug.Log("CHECK: elapsed time: " + elapsedTime);

        levelOutro.ShowLevelOutroScreen( elapsedTime );

        timer.ResetTimer();
        timerEnded = false;
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
