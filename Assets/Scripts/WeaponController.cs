using System;
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

    //Object projectilePrefab;

    GameObject currentEntity;

    WeaponInfo weapon;

    //NEW WEAPON HANDLING STUFF
    [SerializeField] private GameObject[] prefabRefernceList;
    [SerializeField] private GameObject[] WeaponList;
    [SerializeField] private Transform weaponLocation;

    public GameObject currentWeaponPrefab;
    public RangedWeapon currentWeapon;
    int weaponIndex;

    float weaponSwitchTime;
    float switchTime;
    bool canSwitch;

    [SerializeField] private bool Player; //Dirty Hack, TODO: FIX later

    public event EventHandler FinishedWeaponChange;
    public static event EventHandler UpdateUI;

    //public static int maxActiveProjectiles_ref;



    private void Awake()
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
        }
    }


    bool initializedWeapons = false;

    private void Start()
    {

        weaponIndex = 0;
        activateWeapon(weaponIndex);
        currentWeapon = WeaponList[weaponIndex].GetComponent<RangedWeapon>();

        PlayerManager.OnWeaponChange += ChangedWeapon;


        //maxActiveProjectiles_ref = currentWeapon.maxActiveProjectiles;

        weaponSwitchTime = 0.5f;     
        canSwitch = false;
    }

    public void InitializeWeaponUI()
    {
        FinishedWeaponChange?.Invoke(this, EventArgs.Empty);
    }



    private void ChangedWeapon(object sender, PlayerManager.OnWeaponSwitchEventArgs e)
    {
        if (canSwitch)
        {
            canSwitch = false;
            switchTime = 0.0f;
            if (e.Epressed)
            {
                ListDecrement();
            }
            if (e.Qpressed)
            {
                ListIncrement();
            }

            //maxActiveProjectiles_ref = currentWeapon.maxActiveProjectiles;

            FinishedWeaponChange?.Invoke(this, EventArgs.Empty);
            //Debug.Log("E pressed" + e.Epressed);
            //Debug.Log("Q pressed" + e.Qpressed);
        }
        
    }

    private void Update()
    {

    }


    private void FixedUpdate()
    {
        if (switchTime < weaponSwitchTime)
        {
            switchTime += Time.deltaTime;
        }
        else {
            switchTime = weaponSwitchTime; 
            canSwitch = true;
        }

        //if (!initializedWeapons)
        //{
        //    ListIncrement();

        //    maxActiveProjectiles_ref = currentWeapon.maxActiveProjectiles;
        //    UpdateUI?.Invoke(this, EventArgs.Empty);

        //    FinishedWeaponChange?.Invoke(this, EventArgs.Empty);

        //}

        //Debug.Log(switchTime);
    }


    private void LateUpdate()
    {
        //if (!initializedWeapons)
        //{
        //    initializedWeapons = true;

        //    ListIncrement();

        //    maxActiveProjectiles_ref = currentWeapon.maxActiveProjectiles;
        //    UpdateUI?.Invoke(this, EventArgs.Empty);

        //    FinishedWeaponChange?.Invoke(this, EventArgs.Empty);

           
        //}
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        ListDecrement();
    //    }
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        ListIncrement();
    //    }

    //}

    #region Weapon Changing - prefabs
    private void deactivateWeapon(int index) 
    {
        WeaponList[index].SetActive(false);
    }

    private void activateWeapon(int index)
    {
        WeaponList[index].SetActive(true);
    }

    
    private void ListIncrement()
    {
        deactivateWeapon(weaponIndex);
        if (weaponIndex < WeaponList.Length-1)
        {
            weaponIndex++;
            activateWeapon(weaponIndex);
        }
        else
        {
            //Go to start if we reach end
            weaponIndex = 0;
            activateWeapon(weaponIndex);
        }
        currentWeapon = WeaponList[weaponIndex].GetComponent<RangedWeapon>();
    }

    private void ListDecrement()
    {
        deactivateWeapon(weaponIndex);        
        if (weaponIndex > 0)
        {
            weaponIndex--;
            activateWeapon(weaponIndex);
        }
        else
        {
            //When at 0 and Decrement, go to tail-end of list
            weaponIndex = WeaponList.Length-1;
            activateWeapon(weaponIndex);
        }
        currentWeapon = WeaponList[weaponIndex].GetComponent<RangedWeapon>();
    }
    #endregion

    public void ShootWeapon()
    {
        //UpdateUI?.Invoke(this, EventArgs.Empty);
        currentWeapon.Shoot();
    }

    public void PlayerShootWeapon()
    {
        //UpdateUI?.Invoke(this, EventArgs.Empty);
        if (currentWeapon.currentAmmo > 0)
        {
            currentWeapon.PlayerShoot();
        }
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




