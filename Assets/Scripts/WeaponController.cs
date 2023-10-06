using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponController:MonoBehaviour
{
    WeaponInfo tempWeaponInfo;
    List<WeaponInfo> weaponDatabase = new List<WeaponInfo>();



    Vector3 position;

    Object projectilePrefab;

    GameObject currentEntity;

    WeaponInfo weapon;


    private void Awake()
    {

    }



    //Utility for finding appropriate weapon data based on passed in string
    public WeaponInfo MakeWeapon(string weaponName)
    {
        weaponDatabase = WeaponDatabase.Instance().Weapon_Database;

        WeaponInfo item = weaponDatabase.FirstOrDefault(weapon => weapon.weaponName.Contains(weaponName));
        if (item != null) 
        {
            tempWeaponInfo = item;
        }

        return tempWeaponInfo;
    }



}




