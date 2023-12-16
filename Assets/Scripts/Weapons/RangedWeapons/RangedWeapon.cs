using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RangedWeapon : WeaponBase, IShoot
{
    public static event EventHandler OnPlayerShoot;
    public static event EventHandler OnPlayerWeaponChange;


    public Transform shootPoint;
    public int maxActiveProjectiles;
    public static int maxActiveProjectiles_ref;
    private float time;

    private bool canShoot;


    void Start()
    {
        setStats();
        if (!projectilePrefab)
        {
            Debug.LogWarning("No projectile prefab found");
        }



    }
    private void Update()
    {

        if (time != fireRate)
        {
            canShoot = false;
        }
        else if (time >= fireRate)
        { canShoot = true; }
    }
    private void FixedUpdate()
    {
        //reset fire rate WHEN shot
        if (time <= fireRate)
        {
            time += Time.deltaTime;
        }
    }

    GameObject item;
    public void Shoot()
    {
        //Instantiate projectile prefab that we have
        GameObject newProjectile = projectilePrefab;
        AudioManager.PlayClipAtPosition(stats.fireWeaponSound, shootPoint.position);


        Instantiate(newProjectile, shootPoint.position, shootPoint.rotation).AddComponent<PlayerProjectile>();



        //Instantiate(newProjectile, shootPoint.position, Quaternion.LookRotation(Vector3.up, gameObject.transform.forward));


        

        OnPlayerShoot?.Invoke(this, EventArgs.Empty);

        Debug.Log("Spacer");
    }

    public void HandleShooting()
    {
        if (true == true)
        {
            //Set fireRate timer = 0, so it can count back up.
            //This would set 'canShoot = false'
            time = 0.0f;
            Shoot();
        }
    }

    protected override void setStats()
    {
        if (!stats)
        {
            Debug.LogWarning("No stats found");
        }
        else
        {
            weaponName = stats.weaponName;
            fireRate = stats.fireRate;
            walkMultiplier = stats.walkMultiplier;
            projectilePrefab = stats.projectilePrefab;
            maxActiveProjectiles = stats.maxActiveAmount;


        }
    }


}
