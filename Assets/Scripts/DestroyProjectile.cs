using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyProjectile : MonoBehaviour
{
    Projectile bullet;
    BouncingProjectile bounceBullet;

    [SerializeField] private GameObject destroyVFX;

    [SerializeField] private float LifeTime;

    public static event EventHandler<Tuple<Vector3, Quaternion>> OnDestroy;
    void Start()
    {
        if (LifeTime > 0.0f)
        {
            Destroy(gameObject, LifeTime);
        }

        if (TryGetComponent<Projectile>(out Projectile p))
        {
            bullet = p;
            bullet.OnDestroyed += Bullet_OnDestroyed;
        }
        else if (TryGetComponent<BouncingProjectile>(out BouncingProjectile bp))
        {
            bounceBullet = bp;
            bounceBullet.OnDestroyed += Bullet_OnDestroyed;
        }
    }

    private void Bullet_OnDestroyed(object sender, System.EventArgs e)
    {
        AudioManager.PlayClipAtPosition("explosion_sound",transform.position);

        //Set the explosion Decal position here       
        Debug.DrawRay(transform.position, transform.forward * 3.0f, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3.0f))
        {
            Quaternion rotation = Quaternion.LookRotation(-hit.normal, Vector3.up);
            Tuple<Vector3, Quaternion> hitData = new Tuple<Vector3, Quaternion>(hit.point, rotation);
            OnDestroy.Invoke(this, hitData);

            /*
             * DecalPool.GetRandomDecal(transform.position, -hit.normal)
             * 
             * Grabs a decal then sets its postion 
             * and Rotation based of the normal hit.
             * 
             * Decals will disable after time set in the pool
             */
        }

        if (destroyVFX != null)
        {
            GameObject Effect = Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(Effect, 3f);
        }
       

        Destroy(gameObject);
    }

    private void DisableComponents()
    {
        bullet.direction = Vector3.zero;
    }
}
