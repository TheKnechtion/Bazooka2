using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestroyableObject : MonoBehaviour, IDamagable
{
    public int health = 4;

    public bool ArmoredTarget { get;  set; }

    private void Start()
    {
        ArmoredTarget = false;
    }

    public void TakeDamage(int passedDamage)
    {
        health -= passedDamage;

        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }    

}
