using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//version one of the weapon database
public class WeaponDatabase
{
    private static WeaponDatabase _instance;
    private static readonly object _lock = new object();

    public static WeaponDatabase Instance()
    {
        if (_instance == null)
        {
            //This is so if there are mulitple calls at the same time,
            //the first thread is the only one to make the instance. Only one.
            lock(_lock)
            {
                _instance = new WeaponDatabase();
            }
        }
        return _instance;
    }


    //constructs the database
    public WeaponDatabase()
    {
        //holds the data from the database as a list
        Weapon_Database = new List<WeaponInfo>();


        //Test weapon 1
        WeaponInfo testWeapon = new WeaponInfo();
        testWeapon.weaponName = "Bazooka";
        testWeapon.ProjectileName = "Gun";
        testWeapon.projectileType = ProjectileType.Gun;
        testWeapon.projectilePath = ProjectilePath.Straight;

        testWeapon.doesSplashDamageOnDespawn = true;
        testWeapon.doesBounce = true;
        testWeapon.isHoming = false;

        testWeapon.damage = 1;
        testWeapon.splashDamage = 1;
        testWeapon.maxProjectilesOnScreen = 3;
        testWeapon.numberOfProjectilesPerShot = 1;
        testWeapon.numberOfBounces = 1;
        testWeapon.currentAmmo = 12;
        testWeapon.maxAmmo = 12;

        testWeapon.projectileSpeed = 14.0f;
        testWeapon.radiusOfProjectile = 1.0f;
        testWeapon.splashDamageRadius = 3.0f;
        testWeapon.timeBetweenProjectileFire = 0.2f;
        testWeapon.timeBeforeDespawn = 8.0f;
        testWeapon.homingStrength = 0.0f;
        testWeapon.weaponSound = "bazooka_fire";
        Weapon_Database.Add(testWeapon);


        //Test weapon 2
        WeaponInfo testWeapon2 = new WeaponInfo();
        testWeapon2.weaponName = "Floating Mine Launcher";
        testWeapon2.ProjectileName = "Mine";
        testWeapon2.projectileType = ProjectileType.Gun;
        testWeapon2.projectilePath = ProjectilePath.Straight;

        testWeapon2.doesSplashDamageOnDespawn = true;
        testWeapon2.doesBounce = true;
        testWeapon2.isHoming = false;

        testWeapon2.damage = 50;
        testWeapon2.splashDamage = 50;
        testWeapon2.maxProjectilesOnScreen = 6;
        testWeapon2.numberOfProjectilesPerShot = 50;
        testWeapon2.numberOfBounces = 67;
        testWeapon2.currentAmmo = 12;
        testWeapon2.maxAmmo = 12;

        testWeapon2.projectileSpeed = 0.8f;
        testWeapon2.radiusOfProjectile = 1.0f;
        testWeapon2.splashDamageRadius = 1.0f;
        testWeapon2.timeBetweenProjectileFire = 0.8f;
        testWeapon2.timeBeforeDespawn = 10.0f;
        testWeapon2.homingStrength = 0.0f;
        Weapon_Database.Add(testWeapon2);



        //Mage weapon 
        WeaponInfo mageWeapon = new WeaponInfo();
        mageWeapon.weaponName = "Mage";
        mageWeapon.ProjectileName = "MageProjectile";
        mageWeapon.projectileType = ProjectileType.Gun;
        mageWeapon.projectilePath = ProjectilePath.Straight;

        mageWeapon.doesSplashDamageOnDespawn = true;
        mageWeapon.doesBounce = false;
        mageWeapon.isHoming = false;

        mageWeapon.damage = 0;
        mageWeapon.splashDamage = 1;
        mageWeapon.maxProjectilesOnScreen = 3;
        mageWeapon.numberOfProjectilesPerShot = 1;
        mageWeapon.numberOfBounces = 10;
        mageWeapon.currentAmmo = 12;
        mageWeapon.maxAmmo = 12;

        mageWeapon.projectileSpeed = 7.0f;
        mageWeapon.radiusOfProjectile = 1.0f;
        mageWeapon.splashDamageRadius = 1.0f;
        mageWeapon.timeBetweenProjectileFire = 0.8f;
        mageWeapon.timeBeforeDespawn = 30.0f;
        mageWeapon.homingStrength = 0.0f;
        mageWeapon.weaponSound = "hit_sound";
        Weapon_Database.Add(mageWeapon);


        //Dwarf weapon 
        WeaponInfo dwarfWeapon = new WeaponInfo();
        dwarfWeapon.weaponName = "Dwarf";
        dwarfWeapon.ProjectileName = "Gun";
        dwarfWeapon.projectileType = ProjectileType.Gun;
        dwarfWeapon.projectilePath = ProjectilePath.Straight;

        dwarfWeapon.doesSplashDamageOnDespawn = true;
        dwarfWeapon.doesBounce = false;
        dwarfWeapon.isHoming = false;

        dwarfWeapon.damage = 1;
        dwarfWeapon.splashDamage = 1;
        dwarfWeapon.maxProjectilesOnScreen = 3;
        dwarfWeapon.numberOfProjectilesPerShot = 1;
        dwarfWeapon.numberOfBounces = 1;
        dwarfWeapon.currentAmmo = 12;
        dwarfWeapon.maxAmmo = 12;

        dwarfWeapon.projectileSpeed = 10.0f;
        dwarfWeapon.radiusOfProjectile = 1.0f;
        dwarfWeapon.splashDamageRadius = 1.0f;
        dwarfWeapon.timeBetweenProjectileFire = 0.6f;
        dwarfWeapon.timeBeforeDespawn = 10.0f;
        dwarfWeapon.homingStrength = 0.0f;
        Weapon_Database.Add(dwarfWeapon);


        //Knight weaponn
        WeaponInfo knightWeapon = new WeaponInfo();
        knightWeapon.weaponName = "Knight";
        knightWeapon.ProjectileName = "Gun";
        knightWeapon.projectileType = ProjectileType.Gun;
        knightWeapon.projectilePath = ProjectilePath.Straight;

        knightWeapon.doesSplashDamageOnDespawn = true;
        knightWeapon.doesBounce = false;
        knightWeapon.isHoming = true;

        knightWeapon.damage = 1; 
        knightWeapon.splashDamage = 0;
        knightWeapon.maxProjectilesOnScreen = 3;
        knightWeapon.numberOfProjectilesPerShot = 1;
        knightWeapon.numberOfBounces = 0;
        knightWeapon.currentAmmo = 12;
        knightWeapon.maxAmmo = 12;

        knightWeapon.projectileSpeed = 10.5f;
        knightWeapon.radiusOfProjectile = 1.0f;
        knightWeapon.splashDamageRadius = 1.0f;
        knightWeapon.timeBetweenProjectileFire = 1.5f;
        knightWeapon.timeBeforeDespawn = 10.0f;
        knightWeapon.homingStrength = 0.0f;
        knightWeapon.weaponSound = "bazooka_fire";
        Weapon_Database.Add(knightWeapon);

        //Tank boss weapon
        WeaponInfo tankWeapon = new WeaponInfo();
        tankWeapon.weaponName = "TankTan";
        tankWeapon.ProjectileName = "TankMissile";
        tankWeapon.projectileType = ProjectileType.Gun;
        tankWeapon.projectilePath = ProjectilePath.Straight;

        tankWeapon.doesSplashDamageOnDespawn = true;
        tankWeapon.doesBounce = false;
        tankWeapon.isHoming = false;

        tankWeapon.damage = 1;
        tankWeapon.splashDamage = 2;
        tankWeapon.maxProjectilesOnScreen = 1;
        tankWeapon.numberOfProjectilesPerShot = 1;
        tankWeapon.numberOfBounces = 0;
        tankWeapon.currentAmmo = 12;
        tankWeapon.maxAmmo = 12;

        tankWeapon.projectileSpeed = 20.0f;
        tankWeapon.radiusOfProjectile = 1.0f;
        tankWeapon.splashDamageRadius = 1.0f;
        tankWeapon.timeBetweenProjectileFire = 5.0f;
        tankWeapon.timeBeforeDespawn = 4.0f;
        tankWeapon.homingStrength = 0.0f;
        tankWeapon.weaponSound = "bazooka_fire";
        Weapon_Database.Add(tankWeapon);

        WeaponInfo Turret = new WeaponInfo();
        Turret.weaponName = "Turret";
        Turret.ProjectileName = "Gun";
        Turret.projectileType = ProjectileType.Gun;
        Turret.projectilePath = ProjectilePath.Straight;

        Turret.doesSplashDamageOnDespawn = true;
        Turret.doesBounce = true;
        Turret.isHoming = false;

        Turret.damage = 1;
        Turret.splashDamage = 0;
        Turret.maxProjectilesOnScreen = 1;
        Turret.numberOfProjectilesPerShot = 1;
        Turret.numberOfBounces = 0;
        Turret.currentAmmo = 12;
        Turret.maxAmmo = 12;

        Turret.projectileSpeed = 40.0f;
        Turret.radiusOfProjectile = 1.0f;
        Turret.splashDamageRadius = 1.0f;
        Turret.timeBetweenProjectileFire = 3.0f;
        Turret.timeBeforeDespawn = 4.0f;
        Turret.homingStrength = 0.0f;
        Turret.weaponSound = "hit_sound";
        Weapon_Database.Add(Turret);

        //AI_TestWeapon - for enemyAI tests
        WeaponInfo AI_TestWeapon = new WeaponInfo();
        AI_TestWeapon.weaponName = "AI_TestWeapon";
        AI_TestWeapon.projectileType = ProjectileType.Gun;
        AI_TestWeapon.projectilePath = ProjectilePath.Straight;

        AI_TestWeapon.doesSplashDamageOnDespawn = true;
        AI_TestWeapon.doesBounce = true;
        AI_TestWeapon.isHoming = false;

        AI_TestWeapon.damage = 50;
        AI_TestWeapon.splashDamage = 50;
        AI_TestWeapon.maxProjectilesOnScreen = 3;
        AI_TestWeapon.numberOfProjectilesPerShot = 50;
        AI_TestWeapon.numberOfBounces = 67;
        AI_TestWeapon.currentAmmo = 12;
        AI_TestWeapon.maxAmmo = 12;

        AI_TestWeapon.projectileSpeed = 0.8f;
        AI_TestWeapon.radiusOfProjectile = 1.0f;
        AI_TestWeapon.splashDamageRadius = 1.0f;
        AI_TestWeapon.timeBetweenProjectileFire = 0.8f;
        AI_TestWeapon.timeBeforeDespawn = 10.0f;
        AI_TestWeapon.homingStrength = 0.0f;
        Weapon_Database.Add(AI_TestWeapon);
    }

    //allows an external script to call the weapon database
    public List<WeaponInfo> Weapon_Database { get; set; }





}
