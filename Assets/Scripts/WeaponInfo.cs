using System.Collections;
using System.Collections.Generic;

public class WeaponInfo
{

    //name of weapon
    public string weaponName;


    //this is the type of projectile that's fired from this weapon
    public ProjectileType projectileType;

    //The type of path the projectile follows
    public ProjectilePath projectilePath;

    public string ProjectileName;


    //whether or not the project explodes when it despawns
    public bool doesSplashDamageOnDespawn;

    //whether or not the projectile is supposed to bounce off non-enemy objects
    public bool doesBounce;

    //this tracks if the 
    public bool isHoming;



    //damage done to damagable entities collided with
    public int damage;

    //splash damage done to damagable entities collided with
    public int splashDamage;

    //maximum number of projectiles available on screen
    public int maxProjectilesOnScreen;
    
    //this is the projectiles the weapon fires each shot
    public int numberOfProjectilesPerShot;

    //whether or not the projectile is supposed to bounce off non-enemy objects
    public int numberOfBounces;

    //this tracks the current ammo count of the weapon
    public int currentAmmo;

    //this tracks the maximum ammo count of the weapon
    public int maxAmmo;



    //velocity of projectile fired from weapon
    public float projectileSpeed;

    //radius of the projectile launched
    public float radiusOfProjectile;

    //time between projectiles can be fired
    public float timeBetweenProjectileFire;

    //time the projectile lasts before despawning
    public float timeBeforeDespawn;

    //radius of splash damage
    public float splashDamageRadius;

    //how strong the homing effect is
    public float homingStrength;

    public string weaponSound;

}
