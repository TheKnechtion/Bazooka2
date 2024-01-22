using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponScreen : MonoBehaviour
{
 

    public Material WeaponScreenMaterial;

    [SerializeField] Texture2D[] numbers;

    Texture2D WeaponAmmo;
    //[SerializeField] Texture2D WeaponBody;

    WeaponInfo currentPlayerWeapon;
    int activeProjectiles;

    float readyToFire = 1.00f;
    float fired = 0.35f;

    int locked = 0;
    int unlocked = 1;


    float currentAmmo;
    float maxAmmo;

    string[] ammoOpacity = {"_Ammo_One_Opacity", "_Ammo_Two_Opacity", "_Ammo_Three_Opacity", "_Ammo_Four_Opacity", "_Ammo_Five_Opacity", "_Ammo_Six_Opacity", "_Ammo_Seven_Opacity", "_Ammo_Eight_Opacity", "_Ammo_Nine_Opacity", "_Ammo_Ten_Opacity"};
    string[] ammoUnlocked = {"_Ammo_One_Unlocked", "_Ammo_Two_Unlocked", "_Ammo_Three_Unlocked", "_Ammo_Four_Unlocked", "_Ammo_Five_Unlocked", "_Ammo_Six_Unlocked", "_Ammo_Seven_Unlocked", "_Ammo_Eight_Unlocked", "_Ammo_Nine_Unlocked", "_Ammo_Ten_Unlocked"};


    Color darkRed = new Color(.30f, 0.00f, 0.0f);
    Color lightRed = new Color(.70f, 0.00f, 0.0f);

    Color darkGreen = new Color(0.00f, 0.30f, 0.00f);
    Color lightGreen = new Color(.00f, 0.70f, 0.00f);

    WeaponController weaponController;

    private void Awake()
    {
        weaponController = gameObject.GetComponent<WeaponController>();

        PlayerManager.OnPlayerSpawn += UpdateWeaponInfo;
        PlayerManager.OnPlayerSpawn += Update_WeaponUI_CurrentProjectiles;
        RangedWeapon.OnPlayerShoot += Update_WeaponUI_CurrentProjectiles;
        PlayerProjectile.OnExplosion += Update_WeaponUI_CurrentProjectiles;
        PlayerManager.OnPlayerWeaponChange += UpdateWeaponInfo;
        PlayerManager.OnPlayerWeaponChange += UpdateWeaponUI;
        PlayerManager.OnPlayerWeaponChange += Update_WeaponUI_CurrentProjectiles;
       
        //PlayerManager.OnPlayerDetonate += Update_ProjectileUI_CurrentProjectiles;
    }

    RangedWeapon tempWeaponInfo;

    int maxProjOnScreen;

    void UpdateWeaponInfo(object sender, System.EventArgs e)
    {
        tempWeaponInfo = PlayerManager.currentWeapon_ref;
    }


    void UpdateWeaponUI(object sender, System.EventArgs e)
    {
        tempWeaponInfo = PlayerManager.currentWeapon_ref;
        Update_WeaponUI_WeaponIcon();
        Update_WeaponUI_ProjectileIcon();
        Update_WeaponUI_AmmoIcon();
        Update_WeaponUI_AmmoCount();
    }

    void UpdateWeaponUI()
    {
        tempWeaponInfo = PlayerManager.currentWeapon_ref;
        Update_WeaponUI_WeaponIcon();
        Update_WeaponUI_ProjectileIcon();
        Update_WeaponUI_AmmoIcon();
        Update_WeaponUI_AmmoCount();
    }


    void Update_WeaponUI_WeaponIcon()
    {
        WeaponScreenMaterial.SetTexture("_WeaponBody_Texture", tempWeaponInfo.weaponIcon);
    }

    void Update_WeaponUI_ProjectileIcon()
    {
        WeaponScreenMaterial.SetTexture("_WeaponAmmo_Texture", tempWeaponInfo.projectileIcon);
    }

    void Update_WeaponUI_AmmoIcon()
    {
        WeaponScreenMaterial.SetTexture("_Ammo_Texture", tempWeaponInfo.ammoCountIcon);
    }


    void Update_WeaponUI_CurrentProjectiles(object sender, System.EventArgs e)
    {
        
        maxProjOnScreen = PlayerManager.currentWeapon_ref.maxActiveProjectiles;
        activeProjectiles = PlayerManager.activeProjectiles;

        //set current amount of ammo allowed on screen at one time
        for(int i = 0; i < maxProjOnScreen; i++)
        {
            WeaponScreenMaterial.SetInt(ammoUnlocked[i], unlocked);
        }
        for (int i = ammoUnlocked.Length - 1; i > maxProjOnScreen - 1; i--)
        {
            WeaponScreenMaterial.SetInt(ammoUnlocked[i], locked);
        }

        //Set the opacity of unlocked ready-to-fire ammo to be 1. 
        for (int i = 0; i < maxProjOnScreen-activeProjectiles; i++)
        {
            WeaponScreenMaterial.SetFloat(ammoOpacity[i], readyToFire);
        }



        
        if (tempWeaponInfo.currentAmmo < maxProjOnScreen)
        {
            //set opacity of ammo when there's not enough ammo in the weapon to fill the current max projectiles on screen
            for (int i = maxProjOnScreen-1; i >= tempWeaponInfo.currentAmmo; i--)
            {
                if(i >= 0)
                {
                    WeaponScreenMaterial.SetFloat(ammoOpacity[i], fired);
                }
                else 
                {
                    WeaponScreenMaterial.SetFloat(ammoOpacity[0], fired);
                }

            }
        }


        if (activeProjectiles > 0)
        { 
            //Set the opacity of unlocked fired ammo to be 0.35.
            for (int i = maxProjOnScreen; i >= maxProjOnScreen-activeProjectiles; i--)
            {
                WeaponScreenMaterial.SetFloat(ammoOpacity[i], fired);
                UpdateWeaponUI();
            }
        }

        Update_WeaponUI_AmmoCount();
    }



    void Update_WeaponUI_AmmoCount()
    {
        currentAmmo = (float)tempWeaponInfo.currentAmmo;
        maxAmmo = (float)tempWeaponInfo.maxAmmo;

        int ammoTensCount = (int)(currentAmmo/10);
        int ammoOnesCount = (int)(currentAmmo - (ammoTensCount * 10));

        WeaponScreenMaterial.SetTexture("_OnesNumber", numbers[ammoOnesCount]);
        
        WeaponScreenMaterial.SetTexture("_TensNumber", numbers[ammoTensCount]);


        if(currentAmmo == 0)
        {
            WeaponScreenMaterial.SetColor("_AmmoCountColor", lightRed);
        }
        else
        {
            WeaponScreenMaterial.SetColor("_AmmoCountColor", lightGreen);
        }


    }

}
