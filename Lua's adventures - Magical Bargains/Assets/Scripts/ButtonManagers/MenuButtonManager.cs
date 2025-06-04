using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public static class GameLaunchOptions {
    public static bool SkipTutorial = false;
    public static float MusicVolume = 0.2f;
}


public class MenuButtonManager : MonoBehaviour
{

    [SerializeField] private MenuAudioManager audioManager;

    [Header("Buttons")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Toggle skipTutoToggle;

    [Header("Scenes")]
    [SerializeField] private string scene = "MainScene";



    private void Start() {
        audioManager.SetMusicVolume( GameLaunchOptions.MusicVolume );

        startGameButton.onClick.AddListener(OnStartGameButtonClick);

    }

    public void OnStartGameButtonClick()
    {

        GameLaunchOptions.MusicVolume = audioManager.GetMusicVolume();
        GameLaunchOptions.SkipTutorial = skipTutoToggle.isOn;

        SceneManager.LoadScene(scene);
    }



}
