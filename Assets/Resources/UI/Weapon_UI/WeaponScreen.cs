using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponScreen : MonoBehaviour
{
    public Material WeaponScreenMaterial;

    Texture2D WeaponAmmo;
    Texture2D WeaponBody;

    WeaponInfo currentPlayerWeapon;
    int activeProjectiles;

    float readyToFire = 1.00f;
    float fired = 0.35f;

    int locked = 0;
    int unlocked = 1;


    int currentAmmo;
    int maxAmmo;

    string[] ammoOpacity = {"_Ammo_One_Opacity", "_Ammo_Two_Opacity", "_Ammo_Three_Opacity", "_Ammo_Four_Opacity", "_Ammo_Five_Opacity", "_Ammo_Six_Opacity", "_Ammo_Seven_Opacity", "_Ammo_Eight_Opacity", "_Ammo_Nine_Opacity", "_Ammo_Ten_Opacity"};
    string[] ammoUnlocked = {"_Ammo_One_Unlocked", "_Ammo_Two_Unlocked", "_Ammo_Three_Unlocked", "_Ammo_Four_Unlocked", "_Ammo_Five_Unlocked", "_Ammo_Six_Unlocked", "_Ammo_Seven_Unlocked", "_Ammo_Eight_Unlocked", "_Ammo_Nine_Unlocked", "_Ammo_Ten_Unlocked"};

    private void Awake()
    {
        PlayerManager.OnPlayerSpawn += Update_ProjectileUI_CurrentProjectiles;
        PlayerManager.OnPlayerShoot += Update_ProjectileUI_CurrentProjectiles;
        PlayerProjectile.OnExplosion += Update_ProjectileUI_CurrentProjectiles;
        //PlayerManager.OnPlayerDetonate += Update_ProjectileUI_CurrentProjectiles;
        Projectile.OnAnyProjectileDestroyed += Update_ProjectileUI_CurrentProjectiles;

    }


    int maxProjOnScreen;

    void Update_ProjectileUI_CurrentProjectiles(object sender, System.EventArgs e)
    {
        currentPlayerWeapon = PlayerManager.currentWeapon;

        maxProjOnScreen = currentPlayerWeapon.maxProjectilesOnScreen;
        activeProjectiles = PlayerManager.activeProjectiles;

        //set current amount of ammo allowed on screen at one time
        for(int i = 0; i < maxProjOnScreen; i++)
        {
            WeaponScreenMaterial.SetInt(ammoUnlocked[i], unlocked);
        }
        for (int i = ammoUnlocked.Length - 1; i > currentPlayerWeapon.maxProjectilesOnScreen-1; i--)
        {
            WeaponScreenMaterial.SetInt(ammoUnlocked[i], locked);
        }



        //Set the opacity of unlocked ready-to-fire ammo to be 1. 
        for (int i = 0; i < maxProjOnScreen-activeProjectiles; i++)
        {
            WeaponScreenMaterial.SetFloat(ammoOpacity[i], readyToFire);
        }
        //Set the opacity of unlocked fired ammo to be 0.35.
        for (int i = maxProjOnScreen; i >= maxProjOnScreen-activeProjectiles; i--)
        {
            if(activeProjectiles > 0)
            {
                WeaponScreenMaterial.SetFloat(ammoOpacity[i], fired);
            }
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        //playerinfo currentweapon is the weapon we're looking at
        //current weapon is bazooka
        //check current weapon maxammoonscreen is the number of projectiles to activate
        //playermanager.activeprojectiles is the number to fade out
        //playermaanager.activeprojectiles
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
