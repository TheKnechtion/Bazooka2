using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class WeaponDatabase
{

    //implement singleton functionality in the weapon database

    public int dataCount;
    public int CallCount;
    private static WeaponDatabase _instance;
    private static readonly object _lock = new object();
    public static WeaponDatabase Instance()
    {
        if (_instance == null)
        {
            
            //This is so if there are multiple calls at the same time,
            //the first thread is the only one to make instance. Only one.
            lock (_lock)
            {
                _instance = new WeaponDatabase();
            }
        }
        return _instance;  
    }

    //public static WeaponDatabase instance
    //{
        
    //    get
    //    {
    //        return _instance;
    //    }
    //}

    //Only this class can make itself
    private WeaponDatabase()
    {
        dataCount++;
        Weapon_Database = new List<WeaponInfo>();


        //Test weapon 1
        WeaponInfo testWeapon = new WeaponInfo();
        testWeapon.weaponName = "Test_Weapon";
        testWeapon.projectileType = ProjectileType.Gun;
        testWeapon.projectilePath = ProjectilePath.Straight;

        testWeapon.doesSplashDamageOnDespawn = true;
        testWeapon.doesBounce = true;
        testWeapon.isHoming = false;

        testWeapon.damage = 2;
        testWeapon.splashDamage = 1;
        testWeapon.maxProjectilesOnScreen = 3;
        testWeapon.numberOfProjectilesPerShot = 1;
        testWeapon.numberOfBounces = 1;
        testWeapon.currentAmmo = 12;
        testWeapon.maxAmmo = 12;

        testWeapon.projectileSpeed = 0.1f;
        testWeapon.radiusOfProjectile = 1.0f;
        testWeapon.splashDamageRadius = 1.0f;
        testWeapon.timeBetweenProjectileFire = 2.0f;
        testWeapon.timeBeforeDespawn = 5.0f;
        testWeapon.homingStrength = 0.0f;
        Weapon_Database.Add(testWeapon);


        //Test weapon 2 - OP
        WeaponInfo testWeapon2 = new WeaponInfo();
        testWeapon2.weaponName = "Test_Weapon2";
        testWeapon2.projectileType = ProjectileType.Gun;
        testWeapon2.projectilePath = ProjectilePath.Straight;

        testWeapon2.doesSplashDamageOnDespawn = true;
        testWeapon2.doesBounce = true;
        testWeapon2.isHoming = false;

        testWeapon2.damage = 50;
        testWeapon2.splashDamage = 50;
        testWeapon2.maxProjectilesOnScreen = 3;
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


        //AI_TestWeapon - for enemyAI tests
        WeaponInfo AI_TestWeapon = new WeaponInfo();
        AI_TestWeapon.weaponName = "Test_Weapon2";
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


    public List<WeaponInfo> Weapon_Database { get; set; }





}
