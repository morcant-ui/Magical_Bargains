using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Foldout : MonoBehaviour
{
    public Button button;
    public GameObject contentPanel;
    public RectTransform arrowIcon;
    private bool isExpanded = false;

    void Start() {
        button.onClick.AddListener(ToggleFoldout);
        contentPanel.SetActive(false);

    }


    public void ToggleFoldout()
    {
        isExpanded = !isExpanded;
        contentPanel.SetActive(isExpanded);
        arrowIcon.rotation = Quaternion.Euler(0, 0, isExpanded ? 0 : 90); // Rotate arrow
    }


    public bool isFoldoutExpanded() {
        return isExpanded;
    }
}