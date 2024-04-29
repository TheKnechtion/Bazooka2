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

    private Collider[] barrelBuffer;

    //Layer to exclude on barrel collision
    private const int barrelBitMask = ~( (1<<8)|(1<<12)|(1<<10) );
    private const float overlapRadius = 0.4f;

    [SerializeField]private float time;

    private bool canShoot;
    [SerializeField]private bool barrelObstructed;
    public bool EnemyVersion;

    private bool userIsPlayer;

    [SerializeField] private bool UseMiscSound;

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

        barrelBuffer = new Collider[3];
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

        if (CollsionTransform != null)
        {
            float barrelColCount = BarrelCollsions(CollsionTransform);
            if (barrelColCount < 1)
            {
                barrelObstructed = false;
            }
            else if (barrelColCount > 0)
            {
                barrelObstructed = true;
            }

            //Debug.Log(Convert.ToString(barrelBitMask, 2).PadLeft(32,'0'));
            //Debug.Log("Bazooka Obstruc. " + barrelObstructed);
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

        if (UseMiscSound)
        {
            AudioManager.PlayMiscClip(stats.fireWeaponSound, transform.position);
        }
        else
        {
            AudioManager.PlayClipAtPosition(stats.fireWeaponSound, shootPoint.position);
        }

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

    private int BarrelCollsions(Transform collTransform)
    {
        int count = Physics.OverlapSphereNonAlloc(collTransform.position, overlapRadius, barrelBuffer, barrelBitMask);
        return count;
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
    /*
    private void OnDrawGizmos()
    {
        if (CollsionTransform != null)
        {
            Vector3 c = CollsionTransform.position;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(c, overlapRadius);
        }       
    }
    */
}
