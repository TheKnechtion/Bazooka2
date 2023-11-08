using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorTankBoss : EnemyBehavior
{
    private NavigationTankBoss tankNav;
    [SerializeField] private GameObject turret;

    private Quaternion bodyRotation;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

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

        tankNav = GetComponent<NavigationTankBoss>();

        targetToLookAt = PlayerInfo.instance.gameObject.transform;

        ArmoredTarget = true;
        

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

        HandleEnemyAggro();

        switch (currentState)
        {
            case EnemyState.IDLE:
                turret.transform.rotation = bodyRotation;
                break;
            case EnemyState.CHASE:
                turret.transform.LookAt(targetToLookAt);
                break;
            case EnemyState.ATTACK:
                turret.transform.LookAt(targetToLookAt);
                break;
            default:
                break;
        }
    }

    protected override void HandleEnemyAggro()
    {
        //Determines aggro of the enemy
        isAggrod = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_BecomeAggro, playerMask);
        inShootRange = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_AttackRange, playerMask);

        
        if (isAggrod)
        {
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

                if (inShootRange && hit.collider.gameObject.tag == "Player")
                {
                    currentState = EnemyState.ATTACK;
                    HandleShooting();
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

            StartCoroutine(stopandShoot());
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
        Shoot(true);
        yield return new WaitForSeconds(1.5f);
        tankNav.resumeMovement();

        yield return null;
    }





}
