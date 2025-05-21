using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Ink.Runtime;

public class LevelOutro : MonoBehaviour
{

    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject backgroundColor;
    [SerializeField] private GameObject cutsceneImage;
    [SerializeField] private TextMeshProUGUI cutsceneTextDisplay;
    [SerializeField] private TextAsset cutsceneTextContent;

    [SerializeField] private TextMeshProUGUI savingsTextDisplay;
    [SerializeField] private TextMeshProUGUI timerTextDisplay;

    private string inputKey = "space";

    private bool outroActivated = false;

    private string savingsTextBase = "Savings: ";
    private string timerTextBase = "Elapsed Investigation Time: ";

    private Coroutine exitCutsceneCoroutine;

    void Update() {
        if (outroActivated && Input.GetKeyDown(inputKey)) {

            cutsceneImage.SetActive(false);
            cutsceneTextDisplay.text = "";
            savingsTextDisplay.text = "";
            timerTextDisplay.text = "";

            outroActivated = false;

            if (exitCutsceneCoroutine != null) {
                return;
            }

            exitCutsceneCoroutine = StartCoroutine(NewLevelAfterDelay(0.5f));
        }
    }


    // simplistic stuff for now:
    public void ShowLevelOutroScreen(float elapsedTime) {

        exitCutsceneCoroutine = null;

        outroActivated = true;
        holder.SetActive(true);
        cutsceneImage.SetActive(true);

        DialogueManager.GetInstance().CutsceneStarted();

        double savings = GameStateManager.GetInstance().CheckMoney();

        savingsTextDisplay.text = savingsTextBase + "$" + savings.ToString("00.00");

        float elapsedMin = Mathf.FloorToInt(elapsedTime / 60);
        float elapsedSec = Mathf.FloorToInt(elapsedTime % 60);
        timerTextDisplay.text = timerTextBase + string.Format("{0:00}:{1:00}", elapsedMin, elapsedSec);

        Story story = new Story(cutsceneTextContent.text);

        cutsceneTextDisplay.text = story.Continue(); ; 
    }

    // bc using spacebar it will already take note of it for grandpa's first line 
    // THIS IS NOT A PROBLEM IF WE USE A BUTTON INSTEAD OF SPACE !
    IEnumerator NewLevelAfterDelay(float delay)
    {

        Debug.Log("DO WE ENTER HERE");

        yield return new WaitForSeconds(delay);
        holder.SetActive(false);

        DialogueManager.GetInstance().CutsceneStopped();
        GameStateManager.GetInstance().LoadLevelIntro();
    }
}
