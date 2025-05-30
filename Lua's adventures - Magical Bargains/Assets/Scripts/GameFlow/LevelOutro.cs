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
    [SerializeField] private TextMeshProUGUI appreciationTextDisplay;

    [Header("Stats graphics")]
    [SerializeField] private TextMeshProUGUI processedClientsTextDisplay;
    [SerializeField] private TextMeshProUGUI itemsBoughtTextDisplay;
    [SerializeField] private TextMeshProUGUI dailyExpensesTextDisplay;
    [SerializeField] private TextMeshProUGUI totalLossTextDisplay;
    [SerializeField] private TextMeshProUGUI netEarningsDisplay;

    [Header("Actual main outro text (may remove)")]
    [SerializeField] private TextAsset cutsceneTextContent;


    double earnings;

    int hauntedCount;
    int junkCount;
    int nbPurchases;

    private System.Random rand = new System.Random();

    // input to exit cutscene
    private string inputKey = "space";

    // keep track of the state of the scene: mostly to avoid spacebar hijinks
    private bool outroActivated = false;

    // End of level appreciation text:
    private string okTextBase = "Keep doing what you're doing !";
    private string midTextBase = "Are you sure you bought any object today ?";
    private string badTextBase = "You gotta be more careful, your grandpa is getting a little scared !";
    private string junkTextBase = "Be careful not to buy too much junk !";
    private string hauntedTextBase = "That haunted object you bought is gonna cause you some trouble !";

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


    public string ShowLevelOutroScreen( Queue<ClientData> purchases, double initialSavings, int nbProcessedClients, double baseDailyExpenses ) {

        // simplistic cutscene: called from game state manager
        outroActivated = true;

        

        // old statistics:
        //float elapsedMin = Mathf.FloorToInt(elapsedTime / 60);
        //float elapsedSec = Mathf.FloorToInt(elapsedTime % 60);

        double oldSavings = GameStateManager.GetInstance().CheckMoney();

        // CALCULATE EARNINGS
        double earnings = CalculateEarnings(purchases, oldSavings);
        // update earnings
        GameStateManager.GetInstance().EarnMoney(earnings);

        // calculate daily expenses and remove them from savings:
        double dailyExpenses = CalculateDailyExpenses(baseDailyExpenses);
        GameStateManager.GetInstance().RetrieveMoney(dailyExpenses);

        double newSavings = GameStateManager.GetInstance().CheckMoney();

        // set coroutine var to null again so that we can exit safely
        exitCutsceneCoroutine = null;

        // notify dialogue manager that a cutscene is running & not to listen for input
        DialogueManager.GetInstance().CutsceneStarted();

        // display outro graphics
        holder.SetActive(true);
        cutsceneImage.SetActive(true);

 


        // retrieve STATISTICS

        double totalLoss = (initialSavings - oldSavings ) + dailyExpenses;
        double grossEarnings = oldSavings - newSavings;
        double netEarnings = newSavings - initialSavings;

        // absolute value
        if (totalLoss < 0) { totalLoss *= -1;  }
        //if (grossEarnings < 0) { grossEarnings *= -1; }

        // display STATISTICS on screen

        // old statistics:
        //savingsDisplay.text = savingsTextBase + "$" + savings.ToString("00.00");
        // timeElapsed.text = timerTextBase + string.Format("{0:00}:{1:00}", elapsedMin, elapsedSec);
        processedClientsTextDisplay.text = nbProcessedClients.ToString();
        itemsBoughtTextDisplay.text = nbPurchases.ToString();
        dailyExpensesTextDisplay.text = "$" + dailyExpenses.ToString("00.00");
        totalLossTextDisplay.text = "$" + totalLoss.ToString("00.00");
        Debug.Log("GROSS: " + grossEarnings);
        netEarningsDisplay.text = "$" + netEarnings.ToString("0.00"); 
        
        

        // end of level appreciation:
        string textToDisplay = ""; // text to display is the little message at bottom of level outro cutscene
        string appreciation = ""; // appreciation is the string we return from this function to let game state manager know

        if (hauntedCount > 0) { textToDisplay = hauntedTextBase; appreciation = "haunted"; }
        if (junkCount > 1) { textToDisplay = junkTextBase; appreciation = "junk"; }
        if (hauntedCount == 0 && junkCount <= 1) {

            if (netEarnings >= 60.0) { textToDisplay = okTextBase; appreciation = "ok"; }

            else if (netEarnings < 60.0 && netEarnings >= 0.0) { textToDisplay = midTextBase;  appreciation = "bad"; } // ADD MID APPRECIATION ?

            else { textToDisplay = badTextBase; appreciation = "bad"; }


        }

        appreciationTextDisplay.text = textToDisplay;

        Debug.Log("---- INITIAL SAVINGS: " + initialSavings.ToString("00.0") + ", LOSS: " + totalLoss.ToString("00.0") + ", NET EARNING: " + (newSavings - initialSavings).ToString("00.0"));

        return appreciation;
    }


    public double CalculateEarnings(Queue<ClientData> purchases, double savings) {

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
            Debug.Log("--OBJECT BOUGHT: " + p.artifactSprite + ", GRADE: " + grade + ", FINAL PRICE: " + finalPrice.ToString("00.0"));

            double range;
            double max = 0.5;
            double min = 0.5;

            switch (grade)
            {
                case "excellent":
                    min = 2;
                    max = 3;
                    break;
                case "good":
                    min = 0.9;
                    max = 1.8;
                    break;
                case "ok":
                    min = 0.4;
                    max = 1.3;
                    break;
                case "bad":
                    min = 0.5;
                    max = 0.9;
                    break;
                case "haunted":
                    hauntedCount += 1;
                    min = -3.0;
                    max = -1.0;
                    break;
                case "junk":
                    junkCount += 1;
                    double flipCoin = rand.NextDouble();
                    if (flipCoin < 0.5)
                    {
                        Debug.Log("-----HEADS");
                        min = -1.5;
                        max = -0.1;
                    }
                    else {
                        Debug.Log("------TAILS");
                        min = 0.05;
                        max = 0.7;
                    }
                    
                    break;
                default:
                    Debug.Log("Level outro: what why ?");
                    break;
            };

            Debug.Log("MINMAX:" + min + " , " + max);

            range = rand.NextDouble() * (max - min) + min;

            earnings += range * originalPrice;

            Debug.Log("---end of level Calculate Earnings for object: RANGE: " + range.ToString("0.0") + ", EARNED: " + (earnings - prior).ToString("00.0"));
        }

        
        return earnings;
    }

    private double CalculateDailyExpenses(double baseDailyExpenses = 20.0) {

        if (baseDailyExpenses == 20.0) { Debug.Log("----------default daily expense value"); }

        double min = 0.1;
        double max = 1.5;

        double range = rand.NextDouble() * (max - min) + min;

        Debug.Log("DAILY EXPENSES :" + baseDailyExpenses + ", RANGE: " + range);

        return range * baseDailyExpenses;
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
        appreciationTextDisplay.text = "";
        processedClientsTextDisplay.text = "";
        itemsBoughtTextDisplay.text = "";
        totalLossTextDisplay.text = "";

        holder.SetActive(false);
    }
}
