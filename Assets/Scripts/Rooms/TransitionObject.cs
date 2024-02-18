using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionObject : MonoBehaviour
{
    // Doors/Transitions are only at the end, RIGHT NOW
    public static event EventHandler OnEndReached;

    private void OnTriggerEnter(Collider other)
    {
        OnEndReached?.Invoke(this, EventArgs.Empty);
    }

    //Using layers for Collion.
    //Only thing that can interact with this is 'Player'
    // Project settings > Physics > Collision matrix
}
