using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float timerCount;
    public float TimeLeft { get { return timerCount; } }

    public Timer(float passedTime)
    {
        timerCount = passedTime;
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
}
