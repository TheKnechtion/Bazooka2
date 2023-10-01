using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour, IDamagable
{
    //stores the name of the enemy
    string enemyName;


    //these are all public so they can be viewed in the inspector in unity
    //when the game is running, data from the database has visibly seen as
    //successfully passing to each of them.

    //stores the passed in MP
    public int MP;

    //stored the passed in AP
    public int AP;

    //stored the passed in DEF
    public int DEF;

    //stores the passed in HP
    public int health = 2;



    //track the current player position
    Vector3 playerPosition;

    //track the current enemy position
    Vector3 enemyPosition;


    Vector3 enemyLookDirection;

    //the current weapon the enemy has
    WeaponInfo currentEnemyWeapon;


    //controls enemy's weapon
    WeaponController weaponController = new WeaponController();


    //Used by the enemy to track how far the player is
    float enemyPlayerTracker;


    float timeBetweenShots;

    //Used to determine how far the player has to be for the enemy to stop attacking
    float enemyAttackRange_BecomeAggro = 10.0f;

    bool isAggrod;


    //Used to determine how far the player has to be for the enemy to start attacking
    float enemyAttackRange_ExitAggro = 15.0f;

    //holds the reference to the projectile object in the resources folder
    Object projectilePrefab;

    //holds the projectile game object reference
    GameObject currentEntity;

    //holds the material that makes all the enemy projectiles red
    Material projectileMaterial;



    private void Start()
    {
        //ensures that if the room  is beaten, this won't spawn again
        if (GameObject.Find("GameManager").GetComponent<GameManager>().currentNode.isRoomBeaten) { Destroy(this.gameObject); };

        //set the enemy name to that of the game object
        enemyName = this.gameObject.name;

        //create's the correct weapon for an enemy based on the spawned enemy's name
        currentEnemyWeapon = weaponController.MakeWeapon(enemyName);

        //sets the initial state of an enemy to docile
        isAggrod = false;

        //create the red projectile material used by the enemy projectiles
        projectileMaterial = Resources.Load("Red") as Material;

    }

    private void Update()
    {
        //track the enemy position
        enemyPosition = this.transform.position;

        //track the player position
        playerPosition = PlayerInfo.instance.playerPosition;

        //used by the enemy aggro system to see how far the player is from the enemy
        enemyPlayerTracker = Vector3.Distance(playerPosition, enemyPosition);

        //creates an enemy look direction based on the enemy position and the player's current position
        enemyLookDirection = (playerPosition - enemyPosition).normalized;

        //if the player gets within range, the enemy will shoot
        if (enemyPlayerTracker < enemyAttackRange_BecomeAggro) { isAggrod = true; }

        //if the player is out of range, the enemy will stop shooting
        if (enemyPlayerTracker > enemyAttackRange_ExitAggro) { isAggrod = false; }


        //tracks time between shots, stopping at 0.
        timeBetweenShots = (timeBetweenShots > 0) ? timeBetweenShots -= Time.deltaTime : 0;


        //kill if below 0 hp
        if (health <= 0) 
        { 
            GameObject.Find("GameManager").GetComponent<GameManager>().currentNode.isRoomBeaten = true; 
            Destroy(gameObject); 
        }
    }

    private void FixedUpdate()
    {
        //if the enemy is aggro'd, it will shoot at the player
        if (isAggrod == true) { HandleShooting(); }
    }

    private void HandleShooting()
    {
        //manages how quick the player shoots based on their currently equipped weapon
        if (timeBetweenShots <= 0.0f)
        {
            timeBetweenShots = currentEnemyWeapon.timeBetweenProjectileFire;

            Shoot();
        }
    }


    void Shoot()
    {
        //instantiates the projectile prefab
        projectilePrefab = Resources.Load(currentEnemyWeapon.ProjectileName);

        currentEntity = Instantiate(projectilePrefab as GameObject, enemyPosition+enemyLookDirection, new Quaternion(0, 0, 0, 0));
        currentEntity.GetComponent<Projectile>().currentWeaponInfo = currentEnemyWeapon;
        currentEntity.GetComponent<Projectile>().direction = enemyLookDirection;
        currentEntity.GetComponent<Renderer>().material = projectileMaterial;
    }

    //a public method that allows damage to be passed on from the projectile
    public void TakeDamage(int passedDamage)
    {
        health -= passedDamage;
    }




    //set the position of the enemy based on a passed value
    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }
}
