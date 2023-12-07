using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : WeaponBase, IShoot
{
    public Transform shootPoint;
    public int maxActiveProjectiles;
    private float time;
    


    void Start()
    {
        setStats();
        if (!projectilePrefab)
        {
            Debug.LogWarning("No projectile prefab found");
        }
    }
    private void FixedUpdate()
    {
        //reset fire rate WHEN shot
        if (time > 0.0f)
        {
            time -= Time.deltaTime;
            
        }
        if (time <= 0.0f) { time = 0.0f; }
    }
    public void Shoot()
    {
        //Set fireRate timer
        time = fireRate;

        //Instantiate projectile prefab
        GameObject newProjectile = projectilePrefab;
        Instantiate(newProjectile, shootPoint.position, shootPoint.rotation);
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
