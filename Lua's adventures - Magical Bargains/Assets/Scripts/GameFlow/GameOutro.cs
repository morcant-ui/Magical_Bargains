using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class GameOutro : MonoBehaviour
{
    [Header("Outro Graphics")]
    [SerializeField] private GameObject holder;
    [SerializeField] private GameObject backgroundColor;
    [SerializeField] private GameObject cutsceneImage;
    [SerializeField] private TextMeshProUGUI cutsceneTextDisplay;


    private string inputKey = "space";

    private TextAsset textEnding;

    private bool outroActivated = false;

    private string outroPath = System.IO.Path.Combine("Cutscene", "gameOutro");

    private Coroutine exitCutsceneCoroutine;

    private string menuScene = "SimpleMenu";


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (outroActivated && Input.GetKeyDown(inputKey)) {

            if (exitCutsceneCoroutine != null) {
                return;
            }

            exitCutsceneCoroutine = StartCoroutine(StopGameAfterDelay(0.5f));
        }
        
    }


    public void ShowGameOutro(double savings, double finalEarnings) {

        outroActivated = true;

        string appreciation;

        if (finalEarnings >= 100 || savings >= 300){ appreciation = "excellent"; }

        else if (savings >= 50 || savings >= 150){ appreciation = "good"; }

        else if (savings > 0 || savings >= 50){ appreciation = "ok"; }

        else if (savings >= 50){ appreciation = "bad"; }

        //else if (savings < 50 && savings > 0){ appreciation = "terrible"; }

        else { appreciation = "bankrupt"; }


        Debug.Log("---- SAVINGS: " + savings + ", appreciation: " + appreciation);

        DialogueManager.GetInstance().CutsceneStarted();

        string cardFileName = appreciation + "Card";
        string textFileName = appreciation + "Text";

        Sprite spr = Resources.Load<Sprite>(System.IO.Path.Combine(outroPath, cardFileName));
        cutsceneImage.GetComponent<Image>().sprite = spr;

        holder.SetActive(true);
        cutsceneImage.SetActive(true);

        TextAsset textAsset = Resources.Load<TextAsset>(System.IO.Path.Combine(outroPath, textFileName));

        Story story = new Story(textAsset.text);

        cutsceneTextDisplay.text = story.Continue();
    }



    private IEnumerator StopGameAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);

        outroActivated = false;

        DialogueManager.GetInstance().CutsceneStopped();

        cutsceneImage.SetActive(false);
        cutsceneTextDisplay.text = "";
        //holder.SetActive(false);

        SceneManager.LoadScene(menuScene);
        //Application.Quit();
    }
}
