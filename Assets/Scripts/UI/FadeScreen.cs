using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public event EventHandler OnFadedFull;

    public Animator screenAnimator;

    private void Start()
    {
        screenAnimator = GetComponent<Animator>();
    }

    public void InvokeFadeFinished()
    {
        OnFadedFull.Invoke(this, EventArgs.Empty);
    }
}
