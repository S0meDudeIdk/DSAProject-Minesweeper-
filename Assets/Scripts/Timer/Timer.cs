using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour {
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    private bool isRunning = false;

    void Update() {
        if (isRunning) {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            float fraction = (elapsedTime * 100) % 100;
            timerText.text = string.Format("{0:00}:{1:00}.{2:00}", 
                                            minutes, seconds, fraction);
        }
    }

    public void StartTimer() {
        isRunning = true;
        elapsedTime = 0f;
    }

    public void StopTimer() {
        isRunning = false;
    }

    public void ResetTimer() {
        elapsedTime = 0f;
        timerText.text = "00:00.00";
    }
}
