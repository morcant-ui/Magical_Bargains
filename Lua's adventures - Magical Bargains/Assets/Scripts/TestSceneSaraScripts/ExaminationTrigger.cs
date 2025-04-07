using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExaminationTrigger : MonoBehaviour
{

    [SerializeField] private string newScene = "TestSceneSara";



    private void Start() {
    }

    void OnMouseUp()
    {
        GameStateManager.GetInstance().SetState("examination");
        SceneManager.LoadScene(newScene);
        // for the above to work: goto file -> build settings -> drag and drop desired scene to the build
    }
}
