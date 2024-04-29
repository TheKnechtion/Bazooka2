using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Header("Stored Weapon")]
    [Tooltip("This weapon will be instantiated and added to Player weapon array.")]
    [SerializeField] private GameObject weaponPrefab;

    public static event EventHandler OnWeaponPickedUp;

    private void Awake()
    {
        if (weaponPrefab == null)
        {
            Debug.LogWarning("! Weapon Pickup not set !");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<WeaponController>(out WeaponController t))
        {
            OnWeaponPickedUp?.Invoke(this, EventArgs.Empty);
            //GameObject.Find("GameManager").GetComponent<AudioManager>().PlayMiscClip("WeaponPickup", transform.position);
            AudioManager.PlayMiscClip("WeaponPickup", transform.position);

            t.AddWeapon(weaponPrefab);
            Destroy(gameObject);
        }
    }
}
