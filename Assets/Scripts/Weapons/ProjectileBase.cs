using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    [SerializeField] protected ProjectileSO stats;
    protected LayerMask environmentMask;
    protected ProjectileType projectileType;
    protected int damage;
    protected int splashDamage;
    protected int bounceCount;
    protected int speed;

    protected float lifeTime;
    protected float splashRadius;

    protected abstract void setStats();

}
