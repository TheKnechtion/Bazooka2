using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    PlayerController _playerController;

    bool pressed;

    //store the current player position
    Vector3 playerPosition;

    //stores the player look direction
    Vector3 playerLookDirection;



    Object projectilePrefab;
    GameObject currentEntity;
    WeaponInfo currentWeapon;

    float timeBetweenShots = 0.0f;

    float scrollValue;

    int ownedWeaponCount;
    int weaponIndex = 0;

    List<WeaponInfo> playerOwnedWeapons;

    private void Start()
    {
        
    }

    private void Update()
    {
        CheckWeaponChange();

        playerPosition = this.gameObject.transform.position;

        playerOwnedWeapons = this.gameObject.GetComponent<PlayerInfo>().ownedWeapons;

        ownedWeaponCount = playerOwnedWeapons.Count;

        weaponIndex = weaponIndex % ownedWeaponCount;

        currentWeapon = this.gameObject.GetComponent<PlayerInfo>().ownedWeapons[weaponIndex];



        //sets the player look direction based on the player origin and the mouse cursor location
        playerLookDirection = PlayerInfo.instance.playerLookDirection;

        pressed = _playerController.PlayerActions.Shoot.IsPressed();

        if (pressed) 
        { 
            HandleShooting(); 
        }

        //tracks time between shots, stopping at 0.
        timeBetweenShots = (timeBetweenShots > 0) ? timeBetweenShots -= Time.deltaTime : 0;


    }


    private void CheckWeaponChange()
    {
        scrollValue = _playerController.PlayerActions.ChangeWeapon.ReadValue<float>();

        if (scrollValue > 0)
        {
            weaponIndex++;
        }

        if (scrollValue < 0)
        {
            weaponIndex--;
        }

        if (weaponIndex < 0)
        {
            weaponIndex = ownedWeaponCount - 1;
        }


    }





    private void HandleShooting()
    {
       
        if (timeBetweenShots <= 0.0f)
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


    void Shoot()
    {
       
        projectilePrefab = Resources.Load(currentWeapon.ProjectileName);

        currentEntity = Instantiate(projectilePrefab as GameObject, playerPosition+playerLookDirection, new Quaternion(0, 0, 0, 0));

        
        currentEntity.GetComponent<Projectile>().currentWeaponInfo = currentWeapon;

        currentEntity.GetComponent<Projectile>().direction = playerLookDirection.normalized;
    }


}
