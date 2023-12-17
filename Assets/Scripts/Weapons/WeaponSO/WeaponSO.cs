using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = " New Weapon Stats")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public float fireRate;
    public int maxActiveAmount;
    public float walkMultiplier;

    public GameObject projectilePrefab;
    public string fireWeaponSound;

    public int maxAmmo;

    public Texture2D weaponIcon;
    public Texture2D projectileIcon;
    public Texture2D ammoCountIcon;

}
