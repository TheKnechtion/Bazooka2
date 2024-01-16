using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sawblade : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent<IDamagable>(out IDamagable component))
        {
            component.TakeDamage(1);

            Vector3 pushDirection = collision.transform.position - this.transform.position;

            pushDirection = new Vector3(pushDirection.x, 0f, pushDirection.z).normalized;

            collision.transform.GetComponent<Rigidbody>().AddForce(pushDirection * 200f,ForceMode.Impulse);
        }
    }
}
