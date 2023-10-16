using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class PlayerManager : MonoBehaviour
{

    PlayerController _playerController;

    bool pressed;

    //store the current player position
    Vector3 playerPosition;

    //stores the player look direction
    public static Vector3 playerLookDirection;



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

        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

    }



    //variable that holds value for the x,y center of the screen in pixels
    public Vector2 centerScreen = new Vector2();

    //variable that holds value of location of the mouse cursor
    public Vector3 mousePos = new Vector3();






    private void Update()
    {
        //get the position of the center of the screen
        centerScreen = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);


        //current x,y vector of how far away the cursor is from the bottom left of the screen
        mousePos = Input.mousePosition;


        //translate the mouse coordinates to be based around the center of the screen
        mousePos.x -= centerScreen.x;
        mousePos.y -= centerScreen.y;

        //sets the player look direction based on the player origin and the mouse cursor location
        playerLookDirection = (RaycastController.playerLookVector).normalized;


        playerPosition = gameObject.transform.position;
        playerPosition.y = 1.0f;

        CheckWeaponChange();

        

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

    private void FixedUpdate()
    {
        Quaternion rotation = Quaternion.LookRotation(playerLookDirection, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5.0f);

    }



    float cooldownTime = 0.1f;
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
