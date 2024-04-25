using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState {IDLE, CHASE, ATTACK, FLEE}
public class EnemyBehavior : MonoBehaviour, IDamagable
{
    #region Every Behavior class has these

    //Scriptable Object stats
    [SerializeField] private EnemySO stats;

    //stores the name of the enemy
    protected string enemyName;
    protected string weaponName;

    //Enemy behavoir state enum
    [SerializeField] protected EnemyState currentState;

    //these are all public so they can be viewed in the inspector in unity
    //when the game is running, data from the database has visibly seen as
    //successfully passing to each of them.

    /*
    //stores the passed in MP
    public int MP;

    //stored the passed in AP
    public int AP;

    //stored the passed in DEF
    public int DEF;
    */

    //stores the passed in HP
    public int maxHealth = 2;
    public int health = 2;

    [Header("Animation Attributes")]
    protected Animator movementAnimator;
    [SerializeField] protected RuntimeAnimatorController animController;
    public Animator MovementAnimator 
    {
        get { return movementAnimator; }
        set { movementAnimator = value; }   
    }

    //track the current player position
    protected Vector3 playerPosition;

    //track the current enemy position
    protected Vector3 enemyPosition;
    protected Vector3 enemyLookDirection;
    protected Quaternion LookRotation;


    //the current weapon the enemy has
    protected WeaponInfo currentEnemyWeapon;
    protected EnemyWeaponController weaponController;

    public GameObject weaponProjectileSpawnNode;


    //controls enemy's weapon
        //protected WeaponController weaponController;
    protected DataBaseWeaponGrabber weaponGrabber;

    [SerializeField] protected bool SetToAttack;
    [SerializeField] protected bool MuteDamageAudio;


    //Used by the enemy to track how far the player is
    protected float enemyPlayerTracker;

    protected Transform targetToLookAt;
    protected Vector3 wallDetectPosition;

    protected float timeBetweenShots;

    //Used to determine how far the player has to be for the enemy to stop attacking    
    public float enemyAttackRange_BecomeAggro = 15.0f;
    public float enemyAttackRange_AttackRange = 12.0f;

    public bool isAggrod, inShootRange;
    private bool lockedOn;

    [SerializeField] protected LayerMask playerMask;
    [SerializeField] protected LayerMask environmentMask;

    protected Navigation nav;
    protected float distanceToPlayer;
    [SerializeField] private float verticalAngle;

    //Used to determine how far the player has to be for the enemy to start attacking
    protected float enemyAttackRange_ExitAggro = 15.0f;

    //holds the reference to the projectile object in the resources folder
        //protected UnityEngine.Object projectilePrefab;
    protected GameObject projectilePrefab;

    //holds the projectile game object reference
    protected GameObject currentEntity;

    public event EventHandler OnTakeDamage;
    public event EventHandler OnDeath;
    protected bool CanDestroy = false;
    protected bool CalledDie = false;

    public bool TargetingEnabled { get; set; }

    protected NavMeshAgent agent;


    [SerializeField] public bool ArmoredTarget { get; set; }
    #endregion

    [Tooltip("Set the max VERTICAL aiming angle for enemies")]
    [Range(0f, 60f)]
    public float maxAimingAngle;

