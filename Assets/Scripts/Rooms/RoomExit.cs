using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    public static event EventHandler OnPlayerExit_Room;

    private void OnTriggerEnter(Collider other)
    {
        //Check if the object colliding is Player
        if (other.gameObject.tag == "Player")
        {
            OnPlayerExit_Room?.Invoke(this, EventArgs.Empty);
        }
    }
}
