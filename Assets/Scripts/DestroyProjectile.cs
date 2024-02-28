using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyProjectile : MonoBehaviour
{
    Projectile bullet;
    BouncingProjectile bounceBullet;
    //ParticleSystem particleObject;
    UnityEngine.Object destroyedEffect;

    [SerializeField] private GameObject destroyVFX;

    [SerializeField] private float LifeTime;

    private Collider coll;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        if (LifeTime > 0.0f)
        {
            Destroy(gameObject, LifeTime);
        }

        coll = GetComponent<Collider>();
        rend = GetComponent<Renderer>();

        try
        {
            bullet = GetComponent<Projectile>();
            bullet.OnDestroyed += Bullet_OnDestroyed;
        }
        catch 
        {

        }

        try
        {
            bounceBullet = GetComponent<BouncingProjectile>();
            bounceBullet.OnDestroyed += Bullet_OnDestroyed;
        }
        catch
        {

        }

        //destroyedEffect = Resources.Load("GunEffect");
        //destroyedEffect.GetComponent<ParticleSystem>();

        //particleObject = GetComponentInChildren<ParticleSystem>();
        //particleObject.Stop();
    }

    private void Bullet_OnDestroyed(object sender, System.EventArgs e)
    {
        //DisableComponents();
        //particleObject.Play();

        AudioManager.PlayClipAtPosition("explosion_sound",transform.position);

        if (destroyVFX != null)
        {
            GameObject Effect = Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(Effect, 3f);
        }
       

        //Destroy(gameObject, particleObject.duration);
        Destroy(gameObject);
    }

    private void DisableComponents()
    {
        //rend.enabled = false;
        //coll.enabled = false;
        bullet.direction = Vector3.zero;
    }
}
