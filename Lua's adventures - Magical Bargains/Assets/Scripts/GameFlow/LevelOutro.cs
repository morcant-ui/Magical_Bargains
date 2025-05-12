using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Ink.Runtime;

public class LevelOutro : MonoBehaviour
{

    [SerializeField] private GameObject backgroundColor;
    [SerializeField] private GameObject cutsceneImage;
    [SerializeField] private TextMeshProUGUI cutsceneTextDisplay;
    [SerializeField] private TextAsset cutsceneTextContent;

    private string inputKey = "space";

    private bool outroActivated = false;

    void Update() {
        if (outroActivated && Input.GetKeyDown(inputKey)) {
            
            cutsceneImage.SetActive(false);
            cutsceneTextDisplay.text = "";

            outroActivated = false;
            StartCoroutine(NewLevelAfterDelay(0.5f));
        }
    }


    // simplistic stuff for now:
    public void ShowLevelOutroScreen() {
        outroActivated = true;
        backgroundColor.SetActive(true);
        cutsceneImage.SetActive(true);
        Story story = new Story(cutsceneTextContent.text);

        cutsceneTextDisplay.text = story.Continue(); ; 
    }

    // bc using spacebar it will already take note of it for grandpa's first line 
    // THIS IS NOT A PROBLEM IF WE USE A BUTTON INSTEAD OF SPACE !
    IEnumerator NewLevelAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);
        backgroundColor.SetActive(false);

        GameStateManager.GetInstance().LoadLevelIntro();
    }
}
