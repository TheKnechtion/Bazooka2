using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

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
    public int checkProj;

    List<WeaponInfo> playerOwnedWeapons;

    public static event EventHandler OnPlayerWeaponChange;
    public static event EventHandler OnPlayerShoot;
    public static event EventHandler OnPlayerDetonate;
    public static event EventHandler OnPlayerActivatePress;
    public static event EventHandler OnPlayerSpawn;


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
    }


    private void Start()
    {
    
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        CameraSwitcher.OnCameraEnable += cameraSwitched;
        CameraSwitcher.OnCameraDisable += cameraReturned;

        _playerController.PlayerActions.Shoot.performed += HandleShooting;
        _playerController.PlayerActions.Shoot.canceled -= HandleShooting;

        _playerController.PlayerActions.Detonate.performed += DetonateProjectiles;
        _playerController.PlayerActions.Detonate.canceled -= DetonateProjectiles;


        //projectionVector = this.transform.position - AimCursor.cursorLocation;

        //CheckWeaponChange();
    }

    private void cameraReturned(object sender, EventArgs e)
    {
        _playerController.PlayerActions.Enable();
    }

    private void cameraSwitched(object sender, EventArgs e)
    {
        _playerController.PlayerActions.Disable();
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
        //mousePosition = AimCursor.cursorLocation;

        //mousePosition = mousePosition - projectionVector;

        //Debug.Log(projectionVector);

        //mousePosition.z = Vector3.Distance(mainCamera.transform.position, this.transform.position);

        //mouseToWorldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        //mouseToWorldPosition.y = this.transform.position.y;

        cursorLocation = AimCursor.cursorVector;

        playerLookDirection = (Vector3.Dot(cursorLocation, this.transform.forward) * transform.forward + Vector3.Dot(cursorLocation, this.transform.right) * transform.right).normalized;

        //playerLookDirection = new Vector3(playerLookDirection.x, 0, playerLookDirection.z).normalized;

        checkProj = activeProjectiles;

        if (playerSpawn)
        {
            currentWeapon = PlayerInfo.instance.ownedWeapons[0];
            OnPlayerSpawn?.Invoke(this, EventArgs.Empty);
            playerSpawn = false;
        }



        playerPosition = gameObject.transform.position;
        playerPosition.y = 1.0f;

        CheckWeaponChange();






        if(_playerController.PlayerActions.Activate.IsPressed())
        {
            OnPlayerActivatePress?.Invoke(this, EventArgs.Empty);
        }


        //tracks time between shots, stopping at 0.
        timeBetweenShots = (timeBetweenShots > 0) ? timeBetweenShots -= Time.deltaTime : 0;





    }

    Quaternion rotation;

    private void FixedUpdate()
    {


        rotation = Quaternion.LookRotation(playerLookDirection, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 65.0f);

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


        PlayerInfo.currentWeapon = currentWeapon;

    }

    void DetonateProjectiles(InputAction.CallbackContext e)
    {
        OnPlayerDetonate?.Invoke(this, EventArgs.Empty);
    }



    private void HandleShooting(InputAction.CallbackContext e)
    {

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

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

        currentWeapon = PlayerInfo.currentWeapon;

        OnPlayerShoot?.Invoke(this, EventArgs.Empty);

        projectilePrefab = (GameObject)Resources.Load(currentWeapon.ProjectileName);



        currentEntity = Instantiate(projectilePrefab, RaycastController.projectileSpawnLocation.transform.position, Quaternion.LookRotation(Vector3.up, gameObject.transform.forward));
        
        currentEntity.GetComponent<Projectile>().currentWeaponInfo = currentWeapon;
        currentEntity.GetComponent<Projectile>().direction = gameObject.transform.forward;

        currentEntity.AddComponent<PlayerProjectile>();
        currentEntity.AddComponent<Light>();
    }


}
