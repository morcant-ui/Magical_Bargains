using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class InspectSceneButtonManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button magnifierButton;
    [SerializeField] private Button inspectButton;

    [Header("Additionnal objects")]
    [SerializeField] private string dialogueScene = "TestDialogueMode";

    [SerializeField] private GameObject magnifier;

    private bool magnifierButtonActivated = false;
    private bool inspectButtonActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        inspectButton.onClick.AddListener(OnInspectButtonClick);
        magnifierButton.onClick.AddListener(OnMagnifierButtonClick);

        magnifier.SetActive(false);
    }

    public void OnInspectButtonClick() {
        if (!inspectButtonActivated)
        {
            inspectButtonActivated = true;
        }
    }

    public void OnMagnifierButtonClick() {
        if (magnifierButtonActivated)
        {

            magnifier.SetActive(false);
            magnifierButtonActivated = false;
        }
        else
        {

            magnifier.SetActive(true);
            magnifierButtonActivated = true;
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (inspectButtonActivated) {

            string currentState = GameStateManager.GetInstance().GetState();

            Debug.Log(currentState);

            if (currentState == "examination")
            {
                GameStateManager.GetInstance().SetState("dialogue2");
                SceneManager.LoadScene(dialogueScene);
            }
            else {
                Debug.Log("There was a problem with scene management");
            }
        }
    }
}
