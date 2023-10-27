using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTimer : MonoBehaviour
{
    private float timerCount;

    private float setTime;

    public AutoTimer(float passedTime)
    {
        timerCount = setTime;
    }

    // Update is called once per frame
    void Update()
    {
        timerCount -= Time.deltaTime;
        
        if(timerCount <= 0)
        {
            Destroy(this);
        }

    }

    public float tick()
    {
        return Time.deltaTime;
    }

}
