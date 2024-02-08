using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [Header("Enemies Weapon")]
    [SerializeField] private GameObject weaponObj;
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

    void Update()
    {
        
    }

    public void ShootWeapon()
    {
        weapon.HandleShooting();
    }
}
