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


    WeaponInfo currentWeaponInfo = new WeaponInfo();

    private void Start()
    {
        
    }

    private void Update()
    {
        playerPosition = this.gameObject.transform.position;

        currentWeapon = this.gameObject.GetComponent<PlayerInfo>().currentWeapon;
            
            //PlayerInfo.instance.currentWeapon;

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
