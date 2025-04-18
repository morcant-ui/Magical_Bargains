using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class changeTMPtext : MonoBehaviour
{
    public TMP_Text inputField;
    public string text1 = "Inspect mode";
    public string text2 = "Leave";

    // Start is called before the first frame update
    void Start()
    {
        //string str = this.GetComponent<TMP_InputField>();
        

        string currentState = GameStateManager.GetInstance().GetState();

        if (currentState == "dialogue2")
        {
            inputField.text = text2;
        }
        else
        {
            inputField.text = text1;
        }
    }
}
