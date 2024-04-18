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

    private int ExlcudeFromRaycastMask;

    //Used for Decal setting
    public static event EventHandler<Tuple<Vector3, Quaternion, Transform>> ProjectileDestroyed;

    public event EventHandler InstanceDetroyed;
    void Start()
    {
        //These are bitmask layers to EXCLUDE for setting the damage decals
        ExlcudeFromRaycastMask = ~((1 << 7) | (1 << 8));
        //Debug.Log(Convert.ToString(ExlcudeFromRaycastMask, 2).PadLeft(32, '0'));

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
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3.0f, ExlcudeFromRaycastMask))
        {
            if (hit.transform.GetComponent<DestroyableObject>() == null)
            {
                Quaternion rotation = Quaternion.LookRotation(-hit.normal, Vector3.up);
                Tuple<Vector3, Quaternion, Transform> hitData = new Tuple<Vector3, Quaternion, Transform>(
                    hit.point, rotation,
                    hit.collider.gameObject.transform);

                ProjectileDestroyed?.Invoke(this, hitData);
            }
        }

        if (destroyVFX != null)
        {
            GameObject Effect = Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(Effect, 3f);
        }       

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        InstanceDetroyed?.Invoke(this, EventArgs.Empty);
    }
}
