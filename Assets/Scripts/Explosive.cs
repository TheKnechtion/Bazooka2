using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float ExplosionRadius;
    public int Damage;
    [SerializeField] private Collider myCollider;

    [SerializeField] private GameObject ExplosionFXPrefab;
    private ParticleSystem explosionParticles;
    
    /// <summary>
    /// This is a script attached to particle system.
    /// When I play the particle system I want to remove it 
    /// from the scene when done.
    /// </summary>
    private PlayAndDestroy particleSystem;

    //private ParticleSystem VFXRadius;

    public event EventHandler CanDestroy;

    public static event EventHandler OnExploded;

    // Start is called before the first frame update
    void Start()
    {

        //We set this so the barrel doesn't damage itself
        if (!myCollider)
        {
            myCollider = GetComponent<Collider>();
        }

    }

    GameObject temp;

    // Update is called once per frame
    void Awake()
    {

        temp = Instantiate(ExplosionFXPrefab, gameObject.transform.position, Quaternion.identity);

        temp.transform.SetParent(this.gameObject.transform);

        explosionParticles = temp.GetComponent<ParticleSystem>();
        var VFXRadius = explosionParticles.shape;
        VFXRadius.radius = ExplosionRadius;

        particleSystem = temp.GetComponent<PlayAndDestroy>();

    }

    private void Update()
    {
        temp.transform.rotation = Quaternion.Euler(Vector3.zero);
    }


    public void Explode()
    {
        temp.transform.SetParent(null);

        AudioManager.PlayClipAtPosition("explosion_sound", this.transform.position);

        

        //explosionParticles.Play();
        particleSystem.PlayParticles();

        Collider[] collidersHit = Physics.OverlapSphere(gameObject.transform.position, ExplosionRadius);

        if (collidersHit.Length > 0)
        {
            for (int i = 0; i < collidersHit.Length; i++)
            {
                if (collidersHit[i] != myCollider)
                {
                    if (collidersHit[i].gameObject.TryGetComponent<IDamagable>(out IDamagable damageable))
                    {
                        damageable.TakeDamage(Damage);
                    }
                    else if (collidersHit[i].gameObject.GetComponentInParent<IDamagable>() != null)
                    {
                        collidersHit[i].gameObject.GetComponentInParent<IDamagable>().TakeDamage(Damage);
                    }
                }
                
            }
        }

        OnExploded.Invoke(this,EventArgs.Empty);
        CanDestroy.Invoke(this, EventArgs.Empty);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }
}
