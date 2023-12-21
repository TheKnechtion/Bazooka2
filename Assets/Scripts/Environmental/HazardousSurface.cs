using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardousSurface : MonoBehaviour
{
    bool canTakeDamage = true;
    [SerializeField] float damageInterval;
    [SerializeField] int damage;




    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && canTakeDamage)
        {
            PlayerMovement.environmentalEffectSpeed = 0.7f;
            collision.gameObject.GetComponent<IDamagable>().TakeDamage(damage);
            canTakeDamage = false;
            StartCoroutine(DamageInterval());
        }
    }


    private IEnumerator DamageInterval()
    {
        yield return new WaitForSeconds(damageInterval);
        canTakeDamage = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        PlayerMovement.environmentalEffectSpeed = 1.0f;
    }

}
