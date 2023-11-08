using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] private float ExplosionRadius;
    [SerializeField] private int Damage;
    [SerializeField] private Collider myCollider;

    // Start is called before the first frame update
    void Start()
    {

        //We set this so the barrel doesn't damage itself
        if (!myCollider)
        {
            myCollider = GetComponent<Collider>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explode()
    {

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
