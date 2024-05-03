using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_PickUp_Item : MonoBehaviour
{
    [SerializeField] int amountToGain;
    public static event EventHandler pickedUpAmmo;

    [SerializeField] GameObject popupText;

    GameObject popupText_ref;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.TryGetComponent<WeaponController>(out WeaponController t))
            {
                t.RefillAllWeapons(amountToGain);
                pickedUpAmmo?.Invoke(this, EventArgs.Empty);

            }

            //GameObject.Find("GameManager").GetComponent<AudioManager>().PlayMiscClip("AmmoPickup", transform.position);
            AudioManager.PlayMiscClip("AmmoPickup", transform.position);

            //popupText.GetComponent<PopupUI>().Meh(amountToGain);

            popupText_ref = Instantiate(popupText, this.transform.position, popupText.transform.rotation);

            //other.gameObject.GetComponent<PlayerManager>().GainAmmo(amountToGain);
            //pickedUpAmmo?.Invoke(this, EventArgs.Empty);
            Destroy(this.gameObject);
        }
    }
}
