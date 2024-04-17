using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class BehaviorTankBoss : EnemyBehavior
{
    private NavigationTankBoss tankNav;
    [SerializeField] private GameObject turret;

    [SerializeField] private ParticleSystem ShootFX;

    private Quaternion bodyRotation;

    public static event EventHandler OnTankKilled;
    public event EventHandler InstanceTankKilled;

    public static UnityEvent OnTankKilledUEvent;

    [SerializeField] private HealthBar healthBar;

    [SerializeField] private GameObject MountedEnemy;

    private bool initialAggro;
    private bool DavinciAtkActive, CanDavinci;
    private bool SoldierMountActive;

    private ResusableAudioController TankAudioController;

    private GameObject UI_Reference;
    private HealthBar_UI UI_HealthBar;

    protected override void Start()
    {
        initialAggro = false;
        DavinciAtkActive = false;
        CanDavinci = false;
        SoldierMountActive = false;

        TankAudioController = transform.Find("Audio").GetComponent<ResusableAudioController>();

        UI_Reference = GameObject.Find("Canvas");
        UI_HealthBar = UI_Reference.GetComponentInChildren<HealthBar_UI>();
        UI_HealthBar.UpdateHealthbar(health / maxHealth);

        setStats();
        agent = GetComponent<NavMeshAgent>();

        if (MountedEnemy != null)
        {
            MountedEnemy.SetActive(false);
        }


        //ensures that if the room  is beaten, this won't spawn again
        //if (GameObject.Find("GameManager").GetComponent<GameManager>().currentNode.isRoomBeaten) { Destroy(this.gameObject); };

        //Pass the weapon script that attacthed to the object
        weaponGrabber = gameObject.GetComponent<DataBaseWeaponGrabber>();
            //weaponController = gameObject.GetComponent<WeaponController>();

        //set the enemy name to that of the game object
        //enemyName = this.gameObject.name;

        //create's the correct weapon for an enemy based on the spawned enemy's name
        //currentEnemyWeapon = weaponController.MakeWeapon(enemyName);
        currentEnemyWeapon = weaponGrabber.MakeWeapon(weaponName);

        //sets the initial state of an enemy to docile
        isAggrod = false;
        inShootRange = false;

        tankNav = GetComponent<NavigationTankBoss>();

        targetToLookAt = PlayerInfo.instance.gameObject.transform;
        
        healthBar.ChangeStatus(health, maxHealth);

        //ArmoredTarget = true;
        currentState = EnemyState.IDLE;
    }

    protected override void Update()
    {
        

        bodyRotation = gameObject.transform.rotation;

        inShootRange = false;
        isAggrod = false;

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

        if (PlayerInfo.instance != null && PlayerInfo.instance.HeatlthState != PlayerHealthState.DEAD)
        {
            HandleEnemyAggro();
        }
        else
        {
            currentState = EnemyState.IDLE;
        }

        switch (currentState)
        {
            case EnemyState.IDLE:
                turret.transform.rotation = bodyRotation;
                break;
            case EnemyState.CHASE:
                turret.transform.LookAt(targetToLookAt);
                break;
            case EnemyState.ATTACK:
                if (!DavinciAtkActive)
                {
                    turret.transform.LookAt(targetToLookAt);
                }
                break;
            default:
                break;
        }

        //Only play once aggroed
        if (initialAggro)
        {
            if (!agent.isStopped)
            {
                TankAudioController.PlaySound("Chase");
            }
            else
            {
                TankAudioController.StopSound("Chase");
            }
        }

        if (PercentHealth(0.5f) && !SoldierMountActive)
        {
            MountedEnemy.SetActive(true);
            SoldierMountActive = true;
        }

        if (PercentHealth(0.3f) && !CanDavinci)
        {
            CanDavinci = true;
        }
    }

    protected override void HandleEnemyAggro()
    {
        //Determines aggro of the enemy
        isAggrod = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_BecomeAggro, playerMask);
        inShootRange = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_AttackRange, playerMask);

        
        if (isAggrod)
        {
            if (!initialAggro)
            {
                UI_HealthBar.ToggleHpBar(true);
                initialAggro = true;
            }
                

            //If agroed, we want to chase
            currentState = EnemyState.CHASE;

            Ray wallDetect = new Ray(gameObject.transform.position, enemyLookDirection);
            RaycastHit hit;

            tankNav.MoveToPlayer(isAggrod, false);

            #region Detecting Wall Raycast
            //Debug.DrawRay(gameObject.transform.position, enemyLookDirection.normalized, Color.black);
            //if (Physics.Raycast(wallDetect, out hit, 10, environmentMask))
            //{
            //    //Debug.Log("I hit a wall");
            //    nav.MoveToPlayer(isAggrod, false);

            //}
            //else
            //{
            //    nav.MoveToPlayer(isAggrod, true);
            //    //Debug.Log("Not hitting wall");
            //}
            #endregion

            if (SetToAttack)
            {
                Physics.Raycast(weaponProjectileSpawnNode.transform.position, transform.forward, out hit);

                if (hit.transform != null)
                {
                    if (inShootRange && hit.collider.gameObject.tag == "Player")
                    {
                        currentState = EnemyState.ATTACK;
                        HandleShooting();
                    }
                }                
            }
        }
        else
        { currentState= EnemyState.IDLE; }
    }

    protected override void HandleShooting()
    {
        //manages how quick the player shoots based on their currently equipped weapon
        if (timeBetweenShots <= 0.0f)
        {
            timeBetweenShots = currentEnemyWeapon.timeBetweenProjectileFire;

            if (CanDavinci)
            {
                int randomAttack = UnityEngine.Random.Range(1, 3);

                switch (randomAttack)
                {
                    case 1:
                        StartCoroutine(stopandShoot());
                        break;
                    case 2:
                        StartCoroutine(DavinciAttack(3.0f, 3f));
                        break;
                    default:
                        StartCoroutine(stopandShoot());
                        break;
                }
            }
            else
            {
               StartCoroutine(stopandShoot());
            }
        }
    }

    private IEnumerator stopandShoot()
    {
        //Stop the tank
        //Look at player
        //Shoot
        //Resume movement

        tankNav.stopMovement();

        yield return new WaitForSeconds(0.5f);
        Shoot();
        yield return new WaitForSeconds(1.5f);
        tankNav.resumeMovement();

        yield return null;
    }

    private IEnumerator DavinciAttack(float time, float spinSpeed = 1)
    {
        DavinciAtkActive = true;
        

        Quaternion newRotation = new Quaternion();
        float t = 0.0f;
        bool shotsFired = false;

        while (t < time)
        {
            if (!shotsFired)
            {
                shotsFired = true;
                StartCoroutine(RepeatedShooting(0.4f, 5));
            }
            newRotation.eulerAngles += new Vector3(0, 1 * spinSpeed, 0);
            turret.transform.rotation = newRotation;

            t += Time.deltaTime;
            yield return null;
        }

        DavinciAtkActive = false;

        yield return null;
    }

    private IEnumerator RepeatedShooting(float shotDelay, int shotCount)
    {
        float t = 0.0f;

        for (int i = 0; i < shotCount; i++)
        {
            Shoot();

            if (!DavinciAtkActive)
            {
                break;
            }

            while (t < shotDelay || !DavinciAtkActive)
            {
                t += Time.deltaTime;
                yield return null;
            }

            t = 0.0f;
        }

        yield return null;
    }

    protected override void Shoot()
    {
        //instantiates the projectile prefab
            //projectilePrefab = Resources.Load(currentEnemyWeapon.ProjectileName);

        AudioManager.PlayClipAtPosition(currentEnemyWeapon.weaponSound, weaponProjectileSpawnNode.transform.position);

        if (ShootFX != null)
        {
            ShootFX.Play();
        }

        //currentEntity = Instantiate(projectilePrefab, weaponProjectileSpawnNode.transform.position, Quaternion.LookRotation(Vector3.up, enemyLookDirection));
        currentEntity = Instantiate(projectilePrefab, weaponProjectileSpawnNode.transform.position, weaponProjectileSpawnNode.transform.rotation);
        currentEntity.GetComponent<Projectile>().currentWeaponInfo = currentEnemyWeapon;


        //currentEntity.GetComponent<Projectile>().direction = enemyLookDirection;
        currentEntity.GetComponent<Projectile>().direction = weaponProjectileSpawnNode.transform.forward;

        var light = currentEntity.AddComponent<Light>();
        light.color = Color.red;
    }

    public override void TakeDamage(int passedDamage)
    {
        base.TakeDamage(passedDamage);
        UI_HealthBar.UpdateHealthbar((float)health / (float)maxHealth);

        healthBar.ChangeStatus(health, maxHealth);
        //float deltDamage = health/maxHealth;
        //healthForeground.fillAmount = deltDamage;
    }

    public override void Die()
    {
        OnTankKilled.Invoke(this, EventArgs.Empty);
        InstanceTankKilled.Invoke(this, EventArgs.Empty);
        UI_HealthBar.ResetHealthBar();

        base.Die();
    }

    private bool PercentHealth(float percentThreshold)
    {
        return ((float)health / (float)maxHealth) < percentThreshold; 
    }

}
