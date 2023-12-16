using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseWeaponGrabber : MonoBehaviour
{
    WeaponInfo tempWeaponInfo;
    List<WeaponInfo> weaponDatabase;

    public WeaponInfo MakeWeapon(string weaponName)
    {
        weaponDatabase = WeaponDatabase.Instance().Weapon_Database;

        foreach(WeaponInfo weapon in weaponDatabase)
        {
            if(weapon.weaponName == weaponName)
            {
                tempWeaponInfo = weapon;
            }
        }
        return tempWeaponInfo;
    }
}
