using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxProjOnScreen_Increase_PickUp : MonoBehaviour
{
    [SerializeField] int amountToGain;
    public static event EventHandler pickedUpAmmo;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerManager>().IncreaseMaxProjOnScreen(amountToGain);
            pickedUpAmmo?.Invoke(this, EventArgs.Empty);
            Destroy(this.gameObject);
        }
    }
}
