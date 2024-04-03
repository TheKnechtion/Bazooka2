using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConveyerConfigurable : MonoBehaviour
{
    [SerializeField] private float MoveSpeed;

    private Rigidbody rb;
    private void OnTriggerStay(Collider collision)
    {
        //Debug.Log("Something hit");
        if (collision.transform.tag == "Player")
        {
            rb = collision.transform.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * MoveSpeed, ForceMode.Force);
        }
        else if (collision.transform.TryGetComponent<Rigidbody>(out Rigidbody r))
        {
            r.isKinematic = false;
            r.velocity = transform.forward * MoveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider collision)
    {        
        if (collision.TryGetComponent<EnemyBehavior>(out EnemyBehavior enemyBehavior))
        {
            enemyBehavior.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