    protected virtual void Awake()
    {
        TargetingEnabled = true;

        movementAnimator = GetComponent<Animator>();

        if (animController)
        {
            RuntimeAnimatorController newCon = Instantiate(animController);
            movementAnimator.runtimeAnimatorController = newCon;
        }
        else
        {
            Debug.LogWarning("! No animController Set !");
        }

        //Pass the weapon script that attacthed to the object
        if (gameObject.TryGetComponent<DataBaseWeaponGrabber>(out DataBaseWeaponGrabber dataWep))
        {
            weaponGrabber = dataWep;
            currentEnemyWeapon = weaponGrabber.MakeWeapon(weaponName);
        }
        else if (gameObject.TryGetComponent<EnemyWeaponController>(out EnemyWeaponController weapCon))
        {
            weaponController = weapCon;
        }

        //sets the initial state of an enemy to docile
        isAggrod = false;
        inShootRange = false;
        lockedOn = false;
        ArmoredTarget = false;
        setStats();
    }
    protected virtual void Start()
    {
        health = maxHealth;

        agent = GetComponent<NavMeshAgent>();

        //ensures that if the room  is beaten, this won't spawn again
        //if (GameObject.Find("GameManager").GetComponent<GameManager>().currentNode.isRoomBeaten) { Destroy(this.gameObject); };

        

        //set the enemy name to that of the game object
        //enemyName = this.gameObject.name;

        //create's the correct weapon for an enemy based on the spawned enemy's name
        //currentEnemyWeapon = weaponController.MakeWeapon(enemyName);
        
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
        
        wallDetectPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z);
        distanceToPlayer = Vector3.Distance(playerPosition, enemyPosition);

        //track the enemy position
        enemyPosition = gameObject.transform.position;

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

        if (PlayerInfo.instance.HeatlthState == PlayerHealthState.DEAD)
        {
            currentState = EnemyState.IDLE;
        }
        else
        {
            HandleEnemyAggro();
        }



        switch (currentState)
        {
            case EnemyState.IDLE:
                lockedOn = false;
                movementAnimator.SetFloat("MovementSpeed", 0);
                break;
            case EnemyState.CHASE:
                lockedOn = false;
                if (nav.isActiveAndEnabled && nav != null)
                {
                    nav.ResumeMovement();

                }
                break;
            case EnemyState.ATTACK:                
                if (nav.isActiveAndEnabled && nav != null)
                {
                    //nav.StopMovement();
                }

                //verticalAngle = GetVerticalAngleToPlayer(transform.forward, playerPosition);
                verticalAngle = AngleToPlayer(distanceToPlayer, enemyPosition, playerPosition);
                if (verticalAngle < maxAimingAngle)
                {
                    //LookRotation = Quaternion.LookRotation(enemyLookDirection);
                    //gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, LookRotation, 4f);

                    //transform.LookAt(targetToLookAt);

                    //SmoothRotate(enemyLookDirection, 4.0f);


                    if (!lockedOn)
                    {
                        StartCoroutine(Smoothrotate(enemyLookDirection, 1.0f, 0.1f));
                    }
                    else
                    {
                        transform.LookAt(targetToLookAt);
                        HandleShooting();
                    }
                }
                break;
            default:
                break;
        }

