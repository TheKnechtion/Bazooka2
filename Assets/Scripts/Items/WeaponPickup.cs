using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Header("Stored Weapon")]
    [Tooltip("This weapon will be instantiated and added to Player weapon array.")]
    [SerializeField] private GameObject weaponPrefab;

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
            t.AddWeapon(weaponPrefab);
            Destroy(gameObject);
        }
    }
}
