using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool doesBounce = false;


    public ProjectileType projectileType;
    public ProjectilePath projectilePath;


    Vector2 collisionNormal;
    Vector2 direction2D;

    public WeaponInfo currentWeaponInfo = null;


    public Vector3 direction;

    [SerializeField] private LayerMask environMentMask;
    private Vector3 directionToObjectHit;
    private float distanceToObjectHit;

    public int damage;
    float despawnTime;
    float magnitude;
    int numberOfBounces;

    float splashRadius;
    int splashDamage;

    bool exploding = false;
    bool isSpawning = true;


    CapsuleCollider caster;
    SphereCollider thisProjectileCollider;

    public event EventHandler OnDestroyed;

    public static event EventHandler OnAnyProjectileDestroyed;

    float time = 0;

    private void Start()
    {
        //caster = GetComponent<CapsuleCollider>();
        //thisProjectileCollider = GetComponent<SphereCollider>();
        //Physics.IgnoreCollision(thisProjectileCollider, caster);


        damage = currentWeaponInfo.damage;

        splashDamage = currentWeaponInfo.splashDamage;

        splashRadius = currentWeaponInfo.splashDamageRadius;

        numberOfBounces = currentWeaponInfo.numberOfBounces;

        magnitude = currentWeaponInfo.projectileSpeed;

        despawnTime = currentWeaponInfo.timeBeforeDespawn;

        time = 0.0f;

        //Destroy(gameObject, despawnTime);
    }
 



    private void FixedUpdate()
    {
        time += Time.deltaTime;

        if(time >= despawnTime)
        {
            DealSplashDamage();
            DeleteProjectile();
        }

        this.gameObject.GetComponent<Rigidbody>().velocity = direction * magnitude;

    }


    private void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damageable))
        {
            if (!damageable.ArmoredTarget)
            {

                damageable.TakeDamage(damage);
                DealSplashDamage();
                DeleteProjectile();
            }
        }

        if (collision.gameObject.TryGetComponent<PlayerManager>(out PlayerManager pm))
        {
            if (pm.carriedObject != null && pm.carriedObject.TryGetComponent<IDamagable>(out IDamagable damagable))
            {
                damagable.TakeDamage(damage);
            }
        }


        #region Old Damage Detection
        //if (collision.gameObject.tag == "Enemy")
        //{
        //    collision.gameObject.GetComponent<EnemyBehavior>().TakeDamage(damage);
        //    DealSplashDamage();
        //    DeleteProjectile();
        //    //Destroy(gameObject);
        //}

        //if(collision.gameObject.tag == "Player")
        //{
        //    collision.gameObject.GetComponent<PlayerInfo>().TakeDamage(damage);
        //    DealSplashDamage();
        //    DeleteProjectile();
        //}
        #endregion

        //if (numberOfBounces <= 0 || collision.gameObject.tag == "Projectile") { DealSplashDamage(); Destroy(gameObject); }
        if (numberOfBounces <= 0 || collision.gameObject.tag == "Projectile") { DealSplashDamage(); DeleteProjectile(); }


        Bounce(collision);

        //if (collision.gameObject.tag == "DestroyableObject") { collision.gameObject.GetComponent<DestroyableObject>().TakeDamage(damage); }
        if (collision.gameObject.tag == "LimitedBounceObject") { collision.gameObject.GetComponent<LimitedBounceObject>().ProjectileCollision(); }
        if (collision.gameObject.tag == "ActivatableObject") { collision.gameObject.GetComponent<Button>().Activate();}
        //collision.gameObject.GetComponent<Button>().Activate(); 
    }


    

    void Bounce(Collision collision)
    {
        if (numberOfBounces > 0)
        {
            isSpawning = false;

            collisionNormal = new Vector2(collision.contacts[0].normal.x, collision.contacts[0].normal.z).normalized;

            direction2D = (new Vector2(direction.x, direction.z));

            direction2D = (direction2D - 2 * (Vector2.Dot(direction2D, collisionNormal)) * collisionNormal);

            direction = new Vector3(direction2D.x, 0, direction2D.y);

            transform.rotation = Quaternion.LookRotation(Vector3.up, direction);

            numberOfBounces--;
        }
    }

    

    Collider[] collidersHit;

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
            if (!Physics.Raycast(wallDetect, out hit, distanceToObjectHit, environMentMask))
            {
                if (collidersHit[i].gameObject.TryGetComponent<IDamagable>(out IDamagable obj))
                {
                    if (!obj.ArmoredTarget)
                    {
                        obj.TakeDamage(splashDamage);
                    }
                }
            }

            

            #region Old splash Damage dealing
            //collidersHit[i].gameObject.TryGetComponent<EnemyBehavior>(out EnemyBehavior entityInfo);
            //collidersHit[i].gameObject.TryGetComponent<PlayerInfo>(out PlayerInfo player);
            //collidersHit[i].gameObject.TryGetComponent<DestroyableObject>(out DestroyableObject obj);
            //if (entityInfo != null)
            //{
            //    //Debug.Log("SPLASH DMG");
            //    //We deal splash damage if what we hit is not null
            //    entityInfo.TakeDamage(splashDamage);
            //}

            //if (player != null)
            //{
            //    //Debug.Log("SPLASH DMG");
            //    player.TakeDamage(splashDamage);
            //}

            //if(obj != null)
            //{
            //    obj.TakeDamage(splashDamage);
            //}
            #endregion

        }

        exploding = true;
    }


    public void DeleteProjectile()
    {
        OnDestroyed?.Invoke(this, EventArgs.Empty);
        OnAnyProjectileDestroyed?.Invoke(this, EventArgs.Empty);
    }

    private void OnCollisionExit(Collision collision)
    {
        isSpawning = false;
    }

    private void OnDrawGizmos()
    {
        if (exploding)
        {
            //Draws splash damage radius
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(gameObject.transform.position, splashRadius);
        }
    }


}
