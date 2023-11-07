using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : DestroyableObject
{
    [SerializeField] private Explosive explosive;

    // Start is called before the first frame update
    void Start()
    {
        explosive = GetComponent<Explosive>();
    }

    public override void Die()
    {
        explosive.Explode();
        Destroy(gameObject);
       
        //Custom explosion delete
    }
}
