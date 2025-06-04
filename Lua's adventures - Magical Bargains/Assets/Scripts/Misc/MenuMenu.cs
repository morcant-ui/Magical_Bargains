using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuMenu : MonoBehaviour
{

    [SerializeField] private MenuAudioManager audioManager;

    [SerializeField] private GameObject menuPanel;

    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Foldout musicSection;
    [SerializeField] private Foldout controlSection;

    [SerializeField] private Button menuButton;


    private bool isOn = false;

    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(false);

        musicSlider.onValueChanged.AddListener(delegate { ChangeMusicVolume(); });

        menuButton.interactable = true;
        menuButton.onClick.AddListener(OnMenuButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { ToggleMenu(); }
    }

    private void OnMenuButtonClick()
    {
        ToggleMenu();
    }

    void ToggleMenu()
    {

        if (isOn)
        {
            // hide menu

            menuPanel.SetActive(false);

        }
        else
        {



            // update music volume
            float vol = audioManager.GetMusicVolume();
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
        }

        isOn = !isOn;

    }


    private void ChangeMusicVolume()
    {
        float vol = musicSlider.value;

        volumeText.text = vol.ToString("0.00");

        audioManager.SetMusicVolume(vol);

    }


}
