using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponController:MonoBehaviour
{
    WeaponInfo tempWeaponInfo = new WeaponInfo();

    List<WeaponInfo> weaponDatabase = new List<WeaponInfo>();

    PlayerManager playerUser = null;
    EnemyInfo enemyUser = null;

    Vector3 position;

    Object projectilePrefab;

    GameObject currentEntity;

    WeaponInfo weapon;


    private void Awake()
    {
        Debug.Log("WepController Awake at "+gameObject.name);
        getUserComponent();
        //weaponDatabase = new WeaponDatabase().Weapon_Database;
       // weaponDatabase = WeaponDatabase.Instance().Weapon_Database;
    }



    //Utility for finding appropriate weapon data based on passed in string
    public WeaponInfo MakeWeapon(string weaponName)
    {
        Debug.Log("Calling from "+gameObject.name);

        //weaponDatabase = new WeaponDatabase().Weapon_Database;
        weaponDatabase = WeaponDatabase.Instance().Weapon_Database;

        WeaponInfo item = weaponDatabase.FirstOrDefault(weapon => weapon.weaponName.Contains(weaponName));
        if (item != null)
        {
            tempWeaponInfo = item;
        }

        
        return tempWeaponInfo;
    }

    private void getUserComponent()
    {
        playerUser = gameObject.GetComponentInParent<PlayerManager>();
        enemyUser = gameObject.GetComponentInParent<EnemyInfo>();

        if (playerUser != null)
        {
            Debug.Log("User is a: " + playerUser.gameObject.name);
        }
        else if (enemyUser != null)
        {
            Debug.Log("User is a: " + enemyUser.gameObject.name);
        }
    }

    

}




