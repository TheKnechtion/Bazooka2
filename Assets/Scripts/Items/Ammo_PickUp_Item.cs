using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_PickUp_Item : MonoBehaviour
{
    [SerializeField] int amountToGain;
    public static event EventHandler pickedUpAmmo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerManager>().GainAmmo(amountToGain);
            pickedUpAmmo?.Invoke(this, EventArgs.Empty);
            Destroy(this.gameObject);
        }
    }
}
