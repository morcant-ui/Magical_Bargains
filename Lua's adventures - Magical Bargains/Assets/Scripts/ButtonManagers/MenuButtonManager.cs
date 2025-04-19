using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtonManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button dialogueSceneButton;
    [SerializeField] private Button tentativeSceneButton;
    [SerializeField] private Button properSceneButton;

    [Header("Scenes")]
    [SerializeField] private string dialogueScene = "TestDialogueMode";
    [SerializeField] private string tentativeScene = "tentative";
    [SerializeField] private string properScene = "ProperScene";



    private void Start() {

        dialogueSceneButton.onClick.AddListener(OnDialogueSceneButtonClick);
        tentativeSceneButton.onClick.AddListener(OnTentativeSceneButtonClick);
        properSceneButton.onClick.AddListener(OnProperSceneButtonClick);
    }


    public void OnDialogueSceneButtonClick() {

        SceneManager.LoadScene(dialogueScene);
    }

    public void OnTentativeSceneButtonClick() {
        SceneManager.LoadScene(tentativeScene);
    }

    public void OnProperSceneButtonClick()
    {
        SceneManager.LoadScene(properScene);
    }
}
