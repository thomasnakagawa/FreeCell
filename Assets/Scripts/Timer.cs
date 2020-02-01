﻿using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text text;

    private bool timerIsOn = false;
    private float timeAccumulator = 0f;

    void Start()
    {
        text = GetComponentInChildren<Text>();
        Assert.IsNotNull(text);
    }

    void Update()
    {
        if (timerIsOn)
        {
            timeAccumulator += Time.deltaTime;
            string minutes = Mathf.Floor(timeAccumulator / 60f).ToString("00");
            string seconds = (timeAccumulator % 60).ToString("00");
            text.text = minutes + ":" + seconds;
        }
    }

    public void RestartTimer()
    {
        timeAccumulator = 0f;
        timerIsOn = true;
    }

    public void StopTimer()
    {
        timerIsOn = false;
    }
}
