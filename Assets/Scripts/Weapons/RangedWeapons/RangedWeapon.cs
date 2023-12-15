using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : WeaponBase, IShoot
{
    public Transform shootPoint;
    public int maxActiveProjectiles;
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
        else if (time == fireRate)
        { canShoot = true; }
    }
    private void FixedUpdate()
    {
        //reset fire rate WHEN shot
        if (time < fireRate)
        {
            time += Time.deltaTime;
        }
        else
        { time = fireRate; }
    }
    public void Shoot()
    {
        //Instantiate projectile prefab that we have
        GameObject newProjectile = projectilePrefab;
        AudioManager.PlayClipAtPosition(stats.fireWeaponSound, shootPoint.position);
        Instantiate(newProjectile, shootPoint.position, shootPoint.rotation);
        //Instantiate(newProjectile, shootPoint.position, Quaternion.LookRotation(Vector3.up, gameObject.transform.forward));

        Debug.Log("Spacer");
    }

    public void HandleShooting()
    {
        if (canShoot)
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
