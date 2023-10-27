using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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


    public int currentHP = 15;
    public int maximumHP = 15;

    public int shield;


    public float dashCooldown = 3.0f;
    public float movementSpeed = 1.0f;

    //player's currently equipped weapon
    public WeaponInfo currentWeapon;

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

    public event EventHandler OnTakeDamage;
    public static event EventHandler OnPlayerHpChange;

    private void Start()
    {
        ownedWeapons = new List<WeaponInfo>();
        weaponController = GetComponent<WeaponController>();
        currentHP = maximumHP;
        OnPlayerHpChange?.Invoke(this, EventArgs.Empty);
        AddWeapon("Bazooka");

        
    }

    private void Update()
    {
        _instance = this;


        playerPosition = gameObject.transform.position;


        //sets the player look direction based on the player origin and the mouse cursor location
        playerLookDirection = PlayerManager.playerLookDirection;


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

    private void Awake()
    {
        _instance = this;
    }

    //the method used to pass damage from projectiles
    public void TakeDamage(int passedDamage)
    {
        OnTakeDamage?.Invoke(this, EventArgs.Empty);
        currentHP -= passedDamage;

        currentHP = (currentHP >= 0) ? currentHP : 0;

        OnPlayerHpChange?.Invoke(this,EventArgs.Empty);
    }


    
    public void Heal_HP(int amount)
    {
        currentHP += amount;
        currentHP = (maximumHP < currentHP) ? maximumHP : currentHP;

        OnPlayerHpChange?.Invoke(this, EventArgs.Empty);
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
