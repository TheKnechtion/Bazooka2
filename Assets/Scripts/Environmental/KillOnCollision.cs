using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
        else
        {
            other.transform.TryGetComponent<IDamagable>(out IDamagable entity);
            entity.TakeDamage(100);
        }
       
    }
}
