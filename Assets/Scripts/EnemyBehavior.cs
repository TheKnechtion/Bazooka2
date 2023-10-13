using System;
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
    WeaponController weaponController;

    [SerializeField] private bool SetToAttack;


    //Used by the enemy to track how far the player is
    float enemyPlayerTracker;

    float timeBetweenShots;

    //Used to determine how far the player has to be for the enemy to stop attacking    
    float enemyAttackRange_BecomeAggro = 10.0f;
    float enemyAttackRange_AttackRange = 9.0f;
    bool isAggrod, inShootRange;

    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask environmentMask;

    private Navigation nav;


    //Used to determine how far the player has to be for the enemy to start attacking
    float enemyAttackRange_ExitAggro = 15.0f;

    //holds the reference to the projectile object in the resources folder
    UnityEngine.Object projectilePrefab;

    //holds the projectile game object reference
    GameObject currentEntity;

    //holds the material that makes all the enemy projectiles red
    Material projectileMaterial;

    public event EventHandler OnTakeDamage;
    public event EventHandler OnDeath;
    private bool CanDestroy = false;

    private void Start()
    {
        //ensures that if the room  is beaten, this won't spawn again
        //if (GameObject.Find("GameManager").GetComponent<GameManager>().currentNode.isRoomBeaten) { Destroy(this.gameObject); };

        //Pass the weapon script that attacthed to the object
        weaponController = gameObject.GetComponent<WeaponController>();

        //set the enemy name to that of the game object
        enemyName = this.gameObject.name;

        //create's the correct weapon for an enemy based on the spawned enemy's name
        currentEnemyWeapon = weaponController.MakeWeapon(enemyName);

        //sets the initial state of an enemy to docile
        isAggrod = false;
        inShootRange = false;

        nav = GetComponent<Navigation>();

        //create the red projectile material used by the enemy projectiles
        projectileMaterial = Resources.Load("Red") as Material;

    }


    bool CalledDie = false;
    private void Update()
    {
        inShootRange = false;
        isAggrod= false;

        //track the enemy position
        enemyPosition = this.transform.position;

        //track the player position
        playerPosition = PlayerInfo.instance.playerPosition;

        //used by the enemy aggro system to see how far the player is from the enemy
        enemyPlayerTracker = Vector3.Distance(playerPosition, enemyPosition);

        //creates an enemy look direction based on the enemy position and the player's current position
        enemyLookDirection = (playerPosition - enemyPosition).normalized;

        //if the player gets within range, the enemy will shoot
            //if (enemyPlayerTracker < enemyAttackRange_BecomeAggro) { isAggrod = true; }

        //if the player is out of range, the enemy will stop shooting
            //if (enemyPlayerTracker > enemyAttackRange_ExitAggro) { isAggrod = false; }


        //Determines aggro of the enemy
        isAggrod = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_BecomeAggro, playerMask);
        inShootRange = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_AttackRange, playerMask);

        if (isAggrod)
        {
            Ray wallDetect = new Ray(gameObject.transform.position, enemyLookDirection);
            RaycastHit hit;
            Debug.DrawRay(gameObject.transform.position, enemyLookDirection.normalized, Color.black);
            if (Physics.Raycast(wallDetect, out hit, 10, environmentMask))
            {
                //Debug.Log("I hit a wall");
                nav.MoveToPlayer(isAggrod, false);
            }
            else
            {
                nav.MoveToPlayer(isAggrod, true);
                //Debug.Log("Not hitting wall");
            }

            if (SetToAttack)
            {
                if (inShootRange && hit.collider == null)
                {
                    //Debug.Log("I should be shooting");
                    HandleShooting();
                }
            }
            
            
        }


        //tracks time between shots, stopping at 0.
        timeBetweenShots = (timeBetweenShots > 0) ? timeBetweenShots -= Time.deltaTime : 0;


        //kill if below 0 hp
        if (health <= 0) 
        {
            if (!CalledDie)
            {
                Die();
                CalledDie= true;
            }           
        }
    }

    private void FixedUpdate()
    {
        //if the enemy is aggro'd, it will shoot at the player
        //if (isAggrod == true) { HandleShooting(); }
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

        currentEntity = Instantiate(projectilePrefab as GameObject, enemyPosition+ 2*(enemyLookDirection), new Quaternion(0, 0, 0, 0));
        currentEntity.GetComponent<Projectile>().currentWeaponInfo = currentEnemyWeapon;
        currentEntity.GetComponent<Projectile>().direction = enemyLookDirection;
        currentEntity.GetComponent<Renderer>().material = projectileMaterial;
        var light = currentEntity.AddComponent<Light>();
        light.color = Color.red;

    }

    //a public method that allows damage to be passed on from the projectile
    public void TakeDamage(int passedDamage)
    {
        health -= passedDamage;
        OnTakeDamage?.Invoke(this, EventArgs.Empty);
    }


    //set the position of the enemy based on a passed value
    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }

    //called when enemuy hp is at or below 0
    public void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty); //This is for the enemy death particles to activate

        GameObject.Find("GameManager").GetComponent<EnemySpawnManager>().UpdateEnemyCount();
        
        //Destroy(gameObject);  There is a new Death script that handles destroiyng object and visuals
    }

    private void DeathVisualFinsihed(object sender, EventArgs e)
    {
        CanDestroy = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, enemyAttackRange_BecomeAggro);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, enemyAttackRange_AttackRange);
    }
}
