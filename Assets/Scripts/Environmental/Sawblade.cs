using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawblade : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent<IDamagable>(out IDamagable component))
        {
            component.TakeDamage(1);
            collision.transform.GetComponent<Rigidbody>().AddForce((collision.transform.position-this.transform.position).normalized * 100f,ForceMode.Impulse);
        }
    }
}
