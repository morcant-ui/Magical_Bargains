using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private float maxTime;

    private float elapsedTime;

    private bool isTimerOn;


    // Start is called before the first frame update
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
            elapsedTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (elapsedTime >= maxTime)
            {
                Debug.Log("Timer finished !");

                //timerText.text = "Timer Over";
                timerText.color = Color.red;
                isTimerOn = false;
                NotifyManager();
            }

        }
    }

    public void StartTimer(float maximumTime) {
        if (elapsedTime == 0.00F) { maxTime = maximumTime; }

        isTimerOn = true;

        GameStateManager.GetInstance().timerEnded = false;
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

        return returnTime;
    }

    private void NotifyManager() {
        GameStateManager.GetInstance().timerEnded = true;
    }
}
