using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] private float ExplosionRadius;
    [SerializeField] private int Damage;
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

    // Start is called before the first frame update
    void Start()
    {

        //We set this so the barrel doesn't damage itself
        if (!myCollider)
        {
            myCollider = GetComponent<Collider>();
        }

        GameObject temp = Instantiate(ExplosionFXPrefab, gameObject.transform.position, Quaternion.identity);
        explosionParticles = temp.GetComponent<ParticleSystem>();
        var VFXRadius = explosionParticles.shape;
        VFXRadius.radius = ExplosionRadius;

        particleSystem = temp.GetComponent<PlayAndDestroy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explode()
    {
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
                }
                
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }
}
