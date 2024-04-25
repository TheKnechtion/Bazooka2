using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [Header("Enemies Weapon")]
    public GameObject weaponObj;

    private GameObject weaponInstance;

    [SerializeField] private Transform weaponLocation;
    private RangedWeapon weapon;

    void Awake()
    {
        if (weaponObj != null)
        {
            InitWeapon(weaponObj);
        }
    }
    public void InitWeapon(GameObject weaponPrefab)
    {
        if (weaponInstance != null)
        {
            Destroy(weaponInstance);
            weaponInstance = null;
        }

        weaponInstance = Instantiate(weaponPrefab, weaponLocation);
        weapon = weaponInstance.GetComponent<RangedWeapon>();
    }
    public void OvverideFireRate(float newFireRate)
    {
        if (weapon != null)
        {
            weapon.fireRate = newFireRate;
        }
    }

    public void ShootWeapon()
    {
        weapon.HandleShooting();
    }
}
