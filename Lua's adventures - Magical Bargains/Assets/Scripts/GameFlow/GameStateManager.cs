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
    [SerializeField] private GameOutro gameOutro;
    [SerializeField] private Timer timer;

    [Header("Music")]
    [SerializeField] private AudioClip gameIntroMusic;
    [SerializeField] private AudioClip levelOutroMusic;
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private float volume = 0.6f;
    [SerializeField] private float introVolume = 0.1f;

    [Header("Fixed Game Parameters")]
    [SerializeField] private float maxTime;

    private Queue<ClientData> currentPurchases = new Queue<ClientData>();

    private static double savings = 80.0; // static vars cannot show in inspector

    private bool timerEnded = false;

    private static GameStateManager instance;

    

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

        savings = 80.0;
        currentPurchases = new Queue<ClientData>();

        LoadGameIntro();
    }


    ///////////////////////////////////////////////

    public void LoadGameIntro() {
        state = "game intro";

        // start playing the game intro music
        AudioManager.GetInstance().StartMusic(gameIntroMusic, introVolume);

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

        // start playing the level music
        AudioManager.GetInstance().StartMusic(levelMusic, volume);

        grandpa.SetActive(true);
        levelManager.LoadNextLevel();
    }

    public void LoadClientIntro() {

            state = "client intro";

            grandpa.SetActive(false);

            

            DialogueManager.GetInstance().Reset();
            levelManager.LoadNextClient( timerEnded );

    }

    // to delete (make sure it isnt refered anywhere before)
    public void LoadIntroState() 
    {
        state = "intro";
        DialogueManager.GetInstance().Reset();
        levelManager.LoadNextClient( timerEnded );
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
        state = "client outro";
        levelManager.FinishBargainState();
    }


    public void LoadLevelOutro() {
        state = "level outro";

        // stop music
        AudioManager.GetInstance().StopMusic();
        // start playing the level outro music
        AudioManager.GetInstance().StartMusic(levelOutroMusic, volume);

        float elapsedTime = timer.CheckTimer();

        levelOutro.ShowLevelOutroScreen( currentPurchases, elapsedTime );

        timer.ResetTimer();
        //timer.ResetBackGround();
        timerEnded = false;
    }

    public void LoadGameOutro()
    {
        DialogueManager.GetInstance().Reset();

        levelOutro.CalculateEarnings(currentPurchases, savings);

        gameOutro.ShowGameOutro(savings);

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

    public double EarnMoney(double amount) {
        savings += amount;
        return savings;
    }

    public double CheckMoney() {
        return savings;
    }


    public void AddToListPurchases(ClientData newPurchase) {
        currentPurchases.Enqueue(newPurchase);
    }

    public void ResetListPurchases() { currentPurchases = null; }

    public Queue<ClientData> GetListPurchases() {
        return currentPurchases;
    }


    public void setTimerEnded(bool te) {
        timerEnded = te;
    }

    public bool WillIntroPlay() {
        return playIntroCutscene;
    }
}
