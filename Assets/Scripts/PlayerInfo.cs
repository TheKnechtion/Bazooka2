using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInfo:MonoBehaviour, IDamagable
{

    //example of a singleton pattern
    //concept was learned about from user "ericbegue" on the unity forum
    //https://forum.unity.com/threads/best-way-to-find-player-in-the-scene.391663/
    static PlayerInfo _instance;

    public static PlayerInfo instance
    {
        get
        {
            return _instance;
        }
    }    

    private WeaponController weaponController;

    public int currentHP;
    public int maximumHP = 50;

    public int shield;


    public float dashCooldown = 3.0f;
    public float movementSpeed = 0.1f;

    //player's currently equipped weapon
    public WeaponInfo currentWeapon;

    public List<WeaponInfo> ownedWeapons = new List<WeaponInfo>();


    //store the current player position
    public Vector3 playerPosition = new Vector3();

    //variable that holds value for the x,y center of the screen in pixels
    public Vector2 centerScreen = new Vector2();

    //variable that holds value of location of the mouse cursor
    public Vector3 mousePos = new Vector3();

    //stores the player look direction
    public Vector3 playerLookDirection = new Vector3();





    private void Start()
    {
        currentHP = maximumHP;
    }

    private void Update()
    {
        _instance = this;

        //get the position of the center of the screen
        centerScreen = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);


        playerPosition = gameObject.transform.position;


        //current x,y vector of how far away the cursor is from the bottom left of the screen
        mousePos = Input.mousePosition;


        //translate the mouse coordinates to be based around the center of the screen
        mousePos.x -= centerScreen.x;
        mousePos.y -= centerScreen.y;

        //sets the player look direction based on the player origin and the mouse cursor location
        playerLookDirection = new Vector3(playerPosition.x + mousePos.x, playerPosition.y, playerPosition.z + mousePos.y).normalized;
        Debug.Log("Database count "+WeaponDatabase.Instance().dataCount);
    }

    private void Awake()
    {
        _instance = this;
        weaponController= GetComponent<WeaponController>();
    }

    public void TakeDamage(int passedDamage)
    {
        currentHP -= passedDamage;
    }


    //Call method to add any WeaponInfo to list Weapons
    public void AddWeapon(string passedWeapon)
    {
        WeaponInfo newWep = weaponController.MakeWeapon(passedWeapon);
        
        if (newWep != null) { ownedWeapons.Add(newWep); }

        //For now, the player uses the FIRST weapon they aquire
        currentWeapon = ownedWeapons.First();
    }
}
