using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RangedWeapon : WeaponBase, IShoot
{
    public static event EventHandler OnPlayerShoot;
    public static event EventHandler OnPlayerWeaponChange;

    public Texture2D weaponIcon;
    public Texture2D projectileIcon;
    public Texture2D ammoCountIcon;


    public Transform shootPoint;
    public int maxActiveProjectiles;
    public int maxAmmo;
    public int currentAmmo;
    private GameObject newProjectile;

    private List<Collider> barrelCollisions;

    [SerializeField]private float time;

    private bool canShoot;
    [SerializeField]private bool barrelObstructed;
    public bool EnemyVersion;

    private bool userIsPlayer;

    private void Awake()
    {
        setStats();
        if (!projectilePrefab)
        {
            Debug.LogWarning("No projectile prefab found");
        }
    }
    void Start()
    {
        time = 0.0f;

        if (gameObject.GetComponentInParent<PlayerManager>())
        {
            userIsPlayer = true;
        }

        barrelCollisions = new List<Collider>();
    }

    private void Update()
    {
        //if (time != fireRate)
        //{
        //    canShoot = false;
        //}
        //else if (time == fireRate)
        //{ canShoot = true; }

        if (time != 0.0f)
        {
            canShoot = false;
            HandleFireRate();
        }
        else 
        {
            canShoot = true;
        }

        if (barrelCollisions.Count == 0)
        {
            barrelObstructed = false;
        }
        else
        {
            barrelObstructed = true;
            foreach (Collider other in barrelCollisions)
            {
                if (other == null)
                {
                    barrelCollisions.Remove(other);
                    break;
                }
            }
        }

        Debug.Log("Barrel Colls "+barrelCollisions.Count);
    }

    private void HandleFireRate()
    {
        //Debug.Log("Cant shoot");
        time -= Time.deltaTime;

        time = Mathf.Clamp(time, 0.0f, fireRate);
    }
    public void Shoot()
    {
        time = fireRate;

        //Instantiate projectile prefab that we have
        AudioManager.PlayClipAtPosition(stats.fireWeaponSound, shootPoint.position);

        GameObject newProjectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        if (userIsPlayer)
        {
            if (!infiniteAmmo)
            {
                currentAmmo--;
            }
            newProjectile.AddComponent<PlayerProjectile>();
        }

        //Instantiate(newProjectile, shootPoint.position, Quaternion.LookRotation(Vector3.up, gameObject.transform.forward));
    }

    //Unused---
    public void PlayerShoot()
    {
        if (canShoot && !barrelObstructed)
        {
            time = fireRate;

            //Instantiate projectile prefab that we have
            GameObject newProjectile = projectilePrefab;
            AudioManager.PlayClipAtPosition(stats.fireWeaponSound, shootPoint.position);

            Instantiate(newProjectile, shootPoint.position, shootPoint.rotation).AddComponent<PlayerProjectile>();
            //Instantiate(newProjectile, shootPoint.position, Quaternion.LookRotation(Vector3.up, gameObject.transform.forward));

            if (!infiniteAmmo)
            {
                currentAmmo--;
            }

            OnPlayerShoot?.Invoke(this, EventArgs.Empty);

        }
    }

    public void GainAmmo(int amountToGain)
    {
        currentAmmo = (currentAmmo + amountToGain <= maxAmmo) ? currentAmmo + amountToGain : maxAmmo;
    }


    public void HandleShooting()
    {
        //if (true == true)
        //{
        //    //Set fireRate timer = 0, so it can count back up.
        //    //This would set 'canShoot = false'
        //    time = 0.0f;
        //    Shoot();
        //}

        if (canShoot && !barrelObstructed)
        {
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

            if (!EnemyVersion)
            {
                fireRate = stats.fireRate;
                weaponIcon = stats.weaponIcon;
                projectileIcon = stats.projectileIcon;
                ammoCountIcon = stats.ammoCountIcon;
                maxAmmo = stats.maxAmmo;
                currentAmmo = maxAmmo;
                walkMultiplier = stats.walkMultiplier;
                maxActiveProjectiles = stats.maxActiveAmount;
                infiniteAmmo = stats.InfiniteAmmo;
            }

            projectilePrefab = stats.projectilePrefab;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!barrelCollisions.Contains(other))
        {
            barrelCollisions.Add(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (barrelCollisions.Contains(other))
        {
            barrelCollisions.Remove(other);
        }
    }

}
