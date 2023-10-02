using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvacExit : MonoBehaviour
{
    public event EventHandler OnPlayerExit;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the object colliding is Player
        if (collision.gameObject.GetComponent<PlayerInfo>())
        {
            OnPlayerExit?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
