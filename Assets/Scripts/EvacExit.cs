using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvacExit : MonoBehaviour
{
    public static event EventHandler OnPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
        //Check if the object colliding is Player
        if (other.gameObject.GetComponent<PlayerInfo>())
        {
            //We raise event that player has left 

            Debug.Log("Player has enter Exit Trigger");
            OnPlayerExit?.Invoke(this, EventArgs.Empty);
        }
    }
}
