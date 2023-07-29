using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public float time = 0;
    public bool timerActive = false;

    public float addTimeAmount = 5;  // Amount of time to add
    public float removeTimeAmount = 5; // Amount of time to remove
    public GameObject startButton;
    public GameObject resetButton;

    public void AddTime()
    {
        time += addTimeAmount;
        timerText.text = Mathf.Round(time).ToString();
    }

    public void RemoveTime()
    {
        time -= removeTimeAmount;
        timerText.text = Mathf.Round(time).ToString();
    }

    public void StartTimer()
    {
        timerActive = true;
        startButton.SetActive(false);
    }

    public void ResetTimer()
    {
        time = 0;
        timerActive = false;
        timerText.text = "0";
        startButton.SetActive(true);
        resetButton.SetActive(false);
    }

    private void Update()
    {
        if (timerActive)
        {
            time -= Time.deltaTime;
            timerText.text = Mathf.Round(time).ToString();
        }

        if (time <= 0)
        {
            timerActive = false;
            time = 0;
            timerText.text = "0";
        }
    }
}
