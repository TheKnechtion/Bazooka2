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
    public RangedWeapon currentWeapon;
    int weaponIndex;

    float weaponSwitchTime;
    float switchTime;
    bool canSwitch;

    [SerializeField] private bool Player; //Dirty Hack, TODO: FIX later
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

            weaponIndex = 0;
            WeaponList[weaponIndex].SetActive(true);
            currentWeapon = WeaponList[weaponIndex].GetComponent<RangedWeapon>();
        }

        PlayerManager.OnWeaponChange += ChangedWeapon;

        weaponSwitchTime = 0.5f;     
        canSwitch = false;
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
            //Debug.Log("E pressed" + e.Epressed);
            //Debug.Log("Q pressed" + e.Qpressed);
        }
        
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

        //Debug.Log(switchTime);
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
            //When at 0 and Decrement, go to end of list
            weaponIndex = WeaponList.Length-1;
            activateWeapon(weaponIndex);
        }
        currentWeapon = WeaponList[weaponIndex].GetComponent<RangedWeapon>();
    }
    #endregion

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




