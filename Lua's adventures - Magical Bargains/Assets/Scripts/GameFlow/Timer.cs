using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private GameObject bg;

    private float maxTime;

    private float elapsedTime;

    private bool isTimerOn;

    private float R = 1f;
    private float G = 1f;

    private float RGoal = 0f;
    private float GGoal = 0.66f;

    private float RStep;
    private float GStep;


    void Start()
    {
        isTimerOn = false;
        elapsedTime = 0.00F;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerOn)
        {
            float oldSeconds = Mathf.FloorToInt(elapsedTime % 60);

            elapsedTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (oldSeconds != seconds) {
                UpdateBackground();
            }
            

            if (elapsedTime >= maxTime)
            {
                //timerText.text = "Timer Over";

                timerText.color = Color.red;
                isTimerOn = false;
                NotifyManager();
            }
        }
    }

    public void StartTimer(float maximumTime) {
        if (elapsedTime == 0.00F) { 

            maxTime = maximumTime;

            StartBackgroundChanges();
        }



        isTimerOn = true;

        GameStateManager.GetInstance().setTimerEnded(false);
    }

    public float CheckTimer()
    {
        return elapsedTime;
    }

    public float PauseTimer() {
        isTimerOn = false;
        timerText.text = "";

        return elapsedTime;
    }

    public float ResetTimer() {
        float returnTime = elapsedTime;

        maxTime = 0.00F;
        elapsedTime = 0.00F;
        isTimerOn = false;

        timerText.text = "";
        timerText.color = Color.white;

        ResetBackground();

        return returnTime;
    }

    private void NotifyManager() {
        GameStateManager.GetInstance().setTimerEnded(true);
    }


    private void StartBackgroundChanges() {
        
        RStep = Mathf.Abs(R - RGoal) / maxTime;
        GStep = Mathf.Abs(G - GGoal) / maxTime;
    }

    private void UpdateBackground() { 
        
        R -= RStep;
        G -= GStep;

        R = Mathf.Clamp(R, 0f, 1f);
        G = Mathf.Clamp(G, 0f, 1f);

        bg.GetComponent<SpriteRenderer>().color = new Color(R, G, 1f);
    
    }

    public void ResetBackground() {


        R = 1f;
        G = 1f;

        bg.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    }

}
