using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponController:MonoBehaviour
{
    /// <summary>
    /// This class will handle all aspects of the held weapon
    /// [Shooting, firerate, etc.]
    /// The PlayerManager will use this class to use Shoot()
    /// </summary>

    WeaponInfo tempWeaponInfo;
    List<WeaponInfo> weaponDatabase;



    Vector3 position;

    Object projectilePrefab;

    GameObject currentEntity;

    WeaponInfo weapon;

    //NEW WEAPON HANDLING STUFF
    [SerializeField] private GameObject[] prefabRefernceList;
    [SerializeField] private GameObject[] WeaponList;
    [SerializeField] private Transform weaponLocation;

    public GameObject currentWeaponPrefab;
    RangedWeapon currentWeapon;


    [SerializeField] private bool Player; //Dirty Hack, FIX later
    private void Awake()
    {

    }

    private void Start()
    {
        if (Player)
        {
            WeaponList = new GameObject[prefabRefernceList.Length];
            for (int i = 0; i < prefabRefernceList.Length; i++)
            {
                GameObject temp = Instantiate(prefabRefernceList[i], weaponLocation);
                WeaponList[i] = temp;
                WeaponList[i].SetActive(false);
            }

            WeaponList[0].SetActive(true);
            currentWeapon = WeaponList[0].GetComponent<RangedWeapon>();
        }
        
    }

    public void ShootWeapon()
    {
        currentWeapon.Shoot();
    }

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
        //if (item != null) 
        //{
        //    tempWeaponInfo = item;
        //}

        return tempWeaponInfo;
    }



}




