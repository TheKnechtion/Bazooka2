using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : DestroyableObject
{
    [SerializeField] private Explosive explosive;
    private bool isDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        isDestroyed = false;
        explosive = GetComponent<Explosive>();
    }

    public override void Die()
    {
        if (!isDestroyed) 
        {
            isDestroyed= true;
            explosive.Explode();
            Destroy(gameObject);
        }
       
        //Custom explosion delete
    }
}
