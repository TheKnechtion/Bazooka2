using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : WeaponBase, IShoot
{
    public Transform shootPoint;
    
    void Start()
    {
        setStats();
    }
    private void FixedUpdate()
    {
        //reset fire rate WHEN shot
    }
    public void Shoot()
    {
        //shoot code
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
        }
    }
}
