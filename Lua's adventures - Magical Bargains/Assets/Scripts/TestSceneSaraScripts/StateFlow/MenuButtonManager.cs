using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtonManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button startButton;

    [Header("Scenes")]
    [SerializeField] private string firstScene = "TestDialogueMode";



    private void Start() {

        startButton.onClick.AddListener(OnStartButtonClick);
    }


    public void OnStartButtonClick() {

        SceneManager.LoadScene(firstScene);
    }
}
