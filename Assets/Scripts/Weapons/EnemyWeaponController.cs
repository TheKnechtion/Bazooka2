using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [Header("Enemies Weapon")]
    public GameObject weaponObj;
    [SerializeField] private Transform weaponLocation;
    private RangedWeapon weapon;

    void Start()
    {
        if (weaponObj != null)
        {
            GameObject temp = Instantiate(weaponObj, weaponLocation);
            weapon = temp.GetComponent<RangedWeapon>();
        }
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
