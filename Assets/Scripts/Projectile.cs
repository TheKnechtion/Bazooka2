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

    int damage;
    float despawnTime;
    float magnitude;
    int numberOfBounces;

    float splashRadius;
    int splashDamage;

    bool exploding = false;
    bool isSpawning = true;


    public CapsuleCollider caster;
    SphereCollider thisProjectileCollider;

    private void Start()
    {
        //Physics.IgnoreCollision(thisProjectileCollider, caster);


        damage = currentWeaponInfo.damage;

        splashDamage = currentWeaponInfo.splashDamage;

        splashRadius = currentWeaponInfo.splashDamageRadius;

        numberOfBounces = currentWeaponInfo.numberOfBounces;

        magnitude = currentWeaponInfo.projectileSpeed;

        despawnTime = currentWeaponInfo.timeBeforeDespawn;

        Destroy(gameObject, despawnTime);
    }

    private void Awake()
    {

    }



    // Update is called once per frame
    void Update()
    {

        
    }


    private void FixedUpdate()
    {
        gameObject.transform.Translate(direction * magnitude);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && !isSpawning)
        {
            collision.gameObject.GetComponent<EnemyInfo>().TakeDamage(damage);
            DealSplashDamage();
            Destroy(gameObject, 0.03f);
        }

        
        if(collision.gameObject.tag == "Player" && !isSpawning)
        {
            collision.gameObject.GetComponent<PlayerInfo>().TakeDamage(damage);
            DealSplashDamage();
            Destroy(gameObject, 0.03f);
        
        }
        
        
        
        
        if (numberOfBounces <= 0 || collision.gameObject.tag == "Projectile") { DealSplashDamage(); Destroy(gameObject,0.05f); }


        if (collision.gameObject.tag == "BounceSurface" && numberOfBounces > 0)
        {
            collisionNormal = new Vector2(collision.contacts[0].normal.x, collision.contacts[0].normal.z).normalized;

            direction2D = (new Vector2(direction.x, direction.z));

            direction2D = (direction2D - 2 * (Vector2.Dot(direction2D, collisionNormal)) * collisionNormal);

            direction = new Vector3(direction2D.x, 0, direction2D.y);
            numberOfBounces--;

        }


    }

    private void OnCollisionExit(Collision collision)
    {
        isSpawning = false;
    }


    private void Bounce()
    {

    }

    private void DealSplashDamage()
    {
        //checks surrounding area in a sphere
        var collidersHit = Physics.OverlapSphere(gameObject.transform.position, splashRadius);
        
        
        for (int i = 0; i < collidersHit.Length; i++)
        {

            collidersHit[i].gameObject.TryGetComponent<EntityInfo>(out EntityInfo entityInfo);
            if (entityInfo != null)
            {
                Debug.Log("SPLASH DMG");
                //We deal splash damage if what we hit is not null
                entityInfo.TakeDamage(splashDamage);
            }
            
        }

        exploding = true;
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

    public void CollidersToIgnore(CapsuleCollider casterCapsule)
    {
        thisProjectileCollider = GetComponent<SphereCollider>();
        Physics.IgnoreCollision(thisProjectileCollider, casterCapsule);
    }

}
