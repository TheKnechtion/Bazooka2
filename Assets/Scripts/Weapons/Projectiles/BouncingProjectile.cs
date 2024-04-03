using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BouncingProjectile : ProjectileBase
{
    //private int numberOfBounces;
    private bool isSpawning;

    Vector2 collisionNormal;
    Vector2 direction2D;
    private Vector3 direction;

    private Rigidbody projectileRb;

    private Collider[] collidersHit;
    private Vector3 directionToObjectHit;
    private float distanceToObjectHit;

    private float yNormalBuffer;

    private bool doSplashDamage;
    private bool exploding;

    public event EventHandler OnDestroyed;


    void Start()
    {
        projectileRb = GetComponent<Rigidbody>();
        yNormalBuffer = 0.1f;

        setStats();
        //direction = RaycastController.shootVector;
        direction = transform.forward;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        projectileRb.velocity = direction * speed;
    }
    private void Bounce(Collision collision)
    {
        if (Mathf.Abs(collision.contacts[0].normal.y) > yNormalBuffer)
        {
            DeleteProjectile();
        }
        else if (bounceCount > 0)
        {
            isSpawning = false;

            collisionNormal = new Vector2(collision.contacts[0].normal.x, collision.contacts[0].normal.z).normalized;

            direction2D = new Vector2(direction.x, direction.z);

            direction2D = (direction2D - 2 * (Vector2.Dot(direction2D, collisionNormal)) * collisionNormal);

            direction = new Vector3(direction2D.x, 0, direction2D.y);

           // transform.rotation = Quaternion.LookRotation(Vector3.up, direction);
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            bounceCount--;
        }
        else
        {
            DealSplashDamage();
            DeleteProjectile();
        }
    }

    public void DealSplashDamage()
    {
        //Debug.Log("Splash");
        //checks surrounding area in a sphere
        collidersHit = Physics.OverlapSphere(gameObject.transform.position, splashRadius);

        for (int i = 0; i < collidersHit.Length; i++)
        {
            directionToObjectHit = collidersHit[i].transform.position - this.transform.position;
            distanceToObjectHit = Vector3.Distance(collidersHit[i].transform.position, transform.position);
            Ray wallDetect = new Ray(this.transform.position, directionToObjectHit);
            RaycastHit hit;

            //Detects if hit objects are behind a wall or not.
            if (!Physics.Raycast(wallDetect, out hit, distanceToObjectHit, environmentMask))
            {
                if (collidersHit[i].gameObject.TryGetComponent<IDamagable>(out IDamagable obj))
                {
                    if (!obj.ArmoredTarget)
                    {
                        obj.TakeDamage(splashDamage);
                    }
                }
            }

        }

        exploding = true;
    }

    public void DeleteProjectile()
    {
        OnDestroyed?.Invoke(this, EventArgs.Empty);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damageable))
        {
            if (!damageable.ArmoredTarget || ArmorPen)
            {
                damageable.TakeDamage(damage);
                if (doSplashDamage)
                    DealSplashDamage();
               
                //DeleteProjectile();
            }

            if (collision.gameObject.TryGetComponent<PlayerManager>(out PlayerManager pm))
            {
                if (pm.carriedObject != null && pm.carriedObject.TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    damagable.TakeDamage(damage);
                }
            }

            DeleteProjectile();
        }
        else if(bounceCount <= 0 || collision.gameObject.tag == "Projectile")
        {
            DealSplashDamage();
            DeleteProjectile();
        }
        else
        {
            Bounce(collision);
        }

        //if (collision.gameObject.tag == "DestroyableObject") { collision.gameObject.GetComponent<DestroyableObject>().TakeDamage(damage); }
        if (collision.gameObject.tag == "LimitedBounceObject") { collision.gameObject.GetComponent<LimitedBounceObject>().ProjectileCollision(); }
        if (collision.gameObject.tag == "ActivatableObject") { collision.gameObject.GetComponent<Button>().Activate(); }
        //collision.gameObject.GetComponent<Button>().Activate(); 
    }
    protected override void setStats()
    {
        if (!stats)
        {
            Debug.LogWarning("No Projectile stats attatched");
        }
        else
        {
            environmentMask = stats.EnvironmentMask;

            damage = stats.Damage;
            doSplashDamage = stats.DoSplashDamage;
            ArmorPen = stats.ArmorPen;
            splashDamage = stats.SplashDamage;
            splashRadius = stats.SplashRadius;
            bounceCount = stats.BounceCount;
            speed = stats.Speed;
            lifeTime = stats.LifeTime;
        }
    }
}
