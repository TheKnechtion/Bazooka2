using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RB_Item : MonoBehaviour
{
    public Rigidbody thisRigidbody;

    public Rigidbody collision_rb;

    public IDamagable damagable_Object;

    /*
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent<IDamagable>(out damagable_Object))
        {
            collision_rb = collision.transform.GetComponent<Rigidbody>();

            if ((collision_rb.velocity + thisRigidbody.velocity).magnitude > 30f)
            {
                damagable_Object.TakeDamage(1);
            }
        }
        else
        {
            damagable_Object = null;
            collision_rb = null;
            return;
        }

        Debug.Log(collision_rb.velocity);

        Debug.Log(thisRigidbody.velocity);
        Debug.Log((collision_rb.velocity + thisRigidbody.velocity).magnitude);




    }
    */
}
