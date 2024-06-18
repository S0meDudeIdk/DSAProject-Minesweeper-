using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI timerText;
    private float elapsedTime;
    private bool isRunning = false;

    void Update() {
        if (isRunning) {
            UpdateTimer();
        }
    }

    public float GetElapsedTime() {
        return elapsedTime;
    }

    private void UpdateTimer() {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int fraction = Mathf.FloorToInt((elapsedTime * 100) % 100);
        timerText.text = $"{minutes:00}:{seconds:00}.{fraction:00}";
    }

    public void StartTimer() {
        isRunning = true;
        elapsedTime = 0f;
    }

    public void StopTimer() {
        isRunning = false;
    }

    public void ResetTimer() {
        isRunning = false;
        elapsedTime = 0f;
        timerText.text = "00:00.00";
    }
}
