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
            currentAmmo--;
            newProjectile.AddComponent<PlayerProjectile>();
        }

        //Instantiate(newProjectile, shootPoint.position, Quaternion.LookRotation(Vector3.up, gameObject.transform.forward));
    }

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

            currentAmmo--;

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

            }

            projectilePrefab = stats.projectilePrefab;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        barrelObstructed = true;
    }
    private void OnTriggerExit(Collider other)
    {
        barrelObstructed = false;
    }

}
