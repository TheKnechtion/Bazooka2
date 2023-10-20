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

    //Track movement in the mouse
    Vector2 mouseDelta;

    //stores the player look direction
    public static Vector3 playerLookDirection;

    Camera mainCamera;

    GameObject projectilePrefab;
    GameObject currentEntity;
    public static WeaponInfo currentWeapon;

    float timeBetweenShots = 0.0f;

    float scrollValue;

    int ownedWeaponCount;
    int weaponIndex = 0;

    public static int activeProjectiles = 0;


    List<WeaponInfo> playerOwnedWeapons;

    public static event EventHandler OnPlayerWeaponChange;
    public static event EventHandler OnPlayerShoot;
    public static event EventHandler OnPlayerDetonate;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _playerController = new PlayerController();
        _playerController.PlayerActions.MousePosition.performed += mouseMove => mouseDelta = mouseMove.ReadValue<Vector2>();
        _playerController.PlayerActions.MousePosition.canceled += mouseMove => mouseDelta = Vector2.zero;
        mainCamera = Camera.main;
    }



    private void Start()
    {

        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }




    //variable that holds value for the x,y center of the screen in pixels
    public Vector2 centerScreen = new Vector2();

    //variable that holds value of location of the mouse cursor
    public Vector3 mousePosition = new Vector3();


    Vector3 mouseToWorldPosition;

    Vector3 desiredAimPosition;



    Vector3 lookVector;

    private void Update()
    {


        mousePosition = Input.mousePosition;

        mousePosition.z = Vector3.Distance(mainCamera.transform.position, this.transform.position);

        mouseToWorldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        mouseToWorldPosition.y = this.transform.position.y;


        playerLookDirection = (mouseToWorldPosition - this.transform.position).normalized;



        

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

    Quaternion rotation;

    private void FixedUpdate()
    {
        rotation = Quaternion.LookRotation(playerLookDirection, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 15.0f);

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



        currentEntity = Instantiate(projectilePrefab, RaycastController.projectileSpawnLocation.transform.position, Quaternion.LookRotation(Vector3.up, gameObject.transform.forward));
        
        currentEntity.GetComponent<Projectile>().currentWeaponInfo = currentWeapon;
        currentEntity.GetComponent<Projectile>().direction = gameObject.transform.forward;

        currentEntity.AddComponent<PlayerProjectile>();
        currentEntity.AddComponent<Light>();
    }


}
