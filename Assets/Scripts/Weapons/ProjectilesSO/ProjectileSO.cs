using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Projectile Stats")]
public class ProjectileSO : ScriptableObject
{
    public ProjectileType projectileType;
    public LayerMask EnvironmentMask;
    public int Damage;

    public bool DoSplashDamage;
    public bool ArmorPen;

    public int SplashDamage;
    public int BounceCount;
    public int Speed;
    public int Priority;

    public float LifeTime;
    public float SplashRadius;


}
