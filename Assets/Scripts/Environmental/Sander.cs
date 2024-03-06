using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sander : MonoBehaviour
{
    [SerializeField] int damageOnContact;
    

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.TryGetComponent<IDamagable>(out IDamagable component))
        {
            //AudioManager.PlayClipAtPosition("click", transform.position);
            component.TakeDamage(1);

        }

    }
}
