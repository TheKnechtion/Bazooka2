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


    [SerializeField]private float time;

    private bool canShoot;
    public bool EnemyVersion;


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
        }
        else 
        {
            canShoot = true;
        }
    }
    private void FixedUpdate()
    {
        //reset fire rate WHEN shot
        if (!canShoot)
        {
            Debug.Log("Cant shoot");
            time -= Time.deltaTime;
            time = Mathf.Clamp(time, 0.0f, fireRate);
            //if (time <= fireRate)
            //{
            //    time += Time.deltaTime;
            //}
            //else
            //{
            //    time = fireRate;
            //}
        }        
    }

    public void Shoot()
    {
        if (canShoot)
        {
            time = fireRate;

            //Instantiate projectile prefab that we have
            GameObject newProjectile = projectilePrefab;
            AudioManager.PlayClipAtPosition(stats.fireWeaponSound, shootPoint.position);


            Instantiate(newProjectile, shootPoint.position, shootPoint.rotation);
            //Instantiate(newProjectile, shootPoint.position, Quaternion.LookRotation(Vector3.up, gameObject.transform.forward));
        }
    }

    public void PlayerShoot()
    {
        if (canShoot)
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

        if (canShoot)
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


}
