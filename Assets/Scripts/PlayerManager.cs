using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour
{

    PlayerController _playerController;

    bool pressed;

    //store the current player position
    Vector3 playerPosition;

    //stores the player look direction
    Vector3 playerLookDirection;



    GameObject projectilePrefab;
    GameObject currentEntity;
    public static WeaponInfo currentWeapon;

    float timeBetweenShots = 0.0f;

    float scrollValue;

    int ownedWeaponCount;
    int weaponIndex = 0;

    public static int activeProjectiles = 0;


    List<WeaponInfo> playerOwnedWeapons;

    //holds the material that makes all the player projectiles white
    Material projectileMaterial;

    public static event EventHandler OnPlayerWeaponChange;
    public static event EventHandler OnPlayerShoot;
    public static event EventHandler OnPlayerDetonate;

    private void Start()
    {
        //create the white projectile material used by the player projectiles
        projectileMaterial = Resources.Load("White") as Material;

    }




    private void Update()
    {
        playerPosition = this.gameObject.transform.position;

        CheckWeaponChange();


        //sets the player look direction based on the player origin and the mouse cursor location
        playerLookDirection = PlayerInfo.instance.playerLookDirection;

        

        if (_playerController.PlayerActions.Shoot.IsPressed()) 
        { 
            HandleShooting(); 
        }



        if(_playerController.PlayerActions.Detonate.IsPressed())
        {
            OnPlayerDetonate?.Invoke(this, EventArgs.Empty);
        }



        //tracks time between shots, stopping at 0.
        timeBetweenShots = (timeBetweenShots > 0) ? timeBetweenShots -= Time.deltaTime : 0;


    }

    float cooldownTime = 0.5f;
    float nextInputTime = 0.0f;
    private void CheckWeaponChange()
    {
        scrollValue = 0;

        ownedWeaponCount = PlayerInfo.instance.ownedWeapons.Count;

        currentWeapon = PlayerInfo.instance.ownedWeapons[weaponIndex];

        if(Time.time >= nextInputTime && _playerController.PlayerActions.ChangeWeapon.triggered)
        {
            scrollValue = _playerController.PlayerActions.ChangeWeapon.ReadValue<float>();

            nextInputTime = Time.time + cooldownTime;
        }
            

        if (scrollValue > 0)
        {
            weaponIndex = (weaponIndex+1 < ownedWeaponCount) ? ++weaponIndex:0;
            currentWeapon = PlayerInfo.instance.ownedWeapons[weaponIndex];
            OnPlayerWeaponChange?.Invoke(this, EventArgs.Empty);
        }

        if (scrollValue < 0)
        {
            weaponIndex = (weaponIndex - 1 >= 0) ? --weaponIndex : ownedWeaponCount-1;
            currentWeapon = PlayerInfo.instance.ownedWeapons[weaponIndex];
            OnPlayerWeaponChange?.Invoke(this, EventArgs.Empty);
        }
    }





    private void HandleShooting()
    {
       
        if (timeBetweenShots <= 0.0f && activeProjectiles < currentWeapon.maxProjectilesOnScreen)
        {

            timeBetweenShots = currentWeapon.timeBetweenProjectileFire;
          
            Shoot();

        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _playerController = new PlayerController();

    }


    private void OnEnable()
    {
        //begins player movement functions
        _playerController.PlayerActions.Enable();
    }


    private void OnDisable()
    {
        //ends player movement functions
        _playerController.PlayerActions.Disable();
    }

    //List<GameObject> currentProjectiles = new List<GameObject> ();

    void Shoot()
    {
        activeProjectiles++;

        OnPlayerShoot?.Invoke(this, EventArgs.Empty);

        projectilePrefab = (GameObject)Resources.Load(currentWeapon.ProjectileName);

        currentEntity = Instantiate(projectilePrefab, playerPosition+playerLookDirection, new Quaternion(0, 0, 0, 0));

        
        currentEntity.GetComponent<Projectile>().currentWeaponInfo = currentWeapon;

        currentEntity.GetComponent<Projectile>().direction = playerLookDirection.normalized;
        currentEntity.GetComponent<Renderer>().material = projectileMaterial;
        currentEntity.AddComponent<PlayerProjectile>();


        var light = currentEntity.AddComponent<Light>();
    }


}
