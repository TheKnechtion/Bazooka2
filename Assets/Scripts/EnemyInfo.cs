using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour, IDamagable
{
    public string Name { get; set; }


    public int health = 2;

    //track the current player position
    Vector3 playerPosition;

    //track the current enemy position
    Vector3 enemyPosition;


    Vector3 enemyLookDirection;

    //the current weapon the enemy has
    WeaponInfo currentWeapon;


    //controls enemy's weapon
    WeaponController weaponController = new WeaponController();


    //Used by the enemy to track how far the player is
    float enemyPlayerTracker;


    float timeBetweenShots;

    //Used to determine how far the player has to be for the enemy to stop attacking
    float enemyAttackRange_BecomeAggro = 5.0f;

    bool isAggrod = false;


    //Used to determine how far the player has to be for the enemy to start attacking
    float enemyAttackRange_ExitAggro = 10.0f;


    Object projectilePrefab;
    GameObject currentEntity;

    Material projectileMaterial;


    private void Start()
    {
        currentWeapon = weaponController.MakeWeapon("Test_Weapon");



        //create the red projectile material used by the enemy projectiles
        projectileMaterial = Resources.Load("Red") as Material;

    }

    private void Update()
    {
        enemyPosition = this.transform.position;

        playerPosition = PlayerInfo.instance.playerPosition;


        enemyPlayerTracker = Vector3.Distance(playerPosition, enemyPosition);

        enemyLookDirection = (playerPosition - enemyPosition).normalized;

        if(enemyPlayerTracker < enemyAttackRange_BecomeAggro) { isAggrod = true; }

        if (enemyPlayerTracker > enemyAttackRange_ExitAggro) { isAggrod = false; }

        if (isAggrod) { HandleShooting(); }

        //tracks time between shots, stopping at 0.
        timeBetweenShots = (timeBetweenShots > 0) ? timeBetweenShots -= Time.deltaTime : 0;



        if (health <= 0) { Destroy(gameObject); }
    }


    private void HandleShooting()
    {

        if (timeBetweenShots <= 0.0f)
        {
            timeBetweenShots = currentWeapon.timeBetweenProjectileFire;

            Shoot();
        }
    }


    void Shoot()
    {

        projectilePrefab = Resources.Load(currentWeapon.projectileType.ToString());

        currentEntity = Instantiate(projectilePrefab as GameObject, enemyPosition, new Quaternion(0, 0, 0, 0));

        currentEntity.GetComponent<Projectile>().currentWeaponInfo = currentWeapon;
        currentEntity.GetComponent<Projectile>().direction = enemyLookDirection;
        currentEntity.GetComponent<Renderer>().material = projectileMaterial;
    }


    public void TakeDamage(int passedDamage)
    {
        health -= passedDamage;
    }





}
