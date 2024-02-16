using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{

    public static PlayerController _playerController;
    WeaponController weaponController;

    bool pressed;

    //store the current player position
    Vector3 playerPosition;


    //stores the player look direction
    public static Vector3 playerLookDirection;

    Camera mainCamera;

    GameObject projectilePrefab;
    GameObject currentEntity;

    float timeBetweenShots = 0.0f;

    float scrollValue;

    int ownedWeaponCount;
    int weaponIndex = 0;

    public static int activeProjectiles = 0;
    public int checkProj;

    List<WeaponInfo> playerOwnedWeapons;



    public bool isCarryingObjectOnBack = false;
    public bool CanCarryObjectOnBack = true;
    public bool isDraggingObject = false;

    public GameObject carriedObject; 

    public static event EventHandler OnPlayerWeaponChange;
    public static event EventHandler<OnWeaponSwitchEventArgs> OnWeaponChange;
    public static event EventHandler OnPlayerShoot;
    public static event EventHandler OnPlayerDetonate;
    public static event EventHandler OnPlayerActivatePress;
    public static event EventHandler OnPlayerSpawn;

    public class OnWeaponSwitchEventArgs : EventArgs
    {
        public KeyCode keyPressed;
        public bool Qpressed;
        public bool Epressed;
    }


    Vector3 projectionVector;
    private void Awake()
    {
        try
        {
            gameObject.transform.position = GameObject.Find("PlayerSpawnNode").transform.position;
            Destroy(GameObject.Find("PlayerSpawnNode"));
        }
        catch 
        { 
        
        }
        


        DontDestroyOnLoad(this.gameObject);


        _playerController = new PlayerController();


        mainCamera = Camera.main;

        weaponController = GetComponent<WeaponController>();

        weaponController.FinishedWeaponChange += UpdateCurrentWeaponInfo;
        Ammo_PickUp_Item.pickedUpAmmo += UpdateCurrentWeaponInfo;
        MaxProjOnScreen_Increase_PickUp.pickedUpAmmo += UpdateCurrentWeaponInfo;

    }

    public static Vector2 mouseDelta;

    public static event EventHandler OnPlayerAim;
    public static event EventHandler OnPlayerStopAim;


    private void Start()
    {

        CameraSwitcher.OnCameraEnable += cameraSwitched;
        CameraSwitcher.OnCameraDisable += cameraReturned;

        _playerController.PlayerActions.Shoot.performed += HandleShooting;
        _playerController.PlayerActions.Shoot.canceled -= HandleShooting;

        _playerController.PlayerActions.Detonate.performed += DetonateProjectiles;
        _playerController.PlayerActions.Detonate.canceled -= DetonateProjectiles;

        _playerController.PlayerActions.MousePosition.performed += mouseMove => mouseDelta = mouseMove.ReadValue<Vector2>();
        _playerController.PlayerActions.MousePosition.canceled += mouseMove => mouseDelta = Vector2.zero;

        _playerController.PlayerActions.Aim.performed += PlayerAim;
        _playerController.PlayerActions.Aim.canceled += PlayerStopAim;

        _playerController.PlayerActions.ChangeWeapon.performed += ChangedWeapon;





        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        //projectionVector = this.transform.position - AimCursor.cursorLocation;

        //CheckWeaponChange();

        weaponController.InitializeWeaponUI();
    }

    public static RangedWeapon currentWeapon_ref;

    private void UpdateCurrentWeaponInfo(object sender, EventArgs e)
    {
        currentWeapon_ref = weaponController.currentWeapon;
        OnPlayerWeaponChange?.Invoke(this, EventArgs.Empty); //Used for detecting weapon switch
    }


    private void ChangedWeapon(InputAction.CallbackContext obj)
    {
        OnWeaponChange?.Invoke(this, new OnWeaponSwitchEventArgs {
            Qpressed = Keyboard.current.qKey.wasPressedThisFrame,
            Epressed = Keyboard.current.eKey.wasPressedThisFrame
        }); //Used for detecting weapon switch
    }

    private void cameraReturned(object sender, EventArgs e)
    {
        _playerController.PlayerActions.Enable();
    }

    private void cameraSwitched(object sender, EventArgs e)
    {
        _playerController.PlayerActions.Disable();
    }


    private void PlayerAim(InputAction.CallbackContext e)
    {
        OnPlayerAim?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerStopAim(InputAction.CallbackContext e)
    {
        OnPlayerStopAim?.Invoke(this, EventArgs.Empty);
    }


    //variable that holds value for the x,y center of the screen in pixels
    public Vector2 centerScreen = new Vector2();

    //variable that holds value of location of the mouse cursor
    public Vector3 mousePosition;


    Vector3 mouseToWorldPosition;

    Vector3 desiredAimPosition;

    bool playerSpawn = true;

    Vector3 lookVector;

    Vector3 cursorLocation;

    private void Update()
    {


        checkProj = activeProjectiles;



        playerPosition = gameObject.transform.position;
        playerPosition.y = 1.0f;

        //CheckWeaponChange();


        if(_playerController.PlayerActions.Activate.IsPressed())
        {
            OnPlayerActivatePress?.Invoke(this, EventArgs.Empty);
        }


        //tracks time between shots, stopping at 0.
        timeBetweenShots -= Time.deltaTime;

    }

    Quaternion rotation;

    private void FixedUpdate()
    {

        playerLookDirection = RaycastController.playerLookDirection;

        playerLookDirection = new Vector3(playerLookDirection.x,0.0f, playerLookDirection.z);

        rotation = Quaternion.LookRotation(playerLookDirection, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 65.0f);

        if (playerSpawn)
        {
            OnPlayerSpawn?.Invoke(this, EventArgs.Empty);
            playerSpawn = false;
        }

    }

    private void LateUpdate()
    {


    }

    float cooldownTime = 0.1f;
    float nextInputTime = 0.0f;


    void DetonateProjectiles(InputAction.CallbackContext e)
    {
        OnPlayerDetonate?.Invoke(this, EventArgs.Empty);
    }


    public void GainAmmo(int amountToGain)
    {
        weaponController.currentWeapon.currentAmmo = (weaponController.currentWeapon.currentAmmo + amountToGain <= weaponController.currentWeapon.maxAmmo) ? weaponController.currentWeapon.currentAmmo + amountToGain : weaponController.currentWeapon.maxAmmo;   
    }

    public void IncreaseMaxProjOnScreen(int amountToGain)
    {
        weaponController.currentWeapon.maxActiveProjectiles += amountToGain;
    }

    private void HandleShooting(InputAction.CallbackContext e)
    {

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        if (activeProjectiles < weaponController.currentWeapon.maxActiveProjectiles && timeBetweenShots <= 0)
        {
            timeBetweenShots = weaponController.currentWeapon.fireRate;
            weaponController.PlayerShootWeapon();
            //Shoot();
        }
    }




    private void OnEnable()
    {
        //begins player movement functions
        _playerController.PlayerActions.Enable();
        _playerController.PlayerInteract.Enable();

    }


    public bool CheckPlayerBack()
    {
        if(isCarryingObjectOnBack)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    private void OnDisable()
    {
        //ends player movement functions
        _playerController.PlayerActions.Disable();
        _playerController.PlayerInteract.Disable();
    }

    //List<GameObject> currentProjectiles = new List<GameObject> ();

    Vector3 projectileSpawnLocation;

    /*
    void Shoot()
    {
        activeProjectiles++;

        currentWeapon = PlayerInfo.currentWeapon;

        OnPlayerShoot?.Invoke(this, EventArgs.Empty);

        projectilePrefab = (GameObject)Resources.Load(currentWeapon.ProjectileName);

        projectileSpawnLocation = RaycastController.projectileSpawnLocation.transform.position;


        AudioManager.PlayClipAtPosition("bazooka_fire",projectileSpawnLocation);

        currentEntity = Instantiate(projectilePrefab, projectileSpawnLocation, Quaternion.LookRotation(Vector3.up, gameObject.transform.forward));
        
        currentEntity.GetComponent<Projectile>().currentWeaponInfo = currentWeapon;
        currentEntity.GetComponent<Projectile>().direction = RaycastController.shootVector;

        currentEntity.AddComponent<PlayerProjectile>();
        currentEntity.AddComponent<Light>();
    }
    */


}
