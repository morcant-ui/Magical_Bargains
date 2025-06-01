using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelMenu : MonoBehaviour
{

   
    [SerializeField] private GameObject menuPanel;

    [SerializeField] private GameObject musicSlider;
    [SerializeField] private Foldout musicSection;
    [SerializeField] private Foldout controlSection;

    private bool isOn = false;

    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(false);
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
            
        }
        else {



            // update music volume
            Slider slider = musicSlider.GetComponent<Slider>();

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
}