        #region Debugging
        //testFloat = GetVerticalAngleToPlayer(transform.forward, playerPosition);
        //testFloat = AngleToPlayer(distanceToPlayer, enemyPosition, playerPosition);
        #endregion
    }

    protected virtual void FixedUpdate()
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

    protected void SmoothRotate(Quaternion target, float speed)
    {
        gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, target, speed);
    }

    IEnumerator Smoothrotate(Vector3 target, float speed, float time)
    {
        LookRotation = Quaternion.LookRotation(target);
        float t = 0.0f;
        while (t < time)
        {
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, LookRotation, t);
            t += Time.deltaTime;
            yield return null;
        }

        transform.LookAt(targetToLookAt);
        lockedOn = true;

        yield return null;
    }

    private float AngleToPlayer(float hypDistance, Vector3 enPos, Vector3 targetPos)
    {
        float xzDistance = 0.0f;
        float angle = 0.0f;

        enPos.y = 0;
        targetPos.y = 0;

        xzDistance = Vector3.Distance(enPos, targetPos);

        angle = Mathf.Acos(xzDistance / hypDistance);


        angle *= Mathf.Rad2Deg;
        return angle;
    }
    protected virtual void HandleEnemyAggro()
    {
        //Determines aggro of the enemy
        isAggrod = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_BecomeAggro, playerMask);
        inShootRange = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_AttackRange, playerMask);

        if (isAggrod && TargetingEnabled)
        {
           // transform.LookAt(targetToLookAt);
            //If agroed, we want to chase
            currentState = EnemyState.CHASE;

            Ray wallDetect = new Ray(wallDetectPosition, enemyLookDirection);
            RaycastHit hit;
            Debug.DrawRay(wallDetectPosition, enemyLookDirection.normalized * distanceToPlayer, Color.green);
            if (Physics.Raycast(wallDetect, out hit, distanceToPlayer, environmentMask))
            {
                //Debug.Log("I hit a wall");
                
                if (nav.isActiveAndEnabled && nav != null)
                {
                    nav.MoveToPlayer(isAggrod, false);
                    movementAnimator.SetFloat("MovementSpeed", agent.velocity.magnitude);
                }

            }
            else
            {
                //Debug.Log("Not hitting wall");
                //transform.LookAt(targetToLookAt);
                if (nav.isActiveAndEnabled && nav != null)
                {
                    nav.MoveToPlayer(isAggrod, true);
                    movementAnimator.SetFloat("MovementSpeed", agent.velocity.magnitude);
                }
               

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

    protected virtual void HandleShooting()
    {
        if (weaponController != null)
        {
            weaponController.ShootWeapon();
        }
        else
        {
            if (timeBetweenShots <= 0.0f)
            {
                timeBetweenShots = currentEnemyWeapon.timeBetweenProjectileFire;

                Shoot();
            }
        }
    }


    protected virtual void Shoot()
    {
        //instantiates the projectile prefab
         //projectilePrefab = Resources.Load(currentEnemyWeapon.ProjectileName);

        AudioManager.PlayClipAtPosition(currentEnemyWeapon.weaponSound, weaponProjectileSpawnNode.transform.position);

        //currentEntity = Instantiate(projectilePrefab as GameObject, weaponProjectileSpawnNode.transform.position, Quaternion.LookRotation(Vector3.up, gameObject.transform.forward));
        currentEntity = Instantiate(projectilePrefab, weaponProjectileSpawnNode.transform.position, weaponProjectileSpawnNode.transform.rotation);
        currentEntity.GetComponent<Projectile>().currentWeaponInfo = currentEnemyWeapon;


        currentEntity.GetComponent<Projectile>().direction = enemyLookDirection;
        var light = currentEntity.AddComponent<Light>();
        light.color = Color.red;

    }


    //a public method that allows damage to be passed on from the projectile
    public virtual void TakeDamage(int passedDamage)
    {
        health -= passedDamage;
        if (!MuteDamageAudio)
        {
            AudioManager.PlayPainClipAtPosition(this.transform.position);
        }
        OnTakeDamage?.Invoke(this, EventArgs.Empty);
    }


    //set the position of the enemy based on a passed value
    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }

    //called when enemuy hp is at or below 0
    public virtual void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty); //This is for the enemy death particles to activate

        GameObject.Find("GameManager").GetComponent<EnemySpawnManager>().UpdateEnemyCount();
    }

    private void DeathVisualFinsihed(object sender, EventArgs e)
    {
        CanDestroy = true;
    }

    protected virtual void setStats()
    {
        //Check if we stats isn't NULL
        try
        {
            enemyName = stats.Name;
            weaponName= stats.WeaponName;

            if (weaponController == null)
            {
                if (stats.ProjectilePrefab != null)
                {
                    projectilePrefab = stats.ProjectilePrefab;
                }
                else
                {
                    Debug.LogWarning("! No weapon prefab set !");
                }
            }            
            
            ArmoredTarget = stats.ArmoredTarget;
            maxHealth = stats.Health;
            health = maxHealth;

            enemyAttackRange_BecomeAggro = stats.AggroRange;
            enemyAttackRange_AttackRange = stats.AtackRange;

            playerMask = stats.playerMask;
            environmentMask = stats.environmentMask;
        }
        catch 
        {
            Debug.LogWarning("This enemy has no 'Stats' assigned. Add in prefab.");
        }

    }

    /*protected virtual void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, enemyAttackRange_BecomeAggro);

        Handles.color = Color.red;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, enemyAttackRange_AttackRange);
    }
    */
}
