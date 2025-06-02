using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{

   
    [SerializeField] private GameObject menuPanel;

    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Foldout musicSection;
    [SerializeField] private Foldout controlSection;

    [SerializeField] private Button quitButton;

    private string menuScene = "SimpleMenu";

    private bool isOn = false;

    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(false);

        musicSlider.onValueChanged.AddListener(delegate { ChangeMusicVolume(); });

        quitButton.interactable = false;
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {  ToggleMenu(); }
    }

    void ToggleMenu() {

        if (isOn)
        {
            // hide menu

            menuPanel.SetActive(false);
            quitButton.interactable = false;
            
        }
        else {



            // update music volume
            float vol = AudioManager.GetInstance().GetMusicVolume();
            musicSlider.value = vol;
            volumeText.text = vol.ToString("0.00");

            // reset buttons
            if (musicSection.isFoldoutExpanded()) 
            {
                musicSection.ToggleFoldout();
            }

            if (controlSection.isFoldoutExpanded())
            {
                controlSection.ToggleFoldout();
            }

            // show menu
            menuPanel.SetActive(true);
            quitButton.interactable = true;
        }

        isOn = !isOn;

    }


    private void ChangeMusicVolume() {
        float vol = musicSlider.value;

        volumeText.text = vol.ToString("0.00");

        AudioManager.GetInstance().SetMusicVolume(vol);

    }

    private void OnQuitButtonClick() {
        SceneManager.LoadScene(menuScene);
    }
}
