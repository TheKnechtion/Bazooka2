using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    Rigidbody rb;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            rb = collision.transform.GetComponent<Rigidbody>();
            rb.AddForce(-this.transform.forward * 500f, ForceMode.Force);
        }
        else if(collision.transform.TryGetComponent<Rigidbody>(out rb))
        {
            rb.velocity = -this.transform.forward * 10f;
        }

    }
}
