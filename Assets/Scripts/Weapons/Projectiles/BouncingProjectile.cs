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

    [SerializeField] private ParticleSystem CollisionSpark;

    private float yNormalBuffer;

    private bool doSplashDamage;
    private bool exploding;

    //Determines if projectile collision will destroy it
    public int Priority = 1;

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
            CollisionVFX();

            isSpawning = false;

            collisionNormal = new Vector2(collision.contacts[0].normal.x, collision.contacts[0].normal.z).normalized;

            direction2D = new Vector2(direction.x, direction.z);

            direction2D = (direction2D - 2 * (Vector2.Dot(direction2D, collisionNormal)) * collisionNormal);

            direction = new Vector3(direction2D.x, 0, direction2D.y);

            // transform.rotation = Quaternion.LookRotation(Vector3.up, direction);
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            AudioManager.PlayMiscClip("MetalHit", transform.position);

            bounceCount--;
        }
        else
        {
            DealSplashDamage();
            DeleteProjectile();
        }
    }

    private void CollisionVFX()
    {
        if (CollisionSpark != null)
        {
            ParticleSystem m = Instantiate(CollisionSpark, transform.position, transform.rotation);
            m.Play();
            Destroy(m, 1.0f);
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
            else
            {
                CollisionVFX();

                AudioManager.PlayMiscClip("MetalHit", transform.position);
            }

            if (collision.gameObject.TryGetComponent<PlayerManager>(out PlayerManager pm))
            {
                if (pm.carriedObject != null && pm.carriedObject.TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    damagable.TakeDamage(damage);
                }
            }

            if (collision.gameObject.CompareTag("ActivatableObject"))
            {
                if (collision.gameObject.TryGetComponent<Button>(out Button b))
                {
                    b.Activate();
                }
                else
                {
                    Button childButton = collision.gameObject.GetComponentInChildren<Button>();

                    if (childButton != null)
                    {
                        childButton.Activate();
                    }
                }
            }

            DeleteProjectile();
        }
        else if(collision.gameObject.tag == "Projectile")
        {
            BouncingProjectile p = collision.gameObject.GetComponent<BouncingProjectile>();
            if (p != null && p.Priority >= Priority)
            {
                CollisionVFX();

                DeleteProjectile();
            }
        }
        else if (collision.gameObject.CompareTag("ActivatableObject"))
        {
            collision.gameObject.GetComponent<Button>().Activate();
            DeleteProjectile();
        }
        else if (collision.gameObject.CompareTag("LimitedBounceObject"))
        {
            collision.gameObject.GetComponent<LimitedBounceObject>().ProjectileCollision();
        }
        else
        {
            Bounce(collision);
        }
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
            Priority = stats.Priority;
            splashRadius = stats.SplashRadius;
            bounceCount = stats.BounceCount;
            speed = stats.Speed;
            lifeTime = stats.LifeTime;
        }
    }
}
