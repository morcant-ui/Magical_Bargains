using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.EventSystems;

public class GameStateManager : MonoBehaviour
{
    [Header("SEE OR SKIP CUTSCENE ?")]
    [SerializeField] private bool playIntroCutscene = true;

    [Header("SKIP TUTO ?")]
    [SerializeField] public bool tutoActivated = true;

    [Header("Grandpa sprite")]
    [SerializeField] private GameObject grandpa;


    [Header("Managers")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private ButtonManager buttonManager;
    [SerializeField] private GameIntro gameIntro;
    [SerializeField] private LevelOutro levelOutro;
    [SerializeField] private GameOutro gameOutro;
    [SerializeField] private Timer timer;

    [Header("Music")]
    [SerializeField] private AudioClip gameIntroMusic;
    [SerializeField] private AudioClip levelOutroMusic;
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private AudioClip gameOutroMusic;
    [SerializeField] private float volume = 0.6f;

    private float maxTime = -1;

    private Queue<ClientData> currentPurchases = new Queue<ClientData>();

    private static double savings = 99.0; // static vars cannot show in inspector

    private double initialSavings;

    private string lastAppreciation = "";

    private bool timerEnded = false;

    private string tutoPathName = Path.Combine("Dialogues", "grandpa", "Tuto");

    private Coroutine tutoWaitsForClient;
    private Coroutine tutoWaitForGrandpa;
    private Coroutine tutoWaitForBargain;

    private static GameStateManager instance;


    

    private string state;

    // ALL STATES:
        // "game intro"
            // "level intro"
                // "client intro"
                // "inspect"
                // "bargain"
                // "bargainAGAIN"
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

        savings = 300.0;
        currentPurchases = new Queue<ClientData>();

        LoadGameIntro();
    }


    ///////////////////////////////////////////////

    public void LoadGameIntro() {
        state = "game intro";

        // start playing the game intro music

        AudioManager.GetInstance().SetMusicVolume(volume);

        AudioManager.GetInstance().StartMusic(gameIntroMusic, true);

        levelManager.LoadGameData();
        levelManager.SetTutoStatus(tutoActivated);
        buttonManager.SetTutoStatus(tutoActivated);

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

        // set initial savings for start of level
        initialSavings = savings;

        // start playing the level music
        AudioManager.GetInstance().StartMusic(levelMusic, true);

        grandpa.SetActive(true);

        if (tutoActivated) { ReadTutoDialogue("tuto1"); }
        levelManager.LoadNextLevel(lastAppreciation); // we send last level's evaluation to influence grandpa dialogue
    }

    public void LoadClientIntro() {

        state = "client intro";


        if (!tutoActivated)
        {
            grandpa.SetActive(false);
        }

        DialogueManager.GetInstance().Reset();
        levelManager.LoadNextClient( timerEnded );



    }

    

    public void LoadInspectState()
    {
        state = "inspect";

        DialogueManager.GetInstance().Reset();

        if (!tutoActivated)
        {

            // start timer
            timer.StartTimer(maxTime);
            // spawn tools as well
            levelManager.CreateArtifact();
            
        } else {

            ReadTutoDialogue("tuto3");

            if (tutoWaitForGrandpa != null) { StopCoroutine(tutoWaitForGrandpa); }

            tutoWaitForGrandpa = StartCoroutine(WaitForGrandpaToFinish());
        
        }
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

        EventSystem.current.SetSelectedGameObject(null);

    }

    public void LoadBargainAGAINState()
    {
        state = "bargainAGAIN";

        //stop timer
        timer.PauseTimer();

        //move Desk and stuff for layout change 
        // hide tools + show bargaining buttons
        DialogueManager.GetInstance().Reset();
        levelManager.BargainAgain();
    }

    public void LoadSecondBargainDoneState(string action)
    {
        state = "client outro";
        levelManager.FinishBargainStateAgain(action);
    }

    public void LoadBargainDoneState(bool refused)
    {
        state = "client outro";
        levelManager.FinishBargainState(refused);
        refused = false;

        if (tutoActivated)
        {

            tutoActivated = false;
            levelManager.SetTutoStatus(tutoActivated);
            buttonManager.SetTutoStatus(tutoActivated);
        }
    }


    public void LoadLevelOutro(int nbProcessedClients, double baseDailyExpenses) {
        state = "level outro";

        // stop music
        AudioManager.GetInstance().StopMusic();
        // start playing the level outro music
        AudioManager.GetInstance().StartMusic(levelOutroMusic);

        float elapsedTime = timer.CheckTimer();

        lastAppreciation = levelOutro.ShowLevelOutroScreen( currentPurchases, initialSavings, nbProcessedClients, baseDailyExpenses );

        timer.ResetTimer();
        //timer.ResetBackGround();
        timerEnded = false;
    }

    public void LoadGameOutro()
    {
        AudioManager.GetInstance().StartMusic(gameOutroMusic, true);
        DialogueManager.GetInstance().Reset();

        savings += levelOutro.CalculateEarnings(currentPurchases, savings);

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


    public void UpdateMaxTime(int newMaxTime)
    {

        maxTime = newMaxTime;
    }


    public void ReadTutoDialogue(string dialogueName)
    {

        TextAsset dialogue = Resources.Load<TextAsset>(Path.Combine(tutoPathName, dialogueName));
        DialogueManager.GetInstance().EnterDialogueMode(dialogue);

    }


    private IEnumerator WaitForGrandpaToFinish() {


        while (!DialogueManager.GetInstance().dialogueIsFinished) {

            yield return null;
        }
        //yield return new WaitUntil();

        timer.StartTimer(maxTime);
        levelManager.CreateArtifact();
    }
}
