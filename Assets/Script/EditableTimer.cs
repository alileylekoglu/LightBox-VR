using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditableTimer : MonoBehaviour
{
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button resetButton;
    [SerializeField]
    private Button addButton;
    [SerializeField]
    private Button subtractButton;

    private float timerValue = 0f;
    private bool isTimerRunning = false;

    private void Start()
    {
        // Assign button click listeners
        startButton.onClick.AddListener(StartTimer);
        resetButton.onClick.AddListener(ResetTimer);
        addButton.onClick.AddListener(AddOneToTimer);
        subtractButton.onClick.AddListener(SubtractOneFromTimer);

        ResetTimer();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            timerValue -= Time.deltaTime;
            UpdateTimerText();

            if (timerValue <= 0f)
            {
                StopTimer();
                Debug.Log("Timer finished!");
            }
        }
    }

    public void StartTimer()
    {
        if (!isTimerRunning)
        {
            isTimerRunning = true;
        }
    }

    public void ResetTimer()
    {
        timerValue = 0f;
        UpdateTimerText();
        StopTimer();
    }

    public void AddOneToTimer()
    {
        timerValue += 1f;
        UpdateTimerText();
    }

    public void SubtractOneFromTimer()
    {
        timerValue -= 1f;
        UpdateTimerText();
    }

    private void StopTimer()
    {
        isTimerRunning = false;
    }

    private void UpdateTimerText()
    {
        timerText.text = Mathf.Ceil(timerValue).ToString();
    }
}
