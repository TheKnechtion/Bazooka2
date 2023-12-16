using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PlayerState {INVULNERABLE, VULNERABLE}
public class PlayerInfo:MonoBehaviour, IDamagable
{

    //example of a singleton pattern
    //concept was learned about from user "ericbegue" on the unity forum
    //https://forum.unity.com/threads/best-way-to-find-player-in-the-scene.391663/
    private static PlayerInfo _instance;

    public static PlayerInfo instance
    {
        get
        {
            return _instance;
        }
    }


    public int currentHP = 5;
    public int maximumHP = 5;

    public int shield;


    public float dashCooldown = 3.0f;
    public float movementSpeed = 1.0f;

    //player's currently equipped weapon
    public static WeaponInfo currentWeapon;

    public List<WeaponInfo> ownedWeapons;

    private WeaponController weaponController;


    //store the current player position
    public Vector3 playerPosition = new Vector3();

    //variable that holds value for the x,y center of the screen in pixels
    public Vector2 centerScreen = new Vector2();

    //variable that holds value of location of the mouse cursor
    public Vector3 mousePos = new Vector3();

    //stores the player look direction
    public Vector3 playerLookDirection = new Vector3();

    [SerializeField] private PlayerState state;
    private bool isInvulnerable = false;

    public bool ArmoredTarget { get; set; }

    public event EventHandler OnTakeDamage;
    public static event EventHandler OnPlayerHpChange;
    public static event EventHandler OnPlayerSpawn;
    public static event EventHandler OnPlayerWeaponChange;

    void Awake()
    {
        _instance = this;
    }


    private void Start()
    {
        ownedWeapons = new List<WeaponInfo>();
        weaponController = GetComponent<WeaponController>();

        
        AddWeapon("Bazooka");

        ArmoredTarget = false;

        CameraSwitcher.OnCameraEnable += cameraSwitched;
        CameraSwitcher.OnCameraDisable += cameraReturned;
        GameManager.OnPlayerWin += OnWin;

        state = PlayerState.VULNERABLE;

        OnPlayerSpawn?.Invoke(this, EventArgs.Empty);

        _instance = this;
    }

    private void OnWin(object sender, EventArgs e)
    {
        state = PlayerState.INVULNERABLE;
    }

    /*
     * When the camera changes to look at an objective, we disable
     * the players controls and make them invulnerbale so that
     * they don't take damage,
     */
    private void cameraReturned(object sender, EventArgs e)
    {        
        state = PlayerState.VULNERABLE;
    }

    private void cameraSwitched(object sender, EventArgs e)
    {
        state = PlayerState.INVULNERABLE;
    }

    private void Update()
    {
        _instance = this;


        playerPosition = gameObject.transform.position;


        //sets the player look direction based on the player origin and the mouse cursor location
        playerLookDirection = PlayerManager.playerLookDirection;

        switch (state)
        {
            case PlayerState.INVULNERABLE:
                isInvulnerable= true;
                break;
            case PlayerState.VULNERABLE:
                isInvulnerable= false;
                break;
            default:
                break;
        }

        //if the player's hp drops to 0 or less
        if(currentHP <= 0) 
        {
            //the player object is destroyed and the playerloses method is called
            Die();
        }
    }

    //handles player death
    public void Die()
    {
        //calls the 
        GameObject.Find("GameManager").GetComponent<GameManager>().PlayerLoses();

        //destroy's the player game object
        Destroy(this.gameObject);
    }

    //the method used to pass damage from projectiles
    public void TakeDamage(int passedDamage)
    {
        if (!isInvulnerable)
        {
            OnTakeDamage?.Invoke(this, EventArgs.Empty);
            currentHP -= passedDamage;

            currentHP = (currentHP >= 0) ? currentHP : 0;

            OnPlayerHpChange?.Invoke(this, EventArgs.Empty);
        }        
    }
    
    public void Heal_HP(int amount)
    {
        currentHP += amount;
        currentHP = (maximumHP < currentHP) ? maximumHP : currentHP;

        OnPlayerHpChange?.Invoke(this, EventArgs.Empty);
    }
    

    public void Take_HP(int amount)
    {
        maximumHP -= amount;

        if(currentHP > maximumHP)
        {
            currentHP = maximumHP;
        }

        OnPlayerHpChange?.Invoke(this, EventArgs.Empty);
    }

    public void Give_HP(int amount)
    {
        maximumHP += amount;

        if(maximumHP > 15)
        {
            maximumHP = 15;
        }


        OnPlayerHpChange?.Invoke(this, EventArgs.Empty);
    }



    public void Decrease_Max_Proj(int amount)
    {

        OnPlayerWeaponChange?.Invoke(this, EventArgs.Empty);
    }


    public void AddWeapon(string passedWeapon)
    { 
        WeaponInfo newWeapon = weaponController.MakeWeapon(passedWeapon);
        if (newWeapon != null)
        {
            ownedWeapons.Add(newWeapon);
        }
    }

}
