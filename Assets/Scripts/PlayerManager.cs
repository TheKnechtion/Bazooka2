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

    int shotsFired = 0;

    WeaponInfo currentWeaponInfo = new WeaponInfo();

    private void Start()
    {
        
    }

    private void Update()
    {
        playerPosition = PlayerInfo.instance.playerPosition;

        currentWeapon = PlayerInfo.instance.currentWeapon;

        //sets the player look direction based on the player origin and the mouse cursor location
        playerLookDirection = PlayerInfo.instance.playerLookDirection;


        pressed = _playerController.PlayerActions.Shoot.IsPressed();

        if (pressed) { HandleShooting(); }

        //tracks time between shots, stopping at 0.
        timeBetweenShots = (timeBetweenShots > 0) ? timeBetweenShots -= Time.deltaTime : 0;


    }

    
    private void HandleShooting()
    {
       
        if (timeBetweenShots <= 0.0f)
        {
    
            Debug.Log("Hello");

            timeBetweenShots = currentWeapon.timeBetweenProjectileFire;
          
            Shoot();

        }
    }

    private void Awake()
    {
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
       
        projectilePrefab = Resources.Load(currentWeapon.projectileType.ToString());

        currentEntity = Instantiate(projectilePrefab as GameObject, playerPosition, new Quaternion(0, 0, 0, 0));

        
        currentEntity.GetComponent<Projectile>().currentWeaponInfo = currentWeapon;

        currentEntity.GetComponent<Projectile>().direction = playerLookDirection.normalized;
    }


}
