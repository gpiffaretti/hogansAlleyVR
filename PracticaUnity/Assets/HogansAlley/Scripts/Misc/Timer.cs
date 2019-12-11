using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{

    float interval;
    float elapsedTime;

    bool running;

    public float ElapsedTime { get { return elapsedTime;  } }
    public float CountDown { get { return Mathf.Clamp(interval - elapsedTime, 0f, float.MaxValue); } }

    public event Action timerFinished;

    public Timer(float interval)
    {
        this.interval = interval;
        running = false;
    }

    public void Start()
    {
        running = true;
    }

    public void Update()
    {
        if(running)
            elapsedTime += Time.deltaTime;

        if (Completed())
        {
            running = false;
            elapsedTime = interval;
            timerFinished?.Invoke();
        }
    }

    public bool Completed()
    {
        return elapsedTime >= interval;
    }

    public void Reset()
    {
        elapsedTime = 0f;
        running = false;
    }
    
}
