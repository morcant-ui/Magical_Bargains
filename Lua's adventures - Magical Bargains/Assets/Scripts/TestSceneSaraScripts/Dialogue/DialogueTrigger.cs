using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;


    private bool activated = false;


  
    private void Update() {

        // if player presses button, trigger dialogue managment
        if (activated && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            activated = false;
        }
    }

    void OnMouseUp()
    {
        if (!activated) {
            activated = true;
        }
    }



}
