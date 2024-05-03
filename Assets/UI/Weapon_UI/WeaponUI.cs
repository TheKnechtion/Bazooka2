using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WeaponUI : MonoBehaviour
{


    [SerializeField] Texture2D[] numbers;

    [SerializeField] Texture2D infiniteSymbol;

    int currentAmmo;

    bool isInfinite;

    WeaponController weaponController;

    RangedWeapon tempWeaponInfo;


    public Material DogtagMaterial;

    private void Awake()
    {
        weaponController = gameObject.GetComponent<WeaponController>();

        PlayerManager.OnPlayerSpawn += UpdateWeaponInfo;
        PlayerManager.OnPlayerSpawn += UpdateWeaponUI;
        RangedWeapon.OnPlayerShoot += UpdateEvent_AmmoCount;
        PlayerManager.OnPlayerWeaponChange += UpdateWeaponInfo;
        PlayerManager.OnPlayerWeaponChange += UpdateWeaponUI;
    }


    void UpdateWeaponInfo(object sender, System.EventArgs e)
    {
        tempWeaponInfo = PlayerManager.currentWeapon_ref;

    }


    void UpdateWeaponUI(object sender, System.EventArgs e)
    {
        tempWeaponInfo = PlayerManager.currentWeapon_ref;
        Update_WeaponUI_WeaponIcon();
        Update_WeaponUI_ProjectileIcon();
        Update_WeaponUI_AmmoCount();
    }

    void Update_WeaponUI_WeaponIcon()
    {
        if (tempWeaponInfo.weaponIcon)
        {
            DogtagMaterial.SetTexture("_WeaponIcon", tempWeaponInfo.weaponIcon);
        }
    }

    void Update_WeaponUI_ProjectileIcon()
    {
        if (tempWeaponInfo.projectileIcon)
        {
            DogtagMaterial.SetTexture("_AmmoIcon", tempWeaponInfo.projectileIcon);
        }
            
    }


    void UpdateEvent_AmmoCount(object sender, System.EventArgs e)
    {
        Update_WeaponUI_AmmoCount();
    }

    void Update_WeaponUI_AmmoCount()
    {

        if (!tempWeaponInfo.isInfinite)
        {
            currentAmmo = (int)tempWeaponInfo.currentAmmo;

            DogtagMaterial.SetInt("_IsInfinite", 0);
            DogtagMaterial.SetTexture("_NumberIcon", numbers[currentAmmo]);
        }
        else
        {
            DogtagMaterial.SetInt("_IsInfinite", 1);
            DogtagMaterial.SetTexture("_NumberIcon", infiniteSymbol);
        }

    }




}
