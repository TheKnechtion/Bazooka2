using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState {IDLE, CHASE, ATTACK}
public class EnemyBehavior : MonoBehaviour, IDamagable
{
    #region Every Behavior class has these
    //stores the name of the enemy
    protected string enemyName;

    //Enemy behavoir state enum
    [SerializeField] protected EnemyState currentState;


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

    protected Animator movementAnimator;

    //track the current player position
    protected Vector3 playerPosition;

    //track the current enemy position
    protected Vector3 enemyPosition;


    protected Vector3 enemyLookDirection;

    //the current weapon the enemy has
    protected WeaponInfo currentEnemyWeapon;

    public GameObject weaponProjectileSpawnNode;


    //controls enemy's weapon
    protected WeaponController weaponController;

    [SerializeField] protected bool SetToAttack;


    //Used by the enemy to track how far the player is
    protected float enemyPlayerTracker;

    protected Transform targetToLookAt;
    private Vector3 wallDetectPosition;

    protected float timeBetweenShots;

    //Used to determine how far the player has to be for the enemy to stop attacking    
    [SerializeField] protected float enemyAttackRange_BecomeAggro = 15.0f;
    [SerializeField] protected float enemyAttackRange_AttackRange = 12.0f;
    public bool isAggrod, inShootRange;

    [SerializeField] protected LayerMask playerMask;
    [SerializeField] protected LayerMask environmentMask;

    protected Navigation nav;
    private float distanceToPlayer;


    //Used to determine how far the player has to be for the enemy to start attacking
    protected float enemyAttackRange_ExitAggro = 15.0f;

    //holds the reference to the projectile object in the resources folder
    protected UnityEngine.Object projectilePrefab;

    //holds the projectile game object reference
    protected GameObject currentEntity;


    public event EventHandler OnTakeDamage;
    public event EventHandler OnDeath;
    protected bool CanDestroy = false;


    protected NavMeshAgent agent;

    protected bool CalledDie = false;

    [SerializeField] public bool ArmoredTarget { get; set; }
    #endregion


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        movementAnimator = GetComponent<Animator>();

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

        ArmoredTarget= false;

        nav = GetComponent<Navigation>();
        nav.OnStoppedMoving += StoppedMoving;
        //nav.stoppingDistance = enemyAttackRange_AttackRange;

        targetToLookAt = PlayerInfo.instance.gameObject.transform;

        currentState = EnemyState.IDLE;

        wallDetectPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z);
        //Debug.Log("My Pos: "+gameObject.transform.position);
    }

    

    protected virtual void Update()
    {
        inShootRange = false;
        isAggrod = false;
        
        wallDetectPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z);
        distanceToPlayer = Vector3.Distance(playerPosition, enemyPosition);

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

        HandleEnemyAggro();

        switch (currentState)
        {
            case EnemyState.IDLE:
                movementAnimator.SetFloat("MovementSpeed", 0);
                break;
            case EnemyState.CHASE:
                nav.ResumeMovement();
                break;
            case EnemyState.ATTACK:
                nav.StopMovement();
                transform.LookAt(targetToLookAt);
                HandleShooting();
                break;
            default:
                break;
        }
    }

    protected void FixedUpdate()
    {

        //tracks time between shots, stopping at 0.
        timeBetweenShots = (timeBetweenShots > 0) ? timeBetweenShots -= Time.deltaTime : 0;


        //kill if below 0 hp
        if (health <= 0)
        {
            if (!CalledDie)
            {
                Die();
                CalledDie = true;
            }
        }
    }

    protected virtual void HandleEnemyAggro()
    {
        //Determines aggro of the enemy
        isAggrod = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_BecomeAggro, playerMask);
        inShootRange = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_AttackRange, playerMask);

        if (isAggrod)
        {
           // transform.LookAt(targetToLookAt);
            //If agroed, we want to chase
            currentState = EnemyState.CHASE;

            Ray wallDetect = new Ray(wallDetectPosition, enemyLookDirection);
            RaycastHit hit;
            Debug.DrawRay(wallDetectPosition, enemyLookDirection.normalized * distanceToPlayer, Color.green);
            if (Physics.Raycast(wallDetect, out hit, distanceToPlayer, environmentMask))
            {
                Debug.Log("I hit a wall");
                nav.MoveToPlayer(isAggrod, false);
                movementAnimator.SetFloat("MovementSpeed", agent.velocity.magnitude);

            }
            else
            {
                Debug.Log("Not hitting wall");
                //transform.LookAt(targetToLookAt);
                nav.MoveToPlayer(isAggrod, true);   
                movementAnimator.SetFloat("MovementSpeed", agent.velocity.magnitude);

                if (inShootRange && SetToAttack)
                {
                    currentState= EnemyState.ATTACK;
                    //HandleShooting();
                }
            }
        }
    }

    private void StoppedMoving(object sender, EventArgs e)
    {
        currentState = EnemyState.IDLE;
    }

    /*
    private void FixedUpdate()
    {
        //if the enemy is aggro'd, it will shoot at the player
        //if (isAggrod == true) { HandleShooting(); }
    }
    */

    protected virtual void HandleShooting()
    {
        //manages how quick the player shoots based on their currently equipped weapon
        if (timeBetweenShots <= 0.0f)
        {
            timeBetweenShots = currentEnemyWeapon.timeBetweenProjectileFire;

            Shoot();
        }
    }


    protected virtual void Shoot()
    {
        //instantiates the projectile prefab
        projectilePrefab = Resources.Load(currentEnemyWeapon.ProjectileName);

        currentEntity = Instantiate(projectilePrefab as GameObject, weaponProjectileSpawnNode.transform.position, Quaternion.LookRotation(Vector3.up, gameObject.transform.forward));
        currentEntity.GetComponent<Projectile>().currentWeaponInfo = currentEnemyWeapon;


        currentEntity.GetComponent<Projectile>().direction = enemyLookDirection;
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
        
        Destroy(gameObject);  //There is a new Death script that handles destroiyng object and visuals
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
