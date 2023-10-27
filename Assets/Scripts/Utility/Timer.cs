using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float timerCount;
    private float setTime;
    public float TimeLeft { get { return timerCount; } }

    public Timer(float passedTime)
    {
        setTime= passedTime;
        resetTimer();
    }

    public void tickTimer(float deltaTime)
    {
        timerCount -= deltaTime;
    }

    public bool timerFinished()
    {
        if (timerCount <= 0)
        {
            return true;
        }
        else { return false; }
    }

    public void resetTimer()
    {
        timerCount = setTime;
    }

    public void Zero()
    { 
        timerCount = 0.0f;
    }

}
