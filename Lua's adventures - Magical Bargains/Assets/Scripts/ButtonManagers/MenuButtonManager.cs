using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtonManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button properSceneButton;

    [Header("Scenes")]
    [SerializeField] private string properScene = "MainScene";



    private void Start() {
        properSceneButton.onClick.AddListener(OnProperSceneButtonClick);
    }

    public void OnProperSceneButtonClick()
    {
        SceneManager.LoadScene(properScene);
    }
}
