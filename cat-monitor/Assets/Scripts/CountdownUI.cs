using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownUI : MonoBehaviour
{
    public int countDownTime;
    private float timeToStart = -1;

    public RaceManager _rm;

    public TextMeshProUGUI timer;

    bool isTimering;

    private void Update()
    {
        if (isTimering)
        {
            if (Time.time > timeToStart)
            {
                _rm.startRace();
            }
            timer.text = "" + Mathf.Ceil(timeToStart - Time.time);
        }
    }

    public void startTimer()
    {
        isTimering = true;
        timeToStart = Time.time + countDownTime;
    }

    public void resetTimer()
    {
        isTimering = false;
        timeToStart = -1;
    }
}
