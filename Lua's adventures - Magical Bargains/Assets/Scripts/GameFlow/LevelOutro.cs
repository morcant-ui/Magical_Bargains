using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

using Ink.Runtime;

public class LevelOutro : MonoBehaviour
{
    [Header("Outro Graphics")]
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject backgroundColor;
    [SerializeField] private GameObject cutsceneImage;
    [SerializeField] private TextMeshProUGUI cutsceneTextDisplay;

    [Header("Stats graphics")]
    [SerializeField] private TextMeshProUGUI savingsTextDisplay;
    [SerializeField] private TextMeshProUGUI timerTextDisplay;

    [Header("Actual main outro text (may remove)")]
    [SerializeField] private TextAsset cutsceneTextContent;


    double earnings;

    int hauntedCount;
    int junkCount;
    int nbPurchases;

    // input to exit cutscene
    private string inputKey = "space";

    // keep track of the state of the scene: mostly to avoid spacebar hijinks
    private bool outroActivated = false;

    // Displayed Statistics: text base without the stat
    private string savingsTextBase = "Savings: ";
    private string timerTextBase = "Elapsed Investigation Time: ";

    // to exit we store the coroutine in this var to make sure it happens only once
    private Coroutine exitCutsceneCoroutine;


    void Update() {
        if (outroActivated && Input.GetKeyDown(inputKey)) {

            // when we press input it marks the end of the cutscene

            // make sure the coroutine runs only once per end of level
            // ( for that to happen multiple times, make sure to set var to null once you start the next coroutine )
            if (exitCutsceneCoroutine != null) {
                return;
            }

            exitCutsceneCoroutine = StartCoroutine(NewLevelAfterDelay(0.5f));
        }
    }


    public void ShowLevelOutroScreen( Queue<ClientData> purchases, float elapsedTime ) {

        // simplistic cutscene: called from game state manager
        outroActivated = true;

        double savings = GameStateManager.GetInstance().CheckMoney();

        earnings = CalculateEarnings(purchases, savings);

        savings = GameStateManager.GetInstance().CheckMoney();
        Debug.Log("EARNINGS: " + earnings + "jkCount: " + junkCount + "hntCount: " + hauntedCount + "nb purchase: " + nbPurchases);

        // set coroutine var to null again so that we can exit safely
        exitCutsceneCoroutine = null;

        // notify dialogue manager that a cutscene is running & not to listen for input
        DialogueManager.GetInstance().CutsceneStarted();

        // display outro graphics
        holder.SetActive(true);
        cutsceneImage.SetActive(true);

        // STATISTICS: retrieve savings and display them on screen
        

        savingsTextDisplay.text = savingsTextBase + "$" + savings.ToString("00.00");

        // We retrieve elapsed time as argument from state manager: display that (MAY BE DELETED)
        float elapsedMin = Mathf.FloorToInt(elapsedTime / 60);
        float elapsedSec = Mathf.FloorToInt(elapsedTime % 60);

        timerTextDisplay.text = timerTextBase + string.Format("{0:00}:{1:00}", elapsedMin, elapsedSec);

        // Retrieve the cutscene main text and display it directly (MAY BE DELETED)
        Story story = new Story(cutsceneTextContent.text);

        cutsceneTextDisplay.text = story.Continue(); ; 
    }


    private double CalculateEarnings(Queue<ClientData> purchases, double savings) {

        earnings = 0.0;

        hauntedCount = 0;
        junkCount = 0;

        nbPurchases = purchases.Count;

        while (purchases.Count != 0)
        {
            ClientData p = purchases.Dequeue();

            string grade = p.grade;
            double originalPrice = Convert.ToDouble(p.artifactOffer);
            double finalPrice = Convert.ToDouble(p.finalPrice);

            double prior = earnings;
            Debug.Log("OBJECT: " + p.artifactSprite + ", GRADE: " + grade + ", FINAL PRICE: " + finalPrice);

            switch (grade)
            {
                case "excellent":
                    earnings += 3 * (originalPrice);
                    break;
                case "good":
                    earnings += 1.8 * (originalPrice);
                    break;
                case "ok":
                    earnings += 1.2 * (originalPrice);
                    break;
                case "bad":
                    earnings += 0.8 * (originalPrice);
                    break;
                case "haunted":
                    hauntedCount += 1;
                    earnings -= 3 * (originalPrice);
                    break;
                case "junk":
                    junkCount += 1;
                    earnings += 0.2 * (originalPrice);
                    break;
                default:
                    Debug.Log("Level outro: what why ?");
                    break;
            };

            Debug.Log("EARNED: " + (earnings - prior));
        }

        // update earnings
        GameStateManager.GetInstance().EarnMoney(earnings);
        return earnings;
    }

    IEnumerator NewLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // update the boolean
        outroActivated = false;

        // notify dialogue manager that we're done
        DialogueManager.GetInstance().CutsceneStopped();

        // notigy state manager too
        GameStateManager.GetInstance().LoadLevelIntro();

        // hide the cutscene graphics and reset text content
        cutsceneImage.SetActive(false);
        cutsceneTextDisplay.text = "";
        savingsTextDisplay.text = "";
        timerTextDisplay.text = "";

        holder.SetActive(false);
    }
}
