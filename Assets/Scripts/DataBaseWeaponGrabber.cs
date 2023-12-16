using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseWeaponGrabber : MonoBehaviour
{
    WeaponInfo tempWeaponInfo;
    List<WeaponInfo> weaponDatabase;

    //Utility for finding appropriate weapon data based on passed in string
    public WeaponInfo MakeWeapon(string weaponName)
    {
        weaponDatabase = WeaponDatabase.Instance().Weapon_Database;

        //WeaponInfo item = weaponDatabase.FirstOrDefault(weapon => weapon.weaponName.Contains(weaponName));
        foreach (WeaponInfo weapon in weaponDatabase)
        {
            if (weapon.weaponName == weaponName)
            {
                tempWeaponInfo = weapon;
            }
        }

        return tempWeaponInfo;
    }
}
